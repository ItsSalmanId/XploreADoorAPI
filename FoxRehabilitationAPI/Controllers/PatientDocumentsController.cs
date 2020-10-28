using FOX.BusinessOperations.PatientDocumentsService;
using FOX.DataModels.Models.PatientDocuments;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class PatientDocumentsController : BaseApiController
    {
        private readonly IPatientDocumentsService _IPatientDocumentsService;
        public PatientDocumentsController(IPatientDocumentsService DocumentsServices)
        {
            _IPatientDocumentsService = DocumentsServices;
        }
        [HttpGet]
        public HttpResponseMessage getDocumentTypes(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.getDocumentTypes(patientAccount, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetAllDocumentTypes(PatientDocument ObjPatientDocument)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.GetAllDocumentTypes(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetAllSpecialityProgram(PatientDocument ObjPatientDocument)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.GetAllSpecialityProgram(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetAllDocumentTypeswithInactive(PatientDocument ObjPatientDocument)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.GetAllDocumentTypeswithInactive(GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage getDocumentsDataInformation(PatientDocumentRequest ObjPatientDocumentRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.getDocumentsDataInformation(GetProfile(), ObjPatientDocumentRequest));
        }
            [HttpPost]
        public HttpResponseMessage ExportToExcelDocumentInformation(PatientDocumentRequest ObjPatientDocumentRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.ExportToExcelDocumentInformation(GetProfile(), ObjPatientDocumentRequest));
        }
        [HttpPost]
        public HttpResponseMessage AddUpdateNewDocumentInformation(PatientPATDocument ObjPatientPATDocument)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.AddUpdateNewDocumentInformation(ObjPatientPATDocument, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage getDocumentImagesInformation(PatientDocumentRequest ObjPatientDocumentRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.getDocumentImagesInformation(GetProfile(), ObjPatientDocumentRequest));
        }
        [HttpPost]
        public HttpResponseMessage DeleteDocumentFilesInformation(PatientDocument ObjPatientDocument)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IPatientDocumentsService.DeleteDocumentFilesInformation(GetProfile(), ObjPatientDocument));
        }
    }
}
