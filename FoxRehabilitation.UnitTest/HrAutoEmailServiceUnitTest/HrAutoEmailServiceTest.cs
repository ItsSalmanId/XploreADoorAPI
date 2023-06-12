using FOX.BusinessOperations.HrAutoEmail;
using FOX.DataModels.Models.HrAutoEmail;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.HrAutoEmailServiceUnitTest
{
    [TestFixture]
    public class HrAutoEmailServiceTest
    {
        private HrAutoEmailService _hrAutoEmailService;
        private UserProfile _userProfile;
        private HrAutoEmailConfigure _hrAutoEmailConfigure;

        [SetUp]
        public void SetUp()
        {
            _hrAutoEmailService = new HrAutoEmailService();
            _userProfile = new UserProfile();
            _hrAutoEmailConfigure = new HrAutoEmailConfigure();
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetHrAutoEmailConfigureRecords_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _hrAutoEmailService.GetHrAutoEmailConfigureRecords(_userProfile);

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
        [TestCase(0, 0)]
        [TestCase(548356, 0)]
        [TestCase(0, 1011163)]
        [TestCase(548356, 1011163)]
        [TestCase(38403, 38403)]
        public void GetHrAutoEmalById_PassParameters_ReturnData(int hrAutoEmail, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _hrAutoEmailService.GetHrAutoEmalById(hrAutoEmail, _userProfile);

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
        [TestCase(38403)]
        public void GetMtbcDocumentFileDetails_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _hrAutoEmailService.GetMTBCDocumentFileDetails(_userProfile);

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
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetMtbcUnMappedCategory_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _hrAutoEmailService.GetMTBCUnMappedCategory(_userProfile);

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
        [TestCase(1011163)]
        [TestCase(38403)]
        public void AddHrAutoEmailConfigure_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _hrAutoEmailConfigure.NAME = "Student Placement Mentor";

            //Act
            var result = _hrAutoEmailService.AddHrAutoEmailConfigure(_hrAutoEmailConfigure, _userProfile);

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
        [TestCase(1011163)]
        [TestCase(38403)]
        public void DeleteHrAutoEmailConfigure_PassParameter_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.PracticeCode = practiceCode;
            _hrAutoEmailConfigure.NAME = "Student Placement Mentor";
            _hrAutoEmailConfigure.HR_CONFIGURE_ID = 548378;

            //Act
            var result = _hrAutoEmailService.DeleteHrAutoEmailConfigure(_hrAutoEmailConfigure, _userProfile);

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
        [TestCase(1011163)]
        [TestCase(38403)]
        public void EnableHrAutoEmailCertificate_PassParameter_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "test";
            _hrAutoEmailConfigure.NAME = "Student Placement Mentor";
            _hrAutoEmailConfigure.HR_CONFIGURE_ID = 548381;

            //Act
            var result = _hrAutoEmailService.EnableHrAutoEmailCertificate(_hrAutoEmailConfigure, _userProfile);

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
        [TestCase(1011163)]
        [TestCase(38403)]
        public void UpdateHrAutoEmailConfigure_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _hrAutoEmailConfigure.NAME = "Student Placement Mentor";
            _hrAutoEmailConfigure.HR_CONFIGURE_ID = 548381;

            //Act
            var result = _hrAutoEmailService.UpdateHrAutoEmailConfigure(_hrAutoEmailConfigure, _userProfile);

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
        [TestCase(1011163)]
        [TestCase(38403)]
        public void SaveHrMtbcEMailDocumentFiles_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "test";
            _hrAutoEmailConfigure.NAME = "Student Placement Mentor";
            List<HrEmailDocumentFileAll> hrEmailDocumentFileAllList = new List<HrEmailDocumentFileAll>()
            {
                new HrEmailDocumentFileAll
                {
                    HR_MTBC_EMAIL_DOCUMENT_FILE_ID = 123,
                    DOCUMENT_PATH = "HRAutoEmailAttachmentFiles",
                    HR_CONFIGURE_ID = 548381

                }
            };

            //Act
            var result = _hrAutoEmailService.SaveHrMTBCEMailDocumentFiles(hrEmailDocumentFileAllList, _userProfile);

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
            _hrAutoEmailService = new HrAutoEmailService();
            _userProfile = new UserProfile();
        }
    }
}
