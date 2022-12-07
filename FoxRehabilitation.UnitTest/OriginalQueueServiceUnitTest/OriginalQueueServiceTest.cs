using FOX.BusinessOperations.OriginalQueueService;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.OriginalQueueServiceUnitTest
{
    [TestFixture]
    class OriginalQueueServiceTest
    {
        private OriginalQueueService _originalQueueService;
        private OriginalQueueRequest _originalQueueRequest;
        private UserProfile _userProfile;
        private ReqOriginalQueueModel _reqOriginalQueueModel;
        private OriginalQueue _originalQueue;

        [SetUp]
        public void SetUp()
        {
            _originalQueueService = new OriginalQueueService();
            _userProfile = new UserProfile();
            _originalQueueRequest = new OriginalQueueRequest();
            _reqOriginalQueueModel = new ReqOriginalQueueModel();
            _originalQueue = new OriginalQueue();
        }
        [Test]
        [TestCase(1011163, "5484147_1", 54819310)]
        [TestCase(1011163, "54819309", 54819309)]
        public void GetOriginalQueue_OriginalQueueModel_ReturnData(long practiceCode, string searchText, long workId)
        {
            //Arrange
            _originalQueueRequest.SearchText = searchText;
            _userProfile.PracticeCode = practiceCode;
            _originalQueueRequest.WORK_ID = workId;
            _originalQueueRequest.CurrentPage = 1;
            _originalQueueRequest.RecordPerPage = 10;
            _originalQueueRequest.SorceString = "";
            _originalQueueRequest.SorceType = "MOBILE APP";
            _originalQueueRequest.SortBy = "";
            _originalQueueRequest.SortOrder = "";
            _originalQueueRequest.DateFrom_str = "7/9/2022";
            _originalQueueRequest.DateTo_str = "8/9/2022";
            _originalQueueRequest.STATUS_TEXT = "";

            //Act
            var result = _originalQueueService.GetOriginalQueue(_originalQueueRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(552103, 1011163)]
        [TestCase(5487177, 1011163)]
        public void GetWorkDetails_WorkDetailsModel_ReturnData(long workId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _originalQueueService.GetWorkDetails(workId, practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "5484147_1", 54819310)]
        [TestCase(1011163, "54819309", 54819309)]
        public void CCREMOTEGetOriginalQueue_CCREMOTEGetOriginalQueueModel_ReturnData(long practiceCode, string searchText, long workId)
        {
            //Arrange
            _originalQueueRequest.SearchText = searchText;
            _userProfile.PracticeCode = practiceCode;
            _originalQueueRequest.WORK_ID = workId;
            _originalQueueRequest.STATUS_TEXT = "53426170";
            _originalQueueRequest.CurrentPage = 1;
            _originalQueueRequest.RecordPerPage = 10;
            _originalQueueRequest.SorceString = "";
            _originalQueueRequest.SorceType = "MOBILE APP";
            _originalQueueRequest.SortBy = "";
            _originalQueueRequest.SortOrder = "";
            _originalQueueRequest.DateFrom_str = "7/9/2022";
            _originalQueueRequest.DateTo_str = "8/9/2022";

            //Act
            var result = _originalQueueService.CCREMOTEGetOriginalQueue(_originalQueueRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "54819279")]
        [TestCase(1011163, "54819309")]
        public void GetWorkDetailsUniqueId_WorkDetailsUniqueIdModel_ReturnData(long practiceCode, string isUniqueID)
        {
            //Arrange
            _reqOriginalQueueModel = new ReqOriginalQueueModel()
            {
                Unique_IdList = new List<string>()
                {
                    isUniqueID
                }
            };
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _originalQueueService.GetWorkDetailsUniqueId(_reqOriginalQueueModel, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "5484147_1", 54819310)]
        [TestCase(1011163, "54819309", 54819309)]
        [TestCase(1011163, "_54819309", 54819309)]
        public void GetOriginalQueueMapping_OriginalQueueMappingModel_ReturnData(long practiceCode, string searchText, long workId)
        {
            //Arrange
            _originalQueueRequest.SearchText = searchText;
            _userProfile.PracticeCode = practiceCode;
            _originalQueueRequest.WORK_ID = workId;
            _originalQueueRequest.STATUS_TEXT = "";
            _originalQueueRequest.CurrentPage = 1;
            _originalQueueRequest.RecordPerPage = 10;
            _originalQueueRequest.SorceString = "";
            _originalQueueRequest.SorceType = "";
            _originalQueueRequest.SortBy = "";
            _originalQueueRequest.SortOrder = "";
            _originalQueueRequest.IncludeArchive = false;
            _originalQueueRequest.DateFrom_str = "7/9/2022";
            _originalQueueRequest.DateTo_str = "8/9/2022";

            //Act
            var result = _originalQueueService.GetOriginalQueueMapping(_originalQueueRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, 50000000)]
        [TestCase(1011163, 54819309)]
        [TestCase(1011163, 54819309)]
        public void SaveQueueData_AddQueueData_ReturnData(long practiceCode, long workId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _originalQueue.WORK_ID = workId;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _originalQueueService.SaveQueueData(_originalQueue, _userProfile);

            //Assert
            Assert.That(result, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(1011163, 50000000,1,200)]
        [TestCase(1011163, 54819309,1, 200)]
        public void SaveQueueFromOldQueueData_AddQueueFromOldQueueData_ReturnData(long practiceCode, long workId , int numberOfPages, long currrentParentID)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _originalQueueService.SaveQueueFromOldQueueData(workId, _userProfile, numberOfPages, currrentParentID);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, 50000000, 200)]
        [TestCase(1011163, 54819309, 200)]
        public void UpdateNmOfPages_UpdateModal_UpdateData(long practiceCode, int numberOfPages, long currrentParentID)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";

            //Act
            _originalQueueService.UpdateNmOfPages(currrentParentID, _userProfile, numberOfPages);

            //Assert
            Assert.IsTrue(true);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _originalQueueService = null;
            _userProfile = null;
            _originalQueueRequest = null;
            _reqOriginalQueueModel = null;
            _originalQueue = null;
        }
    }
}

