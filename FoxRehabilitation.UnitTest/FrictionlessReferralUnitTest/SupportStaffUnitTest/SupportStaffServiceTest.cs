using FOX.BusinessOperations.FrictionlessReferral.SupportStaff;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.FrictionlessReferralUnitTest.SupportStaffUnitTest
{
    [TestFixture]
    public class SupportStaffServiceTest
    {
        private SupportStaffService _supportStaffService;
        [SetUp]
        public void Setup()
        {
            _supportStaffService = new SupportStaffService();
        }
        [Test]
        public void GetPracticeCode_HasPracticeCode_ReturnData()
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetPracticeCode();

            //Assert
            Assert.That(result, Is.EqualTo(1011163).Or.EqualTo(1012714));
        }
        [Test]
        public void GetInsurancePayers_HasInsurance_ReturnData()
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetInsurancePayers();

            //Assert
            Assert.That(result.Count, Is.GreaterThan(0));
        }
        [Test]
        [TestCase(548103)]
        [TestCase(0)]
        public void GetFrictionLessReferralDetails_HasReferralId_ReturnData(long referralId)
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetFrictionLessReferralDetails(referralId);

            //Assert
            if(referralId != 0)
                Assert.That(result.FRICTIONLESS_REFERRAL_ID, Is.EqualTo(referralId));
            else
                Assert.That(result.FRICTIONLESS_REFERRAL_ID, Is.EqualTo(0));
        }
        [Test]
        [TestCase(548199)]
        [TestCase(0)]
        public void GetFrictionLessReferralDetailsByWorkID_HasWorkId_ReturnData(long workId)
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetFrictionLessReferralDetailsByWorkID(workId);

            //Assert
            if (workId != 0)
                Assert.That(result.WORK_ID, Is.EqualTo(workId));
            else
                Assert.That(result.WORK_ID, Is.EqualTo(0));
        }
        [TearDown]
        public void Teardown()
        {
            _supportStaffService = null;
        }
    }
}
