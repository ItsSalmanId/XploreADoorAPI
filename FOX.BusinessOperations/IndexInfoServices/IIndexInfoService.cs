using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.ReferralSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.IndexInfoServices
{
    public interface IIndexInfoService
    {
        //void InsertSourceSender(FOX_TBL_SENDER obj, UserProfile profile);
        void InsertNotesHistory(FOX_TBL_NOTES_HISTORY obj, UserProfile profile);
        void InsertDiagnosisInfo(FOX_TBL_PATIENT_DIAGNOSIS obj, UserProfile profile);
        void InsertProceureInfo(FOX_TBL_PATIENT_PROCEDURE obj, UserProfile profile);
        OriginalQueue InsertSource_AdditionalInfo(DataModels.Models.OriginalQueueModel.OriginalQueue obj, UserProfile profile);
        OriginalQueue UpdateSource_AdditionalInfo(DataModels.Models.OriginalQueueModel.OriginalQueue obj, UserProfile profile);
        void updataPatientInfo(IndexPatReq obj, UserProfile profile);
        void DeleteDiagnosis(FOX_TBL_PATIENT_DIAGNOSIS obj, UserProfile profile);
        void DeleteProcedures(FOX_TBL_PATIENT_PROCEDURE obj, UserProfile profile);
        void InsertUpdateDocuments(FOX_TBL_PATIENT_DOCUMENTS obj, UserProfile profile);
        void DeleteDocuments(FOX_TBL_PATIENT_DOCUMENTS obj, UserProfile profile);
        ReferralSource InsertUpdateOrderingSource(ReferralSource obj, UserProfile profile);
        IndexPatRes GetIndexPatInfo(getPatientReq req, UserProfile Profile);
        //List<SourceSenderRes> GetSourceSener(UserProfile Profile);
        List<FOX_TBL_PATIENT_PROCEDURE> GetProceduresInfo(Index_infoReq obj, UserProfile Profile);
        List<FOX_TBL_PATIENT_DIAGNOSIS> GetDiagnosisInfo(Index_infoReq obj, UserProfile Profile);
        List<FOX_TBL_NOTES_HISTORY> GetNotes_History(Index_infoReq obj, UserProfile Profile);
        GETtAll_IndexifoRes GetAllIndexinfo(Index_infoReq obj, UserProfile Profile);
        List<GetSmartDiagnosisRes> GetSmartDiagnosisInfo(SmartDiagnosisReq obj, UserProfile profile);
        List<GetSmartProceduresRes> GetSmartProceduresInfo(SmartProceduresReq obj, UserProfile profile);
        List<FOX_TBL_PATIENT_DOCUMENTS> GetDocuments(Index_infoReq obj, UserProfile Profile);
        List<SmartLocationRes> GetSmartLocations(SmartReq obj, UserProfile profile);
        List<SmartLocationRes> GetSmartLocations(string searchText, UserProfile profile);
        List<SmartOrderSource> GetSmartOrderingSource(SmartReq obj, UserProfile profile);
        string GetRegionCoverLetterAttachment(string regionCode);
        List<SmartRefRegion> GetSmartRefRegion(SmartReq obj, UserProfile profile);
        List<AnalaysisReportRes> GetAnalysisRPT(AnalaysisReportReq obj, UserProfile Profile);
        List<SlotAnalysisRes> GetSlotData(SlotAnalysisReq obj, UserProfile Profile);
        List<SlotAnalysisRes> GetSlotData0_15(SlotAnalysisReq obj, UserProfile Profile);
        List<SlotAnalysisRes> GetSlotData16_30(SlotAnalysisReq obj, UserProfile Profile);
        List<SlotAnalysisRes> GetSlotData31_45(SlotAnalysisReq obj, UserProfile Profile);
        List<SlotAnalysisRes> GetSlotData46_60(SlotAnalysisReq obj, UserProfile Profile);
        List<SlotAnalysisRes> GetSlotDataGreater_60(SlotAnalysisReq obj, UserProfile Profile);
        List<SlotAnalysisRes> GetSlotDataGreater_2HR(SlotAnalysisReq obj, UserProfile Profile);
        string Export(AnalaysisReportReq obj, UserProfile profile);
        QRCodeModel GenerateQRCode(QRCodeModel obj, UserProfile profile);
        bool SendEmailOrFaxToSender(EmailFaxToSender model, UserProfile profile, string WORK_ID);

        IndexInfoInitialData GetIndexInfoInitialData(long workId, long practiceCode);
        FacilityLocation GetFacilityIfNoAlreadyExist(long id, UserProfile userProfile, string patientAccount);
        List<FoxDocumentType> getDocumentTypes(UserProfile userProfile);
        string ExportToExcelNotes_History(Index_infoReq obj, UserProfile profile);
        PatientInfoChecklist GetPatientInfoChecklist(long patientAccount);
        bool updateWorkOrderSignature(SubmitSignatureImageWithData obj, UserProfile profile);
        List<PreviousEmailInfo> getPreviousEmailInformation(string WORK_ID, UserProfile profile);
        string setPatientOpenedBy(long patientAccount, UserProfile profile);
        void ClearPatientOpenedBy(long patientAccount, UserProfile profile);
        DsFoxOcr GetOCRData(Index_infoReq obj, UserProfile profile);
        FacilityLocation GetLocationByID(long? loc_id, UserProfile profile);
        bool UpdateOCRValue(long? work_id, UserProfile profile);
        ReferralPatientInfo getPatientReferralDetail(long work_id, UserProfile profile);
        ReferralSourceAndGroups getAllReferralSourceAndGroups(UserProfile profile);
        pendingBalanceAmount GetPatientBalance(long? PATIENT_ACCOUNT);
        List<PatientListResponse> GetpatientsList(getPatientReq req, UserProfile Profile);
        List<DuplicateReferralInfo> GetDuplicateReferralInformation(checkDuplicateReferralRequest checkDuplicateReferral, UserProfile userProfile);
        List<WorkOrderDocs> GetWorkOrderDocs(string patientAccountStr, UserProfile userProfile);
        void SaveLogMessage(Index_infoReq workId, UserProfile userProfile);
        long GetTalkRehabTaskWorkID(long taskId, UserProfile profile);
        long MarkTaskAsComplete(long taskId, UserProfile profile);
    }
}