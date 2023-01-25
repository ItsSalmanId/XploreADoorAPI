using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.QualityAssuranceService.QADashboardService
{
   public interface IQADashboardService
    {
        List<FeedBackCaller> GetEmployeelist(string callScanrioID,UserProfile profile);
        DashBoardMainModel GetDashboardData(QADashboardSearch qADashboardSearch, UserProfile profile);
    }
}
