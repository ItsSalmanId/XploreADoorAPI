using FOX.BusinessOperations.PatientSurveyService.SurveyReportsService;
using FOX.DataModels.Models.PatientSurvey;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.PatientSurvey.SurveyReports
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class SurveyReportsController : BaseApiController
    {
        private readonly ISurveyReportsService _surveyReportsService;

        public SurveyReportsController(ISurveyReportsService surveyReportsService)
        {
            _surveyReportsService = surveyReportsService;
        }

        [HttpPost]
        public HttpResponseMessage GetPSRRegionAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetPSRRegionAndQuestionWise(patientSurveySearchRequest, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetPSRProviderAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetPSRProviderAndQuestionWise(patientSurveySearchRequest, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetPSRRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetPSRRegionAndRecommendationWise(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetRegionWisePatientData(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetRegionWisePatientData(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetPSRProviderAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetPSRProviderAndRecommendationWise(patientSurveySearchRequest, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            patientSurveySearchRequest.objNotAnswered = new PatientSurveyNotAnswered(); ;
            patientSurveySearchRequest.objNotAnswered.NOT_ANSWERED_REASON = "";
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetPSRDetailedReport(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetALLPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetALLPSRDetailedReport(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetALLPsrCount(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetALLPsrCount(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetALLPendingPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest)
        {

            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetALLPendingPSRDetailedReport(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.ExportToExcelPSRDetailedReport(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.ExportToExcelRegionAndRecommendationWise(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelPSRRegionAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.ExportToExcelPSRRegionAndQuestionWise(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelRegionWisePatientData(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.ExportToExcelRegionWisePatientData(patientSurveySearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetAllPendingDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            if(patientSurveySearchRequest != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyReportsService.GetAllPendingDetailedReport(patientSurveySearchRequest, GetProfile()));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Patient Survey Search Request is null");
            }
        }
    }
}
