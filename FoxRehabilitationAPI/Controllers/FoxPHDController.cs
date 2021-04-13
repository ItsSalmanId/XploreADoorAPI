using FOX.BusinessOperations.FoxPHDService;
using FOX.DataModels.Models.FoxPHD;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class FoxPHDController : BaseApiController
    {
        private readonly IFoxPHDService _IFoxPHDService;
        public FoxPHDController(IFoxPHDService PHDServices)
        {
            _IFoxPHDService = PHDServices;
        }
        [HttpPost]
        public HttpResponseMessage GetPatientInformation(PatientsSearchRequest ObjPatientSearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetPatientInformation(ObjPatientSearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage DeleteCallDetailRecordInformation(PHDCallDetail ObjPHDCallDetailRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.DeleteCallDetailRecordInformation(ObjPHDCallDetailRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetPHDCallDetailsInformation(CallDetailsSearchRequest ObjCallDetailsSearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetPHDCallDetailsInformation(ObjCallDetailsSearchRequest, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetDropdownLists()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetDropdownLists(GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage AddUpdatePHDCallDetailInformation(PHDCallDetail ObjPHDCallDetailRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.AddUpdatePHDCallDetailInformation(ObjPHDCallDetailRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage AddUpdateVerificationInformation(PhdPatientVerification ObjPhdPatientVerification)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.AddUpdateVerificationInformation(ObjPhdPatientVerification, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelPHD(CallDetailsSearchRequest ObjCallDetailsSearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.ExportToExcelPHD(ObjCallDetailsSearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage AddUpdateRecordingName(PHDCallDetail ObjCallDetailsSearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.AddUpdateRecordingName(ObjCallDetailsSearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetUnmappedCalls(UnmappedCallsSearchRequest reg)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetUnmappedCalls(reg, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetFoxDocumentTypes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetFoxDocumentTypes(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetFollowUpCalls()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetFollowUpCalls(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetCaseDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetCaseDetails(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetExportAdvancedDailyReports(string callerUserID)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetExportAdvancedDailyReports(GetProfile(), callerUserID));
        }
        [HttpPost]
        public HttpResponseMessage ExportAdvancedDailyReport(ExportAdvancedDailyReport advancedregionreq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.ExportAdvancedDailyReport(advancedregionreq, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetPhdCallLogHistoryDetail(string phdCallDetailID)
        {
            if (phdCallDetailID != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetPhdCallLogHistoryDetails(phdCallDetailID, GetProfile()));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Phd Call ID is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetWebSoftCaseStatusResponse(string sscmCaseNumber)
        {
            if (sscmCaseNumber != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetWebSoftCaseStatusResponses(sscmCaseNumber));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SSCM Case Number is Null");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetdefaultCallHandling(string searchText)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetPhdCallScenariosList(searchText, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetCallHandlingValues()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetPhdCallScenarios(GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage SavedefaultCallHandling(List<DefaultVauesForPhdUsers> Obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.SavePhdScanarios(Obj, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetDefaultPhdScanarios()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IFoxPHDService.GetDefaultHandlingValue(GetProfile()));
        }
    }
}