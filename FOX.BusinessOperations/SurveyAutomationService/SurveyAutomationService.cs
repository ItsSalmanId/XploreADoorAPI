using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Patient;
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

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public class SurveyAutomationService : ISurveyAutomationService
    {
        private readonly DBContextSurveyAutomation _surveyAutomationContext = new DBContextSurveyAutomation();
        private readonly GenericRepository<Patient> _patientRepository;
        private readonly GenericRepository<SurveyQuestions> _surveyQuestions;
        public SurveyAutomationService()
        {
            _patientRepository = new GenericRepository<Patient>(_surveyAutomationContext);
            _surveyQuestions = new GenericRepository<SurveyQuestions>(_surveyAutomationContext);
        }

        public List<FoxRoles> GetFoxRoles(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        public List<SurveyAutomation> GetPatientDetails(SurveyAutomation objSurveyAutomation)
        {
            //string decryptedPatientAccount = Decrypt(objSurveyAutomation.Patient_AccountStr, "sblw-3hn8-sqoy19");
            List<SurveyAutomation> surveyAutomation = new List<SurveyAutomation>();
            SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = objSurveyAutomation.Patient_AccountStr };
            var patientDetails = SpRepository<SurveyAutomation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT", patientAccount);
            return patientDetails;
        }
        public List<SurveyQuestions> GetSurveyQuestionDetails()
        {
            List<SurveyQuestions> surveyQuestionsList = new List<SurveyQuestions>();
            surveyQuestionsList = _surveyQuestions.GetMany(s =>  s.DELETED == false).ToList();
            return surveyQuestionsList;
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
    }
}
