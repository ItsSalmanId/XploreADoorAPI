using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public interface ISurveyAutomationService
    {
        List<Patient> GetPatientDetails();
        List<FoxRoles> GetFoxRoles(UserProfile userProfile);
        //Patient GetPatientDetails();
    }
}
