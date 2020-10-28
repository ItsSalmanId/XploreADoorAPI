using FOX.DataModels.Models.AssignedQueueModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.AssignedQueueService
{
    public interface IAssignedQueueServices
    {
        List<AssignedQueue> GetIndexedQueue(AssignedQueueRequest req, UserProfile Profile);
        string ExportToExcelAssignedQueue(AssignedQueueRequest req, UserProfile profile);
        List<UsersForDropdown> GetSupervisorAndAgentsForDropdown(long practiceCode,long roleid, string userName);
        List<UsersForDropdown> GetIndexersForDropdown(long practiceCode, string userName);
        List<InterfcaeFailedPatient> GeInterfaceFailedPatientList(long practiceCode, string userName);
        //List<AssignedQueue> GetTrashedQueue(AssignedQueueRequest req, UserProfile Profile);
        ResponseModel BlackListOrWhiteListSource(BlacklistWhiteListSourceModel req, UserProfile Profile);
        ResponseModel MakeReferralAsValidOrTrashed(MarkReferralValidOrTrashedModel req, UserProfile profile);
        List<UsersForDropdown> GetSupervisorsForDropdown(long practiceCode, string userName);
    }
}
