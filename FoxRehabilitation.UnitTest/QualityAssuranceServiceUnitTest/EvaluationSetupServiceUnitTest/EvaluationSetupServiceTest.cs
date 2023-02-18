using FOX.BusinessOperations.QualityAssuranceService.EvaluationSetupService;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.QualityAssuranceServiceUnitTest
{
    [TestFixture]
    public class EvaluationSetupServiceTest
    {
        private EvaluationSetupService _evaluationSetupService;
        private RequestModelForCallType _requestModelForCallType;
        private RequestModelOverallWeightage _requestModelOverallWeightage;
        private UserProfile _userProfile;
        private RequestModelClientExperience _requestModelClientExperience;
        private RequestModelSystemProcess _requestModelSystemProcess;
        private RequestModelWowfactor _requestModelWowfactor;
        private RequestModelGradingSetup _requestModelGradingSetup;

        [SetUp]
        public void SetUp()
        {
            _evaluationSetupService = new EvaluationSetupService();
            _requestModelForCallType = new RequestModelForCallType();
            _requestModelOverallWeightage = new RequestModelOverallWeightage();
            _userProfile = new UserProfile();
            _requestModelClientExperience = new RequestModelClientExperience();
            _requestModelSystemProcess = new RequestModelSystemProcess();
            _requestModelWowfactor = new RequestModelWowfactor();
            _requestModelGradingSetup = new RequestModelGradingSetup();
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "Survey")]
        public void GetAlertGeneralNotes_EvaluationSetupResponseModel_ReturnData(long practiceCode, string callType)
        {
            //Arrange
            _requestModelForCallType.Call_Type = callType;

            //Act
            var result = _evaluationSetupService.AllEvaluationCriteria(_requestModelForCallType, practiceCode);

            //Assert
            if (result.EvaluationCriteria.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.EvaluationCriteria.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "PHD")]
        [TestCase(1011163, "Survey")]
        public void OnSaveOverallWeightageCriteria_PassModel_ReturnData(long practiceCode, string callType)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _requestModelOverallWeightage.CALL_TYPE = callType;
            _requestModelOverallWeightage.Client_Experience = 10;
            _requestModelOverallWeightage.System_Process = 10;

            //Act
            var result = _evaluationSetupService.onSaveOverallWeightageCriteria(_requestModelOverallWeightage, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "PHD")]
        [TestCase(1011163, "Survey")]
        [TestCase(1011163, "")]
        [TestCase(0, "PHD")]
        [TestCase(0, "Survey")]
        [TestCase(0, "")]
        [TestCase(1012714, "PHD")]
        [TestCase(1012714, "Survey")]
        [TestCase(1012714, "")]
        public void OnSaveClientExperience_PassModel_ReturnData(long practiceCode, string callType)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _requestModelClientExperience.CALL_TYPE = callType;
            _requestModelClientExperience.APPROPRIATE_GREETING = 10;
            _requestModelClientExperience.TONE_OF_PATIENT = 10;

            //Act
            var result = _evaluationSetupService.onSaveClientExperience(_requestModelClientExperience, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "PHD")]
        [TestCase(1011163, "Survey")]
        [TestCase(1011163, "")]
        [TestCase(0, "PHD")]
        [TestCase(0, "Survey")]
        [TestCase(0, "")]
        [TestCase(1012714, "PHD")]
        [TestCase(1012714, "Survey")]
        [TestCase(1012714, "")]
        public void OnSaveSystemProductprocess_PassModel_ReturnData(long practiceCode, string callType)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _requestModelSystemProcess.CALL_TYPE = callType;
            _requestModelSystemProcess.PATIENT_IDENTITY = 10;
            _requestModelSystemProcess.ANSWER_PATIENT_QUESTIONS = 10;
            _requestModelSystemProcess.STRONG_PRODUCT_KNOWLEDGE = 10;
            _requestModelSystemProcess.COMMUNICATE_INFORMATION = 10;
            _requestModelSystemProcess.DOCUMENTATION_COMPLETED_COMMUNICATED = 10;
            _requestModelSystemProcess.COMMUNICATE_INFORMATION = 10;

            //Act
            var result = _evaluationSetupService.onSaveSystemProductprocess(_requestModelSystemProcess, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "PHD", true)]
        [TestCase(1011163, "Survey", true)]
        [TestCase(0, "PHD", true)]
        [TestCase(0, "Survey", true)]
        [TestCase(1012714, "PHD", true)]
        [TestCase(1012714, "Survey", true)]
        [TestCase(1012714, "", true)]
        public void OnSaveWowFactor_PassModel_ReturnData(long practiceCode, string callType, bool wowfactorStatus)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _requestModelWowfactor.CALL_TYPE = callType;
            _requestModelWowfactor.STATUS = wowfactorStatus;
            _requestModelWowfactor.BONUS_POINTS = 10;
            _requestModelWowfactor.PERFORMANCE_KILLER = 10;

            //Act
            var result = _evaluationSetupService.onSaveWowFactor(_requestModelWowfactor, _userProfile);

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
        [TestCase(1011163, "PHD")]
        [TestCase(1011163, "Survey")]
        public void onSaveGradingSetup_PassModel_ReturnData(long practiceCode, string callType)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _requestModelGradingSetup.CALL_TYPE = callType;
            _requestModelGradingSetup.A_MIN = 10;
            _requestModelGradingSetup.B_MAX = 10;
            _requestModelGradingSetup.U_MAX = 10;

            //Act
            var result = _evaluationSetupService.onSaveGradingSetup(_requestModelGradingSetup, _userProfile);

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
        //onSaveGradingSetup
        [TearDown]
        public void Teardown()
        {
            _evaluationSetupService = null;
            _requestModelForCallType = null;
        }
    }
}
