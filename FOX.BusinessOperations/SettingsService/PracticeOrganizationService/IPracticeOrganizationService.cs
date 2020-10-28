using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Practice;
using System.Collections.Generic;

namespace FOX.BusinessOperations.SettingsService.PracticeOrganizationService
{
    interface IPracticeOrganizationService
    {
        string GetMaxPracticeOrganizationCode(long practiceCode);

        PracticeOrganization AddUpdatePracticeOrganization(PracticeOrganization practiceOrganization, UserProfile profile);

        List<PracticeOrganization> GetPracticeOrganizationList(PracticeOrganizationRequest practiceOrganizationRequest, UserProfile profile);

        List<PracticeOrganization> GetPracticeOrganizationByName(string searchText, long practiceCode);

        string Export(PracticeOrganizationRequest obj, UserProfile profile);
    }
}
