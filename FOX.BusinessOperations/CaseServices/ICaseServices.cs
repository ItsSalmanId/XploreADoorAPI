using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.CaseServices
{
    public interface ICaseServices
    {
        //ResponseGetCasesModel GetCasesData(UserProfile obj);
        ResponseAddEditCase AddEditCase(string locationName, string certifyState, FOX_TBL_CASE caseObj, UserProfile profile); //Get Case data
        //ResponseGetCasesDDL GetCasesDDL(UserProfile profile); //Get Cases drop down lists
        ResponseGetCasesDDL GetCasesDDL(string patient_Account, long practiceCode); //Get Cases drop down lists
        ResponseGetCasesDDL GetCasesDDLTalRehab(string patient_Account, long practiceCode);  //Get Cases drop down lists for TalkRehab Without Practice
        ResponseGetCasesDDL GetCasesDDLTalkrehab(CasesSearchRequest casesmodel);
        List<FOX_TBL_IDENTIFIER> GetIdentifierList(long practiceCode);
        List<SmartIdentifierRes> GetSmartIdentifier(SmartIdentifierReq obj, UserProfile profile);
        List<FOX_TBL_SOURCE_OF_REFERRAL> GetSourceofReferral(long practiceCode);
        List<GetSmartPoslocRes> GetSmartPosLocation(GetSmartPoslocReq obj, UserProfile Profile);
        GetOpenIssueAllListRes GetOpenIssueList(GetOpenIssueListReq req, UserProfile profile);
        GetNONandHOLDAllListRes GetNONandHOLDIssueList(GetOpenIssueListReq req, UserProfile profile);
        List<GetTotalDisciplineRes> GetTotalDiscipline(long? Patient_Account, long PracticeCode);
        OrderInformationAndNotes GetOrderInformationAndNotes(getOrderInfoReq obj, UserProfile profile);
        ResponseModel DeleteOrderInformation(FOX_TBL_ORDER_INFORMATION obj, UserProfile profile);
        List<FOX_VW_CASE> GetAllCases(string patient_Account, long practiceCode);
        List<FOX_VW_CALLS_LOG> GetCallInformation(CallReq obj, UserProfile profile);
        List<WORK_ORDER_INFO_RES> GetWorkOrderInfo(WORK_ORDER_INFO_REQ obj, UserProfile profile);
        GetOrderingRefSourceinfoRes GetOrdering_Ref_Source_info(GetOrderingRefSourceinfoReq obj, UserProfile profile);
        List<FOX_VW_CASE> GetSmartCases(SmartSearchCasesRequestModel request, UserProfile Profile);
        ResponseAddEditCase DeleteTask(OpenIssueListToDelete caseObj, UserProfile profile); //Get Case data
        List<FOX_TBL_HEAR_ABOUT_US_OPTIONS> GetSmartHearAboutFox(string searchText, UserProfile profile);
        ReferralRegion GetReferralRegionAginstPosId(long posId, UserProfile userProfile);
        Referral_Region_View GetReferralRegionAgainstORS(long ORDERING_REF_SOURCE_ID, UserProfile userProfile);
        List<GetSmartPoslocRes> GetSmartPosLocations(string searchText, UserProfile Profile);
        List<Provider> GetSmartClinicains(SmartSearchReq obj, UserProfile Profile);
        List<Provider> GetSmartProviders(string searchValue, int disciplineId, UserProfile Profile);
        List<FOX_TBL_CASE_STATUS> GetAllCaseStatus(UserProfile profile);
        List<FOX_VW_CASE> GetPatientCasesList(long patientAccount, UserProfile profile);
        CaseAndOpenIssues GetCasesAndOpenIssues(long caseId, UserProfile profile);
        GetTreatingProviderRes PopulateTreatingProviderbasedOnPOS(GetTreatingProviderReq obj, UserProfile profile);
        InactiveListOfGroupIDNAndSourceOfReferral GetAllIdentifierANDSourceofReferralList(UserProfile profile);
    }
}
