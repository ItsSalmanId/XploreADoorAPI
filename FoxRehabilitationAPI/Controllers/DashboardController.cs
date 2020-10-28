using FOX.BusinessOperations.DashboardServices;
using FoxRehabilitationAPI.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public HttpResponseMessage GetDashboardGetTotal()
        {
            var profile = GetProfile();
            var result = _dashboardService.GetDashboardGetTotal(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetNoOfRecordBytime(string dateFrom, string dateTo, int hourFrom, int hourTo)
        {
            var profile = GetProfile();
            var result = _dashboardService.GetNoOfRecordBytime(profile.PracticeCode, dateFrom, dateTo, hourFrom, hourTo);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetDashboardTrend(int value, string dateFromUser, string dateToUser)
        {
            var profile = GetProfile();

            var result = _dashboardService.GetDashboardTrend(value, profile.PracticeCode, dateFromUser, dateToUser);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetDashboardData(int value, int hourFrom)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _dashboardService.GetDashboardData(value, hourFrom, profile.PracticeCode));
        }

    }
}
