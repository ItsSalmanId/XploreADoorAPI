using FOX.BusinessOperations.CommonServices.UploadFiles;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.PatientSurveyService;
using FOX.DataModels.Models.CommonModel;
using FoxRehabilitationAPI.Filters;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Configuration;

namespace FoxRehabilitationAPI.Controllers
{
    [ExceptionHandlingFilter]
    [Authorize]
    public class UploadFilesController : BaseApiController
    {
        private readonly IUploadFilesServices _IUploadFilesServices;
        private readonly IPatientSurveyService _patientSurvey;
        public UploadFilesController(IUploadFilesServices uploadFilesServices, IPatientSurveyService patientSurveyService)
        {
            _IUploadFilesServices = uploadFilesServices;
            _patientSurvey = patientSurveyService;
        }

        [HttpPost]
        public Task<HttpResponseMessage> UploadFilesAPI()
        {
            RequestUploadFilesModel requestUploadFilesAPIModel = new RequestUploadFilesModel()
            {
                //AllowedFileExtensions = new List<string> { ".pdf", ".png", ".jpg", ".JPG", ".jpeg", ".tiff", ".tif", ".docx" },
                AllowedFileExtensions = new List<string> { ".pdf", ".docx", ".jpg", ".jpeg", ".png", ".tif", ".gif", ".txt", ".tiff" },
                UploadFilesPath = HttpContext.Current.Server.MapPath("~/" + FOX.BusinessOperations.CommonServices.AppConfiguration.RequestForOrderUploadImages),
                Files = HttpContext.Current.Request.Files
            };
            var uploadFiles = _IUploadFilesServices.UploadFiles(requestUploadFilesAPIModel);
            var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            return Task.FromResult(response);
        }

        [HttpPost]
        public Task<HttpResponseMessage> UploadExcelFilesAPI()
        {
            RequestUploadFilesModel requestUploadFilesModel = new RequestUploadFilesModel()
            {
                AllowedFileExtensions = new List<string> { ".xlsx", ".xls", ".csv" },
                UploadFilesPath = HttpContext.Current.Server.MapPath("~/" + FOX.BusinessOperations.CommonServices.AppConfiguration.PatientSurveyUploadFiles),
                Files = HttpContext.Current.Request.Files
            };
            var uploadFiles = _IUploadFilesServices.UploadFiles(requestUploadFilesModel);
            var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            return Task.FromResult(response);
        }
        [HttpPost]
        public Task<HttpResponseMessage> UploadExcelFilesAPIHR()
        {
            RequestUploadFilesModel requestUploadFilesModel = new RequestUploadFilesModel()
            {
                AllowedFileExtensions = new List<string> { ".xlsx", ".xls", ".csv" },
                UploadFilesPath = HttpContext.Current.Server.MapPath("~/" + FOX.BusinessOperations.CommonServices.AppConfiguration.HREmailUploadFiles),
                Files = HttpContext.Current.Request.Files
            };
            var uploadFiles = _IUploadFilesServices.UploadFiles(requestUploadFilesModel);
            var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            return Task.FromResult(response);
        }
        [HttpPost]
        public Task<HttpResponseMessage> UploadTaskAttachment()
        {
            var taskAttachmentsPath = FOX.BusinessOperations.CommonServices.AppConfiguration.TasksAttachmentsPath;
            var absoluteAttachmentPath = HttpContext.Current.Server.MapPath("~/" + taskAttachmentsPath);
            RequestUploadFilesModel requestUploadFilesModel = new RequestUploadFilesModel()
            {
                AllowedFileExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".bmp", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".pdf" },
                Files = HttpContext.Current.Request.Files,
                UploadFilesPath = absoluteAttachmentPath
            };
            var uploadFiles = _IUploadFilesServices.UploadFiles(requestUploadFilesModel);
            uploadFiles.FilePath = $@"{taskAttachmentsPath}\{uploadFiles.FilePath}";
            var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            return Task.FromResult(response);
        }
        [HttpPost]
        public Task<HttpResponseMessage> UploadDocumentAttachment()
        {
            var documentAttachmentsPath = FOX.BusinessOperations.CommonServices.AppConfiguration.DocumentAttachmentsPath;
            var absoluteAttachmentPath = HttpContext.Current.Server.MapPath("~/" + documentAttachmentsPath);
            RequestUploadFilesModel requestUploadFilesModel = new RequestUploadFilesModel()
            {
                AllowedFileExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".bmp", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".pdf" },
                Files = HttpContext.Current.Request.Files,
                UploadFilesPath = absoluteAttachmentPath
            };
            var uploadFiles = _IUploadFilesServices.UploadFiles(requestUploadFilesModel);
            uploadFiles.FilePath = $@"{documentAttachmentsPath}\{uploadFiles.FilePath}";
            var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            return Task.FromResult(response);
        }

        //public async Task<HttpResponseMessage> UploadReconsiliationLedger()
        //{
        //    var reconsiliationLedgerPath = FOX.BusinessOperations.CommonServices.AppConfiguration.ReconciliationOriginalFilesDirectory;
        //    var absoluteAttachmentPath = HttpContext.Current.Server.MapPath("~/" + reconsiliationLedgerPath);
        //    RequestUploadFilesModel requestUploadFilesModel = new RequestUploadFilesModel()
        //    {
        //        AllowedFileExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".tif", ".tiff", ".pdf" },
        //        Files = HttpContext.Current.Request.Files,
        //        UploadFilesPath = absoluteAttachmentPath,

        //    };
        //    var uploadFiles = _IUploadFilesServices.UploadReconsiliationLedger(requestUploadFilesModel);
        //    uploadFiles.FilePath = $@"{reconsiliationLedgerPath}\{uploadFiles.FilePath}";
        //    var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
        //    return response;
        //}
        [HttpPost]
        public Task<HttpResponseMessage> UploadPHDFiles()
        {
            RequestUploadFilesModel requestUploadFilesAPIModel = new RequestUploadFilesModel()
            {
                //AllowedFileExtensions = new List<string> { ".pdf", ".png", ".jpg", ".JPG", ".jpeg", ".tiff", ".tif", ".docx" },
                AllowedFileExtensions = new List<string> { ".pdf", ".jpg", ".jpeg", ".png" },
                UploadFilesPath = HttpContext.Current.Server.MapPath("~/" + FOX.BusinessOperations.CommonServices.AppConfiguration.PHDFilesUploadImages),
                Files = HttpContext.Current.Request.Files
            };
            var uploadFiles = _IUploadFilesServices.UploadFiles(requestUploadFilesAPIModel);
            var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            return Task.FromResult(response);
        }
        [HttpPost]
        public Task<HttpResponseMessage> UploadHRAutoEmailFiles()
        {
            RequestUploadFilesModel requestUploadFilesAPIModel = new RequestUploadFilesModel()
            {
                AllowedFileExtensions = new List<string> { ".pdf", ".jpg", ".jpeg", ".png" },
                UploadFilesPath = HttpContext.Current.Server.MapPath("~/" + FOX.BusinessOperations.CommonServices.AppConfiguration.HrAutoEmailUploadImages),
                //UploadFilesPath = @"\\IT-126\FoxDocumentDirectory\HRAutoEmailAttachmentFiles",
                Files = HttpContext.Current.Request.Files,
            };
            foreach (var item in HttpContext.Current.Request.Form.AllKeys)
            {
                requestUploadFilesAPIModel.HR_CONFIGURE_ID = int.Parse(HttpContext.Current.Request.Form[item]);
            }
            var uploadFiles = _IUploadFilesServices.UploadHAutoEmailFiles(requestUploadFilesAPIModel, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            return Task.FromResult(response);
        }
    }
}