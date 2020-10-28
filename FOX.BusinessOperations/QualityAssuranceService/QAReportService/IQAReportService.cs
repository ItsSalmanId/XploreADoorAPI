using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.QualityAssuranceService.QAReportService
{
    public interface IQAReportService
    {
        List<FeedBackCaller> GetListOfAgents(long practiceCode);
        List<AuditScoresList> AuditReport(QAReportSearchRequest reg, UserProfile profile);
        List<GradingSetup> GetListOfGradingCriteria(long practiceCode);
        string ExportToExcelQAReport(QAReportSearchRequest req, UserProfile profile);
    }
}
