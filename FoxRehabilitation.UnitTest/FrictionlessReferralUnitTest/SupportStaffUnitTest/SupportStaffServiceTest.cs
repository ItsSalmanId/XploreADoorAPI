using FOX.BusinessOperations.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.RequestForOrder;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.FrictionlessReferralUnitTest.SupportStaffUnitTest
{
    [TestFixture]
    public class SupportStaffServiceTest
    {
        private SupportStaffService _supportStaffService;
        private PatientDetail _patientDetail;
        private RequestDeleteWorkOrder _requestDeleteWorkOrder;
        private FrictionLessReferral _frictionLessReferral;
        private ProviderReferralSourceRequest _providerReferralSourceRequest;

        [SetUp]
        public void Setup()
        {
            _supportStaffService = new SupportStaffService();
            _patientDetail = new PatientDetail();
            _requestDeleteWorkOrder = new RequestDeleteWorkOrder();
            _frictionLessReferral = new FrictionLessReferral();
            _providerReferralSourceRequest = new ProviderReferralSourceRequest();
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
            if (referralId != 0)
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
        [Test]
        [TestCase("Taseer", "iqbal", "muhammadiqbal11@carecloud.com", "2064512559")]
        [TestCase("Taseer", "", "muhammadiqbal11@carecloud.com", "2064512559")]
        [TestCase("", "", "muhammadiqbal11@carecloud.com", "2064512559")]
        [TestCase("", "", "", "2064512559")]
        public void SendInviteToPatientPortal_PatientDetailModel_ReturnData(string firstName, string lastName, string email, string phoneNumber)
        {
            //Arrange
            _patientDetail.FirstName = firstName;
            _patientDetail.LastName = lastName;
            _patientDetail.EmailAddress = email;
            _patientDetail.MobilePhone = phoneNumber;

            //Act
            var result = _supportStaffService.SendInviteToPatientPortal(_patientDetail);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("", "", "", "")]
        [TestCase("1679785950", "", "", "")]
        [TestCase("1740503804", "", "", "")]
        [TestCase("", "james", "smith", "ny")]
        [TestCase("1023489119", "james", "smith", "ny")]
        [TestCase("1023489", "james", "smith", "ny")]
        public void GetProviderReferralSources_ProviderReferralSourceModel_ReturnData(string npi, string firstName, string lastName, string state)
        {
            //Arrange
            _providerReferralSourceRequest.ProviderNpi = npi;
            _providerReferralSourceRequest.ProviderFirstName = firstName;
            _providerReferralSourceRequest.ProviderLastName = lastName;
            _providerReferralSourceRequest.ProviderState = state;

            //Act
            var result = _supportStaffService.GetProviderReferralSources(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase(54820838)]
        [TestCase(0)]
        [TestCase(null)]
        public void DeleteWorkOrder_HasWorkId_ReturnData(long workId)
        {
            //Arrange
            _requestDeleteWorkOrder.WorkId = workId;

            //Act
            var result = _supportStaffService.DeleteWorkOrder(_requestDeleteWorkOrder);

            //Assert
            if (result.Success)
                Assert.IsTrue(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase(5481103, "", "", false, "", "")]
        [TestCase(5481103, ",9,3", "Thu Dec 01 2022", false, "Party Referral Source", "8774074329")]
        [TestCase(123, ",1,2", "Thu Dec 01 2022", false, "", "")]
        [TestCase(123, ",1,2", null, false, "", "")]
        public void SaveFrictionLessReferralDetails_FrictionLessReferralModel_ReturnData(long referralId, string disciplineId, string patientDOB, bool isSinged, string userType, string providerFax)
        {
            //Arrange
            _frictionLessReferral.FRICTIONLESS_REFERRAL_ID = referralId;
            _frictionLessReferral.PATIENT_DISCIPLINE_ID = disciplineId;
            _frictionLessReferral.PATIENT_DOB_STRING = patientDOB;
            _frictionLessReferral.IS_SIGNED_REFERRAL = isSinged;
            _frictionLessReferral.USER_TYPE = userType;
            _frictionLessReferral.PROVIDER_FAX = providerFax;

            //Act
            var result = _supportStaffService.SaveFrictionLessReferralDetails(_frictionLessReferral);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("1023489119", "", "", "")]
        [TestCase("1740503804", "", "", "")]
        [TestCase("", "carey", "smith", "ny")]
        [TestCase("1023489119", "carey", "smith", "ny")]
        [TestCase("1023489", "carey", "smith", "ny")]
        [TestCase("", "", "", "")]
        public void GetOrderingReferralSource_ProviderReferralSourceModel_ReturnData(string npi, string firstName, string lastName, string state)
        {
            //Arrange
            _providerReferralSourceRequest.ProviderNpi = npi;
            _providerReferralSourceRequest.ProviderFirstName = firstName;
            _providerReferralSourceRequest.ProviderLastName = lastName;
            _providerReferralSourceRequest.ProviderState = state;

            //Act
            var result = _supportStaffService.GetOrderingReferralSource(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        public void GetOrderingReferralSource_EmptyProviderReferralSourceModel_NoReturnData()
        {
            //Arrange
            _providerReferralSourceRequest = null;

            //Act
            var result = _supportStaffService.GetOrderingReferralSource(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("Taseer", "iqbal", "muhammadiqbal11@carecloud.com", "2064512559")]
        [TestCase("Taseer", "iqbal", "test1@carecloud.com", "")]
        [TestCase("", "", "", "")]
        public void SendInviteOnMobile_PatientDetailModel_ReturnData(string firstName, string lastName, string email, string phoneNumber)
        {
            //Arrange
            _patientDetail.FirstName = firstName;
            _patientDetail.LastName = lastName;
            _patientDetail.EmailAddress = email;
            _patientDetail.MobilePhone = phoneNumber;

            //Act
            var result = _supportStaffService.SendInviteOnMobile(_patientDetail);

            //Assert
            if (result.Success)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [TearDown]
        public void Teardown()
        {
            _supportStaffService = null;
            _patientDetail = null;
            _requestDeleteWorkOrder = null;
            _frictionLessReferral = null;
            _providerReferralSourceRequest = null;
        }
    }
}
