using FOX.BusinessOperations.PatientSurveyService;
using FOX.DataModels.Models.PatientSurvey;
using FoxRehabilitationAPI.Filters;
using System;
using System.Collections.Generic;
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
        public HttpResponseMessage SetSurveytProgress(string patientAccount, bool progressStatus)
        {
            var result = _patientSurveyService.SetSurveytProgress(Convert.ToInt64(patientAccount), progressStatus);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage UpdatePatientSurvey(FOX.DataModels.Models.PatientSurvey.PatientSurvey patientSurvey)
        {
            if (patientSurvey != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.UpdatePatientSurvey(patientSurvey, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient survey model is empty");
            }
        }

        [HttpPost]
        public HttpResponseMessage AddPatientSurveyNotAnswered(FOX.DataModels.Models.PatientSurvey.PatientSurveyNotAnswered objPatientSurveyNotAnswered)
        {
            if (objPatientSurveyNotAnswered != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.AddPatientSurveyNotAnswered(objPatientSurveyNotAnswered, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient survey model is empty");
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPatientSurveyNotAnswered(PatientSurveyNotAnswered objPatientSurveyNotAnswered)
        {
            if (objPatientSurveyNotAnswered != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPatientSurveyNotAnswered(objPatientSurveyNotAnswered, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Admission Important Notes model is empty");
            }
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
        public HttpResponseMessage GetPatientSurveytProviderList()
        {
            var result = _patientSurveyService.GetPatientSurveytProviderList(GetProfile().PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPSRegionList(string searchText)
        {
            var result = _patientSurveyService.GetPSRegionList(searchText, GetProfile().PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPSStateList()
        {
            var result = _patientSurveyService.GetPSStateList();
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPSUserList(string searchText)
        {
            var result = _patientSurveyService.GetPSUserList(searchText, GetProfile().PracticeCode);
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

        [HttpGet]
        public HttpResponseMessage GetSurveyHistoryList(string patientAccount)
        {
            var result = _patientSurveyService.GetSurveyHistoryList(Convert.ToInt64(patientAccount), GetProfile().PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage MakeSurveyCall(PatientSurveyCall patientSurveyCall)
        {
            var result = _patientSurveyService.MakeSurveyCall(patientSurveyCall);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateSurveyCall(PatientSurveyCallLog patientSurveyCallLog)
        {
            _patientSurveyService.AddUpdateSurveyCall(patientSurveyCallLog, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, "Ok");
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSurveyCallList(string patientAccount)
        {
            var result = _patientSurveyService.GetSurveyCallList(Convert.ToInt64(patientAccount), GetProfile().PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPSSearchData()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPSSearchData(profile.PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage GetPSInitialData(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPSInitialData(patientSurveySearchRequest, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetPSDRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPSDRegionAndRecommendationWise(patientSurveySearchRequest, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetPSCallLogList(SurveyCallsLogs surveyCallsLogs)
        {
            if (surveyCallsLogs != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPSCallLogList(surveyCallsLogs, GetProfile().PracticeCode));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Survey Calls Logs model is null");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetPatientSurveyInboundCall(SurveyCallsLogs surveyCallsLogs)
        {
            if (surveyCallsLogs != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPatientSurveyInBoundCalls(surveyCallsLogs, GetProfile().PracticeCode));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Survey Calls Logs model is null");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetPSStatesList(string region)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPSStatesList(GetProfile().PracticeCode, region));
        }

        [HttpGet]
        public HttpResponseMessage GetPSRegionsList(string state)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPSRegionsList(GetProfile().PracticeCode, state));
        }

        [HttpGet]
        public HttpResponseMessage GetPSFormat()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetPSFormat(GetProfile().PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage UpdatePSFormat(string format)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.UpdatePSFormat(format, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPSDetailsFromEmail(string surveyId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.GetSurveyDetailedFromEmail(surveyId, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage SurveyPerformByUser(SelectiveSurveyList objSelectiveSurveyList)
        {
            if (objSelectiveSurveyList != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _patientSurveyService.SurveyPerformByUser(objSelectiveSurveyList, GetProfile().PracticeCode));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Perform survey model is empty");
            }
        }
    }
}
