using FOX.BusinessOperations.PatientMaintenanceService.PatientInsuranceService;
using FOX.DataModels.Models.Patient;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.PatientMaintenance.PatientInsurance
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class PatientInsuranceController : BaseApiController
    {

        private readonly IPatientInsuranceService _patientInsuranceServices;

        public PatientInsuranceController(IPatientInsuranceService patientInsuranceService)
        {
            _patientInsuranceServices = patientInsuranceService;
        }

        [HttpGet]
        public HttpResponseMessage GetUnmappedInsurancesCount()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientInsuranceServices.GetUnmappedInsurancesCount(GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetUnmappedInsurances(UnmappedInsuranceRequest unmappedInsuranceRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientInsuranceServices.GetUnmappedInsurances(unmappedInsuranceRequest, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetMTBCInsurancesSearchData()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientInsuranceServices.GetMTBCInsurancesSearchData());
        }

        [HttpPost]
        public HttpResponseMessage GetMTBCInsurances(MTBCInsurancesRequest mtbcInsurancesRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientInsuranceServices.GetMTBCInsurances(mtbcInsurancesRequest));
        }

        [HttpPost]
        public HttpResponseMessage MapUnmappedInsurance(FoxInsurancePayers foxInsurancePayors)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientInsuranceServices.MapUnmappedInsurance(foxInsurancePayors, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetUnpaidClaimsForInsurance(ClaimInsuranceSearchReq claimInsuranceSearReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientInsuranceServices.GetUnpaidClaimsForInsurance(claimInsuranceSearReq, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelInsuranceSetup(UnmappedInsuranceRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientInsuranceServices.ExportToExcelInsuranceSetup(req, GetProfile()));
        }
    }
}
