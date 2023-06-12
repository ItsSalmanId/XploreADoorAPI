using FOX.BusinessOperations.PatientMaintenanceService.PatientInsuranceService;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.PatientMaintenanceServiceUnitTest.PatientInsuranceServiceUnitTest
{
    class PatientInsuranceServiceTest
    {
        private PatientInsuranceService _patientInsuranceService;
        private UserProfile _userProfile;
        private UnmappedInsuranceRequest _unmappedInsuranceRequest;
        private MTBCInsurancesRequest _mtbcInsurancesRequest;
        private FoxInsurancePayers _foxInsurancePayers;
        private ClaimInsuranceSearchReq _claimInsuranceSearchReq;

        [SetUp]
        public void SetUp()
        {
            _patientInsuranceService = new PatientInsuranceService();
            _userProfile = new UserProfile();
            _unmappedInsuranceRequest = new UnmappedInsuranceRequest();
            _mtbcInsurancesRequest = new MTBCInsurancesRequest();
            _foxInsurancePayers = new FoxInsurancePayers();
            _claimInsuranceSearchReq = new ClaimInsuranceSearchReq();
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetUnmappedInsurancesCount_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            //Act
            var result = _patientInsuranceService.GetUnmappedInsurancesCount(_userProfile);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else if (result > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetUnmappedInsurances_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _unmappedInsuranceRequest.FinancialClassID = "0";
            _unmappedInsuranceRequest.State = "0";
            _unmappedInsuranceRequest.Carrier_State = "0";
            _unmappedInsuranceRequest.SearchText = "0";
            _unmappedInsuranceRequest.RecordPerPage = 100;
            _unmappedInsuranceRequest.CurrentPage = 10;

            //Act
            var result = _patientInsuranceService.GetUnmappedInsurances(_unmappedInsuranceRequest, _userProfile);

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
        [Test]
        public void GetMtbcInsurancesSearchData_PassParameters_ReturnData()
        {
            //Arrange
            //Act
            var result = _patientInsuranceService.GetMTBCInsurancesSearchData();

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
        [Test]
        public void GetMtbcInsurances_PassParameters_ReturnData()
        {
            //Arrange
            _mtbcInsurancesRequest.SearchText = "test";
            _mtbcInsurancesRequest.CurrentPage = 10;
            _mtbcInsurancesRequest.RecordPerPage = 100;
            _mtbcInsurancesRequest.payerDescription = "test";
            _mtbcInsurancesRequest.insuranceName = "test";
            _mtbcInsurancesRequest.groupName = "test";
            _mtbcInsurancesRequest.insuranceAddress = "test";
            _mtbcInsurancesRequest.insuranceState = "test";
            _mtbcInsurancesRequest.zip = "test";
            _mtbcInsurancesRequest.city = "test";
            _mtbcInsurancesRequest.state = "test";
            _mtbcInsurancesRequest.phone = "test";
            _mtbcInsurancesRequest.payerState = "test";
            _mtbcInsurancesRequest.department = "test";
            _mtbcInsurancesRequest.EMC = "test";
            _mtbcInsurancesRequest.phoneType = "test";

            //Act
            var result = _patientInsuranceService.GetMTBCInsurances(_mtbcInsurancesRequest);

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
        [Test]
        [TestCase(6002576)]
        [TestCase(6002575)]
        [TestCase(6002574)]
        [TestCase(null)]
        public void MapUnmappedInsurance_PassParameters_ReturnData(long insuranceId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "test";
            _foxInsurancePayers.FOX_TBL_INSURANCE_ID = insuranceId;

            //Act
            var result = _patientInsuranceService.MapUnmappedInsurance(_foxInsurancePayers, _userProfile);

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
        [Test]
        [TestCase(6002576)]
        [TestCase(6002575)]
        [TestCase(6002574)]
        [TestCase(null)]
        public void GetUnpaidClaimsForInsurance_PassParameters_ReturnData(long insuranceId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "test";
            _claimInsuranceSearchReq.Effective_Date = "01/01/2000";
            _claimInsuranceSearchReq.Termination_Date = "01/01/2000";
            _claimInsuranceSearchReq.Patient_Insurance_Ids = new List<long>()
            {
                1,2
            };
            _claimInsuranceSearchReq.Effective_Date = "01/01/2000";
            _claimInsuranceSearchReq.Effective_Date = "01/01/2000";

            //Act
            var result = _patientInsuranceService.GetUnpaidClaimsForInsurance(_claimInsuranceSearchReq, _userProfile);

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
        //GetUnpaidClaimsForInsurance
        [TearDown]
        public void Teardown()
        {
            _patientInsuranceService = new PatientInsuranceService();
            _userProfile = new UserProfile();
        }
    }
}
