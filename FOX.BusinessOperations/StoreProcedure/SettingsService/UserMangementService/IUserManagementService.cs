using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.SettingsService.UserMangementService
{
    public interface IUserManagementService
    {
        void CreateUser(User user, UserProfile profile); 
        List<User> GetUsers(UserRequest request, UserProfile profile);  
        List<UsersForDropdown> GetUsersForSupervisorDD(UserProfile profile);
        List<right> GetRoles(UserProfile profile);
        void SetRole(Role role, UserProfile profile);
        List<Role> GetPracticeRole(long practiceCode);
        void UpdatePassword(PasswordChangeRequest request, UserProfile profile);
        void AddRole(RoleToAdd request, UserProfile profile);
        void DeleteRole(Role roleId, UserProfile profile);
        List<RoleAndRights> GetCurrentUserRights(UserProfile profile);
        bool EmailExists(EmailExist request);
        void AddUpdateReferralRegion(ReferralRegion referralRegion, UserProfile profile);
        ReferralRegion GetReferralRegion(ReferralRegionSearch referralRegionSearch);
        List<ReferralRegion> GetReferralRegionList(ReferralRegionSearch referralRegionSearch);
    }
}
