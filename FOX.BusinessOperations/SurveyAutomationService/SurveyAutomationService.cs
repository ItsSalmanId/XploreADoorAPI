using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.PatientSurvey;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public class SurveyAutomationService : ISurveyAutomationService
    {
        private readonly DBContextSurveyAutomation _surveyAutomationContext = new DBContextSurveyAutomation();
        private readonly GenericRepository<PatientSurveyHistory> _patientSurveyHistoryRepository;
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;

        #region CONSTRUCTOR
        public SurveyAutomationService()
        {
            _patientSurveyHistoryRepository = new GenericRepository<PatientSurveyHistory>(_surveyAutomationContext);
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_surveyAutomationContext);
        }
        #endregion
        #region FUNCTIONS
        // Description: This function is decrypt patient account number
        public SurveyLink DecryptionUrl(SurveyLink objSurveyLink)
        {
            if(objSurveyLink != null && objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT != null)
            {
                objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT = Decryption(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT);
            }
            return objSurveyLink;
        }
        // Description: This function is get patient details
        public SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation)
        {
            if (objSurveyAutomation != null && !string.IsNullOrEmpty(objSurveyAutomation.PATIENT_ACCOUNT))
            {
                SqlParameter patientAccountNumber = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = objSurveyAutomation.PATIENT_ACCOUNT };
                var performSurveyhistory = SpRepository<SurveyAutomationLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PERFORM_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT", patientAccountNumber);
                if (performSurveyhistory != null)
                {
                    objSurveyAutomation = null;
                }
                else
                {
                    SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = objSurveyAutomation.PATIENT_ACCOUNT };
                    objSurveyAutomation = SpRepository<SurveyAutomation>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT", patientAccount);
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
                long getPracticeCode = GetPracticeCode();
                SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.VarChar, Value = objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT };
                SqlParameter practiceCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = getPracticeCode };
                surveyQuestionsList = SpRepository<SurveyQuestions>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_QUESTION  @PATIENT_ACCOUNT, @PRACTICE_CODE", patientAccount, practiceCode);
            }
            return surveyQuestionsList;
        }
        #region Decryption
        // Description: This function is decrypt patient account number (Sub function of DecryptionUrl)
        public static string Decryption(string patientAccount)
        {
            string decryptedPatientAccount = string.Empty;
            if (!string.IsNullOrEmpty(patientAccount))
            {
                var enencryptedPatientAccount = patientAccount;
                string removeFirst = enencryptedPatientAccount.Remove(0, 1);
                string replaceString = (removeFirst.Replace("#", ""));
                decryptedPatientAccount = Decrypt(replaceString, "sblw-3hn8-sqoy19");
            }
            return decryptedPatientAccount;
        }
        // Description: This function is decrypt patient account number (Sub function of DecryptionUrl)
        public static string Decrypt(string input, string key)
        {
            try
            {
                byte[] inputArray = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider
                {
                    Key = UTF8Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion
        // Description: This function is trigger to get practice code
        public long GetPracticeCode()
        {
            long practiceCode = Convert.ToInt64(WebConfigurationManager.AppSettings?["GetPracticeCode"]);
            return practiceCode;
        }
        // Description: This function is trigger to update patient survey model
        public ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null && objPatientSurvey.PATIENT_ACCOUNT_NUMBER != 0)
                {
                    long practiceCode = GetPracticeCode();
                    var existingPatientDetails = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.DELETED == false);
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
                        existingPatientDetails.MODIFIED_BY = "1163testing";
                        existingPatientDetails.SURVEY_FLAG = "Green";
                        existingPatientDetails.IS_SURVEYED = true;
                        existingPatientDetails.IN_PROGRESS = false;
                        existingPatientDetails.SURVEY_FORMAT_TYPE = "New Format";
                        existingPatientDetails.SURVEY_COMPLETED_DATE = Helper.GetCurrentDate();
                        existingPatientDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                        existingPatientDetails.DELETED = false;
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
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null)
                {
                    long practiceCode = GetPracticeCode();
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
                        SURVEY_FLAG = "Green",
                        DELETED = false,
                        CREATED_BY = "FOX-TEAM",
                        SURVEY_DATE = Helper.GetCurrentDate(),
                        CREATED_DATE = Helper.GetCurrentDate()
                    };
                    _patientSurveyHistoryRepository.Insert(patientSurveyHistory);
                    _patientSurveyHistoryRepository.Save();
                    response.Message = "Suvery completed successfully";
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = "Suvery not completed successfully";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
