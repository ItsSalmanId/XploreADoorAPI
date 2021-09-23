using FOX.BusinessOperations.CommonServices.UploadFiles;
using FOX.BusinessOperations.ReconciliationService;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Reconciliation;
using FoxRehabilitationAPI.Filters;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class ReconciliationController : BaseApiController
    {
        private readonly IReconciliationService _reconciliationService;
        private readonly IUploadFilesServices _IUploadFilesServices;

        public ReconciliationController(IReconciliationService reconciliationServices, IUploadFilesServices uploadFilesServices)
        {
            _reconciliationService = reconciliationServices;
            _IUploadFilesServices = uploadFilesServices;
        }

        [HttpGet]
        public HttpResponseMessage GetReconciliationStatuses()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetReconciliationStatuses(GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetDepositTypes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetDepositTypes(GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetReconsiliationCategoryDepositTypes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetReconsiliationCategoryDepositTypes(GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetReconciliationCategories()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetReconciliationCategories(GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetReconciliationInsurances([FromBody] ReconciliationCPSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetReconciliationInsurances(searchReq, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetReconciliationCheckNos([FromBody] ReconciliationCPSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetReconciliationCheckNos(searchReq, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetReconciliationAmounts([FromBody] ReconciliationCPSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetAmounts(searchReq, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetReconciliationAmountsPosted([FromBody] ReconciliationCPSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetPostedAmounts(searchReq, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetReconciliationAmountsUnPosted([FromBody] ReconciliationCPSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetNotPostedAmounts(searchReq, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetDDValues()
        {
            var getProfile = GetProfile();
            var getUserRight = getProfile.ApplicationUserRoles.Find(x => x.RIGHT_NAME.ToLower() == "view reconciliation cp");
            if (getUserRight != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetDDValues(GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Access Denied");
            }
        }

        [HttpPost]
        public HttpResponseMessage GetReconciliationsCP([FromBody] ReconciliationCPSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetReconciliationsCP(searchReq, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage SaveReconciliationCP([FromBody] ReconciliationCP reconciliationToSave)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.SaveReconciliationCP(reconciliationToSave, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage EditReconciliationCP([FromBody] ReconciliationCP reconciliationToSave)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.EditReconciliationCP(reconciliationToSave, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage SaveAutoReconciliationCP([FromBody] ReconciliationCP autoreconciliationToSave)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.SaveAutoReconciliationCP(autoreconciliationToSave, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage SaveManualReconciliationCP([FromBody] ReconciliationCP manualreconciliationToSave)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.SaveManualReconciliationCP(manualreconciliationToSave, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage UpdateAutoReconciliationCP([FromBody] ReconciliationCP autoReconciliationToUpdate)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.UpdateAutoReconciliationCP(autoReconciliationToUpdate, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetReconciliationLogs([FromBody] ReconciliationCPLogSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetReconciliationLogs(searchReq, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage DeleteReconsiliationLedger(ReconciliationCPToDelete _reconsiliation)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.DeleteReconsiliationLedger(_reconsiliation.RECONCILIATION_CP_ID, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage DeleteReconciliationCP([FromBody] ReconciliationCPToDelete reconciliationToDelete)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.DeleteReconciliationCP(reconciliationToDelete, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage AssignUserCP([FromBody] UserAssignmentModel userAssignmentDetails)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.AssignUserCP(userAssignmentDetails, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage ExportReconciliationsToExcel([FromBody] ReconciliationCPSearchReq searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.ExportReconciliationsToExcel(searchReq, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage ExportReconciliationCPLogsToExcel([FromBody] List<ReconciliationCPLogs> obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.ExportReconciliationCPLogsToExcel(obj, GetProfile()));
        }


        [HttpPost]
        public HttpResponseMessage GetReconciliationFiles([FromBody] ReconciliationFilesSearchReq reconciliationDetails)
        {
            //return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetReconciliationFiles(reconciliationDetails, GetProfile()));
            return null;
        }

        [HttpPost]
        public HttpResponseMessage DownloadLedger([FromBody] ReconciliationFilesSearchReq reconciliationDetails)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.DownloadLedger(reconciliationDetails, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage AddNewDDValue([FromBody] ReconciliationDDValue reconciliationDDValue)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.AddNewDDValue(reconciliationDDValue, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage AttachLedger([FromBody] LedgerModel ledgerDetails)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.AttachLedger(ledgerDetails, GetProfile()));
        }


        //[HttpPost]
        //public HttpResponseMessage UploadReconsiliationLedger([FromBody] LedgerModel ledgerDetails)
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.AttachLedger(ledgerDetails, GetProfile()));
        //}
        [HttpPost]
        public async Task<ResponseLedgerUploadFilesModel> UploadReconsiliationLedger()
        {
            var reconsiliationLedgerPath = FOX.BusinessOperations.CommonServices.AppConfiguration.ReconciliationOriginalFilesDirectory;
            var absoluteAttachmentPath = HttpContext.Current.Server.MapPath("~/" + reconsiliationLedgerPath);

            RequestUploadFilesModel requestUploadFilesModel = new RequestUploadFilesModel()
            {
                AllowedFileExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".tif", ".tiff", ".pdf" },
                //Files = ledgerDetails.file,
                Files = HttpContext.Current.Request.Files,
                UploadFilesPath = absoluteAttachmentPath
            };
            long id = Convert.ToInt64(HttpContext.Current.Request.Params["reconsiliationId"]);
            var uploadFiles = _IUploadFilesServices.UploadReconsiliationLedger(requestUploadFilesModel, id.ToString());
            if (uploadFiles.Success)
            {
                //uploadFiles.FilePath = reconsiliationLedgerPath + @"\" 

                //uploadFiles.FilePath = $@"{reconsiliationLedgerPath}\{uploadFiles.FilePath}";

                LedgerModel ledgerDetails = new LedgerModel()
                {
                    RECONCILIATION_CP_ID = id,
                    AbsolutePath = uploadFiles.FilePath,
                    FILE_NAME = uploadFiles.OrignalFileName
                };

                var response = Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.AttachLedger(ledgerDetails, GetProfile()));

            }
            return uploadFiles;
            //var response = Request.CreateResponse(HttpStatusCode.OK, uploadFiles);
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            //    LedgerModel lgModel = new LedgerModel()
            //    {
            //        FILE_NAME = requestUploadFilesModel.OrignalFileName,
            //        BASE_64_DOCUMENT = null,
            //        RECONCILIATION_CP_ID = 0,
            //        AbsolutePath = requestUploadFilesModel.UploadFilesPath + "~/" + requestUploadFilesModel.Files[0]?.FileName
            //    };
            //}
            //return response;
        }

        //[HttpPost]
        //public HttpResponseMessage UploadLedger([FromBody] ReconciliationDDValue reconciliationDDValue)
        //{


        //    return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.AttachReconsiliationLedger(reconciliationDDValue, GetProfile()));
        //}

        [HttpGet]
        public HttpResponseMessage InsertReconsiliationDataToDB(string filePath)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.ReadExcel(filePath, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetLastUploadFileStatus()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetLastUploadFileStatus(GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetSoftReconsilitionPayment(SOFT_RECONCILIATION_SERACH_REQUEST request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetSoftReconsilitionPayment(request, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetWebsoftPayment(SOFT_RECONCILIATION_SERACH_REQUEST softRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetWebsoftPayment(softRequest, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage InsertHrAutoEmailnDataToDB(string filePath)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.ReadExcelForHrEmails(filePath, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetLastUploadFileStatusForHrAutoEmailsss(string dataTypeName)
        {
            if (dataTypeName != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetLastUploadFileStatusForHrAutoEmails(GetProfile(), dataTypeName));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Data Type is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetMTBCDistinctCategoryName()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetMTBCDistinctCategoryName(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetLastUploadDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reconciliationService.GetLastFileUploadDetails());
        }
    }
}
