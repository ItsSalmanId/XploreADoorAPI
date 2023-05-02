using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FOX.BusinessOperations.CaseServices;
using FOX.DataModels.Models.CasesModel;
using FoxRehabilitationAPI.Filters;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class CasesController : BaseApiController
    {
        
        private readonly ICaseServices _CaseServices;

        public CasesController(ICaseServices CaseServices)
        {
            _CaseServices = CaseServices;
        }

        [HttpPost]
        public HttpResponseMessage AddEditCase(string locationName, string certifyState, FOX_TBL_CASE model)
        {
            var result = _CaseServices.AddEditCase(locationName,  certifyState, model, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetCasesDDL(string patient_Account)
        {
            var profile = GetProfile();
            var result = new object();
            if (profile.isTalkRehab)
            {
                result = _CaseServices.GetCasesDDLTalRehab(patient_Account, profile.PracticeCode);
            }
            else
            {
                result = _CaseServices.GetCasesDDL(patient_Account, profile.PracticeCode);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetCasesDDLTalkrehab(string patientAccount, string practiceCode)
        {
            ResponseGetCasesDDL result = _CaseServices.GetCasesDDL(patientAccount, Convert.ToInt64(practiceCode));
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetCasesDDLTalkrehab(CasesSearchRequest casesmodel)
        {
            ResponseGetCasesDDL result = _CaseServices.GetCasesDDLTalkrehab(casesmodel);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetIdentifierList()
        {
            var profile = GetProfile();
            var result = _CaseServices.GetIdentifierList(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetAllIdentifierANDSourceofReferralList()
        {
            var profile = GetProfile();
            var result = _CaseServices.GetAllIdentifierANDSourceofReferralList(profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage GetSmartIdentifier([FromBody]SmartIdentifierReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CaseServices.GetSmartIdentifier(obj, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetSourceofReferral()
        {
            var profile = GetProfile();
            var result = _CaseServices.GetSourceofReferral(profile.PracticeCode, profile.isTalkRehab);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetOpenIssueList([FromBody]GetOpenIssueListReq obj)
        {
            var result = _CaseServices.GetOpenIssueList(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetNONandHOLDIssueList([FromBody]GetOpenIssueListReq obj)
        {
            var result = _CaseServices.GetNONandHOLDIssueList(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetSmartPosLocation([FromBody]GetSmartPoslocReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CaseServices.GetSmartPosLocation(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetOrderInformationAndNotes([FromBody]getOrderInfoReq obj)
        {
            var result = _CaseServices.GetOrderInformationAndNotes(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage DeleteOrderInformation([FromBody]FOX_TBL_ORDER_INFORMATION obj)
        {
            var result = _CaseServices.DeleteOrderInformation(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAllCases(string patient_Account)
        {
            var profile = GetProfile();
            var result = _CaseServices.GetAllCases(patient_Account, profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetCallsLog([FromBody]CallReq obj)
        {
            var result = _CaseServices.GetCallInformation(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetWorkOrderInfo([FromBody]WORK_ORDER_INFO_REQ obj)
        {
            var result = _CaseServices.GetWorkOrderInfo(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetOrderingRefSource([FromBody]GetOrderingRefSourceinfoReq obj)
        {
            var result = _CaseServices.GetOrdering_Ref_Source_info(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage DeleteTask(OpenIssueListToDelete model)
        {
            var result = _CaseServices.DeleteTask(model, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSmartHearAboutFox(string searchText)
        {
            var result = _CaseServices.GetSmartHearAboutFox(searchText, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetReferralRegionAginstPosId(long posId)
        {
            var result = _CaseServices.GetReferralRegionAginstPosId(posId, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetReferralRegionAgainstORS(long ORDERING_REF_SOURCE_ID)
        {
            var result = _CaseServices.GetReferralRegionAgainstORS(ORDERING_REF_SOURCE_ID, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSmartProviders(string searchValue, int disciplineId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CaseServices.GetSmartProviders(searchValue, disciplineId, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetSmartPosLocations(string searchValue)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CaseServices.GetSmartPosLocations(searchValue, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSmartClinicain([FromBody] SmartSearchReq obj)
        {
            var result = _CaseServices.GetSmartClinicains(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAllCaseStatus()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CaseServices.GetAllCaseStatus(GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientCasesList(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CaseServices.GetPatientCasesList(Convert.ToInt64(patientAccount), GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetCasesAndOpenIssues(string caseId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CaseServices.GetCasesAndOpenIssues(Convert.ToInt64(caseId), GetProfile()));
        }
        public HttpResponseMessage PopulateTreatingProviderbasedOnPOS(GetTreatingProviderReq obj)
        {
            var result = _CaseServices.PopulateTreatingProviderbasedOnPOS(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}