using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.PatientSurveyService.SearchSurveyService
{
    public interface ISearchSurveyService
    {
        List<PatientSurvey> GetPatientSurvey(string patientAccount, bool isIncludeSurveyed, long practiceCode);
        PatientSurvey GetRandomSurvey(long practiceCode);
    }
}
