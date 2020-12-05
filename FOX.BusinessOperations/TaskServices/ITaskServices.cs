//System Namespaces.
using System.Collections.Generic;

//Portal Namespaces.
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.TasksModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Settings.ReferralSource;

namespace FOX.BusinessOperations.TaskServices
{
    public interface ITaskServices
    {
        CaseTaskTypeList GetAllCasesAndTaskType(string patient_Account, long practiceCode);

        List<FOX_TBL_TASK_TYPE> GetAllTaskType(long practiceCode);
        FOX_TBL_TASK AddUpdateTask(FOX_TBL_TASK task, UserProfile profile);

        FOX_TBL_TASK_SUB_TYPE AddEditTaskSubType(FOX_TBL_TASK_SUB_TYPE model, UserProfile profile);

        ResponseModel DeleteTaskSubType(FOX_TBL_TASK_SUB_TYPE model, UserProfile profile);
        ResponseModel SetTaskTypeBit(FOX_TBL_TASK_TYPE model, UserProfile profile);
        TaskWithHistory GetTaskById(long taskId, long practiceCode);

        GetTaskTemplateResponse GetTask(int taskTypeId, long practiceCode);

        GetTaskTemplateInitialData GetTaskTypeList(long practiceCode);

        List<FOX_TBL_TASK_SUB_TYPE> GetSubTaskTypeList(int taskTypeId, long practiceCode);

        DropDownData GetDropDownData(long PracticeCode);

        List<User> GetTTUserList(string searchText, long practiceCode);

        categoriesData getcategoryandfield(UserProfile profile);

        List<TaskDetail> GetTaskDetailList(TaskSearchRequest taskSearchRequest, UserProfile profile);

        List<ReferralSource> GetProviderList(string searchText, long practiceCode);

        List<ActiveLocation> GetActiveLocationList(string searchText, long practiceCode);

        bool AuthenticateUser(string userName, string password);

        ResponseModel SaveFilterTemplateRecord(GetCategoryFieldResp CatModel, UserProfile profile);

        GetCategoryFieldResp GetCategoryFields(getCatFieldReq obj, UserProfile profile);

        TaskWithHistory GetTaskByGeneralNoteId(long generalNoteId, long practiceCode);

        TaskWithHistory GetTaskByTaskId(long taskId, UserProfile profile);

        List<UserAndGroup> GetUsersGroupList(string searchText, long practiceCode);
        List<UserAndGroup> GetUsersList(string searchText, long practiceCode);

        TaskWithHistory GetTask(long patientAccount, long caseId, long taskTypeId, UserProfile profile);
        List<FOX_TBL_TASK_TYPE> GetInactiveTaskTypeList(long practiceCode);
        string ExportToExcel(TaskSearchRequest taskSearchRequest, UserProfile profile);
        TaskDashboardResponse GetTaskDashBoardData(TaskDashboardSearchRequest req, UserProfile profile);
    }
}
