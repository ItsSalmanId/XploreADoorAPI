using FOX.DataModels.Models.IndexedQueueModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using System.Collections.Generic;

namespace FOX.BusinessOperations.IndexedQueueService
{
    public interface IIndexedQueueServices
    {
        List<IndexedQueue> GetIndexedQueue(IndexedQueueRequest req, UserProfile Profile);
        string ExportToExcelIndexedQueue(IndexedQueueRequest req, UserProfile profile);
        //List<UsersForDropdown> GetAgentsForDropdown(UserProfile Profile);
        List<UsersForDropdown> GetAgentsAndSupervisorsForDropdown(long RoleId, long practiceCode, string userName);
        void ReAssignedMultiple(UserProfile Profile, List<IndexedQueue> ReAssignedList);
        List<FilePages> GetFilePages(IndexedQueueFileRequest req);
        string SetSplitPages(List<FilePages> req, UserProfile profile);
        void AddUpdateWorkTransfer(WorkTransfer workTransfer, UserProfile Profile);
        //string Export(IndexedQueueRequest obj, UserProfile profile);
        void InsertAssignmentData(string ASSIGNED_BY, string ASSIGNED_TO, string AssignToDesignation, string AssignByDesignation, long WORK_ID, UserProfile Profile);
    }
}