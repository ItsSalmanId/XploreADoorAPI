using FOX.BusinessOperations.QualityAssuranceService.QADashboardService;
using FOX.DataModels.Models.QualityAsuranceModel;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.QualityAssurance
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class QADashboardController : BaseApiController
    {
        private readonly IQADashboardService _IQADashboardService;
        public QADashboardController(IQADashboardService qADashboardService)
        {
            _IQADashboardService = qADashboardService;
        }
        [HttpGet]
        public HttpResponseMessage GetEmployeelist(string callScanrioID)
        {
            if (callScanrioID != null)
            {
                var result = _IQADashboardService.GetEmployeelist(callScanrioID, GetProfile());
                var response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Call Scanrio ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetDashboardData(QADashboardSearch qADashboardSearch)
        {
            if (qADashboardSearch != null)
            {
                var result = _IQADashboardService.GetDashboardData(qADashboardSearch, GetProfile());
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "QADashboardSearch Model is Null");
            }
        }
    }
}
