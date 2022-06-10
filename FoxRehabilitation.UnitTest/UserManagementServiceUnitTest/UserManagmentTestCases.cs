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
        private string userId;
        private string roleID;
        private string callerUserID;

        [SetUp]
        public void SetUp()
        {
            userManagementService = new UserManagementService();
            userProfile = new UserProfile();
           
        }
        [Test]
        public void AddUserTeam_Profile_NoReturnData()
        {
            //Arrange
            userProfile = null;
            callerUserID = null;
            userId = null;
            roleID = null;

            bool result = userManagementService.AddUserTeam(userProfile,callerUserID,userId, roleID);
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
        public void AddUserTeam_PassAllParm_Returndata()
        {
            //Arrange
            userProfile.PracticeCode = 1011163 ;
            callerUserID = "123654";
            userId = "123654";
            roleID = "123654";

            bool result = userManagementService.AddUserTeam(userProfile, callerUserID, userId, roleID);
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
        public void AddUserTeam_NullCallerID_NoReturnData()
        {
            //Arrange
            userProfile.PracticeCode = 1011163;
            callerUserID = null;
            userId = "123654";
            roleID = "123654";


            var result = userManagementService.AddUserTeam(userProfile, callerUserID, userId, roleID);
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
        public void AddUserTeam_NullUserID_NoReturnData()
        {
            //Arrange
            userProfile.PracticeCode = 1011163;
            callerUserID = "123654";
            userId = null;
            roleID = null;

            var result = userManagementService.AddUserTeam(userProfile, callerUserID, userId, roleID);
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
        public void GetTeamList_TeamListName_NoReturnData()
        {
            //Arrange
            userProfile = null;
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
        public void GetTeamList_TeamListName_ReturnData()
        {
            //Arrange
            userProfile.PracticeCode = 1011163;
            roleID = "5483091";
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
        public void GetTeamList_RoleIdNull_NoReturnData()
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
        public void GetTeamList_PracticeCodeNegitive_NoReturnData()
        {
            //Arrange
            userProfile.PracticeCode = -0;
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
        public void BaseCleanup()
        {
            // Optionally dispose or cleanup objects
            userManagementService = null;
            userProfile = null;
            callerUserID = null;
            userId = null;
            roleID = null;
        }


    }
}
