using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.ReferralSource;

namespace FOX.BusinessOperations.RequestForOrder
{
    public interface IRequestForOrderService
    {
        ResponseGeneratingWorkOrder GeneratingWorkOrder(long practiceCode, string userName, string email, long userId);
        ResponseModel SendEmail(RequestSendEmailModel requestSendEmailModel, UserProfile Profile);
        ResponseModel SendFAX(RequestSendFAXModel requestSendFAXModel, UserProfile Profile);
        bool VerifyWorkOrderByRecipient(string value);
        ResponseModel DeleteWorkOrder(RequestDeleteWorkOrder requestDeleteWorkOrder, UserProfile Profile);
        ResponseModel DownloadPdf(RequestDownloadPdfModel requestDownloadPdfModel, UserProfile Profile);
        ResponseModel AddDocument_SignOrder(ReqAddDocument_SignOrder reqAddDocument_SignOrder, UserProfile Profile);
        ReferralSource GetUserReferralSource(string email, long userId);
    }
}
