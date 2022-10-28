using FOX.BusinessOperations.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.Patient;
using FoxRehabilitationAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers.FrictionlessReferral.SupportStaff
{
    [ExceptionHandlingFilter]
    [AllowAnonymous]
    public class SupportStaffController : BaseApiController
    {
        #region PROPERTIES
        private readonly ISupportStaffService _supportStaffService;
        #endregion
        #region CONSTRUCTOR
        public SupportStaffController(ISupportStaffService supportStaffService)
        {
            _supportStaffService = supportStaffService;
        }
        #endregion
        #region FUNCTIONS
        [HttpGet]
        public HttpResponseMessage GetInsurancePayer()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _supportStaffService.GetInsurancePayers());
        }
        [HttpPost]
        public HttpResponseMessage SendPatientInviteOnEmail(PatientDetail patientDetails)
        {
            if(patientDetails != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _supportStaffService.SendInviteToPatientPortal(patientDetails));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient Details is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage SendInviteOnMobile(PatientDetail patientDetails)
        {
            if (patientDetails != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _supportStaffService.SendInviteOnMobile(patientDetails));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient Details is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetOrderingReferralSourceInformation(ProviderReferralSourceRequest orderingReferralSourceInfo)
        {
            if(orderingReferralSourceInfo != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _supportStaffService.GetOrderingReferralSource(orderingReferralSourceInfo));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient Details is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetFrictionlessReferralInformation(long referralId)
        {
            if(referralId != 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _supportStaffService.GetFrictionLessReferralDetails(referralId));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage SaveFrictionlessReferralInformation(FrictionLessReferral frictionLessReferralRquest)
        {
            if(frictionLessReferralRquest != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _supportStaffService.SaveFrictionLessReferralDetails(frictionLessReferralRquest));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral Model is Empty");
            }
        }
        #endregion
    }
}
