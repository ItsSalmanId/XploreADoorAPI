﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using FOX.BusinessOperations.ConsentToCareService;
using FoxRehabilitationAPI.Filters;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;

namespace FoxRehabilitationAPI.Controllers
{
    [ExceptionHandlingFilter]
    [AllowAnonymous]
    public class ConsentToCareController : BaseApiController
    {
        private readonly IConsentToCareService _consentToCareService;
        public ConsentToCareController(IConsentToCareService consentToCareService)
        {
            _consentToCareService = consentToCareService;
        }
        [HttpPost]
        public HttpResponseMessage AddUpdateConsentToCare(FoxTblConsentToCare consentToCareObj)
        {
            if (consentToCareObj != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.AddUpdateConsentToCare(consentToCareObj, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Consent To Care model is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetConsentToCare(FoxTblConsentToCare consentToCareObj)
        {
            if (consentToCareObj != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.GetConsentToCare(consentToCareObj, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Consent To Care model is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GeneratePdfForConcentToCare(FoxTblConsentToCare consentToCareObj)
        {
            var profile = GetProfile();
            string pathtoDownload = _consentToCareService.GeneratePdfForConcentToCare(consentToCareObj, profile.PracticeDocumentDirectory);
            var response = Request.CreateResponse(HttpStatusCode.OK, pathtoDownload);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage DecryptionUrl(FoxTblConsentToCare consentToCareObj)
        {
            
            if (consentToCareObj != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.DecryptionUrl(consentToCareObj));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage DobVerificationInvalidAttempt(AddInvalidAttemptRequest addInvalidAttemptRequestObj)
        {
            if (addInvalidAttemptRequestObj != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.DobVerificationInvalidAttempt(addInvalidAttemptRequestObj, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }

        [HttpPost]
        public HttpResponseMessage SubmitConsentToCare(FoxTblConsentToCare consentToCareObj)
        {
            if (consentToCareObj != null)
            {

                return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.SubmitConsentToCare(consentToCareObj, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetConsentToCareImagePath(FoxTblConsentToCare consentToCareObj)
        {
            if (consentToCareObj != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.GetConsentToCareImagePath(consentToCareObj, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Consent To Care model is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage CommentOnCallTap(FoxTblConsentToCare consentToCareObj)
        {
            if (consentToCareObj != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.CommentOnCallTap(consentToCareObj, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Consent To Care model is Empty");
            }
        }
    }
}
