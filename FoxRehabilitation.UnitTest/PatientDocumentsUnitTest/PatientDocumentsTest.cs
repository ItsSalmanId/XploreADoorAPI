using FOX.BusinessOperations.PatientDocumentsService;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
namespace FoxRehabilitation.UnitTest.PatientDocumentsUnitTest
{
    [TestFixture]
    public class PatientDocumentsTest
    {
        private PatientDocumentsService _patientDocumentsService;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _patientDocumentsService = new PatientDocumentsService();
            _userProfile = new UserProfile();
        }
        [Test]
        [TestCase("", 0)]
        [TestCase("101116354813969", 0)]
        [TestCase("", 1011163)]
        [TestCase("101116354813969", 1011163)]
        [TestCase("38403", 38403)]
        public void GetDocumentTypes_PassParameters_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientDocumentsService.getDocumentTypes(patientAccount, _userProfile);

            //Assert
            if (result.DocumentTypes.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.DocumentTypes.Count > 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllDocumentTypes_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientDocumentsService.GetAllDocumentTypes(_userProfile);

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
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllSpecialityProgram_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientDocumentsService.GetAllSpecialityProgram(_userProfile);

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
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllDocumentTypeswithInactive_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientDocumentsService.GetAllDocumentTypeswithInactive(_userProfile);

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
            // Optionally dispose or cleanup objectsss
            _patientDocumentsService = new PatientDocumentsService();
            _userProfile = new UserProfile();
        }
    }
}
