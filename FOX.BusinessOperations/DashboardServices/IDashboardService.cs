using FOX.DataModels.Models.DashboardModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;

namespace FOX.BusinessOperations.DashboardServices
{
    public interface IDashboardService
    {
        AssignedUnassigned GetDashboardGetTotal(long practiceCode);

        List<NoOfRecordBytime> GetNoOfRecordBytime(long practiceCode, string dateFrom, string dateTo, int hourFrom, int hourTo);

        List<DashboardTrend> GetDashboardTrend(int value, long practiceCode, string dateTimeFrom, string dateTimeTo);

        Dashboard GetDashboardData(int value, int hourFrom, long practiceCode);
    }
}