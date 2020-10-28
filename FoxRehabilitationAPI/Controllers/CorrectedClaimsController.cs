using FOX.BusinessOperations.CorrectedClaimService;
using FOX.DataModels.Models.CorrectedClaims;
using FoxRehabilitationAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]     
    public class CorrectedClaimsController : BaseApiController
    {
        private readonly ICorrectedClaimService _CorrectedClaimService;

        public CorrectedClaimsController(ICorrectedClaimService CorrectedClaimService)
        {
            _CorrectedClaimService = CorrectedClaimService;
        }

        [HttpGet]
        public HttpResponseMessage GetClaimTypes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CorrectedClaimService.GetClaimTypes(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetAdjustmentClaimStaus()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CorrectedClaimService.GetAdjustedClaim(GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetCorrectedClaim(CorrectedClaimSearch correctedClaimSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CorrectedClaimService.GetCorrectedClaimData(correctedClaimSearch, GetProfile()));
        }
        public HttpResponseMessage InsertUpdateCorrectedClaim([FromBody] CORRECTED_CLAIM obj)

        {
            //CorrectedClaimResponse Result = new CorrectedClaimResponse();
            var Result =  _CorrectedClaimService.InsertUpdateCorrectedClaim(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }

        //[HttpGet]
        //public HttpResponseMessage getClaimSummary()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _CorrectedClaimService.GetCorrectedClaimSummary(GetProfile()));
        //}
        [HttpGet]
        public HttpResponseMessage GetSmartPatient(string searchText)
        {
            var result = _CorrectedClaimService.GetSmartPatient(searchText, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage Export(CorrectedClaimSearch obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CorrectedClaimService.Export(obj, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage getPatientCases(string PatientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CorrectedClaimService.GetPatientCases(PatientAccount, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetClaimLog(long correctedclaimID)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _CorrectedClaimService.GetCorrectedClaimLog(correctedclaimID, GetProfile()));
        }
    }
}
