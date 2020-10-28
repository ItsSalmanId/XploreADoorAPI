using FOX.BusinessOperations.AdjustmentApprovalService;
using FOX.DataModels.Models.AdjustmentApproval;
using FoxRehabilitationAPI.Filters;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class AdjustmentApprovalController : BaseApiController
    {
        private readonly IAdjusmentApprovalServices _adjusmentApprovalService;

        public AdjustmentApprovalController(IAdjusmentApprovalServices adjustmentApprovalServices)
        {
            _adjusmentApprovalService = adjustmentApprovalServices;
        }

        [HttpGet]
        public HttpResponseMessage GetAdjustmentAmountsRange()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.GetAdjustmentAmountsRange(GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetAdjustmentStatuses()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.GetAdjustmentStatuses(GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetAdjustments([FromBody] AdjustmentsSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.GetAdjustments(searchReq, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetAdjustmentsForExcel([FromBody] AdjustmentsSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.GetAdjustmentsForExcel(searchReq, GetProfile()));
        }

        

        [HttpPost]
        public HttpResponseMessage GetAdjustmentLogs([FromBody] AdjustmentLogSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.GetAdjustmentLogs(searchReq, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetDDValues()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.GetDDValues(GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetStatusCounters([FromBody] StatusCounterSearch statusCounterSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.GetStatusCounters(statusCounterSearch, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetUsersForDD()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.GetUsersForDD(GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage SaveAdjustment([FromBody] PatientAdjustmentDetails adjustmentToSave)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.SaveAdjustment(adjustmentToSave, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage DeleteAdjustment([FromBody] AdjustmentToDelete adjustmentToDelete)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.DeleteAdjustment(adjustmentToDelete, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage AssignUser([FromBody] UserAssignmentModel userAssignmentDetails)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.AssignUser(userAssignmentDetails, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage ExportAdjustmentsToExcel([FromBody] List<PatientAdjustmentDetails> obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.ExportAdjustmentsToExcel(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage ExportAdjustmentLogsToExcel([FromBody] List<AdjustmentLog> obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.ExportAdjustmentLogsToExcel(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage SignRequest([FromBody] SignRequestDetails signReqDetails)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.SignRequest(signReqDetails, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage DownloadAdjustment([FromBody] DownloadAdjustmentModel dowloadDetails)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _adjusmentApprovalService.DownloadAdjustment(dowloadDetails, GetProfile()));
        }
    }
}
