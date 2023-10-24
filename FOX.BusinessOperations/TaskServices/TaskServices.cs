//System Namespaces.
using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

//Portal Namespaces.
using FOX.DataModels.Context;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.TasksModel;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.GroupsModel;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.Security;
using System.Web;
using FOX.BusinessOperations.CommonServices;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Text;
using System.Collections;

namespace FOX.BusinessOperations.TaskServices
{
    public class TaskServices : ITaskServices
    {
        private readonly GenericRepository<FOX_VW_CASE> _vwCaseRepository;
        private readonly DbContextTasks _TaskContext = new DbContextTasks();
        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly DbContextSettings _SettingsContext = new DbContextSettings();
        private readonly GenericRepository<FOX_TBL_TASK> _TaskRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TYPE> _taskTypeRepository;
        private readonly GenericRepository<FOX_TBL_TASK_SUB_TYPE> _taskSubTypeRepository;
        private readonly GenericRepository<FOX_TBL_INTEL_TASK_CATEGORY> _categoryRepository;
        private readonly GenericRepository<FOX_TBL_INTEL_TASK_FIELD> _fieldRepository;
        private readonly GenericRepository<FOX_TBL_SEND_CONTEXT> _sendContextRepository;
        private readonly GenericRepository<FOX_TBL_DELIVERY_METHOD> _deliveryMethodRepository;
        private readonly GenericRepository<FOX_TBL_CATEGORY> _categoryepository;
        private readonly GenericRepository<FOX_TBL_TASK_APPLICATION_USER> _taskApplicationUserRepository;
        private readonly GenericRepository<FOX_TBL_NOTES> _NotesRepository;
        private readonly GenericRepository<FOX_TBL_NOTES_TYPE> _NotesTypeRepository;
        private readonly GenericRepository<User> _UserRepository;
        private readonly GenericRepository<ReferralSource> _referralSourceRepository;
        private readonly GenericRepository<ActiveLocation> _activeLocationRepository;
        private readonly GenericRepository<FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD> _TaskSubTypeIntelTaskFieldRepository;
        private readonly GenericRepository<FOX_TBL_ORDER_STATUS> _OrderStatusRepository;
        private readonly GenericRepository<FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING> _TaskRefMapRepository;
        private readonly GenericRepository<TaskLog> _taskLogRepository;
        private readonly GenericRepository<User> _userRepository;
        private readonly GenericRepository<GROUP> _groupRepository;
        private readonly GenericRepository<USERS_GROUP> _groupUserRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TASK_SUB_TYPE> _TaskTaskSubTypeRepository;
        private readonly GenericRepository<InterfaceSynchModel> __InterfaceSynchModelRepository;
        private CommonServices.CommonServices _commonService;
        private PatientMaintenanceService.PatientMaintenanceService _patientMaintenanceService;


        public TaskServices()
        {
            _vwCaseRepository = new GenericRepository<FOX_VW_CASE>(_CaseContext);
            _categoryRepository = new GenericRepository<FOX_TBL_INTEL_TASK_CATEGORY>(_TaskContext);
            _fieldRepository = new GenericRepository<FOX_TBL_INTEL_TASK_FIELD>(_TaskContext);
            _TaskRepository = new GenericRepository<FOX_TBL_TASK>(_TaskContext);
            _taskTypeRepository = new GenericRepository<FOX_TBL_TASK_TYPE>(_TaskContext);
            _taskSubTypeRepository = new GenericRepository<FOX_TBL_TASK_SUB_TYPE>(_TaskContext);
            _sendContextRepository = new GenericRepository<FOX_TBL_SEND_CONTEXT>(_TaskContext);
            _deliveryMethodRepository = new GenericRepository<FOX_TBL_DELIVERY_METHOD>(_TaskContext);
            _categoryepository = new GenericRepository<FOX_TBL_CATEGORY>(_TaskContext);
            _taskApplicationUserRepository = new GenericRepository<FOX_TBL_TASK_APPLICATION_USER>(_TaskContext);
            _NotesRepository = new GenericRepository<FOX_TBL_NOTES>(_CaseContext);
            _NotesTypeRepository = new GenericRepository<FOX_TBL_NOTES_TYPE>(_CaseContext);
            _UserRepository = new GenericRepository<User>(security);
            _TaskSubTypeIntelTaskFieldRepository = new GenericRepository<FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD>(_TaskContext);
            _referralSourceRepository = new GenericRepository<ReferralSource>(_TaskContext);
            _activeLocationRepository = new GenericRepository<ActiveLocation>(_TaskContext);
            _OrderStatusRepository = new GenericRepository<FOX_TBL_ORDER_STATUS>(_CaseContext);
            _TaskRefMapRepository = new GenericRepository<FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING>(_TaskContext);
            _taskLogRepository = new GenericRepository<TaskLog>(_TaskContext);
            _userRepository = new GenericRepository<User>(security);
            _groupRepository = new GenericRepository<GROUP>(_SettingsContext);
            _groupUserRepository = new GenericRepository<USERS_GROUP>(_SettingsContext);
            _TaskTaskSubTypeRepository = new GenericRepository<FOX_TBL_TASK_TASK_SUB_TYPE>(_TaskContext);
            __InterfaceSynchModelRepository = new GenericRepository<InterfaceSynchModel>(_CaseContext);
            _commonService = new CommonServices.CommonServices();
            _patientMaintenanceService = new PatientMaintenanceService.PatientMaintenanceService();

        }
        
        public CaseTaskTypeList GetAllCasesAndTaskType(string patient_Account, long practiceCode)
        {
            CaseTaskTypeList caseTaskTypeList = new CaseTaskTypeList();
            long? _patient_Account = null;
            if (!string.IsNullOrEmpty(patient_Account) && !string.Equals(patient_Account, "null", StringComparison.OrdinalIgnoreCase))
            {
                _patient_Account = Convert.ToInt64(patient_Account);
            }
            if(_patient_Account != null)
            {
                caseTaskTypeList.CASE = _vwCaseRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode && t.PATIENT_ACCOUNT == _patient_Account);
                caseTaskTypeList.PatientDetail = _patientMaintenanceService.GetPatientByAccountNo(_patient_Account.Value);
            }
            caseTaskTypeList.TASK_TYPE = _taskTypeRepository.GetMany(x => !x.DELETED && (x.IS_ACTIVE ?? true) && x.PRACTICE_CODE == practiceCode).OrderBy(x => x.NAME).ToList();
            return caseTaskTypeList;
        }

        public List<FOX_TBL_TASK_TYPE> GetAllTaskType( long practiceCode)
        {
            List<FOX_TBL_TASK_TYPE> TaskTypeList = new List<FOX_TBL_TASK_TYPE>();

            TaskTypeList = _taskTypeRepository.GetMany(x => !x.DELETED && (x.IS_ACTIVE ?? true) && x.PRACTICE_CODE == practiceCode).OrderBy(x => x.NAME).ToList();
            return TaskTypeList;
        }

        public FOX_TBL_TASK AddUpdateTask(FOX_TBL_TASK task, UserProfile profile)
        {

            if (!string.IsNullOrEmpty(task.PATIENT_ACCOUNT_STR))
            {
                task.PATIENT_ACCOUNT = Convert.ToInt64(task.PATIENT_ACCOUNT_STR);
            }
            
            #region Check if hold/pending task for this case already exist

            //if ((task == null || task.TASK_ID == 0) && (task.CASE_ID != null || task.CASE_ID != 0))
            //{
            //    var _dbHolPendingAssgnTask = _TaskRepository.GetFirst(row => !row.DELETED && row.PRACTICE_CODE == profile.PracticeCode && row.PATIENT_ACCOUNT == task.PATIENT_ACCOUNT && row.CASE_ID == task.CASE_ID && row.TASK_TYPE_ID == task.TASK_TYPE_ID);
            //    if(_dbHolPendingAssgnTask != null)
            //    {
            //        task.dbChangeMsg = "PendingHoldTaskAlreadyExist";
            //        return task;
            //    }
            //}

            #endregion


            InterfaceSynchModel InterfaceSynch = new InterfaceSynchModel();

            if (task != null && profile != null)
            {

                #region Is Task Add/Update - Region

                bool isEdit = true;
                FOX_TBL_TASK dbTask = _TaskRepository.GetFirst(t => t.PRACTICE_CODE == profile.PracticeCode && t.TASK_ID == task.TASK_ID);

                if (dbTask == null)
                {
                    dbTask = new FOX_TBL_TASK();
                    InterfaceSynch.TASK_ID = dbTask.TASK_ID = Helper.getMaximumId("FOX_TASK_ID");
                    dbTask.PRACTICE_CODE = profile.PracticeCode;
                    InterfaceSynch.PATIENT_ACCOUNT = dbTask.PATIENT_ACCOUNT = task.PATIENT_ACCOUNT;
                    InterfaceSynch.CASE_ID = dbTask.CASE_ID;
                    dbTask.IS_COMPLETED_INT = 0;/*0: Initiated 1:Sender Completed 2:Final Route Completed*/
                    dbTask.CREATED_BY = dbTask.MODIFIED_BY = profile.UserName;
                    dbTask.CREATED_DATE = dbTask.MODIFIED_DATE = Helper.GetCurrentDate();
                    dbTask.IS_TEMPLATE = task.IS_TEMPLATE;
                    dbTask.GENERAL_NOTE_ID = task.GENERAL_NOTE_ID;
                    dbTask.ATTACHMENT_PATH = task.ATTACHMENT_PATH;
                    dbTask.ATTACHMENT_TITLE = task.ATTACHMENT_TITLE;
                    isEdit = false;
                }
                else
                {
                    InterfaceSynch.CASE_ID = task.CASE_ID;
                    InterfaceSynch.TASK_ID = task.TASK_ID;
                    InterfaceSynch.PATIENT_ACCOUNT = task.PATIENT_ACCOUNT;
                    dbTask.MODIFIED_BY = profile.UserName;
                    dbTask.MODIFIED_DATE = Helper.GetCurrentDate();
                }

                #endregion

                AddTaskLogs(dbTask, task, profile, isEdit);

                #region Copy Task Data - Region

                if ((dbTask.DUE_DATE_TIME.HasValue && task.DUE_DATE_TIME == null) || (task.DUE_DATE_TIME.HasValue && dbTask.DUE_DATE_TIME == null))
                {
                    task.Is_Change = true;
                }
                if (task.DUE_DATE_TIME != null)
                {
                    if (dbTask.DUE_DATE_TIME.HasValue && dbTask.DUE_DATE_TIME.Value.Date != task.DUE_DATE_TIME.Value.Date)
                    {
                        task.Is_Change = true;
                    }
                }
                dbTask.TASK_TYPE_ID = task.TASK_TYPE_ID;
                dbTask.SEND_TO_ID = task.SEND_TO_ID;
                dbTask.FINAL_ROUTE_ID = task.FINAL_ROUTE_ID;
                dbTask.PRIORITY = task.PRIORITY;
                dbTask.DUE_DATE_TIME = task.DUE_DATE_TIME;
                dbTask.CATEGORY_ID = task.CATEGORY_ID;
                dbTask.IS_REQ_SIGNOFF = task.IS_REQ_SIGNOFF;
                dbTask.IS_SENDING_ROUTE_DETAILS = task.IS_SENDING_ROUTE_DETAILS;
                dbTask.SEND_CONTEXT_ID = task.SEND_CONTEXT_ID;
                dbTask.CONTEXT_INFO = task.CONTEXT_INFO;
                dbTask.DEVELIVERY_ID = task.DEVELIVERY_ID;
                dbTask.DESTINATIONS = task.DESTINATIONS;
                dbTask.LOC_ID = task.LOC_ID;
                dbTask.PROVIDER_ID = task.PROVIDER_ID;
                dbTask.IS_SEND_EMAIL_AUTO = task.IS_SEND_EMAIL_AUTO;
                dbTask.DELETED = task.DELETED;
                dbTask.IS_TEMPLATE = task.IS_TEMPLATE;
                InterfaceSynch.CASE_ID = dbTask.CASE_ID = task.CASE_ID;
                dbTask.ATTACHMENT_PATH = task.ATTACHMENT_PATH;
                dbTask.ATTACHMENT_TITLE = task.ATTACHMENT_TITLE;
                AddEditTask_TaskSubTypes(dbTask.TASK_ID, task.TASK_ALL_SUB_TYPES_LIST, profile);
                dbTask.IS_SEND_TO_USER = task.IS_SEND_TO_USER;
                dbTask.IS_FINAL_ROUTE_USER = task.IS_FINAL_ROUTE_USER;
                dbTask.IS_FINALROUTE_MARK_COMPLETE = task.IS_FINALROUTE_MARK_COMPLETE;
                dbTask.IS_SENDTO_MARK_COMPLETE = task.IS_SENDTO_MARK_COMPLETE;
               
                if (dbTask.DUE_DATE_TIME == null)
                {
                    dbTask.DUE_DATE_TIME = task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                }
                
                if ((task.TASK_APPLICATION_USER?.Count() ?? 0) > 0)
                {
                    AddUpdateTaskApplicationUser(task.TASK_APPLICATION_USER, dbTask.TASK_ID, profile);
                }
                dbTask.COMMENTS = task.COMMENTS;

                #endregion

                #region Add Update Task - Region

                if (isEdit)
                {
                    // if (task.Is_Change)
                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(InterfaceSynch, profile);
                    dbTask.DELETED = false;
                    dbTask.dbChangeMsg = "TaskUpdateSuccessed";
                    _TaskRepository.Update(dbTask);
                }
                else
                {
                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(InterfaceSynch, profile);
                    dbTask.dbChangeMsg = "TaskInsertSuccessed";
                    _TaskRepository.Insert(dbTask);
                }
                _TaskRepository.Save();

                #endregion

                _commonService.AddUpdateNotes(new FOX_TBL_NOTES() { NOTES = dbTask.COMMENTS, TASK_ID = dbTask.TASK_ID }, profile);

                dbTask.FinalRouteGroupUsers = this._groupUserRepository.GetMany(x => x.GROUP_ID == task.FINAL_ROUTE_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                dbTask.SendToGroupUsers = this._groupUserRepository.GetMany(x => x.GROUP_ID == task.SEND_TO_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);

                dbTask.CATEGORY_CODE = _taskTypeRepository.GetByID(dbTask?.TASK_TYPE_ID)?.CATEGORY_CODE;
                return dbTask;
            }
            return null;
        }

        private void AddTaskLogs(FOX_TBL_TASK dbTask, FOX_TBL_TASK task, UserProfile profile, bool isEdit)
        {
            List<TaskLog> taskLoglist = new List<TaskLog>();

            #region Set Task Log Data - Region

            if (isEdit)
            {
                if (task.TASK_TYPE_ID != null && task.TASK_TYPE_ID != dbTask.TASK_TYPE_ID)
                {
                    var tasktype = _taskTypeRepository.GetFirst(x => !x.DELETED && x.TASK_TYPE_ID == task.TASK_TYPE_ID);
                    if(tasktype != null)
                    {
                        taskLoglist.Add(new TaskLog() { ACTION = "Task type changed", ACTION_DETAIL = "Task type updated: " + tasktype.DESCRIPTION });
                    }
                }
                if (task.PROVIDER_ID != null && task.PROVIDER_ID != dbTask.PROVIDER_ID)
                {
                    var _provider = _commonService.GetProvider(task.PROVIDER_ID.Value, profile);
                    taskLoglist.Add(new TaskLog() { ACTION = "Provider changed", ACTION_DETAIL = "Provider updated: " + _provider.LAST_NAME + ", " + _provider.FIRST_NAME });
                }
                if (task.LOC_ID != null && task.LOC_ID != dbTask.LOC_ID)
                {
                    var loc = _activeLocationRepository.GetFirst(x => !x.DELETED && x.LOC_ID == task.LOC_ID);
                    taskLoglist.Add(new TaskLog() { ACTION = "Location changed", ACTION_DETAIL = "Location updated: " + loc.NAME });
                }
                if (task.SEND_TO_ID != null && task.SEND_TO_ID != dbTask.SEND_TO_ID)
                {
                    if (task.IS_SEND_TO_USER)
                    {
                        var sendto = _userRepository.GetFirst(x => !x.DELETED && x.USER_ID == task.SEND_TO_ID);
                        taskLoglist.Add(new TaskLog() { ACTION = "Sent to changed", ACTION_DETAIL = "Sent to updated: " + sendto.LAST_NAME + ", " + sendto.FIRST_NAME });
                    }
                    else
                    {
                        var sendto = _groupRepository.GetFirst(x => !x.DELETED && x.GROUP_ID == task.SEND_TO_ID);
                        taskLoglist.Add(new TaskLog() { ACTION = "sent to changed", ACTION_DETAIL = "Sent to updated: " + sendto.GROUP_NAME });
                    }
                }
                if (task.FINAL_ROUTE_ID != null && task.FINAL_ROUTE_ID != dbTask.FINAL_ROUTE_ID)
                {
                    if (task.IS_FINAL_ROUTE_USER)
                    {
                        var froute = _userRepository.GetFirst(x => !x.DELETED && x.USER_ID == task.FINAL_ROUTE_ID);
                        taskLoglist.Add(new TaskLog() { ACTION = "Final Route Changed", ACTION_DETAIL = "Final route updated: " + froute?.LAST_NAME + ", " + froute?.FIRST_NAME });
                    }
                    else
                    {
                        var froute = _groupRepository.GetFirst(x => !x.DELETED && x.GROUP_ID == task.FINAL_ROUTE_ID);
                        taskLoglist.Add(new TaskLog() { ACTION = "Final Route Changed", ACTION_DETAIL = "Final route updated: " + froute?.GROUP_NAME });
                    }
                }
                if (!string.IsNullOrEmpty(task.PRIORITY) && task.PRIORITY != dbTask.PRIORITY)
                {
                    taskLoglist.Add(new TaskLog() { ACTION = "Priority changed", ACTION_DETAIL = "Priority updated: " + task.PRIORITY });
                }
                if (!string.IsNullOrEmpty(task.DUE_DATE_TIME_str) && Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str)?.ToString("MM/dd/yyyy") != dbTask.DUE_DATE_TIME?.ToString("MM/dd/yyyy"))
                {
                    taskLoglist.Add(new TaskLog() { ACTION = "Due Date changed", ACTION_DETAIL = "Due date updated: " + Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str)?.ToString("MM/dd/yyyy") });
                }
                if (!string.IsNullOrEmpty(task.ATTACHMENT_PATH) && task.ATTACHMENT_TITLE != dbTask.ATTACHMENT_TITLE)
                {
                    taskLoglist.Add(new TaskLog() { ACTION = "Attachment changed", ACTION_DETAIL = "Attachment updated: " + task.ATTACHMENT_TITLE });
                }
                //task mark comalpete.
                if (dbTask.IS_SENDTO_MARK_COMPLETE != true && dbTask.IS_FINALROUTE_MARK_COMPLETE != true)
                {
                    if(task.IS_SENDTO_MARK_COMPLETE == true && task.IS_FINALROUTE_MARK_COMPLETE != true)
                    {
                        if(task.IS_SEND_TO_USER == true)
                        {
                            var _sendToUser = _userRepository.GetFirst(x => !x.DELETED && x.USER_ID == task.SEND_TO_ID);
                            taskLoglist.Add(new TaskLog() { ACTION = "Task routed to", ACTION_DETAIL = "Task routed to : " + Helper.ChangeStringToTitleCase(_sendToUser.LAST_NAME) + ", " + Helper.ChangeStringToTitleCase(_sendToUser.FIRST_NAME) });
                        }
                        else
                        {
                            var _sendToGroup = _groupRepository.GetFirst(x => !x.DELETED && x.GROUP_ID == task.SEND_TO_ID);
                            taskLoglist.Add(new TaskLog() { ACTION = "Task routed to", ACTION_DETAIL = "Task routed to: " + Helper.ChangeStringToTitleCase(_sendToGroup.GROUP_NAME) });
                        }
                    }
                    else if (task.IS_FINALROUTE_MARK_COMPLETE == true)
                    {
                        taskLoglist.Add(new TaskLog() { ACTION = "Task marked as", ACTION_DETAIL = "Task marked as ‘Completed’" });
                    }
                }
                else if (dbTask.IS_SENDTO_MARK_COMPLETE == true && dbTask.IS_FINALROUTE_MARK_COMPLETE == true)
                {
                    if (task.IS_SENDTO_MARK_COMPLETE != true && task.IS_FINALROUTE_MARK_COMPLETE != true)
                    {
                        taskLoglist.Add(new TaskLog() { ACTION = "Task reopened", ACTION_DETAIL = "Task ‘Reopened’" });
                    }
                }
                else if (dbTask.IS_SENDTO_MARK_COMPLETE == true && dbTask.IS_FINALROUTE_MARK_COMPLETE != true)
                {
                    if (task.IS_SENDTO_MARK_COMPLETE != true && task.IS_FINALROUTE_MARK_COMPLETE != true)
                    {
                        taskLoglist.Add(new TaskLog() { ACTION = "Task reopened", ACTION_DETAIL = "Task ‘Reopened’" });
                    }
                }
                // hold pending change
                //if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_TYPE.ToLower() == "call patient") != null)
                //{
                //    if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_TYPE.ToLower() == "call patient").TASK_TYPE_ID == task.TASK_TYPE_ID)
                //    {
                //        var practice_Code = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                //        var task_Id = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_ID };
                //        var task_Type_Id = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_TYPE_ID };
                //        var taskAllSubTypesList = SpRepository<OpenIssueViewModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_ALL_SUB_TYPE_LIST @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID", practice_Code, task_Id, task_Type_Id);



                //        dbTask.TASK_ALL_SUB_TYPES_LIST = taskAllSubTypesList;

                //        if (task.TASK_ALL_SUB_TYPES_LIST.Exists(e => e.TASK_TYPE.ToLower() == "call patient"))
                //        {
                //            if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "pending assignment").IS_CHECKED
                //            &&
                //            task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "pending assignment").IS_CHECKED != dbTask.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "pending assignment").IS_CHECKED)
                //            {
                //                taskLoglist.Add(new TaskLog() { ACTION = "Pending Assignment", ACTION_DETAIL = "This is the task for pending assignment " });
                //            }
                //            else if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "pending welcome call").IS_CHECKED
                //                &&
                //                task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "pending welcome call").IS_CHECKED != dbTask.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "pending welcome call").IS_CHECKED)
                //            {
                //                taskLoglist.Add(new TaskLog() { ACTION = "pending welcome call", ACTION_DETAIL = "This is the task for pending welcome call " });
                //            }
                //            else if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "new hire pending assignment").IS_CHECKED
                //                &&
                //                task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "new hire pending assignment").IS_CHECKED != dbTask.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "new hire pending assignment").IS_CHECKED)
                //            {
                //                taskLoglist.Add(new TaskLog() { ACTION = "new hire pending assignment", ACTION_DETAIL = "This is the task for pending new hire orientation" });
                //            }
                //        }

                //    }
                //}
            }
            else
            {
                taskLoglist.Add(new TaskLog() { ACTION = "Task created", ACTION_DETAIL = "Task created by: " + Helper.ChangeStringToTitleCase(profile.LastName) + ", " + Helper.ChangeStringToTitleCase(profile.FirstName) });
                if (task.PATIENT_ACCOUNT != null)
                {
                    var _patient = _patientMaintenanceService.GetPatientByAccountNo(task.PATIENT_ACCOUNT.Value);
                    if (_patient != null)
                    {
                        taskLoglist.Add(new TaskLog() { ACTION = "Patient marked", ACTION_DETAIL = "Patient marked: " + Helper.ChangeStringToTitleCase(_patient.LastName) + ", " + Helper.ChangeStringToTitleCase(_patient.FirstName) + "(Account # " + _patient.Patient_Account.ToString() + ")" });
                    }
                }
                if (task.CASE_ID != null)
                {
                    var _case = _vwCaseRepository.GetFirst(x=>x.CASE_ID==task.CASE_ID);
                    taskLoglist.Add(new TaskLog() { ACTION = "Case marked", ACTION_DETAIL = "Case marked: " + _case.CASE_NO });
                }
                if (task.TASK_TYPE_ID != null)
                {
                    var _taskType = _taskTypeRepository.GetFirst(row => row.PRACTICE_CODE == profile.PracticeCode && row.TASK_TYPE_ID == task.TASK_TYPE_ID);
                    taskLoglist.Add(new TaskLog() { ACTION = "Task Type", ACTION_DETAIL = "Task type: " + Helper.ChangeStringToTitleCase(_taskType.NAME) });
                }
                if (task.PROVIDER_ID != null && task.PROVIDER_ID != 0)
                {
                    var _provider = _commonService.GetProvider(task.PROVIDER_ID.Value, profile);
                    taskLoglist.Add(new TaskLog() { ACTION = "Provider", ACTION_DETAIL = "Provider: " + Helper.ChangeStringToTitleCase(_provider.LAST_NAME) + ", " + Helper.ChangeStringToTitleCase(_provider.FIRST_NAME) });
                }
                if (task.LOC_ID != null && task.LOC_ID != 0)
                {
                    var _location = _activeLocationRepository.GetFirst(x => !x.DELETED && x.LOC_ID == task.LOC_ID);
                    taskLoglist.Add(new TaskLog() { ACTION = "Location", ACTION_DETAIL = "Location: " + Helper.ChangeStringToTitleCase(_location.NAME) });
                }
                if (task.SEND_TO_ID != null)
                {
                    if (task.IS_SEND_TO_USER)
                    {
                        var sendTo = _userRepository.GetFirst(x => !x.DELETED && x.USER_ID == task.SEND_TO_ID);
                        taskLoglist.Add(new TaskLog() { ACTION = "Sent To: ", ACTION_DETAIL = "Sent to: " + Helper.ChangeStringToTitleCase(sendTo.LAST_NAME) + ", " + Helper.ChangeStringToTitleCase(sendTo.FIRST_NAME) });
                    }
                    else
                    {
                        var sendTo = _groupRepository.GetFirst(x => !x.DELETED && x.GROUP_ID == task.SEND_TO_ID);
                        taskLoglist.Add(new TaskLog() { ACTION = "Sent To: ", ACTION_DETAIL = "Sent to: " + Helper.ChangeStringToTitleCase(sendTo.GROUP_NAME) });
                    }
                }
                if (task.FINAL_ROUTE_ID != null)
                {
                    if (task.IS_FINAL_ROUTE_USER)
                    {
                        var finalRoute = _userRepository.GetFirst(x => !x.DELETED && x.USER_ID == task.FINAL_ROUTE_ID);
                        taskLoglist.Add(new TaskLog() { ACTION = "Final Route: ", ACTION_DETAIL = "Final route: " + Helper.ChangeStringToTitleCase(finalRoute.LAST_NAME) + ", " + Helper.ChangeStringToTitleCase(finalRoute.FIRST_NAME) });
                    }
                    else
                    {
                        var finalRoute = _groupRepository.GetFirst(x => !x.DELETED && x.GROUP_ID == task.FINAL_ROUTE_ID);
                        taskLoglist.Add(new TaskLog() { ACTION = "Final Route: ", ACTION_DETAIL = "Final route: " + finalRoute.GROUP_NAME });
                    }
                }
                if (!string.IsNullOrEmpty(task.PRIORITY))
                {
                    taskLoglist.Add(new TaskLog() { ACTION = "Priority", ACTION_DETAIL = "Priority: " + Helper.ChangeStringToTitleCase(task.PRIORITY) });
                }
                if (!string.IsNullOrEmpty(task.DUE_DATE_TIME_str))
                {
                    taskLoglist.Add(new TaskLog() { ACTION = "Due Date", ACTION_DETAIL = "Due date: " + Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str)?.ToString("MM/dd/yyyy") });
                }
                if (!string.IsNullOrEmpty(task.ATTACHMENT_PATH))
                {
                    taskLoglist.Add(new TaskLog() { ACTION = "Attachment", ACTION_DETAIL = "Attachment: " + task.ATTACHMENT_TITLE });
                }
                // hold pending change
                //if (task.TASK_ALL_SUB_TYPES_LIST.Exists(e => e.TASK_TYPE.ToLower() == "call patient"))
                //{
                //    if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_TYPE.ToLower() == "call patient").TASK_TYPE_ID == task.TASK_TYPE_ID)
                //    {
                //        if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "pending assignment").IS_CHECKED)
                //        {
                //            taskLoglist.Add(new TaskLog() { ACTION = "Pending Assignment", ACTION_DETAIL = "This is the task for pending assignment " });
                //        }
                //        else if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "pending welcome call").IS_CHECKED)
                //        {
                //            taskLoglist.Add(new TaskLog() { ACTION = "pending welcome call", ACTION_DETAIL = "This is the task for pending welcome call " });
                //        }
                //        else if (task.TASK_ALL_SUB_TYPES_LIST.Find(e => e.TASK_SUB_TYPE.ToLower() == "new hire pending assignment").IS_CHECKED)
                //        {
                //            taskLoglist.Add(new TaskLog() { ACTION = "new hire pending assignment", ACTION_DETAIL = "This is the task for pending new hire orientation" });
                //        }

                //    }
                //}
            }

            if (taskLoglist.Count() > 0)
            {
                taskLoglist.ForEach(row =>
                {
                    row.TASK_LOG_ID = Helper.getMaximumId("FOX_TASK_LOG_ID");
                    row.PRACTICE_CODE = profile.PracticeCode;
                    row.TASK_ID = dbTask.TASK_ID;
                    row.CREATED_BY = row.MODIFIED_BY = profile.UserName;
                    row.CREATED_DATE = row.MODIFIED_DATE = Helper.GetCurrentDate();
                });
            }

            #endregion

            try
            {
                foreach (var taskLog in taskLoglist)
                {
                    _taskLogRepository.Insert(taskLog);
                    _taskLogRepository.Save();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public FOX_TBL_TASK_SUB_TYPE AddEditTaskSubType(FOX_TBL_TASK_SUB_TYPE model, UserProfile profile)
        {
            try
            {
                if (model != null && profile != null)
                {
                    var taskSubType = _taskSubTypeRepository
                                .Get(
                                    t => !t.DELETED
                                    && t.PRACTICE_CODE == profile.PracticeCode
                                    && t.TASK_SUB_TYPE_ID == model.TASK_SUB_TYPE_ID
                                );
                    bool IsEdit = false;

                    if (taskSubType == null)
                    {
                        taskSubType = new FOX_TBL_TASK_SUB_TYPE();
                        taskSubType.TASK_SUB_TYPE_ID = (int)Helper.getMaximumId("TASK_SUB_TYPE_ID");
                        taskSubType.PRACTICE_CODE = profile.PracticeCode;
                        taskSubType.CREATED_BY = taskSubType.MODIFIED_BY = profile.UserName;
                        taskSubType.CREATED_DATE = taskSubType.MODIFIED_DATE = Helper.GetCurrentDate();
                        taskSubType.IS_EDITABLE = true;
                        IsEdit = false;
                    }
                    else
                    {
                        taskSubType.MODIFIED_BY = profile.UserName;
                        taskSubType.MODIFIED_DATE = Helper.GetCurrentDate();
                        IsEdit = true;
                    }
                    taskSubType.TASK_TYPE_ID = model.TASK_TYPE_ID;
                    taskSubType.NAME = model.NAME;
                    taskSubType.DESCRIPTION = model.DESCRIPTION;
                    taskSubType.DELETED = false;

                    if (!IsEdit)
                    {
                        _taskSubTypeRepository.Insert(taskSubType);
                    }
                    else
                    {
                        _taskSubTypeRepository.Update(taskSubType);
                    }
                    _taskSubTypeRepository.Save();
                    return taskSubType;
                }
                return null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public ResponseModel DeleteTaskSubType(FOX_TBL_TASK_SUB_TYPE model, UserProfile profile)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (model != null && profile != null)
                {
                    var taskSubType = _taskSubTypeRepository
                                .Get(
                                    t => !t.DELETED
                                    && t.PRACTICE_CODE == profile.PracticeCode
                                    && t.TASK_SUB_TYPE_ID == model.TASK_SUB_TYPE_ID
                                );

                    if (taskSubType != null)
                    {
                        taskSubType.MODIFIED_BY = profile.UserName;
                        taskSubType.MODIFIED_DATE = Helper.GetCurrentDate();

                        taskSubType.DELETED = true;

                        _taskSubTypeRepository.Update(taskSubType);
                        _taskSubTypeRepository.Save();

                        responseModel.Message = "FOX_TBL_TASK_SUB_TYPE deleted successfully.";
                    }
                    else
                    {
                        responseModel.Message = "FOX_TBL_TASK_SUB_TYPE not found";
                    }
                    responseModel.Success = true;
                    responseModel.ErrorMessage = "";

                    return responseModel;
                }
                return null;
            }
            catch (Exception exception)
            {
                responseModel.Message = exception.Message;
                responseModel.Success = true;
                responseModel.ErrorMessage = exception.ToString();
                return responseModel;
            }
        }
        //<summary>Set task type bit SHOW_IN_TASKS(ON or OFF) in FOX_TBL_TASK_TYPE</summary>
        public ResponseModel SetTaskTypeBit(FOX_TBL_TASK_TYPE model, UserProfile profile)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (model != null)
                {
                    var taskType = _taskTypeRepository.Get(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.TASK_TYPE_ID == model.TASK_TYPE_ID);

                    if (taskType != null)
                    {
                        taskType.MODIFIED_BY = profile.UserName;
                        taskType.MODIFIED_DATE = Helper.GetCurrentDate();
                        taskType.SHOW_IN_TASKS = model.SHOW_IN_TASKS;
                        _taskTypeRepository.Update(taskType);
                        _taskTypeRepository.Save();
                        responseModel.Message = "FOX_TBL_TASK_TYPE Updated successfully.";
                    }
                    else
                    {
                        responseModel.Message = "FOX_TBL_TASK_SUB_TYPE not found";
                    }
                    responseModel.Success = true;
                    responseModel.ErrorMessage = "";
                    return responseModel;
                }
                return new ResponseModel();
            }
            catch (Exception exception)
            {
                responseModel.Message = exception.Message;
                responseModel.Success = true;
                responseModel.ErrorMessage = exception.ToString();
                return responseModel;
            }
        }
        private void AddUpdateTaskApplicationUser(List<FOX_TBL_TASK_APPLICATION_USER> taskApplicationUser, long taskId, UserProfile profile)
        {
            if (taskApplicationUser != null)
            {
                foreach (var appUsr in taskApplicationUser)
                {
                    var dbTaskApplicationUser = _taskApplicationUserRepository.GetByID(appUsr.TASK_APPLICATION_USER_ID);
                    if (dbTaskApplicationUser == null)
                    {
                        appUsr.TASK_APPLICATION_USER_ID = Helper.getMaximumId("TASK_APPLICATION_USER_ID");
                        appUsr.TASK_ID = taskId;
                        appUsr.PRACTICE_CODE = profile.PracticeCode;
                        appUsr.CREATED_BY = profile.UserName;
                        appUsr.CREATED_DATE = Helper.GetCurrentDate();
                        appUsr.MODIFIED_BY = profile.UserName;
                        appUsr.MODIFIED_DATE = Helper.GetCurrentDate();
                        _taskApplicationUserRepository.Insert(appUsr);
                        _taskApplicationUserRepository.Save();
                    }
                    else
                    {
                        dbTaskApplicationUser.USER_ID = appUsr.USER_ID;
                        dbTaskApplicationUser.TASK_ID = appUsr.TASK_ID;
                        dbTaskApplicationUser.DELETED = appUsr.DELETED;
                        dbTaskApplicationUser.MODIFIED_BY = profile.UserName;
                        dbTaskApplicationUser.MODIFIED_DATE = Helper.GetCurrentDate();
                        _taskApplicationUserRepository.Update(dbTaskApplicationUser);
                        _taskApplicationUserRepository.Save();
                    }
                }
            }
        }
        public List<FOX_TBL_TASK_TYPE>  GetInactiveTaskTypeList(long practiceCode)
        {
            try
            {
                var task_Types = _taskTypeRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode).ToList();

                if (task_Types.Any())
                {
                    return task_Types;
                }
                else
                {
                    return new List<FOX_TBL_TASK_TYPE>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetTaskTemplateInitialData GetTaskTypeList(long practiceCode)
        {
            var task_Types = _taskTypeRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode).OrderBy(x => x.NAME).ToList();

            return new GetTaskTemplateInitialData()
            {
                Task_Types = task_Types,
                dropDownData = new DropDownData()
                {
                    SEND_CONTEXT = _sendContextRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode),
                    DELIVERY_METHOD = _deliveryMethodRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode),
                    CATEGORY = _categoryepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode),
                    ORDER_STATUS_RESULT = _OrderStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode)
                },
                getTaskTemplateResponse = GetTask(task_Types[0].TASK_TYPE_ID, practiceCode)
            };
            //return _taskTypeRepository.GetMany(x => x.DELETED == false && x.PRACTICE_CODE == practiceCode).OrderBy(x => x.NAME).ToList();
        }

        
        public List<FOX_TBL_TASK_SUB_TYPE> GetSubTaskTypeList(int taskTypeIdd, long practiceCode)
        {
            if (taskTypeIdd != 0 && practiceCode > 0)
            {
                return _taskSubTypeRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.TASK_TYPE_ID == taskTypeIdd)
                        .OrderBy(x => x.NAME)
                        .ToList();
            }
            else 
            //if (taskTypeIdd == 0 && practiceCode > 0)
            {
                return _taskSubTypeRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode )
                        .OrderBy(x => x.NAME)
                        .ToList();
            }
        }

        public DropDownData GetDropDownData(long practiceCode)
        {
            DropDownData dpData = new DropDownData();
            dpData.SEND_CONTEXT = _sendContextRepository.GetMany(x => x.DELETED == false && x.PRACTICE_CODE == practiceCode);
            dpData.DELIVERY_METHOD = _deliveryMethodRepository.GetMany(x => x.DELETED == false && x.PRACTICE_CODE == practiceCode);
            dpData.CATEGORY = _categoryepository.GetMany(x => x.DELETED == false && x.PRACTICE_CODE == practiceCode);
            dpData.ORDER_STATUS_RESULT = _OrderStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
            return dpData;
        }

        public TaskWithHistory GetTaskById(long taskId, long practiceCode)
        {
            TaskWithHistory _taskWithHistory = new TaskWithHistory();
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _taskTId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = taskId };
            var _taskTypeId = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = -1 };
            var _isTemplate = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = false };
            var task = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_TASK_BY_TASK_TYPE_ID]
                        @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCode, _taskTId, _taskTypeId, _isTemplate);
            if (task != null)
            {
                if (task.SEND_CONTEXT_ID == null) { task.SEND_CONTEXT_ID = 0; }
                if (task.DEVELIVERY_ID == null) { task.DEVELIVERY_ID = 0; }
                if (task.CATEGORY_ID == null) { task.CATEGORY_ID = 0; }
                var PracticeCode2 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var _taskId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_ID };
                var taskSubType = SpRepository<FOX_TBL_TASK_SUB_TYPE>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_SUB_TYPE_LIST @PRACTICE_CODE, @TASK_ID", PracticeCode2, _taskId);
                task.TASK_SUB_TYPE = taskSubType;
                var practice_Code = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var task_Id = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_ID };
                var task_Type_Id = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_TYPE_ID };
                var taskAllSubTypesList = SpRepository<OpenIssueViewModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_ALL_SUB_TYPE_LIST @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID", practice_Code, task_Id, task_Type_Id);
                task.TASK_ALL_SUB_TYPES_LIST = taskAllSubTypesList;

                if (task.PROVIDER_ID != null)
                {
                    task.PROVIDER_DETAIL = _referralSourceRepository.GetByID(task.PROVIDER_ID);
                }
                _taskWithHistory.Task = task;
                var activeNotes = _NotesRepository.GetFirst(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.TASK_ID == taskId && x.IS_ACTIVE == true);
                if(activeNotes != null)
                {
                    _taskWithHistory.Task.COMMENTS = activeNotes.NOTES;
                }
                _taskWithHistory.taskHistory = GetTaskHistory(task.TASK_ID, practiceCode);
                _taskWithHistory.Task.FinalRouteGroupUsers = this._groupUserRepository.GetMany(x => x.GROUP_ID == task.FINAL_ROUTE_ID && !x.DELETED && x.PRACTICE_CODE == practiceCode);
                _taskWithHistory.Task.SendToGroupUsers = this._groupUserRepository.GetMany(x => x.GROUP_ID == task.SEND_TO_ID && !x.DELETED && x.PRACTICE_CODE == practiceCode);
                return _taskWithHistory;
            }
            return null;
        }

        public GetTaskTemplateResponse GetTask(int taskTypeId, long practiceCode)
        {
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _taskTId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = -1 };
            var _taskTypeId = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = taskTypeId };
            var _isTemplate = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = true };
            var task = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_TASK_BY_TASK_TYPE_ID
                        @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCode, _taskTId, _taskTypeId, _isTemplate);

            if (task != null)
            {
                if (task.SEND_CONTEXT_ID == null) { task.SEND_CONTEXT_ID = 0; }
                if (task.DEVELIVERY_ID == null) { task.DEVELIVERY_ID = 0; }
                if (task.CATEGORY_ID == null) { task.CATEGORY_ID = 0; }
                task.COMMENTS = _NotesRepository.GetFirst(x => x.PRACTICE_CODE == practiceCode && x.TASK_ID == task.TASK_ID)?.NOTES;
                var PracticeCode2 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var _taskId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_ID };
                var taskApplicationUser = SpRepository<FOX_TBL_TASK_APPLICATION_USER>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_APPLICATION_USER_LIST @PRACTICE_CODE, @TASK_ID", PracticeCode2, _taskId);
                task.TASK_APPLICATION_USER = taskApplicationUser;
            }
            else
            {
                task = new FOX_TBL_TASK();
                task.TASK_TYPE_ID = taskTypeId;
                task.TASK_APPLICATION_USER = new List<FOX_TBL_TASK_APPLICATION_USER>();
            }

            var practice_Code = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var task_Id = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = DBNull.Value };
            var task_Type_Id = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.BigInt, Value = taskTypeId };
            var taskAllSubTypesList = SpRepository<OpenIssueViewModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_ALL_SUB_TYPE_LIST @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID", practice_Code, task_Id, task_Type_Id);

            task.TASK_ALL_SUB_TYPES_LIST = taskAllSubTypesList;
            //return task;

            return new GetTaskTemplateResponse()
            {
                Task = task,
                Task_Sub_Types = GetSubTaskTypeList(taskTypeId, practiceCode)

            };
        }

        public List<User> GetTTUserList(string searchText, long practiceCode) //TT stand for Task Type
        {
            return _UserRepository.GetMany(x => x.DELETED == false && x.IS_ACTIVE == true && x.PRACTICE_CODE == practiceCode && (x.LAST_NAME.Contains(searchText) || x.FIRST_NAME.Contains(searchText) || (x.LAST_NAME + ", " + x.FIRST_NAME).Contains(searchText)));
        }

        public List<UserAndGroup> GetUsersGroupList(string searchText, long practiceCode)
        {
            var user_group_ist = new List<UserAndGroup>();
            var paramsPracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var paramsSearchText = new SqlParameter { ParameterName = "SEARCH_TEXT", SqlDbType = SqlDbType.NVarChar, Value = searchText };
            user_group_ist = SpRepository<UserAndGroup>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_SMART_GROUPS_USMAN] @PRACTICE_CODE, @SEARCH_TEXT", paramsPracticeCode, paramsSearchText);

            //if (user_group_ist == null)
            //{
            //    user_group_ist = new List<UserAndGroup>();
            //}

            var paramsPracticeCodeUsr = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var paramsSearchTextUsr = new SqlParameter { ParameterName = "SEARCH_TEXT", SqlDbType = SqlDbType.NVarChar, Value = searchText };

            var usersList = SpRepository<UserAndGroup>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_SMART_USERS_USMAN] @PRACTICE_CODE, @SEARCH_TEXT", paramsPracticeCodeUsr, paramsSearchTextUsr);

            if (usersList.Count > 0)
            {
                user_group_ist.AddRange(usersList);
            }
            if (user_group_ist == null)
            {
                user_group_ist = new List<UserAndGroup>();
            }
            return user_group_ist;
        }

        public List<UserAndGroup> GetUsersList(string searchText, long practiceCode)
        {
         

            var paramsPracticeCodeUsr = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var paramsSearchTextUsr = new SqlParameter { ParameterName = "SEARCH_TEXT", SqlDbType = SqlDbType.NVarChar, Value = searchText };

            var usersList = SpRepository<UserAndGroup>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_SMART_USERS_USMAN] @PRACTICE_CODE, @SEARCH_TEXT", paramsPracticeCodeUsr, paramsSearchTextUsr);

           
            return usersList;
        }

        public categoriesData getcategoryandfield(UserProfile profile)
        {
            categoriesData dpData = new categoriesData();
            dpData.categories = _categoryRepository.GetMany(x => x.DELETED == false && x.PRACTICE_CODE == profile.PracticeCode);
            dpData.Fields = _fieldRepository.GetMany(x => x.DELETED == false && x.PRACTICE_CODE == profile.PracticeCode);
            var mappedData = _TaskSubTypeIntelTaskFieldRepository.GetAll().ToList();//.GetMany(t => t.USER_ID == profile.userID);
            if (mappedData != null && mappedData.Count() > 0)
            {
                foreach (var field in dpData.Fields)
                {
                    var mapping = mappedData.Find(e => e.INTEL_TASK_FIELD_ID == field.INTEL_TASK_FIELD_ID);
                    if (mapping != null)
                    {
                        field.IS_REQUIRED = mapping.IS_REQUIRED;
                    }
                }
            }
            return dpData;
        }

        public List<TaskDetail> GetTaskDetailList(TaskSearchRequest taskSearchRequest, UserProfile profile)
        {
            
            switch (taskSearchRequest.TIME_FRAME)
            {
                case 1:
                    taskSearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    taskSearchRequest.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 2:
                    taskSearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    taskSearchRequest.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 3:
                    taskSearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    taskSearchRequest.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(taskSearchRequest.DATE_FROM_STR))
                        taskSearchRequest.DATE_FROM = Convert.ToDateTime(taskSearchRequest.DATE_FROM_STR);
                    else
                        taskSearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-1);
                    if (!string.IsNullOrEmpty(taskSearchRequest.DATE_TO_STR))
                        taskSearchRequest.DATE_TO = Convert.ToDateTime(taskSearchRequest.DATE_TO_STR);
                    else
                    {
                        taskSearchRequest.DATE_TO = Helper.GetCurrentDate();
                    }
                    break;
                case 5:
                    taskSearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-2);
                    taskSearchRequest.DATE_TO = Helper.GetCurrentDate();
                    break;
                default:
                    break;
            }


            if(!string.IsNullOrEmpty(taskSearchRequest.DUE_DATE_TIME_str))
                taskSearchRequest.DUE_DATE_TIME = Convert.ToDateTime(taskSearchRequest.DUE_DATE_TIME_str);


            if(taskSearchRequest.TASK_TYPE_ID == 0)
            {
                taskSearchRequest.TASK_TYPE_ID = null;
            }
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var caseId = Helper.getDBNullOrValue("CASE_ID", taskSearchRequest.CASE_ID.ToString());
            var patientAccount = Helper.getDBNullOrValue("PATIENT_ACCOUNT", taskSearchRequest.Patient_AccountStr);
            var option = new SqlParameter { ParameterName = "OPTION", Value = taskSearchRequest.statusOption };
            var userId = new SqlParameter { ParameterName = "USER_ID", SqlDbType = SqlDbType.BigInt, Value = profile.userID };
            var currentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = taskSearchRequest.CurrentPage };
            var recordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = taskSearchRequest.RecordPerPage };
            var searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = taskSearchRequest.SearchText };
            var sortBy = new SqlParameter { ParameterName = "SORT_BY", Value = taskSearchRequest.SortBy };
            var sortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = taskSearchRequest.SortOrder };
            var insuranceId = Helper.getDBNullOrValue("INSURANCE_ID", taskSearchRequest.INSURANCE_ID.ToString() );
            var TaskTypeId = Helper.getDBNullOrValue("TASK_TYPE_ID", taskSearchRequest.TASK_TYPE_ID.ToString() );
            var TaskSubtypeId = Helper.getDBNullOrValue("TASK_SUB_TYPE_ID", taskSearchRequest.TASK_SUB_TYPE_ID.ToString() );
            var providereId = Helper.getDBNullOrValue("PROVIDER_ID", taskSearchRequest.PROVIDER_ID.ToString() );
            var region = new SqlParameter { ParameterName = "REGION", Value = taskSearchRequest.REGION ?? "" };
            var locID = Helper.getDBNullOrValue("LOC_ID", taskSearchRequest.LOC_ID.ToString());
            var cer_ref_source_ID = Helper.getDBNullOrValue("CERTIFYING_REF_SOURCE_ID",taskSearchRequest.CERTIFYING_REF_SOURCE_ID.ToString());
            var cer_ref_source_FAX = new SqlParameter { ParameterName = "CERTIFYING_REF_SOURCE_FAX", Value = taskSearchRequest.CERTIFYING_REF_SOURCE_FAX.ToString() ?? ""};
            var patient_zip = new SqlParameter { ParameterName = "PATIENT_ZIP_CODE", Value = taskSearchRequest.PATIENT_ZIP_CODE.ToString() ?? "" };
            var due_date = Helper.getDBNullOrValue("DUE_DATE_TIME", taskSearchRequest.DUE_DATE_TIME.ToString());
            var dateFrom = Helper.getDBNullOrValue("DATE_FROM", taskSearchRequest.DATE_FROM.ToString());
            var dateTo = Helper.getDBNullOrValue("DATE_TO", taskSearchRequest.DATE_TO.ToString());
            var ownerID = Helper.getDBNullOrValue("OWNER_ID", taskSearchRequest.OWNER_ID.ToString());
            var isUserLevel = Helper.getDBNullOrValue("IS_USER_LEVEL", taskSearchRequest.isUserLevel.ToString());
            SqlParameter Modified_By = new SqlParameter("MODIFIED_BY", string.IsNullOrEmpty( taskSearchRequest.Modified_By) ? string.Empty : taskSearchRequest.Modified_By);

            string storedProcedureName = string.Empty;
            if (profile?.isTalkRehab == true)
            {
                storedProcedureName = "FOX_PROC_GET_TASK_DETAIL_LIST_TalkRehab";
            }
            else
            {
                storedProcedureName = "FOX_PROC_GET_TASK_DETAIL_LIST";
            }

            var taskDetail = SpRepository<TaskDetail>.GetListWithStoreProcedure(@"exec "+ storedProcedureName + " @PRACTICE_CODE, @PATIENT_ACCOUNT, @CASE_ID, @OPTION, @USER_ID, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORT_BY, @SORT_ORDER,@INSURANCE_ID,@TASK_TYPE_ID,@TASK_SUB_TYPE_ID,@PROVIDER_ID,  @REGION,@LOC_ID,@CERTIFYING_REF_SOURCE_ID,@CERTIFYING_REF_SOURCE_FAX,@PATIENT_ZIP_CODE,@DUE_DATE_TIME,@DATE_FROM,@DATE_TO,@OWNER_ID, @MODIFIED_BY,@IS_USER_LEVEL",
                        PracticeCode, patientAccount, caseId, option, userId, currentPage, recordPerPage, searchText, sortBy, sortOrder, insuranceId, TaskTypeId, TaskSubtypeId, providereId, region, locID,
                       cer_ref_source_ID, cer_ref_source_FAX, patient_zip, due_date, dateFrom, dateTo, ownerID, isUserLevel, Modified_By);
            
            return taskDetail;
        }

        public List<ReferralSource> GetProviderList(string searchText, long practiceCode)
        {
            return _referralSourceRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && (x.FIRST_NAME.Contains(searchText) || x.LAST_NAME.Contains(searchText)));
        }

        public List<ActiveLocation> GetActiveLocationList(string searchText, long practiceCode)
        {
            return _activeLocationRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && (x.NAME.Contains(searchText) || x.CODE.Contains(searchText))).OrderBy(x => x.NAME).ToList();
        }

        public bool AuthenticateUser(string userName, string password)
        {
            var _userName = new SqlParameter("USERNAME", SqlDbType.VarChar) { Value = userName };
            var _password = new SqlParameter("PASSWORD", SqlDbType.VarChar) { Value = Encrypt.getEncryptedCode(password) };
            var user = SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_AUTHENTICATE_USER] @USERNAME,@PASSWORD", _userName, _password);
            if (user.STATUS == 200)
                return true;
            return false;
        }

        public ResponseModel SaveFilterTemplateRecord(GetCategoryFieldResp CatModel, UserProfile profile)
        {
            string resultisrequire = "";
            string result = "";
            ResponseModel model = new ResponseModel();
            try
            {
                foreach (var cat in CatModel.CategoryList)
                {
                    foreach (var field in cat.FieldList)
                    {
                        //var IntelTaskInfo = _TaskSubTypeIntelTaskFieldRepository.GetFirst(t => t.INTEL_TASK_FIELD_ID == field.INTEL_TASK_FIELD_ID && !t.DELETED && t.USER_ID == profile.userID && t.TASK_SUB_TYPE_ID == field.TASK_SUB_TYPE_ID);
                        var IntelTaskInfo = _TaskSubTypeIntelTaskFieldRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.INTEL_TASK_FIELD_ID == field.INTEL_TASK_FIELD_ID && t.TASK_SUB_TYPE_ID == field.TASK_SUB_TYPE_ID);
                        bool IsEditIntelTaskInfo = false;
                        bool doNotInsert = true;
                        if (IntelTaskInfo == null)
                        {
                            if (field.IS_REQUIRED ?? false)
                            {
                                IntelTaskInfo = new FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD();
                                IntelTaskInfo.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID = Helper.getMaximumId("FOX_TASK_SUB_TYPE_INTEL_TASK_FIELD_ID");
                                IntelTaskInfo.PRACTICE_CODE = profile.PracticeCode;
                                IntelTaskInfo.TASK_SUB_TYPE_ID = field.TASK_SUB_TYPE_ID;
                                IntelTaskInfo.INTEL_TASK_FIELD_ID = field.INTEL_TASK_FIELD_ID;
                                IntelTaskInfo.CREATED_BY = IntelTaskInfo.MODIFIED_BY = profile.UserName;
                                IntelTaskInfo.CREATED_DATE = IntelTaskInfo.MODIFIED_DATE = DateTime.Now;
                                IsEditIntelTaskInfo = false;
                                doNotInsert = false;
                            }
                        }
                        else
                        {
                            IntelTaskInfo.MODIFIED_BY = profile.UserName;
                            IntelTaskInfo.MODIFIED_DATE = DateTime.Now;
                            IsEditIntelTaskInfo = true;
                            doNotInsert = false;
                        }
                        if (!doNotInsert)//means insert
                        {
                            IntelTaskInfo.IS_REQUIRED = field.IS_REQUIRED;
                            //IntelTaskInfo.USER_ID = profile.userID;

                            if (IsEditIntelTaskInfo)
                            {
                                _TaskSubTypeIntelTaskFieldRepository.Update(IntelTaskInfo);
                                resultisrequire = "update";
                            }
                            else
                            {
                                _TaskSubTypeIntelTaskFieldRepository.Insert(IntelTaskInfo);
                            }

                            field.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID = IntelTaskInfo.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID;

                            result = SaveSubTypeMapping(field, profile);
                        }

                        if (!doNotInsert)
                        {
                            doNotInsert = true;
                            _TaskContext.SaveChanges();
                        }
                        // AddEditSourceMappInfo(CatModel.SubTypeRef_MappingList,profile);
                    }

                    if (resultisrequire.Contains("update") || result.Contains("update"))
                    {
                        //records were ONLY added
                        model.Message = "update";
                    }
                }

                model.Success = true;
                return model;
            }
            catch (Exception exception)
            {
                model.Success = false;
                return model;
                //To Do Log Exception here
                throw exception;
            }
        }

        public void DeleteOrdStatusMappingIfExistedBefore(long? appUserFieldId, List<int?> OrderStatusIdList)
        {
            try
            {
                if (appUserFieldId != null)
                {
                    var mappedData = _TaskRefMapRepository.GetMany(e => e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.HasValue && e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.Value == appUserFieldId.Value && e.ORDER_STATUS_ID.HasValue);
                    foreach (var item in mappedData)
                    {
                        if (OrderStatusIdList != null && !OrderStatusIdList.Contains(item.ORDER_STATUS_ID))
                        {
                            item.DELETED = true;
                            _TaskRefMapRepository.Update(item);
                            _TaskRefMapRepository.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateUpdateOrdStatusMapping(CatFieldRes fieldData, UserProfile profile)
        {
            string resultorder = "";
            try
            {
                //if (fieldData.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.HasValue && fieldData.OrderStatusIdList != null && fieldData.OrderStatusIdList.Count() > 0)
                //{
                if (fieldData.OrderStatusIdList != null)
                {
                    foreach (var orderStatusId in fieldData.OrderStatusIdList)
                    {
                        var mapping = new FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING();
                        var mappedData = _TaskRefMapRepository.GetFirst(e => e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.HasValue && e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.Value == fieldData.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.Value && e.ORDER_STATUS_ID == orderStatusId && !e.DELETED);

                        if (mappedData == null)
                        {
                            mapping.TASKSUBTYPE_REFSOURCE_MAPPING_ID = Helper.getMaximumId("FOX_TASKSUBTYPE_REFSOURCE_MAPPING_ID");
                            mapping.PRACTICE_CODE = profile.PracticeCode;
                            mapping.CREATED_BY = mapping.MODIFIED_BY = profile.UserName;
                            mapping.CREATED_DATE = mapping.MODIFIED_DATE = Helper.GetCurrentDate();
                            mapping.DELETED = false;
                        }
                        else
                        {
                            mapping = mappedData;
                            mapping.MODIFIED_BY = profile.UserName;
                            mapping.MODIFIED_DATE = Helper.GetCurrentDate();
                        }

                        mapping.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID = fieldData.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID;
                        mapping.ORDER_STATUS_ID = orderStatusId;
                        mapping.CONDITION = fieldData.Condition;

                        if (mappedData == null)
                        {
                            _TaskRefMapRepository.Insert(mapping);
                        }
                        else
                        {
                            _TaskRefMapRepository.Update(mapping);
                            resultorder = "update";
                        }

                        _TaskRefMapRepository.Save();
                    }
                }
                return resultorder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaveSubTypeMapping(CatFieldRes obj, UserProfile profile)
        {
            string resultrefrenceorder = "";
            try
            {
                if (obj.CATEGORY_NAME.ToLower().Contains("housecall") && obj.FIELD_NAME.ToLower().Contains("order status"))
                {
                    DeleteOrdStatusMappingIfExistedBefore(obj.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID, obj.OrderStatusIdList);
                    resultrefrenceorder = CreateUpdateOrdStatusMapping(obj, profile);
                }
                else if (obj.CATEGORY_NAME.ToLower().Contains("index info source") && obj.FIELD_NAME.ToLower().Contains("ordering referral source"))
                {
                    DeleteSrcMappingIfExistedBefore(obj.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID, obj.SourceIdList);
                    resultrefrenceorder = CreateUpdateRefSourceMapping(obj, profile);
                }
                return resultrefrenceorder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateUpdateRefSourceMapping(CatFieldRes fieldData, UserProfile profile)
        {
            string resultrefrence = "";
            try
            {
                //if (fieldData.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.HasValue && fieldData.OrderStatusIdList != null && fieldData.OrderStatusIdList.Count() > 0)
                //{
                if (fieldData.SourceIdList != null)
                {
                    foreach (var refSrcId in fieldData.SourceIdList)
                    {
                        var mapping = new FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING();
                        var mappedData = _TaskRefMapRepository.GetFirst(e => e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.HasValue && e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.Value == fieldData.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.Value && e.SOURCE_ID == refSrcId && !e.DELETED);

                        if (mappedData == null)
                        {
                            mapping.TASKSUBTYPE_REFSOURCE_MAPPING_ID = Helper.getMaximumId("FOX_TASKSUBTYPE_REFSOURCE_MAPPING_ID");
                            mapping.PRACTICE_CODE = profile.PracticeCode;
                            mapping.CREATED_BY = mapping.MODIFIED_BY = profile.UserName;
                            mapping.CREATED_DATE = mapping.MODIFIED_DATE = Helper.GetCurrentDate();
                            mapping.DELETED = false;
                        }
                        else
                        {
                            mapping = mappedData;
                            mapping.MODIFIED_BY = profile.UserName;
                            mapping.MODIFIED_DATE = Helper.GetCurrentDate();
                        }
                        mapping.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID = fieldData.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID;
                        mapping.SOURCE_ID = refSrcId;
                        mapping.CONDITION = fieldData.Condition;

                        if (mappedData == null)
                        {
                            _TaskRefMapRepository.Insert(mapping);
                        }
                        else
                        {
                            _TaskRefMapRepository.Update(mapping);
                            resultrefrence = "update";
                        }

                        _TaskRefMapRepository.Save();
                    }
                }
                return resultrefrence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteSrcMappingIfExistedBefore(long? appUserFieldId, List<long?> refSourceIdList)
        {
            try
            {
                if (appUserFieldId != null)
                {
                    var mappedData = _TaskRefMapRepository.GetMany(e => e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.HasValue && e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID.Value == appUserFieldId.Value && e.SOURCE_ID.HasValue);
                    foreach (var item in mappedData)
                    {
                        if (refSourceIdList != null && !refSourceIdList.Contains(item.SOURCE_ID))
                        {
                            item.DELETED = true;
                            _TaskRefMapRepository.Update(item);
                            _TaskRefMapRepository.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetCategoryFieldResp GetCategoryFields(getCatFieldReq obj, UserProfile profile)
        {
            var categoryList = new List<FOX_TBL_INTEL_TASK_CATEGORY>();
            try
            {

                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var UserId = new SqlParameter { ParameterName = "USER_ID", SqlDbType = SqlDbType.BigInt, Value = profile.userID };
                var _TASK_SUB_TYPE_ID = new SqlParameter { ParameterName = "TASK_SUB_TYPE_ID", SqlDbType = SqlDbType.Int, Value = obj.TASK_SUB_TYPE_ID };
                var result = SpRepository<CatFieldRes>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_INTEL_TEMPL_LIST @PRACTICE_CODE, @USER_ID, @TASK_SUB_TYPE_ID",
                          PracticeCode, UserId, _TASK_SUB_TYPE_ID).ToList();

                if (result != null && result.Count() > 0)
                {
                    var categories = _categoryRepository.GetMany(e => !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);
                    if (categories != null && categories.Count() > 0)
                    {
                        foreach (var cat in categories)
                        {
                            var _fieldList = result.Where(x => x.INTEL_TASK_CATEGORY_ID == cat.INTEL_TASK_CATEGORY_ID).ToList();
                            cat.FieldList = _fieldList;
                            if (cat.NAME.ToLower().Contains("index info source"))
                            {
                                foreach (var field in cat.FieldList)
                                {
                                    if (field.FIELD_NAME.ToLower().Contains("referral source"))
                                    {
                                        var refSourceRow = result.Where(t => t.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID != null && t.FIELD_NAME.ToLower().Contains("referral source"));
                                        if (refSourceRow != null)
                                        {
                                            var appUserId = refSourceRow.Select(e => e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID).FirstOrDefault();
                                            if (appUserId != null)
                                            {
                                                var refSourceMappingData = _TaskRefMapRepository.GetMany(x => x.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID == appUserId && x.SOURCE_ID.HasValue && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                                                if (refSourceMappingData != null && refSourceMappingData.Count > 0)
                                                {
                                                    field.SourceIdList = refSourceMappingData.Select(t => t.SOURCE_ID).ToList();
                                                    field.Condition = refSourceMappingData.FirstOrDefault() != null ? (refSourceMappingData.FirstOrDefault().CONDITION ?? "") : "";
                                                    if (field.SourceIdList != null && field.SourceIdList.Count() > 0)
                                                    {
                                                        field.SourceListDataToDisplay = new List<SmartOrderSource>();
                                                        foreach (var srcId in field.SourceIdList)
                                                        {
                                                            var src = _referralSourceRepository.GetByID(srcId);
                                                            if (src != null)
                                                            {
                                                                var srcInfo = new SmartOrderSource();
                                                                srcInfo.SOURCE_ID = src.SOURCE_ID;
                                                                srcInfo.Name = src.FIRST_NAME + " " + src.LAST_NAME +
                                                                    (!string.IsNullOrEmpty(src.REFERRAL_REGION) ? " | " + src.REFERRAL_REGION : "");
                                                                srcInfo.CODE = src.CODE;
                                                                srcInfo.CITY = src.CITY;
                                                                srcInfo.STATE = src.STATE;
                                                                srcInfo.PHONE = src.PHONE;
                                                                srcInfo.FAX = src.FAX;
                                                                srcInfo.NPI = src.NPI;

                                                                field.SourceListDataToDisplay.Add(srcInfo);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //
                                    if (field.SourceIdList == null)
                                    {
                                        field.SourceIdList = new List<long?>();
                                        field.SourceListDataToDisplay = new List<SmartOrderSource>();
                                    }
                                    if (field.OrderStatusIdList == null)
                                        field.OrderStatusIdList = new List<int?>();
                                }
                            }
                            else if (cat.NAME.ToLower().Contains("housecall"))
                            {
                                foreach (var field in cat.FieldList)
                                {
                                    if (field.FIELD_NAME.ToLower().Contains("order status"))
                                    {
                                        var orderStatusRow = result.Where(t => t.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID != null && t.FIELD_NAME.ToLower().Contains("order status"));
                                        if (orderStatusRow != null)
                                        {
                                            var appUserId = orderStatusRow.Select(e => e.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID).FirstOrDefault();
                                            if (appUserId != null)
                                            {
                                                var orderStatusMappingData = _TaskRefMapRepository.GetMany(x => x.TASK_SUB_TYPE_INTEL_TASK_FIELD_ID == appUserId && x.ORDER_STATUS_ID.HasValue && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                                                if (orderStatusMappingData != null && orderStatusMappingData.Count > 0)
                                                {
                                                    field.OrderStatusIdList = orderStatusMappingData.Select(t => t.ORDER_STATUS_ID).ToList();
                                                    field.Condition = orderStatusMappingData.FirstOrDefault() != null ? (orderStatusMappingData.FirstOrDefault().CONDITION ?? "") : "";
                                                }
                                            }
                                        }
                                    }

                                    //
                                    if (field.SourceIdList == null)
                                    {
                                        field.SourceIdList = new List<long?>();
                                        field.SourceListDataToDisplay = new List<SmartOrderSource>();
                                    }
                                    if (field.OrderStatusIdList == null)
                                        field.OrderStatusIdList = new List<int?>();
                                }
                            }

                            categoryList.Add(cat);
                        }
                    }

                }
                return new GetCategoryFieldResp()
                {
                    Success = true,
                    Message = "",
                    ErrorMessage = "",
                    CategoryList = categoryList,
                };
            }
            catch (Exception ex)
            {
                return new GetCategoryFieldResp()
                {
                    Success = false,
                    Message = "",
                    ErrorMessage = ex.Message,
                    CategoryList = categoryList,
                };
            }
        }

        public TaskWithHistory GetTaskByGeneralNoteId(long generalNoteId, long practiceCode)
        {
            TaskWithHistory _taskWithHistory = new TaskWithHistory();
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _generalNoteId = new SqlParameter { ParameterName = "GENERAL_NOTE_ID", SqlDbType = SqlDbType.BigInt, Value = generalNoteId };
            var _taskTypeId = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = -1 };
            var _isTemplate = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = false };
            var task = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_TASK_BY_GENERAL_NOTE_ID
                        @PRACTICE_CODE, @GENERAL_NOTE_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCode, _generalNoteId, _taskTypeId, _isTemplate);
            if (task != null)
            {
                if (task.SEND_CONTEXT_ID == null) { task.SEND_CONTEXT_ID = 0; }
                if (task.DEVELIVERY_ID == null) { task.DEVELIVERY_ID = 0; }
                if (task.CATEGORY_ID == null) { task.CATEGORY_ID = 0; }
                //Comments for this task
                var note = _NotesRepository.GetMany(x => x.PRACTICE_CODE == practiceCode && x.TASK_ID == task.TASK_ID).OrderByDescending(t => t.CREATED_DATE).FirstOrDefault();
                if (note != null)
                {
                    task.COMMENTS = note.NOTES;
                }
                //Task sub type list for this task
                var PracticeCode2 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var _taskId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_ID };
                var taskSubType = SpRepository<FOX_TBL_TASK_SUB_TYPE>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_SUB_TYPE_LIST @PRACTICE_CODE, @TASK_ID", PracticeCode2, _taskId); if (task.PROVIDER_ID != null)
                {
                    task.PROVIDER_DETAIL = _referralSourceRepository.GetByID(task.PROVIDER_ID);
                }
                task.TASK_SUB_TYPE = taskSubType;

                //usman
                var practice_Code = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var task_Id = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_ID };
                var task_Type_Id = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_TYPE_ID };
                var taskAllSubTypesList = SpRepository<OpenIssueViewModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_ALL_SUB_TYPE_LIST @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID", practice_Code, task_Id, task_Type_Id);
                task.TASK_ALL_SUB_TYPES_LIST = taskAllSubTypesList;


                task.FinalRouteGroupUsers = this._groupUserRepository.GetMany(x => x.GROUP_ID == task.FINAL_ROUTE_ID && !x.DELETED && x.PRACTICE_CODE == practiceCode);
                task.SendToGroupUsers = this._groupUserRepository.GetMany(x => x.GROUP_ID == task.SEND_TO_ID && !x.DELETED && x.PRACTICE_CODE == practiceCode);
                _taskWithHistory.Task = task;

                var activeNotes = _NotesRepository.GetFirst(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.TASK_ID == task.TASK_ID && x.IS_ACTIVE == true);
                if (activeNotes != null)
                {
                    _taskWithHistory.Task.COMMENTS = activeNotes.NOTES;
                }

                _taskWithHistory.taskHistory = GetTaskHistory(task.TASK_ID, practiceCode);
            }
            return _taskWithHistory;
        }

        public TaskWithHistory GetTaskByTaskId(long taskId, UserProfile profile)
        {
            TaskWithHistory _taskWithHistory = new TaskWithHistory();
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var _taskId1 = new SqlParameter { ParameterName = "GENERAL_NOTE_ID", SqlDbType = SqlDbType.BigInt, Value = taskId };
            var _taskTypeId = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = -1 };
            var _isTemplate = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = false };
            var task = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_TASK_BY_TASK_ID
                        @PRACTICE_CODE, @GENERAL_NOTE_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCode, _taskId1, _taskTypeId, _isTemplate);
            if (task != null)
            {
                var PracticeCode2 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var _taskId2 = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_ID };
                var taskSubType = SpRepository<FOX_TBL_TASK_SUB_TYPE>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_SUB_TYPE_LIST @PRACTICE_CODE, @TASK_ID", PracticeCode2, _taskId2); if (task.PROVIDER_ID != null)
                {
                    task.PROVIDER_DETAIL = _referralSourceRepository.GetByID(task.PROVIDER_ID);
                }
                task.TASK_SUB_TYPE = taskSubType;

                var practice_Code = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var task_Id = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_ID };
                var task_Type_Id = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.BigInt, Value = task.TASK_TYPE_ID };
                var taskAllSubTypesList = SpRepository<OpenIssueViewModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_TASK_ALL_SUB_TYPE_LIST @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID", practice_Code, task_Id, task_Type_Id);
                task.TASK_ALL_SUB_TYPES_LIST = taskAllSubTypesList;

                task.FinalRouteGroupUsers = this._groupUserRepository.GetMany(x => x.GROUP_ID == task.FINAL_ROUTE_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                task.SendToGroupUsers = this._groupUserRepository.GetMany(x => x.GROUP_ID == task.SEND_TO_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                _taskWithHistory.Task = task;

                var activeNotes = _NotesRepository.GetFirst(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.TASK_ID == taskId && x.IS_ACTIVE == true);
                if (activeNotes != null)
                {
                    _taskWithHistory.Task.COMMENTS = activeNotes.NOTES;
                }

                _taskWithHistory.taskHistory = GetTaskHistory(task.TASK_ID, profile.PracticeCode);
            }
            return _taskWithHistory;
        }

        private List<TaskHistory> GetTaskHistory(long taskId, long practiceCode)
        {
            var PracticeCodelog = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _taskTIdlog = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = taskId };
            var _taskTypeIdlog = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = -1 };
            var _isTemplatelog = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = false };
            var _taskHistory = SpRepository<TaskHistory>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_TASK_BY_TASK_LOG]
                        @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCodelog, _taskTIdlog, _taskTypeIdlog, _isTemplatelog);

            return _taskHistory;
        }

        private void AddEditTask_TaskSubTypes(long tASK_ID, List<OpenIssueViewModel> taskSubTypeList, UserProfile profile)
        {
            try
            {
                var NewSubtypeList = taskSubTypeList.Select(t => t.TASK_SUB_TYPE_ID).ToList();
                var OldSubtypeList = _TaskTaskSubTypeRepository.GetMany(t => t.TASK_ID == tASK_ID && !NewSubtypeList.Contains(t.TASK_SUB_TYPE_ID ?? 0) && !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode);
                OldSubtypeList?.ForEach(t =>
                {
                    t.DELETED = true;
                    _TaskTaskSubTypeRepository.Update(t);
                    _TaskTaskSubTypeRepository.Save();
                }
                );

                foreach (var taskSubType in taskSubTypeList)
                {
                    if (taskSubType.IS_CHECKED)
                    {
                        var taskTaskSubType = _TaskTaskSubTypeRepository.GetFirst(t =>
                                                                                  t.TASK_ID == tASK_ID
                                                                                  && t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID
                                                                                  //&& !t.DELETED
                                                                                  && t.PRACTICE_CODE == profile.PracticeCode);
                        bool IsEdit = false;

                        if (taskTaskSubType == null && taskSubType.IS_CHECKED)
                        {
                            taskTaskSubType = new FOX_TBL_TASK_TASK_SUB_TYPE();
                            taskTaskSubType.TASK_TASK_SUB_TYPE_ID = Helper.getMaximumId("FOX_TBL_TASK_TASK_SUB_TYPE_ID");
                            taskTaskSubType.PRACTICE_CODE = profile.PracticeCode;
                            taskTaskSubType.TASK_ID = tASK_ID;
                            taskTaskSubType.TASK_SUB_TYPE_ID = taskSubType.TASK_SUB_TYPE_ID;
                            taskTaskSubType.CREATED_BY = taskTaskSubType.MODIFIED_BY = profile.UserName;
                            taskTaskSubType.CREATED_DATE = taskTaskSubType.MODIFIED_DATE = DateTime.Now;
                            IsEdit = false;
                        }
                        else
                        {
                            taskTaskSubType.MODIFIED_BY = profile.UserName;
                            taskTaskSubType.MODIFIED_DATE = DateTime.Now;
                            IsEdit = true;
                        }
                        //taskTaskSubType.DELETED = false;
                        taskTaskSubType.DELETED = !taskSubType.IS_CHECKED;

                        if (IsEdit)
                        {
                            _TaskTaskSubTypeRepository.Update(taskTaskSubType);
                        }
                        else
                        {
                            _TaskTaskSubTypeRepository.Insert(taskTaskSubType);
                        }
                        _TaskTaskSubTypeRepository.Save();
                    }
                    else
                    {
                        var taskTaskSubType = _TaskTaskSubTypeRepository.GetFirst(t =>
                                                                                  t.TASK_ID == taskSubType.TASK_ID
                                                                                  && t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID
                                                                                  && !t.DELETED
                                                                                  && t.PRACTICE_CODE == profile.PracticeCode);
                        if (taskTaskSubType != null)
                        {
                            taskTaskSubType.DELETED = true;
                            _TaskTaskSubTypeRepository.Update(taskTaskSubType);
                            _TaskTaskSubTypeRepository.Save();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void InsertInterfaceTeamData(InterfaceSynchModel obj, UserProfile Profile)
        {
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();

            if (obj.PATIENT_ACCOUNT == null && obj.CASE_ID == null && obj.TASK_ID == null)
            {

            }
            else
            {
                interfaceSynch.FOX_INTERFACE_SYNCH_ID = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                interfaceSynch.CASE_ID = obj.CASE_ID;
                interfaceSynch.Work_ID = obj.Work_ID;
                interfaceSynch.TASK_ID = obj.TASK_ID;
                interfaceSynch.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                interfaceSynch.PRACTICE_CODE = Profile.PracticeCode;
                interfaceSynch.MODIFIED_BY = interfaceSynch.CREATED_BY = Profile.UserName;
                interfaceSynch.MODIFIED_DATE = interfaceSynch.CREATED_DATE = DateTime.Now;
                interfaceSynch.DELETED = false;
                interfaceSynch.IS_SYNCED = false;
                __InterfaceSynchModelRepository.Insert(interfaceSynch);
                _CaseContext.SaveChanges();
            }
        }

        public TaskWithHistory GetTask(long patientAccount, long caseId, long taskTypeId, UserProfile profile)
        {
            var _task = _TaskRepository.GetFirst(row => !row.DELETED && row.PRACTICE_CODE == profile.PracticeCode && row.PATIENT_ACCOUNT == patientAccount && row.CASE_ID == caseId && row.TASK_TYPE_ID == taskTypeId);
            if(_task != null)
            {
                return GetTaskByTaskId(_task.TASK_ID, profile);
            }
            return new TaskWithHistory();
        }

        public string ExportToExcel(TaskSearchRequest taskSearchRequest, UserProfile profile)
        {
            try
            {
                string fileName = "Task";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                var CalledFrom = "";
                if (taskSearchRequest.PATIENT_ACCOUNT == null)
                {
                    CalledFrom = "Task_User_Level";
                }
                else
                {
                    CalledFrom = "Task";
                }
              
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                var pathtowriteFile = exportPath + "\\" + fileName;
                var result = GetTaskDetailList(taskSearchRequest, profile);
                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
                TextInfo text_info = culture_info.TextInfo;
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].CREATED_BY_FULL_NAME = text_info.ToTitleCase(result[i].CREATED_BY_FULL_NAME);
                    result[i].PATIENT_FULL_NAME = text_info.ToTitleCase(result[i].PATIENT_FULL_NAME);
                    result[i].MODIFIED_BY_FULL_NAME = text_info.ToTitleCase(result[i].MODIFIED_BY_FULL_NAME);
                    result[i].ROW = i + 1;

                }
                foreach (var row in result)
                {
                    row.CREATED_DATE_STR = string.IsNullOrEmpty( row.CREATED_DATE.ToString("MM/dd/yyyy")) ? string.Empty : row.CREATED_DATE.ToString("MM/dd/yyyy");
                    row.CREATED_TIME_STR = string.IsNullOrEmpty( row.CREATED_DATE.ToString("hh:mm:ss tt")) ? string.Empty : row.CREATED_DATE.ToString("hh:mm:ss tt");

                    
                    row.DUE_DATE_TIME_STR = string.IsNullOrEmpty(row.DUE_DATE_TIME?.ToString("MM/dd/yyyy")) ? string.Empty: row.DUE_DATE_TIME?.ToString("MM/dd/yyyy");
                    row.PRIORITY = Helper.ChangeStringToTitleCase(row.PRIORITY);
                }

                exported = FOX.BusinessOperations.CommonServices.ExportToExcel.CreateExcelDocument<TaskDetail>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TaskDashboardResponse GetTaskDashBoardData(TaskDashboardSearchRequest req, UserProfile profile)
        {
            List <string> dateList = new List<string>();
            List<string> TaskTypes = new List<string>();
            TaskDashboardResponse taskDashboardResponse = new TaskDashboardResponse();
            req.DATE_TO = Helper.GetCurrentDate();
            if (!string.IsNullOrEmpty(req.DATE_FROM_STR))
                req.DATE_FROM = Convert.ToDateTime(req.DATE_FROM_STR);
            if (!string.IsNullOrEmpty(req.DATE_TO_STR))
            { 
                req.DATE_TO = Convert.ToDateTime(req.DATE_TO_STR);
            }
            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode  };
            SqlParameter _groupIds = new SqlParameter { ParameterName = "GROUP_IDs", SqlDbType = SqlDbType.VarChar, Value = req.GROUP_IDs ?? (object)DBNull.Value };
            SqlParameter _taskTypeIds = new SqlParameter { ParameterName = "TASK_TYPE_IDs", SqlDbType = SqlDbType.VarChar, Value = req.TASK_TYPE_IDs ?? (object)DBNull.Value };
            SqlParameter _dateFrom = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.ToString() ?? "");
            SqlParameter _dateTos = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.ToString() ?? "");
            SqlParameter _timeFrame= new SqlParameter { ParameterName = "TIME_FRAME", SqlDbType = SqlDbType.VarChar, Value = req.TIME_FRAME ?? (object)DBNull.Value };
            var openCloseStatus = SpRepository<TaskStatus>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_TASK_STATUS_DATA_FOR_DASHBOARD]
                        @PRACTICE_CODE, @GROUP_IDs, @TASK_TYPE_IDs, @DATE_FROM, @DATE_TO, @TIME_FRAME", _practiceCode, _groupIds, _taskTypeIds, _dateFrom, _dateTos, _timeFrame);
            taskDashboardResponse.TaskStatus = openCloseStatus;


            SqlParameter _practiceCode1 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _groupIds1 = new SqlParameter { ParameterName = "GROUP_IDs", SqlDbType = SqlDbType.VarChar, Value = req.GROUP_IDs ?? (object)DBNull.Value };
            SqlParameter _taskTypeIds1 = new SqlParameter { ParameterName = "TASK_TYPE_IDs", SqlDbType = SqlDbType.VarChar, Value = req.TASK_TYPE_IDs ?? (object)DBNull.Value };
            SqlParameter _dateFrom1 = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.ToString() ?? "");
            SqlParameter _dateTos1 = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.ToString() ?? "");
            SqlParameter _timeFrame1 = new SqlParameter { ParameterName = "TIME_FRAME", SqlDbType = SqlDbType.VarChar, Value = req.TIME_FRAME ?? (object)DBNull.Value };
            var pastDueStatus = SpRepository<TaskDueDateStatus>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_TASK_PAST_DUE_DATE_DATA_FOR_DASHBOARD]
                        @PRACTICE_CODE, @GROUP_IDs, @TASK_TYPE_IDs, @DATE_FROM, @DATE_TO, @TIME_FRAME", _practiceCode1, _groupIds1, _taskTypeIds1, _dateFrom1, _dateTos1, _timeFrame1);
            taskDashboardResponse.TaskDueDateStatus = pastDueStatus;


            SqlParameter _practiceCode2 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _groupIds2 = new SqlParameter { ParameterName = "GROUP_IDs", SqlDbType = SqlDbType.VarChar, Value = req.GROUP_IDs ?? (object)DBNull.Value };
            SqlParameter _taskTypeIds2 = new SqlParameter { ParameterName = "TASK_TYPE_IDs", SqlDbType = SqlDbType.VarChar, Value = req.TASK_TYPE_IDs ?? (object)DBNull.Value };
            SqlParameter _dateFrom2 = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.ToString()?? "");
            SqlParameter _dateTos2 = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.ToString()?? "");
            SqlParameter _timeFram2 = new SqlParameter { ParameterName = "TIME_FRAME", SqlDbType = SqlDbType.VarChar, Value = req.TIME_FRAME ?? (object)DBNull.Value };
            var avgClosureTime = SpRepository<TaskaAverageClosureTime>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_TASK_AVERAGE_CLOSURE_TIME_DATA_FOR_DASHBOARD]
                        @PRACTICE_CODE, @GROUP_IDs, @TASK_TYPE_IDs, @DATE_FROM, @DATE_TO, @TIME_FRAME", _practiceCode2, _groupIds2, _taskTypeIds2, _dateFrom2, _dateTos2, _timeFram2);
            if (avgClosureTime != null && avgClosureTime.Count != 0)
            {
                taskDashboardResponse.TaskaAverageClosureTime = avgClosureTime;
            }
            else
            {
                taskDashboardResponse.TaskaAverageClosureTime = new List<TaskaAverageClosureTime>();
            }
            SqlParameter _practiceCode3 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _groupIds3 = new SqlParameter { ParameterName = "GROUP_IDs", SqlDbType = SqlDbType.VarChar, Value = req.GROUP_IDs ?? (object)DBNull.Value };
            SqlParameter _taskTypeIds3 = new SqlParameter { ParameterName = "TASK_TYPE_IDs", SqlDbType = SqlDbType.VarChar, Value = req.TASK_TYPE_IDs ?? (object)DBNull.Value };
            SqlParameter _dateFrom3 = Helper.getDBNullOrValue("@DateFromUser", req.DATE_FROM.ToString() ?? "");
            SqlParameter _dateTos3 = Helper.getDBNullOrValue("@DateToUser", req.DATE_TO.ToString() ?? "");
            SqlParameter _timeFrame3 = new SqlParameter { ParameterName = "TIME_FRAME", SqlDbType = SqlDbType.VarChar, Value = req.TIME_FRAME ?? (object)DBNull.Value };
            var newTAskData = SpRepository<NewTaskStatus>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_NEW_TASK_DATA_FOR_DASHBOARD]
                        @PRACTICE_CODE,@DateFromUser, @DateToUser, @GROUP_IDs, @TASK_TYPE_IDs,  @TIME_FRAME", _practiceCode3, _dateFrom3, _dateTos3, _groupIds3, _taskTypeIds3,  _timeFrame3);
            taskDashboardResponse.NewTaskStatus = newTAskData;


            SqlParameter _practiceCode4 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _groupIds4 = new SqlParameter { ParameterName = "GROUP_IDs", SqlDbType = SqlDbType.VarChar, Value = req.GROUP_IDs ?? (object)DBNull.Value };
            SqlParameter _taskTypeIds4 = new SqlParameter { ParameterName = "TASK_TYPE_IDs", SqlDbType = SqlDbType.VarChar, Value = req.TASK_TYPE_IDs ?? (object)DBNull.Value };
            SqlParameter _dateFrom4 = Helper.getDBNullOrValue("@DateFromUser", req.DATE_FROM.ToString() ?? "");
            SqlParameter _dateTos4 = Helper.getDBNullOrValue("@DateToUser", req.DATE_TO.ToString() ?? "");
            SqlParameter _timeFrame4 = new SqlParameter { ParameterName = "TIME_FRAME", SqlDbType = SqlDbType.VarChar, Value = req.TIME_FRAME ?? (object)DBNull.Value };
            var taskOverAllStatus = SpRepository<TaskOverAllStatus>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_TASK_OVERALL_DATA_FOR_DASHBOARD]
                        @PRACTICE_CODE, @GROUP_IDs, @TASK_TYPE_IDs, @DateFromUser, @DateToUser, @TIME_FRAME", _practiceCode4, _groupIds4, _taskTypeIds4, _dateFrom4, _dateTos4, _timeFrame4);
            if(taskOverAllStatus !=null)
            {
                taskDashboardResponse.TaskOverAllStatus = taskOverAllStatus;
                if (taskOverAllStatus.AVERAGE_CLOSURE != null)
                {
                    if(string.Equals(taskOverAllStatus.AVERAGE_CLOSURE.ToLower(), "1 days"))
                    {
                        taskOverAllStatus.AVERAGE_CLOSURE =  taskOverAllStatus.AVERAGE_CLOSURE.Substring(0, taskOverAllStatus.AVERAGE_CLOSURE.Length - 1);
                    }
                    if (string.Equals(taskOverAllStatus.AVERAGE_CLOSURE.ToLower(), "1 hours"))
                    {
                        taskOverAllStatus.AVERAGE_CLOSURE = taskOverAllStatus.AVERAGE_CLOSURE.Substring(0, taskOverAllStatus.AVERAGE_CLOSURE.Length - 1);
                    }
                }               
            }
            else
            {
                taskDashboardResponse.TaskOverAllStatus = new TaskOverAllStatus();    
            }
            SqlParameter _practiceCode6 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _groupIds6 = new SqlParameter { ParameterName = "GROUP_IDs", SqlDbType = SqlDbType.VarChar, Value = req.GROUP_IDs ?? (object)DBNull.Value };
            SqlParameter _taskTypeIds6 = new SqlParameter { ParameterName = "TASK_TYPE_IDs", SqlDbType = SqlDbType.VarChar, Value = req.TASK_TYPE_IDs ?? (object)DBNull.Value };
            SqlParameter _dateFrom6 = Helper.getDBNullOrValue("@DATE_FROM", req.DATE_FROM.ToString() ?? "");
            SqlParameter _dateTo6 = Helper.getDBNullOrValue("@DATE_TO", req.DATE_TO.ToString() ?? "");
            SqlParameter _timeFrame6 = new SqlParameter { ParameterName = "TIME_FRAME", SqlDbType = SqlDbType.VarChar, Value = req.TIME_FRAME ?? (object)DBNull.Value };
            var taskTypes = SpRepository<TaskTypes>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_TYPE_NAMES_FOR_DASHBOARD]
                        @PRACTICE_CODE, @GROUP_IDs, @TASK_TYPE_IDs, @DATE_FROM, @DATE_TO, @TIME_FRAME", _practiceCode6, _dateFrom6, _dateTo6, _groupIds6, _taskTypeIds6, _timeFrame6);

            if (req.TIME_FRAME == "LAST_THREE_MONTHS")
            {
                DateTime Today = DateTime.Today;
                DateTime startDate = (new DateTime(Today.Year, Today.Month, 1)).AddMonths(-3);
                DateTime endDate = startDate.AddMonths(3).AddSeconds(-1);
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateList.Add(date.ToString());
                }
            }

            if (req.TIME_FRAME == "YESTERDAY")
            {
                DateTime startDate = DateTime.Now.AddDays(-1);
                DateTime endDate = DateTime.Now;
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateList.Add(date.ToString());
                }
            }

            if (req.TIME_FRAME == "TODAY")
            {
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateList.Add(date.ToString());
                }
            }

            if(req.TIME_FRAME == "THIS_WEEK")
            {
                DateTime startDate = (DateTime.Today.AddDays(
                (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek -
                (int)DateTime.Today.DayOfWeek)).AddDays(1);

                DateTime endDate = DateTime.Now;
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateList.Add(date.ToString());
                }
            }

            if (req.TIME_FRAME == "THIS_MONTH")
            {
                DateTime Today = DateTime.Today;
                DateTime startDate = new DateTime(Today.Year, Today.Month, 1);
                DateTime endDate = DateTime.Now;
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateList.Add(date.ToString());
                }
            }

            if (req.TIME_FRAME == "LAST_MONTH")
            {
                DateTime Today = DateTime.Today;
                DateTime startDate = (new DateTime(Today.Year, Today.Month, 1)).AddMonths(-1);
                DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateList.Add(date.ToString());
                }
            }

            if (req.TIME_FRAME == "DATE_RANGE")
            {
                DateTime startDate = Convert.ToDateTime(req.DATE_FROM_STR);
                DateTime endDate = Convert.ToDateTime(req.DATE_TO_STR);
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateList.Add(date.ToString());
                }
            }
            var maindictionary = new Dictionary<string, Dictionary<string, string>>();
            int countlist = 0;
            foreach (string date in dateList)
            {                
                req.CREATED_DATE = date;
                SqlParameter _practiceCode5 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                SqlParameter _groupIds5 = new SqlParameter { ParameterName = "GROUP_IDs", SqlDbType = SqlDbType.VarChar, Value = req.GROUP_IDs ?? (object)DBNull.Value };
                SqlParameter _taskTypeIds5 = new SqlParameter { ParameterName = "TASK_TYPE_IDs", SqlDbType = SqlDbType.VarChar, Value = req.TASK_TYPE_IDs ?? (object)DBNull.Value };
                SqlParameter _createdDate5 = new SqlParameter { ParameterName = "CREATED_DATE", SqlDbType = SqlDbType.DateTime, Value = req.CREATED_DATE.ToString() ?? "" };
                SqlParameter _dateTos5 = new SqlParameter { ParameterName = "DATE_TO", SqlDbType = SqlDbType.DateTime, Value = req.CREATED_DATE.ToString() ?? "" };
                SqlParameter _timeFrame5 = new SqlParameter { ParameterName = "TIME_FRAME", SqlDbType = SqlDbType.VarChar, Value = req.TIME_FRAME ?? (object)DBNull.Value };
                var createdTasktypesdata = SpRepository<CreatedTaskTypedata>.GetListWithStoreProcedure(@"exec [FOX_PROC_CREATED_TASK_TYPE_DATA_ALTERNATIVE] @PRACTICE_CODE, @GROUP_IDs, @TASK_TYPE_IDs, @CREATED_DATE", _practiceCode5, _groupIds5, _taskTypeIds5, _createdDate5);
                    var dict = new Dictionary<string, string>();
                string dateInString = Convert.ToDateTime(date).ToString();
                dict.Add("Date", dateInString);
                if (createdTasktypesdata  != null && createdTasktypesdata.Count != 0 && taskTypes != null && taskTypes.Count != 0)
                {
                    foreach (TaskTypes taskType in taskTypes)
                    {
                        var havetype = createdTasktypesdata.Find(x => x.TASK_TYPE_NAME == taskType.NAME);
                        if(havetype != null)
                        {
                            dict.Add(havetype.TASK_TYPE_NAME, havetype.TASK_COUNT.ToString());
                        }
                        else
                        {
                            dict.Add(taskType.NAME, "0");
                        }
                    }
                }
                else
                {
                    foreach (TaskTypes taskType in taskTypes)
                    {
                        dict.Add(taskType.NAME, "0");
                    }
                }
                maindictionary.Add(countlist.ToString(), dict);
                countlist = countlist + 1;
            }
            var output = Newtonsoft.Json.JsonConvert.SerializeObject(maindictionary);
            taskDashboardResponse.TaskTypeDashboardDataString = output;
            return taskDashboardResponse;
        }
        public List<FOX_TBL_NOTIFICATIONS> GetTasksNotifications(NotificationRequestModel req, UserProfile profile)
        {

            switch (req.TIME_FRAME)
            {
                case 1:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    req.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 2:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    req.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 3:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    req.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(req.DATE_FROM_STR))
                        req.DATE_FROM = Convert.ToDateTime(req.DATE_FROM_STR);
                    else
                        req.DATE_FROM = Helper.GetCurrentDate().AddYears(-1);
                    if (!string.IsNullOrEmpty(req.DATE_TO_STR))
                        req.DATE_TO = Convert.ToDateTime(req.DATE_TO_STR);
                    else
                    {
                        req.DATE_TO = Helper.GetCurrentDate();
                    }
                    break;
                case 5:
                    req.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    req.DATE_TO = Helper.GetCurrentDate();
                    break;
                default:
                    break;
            }

            var pracCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var userid = new SqlParameter { ParameterName = "@USER_ID", SqlDbType = SqlDbType.BigInt, Value = profile.userID };
            var dateFrom = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.ToString());
            var dateTo = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.ToString());


            var result = SpRepository<FOX_TBL_NOTIFICATIONS>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_NOTIFICATIONS_OF_TASKS @USER_ID, @PRACTICE_CODE, @DATE_FROM, @DATE_TO",
                    userid, pracCode, dateFrom, dateTo);
            return result;
        }

        public ListResponseModel GetTasksNotificationsList(NotificationRequestModel req, UserProfile profile)
        {

            switch (req.TIME_FRAME)
            {
                case 1:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    req.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 2:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    req.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 3:
                    req.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    req.DATE_TO = Helper.GetCurrentDate();
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(req.DATE_FROM_STR))
                        req.DATE_FROM = Convert.ToDateTime(req.DATE_FROM_STR);
                    else
                        req.DATE_FROM = Helper.GetCurrentDate().AddYears(-1);
                    if (!string.IsNullOrEmpty(req.DATE_TO_STR))
                        req.DATE_TO = Convert.ToDateTime(req.DATE_TO_STR);
                    else
                    {
                        req.DATE_TO = Helper.GetCurrentDate();
                    }
                    break;
                case 5:
                    req.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    req.DATE_TO = Helper.GetCurrentDate();
                    break;
                default:
                    break;
            }

            var pracCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var userid = new SqlParameter { ParameterName = "@USER_ID", SqlDbType = SqlDbType.BigInt, Value = profile.userID };
            var dateFrom = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.ToString());
            var dateTo = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.ToString());


            var result = SpRepository<string>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_NOTIFICATIONS_OF_TASKS_LIST @USER_ID, @PRACTICE_CODE, @DATE_FROM, @DATE_TO",
                    userid, pracCode, dateFrom, dateTo);
            ListResponseModel response = new ListResponseModel();
            response.DATE = result;
            response.NotificationList = new List<List<FOX_TBL_NOTIFICATIONS>>();
            var allData = GetTasksNotifications(req, profile);

            foreach (var item in response.DATE)
            {
                var list = allData.FindAll(x => x.SENT_ON_STR == item.ToString());
                response.NotificationList.Add(list);

            }
            return response;
        }
        public bool DeleteNotification(long ID, UserProfile profile)
        {
            var id = new SqlParameter { ParameterName = "@FOX_NOTIFICATION_ID", SqlDbType = SqlDbType.BigInt, Value = ID };
            var user = Helper.getDBNullOrValue("@USER",profile.UserName);

            var result = SpRepository<FOX_TBL_NOTIFICATIONS>.GetListWithStoreProcedure(@"exec FOX_PROC_DELETE_NOTIFICATIONS @FOX_NOTIFICATION_ID, @USER", id, user);
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}