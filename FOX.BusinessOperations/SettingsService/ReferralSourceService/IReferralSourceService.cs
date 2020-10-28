using System.Collections.Generic;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.Security;

namespace FOX.BusinessOperations.StoreProcedure.SettingsService.ReferralSourceService
{
    public interface IReferralSourceService
    {
        ReferralSource AddUpdateReferralSource(ReferralSource referralSource, UserProfile profile);

        ReferralSource GetReferralSourceBySourceID(long sourceId);

        List<ReferralSource> GetReferralSourceList(ReferralSourceSearch referralSourceSearch, UserProfile profile);
        string ExportToExcelReferralSource(ReferralSourceSearch referralSourceSearch, UserProfile profile);
        InactiveReasonAndDeliveryMethod GetInactiveReasonAndDeliveryMethod(long practiceCode);

        List<ReferralSource> GetReferralSourceByName(string searchString, long practiceCode);

        //ReferralSource GetCurrentUserReferralSource(UserProfile profile);
    }
}