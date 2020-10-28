using FOX.BusinessOperations.GroupServices;
using FOX.DataModels.Models.GroupsModel;
using FOX.DataModels.Models.Security;
using FoxRehabilitationAPI.Filters;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class GroupController : BaseApiController
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        [HttpPost]
        public HttpResponseMessage AddUpdateGroup(GROUP group)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _groupService.AddUpdateGroup(group, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetGroupsList()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _groupService.GetGroupsList(GetProfile()));

        }

        [HttpPost]
        public HttpResponseMessage DeleteGroup(GROUP group)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _groupService.DeleteGroup(group, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetUsersWithTheirRole()
        {
            var res = _groupService.GetUsersWithTheirRole(GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpPost]
        public HttpResponseMessage AddUsersInGroup(GroupUsersCreateViewModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _groupService.AddUsersInGroup(model, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetGroupUsersByGroupId(long groupId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _groupService.GetGroupUsersByGroupId(groupId, GetProfile()));

        }
    }
}
