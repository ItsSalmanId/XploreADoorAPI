using FOX.BusinessOperations.OriginalQueueService;
using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.OriginalQueueModel;
using FoxRehabilitationAPI.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class OriginalQueueController : BaseApiController
    {
        private readonly IOriginalQueueService _OriginalService;

        public OriginalQueueController(IOriginalQueueService originalService)
        {
            _OriginalService = originalService;
        }
        [HttpPost]
        public HttpResponseMessage GetOriginalQueue(OriginalQueueRequest req)
        {
            var users = _OriginalService.GetOriginalQueue(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelOrignalQueue(OriginalQueueRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _OriginalService.ExportToExcelOrignalQueue(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage SaveNewQueue(OriginalQueue req)
        {
            _OriginalService.SaveQueueData(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, true);
            return response;
        }
       
        //public HttpResponseMessage ExportOriginalQueue(OriginalQueueRequest obj)
        //{
        //    var test= Request.CreateResponse(HttpStatusCode.OK, _OriginalService.ExportOriginalQueue(obj, GetProfile()));
        //    // return Request.CreateResponse(HttpStatusCode.OK, _OriginalService.ExportOriginalQueue(obj, GetProfile()));
        //    return test;
        //}

        [HttpPost]
        public HttpResponseMessage ExportOriginalQueue(OriginalQueueRequest req)
        {
            var record = _OriginalService.GetOriginalQueue(req, GetProfile()).OriginalQueueList;
            var fileName = "OriginalQueue_Export_" + DateTime.Now.Ticks + ".csv";
            var data = record.ExportCSV(fileName);
            var response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetWorkDetails(long workId)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _OriginalService.GetWorkDetails(workId, profile.PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage GetWorkDetailsUniqueId(ReqOriginalQueueModel reqOriginalQueueModel)
        {
            var data = _OriginalService.GetWorkDetailsUniqueId(reqOriginalQueueModel, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

    }
}
