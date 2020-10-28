using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.QualityAsuranceModel
{
    public class QAReportSearchRequest : BaseModel
    {
        public long? PRACTICE_CODE { get; set; }
        public string SEARCH_TEXT { get; set; }
        public string AGENT_NAME { get; set; }
        public string AUDITOR_NAME { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public string DATE_FROM_STR { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_TO_STR { get; set; }
        public int CURRENT_PAGE { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public int TIME_FRAME { get; set; }
        public string CALL_TYPE { get; set; }
    }
    public class AuditScoresList : BaseModel
    {
        [NotMapped]
        public int? ROW { get; set; }
        public string AGENT_NAME { get; set; }
        public string NAME { get; set; }
        public string TOTAL_POINTS { get; set; }
        public string CLIENT_EXPERIENCE_TOTAL { get; set; }
        public string SYSTEM_PROCESS_TOTAL { get; set; }
        public string WOW_FACTOR { get; set; }
        public string EVALUATIONS { get; set; }
        public int? TOTAL_RECORD_PAGES { get; set; }
        public int? TOTAL_RECORDS { get; set; }

    }
    public class AuditScoresListTemp : BaseModel
    {
        [NotMapped]
        public int? ROW { get; set; }
        public string AGENT_NAME { get; set; }
        public string NAME { get; set; }
        public decimal? TOTAL_POINTS { get; set; }
        public decimal? CLIENT_EXPERIENCE_TOTAL { get; set; }
        public decimal? SYSTEM_PROCESS_TOTAL { get; set; }
        public decimal? WOW_FACTOR { get; set; }
        public string EVALUATIONS { get; set; }
        public string GRADE { get; set; }
        public decimal? TOTAL_AVG { get; set; }
        public decimal? CLIENT_EXPERIENCE_AVG_TOTAL { get; set; }
        public decimal? SYSTEM_PROCESS_AVG_TOTAL { get; set; }
        public decimal? AVG_WOW_FACTOR { get; set; }
        public string AVG_EVALUATIONS { get; set; }
        public string AVG_GRADE { get; set; }
        public int? TOTAL_RECORD_PAGES { get; set; }
        public int? TOTAL_RECORDS { get; set; }

    }
}
