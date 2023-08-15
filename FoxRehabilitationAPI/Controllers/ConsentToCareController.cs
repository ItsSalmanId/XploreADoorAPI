using System.Net;
using System.Net.Http;
using System.Web.Http;
using FOX.BusinessOperations.ConsentToCareService;
using FOX.BusinessOperations.SurveyAutomationService;
using FoxRehabilitationAPI.Filters;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

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
        //[HttpPost]
        //public HttpResponseMessage GetPatientDetails(SurveyAutomation objSurveyAutomation)
        //{
        //    if (objSurveyAutomation != null)
        //    {
        //        //return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.GetPatientDetails(objSurveyAutomation));
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient survey model is empty");
        //    }
        //}
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
        //[HttpPost]
        //public HttpResponseMessage GetConsentToCareDocumentsInfo(FoxTblConsentToCare consentToCareObj)
        //{
        //    if (consentToCareObj != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.GetConsentToCareDocumentsInfo(consentToCareObj, GetProfile()));
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "Consent To Care model is Empty");
        //    }
        //}
    }
}
