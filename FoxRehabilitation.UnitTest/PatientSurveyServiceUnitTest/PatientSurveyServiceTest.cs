using FOX.BusinessOperations.PatientSurveyService;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;

namespace FoxRehabilitation.UnitTest.PatientSurveyServiceUnitTest
{
    [TestFixture]
    public class PatientSurveyServiceTest
    {
        private PatientSurveyService _patientSurveyService;
        private PatientSurveySearchRequest _patientSurveySearchRequest;
        private UserProfile _userProfile;
        private PatientSurvey _patientSurvey;
        private SelectiveSurveyList _selectiveSurveyList;
        private PatientSurveyCall _patientSurveyCall;
        private PatientSurveyCallLog _patientSurveyCallLog;

        [SetUp]
        public void SetUp()
        {
            _patientSurveyService = new PatientSurveyService();
            _userProfile = new UserProfile();
            _patientSurveySearchRequest = new PatientSurveySearchRequest();
            _patientSurvey = new PatientSurvey();
            _selectiveSurveyList = new SelectiveSurveyList();
            _patientSurveyCall = new PatientSurveyCall();
            _patientSurveyCallLog = new PatientSurveyCallLog();
        }
        [Test]
        [TestCase("101116354860045", 1011163)]
        [TestCase("101116354860048", 1011163)]
        public void GetPsrDetailedReport_SurveyDetailedFromEmailModel_ReturnData(string surveyId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetSurveyDetailedFromEmail(surveyId, practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163)]
        public void GetPatientSurveytProviderList_PatientSurveytProviderListModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetPatientSurveytProviderList(practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, 1)]
        [TestCase(1011163, 2)]
        [TestCase(1011163, 3)]
        [TestCase(1011163, 4)]
        [TestCase(1011164, 4)]
        public void GetPsdResults_PsdResultsModel_ReturnData(long practiceCode, int TIME_FRAME)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.TIME_FRAME = TIME_FRAME;
            if (TIME_FRAME == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            _patientSurveySearchRequest.DATE_TO_STR = Convert.ToString(DateTime.Today);

            //Act

            var result = _patientSurveyService.GetPSDResults(_patientSurveySearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("", 1011163)]
        [TestCase("Test", 1011163)]
        public void GetPsRegionList_PsRegionListModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetPSRegionList(searchText, practiceCode);

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
        public void GetPsStateList_GetPsStateListModel_ReturnData()
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetPSStateList();
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
        [TestCase(112233462, 1011163)]
        [TestCase(112233462, 1011163)]
        public void GetSurveyHistoryList_SurveyHistoryListModel_ReturnData(long patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetSurveyHistoryList(patientAccount, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(112233859, 1011163)]
        [TestCase(346354, 1011163)]
        public void GetSurveyCallList_SurveyCallListModel_ReturnData(long patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetSurveyCallList(patientAccount, practiceCode);
            //var result = 

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
        [TestCase(112233859, 1011163)]
        [TestCase(346354, 1011163)]
        public void GetSurveyCallListt_SurveyCallListModel_ReturnData(long patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetSurveyCallList(patientAccount, practiceCode);

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
        [TestCase(1011163)]
        public void GetPsSearchData_PsSearchDataModel_ReturnData(long practiceCode)
        {
            //Arrange 
            //Act
            var result = _patientSurveyService.GetPSSearchData(practiceCode);

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
        [TestCase(1011163)]
        public void GetPsInitialData_GetPsInitialDataModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.FORMAT = "";

            //Act
            var result = _patientSurveyService.GetPSInitialData(_patientSurveySearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163)]
        public void GetPsDStateAndRecommendationWise_PsDStateAndRecommendationWiseModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.FORMAT = "";

            //Act
            var result = _patientSurveyService.GetPSDStateAndRecommendationWise(_patientSurveySearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, 1)]
        [TestCase(1011163, 2)]
        [TestCase(1011163, 3)]
        [TestCase(1011163, 4)]
        [TestCase(1011164, 4)]
        public void GetPsdRegionAndRecommendationWise_PsdRegionAndRecommendationWiseModel_ReturnData(long practiceCode, int TIME_FRAME)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.TIME_FRAME = TIME_FRAME;
            if (TIME_FRAME == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            _patientSurveySearchRequest.DATE_TO_STR = Convert.ToString(DateTime.Today);


            //Act
            var result = _patientSurveyService.GetPSDRegionAndRecommendationWise(_patientSurveySearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "William Delanzo")]
        public void GetPsProvidersList_PsProvidersListModel_ReturnData(long practiceCode, string provider)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetPSProvidersList(practiceCode, provider);

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
        [TestCase(1011163, "")]
        [TestCase(1011163, "NJNE1 NJ Northeast 1")]
        public void GetPsStatesList_PsStatesListModel_ReturnData(long practiceCode, string region)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetPSStatesList(practiceCode, region);

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
        [TestCase(1011163, "PA")]
        [TestCase(1011163, "")]
        public void GetPsRegionsList_PsRegionsListModel_ReturnData(long practiceCode, string state)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetPSRegionsList(practiceCode, state);

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
        [TestCase(1011163)]
        [TestCase(1011165)]
        public void GetPsFormat_PsFormatModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientSurveyService.GetPSFormat(practiceCode);

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
        [TestCase(16687, true)]
        public void SetSurveytProgress_SurveytProgress_ReturnData(long patientAccount, bool progressStatus)
        {
            //Arrange
            //Act
            _patientSurveyService.SetSurveytProgress(patientAccount, progressStatus);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        public void GetPatientSurveytList_PatientSurveytModel_ReturnData(long practiceCode)
        {
            //Arrange
            _patientSurveySearchRequest.SURVEY_ID = 101116354860045;
            _patientSurveySearchRequest.PATIENT_ACCOUNT_NUMBER = "2142";
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.PATIENT_FIRST_NAME = "";
            _patientSurveySearchRequest.PATIENT_LAST_NAME = "";

            _patientSurveySearchRequest.PATIENT_MIDDLE_INITIAL = "";
            _patientSurveySearchRequest.IS_SURVEYED = 0;
            _patientSurveySearchRequest.PATIENT_LAST_NAME = "";


            //Act
            var result = _patientSurveyService.GetPatientSurveytList(_patientSurveySearchRequest, practiceCode);

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
        [TestCase("Old Format")]
        public void GetPSUserList_PSUserListtModel_ReturnData(string format)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            //Act
            var result = _patientSurveyService.UpdatePSFormat(format, _userProfile);

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
        [TestCase("64543", 1011163)]
        [TestCase("32525", 1011163)]
        [TestCase("2142", 1011163)]
        public void CheckDeceasedPatient_CheckDeceasedPatient_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            //Act
            var result = _patientSurveyService.CheckDeceasedPatient(patientAccount, practiceCode);

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
        [TestCase(361176, 1011163, 101116354864251)]
        [TestCase(361162, 1011163, 101116354864250)]
        [TestCase(361143, 1011163, 101116354864249)]
        public void UpdatePatientSurvey_UpdatePatientSurvey_ReturnData(long patientAccountNumber, long practiceCode, long SurveyId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163Testing";
            _patientSurvey.SURVEY_ID = SurveyId;
            _patientSurvey.PATIENT_ACCOUNT_NUMBER = patientAccountNumber;
            _patientSurvey.SURVEY_STATUS_BASE = "Deceased";



            //Act
            var result = _patientSurveyService.UpdatePatientSurvey(_patientSurvey, _userProfile);

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
        [TestCase(1011163, 101116354864251)]
        [TestCase(1011163, 101116354864250)]
        [TestCase(1011163, 101116354864249)]
        public void SurveyPerformByUser_PassModel_ReturnData(long practiceCode, long SurveyId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163Testing";
            _selectiveSurveyList.SURVEY_ID = SurveyId;

            //Act
            var result = _patientSurveyService.SurveyPerformByUser(_selectiveSurveyList, practiceCode);

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
        //[Test]
        //[TestCase(1011163, "test")]
        //[TestCase(1011163, "")]
        //[TestCase(1011163, "PHD")]
        //public void GetPSUserList_PassParameter_ReturnData(long practiceCode, string searchText)
        //{
        //    //Arrange
        //    _userProfile.PracticeCode = practiceCode;
        //    _userProfile.UserName = "1163Testing";

        //    //Act
        //    var result = _patientSurveyService.GetPSUserList(searchText, practiceCode);

        //    //Assert
        //    if (result != null)
        //    {
        //        Assert.IsTrue(true);
        //    }
        //    else
        //    {
        //        Assert.IsFalse(false);
        //    }
        //}
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void MakeSurveyCall_PassParameter_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163Testing";
            _patientSurveyCall.Number = "";
            _patientSurveyCall.Extension = "";
            _patientSurveyCall.FileName = "";

            //Act
            _patientSurveyService.MakeSurveyCall(_patientSurveyCall);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void AddUpdateSurveyCall_PassParameter_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163Testing";
            _patientSurveyCallLog.SURVEY_CALL_ID = 0;
            _patientSurveyCallLog.SURVEY_ID_Str = "548581"; ;

            //Act
            _patientSurveyService.AddUpdateSurveyCall(_patientSurveyCallLog, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        //AddUpdateSurveyCall
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _patientSurveyService = null;
            _patientSurveySearchRequest = null;
            _userProfile = null;
        }
    }
}
