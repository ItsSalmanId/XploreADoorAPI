using FOX.DataModels.Models.AdjustmentApproval;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using System.Collections.Generic;

namespace FOX.BusinessOperations.AdjustmentApprovalService
{
    public interface IAdjusmentApprovalServices
    {
        List<AdjustmentAmount> GetAdjustmentAmountsRange(UserProfile profile);
        List<AdjustmentClaimStatus> GetAdjustmentStatuses(UserProfile profile);
        List<PatientAdjustmentDetails> GetAdjustments(AdjustmentsSearchReq searchReq, UserProfile profile);
        ResponseModel GetAdjustmentsForExcel(AdjustmentsSearchReq searchReq, UserProfile profile);        
        List<AdjustmentLog> GetAdjustmentLogs(AdjustmentLogSearchReq adjustmentDetailsId, UserProfile profile);
        DDValues GetDDValues(UserProfile profile);
        StatusCounter GetStatusCounters(StatusCounterSearch statusCounterSearch, UserProfile profile);
        List<UsersForDropdown> GetUsersForDD(UserProfile profile);
        ResponseModel SaveAdjustment(PatientAdjustmentDetails adjustmentToSave, UserProfile profile);
        ResponseModel DeleteAdjustment(AdjustmentToDelete adjustmentToDelete, UserProfile profile);
        ResponseModel AssignUser(UserAssignmentModel userAssignmentDetails, UserProfile profile);
        ResponseModel ExportAdjustmentsToExcel(List<PatientAdjustmentDetails> obj, UserProfile profile);
        ResponseModel ExportAdjustmentLogsToExcel(List<AdjustmentLog> obj, UserProfile profile);
        ResponseModel SignRequest(SignRequestDetails signReqDetails, UserProfile profile);
        ResponseModel DownloadAdjustment(DownloadAdjustmentModel dowloadDetails, UserProfile profile);
    }
}
