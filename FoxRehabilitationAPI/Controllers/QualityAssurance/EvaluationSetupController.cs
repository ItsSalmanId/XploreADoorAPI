using FOX.BusinessOperations.QualityAssuranceService.EvaluationSetupService;
using FOX.DataModels.Models.QualityAsuranceModel;
using FoxRehabilitationAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace FoxRehabilitationAPI.Controllers.Quality_Assurance
{
    [ExceptionHandlingFilter]
    public class EvaluationSetupController : BaseApiController
    {
        private readonly IEvaluationSetupService _EvaluationSetupService;
        public EvaluationSetupController(IEvaluationSetupService EvaluationSetuServices)
        {
            _EvaluationSetupService = EvaluationSetuServices;
        }
        [HttpPost]
        public HttpResponseMessage AllEvaluationCriteria(RequestModelForCallType model)
        {
            var profile = GetProfile();
            var result = _EvaluationSetupService.AllEvaluationCriteria(model, profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage onSaveOverallWeightageCriteria(RequestModelOverallWeightage model)
        {
            var result = _EvaluationSetupService.onSaveOverallWeightageCriteria(model, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage onSaveClientExperience(RequestModelClientExperience model)
        {
            var result = _EvaluationSetupService.onSaveClientExperience(model, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage onSaveSystemProductprocess(RequestModelSystemProcess model)
        {
            var result = _EvaluationSetupService.onSaveSystemProductprocess(model, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage onSaveWowFactor(RequestModelWowfactor model)
        {
            var result = _EvaluationSetupService.onSaveWowFactor(model, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage onSaveGradingSetup(RequestModelGradingSetup model)
        {
            var result = _EvaluationSetupService.onSaveGradingSetup(model, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}