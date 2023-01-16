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
        #region #PROPERTIES
        private readonly IAnnouncementService _announcementService;
        #endregion

        #region #CONSTRUCTOR
        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        #endregion

        #region #FUNCTIONS
        [HttpGet]
        public HttpResponseMessage GetFoxRoles()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _announcementService.GetFoxRoles(GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage InsertAnnouncement(AddEditFoxAnnouncement objAnnouncement)
        {
            if (objAnnouncement != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _announcementService.InsertAnnouncement(objAnnouncement, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Announcement is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetAnnouncement(Announcements objAnnouncement)
        {
            if (objAnnouncement != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _announcementService.GetAnnouncement(objAnnouncement, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Announcement is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetAnnouncementDetails(Announcements objAnnouncement)
        {
            if (objAnnouncement != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _announcementService.GetAnnouncementDetails(objAnnouncement, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Announcement is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage DeleteAnnouncement(Announcements objAnnouncementRoles)
        {
            if (objAnnouncementRoles != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _announcementService.DeleteAnnouncement(objAnnouncementRoles, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Announcement is empty");
            }
        }
    }
    #endregion
}
