using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
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
        public QADashboardData GetDashboardData(QADashboardSearch qADashboardSearch, UserProfile profile)
        {
            try
            {
                if (qADashboardSearch.TIME_FRAME == "LAST_WEEK")
                {
                    DayOfWeek weekStart = DayOfWeek.Monday;
                    DateTime startingDate = DateTime.Today;
                    while (startingDate.DayOfWeek != weekStart)
                        startingDate = startingDate.AddDays(-1);
                    qADashboardSearch.START_DATE = startingDate.AddDays(-7);
                    qADashboardSearch.END_DATE = startingDate.AddDays(-1);
                }
                if (qADashboardSearch.TIME_FRAME == "LAST_TWO_WEEK")
                {
                    DayOfWeek weekStart = DayOfWeek.Monday;
                    DateTime startingDate = DateTime.Today;
                    while (startingDate.DayOfWeek != weekStart)
                        startingDate = startingDate.AddDays(-1);
                    qADashboardSearch.START_DATE = startingDate.AddDays(-14);
                    qADashboardSearch.END_DATE = startingDate.AddDays(-1);
                }
                if (qADashboardSearch.TIME_FRAME == "LAST_THREE_WEEK")
                {
                    DayOfWeek weekStart = DayOfWeek.Monday;
                    DateTime startingDate = DateTime.Today;
                    while (startingDate.DayOfWeek != weekStart)
                        startingDate = startingDate.AddDays(-1);
                    qADashboardSearch.START_DATE = startingDate.AddDays(-21);
                    qADashboardSearch.END_DATE = startingDate.AddDays(-1);
                }
                if (qADashboardSearch.TIME_FRAME == "LAST_MONTH")
                {
                    DateTime Today = DateTime.Today;
                    qADashboardSearch.START_DATE = (new DateTime(Today.Year, Today.Month, 1)).AddMonths(-1);
                    qADashboardSearch.END_DATE = qADashboardSearch.START_DATE.AddMonths(1).AddSeconds(-1);
                }
                if (qADashboardSearch.TIME_FRAME == "LAST_TWO_MONTHS")
                {
                    DateTime Today = DateTime.Today;
                    qADashboardSearch.START_DATE = (new DateTime(Today.Year, Today.Month, 1)).AddMonths(-2);
                    qADashboardSearch.END_DATE = qADashboardSearch.START_DATE.AddMonths(2).AddSeconds(-1);
                }
                if (qADashboardSearch.TIME_FRAME == "LAST_THREE_MONTHS")
                {
                    DateTime Today = DateTime.Today;
                    qADashboardSearch.START_DATE = (new DateTime(Today.Year, Today.Month, 1)).AddMonths(-3);
                    qADashboardSearch.END_DATE = qADashboardSearch.START_DATE.AddMonths(3).AddSeconds(-1);
                }
                if (qADashboardSearch.EVALUATION_NAME.Contains("Client Experience") && qADashboardSearch.EVALUATION_NAME.Contains("System Product and Process") || qADashboardSearch.EVALUATION_NAME.Contains("Call Quality") && qADashboardSearch.EVALUATION_NAME.Contains("System Usage"))
                {
                    qADashboardSearch.EVALUATION_NAME = "System and Client";
                }
                if (qADashboardSearch.EVALUATION_NAME.Contains("Call Quality"))
                {
                    qADashboardSearch.EVALUATION_NAME = "Client Experience";
                }
                if (qADashboardSearch.EVALUATION_NAME.Contains("System Usage"))
                {
                    qADashboardSearch.EVALUATION_NAME = "System Product and Process";
                }
                QADashboardData EvaluatedData = new QADashboardData();
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
                    var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    var callHandlingId = new SqlParameter { ParameterName = "CALL_SCANRIO_ID", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.CALL_HANDLING_ID };
                    var callType = new SqlParameter { ParameterName = "CALL_TYPE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.CALL_TYPE ?? (object)DBNull.Value };
                    var empUserName = new SqlParameter { ParameterName = "AGENT_NAME", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.EMPLOYEE_USER_NAME ?? (object)DBNull.Value };
                    var criteriaName = new SqlParameter { ParameterName = "CRITERIA_NAME", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.EVALUATION_NAME ?? (object)DBNull.Value };
                    var start = new SqlParameter { ParameterName = "START_DATE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.START_DATE };
                    var end = new SqlParameter { ParameterName = "END_DATE", SqlDbType = SqlDbType.VarChar, Value = qADashboardSearch.END_DATE };
                    EvaluatedData = SpRepository<QADashboardData>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_QA_DASHBOARD_DATA @PRACTICE_CODE,@CALL_SCANRIO_ID,@CALL_TYPE,@AGENT_NAME,@CRITERIA_NAME,@START_DATE,@END_DATE", practiceCode, callHandlingId, callType, empUserName, criteriaName, start, end);            
                }
                return EvaluatedData;
            }
            catch (Exception)
            {
                throw;
            }         
        }

        public List<FeedBackCaller> GetEmployeelist(string callScanrioID, UserProfile profile)
        {
            try
            {
                List<FeedBackCaller> list = new List<FeedBackCaller>();
                if (callScanrioID != null && profile != null)
                {
                    if (callScanrioID.StartsWith(","))
                    {
                        callScanrioID = callScanrioID.Remove(0, 1);
                    }
                    var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    var callID = new SqlParameter { ParameterName = "CALL_SCANRIO_ID", SqlDbType = SqlDbType.VarChar, Value = callScanrioID };
                    list = SpRepository<FeedBackCaller>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TEAM_EMPLOYEE_LIST  @PRACTICE_CODE, @CALL_SCANRIO_ID", practiceCode, callID);
                    return list;
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
