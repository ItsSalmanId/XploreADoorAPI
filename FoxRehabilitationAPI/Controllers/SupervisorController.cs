using FOX.BusinessOperations.SupervisorWorkService;
using FOX.DataModels.Models.AssignedQueueModel;
using FOX.DataModels.Models.SupervisorWorkModel;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class SupervisorController : BaseApiController
    {
        private readonly ISupervisorWorkService _SupervisorQueuedService;
        public SupervisorController(ISupervisorWorkService completeService)
        {
            _SupervisorQueuedService = completeService;
        }

        [HttpPost]
        public HttpResponseMessage GetCompleteQueueLIst(SupervisorWorkRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _SupervisorQueuedService.GetSupervisorList(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelSupervisorQueu(SupervisorWorkRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _SupervisorQueuedService.ExportToExcelSupervisorQueu(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage Export(SupervisorWorkRequest obj)
        {
            var Ref = _SupervisorQueuedService.SupervisorExport(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, Ref);
            return response;
            //return Request.CreateResponse(HttpStatusCode.OK, _SupervisorQueuedService.SupervisorExport(obj, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetIndxersAndSupervisorsForDropdown()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _SupervisorQueuedService.GetIndxersAndSupervisorsForDropdown(profile.PracticeCode, profile.UserName));
        }
        [HttpGet]
        public HttpResponseMessage GetWorkTransferComments(long workId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _SupervisorQueuedService.GetWorkTransferComments(workId));
        }
        [HttpPost]
        public HttpResponseMessage MakeReferralAsValidOrTrashed(MarkReferralValidOrTrashedModel req)
        {
            if (req != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _SupervisorQueuedService.MakeReferralAsValidOrTrashed(req, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Model is empty");
            }
        }
    }
}
