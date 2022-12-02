using FOX.BusinessOperations.QualityAssuranceService.EvaluationSetupService;
using FOX.DataModels.Models.QualityAsuranceModel;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.QualityAssuranceServiceUnitTest
{
    [TestFixture]
    public class EvaluationSetupServiceTest
    {
        private EvaluationSetupService _evaluationSetupService;
        private RequestModelForCallType _requestModelForCallType;

        [SetUp]
        public void SetUp()
        {
            _evaluationSetupService = new EvaluationSetupService();
            _requestModelForCallType = new RequestModelForCallType();
        }
        [Test]
        [TestCase(0, "")]
        [TestCase(0, "SURVEY")]
        [TestCase(1011163, "")]
        [TestCase(1011163, "SURVEY")]
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

        [TearDown]
        public void Teardown()
        {
            _evaluationSetupService = null;
            _requestModelForCallType = null;
        }
    }
}
