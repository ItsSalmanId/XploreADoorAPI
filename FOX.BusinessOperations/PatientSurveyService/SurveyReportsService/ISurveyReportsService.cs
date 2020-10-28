using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.PatientSurveyService.SurveyReportsService
{
    public interface ISurveyReportsService
    {
        List<PatientSurvey> GetPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PatientSurvey> GetALLPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        PSDRChartData GetALLPendingPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PSRRegionAndQuestionWise> GetPSRRegionAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PSRProviderAndQuestionWise> GetPSRProviderAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PSRRegionAndRecommendationWise> GetPSRRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<RegionWisePatientData> GetRegionWisePatientData(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        List<PSRProviderAndRecommendationWise> GetPSRProviderAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        string ExportToExcelPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        string ExportToExcelPSRRegionAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        string ExportToExcelRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
        string ExportToExcelRegionWisePatientData(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile);
    }
}
