using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.FoxPHD;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.QualityAssuranceService.PerformAuditService
{
    public class PerformAuditService : IPerformAuditService
    {
        private readonly DbContextPatient _PatientContext = new DbContextPatient();
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
        private readonly GenericRepository<PhdCallScenario> _PhdCallScenarioRepository;
        private readonly DBContextFoxPHD _DBContextFoxPHD = new DBContextFoxPHD();
        private readonly GenericRepository<Patient> _PatientRepository;
        public List<GradingSetup> GradingCriteria = new List<GradingSetup>();
        private readonly GenericRepository<PHDCallDetail> _PHDDetailRepository;

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
            _PhdCallScenarioRepository = new GenericRepository<PhdCallScenario>(_DBContextFoxPHD);
            _PatientRepository = new GenericRepository<Patient>(_PatientContext);
            GradingCriteria = new List<GradingSetup>();
            _PHDDetailRepository = new GenericRepository<PHDCallDetail>(_DBContextFoxPHD);

        }
        public TotalNumbers GetTotalNumbersOfCriteria(long practiceCode, RequestModelForCallType obj)
        {

            TotalNumbers response = new TotalNumbers();
            _patientSurveyContext.PatientSurvey.Where(t => t.PRACTICE_CODE == practiceCode && !t.DELETED);
            response.EvaluationCriteriaCategories = _evaluationCriteriaCategoriesRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED && t.CALL_TYPE == obj.Call_Type).ToList();
            response.WowFactor = _wowFactorRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED && t.CALL_TYPE == obj.Call_Type).ToList();
            response.PhdCallScenarios = _PhdCallScenarioRepository.GetMany(s => s.PRACTICE_CODE == practiceCode && s.DELETED == false).OrderBy(o => o.NAME).ToList();

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
            List<SurveyAuditScores> List = new List<SurveyAuditScores>();
            List<SurveyAuditScores> noAssociatedList = new List<SurveyAuditScores>();
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
            var _callType = new SqlParameter { ParameterName = "CALL_TYPE", SqlDbType = SqlDbType.VarChar, Value = request.CALL_TYPE };
            var dateFrom = Helper.getDBNullOrValue("DATE_FROM", request.DATE_FROM.ToString());
            var dateTo = Helper.getDBNullOrValue("@DATE_TO", request.DATE_TO.ToString());
            var callScanario = new SqlParameter { ParameterName = "PHD_CALL_SCENARIO_ID", SqlDbType = SqlDbType.VarChar, Value = request.PHD_CALL_SCENARIO_ID };
            //var result = SpRepository<CallLogModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_CALL_LIST
            //             @PRACTICE_CODE, @SURVEY_BY, @DATE_FROM, @DATE_TO", PracticeCode, _surveyBy, dateFrom, dateTo);

            var result = SpRepository<CallLogModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_CALL_LIST_FOR_AUDIT
                         @PRACTICE_CODE, @CALL_BY, @CALL_TYPE, @DATE_FROM, @DATE_TO, @PHD_CALL_SCENARIO_ID", PracticeCode, _callBy, _callType, dateFrom, dateTo, callScanario);
            lst = result;

            // var List = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.AUDITOR_NAME == profile.UserName).Select(x => x.SURVEY_CALL_ID).ToList();


            if (request.IS_READ_ONLY_MODE)
            {
                noAssociatedList = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode /* && x.AUDITOR_NAME == profile.UserName*/ &&
               x.PHD_CALL_ID.ToString().EndsWith("0000") &&/* x.PATIENT_ACCOUNT != null &&*/
               (request.PHD_CALL_SCENARIO_ID != 0 ? (x.PHD_CALL_SCENARIO_ID == request.PHD_CALL_SCENARIO_ID) : true));

                List = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode /* && x.AUDITOR_NAME == profile.UserName*/).Select(x => new SurveyAuditScores() { SURVEY_CALL_ID = x.SURVEY_CALL_ID, PHD_CALL_ID = x.PHD_CALL_ID }).ToList();
            }
            else
            {
                noAssociatedList = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.AUDITOR_NAME == profile.UserName &&
               x.PHD_CALL_ID.ToString().EndsWith("0000") &&/* x.PATIENT_ACCOUNT != null &&*/
               (request.PHD_CALL_SCENARIO_ID != 0 ? (x.PHD_CALL_SCENARIO_ID == request.PHD_CALL_SCENARIO_ID) : true));

                List = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode /* && x.AUDITOR_NAME == profile.UserName*/).Select(x => new SurveyAuditScores() { SURVEY_CALL_ID = x.SURVEY_CALL_ID, PHD_CALL_ID = x.PHD_CALL_ID }).ToList();
            }

            if (lst.Count > 0)
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
                if(request.IS_READ_ONLY_MODE && noAssociatedList.Count >0)
                {
                    foreach (SurveyAuditScores list in noAssociatedList)
                    {
                        if(list.AGENT_NAME == request.SURVEY_BY)
                        {
                            CallLogModel notAssociatedCall = new CallLogModel();
                            //notAssociatedCall.ID = list.PHD_CALL_ID.GetValueOrDefault() + list.SURVEY_AUDIT_SCORES_ID;
                            notAssociatedCall.ID = list.PHD_CALL_ID.GetValueOrDefault();
                            notAssociatedCall.CREATED_BY = list.AGENT_NAME;
                            notAssociatedCall.CREATED_DATE = list.CREATED_DATE;
                            notAssociatedCall.LOGS = "Not Associated Call | Patient Helpdesk";
                            notAssociatedCall.IS_AUDITED = true;
                            var patient = _PatientRepository.GetFirst(x => x.Patient_Account == list.PATIENT_ACCOUNT && x.Practice_Code == profile.PracticeCode && (x.DELETED ?? false) == false);
                            if (patient != null)
                            {
                                notAssociatedCall.MRN = patient.Chart_Id == null ? "" : patient.Chart_Id;
                                notAssociatedCall.FIRST_NAME = patient.First_Name == null ? "" : patient.First_Name;
                                notAssociatedCall.LAST_NAME = patient.Last_Name == null ? "" : patient.Last_Name;
                            }
                            lst.Add(notAssociatedCall);

                        }
                    }

                }
                return lst;
            }
            if(noAssociatedList.Count > 0 && lst.Count == 0 && request.IS_READ_ONLY_MODE)
            {
                foreach (SurveyAuditScores list in noAssociatedList)
                {
                    if (list.AGENT_NAME == request.SURVEY_BY )
                    {
                        CallLogModel notAssociatedCall = new CallLogModel();
                        notAssociatedCall.ID = list.PHD_CALL_ID.GetValueOrDefault();
                        notAssociatedCall.CREATED_BY = list.AGENT_NAME;
                        notAssociatedCall.CREATED_DATE = list.CREATED_DATE;
                        notAssociatedCall.LOGS = "Not Associated Call | Patient Helpdesk";
                        var patient = _PatientRepository.GetFirst(x => x.Patient_Account == list.PATIENT_ACCOUNT && x.Practice_Code == profile.PracticeCode && (x.DELETED ?? false) == false);
                        if (patient != null)
                        {
                            notAssociatedCall.MRN = patient.Chart_Id == null ? "" : patient.Chart_Id;
                            notAssociatedCall.FIRST_NAME = patient.First_Name == null ? "" : patient.First_Name;
                            notAssociatedCall.LAST_NAME = patient.Last_Name == null ? "" : patient.Last_Name;
                        }
                        lst.Add(notAssociatedCall);
                    }
                }
                return lst;
            }
            else
            {
                return new List<CallLogModel>();
            }
        }
        public bool InsertAuditScores(SurveyAuditScores req, UserProfile profile)
        {
            GradingCriteria = GetListOfGradingCriteria(profile.PracticeCode, req);
            var Obj = new List<SurveyAuditScores>();
            SurveyAuditScores existingScores = new SurveyAuditScores();
            string createdBy = "";
            DateTime? createdDate = new DateTime();
            long survey_score_id = 0;
            long? patientAccount = 0;
            
            if ((req.SURVEY_CALL_ID != 0 && req.SURVEY_CALL_ID != null)) // in case of patient survey
            {
                Obj = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.SURVEY_CALL_ID == req.SURVEY_CALL_ID /* && x.AUDITOR_NAME == profile.UserName*/);
                existingScores = _auditScoresRepository.GetFirst(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.SURVEY_CALL_ID == req.SURVEY_CALL_ID /* && x.AUDITOR_NAME == profile.UserName*/);
                req.SCORING_CRITERIA = "new";
                req.CALL_TYPE = "survey";
                req.PHD_CALL_ID = null;
            }
            if ((req.PHD_CALL_ID != 0 && req.PHD_CALL_ID != null)) // in case of patient helpdesk
            {
                Obj = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.PHD_CALL_ID == req.PHD_CALL_ID /* && x.AUDITOR_NAME == profile.UserName*/);
                existingScores = _auditScoresRepository.GetFirst(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.PHD_CALL_ID == req.PHD_CALL_ID /* && x.AUDITOR_NAME == profile.UserName*/);
                req.CALL_TYPE = "phd";
                req.SURVEY_CALL_ID = null;
            }
            if (req.EDIT_AUDIT_REPORT)
            {
                survey_score_id = existingScores.SURVEY_AUDIT_SCORES_ID;
                createdBy = existingScores.CREATED_BY;
                createdDate = existingScores.CREATED_DATE;
                patientAccount = existingScores.PATIENT_ACCOUNT;
            }
            if (existingScores != null && req.EDIT_AUDIT_REPORT)
            {
                var parentProperties = req.GetType().GetProperties();
                var childProperties = existingScores.GetType().GetProperties();
                foreach (var parentProperty in parentProperties)
                {
                    foreach (var childProperty in childProperties)
                    {
                        if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                        {
                            childProperty.SetValue(existingScores, parentProperty.GetValue(req));
                            break;
                        }
                    }
                }
                existingScores.SURVEY_AUDIT_SCORES_ID = survey_score_id;
                existingScores.PRACTICE_CODE = profile.PracticeCode;
                existingScores.AUDITOR_NAME = profile.UserName;
                existingScores.GRADE = getGrade(req.TOTAL_POINTS);
                if (req.CALL_TYPE == "survey")
                {
                    existingScores.SCORING_CRITERIA = "new";
                }
                existingScores.CREATED_BY = createdBy;
                existingScores.CREATED_DATE = createdDate;
                existingScores.MODIFIED_BY = profile.UserName;
                existingScores.MODIFIED_DATE = Helper.GetCurrentDate();
                existingScores.DELETED = false;
                if (patientAccount != null)
                {
                    existingScores.PATIENT_ACCOUNT_STR = patientAccount.ToString();
                    if (existingScores.PATIENT_ACCOUNT_STR == "")
                    {
                        existingScores.PATIENT_ACCOUNT = null;
                    }
                    else
                    {
                        long account;
                        bool success = long.TryParse(existingScores.PATIENT_ACCOUNT_STR, out account);
                        if (success)
                        {
                            existingScores.PATIENT_ACCOUNT = account;
                        }

                    }
                }
                if (existingScores.PHD_CALL_ID == 0)
                {
                    var date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    date = date + "0000";
                    existingScores.PHD_CALL_ID = long.Parse(date);
                }
                Obj = null;
                _auditScoresRepository.Update(existingScores);
                _auditScoresRepository.Save();
                //Sending Email to Auditor in PHD CASE
                SendEmailForAudit(existingScores, profile);
                return true;
                //}


            }
            if (Obj.Count == 0 && req.EDIT_AUDIT_REPORT == false)
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
                if (req.PATIENT_ACCOUNT_STR == "")
                {
                    req.PATIENT_ACCOUNT = null;
                }
                else
                {
                    req.PATIENT_ACCOUNT = long.Parse(req.PATIENT_ACCOUNT_STR);
                }
                if (req.PHD_CALL_ID == 0)
                {
                    var date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    date =date + "0000";
                    req.PHD_CALL_ID = long.Parse(date);
                }
                _auditScoresRepository.Insert(req);
                _auditScoresRepository.Save();
                //Sending Email to Auditor in PHD CASE
                SendEmailForAudit(req, profile);
                return true;
            }
            else
            {
                return false;
            }


        }
        public void SendEmailForAudit(SurveyAuditScores req, UserProfile profile)
        {
            string _body = string.Empty;
            List<string> cc = new List<string>();

            _body = "<style>  body, table, td {font-family:'Calibri'!important;} table { border-collapse:separate; }@media screen and(max-width:740px) { table { width: 100 % !important; text-align:center!important;} } body {font-size:14px!important;}  table th { font-weight: normal; border-right: 1px solid #fff;text-align: center;font-weight: bold;line-height: normal;}table td, th{ padding: 3px 7px; color: #555555;font-size: 16px; height: 24px; font-weight: normal;}a{ text-decoration: none; }.first-section th{background: #f2f2f2;}.first-section {background: #f2f2f2;}.second-section {background: #e1f4ff;}.third-section {background: #fff2cc;}.fourth-section {background:#DAA520;}.totalscor{font-size:16px!important;color:#000!important}</style> ";
            string _subject = string.Empty;
            string sendTo = string.Empty;
            DateTime? callDate = new DateTime();
            if (req.CALL_TYPE.ToLower() == "survey")
            {
                var temp = _patientSurveyCallLogRepository.GetFirst(x => x.SURVEY_CALL_ID == req.SURVEY_CALL_ID);
                if (temp != null)
                {
                    callDate = temp.CREATED_DATE;
                }
            }
            else
            {
                var temp = _PHDDetailRepository.GetFirst(x => x.FOX_PHD_CALL_DETAILS_ID == req.PHD_CALL_ID);
                if (temp != null)
                {
                    callDate = temp.CREATED_DATE;
                }



                //callDate = req.CREATED_DATE;
                req.AUDITOR_NAME = profile.FirstName + ' ' + profile.LastName;
                req.AGENT_EMAIL = req.AGENT_EMAIL;

                if (req.EDIT_AUDIT_REPORT && AppConfiguration.ClientURL.Contains("https://fox.mtbc.com/") && profile.PracticeCode == 1012714)
                {
                    cc = new List<string>(ConfigurationManager.AppSettings["CClistForEditAuditEmail"].Split(new char[] { ';' }));
                }
                else
                {
                    cc = new List<string>(ConfigurationManager.AppSettings["CClistForEditAuditEmailTest"].Split(new char[] { ';' }));
                }
                _body += "<div style='font-family:Calibri'>A helpdesk record has been audited with following specifics:<br/><br/>";
                var link = AppConfiguration.ClientURL + @"#/PlayRecording?value=" + req.CALL_RECORDING_URL;
                link += "&name=" + profile.UserEmailAddress;
                _body += "<b>Date of Call: " + callDate.Value.ToString("MM/dd/yyyy") + "<a href = " + link + ">" + " Click here to listen audio call</a></b>" + "</br>";
                _body += "<b>Auditor: </b> " + req.AUDITOR_NAME + "</br>";
                _body += "<b>Audited on: </b> " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "</br>";
                if (req.MRN != null && req.CALL_TYPE == "survey")
                {
                    _body += "<b>MRN: </b> " + req.MRN + "</br></br>";
                }
                if(req.MRN != null && req.CALL_TYPE == "phd")
                {
                    _body += "<b>MRN: </b> " + req.MRN + "</br>";
                }
                if (req.CALL_SCANARIO != null)
                {
                    _body += "<b>Call handling: </b> " + req.CALL_SCANARIO + "</br></br>";
                }
                _body += "<b>Evaluation details: </b></br></br></br></div>";
                _body += req.HTML_TEMPLETE;
                _subject = req.CALL_TYPE.ToUpper() + " audit summary-" + (string.IsNullOrEmpty(req.AUDITOR_NAME) ? "" : req.AUDITOR_NAME + ".") + (string.IsNullOrEmpty(req.CALL_SCANARIO) ? "" : req.CALL_SCANARIO);
                Helper.Email(req.AGENT_EMAIL, _subject, _body, profile, null, null, cc, null);
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

            SqlParameter _patientAcount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = req.PATIENT_ACCOUNT ?? 0 };
            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _agentName = new SqlParameter { ParameterName = "AGENT_NAME", Value = req.AGENT_NAME };
            SqlParameter _auditorName = new SqlParameter { ParameterName = "AUDITOR_NAME", Value = req.AUDITOR_NAME };
            SqlParameter _dateFrom = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.ToString());
            SqlParameter _dateTos = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.ToString());
            SqlParameter _calltype = Helper.getDBNullOrValue("CALL_TYPE", req.CALL_TYPE);


            var Result = SpRepository<SurveyAuditScores>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_AUDITED_CALL_LIST
                             @PATIENT_ACCOUNT, @PRACTICE_CODE, @AGENT_NAME, @AUDITOR_NAME, @DATE_FROM, @DATE_TO ,@CALL_TYPE",
                             _patientAcount, _practiceCode,_agentName, _auditorName, _dateFrom, _dateTos, _calltype);
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
