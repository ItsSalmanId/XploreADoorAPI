using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.AssignedQueueModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.AssignedQueueService
{
    public class AssignedQueueServices : IAssignedQueueServices
    {
        private readonly DBContextQueue _queueContext = new DBContextQueue();
        private readonly GenericRepository<OriginalQueue> _queueRepository;
        private readonly GenericRepository<BlacklistedWhitelistedSource> _blacklistedWhitelistedSourceRepository;
        //private readonly GenericRepository<WorkOrderAddtionalInfo> _workOrderAddtionalInfoRepository;


        public AssignedQueueServices()
        {
            _queueRepository = new GenericRepository<OriginalQueue>(_queueContext);
            _blacklistedWhitelistedSourceRepository = new GenericRepository<BlacklistedWhitelistedSource>(_queueContext);
            //_workOrderAddtionalInfoRepository = new GenericRepository<WorkOrderAddtionalInfo>(_queueContext);
        }

        public List<AssignedQueue> GetIndexedQueue(AssignedQueueRequest req, UserProfile Profile)
        {
            //var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            //var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
            //var parmIncludeArchive = new SqlParameter("INCLUDE_ARCHIVE", SqlDbType.Bit) { Value = req.IncludeArchive };
            //var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RecordPerPage };
            //var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText };
            //var parmSourceType = new SqlParameter("SORCE_TYPE", SqlDbType.VarChar) { Value = req.SorceType };
            //var parmIndexedBy = new SqlParameter("INDEXED_BY", SqlDbType.VarChar) { Value = req.IndexedBy };
            //var parmSortBy = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SortBy };
            //var parmSortOrder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SortOrder };
            //var queue = SpRepository<AssignedQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ASSIGNED_QUEUE  @PRACTICE_CODE, @CURRENT_PAGE, @INCLUDE_ARCHIVE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORCE_TYPE, @INDEXED_BY, @SORT_BY,@SORT_ORDER",
            //            parmPracticeCode, parmCurrentPage, parmIncludeArchive, parmRecordsPerPage, parmSearchText, parmSourceType, parmIndexedBy, parmSortBy, parmSortOrder);
            //return queue;
            if (req.reportUser != "" && req.reportUser != null)
            {
                if (!string.IsNullOrEmpty(req.DATE_FROM_STR))
                    req.DATE_FROM = Convert.ToDateTime(req.DATE_FROM_STR);
                if (!string.IsNullOrEmpty(req.DATE_TO_STR))
                    req.DATE_TO = Convert.ToDateTime(req.DATE_TO_STR);
                if (req.DATE_FROM.HasValue)
                    if (req.DATE_TO.HasValue)
                        if (String.Equals(req.DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                            req.DATE_TO = req.DATE_TO.Value.AddDays(1).AddSeconds(-1);
                        else
                            req.DATE_TO = req.DATE_TO.Value.AddSeconds(59);
                    else
                        req.DATE_TO = Helper.GetCurrentDate();
                else if (req.DATE_TO.HasValue)
                {
                    if (String.Equals(req.DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                        req.DATE_TO = req.DATE_TO.Value.AddDays(1).AddSeconds(-1);
                    var dateNow = Helper.GetCurrentDate();
                    req.DATE_TO = dateNow.AddYears(-100);

                }
                Profile.UserName = req.reportUser;
            }

            if (req.WorkID == null)
                req.WorkID = "";
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
            var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RecordPerPage };
            var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText };
            var parmSourceType = new SqlParameter("SORCE_TYPE", SqlDbType.VarChar) { Value = req.SorceType };
            var parmSorceName = new SqlParameter("SORCE_NAME", SqlDbType.VarChar) { Value = req.SorceName };
            var parmSortBy = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SortBy };
            var parmSortOrder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SortOrder };
            var parmWorkId = new SqlParameter("WORK_ID", SqlDbType.VarChar) { Value = req.WorkID };
            var parmAssignTo = new SqlParameter("ASSIGNED_TO", SqlDbType.VarChar) { Value = req.AssignTo };
            var parmStatus = new SqlParameter("STATUS", SqlDbType.VarChar) { Value = req.Status??"" };
            var USER_NAME = new SqlParameter("USER_NAME", SqlDbType.VarChar) { Value = Profile.UserName };
            var dATE_FROM = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.HasValue ? req.DATE_FROM.Value.ToString() : "");
            var dATE_TO = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.HasValue ? req.DATE_TO.Value.ToString() : "");
            var parmIncludeArchive = new SqlParameter("INCLUDE_ARCHIVE", SqlDbType.Bit) { Value = req.IncludeArchive };
            var parmIsTrash = new SqlParameter("Is_Trash", SqlDbType.Bit) { Value = req.Is_Trash };

            if (req.reportUser != "" && req.reportUser != null)
            {
                var queue1 = SpRepository<AssignedQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ASSIGNED_QUEUE_TASKDATA @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT,@SORT_BY,@SORT_ORDER,@USER_NAME,@DATE_FROM,@DATE_TO,@IS_TRASH",
            parmPracticeCode, parmCurrentPage, parmRecordsPerPage, parmSearchText, parmSortBy, parmSortOrder, USER_NAME, dATE_FROM, dATE_TO, parmIsTrash);
                 return queue1;
            }
            else
            {
                var queue = SpRepository<AssignedQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ASSIGNED_QUEUE @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORCE_TYPE, @SORCE_NAME, @WORK_ID, @SORT_BY,@SORT_ORDER,@ASSIGNED_TO,@STATUS,@USER_NAME,@INCLUDE_ARCHIVE,@IS_TRASH",
           parmPracticeCode, parmCurrentPage, parmRecordsPerPage, parmSearchText, parmSourceType, parmSorceName, parmWorkId, parmSortBy, parmSortOrder, parmAssignTo, parmStatus, USER_NAME, parmIncludeArchive, parmIsTrash);
                return queue;
            }

        }
        public string ExportToExcelAssignedQueue(AssignedQueueRequest req, UserProfile profile)
        {
            try
            {
                string fileName = "Assigned_Queue_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CurrentPage = 1;
                req.RecordPerPage = 0;
                var CalledFrom = "Assigned_Queue";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<AssignedQueue> result = new List<AssignedQueue>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetIndexedQueue(req, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<AssignedQueue>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'), profile.isTalkRehab);
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UsersForDropdown> GetSupervisorAndAgentsForDropdown(long practiceCode, long roleId, string userName)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var current_user = new SqlParameter("@CURRENT_USER", SqlDbType.VarChar) { Value = userName };
            var role_id = new SqlParameter("@ROLE_ID", SqlDbType.BigInt) { Value = roleId };
            var queue = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_AGENTS_AND_SUPERVISOR @PRACTICE_CODE, @CURRENT_USER, @ROLE_ID", parmPracticeCode, current_user, role_id);
            return queue;
        }
        public List<UsersForDropdown> GetIndexersForDropdown(long practiceCode, string userName)
        {
            var current_user = new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = userName };
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var queue = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_INDEXER  @PRACTICE_CODE, @CURRENT_USER", parmPracticeCode, current_user);
            return queue;

        }
        public List<InterfcaeFailedPatient> GeInterfaceFailedPatientList(long practiceCode, string userName)
        {
            var current_user = new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = userName };
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var queue = SpRepository<InterfcaeFailedPatient>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_INTERFACE_FAILED_PATIENT @PRACTICE_CODE, @CURRENT_USER", parmPracticeCode, current_user);
            return queue;

        }

        public List<UsersForDropdown> GetSupervisorsForDropdown(long practiceCode, string userName)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var current_user = new SqlParameter("@CURRENT_USER", SqlDbType.VarChar) { Value = userName };
            var queue = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SUPERVISORS_FOR_DROPDOWN @PRACTICE_CODE, @CURRENT_USER", parmPracticeCode, current_user);
            return queue;
        }

        public ResponseModel BlackListOrWhiteListSource(BlacklistWhiteListSourceModel req, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            response.Success = false;
            if (req != null && req.Work_Ids != null && req.Work_Ids.Count() > 0)
            {
                List<Tuple<string, string>> sourceDatalist = new List<Tuple<string, string>>();
                foreach (var work_id in req.Work_Ids)
                {
                    var w_Order = _queueRepository.GetFirst(e => e.WORK_ID == work_id && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                    if (w_Order != null)
                    {
                        if (sourceDatalist.Where(w => w.Item1.Equals(w_Order.SORCE_TYPE) && w.Item2.Equals(w_Order.SORCE_NAME)).ToList() != null)
                        {
                            sourceDatalist.Add(new Tuple<string, string>(w_Order.SORCE_TYPE, w_Order.SORCE_NAME));
                        }
                        if (!req.Mark_All_Orders_As_White_Listed)
                            MarkReferralAsTrashedOrValid(w_Order.WORK_ID, req.Is_Black_List, profile);
                        //w_Order.IS_TRASH_REFERRAL = true;
                        //w_Order.MODIFIED_BY = profile.UserName;
                        //w_Order.MODIFIED_DATE = Helper.GetCurrentDate();
                        //_queueRepository.Update(w_Order);
                        //_queueRepository.Save();
                    }
                }

                if (sourceDatalist.Count() > 0)
                {
                    if (!req.Is_Black_List && req.Mark_All_Orders_As_White_Listed)
                    {
                        MarkAllReferralsAsValid(sourceDatalist, profile);
                    }
                    response = MarkSourceAsBlackWhiteListed(sourceDatalist, req.Is_Black_List, profile);
                }
            }

            return response;
        }

        public ResponseModel MarkSourceAsBlackWhiteListed(List<Tuple<string, string>> sourceDatalist, bool Is_Black_List, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            if (sourceDatalist.Count > 0)
            {
                foreach (var sourceItem in sourceDatalist)
                {
                    AddUpdateSourceAsBlackOrWhiteList(sourceItem.Item1, sourceItem.Item2, Is_Black_List, profile);
                }

                response.Success = true;
                response.Message = "Source(s) marked as blacklisted successfully.";
            }
            return response;
        }

        public ResponseModel AddUpdateSourceAsBlackOrWhiteList(string _sourcetype, string _sourcename, bool Is_Black_List, UserProfile profile)
        {
            var response = new ResponseModel();
            response.Success = false;
            var blacklistedWhitelistedSource = new BlacklistedWhitelistedSource();
            var dbData = _blacklistedWhitelistedSourceRepository.GetFirst(e => e.SOURCE_TYPE == _sourcetype && e.SOURCE_NAME == _sourcename && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
            if (dbData == null)
            {
                blacklistedWhitelistedSource.BLACKLISTED_WHITELISTED_SOURCE_ID = Helper.getMaximumId("FOX_BLACKLISTED_WHITELISTED_SOURCE_ID");
                blacklistedWhitelistedSource.PRACTICE_CODE = profile.PracticeCode;
                blacklistedWhitelistedSource.CREATED_BY = blacklistedWhitelistedSource.MODIFIED_BY = profile.UserName;
                blacklistedWhitelistedSource.CREATED_DATE = blacklistedWhitelistedSource.MODIFIED_DATE = Helper.GetCurrentDate();
                blacklistedWhitelistedSource.DELETED = false;
                blacklistedWhitelistedSource.SOURCE_TYPE = _sourcetype;
                blacklistedWhitelistedSource.SOURCE_NAME = _sourcename;
            }
            else
            {
                blacklistedWhitelistedSource = dbData;
                blacklistedWhitelistedSource.MODIFIED_BY = profile.UserName;
                blacklistedWhitelistedSource.MODIFIED_DATE = Helper.GetCurrentDate();
            }

            blacklistedWhitelistedSource.IS_BLACK_LISTED = Is_Black_List;

            if (dbData == null)
            {
                _blacklistedWhitelistedSourceRepository.Insert(blacklistedWhitelistedSource);
            }
            else
            {
                _blacklistedWhitelistedSourceRepository.Update(blacklistedWhitelistedSource);
            }

            _blacklistedWhitelistedSourceRepository.Save();
            response.Success = true;
            return response;
        }

        public ResponseModel MarkAllReferralsAsValid(List<Tuple<string, string>> sourceDatalist, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            response.Success = false;
            if (sourceDatalist.Count > 0)
            {
                foreach (var uniqueSourceItem in sourceDatalist)
                {
                    var w_Order_Ids = (from wq in _queueContext.WorkQueue
                                       //join wqai in _queueContext.WorkOrderAddtionalInfo on wq.WORK_ID equals wqai.WORK_ID
                                       where
                                            wq.SORCE_TYPE.Equals(uniqueSourceItem.Item1)
                                            && wq.SORCE_NAME.Equals(uniqueSourceItem.Item2)
                                            && (wq.IS_TRASH_REFERRAL ?? false) == true
                                            && wq.DELETED == false
                                            && wq.PRACTICE_CODE == profile.PracticeCode
                                            //&& wqai.DELETED == false
                                       select wq.WORK_ID).ToList();

                    //var w_Order = _queueRepository.GetMany(e => e.SORCE_TYPE.Equals(sourceItem.Item1) && e.SORCE_NAME.Equals(sourceItem.Item2) && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);
                    if (w_Order_Ids != null && w_Order_Ids.Count > 0)
                    {
                        foreach (var workId in w_Order_Ids)
                        {
                            MarkReferralAsTrashedOrValid(workId, false, profile);
                            //var order = _workOrderAddtionalInfoRepository.GetFirst(e => e.WORK_ID == workId && !e.DELETED && (e.IS_TRASH_REFERRAL??false) == true);
                            //if (order != null) {
                            //    order.IS_TRASH_REFERRAL = false;
                            //    order.MODIFIED_BY = profile.UserName;
                            //    order.MODIFIED_DATE = Helper.GetCurrentDate();
                            //    _workOrderAddtionalInfoRepository.Update(order);
                            //    _workOrderAddtionalInfoRepository.Save();
                            //}
                        }
                    }
                }
            }

            response.Success = true;
            return response;
        }

        public ResponseModel MakeReferralAsValidOrTrashed(MarkReferralValidOrTrashedModel req, UserProfile profile)
        {
            var response = new ResponseModel();
            response.Success = false;
            if (req != null) {
                MarkReferralAsTrashedOrValid(req.Work_Id, req.Is_Trash, profile);
                response.Success = true;
                var workQueue = GetQueueById(req.Work_Id);
                string logMsg = "";
                if (req.Is_Trash == true)
                {
                    logMsg = string.Format("ID: {0} has been trashed.", req.Work_Id);
                }
                else
                {
                    logMsg = string.Format("ID: {0} has been restored.", req.Work_Id);
                }
                          
                string user = !string.IsNullOrEmpty(profile.FirstName) ? profile.FirstName + " " + profile.LastName : profile.UserName;
                Helper.LogSingleWorkOrderChange(req.Work_Id, workQueue.UNIQUE_ID, logMsg, user);
            }

            return response;
        }

        public ResponseModel MarkReferralAsTrashedOrValid(long work_id, bool markAsTrashed, UserProfile profile)
        {
            var response = new ResponseModel();
            response.Success = false;
            //var dbData = _workOrderAddtionalInfoRepository.GetFirst(e => e.WORK_ID == work_id && !e.DELETED);
            //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var workId = new SqlParameter("UNIQUE_ID", SqlDbType.VarChar) { Value = work_id };
            var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var dbData = SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_WORK_QUEUE_DETAILS  @UNIQUE_ID, @PRACTICE_CODE", workId, practiceCode);
           //var dbData = _queueRepository.GetFirst(e => e.WORK_ID == work_id && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);
            //if (dbData == null)
            //{
            //    addInfo.WORK_ORDER_ADDTIONAL_INFO_ID = Helper.getMaximumId("FOX_WORK_ORDER_ADDTIONAL_INFO_ID");
            //    addInfo.CREATED_BY = addInfo.MODIFIED_BY = profile.UserName;
            //    addInfo.CREATED_DATE = addInfo.MODIFIED_DATE = Helper.GetCurrentDate();
            //    addInfo.DELETED = false;
            //    addInfo.WORK_ID = work_id;
            //}
            //else
            //{
            //    addInfo = dbData;
            //    addInfo.MODIFIED_BY = profile.UserName;
            //    addInfo.MODIFIED_DATE = Helper.GetCurrentDate();
            //}
            if(dbData != null)
            {
                dbData.IS_TRASH_REFERRAL = markAsTrashed;
                dbData.MODIFIED_BY = profile.UserName;
                dbData.MODIFIED_DATE = Helper.GetCurrentDate();
            }
            if (dbData == null)
            {
                _queueRepository.Insert(dbData);
            }
            else
            {
                _queueRepository.Update(dbData);
            }
            _queueRepository.Save();
            response.Success = true;
            return response;
        }
        private OriginalQueue GetQueueById(long workId)
        {
            SqlParameter work_Id = new SqlParameter("WORK_ID", workId);
            return SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_WORKQUEUE_BY_ID @WORK_ID", work_Id);
        }

    }
}