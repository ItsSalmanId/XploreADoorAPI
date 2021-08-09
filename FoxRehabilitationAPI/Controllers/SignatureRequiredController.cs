using FOX.DataModels.Models.SignatureRequired;
using FoxRehabilitationAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using FOX.BusinessOperations.SignatureRequiredServices;
using System.Web.Http;
using FOX.DataModels.Models.RequestForOrder;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class SignatureRequiredController : BaseApiController
    {
        private readonly ISignatureRequiredService _SignatureService;
        public SignatureRequiredController(ISignatureRequiredService signatureRequired)
        {
            _SignatureService = signatureRequired;
        }

        [HttpPost]
        public HttpResponseMessage GetReferralList(SignatureRequiredRequest req)
        {
            var user = _SignatureService.GetReferralList(req, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, user);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetWorkDetailsUniqueId(ReqsignatureModel reqsignModel)
        {
            var data = _SignatureService.GetWorkDetailsUniqueId(reqsignModel, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

    }
}