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
    public class UserManagementServiceUnitTest
    {
        UserManagementService userManagementService = new UserManagementService();
        UserProfile userProfile = new UserProfile();
        private string roleID;


        [SetUp]
        public void Setup()
        {
            userManagementService = new UserManagementService();
        }
        [Test]
        public void GetTeamList_TeamListName_NoReturnData()
        {
            //Arrange
            userProfile = null;
            roleID = null;
            //Act
            var result = userManagementService.GetTeamList(roleID,userProfile);
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
            userProfile.PracticeCode = 123;
            roleID = "5483043";
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
            userProfile = null;
            roleID = null;
        }
    }
}
