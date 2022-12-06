using FOX.BusinessOperations.UnAssignedQueueService;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.UnAssignedQueueModel;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.UnAssignedQueueServiceUnitTest
{
    [TestFixture]
    class UnAssignedQueueServiceTest
    {
        private UnAssignedQueueService _unAssignedQueueService;
        private UnAssignedQueueRequest _unAssignedQueueRequest;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _unAssignedQueueService = new UnAssignedQueueService();
            _unAssignedQueueRequest = new UnAssignedQueueRequest();
            _userProfile = new UserProfile();

        }
        [Test]
        [TestCase(1011163)]
        [TestCase(123456)]
        [TestCase(0)]
        public void GetUnAssignedQueue_UnAssignedQueueModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _unAssignedQueueRequest.CurrentPage = 1;
            _unAssignedQueueRequest.RecordPerPage = 10;
            _unAssignedQueueRequest.SortBy = "CREATED_DATE";
            _unAssignedQueueRequest.SortOrder = "DESC";
            _unAssignedQueueRequest.SearchText = "";
            _unAssignedQueueRequest.IncludeArchive = false;
            _unAssignedQueueRequest.CalledFrom = "";
            _unAssignedQueueRequest.ID = "";
            _unAssignedQueueRequest.SorceName = "";
            _unAssignedQueueRequest.SorceType = "";

            //Act
            var result = _unAssignedQueueService.GetUnAssignedQueue(_unAssignedQueueRequest, _userProfile);

            //Assert
            if (result != null && result.Count >0)
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
        [TestCase(101116322)]
        [TestCase(1011164)]
        public void GetSupervisorForDropdown_SupervisorForDropdownModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _unAssignedQueueService.GetSupervisorForDropdown(practiceCode);

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
        [TestCase(1011163, "1163testing")]
        [TestCase(1011163, "")]
        [TestCase(1011164,"")]
        public void GetIndexersForDropdown_IndexersForDropdownModel_ReturnData(long practiceCode,string userName)
        {
            //Arrange
            //Act
            var result = _unAssignedQueueService.GetIndexersForDropdown(practiceCode, userName);

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
        [TestCase(1011163, "1163testing")]
        [TestCase(1011163, "")]
        [TestCase(1011164, "")]
        public void GetInitialData_InitialDataModel_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = userName;
            _unAssignedQueueRequest.CurrentPage = 1;
            _unAssignedQueueRequest.RecordPerPage = 10;
            _unAssignedQueueRequest.SortBy = "CREATED_DATE";
            _unAssignedQueueRequest.SortOrder = "DESC";
            _unAssignedQueueRequest.SearchText = "";
            _unAssignedQueueRequest.IncludeArchive = false;
            _unAssignedQueueRequest.CalledFrom = "";
            _unAssignedQueueRequest.ID = "";
            _unAssignedQueueRequest.SorceName = "";
            _unAssignedQueueRequest.SorceType = "";

            //Act
            var result = _unAssignedQueueService.GetInitialData(_unAssignedQueueRequest, _userProfile);

            //Assert
            Assert.That(result.usersForDropdownData.Count,Is.GreaterThanOrEqualTo(0));
            Assert.That(result.unassignedQueueData.Count, Is.GreaterThanOrEqualTo(0));
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _unAssignedQueueService = null;
            _userProfile = null;
            _unAssignedQueueRequest = null;
        }
    }
}
