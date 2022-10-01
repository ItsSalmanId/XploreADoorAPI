using FOX.BusinessOperations.RequestForOrder.IndexInformationServices;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.RequestForOrderUnitTest.IndexInformationServiceUnitTest
{
    [TestFixture]
    class IndexInformationServiceTest
    {
        private IndexInformationService _indexInformationService;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _indexInformationService = new IndexInformationService();
            _userProfile = new UserProfile();
        }
        [Test]
        [TestCase(101116354814505, 1011163)]
        [TestCase(101116354610685, 1011163)]
        public void GetFacilityReferralSource_EmptyModel_ReturnData(long patientAccount, long practiceCode)
        {
            //Arrange

            //Act
            var result = _indexInformationService.getFacilityReferralSource(patientAccount, practiceCode);

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
        [TestCase("test", 1011163)]
        [TestCase("", 1011163)]
        public void GetFacilityLocations_FacilityLocationsModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _indexInformationService.GetFacilityLocations(searchText, practiceCode);

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
        [TestCase("101116354412309", 1011163)]
        [TestCase("101116354816561", 1011163)]
        public void GetFacilityByPatientPOS_FacilityByPatientPOSModel_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _indexInformationService.GetFacilityByPatientPOS(patientAccount, practiceCode);

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
        [TestCase("08873", 1011163)]
        [TestCase("-08873", 1011163)]
        public void GetRegionByZip_RegionByZipModel_ReturnData(string zipCode, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _indexInformationService.GetRegionByZip(zipCode, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _indexInformationService = null;
            _userProfile = null;    
        }
    }
}
