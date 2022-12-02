﻿using FOX.BusinessOperations.SettingsService.UserMangementService;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
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
        private string roleID;

        [SetUp]
        public void SetUp()
        {
            _userManagementService = new UserManagementService();
            _userTeamModel = new List<TeamAddUpdateModel>();
            _userProfile = new UserProfile();
            userTeamModelobj = new TeamAddUpdateModel();
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
            TeamAddUpdateModel userTeamModelobj = new TeamAddUpdateModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.DELETED = false;
            userTeamModelobj.ROLE_ID = 123;
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
            userTeamModelobj = new TeamAddUpdateModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.DELETED = false;
            userTeamModelobj.ROLE_ID = 123;
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
            userTeamModelobj = new TeamAddUpdateModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.DELETED = false;
            userTeamModelobj.ROLE_ID = 123;
            _userTeamModel.Add(userTeamModelobj);
            //Act
            bool result = _userManagementService.AddUserTeam(_userTeamModel, _userProfile);
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
            TeamAddUpdateModel userTeamModelobj = new TeamAddUpdateModel();
            userTeamModelobj.USER_ID = 123;
            userTeamModelobj.PHD_CALL_SCENARIO_ID = 123;
            userTeamModelobj.DELETED = false;
            userTeamModelobj.ROLE_ID = 123;
            _userTeamModel.Add(userTeamModelobj);
            //Act
            var result = _userManagementService.AddUserTeam(_userTeamModel, _userProfile);
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
                Assert.IsTrue(false);
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
                Assert.IsTrue(false);
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
                Assert.IsTrue(false);
            }
        }

        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _userManagementService = null;
            _userProfile = null;
            roleID = null;
            userTeamModelobj = null;
            _userTeamModel = null;
        }
    }
}
