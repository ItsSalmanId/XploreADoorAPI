using FOX.BusinessOperations.SettingsService.AnnouncementService;
using FOX.DataModels.Models.Settings.Announcement;
using FoxRehabilitationAPI.Filters;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.Settings.Announcement
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class AnnouncementController : BaseApiController
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        [HttpGet]
        public HttpResponseMessage GetFoxRoles()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.GetFoxRoles(GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage InsertAnnouncement(Announcements objAnnouncement)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.InsertAnnouncement(objAnnouncement, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetAnnouncement(Announcements objAnnouncement)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.GetAnnouncement(objAnnouncement, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetAnnouncementDetails(Announcements objAnnouncement)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.GetAnnouncementDetails(objAnnouncement, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage AddAnnouncementRole(List<AnnouncementRoles> objAnnouncementRoles)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.AddAnnouncementRole(objAnnouncementRoles, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage DeleteAnnouncement(Announcements objAnnouncementRoles)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.DeleteAnnouncement(objAnnouncementRoles, GetProfile()));
        }
    }
}
