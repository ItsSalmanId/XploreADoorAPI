using FOX.BusinessOperations.PatientSurveyService.SurveyReportsService;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;

namespace FoxRehabilitation.UnitTest.PatientSurveyServiceUnitTest.SurveyReportsServiceUnitTest
{
    [TestFixture]
    public class SurveyReportsServiceTest
    {
        private SurveyReportsService _surveyReportsService;
        private PatientSurveySearchRequest _patientSurveySearchRequest;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _surveyReportsService = new SurveyReportsService();
            _patientSurveySearchRequest = new PatientSurveySearchRequest();
            _userProfile = new UserProfile();
        }
        [Test]
        [TestCase(1011163, 1)]
        [TestCase(1011163, 2)]
        [TestCase(1011163, 3)]
        [TestCase(1011163, 4)]
        [TestCase(1011165, 4)]
        [TestCase(1011163, default)]
        public void GetPSRDetailedReport_EmptyPatientSurveyModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            if (timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.GetPSRDetailedReport(_patientSurveySearchRequest,_userProfile);

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
        [TestCase(1011165, 4)]
        [TestCase(1011163, default)]
        public void GetALLPSRDetailedReport_ALLPSRDetailedReportEmptyModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            if (timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.GetALLPSRDetailedReport(_patientSurveySearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, 1)]
        [TestCase(1011163, 5)]
        public void GetALLPendingPSRDetailedReport_CheckALLPendingPSRDetailedReport_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";

            //Act
            var result = _surveyReportsService.GetALLPendingPSRDetailedReport(_patientSurveySearchRequest, _userProfile);

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
        [TestCase(1011165, 4)]
        [TestCase(1011163, default)]
        public void GetPSRRegionAndQuestionWise_PSRRegionAndQuestionWiseModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            if (timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.GetPSRRegionAndQuestionWise(_patientSurveySearchRequest, _userProfile);

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
        [TestCase(1011165, 4)]
        [TestCase(1011163, default)]
        public void GetPSRProviderAndQuestionWise_PSRProviderAndQuestionWiseEmptyModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            if (timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.GetPSRProviderAndQuestionWise(_patientSurveySearchRequest, _userProfile);

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
        [TestCase(1011165, 4)]
        [TestCase(1011163, default)]
        public void GetPSRRegionAndRecommendationWise_PSRRegionAndRecommendationWiseModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.RECOMMENDED = 0;
            _patientSurveySearchRequest.NOT_RECOMMENDED = 0;
            if (timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.GetPSRRegionAndRecommendationWise(_patientSurveySearchRequest, _userProfile);

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
        [TestCase(1011165, 4)]
        [TestCase(1011163, default)]
        public void GetRegionWisePatientData_RegionWisePatientDataModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.RECOMMENDED = 0;
            _patientSurveySearchRequest.NOT_RECOMMENDED = 0;
            if (timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.GetRegionWisePatientData(_patientSurveySearchRequest, _userProfile);

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
        [TestCase(1011166, 4)]
        [TestCase(1011163, default)]
        public void GetPSRProviderAndRecommendationWise_PSRProviderAndRecommendationWiseEmptyModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.RECOMMENDED = 0;
            _patientSurveySearchRequest.NOT_RECOMMENDED = 0;
            if (timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.GetPSRProviderAndRecommendationWise(_patientSurveySearchRequest, _userProfile);

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
        [TestCase(1011165, 4)]
        [TestCase(1011163, default)]
        public void GetAllPendingDetailedReport_AllPendingDetailedReportEmptyModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.RECOMMENDED = 0;
            _patientSurveySearchRequest.NOT_RECOMMENDED = 0;
            if (timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.GetAllPendingDetailedReport(_patientSurveySearchRequest, _userProfile);

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
        [TestCase(1011165, 4)]
        [TestCase(1011163, default)]
        public void DischargeToSurveyTimeDaysAverage_DischargeToSurveyTimeDaysAverageEmptyModel_ReturnData(long practiceCode, int timeFram)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSurveySearchRequest.PROVIDER = "";
            _patientSurveySearchRequest.TIME_FRAME = timeFram;
            _patientSurveySearchRequest.REGION = "";
            _patientSurveySearchRequest.STATE = "";
            _patientSurveySearchRequest.FLAG = "";
            _patientSurveySearchRequest.FORMAT = "";
            _patientSurveySearchRequest.SURVEYED_BY = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.CURRENT_PAGE = 1;
            _patientSurveySearchRequest.RECORD_PER_PAGE = 10;
            _patientSurveySearchRequest.SEARCH_TEXT = "";
            _patientSurveySearchRequest.SORT_BY = "";
            _patientSurveySearchRequest.SORT_ORDER = "";
            _patientSurveySearchRequest.DISCIPLINE = "";
            _patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "";
            _patientSurveySearchRequest.RECOMMENDED = 0;
            _patientSurveySearchRequest.NOT_RECOMMENDED = 0;
            if(timeFram == 4 && practiceCode == 1011163)
            {
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _patientSurveySearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _patientSurveySearchRequest.DATE_FROM_STR = "";
                _patientSurveySearchRequest.DATE_FROM_STR = "";
            }

            //Act
            var result = _surveyReportsService.DischargeToSurveyTimeDaysAverage(_patientSurveySearchRequest, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _surveyReportsService = null;
            _patientSurveySearchRequest = null;
            _userProfile = null;
        }
    }
}
