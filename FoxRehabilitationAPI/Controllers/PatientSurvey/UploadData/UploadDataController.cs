using FOX.BusinessOperations.PatientSurveyService.UploadDataService;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.PatientSurvey.UploadData
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class UploadDataController : BaseApiController
    {
        private readonly IUploadDataService _uploadDataService;

        public UploadDataController(IUploadDataService uploadDataService)
        {
            _uploadDataService = uploadDataService;
        }

        [HttpGet]
        public HttpResponseMessage GetLastUpload()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _uploadDataService.GetLastUpload(profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage ImportDataExcelToSQL(string filePath)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _uploadDataService.ReadExcel(filePath, profile.PracticeCode, profile.UserName));
        }
    }
}
