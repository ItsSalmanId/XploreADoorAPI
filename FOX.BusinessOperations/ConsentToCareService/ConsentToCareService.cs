using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.IndexInfoServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.DataModels.Models.TasksModel;
using FOX.ExternalServices;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using FOX.DataModels.Models.IndexInfo;
using SelectPdf;
using FOX.DataModels.Models.GroupsModel;

namespace FOX.BusinessOperations.ConsentToCareService
{
    public class ConsentToCareService : IConsentToCareService
    {

        #region PROPERTIES
        private readonly DBContextConsentToCare _consentToCareContext = new DBContextConsentToCare();
        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly DbContextTasks _TaskContext = new DbContextTasks();
        private readonly GenericRepository<FoxTblConsentToCare> _consentToCareRepository;
        private readonly GenericRepository<FoxTblConsentToCare> _consentToCareRepositoryForUseMultiple;
        private readonly GenericRepository<ConsentToCareDocument> _consentToCareDocumentRepository;
        private readonly GenericRepository<ConsentToCareStatus> _consentToCareStatusRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TYPE> _taskTypeRepository;
        private readonly GenericRepository<FOX_TBL_TASK> _TaskRepository;
        private readonly GenericRepository<PatientContact> _PatientContactRepository;
        private readonly DbContextPatient _PatientContext = new DbContextPatient();
        private readonly DBContextPatientDocuments _patientDocument = new DBContextPatientDocuments();
        private readonly GenericRepository<FoxDocumentType> _foxDocumentTypeRepository;
        private readonly GenericRepository<Patient> _PatientRepository;
        public static string SurveyMethod = string.Empty;
        private static List<Thread> threadsList = new List<Thread>();
        // To handle encryption/decryption Objective-C,C#
        private string passPhrase = "2657894562368456";
        private ResponseHTMLToPDF htmlToPdfResponseObj;
        #endregion

        #region CONSTRUCTOR
        public ConsentToCareService()
        {
            _consentToCareRepository = new GenericRepository<FoxTblConsentToCare>(_consentToCareContext);
            _consentToCareRepositoryForUseMultiple = new GenericRepository<FoxTblConsentToCare>(_consentToCareContext);
            _consentToCareDocumentRepository = new GenericRepository<ConsentToCareDocument>(_consentToCareContext);
            _consentToCareStatusRepository = new GenericRepository<ConsentToCareStatus>(_consentToCareContext);
            _TaskRepository = new GenericRepository<FOX_TBL_TASK>(_CaseContext);
            _taskTypeRepository = new GenericRepository<FOX_TBL_TASK_TYPE>(_TaskContext);
            htmlToPdfResponseObj = new ResponseHTMLToPDF();
            _PatientContactRepository = new GenericRepository<PatientContact>(_PatientContext);
            _PatientRepository = new GenericRepository<Patient>(_PatientContext);
            _foxDocumentTypeRepository = new GenericRepository<FoxDocumentType>(_patientDocument);
        }
        #endregion
        #region FUNCTIONS
        public FoxTblConsentToCare AddUpdateConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            string htmlTemplate = string.Empty;
            string consentReceiverName = string.Empty;
            if (consentToCareObj != null)
            {
                string concentToCareReceiverEmail = string.Empty;
                string concentToCareHomePhone = string.Empty;
                string subject = string.Empty;
                if (consentToCareObj.disciplineName != null)
                {
                    subject = "FOX Rehabilitation - Your "+ consentToCareObj.disciplineName + " Services PLEASE READ!"; 
                }
                consentToCareObj.PATIENT_ACCOUNT = long.Parse(consentToCareObj.PATIENT_ACCOUNT_Str == null ? "0" : consentToCareObj.PATIENT_ACCOUNT_Str);
                if (consentToCareObj.SEND_TO == "Patient")
                {
                    concentToCareReceiverEmail = consentToCareObj.PatientEmailAddress;
                    concentToCareHomePhone = consentToCareObj.PatientHomePhone;
                }
                else if (consentToCareObj.SEND_TO == "Financially Responsible Party")
                {
                    concentToCareReceiverEmail = consentToCareObj.FrpEmailAddress;
                    concentToCareHomePhone = consentToCareObj.FrpHomePhone;
                }
                if (consentToCareObj.SEND_TO == "Power of Attorney")
                {
                    concentToCareReceiverEmail = consentToCareObj.PoaEmailAddress;
                    concentToCareHomePhone = consentToCareObj.PoaHomePhone;
                }
                if (consentToCareObj.SENT_TO_ID != 0 && consentToCareObj.SEND_TO != "Patient")
                {
                    var patinetContactID = consentToCareObj.SENT_TO_ID;
                    var selectedSentToId = new SqlParameter("@SENT_TO_ID", SqlDbType.BigInt) { Value = consentToCareObj.SENT_TO_ID };
                    var pracCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    var existingConsentDetail = SpRepository<PatientContactDetails>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_GET_PATINET_CONTACT_DETAILS @SENT_TO_ID, @PRACTICE_CODE", selectedSentToId, pracCode);
                    if(existingConsentDetail != null)
                    {
                        consentReceiverName = existingConsentDetail.First_Name + " " + existingConsentDetail.Last_Name.Substring(0, 1)[0] + ",";
                    }
                }
                else
                {
                    consentReceiverName = consentToCareObj.PatientFirstName + " " + consentToCareObj.PatientLastName.Substring(0, 1)[0] + ",";
                }
                var selectedCaseId = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
                var praCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                var existingConsentDetails = SpRepository<string>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_DETAILS_BY_CASE_ID @CASE_ID, @PRACTICE_CODE", selectedCaseId, praCode);
                var isUpdateExpired = existingConsentDetails.Contains("True");
                if(isUpdateExpired == true)
                {
                    List<TaskLog> taskLoglist = new List<TaskLog>();
                    List<string> consentTocarelogs = new List<string>();
                    StringBuilder consentTocarelogsString = new StringBuilder();
                    consentTocarelogs.Add("The consent to care link for " + consentToCareObj.lastConsentreceiver +" "+ consentToCareObj.lastConsentReceiverName + " has been expired because it has been sent to another recipient.");
                    foreach (string str in consentTocarelogs)
                    {
                        consentTocarelogsString.Append(str + "<br>");
                    }
                    taskLoglist.Add(new TaskLog()
                    {
                        ACTION = "Task Comment",
                        ACTION_DETAIL = consentTocarelogsString.ToString()
                    }
                        );

                    if (taskLoglist.Count() > 0)
                    {
                        profile.UserName = "FOX TEAM";
                        InsertTaskLog(consentToCareObj.TASK_ID, taskLoglist, profile);
                    }
                    InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                    if (consentToCareObj != null)
                    {
                        interfaceSynch.TASK_ID = consentToCareObj.TASK_ID;
                        interfaceSynch.PATIENT_ACCOUNT = consentToCareObj.PATIENT_ACCOUNT;
                        interfaceSynch.CASE_ID = consentToCareObj.CASE_ID;
                        ////Task Interface
                        InsertInterfaceTeamData(interfaceSynch, profile);
                    }
                }
                consentToCareObj.PATIENT_ACCOUNT = long.Parse(consentToCareObj.PATIENT_ACCOUNT_Str == null ? "0" : consentToCareObj.PATIENT_ACCOUNT_Str);
                profile.PracticeCode = GetPracticeCode();
                var config = GetServiceConfiguration(AppConfiguration.GetPracticeCode);
                htmlTemplate = consentToCareObj.TEMPLATE_HTML;
                if (consentToCareObj.SEND_TO == "Patient")
                {
                    consentToCareObj.SENT_TO_ID = consentToCareObj.PATIENT_ACCOUNT;
                }
                var consentToCareIdStr = consentToCareObj.CONSENT_TO_CARE_ID.ToString();
                var consentToCareId = new SqlParameter("@CONSENT_TO_CARE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CONSENT_TO_CARE_ID };
                var existingCaseId = consentToCareObj.CASE_ID;
                var caseId = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
                var practiceCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                var sendTo = new SqlParameter("@SEND_TO", SqlDbType.VarChar) { Value = consentToCareObj.SEND_TO };
                var sentToId = new SqlParameter("@SENT_TO_ID", SqlDbType.VarChar) { Value = consentToCareObj.SENT_TO_ID };
                var existingInformation = SpRepository<FoxTblConsentToCare>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID_AND_SEND_TO @CASE_ID, @PRACTICE_CODE, @SEND_TO, @SENT_TO_ID", caseId, practiceCode, sendTo, sentToId);
                if (existingInformation == null)
                {
                    ////Add Consent To Care 
                    consentToCareObj.CONSENT_TO_CARE_ID = Helper.getMaximumId("CONSENT_TO_CARE_ID");
                    consentToCareObj.CREATED_DATE = DateTime.Now;
                    consentToCareObj.EXPIRY_DATE_UTC = DateTime.UtcNow.AddDays(5);
                    consentToCareObj.CREATED_BY = profile.UserName;
                    consentToCareObj.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                    var consentStatus = _consentToCareStatusRepository.GetFirst(x => x.STATUS_NAME == "Sent" && x.PRACTICE_CODE == consentToCareObj.PRACTICE_CODE && !x.DELETED);
                    if (consentStatus != null)
                    {
                        consentToCareObj.STATUS_ID = consentStatus.CONSENT_TO_CARE_STATUS_ID;
                    }
                    consentToCareObj.SOURCE_TYPE = "FOX TEAM";
                    var currentConsentToCareId = consentToCareObj.CONSENT_TO_CARE_ID;
                    string currentConsentToCareIdStr = consentToCareObj.CONSENT_TO_CARE_ID.ToString();

                    //// task Implementation
                    string tasktypeHBR = "";
                    tasktypeHBR = "CONSENTTOCARE";
                    var currentDate = DateTime.Now.ToString();
                    IndexInfoService indexInfoServiceObj = new IndexInfoService();
                    //// set Task Data 
                    var consentToCareTask = SetTaskData(profile, consentToCareObj.PATIENT_ACCOUNT, tasktypeHBR, currentDate, consentToCareObj);
                    //consentToCareTask.PROVIDER_ID = consentToCareObj.TREATING_PROVIDER_ID;
                    //consentToCareTask.LOC_ID = consentToCareObj.POS_ID;
                    consentToCareTask.CASE_ID = consentToCareObj.CASE_ID;
                    if (consentToCareObj.TREATING_PROVIDER_ID != 0 && consentToCareObj.TREATING_PROVIDER_ID != null)
                    {
                        var pracCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.VarChar) { Value = GetPracticeCode() };
                        var treatingProviderId = new SqlParameter("@TREATING_PROVIDER_ID", SqlDbType.VarChar) { Value = consentToCareObj.TREATING_PROVIDER_ID };
                        var sendToId = SpRepository<string>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_GET_USER_ID_BY_PROVIDER_CODE @PRACTICE_CODE, @TREATING_PROVIDER_ID", pracCode, treatingProviderId);
                        if (sendToId != null)
                        {
                            consentToCareTask.SEND_TO_ID = Convert.ToInt64(sendToId);
                            consentToCareTask.IS_SEND_TO_USER = true;
                        }
                    }
                    ////Add Update Task 
                    var existingConsentCaseId = _consentToCareRepository.GetFirst(x => x.CASE_ID == consentToCareObj.CASE_ID && !x.DELETED);
                    long currentTaskId = 0;
                    if(existingConsentCaseId == null)
                    {
                        var taskInterfacedHBR = AddUpdateTask(consentToCareTask, profile, consentToCareObj.SEND_TO, consentReceiverName);
                        InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                        if (taskInterfacedHBR != null)
                        {
                            interfaceSynch.TASK_ID = taskInterfacedHBR.TASK_ID;
                            currentTaskId = taskInterfacedHBR.TASK_ID;
                            interfaceSynch.PATIENT_ACCOUNT = consentToCareObj.PATIENT_ACCOUNT;
                            interfaceSynch.CASE_ID = consentToCareObj.CASE_ID;
                            ////Task Interface
                            InsertInterfaceTeamData(interfaceSynch, profile);
                        }
                    }
                    else
                    {
                        List<TaskLog> taskLoglist = new List<TaskLog>();
                        List<string> consentTocarelogs = new List<string>();
                        StringBuilder consentTocarelogsString = new StringBuilder();
                        consentTocarelogs.Add("Consent to care link has been sent to: " + consentToCareObj.SEND_TO + " (" + consentReceiverName + ")");
                        foreach (string str in consentTocarelogs)
                        {
                            consentTocarelogsString.Append(str + "<br>");
                        }
                        taskLoglist.Add(new TaskLog()
                        {
                            ACTION = "Task Comment",
                            ACTION_DETAIL = consentTocarelogsString.ToString()
                        }
                            );

                        if (taskLoglist.Count() > 0)
                        {
                            profile.UserName = "FOX TEAM";
                            InsertTaskLog(existingConsentCaseId.TASK_ID, taskLoglist, profile);
                        }
                        InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                        if (existingConsentCaseId != null)
                        {
                            interfaceSynch.TASK_ID = existingConsentCaseId.TASK_ID;
                            currentTaskId = Convert.ToInt32(existingConsentCaseId.TASK_ID);
                            interfaceSynch.PATIENT_ACCOUNT = existingConsentCaseId.PATIENT_ACCOUNT;
                            interfaceSynch.CASE_ID = existingConsentCaseId.CASE_ID;
                            ////Task Interface
                            InsertInterfaceTeamData(interfaceSynch, profile);
                        }
                    }
                    consentToCareObj.TASK_ID = currentTaskId;
                    _consentToCareRepository.Insert(consentToCareObj);
                    _consentToCareRepository.Save();
                    var encryptedUrl = EncryptTemp(consentToCareObj.CONSENT_TO_CARE_ID.ToString());
                    var decryptioUrl = Decryption(encryptedUrl.ToString());

                    var encryptedEmailURL = GenerateEncryptedEmailURL(consentToCareObj);
                    var emailBody = EmailBody(consentReceiverName, encryptedEmailURL, consentToCareObj.disciplineName);
                    var email = concentToCareReceiverEmail;
                    var number = concentToCareHomePhone;
                    var smsBody = SmsBody(consentReceiverName, encryptedEmailURL, consentToCareObj.disciplineName);
                    if (!string.IsNullOrEmpty(concentToCareReceiverEmail))
                    {
                        Thread emailThread = new Thread(() =>
                        {
                            bool sent = Helper.ConcentToCareEmail(to: email, subject: subject, body: emailBody, profile: profile, CC: null, BCC: null);
                        });
                        emailThread.Start();
                    }
                    if (!string.IsNullOrEmpty(number))
                    {
                        Thread smsThread = new Thread(() =>
                        {
                            var status = SmsService.SMSTwilio(number, smsBody);
                        });
                        smsThread.Start();
                    }
                }
                else
                {
                    existingInformation.TEMPLATE_HTML = htmlTemplate;
                    existingInformation.EXPIRY_DATE_UTC = DateTime.UtcNow.AddDays(5);
                    existingInformation.MODIFIED_BY = profile.UserName;
                    existingInformation.MODIFIED_DATE = DateTime.Now;
                    var consentStatus = _consentToCareStatusRepository.GetFirst(x => x.STATUS_NAME == "Sent" && x.PRACTICE_CODE == AppConfiguration.GetPracticeCode && !x.DELETED);
                    if (consentStatus != null)
                    {
                        existingInformation.STATUS_ID = consentStatus.CONSENT_TO_CARE_STATUS_ID;
                    }
                    existingInformation.FAILED_ATTEMPTS = 0;
                    _consentToCareRepository.Update(existingInformation);
                    _consentToCareRepository.Save();
                    var existingconsentToCareId = existingInformation.CONSENT_TO_CARE_ID;
                    // send Email Or SMS
                    consentToCareObj.CASE_ID = existingCaseId;
                    consentToCareObj.CONSENT_TO_CARE_ID = existingconsentToCareId;
                    var encryptedUrl = EncryptTemp(existingconsentToCareId.ToString());
                    var decryptioUrl = Decryption(encryptedUrl.ToString());
                    var encryptedEmailURL = GenerateEncryptedEmailURL(consentToCareObj);
                    var emailBody = EmailBody(consentReceiverName, encryptedEmailURL, consentToCareObj.disciplineName);
                    var email = concentToCareReceiverEmail;
                    var number = concentToCareHomePhone;
                    var smsBody = SmsBody(consentReceiverName, encryptedEmailURL, consentToCareObj.disciplineName);
                    if (!string.IsNullOrEmpty(concentToCareReceiverEmail))
                    {
                        Thread emailThread = new Thread(() =>
                        {
                            bool sent = Helper.ConcentToCareEmail(to: email, subject: subject, body: emailBody, profile: profile, CC: null, BCC: null);
                        });
                        emailThread.Start();
                    }
                    if (!string.IsNullOrEmpty(number))
                    {
                        Thread smsThread = new Thread(() =>
                        {
                            var status = SmsService.SMSTwilio(number, smsBody);
                        });
                        smsThread.Start();
                    }
                    List<TaskLog> taskLoglist = new List<TaskLog>();
                    List<string> consentTocarelogs = new List<string>();
                    StringBuilder consentTocarelogsString = new StringBuilder();
                    consentTocarelogs.Add("Consent to care link has been resent to: " + existingInformation.SEND_TO + " (" + consentReceiverName + ")");
                    foreach (string str in consentTocarelogs)
                    {
                        consentTocarelogsString.Append(str + "<br>");
                    }
                    taskLoglist.Add(new TaskLog()
                    {
                        ACTION = "Task Comment",
                        ACTION_DETAIL = consentTocarelogsString.ToString()
                    }
                        );

                    if (taskLoglist.Count() > 0)
                    {
                        profile.UserName = "FOX TEAM";
                        InsertTaskLog(existingInformation.TASK_ID, taskLoglist, profile);
                    }
                    InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                    if (existingInformation != null)
                    {
                        interfaceSynch.TASK_ID = existingInformation.TASK_ID;
                        interfaceSynch.PATIENT_ACCOUNT = existingInformation.PATIENT_ACCOUNT;
                        interfaceSynch.CASE_ID = existingInformation.CASE_ID;
                        ////Task Interface
                        InsertInterfaceTeamData(interfaceSynch, profile);
                    }
                }
            }
            return consentToCareObj;
        }
        public static ServiceConfiguration GetServiceConfiguration(long practiceCode)
        {
            var config = new ServiceConfiguration();
            var configList = SpRepository<ServiceConfiguration>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SERVICE_CONFIGURATION_CONSENT_TO_CARE");
            if (configList.Count() > 0)
            {
                config = configList.Where(e => e.PRACTICE_CODE.HasValue && e.PRACTICE_CODE.Value == practiceCode).FirstOrDefault();
            }
            return config;
        }  
        public ResponseHTMLToPDF HTMLToPDF(ServiceConfiguration config, string htmlString, string fileName, string type, string linkMessage = null)
        {
            try
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlString);
                htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'print-footer')]")?.Remove();
                if (!string.IsNullOrWhiteSpace(linkMessage))
                {
                    var htmlNode_link = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'link')]");
                    if (htmlNode_link != null)
                    {
                        htmlNode_link.InnerHtml = linkMessage;
                    }
                }
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginBottom = 10;
                converter.Options.MarginTop = 10;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.DisplayHeader = false;
                converter.Options.DisplayHeader = false;
                converter.Options.WebPageWidth = 1580;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                PdfDocument doc = converter.ConvertHtmlString(htmlDoc.DocumentNode.OuterHtml);
                string pdfPath = config.ORIGINAL_FILES_PATH_SERVER;
                if (!Directory.Exists(pdfPath))
                {
                    Directory.CreateDirectory(pdfPath);
                }
                fileName += DateTime.Now.Ticks + ".pdf";
                string pdfFilePath = pdfPath + fileName;
                // save pdf document
                doc.Save(pdfFilePath);
                // close pdf document
                doc.Close();
                return new ResponseHTMLToPDF() { FileName = fileName, FilePath = pdfPath, Success = true, ErrorMessage = "" };
            }
            catch (Exception exception)
            {
                return new ResponseHTMLToPDF() { FileName = "", FilePath = "", Success = false, ErrorMessage = exception.ToString() };
            }
        }
        private int getNumberOfPagesOfPDF(string PdfPath)
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(PdfPath);
            return pdfReader.NumberOfPages;
        }
        public int InsertInterfaceTeamData(InterfaceSynchModel obj, UserProfile Profile)
        {
            bool isSync = false;
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();

            if (obj.PATIENT_ACCOUNT == null && obj.CASE_ID == null && obj.TASK_ID == null)
            {

            }
            else
            {
                obj.APPLICATION = "PORTAL";
                SqlParameter pracCode = new SqlParameter("@Practice_code", AppConfiguration.GetPracticeCode);
                var response = SpRepository<string>.GetSingleObjectWithStoreProcedure(@"Exec Af_proc_is_talkrehab_practice @Practice_code", pracCode);
                if (response == null)
                {
                    isSync = false;
                }
                else
                {
                    isSync = true;
                }
                long Pid = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                SqlParameter id = new SqlParameter("ID", Pid);

                SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", AppConfiguration.GetPracticeCode);
                SqlParameter caseId = new SqlParameter("CASE_ID", obj.CASE_ID ?? (object)DBNull.Value);
                SqlParameter workId = new SqlParameter("Work_ID", obj.Work_ID ?? (object)DBNull.Value);
                SqlParameter taskId = new SqlParameter("TASK_ID", obj.TASK_ID ?? (object)DBNull.Value);
                SqlParameter patientAccount = new SqlParameter("PATIENT_ACCOUNT", obj.PATIENT_ACCOUNT);
                SqlParameter userName = new SqlParameter("USER_NAME", Profile.UserName);
                SqlParameter isSynced = new SqlParameter("IS_SYNCED", isSync);
                SqlParameter application = new SqlParameter("APPLICATION", string.IsNullOrEmpty(obj.APPLICATION) ? string.Empty : obj.APPLICATION);
                var result = SpRepository<InterfaceSynchModel>.GetListWithStoreProcedure(@"FOX_PROC_INSERT_INTERFACE_DATA @ID, @PRACTICE_CODE,@CASE_ID,@Work_ID,@TASK_ID,@PATIENT_ACCOUNT,@USER_NAME,@IS_SYNCED,@APPLICATION",
                                                                                                       id, practiceCode, caseId, workId, taskId, patientAccount, userName, isSynced, application);
                return result.Count;
            }
            return 0;
        }
        private FOX_TBL_TASK GetTask(long Practice_Code, long Task_Id)
        {
            SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", Practice_Code);
            SqlParameter TaskID = new SqlParameter("TASK_ID", Task_Id);
            return SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK @PRACTICE_CODE, @TASK_ID", practiceCode, TaskID);

        }
        public FOX_TBL_TASK AddUpdateTask(FOX_TBL_TASK task, UserProfile profile, string sendTo, string consentReceiverName)
        {

            if (!string.IsNullOrEmpty(task.PATIENT_ACCOUNT_STR))
            {
                task.PATIENT_ACCOUNT = Convert.ToInt64(task.PATIENT_ACCOUNT_STR);
            }
            if (task != null && profile != null)
            {
                FOX_TBL_TASK dbTask = GetTask(profile.PracticeCode, task.TASK_ID);
                if (dbTask == null)
                {

                    SqlParameter sendToId = new SqlParameter("SEND_TO_ID", task.SEND_TO_ID ?? (object)DBNull.Value);
                    long primaryKey = Helper.getMaximumId("FOX_TASK_ID");
                    SqlParameter id = new SqlParameter("ID", primaryKey);
                    SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                    SqlParameter patientAccount = new SqlParameter("PATIENT_ACCOUNT", task.PATIENT_ACCOUNT);
                    SqlParameter isCompletedInt = new SqlParameter("IS_COMPLETED_INT", SqlDbType.Int);
                    isCompletedInt.Value = 0;// new SqlParameter("IS_COMPLETED_INT", 0);/*0: Initiated 1:Sender Completed 2:Final Route Completed*/
                    SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                    SqlParameter isTemplate = new SqlParameter("IS_TEMPLATE", task.IS_TEMPLATE);
                    SqlParameter taskTypeId = new SqlParameter("TASK_TYPE_ID", task.TASK_TYPE_ID);
                    SqlParameter finalRouteId = new SqlParameter("FINAL_ROUTE_ID", task.FINAL_ROUTE_ID ?? (object)DBNull.Value);
                    SqlParameter priority = new SqlParameter("PRIORITY", string.IsNullOrEmpty(task.PRIORITY) ? string.Empty : task.PRIORITY);
                    SqlParameter dueDateTime = new SqlParameter("DUE_DATE_TIME", string.IsNullOrEmpty(task.DUE_DATE_TIME_str) ? (object)DBNull.Value : Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str) ?? Helper.GetCurrentDate());
                    task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                    SqlParameter categoryID = new SqlParameter("CATEGORY_ID", task.CATEGORY_ID ?? (object)DBNull.Value);
                    SqlParameter isReqSignedoff = new SqlParameter("IS_REQ_SIGNOFF", task.IS_REQ_SIGNOFF ?? (object)DBNull.Value);
                    SqlParameter isSendingRouteDetails = new SqlParameter("IS_SENDING_ROUTE_DETAILS", task.IS_SENDING_ROUTE_DETAILS ?? (object)DBNull.Value);
                    SqlParameter sendContextId = new SqlParameter("SEND_CONTEXT_ID", task.SEND_CONTEXT_ID ?? (object)DBNull.Value);
                    SqlParameter contextInfo = new SqlParameter("CONTEXT_INFO", task.CONTEXT_INFO ?? (object)DBNull.Value);
                    SqlParameter deliveryId = new SqlParameter("DEVELIVERY_ID", task.DEVELIVERY_ID ?? (object)DBNull.Value);
                    SqlParameter destinations = new SqlParameter("DESTINATIONS", task.DESTINATIONS ?? (object)DBNull.Value);
                    SqlParameter loc_Id = new SqlParameter("LOC_ID", task.LOC_ID ?? (object)DBNull.Value);
                    SqlParameter provider_ID = new SqlParameter("PROVIDER_ID", task.PROVIDER_ID ?? (object)DBNull.Value);
                    SqlParameter isSendMailAuto = new SqlParameter("IS_SEND_EMAIL_AUTO", task.IS_SEND_EMAIL_AUTO ?? (object)DBNull.Value);
                    SqlParameter deleted = new SqlParameter("DELETED", task.DELETED);
                    SqlParameter isSendToUser = new SqlParameter("IS_SEND_TO_USER", task.IS_SEND_TO_USER);
                    SqlParameter isFinalRouteUser = new SqlParameter("IS_FINAL_ROUTE_USER", task.IS_FINAL_ROUTE_USER);
                    SqlParameter isFinalRouteMarkComplete = new SqlParameter("IS_FINALROUTE_MARK_COMPLETE", task.IS_FINALROUTE_MARK_COMPLETE);
                    SqlParameter isSendToMarkComplete = new SqlParameter("IS_SENDTO_MARK_COMPLETE", task.IS_SENDTO_MARK_COMPLETE);
                    SqlParameter caseID = new SqlParameter("CASE_ID", task.CASE_ID);

                    dbTask = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_CONSENT_TO_CARE_TASK @ID, @PRACTICE_CODE,@PATIENT_ACCOUNT ,@IS_COMPLETED_INT ,@USER_NAME,@IS_TEMPLATE,@TASK_TYPE_ID,@SEND_TO_ID,@FINAL_ROUTE_ID,@PRIORITY,@DUE_DATE_TIME,@CATEGORY_ID,@IS_REQ_SIGNOFF,@IS_SENDING_ROUTE_DETAILS,@SEND_CONTEXT_ID,@CONTEXT_INFO,@DEVELIVERY_ID,@DESTINATIONS,@LOC_ID,@PROVIDER_ID,@IS_SEND_EMAIL_AUTO,@DELETED,@IS_SEND_TO_USER,@IS_FINAL_ROUTE_USER,@IS_FINALROUTE_MARK_COMPLETE,@IS_SENDTO_MARK_COMPLETE, @CASE_ID",
                                                                                                                    id, practiceCode, patientAccount, isCompletedInt, userName, isTemplate, taskTypeId, sendToId, finalRouteId, priority, dueDateTime, categoryID, isReqSignedoff, isSendingRouteDetails, sendContextId, contextInfo, deliveryId, destinations, loc_Id, provider_ID, isSendMailAuto, deleted, isSendToUser, isFinalRouteUser, isFinalRouteMarkComplete, isSendToMarkComplete, caseID);
                    dbTask.dbChangeMsg = "TaskInsertSuccessed";
                    dbTask.CATEGORY_CODE = _taskTypeRepository.GetByID(dbTask?.TASK_TYPE_ID)?.CATEGORY_CODE;

                    List<TaskLog> taskLoglist = new List<TaskLog>();
                    List<string> consentTocarelogs = new List<string>();
                    StringBuilder consentTocarelogsString = new StringBuilder();
                    consentTocarelogs.Add("Consent to care link has been sent to: " + sendTo + " (" + consentReceiverName + ")");
                    foreach (string str in consentTocarelogs)
                    {
                        consentTocarelogsString.Append(str + "<br>");
                    }
                    taskLoglist.Add(new TaskLog()
                    {
                        ACTION = "Task Comment",
                        ACTION_DETAIL = consentTocarelogsString.ToString()
                    }
                        );

                    if (taskLoglist.Count() > 0)
                    {
                        InsertTaskLog(dbTask.TASK_ID, taskLoglist, profile);
                    }

                }
                return dbTask;
            }
            return null;
        }
        public void InsertTaskLog(long? taskId, List<TaskLog> tasklog, UserProfile profile)
        {
            if (taskId != null && taskId.Value > 0)
            {
                foreach (var item in tasklog)
                {
                        SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", AppConfiguration.GetPracticeCode);
                        SqlParameter Task_Id = new SqlParameter("TASK_ID", taskId);
                        SqlParameter user_Name = new SqlParameter("USER_NAME", profile.UserName);
                        SqlParameter actionDetails = new SqlParameter("ACTION_DETAIL", item.ACTION_DETAIL);
                        SqlParameter action = new SqlParameter("ACTION", item.ACTION);
                        SpRepository<TaskLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_UPDATE_TASK_LOG  @PRACTICE_CODE, @TASK_ID, @USER_NAME, @ACTION_DETAIL, @ACTION", practice_Code, Task_Id, user_Name, actionDetails, action);
                
               }
            }
        }
        private DataTable GetTaskLogTable(List<TaskLog> lst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ACTION", typeof(string));
            dt.Columns.Add("ACTION_DETAIL", typeof(string));
            foreach (TaskLog task in lst)
            {
                dt.Rows.Add(task.ACTION, task.ACTION_DETAIL);
            }
            return dt;
        }
        private FOX_TBL_TASK SetTaskData(UserProfile profile, long? PATIENT_ACCOUNT, string tasktypeHBR, string CURRENT_DATE_STR, FoxTblConsentToCare objConsentToCare)
        {
            var task = new FOX_TBL_TASK();
            SqlParameter pPracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter pTaskTypeName = new SqlParameter();
            pTaskTypeName.ParameterName = "NAME";
            pTaskTypeName.Value = "XELEC";
            var Task_type_Id = SpRepository<FOX_TBL_TASK_TYPE>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK_ID @PRACTICE_CODE, @NAME", pPracticeCode, pTaskTypeName);
            if(objConsentToCare.TREATING_PROVIDER_ID == 0 || objConsentToCare.TREATING_PROVIDER_ID == null)
            {
                SqlParameter pPractice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                SqlParameter pGroupName = new SqlParameter("GROUP_NAME", "02CC1");
                var group02CC1 = SpRepository<GROUP>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_GROUP_ID @PRACTICE_CODE, @GROUP_NAME", pPractice_Code, pGroupName);
                if (group02CC1 != null)
                {
                    task.SEND_TO_ID = group02CC1.GROUP_ID;
                }
            } 
            else
            {
                task.SEND_TO_ID = objConsentToCare.TREATING_PROVIDER_ID;
                task.LOC_ID = objConsentToCare.POS_ID;
            }
            task.TASK_TYPE_ID = Task_type_Id?.TASK_TYPE_ID ?? 0;
            task.PRACTICE_CODE = profile.PracticeCode;
            task.PATIENT_ACCOUNT = PATIENT_ACCOUNT;
            task.PRIORITY = "NORMAL";
            if (task.DUE_DATE_TIME == null)
            {
                task.DUE_DATE_TIME_str = Convert.ToString(Helper.GetCurrentDate().AddDays(5));
            }
            return task;
        }

        public void AddFilesToDatabase(string filePath, string logoPath, long consentToCareId)
        {
            try
            {
                var consentDocuments = _consentToCareDocumentRepository.GetFirst(x => x.CONSENT_TO_CARE_ID == consentToCareId && x.PRACTICE_CODE == AppConfiguration.GetPracticeCode && !x.DELETED);
                if (consentDocuments == null)
                {
                    ConsentToCareDocument consentToCareDocumentObj = new ConsentToCareDocument();
                    var documentType = _foxDocumentTypeRepository.GetFirst(x => x.NAME == "PCORS" && !x.DELETED);
                    if (documentType != null)
                    {
                        consentToCareDocumentObj.DOCUMENT_TYPE_ID = documentType.DOCUMENT_TYPE_ID;
                    }
                    consentToCareDocumentObj.DOCUMENTS_ID = Helper.getMaximumId("DOCUMENTS_ID");
                    consentToCareDocumentObj.IMAGE_PATH = filePath;
                    consentToCareDocumentObj.LOGO_PATH = logoPath;
                    consentToCareDocumentObj.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                    consentToCareDocumentObj.CONSENT_TO_CARE_ID = consentToCareId;
                    consentToCareDocumentObj.IsSigned = true;
                    _consentToCareDocumentRepository.Insert(consentToCareDocumentObj);
                    _consentToCareDocumentRepository.Save();

                }
                else
                {
                    var documentType = _foxDocumentTypeRepository.GetFirst(x => x.NAME == "PCORS" && !x.DELETED);
                    if (documentType != null)
                    {
                        consentDocuments.DOCUMENT_TYPE_ID = documentType.DOCUMENT_TYPE_ID;
                    }
                    consentDocuments.IMAGE_PATH = filePath;
                    consentDocuments.LOGO_PATH = logoPath;
                    consentDocuments.MODIFIED_DATE = Helper.GetCurrentDate();
                    consentDocuments.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                    consentDocuments.IsSigned = true;
                    _consentToCareDocumentRepository.Update(consentDocuments);
                    _consentToCareDocumentRepository.Save();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void SavePdfToImages(string PdfPath, ServiceConfiguration config, long consentToCareId, int noOfPages)
        {
            List<int> threadCounter = new List<int>();
            if (!Directory.Exists(config.IMAGES_PATH_SERVER))
            {
                Directory.CreateDirectory(config.IMAGES_PATH_SERVER);
            }

            if (System.IO.File.Exists(PdfPath))
            {
                for (int i = 0; i < noOfPages; i++)
                {
                    var imgPath = "";
                    var logoImgPath = "";
                    var imgPathServer = "";
                    var logoImgPathServer = "";
                    Random random = new Random();
                    var randomString = random.Next();
                    imgPath = config.IMAGES_PATH_DB + "\\" + consentToCareId + "_" + i + "_" + randomString + ".jpg";
                    imgPathServer = config.IMAGES_PATH_SERVER + "\\" + consentToCareId + "_" + i + "_" + randomString + ".jpg";

                    randomString = random.Next();
                    logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + consentToCareId + "_" + i + "_" + randomString + ".jpg";
                    logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + consentToCareId + "_" + i + "_" + randomString + ".jpg";

                    Thread myThread = new Thread(() => this.newThreadImplementaion(ref threadCounter, PdfPath, i, imgPathServer, logoImgPathServer));
                    myThread.Start();
                    threadsList.Add(myThread);
                    AddFilesToDatabase(imgPath, logoImgPath, consentToCareId);
                }
                while (noOfPages > threadCounter.Count)
                {
                    //loop untill record complete
                }
                foreach (var thread in threadsList)
                {
                    thread.Abort();
                }
            }
        }
        public void AddToDatabase(string filePath, int noOfPages, string workId, string sorcetype, string sorceName, string userName, bool approval, long? practice_code, bool decline)
        {
            try
            {
                var PracticeCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practice_code };
                var workid = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };
                var username = new SqlParameter { ParameterName = "@USER_NAME", SqlDbType = SqlDbType.VarChar, Value = userName };
                var noofpages = new SqlParameter { ParameterName = "@NO_OF_PAGES", SqlDbType = SqlDbType.Int, Value = noOfPages };
                var approve = new SqlParameter { ParameterName = "@APPROVAL", SqlDbType = SqlDbType.Bit, Value = approval };
                var declined = new SqlParameter { ParameterName = "@DECLINE", SqlDbType = SqlDbType.Bit, Value = decline };

                var result = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_ADD_TODB_FROM_INDEXINFO @PRACTICE_CODE,@WORK_ID,@USER_NAME,@NO_OF_PAGES,@APPROVAL,@DECLINE",
                    PracticeCode, workid, username, noofpages, approve, declined);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void newThreadImplementaion(ref List<int> threadCounter, string PdfPath, int i, string imgPath, string logoImgPath)
        {
            try
            {
                System.Drawing.Image img;
                PdfFocus f = new PdfFocus();
                // f.Serial = "10261435399";
                f.Serial = "80033727929";
                f.OpenPdf(PdfPath);

                if (f.PageCount > 0)
                {
                    //Save all PDF pages to jpeg images
                    f.ImageOptions.Dpi = 120;
                    f.ImageOptions.ImageFormat = ImageFormat.Jpeg;

                    var image = f.ToImage(i + 1);
                    //Next manipulate with Jpeg in memory or save to HDD, open in a viewer
                    using (var ms = new MemoryStream(image))
                    {
                        img = System.Drawing.Image.FromStream(ms);
                        img.Save(imgPath, ImageFormat.Jpeg);
                        Bitmap bmp = new Bitmap(img);
                        ConvertPDFToImages ctp = new ConvertPDFToImages();
                        img.Dispose();
                        ctp.SaveWithNewDimention(bmp, 115, 150, 100, logoImgPath);
                        bmp.Dispose();
                    }
                }
                threadCounter.Add(1);
            }
            catch (Exception)
            {
                threadCounter.Add(1);
            }
        }
        private string HTMLToPDFSautinsoft(ServiceConfiguration conf, string htmlString, string fileName, string linkMessage = null)
        {
            try
            {
                PdfMetamorphosis p = new PdfMetamorphosis();
                p.Serial = "10262870570";
                p.PageSettings.Size.A4();
                //p.PageSettings.Orientation = PdfMetamorphosis.PageSetting.Orientations.Portrait;
                p.PageSettings.Orientation = PdfMetamorphosis.PageSetting.Orientations.Landscape;
                p.PageSettings.MarginLeft.Inch(0.1f);
                p.PageSettings.MarginRight.Inch(0.1f);
                if (p != null)
                {
                    string pdfFilePath = Path.Combine(conf.DOCUMENTS_PATH_SERVER);
                    //string finalsetpath = conf.ORIGINAL_FILES_PATH_SERVER.Remove(conf.ORIGINAL_FILES_PATH_SERVER.Length - 1);
                    if (!Directory.Exists(pdfFilePath))
                    {
                        Directory.CreateDirectory(pdfFilePath);
                    }
                    fileName = fileName + DateTime.Now.Ticks + ".pdf";
                    string pdfFilePathnew = pdfFilePath + "\\" + fileName;
                    if (p.HtmlToPdfConvertStringToFile(htmlString, pdfFilePathnew) == 0)
                    {
                        return pdfFilePathnew;
                    }
                    else
                    {
                        var ex = p.TraceSettings.ExceptionList.Count > 0 ? p.TraceSettings.ExceptionList[0] : null;
                        var msg = ex != null ? ex.Message + Environment.NewLine + ex.StackTrace : "An error occured during converting HTML to PDF!";
                        return "";
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        public ConsentToCareList GetConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            ConsentToCareList response = new ConsentToCareList();
            string statusName = string.Empty;
            var patinetAccount = new SqlParameter("@PATINET_ACCOUNT", SqlDbType.BigInt) { Value = long.Parse(consentToCareObj.PATIENT_ACCOUNT_Str) };
            var selectedCaseId = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
            var pracCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
            var patinetAccountForCheckSignedConcent = new SqlParameter("@PATINET_ACCOUNT", SqlDbType.BigInt) { Value = long.Parse(consentToCareObj.PATIENT_ACCOUNT_Str) };
            var selectedCaseIdForCheckSignedConcent = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
            var pracCodeForCheckSignedConcent = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
            var alreadySentToSameDiscipline = SpRepository<FoxTblConsentToCare>.GetListWithStoreProcedure(@"EXEC FOX_PROC_CHECK_IF_CCARE_ALREADY_SENT_TO_SAME_DISCIPLINE @PATINET_ACCOUNT, @CASE_ID, @PRACTICE_CODE", patinetAccount, selectedCaseId, pracCode);
            var signedReviceToSameDiscipline = SpRepository<FoxTblConsentToCare>.GetListWithStoreProcedure(@"EXEC FOX_PROC_CHECK_IF_CCARE_ALREADY_SIGNED_SENT_TO_SAME_DISCIPLINE @PATINET_ACCOUNT, @CASE_ID, @PRACTICE_CODE", patinetAccountForCheckSignedConcent, selectedCaseIdForCheckSignedConcent, pracCodeForCheckSignedConcent);
            if (consentToCareObj != null)
            {
                var caseId = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
                var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                response.ConsentToCareDbList = SpRepository<FoxTblConsentToCare>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID @CASE_ID, @PRACTICE_CODE", caseId, practiceCode);
            }
            if (signedReviceToSameDiscipline.Count != 0)
            {
                statusName = signedReviceToSameDiscipline[0].STATUS_NAME;
                alreadySentToSameDiscipline[0].CASE_NO = signedReviceToSameDiscipline[0].CASE_NO;
            }
            else
            {
                statusName = "";
            }
            if(alreadySentToSameDiscipline[0].alreadySentToSameDiscipline == 1 && response.ConsentToCareDbList.Count == 0 && statusName != "Signed")
            {
                response.ConsentToCareDbList = alreadySentToSameDiscipline;
            }
            return response;
        }
            public ConsentToCareList GetConsentToCareDocumentsInfo(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            List<FoxTblConsentToCare> consentToCareList = new List<FoxTblConsentToCare>();
            ConsentToCareList response = new ConsentToCareList();
            if (consentToCareObj != null)
            {
                var caseId = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
                var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                response.ConsentToCareDbList = SpRepository<FoxTblConsentToCare>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID @CASE_ID, @PRACTICE_CODE", caseId, practiceCode);
            }
            return response;
        }
        public ConsentToCareList GetConsentToCareImagePath(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            ConsentToCareList response = new ConsentToCareList();
            if (consentToCareObj != null)
            {
                var consentToCareId = new SqlParameter("@CONSENT_TO_CARE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CONSENT_TO_CARE_ID };
                var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                response.ConsentToCareDbList = SpRepository<FoxTblConsentToCare>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_DOCUMENTS_INFO @CONSENT_TO_CARE_ID, @PRACTICE_CODE", consentToCareId, practiceCode);
            }
            return response;
        }

        public long GetPracticeCode()
        {
            long practiceCode = Convert.ToInt64(WebConfigurationManager.AppSettings?["GetPracticeCode"]);
            return practiceCode;
        }
        public string GeneratePdfForConcentToCare(FoxTblConsentToCare consentToCareObj)
        {
            try
            {
                var practiceCode = GetPracticeCode();
                var config = GetServiceConfiguration(practiceCode);
                var htmlTemplate = consentToCareObj.TemplateHtmlWithInsuranceDetails;
                var consentToCareIdStr = consentToCareObj.CONSENT_TO_CARE_ID.ToString();
                var updatedHtml = consentToCareObj.TemplateHtmlWithInsuranceDetails;
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-foxrehab-url")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-checkbox")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-sign-form")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-download-pdf-br")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-download-pdf-b")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-download-pdf")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-contactus")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-check-eligibility")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-contactus-questions")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-foxrehab-url-br")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-sign-form-br")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-contactus-questions-br")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                htmlDoc.GetElementbyId("consent-to-care-check-eligibility-br")?.Remove();
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                htmlDoc.LoadHtml(updatedHtml);
                updatedHtml = htmlDoc.DocumentNode.OuterHtml;
                //HTML to PDF
                htmlToPdfResponseObj = new ResponseHTMLToPDF();
                htmlToPdfResponseObj = HTMLToPDF(config, updatedHtml, consentToCareObj.CONSENT_TO_CARE_ID.ToString(), "email", "");
                var coverFilePath = htmlToPdfResponseObj.FilePath + "\\" + htmlToPdfResponseObj.FileName;
                //var coverFilePath = HTMLToPDFSautinsoft(config, htmlTemplate, consentToCareIdStr);
                var consentToCareID = consentToCareObj.CONSENT_TO_CARE_ID;
                var CASE_ID = consentToCareObj.CASE_ID;
                var currentConsentToCareId = consentToCareObj.CONSENT_TO_CARE_ID;
                int numberOfPages = getNumberOfPagesOfPDF(coverFilePath);
                SavePdfToImages(coverFilePath, config, currentConsentToCareId, numberOfPages);
                return coverFilePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string EmailBody(string patientFirstName, string link, string disciplineName)
        {
            string mailBody = string.Empty;
            string templatePathOfSenderEmail = AppDomain.CurrentDomain.BaseDirectory;
            templatePathOfSenderEmail = templatePathOfSenderEmail.Replace(@"\bin\Debug", "") + "HtmlTemplates\\Consent-To-Care-Email_Template.html";
            if (File.Exists(templatePathOfSenderEmail))
            {
                mailBody = File.ReadAllText(templatePathOfSenderEmail);
                mailBody = mailBody.Replace("[[PATIENT_FIRST_NAME]]", patientFirstName);
                mailBody = mailBody.Replace("[[DISCIPLINE_NAME]]", disciplineName);
                mailBody = mailBody.Replace("[[LINK]]", link);
            }
            return mailBody ?? "";
        }
        public static string GenerateEncryptedEmailURL(FoxTblConsentToCare consentToCareObj)
        {
            string encryptedUrl = string.Empty;
            if (consentToCareObj != null)
            {
                string environmentURL = GetClientURL() + "#/ConsentToCare";
                encryptedUrl = EncryptTemp(consentToCareObj.CONSENT_TO_CARE_ID.ToString());
                encryptedUrl = environmentURL + "?" + encryptedUrl;
            }
            return encryptedUrl;
        }
        public static string GetClientURL()
        {
            string url = ConfigurationManager.AppSettings?["ClientURL"].ToString();
            return url ?? "";
        }
        public static string EncryptTemp(string plainText)
        {
            try
            {
                // To handle encryption/decryption Objective-C,C#
                string passPhrase = "2657894562368456";
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(passPhrase)));
            }
            catch (Exception ex)
            {
                return "exception : " + ex.Message;
            }


        }
        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        // To handle encryption/decryption java,C#
        private static RijndaelManaged GetRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }
        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = UTF8Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        // Description: This function is get survey method details
        private byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }
        public string Decryption(string encryptedText)
        {

            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(passPhrase)));
            }
            catch (Exception ex)
            {
                return "exception : " + ex.Message;
            }

        }
        // Description: This function is decrypt patient account number & handle the flow of Unsubscribe Email & SMS
        public FoxTblConsentToCare DecryptionUrl(FoxTblConsentToCare consentToCareObj)
        {
            ConsentToCareResponse consentToCareResponse = new ConsentToCareResponse();
            var decryptioUrl = Decryption(consentToCareObj.encryptedCaseId.ToString());
            consentToCareObj.CONSENT_TO_CARE_ID = int.Parse(decryptioUrl);
            var consentToCareId = new SqlParameter("@CONSENT_TO_CARE_ID", SqlDbType.VarChar) { Value = consentToCareObj.CONSENT_TO_CARE_ID };
            var practiceCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                consentToCareObj = SpRepository<FoxTblConsentToCare>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CONSENT_TO_CARE_ID @CONSENT_TO_CARE_ID, @PRACTICE_CODE", consentToCareId, practiceCode);
            if (consentToCareObj != null)
            {
                consentToCareObj.PATIENT_ACCOUNT_Str = consentToCareObj.PATIENT_ACCOUNT.ToString();
                consentToCareObj.OrderingRefNotes = consentToCareObj.OrderingRefNotes == null ? "" : " from " + consentToCareObj.OrderingRefNotes;
            }    
            return consentToCareObj;
        }
        // Description: This function is decrypt patient account number & handle the flow of Unsubscribe Email & SMS
        public bool DobVerificationInvalidAttempt(AddInvalidAttemptRequest addInvalidAttemptRequestObj, UserProfile profile)
        {
            bool invalidAttemptsLimitExceed = false;
            var dbResult = _consentToCareRepository.GetFirst(x => x.CONSENT_TO_CARE_ID == addInvalidAttemptRequestObj.CONSENT_TO_CARE_ID && !x.DELETED);
            if (dbResult != null)
            {
                dbResult.FAILED_ATTEMPTS = (dbResult.FAILED_ATTEMPTS == null ? 0 : dbResult.FAILED_ATTEMPTS) + 1;

                _consentToCareRepository.Update(dbResult);
                _consentToCareRepository.Save();
                if (dbResult.FAILED_ATTEMPTS >= 5)
                {
                    var consentStatus = _consentToCareStatusRepository.GetFirst(x => x.STATUS_NAME == "Expired" && x.PRACTICE_CODE == AppConfiguration.GetPracticeCode && !x.DELETED);
                    if (consentStatus != null)
                    {
                        dbResult.STATUS_ID = consentStatus.CONSENT_TO_CARE_STATUS_ID;
                        /// consentToCareObj.STATUS = consentStatus.STATUS_NAME;
                    }
                    _consentToCareRepository.Update(dbResult);
                    _consentToCareRepository.Save();
                    string consentReceiverName = string.Empty;
                    if (dbResult.SENT_TO_ID != 0 && dbResult.SEND_TO != "Patient")
                    {
                        var patinetContactID = dbResult.SENT_TO_ID;
                        var conList = _PatientContactRepository.GetFirst(x => x.Contact_ID == dbResult.SENT_TO_ID && x.Deleted == false);
                        if (conList != null)
                        {
                            consentReceiverName = conList.Last_Name == null ? "" : conList.Last_Name;
                        }
                    }
                    else
                    {
                        var patient = _PatientRepository.GetFirst(e => e.Patient_Account == dbResult.PATIENT_ACCOUNT && (e.DELETED ?? false) == false);
                        if (patient != null)
                        {
                            consentReceiverName = patient.Last_Name == null ? "" : patient.Last_Name;
                        }
                    }
                    List<TaskLog> taskLoglist = new List<TaskLog>();
                    List<string> consentTocarelogs = new List<string>();
                    StringBuilder consentTocarelogsString = new StringBuilder();
                    consentTocarelogs.Add("Consent to Care link has been expired due to invalid attempts by: " + dbResult.SEND_TO + " (" + consentReceiverName + ")");
                    foreach (string str in consentTocarelogs)
                    {
                        consentTocarelogsString.Append(str + "<br>");
                    }
                    taskLoglist.Add(new TaskLog()
                    {
                        ACTION = "Task Comment",
                        ACTION_DETAIL = consentTocarelogsString.ToString()
                    }
                        );

                    if (taskLoglist.Count() > 0)
                    {
                        profile.UserName = "FOX TEAM";
                        InsertTaskLog(dbResult.TASK_ID, taskLoglist, profile);
                    }
                    InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                    if (dbResult != null)
                    {
                        interfaceSynch.TASK_ID = dbResult.TASK_ID;
                        interfaceSynch.PATIENT_ACCOUNT = dbResult.PATIENT_ACCOUNT;
                        interfaceSynch.CASE_ID = dbResult.CASE_ID;
                        ////Task Interface
                        InsertInterfaceTeamData(interfaceSynch, profile);
                    }

                    invalidAttemptsLimitExceed = true;
                }
            }
            return invalidAttemptsLimitExceed;
        }
        public bool CommentOnCallTap(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            bool CommentOnCallTap = false;
            var dbResult = _consentToCareRepository.GetFirst(x => x.CONSENT_TO_CARE_ID == consentToCareObj.CONSENT_TO_CARE_ID && !x.DELETED);
                List<TaskLog> taskLoglist = new List<TaskLog>();
                List<string> consentTocarelogs = new List<string>();
                StringBuilder consentTocarelogsString = new StringBuilder();
                consentTocarelogs.Add("Patient need to talk with someone before showing consent");
                foreach (string str in consentTocarelogs)
                {
                    consentTocarelogsString.Append(str + "<br>");
                }
                taskLoglist.Add(new TaskLog()
                {
                    ACTION = "Task Comment",
                    ACTION_DETAIL = consentTocarelogsString.ToString()
                }
                    );

                if (taskLoglist.Count() > 0)
                {
                    profile.UserName = "FOX TEAM";
                    InsertTaskLog(dbResult.TASK_ID, taskLoglist, profile);
                }
                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                if (dbResult != null)
                {
                    interfaceSynch.TASK_ID = dbResult.TASK_ID;
                    interfaceSynch.PATIENT_ACCOUNT = dbResult.PATIENT_ACCOUNT;
                    interfaceSynch.CASE_ID = dbResult.CASE_ID;
                    ////Task Interface
                    InsertInterfaceTeamData(interfaceSynch, profile);
                }

            CommentOnCallTap = true;
            return CommentOnCallTap;
        }
        public FoxTblConsentToCare SubmitConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            var config = GetServiceConfiguration(AppConfiguration.GetPracticeCode);
            var updatedHtml = consentToCareObj.TEMPLATE_HTML;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-foxrehab-url")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-checkbox")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-sign-form")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-download-pdf-br")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-download-pdf-b")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-download-pdf")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-contactus")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-check-eligibility")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-contactus-questions")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-foxrehab-url-br")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-sign-form-br")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-contactus-questions-br")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            htmlDoc.GetElementbyId("consent-to-care-check-eligibility-br")?.Remove();
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            htmlDoc.LoadHtml(updatedHtml);
            updatedHtml = htmlDoc.DocumentNode.OuterHtml;
            consentToCareObj = _consentToCareRepository.GetFirst(x => x.CONSENT_TO_CARE_ID == consentToCareObj.CONSENT_TO_CARE_ID && !x.DELETED);
            var consentStatus = _consentToCareStatusRepository.GetFirst(x => x.STATUS_NAME == "Signed" && x.PRACTICE_CODE == consentToCareObj.PRACTICE_CODE && !x.DELETED);
            if (consentStatus != null)
            {
                consentToCareObj.STATUS_ID = consentStatus.CONSENT_TO_CARE_STATUS_ID;
            }
            consentToCareObj.MODIFIED_DATE = Helper.GetCurrentDate();
            //HTML to PDF
            htmlToPdfResponseObj = new ResponseHTMLToPDF();
            htmlToPdfResponseObj = HTMLToPDF(config, updatedHtml, consentToCareObj.CONSENT_TO_CARE_ID.ToString(), "email", "");
            var coverFilePath = htmlToPdfResponseObj.FilePath + "\\" + htmlToPdfResponseObj.FileName;
            var consentToCareID = consentToCareObj.CONSENT_TO_CARE_ID;
            var CASE_ID = consentToCareObj.CASE_ID;
            //Save PDF to Images and Save in consent-to-care document table
            int numberOfPages = getNumberOfPagesOfPDF(coverFilePath);
            SavePdfToImages(coverFilePath, config, consentToCareObj.CONSENT_TO_CARE_ID, numberOfPages);
            //Update consent table data
            if (consentToCareObj.SENT_TO_ID != 0 && consentToCareObj.SEND_TO != "Patient")
            {
                var patinetContactID = consentToCareObj.SENT_TO_ID;
                var conList = _PatientContactRepository.GetFirst(x => x.Contact_ID == consentToCareObj.SENT_TO_ID && x.Deleted == false);
                if(conList != null)
                {
                    var signatoryName = conList.Last_Name + ", " + conList.First_Name;
                    consentToCareObj.SIGNATORY = signatoryName;
                }
            }
            else
            {
                var patient = _PatientRepository.GetFirst(e => e.Patient_Account == consentToCareObj.PATIENT_ACCOUNT && (e.DELETED ?? false) == false);
                if(patient != null)
                {
                    consentToCareObj.SIGNATORY = patient.Last_Name + ", " + patient.First_Name;
                }
            }
            consentToCareObj.SIGNED_PDF_PATH = coverFilePath;
            _consentToCareRepository.Update(consentToCareObj);
            _consentToCareRepository.Save();

            //Add task logs
            Helper.TokenTaskCancellationExceptionLog("_PatientContactRepository START");
            string consentReceiverName = string.Empty;
            if (consentToCareObj.SENT_TO_ID != 0 && consentToCareObj.SEND_TO != "Patient")
            {
                var patinetContactID = consentToCareObj.SENT_TO_ID;
                var conList = _PatientContactRepository.GetFirst(x => x.Contact_ID == consentToCareObj.SENT_TO_ID && x.Deleted == false);
                if (conList != null)
                {
                    consentReceiverName = conList.Last_Name;
                }
            }
            else
            {
                consentReceiverName = consentToCareObj.SIGNATORY;
            }
            Helper.TokenTaskCancellationExceptionLog("_PatientContactRepository END");
            Helper.TokenTaskCancellationExceptionLog("Add task logs START");
            List<TaskLog> taskLoglist = new List<TaskLog>();
            List<string> consentTocarelogs = new List<string>();
            consentTocarelogs.Add("Signed Consent to Care form has been received from: " + consentToCareObj.SEND_TO + " (" + consentReceiverName + ")");
            StringBuilder consentTocarelogsString = new StringBuilder();
            foreach (string str in consentTocarelogs)
            {
                consentTocarelogsString.Append(str + "<br>");
            }
            taskLoglist.Add(new TaskLog()
            {
                ACTION = "Task Comment",
                ACTION_DETAIL = consentTocarelogsString.ToString()
            }
                );

            if (taskLoglist.Count() > 0)
            {
                profile.UserName = consentToCareObj.CREATED_BY;
                InsertTaskLog(consentToCareObj.TASK_ID, taskLoglist, profile);
            }
            Helper.TokenTaskCancellationExceptionLog("Add task logs END");
            //Task mark as complete
            Helper.TokenTaskCancellationExceptionLog("_TaskRepository START");
            var dbTask = _TaskRepository.GetSingleOrDefault(x => x.TASK_ID == consentToCareObj.TASK_ID && !x.DELETED && x.PRACTICE_CODE == consentToCareObj.PRACTICE_CODE);
            Helper.TokenTaskCancellationExceptionLog("_TaskRepository END");
            Helper.TokenTaskCancellationExceptionLog("dbTask START");
            if (dbTask != null)
            {
                dbTask.IS_SENDTO_MARK_COMPLETE = true;
                dbTask.Completed_Date = Helper.GetCurrentDate();
                _TaskRepository.Update(dbTask);
                _TaskRepository.Save();
            }
            Helper.TokenTaskCancellationExceptionLog("dbTask END");
            Helper.TokenTaskCancellationExceptionLog("InterfaceSynchModel START");
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            if (dbTask != null)
            {
                interfaceSynch.TASK_ID = dbTask.TASK_ID;
                interfaceSynch.PATIENT_ACCOUNT = consentToCareObj.PATIENT_ACCOUNT;
                interfaceSynch.CASE_ID = consentToCareObj.CASE_ID;
                ////Task Interface
                InsertInterfaceTeamData(interfaceSynch, profile);
            }
            Helper.TokenTaskCancellationExceptionLog("InterfaceSynchModel END");
            return consentToCareObj;
        }
        #endregion
        #region Email & SMS body 
        // Description: This function is used forcreate SMS body
        public static string SmsBody(string patientFirstName, string link, string disciplineName)
        {
            string smsBody = "Dear " + patientFirstName + "\n \nThis is FOX Rehabilitation. We would like to obtain your consent for " + disciplineName + " services with us. Please click the link to verify you are the correct recipient and view additional details:\n\n" + link + "\n\nSincerely,\nFOX Rehabilitation Client Services Team\n1-877-407-3422"+ ",Option. 4\nwww.foxrehab.org";
            return smsBody ?? "";
        }
        // Description: This function is decrypt patient account number & handle the flow of Unsubscribe Email & SMS
        public List<InsuranceDetails> GetInsuranceDetails(FoxTblConsentToCare insuranceDetailsObj, UserProfile profile)
        {
            List<InsuranceDetails> insuranceDetailsList = new List<InsuranceDetails>();
            var patientAccount = new SqlParameter("@PATINET_ACCOUNT", SqlDbType.BigInt) { Value = long.Parse(insuranceDetailsObj.PATIENT_ACCOUNT_Str) };
            var caseID = new SqlParameter("@CASE_ID", SqlDbType.VarChar) { Value = insuranceDetailsObj.CASE_ID };
            var practiceCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
            insuranceDetailsList = SpRepository<InsuranceDetails>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_INSURANCE_DETAILS_FOR_CONSENT_TO_CARE @PATINET_ACCOUNT, @CASE_ID", patientAccount, caseID);
            return insuranceDetailsList;
        }
        #endregion
    }
}
