using FOX.BusinessOperations.FaxServices;
using FOX.DataEntities.Model.Fax;
using FOX.DataModels.Models.Settings.Practice;
using FoxRehabilitationAPI.Controllers;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AF.WebAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class FaxController : BaseApiController
    {
        private readonly IFaxService _IFaxService;

        public FaxController(IFaxService IFaxService)
        {
            _IFaxService = IFaxService;
        }

        [HttpPost]
        public HttpResponseMessage SendFax(SendFaxRequest objSend)
        {
            var fax = _IFaxService.SendFax(objSend.Recp_Fax, objSend.RecpientName, objSend.CoverLetter, objSend.FileName, objSend.FilePath, objSend.Subject, objSend.isCallFromFax, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpPost]
        public HttpResponseMessage GetSentFax(FaxRequest objReq)
        {
            var fax = _IFaxService.GetSentFax(objReq, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpPost]
        public HttpResponseMessage GetReceivedFax(FaxRequest objReq)
        {

            var fax = _IFaxService.GetReceivedFax(objReq, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpGet]
        public HttpResponseMessage GetSentFaxContent(string faxID)
        {

            var fax = _IFaxService.getSentFaxContent(faxID, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpGet]
        public HttpResponseMessage GetReceivedFaxContent(string faxID)
        {

            var fax = _IFaxService.getReceivedFaxContent(faxID, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpGet]
        public HttpResponseMessage ForwardSentFaxContent(string faxID)
        {

            var fax = _IFaxService.forwardSentFaxContent(faxID, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpGet]
        public HttpResponseMessage ForwardReceivedFaxContent(string faxID)
        {

            var fax = _IFaxService.forwardReceivedFaxContent(faxID, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpPost]
        public HttpResponseMessage DeleteSentFax(string[] faxIdList)
        {
            var fax = _IFaxService.deleteSentFax(faxIdList);
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpPost]
        public HttpResponseMessage DeleteReceivedFax(string[] faxIdList)
        {

            var fax = _IFaxService.deleteReceivedFax(faxIdList);
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpPost]
        public HttpResponseMessage GetMultipleSentFax(string[] faxID)
        {

            var fax = _IFaxService.getMultipleSentFax(faxID, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpPost]
        public HttpResponseMessage GetMultipleReceivedFax(string[] faxID)
        {

            var fax = _IFaxService.getMultipleReceivedFax(faxID, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpPost]
        public HttpResponseMessage SaveFaxSetting(InterFaxDetail objFax)
        {

            var fax = _IFaxService.saveFaxSetting(objFax, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

        [HttpGet]
        public HttpResponseMessage GetFaxSetting()
        {
            var fax = _IFaxService.GetFaxUserDetails(GetProfile().PracticeCode);
            return Request.CreateResponse(HttpStatusCode.OK, fax);
        }

    }
}
