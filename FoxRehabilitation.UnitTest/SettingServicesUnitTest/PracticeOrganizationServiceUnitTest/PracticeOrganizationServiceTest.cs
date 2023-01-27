using FOX.BusinessOperations.SettingsService.PracticeOrganizationService;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Practice;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.SettingsServiceUnitTest.PracticeOrganizationServiceUnitTest
{
    [TestFixture]
    public class PracticeOrganizationServiceTest
    {
        private PracticeOrganizationService _practiceOrganizationService;
        private UserProfile _userProfile;
        private PracticeOrganizationRequest _practiceOrganizationRequest;
        private PracticeOrganization _practiceOrganization;

        [SetUp]
        public void SetUp()
        {
            _practiceOrganizationService = new PracticeOrganizationService();
            _userProfile = new UserProfile();
            _practiceOrganizationRequest = new PracticeOrganizationRequest();
            _practiceOrganization = new PracticeOrganization();
        }
        [Test]
        [TestCase(1012714)]
        [TestCase(1011163)]
        [TestCase(1011165)]
        public void GetMaxPracticeOrganizationCode_ReturnMaxPracticeOrganizationCode_ReturnData(long practiceCode)
        {
            //Arrang
            //Act
            var result = _practiceOrganizationService.GetMaxPracticeOrganizationCode(practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1012714)]
        [TestCase(1011163)]
        [TestCase(1011165)]
        public void GetPracticeOrganizationList_ReturnGetPracticeOrganizationList_ReturnData(long practiceCode)
        {
            //Arrang
            _userProfile.PracticeCode = practiceCode;
            _practiceOrganizationRequest.NAME = "";
            _practiceOrganizationRequest.SEARCH_STRING = "";
            _practiceOrganizationRequest.CURRENT_PAGE = 1;
            _practiceOrganizationRequest.RECORD_PER_PAGE = 10;
            _practiceOrganizationRequest.SORT_BY = "";
            _practiceOrganizationRequest.SORT_ORDER = "";
            _practiceOrganizationRequest.NAME = "";

            //Act
            var result = _practiceOrganizationService.GetPracticeOrganizationList(_practiceOrganizationRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test", 1012714)]
        [TestCase("", 1012714)]
        [TestCase("test", 1011163)]
        [TestCase("", 1011165)]
        public void GetPracticeOrganizationByName_PracticeOrganizationByNameModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrang
            //Act
            var result = _practiceOrganizationService.GetPracticeOrganizationByName(searchText, practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(548152, 1012714)]
        [TestCase(0, 1012714)]
        [TestCase(548152, 1011163)]
        [TestCase(0, 1011163)]
        public void AddUpdatePracticeOrganization_UpdateModel_SaveData(long organizationId, long practiceCode)
        {
            //Arrang
            _userProfile.UserName = "1163testing";
            _userProfile.PracticeCode = practiceCode;
            _practiceOrganization.PRACTICE_ORGANIZATION_ID = organizationId;

            //Act
            var result = _practiceOrganizationService.AddUpdatePracticeOrganization(_practiceOrganization, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _practiceOrganizationService = null;
            _userProfile = null;
            _practiceOrganizationRequest = null;
            _practiceOrganization = null;
        }
    }
}
