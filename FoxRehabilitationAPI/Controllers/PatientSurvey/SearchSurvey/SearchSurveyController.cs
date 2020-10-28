using FOX.BusinessOperations.PatientSurveyService.SearchSurveyService;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.PatientSurvey.SearchSurvey
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class SearchSurveyController : BaseApiController
    {
        private readonly ISearchSurveyService _searchSurveyService;

        public SearchSurveyController(ISearchSurveyService searchSurveyService)
        {
            _searchSurveyService = searchSurveyService;
        }

        [HttpGet]
        public HttpResponseMessage GetPatientSurvey(string patientAccount, bool isIncludeSurveyed)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _searchSurveyService.GetPatientSurvey(patientAccount, isIncludeSurveyed, profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetRandomSurvey()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _searchSurveyService.GetRandomSurvey(profile.PracticeCode));
        }
    }
}
