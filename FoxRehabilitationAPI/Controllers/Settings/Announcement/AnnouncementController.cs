using FOX.BusinessOperations.SettingsService.AnnouncementService;
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
    }
}
