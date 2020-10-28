using System.Net;
using System.Net.Http;
using System.Web.Http;
using FOX.DataModels.Models.RequestForOrder.UploadOrderImages;
using FOX.BusinessOperations.RequestForOrder.UploadOrderImages;
using FoxRehabilitationAPI.Filters;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class UploadOrderImagesController : BaseApiController
    {
        private readonly IUploadOrderImagesService _UploadOrderImagesService;
        public UploadOrderImagesController(IUploadOrderImagesService uploadOrderImagesService)
        {
            _UploadOrderImagesService = uploadOrderImagesService;
        }

        [HttpGet]
        public HttpResponseMessage GetSourceData()
        {
            var profile = GetProfile();
            var responseModel = _UploadOrderImagesService.GetSourceData(profile.UserEmailAddress, profile.userID.ToString(), profile.PracticeCode, profile.UserName);
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SubmitUploadOrderImages(ReqSubmitUploadOrderImagesModel reqSubmitUploadOrderImagesModel)
        {
            var responseModel = _UploadOrderImagesService.SubmitUploadOrderImages(reqSubmitUploadOrderImagesModel, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
            return response;
        }
    }
}
