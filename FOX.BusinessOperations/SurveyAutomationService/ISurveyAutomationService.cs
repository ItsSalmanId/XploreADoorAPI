using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using FOX.DataModels.Models.SurveyAutomation;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public interface ISurveyAutomationService
    {
        SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation);
        List<SurveyQuestions> GetSurveyQuestionDetails(string patinetAccount);
        ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey);
    }
}
