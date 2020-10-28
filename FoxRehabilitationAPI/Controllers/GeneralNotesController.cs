using FOX.BusinessOperations.CaseServices;
using FOX.BusinessOperations.GeneralNotesService;
using FOX.DataModels.Models.GeneralNotesModel;
using FoxRehabilitationAPI.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class GeneralNotesController : BaseApiController
    {
        private readonly IGeneralNotesServices _generalNotesServices;
        private readonly ICaseServices _caseServices;
        public GeneralNotesController(IGeneralNotesServices generalNotesServices, ICaseServices caseServices)
        {
            _generalNotesServices = generalNotesServices;
            _caseServices = caseServices;
        }

        [HttpPost]
        [ActionName("GetGeneralNotes")]
        public HttpResponseMessage GetGeneralNotes(GeneralNotesSearchRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetGeneralNotes(GetProfile(), request));
        }

        [HttpGet]
        [ActionName("GetAlertGeneralNotes")]
        public HttpResponseMessage GetAlertGeneralNotes(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetAlertGeneralNotes(GetProfile(), patientAccount));
        }

        [HttpPost]
        [ActionName("GetSingleNote")]
        public HttpResponseMessage GetSingleNote(GeneralNoteRequestModel request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetSingleGeneralNote(GetProfile(), request));
        }

        [HttpPost]
        [ActionName("GetSingleNoteForUpdate")]
        public HttpResponseMessage GetSingleNoteForUpdate(GeneralNoteRequestModel request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetSingleNoteForUpdate(GetProfile(), request));
        }

        [HttpPost]
        [ActionName("CreateUpdateNotes")]
        public HttpResponseMessage CreateUpdateNotes([FromBody]GeneralNoteCreateUpdateRequestModel request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.CreateUpdateNote(request, GetProfile()));
        }

        [HttpPost]
        [ActionName("UpdateNote")]
        public HttpResponseMessage UpdateNote([FromBody]GeneralNoteCreateUpdateRequestModel request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.UpdateNote(request, GetProfile()));
        }

        [HttpPost]
        [ActionName("DeleteGeneralNote")]
        public HttpResponseMessage DeleteGeneralNote([FromBody]GeneralNoteDeleteRequestModel request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.DeleteGeneralNote(GetProfile(), request));
        }

        [HttpPost]
        [ActionName("GetSmartCases")]
        public HttpResponseMessage GetSmartCases(SmartSearchCasesRequestModel request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _caseServices.GetSmartCases(request, GetProfile()));
        }

        [HttpPost]
        [ActionName("GetGeneralNoteHistory")]
        public HttpResponseMessage GetGeneralNoteHistory(GeneralNoteHistoryRequestModel request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetGeneralNoteHistory(request, GetProfile()));
        }
        [HttpGet]
        [ActionName("GetAlertTypes")]
        public HttpResponseMessage GetAlertTypes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetAlertTypes(GetProfile().PracticeCode));
        }

        [HttpGet]
        [ActionName("GetAlertTypeswithInactive")]
        public HttpResponseMessage GetAllDocumentTypeswithInactive()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetAlertTypeswithInactive(GetProfile().PracticeCode));
        }

        [HttpGet]
        [ActionName("GetPatientCasesList")]
        public HttpResponseMessage GetPatientCasesList(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetPatientCasesList(Convert.ToInt64(patientAccount), GetProfile()));
        }

        [HttpPost]
        [ActionName("GetNoteAlert")]
        public HttpResponseMessage GetNoteAlert(AlertSearchRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetNoteAlert(request, GetProfile().PracticeCode));
        }
        [HttpPost]
        [ActionName("CreateUpdateNoteAlert")]
        public HttpResponseMessage CreateUpdateNoteAlert(NoteAlert request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.CreateUpdateNoteAlert(request, GetProfile()));
        }
        [HttpGet]
        [ActionName("DeleteAlert")]
        public HttpResponseMessage DeleteAlert(long alertId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.DeleteAlert(alertId, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetGeneralNotesInterfaceLogs(InterfaceLogSearchRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.GetGeneralNotesInterfaceLogs(request, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage RetryInterfacing(InterfaceLogSearchRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.RetryInterfacing(request, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelInterfaceLogs(InterfaceLogSearchRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.ExportToExcelInterfaceLogs(request, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage checkPatientisInterfaced(string Patient_Account)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _generalNotesServices.checkPatientisInterfaced(Patient_Account, GetProfile()));
        }
    }
}
