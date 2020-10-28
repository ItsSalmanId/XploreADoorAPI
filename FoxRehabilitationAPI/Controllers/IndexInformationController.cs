using FOX.BusinessOperations.RequestForOrder.IndexInformationServices;
using FoxRehabilitationAPI.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class IndexInformationController : BaseApiController
    {
        private readonly IIndexInformationService _indexInformationService;
        public IndexInformationController(IIndexInformationService indexInformationService)
        {
            _indexInformationService = indexInformationService;
        }

        [HttpGet]
        public HttpResponseMessage getFacilityReferralSource(string patientAccount)
        {
            var profile = GetProfile();
            var result = _indexInformationService.getFacilityReferralSource(Convert.ToInt64(patientAccount), profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetFacilityLocations(string searchText)
        {
            var profile = GetProfile();
            var result = _indexInformationService.GetFacilityLocations(searchText, profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetFacilityByPatientPOS(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _indexInformationService.GetFacilityByPatientPOS(patientAccount, GetProfile().PracticeCode));
        }
    }
}
