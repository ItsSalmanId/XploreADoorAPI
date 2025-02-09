﻿using FOX.BusinessOperations.SupervisorWorkService;
using FOX.DataModels.Models.AssignedQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.SupervisorWorkModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.SupervisorWorkServiceUnitTest
{
    [TestFixture]
    public class SupervisorWorkServiceTest
    {
        private SupervisorWorkService _supervisorWorkService;
        private UserProfile _userProfile;
        private SupervisorWorkRequest _supervisorWorkRequest;
        private MarkReferralValidOrTrashedModel _markReferralValidOrTrashedModel;
        [SetUp]
        public void SetUp()
        {
            _supervisorWorkService = new SupervisorWorkService();
            _userProfile = new UserProfile();
            _supervisorWorkRequest = new SupervisorWorkRequest();
            _markReferralValidOrTrashedModel = new MarkReferralValidOrTrashedModel();
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1011165)]
        public void GetSupervisorList_SupervisorListModel_ReturnData(long PracticeCode)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _userProfile.userID = 1011163415;
            _userProfile.UserName = "1163testing";
            _supervisorWorkRequest.TransferReason = "";
            _supervisorWorkRequest.RecordPerPage = 10;
            _supervisorWorkRequest.CurrentPage = 1;
            _supervisorWorkRequest.SortOrder = "";
            _supervisorWorkRequest.SearchText = "";
            _supervisorWorkRequest.SourceString = "";
            _supervisorWorkRequest.SourceName = "";
            _supervisorWorkRequest.SourceType = "";
            _supervisorWorkRequest.SupervisorName = "";
            _supervisorWorkRequest.TransferComments = "";
            _supervisorWorkRequest.IndexBy = null;
            _supervisorWorkRequest.SortBy = null;

            //Act
            var result = _supervisorWorkService.GetSupervisorList(_supervisorWorkRequest, _userProfile);

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
        [TestCase(1011163, "asd_5482973")]
        [TestCase(1011163, "test")]
        [TestCase(0, "")]
        public void GetIndxersAndSupervisorsForDropdown_IndxersAndSupervisorsForDropdownModel_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _supervisorWorkService.GetIndxersAndSupervisorsForDropdown(practiceCode, userName);

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
        [TestCase(true, 1011163)]
        [TestCase(false, 1011163)]
        public void MakeReferralAsValidOrTrashed_PassModel_ReturnData(bool isTrash, long practiceCode)
        {
            //Arrange
            _userProfile.UserName = "1163testing";
            _userProfile.PracticeCode = practiceCode;
            _markReferralValidOrTrashedModel.Work_Id = 5487240;
            _markReferralValidOrTrashedModel.Is_Trash = isTrash;

            //Act
            var result = _supervisorWorkService.MakeReferralAsValidOrTrashed(_markReferralValidOrTrashedModel, _userProfile);

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
        [TestCase(544557)]
        [TestCase(5448057)]
        [TestCase(1011163)]
        public void GetWorkTransferComments_WorkTransferCommentsModel_ReturnData(long workId)
        {
            //Arrange
            //Act
            var result = _supervisorWorkService.GetWorkTransferComments(workId);

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
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _userProfile = null;
            _supervisorWorkService = null;
            _supervisorWorkRequest = null;
            _markReferralValidOrTrashedModel = null;
        }

    }
}
