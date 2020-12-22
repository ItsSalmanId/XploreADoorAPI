using FOX.BusinessOperations.HrAutoEmail;
using FOX.DataModels.Models.HrAutoEmail;
using FoxRehabilitationAPI.Filters;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class HrAutoEmailController : BaseApiController
    {
        private readonly IHrAutoEmailService _IHrAutoEmailService;
        public HrAutoEmailController(IHrAutoEmailService hrAutoEmailService)
        {
            _IHrAutoEmailService = hrAutoEmailService;
        }
        [HttpPost]
        public HttpResponseMessage AddHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmailConfigure)
        {
            if(hrAutoEmailConfigure != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.AddHrAutoEmailConfigure(hrAutoEmailConfigure, GetProfile()));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetHrAutoEmailNames()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.GetHrAutoEmailConfigureRecords(GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage SaveUploadWorkOrderFiles(List<HrEmailDocumentFileAll> hrEmailDocumentFileAll)
        {
            if(hrEmailDocumentFileAll != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.SaveHrMTBCEMailDocumentFiles(hrEmailDocumentFileAll, GetProfile()));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage UpdateHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmailConfigure)
        {
            if (hrAutoEmailConfigure != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.UpdateHrAutoEmailConfigure(hrAutoEmailConfigure, GetProfile()));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage DeleteHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmailConfigure)
        {
            if (hrAutoEmailConfigure != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.DeleteHrAutoEmailConfigure(hrAutoEmailConfigure, GetProfile()));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetDocumentFileDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.GetMTBCDocumentFileDetails(GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage EnableHrAutoEmailCertificate(HrAutoEmailConfigure hrAutoEmailConfigure)
        {
            if (hrAutoEmailConfigure != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.EnableHrAutoEmailCertificate(hrAutoEmailConfigure, GetProfile()));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetMTBCUnMappedCategory()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.GetMTBCUnMappedCategory(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetHrAutoEmailByID(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IHrAutoEmailService.GetHrAutoEmalById(id, GetProfile()));
        }
    }
}
