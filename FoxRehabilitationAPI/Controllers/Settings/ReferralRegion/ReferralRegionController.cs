using FOX.BusinessOperations.SettingsService.ReferralRegionServices;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.StatesModel;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.Settings.ReferralRegion
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class ReferralRegionController : BaseApiController
    {
        private readonly IReferralRegionService _referralRegionService;

        public ReferralRegionController(IReferralRegionService referralRegionService)
        {
            _referralRegionService = referralRegionService;
        }

        [HttpGet]
        public HttpResponseMessage GetReferralRegionByName(string searchText)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetReferralRegionByName(searchText, profile.PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetCountiesListByStateCode(string stateCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetCountiesListByStateCode(stateCode, GetProfile().PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetCountiesByReferralRegionId(long referralRegionId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetCountiesByReferralRegionId(referralRegionId, GetProfile().PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetReferralRegionByZipCode(string zipCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetReferralRegionByZipCode(zipCode, GetProfile().PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetReferralRegionByPatientHomeAddressZipCode(string Patient_AccountStr)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetReferralRegionByPatientHomeAddressZipCode(Patient_AccountStr, GetProfile().PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetFacilityTypes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetFacilityTypes(GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage GetZipData(RegionZipCodeDataReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetReferralRegionZipCodeData(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage SaveMappedCounty(SaveMappCountyReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.SaveMappedCounty(obj, GetProfile()));
        }
        [HttpPost]
        public ResponseModel DeleteRegionCounty(FOX_TBL_ZIP_STATE_COUNTY obj)
        {
            return _referralRegionService.RemoveReferralRegionCounty(obj, GetProfile());
        }
        [HttpPost]
        public HttpResponseMessage MapAllZipCounties(RegionZipCodeDataReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.MapAllZipCounties(obj, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetCityStateCountyRegion(string zipCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetCityStateCountyRegion(zipCode, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetCityStateCountyRegionByCity(string zipCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetCityStateCountyRegionByCity(zipCode, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetCityStateCountyRegionByCounty(string zipCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetCityStateCountyRegionByCounty(zipCode, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetSmartCity(string city)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetSmartCity(city, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetSmartCounty(string county)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetSmartCounty(county, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetAdvancedRegionSmartSearch(string searchText)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetAdvancedRegionSmartSearch(searchText, profile.PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage GetAdvancedRegionSearch(AdvanceRegionSearchRequest ObjAdvanceRegionSearchRequest)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetAdvancedRegionSearch(ObjAdvanceRegionSearchRequest, profile.PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportAdvancedRegion(showHideAdvancedRegionCol advancedregionreq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.ExportAdvancedRegion(advancedregionreq, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetRegionDashBoardUsers(long ReferralRegionId)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _referralRegionService.GetRegionDashBoardUsers(ReferralRegionId, profile.PracticeCode));
        }
    }
}
