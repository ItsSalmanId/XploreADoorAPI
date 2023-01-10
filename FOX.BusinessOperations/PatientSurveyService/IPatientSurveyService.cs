using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.PatientSurveyService
{
    public interface IPatientSurveyService
    {
        bool SetSurveytProgress(long patientAccount, bool InProgress);
        void UpdatePatientSurvey(PatientSurvey patientSurvey, UserProfile profile);
        List<PatientSurvey> GetPatientSurveytList(PatientSurveySearchRequest patientSurveySearchRequest, long practiceCode);
        List<string> GetPatientSurveytProviderList(long practiceCode);
        List<string> GetPSRegionList(string searchText, long practiceCode);
        List<string> GetPSStateList();
        List<PSUserList> GetPSUserList(string searchText, long practiceCode);
        PSDResults GetPSDResults(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PatientSurveyHistory> GetSurveyHistoryList(long patientAccount, long practiceCode);
        string MakeSurveyCall(PatientSurveyCall patientSurveyCall);
        void AddUpdateSurveyCall(PatientSurveyCallLog patientSurveyCallLog, UserProfile profile);
        List<PatientSurveyCallLog> GetSurveyCallList(long patientAccount, long practiceCode);
        PSSearchData GetPSSearchData(long practiceCode);
        PSInitialData GetPSInitialData(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PSDStateAndRegionRecommendationWise> GetPSDStateAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PSDStateAndRegionRecommendationWise> GetPSDRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PatientSurveyCallLog> GetPSCallLogList(long patientAccount, long practiceCode);
        List<PatientSurveyInBoundCallResponse> GetPatientSurveyInBoundCalls(long patientAccount, long practiceCode);
        List<string> GetPSStatesList(long practiceCode, string region);
        List<string> GetPSRegionsList(long practiceCode, string state);
        string GetPSFormat(long practiceCode);
        bool UpdatePSFormat(string format, UserProfile profile);
        PatientSurvey GetSurveyDetailedFromEmail(string surveyId, long practiceCode);
        List<SurveyServiceLog> SurveyPerformByUser(long patientAccount, long practiceCode);
    }
}
