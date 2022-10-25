using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.Patient;
using System.Collections.Generic;

namespace FOX.BusinessOperations.FrictionlessReferral.SupportStaff
{
    public interface ISupportStaffService
    {
        #region FUNCTIONS
        List<InsurancePayer> GetInsurancePayers();
        ResponseModel SendInviteToPatientPortal(PatientDetail obj);
        ResponseModel SendInviteOnMobile(PatientDetail obj);
        List<OrderingReferralSourceResponse> GetOrderingReferralSource(OrderReferralSourceRequest obj);
        FrictionLessReferral GetFrictionLessReferralDetails(long referralId);
        ResponseModel SaveFrictionLessReferralDetails(FrictionLessReferral frictionLessReferral);
        #endregion
    }
}
