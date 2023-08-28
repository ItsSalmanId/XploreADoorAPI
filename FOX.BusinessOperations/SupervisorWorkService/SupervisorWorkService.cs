using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.SupervisorWorkModel;
using FOX.DataModels.GenericRepository;
using System.Data.SqlClient;
using System.Data;
using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Models.Settings.User;
using FOX.BusinessOperations.CommonServices;
using System.IO;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.AssignedQueueModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Context;

namespace FOX.BusinessOperations.SupervisorWorkService
{
    public class SupervisorWorkService : ISupervisorWorkService
    {
        private readonly DBContextQueue _queueContext = new DBContextQueue();
        private readonly GenericRepository<OriginalQueue> _queueRepository;

        public SupervisorWorkService()
        {
            _queueRepository = new GenericRepository<OriginalQueue>(_queueContext);
        }
        public List<SupervisorWork> GetSupervisorList(SupervisorWorkRequest request, UserProfile Profile)
        {
            if (request.TransferReason == "")
                request.TransferReason = null;
            if (request.IndexBy == "")
                request.IndexBy = null;
            if (request.SearchText == "")
                request.SearchText = null;
            if (request.SortBy == "")
                request.SortBy = null;
            if (request.SortOrder == "")
                request.SortOrder = null;
            if (request.SourceString == "")
                request.SourceString = null;
            if (request.SourceName == "")
                request.SourceName = null;
            if (request.SourceType == "")
                request.SourceType = null;
            if (request.SupervisorName == "")
                request.SupervisorName = null;
            if (request.UniqueId == "")
                request.UniqueId = null;
            if (request.TransferComments == "")
                request.TransferComments = null;
            if (request.SortOrder == "" || request.SortOrder==null) {
                request.SortOrder = "DESC";
                }
            if (request.SortBy == "" || request.SortBy==null)
            {
                request.SortBy = "TransferDate";
            }
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var uniqueid = Helper.getDBNullOrValue("UNIQUE_ID", request.UniqueId);
            var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = request.CurrentPage };
            var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = request.RecordPerPage };
            var parmSourceType =  Helper.getDBNullOrValue("SOURCE_TYPE", request.SourceType);
            var parmSourceName =  Helper.getDBNullOrValue("SOURCE_NAME", request.SourceName);
            var parmSearchText =  Helper.getDBNullOrValue("SEARCH_TEXT", request.SearchText);
            var parmSourceString = Helper.getDBNullOrValue("SORCE_STRING", request.SourceString);
            var parmsortby =  Helper.getDBNullOrValue("SORT_BY", request.SortBy);
            var parmsortorder =  Helper.getDBNullOrValue("SORT_ORDER", request.SortOrder);
            var parmIndexBy =  Helper.getDBNullOrValue("INDEXED_BY", request.IndexBy);
            var parmSupervisorName =  Helper.getDBNullOrValue("SUPERVISORNAME", request.SupervisorName);
            var parmTransferReason =  Helper.getDBNullOrValue("TRANSFER_REASON", request.TransferReason);
            var parmTransferComments =  Helper.getDBNullOrValue("TRANSFER_COMMENTS", request.TransferComments);
            var parmStatus =  Helper.getDBNullOrValue("STATUS", request.Status);
            var parmIsTrash = new SqlParameter("Is_Trash", SqlDbType.Bit) { Value = request.Is_Trash };

            var queue = SpRepository<SupervisorWork>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SUPERVISOR_WORK @PRACTICE_CODE ,@UNIQUE_ID ,@CURRENT_PAGE,@RECORD_PER_PAGE,@SOURCE_TYPE,@SOURCE_NAME,@SEARCH_TEXT ,@SORT_BY,@SORT_ORDER ,@INDEXED_BY,@SUPERVISORNAME ,@TRANSFER_REASON,@TRANSFER_COMMENTS,@STATUS, @IS_TRASH",
                        parmPracticeCode, uniqueid, parmCurrentPage, parmRecordsPerPage, parmSourceType, parmSourceName, parmSearchText,parmsortby,parmsortorder, parmIndexBy, parmSupervisorName, parmTransferReason, parmTransferComments,parmStatus, parmIsTrash);
            return queue;
        }
        public string ExportToExcelSupervisorQueu(SupervisorWorkRequest req, UserProfile profile)
        {
            try
            {
                string fileName = "Supervisor_Queue_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CurrentPage = 1;
                req.RecordPerPage = 0;
                var CalledFrom = "Supervisor_Queue";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<SupervisorWork> result = new List<SupervisorWork>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetSupervisorList(req, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<SupervisorWork>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SupervisorExport(SupervisorWorkRequest obj, UserProfile profile)
        {
            try
            {
                string fileName = "Supervisor_Work";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                obj.CurrentPage = 1;
                obj.RecordPerPage = 0;
                //obj.CalledFrom = "Analysis_Report";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!System.IO.Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                var pathtowriteFile = exportPath + "\\" + fileName;
                switch (obj.CalledFrom)
                {
                    #region RBS Blocked Claims
                    case "Supervisor_Work":
                        {
                            var result = GetSupervisorList(obj, profile);
                            //if (result.Count > 0)
                            //{
                            //    //var item = result[0];
                            //    //var item2 = result[1];
                            //    //result.RemoveAt(0);
                            //    //result.RemoveAt(0);

                            //    //result.Add(item);
                            //    //result.Add(item2);
                            //}
                            //for (int i = 0; i < result.Count; i++)
                            //{
                            //    result[i].ROW = i + 1;
                            //}
                            exported = ExportToExcel.CreateExcelDocument<SupervisorWork>(result, pathtowriteFile, obj.CalledFrom.Replace(' ', '_'));
                            break;
                        }
                        #endregion
                }
                return virtualPath + fileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UsersForDropdown> GetIndxersAndSupervisorsForDropdown(long practiceCode, string userName)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var parmUserName = new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = userName };

            var queue = SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_INDEXERS_AND_SUPERVISORS  @PRACTICE_CODE, @CURRENT_USER", parmPracticeCode, parmUserName);
            return queue;

        }
        public List<WorkTransfer> GetWorkTransferComments(long Work_Id)
        {
            var workId = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = Work_Id };
            var queue = SpRepository<WorkTransfer>.GetListWithStoreProcedure(@"exec [FOX_GET_TRANSFERCOMMENTS]  @WORK_ID", workId);
            return queue;
        }

        public ResponseModel MakeReferralAsValidOrTrashed(MarkReferralValidOrTrashedModel req, UserProfile profile)
        {
            var response = new ResponseModel();
            response.Success = false;
            if (req != null)
            {
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
            var dbData = _queueRepository.GetFirst(e => e.WORK_ID == work_id && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);

            if (dbData != null)
            {
                dbData.IS_TRASH_REFERRAL = markAsTrashed;
                if (markAsTrashed == true)
                {
                    dbData.WORK_STATUS = "Trashed";
                }
                if(markAsTrashed == false)
                {
                    dbData.WORK_STATUS = "Index Pending";
                }
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


