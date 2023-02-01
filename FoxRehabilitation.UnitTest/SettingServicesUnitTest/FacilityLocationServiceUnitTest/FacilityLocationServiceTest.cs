using FOX.BusinessOperations.SettingsService.FacilityLocationService;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.SettingsServiceUnitTest.FacilityLocationServiceUnitTest
{
    [TestFixture]
    public class FacilityLocationServiceTest
    {
        private FacilityLocationService _facilityLocationService;
        private UserProfile _userProfile;
        private FacilityLocationSearch _facilityLocationSearch;
        private LocationPatientAccount _locationPatientAccount;
        private FacilityLocation _facilityLocation;
        private GroupIdentifierSearch _groupIdentifierSearch;
        private LocationCorporationSearch _locationCorporationSearch;
        private IdentifierSearch _identifierSearch;
        private AuthStatusSearch _authStatusSearch;
        private TaskTpyeSearch _taskTpyeSearch;
        private OrderStatusSearch _orderStatusSearch;

        [SetUp]
        public void SetUp()
        {
            _facilityLocationService = new FacilityLocationService();
            _userProfile = new UserProfile();
            _facilityLocationSearch = new FacilityLocationSearch();
            _locationPatientAccount = new LocationPatientAccount();
            _facilityLocation = new FacilityLocation();
            _groupIdentifierSearch = new GroupIdentifierSearch();
            _locationCorporationSearch = new LocationCorporationSearch();
            _identifierSearch = new IdentifierSearch();
            _authStatusSearch = new AuthStatusSearch();
            _taskTpyeSearch = new TaskTpyeSearch();
            _orderStatusSearch = new OrderStatusSearch();
        }
        [Test]
        public void GetFacilityLocationList_GetFacilityLocationListModel_NoReturnData()
        {
            //Arrange
            _userProfile.userID = 1011163415;
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _facilityLocationSearch.currentPage = 1;
            _facilityLocationSearch.recordPerpage = 1;
            _facilityLocationSearch.searchString = "";
            _facilityLocationSearch.Code = "";
            _facilityLocationSearch.Description = "";
            _facilityLocationSearch.zip = "";
            _facilityLocationSearch.city = "";
            _facilityLocationSearch.state = "";
            _facilityLocationSearch.currentPage = 1;
            _facilityLocationSearch.FacilityType = 1;
            _facilityLocationSearch.sortBy = "";
            _facilityLocationSearch.sortOrder = "";
            _facilityLocationSearch.Complete_Address = "";
            _facilityLocationSearch.Region = "";
            _facilityLocationSearch.Country = "";

            //Act
            var result = _facilityLocationService.GetFacilityLocationList(_facilityLocationSearch, _userProfile);

            //Assert
            if (result != null && result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("1011163", 398, "101116354816561")]
        [TestCase("1011163", 6002564, "101116354816561")]
        [TestCase("1011163", 583, "101116354816561")]
        [TestCase("1011163", 6002595, "101116354412309")]
        [TestCase("5483663", 600126, "101116354815314")]

        public void GetFacilityLocationById_ReturnFacilityLocationById_ReturnData(string practiceCode, long Location_id, string PATIENT_ACCOUNT)
        {
            //Arrange
            _facilityLocation.CODE = "";
            _facilityLocation.COMPLETE_ADDRESS = "";
            _facilityLocation.Country = "";
            _facilityLocation.Description = "";
            _facilityLocation.FACILITY_TYPE_ID = 0;
            _facilityLocation.REGION = "";
            _facilityLocation.State = "";
            _facilityLocation.Zip = "";
            _facilityLocation.PATIENT_ACCOUNT = 0;
            _facilityLocation.PATIENT_ACCOUNT = 0;
            _facilityLocation.PATIENT_ACCOUNT = 0;
            _locationPatientAccount.Location_id = Location_id;
            _locationPatientAccount.PATIENT_ACCOUNT = PATIENT_ACCOUNT;

            //Act
            var result = _facilityLocationService.GetFacilityLocationById(_locationPatientAccount, practiceCode);

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
        [TestCase("", "1011163")]
        [TestCase("test", "1011163")]
        [TestCase("test", "000000")]
        public void GetProviderNamesList_ProviderNamesListModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _facilityLocationService.GetProviderNamesList(searchText, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("", 1011163)]
        public void GetProviderCode_ProviderCodeListCount_ReturnData(string state, long practiceCode)
        {
            //Arrange
            //Act
            var result = _facilityLocationService.GetProviderCode(state, practiceCode);

            //Assert
            if (result != null && result != "001")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(544100, "1011163")]
        [TestCase(544102, "1011163")]
        [TestCase(123456, "000000")]
        public void GetFacilityTypeById_FacilityTypeByIdModel_ReturnData(long facilityTypeId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _facilityLocationService.GetFacilityTypeById(facilityTypeId, practiceCode);

            //Assert
            if (result != null && result.DELETED == false)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("", "1011163")]
        [TestCase("test", "1011163")]
        [TestCase(null, "1011163")]
        public void GetSmartReferralRegions_SmartReferralRegionsModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _facilityLocationService.GetSmartReferralRegions(searchText, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("1011163")]
        [TestCase("000000")]
        public void GetGroupIdentifierList_GroupIdentifierListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _groupIdentifierSearch.searchString = "";
            _groupIdentifierSearch.Name = "";
            _groupIdentifierSearch.Description = "";
            _groupIdentifierSearch.currentPage = 1;
            _groupIdentifierSearch.recordPerpage = 1;
            _groupIdentifierSearch.sortBy = "";
            _groupIdentifierSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetGroupIdentifierList(_groupIdentifierSearch, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
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
        [TestCase(000000)]
        public void LocationCorporationList_LocationCorporationListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _locationCorporationSearch.searchString = "";
            _locationCorporationSearch.Name = "";
            _locationCorporationSearch.Code = "";
            _locationCorporationSearch.currentPage = 1;
            _locationCorporationSearch.recordPerpage = 1;
            _locationCorporationSearch.sortBy = "";
            _locationCorporationSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.LocationCorporationList(_locationCorporationSearch, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
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
        [TestCase(000000)]
        public void GetIdentifierList_IdentifierListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _identifierSearch.searchString = "test";
            _identifierSearch.Name = "";
            _identifierSearch.Code = "";
            _identifierSearch.currentPage = 1;
            _identifierSearch.recordPerpage = 1;
            _identifierSearch.sortBy = "";
            _identifierSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetIdentifierList(_identifierSearch, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
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
        [TestCase(000000)]
        public void GetAuthStatusList_AuthStatusListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _authStatusSearch.searchString = "";
            _authStatusSearch.Description = "Active";
            _authStatusSearch.Code = "ACT";
            _authStatusSearch.currentPage = 1;
            _authStatusSearch.recordPerpage = 1;
            _authStatusSearch.sortBy = "";
            _authStatusSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetAuthStatusList(_authStatusSearch, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
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
        [TestCase(000000)]
        public void GetTaskTypeList_TaskTypeListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _taskTpyeSearch.searchString = "";
            _taskTpyeSearch.Name = "Insurance Follow-up";
            _taskTpyeSearch.Code = "ACT";
            _taskTpyeSearch.currentPage = 1;
            _taskTpyeSearch.recordPerpage = 1;
            _taskTpyeSearch.sortBy = "";
            _taskTpyeSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetTaskTypeList(_taskTpyeSearch, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
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
        [TestCase(000000)]
        public void GetOrderStatusList_OrderStatusListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _orderStatusSearch.searchString = "";
            _orderStatusSearch.Description = "Insurance Follow-up";
            _orderStatusSearch.Code = "ACT";
            _orderStatusSearch.currentPage = 1;
            _orderStatusSearch.recordPerpage = 1;
            _orderStatusSearch.sortBy = "";
            _orderStatusSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetOrderStatusList(_orderStatusSearch, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
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
            _userProfile = null;
            _facilityLocationService = null;
            _facilityLocationSearch = null;
            _locationPatientAccount = null;
            _groupIdentifierSearch = null;
            _identifierSearch = null;
            _authStatusSearch = null;
            _taskTpyeSearch = null;
            _orderStatusSearch = null;
        }
    }
}
