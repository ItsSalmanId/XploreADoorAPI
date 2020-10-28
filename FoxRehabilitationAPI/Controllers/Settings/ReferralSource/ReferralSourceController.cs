using FOX.BusinessOperations.StoreProcedure.SettingsService.ReferralSourceService;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.Settings.ReferralSource
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class ReferralSourceController : BaseApiController
    {
        private readonly IReferralSourceService _referralSourceService;

        public ReferralSourceController(IReferralSourceService referralSourceService)
        {
            _referralSourceService = referralSourceService;
        }

        [HttpGet]
        public HttpResponseMessage GetInactiveReasonAndDeliveryMethod()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _referralSourceService.GetInactiveReasonAndDeliveryMethod(profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetReferralSourceByName(string searchString)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _referralSourceService.GetReferralSourceByName(searchString, profile.PracticeCode));
        }
    }
}
