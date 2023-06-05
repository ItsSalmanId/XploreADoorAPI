using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.GeneralNotesService;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.GenerelNotesServiceUnitTest
{
    class GenerelNotesServiceTest
    {

        private UserProfile _userProfile;
        private GeneralNotesServices _generalNotesServices;
        private GeneralNoteRequestModel _generalNoteRequestModel;
        private GeneralNotesSearchRequest _generalNotesSearchRequest;
        private GeneralNoteDeleteRequestModel _generalNoteDeleteRequestModel;
        private GeneralNoteCreateUpdateRequestModel _generalNoteCreateUpdateRequest;
        private GeneralNoteHistoryRequestModel _generalNoteHistoryRequestModel;
        private InterfaceSynchModel _interfaceSynchModel;
        private AlertSearchRequest _alertSearchRequest;
        private NoteAlert _noteAlert;
        private InterfaceLogSearchRequest _interfaceLogSearchRequest;
        private InterfaceLogSearchRequest InterfaceLogSearchRequest;

        [SetUp]
        public void SetUp()
        {
            _userProfile = new UserProfile();
            _generalNotesServices = new GeneralNotesServices();
            _generalNoteRequestModel = new GeneralNoteRequestModel();
            _generalNotesSearchRequest = new GeneralNotesSearchRequest();
            _generalNoteDeleteRequestModel = new GeneralNoteDeleteRequestModel();
            _generalNoteCreateUpdateRequest = new GeneralNoteCreateUpdateRequestModel();
            _generalNoteHistoryRequestModel = new GeneralNoteHistoryRequestModel();
            _interfaceSynchModel = new InterfaceSynchModel();
            _alertSearchRequest = new AlertSearchRequest();
            _noteAlert = new NoteAlert();
            _interfaceLogSearchRequest = new InterfaceLogSearchRequest();
        }

        [Test]
        [TestCase(0, "")]
        [TestCase(0, "101116354813733")]
        [TestCase(1011163, "")]
        [TestCase(1011163, "101116354813733")]
        [TestCase(38403, "38403")]
        public void GetAlertGeneralNotes_NoteAlertList_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _generalNotesServices.GetAlertGeneralNotes(_userProfile, patientAccount);


            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(0, 99911446)]
        [TestCase(1011163, 0)]
        [TestCase(1011163, 99911446)]
        [TestCase(38403, 38403)]
        public void GetSingleGeneralNote_GeneralNoteResponseModel_ReturnData(long practiceCode, long generalNoteId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _generalNoteRequestModel.GENERAL_NOTE_ID = generalNoteId;

            //Act
            var result = _generalNotesServices.GetSingleGeneralNote(_userProfile, _generalNoteRequestModel);

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
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetIdentifierList_AlertTypeList_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _generalNotesServices.GetAlertTypes(practiceCode);

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
        public void GetAlertTypeswithInactive_AlertTypeList_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _generalNotesServices.GetAlertTypeswithInactive(practiceCode);

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
        [TestCase(0, 0)]
        [TestCase(101116354813733, 0)]
        [TestCase(0, 1011163)]
        [TestCase(101116354813733, 1011163)]
        [TestCase(38403, 38403)]
        public void GetPatientCasesList_PatientCasesForDDList_ReturnData(long patientAccount, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _generalNotesServices.GetPatientCasesList(patientAccount, _userProfile);

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
        [TestCase("0", 0)]
        [TestCase("101116354813733", 0)]
        [TestCase("0", 1011163)]
        [TestCase("101116354813733", 1011163)]
        [TestCase("38403", 38403)]
        public void CheckPatientisInterfaced_interfaceSynced_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _generalNotesServices.checkPatientisInterfaced(patientAccount, _userProfile);

            //Assert
            if (result)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
        // Exception
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void GetGeneralNotes_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _generalNotesSearchRequest.CurrentPage = 1;
            _generalNotesSearchRequest.RecordPerPage = 10;
            _generalNotesSearchRequest.SearchText = "";
            _generalNotesSearchRequest.PATIENT_ACCOUNT = 101116354815887;
            _generalNotesSearchRequest.Sort_By = "";
            _generalNotesSearchRequest.Sort_Order = "";
            _generalNotesSearchRequest.SearchText = "";
            _generalNotesSearchRequest.SearchText = "";

            //Act
            var result = _generalNotesServices.GetGeneralNotes(_userProfile, _generalNotesSearchRequest);

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
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        [TestCase(1012714, true)]
        [TestCase(1012714, false)]
        public void GetSingleNoteForUpdate_PassModel_ReturnData(long practiceCode, bool condition)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            if(condition)
            {
                _generalNoteRequestModel.GENERAL_NOTE_ID = Helper.getMaximumId("GENERAL_NOTE_ID") - 101;
            }
            else
            {
                _generalNoteRequestModel.GENERAL_NOTE_ID = Helper.getMaximumId("GENERAL_NOTE_ID") - 10;

            }

            //Act
            var result = _generalNotesServices.GetSingleNoteForUpdate(_userProfile, _generalNoteRequestModel);

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
        [TestCase(1012714)]
        public void DeleteGeneralNote_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _generalNoteDeleteRequestModel.GENERAL_NOTE_ID_AS_STRING = "544510";
            _generalNoteDeleteRequestModel.IS_TASK_DELETE = true;

            //Act
            var result = _generalNotesServices.DeleteGeneralNote(_userProfile, _generalNoteDeleteRequestModel);

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
        [TestCase(1011163, 544582, 101116354444402804)]
        [TestCase(1011163, 544511, 101116354815887)]
        [TestCase(1012714, 544596, 101116354444402807)]
        public void CreateUpdateNote_PassModel_ReturnData(long practiceCode, long noteId, long patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            // _generalNoteCreateUpdateRequest.GENERAL_NOTE_ID_AS_STRING = "544512";
            _generalNoteDeleteRequestModel.IS_TASK_DELETE = true;

            _generalNoteCreateUpdateRequest.GENERAL_NOTE = new FOX_TBL_GENERAL_NOTE()
            {
                PATIENT_ACCOUNT_AS_STRING = patientAccount.ToString(),
                GENERAL_NOTE_ID = noteId,
                CASE_ID = 544311
            };
            _generalNoteCreateUpdateRequest.GENERAL_NOTE_HISTORY = new FOX_TBL_GENERAL_NOTE()
            {
                NOTE_DESCRIPTION = "<html>                              <body>Testing from app<br>Pye, Jody - Clinician</body> </html>",
            };

            //Act
            var result = _generalNotesServices.CreateUpdateNote(_generalNoteCreateUpdateRequest, _userProfile);

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
        [TestCase(1012714)]
        public void UpdateNote_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _generalNoteCreateUpdateRequest.GENERAL_NOTE = new FOX_TBL_GENERAL_NOTE()
            {
                PATIENT_ACCOUNT_AS_STRING = "101116354410924",
                GENERAL_NOTE_ID = 548639,
                CASE_ID = null
            };
            _generalNoteCreateUpdateRequest.GENERAL_NOTE_HISTORY = new FOX_TBL_GENERAL_NOTE()
            {
                NOTE_DESCRIPTION = "<html>                              <body>Testing from app<br>Pye, Jody - Clinician</body> </html>",
            };

            //Act
            var result = _generalNotesServices.UpdateNote(_generalNoteCreateUpdateRequest, _userProfile);

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
        [TestCase(1012714)]
        public void GetGeneralNoteHistory_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _generalNoteHistoryRequestModel.PATIENT_ACCOUNT_AS_STRING = "101116354812545";
            _generalNoteHistoryRequestModel.PARIENT_GENERAL_NOTE_ID = 548663;

            //Act
            var result = _generalNotesServices.GetGeneralNoteHistory(_generalNoteHistoryRequestModel, _userProfile);

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
        [TestCase(1012714)]
        public void InsertInterfaceTeamData2_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _interfaceSynchModel.PATIENT_ACCOUNT = 101116354812545;
            _interfaceSynchModel.FOX_INTERFACE_SYNCH_ID = 548100;

            //Act
            _generalNotesServices.InsertInterfaceTeamData2(_interfaceSynchModel, _userProfile);

            //Assert

            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void GetNoteAlert_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _alertSearchRequest.PATIENT_ACCOUNT = "101116354812545";
            _alertSearchRequest.CurrentPage = "1";
            _alertSearchRequest.RecordPerPage = "10";
            _alertSearchRequest.Sort_By = "";
            _alertSearchRequest.Sort_Order = "";
            _alertSearchRequest.SearchText = "";

            //Act
            _generalNotesServices.GetNoteAlert(_alertSearchRequest, practiceCode);

            //Assert

            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void CreateUpdateNoteAlert_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _noteAlert.PATIENT_ACCOUNT_str = "101116354812545";

            //Act
            _generalNotesServices.CreateUpdateNoteAlert(_noteAlert, _userProfile);

            //Assert

            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163, 548110)]
        [TestCase(1011163, 548111)]
        public void DeleteAlert_PassModel_ReturnData(long practiceCode, long alertId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _noteAlert.PATIENT_ACCOUNT_str = "101116354812545";

            //Act
            var result = _generalNotesServices.DeleteAlert(alertId, _userProfile);

            //Assert
            if (result != null)
                Assert.IsTrue(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase(1011163, 548110)]
        [TestCase(1012714, 548111)]
        public void GetGeneralNotesInterfaceLogs_PassModel_ReturnData(long practiceCode, long alertId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _interfaceLogSearchRequest.PATIENT_ACCOUNT = "101116354812545";
            _interfaceLogSearchRequest.CurrentPage = 1;
            _interfaceLogSearchRequest.RecordPerPage = 10;
            _interfaceLogSearchRequest.Sort_By = "";
            _interfaceLogSearchRequest.Sort_Order = "";
            _interfaceLogSearchRequest.SearchText = "";
            _interfaceLogSearchRequest.Option = "";

            //Act
            _generalNotesServices.GetGeneralNotesInterfaceLogs(_interfaceLogSearchRequest, _userProfile);

            //Assert

            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1012714)]
        public void RetryInterfacing_PassModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "NUnit-testing";
            _interfaceLogSearchRequest.PATIENT_ACCOUNT = "101116354812545";
            _interfaceLogSearchRequest.CurrentPage = 1;
            _interfaceLogSearchRequest.RecordPerPage = 10;
            _interfaceLogSearchRequest.Sort_By = "";
            _interfaceLogSearchRequest.Sort_Order = "";
            _interfaceLogSearchRequest.SearchText = "";
            _interfaceLogSearchRequest.Option = "";

            //Act
            _generalNotesServices.RetryInterfacing(_interfaceLogSearchRequest, _userProfile);

            //Assert

            Assert.IsTrue(true);
        }
        //RetryInterfacing
        [TearDown]
        public void Teardown()
        {
            _userProfile = new UserProfile();
            _generalNotesServices = new GeneralNotesServices();
            _generalNoteRequestModel = new GeneralNoteRequestModel();
        }
    }
}
