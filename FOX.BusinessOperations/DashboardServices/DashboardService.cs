using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.DashboardModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace FOX.BusinessOperations.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly DbContextDashboard _DashboardContext = new DbContextDashboard();
        private readonly GenericRepository<Dashboard> _DashboardRepository;

        public DashboardService()
        {
            _DashboardRepository = new GenericRepository<Dashboard>(_DashboardContext);
        }

        public AssignedUnassigned GetDashboardGetTotal(long practiceCode)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var result = SpRepository<AssignedUnassigned>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_DASHBOARD_GET_TOTAL @PRACTICE_CODE", parmPracticeCode);
            return result;
        }

        public List<NoOfRecordBytime> GetNoOfRecordBytime(long practiceCode, string dateFrom, string dateTo, int hourFrom, int hourTo)
        {

            hourTo = hourTo == 0 ? 23 : hourTo;
            DateTime dDateFrom = DateTime.Now;
            DateTime dDateTo = DateTime.Now;
            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                dDateFrom = Convert.ToDateTime(dateFrom);
                dDateTo = Convert.ToDateTime(dateTo);
                hourFrom -= 1;
            }
            var _practiceCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", Value = practiceCode };
            var _hourFrom = new SqlParameter { ParameterName = "@HOUR_FROM", Value = hourFrom };
            var _hourTo = new SqlParameter { ParameterName = "@HOUR_TO", Value = hourTo };
            var _fromDate = new SqlParameter { ParameterName = "@DATE_FROM", Value = dDateFrom };
            var _toDate = new SqlParameter { ParameterName = "@DATE_TO", Value = dDateTo };
            var result = SpRepository<NoOfRecordBytime>.GetListWithStoreProcedure(@"exec FOX_PROC_DASHBOARD_GET_NO_OF_RECORD_BY_TIME @PRACTICE_CODE,@HOUR_FROM, @HOUR_TO,@DATE_FROM,@DATE_TO", _practiceCode, _hourFrom, _hourTo, _fromDate, _toDate);
            return result;
        }

        public List<DashboardTrend> GetDashboardTrend(int value, long practiceCode, string dateFromUser, string dateToUser)
        {
            DateTime dateFrom = DateTime.Now;
            DateTime dateTo = DateTime.Now;
            if (!string.IsNullOrEmpty(dateFromUser) && !string.IsNullOrEmpty(dateToUser))
            {
                dateFrom = Convert.ToDateTime(dateFromUser);
                dateTo = Convert.ToDateTime(dateToUser);
            }
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };

            var _value = new SqlParameter { ParameterName = "@values", Value = value };
            var _fromDate = new SqlParameter { ParameterName = "@DateFromUser", Value = dateFrom };
            var _toDate = new SqlParameter { ParameterName = "@DateToUser", Value = dateTo };
            var result = SpRepository<DashboardTrend>.GetListWithStoreProcedure(@"exec FOX_PROC_DASHBOARD_AVERAGE_TREND @PRACTICE_CODE, @values, @DateFromUser,@DateToUser", parmPracticeCode, _value, _fromDate, _toDate);
            return result;
        }

        public Dashboard GetDashboardData(int value, int hourFrom, long practiceCode)
        {
            int hourTo = DateTime.Now.Hour;
            if (hourTo > 18)
            {
                hourTo = 18;
            }
            Dashboard _dashboard = new Dashboard();
            var _practiceCode1 = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _practiceCode2 = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _practiceCode3 = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            _dashboard.AssignedUnassigned = SpRepository<AssignedUnassigned>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_DASHBOARD_GET_TOTAL @PRACTICE_CODE", _practiceCode1);


            var _hourFrom = new SqlParameter { ParameterName = "@HOUR_FROM", Value = hourFrom };
            var _hourTo = new SqlParameter { ParameterName = "@HOUR_TO", Value = hourTo };
            var _fromDate = new SqlParameter { ParameterName = "@DATE_FROM", Value = DateTime.Now.Date };
            var _toDate = new SqlParameter { ParameterName = "@DATE_TO", Value = DateTime.Now.Date };
            _dashboard.NoOfRecordBytime = SpRepository<NoOfRecordBytime>.GetListWithStoreProcedure(@"exec FOX_PROC_DASHBOARD_GET_NO_OF_RECORD_BY_TIME @PRACTICE_CODE, @HOUR_FROM,@HOUR_TO,@DATE_FROM,@DATE_TO", _practiceCode2, _hourFrom, _hourTo, _fromDate, _toDate);



            var _value = new SqlParameter { ParameterName = "@values", Value = value };
            _dashboard.DashboardTrend = SpRepository<DashboardTrend>.GetListWithStoreProcedure(@"exec FOX_PROC_DASHBOARD_AVERAGE_TREND @PRACTICE_CODE,@values", _practiceCode3, _value);
            return _dashboard;
        }
    }
}