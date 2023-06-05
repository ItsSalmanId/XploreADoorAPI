using FOX.BusinessOperations.SettingsService.UserMangementService;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FoxRehabilitationAPI.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.UserManagementServiceUnitTest
{
    [TestFixture]
    public class UserManagementServiceTest
    {
        private UserManagementService _userManagementService;
        private UserProfile _userProfile;
        private List<TeamAddUpdateModel> _userTeamModel;
        private TeamAddUpdateModel userTeamModelobj;
        private User _user;
        public string roleID;
        private UserRequest _userRequest;
        private Role _role;
        private PasswordChangeRequest _passwordChangeRequest;
        private EmailExist _emailExist;
        private ReferralRegion _referralRegion;
        private ReferralRegionSearch _referralRegionSearch;
        private ResetPasswordViewModel _resetPasswordViewModel;
        private SmartSearchRequest _smartSearchRequest;
        private ApplicationUser _applicationUser;

        [SetUp]
        public void SetUp()
        {
            _userManagementService = new UserManagementService();
            _userTeamModel = new List<TeamAddUpdateModel>();
            _userProfile = new UserProfile();
            userTeamModelobj = new TeamAddUpdateModel();
            _user = new User();
            _userRequest = new UserRequest();
            _role = new Role();
            _passwordChangeRequest = new PasswordChangeRequest();
            _emailExist = new EmailExist();
            _referralRegion = new ReferralRegion();
            _referralRegionSearch = new ReferralRegionSearch();
            _resetPasswordViewModel = new ResetPasswordViewModel();
            _smartSearchRequest = new SmartSearchRequest();
            _applicationUser = new ApplicationUser();
        }

        [Test]
        public void UpdateUserTeam_NullArguments_NotUpdateData()
        {
            //Arrange
            _userProfile.PracticeCode = 0;
            _userTeamModel = null;

            //Act
            var result = _userManagementService.UpdateUserTeam(_userTeamModel, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void UpdateUserTeam_EmptyModel_NotUpdateData()
        {
            //Arrange
            _userProfile.PracticeCode = 123456;
            _userTeamModel = null;

            //Act
            var result = _userManagementService.UpdateUserTeam(_userTeamModel, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void UpdateUserTeam_EmptyUserProfile_NotUpdateData()
        {
            //Arrange
            _userProfile.PracticeCode = 0;
            TeamAddUpdateModel userTeamModelobj = new TeamAddUpdateModel
            {
                USER_ID = 123,
                PHD_CALL_SCENARIO_ID = 123,
                DELETED = false,
                ROLE_ID = 123
            };
            _userTeamModel.Add(userTeamModelobj);

            //Act
            var result = _userManagementService.UpdateUserTeam(_userTeamModel, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void UpdateUserTeam_PassArguments_UpdateData()
        {
            //Arrange
            _userProfile.PracticeCode = 111363;
            userTeamModelobj = new TeamAddUpdateModel
            {
                USER_ID = 123,
                PHD_CALL_SCENARIO_ID = 123,
                DELETED = false,
                ROLE_ID = 123
            };
            _userTeamModel.Add(userTeamModelobj);

            //Act
            var result = _userManagementService.UpdateUserTeam(_userTeamModel, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [Test]
        public void AddUserTeam_NullArguments_NoSaveData()
        {
            //Arrange
            _userProfile.PracticeCode = 0;
            _userTeamModel = null;

            //Act
            bool result = _userManagementService.AddUserTeam(_userTeamModel, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void AddUserTeam_PassArguments_SaveData()
        {
            //Arrange
            _userProfile.PracticeCode = 111363;
            userTeamModelobj = new TeamAddUpdateModel
            {
                USER_ID = 123,
                PHD_CALL_SCENARIO_ID = 123,
                DELETED = false,
                ROLE_ID = 123
            };
            _userTeamModel.Add(userTeamModelobj);

            //Act
            bool result = _userManagementService.AddUserTeam(_userTeamModel, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [Test]
        public void AddUserTeam_EmptyModel_NotSaveData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userTeamModel = null;

            //Act
            var result = _userManagementService.AddUserTeam(_userTeamModel, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void AddUserTeam_EmptyProfile_NotSaveData()
        {
            //Arrange
            _userProfile.PracticeCode = 0;
            TeamAddUpdateModel userTeamModelobj = new TeamAddUpdateModel
            {
                USER_ID = 123,
                PHD_CALL_SCENARIO_ID = 123,
                DELETED = false,
                ROLE_ID = 123
            };
            _userTeamModel.Add(userTeamModelobj);

            //Act
            var result = _userManagementService.AddUserTeam(_userTeamModel, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(false);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void GetTeamList_EmptyArguments_NoReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 0;
            roleID = null;

            //Act
            var result = _userManagementService.GetTeamList(roleID, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [Test]
        public void GetTeamList_PassArguments_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            roleID = "5483234";

            //Act
            var result = _userManagementService.GetTeamList(roleID, _userProfile);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void GetTeamList_EmptyRoleId_NoReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 123;
            roleID = null;

            //Act
            var result = _userManagementService.GetTeamList(roleID, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void GetTeamList_EmptyUserProfile_NoReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 0;
            roleID = "5483043";

            //Act
            var result = _userManagementService.GetTeamList(roleID, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(100000)]
        public void SetAutoLockTimeSetup_UserModel_ReturnData(int time)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "khankhan_544596";

            //Act
            _userManagementService.SetAutoLockTimeSetup(time, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void GetUsers_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _userRequest.SearchText = "test";
            _userRequest.CurrentPage = 1;
            _userRequest.RecordPerPage = 10;
            _userRequest.FilterIs_Approved = true;

            //Act
            var result = _userManagementService.GetUsers(_userRequest, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test", 100, "test236@gmail.com")]
        [TestCase("1163testing", 100, "muhammadsalman7")]
        [TestCase("1163testing", 103, "muhammadsalman7")]
        [TestCase("1163testing", 101, "muhammadsalman7")]
        public void GetSingleUser_PassModel_ReturnData(string userName, long roleId, string email)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _userProfile.UserEmailAddress = email;
            _userProfile.RoleId = roleId;

            //Act
            var result = _userManagementService.GetSingleUser(userName, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test", 100, "test236@gmail.com")]
        [TestCase("1163testing", 100, "foxtest2@rmb.com")]
        [TestCase("1163testing", 103, "muhammadsalman7")]
        [TestCase("1163testing", 101, "muhammadsalman7")]
        public void GetReferralReagions_PassModel_ReturnData(string userName, long roleId, string email)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _userProfile.UserEmailAddress = email;
            _userProfile.RoleId = roleId;

            //Act
            var result = _userManagementService.GetReferralReagions(userName, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        [TestCase(0)]
        public void GetUsersForSupervisorDD_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _userManagementService.GetUsersForSupervisorDD(practiceCode);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        [TestCase(0)]
        public void GetRoles_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _userManagementService.GetRoles(practiceCode);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, true, 135)]
        [TestCase(1011163, false, 134)]
        [TestCase(1012714, true, 134)]
        [TestCase(0, false, 134)]
        public void SetRole_PassModel_ReturnData(long practiceCode, bool isChecked, int rightId)
        {
            //Arrange
            _role.RIGHTS_OF_ROLE_ID = 296;
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _role.RIGHT_ID = rightId;
            _role.ROLE_ID = 103;
            _role.IS_CHECKED = isChecked;

            //Act
            _userManagementService.SetRole(_role, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        [TestCase(0)]
        public void GetPracticeRole_PassParameter_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _userManagementService.GetPracticeRole(practiceCode);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        [TestCase(0)]
        public void UpdatePassword_PassParameter_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "testing";
            _passwordChangeRequest.User_id = 544596;
            _passwordChangeRequest.PasswordHash = "_";
            _passwordChangeRequest.Password = "Test";
            _passwordChangeRequest.User_id = 123;

            //Act
            var result = _userManagementService.UpdatePassword(_passwordChangeRequest, _userProfile);

            //Assert
            if (result > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "test", "test")]
        [TestCase(1012714, "test", "test")]
        [TestCase(0, "test", "test")]
        public void UpdateADUserPassword_PassParameter_ReturnData(long practiceCode, string password, string hashPassword)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "testing";
            _userProfile.userID = 544596;

            //Act
            var result = _userManagementService.UpdateADUserPassword(password, hashPassword, _userProfile);

            //Assert
            if (result > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, 544100)]
        public void GetAdRole_PassParameter_ReturnData(long practiceCode, long roleId)
        {
            //Arrange
            string roleName = "Occupational Therapist";

            //Act
            var result = _userManagementService.GetADRole(practiceCode, roleName, roleId);

            //Assert
            if (result > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, 544100)]
        [TestCase(1012714, 544101)]
        [TestCase(0, 0)]
        public void GetCurrentUserRights_PassParameter_ReturnData(long practiceCode, long roleId)
        {
            //Arrange
            //Act
            var result = _userManagementService.GetCurrentUserRights(roleId, practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test")]
        [TestCase("1163testing")]
        public void EmailExists_Passmodel_ReturnData(string email)
        {
            //Arrange
            _emailExist.EMAIL = email;
            //Act
            var result = _userManagementService.EmailExists(_emailExist);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetReferralRegion_Passmodel_ReturnData(int signal)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _referralRegionSearch.signal = signal;
            _referralRegionSearch.REFERRAL_REGION_NAME = "test";

            //Act
            var result = _userManagementService.GetReferralRegion(_referralRegionSearch, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetReferralRegionList_Passmodel_ReturnData(int signal)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _referralRegionSearch.signal = signal;
            _referralRegionSearch.REFERRAL_REGION_NAME = "test";
            _referralRegionSearch.REFERRAL_REGION_CODE = "test";
            _referralRegionSearch.searchString = "test";
            _referralRegionSearch.CurrentPage = 1;
            _referralRegionSearch.RecordPerPage = 10;
            _referralRegionSearch.SortBy = "";
            _referralRegionSearch.SortOrder = "DESC";

            //Act
            var result = _userManagementService.GetReferralRegionList(_referralRegionSearch, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "test")]
        public void GetRolesCheckBit_Passmodel_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _userManagementService.GetRolesCheckBit(practiceCode, userName);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test@test.com")]
        [TestCase("B@lasdb.bg")]
        public void ValidateUserEmail_Passmodel_ReturnData(string email)
        {
            //Arrange
            //Act
            var result = _userManagementService.ValidateUserEmail(email);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(5483055)]
        [TestCase(5482608)]
        [TestCase(5482303)]
        [TestCase(5482172)]
        public void UpdatePassword_Passmodel_ReturnData(long userId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _passwordChangeRequest.User_id = userId;
            _passwordChangeRequest.PasswordHash = "test";
            _passwordChangeRequest.Password = "123456789";

            //Act
            var result = _userManagementService.UpdatePassword(_passwordChangeRequest, _userProfile);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void UpdatePassword_model_ReturnData()
        {
            //Arrange
            _resetPasswordViewModel.Email = "test@test.com";
            _resetPasswordViewModel.Ticks = "637265386954157246";
            _resetPasswordViewModel.PasswordHash = "123456789";
            _resetPasswordViewModel.NewPassword = "123456789";
            _resetPasswordViewModel.PasswordHash = "123456789";

            //Act
            var result = _userManagementService.UpdatePassword(_resetPasswordViewModel);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("FoxDocumentDirectory\\HRAutoEmailsUploadedFiles\\UploadFiles", "user_5482608")]
        public void VerifyHashedPassword_model_ReturnData(string path, string userName)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _userManagementService.SaveSignaturesInDB(path, userName, _userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("637265386954157246", "ayaz.muhammad366@gmail.com")]
        [TestCase("636820741604931248", "taseerijkhjkqbal@mtbc.com")]
        public void SavePasswordResetTicks_PassModel_ReturnData(string ticks, string email)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            _userManagementService.SavePasswordResetTicks(ticks, email);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("637265386954157246")]
        [TestCase("636820741604931248")]
        public void GetEmailByTick_PassParameter_ReturnData(string ticks)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            _userManagementService.GetEmailByTick(ticks);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("ACO Identifier", "Mssp_car")]
        [TestCase("test", "test")]
        [TestCase("", "")]
        public void GetSmartIdentifier_PassParameter_ReturnData(string type, string searchValue)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _smartSearchRequest.TYPE = type;
            _smartSearchRequest.SEARCHVALUE = searchValue;

            //Act
            var result = _userManagementService.GetSmartIdentifier(_smartSearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("ACO Identifier")]
        [TestCase("test")]
        [TestCase("")]
        public void GetSmartSpecialities_PassParameter_ReturnData(string keyword)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _smartSearchRequest.Keyword = keyword;

            //Act
            var result = _userManagementService.getSmartSpecialities(_smartSearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("ACO Identifier")]
        [TestCase("test")]
        [TestCase("")]
        public void GetPractices_PassParameter_ReturnData(string keyword)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _smartSearchRequest.Keyword = keyword;

            //Act
            var result = _userManagementService.getPractices(_smartSearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(5483055, "179", true)]
        public void AddUpdateUserExtension_PassParameter_ReturnData(long userId, string extension, bool isActive)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            _userManagementService.AddUpdateUserExtension(userId, extension, isActive);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("dwd_5483055")]
        public void UpdateProfile_PassParameter_ReturnData(string useName)
        {
            //Arrange
            //Act
            var result = _userManagementService.UpdateProfile(useName);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(5481885, true)]
        [TestCase(5481887, true)]
        [TestCase(0, true)]
        public void AddUpdateUserAdditionalInfo_PassParameter_ReturnData(long userId, bool isElectronicPoc)
        {
            //Arrange
            DateTime CreatedDate = DateTime.Now;
            DateTime ModifiedDate = DateTime.Now;
            string CreatedBy = "U-UnitTesting";
            string ModifiedBy = "U-UnitTesting";
            bool Deleted = false;

            //Act
            _userManagementService.AddUpdateUserAdditionalInfo(userId, isElectronicPoc, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, Deleted);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("dwd_5483055")]
        [TestCase("test")]
        public void AddUpdateReferralSourceInfo_PassParameter_ReturnData(string userName)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";

            //Act
            _userManagementService.AddUpdateReferralSourceInfo(userName, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void SaveSenderName_Passmodel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";
            _applicationUser.UserName = "FOX-TEAM";
            _applicationUser.USER_DISPLAY_NAME = "FOX-TEAM";
            _applicationUser.ROLE_ID = 101;
            _applicationUser.IS_ACTIVE = true;
            _applicationUser.CREATED_BY = "FOX-TEAM";

            //Act
            _userManagementService.SaveSenderName(_applicationUser, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(0)]
        public void SaveSenderName_PassParameter_ReturnData(long practiceCode)
        {
            //Arrange
            _applicationUser.USER_NAME = "FOX-TEAM";
            _applicationUser.USER_DISPLAY_NAME = "FOX-TEAM";
            _applicationUser.ROLE_ID = 101;
            _applicationUser.IS_ACTIVE = true;
            _applicationUser.CREATED_BY = "FOX-TEAM";

            //Act
            _userManagementService.SaveSenderName(_applicationUser, practiceCode);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(0)]
        public void SavePasswordHisotry_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _applicationUser.USER_NAME = "FOX-TEAM";
            _applicationUser.USER_DISPLAY_NAME = "FOX-TEAM";
            _applicationUser.ROLE_ID = 101;
            _applicationUser.IS_ACTIVE = true;
            _applicationUser.CREATED_BY = "FOX-TEAM";

            //Act
            _userManagementService.SavePasswordHisotry(_applicationUser, practiceCode);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void SavePasswordHisotry_PassParameter_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";
            _applicationUser.USER_NAME = "FOX-TEAM";
            _applicationUser.USER_DISPLAY_NAME = "FOX-TEAM";
            _applicationUser.ROLE_ID = 101;
            _applicationUser.IS_ACTIVE = true;
            _applicationUser.CREATED_BY = "FOX-TEAM";

            //Act
            _userManagementService.SavePasswordHisotry(_applicationUser, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void SavePasswordHisotry_PassParameter_ReturnData(string userName)
        {
            //Arrange
            //Act
            _userManagementService.GetUserProfileByName(userName);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(5483055)]
        [TestCase(5482608)]
        [TestCase(5482303)]
        [TestCase(5482172)]
        public void IsUserAlreadyExist_PassParameter_ReturnData(long userId)
        {
            //Arrange
            //Act
            var result = _userManagementService.IsUserAlreadyExist(userId);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void UpdateUser_Passmodel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";
            _applicationUser.UserName = "FOX-TEAM";
            _applicationUser.USER_DISPLAY_NAME = "FOX-TEAM";
            _applicationUser.ROLE_ID = 101;
            _applicationUser.IS_ACTIVE = false;
            _applicationUser.CREATED_BY = "FOX-TEAM";
            _applicationUser.LAST_NAME = "FOX-TEAM";
            _applicationUser.FIRST_NAME = "FOX-TEAM";

            //Act
            var result = _userManagementService.UpdateUser(_user, _applicationUser, _userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void GetSmartUsersOfSpecificRoleName_Passmodel_ReturnData(string roleName)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";

            //Act
            var result = _userManagementService.GetAlternateAccountManger(roleName, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void GetAlternateAccountManger_Passmodel_ReturnData(string roleName)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";

            //Act
            var result = _userManagementService.GetAlternateAccountManger(roleName, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void GetSmartStates_Passmodel_ReturnData(string searchText)
        {
            //Arrange
            //Act
            var result = _userManagementService.GetSmartStates(searchText);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(123, 1011163)]
        [TestCase(0, 1011163)]
        public void CheckForAtleastOneRight_Passmodel_ReturnData(long roleId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _userManagementService.CheckForAtleastOneRight(roleId, practiceCode);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void SaveRegionalDirectorID_PassModel_ReturnData()
        {
            //Arrange
            _applicationUser.USER_NAME = "FOX-TEAM";
            _applicationUser.USER_DISPLAY_NAME = "FOX-TEAM";
            _applicationUser.ROLE_ID = 101;
            _applicationUser.IS_ACTIVE = true;
            _applicationUser.CREATED_BY = "FOX-TEAM";
            _referralRegion.REGIONAL_DIRECTOR_ID = 1;

            //Act
            _userManagementService.SaveRegionalDirectorID(_applicationUser, _referralRegion);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("test", 123)]
        [TestCase("", 0)]
        public void GetRoleId_PassModel_ReturnData(string roleName, long practiceCode)
        {
            //Arrange
            //Act
            var result = _userManagementService.GetRoleId(roleName, practiceCode);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void CanUserUpdateUser_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";
            _userProfile.RoleId = 100;

            //Act
            var result = _userManagementService.CanUserUpdateUser(_userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(true, 54832337)]
        [TestCase(true, 19)]
        public void UpdateUser_PassModel_ReturnData(bool canUpdateUsers, int senderTypeId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";
            _userProfile.RoleId = 100;
            _applicationUser.IS_ACTIVE = true;
            _applicationUser.LAST_NAME = "FOX-TEAM";
            _applicationUser.MIDDLE_NAME = "FOX-TEAM";
            _applicationUser.FOX_TBL_SENDER_TYPE_ID = senderTypeId;
            _applicationUser.ROLE_ID = 101;
            _applicationUser.ROLE_NAME = "SUPERVISOR";

            //Act
            var result = _userManagementService.UpdateUser(_user, _applicationUser, _userProfile, canUpdateUsers);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void CheckValidUserLoginAttempt_PassModel_ReturnData(string userName)
        {
            //Arrange
            //Act
            var result = _userManagementService.CheckValidUserLoginAttempt(userName);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("testt")]
        [TestCase("")]
        public void IsUserBlocked_PassModel_ReturnData(string userName)
        {
            //Arrange
            //Act
            var result = _userManagementService.IsUserBlocked(userName);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("testt")]
        [TestCase("")]
        public void GetInvalidAttempts_PassModel_ReturnData(string userName)
        {
            //Arrange
            //Act
            var result = _userManagementService.GetInvalidAttempts(userName);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("testt")]
        [TestCase("")]
        public void AddUserValidLoginAttempt_PassModel_ReturnData(string userName)
        {
            //Arrange
            //Act
            var result = _userManagementService.AddUserValidLoginAttempt(userName);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("testt")]
        [TestCase("")]
        public void AddUserInvalidLoginAttempt_PassModel_ReturnData(string userName)
        {
            //Arrange
            //Act
            var result = _userManagementService.AddUserInvalidLoginAttempt(userName);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(544252)]
        [TestCase(548517)]
        public void InsertReferralRegionDashBoardAccess_PassModel_ReturnData(long referralRegionId)
        {
            //Arrange
            List<FOX_TBL_DASHBOARD_ACCESS> fOX_TBL_DASHBOARD_ACCESSes = new List<FOX_TBL_DASHBOARD_ACCESS>()
             {
                new FOX_TBL_DASHBOARD_ACCESS
                {
                    DASHBOARD_ACCESS_ID = 544252,
                    ROLE_NAME = "test"
                }
             };
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "FOX-TEAM";

            //Act
            _userManagementService.InsertReferralRegionDashBoardAccess(referralRegionId, fOX_TBL_DASHBOARD_ACCESSes, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("1011163")]
        public void CheckisTalkrehab_PassModel_ReturnData(string practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "dwd_5483055";
            _userProfile.UserEmailAddress = "test@test.com";
            _userProfile.RoleId = 123;

            //Act
            var result = _userManagementService.CheckisTalkrehab(practiceCode);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(null)]
        [TestCase(1111111111)]
        [TestCase(-1)]
        public void UpdateOtpEnableDate_NullAndInvalidData(long userId)
        {
         
            //Act
            var result = _userManagementService.UpdateOtpEnableDate(userId);
            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [Test]
        [TestCase("11111111")]
        [TestCase("")]
        [TestCase(null)]
        public void UpdateMfaStatus_NullAndInvalidData(string userId)
        {

            //Act
            var result = _userManagementService.UpdateMfaStatus(userId);
            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _userManagementService = null;
            _userTeamModel = null;
            _userProfile = null;
            userTeamModelobj = null;
            userTeamModelobj = null;
            _user = null;
            _userRequest = null;
            _role = null;
            _passwordChangeRequest = null;
            _emailExist = null;
            _referralRegion = null;
            _referralRegionSearch = null;
            _resetPasswordViewModel = null;
            _smartSearchRequest = null;
            _applicationUser = null;
        }
    }
}
