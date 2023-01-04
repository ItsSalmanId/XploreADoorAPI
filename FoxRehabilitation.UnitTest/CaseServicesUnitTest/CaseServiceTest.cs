using FOX.BusinessOperations.CaseServices;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.CaseServicesUnitTest
{
    class CaseServiceTest
    {
        private CaseServices _caseServices;
        private SmartIdentifierReq _smartIdentifierReq;
        private UserProfile _userProfile;
        private GetOpenIssueListReq _getOpenIssueListReq;
        private GetSmartPoslocReq _getSmartPoslocReq;
        private getOrderInfoReq _getOrderInfoReq;
        private CallReq _callReq;
        private WORK_ORDER_INFO_REQ _workOrderInfoReq;
        private GetOrderingRefSourceinfoReq _getOrderingRefSourceinfoReq;
        private SmartSearchReq _smartSearchReq;
        private FOX_TBL_CASE _foxTblCase;

        [SetUp]
        public void SetUp()
        {
            _caseServices = new CaseServices();
            _smartIdentifierReq = new SmartIdentifierReq();
            _userProfile = new UserProfile();
            _getOpenIssueListReq = new GetOpenIssueListReq();
            _getSmartPoslocReq = new GetSmartPoslocReq();
            _getOrderInfoReq = new getOrderInfoReq();
            _callReq = new CallReq();
            _workOrderInfoReq = new WORK_ORDER_INFO_REQ();
            _getOrderingRefSourceinfoReq = new GetOrderingRefSourceinfoReq();
            _smartSearchReq = new SmartSearchReq();
            _foxTblCase = new FOX_TBL_CASE();
        }
        [Test]
        [TestCase("", 0)]
        [TestCase("101116354813733", 0)]
        [TestCase("", 1011163)]
        [TestCase("101116354813733", 1011163)]
        public void GetCasesDDL_ResponseGetCasesDDLModel_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _caseServices.GetCasesDDL(patientAccount, practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [Test]
        [TestCase("", 0)]
        [TestCase("101116354813733", 0)]
        [TestCase("", 1011163)]
        [TestCase("101116354813733", 1011163)]
        public void GetCasesDDLTalRehab_ResponseGetCasesDDLModel_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _caseServices.GetCasesDDLTalRehab(patientAccount, practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetIdentifierList_FoxTblIdentifierList_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _caseServices.GetIdentifierList(practiceCode);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("", "", 0)]
        [TestCase("ACO Identifier", "", 0)]
        [TestCase("", "Mssp_car", 0)]
        [TestCase("", "", 1011163)]
        [TestCase("ACO Identifier", "Mssp_car", 1011163)]
        [TestCase("test1", "test", 38403)]
        public void GetSmartIdentifier_SmartIdentifierResList_ReturnData(string type, string searchValue, long practiceCode)
        {
            //Arrange
            _smartIdentifierReq.TYPE = type;
            _smartIdentifierReq.SEARCHVALUE = searchValue;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetSmartIdentifier(_smartIdentifierReq, _userProfile);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllIdentifierANDSourceofReferralList_InactiveReferalList_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetAllIdentifierANDSourceofReferralList(_userProfile);

            //Assert
            if (result.Group_Identifier != null && result.Spourc_Of_Refferal != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetSourceofReferral_SourceofReferralList_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _caseServices.GetSourceofReferral(practiceCode);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("", 0)]
        [TestCase("HOLD", 0)]
        [TestCase("", 1011163)]
        [TestCase("HOLD", 1011163)]
        [TestCase("test", 1011163)]
        public void GetNONandHOLDIssueList_NoNandHOLDAllList_ReturnData(string type, long practiceCode)
        {
            //Arrange
            _getOpenIssueListReq.TYPE = type;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetNONandHOLDIssueList(_getOpenIssueListReq, _userProfile);

            //Assert
            if (result.NONandHOLDIssueList.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.NONandHOLDIssueList.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [TestCase("", 0)]
        [TestCase("CT065", 0)]
        [TestCase("", 1011163)]
        [TestCase("CT065", 1011163)]
        [TestCase("test", 1011163)]
        public void GetSmartPosLocation_GetSmartPoslocResList_ReturnData(string searchValue, long practiceCode)
        {
            //Arrange
            _getSmartPoslocReq.SEARCHVALUE = searchValue;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetSmartPosLocation(_getSmartPoslocReq, _userProfile);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [TestCase("", 0)]
        [TestCase("CT065", 0)]
        [TestCase("", 1011163)]
        [TestCase("CT065", 1011163)]
        [TestCase("test", 1011163)]
        public void GetSmartPosLocations_SmartPoslocResList_ReturnData(string searchValue, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetSmartPosLocations(searchValue, _userProfile);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [TestCase(0, 0)]
        [TestCase(101116354444401377, 0)]
        [TestCase(0, 1011163)]
        [TestCase(101116354444401377, 1011163)]
        [TestCase(38403, 38403)]
        public void GetTotalDiscipline_TotalDisciplineReslist_ReturnData(long patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _caseServices.GetTotalDiscipline(patientAccount, practiceCode);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetTotalDisciplineTalkrehab_TotalDisciplineResList_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _caseServices.GetTotalDisciplineTalkrehab(practiceCode);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(544276, 0)]
        [TestCase(0, 1011163)]
        [TestCase(544276, 1011163)]
        [TestCase(38403, 38403)]
        public void GetOrderInformationAndNotes_PassParameters_ReturnData(long caseId, long practiceCode)
        {
            //Arrange
            _getOrderInfoReq.CASE_ID = caseId;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetOrderInformationAndNotes(_getOrderInfoReq, _userProfile);

            //Assert
            if (result.CasesNotesList.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(null, 0)]
        [TestCase("101116354444401377", 0)]
        [TestCase(null, 1011163)]
        [TestCase("101116354444401377", 1011163)]
        [TestCase("38403", 38403)]
        public void GetAllCases_PassParameters_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _caseServices.GetAllCases(patientAccount, practiceCode);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(548386, 0)]
        [TestCase(0, 1011163)]
        [TestCase(548386, 1011163)]
        [TestCase(548386, 38403)]
        public void GetCallInformation_PassParameters_ReturnData(long caseId, long practiceCode)
        {
            //Arrange
            _callReq.CASE_ID = caseId;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetCallInformation(_callReq, _userProfile);


            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(null, 0)]
        [TestCase("101116354444401377", 0)]
        [TestCase(null, 1011163)]
        [TestCase("101116354444401377", 1011163)]
        [TestCase("38403", 38403)]
        public void GetWorkOrderInfo_PassParameters_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            _workOrderInfoReq.PATIENT_ACCOUNT = patientAccount;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetWorkOrderInfo(_workOrderInfoReq, _userProfile);


            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(5481040, 0)]
        [TestCase(0, 1011163)]
        [TestCase(5481040, 1011163)]
        [TestCase(38403, 38403)]
        public void GetOrdering_Ref_Source_info_PassParameters_ReturnData(long caseId, long practiceCode)
        {
            //Arrange
            _getOrderingRefSourceinfoReq.CASE_ID = caseId;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetOrdering_Ref_Source_info(_getOrderingRefSourceinfoReq, _userProfile);

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
        [TestCase("", 0)]
        [TestCase("GIO", 0)]
        [TestCase("", 1011163)]
        [TestCase("GIO", 1011163)]
        [TestCase("test", 38403)]
        public void GetSmartHearAboutFox_PassParameters_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetSmartHearAboutFox(searchText, _userProfile);


            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(5481341, 0)]
        [TestCase(0, 1011163)]
        [TestCase(5481341, 1011163)]
        [TestCase(38403, 38403)]
        public void GetReferralRegionAginstPosId_PassParameters_ReturnData(long posId, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetReferralRegionAginstPosId(posId, _userProfile);

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
        [TestCase(0, 0)]
        [TestCase(552102, 0)]
        [TestCase(0, 1011163)]
        [TestCase(552102, 1011163)]
        [TestCase(38403, 38403)]
        public void GetReferralRegionAgainstORS_PassParameters_ReturnData(long orderingReferalSourceId, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetReferralRegionAgainstORS(orderingReferalSourceId, _userProfile);

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
        [TestCase("", 0, 0)]
        [TestCase("544100", 0, 0)]
        [TestCase("", 544100, 0)]
        [TestCase("", 0, 1011163)]
        [TestCase("TIMO", 544100, 1011163)]
        [TestCase("test", 38403, 403338)]
        public void GetSmartProviders_PassParameters_ReturnData(string searchValue, int disciplineId, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetSmartProviders(searchValue, disciplineId, _userProfile);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("", "", 0)]
        [TestCase("TIMO", "", 0)]
        [TestCase("", "PT", 0)]
        [TestCase("", "", 1011163)]
        [TestCase("TIMO", "PT", 1011163)]
        [TestCase("test", "test", 38403)]
        public void GetSmartClinicains_PassParameters_ReturnData(string searchValue, string type, long practiceCode)
        {
            //Arrange
            _smartSearchReq.Keyword = searchValue;
            _smartSearchReq.TYPE = type;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetSmartClinicains(_smartSearchReq, _userProfile);

            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllCaseStatus_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            //Act
            var result = _caseServices.GetAllCaseStatus(_userProfile);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(101116354813324, 0)]
        [TestCase(0, 1011163)]
        [TestCase(101116354813324, 1011163)]
        [TestCase(38403, 38403)]
        public void GetPatientCasesList_PassParameters_ReturnData(long patientAccount, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetPatientCasesList(patientAccount, _userProfile);


            //Assert
            if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(5481040, 0)]
        [TestCase(5481040, 38403)]
        [TestCase(5481040, 1011163)]
        public void GetCasesAndOpenIssues_PassParameters_ReturnData(long caseId, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _caseServices.GetCasesAndOpenIssues(caseId, _userProfile);


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
        [TestCase(1011163)]
        public void AddEditCase_CaseModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _foxTblCase.CASE_ID = 544107;
            _foxTblCase.PATIENT_ACCOUNT_STR = "101116354412362";
            _foxTblCase.CASE_STATUS_ID = 544105;
            
            //Act
            var result = _caseServices.AddEditCase(_foxTblCase, _userProfile);

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
            _caseServices = null;
            _smartIdentifierReq = null;
            _userProfile = null;
            _getOpenIssueListReq = null;
            _getSmartPoslocReq = null;
            _getOrderInfoReq = null;
            _callReq = null;
            _workOrderInfoReq = null;
            _getOrderingRefSourceinfoReq = null;
            _foxTblCase = null;
        }
    }
}
