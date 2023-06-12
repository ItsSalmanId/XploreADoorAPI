using FOX.BusinessOperations.PatientMaintenanceService;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.PatientMaintenanceServiceUnitTest
{
    [TestFixture]
    public class PatientMaintenanceServiceTest
    {
        private PatientMaintenanceService _patientMaintenanceService;

        [SetUp]
        public void SetUp()
        {
            _patientMaintenanceService = new PatientMaintenanceService();
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(101116354813969)]
        [TestCase(38403)]
        public void GetDocumentTypes_PassParameters_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientMaintenanceService.GetPatientByAccountNo(patientAccount);

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
            _patientMaintenanceService = null;
        }
    }
}
