using FOX.BusinessOperations.UnAssignedQueueService;
using FOX.DataModels.Models.UnAssignedQueueModel;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class UnAssignedQueueController : BaseApiController
    {
        private readonly IUnAssignedQueueService _UnAssignedService;
        public UnAssignedQueueController(IUnAssignedQueueService unAssignedService)
        {
            _UnAssignedService = unAssignedService;
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelUnassignedQueue(UnAssignedQueueRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _UnAssignedService.ExportToExcelUnassignedQueue(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportUnassignedQueue(UnAssignedQueueRequest obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _UnAssignedService.Export(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetUnAssignedQueue(UnAssignedQueueRequest req)
        {
            var users = _UnAssignedService.GetUnAssignedQueue(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSupervisorForDropdown()
        {
            var profile = GetProfile();
            var users = _UnAssignedService.GetSupervisorForDropdown(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetIndexersForDropdown()
        {
            var profile = GetProfile();
            var users = _UnAssignedService.GetIndexersForDropdown(profile.PracticeCode, profile.UserName);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetInitialData(UnAssignedQueueRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _UnAssignedService.GetInitialData(req, GetProfile()));
        }
    }
}
