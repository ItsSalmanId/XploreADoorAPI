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
        public SurveyLink DecryptionUrl(SurveyLink objsurveyLink)
        {
            if(objsurveyLink != null && objsurveyLink.ENCRYPTED_PATIENT_ACCOUNT != null)
            {
                objsurveyLink.ENCRYPTED_PATIENT_ACCOUNT = Decryption(objsurveyLink.ENCRYPTED_PATIENT_ACCOUNT);
            }
            return objsurveyLink;
        }
        public SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation)
        {
            if (objSurveyAutomation != null && objSurveyAutomation.PATIENT_ACCOUNT != "")
            {
                SqlParameter patientAccountNumber = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = objSurveyAutomation.PATIENT_ACCOUNT };
                var existingDetailInfo = SpRepository<SurveyAutomationLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PERFORM_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT", patientAccountNumber);
                if (existingDetailInfo != null)
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

        public static string Decryption(string patientAccount)
        {
            string decryptedPatientAccount = string.Empty;
            if (patientAccount != null)
            {
                var enencryptedPatientAccount = patientAccount;
                string removeFirst = enencryptedPatientAccount.Remove(0, 1);
                string replaceString = (removeFirst.Replace("#", ""));
                decryptedPatientAccount = Decrypt(replaceString, "sblw-3hn8-sqoy19");
            }
            return decryptedPatientAccount;
        }

        public List<SurveyQuestions> GetSurveyQuestionDetails(SurveyLink objsurveyLink)
        {
            List<SurveyQuestions> surveyQuestionsList = new List<SurveyQuestions>();
            if (objsurveyLink != null && objsurveyLink.ENCRYPTED_PATIENT_ACCOUNT !="")
            {
                SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.VarChar, Value = objsurveyLink.ENCRYPTED_PATIENT_ACCOUNT };
                surveyQuestionsList = SpRepository<SurveyQuestions>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_QUESTION  @PATIENT_ACCOUNT", patientAccount);
            }
            return surveyQuestionsList;
        }
        #region Decryption
        public static string Decrypt(string input, string key)
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
        #endregion
        public long GetPracticeCode()
        {
            long practiceCode = Convert.ToInt64(WebConfigurationManager.AppSettings?["GetPracticeCode"]);
            return practiceCode;
        }
        public ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null && objPatientSurvey.PATIENT_ACCOUNT_NUMBER != 0)
                {
                    long practiceCode = GetPracticeCode();
                    var existingDetailInfo = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.DELETED == false);
                    PatientSurvey patientSurvey = new PatientSurvey();
                    if (existingDetailInfo != null)
                    {
                        objPatientSurvey.SURVEY_ID = existingDetailInfo.SURVEY_ID;
                        AddPatientSurvey(objPatientSurvey);
                        existingDetailInfo.IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ;
                        existingDetailInfo.IS_REFERABLE = objPatientSurvey.IS_REFERABLE;
                        existingDetailInfo.IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION;
                        existingDetailInfo.FEEDBACK = objPatientSurvey.FEEDBACK;
                        existingDetailInfo.SURVEY_STATUS_BASE = "Completed";
                        existingDetailInfo.SURVEY_STATUS_CHILD = "Completed Survey";
                        existingDetailInfo.MODIFIED_BY = "1163testing";
                        existingDetailInfo.SURVEY_FLAG = "Green";
                        existingDetailInfo.IS_SURVEYED = true;
                        existingDetailInfo.IN_PROGRESS = false;
                        existingDetailInfo.SURVEY_FORMAT_TYPE = "New Format";
                        existingDetailInfo.SURVEY_COMPLETED_DATE = Helper.GetCurrentDate();
                        existingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                        existingDetailInfo.DELETED = false;
                        _patientSurveyRepository.Update(existingDetailInfo);
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
