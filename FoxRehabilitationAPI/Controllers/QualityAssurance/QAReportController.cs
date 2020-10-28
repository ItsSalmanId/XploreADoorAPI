using FOX.BusinessOperations.QualityAssuranceService.QAReportService;
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
    public class QAReportController : BaseApiController
    {
        private readonly IQAReportService _QAReportService;
        public QAReportController(IQAReportService IQAReportService)
        {
            _QAReportService = IQAReportService;
        }
        [HttpGet]
        public HttpResponseMessage GetListOfAgents()
        {
            var profile = GetProfile();
            var result = _QAReportService.GetListOfAgents(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage AuditReport(QAReportSearchRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                _QAReportService.AuditReport(req, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetListOfGradingCriteria()
        {
            var profile = GetProfile();
            var result = _QAReportService.GetListOfGradingCriteria(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelQAReport(QAReportSearchRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _QAReportService.ExportToExcelQAReport(req, GetProfile()));
        }
    }
}