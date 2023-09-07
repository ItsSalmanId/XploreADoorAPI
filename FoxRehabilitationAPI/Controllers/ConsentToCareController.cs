using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
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
        [HttpPost]
        public HttpResponseMessage GetInsuranceDetails(FoxTblConsentToCare insuranceDetailsObj)
        {
            if (insuranceDetailsObj != null)
            {

                return Request.CreateResponse(HttpStatusCode.OK, _consentToCareService.GetInsuranceDetails(insuranceDetailsObj, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage DownloadSingleFile(string FilePath)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            //string FilePath = FilePath;
            if (!string.IsNullOrEmpty(FilePath))
            {
                try
                {
                    string fileName = Path.GetFileName(FilePath);
                    string test = Path.GetDirectoryName(FilePath);
                    string fileExtension = Path.GetExtension(FilePath);
                    if (!string.IsNullOrEmpty(fileName) && new[] { ".xml", ".zip", ".jpeg", ".jpg", ".png", ".pdf", ".tiff", ".tif", ".docx", ".doc", ".xls", ".xlsx", ".csv", ".ppt", ".pptx", ".mp3", ".wav", ".txt", ".opus" }.Contains(fileExtension))
                    {
                        if (!Directory.Exists(test))
                            FilePath = HttpContext.Current.Server.MapPath("~/" + FilePath);
                        //FilePath = @"\\\\It-bkp\\d\\IT-126\\" + FilePath;                    
                        if (File.Exists(FilePath))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                                {
                                    byte[] bytes = new byte[file.Length];
                                    file.Read(bytes, 0, (int)file.Length);
                                    ms.Write(bytes, 0, (int)file.Length);
                                    httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                                    httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                                    ms.Close();
                                    return httpResponseMessage;

                                }
                            }
                        }
                    }
                    return this.Request.CreateResponse(HttpStatusCode.OK, "File not found.");
                }
                catch
                {
                    throw;
                    //Helper.CustomExceptionLog(ex);
                    //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, "Path not found.");
        }
    }
}
