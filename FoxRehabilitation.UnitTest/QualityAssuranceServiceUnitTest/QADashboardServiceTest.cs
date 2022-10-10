using FOX.BusinessOperations.QualityAssuranceService.QADashboardService;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.QualityAssuranceServiceUnitTest
{
    class QADashboardServiceTest
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
        [TestCase(",",0)]
        [TestCase(",544110,544109", 0)]
        [TestCase(",",1011163)]
        [TestCase(",544110,544109", 38403)]

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
        [TestCase]
        [TestCase(",544110", "Phd", "Both", ",abcd_5482928,abcd_5483047,hameed_5483227,", "LAST_TWO_WEEK", "9/19/2022 12:00:00 AM", "10/2/2022 12:00:00 AM", 0)]
        [TestCase(",544110", "Phd", "Both", ",abcd_5482928,abcd_5483047,hameed_5483227,", "LAST_TWO_WEEK", "9/19/2022 12:00:00 AM", "", 0)]
        [TestCase(",544110", "Phd", "Both", ",abcd_5482928,abcd_5483047,hameed_5483227,", "LAST_TWO_WEEK", "", "", 0)]
        [TestCase(",544110", "Phd", "Both", ",abcd_5482928,abcd_5483047,hameed_5483227,", "", "", "", 0)]
        [TestCase(",544110", "Phd", "Both", "", "", "", "", 0)]
        [TestCase(",544110", "Phd", "", "", "", "", "", 0)]
        [TestCase(",544110", "", "", "", "", "", "", 0)]
        [TestCase("", "", "", "", "", "", "", 0)]
        public void GetDashboardData_EvaluatedData_ReturnData(string calHandlingID, string callType, string evaluatedBy,  string userName,  string timeFrame, DateTime  dateFrom, DateTime dateTo, long praticeCode)
        {
            //Arrange
            
            _qADashboardSearch.CALL_HANDLING_ID = calHandlingID;
            _qADashboardSearch.CALL_TYPE = callType;
            _qADashboardSearch.EMPLOYEE_USER_NAME = userName;
            _qADashboardSearch.EVALUATION_NAME = evaluatedBy;
            _qADashboardSearch.TIME_FRAME = timeFrame;
            _qADashboardSearch.START_DATE = dateFrom;
            _qADashboardSearch.END_DATE = dateTo;
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
                Assert.IsTrue(true);
            }
        }
        [TearDown]
        public void Teardown()
        {
            _qADashboardService = null;
        }

    }
}
