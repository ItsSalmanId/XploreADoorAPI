using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.QualityAssuranceService.PerformAuditService
{
   public class PerformAuditService : IPerformAuditService
    {
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly DBContextQualityAssurance _qualityAssuranceContext = new DBContextQualityAssurance();
        private readonly DbContextPatientSurvey _patientSurveyContext = new DbContextPatientSurvey();
        private readonly GenericRepository<EvaluationCriteriaCategories> _evaluationCriteriaCategoriesRepository;
        private readonly GenericRepository<WowFactor> _wowFactorRepository;
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        private readonly GenericRepository<PatientSurveyCallLog> _patientSurveyCallLogRepository;
        private readonly GenericRepository<SurveyAuditScores> _auditScoresRepository;
        private readonly GenericRepository<RoleToAdd> _roleRepository;
        private readonly GenericRepository<User> _userTableRepository;
        private readonly GenericRepository<GradingSetup> _gradingSetupRepository;
        public List<GradingSetup> GradingCriteria = new List<GradingSetup>();

        public PerformAuditService()
        {
            _evaluationCriteriaCategoriesRepository = new GenericRepository<EvaluationCriteriaCategories>(_qualityAssuranceContext);
            _wowFactorRepository = new GenericRepository<WowFactor>(_qualityAssuranceContext);
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_patientSurveyContext);
            _patientSurveyCallLogRepository = new GenericRepository<PatientSurveyCallLog>(_patientSurveyContext);
            _auditScoresRepository = new GenericRepository<SurveyAuditScores>(_qualityAssuranceContext);
            _roleRepository = new GenericRepository<RoleToAdd>(_QueueContext);
            _userTableRepository = new GenericRepository<User>(_qualityAssuranceContext);
            _gradingSetupRepository = new GenericRepository<GradingSetup>(_qualityAssuranceContext);
            GradingCriteria = new List<GradingSetup>();
        }
        public TotalNumbers GetTotalNumbersOfCriteria(long practiceCode, RequestModelForCallType obj) {

            TotalNumbers response = new TotalNumbers();

            _patientSurveyContext.PatientSurvey.Where(t => t.PRACTICE_CODE == practiceCode && !t.DELETED);
            response.EvaluationCriteriaCategories = _evaluationCriteriaCategoriesRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED && t.CALL_TYPE == obj.Call_Type).ToList();
            response.WowFactor = _wowFactorRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED && t.CALL_TYPE == obj.Call_Type).ToList();
           
            return response;
        }
        public List<FeedBackCaller> GetListOfReps(long practiceCode)
        {
            try
            {
                var List = new List<FeedBackCaller>(); ;
                var rep = _roleRepository.GetSingle(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.ROLE_NAME == "Feedback Caller").ROLE_ID;

                if (!rep.Equals(null))
                {
                    var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                    var _roleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.BigInt, Value = rep };

                    List = SpRepository<FeedBackCaller>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_FEEDBACK_CALLER_LIST
                         @PRACTICE_CODE, @ROLE_ID", PracticeCode, _roleId);
                }
              

                List<FeedBackCaller> response = new List<FeedBackCaller>();
                response = List;

                if (List.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<FeedBackCaller>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CallLogModel> PostCallList(RequestCallList request, UserProfile profile)
        {
            List<CallLogModel> lst = new List<CallLogModel>();
            request.DATE_TO = Helper.GetCurrentDate();
            switch (request.TIME_FRAME)
            {
                case 1:
                    request.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    request.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    request.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(request.DATE_FROM_STR))
                        request.DATE_FROM = Convert.ToDateTime(request.DATE_FROM_STR);
                    else
                        request.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(request.DATE_TO_STR))
                        request.DATE_TO = Convert.ToDateTime(request.DATE_TO_STR);
                    break;
                default:
                    break;
            }

            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
          //  var _surveyBy = new SqlParameter { ParameterName = "SURVEY_BY", SqlDbType = SqlDbType.VarChar, Value = request.SURVEY_BY };
            var _callBy = new SqlParameter { ParameterName = "CALL_BY", SqlDbType = SqlDbType.VarChar, Value = request.SURVEY_BY };
            var _callType= new SqlParameter { ParameterName = "CALL_TYPE", SqlDbType = SqlDbType.VarChar, Value = request.CALL_TYPE };
            var dateFrom = Helper.getDBNullOrValue("DATE_FROM", request.DATE_FROM.ToString());
            var dateTo = Helper.getDBNullOrValue("@DATE_TO", request.DATE_TO.ToString());
            //var result = SpRepository<CallLogModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_CALL_LIST
            //             @PRACTICE_CODE, @SURVEY_BY, @DATE_FROM, @DATE_TO", PracticeCode, _surveyBy, dateFrom, dateTo);

            var result = SpRepository<CallLogModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_CALL_LIST_FOR_AUDIT
                         @PRACTICE_CODE, @CALL_BY, @CALL_TYPE, @DATE_FROM, @DATE_TO", PracticeCode, _callBy, _callType, dateFrom, dateTo);
            lst = result;

           // var List = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.AUDITOR_NAME == profile.UserName).Select(x => x.SURVEY_CALL_ID).ToList();

            var List = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.AUDITOR_NAME == profile.UserName).Select(x => new SurveyAuditScores() { SURVEY_CALL_ID = x.SURVEY_CALL_ID, PHD_CALL_ID = x.PHD_CALL_ID }).ToList();

            if(lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    for (int j = 0; j < List.Count; j++)
                    {
                        if (lst[i].ID == List[j].PHD_CALL_ID || lst[i].ID == List[j].SURVEY_CALL_ID)
                        {
                            lst[i].IS_AUDITED = true;
                        }
                    }
                }
                return lst;
            }
            else
            {
                return new List<CallLogModel>();
            }
}
        public bool InsertAuditScores(SurveyAuditScores req, UserProfile profile )
        {
            GradingCriteria = GetListOfGradingCriteria(profile.PracticeCode ,req);
            var Obj = new List<SurveyAuditScores>();
            if ((req.SURVEY_CALL_ID != 0 && req.SURVEY_CALL_ID != null)) // in case of patient survey
            {
                 Obj = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.SURVEY_CALL_ID == req.SURVEY_CALL_ID && x.AUDITOR_NAME == profile.UserName);
                req.PHD_CALL_ID = null;
            }
            if ((req.PHD_CALL_ID != 0 && req.PHD_CALL_ID != null)) // in case of patient helpdesk
            {
                Obj = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.PHD_CALL_ID == req.PHD_CALL_ID && x.AUDITOR_NAME == profile.UserName);
                req.SURVEY_CALL_ID = null;
            }

            if (Obj.Count == 0)
            {
                req.SURVEY_AUDIT_SCORES_ID = Helper.getMaximumId("FOX_TBL_SURVEY_AUDIT_SCORES");
                req.PRACTICE_CODE = profile.PracticeCode;
                req.AUDITOR_NAME = profile.UserName;
                req.GRADE = getGrade(req.TOTAL_POINTS);
                req.CREATED_BY = profile.UserName;
                req.CREATED_DATE = Helper.GetCurrentDate();
                req.MODIFIED_BY = profile.UserName;
                req.MODIFIED_DATE = Helper.GetCurrentDate();
                req.DELETED = false;
                _auditScoresRepository.Insert(req);
                _auditScoresRepository.Save();
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public List<SurveyAuditScores> ListAuditedCalls(RequestCallFromQA req, UserProfile profile)
        {

            req.DATE_TO = Helper.GetCurrentDate();
            switch (req.TIME_FRAME)
            {
                case 1:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(req.DATE_FROM_STR))
                        req.DATE_FROM = Convert.ToDateTime(req.DATE_FROM_STR);
                    else
                        req.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(req.DATE_TO_STR))
                        req.DATE_TO = Convert.ToDateTime(req.DATE_TO_STR);
                    break;
                default:
                    break;
            }

            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _agentName = new SqlParameter { ParameterName = "AGENT_NAME", Value = req.AGENT_NAME };    
            SqlParameter _auditorName = new SqlParameter { ParameterName = "AUDITOR_NAME", Value = req.AUDITOR_NAME };
            SqlParameter _dateFrom = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.ToString());
            SqlParameter _dateTos = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.ToString());
            SqlParameter _calltype = Helper.getDBNullOrValue("CALL_TYPE", req.CALL_TYPE);


            var Result = SpRepository<SurveyAuditScores>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_AUDITED_CALL_LIST
                            @PRACTICE_CODE, @AGENT_NAME, @AUDITOR_NAME, @DATE_FROM, @DATE_TO ,@CALL_TYPE",
                             _practiceCode,  _agentName, _auditorName, _dateFrom, _dateTos,_calltype);
            return Result;
        }

        public string getGrade(decimal? result)
        {
            if (result >= GradingCriteria[0].OVERALL_MIN)
                return "A";
            if (result <= GradingCriteria[1].OVERALL_MAX && result >= GradingCriteria[1].OVERALL_MIN)
                return "B";
            if (result <= GradingCriteria[2].OVERALL_MAX)
                return "U";

            return "";
        }
        public List<GradingSetup> GetListOfGradingCriteria(long practiceCode, SurveyAuditScores req)
        {
            try
            {
                var response = _gradingSetupRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.CALL_TYPE == req.CALL_TYPE).ToList();

                if (response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<GradingSetup>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
