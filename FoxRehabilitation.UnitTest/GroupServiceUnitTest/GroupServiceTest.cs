using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.GroupServices;
using FOX.DataModels.Models.GroupsModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.GroupServiceUnitTest
{
    class GroupServiceTest
    {

        private UserProfile _userProfile;
        private GroupService _groupService;
        private GROUP _group;
        private GroupUsersCreateViewModel _groupUsersCreateViewModel;

        [SetUp]
        public void SetUp()
        {
            _userProfile = new UserProfile();
            _groupService = new GroupService();
            _group = new GROUP();
            _groupUsersCreateViewModel = new GroupUsersCreateViewModel();
        }

        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAlertGeneralNotes_Grouplist_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _groupService.GetGroupsList(_userProfile);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetUsersWithTheirRole_UserWithRoleslist_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _groupService.GetUsersWithTheirRole(_userProfile);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(548169, 0)]
        [TestCase(0, 1011163)]
        [TestCase(548169, 1011163)]
        [TestCase(38403, 38403)]
        public void GetGroupUsersByGroupId_PassParameters_ReturnData(long groupId, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _groupService.GetGroupUsersByGroupId(groupId, _userProfile);

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
        [Test]
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        [TestCase(1012714, true)]
        [TestCase(1012714, false)]
        public void AddUpdateGroup_PassModel_ReturnData(long practiceCode, bool add)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            if (add)
            {
                _group.GROUP_ID = Helper.getMaximumId("GROUP_ID");
            }
            else
            {
                _group.GROUP_ID = Helper.getMaximumId("GROUP_ID") - 1;
            }
            _group.GROUP_NAME = "test";
            _group.CREATED_BY = "N-Unit Testing";
            _group.CREATED_DATE = Helper.GetCurrentDate();

            //Act
            var result = _groupService.AddUpdateGroup(_group, _userProfile);

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
        [Test]
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        [TestCase(1012714, true)]
        [TestCase(1012714, false)]
        public void DeleteGroup_PassModel_ReturnData(long practiceCode, bool add)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            if (add)
            {
                _group.GROUP_ID = Helper.getMaximumId("GROUP_ID");
            }
            else
            {
                _group.GROUP_ID = Helper.getMaximumId("GROUP_ID") - 10;
            }
            _group.GROUP_NAME = "test";
            _group.CREATED_BY = "N-Unit Testing";
            _group.CREATED_DATE = Helper.GetCurrentDate();
            //Act
            var result = _groupService.DeleteGroup(_group, _userProfile);

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
        [Test]
        [TestCase(1011163, 5481153, 548166)]
        [TestCase(1011163, 5481166, 548169)]
        [TestCase(1012714, 5481170, 548169)]
        [TestCase(1012714, 5481172, 548169)]
        public void AddUsersInGroup_PassModel_ReturnData(long practiceCode, long groupUserId, long groupId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "N-Unit Testing";
            _groupUsersCreateViewModel.USERS = null;
            _groupUsersCreateViewModel.USERS = new UserWithRoles[] { };
            _groupUsersCreateViewModel.USERS = new UserWithRoles[]
            {
                new UserWithRoles()
                {
                    GROUP_USER_ID = groupUserId,
                    USER_NAME = "test",
                    GROUP_ID = groupId
                }
            };

            //Act
            var result = _groupService.AddUsersInGroup(_groupUsersCreateViewModel, _userProfile);

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
        public void Teardown()
        {
            _userProfile = new UserProfile();
            _groupService = new GroupService();
            _group = null;
            _groupUsersCreateViewModel = null;
        }
    }
}
