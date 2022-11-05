using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.SettingsService.AnnouncementService;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.SettingServicesUnitTest.AnnouncementUnitTest
{
    [TestFixture]
    class AnnouncementTest
    {
        private AnnouncementService _announcementService;
        private UserProfile _userProfile;
        private Announcements _announcements;
        private AddEditFoxAnnouncement _addEditFoxAnnouncement;

        [SetUp]
        public void Setup()
        {
            _announcementService = new AnnouncementService();
            _userProfile = new UserProfile();
            _announcements = new Announcements();
            _addEditFoxAnnouncement = new AddEditFoxAnnouncement();
        }
        [Test]
        [TestCase(1011163, "103")]
        [TestCase(0, "")]
        [TestCase(1011163, "")]
        [TestCase(1011163, "54810141")]
        [TestCase(1011163, "54810141,103")]
        public void GetAnnouncement_AnnouncementExistModel_ReturnsData(long practiceCode, string role)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _announcements.ROLE_ID = role;
            var curruntDate = Helper.GetCurrentDate();
            _announcements.ANNOUNCEMENT_DATE_FROM_STR = curruntDate.ToString();
            _announcements.ANNOUNCEMENT_DATE_TO_STR = curruntDate.ToString();

            //Act
            var result = _announcementService.GetAnnouncement(_announcements, _userProfile);

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
        [TestCase(1011163, 548667)]
        [TestCase(0, 548661)]
        [TestCase(1011163, 0)]
        [TestCase(0, 0)]
        public void GetAnnouncementDetails_AnnouncementExistModel_ReturnsData(long practiceCode, long announcementId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _announcements.ANNOUNCEMENT_ID = announcementId;
            _announcements.ANNOUNCEMENT_DATE_FROM = Helper.GetCurrentDate();
            _announcements.ANNOUNCEMENT_DATE_TO = Helper.GetCurrentDate();

            //Act
            var result = _announcementService.GetAnnouncementDetails(_announcements, _userProfile);

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
        [TestCase(1011163, 548667)]
        [TestCase(1011163, 548668)]
        [TestCase(0, 548668)]
        [TestCase(0, 0)]
        public void InsertAnnouncement_AnnouncementAddModel_SaveData(long practiceCode, long announcementId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _addEditFoxAnnouncement.ANNOUNCEMENT_ID = announcementId;
            _addEditFoxAnnouncement.ANNOUNCEMENT_DATE_FROM = Helper.GetCurrentDate();
            _addEditFoxAnnouncement.ANNOUNCEMENT_DATE_TO = Helper.GetCurrentDate();
            _addEditFoxAnnouncement.ANNOUNCEMENT_TITLE = "test";
            _addEditFoxAnnouncement.ANNOUNCEMENT_DETAILS = "test";
            _addEditFoxAnnouncement.RoleRequest = new List<FoxRoleRequest>()
            {
                new FoxRoleRequest
                {
                    ROLE_ID = 103,
                    ROLE_NAME = ""
                }
            };

            //Act
            var result = _announcementService.InsertAnnouncement(_addEditFoxAnnouncement, _userProfile);

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
        [TestCase(1011163, 548667)]
        [TestCase(1011163, 548668)]
        [TestCase(0, 548668)]
        [TestCase(0, 0)]
        public void DeleteAnnouncement_AnnouncementDeleteModel_DeleteData(long practiceCode, long announcementId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _announcements.ANNOUNCEMENT_ID = announcementId;

            //Act
            var result = _announcementService.DeleteAnnouncement(_announcements, _userProfile);

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
            // Optionally dispose or cleanup objects
            _announcementService = null;
            _userProfile = null;
            _announcements = null;
            _addEditFoxAnnouncement = null;
        }
    }
}
