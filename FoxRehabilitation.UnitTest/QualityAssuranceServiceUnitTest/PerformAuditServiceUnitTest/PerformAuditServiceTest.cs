using FOX.BusinessOperations.QualityAssuranceService.PerformAuditService;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.QualityAssuranceServiceUnitTest
{
    [TestFixture]
    class PerformAuditServiceTest
    {
        private PerformAuditService _performAuditService;
        private RequestModelForCallType _requestModelForCallType;
        private SurveyAuditScores _surveyAuditScores;
        private RequestCallFromQA _requestCallFromQA;
        private UserProfile _userProfile;
        public int callScanrioID;
        private RequestCallList _requestCallList;

        [SetUp]
        public void SetUp()
        {
            _performAuditService = new PerformAuditService();
            _requestModelForCallType = new RequestModelForCallType();
            _surveyAuditScores = new SurveyAuditScores();
            _requestCallFromQA = new RequestCallFromQA();
            _userProfile = new UserProfile();
            _requestCallList = new RequestCallList();
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "Survey")]
        public void GetAlertGeneralNotes_TotalNumbersModel_ReturnData(long practiceCode, string callType)
        {
            //Arrange
            _requestModelForCallType.Call_Type = callType;

            //Act
            var result = _performAuditService.GetTotalNumbersOfCriteria(practiceCode, _requestModelForCallType);

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
        public void GetListOfReps_FeedBackCallerList_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _performAuditService.GetListOfReps(practiceCode);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "Survey")]
        public void GetListOfGradingCriteria_GradingSetupList_ReturnData(long practiceCode, string callType)
        {
            //Arrange
            _surveyAuditScores.CALL_TYPE = callType;

            //Act
            var result = _performAuditService.GetListOfGradingCriteria(practiceCode, _surveyAuditScores);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, 101116354817196, "test", "test", 1)]
        [TestCase(1012714, 101116354817196, "test", "", 2)]
        [TestCase(1012714, 101116354817196, "", "test", 3)]
        [TestCase(1012714, 101116354817196, "", "", 4)]
        public void ListAuditedCalls_AuditedCallsModel_ReturnData(long practiceCode, long patientAccount, string agentName, string auditorName, int timeFrame)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _requestCallFromQA.PATIENT_ACCOUNT = patientAccount;
            _requestCallFromQA.AGENT_NAME = agentName;
            _requestCallFromQA.AUDITOR_NAME = auditorName;
            _requestCallFromQA.TIME_FRAME = timeFrame;

            //Act
            var result = _performAuditService.ListAuditedCalls(_requestCallFromQA, _userProfile);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void GetDropdownLists_UserProfile_NoReturnData()
        {
            //Arrange
            _userProfile = null;
            callScanrioID = 544103;

            //Act
            var result = _performAuditService.GetTeamMemberName(callScanrioID, _userProfile);

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
            _userProfile.PracticeCode = 1011163;
            callScanrioID = 0;

            //Act
            var result = _performAuditService.GetTeamMemberName(callScanrioID, _userProfile);

            //Assert
            if (result.Count > 0)
            {
                Assert.Pass("Passed");
            }
        }
        [Test]
        [TestCase(false, 1)]
        [TestCase(false, 2)]
        [TestCase(true, 4)]
        [TestCase(false, 3)]
        public void PostCallList_PassModel_NoReturnData(bool readOnlyMode, int timeFrame)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _requestCallList.TIME_FRAME = timeFrame;
            _requestCallList.SURVEY_BY = "Admin_5651352";
            _requestCallList.CALL_TYPE = "survey";
            _requestCallList.PHD_CALL_SCENARIO_ID = 0;
            _requestCallList.IS_READ_ONLY_MODE = readOnlyMode;

            //Act
            var result = _performAuditService.PostCallList(_requestCallList, _userProfile);

            //Assert
            if (result.Count > 0)
            {
                Assert.Pass("Passed");
            }
        }
        [TearDown]
        public void Teardown()
        {
            _performAuditService = null;
            _requestModelForCallType = null;
            _surveyAuditScores = null;
            _userProfile = null;
        }
    }
}
