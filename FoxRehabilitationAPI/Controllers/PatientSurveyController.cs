using FOX.BusinessOperations.PatientSurveyService;
using FOX.DataModels.Models.PatientSurvey;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class PatientSurveyController : BaseApiController
    {
        private readonly IPatientSurveyService _patientSurveyService;

        public PatientSurveyController(IPatientSurveyService patientSurveyService)
        {
            _patientSurveyService = patientSurveyService;
        }

        [HttpGet]
        public HttpResponseMessage GetLastUpload()
        {
            var patientSurvey = _patientSurveyService.GetLastUpload(GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, patientSurvey);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpdatePatientSurvey(PatientSurvey patientSurvey)
        {
            _patientSurveyService.UpdatePatientSurvey(patientSurvey, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, "Add/Update successfull");
            return response;
        }

        [HttpGet]
        public HttpResponseMessage ImportDataExcelToSQL(string filePath)
        {
            var responseModel = _patientSurveyService.ReadExcel(filePath, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPatientSurvey(long patientAccount)
        {
            PatientSurveySearchRequest patientSurveySearchRequest = new PatientSurveySearchRequest();
            patientSurveySearchRequest.PATIENT_ACCOUNT_NUMBER = patientAccount.ToString();
            var result = _patientSurveyService.GetPatientSurvey(patientSurveySearchRequest, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPatientSurveyList(long patientAccount, int isSurveyed)
        {
            PatientSurveySearchRequest patientSurveySearchRequest = new PatientSurveySearchRequest();
            patientSurveySearchRequest.PATIENT_ACCOUNT_NUMBER = patientAccount.ToString();
            patientSurveySearchRequest.IS_SURVEYED = isSurveyed;
            var result = _patientSurveyService.GetPatientSurveytList(patientSurveySearchRequest, GetProfile().PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPatientSurveytProviderList(string providerName)
        {
            var result = _patientSurveyService.GetPatientSurveytProviderList(providerName, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetPSRRegionAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            var result = _patientSurveyService.GetPSRRegionAndQuestionWise(patientSurveySearchRequest, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetPSRProviderAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            var result = _patientSurveyService.GetPSRProviderAndQuestionWise(patientSurveySearchRequest, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetPSRRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            var result = _patientSurveyService.GetPSRRegionAndRecommendationWise(patientSurveySearchRequest, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetPSRProviderAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            var result = _patientSurveyService.GetPSRProviderAndRecommendationWise(patientSurveySearchRequest, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            var result = _patientSurveyService.GetPSRDetailedReport(patientSurveySearchRequest, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPSRegionList(string searchText)
        {
            var result = _patientSurveyService.GetPSRegionList(searchText, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPSStateList(string searchText)
        {
            var result = _patientSurveyService.GetPSStateList(searchText);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPSUserList(string searchText)
        {
            var result = _patientSurveyService.GetPSUserList(searchText, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetPSDResults(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            var result = _patientSurveyService.GetPSDResults(patientSurveySearchRequest, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}
