using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.UnAssignedQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;

namespace FOX.BusinessOperations.UnAssignedQueueService
{
    public interface IUnAssignedQueueService
    {
        List<UnAssignedQueue> GetUnAssignedQueue(UnAssignedQueueRequest req, UserProfile Profile);
        string ExportToExcelUnassignedQueue(UnAssignedQueueRequest req, UserProfile profile);
        List<UsersForDropdown> GetSupervisorForDropdown(long practiceCode);

        List<UsersForDropdown> GetIndexersForDropdown(long practiceCode, string userName);

        UnAssignedQueueAndUsersForDropdown GetInitialData(UnAssignedQueueRequest req, UserProfile Profile);
        string Export(UnAssignedQueueRequest obj, UserProfile userProfile);

    }
}