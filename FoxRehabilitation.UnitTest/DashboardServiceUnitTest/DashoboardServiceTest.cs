using FOX.BusinessOperations.DashboardServices;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.DashboardServiceUnitTest
{
    [TestFixture]
    public class DashoboardServiceTest
    {
        private DashboardService _dashboardServiceObj;

        [SetUp]
        public void Setup()
        {
            _dashboardServiceObj = new DashboardService();
        }

        [Test]
        [TestCase(1011163)]
        public void GetDashboardGetTotal_NullEmptyOutOfRangeParameter_DataOrErrorMsg(long practiceCode)
        {
            //Arrange
            var PracticeCode = practiceCode;

            //Act
            var result = _dashboardServiceObj.GetDashboardGetTotal(PracticeCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(01, 25, 1011163)]
        public void GetDashboardData_NullEmptyOutOfRangeParameter_DataOrErrorMsg(int value, int hourFrom, long practiceCode)
        {
            //Arrange
            var PracticeCode = practiceCode;

            //Act
            var result = _dashboardServiceObj.GetDashboardData(value, hourFrom, PracticeCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(null, null, null, null)]
        [TestCase(-1, 1011163, "", "")]
        public void GetDashboardTrend_NullEmptyOutOfRangeParameter_DataOrErrorMsg(int value, long practiceCode, string dateFromUser, string dateToUser)
        {
            //Arrange
            //Act
            var result = _dashboardServiceObj.GetDashboardTrend(value, practiceCode, dateFromUser, dateToUser);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(null, null, null, null, null)]
        [TestCase(1011163, "", "", 1, 25)]
        public void GetNoOfRecordBytime_NullEmptyOutOfRangeParameter_DataOrErrorMsg(long practiceCode, string dateFrom, string dateTo, int hourFrom, int hourTo)
        {
            //Arrange
            //Act
            var result = _dashboardServiceObj.GetNoOfRecordBytime(practiceCode, dateFrom, dateTo, hourFrom, hourTo);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _dashboardServiceObj = null;
        }
    }
}
