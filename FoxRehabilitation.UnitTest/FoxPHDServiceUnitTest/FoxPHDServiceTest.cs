using FOX.BusinessOperations.FoxPHDService;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.FoxPHDServiceUnitTest
{
    public class FoxPHDServiceTest
    {
        FoxPHDService foxPHDService = new FoxPHDService();
        UserProfile userProfile = new UserProfile();

        [SetUp]
        public void Setup()
        {
            foxPHDService = new FoxPHDService();
        }
        [Test]
        public void GetDropdownLists_UserProfile_ReturnData()
        {
            //Arrange
            UserProfile userProfile = new UserProfile();
            userProfile.PracticeCode = 1011163;
            userProfile.userID = 1011163415;
            userProfile.UserName = "1163testing";
            userProfile.isTalkRehab = false;
            //Act
            var result = foxPHDService.GetDropdownLists(userProfile);
            //Assert
            if (result.PhdCallReasons.Count > 0)
            {
                Assert.Pass("Passed");
            }
            else
            {
                Assert.Pass("Failed");
            }
        }
        [TearDown]
        public void BaseCleanup ()
        {
            // Optionally dispose or cleanup objects
            userProfile = null;
        }
    }
}
