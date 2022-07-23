using FOX.BusinessOperations.SettingsService.UserMangementService;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.UserManagementServiceUnitTest
{
    public class UserManagmentTestCases
    {
        UserManagementService userManagementService = new UserManagementService();
        UserProfile userProfile = new UserProfile();
        private List<TeamAddUpdateModel> _userTeamModel;
        TeamAddUpdateModel userTeamModelobj;
        private string roleID;

        [SetUp]
        public void SetUp()
        {
            _userTeamModel = new List<TeamAddUpdateModel>();
            userManagementService = new UserManagementService();
            userProfile = new UserProfile();
           
        }

        [Test]
        public void UpdateUserTeam_NullArguments_NotUpdateData()
        {
            //Arrange
            userProfile.PracticeCode = 0;
            _userTeamModel = null;
            //Act
            var result = userManagementService.UpdateUserTeam(_userTeamModel, userProfile);
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
            userProfile.PracticeCode = 123456;
            _userTeamModel = null;
            //Act
            var result = userManagementService.UpdateUserTeam(_userTeamModel, userProfile);
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
            userProfile.PracticeCode = 0;
            TeamAddUpdateModel userTeamModelobj = new TeamAddUpdateModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.DELETED = false;
            userTeamModelobj.ROLE_ID = 123;
            _userTeamModel.Add(userTeamModelobj);

            //Act
            var result = userManagementService.UpdateUserTeam(_userTeamModel, userProfile);

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
            userProfile.PracticeCode = 111363;
            userTeamModelobj = new TeamAddUpdateModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.DELETED = false;
            userTeamModelobj.ROLE_ID = 123;
            _userTeamModel.Add(userTeamModelobj);
            //Act
            var result = userManagementService.UpdateUserTeam(_userTeamModel, userProfile);
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
            userProfile.PracticeCode = 0;
            _userTeamModel = null;
            //Act
            bool result = userManagementService.AddUserTeam(_userTeamModel, userProfile);
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
            userProfile.PracticeCode = 111363;
            userTeamModelobj = new TeamAddUpdateModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;

            userTeamModelobj.DELETED = false;
     
            userTeamModelobj.ROLE_ID = 123;
            _userTeamModel.Add(userTeamModelobj);
            //Act
            bool result = userManagementService.AddUserTeam(_userTeamModel, userProfile);
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
            userProfile.PracticeCode = 1011163;
            _userTeamModel = null;

            //Act
            var result = userManagementService.AddUserTeam(_userTeamModel, userProfile);
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
            userProfile.PracticeCode = 0;
            TeamAddUpdateModel userTeamModelobj = new TeamAddUpdateModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.DELETED = false;
            userTeamModelobj.ROLE_ID = 123;
            _userTeamModel.Add(userTeamModelobj);
            //Act
            var result = userManagementService.AddUserTeam(_userTeamModel, userProfile);
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
            userProfile.PracticeCode = 0;
            roleID = null;
            //Act
            var result = userManagementService.GetTeamList(roleID, userProfile);
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
            userProfile.PracticeCode = 1011163;
            roleID = "5483234";
            //Act
            var result = userManagementService.GetTeamList(roleID, userProfile);
            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [Test]
        public void GetTeamList_EmptyRoleId_NoReturnData()
        {
            //Arrange
            userProfile.PracticeCode = 123;
            roleID = null;
            //Act
            var result = userManagementService.GetTeamList(roleID, userProfile);
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
        public void GetTeamList_EmptyUserProfile_NoReturnData()
        {
            //Arrange
            userProfile.PracticeCode = 0;
            roleID = "5483043";
            //Act
            var result = userManagementService.GetTeamList(roleID, userProfile);
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

        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            userManagementService = null;
            userProfile = null;
            roleID = null;
            userTeamModelobj = null;
            userProfile = null;
            _userTeamModel = null;
        }


    }
}
