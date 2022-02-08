using FOX.BusinessOperations.ReportingServices.ReferralReportServices;
using FOX.DataModels.Models.Reporting;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class ReportingController : BaseApiController
    {
        private readonly IReferralReportServices _reportServices;

        public ReportingController(IReferralReportServices reportServices)
        {
            _reportServices = reportServices;
        }

        [HttpPost]
        public HttpResponseMessage GetReferralReportList(ReferralReportRequest referralReportRequest)
        {
            var result = _reportServices.GetReferralReportList(referralReportRequest, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelReferralReport(ReferralReportRequest referralRegionSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reportServices.ExportToExcelReferralReport(referralRegionSearch, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetApplicatinUserNameList(string name)
        {
            var profile = GetProfile();
            var result = _reportServices.GetApplicatinUserNameList(name, profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetTaskReportList(TaskListRequest obj)
        {
            var result = _reportServices.GetTaskReportList(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage Export(TaskListRequest obj)
        {
            var Ref = _reportServices.Export(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, Ref);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage getHighBalanceReportList(HighBalanceReportReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reportServices.getHighBalanceReportList(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelHighBalanceReport(HighBalanceReportReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reportServices.ExportToExcelHighBalanceReport(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage getInterfaceLogReportList(InterfaceLogReportReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reportServices.getInterfaceLogReportList(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelInterfaceLogReport(InterfaceLogReportReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reportServices.ExportToExcelInterfaceLogReport(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage getPHRReportList(PHRReportReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reportServices.getPHRReportList(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelRequestToPHRReport(PHRReportReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reportServices.ExportToExcelRequestToPHRReport(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetPHRUserLastLoginReport(PHRUserLastLoginRequest request)
        {
            if(request != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _reportServices.GetPHRUsersLoginList(request, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Request is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage ExportPHRUserLastLoginReport(PHRUserLastLoginRequest request)
        {
            if(request != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _reportServices.ExportPHRUserLastLoginReport(request, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Request is empty");
            }
        }
    }
}
