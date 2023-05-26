using FOX.BusinessOperations.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.IndexInfo;
using FoxRehabilitationAPI.Filters;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.CommonModel;
using System.Threading.Tasks;
using System.Web;
using FOX.DataModels;

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
            EntityHelper.isTalkRehab = false;
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
        [HttpPost]
        public HttpResponseMessage SubmitReferral(SubmitReferralModel submitReferralModel)
        {
            var responseModel = _supportStaffService.SubmitReferral(submitReferralModel);
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage DeleteWorkOrder(RequestDeleteWorkOrder requestDeleteWorkOrder)
        {
            var responseModel = _supportStaffService.DeleteWorkOrder(requestDeleteWorkOrder);
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage GenerateQRCode([FromBody] QRCodeModel obj)
        {
            obj.SignPath = "";
            obj.AbsolutePath = System.Web.HttpContext.Current.Server.MapPath("~/" + AppConfiguration.QRCodeTempPath);
            return Request.CreateResponse(HttpStatusCode.OK, _supportStaffService.GenerateQRCode(obj));
        }
        [HttpPost]
        public Task<HttpResponseMessage> UploadFrictionlessFilesAPI()
        {
            RequestUploadFilesModel requestUploadFilesAPIModel = new RequestUploadFilesModel()
            {
                AllowedFileExtensions = new List<string> { ".pdf", ".docx", ".jpg", ".jpeg", ".png", ".tif", ".gif", ".txt", ".tiff", ".bmp" },
                UploadFilesPath = HttpContext.Current.Server.MapPath("~/" + FOX.BusinessOperations.CommonServices.AppConfiguration.RequestForOrderUploadImages),
                Files = HttpContext.Current.Request.Files
            };
            var uploadFiles = _supportStaffService.UploadFiles(requestUploadFilesAPIModel);
            var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            return Task.FromResult(response);
        }
        [HttpPost]
        public HttpResponseMessage CheckServiceAvailability(ServiceAvailability serviceAvailability)
        {
            if(serviceAvailability != null)
            {
                var responseModel = _supportStaffService.CheckServiceAvailability(serviceAvailability);
                var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral Model is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage SaveExternalUserInfo(ExternalUserInfo externalUserInfo)
       {
            if (externalUserInfo != null)
             {
                var responseModel = _supportStaffService.SaveExternalUserInfo(externalUserInfo);
                var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral Model is Empty");
            }
        }
        #endregion
    }
}
