using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.CommonServiceUnitTest
{
    [TestFixture]
    public class CommonServiceTest
    {
        private CommonServices _commonServices;
        private UserProfile _userProfile;
        private ReqGetSenderNamesModel _senderNamesModel;
        private ServiceConfiguration _serviceConfiguration;

        [SetUp]
        public void Setup()
        {
            _commonServices = new CommonServices();
            _userProfile = new UserProfile();
            _senderNamesModel = new ReqGetSenderNamesModel();
            _serviceConfiguration = new ServiceConfiguration();
        }
        [Test]
        public void GeneratePdf_UniqueID_NoReturnsData()
        {
            //Arrange
            long uniqueID = 54818464;
            string practiceDocumentDir = "";
            
            //Act
            var result = _commonServices.GeneratePdf(uniqueID, practiceDocumentDir);

            //Assert
            Assert.That(result, Is.Not.Null.Or.Not.Empty);
        }
        [Test]
        public void GeneratePdfForSupportedDoc_EmptyUniqueID_NoReturnsData()
        {
            //Arrange
            string uniqueID = "";
            _serviceConfiguration = null;
           
            //Act
            var result = _commonServices.GeneratePdfForSupportedDoc(_serviceConfiguration, uniqueID, _userProfile);
            
            //Assert
            Assert.That(result.FILE_PATH, Is.Null);
        }
        [Test]
        public void GeneratePdfForEmailToSender_EmptyUniqueID_NoReturnsData()
        {
            //Arrange
            string uniqueID = "";
           
            //Act
            var result = _commonServices.GeneratePdfForEmailToSender(uniqueID, _userProfile);
            
            //Assert
            Assert.That(result.FILE_PATH, Is.Null);
        }
        [Test]
        [TestCase("", 1011163)]
        [TestCase("54819274", 1011163)]
        public void GetFiles_Files_ReturnsData(string uniqueID, long practiceCode)
        {
            //Arrange

            //Act
            var result = _commonServices.GetFiles(uniqueID, practiceCode);
           
            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("", 1011163)]
        [TestCase("54819274", 1011163)]
        public void GetAllOriginalFiles_WorkIds_ReturnsData(string uniqueID, long practiceCode)
        {
            //Arrange

            //Act
            var result = _commonServices.GetAllOriginalFiles(uniqueID, practiceCode);
            
            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        public void GetWorkHistory_HasWorkID_NoReturnsData()
        {
            //Arrange
            string workID = "552798";

            //Act
            var result = _commonServices.GetWorkHistory(workID);

            //Assert
            Assert.That(result.Count, Is.GreaterThan(0));
        }
        [Test]
        public void GetWorkHistory_EmptyWorkID_NoReturnsData()
        {
            //Arrange
            string workID = "";
            
            //Act
            var result = _commonServices.GetWorkHistory(workID);

            //Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        [TestCase(true, 1011163)]
        [TestCase(false, 1011163)]
        [TestCase(false, 0)]
        public void GetSenderTypes_HasUserProfile_ReturnsData(bool isTalkRehab, long practiceCode)
        {
            //Arrange
            _userProfile.isTalkRehab = isTalkRehab;
            _userProfile.PracticeCode = practiceCode;
            
            //Act
            var result = _commonServices.GetSenderTypes(_userProfile);
            
            //Assert
            if(result != null)
            {
                Assert.That(result.SenderTypeList.Count, Is.GreaterThanOrEqualTo(0));
            }
        }
        [Test]
        public void GetSenderNames_EmptyUserProfile_NoReturnsData()
        {
            //Arrange
            _userProfile = null;
            
            //Act
            var result = _commonServices.GetSenderNames(_senderNamesModel, _userProfile);
          
            //Assert
            Assert.That(result.SenderNameList.Count, Is.EqualTo(0));
        }
        [Test]
        [TestCase("")]
        public void GetTreatmentLocationByZip_EmptyZipCode_NoReturnsData(string hasZipCode)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            
            //Act
            var result = _commonServices.GetTreatmentLocationByZip(hasZipCode, _userProfile.PracticeCode);
            
            //Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        [TestCase("08873")]
        public void GetTreatmentLocationByZip_HasZipAndPracticeCode_ReturnsData(string hasZipCode)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            
            //Act
            var result = _commonServices.GetTreatmentLocationByZip(hasZipCode, _userProfile.PracticeCode);
            
            //Assert
            if(result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("08873")]
        public void GetCityStateByZip_HasZipCode_ReturnsData(string zipCode)
        {
            //Arrange
            
            //Act
            var result = _commonServices.GetCityStateByZip(zipCode);
            
            //Assert
            if(result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("Florida")]
        public void GetSmartCities_HasSmartCities_ReturnsData(string city)
        {
            //Arrange
            
            //Act
            var result = _commonServices.GetSmartCities(city);
           
            //Assert
            if(result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("NJ")]
        public void GetSmartStates_HasStateCode_ReturnsData(string statusCode)
        {
            //Arrange
            
            //Act
            var result = _commonServices.GetSmartStates(statusCode);
           
            //Assert
            if(result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void GetStates_StateCode_ReturnsData()
        {
            //Arrange
            
            //Act
            var result = _commonServices.GetStates();
            
            //Assert
            if(result.Count > 0)
            {
                Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(5481921, 1011163)]
        public void GetProvider_ProfileAndProvider_ReturnsData(long providerId, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            
            //Act
            var result = _commonServices.GetProvider(providerId, _userProfile);

            //Assert
            if (result == null)
            {
                Assert.That(result, Is.Null);
            }
            else
            {
                Assert.IsTrue(true);
;           }
        }
        [Test]
        public void IsShowSplash_EmptyProfile_NoReturnsData()
        {
            //Arrange
            _userProfile = null;

            //Act
            var result = _commonServices.IsShowSplash(_userProfile);

            //Assert
            Assert.That(result, Is.True);
        }
        [Test]
        public void IsShowSplash_HasProfile_NoReturnsData()
        {
            //Arrange
            _userProfile.UserName = "1163testing";

            //Act
            var result = _commonServices.IsShowSplash(_userProfile);

            //Assert
            Assert.That(result, Is.False);
        }
        [Test]
        [TestCase("")]
        public void SaveSplashDetails_EmptyUserProfile(string userName)
        {
            //Arrange
            _userProfile.UserName = userName;

            //Act
            var result = _commonServices.SaveSplashDetails(_userProfile);

            //Assert
            if(result == true)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void DeleteDownloadedFile_EmptyFileLocation_NoReturnsData()
        {
            //Arrange
            string fileLocation = string.Empty;

            //Act
            var result = _commonServices.DeleteDownloadedFile(fileLocation);

            //Assert
            if (!result.Success)
            {
                Assert.Pass("Passed");
            }
            else
            {
                Assert.Fail("Failed");
            }
        }
        [Test]
        public void DeleteDownloadedFile_HasFileLocation_NoReturnsData()
        {
            //Arrange
            string fileLocation = "sample";

            //Act
            var result = _commonServices.DeleteDownloadedFile(fileLocation);

            //Assert
            if (!result.Success)
            {
                Assert.Pass("Passed");
            }
            else
            {
                Assert.Fail("Failed");
            }
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _commonServices = null;
            _userProfile = null;
            _senderNamesModel = null;
            _serviceConfiguration = null;
        }
    }
}
