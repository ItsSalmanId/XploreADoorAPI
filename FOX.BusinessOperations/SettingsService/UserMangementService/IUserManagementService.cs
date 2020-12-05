using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.StatesModel;

namespace FOX.BusinessOperations.SettingsService.UserMangementService
{
    public interface IUserManagementService
    {
        bool CreateUser(User user, UserProfile profile);
        bool UpdateUser(User userToUpdate, dynamic user, UserProfile profile);
        List<User> GetUsers(UserRequest request, UserProfile profile);
        User GetSingleUser(string user, UserProfile profile);
        User_And_Regions_Data GetReferralReagions(string user, UserProfile profile);
        List<UsersForDropdown> GetUsersForSupervisorDD(long practiceCode);
        List<right> GetRoles(long practiceCode);
        void SetRole(Role role, UserProfile profile);
        List<Role> GetPracticeRole(long practiceCode);
        int UpdatePassword(PasswordChangeRequest request, UserProfile profile);
        void AddRole(RoleToAdd request, UserProfile profile);
        void DeleteRole(Role roleId, UserProfile profile);
        List<RoleAndRights> GetCurrentUserRights(long roleId, long practiceCode);
        bool EmailExists(EmailExist request);
        User IsUserAlreadyExist(long userId);
        void AddUpdateReferralRegion(ReferralRegion referralRegion, UserProfile profile);
        ReferralRegion GetReferralRegion(ReferralRegionSearch referralRegionSearch, UserProfile profile);
        List<ReferralRegion> GetReferralRegionList(ReferralRegionSearch referralRegionSearch, UserProfile profile);
        string ExportToExcelReferralRegion(ReferralRegionSearch referralRegionSearch, UserProfile profile);
        List<right> GetRolesCheckBit(long practiceCode, string userName);
        ResponseModel ValidateUserEmail(string email);
        int UpdatePassword(ResetPasswordViewModel data);
        List<FileRecieverResult> UploadSignatures(HttpFileCollection files, string username, UserProfile profile);
        bool SavePasswordResetTicks(string ticks, string email);
        string GetEmailByTick(string ticks);
        SmartSpecialitySearchResponseModel getSmartSpecialities(SmartSearchRequest model, UserProfile Profile);
        FoxTBLPracticeOrganizationResponseModel getPractices(SmartSearchRequest model, UserProfile Profile);
        UserProfile UpdateProfile(string uSER_NAME);
        List<SmartIdentifierRes> GetSmartIdentifier(SmartSearchRequest obj, UserProfile Profile);
        bool DiscardAndDeleteExternalUser(User user, UserProfile profile);
        void AddUpdateUserExtension(long userId, string extension, bool? isActive);
        UserProfile GetUserProfileByName(string userName);
        void SaveSenderName(dynamic user, UserProfile profile);
        void SavePasswordHisotry(dynamic user, UserProfile profile);
        List<User> GetSmartUsersOfSpecificRoleName(string searchText, string roleName, UserProfile userProfile);
        List<User> GetAlternateAccountManger(string roleName, UserProfile userProfile);
        List<States> GetSmartStates(string searchText);
        bool CheckForAtleastOneRight(long roleId, long practiceCode);
        bool CanUserUpdateUser(UserProfile profile);
        bool UpdateUser(User userToUpdate, dynamic user, UserProfile profile, bool canUpdateUsers);
        void AddUpdateUserAdditionalInfo(long UserId, bool isElectronicPOC, DateTime CreatedDate, string CreatedBy, DateTime ModifiedDate, string ModifiedBy, bool Deleted);
        string RedirecToTalkEhr(UserProfile profile);
        bool SetAutoLockTimeSetup(int time, UserProfile profile);
        bool DeleteUser(DeleteUserModel res, UserProfile profile);
    }
}
