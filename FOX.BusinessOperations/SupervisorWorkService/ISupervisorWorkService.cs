using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.SupervisorWorkModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;

namespace FOX.BusinessOperations.SupervisorWorkService
{
    public interface ISupervisorWorkService 
    {

        List<SupervisorWork> GetSupervisorList(SupervisorWorkRequest req, UserProfile prof);
        string ExportToExcelSupervisorQueu(SupervisorWorkRequest req, UserProfile profile);
        List<UsersForDropdown> GetIndxersAndSupervisorsForDropdown(long practiceCode, string userName);
        List<WorkTransfer> GetWorkTransferComments(long workid);
        string SupervisorExport(SupervisorWorkRequest obj, UserProfile profile);
    }
}