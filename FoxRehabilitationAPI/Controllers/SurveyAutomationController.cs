using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FOX.BusinessOperations.SurveyAutomationService;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.SurveyAutomation;
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
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.UpdatePatientSurvey(objPatientSurvey, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient is empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetSurveyQuestionDetails(string patinetAccount)
        {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetSurveyQuestionDetails(patinetAccount));   
        }

        [HttpGet]
        public HttpResponseMessage GetFoxRoles()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetFoxRoles(GetProfile()));
        }
    }
}
