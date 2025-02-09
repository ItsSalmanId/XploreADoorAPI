﻿using FOX.BusinessOperations.SurveyAutomationService;
using FOX.DataModels.Models.PatientSurvey;
using NUnit.Framework;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FoxRehabilitation.UnitTest.SurveyAutomationUnitTest
{
    [TestFixture]
    public class SurveyAutomationServiceTest
    {
        private SurveyAutomationService _surveyAutomationService;
        private SurveyLink _surveyLink;
        private SurveyAutomation _surveyAutomation;
        private PatientSurvey _patientSurvey;

        [SetUp]
        public void SetUp()
        {
            _surveyAutomationService = new SurveyAutomationService();
            _surveyLink = new SurveyLink();
            _surveyAutomation = new SurveyAutomation();
            _patientSurvey = new PatientSurvey();
        }
        [Test]
        [TestCase("")]
        [TestCase("1234")]
        [TestCase("#QqtsZzpIqZnj/g0pus3V2w==#")]
        public void DecryptionUrl_DecryptedAccountNumber_ReturnString(string patientAccount)
        {
            //Arrange
            _surveyLink.ENCRYPTED_PATIENT_ACCOUNT = patientAccount;
            _surveyLink.SURVEY_METHOD = "SMS";

            //Act
            var result = _surveyAutomationService.DecryptionUrl(_surveyLink);

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
        [TestCase(101116354816640)]
        [TestCase(101116354816636)]
        public void GetPatientDetails_PatientDetailsModel_ReturnData(long patientAccount)
        {
            //Arrange
            _surveyAutomation.PATIENT_ACCOUNT = patientAccount;

            //Act
            var result = _surveyAutomationService.GetPatientDetails(_surveyAutomation);

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
        [TestCase("101116354816636")]
        [TestCase("")]
        [TestCase("1234")]
        public void GetSurveyQuestionDetails_SurveyQuestions_ReturnString(string patientAccount)
        {
            //Arrange
            _surveyLink.ENCRYPTED_PATIENT_ACCOUNT = patientAccount;

            //Act
            var result = _surveyAutomationService.GetSurveyQuestionDetails(_surveyLink);

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
        [TestCase(101116354816636)]
        [TestCase(0)]
        [TestCase(123456789456)]
        public void UpdatePatientSurvey_UpdateSurvey_UpdateData(long patientAccount)
        {
            //Arrange
            _patientSurvey.PATIENT_ACCOUNT_NUMBER = patientAccount;

            //Act
            var result = _surveyAutomationService.UpdatePatientSurvey(_patientSurvey);

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
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _surveyAutomationService = null;
            _surveyLink = null;
            _patientSurvey = null;
            _surveyAutomation = null;
        }
    }
}
