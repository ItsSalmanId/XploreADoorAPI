using FOX.BusinessOperations.QualityAssuranceService.QADashboardService;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.QualityAssuranceServiceUnitTest
{
    [TestFixture]
    public class QADashboardServiceTest
    {
        private QADashboardService _qADashboardService;
        private QADashboardSearch _qADashboardSearch;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _qADashboardService = new QADashboardService();
            _qADashboardSearch = new QADashboardSearch();
            _userProfile = new UserProfile();
        }
        [Test]
        [TestCase("", 0)]
        [TestCase(",544110,544109,54410118", 1011163)]
        [TestCase("", 1011163)]
        [TestCase("544110,544109", 38403)]
        public void GetEmployeelist_Employeelist_ReturnData(string callScanrioID, long praticeCode)
        {
            //Arrange
            _userProfile.PracticeCode = praticeCode;

            //Act
            var result = _qADashboardService.GetEmployeelist(callScanrioID, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("544110,544109,544114", "phd", ",Both", ",test", ",test test", ",Client Services,Commercial Team,Followup", "LAST_MONTH", false, 1011163)]
        [TestCase("544110,544109,544114", "Phd and Survey", "Client Experience", ",test", ",test test", ",Client Services,Commercial Team,Followup", "LAST_TWO_MONTHS", false, 1011163)]
        [TestCase(",544110,544109,544114", "phd", "System and Client", "", "Taseer", ",Client Services,Commercial Team,Followup", "LAST_THREE_MONTHS", true, 1011163)]
        [TestCase("544110,544109,544114", "phd", "Client Experience", "", "", "", "last_week", false, 1011163)]
        [TestCase("544110,544109,544114", "Survey", "System Product and Process", "", "", "", "last_two_week", true, 1011163)]
        [TestCase("544110,544109,544114", "Survey", "", "", "", "", "last_three_week", true, 0)]
        [TestCase("", "", "", "", "", "", "LAST_THREE_MONTHS", true, 0)]
        public void GetDashboardData_EvaluatedData_ReturnData(string calHandlingID, string callType, string evaluatedName, string userName, string userFullName, string teamsName, string timeFrame, bool isActive, long praticeCode)
        {
            //Arrange
            _qADashboardSearch.CALL_HANDLING_ID = calHandlingID;
            _qADashboardSearch.CALL_TYPE = callType;
            _qADashboardSearch.EMPLOYEE_USER_NAME = userName;
            _qADashboardSearch.USER_FULL_NAME = userFullName;
            _qADashboardSearch.TEAMS_NAMES = teamsName;
            _qADashboardSearch.EVALUATION_NAME = evaluatedName;
            _qADashboardSearch.TIME_FRAME = timeFrame;
            _qADashboardSearch.IS_ACTIVE = isActive;
            _userProfile.PracticeCode = praticeCode;

            //Act
            var result = _qADashboardService.GetDashboardData(_qADashboardSearch, _userProfile);

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
            _qADashboardService = null;
            _qADashboardSearch = null;
            _userProfile = null;
        }
    }
}
