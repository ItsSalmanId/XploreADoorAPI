using FOX.BusinessOperations.PatientMaintenanceService;
using FOX.DataModels.Models.IndexInfo;
using FoxRehabilitationAPI.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.PatientMaintenance
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class PatientMaintenanceController : BaseApiController
    {
        private readonly IPatientMaintenanceService _patientMaintenanceService;

        public PatientMaintenanceController(IPatientMaintenanceService patientMaintenanceService)
        {
            _patientMaintenanceService = patientMaintenanceService;
        }

        [HttpGet]
        public HttpResponseMessage GetPatientByAcoountNo(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientMaintenanceService.GetPatientByAccountNo(Convert.ToInt64(patientAccount)));
        }

        [HttpPost]
        public HttpResponseMessage SearchPatients(getPatientReq patientReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientMaintenanceService.SearchPatients(patientReq, GetProfile()));
        }
    }
}
