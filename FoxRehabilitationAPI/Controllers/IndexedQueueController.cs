using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FOX.BusinessOperations.IndexedQueueService;
using FOX.DataModels.Models.IndexedQueueModel;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class IndexedQueueController : BaseApiController
    {
        private readonly IIndexedQueueServices _indexedQueueServices;

        public IndexedQueueController(IIndexedQueueServices indexedQueueServices)
        {
            _indexedQueueServices = indexedQueueServices;
        }

        [HttpPost]
        public HttpResponseMessage GetIndexedQueue(IndexedQueueRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _indexedQueueServices.GetIndexedQueue(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelIndexedQueue(IndexedQueueRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _indexedQueueServices.ExportToExcelIndexedQueue(req, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetAgentsAndSupervisorsForDropdown(long RoleId)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _indexedQueueServices.GetAgentsAndSupervisorsForDropdown(RoleId, profile.PracticeCode, profile.UserName));
        }

        [HttpPost]
        public HttpResponseMessage ReAssignedMultiple(AssignedQueueMultipleModel model)
        {
            _indexedQueueServices.ReAssignedMultiple(GetProfile(), model.MultipleQueue);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpPost]
        public HttpResponseMessage GetFilePages(IndexedQueueFileRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _indexedQueueServices.GetFilePages(req));
        }

        [HttpPost]
        public HttpResponseMessage SetSplitPages(SetSplitPagesRequestModel req)
        {
            _indexedQueueServices.SetSplitPages(req.filePages, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateWorkTransfer(WorkTransfer workTransfer)
        {
            _indexedQueueServices.AddUpdateWorkTransfer(workTransfer, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "Added");
        }

        //[HttpPost]
        //public HttpResponseMessage Export(IndexedQueueRequest obj)
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _indexedQueueServices.Export(obj, GetProfile()));
        //}
    }
}
