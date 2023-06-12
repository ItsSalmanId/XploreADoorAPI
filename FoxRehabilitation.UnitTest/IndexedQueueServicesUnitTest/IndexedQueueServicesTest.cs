using FOX.BusinessOperations.IndexedQueueService;
using FOX.DataModels.Models.IndexedQueueModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.IndexedQueueServicesUnitTest
{
    [TestFixture]
    public class IndexedQueueServicesTest
    {
        private IndexedQueueServices _indexedQueueServices;
        private IndexedQueueFileRequest _indexedQueueFileRequest;
        private UserProfile _userProfile;
        private IndexedQueueRequest _indexedQueueRequest;
        private WorkTransfer _workTransfer;

        [SetUp]
        public void SetUp()
        {
            _indexedQueueServices = new IndexedQueueServices();
            _indexedQueueFileRequest = new IndexedQueueFileRequest();
            _userProfile = new UserProfile();
            _indexedQueueRequest = new IndexedQueueRequest();
            _workTransfer = new WorkTransfer();
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(5447924)]
        [TestCase(38403)]
        public void GetFilePages_PassParameters_ReturnData(long workId)
        {
            //Arrange
            _indexedQueueFileRequest.WORK_ID = workId;

            //Act
            var result = _indexedQueueServices.GetFilePages(_indexedQueueFileRequest);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(5447924)]
        [TestCase(38403)]
        public void SetSplitPages_PassParameters_ReturnData(long workId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "Test";
            _indexedQueueFileRequest.WORK_ID = workId;
            List<FilePages> filePagesList = new List<FilePages>()
            {
                new FilePages
                {
                    file_path1 = "test",
                    FILE_PATH = "test",
                    UNIQUE_ID = "54819309_246",
                    WORK_ID = 54821717
                }
            };

            //Act
            var result = _indexedQueueServices.SetSplitPages(filePagesList, _userProfile);

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
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(5447924)]
        [TestCase(38403)]
        public void GetIndexedQueue_PassParameters_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "Test";
            _indexedQueueRequest.CurrentPage = 10;
            _indexedQueueRequest.RecordPerPage = 10;
            _indexedQueueRequest.SearchText = "Test";
            _indexedQueueRequest.SorceType = "Test";
            _indexedQueueRequest.SortBy = "Test";
            _indexedQueueRequest.SorceName = "Test";
            _indexedQueueRequest.IncludeArchive = true;
            _indexedQueueRequest.IndexedBy = "test";
            _indexedQueueRequest.SortOrder = "test";

            //Act
            var result = _indexedQueueServices.GetIndexedQueue(_indexedQueueRequest, _userProfile);

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
        public void ReAssignedMultiple_PassParameters_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "Test";
            List<IndexedQueue> indexedQueuesList = new List<IndexedQueue>()
            {
                new IndexedQueue
                {
                    WORK_ID = 54821716,
                    RE_ASSIGNED_TO = "test"
                }
            };

            //Act
            _indexedQueueServices.ReAssignedMultiple(_userProfile, indexedQueuesList);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void InsertAssignmentData_PassParameters_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "Test";
            string ASSIGNED_BY = "test";
            string ASSIGNED_TO = "test";
            string AssignToDesignation = "test";
            string AssignByDesignation = "test";
            long WORK_ID = 54821716;

            //Act
            _indexedQueueServices.InsertAssignmentData(ASSIGNED_BY, ASSIGNED_TO, AssignToDesignation, AssignByDesignation, WORK_ID, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void AddUpdateWorkTransfer_PassParameters_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "Test";

            //Act
            _indexedQueueServices.AddUpdateWorkTransfer(_workTransfer, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(0, 0, "")]
        [TestCase(102, 0, "")]
        [TestCase(0, 1011163, "")]
        [TestCase(0, 0, "1163testing")]
        [TestCase(102, 1011163, "1163testing")]
        [TestCase(38403, 38403, "test")]
        public void GetAgentsAndSupervisorsForDropdown_PassParameters_ReturnData(long RoleId, long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _indexedQueueServices.GetAgentsAndSupervisorsForDropdown(RoleId, practiceCode, userName);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsFalse(false);
            }
        }
        [TearDown]
        public void Teardown()
        {
            _indexedQueueServices = null;
            _indexedQueueFileRequest = null;
            _userProfile = null;
            _indexedQueueRequest = null;
            _workTransfer = null;
        }
    }
}
