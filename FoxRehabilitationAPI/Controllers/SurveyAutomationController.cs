using System.Net;
using System.Net.Http;
using System.Web.Http;
using FOX.BusinessOperations.SurveyAutomationService;
using FoxRehabilitationAPI.Filters;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FoxRehabilitationAPI.Controllers
{
    [ExceptionHandlingFilter]
    [AllowAnonymous]
    public class SurveyAutomationController : BaseApiController
    {
        private readonly ISurveyAutomationService _surveyAutomationService;
        public SurveyAutomationController(ISurveyAutomationService surveyAutomationService)
        {
            _surveyAutomationService = surveyAutomationService;
        }
        [HttpPost]
        public HttpResponseMessage GetPatientDetails(SurveyAutomation objSurveyAutomation)
        {

            if (objSurveyAutomation != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetPatientDetails(objSurveyAutomation));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage UpdatePatientSurvey(FOX.DataModels.Models.PatientSurvey.PatientSurvey objPatientSurvey)
        {

            if (objPatientSurvey != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.UpdatePatientSurvey(objPatientSurvey));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient survey model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetSurveyQuestionDetails(SurveyLink objsurveyLink)
        {
            if (objsurveyLink != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetSurveyQuestionDetails(objsurveyLink));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patinet Account is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage DecryptionUrl(SurveyLink objsurveyLink)
        {
            if (objsurveyLink != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.DecryptionUrl(objsurveyLink));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patinet Account is empty");
            }
        }
    }
}
