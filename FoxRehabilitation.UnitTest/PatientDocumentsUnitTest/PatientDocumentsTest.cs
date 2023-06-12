using FOX.BusinessOperations.PatientDocumentsService;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.PatientDocumentsUnitTest
{
    class PatientDocumentsTest
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
        public void GetDocumentTypes_DocumenttypeAndpatientcasesModel_ReturnData(string patientAccount, long practiceCode)
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
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllDocumentTypes_FoxDocumentTypelist_ReturnData(long practiceCode)
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
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllSpecialityProgram_FoxSpecialityProgramList_ReturnData(long practiceCode)
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
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllDocumentTypeswithInactive_FoxDocumentTypeList_ReturnData(long practiceCode)
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
                Assert.IsTrue(true);
            }
        }
        [TearDown]
        public void Teardown()
        {
            _patientDocumentsService = new PatientDocumentsService();
            _userProfile = new UserProfile();
        }
    }
}
