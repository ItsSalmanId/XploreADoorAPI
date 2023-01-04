using FOX.BusinessOperations.Scheduler;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using static FOX.DataModels.Models.Scheduler.SchedulerModel;

namespace FoxRehabilitation.UnitTest.SchedulerUnitTest
{
    [TestFixture]
    class SchedulerServiceTest
    {
        private SchedulerService _scheduler;
        private AppointmentSearchRequest _appointmentSearchRequest;
        private UserProfile _userProfile;
        private Appointment _appointment;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new SchedulerService();
            _appointmentSearchRequest = new AppointmentSearchRequest();
            _userProfile = new UserProfile();
            _appointment = new Appointment();
        }
        [Test]
        [TestCase(1011163, 4)]
        [TestCase(1011163, 1)]
        [TestCase(1011163, 2)]
        [TestCase(1011163, 3)]
        public void GetAllAppointments_AppointmentsModel_ReturnData(long practiceCode, int timeFrame)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            if(timeFrame == 1)
            {
                _appointmentSearchRequest.PATIENT_ACCOUNT = "";
            }
            else
            {
                _appointmentSearchRequest.PATIENT_ACCOUNT = "0";
            }
            
            _appointmentSearchRequest.TIME_FRAME = timeFrame;
            _appointmentSearchRequest.SEARCH_TEXT = "";
            _appointmentSearchRequest.PROVIDER_ID = "0";
            _appointmentSearchRequest.REASON = "0";
            _appointmentSearchRequest.STATUS = "Cancelled,Completed,Pending,Void,Rescheduled,NULL";
            _appointmentSearchRequest.LOCATION = 0;
            _appointmentSearchRequest.DISCIPLINE = "0";
            _appointmentSearchRequest.INSURANCE_ID = "0";
            _appointmentSearchRequest.CURRENT_PAGE = 1;
            _appointmentSearchRequest.RECORD_PER_PAGE = 10000;
            _appointmentSearchRequest.SORT_BY = "AppointmentDate";
            _appointmentSearchRequest.SORT_ORDER = "DESC";
            _appointmentSearchRequest.REGION = "0";
            _appointmentSearchRequest.DATE_TO_STR = "08/03/2020";
            _appointmentSearchRequest.DATE_FROM_STR = "08/03/2020";
            _appointmentSearchRequest.PROVIDER_ID = "0";
            _appointmentSearchRequest.LOCATION = 0;

            //Act
            var result = _scheduler.GetAllAppointments(_appointmentSearchRequest, _userProfile);

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
        [TestCase(1011163, 1)]
        [TestCase(1011163, 2)]
        [TestCase(1011163, 3)]
        [TestCase(1011163, 4)]
        [TestCase(1011163, default)]

        public void GetAllAppointmentsWeekly_AppointmentsWeeklyModel_ReturnData(long practiceCode, int timeFrame)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _appointmentSearchRequest.PATIENT_ACCOUNT = "0";
            _appointmentSearchRequest.TIME_FRAME = timeFrame;
            _appointmentSearchRequest.SEARCH_TEXT = "";
            _appointmentSearchRequest.PROVIDER_ID = "0";
            _appointmentSearchRequest.REASON = "0";
            _appointmentSearchRequest.STATUS = "Cancelled,Completed,Pending,Void,Rescheduled,NULL";
            _appointmentSearchRequest.LOCATION = 0;
            _appointmentSearchRequest.DISCIPLINE = "0";
            _appointmentSearchRequest.INSURANCE_ID = "0";
            _appointmentSearchRequest.CURRENT_PAGE = 1;
            _appointmentSearchRequest.RECORD_PER_PAGE = 10000;
            _appointmentSearchRequest.SORT_BY = "AppointmentDate";
            _appointmentSearchRequest.SORT_ORDER = "DESC";
            _appointmentSearchRequest.REGION = "0";
            _appointmentSearchRequest.DATE_TO_STR = "08/03/2020";
            _appointmentSearchRequest.DATE_FROM_STR = "08/03/2020";
            _appointmentSearchRequest.PROVIDER_ID = "0";
            _appointmentSearchRequest.LOCATION = 0;
            if (timeFrame == 4 && practiceCode == 1011163)
            {
                _appointmentSearchRequest.DATE_FROM_STR = "";
                _appointmentSearchRequest.DATE_TO_STR = "";
            }
            else
            {
                _appointmentSearchRequest.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _appointmentSearchRequest.DATE_TO_STR = Convert.ToString(DateTime.Today);
            }

            //Act
            var result = _scheduler.GetAllAppointmentsWeekly(_appointmentSearchRequest, _userProfile);

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
        public void GetAppointmentStatusList_AppointmentStatusListModel_ReturnData()
        {
            //Arrange
            //Act
            var result = _scheduler.GetAppointmentStatusList();

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
        public void GetVisitTypeList_VisitTypeListModel_ReturnData()
        {
            //Arrange
            //Act
            var result = _scheduler.GetVisitTypeList();

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
        public void GetCancellationReasonsList_CancellationReasonsListModel_ReturnData()
        {
            //Arrange
            //Act
            var result = _scheduler.GetCancellationReasonsList();

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
        [TestCase(1011165)]
        public void GetProviderList_ProviderListModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _scheduler.GetProviderList(practiceCode);

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
        [TestCase(1011165)]
        public void GetRegionsList_RegionsListModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _scheduler.GetRegionsList(practiceCode);

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
        [TestCase(null)]
        [TestCase(60)]
        [TestCase(40)]
        [TestCase(140)]
        public void ConvertTimeLength_ConvertTimeLength_ReturnData(int time)
        {
            //Arrange
            //Act
            var result = _scheduler.ConvertTimeLength(time);

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
        [TestCase("03015487897")]
        [TestCase("")]
        public void GetHomePhoneInFormat_HomePhoneInFormat_ReturnData(string phone)
        {
            //Arrange
            //Act
            var result = _scheduler.getHomePhoneInFormat(phone);

            //Assert
            if (result != null && result== "Home: ")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("Haider", "")]
        [TestCase("", "FC")]
        [TestCase("Haider", "FC")]
        [TestCase(null, "" )]
        public void getPatientInFormat_SetPatientInFormat_ReturnData(string name, string fc)
        {
            //Arrange
            //Act
            var result = _scheduler.getPatientInFormat(name,fc);

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
        [TestCase("0301548789")]
        [TestCase("454")]
        [TestCase("")]
        public void GetNumberInFormat_SetNumberInFormat_ReturnData(string number)
        {
            //Arrange
            //Act
            var result = _scheduler.getNumberInFormat(number);

            //Assert
            if (result != null && result != "")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("654285225")]
        [TestCase("")]
        public void GetZIPInFormat_SetZIPInFormat_ReturnData(string zip)
        {
            //Arrange
            //Act
            var result = _scheduler.getZIPInFormat(zip);

            //Assert
            if (result != null && result != "")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("Fox")]
        [TestCase(null)]
        public void CheckNull_StringNullOrNot_ReturnData(string str)
        {
            //Arrange
            //Act
            var result = _scheduler.checkNull(str);

            //Assert
            if (result != null && result != "")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("101116354813049", "1011163")]
        [TestCase("", "1011163")]
        public void GetAlerts_AlertsModel_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _scheduler.GetAlerts(_userProfile, patientAccount);

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
        [TestCase("1011163")]
        [TestCase("1011165")]
        public void GetLocations_LocationsModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _scheduler.GetLocations(practiceCode);

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
        [TestCase(1011163, true, "04:43 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "04:44 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "04:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "05:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "06:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "02:00 AM", 5481855, 5485388, "2020-06-28")]
        [TestCase(1011163, false, "04:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "02:00 AM", 5481855, 5485388, "2020-06-28")]
        public void RescheduleAppoinment_UpdateModel_ReturnData(long practiceCode, bool isNew, string time, long providerId, long appointmentId, string appointmentDateStr)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _appointment.TIME_FROM = time;
            _appointment.TIME_TO = time;
            _appointment.PROVIDER_ID = providerId;
            _appointment.APPOINTMENT_ID = appointmentId;
            _appointment.APPOINTMENT_DATE_STR = appointmentDateStr;
            _appointment.PATIENT_ACCOUNT = 0;
            _appointment.IS_NEW = isNew;

            //Act
            var result = _scheduler.RescheduleAppoinment(_appointment, _userProfile);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, 54810864)]
        [TestCase(1011163, 54810863)]
        [TestCase(1012714, 54810864)]
        public void CancelAppoinment_AppoinmentModel_ReturnData(long practiceCode, long appointmentId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _appointment.APPOINTMENT_ID = appointmentId;

            //Act
            var result = _scheduler.CancelAppoinment(_appointment, _userProfile);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, true, "04:43 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "04:44 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "04:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "05:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "06:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "02:00 AM", 5481855, 5485388, "2020-06-28")]
        [TestCase(1011163, false, "04:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "02:00 AM", 5481855, 5485388, "2020-06-28")]
        public void EditblockAppoinment_UpdateModel_ReturnData(long practiceCode, bool isNew, string time, long providerId, long appointmentId, string appointmentDateStr)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _appointment.TIME_FROM = time;
            _appointment.TIME_TO = time;
            _appointment.PROVIDER_ID = providerId;
            _appointment.APPOINTMENT_ID = appointmentId;
            _appointment.APPOINTMENT_DATE_STR = appointmentDateStr;
            _appointment.PATIENT_ACCOUNT = 0;
            _appointment.IS_NEW = isNew;

            //Act
            var result = _scheduler.EditblockAppoinment(_appointment, _userProfile);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, 54810863)]
        [TestCase(1011163, 54810864)]
        [TestCase(1012714, 54810864)]
        public void DeleteBlockAppoinment_AppoinmentModel_ReturnData(long practiceCode, long appointmentId)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _appointment.APPOINTMENT_ID = appointmentId;

            //Act
            var result = _scheduler.DeleteBlockAppoinment(_appointment, _userProfile);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, true, "04:43 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "04:44 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "04:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "05:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "06:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, false, "02:00 AM", 5481855, 5485388, "2020-06-28")]
        [TestCase(1011163, false, "04:42 PM", 544100, 54810699, "2019-09-14")]
        [TestCase(1011163, true, "02:00 AM", 5481855, 5485388, "2020-06-28")]
        public void OnSaveAddBlock_AppointmentModel_ReturnData(long practiceCode, bool isNew, string time, long providerId, long appointmentId, string appointmentDateStr)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _appointment.TIME_FROM = time;
            _appointment.TIME_TO = time;
            _appointment.PROVIDER_ID = providerId;
            _appointment.APPOINTMENT_ID = appointmentId;
            _appointment.APPOINTMENT_DATE_STR = appointmentDateStr;
            _appointment.PATIENT_ACCOUNT = 0;
            _appointment.IS_NEW = isNew;

            //Act
            var result = _scheduler.OnSaveAddBlock(_appointment, _userProfile);

            //Assert
            if (result != 0)
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
            _scheduler = null;
            _userProfile = null;
            _appointmentSearchRequest = null;
            _appointment = null;
        }

    }
}
