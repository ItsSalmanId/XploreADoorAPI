using FOX.BusinessOperations.SettingsService.ClinicianSetupService;
using FOX.DataModels.Context;
using FOX.DataModels.Models.Settings.ClinicianSetup;
using FoxRehabilitationAPI.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.Settings.ClinicianSetup
{

    [Authorize]
    [ExceptionHandlingFilter]
    public class ClinicianSetupController : BaseApiController
    {
        public readonly IClinicianSetupService _clinicianSetupService;
        public ClinicianSetupController(IClinicianSetupService clinicianservice)
        {
            _clinicianSetupService = clinicianservice;
        }
        [HttpGet]
        public HttpResponseMessage GetVisitQoutaperweek()
        {
            var profile = GetProfile();
            var result = _clinicianSetupService.GetVisitQoutaPerWeek(profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetDisciplines()
        {
            var profile = GetProfile();
            var result = _clinicianSetupService.GetDisciplines(profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage InsertUpdateClinician(FoxProviderClass obj)
        {
            var profile = GetProfile();
            var result = _clinicianSetupService.InsertUpdateClinician(obj,profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetSmartRefRegion(string searchValue)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _clinicianSetupService.GetSmartRefRegion(searchValue, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetClinicainRecord(GetClinicanReq obj)
        {
            var profile = GetProfile();
            var result = _clinicianSetupService.GetClinician(obj, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage ExportClinician(GetClinicanReq obj)
        {
            var profile = GetProfile();
            var result = _clinicianSetupService.Export(obj, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage CheckNPIExist(string NPI)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _clinicianSetupService.CheckNPI(NPI, profile));
        }

        [HttpGet]
        public HttpResponseMessage CheckSSNExist(string SSN)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _clinicianSetupService.CheckSSN(SSN, profile));
        }
        [HttpPost]
        public HttpResponseMessage GetProviderLocation(ProviderLocationReq obj)
        {
            var profile = GetProfile();
            var result = _clinicianSetupService.GetSpecficProviderLocation(obj, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage ImportDataExcelToSQL(string filePath)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _clinicianSetupService.ReadExcel(filePath, profile.PracticeCode, profile.UserName));
        }

    }

}
