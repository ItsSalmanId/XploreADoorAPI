using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientSurvey;
using FOX.ExternalServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public class SurveyAutomationService : ISurveyAutomationService
    {

        #region PROPERTIES
        private readonly DBContextSurveyAutomation _surveyAutomationContext = new DBContextSurveyAutomation();
        private readonly GenericRepository<PatientSurveyHistory> _patientSurveyHistoryRepository;
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        private readonly GenericRepository<AutomatedSurveyUnSubscription> _automatedSurveyUnSubscription;
        private readonly GenericRepository<Patient> _patientRepository;
        private readonly GenericRepository<SurveyServiceLog> _surveyServiceLogRepository;
        public static string SurveyMethod = string.Empty;
        #endregion

        #region CONSTRUCTOR
        public SurveyAutomationService()
        {
            _patientSurveyHistoryRepository = new GenericRepository<PatientSurveyHistory>(_surveyAutomationContext);
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_surveyAutomationContext);
            _automatedSurveyUnSubscription = new GenericRepository<AutomatedSurveyUnSubscription>(_surveyAutomationContext);
            _patientRepository = new GenericRepository<Patient>(_surveyAutomationContext);
            _surveyServiceLogRepository = new GenericRepository<SurveyServiceLog>(_surveyAutomationContext);
        }
        #endregion
        #region FUNCTIONS
        // Description: This function is decrypt patient account number & handle the flow of Unsubscribe Email & SMS
        public SurveyLink DecryptionUrl(SurveyLink objSurveyLink)
        {
            if (objSurveyLink != null && !string.IsNullOrEmpty(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT))
            {
                objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT = objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT.Replace("E", "");
                objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT = AppConfiguration.GetPracticeCode + objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT;
                if (!string.IsNullOrEmpty(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT))
                {
                    string surveyMethodRemoveChr = (objSurveyLink.SURVEY_METHOD.Replace("#", ""));
                    string surveyMethodRemoveChar = (surveyMethodRemoveChr.Replace("?", ""));
                    string surveyMethod = GetSurveyMethod(surveyMethodRemoveChar);
                    if (!string.IsNullOrEmpty(surveyMethod))
                    {
                        if (surveyMethod == "SMS" || surveyMethod == "EMAIL")
                        {
                            objSurveyLink.OPEN_SURVEY_METHOD = surveyMethod;
                        }
                        if (!string.IsNullOrEmpty(objSurveyLink.SURVEY_METHOD) && (surveyMethod == "UnsubscribeEmail" || surveyMethod == "UnsubscribeSMS"))
                        {
                            var surveyId = long.Parse(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT);
                            var existingPatientDetails = _patientSurveyRepository.GetFirst(r => r.SURVEY_ID == surveyId && r.DELETED == false);
                            if (existingPatientDetails != null && existingPatientDetails.PATIENT_ACCOUNT_NUMBER != 0)
                            {
                                var patientAccount = long.Parse(existingPatientDetails.PATIENT_ACCOUNT_NUMBER.ToString());
                                string charId = patientAccount.ToString();
                                var patientDetails = _patientRepository.GetFirst(r => r.Chart_Id == charId && r.DELETED == false);
                                if (surveyMethod == "UnsubscribeEmail")
                                {
                                    var unsubscribeEmailDetail = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == patientAccount && r.EMAIL_UNSUBSCRIBE == true && r.DELETED == false);
                                    if (unsubscribeEmailDetail == null)
                                    {
                                        AutomatedSurveyUnSubscription objAutomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                                        objAutomatedSurveyUnSubscription = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == patientAccount && r.DELETED == false);
                                        if (objAutomatedSurveyUnSubscription == null)
                                        {
                                            AutomatedSurveyUnSubscription objAutomatedSurveyUnSubscriptions = new AutomatedSurveyUnSubscription
                                            {
                                                AUTOMATED_SURVEY_UNSUBSCRIPTION_ID = Helper.getMaximumId("AUTOMATED_SURVEY_UNSUBSCRIPTION_ID"),
                                                PATIENT_ACCOUNT = patientAccount,
                                                PRACTICE_CODE = AppConfiguration.GetPracticeCode,
                                                SMS_UNSUBSCRIBE = false,
                                                EMAIL_UNSUBSCRIBE = true,
                                                SURVEY_ID = surveyId,
                                                CREATED_DATE = Helper.GetCurrentDate(),
                                                CREATED_BY = "FOX_TEAM"
                                            };
                                            _automatedSurveyUnSubscription.Insert(objAutomatedSurveyUnSubscriptions);
                                            _automatedSurveyUnSubscription.Save();
                                        }
                                        else
                                        {
                                            objAutomatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = true;
                                            _automatedSurveyUnSubscription.Update(objAutomatedSurveyUnSubscription);
                                            _automatedSurveyUnSubscription.Save();
                                        }
                                        objSurveyLink.SURVEY_METHOD = "Email Unsubscribe";
                                        // Get List of CC Users
                                        var cc = GetEmailCCList();
                                        // Get List of BCC Users
                                        var bcc = GetEmailBCCList();
                                        var emailBody = EmailBody(existingPatientDetails.PATIENT_FIRST_NAME);
                                        SendEmail(patientDetails.Email_Address, "FOX Patient Survey", emailBody, cc, bcc);
                                    }
                                    else
                                    {
                                        objSurveyLink.SURVEY_METHOD = "Link Expire";
                                    }
                                }
                                if (surveyMethod == "UnsubscribeSMS")
                                {
                                    var unsubscribeSMSDetail = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == patientAccount && r.SMS_UNSUBSCRIBE == true && r.DELETED == false);
                                    if (unsubscribeSMSDetail == null)
                                    {
                                        AutomatedSurveyUnSubscription objAutomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                                        objAutomatedSurveyUnSubscription = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == patientAccount && r.DELETED == false);
                                        if (objAutomatedSurveyUnSubscription == null)
                                        {
                                            AutomatedSurveyUnSubscription objAutomatedSurveyUnSubscriptions = new AutomatedSurveyUnSubscription
                                            {
                                                AUTOMATED_SURVEY_UNSUBSCRIPTION_ID = Helper.getMaximumId("AUTOMATED_SURVEY_UNSUBSCRIPTION_ID"),
                                                PATIENT_ACCOUNT = patientAccount,
                                                PRACTICE_CODE = AppConfiguration.GetPracticeCode,
                                                SMS_UNSUBSCRIBE = true,
                                                EMAIL_UNSUBSCRIBE = false,
                                                SURVEY_ID = surveyId,
                                                CREATED_DATE = Helper.GetCurrentDate(),
                                                CREATED_BY = "FOX_TEAM"
                                            };
                                            _automatedSurveyUnSubscription.Insert(objAutomatedSurveyUnSubscriptions);
                                            _automatedSurveyUnSubscription.Save();
                                        }
                                        else
                                        {
                                            objAutomatedSurveyUnSubscription.SMS_UNSUBSCRIBE = true;
                                            _automatedSurveyUnSubscription.Update(objAutomatedSurveyUnSubscription);
                                            _automatedSurveyUnSubscription.Save();
                                        }

                                        var smsBody = SmsBody(patientDetails.FirstName);
                                        var status = SmsService.SMSTwilio(patientDetails.Home_Phone, smsBody);
                                        objSurveyLink.SURVEY_METHOD = "SMS Unsubscribe";
                                    }
                                    else
                                    {
                                        objSurveyLink.SURVEY_METHOD = "Link Expire";
                                    }
                                }
                            }
                        }
                        else
                        {
                            objSurveyLink.SURVEY_METHOD = null;
                        }
                    }
                    else
                    {
                        objSurveyLink.SURVEY_METHOD = "Link Expire";
                    }
                }
                else
                {
                    objSurveyLink = null;
                }
            }
            return objSurveyLink;
        }
        // Description: This function is get survey method details
        public static string GetSurveyMethod(string surveyMethodRemoveChar)
        {
            switch (surveyMethodRemoveChar)
            {
                case "1":
                    surveyMethodRemoveChar = "SMS";
                    break;
                case "2":
                    surveyMethodRemoveChar = "EMAIL";
                    break;
                case "3":
                    surveyMethodRemoveChar = "UnsubscribeSMS";
                    break;
                case "4":
                    surveyMethodRemoveChar = "UnsubscribeEmail";
                    break;
                default:
                    surveyMethodRemoveChar = "";
                    break;
            }
            return surveyMethodRemoveChar;
        }

        // Description: This function is get patient details
        public SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation)
        {
            if (objSurveyAutomation != null && objSurveyAutomation.PATIENT_ACCOUNT != 0)
            {
                long tempSurveyId = objSurveyAutomation.PATIENT_ACCOUNT;
                var existingPatientDetails = _patientSurveyRepository.GetFirst(r => r.SURVEY_ID == tempSurveyId && r.DELETED == false);
                if (existingPatientDetails != null && existingPatientDetails.PATIENT_ACCOUNT_NUMBER != null)
                {
                    long getPracticeCode = AppConfiguration.GetPracticeCode;
                    SqlParameter pracCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = getPracticeCode };
                    SqlParameter patientAccountNumber = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = existingPatientDetails.PATIENT_ACCOUNT_NUMBER };
                    SqlParameter surveyId = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = tempSurveyId };
                    var performSurveyHistory = SpRepository<SurveyServiceLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PERFORM_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT, @PRACTICE_CODE, @SURVEY_ID", patientAccountNumber, pracCode, surveyId);
                    if (performSurveyHistory != null)
                    {
                        objSurveyAutomation = null;
                    }
                    else
                    {
                        SqlParameter practiceCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = getPracticeCode };
                        SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = existingPatientDetails.PATIENT_ACCOUNT_NUMBER };
                        SqlParameter surveyIdd = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = tempSurveyId };
                        objSurveyAutomation = SpRepository<SurveyAutomation>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT, @PRACTICE_CODE, @SURVEY_ID", patientAccount, practiceCode, surveyIdd);
                    }
                }
                else
                {
                    objSurveyAutomation = null;
                }
            }
            else
            {
                objSurveyAutomation = null;
            }
            return objSurveyAutomation;
        }
        // Description: This function is get patient survey questions details
        public List<SurveyQuestions> GetSurveyQuestionDetails(SurveyLink objSurveyLink)
        {
            List<SurveyQuestions> surveyQuestionsList = new List<SurveyQuestions>();
            if (objSurveyLink != null && !string.IsNullOrEmpty(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT))
            {
                long getPracticeCode = AppConfiguration.GetPracticeCode;
                SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.VarChar, Value = objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT };
                SqlParameter practiceCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = getPracticeCode };
                surveyQuestionsList = SpRepository<SurveyQuestions>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_QUESTION  @PATIENT_ACCOUNT, @PRACTICE_CODE", patientAccount, practiceCode);
            }
            return surveyQuestionsList;
        }
        // Description: This function is trigger to update patient survey model
        public ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null && objPatientSurvey.PATIENT_ACCOUNT_NUMBER != 0)
                {
                    var existingSurveyDetails = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.IS_SURVEYED == true && r.DELETED == false);
                    if (existingSurveyDetails == null)
                    {
                        SurveyServiceLog objSurveyServiceLog = new SurveyServiceLog();
                        long practiceCode = AppConfiguration.GetPracticeCode;
                        objSurveyServiceLog = _surveyServiceLogRepository.GetFirst(r => r.PATIENT_ACCOUNT == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.PRACTICE_CODE == practiceCode && r.DELETED == false);
                        if (objSurveyServiceLog != null)
                        {
                            if (objPatientSurvey.IS_SMS == true)
                            {
                                objSurveyServiceLog = _surveyServiceLogRepository.GetByID(objSurveyServiceLog.SURVEY_AUTOMATION_LOG_ID);
                                objSurveyServiceLog.IS_SMS = true;
                                objSurveyServiceLog.IS_EMAIL = false;
                                _surveyServiceLogRepository.Update(objSurveyServiceLog);
                                _surveyServiceLogRepository.Save();
                            }
                            else
                            {
                                objSurveyServiceLog = _surveyServiceLogRepository.GetByID(objSurveyServiceLog.SURVEY_AUTOMATION_LOG_ID);
                                objSurveyServiceLog.IS_SMS = false;
                                objSurveyServiceLog.IS_EMAIL = true;
                                _surveyServiceLogRepository.Update(objSurveyServiceLog);
                                _surveyServiceLogRepository.Save();
                            }
                        }
                        var existingPatientDetails = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.DELETED == false);
                        PatientSurvey patientSurvey = new PatientSurvey();
                        if (existingPatientDetails != null)
                        {
                            objPatientSurvey.SURVEY_ID = existingPatientDetails.SURVEY_ID;
                            AddPatientSurvey(objPatientSurvey);
                            existingPatientDetails.IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ;
                            existingPatientDetails.IS_REFERABLE = objPatientSurvey.IS_REFERABLE;
                            existingPatientDetails.IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION;
                            existingPatientDetails.FEEDBACK = objPatientSurvey.FEEDBACK;
                            existingPatientDetails.SURVEY_STATUS_BASE = "Completed";
                            existingPatientDetails.SURVEY_STATUS_CHILD = "Completed Survey";
                            existingPatientDetails.MODIFIED_BY = "Automation_5483326";
                            existingPatientDetails.IS_SURVEYED = true;
                            existingPatientDetails.IN_PROGRESS = false;
                            existingPatientDetails.SURVEY_FORMAT_TYPE = "New Format";
                            existingPatientDetails.SURVEY_COMPLETED_DATE = Helper.GetCurrentDate();
                            existingPatientDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                            existingPatientDetails.DELETED = false;
                            if (objPatientSurvey.IS_REFERABLE == true && objPatientSurvey.IS_REFERABLE != null)
                            {
                                existingPatientDetails.SURVEY_FLAG = "Green";
                            }
                            else
                            {
                                existingPatientDetails.SURVEY_FLAG = "Red";
                            }
                            _patientSurveyRepository.Update(existingPatientDetails);
                            _patientSurveyRepository.Save();
                            response.ErrorMessage = "";
                            response.Message = "Suvery completed successfully";
                            response.Success = true;
                        }
                        else
                        {
                            response.ErrorMessage = "";
                            response.Message = "Suvery not completed successfully";
                            response.Success = false;
                        }
                    }
                    else
                    {
                        SurveyServiceLog objSurveyServiceLog = new SurveyServiceLog();
                        long practiceCode = AppConfiguration.GetPracticeCode;
                        objSurveyServiceLog = _surveyServiceLogRepository.GetFirst(r => r.PATIENT_ACCOUNT == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.PRACTICE_CODE == practiceCode && r.DELETED == false);
                        if (objSurveyServiceLog != null)
                        {
                            if (objSurveyServiceLog.IS_SMS == true && objSurveyServiceLog != null)
                            {
                                response.ErrorMessage = "";
                                response.Message = "Suvery completed via SMS";
                                response.Success = false;
                            }
                            else if (objSurveyServiceLog.IS_EMAIL == true && objSurveyServiceLog != null)
                            {
                                response.ErrorMessage = "";
                                response.Message = "Suvery completed via Email";
                                response.Success = false;
                            }
                        }
                        else
                        {
                            response.ErrorMessage = "";
                            response.Message = "";
                            response.Success = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        // Description: This function is trigger to add patient survey model
        private void AddPatientSurvey(PatientSurvey objPatientSurvey)
        {
            if (objPatientSurvey != null)
            {
                long practiceCode = AppConfiguration.GetPracticeCode;
                PatientSurveyHistory patientSurveyHistory = new PatientSurveyHistory
                {
                    SURVEY_HISTORY_ID = Helper.getMaximumId("SURVEY_HISTORY_ID"),
                    SURVEY_ID = objPatientSurvey.SURVEY_ID,
                    PATIENT_ACCOUNT = objPatientSurvey.PATIENT_ACCOUNT_NUMBER,
                    PRACTICE_CODE = practiceCode,
                    IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ,
                    IS_REFERABLE = objPatientSurvey.IS_REFERABLE,
                    IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION,
                    FEEDBACK = objPatientSurvey.FEEDBACK,
                    SURVEY_STATUS_BASE = "Completed",
                    SURVEY_STATUS_CHILD = "Completed Survey",
                    DELETED = false,
                    CREATED_BY = "Automation_5483326",
                    SURVEY_DATE = Helper.GetCurrentDate(),
                    CREATED_DATE = Helper.GetCurrentDate()
                };
                _patientSurveyHistoryRepository.Insert(patientSurveyHistory);
                _patientSurveyHistoryRepository.Save();
            }
        }
        #endregion
        #region Email & SMS body 
        // Description: This function is used for Email body
        public static string EmailBody(string patientFirstName)
        {
            string mailBody = string.Empty;
            string templatePathOfSenderEmail = AppDomain.CurrentDomain.BaseDirectory;
            templatePathOfSenderEmail = templatePathOfSenderEmail.Replace(@"\bin\Debug", "") + "HtmlTemplates\\UnsubscribeAutomatedPatientSurveyEmailTemplate.html";
            if (File.Exists(templatePathOfSenderEmail))
            {
                mailBody = File.ReadAllText(templatePathOfSenderEmail);
                mailBody = mailBody.Replace("[[PATIENT_FIRST_NAME]]", patientFirstName);
            }
            return mailBody ?? "";
        }
        // Description: This function is used for send email
        public static bool SendEmail(string to, string subject, string body, List<string> CC = null, List<string> BCC = null, string AttachmentFilePaths = null, string from = "foxrehab@carecloud.com")
        {
            bool IsMailSent = false;
            var bodyHTML = "";
            bodyHTML += "<body>";
            bodyHTML += body;
            bodyHTML += "</body>";
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(from);
                        mail.To.Add(new MailAddress(to));
                        mail.Subject = subject;
                        mail.Body = bodyHTML;
                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = Encoding.UTF8;
                        if (CC != null && CC.Count > 0)
                        {
                            foreach (var item in CC) { mail.CC.Add(item); }
                        }
                        if (BCC != null && BCC.Count > 0)
                        {
                            foreach (var item in BCC) { mail.Bcc.Add(item); }
                        }
                        if (AttachmentFilePaths != null)
                        {
                            if (File.Exists(AttachmentFilePaths)) { mail.Attachments.Add(new Attachment(AttachmentFilePaths)); }
                        }
                        smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["FoxRehabUserName"], ConfigurationManager.AppSettings["FoxRehabPassword"]);
                        smtp.Send(mail);
                        IsMailSent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsMailSent;
        }
        // Description: This function is used for get list of Email CC
        public static List<string> GetEmailCCList()
        {
            List<string> cc = new List<string>();
            var ccUsers = ConfigurationManager.AppSettings?["SurveyAutomationCCList"].ToString();
            if (!string.IsNullOrEmpty(ccUsers))
            {
                cc = ccUsers.Split(',').ToList();
            }
            return cc ?? null;
        }
        // Description: This function is used for get list of Email BCC
        public static List<string> GetEmailBCCList()
        {
            List<string> bcc = new List<string>();
            var ccUsers = ConfigurationManager.AppSettings?["SurveyAutomationBCCList"].ToString();
            if (!string.IsNullOrEmpty(ccUsers))
            {
                bcc = ccUsers.Split(',').ToList();
            }
            return bcc ?? null;
        }
        // Description: This function is used forcreate SMS body
        public static string SmsBody(string patientFirstName)
        {
            string smsBody = "Hello " + patientFirstName + "!\n \nYour request to unsubscribe from receiving patient surveys is received. You will not receive any messages with patient survey link in future.\n\nRegards\n\nFox Rehab Team ";
            return smsBody ?? "";
        }
        #endregion
    }
}
