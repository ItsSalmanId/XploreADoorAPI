using FOX.BusinessOperations.SettingsService.AnnouncementService;
using FOX.DataModels.Models.Settings.Announcement;
using FoxRehabilitationAPI.Filters;
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
        public HttpResponseMessage InsertAnnouncement(Announcements objAnnouncement, AnnouncementRoles announcementRoles)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.InsertAnnouncement(objAnnouncement, announcementRoles, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetAnnouncement(Announcements objAnnouncement)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.GetAnnouncement(objAnnouncement, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage AddAnnouncementRole(AnnouncementRoles announcementRoles)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.AddAnnouncementRole(announcementRoles, GetProfile()));
        }
    }
}
