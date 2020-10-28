using FOX.BusinessOperations.AssignedQueueService;
using FOX.DataModels.Models.AssignedQueueModel;
using FoxRehabilitationAPI.Filters;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class AssignedQueueController : BaseApiController
    {
        private readonly IAssignedQueueServices _AssignService;
        public AssignedQueueController(IAssignedQueueServices assignService)
        {
            _AssignService = assignService;
        }

        [HttpPost]
        public HttpResponseMessage GetAssignedQueue(AssignedQueueRequest req)
        {
            var users = _AssignService.GetIndexedQueue(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelAssignedQueue(AssignedQueueRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _AssignService.ExportToExcelAssignedQueue(req, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetIndexersForDropdown()
        {
            var profile = GetProfile();
            var users = _AssignService.GetIndexersForDropdown(profile.PracticeCode, profile.UserName);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSupervisorAndAgentsForDropdown(long roleid)
        {
            var profile = GetProfile();
            var users = _AssignService.GetSupervisorAndAgentsForDropdown(profile.PracticeCode, roleid, profile.UserName);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GeInterfaceFailedPatientList()
        {
            var profile = GetProfile();
            var users = _AssignService.GeInterfaceFailedPatientList(profile.PracticeCode, profile.UserName);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        //[HttpPost]
        //public HttpResponseMessage GetTrashedQueue(AssignedQueueRequest req)
        //{
        //    var users = _AssignService.GetTrashedQueue(req, GetProfile());
        //    var response = Request.CreateResponse(HttpStatusCode.OK, users);
        //    return response;
        //}

        [HttpPost]
        public HttpResponseMessage BlackListOrWhiteListSource(BlacklistWhiteListSourceModel req)
        {
            var users = _AssignService.BlackListOrWhiteListSource(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage MakeReferralAsValidOrTrashed(MarkReferralValidOrTrashedModel req)
        {
            var users = _AssignService.MakeReferralAsValidOrTrashed(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSupervisorsForDropdown()
        {
            var profile = GetProfile();
            var users = _AssignService.GetSupervisorsForDropdown(profile.PracticeCode, profile.UserName);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }
    }
}
