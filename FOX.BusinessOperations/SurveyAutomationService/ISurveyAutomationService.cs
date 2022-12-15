using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using FOX.DataModels.Models.SurveyAutomation;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public interface ISurveyAutomationService
    {
        List<SurveyAutomation> GetPatientDetails(SurveyAutomation objSurveyAutomation);
        List<FoxRoles> GetFoxRoles(UserProfile userProfile);
        List<SurveyQuestions> GetSurveyQuestionDetails();
        //Patient GetPatientDetails();
    }
}
