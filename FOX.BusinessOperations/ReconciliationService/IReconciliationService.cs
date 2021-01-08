using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Reconciliation;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.ReconciliationService
{
    public interface IReconciliationService
    {
        List<ReconciliationStatus> GetReconciliationStatuses(UserProfile profile);
        List<ReconciliationDepositType> GetDepositTypes(UserProfile profile);
        List<ReconciliationCategory> GetReconciliationCategories(UserProfile profile);
        List<ReconciliationCP> GetReconciliationInsurances(ReconciliationCPSearchReq searchReq, UserProfile profile);
        List<CheckNoSelectionModel> GetReconciliationCheckNos(ReconciliationCPSearchReq searchReq, UserProfile profile);
        List<AmountSelectionModel> GetAmounts(ReconciliationCPSearchReq searchReq, UserProfile profile);
        List<AmountPostedSelectionModel> GetPostedAmounts(ReconciliationCPSearchReq searchReq, UserProfile profile);
        List<AmountNotPostedSelectionModel> GetNotPostedAmounts(ReconciliationCPSearchReq searchReq, UserProfile profile);
        List<ReconciliationCP> GetReconciliationsCP(ReconciliationCPSearchReq searchReq, UserProfile profile);
        List<ReconciliationCPLogs> GetReconciliationLogs(ReconciliationCPLogSearchReq searchReq, UserProfile profile);
        DDValues GetDDValues(UserProfile profile);
        ResponseModel SaveReconciliationCP(ReconciliationCP reconciliationToSave, UserProfile profile);
        ResponseModel EditReconciliationCP(ReconciliationCP reconciliationToSave, UserProfile profile);
        List<ReconciliationCP> SaveAutoReconciliationCP(ReconciliationCP autoreconciliationToSave, UserProfile profile);
        List<ReconciliationCP> SaveManualReconciliationCP(ReconciliationCP manualreconciliationToSave, UserProfile profile);
        List<ReconciliationCP> UpdateAutoReconciliationCP(ReconciliationCP autoReconciliationToUpdate, UserProfile profile);
        ResponseModel DeleteReconciliationCP(ReconciliationCPToDelete reconciliationToDelete, UserProfile profile);

        ResponseModel DeleteReconsiliationLedger(long reconsiliationId, UserProfile profile);

        ResponseModel AssignUserCP(UserAssignmentModel userAssignmentDetails, UserProfile profile);
        ResponseModel ExportReconciliationsToExcel(ReconciliationCPSearchReq searchReq, UserProfile profile);
        ResponseModel ExportReconciliationCPLogsToExcel(List<ReconciliationCPLogs> obj, UserProfile profile);
        ResponseModel AttachLedger(LedgerModel ledgerDetails, UserProfile profile);
        // List<FilePath> GetReconciliationFiles(ReconciliationFilesSearchReq reconciliationDetails, UserProfile profile);
        string DownloadLedger(ReconciliationFilesSearchReq reconciliationDetails, UserProfile profile);
        ReconciliationDDValueResponse AddNewDDValue(ReconciliationDDValue reconciliationDDValue, UserProfile profile);
        ReconsiliationCategoryDepositType GetReconsiliationCategoryDepositTypes(UserProfile profile);
        ReconciliationUploadResponse ReadExcel(string fileName, UserProfile profile);
        FOX_TBL_RECONCILIATION_UPLOAD_LOG GetLastUploadFileStatus(UserProfile profile);
        List<SOFT_RECONCILIATION_PAYMENT> GetSoftReconsilitionPayment(SOFT_RECONCILIATION_SERACH_REQUEST obj, UserProfile profile);
        List<SOFT_RECONCILIATION_PAYMENT> GetWebsoftPayment(SOFT_RECONCILIATION_SERACH_REQUEST softRequest, UserProfile profile);
        HRAutoEmailsUploadResponse ReadExcelForHrEmails(string fileName, UserProfile profile);
        MTBC_Credentials_Fox_Automation GetLastUploadFileStatusForHrAutoEmails(UserProfile profile);
    }
}
