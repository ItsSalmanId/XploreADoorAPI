using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FOX.BusinessOperations.PatientSurveyService.SearchSurveyService
{
    public class SearchSurveyService : ISearchSurveyService
    {
        private readonly DbContextPatientSurvey _patientSurveyContext = new DbContextPatientSurvey();
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;

        public SearchSurveyService()
        {
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_patientSurveyContext);
        }
        
        public List<PatientSurvey> GetPatientSurvey(string patientAccount, bool isIncludeSurveyed, long practiceCode)
        {
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = patientAccount };
            var _isIncludeSurveyed = new SqlParameter { ParameterName = "IS_INCLUDE_SURVEYED", SqlDbType = SqlDbType.Bit, Value = isIncludeSurveyed };
            var PatientSurveyList = SpRepository<PatientSurvey>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY @PRACTICE_CODE, @PATIENT_ACCOUNT, @IS_INCLUDE_SURVEYED",
                PracticeCode, _patientAccount, _isIncludeSurveyed);
            return PatientSurveyList;
        }
        public PatientSurvey GetRandomSurvey(long practiceCode )
        {
            DateTime dateFrom, dateTo;
            dateFrom = Helper.GetCurrentDate().AddDays(-30);
            dateTo = Helper.GetCurrentDate();
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _dateFrom = Helper.getDBNullOrValue("DATE_FROM", dateFrom.ToString());
            var _dateTo = Helper.getDBNullOrValue("DATE_TO",dateTo.ToString());
            var PatientSurveyList = SpRepository<PatientSurvey>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_RANDOM_PATIENT_SURVEY @PRACTICE_CODE, @DATE_FROM, @DATE_TO", 
                PracticeCode, _dateFrom, _dateTo);
            if(PatientSurveyList != null && PatientSurveyList.PATIENT_ACCOUNT_NUMBER.ToString() != null && !string.IsNullOrEmpty(PatientSurveyList.PATIENT_ACCOUNT_NUMBER.ToString()))
            {
                CheckDeceasedPatient(PatientSurveyList.PATIENT_ACCOUNT_NUMBER.ToString(), practiceCode);
            }
            return PatientSurveyList;
        }
        public ResponseModel CheckDeceasedPatient(string patientAccount, long practiceCode)
        {
            ResponseModel responseModel = new ResponseModel();
            var result = _patientSurveyRepository.GetMany(x => x.PATIENT_ACCOUNT_NUMBER.ToString() == patientAccount && x.DELETED != true && x.PRACTICE_CODE == practiceCode);
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    if (item.SURVEY_STATUS_CHILD != null && item.SURVEY_STATUS_CHILD.ToLower().Contains("deceased"))
                    {
                        UpdatePatientSurvey(result);
                        responseModel.Message = "SUccessfully Updated";
                        responseModel.Success = true;
                        responseModel.ErrorMessage = "";
                    }
                    else
                    {
                        responseModel.Message = "Not Updated";
                        responseModel.Success = false;
                        responseModel.ErrorMessage = "";
                    }
                }
            }
            else
            {
                responseModel.Message = "Response is Empty";
                responseModel.Success = false;
                responseModel.ErrorMessage = "";
            }
            return responseModel;
        }
        public void UpdatePatientSurvey(List<PatientSurvey> patientSurveys)
        {
            if (patientSurveys != null)
            {
                foreach (var item in patientSurveys)
                {
                    item.DELETED = true;
                    item.MODIFIED_DATE = DateTime.Now;
                    item.FEEDBACK = "Patient marked as deceased as of " + DateTime.Now;
                    _patientSurveyRepository.Update(item);
                    _patientSurveyRepository.Save();
                }
            }
        }
    }
}