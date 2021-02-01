using FOX.BusinessOperations.QualityAssuranceService.PerformAuditService;
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
    [Authorize]
    public class PerformAuditController : BaseApiController
    {
        private readonly IPerformAuditService _PerformAuditService;
        public PerformAuditController(IPerformAuditService PerformAuditService)
        {
            _PerformAuditService = PerformAuditService;
        }
        [HttpPost]
        public HttpResponseMessage TotalNumbersOfCriteria(RequestModelForCallType req)
        {
            var profile = GetProfile();
            var result = _PerformAuditService.GetTotalNumbersOfCriteria(profile.PracticeCode, req);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetListOfReps()
        {
            var profile = GetProfile();
            var result = _PerformAuditService.GetListOfReps(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage PostCallList(RequestCallList req)
        {
            var result = _PerformAuditService.PostCallList(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage InsertAuditScores(SurveyAuditScores req)
        {
            var result = _PerformAuditService.InsertAuditScores(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage ListAuditedCalls(RequestCallFromQA req)
        {
            var result = _PerformAuditService.ListAuditedCalls(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}