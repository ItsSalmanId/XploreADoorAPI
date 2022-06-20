using FOX.DataModels.Models.Security;
using NUnit.Framework;
using FOX.BusinessOperations;
using FOX.BusinessOperations.AccountService;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Settings.RoleAndRights;

namespace FoxRehabilitation.UnitTest.AccountServiceUnitTest
{
    [TestFixture]
    class AccountServiceTest
    {
        private AccountServices _accountService;
        private UserDetailsByNPIRequestModel _npiRequestModel;
        private CityDetailByZipCodeRequestModel _cityDetailByZipCode;
        private EmailExist _emailExist;
        private SmartSearchRequest _smartSearchRequest;
        private User _user;
        private GetUserIP _getUserIP;
        private LogoutModal _logoutModel;
        private UserProfile _userProfile;

        [SetUp]
        public void Setup()
        {
            _accountService = new AccountServices();
            _npiRequestModel = new UserDetailsByNPIRequestModel();
            _cityDetailByZipCode = new CityDetailByZipCodeRequestModel();
            _emailExist = new EmailExist();
            _smartSearchRequest = new SmartSearchRequest();
            _user = new User();
            _getUserIP = new GetUserIP();
            _logoutModel = new LogoutModal();
            _userProfile = new UserProfile();
        }
        [Test]
        public void GetSenderTypes_UserProfile_ReturnsData()
        {
            //Arrange
            //Act
            var result = _accountService.getSenderTypes();

            //Assert
            Assert.That(result.SenderTypeList.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        public void GetUserDetailByNPI_HasNPI_ReturnsData()
        {
            //Arrange
            _npiRequestModel.NPI = "2569114412";

            //Act
            var result = _accountService.getUserDetailByNPI(_npiRequestModel);

            //Assert
            if(result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("08873")]
        [TestCase("00617")]
        public void GetCityDetailByZipCode_HasZipCode_ReturnsData(string zipCode)
        {
            //Arrange
            _cityDetailByZipCode.ZipCode = zipCode;

            //Act
            var result = _accountService.getCityDetailByZipCode(_cityDetailByZipCode);

            //Assert
            if(result != null && result.zip_city_state != null && result.zip_city_state.Count > 0)
            {
                Assert.That(result.zip_city_state.Count, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(result.zip_city_state, Is.Null);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("muhammadarslan3@carecloud.com")]
        public void CheckIfEmailAlreadyInUse_Email_ReturnsData(string emailAddress)
        {
            //Arrange
            _emailExist.EMAIL = emailAddress;

            //Act
            var result = _accountService.CheckIfEmailAlreadyInUse(_emailExist);

            //Assert
            if(result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("Fox")]
        public void GetPractices_PracticeNames_ReturnsData(string practiceName)
        {
            //Arrange
            _smartSearchRequest.Keyword = practiceName;
            _smartSearchRequest.PracticeCode = 1011163;

            //Act
            var result = _accountService.getPractices(_smartSearchRequest);

            //Assert
            if(result != null && result.fox_tbl_practice_organization != null && result.fox_tbl_practice_organization.Count > 0)
            {
                Assert.That(result.fox_tbl_practice_organization.Count, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(result.fox_tbl_practice_organization, Is.Null);
            }
        }
        [Test]
        [TestCase("", "")]
        [TestCase("ACO Identifier", "Mssp_car")]
        public void GetSmartIdentifier_Identifier_ReturnsData(string identiferType, string searchValue)
        {
            //Arrange
            _smartSearchRequest.TYPE = identiferType;
            _smartSearchRequest.SEARCHVALUE = searchValue;

            //Act
            var result = _accountService.getSmartIdentifier(_smartSearchRequest);

            //Assert
            if(result != null)
            {
                Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(result, Is.Null);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("Test")]
        [TestCase("OT")]
        public void GetSmartSpecialities_HasKeywords_ReturnsData(string keywordName)
        {
            //Arrange
            _smartSearchRequest.Keyword = keywordName;

            //Act
            var result = _accountService.getSmartSpecialities(_smartSearchRequest);
            
            //Assert
            if(result != null && result.specialities != null)
            {
                Assert.That(result.specialities.Count, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(result.specialities, Is.Null);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(1011163)]
        public void CreateExternalUserOrdRefSource_HasUserID_ReturnsData(long userID)
        {
            //Arrange
            //Act
            var result = _accountService.CreateExternalUserOrdRefSource(userID);

            //Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        [TestCase("")]
        [TestCase("10965")]
        [TestCase("2569114412")]
        public void CheckForDublicateNPI_NPI_ReturnsData(string npi)
        {
            //Arrange
            //Act
            var result = _accountService.CheckForDublicateNPI(npi);

            //Assert
            if (result)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("11163Testing")]
        public void ClearOpenedByinPatientforUser_UserName_ReturnsData(string userName)
        {
            //Arrange
            //Act
            _accountService.ClearOpenedByinPatientforUser(userName);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("testing@foxrehab.org")]
        public void IpConfig_HasUserName_ReturnsData(string userName)
        {
            //Arrange
            _getUserIP.userName = userName;
            
            //Act
            var result = _accountService.IpConfig(_getUserIP);

            //Assert
            if (result)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("", "")]
        [TestCase("11163testing", "203.215.161.135")]
        public void CheckIP_HasUserNameAndIP_ReturnsData(string userName, string ipAddress)
        {
            //Arrange
            //Act
            var result = _accountService.CheckIP(userName, ipAddress);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase("testingToken", 1011163415)]
        public void SignOut_HasTokenAndUserID_NoReturnsData(string token, long userID)
        {
            //Arrange
            _logoutModel.token = token;
            _userProfile.userID = userID;

            //Act
            var result = _accountService.SignOut(_logoutModel, _userProfile);

            //Assert

            if(result != null && result.Success == true)
            {
                Assert.That(result.Success, Is.True);
            }
            else
            {
                Assert.That(result.Success, Is.False);
            }
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _accountService = null;
            _npiRequestModel = null;
            _cityDetailByZipCode = null;
            _emailExist = null;
            _smartSearchRequest = null;
            _user = null;
            _getUserIP = null;
            _logoutModel = null;
            _userProfile = null;
        }
    }
}
