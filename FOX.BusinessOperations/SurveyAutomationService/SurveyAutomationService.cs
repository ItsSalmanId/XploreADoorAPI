using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using FOX.DataModels.Models.SurveyAutomation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public class SurveyAutomationService : ISurveyAutomationService
    {
        private readonly DBContextSurveyAutomation _surveyAutomationContext = new DBContextSurveyAutomation();
        private readonly GenericRepository<Patient> _patientRepository;
        private readonly GenericRepository<SurveyQuestions> _surveyQuestions;
        private readonly GenericRepository<PatientSurveyHistory> _patientSurveyHistoryRepository;
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        public SurveyAutomationService()
        {
            _patientRepository = new GenericRepository<Patient>(_surveyAutomationContext);
            _surveyQuestions = new GenericRepository<SurveyQuestions>(_surveyAutomationContext);
            _patientSurveyHistoryRepository = new GenericRepository<PatientSurveyHistory>(_surveyAutomationContext);
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_surveyAutomationContext);
        }

        public SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation)
        {
            SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = objSurveyAutomation.Patient_AccountStr };
            var patientDetails = SpRepository<SurveyAutomation>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT", patientAccount);
            return patientDetails;
        }
        public List<SurveyQuestions> GetSurveyQuestionDetails(string patinetAccount)
        {
            List<SurveyQuestions> surveyQuestionsList = new List<SurveyQuestions>();
           // var patientAccountNo = Helper.getDBNullOrValue("PATIENT_ACCOUNT", patinetAccount);
            //SqlParameter patientAccountNo = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = SqlDbType.VarChar, Value = patinetAccount ?? (object)DBNull.Value };
            SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.VarChar, Value = patinetAccount };
            //var patientQuestionDetails = SpRepository<SurveyQuestions>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_QUESTIONS @PATIENT_ACCOUNT", patientAccountNo);
            surveyQuestionsList = SpRepository<SurveyQuestions>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_QUESTIONS  @PATIENT_ACCOUNT", patientAccount);
            //surveyQuestionsList = _surveyQuestions.GetMany(s =>  s.DELETED == false).ToList();
            return surveyQuestionsList;
        }
        #region Useless Code 
        public List<FoxRoles> GetFoxRoles(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }
        public static string Decrypt(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion
        public ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null)
                {
                    var existingDetailInfo = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.DELETED == false);
                    objPatientSurvey.SURVEY_ID = existingDetailInfo.SURVEY_ID;
                    AddPatientSurvey(objPatientSurvey, profile);
                    PatientSurvey patientSurvey = new PatientSurvey();
                    if (existingDetailInfo != null)
                    {
                        existingDetailInfo.IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ;
                        //existingDetailInfo.IS_RESPONSED_BY_HQ = objPatientSurvey.IS_RESPONSED_BY_HQ;
                        existingDetailInfo.IS_REFERABLE = objPatientSurvey.IS_REFERABLE;
                        existingDetailInfo.IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION;
                        //existingDetailInfo.IS_QUESTION_ANSWERED = objPatientSurvey.IS_QUESTION_ANSWERED;
                        existingDetailInfo.FEEDBACK = objPatientSurvey.FEEDBACK;
                        existingDetailInfo.SURVEY_STATUS_BASE = "Completed";
                        existingDetailInfo.SURVEY_STATUS_CHILD = "Completed Survey";
                        existingDetailInfo.MODIFIED_BY = "FOX-TEAM";
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
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        private void AddPatientSurvey(PatientSurvey objPatientSurvey, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null)
                {
                    PatientSurveyHistory patientSurveyHistory = new PatientSurveyHistory();
                    patientSurveyHistory.SURVEY_HISTORY_ID = Helper.getMaximumId("SURVEY_HISTORY_ID");
                    patientSurveyHistory.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                    patientSurveyHistory.PATIENT_ACCOUNT = objPatientSurvey.PATIENT_ACCOUNT_NUMBER;
                    patientSurveyHistory.PRACTICE_CODE = profile.PracticeCode;
                    patientSurveyHistory.IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ;
                    //patientSurveyHistory.IS_RESPONSED_BY_HQ = objPatientSurvey.IS_RESPONSED_BY_HQ;
                    patientSurveyHistory.IS_REFERABLE = objPatientSurvey.IS_REFERABLE;
                    patientSurveyHistory.IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION;
                    //patientSurveyHistory.IS_QUESTION_ANSWERED = objPatientSurvey.IS_QUESTION_ANSWERED;
                    patientSurveyHistory.FEEDBACK = objPatientSurvey.FEEDBACK;
                    patientSurveyHistory.SURVEY_STATUS_BASE = "Completed";
                    patientSurveyHistory.SURVEY_STATUS_CHILD = "Completed Survey";
                    patientSurveyHistory.DELETED = false;
                    patientSurveyHistory.CREATED_BY = "FOX-TEAM";
                    patientSurveyHistory.CREATED_DATE = Helper.GetCurrentDate();
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
    }
}
