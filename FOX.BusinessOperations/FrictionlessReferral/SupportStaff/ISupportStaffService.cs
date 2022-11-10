using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.RequestForOrder;
using System.Collections.Generic;

namespace FOX.BusinessOperations.FrictionlessReferral.SupportStaff
{
    public interface ISupportStaffService
    {
        #region FUNCTIONS
        List<InsurancePayer> GetInsurancePayers();
        ResponseModel SendInviteToPatientPortal(PatientDetail obj);
        ResponseModel SendInviteOnMobile(PatientDetail obj);
        List<ProviderReferralSourceResponse> GetOrderingReferralSource(ProviderReferralSourceRequest obj);
        FrictionLessReferral GetFrictionLessReferralDetails(long referralId);
        FrictionLessReferralResponse SaveFrictionLessReferralDetails(FrictionLessReferral frictionLessReferral);
        ResponseModel DownloadPdf(RequestDownloadPdfFrictionlessModel requestDownloadPdfModel);
        ResponseModel SendFAX(FrictionLessRequestSendFAXModel requestSendFAXModel);
        ResponseModel SendEmail(RequestSendEmailModel requestSendEmailModel);
        QRCodeModel GenerateQRCode(QRCodeModel obj);
        ResponseUploadFilesModel UploadFiles(RequestUploadFilesModel requestUploadFilesAPIModel);
        #endregion
    }
}
