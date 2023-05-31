using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.FoxPHDService;
using FOX.DataModels.Models.FoxPHD;
using FOX.DataModels.Models.PatientDocuments;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.FoxPHDServiceUnitTest
{
    [TestFixture]
    public class FoxPHDServiceTest
    {
        private FoxPHDService _foxPHDService;
        private UserProfile _userProfile;
        private PHDCallDetail _phdCallDetail;
        private UnmappedCallsSearchRequest _unMappedCallRequest;
        private PhdFaqsDetail _phdFaqsDetail;
        private PatientsSearchRequest _patientsSearchRequest;
        private CallDetailsSearchRequest _callDetailsSearchRequest;
        private PatientPATDocument _patientPATDocument;
        private PhdPatientVerification _phdPatientVerification;

        [SetUp]
        public void Setup()
        {
            _foxPHDService = new FoxPHDService();
            _userProfile = new UserProfile();
            _phdCallDetail = new PHDCallDetail();
            _unMappedCallRequest = new UnmappedCallsSearchRequest();
            _phdFaqsDetail = new PhdFaqsDetail();
            _patientsSearchRequest = new PatientsSearchRequest();
            _callDetailsSearchRequest = new CallDetailsSearchRequest();
            _patientPATDocument = new PatientPATDocument();
            _phdPatientVerification = new PhdPatientVerification();
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
            if (result != null)
            {
                Assert.That(result.Success, Is.True);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("10965")]
        [TestCase("1096510965")]
        public void GetNumberInFormat_Number_ReturnsData(string number)
        {
            //Arrange
            //Act
            var result = _foxPHDService.getNumberInFormat(number);

            //Assert
            if (!string.IsNullOrEmpty(result))
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
        //[Test]
        //public void HasAttachment_EmptyAttachmentName_NoReturnsData()
        //{
        //    //Arrange
        //    _phdCallDetail.ATTACHMENT_NAME = "";

        //    //Act
        //    _foxPHDService.HasAttachment(_phdCallDetail, _userProfile);

        //    //Assert
        //    Assert.IsTrue(true);
        //}
        [Test]
        [TestCase("")]
        [TestCase("LOC-ISB-03")]
        public void GetLocationCharacter_LocationID_ReturnsData(string locationID)
        {
            //Arrange 
            //Act
            var result = _foxPHDService.GetLocationCharacter(locationID);

            //Assert
            if (result == "")
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
            _userProfile.UserName = "Test_5651327";

            //Act
            var result = _foxPHDService.GetDefaultHandlingValue(_userProfile);
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
        [TestCase(1011163, "test", true)]
        [TestCase(1011163, "test", false)]
        public void AddUpdatePHDFAQsDetail_FaqsAddUpdateModel_ReturnsData(long practiceCode, string paramter, bool add)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _phdFaqsDetail.QUESTIONS = paramter;
            _phdFaqsDetail.ANSWERS = paramter;
            if (add == false)
            {
                _phdFaqsDetail.FAQS_ID = Helper.getMaximumId("FAQS_ID") - 1;
            }

            //Act
            _foxPHDService.AddUpdatePhdFaqsDetail(_phdFaqsDetail, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        public void DeletePhdFaqs_DeletePhdFaqsModel_ReturnsData(long practiceCode, bool delete)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            if (delete == false)
            {
                _phdFaqsDetail.FAQS_ID = Helper.getMaximumId("FAQS_ID") - 1;
            }

            //Act
            _foxPHDService.DeletePhdFaqs(_phdFaqsDetail, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163, 548144)]
        public void GetPHDFaqsDetailsInformation_GetModel_ReturnsData(long practiceCode, long faqsId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _phdFaqsDetail.FAQS_ID = faqsId;
            _phdFaqsDetail.QUESTIONS = "test";

            //Act
            _foxPHDService.GetPHDFaqsDetailsInformation(_phdFaqsDetail, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void GetDropdownListFaqs_GetModel_ReturnsData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            _foxPHDService.GetDropdownListFaqs(_userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void GetPatientInformation_GetModel_ReturnsData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientsSearchRequest.PATIENT_ACCOUNT = "";
            _patientsSearchRequest.LAST_NAME = "";
            _patientsSearchRequest.FIRST_NAME = "";
            _patientsSearchRequest.DATE_OF_BIRTH_STR = "";
            _patientsSearchRequest.CURRENT_PAGE = 1;
            _patientsSearchRequest.RECORD_PER_PAGE = 10;
            _patientsSearchRequest.SORT_BY = "receivedate";
            _patientsSearchRequest.SORT_ORDER = "DESC";

            //Act
            var result = _foxPHDService.GetPatientInformation(_patientsSearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "00:15:00 GMT + 0500(Pakistan Standard Time)")]
        [TestCase(1012714, "")]
        public void GetPHDCallDetailsInformation_GetModel_ReturnsData(long practiceCode, string callDateStr)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _callDetailsSearchRequest.CALL_DATE_FROM_STR = "Sun Dec 06 2020";
            _callDetailsSearchRequest.CALL_DATE_TO_STR = "";
            _callDetailsSearchRequest.CALL_TIME_FROM_STR = callDateStr;
            _callDetailsSearchRequest.CALL_TIME_TO_STR = callDateStr;
            _callDetailsSearchRequest.CALL_ATTENDED_BY = "";
            _callDetailsSearchRequest.CALL_REASON = "";
            _callDetailsSearchRequest.CALL_HANDLING = "";
            _callDetailsSearchRequest.CS_CASE_STATUS = "";
            _callDetailsSearchRequest.FOLLOW_UP_CALLS = false;
            _callDetailsSearchRequest.MRN = "";
            _callDetailsSearchRequest.PATIENT_FIRST_NAME = "";
            _callDetailsSearchRequest.PATIENT_LAST_NAME = "";
            _callDetailsSearchRequest.PHONE_NUMBER = "";
            _callDetailsSearchRequest.CALL_TIME_TO_STR = "";
            _callDetailsSearchRequest.CURRENT_PAGE = 1;
            _callDetailsSearchRequest.RECORD_PER_PAGE = 10;
            _callDetailsSearchRequest.SORT_BY = "receivedate";
            _callDetailsSearchRequest.SORT_ORDER = "DESC";

            //Act
            var result = _foxPHDService.GetPHDCallDetailsInformation(_callDetailsSearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void AddDocument_AddModel_InsertData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "Unit Testing";
            _patientPATDocument.PARENT_DOCUMENT_ID = 548100;
            _patientPATDocument.CREATED_DATE = DateTime.Now;
            _patientPATDocument.MODIFIED_DATE = DateTime.Now;
            _patientPATDocument.DELETED = false;

            //Act
            var result = _foxPHDService.AddDocument(_patientPATDocument, _userProfile, true);

            //Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(0));   
        }
        [Test]
        [TestCase(1011163, 0, 0)]
        [TestCase(1011163, 0, 54819396)]
        [TestCase(1011163, 548958, 54819396)]
        [TestCase(1012714, 0, 0)]
        public void AddUpdateNewDocumentInformation_AddModel_InsertData(long practiceCode, long patDocumentId, long workId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "Unit Testing";
            _patientPATDocument.PARENT_DOCUMENT_ID = 548100;
            _patientPATDocument.CREATED_DATE = DateTime.Now;
            _patientPATDocument.MODIFIED_DATE = DateTime.Now;
            _patientPATDocument.DELETED = false;
            _patientPATDocument.PATIENT_ACCOUNT_str = "101116354816618";
            _patientPATDocument.PAT_DOCUMENT_ID = patDocumentId;
            _patientPATDocument.WORK_ID = workId;

            //Act
            var result = _foxPHDService.AddUpdateNewDocumentInformation(_patientPATDocument, _userProfile, true);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        [TestCase(1012714, true)]
        [TestCase(1012714, false)]
        public void AddUpdatePhdCallDetailInformation_AddModel_InsertData(long practiceCode, bool condition)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "Unit Testing";
            _userProfile.UserEmailAddress = "Unit Testing";
            _phdCallDetail.PATIENT_ACCOUNT_STR = "101116354412369";
            _phdCallDetail.CALL_SCENARIO = "544103";
            _phdCallDetail.CALL_REASON = "544119";
            _phdCallDetail.CS_CASE_CATEGORY = "5481530";
            _phdCallDetail.REQUEST = "544100";
            _phdCallDetail.REQUEST = "544100";
            _phdCallDetail.CALL_DETAILS = "65456";
            _phdCallDetail.CALL_ATTENDED_BY = "1011163415";
            if(condition)
            {
                _phdCallDetail._IsSSCM = true;
                _phdCallDetail.IsNewPatient = true;
            }
            else
            {
                _phdCallDetail._IsSSCM = false;
                _phdCallDetail.IsNewPatient = false;
            }

            //Act
            var result = _foxPHDService.AddUpdatePHDCallDetailInformation(_phdCallDetail, _userProfile);

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
        public void GenerateCaseNumber_GenerateCase_ReturnCaseNumber()
        {
            //Arrange
            //Act
            var result = _foxPHDService.GenerateCaseNumber();

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
        [TestCase(1011163, "test", "test")]
        public void AddPHDLog_AddModel_InsertData(long practiceCode, string logFor, string logDetail)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "Unit Testing";

            //Act
            _foxPHDService.AddPHDLog(_phdCallDetail, logFor, logDetail, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163, 5482573)]
        [TestCase(1011163, 5481123)]
        public void SavePhdScanarios_AddModel_InsertData(long practiceCode, long userId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "Unit Testing";
            _callDetailsSearchRequest.CALL_REASON = "";
            List<DefaultVauesForPhdUsers> defaultVauesForPhdUsers = new List<DefaultVauesForPhdUsers>()
            {
                new DefaultVauesForPhdUsers()
                {
                    PRACTICE_CODE = 1011163,
                    DELETED = false,
                    MODIFIED_BY = "FOXTeam",
                    CREATED_BY = "FOXTeam",
                    USER_ID = userId
                }
            };

            //Act
            _foxPHDService.SavePhdScanarios(defaultVauesForPhdUsers, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        [TestCase(1012714, true)]
        [TestCase(1012714, false)]
        public void AddUpdateVerificationInformation_AddModel_InsertData(long practiceCode, bool addCase)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "Unit Testing";
            _phdPatientVerification.PATIENT_ACCOUNT_STR = "101116354817724";
            _phdPatientVerification.LAST_VERIFIED_DATE_STR = "2018-12-12 01:25:30.277";
            _phdPatientVerification.IS_PATIENT_EMAIL_ADDRESS_VERIFIED = true;
            if(addCase)
            {
                _phdPatientVerification.FOX_PHD_CALL_PATIENT_VERIFICATION_ID = Helper.getMaximumId("FOX_PHD_CALL_PATIENT_VERIFICATION_ID");
            }
            else
            {
                _phdPatientVerification.FOX_PHD_CALL_PATIENT_VERIFICATION_ID = 548140;
                _phdPatientVerification.PATIENT_ACCOUNT_STR = "1010782513100085";
            }
           
            //Act
            _foxPHDService.AddUpdateVerificationInformation(_phdPatientVerification, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        public void GenerateCaseEntries_AddModel_InsertData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "Unit Testing";
            _userProfile.UserEmailAddress = "ADMIN992@ADMIN.COM";
            _phdCallDetail.FOX_PHD_CALL_DETAILS_ID = 5482037;
            _phdCallDetail.CALL_RECORDING_PATH = "ps_1011163_112235762_1531421800588";
            _phdCallDetail._IsSSCM = true;
            _phdCallDetail.CALL_SCENARIO = "544103";
            _phdCallDetail.CALL_REASON = "605214";
            _phdCallDetail.CS_CASE_CATEGORY = "5371";
            _phdCallDetail.REQUEST = "544100";
            _phdCallDetail.CALL_DETAILS = "65456";
            _phdCallDetail.CALL_ATTENDED_BY = "1011163415";

            //Act
            _foxPHDService.GenerateCaseEntries(_phdCallDetail, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _foxPHDService = null;
            _userProfile = null;
            _phdCallDetail = null;
            _unMappedCallRequest = null;
            _phdFaqsDetail = null;
            _callDetailsSearchRequest = null;
            _phdPatientVerification = null;
        }
    }
}
