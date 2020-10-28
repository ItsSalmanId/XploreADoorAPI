using FOX.BusinessOperations.CompleteQueueService;
using FOX.DataModels.Models.CompleteQueueModel;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class CompleteQueueController : BaseApiController
    {
        private readonly ICompleteQueueService _CompleteQueuedService;
        public CompleteQueueController(ICompleteQueueService completeService)
        {
            _CompleteQueuedService = completeService;
        }

        [HttpPost]
        public HttpResponseMessage GetCompleteQueueLIst(SearchRequestCompletedQueue req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CompleteQueuedService.GetCompleteQueue(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelCompleteQueu(SearchRequestCompletedQueue req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CompleteQueuedService.ExportToExcelCompleteQueu(req, GetProfile()));
        }
    }
}
