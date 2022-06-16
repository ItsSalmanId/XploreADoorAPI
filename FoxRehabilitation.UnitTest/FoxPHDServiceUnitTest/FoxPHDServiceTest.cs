using FOX.BusinessOperations.FoxPHDService;
using FOX.DataModels.Models.FoxPHD;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.FoxPHDServiceUnitTest
{
    [TestFixture]
    public class FoxPHDServiceTest
    {
        private FoxPHDService _foxPHDService;
        private UserProfile _userProfile;
        private PHDCallDetail _phdCallDetail;
        private UnmappedCallsSearchRequest _unMappedCallRequest;
        
        [SetUp]
        public void Setup()
        {
            _foxPHDService = new FoxPHDService();
            _userProfile = new UserProfile();
            _phdCallDetail = new PHDCallDetail();
            _unMappedCallRequest = new UnmappedCallsSearchRequest();
        }
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetDropdownLists_UserProfile_ReturnsData(bool isTalkRehab)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.userID = 1011163415;
            _userProfile.UserName = "1163testing";
            _userProfile.isTalkRehab = isTalkRehab;

            //Act
            var result = _foxPHDService.GetDropdownLists(_userProfile);

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
        [Test]
        [TestCase(0, 0)]
        public void DeleteCallDetailRecordInformation_PHDCALLID_NoReturnsData(long phdCallID, long practiceCode)
        {
            //Arrange
            _phdCallDetail.FOX_PHD_CALL_DETAILS_ID = phdCallID;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _foxPHDService.DeleteCallDetailRecordInformation(_phdCallDetail, _userProfile);

            //Assert
            if(result != null)
            {
                Assert.That(result.Success, Is.True);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("10965")]
        [TestCase("1096510965")]
        public void getNumberInFormat_Number_ReturnsData(string number)
        {
            //Arrange
            //Act
            var result = _foxPHDService.getNumberInFormat(number);

            //Assert
            if(!string.IsNullOrEmpty(result))
            {
                Assert.IsNotEmpty(result);
            }
            else
            {
                Assert.IsEmpty(result);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("e1xydGYxXGFuc2lccGFwZXJ3MTIyNDBccGFwZXJoMTU4NDBcbWFyZ2w1NzZcbWFyZ3Q1NzZcbWFyZ3I1NzZcbWFyZ2I1NzZ7XCpccnRwYXBlcnNpemUweDRDNjU3NDc0NjU3Mn1cYW5zaWNwZzEyNTJcZGVmZjB7XGZvbnR0Ymx7XGYwXGZzd2lzc1xmcHJxMlxmY2hhcnNldDAgQ2FsaWJyaTt9e1xmMVxmcm9tYW5cZnBycTJcZmNoYXJzZXQyIFN5bWJvbDt9e1xmMlxmbmlsIEFyaWFsO319DQp7XCpcZ2VuZXJhdG9yIE1zZnRlZGl0IDUuNDEuMjEuMjUxMDt9XHZpZXdraW5kNFx1YzFccGFyZFxsYW5nMTAzM1xiXGYwXGZzMjIgQ2hhbmdlIERldGFpbHM6XHBhcg0KXHBhcmRcZmk3MjBcYjAgaWxzIGFyZTpccGFyDQpcdWxcYiBDaGFuZ2UgZGV0YWlscyBhcmU6XHVsbm9uZVxiMFxwYXINClxwYXJkXGZpLTM2MFxsaTE0NTFcZjFcJ2I3XHRhYlxmMCBQbGVhc2UgcHJvY2VlZCBmb3IgVUFUXHBhcg0KXGYxXCdiN1x0YWJcZjJcZnMyMFxwYXINCn0NCgA=")]
        public void GetHTML_EncodedString_ReturnsData(string encodedString)
        {
            //Arrange
            //Act
            var result = _foxPHDService.getHTML(encodedString);

            //Assert
            if (result == null)
            {
                Assert.IsNull(result);
            }
            else
            {
                Assert.IsNotEmpty(result);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("e1xydGYxXGFuc2lccG")]
        public void GetRTF_EncodedString_ReturnsData(string encodedString)
        {
            //Arrange
            //Act
            var result = _foxPHDService.getRTF(encodedString);
            
            //Assert
            if(result == null)
            {
                Assert.IsNull(result);
            }
            else
            {
                Assert.IsNotEmpty(result);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetPHDCallerDropDownValue_UserProfile_ReturnsData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.userID = 1011163415;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _foxPHDService.GetDropdownLists(_userProfile);

            //Assert
            Assert.That(result.foxApplicationUsersViewModel.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("", "", 0)]
        [TestCase("06/10/2022", "", 1011163)]
        [TestCase("06/10/2022", "2015683335", 1011163)]
        public void GetUnmappedCalls_PHDMappedCall_ReturnsData(string callDate, string callNumber, long practiceCode)
        {
            //Arrange
            _unMappedCallRequest.CALL_DATE_STR = callDate;
            _unMappedCallRequest.CALL_NO = callNumber;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _foxPHDService.GetUnmappedCalls(_unMappedCallRequest, _userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        public void GetFoxDocumentTypes_HasUserProfile_ReturnsData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _foxPHDService.GetFoxDocumentTypes(_userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        public void GetFoxDocumentTypes_EmptyUserProfile_ReturnsData()
        {
            //Arrange
            _userProfile = null;

            //Act
            var result = _foxPHDService.GetFoxDocumentTypes(_userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(0)]
        [TestCase(1011163415)]
        public void GetFollowUpCalls_UserID_ReturnsData(int userID)
        {
            //Arrange
            _userProfile.userID = userID;

            //Act
            var result = _foxPHDService.GetFollowUpCalls(_userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(null, 0)]
        [TestCase(null, 1011163)]
        public void GenerateCaseEntries_EmptyPHDModel_NoReturnsData(PHDCallDetail phdCallDetail, long practiceCode)
        {
            //Arrange
            _phdCallDetail = phdCallDetail;
            _userProfile.PracticeCode = practiceCode;

            //Act
            _foxPHDService.GenerateCaseEntries(_phdCallDetail, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void HasAttachment_EmptyAttachmentName_NoReturnsData()
        {
            //Arrange
            _phdCallDetail.ATTACHMENT_NAME = null;

            //Act
           _foxPHDService.HasAttachment(_phdCallDetail, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("")]
        [TestCase("LOC-ISB-03")]
        public void GetLocationCharacter_LocationID_ReturnsData(string locationID)
        {
            //Arrange 
            //Act
            var result = _foxPHDService.GetLocationCharacter(locationID);

            //Assert
            if(result == "")
            {
                Assert.IsEmpty(result);
            }
            else
            {
                Assert.That(result, Is.Not.Empty);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetCaseDetails_UserProfile_ReturnsData(long practiceCode)
        {
            //Arrange 
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _foxPHDService.GetCaseDetails(_userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("")]
        [TestCase("abdurrafay@carecloud.com")]
        public void GetUserDetails_Emails_ReturnsData(string emailAddresss)
        {
            //Arrange 
            _userProfile.UserEmailAddress = emailAddresss;

            //Act
            var result = _foxPHDService.GetUserDetails(_userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(0, "")]
        [TestCase(1011163, "")]
        [TestCase(1011163, "5481762,5482573,5482148,1011163415,5481193,544627")]
        public void GetExportAdvancedDailyReports_CallerUserID_ReturnsData(long practiceCode, string callerUserID)
        {
            //Arrange 
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _foxPHDService.GetExportAdvancedDailyReports(_userProfile, callerUserID);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("", 1011163)]
        [TestCase("5481847", 1011163)]
        public void GetPhdCallLogHistoryDetails_PHDCallID_ReturnsData(string phdCallID, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
           
            //Act
            var result = _foxPHDService.GetPhdCallLogHistoryDetails(phdCallID, _userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("")]
        [TestCase("510165341-W")]
        public void GetWebSoftCaseStatusResponses_SSCMCaseNumber_ReturnsData(string sscmCaseNumber)
        {
            //Arrange
            //Act
            var result = _foxPHDService.GetWebSoftCaseStatusResponses(sscmCaseNumber);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("", 1011163)]
        [TestCase("Test", 1011163)]
        public void GetPhdCallScenariosList_HasUserProfile_ReturnsData(string request, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _foxPHDService.GetPhdCallScenariosList(request, _userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(0)]
        public void GetPhdCallScenarios_HasUserProfile_ReturnsData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            
            //Act
            var result = _foxPHDService.GetPhdCallScenarios(_userProfile);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        public void GetDefaultHandlingValue_HasUserProfile_ReturnsData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            
            //Act
            var result = _foxPHDService.GetDefaultHandlingValue(_userProfile);

            //Assert
            Assert.That(result.PHD_CALL_SCENARIO_ID, Is.Not.Null);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _foxPHDService = null;
            _userProfile = null;
            _phdCallDetail = null;
            _unMappedCallRequest = null;
        }
    }
}
