using FOX.BusinessOperations.SettingsService.UserMangementService;
using FOX.BusinessOperations.TaskServices;
using FOX.DataModels.Models.TasksModel;
using FoxRehabilitationAPI.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class TasksController : BaseApiController
    {
        // GET: Tasks
        private readonly ITaskServices _TaskServices;
        private readonly IUserManagementService _userServices;

        public TasksController(ITaskServices TaskServices, IUserManagementService userServices)
        {
            _TaskServices = TaskServices;
            _userServices = userServices;
        }

        [HttpGet]
        public HttpResponseMessage GetAllCasesAndTaskType(string patient_Account)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetAllCasesAndTaskType(patient_Account, profile.PracticeCode));
        }
        

        [HttpGet]
        public HttpResponseMessage GetAllTaskType()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetAllTaskType(profile.PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateTasks(FOX_TBL_TASK task)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.AddUpdateTask(task, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage AddEditTaskSubTypes(FOX_TBL_TASK_SUB_TYPE model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.AddEditTaskSubType(model, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage DeleteTaskSubTypes(FOX_TBL_TASK_SUB_TYPE model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.DeleteTaskSubType(model, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetTaskById(long taskId)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetTaskById(taskId, profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetTaskByTaskId(long taskId)
        {
            var result = _TaskServices.GetTaskByTaskId(taskId, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetTaskByGeneralNoteId(long generalNoteId)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetTaskByGeneralNoteId(generalNoteId, profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetTask(int taskTypeId)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetTask(taskTypeId, profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetTaskTypeList()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetTaskTypeList(profile.PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetInactiveTaskTypeList()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetInactiveTaskTypeList(profile.PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetSubTaskTypeList(int taskTypeId)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetSubTaskTypeList(taskTypeId, profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetDropDownData()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetDropDownData(profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetTTUserList(string searchText)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetTTUserList(searchText, profile.PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage TaskDetailList(TaskSearchRequest taskSearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetTaskDetailList(taskSearchRequest, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage getcategoryandfield()
        {
            var profile = GetProfile();
            var result = _TaskServices.getcategoryandfield(profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetProviderList(string searchText)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetProviderList(searchText, profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetActiveLocationList(string searchText)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetActiveLocationList(searchText, profile.PracticeCode));
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetUserAuthenticate(string userName, string password)
        {
            var user = await UserManager.FindAsync(userName, password);
            return Request.CreateResponse(HttpStatusCode.OK, user == null ? false : true);
        }

        [HttpPost]
        public HttpResponseMessage SaveFilterRec(GetCategoryFieldResp FileteredFieldList)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.SaveFilterTemplateRecord(FileteredFieldList, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage getcategoryfield([FromBody]getCatFieldReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetCategoryFields(obj, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetUsersGroupList(string searchText)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetUsersGroupList(searchText, GetProfile().PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetUsersList(string searchText)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetUsersList(searchText, GetProfile().PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetTask(string patientAccount, string caseId, string taskTypeId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetTask(Convert.ToInt64(patientAccount), Convert.ToInt64(caseId), Convert.ToInt64(taskTypeId), GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage ExportToExcel(TaskSearchRequest taskSearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.ExportToExcel(taskSearchRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage SetTaskTypeList(FOX_TBL_TASK_TYPE model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.SetTaskTypeBit(model, GetProfile()));
         
        }
        [HttpPost]
        public HttpResponseMessage GetTaskDashboardData(TaskDashboardSearchRequest model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _TaskServices.GetTaskDashBoardData(model, GetProfile()));
        }
    }
}