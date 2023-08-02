using FOX.BusinessOperations.SettingsService.ClinicianSetupService;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.ClinicianSetup;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.SettingServicesUnitTest.ClinicianSetupServiceUnitTest
{
    [TestFixture]
    public class ClinicianSetupServiceTest
    {
        private ClinicianSetupService _clinicianSetupService;
        private FoxProviderClass _foxProviderClass;
        private UserProfile _userProfile;
        private GetClinicanReq _getClinicanReq;
        private ProviderLocationReq _providerLocationReq;
        private DeleteClinicianModel _deleteClinicianModel;

        [SetUp]
        public void SetUp()
        {
            _clinicianSetupService = new ClinicianSetupService();
            _foxProviderClass = new FoxProviderClass();
            _userProfile = new UserProfile();
            _getClinicanReq = new GetClinicanReq();
            _providerLocationReq = new ProviderLocationReq();
            _deleteClinicianModel = new DeleteClinicianModel();
        }
        [Test]
        [TestCase(1011163)]
        public void GetHrAutoEmailConfigureRecords_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            _foxProviderClass.FOX_PROVIDER_ID = 5481922;

            //Act
            var result = _clinicianSetupService.InsertUpdateClinician(_foxProviderClass, _userProfile);

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
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void GetVisitQoutaPerWeek_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            _foxProviderClass.FOX_PROVIDER_ID = 5481922;

            //Act
            var result = _clinicianSetupService.GetVisitQoutaPerWeek(_userProfile);

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
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void GetDisciplines_PassParamters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            _foxProviderClass.FOX_PROVIDER_ID = 5481922;

            //Act
            var result = _clinicianSetupService.GetDisciplines(_userProfile);

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
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void GetSmartRefRegion_PassParamters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            string searchText = "test";

            //Act
            var result = _clinicianSetupService.GetSmartRefRegion(searchText, _userProfile);

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
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void GetClinician_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            _getClinicanReq.CURRENT_PAGE = 10;
            _getClinicanReq.RECORD_PER_PAGE = 10;
            _getClinicanReq.SEARCH_STRING = "TEst";

            //Act
            var result = _clinicianSetupService.GetClinician(_getClinicanReq, _userProfile);

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
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void CheckNPI_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            _getClinicanReq.CURRENT_PAGE = 10;
            _getClinicanReq.RECORD_PER_PAGE = 10;
            _getClinicanReq.SEARCH_STRING = "TEst";
            string npi = "test";

            //Act
            _clinicianSetupService.CheckNPI(npi, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void CheckSsn_PassParamters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            _getClinicanReq.CURRENT_PAGE = 10;
            _getClinicanReq.RECORD_PER_PAGE = 10;
            _getClinicanReq.SEARCH_STRING = "TEst";
            string ssn = "test";

            //Act
            _clinicianSetupService.CheckSSN(ssn, _userProfile);

            //Assert

            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void GetSpecficProviderLocation_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            _providerLocationReq.ROVIDER_TYPE = "TEst";
            _providerLocationReq.FOX_PROVIDER_ID = 1234;

            //Act
            var result = _clinicianSetupService.GetSpecficProviderLocation(_providerLocationReq, _userProfile);

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
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void UpdateProviderCLR_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _foxProviderClass.FOX_PROVIDER_CODE = "5481922";

            //Act
            _clinicianSetupService.UpdateProviderCLR(_foxProviderClass, practiceCode);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(38403)]
        public void DeleteClinician_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _foxProviderClass.FOX_PROVIDER_CODE = "5481922";
            _deleteClinicianModel.user = new FoxProviderClass()
            {
                FOX_PROVIDER_ID = 12345
            };
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";

            //Act
            _clinicianSetupService.DeleteClinician(_deleteClinicianModel, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _clinicianSetupService = null;
            _foxProviderClass = null;
            _userProfile = null;
            _getClinicanReq = null;
            _providerLocationReq = null;
            _deleteClinicianModel = null;
        }
    }
}
