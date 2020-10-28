
using System.Collections.Generic;

namespace FOX.DataModels.Models.DashboardModel
{
    public class AssignedUnassigned
    {
        public int actual { get; set; }
        public int splits { get; set; }
        public int emergencyOrder { get; set; }
        public int unassign { get; set; }
        public int assign { get; set; }
        public int compeleted { get; set; }
        public int assinged_to_super { get; set; }
        public int assinged_to_indexers { get; set; }
        public int transfer_to_client { get; set; }
        public int assinged_to_agent { get; set; }
        public int indexed { get; set; }
    }

    public class NoOfRecordBytime
    {
        public int HOUR { get; set; }
        public int NO_OF_RECORDS { get; set; }
        public string RECEIVED_DATE { get; set; }
    }

    public class DashboardTrend
    {
        public int day { get; set; }
        public int month { get; set; }
        public string monthName { get; set; }
        public int year { get; set; }
        public int totalRecord { get; set; }
        public int? average { get; set; }
    }

    public class Dashboard
    {
        public AssignedUnassigned AssignedUnassigned { get; set; }
        public List<NoOfRecordBytime> NoOfRecordBytime { get; set; }
        public List<DashboardTrend> DashboardTrend { get; set; }
    }
}