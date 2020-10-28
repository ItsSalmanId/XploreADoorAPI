using FOX.BusinessOperations.SettingsService.PracticeOrganizationService;
using FOX.DataModels.Models.Settings.Practice;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.Settings.PracticeOrganization
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class PracticeOrganizationController : BaseApiController
    {
        private readonly PracticeOrganizationService _practiceOrganizationService;

        public PracticeOrganizationController(PracticeOrganizationService practiceOrganizationService)
        {
            _practiceOrganizationService = practiceOrganizationService;
        }

        [HttpGet]
        public HttpResponseMessage GetMaxPracticeOrganizationCode()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK,
                _practiceOrganizationService.GetMaxPracticeOrganizationCode(profile.PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage GetPracticeOrganizationList(PracticeOrganizationRequest practiceOrganizationRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                _practiceOrganizationService.GetPracticeOrganizationList(practiceOrganizationRequest, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage AddUpdatePracticeOrganization(FOX.DataModels.Models.Settings.Practice.PracticeOrganization practiceOrganization)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                _practiceOrganizationService.AddUpdatePracticeOrganization(practiceOrganization, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPracticeOrganizationByName(string searchText)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK,
                _practiceOrganizationService.GetPracticeOrganizationByName(searchText, profile.PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage Export(PracticeOrganizationRequest obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _practiceOrganizationService.Export(obj, GetProfile()));
        }
    }
}
