using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.OriginalQueueService
{
    public class OriginalQueueService : IOriginalQueueService
    {
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFilesRepository;

        public OriginalQueueService()
        {
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _OriginalQueueFilesRepository = new GenericRepository<OriginalQueueFiles>(_QueueContext);
        }
        public ResOriginalQueueModel GetOriginalQueue(OriginalQueueRequest req, UserProfile Profile)
        {
            ResOriginalQueueModel resOriginalQueueModel = new ResOriginalQueueModel();
            if (!string.IsNullOrEmpty(req.DateFrom_str))
                req.DateFrom = req.DateFrom_str;
            if (!string.IsNullOrEmpty(req.DateTo_str))
                req.DateTo = req.DateTo_str;

            //Edit By Ali
            if (req.SearchText.Contains("_"))
            {
                var IsUniqueIdExist = _QueueRepository.GetMany(t => t.UNIQUE_ID.Contains(req.SearchText));
                if (IsUniqueIdExist != null && IsUniqueIdExist.Count > 0)
                {
                    req.SearchText = req.SearchText.Split('_')[0];
                    resOriginalQueueModel.IsUniqueIdExist = true;
                    resOriginalQueueModel.Unique_IdList = IsUniqueIdExist.Select(t => t.UNIQUE_ID).ToList();
                }
                req.SearchText = req.SearchText.Replace("_", @"\_");
            }
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
            var parmDateFrom = Helper.getDBNullOrValue("DATE_FROM", req.DateFrom);
            var parmDateTo = Helper.getDBNullOrValue("DATE_TO", req.DateTo);

            //var parmDateFrom = new SqlParameter("DATE_FROM", SqlDbType.DateTime) { Value = req.DateFrom == null ? new DateTime(1800, 1, 1) : req.DateFrom };
            //var parmDateTo = new SqlParameter("DATE_TO", SqlDbType.DateTime) { Value = req.DateTo == null ? new DateTime(1800, 1, 1) : req.DateTo };
            var parmIncludeArchive = new SqlParameter("INCLUDE_ARCHIVE", SqlDbType.Bit) { Value = req.IncludeArchive };
            var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RecordPerPage };
            //var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText.Split('_')[0] };
            var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText };
            var parmstatus = new SqlParameter("STATUS_TEXT", SqlDbType.VarChar) { Value = req.STATUS_TEXT };
            var parmSourceString = new SqlParameter("SORCE_STRING", SqlDbType.VarChar) { Value = req.SorceString };
            var parmSourceType = new SqlParameter("SORCE_TYPE", SqlDbType.VarChar) { Value = req.SorceType };
            var parmworkid = new SqlParameter("WORK_ID", SqlDbType.VarChar) { Value = req.WORK_ID };
            var parmSortBy = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SortBy };
            var parmSortOrder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SortOrder };
            var queue = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ORIGINAL_QUEUE @PRACTICE_CODE, @CURRENT_PAGE, @DATE_FROM, @DATE_TO, @INCLUDE_ARCHIVE, @RECORD_PER_PAGE, @SEARCH_TEXT,@STATUS_TEXT, @SORCE_STRING, @SORCE_TYPE, @WORK_ID, @SORT_BY,@SORT_ORDER",
                parmPracticeCode, parmCurrentPage, parmDateFrom, parmDateTo, parmIncludeArchive, parmRecordsPerPage, parmSearchText, parmstatus, parmSourceString, parmSourceType, parmworkid, parmSortBy, parmSortOrder);
            resOriginalQueueModel.OriginalQueueList = queue;
            Helper.TokenTaskCancellationExceptionLog(queue != null && queue.Count() > 0 ? queue[0].TOTAL_RECORDS.ToString() : "Empty");
            return resOriginalQueueModel;
        }
        public ResOriginalQueueModel GetReferralList(OriginalQueueRequest req, UserProfile Profile)
        {
            ResOriginalQueueModel resOriginalQueueModel = new ResOriginalQueueModel();
            var parmSortBy = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SortBy };
            var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RecordPerPage };
            var parmSortOrder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SortOrder };
            var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText };
            var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
            var queue = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ORIGINAL_QUEUE_SIGNATURE @CURRENT_PAGE,@RECORD_PER_PAGE,@SEARCH_TEXT,@SORT_BY,@SORT_ORDER", parmCurrentPage, parmRecordsPerPage, parmSearchText, parmSortBy, parmSortOrder);
            resOriginalQueueModel.OriginalQueueList = queue;
            return resOriginalQueueModel;
            //if (!string.IsNullOrEmpty(req.DateFrom_str))
            //    req.DateFrom = req.DateFrom_str;
            //if (!string.IsNullOrEmpty(req.DateTo_str))
            //    req.DateTo = req.DateTo_str;

            ////Edit By Ali
            //if (req.SearchText.Contains("_"))
            //{
            //    var IsUniqueIdExist = _QueueRepository.GetMany(t => t.UNIQUE_ID.Contains(req.SearchText));
            //    if (IsUniqueIdExist != null && IsUniqueIdExist.Count > 0)
            //    {
            //        req.SearchText = req.SearchText.Split('_')[0];
            //        resOriginalQueueModel.IsUniqueIdExist = true;
            //        resOriginalQueueModel.Unique_IdList = IsUniqueIdExist.Select(t => t.UNIQUE_ID).ToList();
            //    }
            //    req.SearchText = req.SearchText.Replace("_", @"\_");
            //}
            //var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            //var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
            //var parmDateFrom = Helper.getDBNullOrValue("DATE_FROM", req.DateFrom);
            //var parmDateTo = Helper.getDBNullOrValue("DATE_TO", req.DateTo);

            ////var parmDateFrom = new SqlParameter("DATE_FROM", SqlDbType.DateTime) { Value = req.DateFrom == null ? new DateTime(1800, 1, 1) : req.DateFrom };
            ////var parmDateTo = new SqlParameter("DATE_TO", SqlDbType.DateTime) { Value = req.DateTo == null ? new DateTime(1800, 1, 1) : req.DateTo };
            //var parmIncludeArchive = new SqlParameter("INCLUDE_ARCHIVE", SqlDbType.Bit) { Value = req.IncludeArchive };
            //var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RecordPerPage };
            ////var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText.Split('_')[0] };
            //var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText };
            //var parmSourceString = new SqlParameter("SORCE_STRING", SqlDbType.VarChar) { Value = req.SorceString };
            //var parmSourceType = new SqlParameter("SORCE_TYPE", SqlDbType.VarChar) { Value = req.SorceType };
            //var parmworkid = new SqlParameter("WORK_ID", SqlDbType.VarChar) { Value = req.WORK_ID };
            //var parmSortBy = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SortBy };
            //var parmSortOrder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SortOrder };
            //var queue = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ORIGINAL_QUEUE");

            //resOriginalQueueModel.OriginalQueueList = queue;
            //return resOriginalQueueModel;
        }
        public string ExportToExcelOrignalQueue(OriginalQueueRequest req, UserProfile profile)
        {
            try
            {
                string fileName = "Original_Queue_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CurrentPage = 1;
                req.RecordPerPage = 0;
                var CalledFrom = "Original_Queue";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                ResOriginalQueueModel resOriginalQueueModel = new ResOriginalQueueModel();
                List< OriginalQueue> result = new List<OriginalQueue>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                resOriginalQueueModel = GetOriginalQueue(req, profile);
                result = resOriginalQueueModel.OriginalQueueList; 
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<OriginalQueue>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long SaveQueueData(OriginalQueue req, UserProfile Profile)
        {
            var queue = _QueueRepository.GetByID(req.WORK_ID);
            if (queue != null)
            {
                return queue.WORK_ID;

            }
            else
            {
                OriginalQueueFiles file = new OriginalQueueFiles();
                file.FILE_PATH1 = req.FILE_PATH;

                req.WORK_ID = Helper.getMaximumId("WORK_ID");
                req.UNIQUE_ID = req.WORK_ID.ToString();
                req.CREATED_BY = Profile.UserName;
                req.MODIFIED_BY = Profile.UserName;
                req.CREATED_DATE = Helper.GetCurrentDate();
                req.MODIFIED_DATE = Helper.GetCurrentDate();
                req.RECEIVE_DATE = Helper.GetCurrentDate();
                req.PRACTICE_CODE = Profile.PracticeCode;
                req.DELETED = false;
                req.FILE_PATH = "";
                _QueueRepository.Insert(req);
                _QueueRepository.Save();



                file.deleted = false;
                file.FILE_PATH = req.FILE_PATH_LOGO;
                file.FILE_ID = Helper.getMaximumId("FOXREHAB_FILE_ID");
                file.WORK_ID = req.WORK_ID;
                file.UNIQUE_ID = req.UNIQUE_ID;
                _OriginalQueueFilesRepository.Insert(file);
                _OriginalQueueFilesRepository.Save();
                return req.WORK_ID;
            }
        }

        public List<WorkDetails> GetWorkDetails(long workId, long practiceCode)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var parmWorkId = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = workId };
            var queue = SpRepository<WorkDetails>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS]  @PRACTICE_CODE, @WORK_ID",
                parmPracticeCode, parmWorkId);
            return queue;
        }

        //public List<WorkDetails> GetWorkDetailsUniqueId(string uniqueId, UserProfile Profile)
        //{
        //    SqlParameter parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
        //    SqlParameter parmUniqueId = new SqlParameter("UNIQUE_ID", SqlDbType.VarChar) { Value = uniqueId };
        //    var queue = SpRepository<WorkDetails>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_ORIGINAL_QUEUE_DETAILS_BY_UNIQUE_ID]  @PRACTICE_CODE, @UNIQUE_ID",
        //        parmPracticeCode, parmUniqueId);
        //    return queue;
        //}

        public List<WorkDetails> GetWorkDetailsUniqueId(ReqOriginalQueueModel reqOriginalQueueModel, UserProfile Profile)
        {
            string uniqueId = string.Join("', '", reqOriginalQueueModel.Unique_IdList);
            var queue = SpRepository<WorkDetails>.GetListWithStoreProcedure(@"SELECT WORK_ID, UNIQUE_ID, WORK_STATUS, TOTAL_PAGES NO_OF_PAGES, AT.LAST_NAME + ', ' + AT.FIRST_NAME AS ASSIGNED_TO, CB.LAST_NAME + ', ' + CB.FIRST_NAME AS COMPLETED_BY, FILE_PATH, wq.IS_EMERGENCY_ORDER FROM FOX_TBL_WORK_QUEUE WQ LEFT JOIN FOX_TBL_APPLICATION_USER AT ON WQ.ASSIGNED_TO = AT.USER_NAME LEFT JOIN FOX_TBL_APPLICATION_USER CB ON WQ.COMPLETED_BY = CB.USER_NAME WHERE UNIQUE_ID IN ('" + uniqueId + @"') AND WQ.PRACTICE_CODE = " + Profile.PracticeCode.ToString());
            return queue;
        }

        public OriginalQueue SaveQueueFromOldQueueData(long work_ID, UserProfile Profile, int numberOfPages, long CurrrentParentID)
        {
            var queue = _QueueRepository.GetByID(work_ID);
            var count = _QueueRepository.GetMany(x => x.UNIQUE_ID.Contains(work_ID.ToString())).Count;

            if (queue != null)
            {
                OriginalQueue req = new OriginalQueue();
                req.FILE_PATH = null;
                req.WORK_ID = Helper.getMaximumId("WORK_ID");
                req.UNIQUE_ID = work_ID.ToString() + "_" + count;
                req.DOCUMENT_TYPE = queue.DOCUMENT_TYPE;
                req.SORCE_NAME = queue.SORCE_NAME;
                req.SORCE_TYPE = queue.SORCE_TYPE;
                req.TOTAL_ROCORD_PAGES = queue.TOTAL_ROCORD_PAGES;
                req.PRACTICE_CODE = queue.PRACTICE_CODE;
                req.CREATED_BY = Profile.UserName;
                req.MODIFIED_BY = Profile.UserName;
                req.CREATED_DATE = Helper.GetCurrentDate();
                req.MODIFIED_DATE = Helper.GetCurrentDate();
                req.ASSIGNED_DATE = Helper.GetCurrentDate();
                req.ASSIGNED_BY = Profile.UserName;
                req.ASSIGNED_TO = queue.ASSIGNED_TO;

                req.RECEIVE_DATE = Helper.GetCurrentDate();
                req.PRACTICE_CODE = Profile.PracticeCode;
                req.TOTAL_PAGES = numberOfPages;
                req.IS_EMERGENCY_ORDER = queue.IS_EMERGENCY_ORDER;
                req.WORK_STATUS = "Index Pending";

                req.DELETED = false;
                req.FILE_PATH = "";
                _QueueRepository.Insert(req);
                _QueueRepository.Save();
                UpdateNmOfPages(CurrrentParentID, Profile, numberOfPages);

                //           string connectionString = System.Configuration.ConfigurationManager
                //                    .ConnectionStrings["LocalDB"].ConnectionString;
                //           using (SqlConnection connection = new SqlConnection(connectionString))
                //using (SqlCommand command = connection.CreateCommand())
                //           {
                //               command.CommandText = "INSERT INTO Student (LastName, FirstName, Address, City) 
                //                     VALUES(@ln, @fn, @add, @cit)";
                //           command.CommandText = "INSERT INTO Student (LastName, FirstName, Address, City) 
                //                     VALUES(@ln, @fn, @add, @cit)";
                return req;
            }
            else
            {

                return null;
            }
        }


        //public string ExportOriginalQueue(OriginalQueueRequest obj, UserProfile profile)
        //{
        //    try
        //    {
        //        string fileName = "Orignal_Queue";
        //        string exportPath = "";
        //        string path = string.Empty;
        //        bool exported;
        //       //  obj.CurrentPage = 1;
        //        //    obj.RecordPerPage = 0;
        //        //obj.CalledFrom = "Analysis_Report";
        //        string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
        //        exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
        //        fileName =DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
        //        if (!Directory.Exists(exportPath))
        //        {
        //            Directory.CreateDirectory(exportPath);
        //        }
        //        var pathtowriteFile = exportPath + "\\" + fileName;
        //        switch (obj.CalledFrom)
        //        {
        //            #region RBS Blocked Claims
        //            case "Orignal_Queue":
        //                {
        //                    // List<IndexedQueue> GetIndexedQueue(IndexedQueueRequest req, UserProfile Profile)
        //                    var result = GetOriginalQueue(obj, profile).OriginalQueueList;
        //                    exported = ExportToExcel.CreateExcelDocument<OriginalQueue>(result,  pathtowriteFile, obj.CalledFrom.Replace(' ', '_'));
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





        public void UpdateNmOfPages(long CurrrentParentID, UserProfile Profile, int oldPages)
        {
            OriginalQueue req = new OriginalQueue();

            var queue = _QueueRepository.GetByID(CurrrentParentID);

            if (queue != null)
            {
                var newPages = queue.TOTAL_PAGES - oldPages;
                queue.TOTAL_PAGES = newPages;
                queue.MODIFIED_DATE = Helper.GetCurrentDate();
                queue.MODIFIED_BY = Profile.UserName;
                _QueueRepository.Update(queue);
                _QueueRepository.Save();
            }
            else
            {

            }
        }

        //object IOriginalQueueService.Export(OriginalQueueRequest obj, UserProfile userProfile)
        //{
        //    throw new NotImplementedException();
        //}
    }
}


//FOX_TBL_WORK_QUEUE_File_All