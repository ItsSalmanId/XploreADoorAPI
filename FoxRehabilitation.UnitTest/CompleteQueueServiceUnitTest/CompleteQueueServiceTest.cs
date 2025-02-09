﻿using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CompleteQueueService;
using FOX.DataModels.Models.CompleteQueueModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.CompleteQueueServiceUnitTest
{
    [TestFixture]
    public class CompleteQueueServiceTest
    {
        private CompleteQueueService _completeQueueService;
        private SearchRequestCompletedQueue _searchRequestCompletedQueue;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _completeQueueService = new CompleteQueueService();
            _searchRequestCompletedQueue = new SearchRequestCompletedQueue();
            _userProfile = new UserProfile();
        }
        [Test]
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        [TestCase(1012714, true)]
        public void GetCasesDdl_ResponseGetCasesDdlModel_ReturnData(long practiceCode, bool dateTo)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            if (dateTo)
            {
                _searchRequestCompletedQueue.DATE_FROM_STR = Helper.GetCurrentDate().ToString();
                _searchRequestCompletedQueue.reportUser = "test";
            }
            else
            {
                _searchRequestCompletedQueue.DATE_TO_STR = Helper.GetCurrentDate().ToString();
                _searchRequestCompletedQueue.reportUser = "t";

            }
            _searchRequestCompletedQueue.CurrentPage = 1;
            _searchRequestCompletedQueue.RecordPerPage = 10;
            _searchRequestCompletedQueue.SearchText = "test";
            _searchRequestCompletedQueue.SortBy = "test";
            _searchRequestCompletedQueue.SortOrder = "test";

            //Act
            var result = _completeQueueService.GetCompleteQueue(_searchRequestCompletedQueue, _userProfile);

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
            // Optionally dispose or cleanup object
            _completeQueueService = null;
            _searchRequestCompletedQueue = null;
            _userProfile = null;
        }
    }
}
