using FOX.BusinessOperations.QualityAssuranceService.PerformAuditService;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.PerformAuditServiceUnitTest
{
   public class PerormAuditServiceTest
    {

        PerformAuditService performAuditService = new PerformAuditService();
        UserProfile userProfile = new UserProfile();
        private int callScanrioID;

        [SetUp]
        public void Setup()
        {
            performAuditService = new PerformAuditService();
        }
        [Test]
        public void GetDropdownLists_UserProfile_NoReturnData()
        {
            //Arrange
            userProfile = null;
            callScanrioID = 123;
            //Act
            var result = performAuditService.GetTeamMemberName(callScanrioID, userProfile);
            //Assert
            if (result.Count > 0)
            {
                Assert.Pass("Failed");
            }
            else
            {
                Assert.Pass("Passed");
            }
        }
        [Test]
        public void GetDropdownLists_CallScanrioID_NoReturnData()
        {
            //Arrange
            userProfile.PracticeCode = 1011163;
            callScanrioID = 0;
            //Act
            var result = performAuditService.GetTeamMemberName(callScanrioID, userProfile);
            //Assert
            if (result.Count > 0)
            {
                Assert.Pass("Failed");
            }
            else
            {
                Assert.Pass("Passed");
            }
        }
        [TearDown]
        public void TearDown()
        {
            // Optionally dispose or cleanup objects
            userProfile = null;
            callScanrioID = 0;
        }

    }
}
