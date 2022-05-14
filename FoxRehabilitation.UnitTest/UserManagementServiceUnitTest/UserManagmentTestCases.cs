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

            bool result = userManagementService.AddUserTeam(userProfile,callerUserID,userId);
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
        public void AddUserTeam_PassAllParm()
        {
            //Arrange
            userProfile.PracticeCode = 1011163 ;
            callerUserID = "123654";
            userId = "123654";

            bool result = userManagementService.AddUserTeam(userProfile, callerUserID, userId);
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
        public void AddUserTeam_NullCallerID()
        {
            //Arrange
            userProfile.PracticeCode = 1011163;
            callerUserID = null;
            userId = "123654";

            var result = userManagementService.AddUserTeam(userProfile, callerUserID, userId);
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
        public void AddUserTeam_NullUserID()
        {
            //Arrange
            userProfile.PracticeCode = 1011163;
            callerUserID = "123654";
            userId = null;

            var result = userManagementService.AddUserTeam(userProfile, callerUserID, userId);
            if (result)
            {
                Assert.IsTrue(false);

            }
            else
            {
                Assert.IsTrue(true);
            }

        }

        [TearDown]
        public void BaseCleanup()
        {
            // Optionally dispose or cleanup objects
            userManagementService = null;
            userProfile = null;
            callerUserID = "";
            userId = "";
        }


    }
}
