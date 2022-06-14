using FOX.BusinessOperations.SettingsService.UserMangementService;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.FoxUserManagementServiceUnitTest
{
    public class FoxUserManagementServiceTest
    {

        UserManagementService userManagementService = new UserManagementService();
        UserProfile userProfile = new UserProfile();
        List<UserTeamModel> userTeamModel = new List<UserTeamModel>();
        UserTeamModel userTeamModelobj;
        [SetUp]
        public void Setup()
        {
            userManagementService = new UserManagementService();
            UserProfile userProfile = new UserProfile();
            List<UserTeamModel> userTeamModel = new List<UserTeamModel>();
        }

        [Test]
        public void UpdateUserTeam_userTeamModel_Profile_NotSaveData()
        {
            //Arrange
            userProfile.PracticeCode = 0;
            userTeamModel = null;
            //Act
            var result = userManagementService.UpdateUserTeam(userTeamModel, userProfile);
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
        public void UpdateUserTeam_UserProfile_NotSaveData()
        {
            //Arrange
            userProfile.PracticeCode = 123456;
            userTeamModel = null;
            //Act
            var result = userManagementService.UpdateUserTeam(userTeamModel, userProfile);
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
        public void UpdateUserTeam_userTeamModel_NotSaveData()
        {
            //Arrange
            userProfile.PracticeCode = 0;
            UserTeamModel userTeamModelobj = new UserTeamModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.PRACTICE_CODE = "123";
            userTeamModelobj.CREATED_DATE = DateTime.UtcNow;
            userTeamModelobj.CREATED_BY = "Ali";
            userTeamModelobj.MODIFIED_DATE = DateTime.UtcNow;
            userTeamModelobj.MODIFIED_BY = "Ali";
            userTeamModelobj.DELETED = false;
            userTeamModelobj.IS_CHECK_DISABLED = false;
            userTeamModelobj.ROLE_ID = 123;
            userTeamModel.Add(userTeamModelobj);
            //Act
            var result = userManagementService.UpdateUserTeam(userTeamModel, userProfile);
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
        public void UpdateUserTeam_userTeamModel_SaveData()
        {
            //Arrange
            userProfile.PracticeCode = 111363;
            userTeamModelobj = new UserTeamModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.PRACTICE_CODE = "123";
            userTeamModelobj.CREATED_DATE = DateTime.UtcNow;
            userTeamModelobj.CREATED_BY = "Ali";
            userTeamModelobj.MODIFIED_DATE = DateTime.UtcNow;
            userTeamModelobj.MODIFIED_BY = "Ali";
            userTeamModelobj.DELETED = false;
            userTeamModelobj.IS_CHECK_DISABLED = false;
            userTeamModelobj.ROLE_ID = 123;
            userTeamModel.Add(userTeamModelobj);
            //Act
            var result = userManagementService.UpdateUserTeam(userTeamModel, userProfile);
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

        [TearDown]
        public void BaseCleanup()
        {
            // Optionally dispose or cleanup objects
            userTeamModelobj = null;
            userProfile = null;
            userTeamModel = null;
        }
    }
}
