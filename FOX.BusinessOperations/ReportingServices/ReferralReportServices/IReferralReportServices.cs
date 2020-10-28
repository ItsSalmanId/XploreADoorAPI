
using FOX.DataModels.Models.Reporting;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.ReportingServices.ReferralReportServices
{
    public interface IReferralReportServices
    {
        List<ReferralReport> GetReferralReportList(ReferralReportRequest referralReportRequest, UserProfile profile);
        List<ApplicatinUserName> GetApplicatinUserNameList(string name, long practiceCode);
        List<TaskListResponse> GetTaskReportList(TaskListRequest obj, UserProfile profile);
        string Export(TaskListRequest obj, UserProfile profile);
        string ExportToExcelReferralReport(ReferralReportRequest referralReportRequest, UserProfile profile);
        List<HighBalanceReportRes> getHighBalanceReportList(HighBalanceReportReq obj, UserProfile profile);
        string ExportToExcelHighBalanceReport(HighBalanceReportReq obj, UserProfile profile);
        List<InterfaceLogReportRes> getInterfaceLogReportList(InterfaceLogReportReq obj, UserProfile profile);
        string ExportToExcelInterfaceLogReport(InterfaceLogReportReq obj, UserProfile profile);
        List<PHRReportRes> getPHRReportList(PHRReportReq obj, UserProfile profile);
        string ExportToExcelRequestToPHRReport(PHRReportReq obj, UserProfile profile);
    }
}
