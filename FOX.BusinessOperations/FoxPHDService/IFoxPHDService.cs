using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.FoxPHD;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.FoxPHDService
{
    public interface IFoxPHDService
    {
        DropdownLists GetDropdownLists(UserProfile profile);
        List<Patient> GetPatientInformation(PatientsSearchRequest PatientSearchRequest, UserProfile profile);
        ResponseModel DeleteCallDetailRecordInformation(PHDCallDetail ObjAddUpdatePHDDetail, UserProfile profile);
        List<PHDCallDetail> GetPHDCallDetailsInformation(CallDetailsSearchRequest ObjCallDetailsSearchRequest, UserProfile profile);
        ResponseModel AddUpdatePHDCallDetailInformation(PHDCallDetail objAddUpdatePHDDetail, UserProfile profile);
        List<PhdFaqsDetail> GetPHDFaqsDetailsInformation(PhdFaqsDetail objPhdFaqsDetail, UserProfile profile);
        List<PhdFaqsDetail> GetDropdownListFaqs(UserProfile profile);
        ResponseModel AddUpdatePhdFaqsDetail(PhdFaqsDetail objPhdFaqsDetail, UserProfile profile);
        ResponseModel DeletePhdFaqs(PhdFaqsDetail objPhdFaqsDetail, UserProfile profile);
        ResponseModel AddUpdateVerificationInformation(PhdPatientVerification ObjPhdPatientVerification, UserProfile profile);
        string ExportToExcelPHD(CallDetailsSearchRequest ObjCallDetailsSearchRequest, UserProfile profile);
        bool AddUpdateRecordingName(PHDCallDetail ObjPHDCallDetailRequest, UserProfile profile);
        List<PHDUnmappedCalls> GetUnmappedCalls(UnmappedCallsSearchRequest reg, UserProfile profile);
        List<FoxDocumentType> GetFoxDocumentTypes(UserProfile userProfile);
        List<PHDCallDetail> GetFollowUpCalls(UserProfile userProfile);
        List<SscmCaseDetail> GetCaseDetails(UserProfile userProfile);
        List<ExportAdvancedDailyReport> GetExportAdvancedDailyReports(UserProfile profile, string callerUserID);
        string ExportAdvancedDailyReport(ExportAdvancedDailyReport exportAdvancedDailyReport, UserProfile profile);
        List<PhdCallLogHistoryDetail> GetPhdCallLogHistoryDetails(string phdCallDetailID, UserProfile userProfile);
        List<WebSoftCaseStatusResponse> GetWebSoftCaseStatusResponses(string sscmCaseNumber);
        List<CallHandlingDefaultValues> GetPhdCallScenariosList(string req, UserProfile profile);
        List<PhdCallScenario> GetPhdCallScenarios(UserProfile profile);
        ResponseModel SavePhdScanarios(List<DefaultVauesForPhdUsers> obj, UserProfile profile);
        DefaultVauesForPhdUsers GetDefaultHandlingValue(UserProfile profile);
    }
}
