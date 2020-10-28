using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.UnAssignedQueueModel;
using System.Data.SqlClient;
using FOX.DataModels.GenericRepository;
using System.Data;
using FOX.DataModels.Models.Settings.User;
using FOX.BusinessOperations.CommonServices;
using System.IO;

namespace FOX.BusinessOperations.UnAssignedQueueService
{
    public class UnAssignedQueueService : IUnAssignedQueueService
    {
        public List<UnAssignedQueue> GetUnAssignedQueue(UnAssignedQueueRequest req, UserProfile Profile)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
          //  var parmClient = new SqlParameter("CLIENT", SqlDbType.VarChar) { Value = req.Client };
            var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
            //var parmDateFrom = new SqlParameter("DATE_FROM", SqlDbType.DateTime) { Value = req.DateFrom == null ? new DateTime(1800, 1, 1) : req.DateFrom };
            //var parmDateTo = new SqlParameter("DATE_TO", SqlDbType.DateTime) { Value = req.DateTo == null ? new DateTime(1800, 1, 1) : req.DateTo };
            var parmIncludeArchive = new SqlParameter("INCLUDE_ARCHIVE", SqlDbType.Bit) { Value = req.IncludeArchive };
            var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RecordPerPage };
            var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText };
            var parmSourceString = new SqlParameter("SORCE_STRING", SqlDbType.VarChar) { Value = req.SorceName };
            var parmSourceType = new SqlParameter("SORCE_TYPE", SqlDbType.VarChar) { Value = req.SorceType };
            var uniqueid = new SqlParameter("UNIQUE_ID", SqlDbType.VarChar) { Value = req.ID };           
            var sortby = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SortBy};
            var sortorder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SortOrder };
            var queue = SpRepository<UnAssignedQueue>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_UNASSIGNED_QUEUE] @PRACTICE_CODE, @CURRENT_PAGE,@RECORD_PER_PAGE, @SEARCH_TEXT, @SORCE_STRING, @SORCE_TYPE,@UNIQUE_ID,@SORT_BY,@SORT_ORDER,@INCLUDE_ARCHIVE",
                parmPracticeCode, parmCurrentPage, parmRecordsPerPage, parmSearchText, parmSourceString, parmSourceType, uniqueid, sortby, sortorder, parmIncludeArchive);

            return queue;
        }
        public string ExportToExcelUnassignedQueue(UnAssignedQueueRequest req, UserProfile profile)
        {
            try
            {
                string fileName = "Unassigned_Queue_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CurrentPage = 1;
                req.RecordPerPage = 0;
                var CalledFrom = "Unassigned_Queue";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<UnAssignedQueue> result = new List<UnAssignedQueue>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetUnAssignedQueue(req, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<UnAssignedQueue>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UsersForDropdown> GetSupervisorForDropdown(long practiceCode)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var queue = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_SUPERVISOR  @PRACTICE_CODE", parmPracticeCode);
            return queue;

        }

        public List<UsersForDropdown> GetIndexersForDropdown(long practiceCode, string userName)
        {
            var current_user = new SqlParameter("CURRENT_USER", SqlDbType.VarChar) {    Value =  userName };
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var queue = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_INDEXER  @PRACTICE_CODE,@CURRENT_USER", parmPracticeCode, current_user);
            return queue;

        }

        public string Export(UnAssignedQueueRequest obj, UserProfile profile)
        {
            try
            {
                string fileName = "Unassigned_Queue";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                var pathtowriteFile = exportPath + "\\" + fileName;

                switch (obj.CalledFrom)
                {
                    #region RBS Blocked Claims
                    case "Unassigned_Queue":
                        {
                            // List<IndexedQueue> GetIndexedQueue(IndexedQueueRequest req, UserProfile Profile)
                            var result = GetUnAssignedQueue(obj, profile);
                            exported = ExportToExcel.CreateExcelDocument<UnAssignedQueue>(result, pathtowriteFile, obj.CalledFrom.Replace(' ', '_'));
                            break;
                        }
                        #endregion
                }

                //switch (obj.CalledFrom)
                //{
                //    case "Unassigned_Queue":
                //        {
                //            var result = GetUnAssignedQueue(obj, profile);
                //            exported = ExportToExcel.CreateExcelDocument<UnAssignedQueue>(result, pathToWriteFile, obj.CalledFrom.Replace(' ', '_'));
                //            break;

                //        }

                //}
                return virtualPath + fileName;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public UnAssignedQueueAndUsersForDropdown GetInitialData(UnAssignedQueueRequest req, UserProfile Profile)
        {
            UnAssignedQueueAndUsersForDropdown _obj = new UnAssignedQueueAndUsersForDropdown();
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
            var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RecordPerPage };
            var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SearchText };
            var parmSourceString = new SqlParameter("SORCE_STRING", SqlDbType.VarChar) { Value = req.SorceName };
            var parmSourceType = new SqlParameter("SORCE_TYPE", SqlDbType.VarChar) { Value = req.SorceType };
            var uniqueid = new SqlParameter("UNIQUE_ID", SqlDbType.VarChar) { Value = req.ID };
            var sortby = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SortBy };
            var sortorder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SortOrder };
            var includeArchive = new SqlParameter("INCLUDE_ARCHIVE", SqlDbType.Bit) { Value = req.IncludeArchive };
            _obj.unassignedQueueData = SpRepository<UnAssignedQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_UNASSIGNED_QUEUE
                                  @PRACTICE_CODE, @CURRENT_PAGE,@RECORD_PER_PAGE, @SEARCH_TEXT, @SORCE_STRING, @SORCE_TYPE,@UNIQUE_ID,@SORT_BY,@SORT_ORDER,@INCLUDE_ARCHIVE",
                                  parmPracticeCode, parmCurrentPage, parmRecordsPerPage, parmSearchText, parmSourceString, parmSourceType, uniqueid, sortby, sortorder, includeArchive);
            var current_user = new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = Profile.UserName };
            var parmPracticeCode2 = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            _obj.usersForDropdownData = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_INDEXER  @PRACTICE_CODE,@CURRENT_USER",
                                   parmPracticeCode2, current_user);
            return _obj;
        }
    }
}