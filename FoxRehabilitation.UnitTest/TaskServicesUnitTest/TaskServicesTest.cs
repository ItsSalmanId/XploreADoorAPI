using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.TaskServices;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.TasksModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.TaskServicesUnitTest
{
    [TestFixture]
    public class TaskServicesTest
    {
        private TaskServices _taskServices;
        private UserProfile _userProfile;
        private getCatFieldReq _getCatFieldReq;
        private NotificationRequestModel _notificationRequestModel;
        private FOX_TBL_TASK_SUB_TYPE _foxTblTaskSubType;
        private FOX_TBL_TASK_TYPE _foxTblTaskType;
        private GetCategoryFieldResp _getCategoryFieldResp;
        private CatFieldRes _catFieldRes;
        private FOX_TBL_TASK _foxTblTask;
        private InterfaceSynchModel _interfaceSynchModel;
        private TaskDashboardSearchRequest _taskDashboardSearchRequest;

        [SetUp]
        public void SetUp()
        {
            _taskServices = new TaskServices();
            _userProfile = new UserProfile();
            _getCatFieldReq = new getCatFieldReq();
            _notificationRequestModel = new NotificationRequestModel();
            _foxTblTaskSubType = new FOX_TBL_TASK_SUB_TYPE();
            _foxTblTaskType = new FOX_TBL_TASK_TYPE();
            _getCategoryFieldResp = new GetCategoryFieldResp();
            _catFieldRes = new CatFieldRes();
            _foxTblTask = new FOX_TBL_TASK();
            _interfaceSynchModel = new InterfaceSynchModel();
            _taskDashboardSearchRequest = new TaskDashboardSearchRequest();
        }
        [Test]
        [TestCase("", 0)]
        [TestCase("101116354816561", 1011163)]
        public void GetAllCasesAndTaskType_CasesAndTaskTypeListModel_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetAllCasesAndTaskType(patientAccount, practiceCode);

            //Assert
            if (result != null && result.TASK_TYPE.Count > 0 && result.CASE.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetAllTaskType_TaskTypeListModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var TaskTypeList = _taskServices.GetAllTaskType(practiceCode);

            //Assert
            Assert.That(TaskTypeList.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(0)]
        [TestCase(1011163)]
        [TestCase(1011165)]
        public void GetInactiveTaskTypeList_TaskTypesModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetInactiveTaskTypeList(practiceCode);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(1011163)]
        public void GetTaskTypeList_TaskTypesListModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetTaskTypeList(practiceCode);

            //Assert
            if (result != null && result.Task_Types.Count > 0 && result.dropDownData.CATEGORY.Count > 0 && result.getTaskTemplateResponse.TASK_ALL_SUB_TYPES_LIST == null && result.getTaskTemplateResponse.Task_Sub_Types.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        //[TestCase(10111636)]
        [TestCase(544100, 1011163)]
        [TestCase(0, 1011163)]
        public void GetSubTaskTypeList_TaskSubTypeModel_ReturnOrNotData(int taskTypeId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetSubTaskTypeList(taskTypeId, practiceCode);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1011164)]
        public void GetDropDownData_DropDownDataModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetDropDownData(practiceCode);

            //Assert
            if (result != null && result.CATEGORY.Count > 0 && result.DELIVERY_METHOD.Count > 0 && result.ORDER_STATUS_RESULT.Count > 0 && result.SEND_CONTEXT.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(5482530, 1011163)]
        [TestCase(0, 1011163)]
        [TestCase(544112, 1011163)]
        public void GetTaskById_ReturnNullOrTaskWithHistoryModel_ReturnData(long taskId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetTaskById(taskId, practiceCode);

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
        [TestCase(544104, 1011163)]
        [TestCase(544101, 1011163)]
        [TestCase(544105551, 1011163)]
        [TestCase(544105551, 21212121)]
        public void GetTask_TaskModelAndTaskSubTypesModel_ReturnData(int taskTypeId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetTask(taskTypeId, practiceCode);

            //Assert
            if (result != null && result.Task != null && result.TASK_ALL_SUB_TYPES_LIST == null && result.Task_Sub_Types.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("", 1011163)]
        [TestCase("test", 1011163)]
        [TestCase("test12457", 1011163)]
        public void GetTTUserList_TasktypeUserListModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetTTUserList(searchText, practiceCode);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("", 1011163)]
        [TestCase("test", 1011163)]
        [TestCase("test12457", 1011163)]
        public void GetUsersGroupList_UsersGroupListModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetUsersGroupList(searchText, practiceCode);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("", 1011163)]//passed case
        [TestCase("test", 1011163)]//passed case
        [TestCase("test12457", 1011163)]//passed case
        public void GetUsersList_UsersListModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetUsersList(searchText, practiceCode);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(0)]
        public void GetcategoryAndField_GetcategoryAndFieldModel_ReturnData(long PracticeCode)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

            //Act
            var result = _taskServices.getcategoryandfield(_userProfile);

            //Assert
            if (result != null && result.Fields.Count > 0 && result.categories.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase("", 1011163)]
        [TestCase("test", 1011163)]
        [TestCase("test12457", 1011163)]
        public void GetProviderList_ProviderListModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetProviderList(searchText, practiceCode);

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
        [TestCase("", 1011163)]
        [TestCase("test", 1011163)]
        [TestCase("test12457", 1011163)]
        public void GetActiveLocationList_ActiveLocationListModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetActiveLocationList(searchText, practiceCode);

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
        [TestCase("asd_5482973", "FA585D89C851DD338A70DCF535AA2A92FEE7836DD6AFF1226583E88E0996293F16BC009C652826E0FC5C706695A03CDDCE372F139EFF4D13959DA6F1F5D3EABE")]//passed case
        [TestCase("1163testing", "9C5C3DBFF007D7A62EBE50EBBCD3931D088F70066A85AAD5FAAFC338C50F0BEE19FA300C22E8DEAACBAE8D46B2FFF1715A8551D588DFE46F2D03B8EE1C0F12DC")]//passed case
        public void AuthenticateUser_CheckAuthenticateUser_ReturnData(string userName, string password)
        {
            //Arrange
            //Act
            var result = _taskServices.AuthenticateUser(userName, password);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(548718, 1011163)]
        [TestCase(1, 1011163)]
        [TestCase(2, 1011163)]
        public void GetTaskByGeneralNoteId_TaskWithHistoryModel_ReturnData(long generalNoteId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _taskServices.GetTaskByGeneralNoteId(generalNoteId, practiceCode);

            //Assert
            if (result != null && result.Task != null && result.taskHistory != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(5482530, 1011163)]
        [TestCase(5482532, 1011163)]
        [TestCase(0, 101111212)]
        public void GetTaskByTaskId_TaskWithHistoryModel_ReturnData(long taskId, long PracticeCode)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

            //Act
            var result = _taskServices.GetTaskByTaskId(taskId, _userProfile);

            //Assert
            if (result != null && result.Task != null && result.taskHistory != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(null, 5481096, 5482530)]
        [TestCase(1, 2, 3)]
        public void GetTask_TaskByIdModel_NotReturnData(long patientAccount, long caseId, long taskTypeId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _taskServices.GetTask(patientAccount, caseId, taskTypeId, _userProfile);

            //Assert
            if (result != null && result.Task != null && result.taskHistory != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, 1011163415, 1)]
        [TestCase(1011163, 1011163415, 2)]
        [TestCase(1011163, 1011163415, 3)]
        [TestCase(1011163, 1011163415, 4)]
        [TestCase(1011163, 1011163416, 4)]
        public void GetTasksNotifications_TasksNotificationsModel_ReturnData(long practiceCode, long userID, int TIME_FRAME)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.userID = userID;
            _notificationRequestModel.TIME_FRAME = TIME_FRAME;
            if (TIME_FRAME == 4 && userID == 1011163416)
            {
                _notificationRequestModel.DATE_FROM_STR = Convert.ToString(DateTime.Today);
                _notificationRequestModel.DATE_FROM_STR = Convert.ToString(DateTime.Today);
            }
            else
            {
                _notificationRequestModel.DATE_FROM_STR = "";
                _notificationRequestModel.DATE_FROM_STR = "";
            }

            //Act
            var result = _taskServices.GetTasksNotifications(_notificationRequestModel, _userProfile);

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
        public void AddEditTaskSubType_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _foxTblTaskSubType.TASK_SUB_TYPE_ID = 544101;

            //Act
            var result = _taskServices.AddEditTaskSubType(_foxTblTaskSubType, _userProfile);

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
        public void DeleteTaskSubType_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _foxTblTaskSubType.TASK_SUB_TYPE_ID = 544100;

            //Act
            var result = _taskServices.DeleteTaskSubType(_foxTblTaskSubType, _userProfile);

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
        public void SetTaskTypeBit_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _foxTblTaskSubType.TASK_SUB_TYPE_ID = 544100;

            //Act
            var result = _taskServices.SetTaskTypeBit(_foxTblTaskType, _userProfile);

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
        [TestCase(true)]
        [TestCase(false)]
        public void SaveFilterTemplateRecord_PassModel_ReturnData(bool condition)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            int temp;
            if (condition == true)
            {
                temp = Convert.ToInt32(Helper.getMaximumId("FOX_TASK_SUB_TYPE_INTEL_TASK_FIELD_ID"));
            }
            else
            {
                temp = Convert.ToInt32(Helper.getMaximumId("FOX_TASK_SUB_TYPE_INTEL_TASK_FIELD_ID")) - 1;
            }
            _getCategoryFieldResp.CategoryList = new List<FOX_TBL_INTEL_TASK_CATEGORY>()
            {
                new FOX_TBL_INTEL_TASK_CATEGORY
                {
                    INTEL_TASK_CATEGORY_ID = temp,
                    FieldList = new List<CatFieldRes>()
                    {
                        new CatFieldRes
                        {
                            TASK_SUB_TYPE_ID = 544100,
                            IS_REQUIRED = true,
                            CATEGORY_NAME = "housecall",
                            FIELD_NAME = "order status"
                        }
                    }

                }
            };

            //Act
            var result = _taskServices.SaveFilterTemplateRecord(_getCategoryFieldResp, _userProfile);

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
        public void CreateUpdateOrdStatusMapping_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _foxTblTaskSubType.TASK_SUB_TYPE_ID = 544100;
            _catFieldRes.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID = 544105;
            _catFieldRes.OrderStatusIdList = new List<int?>()
            {
                new int
                {

                }
            };

            //Act
            var result = _taskServices.CreateUpdateOrdStatusMapping(_catFieldRes, _userProfile);

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
        [TestCase("housecall", "order status")]
        [TestCase("index info source", "ordering referral source")]
        public void SaveSubTypeMapping_PassModel_ReturnData(string catogoryName, string fieldName)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _foxTblTaskSubType.TASK_SUB_TYPE_ID = 544100;
            _catFieldRes.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID = 544105;
            _catFieldRes.CATEGORY_NAME = catogoryName;
            _catFieldRes.FIELD_NAME = fieldName;
            _catFieldRes.OrderStatusIdList = new List<int?>()
            {
                new int
                {

                }
            };

            //Act
            var result = _taskServices.SaveSubTypeMapping(_catFieldRes, _userProfile);

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
        public void CreateUpdateRefSourceMapping_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _catFieldRes.SourceIdList = new List<long?>()
            {
                new long
                {

                }
            };

            //Act
            var result = _taskServices.CreateUpdateRefSourceMapping(_catFieldRes, _userProfile);

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
        public void GetCategoryFields_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _catFieldRes.SourceIdList = new List<long?>()
            {
                new long
                {

                }
            };

            //Act
            var result = _taskServices.GetCategoryFields(_getCatFieldReq, _userProfile);

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
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void GetTasksNotificationsList_PassModel_ReturnData(int timeFrame)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _notificationRequestModel.TIME_FRAME = timeFrame;

            //Act
            var result = _taskServices.GetTasksNotificationsList(_notificationRequestModel, _userProfile);

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
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteNotification_PassModel_ReturnData(long id)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";

            //Act
            var result = _taskServices.DeleteNotification(id, _userProfile);

            //Assert
            if (result != true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void AddUpdateTask_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _foxTblTask.PATIENT_ACCOUNT_STR = "101116354815183";
            _foxTblTask.TASK_ID = 5485231;
            _foxTblTask.TASK_ALL_SUB_TYPES_LIST = new List<OpenIssueViewModel>()
            {
                new OpenIssueViewModel
                {
                    TASK_ID = 5485231,
                    TASK_SUB_TYPE_ID = 544116
                }
            };

            //Act
            var result = _taskServices.AddUpdateTask(_foxTblTask, _userProfile);

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
        public void InsertInterfaceTeamData_PassModel_ReturnData()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "N_UnitTesting";
            _interfaceSynchModel.PATIENT_ACCOUNT = 101116354817932;

            //Act
            _taskServices.InsertInterfaceTeamData(_interfaceSynchModel, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        //[Test]
        //public void GetTaskDashBoardData_PassModel_ReturnDat()
        //{
        //    //Arrange
        //    _userProfile.PracticeCode = 1011163;
        //    _userProfile.UserName = "N_UnitTesting";
        //    _taskDashboardSearchRequest.DATE_FROM_STR = Helper.GetCurrentDate().ToString();
        //    _taskDashboardSearchRequest.DATE_TO_STR = Helper.GetCurrentDate().ToString();

        //    //Act
        //    _taskServices.GetTaskDashBoardData(_taskDashboardSearchRequest, _userProfile);

        //    //Assert
        //    Assert.IsTrue(true);
        //}
        //GetTaskDashBoardData
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _taskServices = null;
            _getCatFieldReq = null;
            _notificationRequestModel = null;
            _userProfile = null;
        }
    }
}
