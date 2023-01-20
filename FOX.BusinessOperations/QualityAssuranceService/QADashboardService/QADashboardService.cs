using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
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

namespace FOX.BusinessOperations.QualityAssuranceService.QADashboardService
{
    public class QADashboardService : IQADashboardService
    {
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<RoleToAdd> _roleRepository;

        public QADashboardService()
        {
            _roleRepository = new GenericRepository<RoleToAdd>(_QueueContext);
        }
        public DashBoardMainModel GetDashboardData(QADashboardSearch qADashboardSearch, UserProfile profile)
        {
            try
            {
                DashBoardMainModel dashBoardMainModel = new DashBoardMainModel();
                dashBoardMainModel.PieChartData = new QADashboardData();
                if (qADashboardSearch != null)
                {
                    if(!string.IsNullOrEmpty(qADashboardSearch.TIME_FRAME))
                    {
                        if (qADashboardSearch.TIME_FRAME.ToLower() == "last_week")
                        {
                            DayOfWeek weekStart = DayOfWeek.Monday;
                            DateTime startingDate = DateTime.Today;
                            while (startingDate.DayOfWeek != weekStart)
                                startingDate = startingDate.AddDays(-1);
                            qADashboardSearch.START_DATE = startingDate.AddDays(-7);
                            qADashboardSearch.END_DATE = startingDate.AddDays(-2);
                        }
                        else if (qADashboardSearch.TIME_FRAME.ToLower() == "last_two_week")
                        {
                            DayOfWeek weekStart = DayOfWeek.Monday;
                            DateTime startingDate = DateTime.Today;
                            while (startingDate.DayOfWeek != weekStart)
                                startingDate = startingDate.AddDays(-1);
                            qADashboardSearch.START_DATE = startingDate.AddDays(-14);
                            qADashboardSearch.END_DATE = startingDate.AddDays(-2);
                        }
                        else if (qADashboardSearch.TIME_FRAME.ToLower() == "last_three_week")
                        {
                            DayOfWeek weekStart = DayOfWeek.Monday;
                            DateTime startingDate = DateTime.Today;
                            while (startingDate.DayOfWeek != weekStart)
                                startingDate = startingDate.AddDays(-1);
                            qADashboardSearch.START_DATE = startingDate.AddDays(-21);
                            qADashboardSearch.END_DATE = startingDate.AddDays(-2);
                        }
                        else if (qADashboardSearch.TIME_FRAME.ToLower() == "last_month")
                        {
                            DayOfWeek weekStart = DayOfWeek.Monday;
                            DateTime startingDate = DateTime.Today;
                            while (startingDate.DayOfWeek != weekStart)
                                startingDate = startingDate.AddDays(-1);
                            qADashboardSearch.START_DATE = startingDate.AddDays(-28);
                            qADashboardSearch.END_DATE = startingDate.AddDays(-2);
                        }
                        else if (qADashboardSearch.TIME_FRAME.ToLower() == "last_two_months")
                        {
                            DayOfWeek weekStart = DayOfWeek.Monday;
                            DateTime startingDate = DateTime.Today;
                            while (startingDate.DayOfWeek != weekStart)
                                startingDate = startingDate.AddDays(-1);
                            qADashboardSearch.START_DATE = startingDate.AddDays(-56);
                            qADashboardSearch.END_DATE = startingDate.AddDays(-2);
                        }
                        else if (qADashboardSearch.TIME_FRAME.ToLower() == "last_three_months")
                        {
                            DayOfWeek weekStart = DayOfWeek.Monday;
                            DateTime startingDate = DateTime.Today;
                            while (startingDate.DayOfWeek != weekStart)
                                startingDate = startingDate.AddDays(-1);
                            qADashboardSearch.START_DATE = startingDate.AddDays(-84);
                            qADashboardSearch.END_DATE = startingDate.AddDays(-2);
                        }
                    }
                    if (!string.IsNullOrEmpty(qADashboardSearch.EVALUATION_NAME))
                    {
                        if (qADashboardSearch.EVALUATION_NAME.Contains("Client Experience") && qADashboardSearch.EVALUATION_NAME.Contains("System Product and Process") || qADashboardSearch.EVALUATION_NAME.Contains("Call Quality") && qADashboardSearch.EVALUATION_NAME.Contains("System Usage"))
                        {
                            qADashboardSearch.EVALUATION_NAME = "System and Client";
                        }
                        else if (qADashboardSearch.EVALUATION_NAME.Contains("Call Quality"))
                        {
                            qADashboardSearch.EVALUATION_NAME = "Client Experience";
                        }
                        else if (qADashboardSearch.EVALUATION_NAME.Contains("System Usage"))
                        {
                            qADashboardSearch.EVALUATION_NAME = "System Product and Process";
                        }
                    }
                    if (qADashboardSearch != null)
                    {
                        if (qADashboardSearch.EVALUATION_NAME != null && qADashboardSearch.EVALUATION_NAME.StartsWith(","))
                        {
                            qADashboardSearch.EVALUATION_NAME = qADashboardSearch.EVALUATION_NAME.Remove(0, 1);
                        }
                        if (qADashboardSearch.EMPLOYEE_USER_NAME != null && qADashboardSearch.EMPLOYEE_USER_NAME.StartsWith(","))
                        {
                            qADashboardSearch.EMPLOYEE_USER_NAME = qADashboardSearch.EMPLOYEE_USER_NAME.Remove(0, 1);
                        }
                        if (qADashboardSearch.CALL_HANDLING_ID != null && qADashboardSearch.CALL_HANDLING_ID.StartsWith(","))
                        {
                            qADashboardSearch.CALL_HANDLING_ID = qADashboardSearch.CALL_HANDLING_ID.Remove(0, 1);
                        }
                        if (qADashboardSearch.IS_ACTIVE && qADashboardSearch.EMPLOYEE_USER_NAME == null)
                        {
                            qADashboardSearch.EMPLOYEE_USER_NAME = "";
                        }
                        var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                        var callHandlingId = new SqlParameter { ParameterName = "CALL_SCANRIO_ID", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.CALL_HANDLING_ID };
                        var callType = new SqlParameter { ParameterName = "CALL_TYPE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.CALL_TYPE ?? (object)DBNull.Value };
                        var empUserName = new SqlParameter { ParameterName = "AGENT_NAME", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.EMPLOYEE_USER_NAME ?? (object)DBNull.Value };
                        var criteriaName = new SqlParameter { ParameterName = "CRITERIA_NAME", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.EVALUATION_NAME ?? (object)DBNull.Value };
                        var startDate = new SqlParameter { ParameterName = "START_DATE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.START_DATE };
                        var endDate = new SqlParameter { ParameterName = "END_DATE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.END_DATE };
                        dashBoardMainModel.PieChartData = SpRepository<QADashboardData>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_QA_DASHBOARD_DATA @PRACTICE_CODE,@CALL_SCANRIO_ID,@CALL_TYPE,@AGENT_NAME,@CRITERIA_NAME,@START_DATE,@END_DATE", practiceCode, callHandlingId, callType, empUserName, criteriaName, startDate, endDate);

                        if (!qADashboardSearch.IS_ACTIVE && qADashboardSearch.CALL_TYPE == "Phd and Survey")
                        {
                            qADashboardSearch.CALL_TYPE = "Phd";
                            dashBoardMainModel.LineChartData = GetDashboardLineData(qADashboardSearch, profile);
                            qADashboardSearch.CALL_TYPE = "Survey";
                            var surveyData = GetDashboardLineData(qADashboardSearch, profile);
                            dashBoardMainModel.LineChartData.series.Add(surveyData.series[0]);
                        }
                        else if (qADashboardSearch.EVALUATION_NAME == "System and Client")
                        {
                            qADashboardSearch.EVALUATION_NAME = "Client Experience";
                            dashBoardMainModel.LineChartData = GetDashboardLineData(qADashboardSearch, profile);
                            qADashboardSearch.EVALUATION_NAME = "System Product and Process";
                            var surveyData = GetDashboardLineData(qADashboardSearch, profile);
                            dashBoardMainModel.LineChartData.series.Add(surveyData.series[0]);
                        }
                        else
                        {
                            dashBoardMainModel.LineChartData = GetDashboardLineData(qADashboardSearch, profile);
                        }
                    }
                }
                return dashBoardMainModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LineGraphResponseModel GetDashboardLineData(QADashboardSearch qADashboardSearch, UserProfile profile)
        {
            LineGraphResponseModel lineGraphResponseModel = new LineGraphResponseModel();
            lineGraphResponseModel.lineGraphData = new List<LineGraphData>();

            var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var callHandlingId = new SqlParameter { ParameterName = "CALL_SCANRIO_ID", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.CALL_HANDLING_ID };
            var callType = new SqlParameter { ParameterName = "CALL_TYPE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.CALL_TYPE ?? (object)DBNull.Value };
            var empUserName = new SqlParameter { ParameterName = "AGENT_NAME", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.EMPLOYEE_USER_NAME ?? (object)DBNull.Value };
            var criteriaName = new SqlParameter { ParameterName = "CRITERIA_NAME", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.EVALUATION_NAME ?? (object)DBNull.Value };
            var startDate = new SqlParameter { ParameterName = "START_DATE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.START_DATE };
            var endDate = new SqlParameter { ParameterName = "END_DATE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.END_DATE };
            lineGraphResponseModel.lineGraphData = SpRepository<LineGraphData>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_QA_DASHBOARD_LINE_GRAPH_DATA @PRACTICE_CODE,@CALL_SCANRIO_ID,@CALL_TYPE,@AGENT_NAME,@CRITERIA_NAME,@START_DATE,@END_DATE", practiceCode, callHandlingId, callType, empUserName, criteriaName, startDate, endDate);
            if (!string.IsNullOrEmpty(qADashboardSearch.CALL_TYPE) && qADashboardSearch.CALL_TYPE == "Survey")
            {
                int i = 0;
                foreach (var item in lineGraphResponseModel.lineGraphData)
                {
                    lineGraphResponseModel.lineGraphData[i].TEAM_NAME = "Survey Calls";
                    i++;
                }
                qADashboardSearch.TEAMS_NAMES = "Survey Calls";
            }
            if (qADashboardSearch.USER_FULL_NAME != null && qADashboardSearch.USER_FULL_NAME.StartsWith(","))
            {
                qADashboardSearch.USER_FULL_NAME = qADashboardSearch.USER_FULL_NAME.Remove(0, 1);
            }
            if (qADashboardSearch.TEAMS_NAMES != null && qADashboardSearch.TEAMS_NAMES.StartsWith(","))
            {
                qADashboardSearch.TEAMS_NAMES = qADashboardSearch.TEAMS_NAMES.Remove(0, 1);
            }
            if (!qADashboardSearch.IS_ACTIVE && !string.IsNullOrEmpty(qADashboardSearch.TEAMS_NAMES) && lineGraphResponseModel.lineGraphData != null && lineGraphResponseModel.lineGraphData.Count > 0)
            {
                if (!string.IsNullOrEmpty(qADashboardSearch.CALL_TYPE) && qADashboardSearch.CALL_TYPE == "Phd")
                {
                    qADashboardSearch.TEAMS_NAMES = qADashboardSearch.TEAMS_NAMES.Replace(",Survey Calls", "");
                    qADashboardSearch.TEAMS_NAMES = qADashboardSearch.TEAMS_NAMES.Replace("Survey Calls", "");
                    if (qADashboardSearch.TEAMS_NAMES != null && qADashboardSearch.TEAMS_NAMES.StartsWith(","))
                    {
                        qADashboardSearch.TEAMS_NAMES = qADashboardSearch.TEAMS_NAMES.Remove(0, 1);
                    }
                }
                var teamName = qADashboardSearch.TEAMS_NAMES.Split(',');
                lineGraphResponseModel.series = new List<series>();
                lineGraphResponseModel.dateRanges = lineGraphResponseModel.lineGraphData.FindAll(x => !string.IsNullOrEmpty(x.DATE_RANGE)).Select(d => d.DATE_RANGE).Distinct().ToList();
                foreach (var team in teamName.Select((value, i) => new { i, value }))
                {
                    series lineGraphSeriesObj = new series();
                    lineGraphSeriesObj.name = team.value;
                    lineGraphSeriesObj.type = "line";
                    lineGraphSeriesObj.data = new List<long>();
                    foreach (var range in lineGraphResponseModel.dateRanges)
                    {
                        var avgValue = lineGraphResponseModel.lineGraphData.Find(a => a.DATE_RANGE == range && a.TEAM_NAME == team.value);
                        lineGraphSeriesObj.data.Add(avgValue == null ? 0 : avgValue.EVALUATION_PERCENTAGE);
                    }
                    lineGraphResponseModel.series.Add(lineGraphSeriesObj);
                }
                if (qADashboardSearch.EVALUATION_NAME != null && qADashboardSearch.CALL_TYPE != null)
                {
                    if (qADashboardSearch.EVALUATION_NAME == "Client Experience" && qADashboardSearch.CALL_TYPE == "phd" || qADashboardSearch.EVALUATION_NAME == "System Product and Process" && qADashboardSearch.CALL_TYPE == "phd")
                    {
                        series singleObj = new series();
                        singleObj.name = qADashboardSearch.EVALUATION_NAME;
                        singleObj.type = "line";
                        singleObj.data = new List<long>();
                        singleObj = CalculateAverage(lineGraphResponseModel, teamName, singleObj);
                        lineGraphResponseModel.series = new List<series>();
                        lineGraphResponseModel.series.Add(singleObj);
                    }
                    if (qADashboardSearch.EVALUATION_NAME == "Client Experience" && qADashboardSearch.CALL_TYPE == "Survey")
                    {
                        series singleObj = new series();
                        singleObj.name = "Call Quality";
                        singleObj.type = "line";
                        singleObj.data = new List<long>();
                        singleObj = CalculateAverage(lineGraphResponseModel, teamName, singleObj);
                        lineGraphResponseModel.series = new List<series>();
                        lineGraphResponseModel.series.Add(singleObj);
                    }
                    if (qADashboardSearch.EVALUATION_NAME == "System Product and Process" && qADashboardSearch.CALL_TYPE == "Survey")
                    {
                        series singleObj = new series();
                        singleObj.name = "System Usage";
                        singleObj.type = "line";
                        singleObj.data = new List<long>();
                        singleObj = CalculateAverage(lineGraphResponseModel, teamName, singleObj);
                        lineGraphResponseModel.series = new List<series>();
                        lineGraphResponseModel.series.Add(singleObj);
                    }
                }
            }
            if (qADashboardSearch.IS_ACTIVE && !string.IsNullOrEmpty(qADashboardSearch.USER_FULL_NAME) && lineGraphResponseModel.lineGraphData != null && lineGraphResponseModel.lineGraphData.Count > 0)
            {
                var agentName = qADashboardSearch.USER_FULL_NAME.Split(',');
                lineGraphResponseModel.series = new List<series>();
                lineGraphResponseModel.dateRanges = lineGraphResponseModel.lineGraphData.FindAll(x => !string.IsNullOrEmpty(x.DATE_RANGE)).Select(d => d.DATE_RANGE).Distinct().ToList();
                foreach (var name in agentName.Select((value, i) => new { i, value }))
                {
                    series obj = new series();
                    obj.name = name.value;
                    obj.type = "line";
                    obj.data = new List<long>();
                    foreach (var range in lineGraphResponseModel.dateRanges)
                    {
                        var avgValue = lineGraphResponseModel.lineGraphData.Find(a => a.DATE_RANGE == range && a.AGENT_NAME == name.value);
                        obj.data.Add(avgValue == null ? 0 : avgValue.EVALUATION_PERCENTAGE);
                    }
                    lineGraphResponseModel.series.Add(obj);
                }
            }
            return lineGraphResponseModel;
        }
        public series CalculateAverage(LineGraphResponseModel lineGraphResponseModel, string[] teamName, series singleObj)
        {
            for (int i = 0; i < lineGraphResponseModel.dateRanges.Count; i++)
            {
                var sumOfArrays = lineGraphResponseModel.series.FindAll(x => x.data != null).Sum(s => s.data[i]);
                sumOfArrays = sumOfArrays / teamName.Length;
                singleObj.data.Add(sumOfArrays);
            }
            return singleObj;
        }
        public List<FeedBackCaller> GetEmployeelist(string callScanrioID, UserProfile profile)
        {
            try
            {
                List<FeedBackCaller> feedBackCallerList = new List<FeedBackCaller>();
                if (!string.IsNullOrEmpty(callScanrioID))
                {
                    List<FeedBackCaller> surveyAgentList = new List<FeedBackCaller>();
                    if (profile != null)
                    {
                        if (callScanrioID.StartsWith(","))
                        {
                            callScanrioID = callScanrioID.Remove(0, 1);
                        }
                        var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                        var callID = new SqlParameter { ParameterName = "CALL_SCANRIO_ID", SqlDbType = SqlDbType.VarChar, Value = callScanrioID };
                        feedBackCallerList = SpRepository<FeedBackCaller>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TEAM_EMPLOYEE_LIST  @PRACTICE_CODE, @CALL_SCANRIO_ID", practiceCode, callID);
                    }
                    if (callScanrioID.Contains("54410118"))
                    {
                        var feedbackCallerRole = _roleRepository.GetFirst(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.ROLE_NAME == "Feedback Caller");
                        var repRoleID = feedbackCallerRole != null ? feedbackCallerRole.ROLE_ID : 0;
                        var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                        var roleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.BigInt, Value = repRoleID };
                        surveyAgentList = SpRepository<FeedBackCaller>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_FEEDBACK_CALLER_LIST @PRACTICE_CODE, @ROLE_ID", practiceCode, roleId);
                    }
                    feedBackCallerList.AddRange(surveyAgentList);
                    feedBackCallerList = feedBackCallerList.GroupBy(item => item.NAME).Select(grp => grp.OrderBy(item => item.NAME).First()).ToList();
                    feedBackCallerList = feedBackCallerList.OrderBy(x => x.NAME).ToList();
                }
                return feedBackCallerList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
