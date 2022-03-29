using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.IndexInfoServices;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.ReferralSource;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class IndexInfoController : BaseApiController
    {
        private readonly IIndexInfoService _IndexInfoService;
        public IndexInfoController(IIndexInfoService IndexInfoServ)
        {
            _IndexInfoService = IndexInfoServ;
        }

        [HttpPost]
        public HttpResponseMessage GetPatientInformation(getPatientReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetIndexPatInfo(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetDiagnosisInfo(Index_infoReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetDiagnosisInfo(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetProceduresInfo(Index_infoReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetProceduresInfo(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetDocuments(Index_infoReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetDocuments(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage InsertNotesHistory([FromBody] FOX_TBL_NOTES_HISTORY obj)
        {
            _IndexInfoService.InsertNotesHistory(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "created");
        }
        [HttpPost]
        public HttpResponseMessage GetNotes_History(Index_infoReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetNotes_History(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage InsertDiagnosisInfo([FromBody] FOX_TBL_PATIENT_DIAGNOSIS obj)
        {
            _IndexInfoService.InsertDiagnosisInfo(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "created");
        }
        [HttpPost]
        public HttpResponseMessage InsertProceureInfo([FromBody] FOX_TBL_PATIENT_PROCEDURE obj)
        {
            _IndexInfoService.InsertProceureInfo(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "created");
        }
        [HttpPost]
        public HttpResponseMessage InsertSource_AdditionalInfo([FromBody] OriginalQueue obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.InsertSource_AdditionalInfo(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage UpdateSource_AdditionalInfo([FromBody] FOX.DataModels.Models.OriginalQueueModel.OriginalQueue obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.UpdateSource_AdditionalInfo(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetAllIndexinfo(Index_infoReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetAllIndexinfo(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage updataPatientInfo([FromBody] IndexPatReq obj)
        {
            _IndexInfoService.updataPatientInfo(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "updated");
        }
        [HttpPost]
        public HttpResponseMessage DeleteDiagnosis([FromBody] FOX_TBL_PATIENT_DIAGNOSIS obj)
        {
            _IndexInfoService.DeleteDiagnosis(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
        }
        
        [HttpPost]
        public HttpResponseMessage DeleteProcedures([FromBody] FOX_TBL_PATIENT_PROCEDURE obj)
        {
            _IndexInfoService.DeleteProcedures(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
        }
        [HttpPost]
        public HttpResponseMessage GetSmartDiagnosisInfo(SmartDiagnosisReq obj)
        {
            var diagInfo = _IndexInfoService.GetSmartDiagnosisInfo(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, diagInfo);
        }

        [HttpPost]
        public HttpResponseMessage GetSmartProceduresInfo(SmartProceduresReq obj)
        {
            var procInfo = _IndexInfoService.GetSmartProceduresInfo(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, procInfo);
        }
        [HttpPost]
        public HttpResponseMessage InsertUpdateDocuments([FromBody] FOX_TBL_PATIENT_DOCUMENTS obj)
        {
            _IndexInfoService.InsertUpdateDocuments(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "created");
        }

        [HttpPost]
        public HttpResponseMessage DeleteDocuments([FromBody] FOX_TBL_PATIENT_DOCUMENTS obj)
        {
            _IndexInfoService.DeleteDocuments(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
        }

        [HttpPost]
        public HttpResponseMessage GetSmartLocations(SmartReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSmartLocations(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage InsertUpdateOrderingSource([FromBody] ReferralSource obj)
        {
            _IndexInfoService.InsertUpdateOrderingSource(obj, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        [HttpPost]
        public HttpResponseMessage GetSmartOrderingSource(SmartReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSmartOrderingSource(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSmartRefRegion(SmartReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSmartRefRegion(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetAnalysisRPT(AnalaysisReportReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetAnalysisRPT(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSlotData(SlotAnalysisReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSlotData(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSlotData0_15(SlotAnalysisReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSlotData0_15(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSlotData16_30(SlotAnalysisReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSlotData16_30(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSlotData31_45(SlotAnalysisReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSlotData31_45(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSlotData46_60(SlotAnalysisReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSlotData46_60(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSlotDataGreater_60(SlotAnalysisReq obj)
        {
            var Ref = _IndexInfoService.GetSlotDataGreater_60(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, Ref);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage GetSlotDataGreater_2HR(SlotAnalysisReq obj)
        {
            var Ref = _IndexInfoService.GetSlotDataGreater_2HR(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, Ref);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage Export(AnalaysisReportReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.Export(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GenerateQRCode([FromBody] QRCodeModel obj)
        {
            obj.SignPath = GetProfile().SIGNATURE_PATH;
            obj.AbsolutePath = System.Web.HttpContext.Current.Server.MapPath("~/" + AppConfiguration.QRCodeTempPath);
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GenerateQRCode(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage SendEmailOrFaxToSender([FromBody] EmailFaxToSender obj)
        {
           
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.SendEmailOrFaxToSender(obj, GetProfile(), obj.UNIQUE_ID));
        }

        [HttpGet]
        public HttpResponseMessage GetIndexInfoInitialData(long workId)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetIndexInfoInitialData(workId, profile.PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetSmartPlaceOfServices(string searchText)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetSmartLocations(searchText, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetFacilityIfNoAlreadyExist(long id, string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetFacilityIfNoAlreadyExist(id, GetProfile(), patientAccount));

        }

        [HttpGet]
        public HttpResponseMessage getDocumentTypes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.getDocumentTypes(GetProfile()));

        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelNotes_History(Index_infoReq req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.ExportToExcelNotes_History(req, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetPatientInfoChecklist( long patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetPatientInfoChecklist(patientAccount));

        }

        [HttpPost]
        //[AllowAnonymous]
        public bool updateWorkOrderSignature(SubmitSignatureImageWithData obj)
        {
            return _IndexInfoService.updateWorkOrderSignature(obj, GetProfile());
        }

        [HttpGet]
        public HttpResponseMessage getPreviousEmailInformation(string WORK_ID)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.getPreviousEmailInformation(WORK_ID, profile));
        }

        [HttpGet]
        public HttpResponseMessage setPatientOpenedBy(long patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.setPatientOpenedBy(patientAccount, GetProfile()));

        }
        [HttpGet]
        public HttpResponseMessage ClearPatientOpenedBy(long patientAccount)
        {
             _IndexInfoService.ClearPatientOpenedBy(patientAccount, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "Cleared");

        }
        [HttpPost]
        public HttpResponseMessage GetOCRData(Index_infoReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetOCRData(obj, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetLocationByID(long? loc_id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetLocationByID(loc_id, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage UpdateOCRValue(long? work_id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.UpdateOCRValue(work_id, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage getPatientReferralDetail(long work_id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.getPatientReferralDetail(work_id, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage getReferralSourceAndGroups ()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.getAllReferralSourceAndGroups( GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetPatientBalance(long? patientAccount)
        {
            if (patientAccount != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetPatientBalance(patientAccount));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Patient Account is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetpatientsList(getPatientReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetpatientsList(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetDuplicateReferralDetail(checkDuplicateReferralRequest checkDuplicateReferral)
        {
            if (checkDuplicateReferral != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetDuplicateReferralInformation(checkDuplicateReferral, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Model is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetWorkOrderDocs(string patientAccountStr)
        {
            if (!string.IsNullOrEmpty(patientAccountStr.ToString()))
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetWorkOrderDocs(patientAccountStr, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Model is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage SaveLogMessage(Index_infoReq work_Id)
        {
            if (!string.IsNullOrEmpty(work_Id.ToString()))
            {
                 _IndexInfoService.SaveLogMessage(work_Id, GetProfile());
                return Request.CreateResponse(HttpStatusCode.OK, "Log Saved");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Work ID is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetRegionCoverLetterAttachment(string regionCode)
        {
            if (!string.IsNullOrEmpty(regionCode))
            {
                return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetRegionCoverLetterAttachment(regionCode));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Model is Empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTalkRehabTaskWorkID(long taskId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.GetTalkRehabTaskWorkID(taskId, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage MarkTaskAsComplete(long taskId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _IndexInfoService.MarkTaskAsComplete(taskId, GetProfile()));
        }
    }
}
