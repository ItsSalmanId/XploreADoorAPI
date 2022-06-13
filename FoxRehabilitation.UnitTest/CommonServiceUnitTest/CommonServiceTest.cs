using FOX.BusinessOperations.CommonServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.CommonServiceUnitTest
{
    public class CommonServiceTest
    {
        CommonServices commonServices = new CommonServices();

        [SetUp]
        public void SetUp()
        {
            commonServices = new CommonServices();
        }

        [Test]
        public void DeleteDownloadedFile_EmptyFileLocation_NoReturnData()
        {
            //Arrange
            string fileLocation = string.Empty;

            //Act
            var result = commonServices.DeleteDownloadedFile(fileLocation);

            //Assert
            if (!result.Success)
            {
                Assert.Pass("Passed");
            }
            else
            {
                Assert.Fail("Failed");
            }
        }
        [Test]
        public void DeleteDownloadedFile_HasFileLocation_NoReturnData()
        {
            //Arrange
            string fileLocation = "sample";

            //Act
            var result = commonServices.DeleteDownloadedFile(fileLocation);

            //Assert
            if (!result.Success)
            {
                Assert.Pass("Passed");
            }
            else
            {
                Assert.Fail("Failed");
            }
        }
        [TearDown]
        public void BaseCleanup()
        {

        }
    }
}
