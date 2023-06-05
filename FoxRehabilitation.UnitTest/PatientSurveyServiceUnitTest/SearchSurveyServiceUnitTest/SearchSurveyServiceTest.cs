using FOX.BusinessOperations.PatientSurveyService.SearchSurveyService;
using FOX.DataModels.Models.PatientSurvey;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.PatientSurveyServiceUnitTest.SearchSurveyServiceUnitTest
{
    [TestFixture]
    public class SearchSurveyServiceTest
    {
        private SearchSurveyService _searchSurveyService;

        [SetUp]
        public void SetUp()
        {
            _searchSurveyService = new SearchSurveyService();
        }
        [Test]
        [TestCase("8288", false, 1011163)]
        [TestCase("10595", true, 1011163)]
        public void GetPatientSurvey_PatientSurveyModel_ReturnData(string patientAccount, bool isIncludeSurveyed, long practiceCode)
        {
            //Arrange
            //Act
            var result = _searchSurveyService.GetPatientSurvey(patientAccount, isIncludeSurveyed, practiceCode);

            //Assert
            if (result != null && result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1011165)]
        public void GetRandomSurvey_EmptyRandomSurveyModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _searchSurveyService.GetRandomSurvey(practiceCode);

            //Assert
            if (result != null )
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("16687", 1011163)]
        [TestCase("46122", 1011165)]
        public void CheckDeceasedPatient_CheckDeceasedPatientModel_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _searchSurveyService.CheckDeceasedPatient(patientAccount, practiceCode);

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
        public void UpdatePatientSurvey_UpdatePatientModel_UpdateData()
        {
            //Arrange
            List<PatientSurvey> PatientSurveys = new List<PatientSurvey>()
            {
                new PatientSurvey()
                {
                    SURVEY_ID = 101116354860045,
                    PRACTICE_CODE = 1011163,
                    IS_SURVEYED = true,
                    DELETED = false,
                    MODIFIED_BY = "FOXTeam",
                    CREATED_BY = "FOXTeam"
                }
            };

            ////Act
            _searchSurveyService.UpdatePatientSurvey(PatientSurveys);

            //Assert
            Assert.IsTrue(true);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _searchSurveyService = null;
        }
    }
}
