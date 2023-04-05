using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.IndexedQueueModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FOX.DataModels.Models.TasksModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.IndexedQueueService
{
    public class IndexedQueueServices : IIndexedQueueServices
    {
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly GenericRepository<Referral_Assignment_details> _AssignmentQueueRepository;
        private readonly GenericRepository<WorkTransfer> _WorkTransferRepository;
        private readonly GenericRepository<User> _userRepository;
        private readonly GenericRepository<RoleToAdd> _roleRepository;
        static long retrycatch = 0;
        public IndexedQueueServices()
        {
            retrycatch = 0;
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _AssignmentQueueRepository = new GenericRepository<Referral_Assignment_details>(_QueueContext);
            _WorkTransferRepository = new GenericRepository<WorkTransfer>(_QueueContext);
            _userRepository = new GenericRepository<User>(_QueueContext);
            _roleRepository = new GenericRepository<RoleToAdd>(_QueueContext);
        }
        public List<FilePages> GetFilePages(IndexedQueueFileRequest req)
        {

            var work_id = new SqlParameter("WORK_ID ", SqlDbType.BigInt) { Value = req.WORK_ID };
            //var ID = new SqlParameter("id ", SqlDbType.VarChar) { Value = req.id };
            var result = SpRepository<FilePages>.GetListWithStoreProcedure(@"exec FOX_GET_File_PAGES @WORK_ID", work_id
                       );
            return result;
        }

        public string SetSplitPages(List<FilePages> req, UserProfile profile)
        {
            var result = "";
            OriginalQueueService.OriginalQueueService os = new OriginalQueueService.OriginalQueueService();
            int numberOfPages = req.Count(x => x.Checked == true);


            // select * from FOX_TBL_WORK_QUEUE where UNIQUE_ID = '5447928_2'

            string uqId = req?.FirstOrDefault().UNIQUE_ID ?? "";
            var queue = _QueueRepository.GetFirst(t => t.UNIQUE_ID.Equals(uqId));
            long CurrrentParentID = queue?.WORK_ID ?? 0;

            var workID = req.FirstOrDefault().WORK_ID;
            var a = req.FirstOrDefault().UNIQUE_ID;
            if (a.Contains("_"))
            {
                var arr = a.Split('_').ToArray();
                workID = Convert.ToInt64(arr[0]);
            }

            var workQueue = os.SaveQueueFromOldQueueData(workID, profile, numberOfPages, CurrrentParentID);


            foreach (var item in req)
            {
                if (item.Checked == true)
                {
                    var uniueId = string.IsNullOrEmpty(workQueue?.UNIQUE_ID) ? "0" : workQueue?.UNIQUE_ID;
                    var NEWWORKID = workQueue?.WORK_ID == null ? 0 : workQueue?.WORK_ID;
                    var fileid = Helper.getMaximumId("FOXREHAB_FILE_ID");
                    var FILE_ID = new SqlParameter("@FILE_ID ", SqlDbType.BigInt) { Value = fileid };
                    var UNIQUE_ID = new SqlParameter("@UNIQUE_ID ", SqlDbType.VarChar) { Value = uniueId };
                    //  var UNIQUE_ID_1 = new SqlParameter("UNIQUE_ID_1", SqlDbType.VarChar) { Value = item.UNIQUE_ID };
                    var File_Path_1 = new SqlParameter("@File_Path_1", SqlDbType.VarChar) { Value = item.file_path1 };
                    var File_path = new SqlParameter("@File_path", SqlDbType.VarChar) { Value = item.FILE_PATH };
                    var NEW_WORK_ID = new SqlParameter("@NEW_WORK_ID ", SqlDbType.BigInt) { Value = NEWWORKID };
                    var OLD_Work_ID = new SqlParameter("@Old_work_id ", SqlDbType.BigInt) { Value = item.WORK_ID };
                    result = SpRepository<string>.GetSingleObjectWithStoreProcedure(
                        @"exec Fox_Queue_Split_Document  @FILE_ID,@UNIQUE_ID,@File_Path_1,@File_path,@NEW_WORK_ID,@Old_work_id",
                        FILE_ID, UNIQUE_ID, File_Path_1, File_path, NEW_WORK_ID, OLD_Work_ID
                    );
                }
            }

            return result;
        }

        public List<IndexedQueue> GetIndexedQueue(IndexedQueueRequest req, UserProfile Profile)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
            var parmIncludeArchive = new SqlParameter("INCLUDE_ARCHIVE", SqlDbType.Bit) { Value = req.IncludeArchive };
            var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RecordPerPage };
            var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText };
            var parmSourceType = new SqlParameter("SORCE_TYPE", SqlDbType.VarChar) { Value = req.SorceType };
            var parmIndexedBy = new SqlParameter("INDEXED_BY", SqlDbType.VarChar) { Value = req.IndexedBy };
            var parmSortBy = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SortBy };
            var parmSortOrder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SortOrder };
            var parmUserName = new SqlParameter("USER_NAME", SqlDbType.VarChar) { Value = Profile.UserName };
            var queue = SpRepository<IndexedQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_INDEXED_QUEUE  @PRACTICE_CODE, @CURRENT_PAGE, @INCLUDE_ARCHIVE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORCE_TYPE, @INDEXED_BY, @SORT_BY,@SORT_ORDER,@USER_NAME",
                        parmPracticeCode, parmCurrentPage, parmIncludeArchive, parmRecordsPerPage, parmSearchText, parmSourceType, parmIndexedBy, parmSortBy, parmSortOrder, parmUserName);
            return queue;
        }
        public string ExportToExcelIndexedQueue(IndexedQueueRequest req, UserProfile profile)
        {
            try
            {
                string fileName = "Indexed_Queue_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CurrentPage = 1;
                req.RecordPerPage = 0;
                var CalledFrom = "Indexed_Queue";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<IndexedQueue> result = new List<IndexedQueue>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetIndexedQueue(req, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<IndexedQueue>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UsersForDropdown> GetAgentsForDropdown(UserProfile Profile)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var queue = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_AGENTS_AND_SUPERVISOR  @PRACTICE_CODE", parmPracticeCode);
            return queue;

        }

        public List<UsersForDropdown> GetAgentsAndSupervisorsForDropdown(long RoleId, long practiceCode, string userName)
        {
            // var current_user = Profile.UserName;
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var current_user = new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = userName };
            var role_id = new SqlParameter("@ROLE_ID", SqlDbType.BigInt) { Value = RoleId };
            var queue = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_AGENTS_AND_SUPERVISOR  @PRACTICE_CODE ,@CURRENT_USER, @ROLE_ID", parmPracticeCode, current_user, role_id);
            return queue;

        }

        public void ReAssignedMultiple(UserProfile Profile, List<IndexedQueue> ReAssignedList)
        {
            foreach (var item in ReAssignedList)
            {
                long? RoleID = 0;
                string AssignToDesign = "";
                string AssignByDesignation = "";
                var workQueue = GetQueueById(item.WORK_ID);
                //FOX_PROC_UPDATE_WORK_QUEUE
                //               @WORK_ID BIGINT NULL,
                //@USER_NAME VARCHAR(70) NULL,
                //   @ASSIGNED_TO VARCHAR(70) NULL,
                //   @WORK_STATUS VARCHAR(50) NULL,
                //   @UPDATE_WORK_STATUS BIT NULL,
                //   @UPDATE_INDEX_DATE BIT NULL,
                //   @UPDATE_AGENT_DATE BIT NULL
                SqlParameter workId = new SqlParameter("WORK_ID", item.WORK_ID);
                SqlParameter userName = new SqlParameter("USER_NAME", Profile.UserName);
                SqlParameter assignedTo = new SqlParameter("ASSIGNED_TO", item.RE_ASSIGNED_TO);
                SqlParameter workStatus = new SqlParameter("WORK_STATUS", string.Empty);
                SqlParameter updateWorkStatus = new SqlParameter("UPDATE_WORK_STATUS", false);
                SqlParameter updateIndexDate = new SqlParameter("UPDATE_INDEX_DATE", false);
                SqlParameter updateAgentDate = new SqlParameter("UPDATE_AGENT_DATE", false);
                SqlParameter supervisor_status = new SqlParameter("SUPERVISOR_STATUS", false);
                if (workQueue != null)
                {
                    var prevAssignedTo = workQueue.ASSIGNED_TO;
                    //workQueue.MODIFIED_DATE = Helper.GetCurrentDate();
                    //workQueue.MODIFIED_BY = Profile.UserName;
                    //workQueue.ASSIGNED_TO = item.RE_ASSIGNED_TO;
                    //workQueue.ASSIGNED_BY = Profile.UserName;
                    //workQueue.ASSIGNED_DATE = Helper.GetCurrentDate();
                    if (!(string.IsNullOrEmpty(workQueue.ASSIGNED_TO) && string.IsNullOrEmpty(item.RE_ASSIGNED_TO)))
                    {
                        RoleID = GetUserRole(Profile, item.RE_ASSIGNED_TO)?.ROLE_ID ?? 0;// _userRepository.GetFirst(x => x.USER_NAME == workQueue.ASSIGNED_TO).ROLE_ID;
                        AssignToDesign = RoleID.HasValue ? GetRoleById(RoleID.Value)?.ROLE_NAME : string.Empty ?? string.Empty; //_roleRepository.GetFirst(x => x.ROLE_ID == RoleID).ROLE_NAME;
                        AssignByDesignation = GetRoleById(Profile.RoleId)?.ROLE_NAME ?? string.Empty;
                        if (AssignToDesign.ToLower().Equals("INDEXER".ToLower()))
                        {
                            updateIndexDate.Value = true;
                            workQueue.INDEXER_ASSIGN_DATE = Helper.GetCurrentDate();
                            workQueue.supervisor_status = false;
                        }
                        if (AssignToDesign.ToLower().Equals("AGENT".ToLower()))
                        {
                            updateAgentDate.Value = true;
                            workQueue.AGENT_ASSIGN_DATE = Helper.GetCurrentDate();
                        }
                        if (AssignToDesign.ToLower().Equals("Supervisor".ToLower()))
                        {
                            supervisor_status.Value = true;
                        }
                    }
                    if (string.IsNullOrEmpty(prevAssignedTo))
                    {
                        updateWorkStatus.Value = true;
                        workStatus.Value = "Index Pending";
                        ///workQueue.WORK_STATUS = "Index Pending";//when moved from unassigned to assigned
                        ///workQueue.INDEXER_ASSIGN_DATE = Helper.GetCurrentDate();
                    }

                    InsertAssignmentData(workQueue.ASSIGNED_BY, workQueue.ASSIGNED_TO, AssignToDesign, AssignByDesignation, item.WORK_ID, Profile);
                    SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_UPDATE_WORK_QUEUE @WORK_ID, @USER_NAME, @ASSIGNED_TO, @WORK_STATUS, @UPDATE_WORK_STATUS, @UPDATE_INDEX_DATE, @UPDATE_AGENT_DATE, @SUPERVISOR_STATUS", workId, userName, assignedTo, workStatus, updateWorkStatus, updateIndexDate, updateAgentDate, supervisor_status);
                    //_QueueRepository.Update(workQueue);
                    //_QueueRepository.Save();

                    //Log Changes
                    string logMsg = "";
                    if (string.IsNullOrEmpty(prevAssignedTo))
                        logMsg = string.Format("ID: {0} has been assigned to {1}.", workQueue.UNIQUE_ID, Helper.GetFullName(item.RE_ASSIGNED_TO));
                    else
                        logMsg = string.Format("ID: {0} has been re-assigned to {1}.", workQueue.UNIQUE_ID, Helper.GetFullName(item.RE_ASSIGNED_TO));

                    string user = !string.IsNullOrEmpty(Profile.FirstName) ? Profile.FirstName + " " + Profile.LastName : Profile.UserName;
                    Helper.LogSingleWorkOrderChange(workQueue.WORK_ID, workQueue.UNIQUE_ID, logMsg, user);
                }
            }
        }
        public void InsertAssignmentData(string ASSIGNED_BY, string ASSIGNED_TO, string AssignToDesignation, string AssignByDesignation, long WORK_ID, UserProfile Profile)
        {
            try
            {
                long Pid = Helper.getMaximumId("FOX_REFRRAL_ASSIGNMENT_ID");
                SqlParameter id = new SqlParameter("ID", Pid);
                SqlParameter workId = new SqlParameter("WORK_ID", WORK_ID);
                SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", Profile.PracticeCode);
                SqlParameter assignedBy = new SqlParameter("ASSIGNED_BY", string.IsNullOrEmpty(ASSIGNED_BY) ? string.Empty : ASSIGNED_BY);
                SqlParameter assignedByDesignation = new SqlParameter("ASSIGNED_BY_DESIGNATION", string.IsNullOrEmpty(AssignByDesignation) ? string.Empty : AssignByDesignation);
                SqlParameter assignedTo = new SqlParameter("ASSIGNED_TO", string.IsNullOrEmpty(ASSIGNED_TO) ? string.Empty : ASSIGNED_TO);
                SqlParameter assignedToDesignation = new SqlParameter("ASSIGNED_TO_DESIGNATION", string.IsNullOrEmpty(AssignToDesignation) ? string.Empty : AssignToDesignation);
                SqlParameter userName = new SqlParameter("USER_NAME", Profile.UserName);
                SpRepository<Referral_Assignment_details>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_REFERRAL_ASSIGNMENT_DETAILS  @ID, @WORK_ID, @PRACTICE_CODE, @ASSIGNED_BY, @ASSIGNED_BY_DESIGNATION, @ASSIGNED_TO, @ASSIGNED_TO_DESIGNATION, @USER_NAME",
                                                                                                                                  id, workId, practiceCode, assignedBy, assignedByDesignation, assignedTo, assignedToDesignation, userName);
            }
            catch (Exception ex)
            {
                if (retrycatch <= 2 && !string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    retrycatch = retrycatch + 1;
                    InsertAssignmentData(ASSIGNED_BY, ASSIGNED_TO, AssignToDesignation, AssignByDesignation, WORK_ID, Profile);
                }
                else
                {
                    throw ex;
                }
            }
            //Referral_Assignment_details obj = new Referral_Assignment_details();
            //obj.FOX_REFRRAL_ASSIGNMENT_ID = Helper.getMaximumId("FOX_REFRRAL_ASSIGNMENT_ID");
            //obj.WORK_ID = WORK_ID;
            //obj.PRACTICE_CODE = Profile.PracticeCode;
            //obj.ASSIGNED_TIME = Helper.GetCurrentDate();
            //obj.ASSIGNED_BY = ASSIGNED_BY;
            //obj.ASSIGNED_BY_DESIGNATION = AssignByDesignation;
            //obj.ASSIGNED_TO = ASSIGNED_TO;
            //obj.ASSIGNED_TO_DESIGNATION = AssignToDesignation;
            //obj.CREATED_BY = Profile.UserName;
            //obj.CREATED_DATE = Helper.GetCurrentDate();
            //obj.MODIFIED_BY = Profile.UserName;
            //obj.MODIFIED_DATE = Helper.GetCurrentDate();
            //obj.DELETED = false;
            //_AssignmentQueueRepository.Insert(obj);
            //_AssignmentQueueRepository.Save();
        }
        public void AddUpdateWorkTransfer(WorkTransfer workTransfer, UserProfile Profile)
        {
            workTransfer.WORK_TRANFER_ID = Helper.getMaximumId("WORK_TRANFER_ID");
            workTransfer.TRANSFER_BY = Profile.UserName;
            workTransfer.CREATED_BY = Profile.UserName;
            workTransfer.CREATED_DATE = Helper.GetCurrentDate();
            workTransfer.MODIFIED_BY = Profile.UserName;
            workTransfer.MODIFIED_DATE = Helper.GetCurrentDate();
            workTransfer.DELETED = false;

            _WorkTransferRepository.Insert(workTransfer);
            _WorkTransferRepository.Save();
        }

        private RoleToAdd GetRoleById(long roleId)
        {
            SqlParameter role_Id = new SqlParameter("ROLE_ID", roleId);
            return SpRepository<RoleToAdd>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_ROLE_BY_ID @ROLE_ID", role_Id);
        }

        private RoleToAdd GetUserRole(UserProfile profile, string userName)
        {
            SqlParameter user_Name = new SqlParameter("USER_NAME", userName);
            SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            return SpRepository<RoleToAdd>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_USER_ROLE @USER_NAME, @PRACTICE_CODE", user_Name, practiceCode);
        }

        private OriginalQueue GetQueueById(long workId)
        {
            SqlParameter work_Id = new SqlParameter("WORK_ID", workId);
            return SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_WORKQUEUE_BY_ID @WORK_ID", work_Id);
        }

        //public string Export(IndexedQueueRequest obj, UserProfile profile)
        //{
        //    try
        //    {
        //        string fileName = "Indexed_Queue";
        //        string exportPath = "";
        //        string path = string.Empty;
        //        bool exported;
        //      // obj.CurrentPage = 1;
        //    //    obj.RecordPerPage = 0;
        //        //obj.CalledFrom = "Analysis_Report";
        //        string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + " Fox/ExportedFiles/";
        //        exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
        //        fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
        //        if (!Directory.Exists(exportPath))
        //        {
        //            Directory.CreateDirectory(exportPath);
        //        }
        //        var pathtowriteFile = exportPath + "\\" + fileName;
        //        switch (obj.CalledFrom)
        //        {
        //            #region RBS Blocked Claims
        //            case "Indexed_Queue":
        //                {
        //                    // List<IndexedQueue> GetIndexedQueue(IndexedQueueRequest req, UserProfile Profile)
        //                    var result = GetIndexedQueue(obj, profile);
        //                    exported = ExportToExcel.CreateExcelDocument<IndexedQueue>(result, pathtowriteFile, obj.CalledFrom.Replace(' ', '_'));
        //                    break;
        //                }
        //                #endregion
        //        }
        //        return virtualPath + fileName;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}