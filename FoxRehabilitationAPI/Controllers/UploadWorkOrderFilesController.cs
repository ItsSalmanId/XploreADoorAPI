using FOX.BusinessOperations.IndexInfoServices.UploadWorkOrderFiles;
using FOX.DataModels.Models.UploadWorkOrderFiles;
using FoxRehabilitationAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class UploadWorkOrderFilesController : BaseApiController
    {
        private readonly IUploadWorkOrderFilesService _IUploadWorkOrderFilesService;
        public UploadWorkOrderFilesController(IUploadWorkOrderFilesService uploadWorkOrderFilesService)
        {
            _IUploadWorkOrderFilesService = uploadWorkOrderFilesService;
        }

        [HttpPost]
        public HttpResponseMessage SaveUploadWorkOrderFiles(ReqSaveUploadWorkOrderFiles reqSaveUploadWorkOrderFiles)
        {
            var responseModel = _IUploadWorkOrderFilesService.SaveUploadWorkOrderFiles(reqSaveUploadWorkOrderFiles, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage SaveUploadAdditionalWorkOrderFiles(ReqSaveUploadWorkOrderFiles reqSaveUploadWorkOrderFiles)
        {
            var responseModel = _IUploadWorkOrderFilesService.saveUploadAdditionalWorkOrderFiles(reqSaveUploadWorkOrderFiles, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }
    }
}
