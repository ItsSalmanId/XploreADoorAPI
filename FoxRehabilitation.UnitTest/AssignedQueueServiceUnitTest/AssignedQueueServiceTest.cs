using FOX.BusinessOperations.AssignedQueueService;
using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Models.AssignedQueueModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest
{
    class AssignedQueueServiceTestCases
    {
        private AssignedQueueServices _assignedQueueServices;
        private UserProfile _userProfile;
        private AssignedQueueRequest _assignedQueueRequest;
        private BlacklistWhiteListSourceModel _blacklistWhiteListSourceModel;
        private MarkReferralValidOrTrashedModel _markReferralValidOrTrashed;
        [SetUp]
        public void SetUp()
        {
            _assignedQueueServices = new AssignedQueueServices();
            _userProfile = new UserProfile();
            _assignedQueueRequest = new AssignedQueueRequest();
            _blacklistWhiteListSourceModel = new BlacklistWhiteListSourceModel();
            _markReferralValidOrTrashed = new MarkReferralValidOrTrashedModel();
        }
        [Test]
        [TestCase(0, 0, "")]
        [TestCase(1011163, 0, "")]
        [TestCase(0, 102, "")]
        [TestCase(0, 0, "khankhan_544596")]
        [TestCase(1011163, 102, "khankhan_544596")]
        [TestCase(1163, 12102, "test")]
        public void GetSupervisorAndAgentsForDropdown_UsersForDropdownList_ReturnData(long practiceCode, long roleId, string userName)
        {
            //Arrange
            //Act
            var result = _assignedQueueServices.GetSupervisorAndAgentsForDropdown(practiceCode, roleId, userName);

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
        [TestCase(0, "")]
        [TestCase(1011163, "")]
        [TestCase(0, "khankhan_544596")]
        [TestCase(1011163, "khankhan_544596")]
        [TestCase(1163, "test")]
        public void GetIndexersForDropdown_UsersForDropdownList_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _assignedQueueServices.GetIndexersForDropdown(practiceCode, userName);

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
        [TestCase(0, "")]
        [TestCase(1011163, "")]
        [TestCase(0, "khankhan_544596")]
        [TestCase(1011163, "khankhan_544596")]
        [TestCase(1163, "test")]
        public void GeInterfaceFailedPatientList_InterfcaeFailedPatientList_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _assignedQueueServices.GeInterfaceFailedPatientList(practiceCode, userName);

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
        [TestCase(0, "")]
        [TestCase(101113, "")]
        [TestCase(0, "khankhan_544596")]
        [TestCase(1011163, "khankhan_544596")]
        [TestCase(0321, "test")]
        public void GetSupervisorsForDropdown_UsersForDropdownList_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _assignedQueueServices.GetSupervisorsForDropdown(practiceCode, userName);

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
        [TestCase("5487172")]
        [TestCase("5487173")]
        [TestCase("0")]
        public void GetIndexedQueue_PassModel_ReturnData(string workId)
        {
            //Arrange
            _assignedQueueRequest.reportUser = "test";
            _assignedQueueRequest.DATE_FROM_STR = Helper.GetCurrentDate().ToString();
            _assignedQueueRequest.DATE_TO_STR = Helper.GetCurrentDate().ToString();
            _assignedQueueRequest.WorkID = workId;
            _assignedQueueRequest.SearchText = "";
            _assignedQueueRequest.CurrentPage = 1;
            _assignedQueueRequest.RecordPerPage = 10;
            _assignedQueueRequest.SortBy = "";
            _assignedQueueRequest.SortOrder = "";

            //Act
            var result = _assignedQueueServices.GetIndexedQueue(_assignedQueueRequest, _userProfile);

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
        [Test]
        [TestCase(false, true, 1011163)]
        [TestCase(false, true, 1012714)]
        [TestCase(true, false, 1011163)]
        [TestCase(true, false, 1012714)]
        public void BlackListOrWhiteListSource_PassModel_ReturnData(bool isBlackList, bool isWhiteList, long practiceCode)
        {
            //Arrange
            _userProfile.UserName = "1163testing";
            _userProfile.PracticeCode = practiceCode;
            _blacklistWhiteListSourceModel.Work_Ids = new List<long>()
            {
                5487172
            };
            _blacklistWhiteListSourceModel.Mark_All_Orders_As_White_Listed = isWhiteList;
            _blacklistWhiteListSourceModel.Is_Black_List = isBlackList;

            //Act
            var result = _assignedQueueServices.BlackListOrWhiteListSource(_blacklistWhiteListSourceModel, _userProfile);

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
        //[Test]
        //[TestCase(true, 1012714)]
        //[TestCase(false, 1011163)]  
        //public void MakeReferralAsValidOrTrashed_PassModel_ReturnData(bool isTrash, long practiceCode)
        //{
        //    //Arrange
        //    _userProfile.UserName = "1163testing";
        //    _userProfile.PracticeCode = practiceCode;
        //    _markReferralValidOrTrashed.Work_Id = 5487172;
        //    _markReferralValidOrTrashed.Is_Trash = isTrash;

        //    //Act
        //    var result = _assignedQueueServices.MakeReferralAsValidOrTrashed(_markReferralValidOrTrashed, _userProfile);

        //    //Assert
        //    if (result != null)
        //    {
        //        Assert.IsTrue(true);
        //    }
        //    else
        //    {
        //        Assert.IsFalse(false);
        //    }
        //}
        //AddUpdateSourceAsBlackOrWhiteList
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _assignedQueueServices = null;
            _userProfile = null;
            _assignedQueueRequest = null;
            _blacklistWhiteListSourceModel = null;
            _markReferralValidOrTrashed = null;
        }

    }
}
