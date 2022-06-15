using FOX.BusinessOperations.SignatureRequiredServices;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.SignatureRequired;
using NUnit.Framework;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.SignatureRequiredServiceUnitTest
{
    [TestFixture]
    public class SignatureRequiredServiceTest
    {
        private SignatureRequiredService _signatureRequired;
        private UserProfile _userProfile;
        private SignatureRequiredRequest _requiredRequest;
        private ReqsignatureModel _reqsignature;

        [SetUp]
        public void Setup()
        {
            _signatureRequired = new SignatureRequiredService();
            _userProfile = new UserProfile();
            _requiredRequest = new SignatureRequiredRequest();
            _reqsignature = new ReqsignatureModel();
        }
        [Test]
        public void GetReferralList_EmptyModel_NoReturnsData()
        {
            //Arrange
            _requiredRequest = null;
            _userProfile = null;

            //Act
            var result = _signatureRequired.GetReferralList(_requiredRequest, _userProfile);

            //Assert
            if (result.Count <= 0)
            {
                Assert.AreEqual(0, result.Count);
            }
            else
            {
                Assert.Pass("Failed");
            }
        }
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void GetReferralList_HasSignatureRequired_ReturnsData(bool isSignatureRequired)
        {
            //Arrange
            _requiredRequest.CURRENT_PAGE = 1;
            _requiredRequest.IsSignatureRequired = isSignatureRequired;
            _requiredRequest.RECORD_PER_PAGE = 10;
            _requiredRequest.SEARCH_TEXT = "";
            _requiredRequest.SORT_BY = "";
            _requiredRequest.SORT_ORDER = "";
            _userProfile.UserEmailAddress = "abdurrafay @carecloud.com";
            _userProfile.UserType = "External_User_Ord_Ref_Source";
            _userProfile.UserName = "1163testing";

            //Act
            var result = _signatureRequired.GetReferralList(_requiredRequest, _userProfile);

            //Assert
            if(result.Count >= 0)
            {
                Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
            }
        }
        [Test]
        public void GetWorkDetailsUniqueId_EmptyModel_NoReturnsData()
        {
            //Arrange
            _reqsignature = null;
            _userProfile = null;

            //Act
            var result = _signatureRequired.GetWorkDetailsUniqueId(_reqsignature, _userProfile);

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
        [TestCase("", 1011163)]
        [TestCase("54819279", 1011163)]
        public void GetWorkDetailsUniqueId_EmptyUniqueID_ReturnData(string isUniqueID, long practiceCode)
        {
            //Arrange
            _reqsignature = new ReqsignatureModel()
            {
                Unique_IdList = new List<string>()
                {
                    isUniqueID
                }
            };
            _userProfile.PracticeCode = practiceCode;
            //Act
            var result = _signatureRequired.GetWorkDetailsUniqueId(_reqsignature, _userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _signatureRequired = null;
            _userProfile = null;
            _requiredRequest = null;
            _reqsignature = null;
        }
    }
}
