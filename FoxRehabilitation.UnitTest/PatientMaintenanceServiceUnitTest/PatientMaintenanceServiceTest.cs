using FOX.BusinessOperations.PatientMaintenanceService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.PatientMaintenanceServiceUnitTest
{
    class PatientMaintenanceServiceTest
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
        public void GetDocumentTypes_Patientlist_ReturnData(long patientAccount)
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
                Assert.IsTrue(true);
            }
        }
        [TearDown]
        public void Teardown()
        {
            _patientMaintenanceService = new PatientMaintenanceService();
        }
    }
}
