using FOX.BusinessOperations.RequestForOrder;
using FOX.DataModels.Models.RequestForOrder;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class RequestForOrderController : BaseApiController
    {
        private readonly IRequestForOrderService _RequestForOrderService;
        public RequestForOrderController(IRequestForOrderService requestForOrderServiceObj)
        {
            _RequestForOrderService = requestForOrderServiceObj;
        }

        [HttpGet]
        public HttpResponseMessage GeneratingWorkOrder()
        {
            var profile = GetProfile();
            var responseGeneratingWorkOrder = _RequestForOrderService.GeneratingWorkOrder(profile.PracticeCode, profile.UserName, profile.UserEmailAddress, profile.userID, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, responseGeneratingWorkOrder);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SendEmail(RequestSendEmailModel requestSendEmailModel)
        {
            var responseModel = _RequestForOrderService.SendEmail(requestSendEmailModel, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SendFAX(RequestSendFAXModel requestSendFAXModel)
        {
            var responseModel = _RequestForOrderService.SendFAX(requestSendFAXModel, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage DeleteWorkOrder(RequestDeleteWorkOrder requestDeleteWorkOrder)
        {
            var responseModel = _RequestForOrderService.DeleteWorkOrder(requestDeleteWorkOrder, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage DownloadPdf(RequestDownloadPdfModel requestDownloadPdfModel)
        {
            var responseModel = _RequestForOrderService.DownloadPdf(requestDownloadPdfModel, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage AddDocument_SignOrder(ReqAddDocument_SignOrder reqAddDocument_SignOrder)
        {
            var responseModel = _RequestForOrderService.AddDocument_SignOrder(reqAddDocument_SignOrder, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetUserReferralSource()
        {
            var profile = GetProfile();
            var responseGeneratingWorkOrder = _RequestForOrderService.GetUserReferralSource(profile.UserEmailAddress, profile.userID);
            var response = Request.CreateResponse(HttpStatusCode.OK, responseGeneratingWorkOrder);
            return response;
        }
    }
}
