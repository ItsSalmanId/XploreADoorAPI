using System.Collections.Generic;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.PatientSurvey;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public interface ISurveyAutomationService
    {
        SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation);
        List<SurveyQuestions> GetSurveyQuestionDetails(SurveyLink objSurveyLink);
        ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey);
        SurveyLink DecryptionUrl(SurveyLink objSurveyLink);
    }
}
