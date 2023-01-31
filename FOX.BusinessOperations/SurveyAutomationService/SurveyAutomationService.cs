using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
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
            if(objSurveyLink != null && !string.IsNullOrEmpty(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT))
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
                long tempSurveyId = long.Parse(objSurveyAutomation.PATIENT_ACCOUNT);
                var existingPatientDetails = _patientSurveyRepository.GetFirst(r => r.SURVEY_ID == tempSurveyId && r.DELETED == false);
                long getPracticeCode = AppConfiguration.GetPracticeCode;
                SqlParameter pracCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = getPracticeCode };
                SqlParameter patientAccountNumber = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = existingPatientDetails.PATIENT_ACCOUNT_NUMBER };
                SqlParameter surveyId = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = tempSurveyId };
                var performSurveyHistory = SpRepository<SurveyAutomationLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PERFORM_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT, @PRACTICE_CODE, @SURVEY_ID", patientAccountNumber, pracCode, surveyId);
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
                        long practiceCode = AppConfiguration.GetPracticeCode;
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
                        CREATED_BY = "FOX-TEAM",
                        SURVEY_DATE = Helper.GetCurrentDate(),
                        CREATED_DATE = Helper.GetCurrentDate()
                    };
                    _patientSurveyHistoryRepository.Insert(patientSurveyHistory);
                    _patientSurveyHistoryRepository.Save();
                }
        }
        #endregion
    }
}
