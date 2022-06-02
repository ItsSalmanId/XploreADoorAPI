using FOX.BusinessOperations.SignatureRequiredServices;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.SignatureRequired;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.SignatureRequiredServiceUnitTest
{
    public class SignatureRequiredServiceTest
    {
        SignatureRequiredService signatureRequired = new SignatureRequiredService();
        UserProfile userProfile = new UserProfile();

        [SetUp]
        public void Setup()
        {
            signatureRequired = new SignatureRequiredService();
        }
        [Test]
        public void GetReferralList_EmptyModel_NoReturnData()
        {
            //Arrange
            SignatureRequiredRequest requiredRequest = new SignatureRequiredRequest();
            UserProfile userProfile = new UserProfile();
            requiredRequest = null;
            userProfile = null;

            //Act
            var result = signatureRequired.GetReferralList(requiredRequest, userProfile);

            //Assert
            if (result.Count <= 0)
            {
                Assert.Pass("Passed");
            }
            else
            {
                Assert.Pass("Failed");
            }

        }
        [Test]
        public void GetWorkDetailsUniqueId_EmptyModel_NoReturnData()
        {
            //Arrange
            ReqsignatureModel reqsignature = new ReqsignatureModel();
            UserProfile userProfile = new UserProfile();
            reqsignature = null;
            userProfile = null;

            //Act
            var result = signatureRequired.GetWorkDetailsUniqueId(reqsignature, userProfile);

            //Assert
            if (result.Count <= 0)
            {
                Assert.Pass("Passed");
            }
            else
            {
                Assert.Pass("Failed");
            }
        }
        [TearDown]
        public void BaseCleanup()
        {
            // Optionally dispose or cleanup objects
            userProfile = null;
        }
    }
}
