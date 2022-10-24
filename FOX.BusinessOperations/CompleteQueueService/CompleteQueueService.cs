using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.CompleteQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.GenericRepository;
using System.Data.SqlClient;
using System.Data;
using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using System.IO;

namespace FOX.BusinessOperations.CompleteQueueService
{
    public class CompleteQueueService : ICompleteQueueService
    {
        private long retrycatch = 0;
        public List<CompleteQueue> GetCompleteQueue(SearchRequestCompletedQueue req, UserProfile user)
        {
            try {
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
                    user.UserName = req.reportUser;
                }
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = user.PracticeCode };
                var parmUniqueId = Helper.getDBNullOrValue("UNIQUE_ID", req.ID);
                var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CurrentPage };
                var recordperpage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = req.RecordPerPage };
                var parmSourceType = Helper.getDBNullOrValue("SOURCE_TYPE", req.SorceType);
                var parmSourceName = Helper.getDBNullOrValue("SOURCE_NAME", req.SorceName);
                var searchtext = Helper.getDBNullOrValue("SEARCH_TEXT", req.SearchText);
                var parmSSN = Helper.getDBNullOrValue("SSN", req.SSN);
                var parmFirstName = Helper.getDBNullOrValue("FIRST_NAME", req.patientFirstName);
                var ParmLastName = Helper.getDBNullOrValue("LAST_NAME", req.patientLastName);
                var parmAssingTo = Helper.getDBNullOrValue("ASSIGN_TO", req.AssignTo);
                var parmsortby = Helper.getDBNullOrValue("SORT_BY", req.SortBy);
                var parmsortorder = Helper.getDBNullOrValue("SORT_ORDER", req.SortOrder);
                var parmindexby = Helper.getDBNullOrValue("INDEXED_BY", req.IndexedBy);
                var parmincludeArchive = new SqlParameter("INCLUDE_ARCHIVE", SqlDbType.Bit) { Value = req.IncludeArchive };
                var parmMedicalRecordNum = Helper.getDBNullOrValue("MEDICAL_RECORD_NUMBER", req.medicalRecordNumber);
                var parmUserName = new SqlParameter("USER_NAME", SqlDbType.VarChar) { Value = user.UserName };
                var dATE_FROM = Helper.getDBNullOrValue("DATE_FROM", req.DATE_FROM.HasValue ? req.DATE_FROM.Value.ToString() : "");
                var dATE_TO = Helper.getDBNullOrValue("DATE_TO", req.DATE_TO.HasValue ? req.DATE_TO.Value.ToString() : "");
                if (req.reportUser != "" && req.reportUser != null)
                {
                    var CompleteQueue1 = SpRepository<CompleteQueue>.GetListWithStoreProcedure(@"[dbo].[FOX_PROC_GET_COMPLETED_QUEUE_TASK] @PRACTICE_CODE,@UNIQUE_ID,@CURRENT_PAGE,@RECORD_PER_PAGE,@SOURCE_TYPE,@SOURCE_NAME,@SEARCH_TEXT,@SSN,@FIRST_NAME,@LAST_NAME,@ASSIGN_TO,@SORT_BY,@SORT_ORDER,@INDEXED_BY,@INCLUDE_ARCHIVE,@MEDICAL_RECORD_NUMBER,@USER_NAME,@DATE_FROM,@DATE_TO",
                    parmPracticeCode, parmUniqueId, parmCurrentPage, recordperpage, parmSourceType, parmSourceName, searchtext, parmSSN, parmFirstName, ParmLastName, parmAssingTo, parmsortby, parmsortorder, parmindexby, parmincludeArchive, parmMedicalRecordNum, parmUserName, dATE_FROM, dATE_TO);
                    return CompleteQueue1;
                }
                else {
                    var CompleteQueue = SpRepository<CompleteQueue>.GetListWithStoreProcedure(@"[dbo].[FOX_PROC_GET_COMPLETED_QUEUE] @PRACTICE_CODE,@UNIQUE_ID,@CURRENT_PAGE,@RECORD_PER_PAGE,@SOURCE_TYPE,@SOURCE_NAME,@SEARCH_TEXT,@SSN,@FIRST_NAME,@LAST_NAME,@ASSIGN_TO,@SORT_BY,@SORT_ORDER,@INDEXED_BY,@INCLUDE_ARCHIVE,@MEDICAL_RECORD_NUMBER,@USER_NAME",
                    parmPracticeCode, parmUniqueId, parmCurrentPage, recordperpage, parmSourceType, parmSourceName, searchtext, parmSSN, parmFirstName, ParmLastName, parmAssingTo, parmsortby, parmsortorder, parmindexby, parmincludeArchive, parmMedicalRecordNum, parmUserName);
                    return CompleteQueue;
                }
            }
            catch (Exception ex)
            {
                if (retrycatch <= 2 && (!string.IsNullOrEmpty(ex.Message) &&
                  ex.Message.Contains("deadlocked on lock resources with another process"))
                  || ((ex.InnerException != null) &&
                  !string.IsNullOrEmpty(ex.InnerException.Message)
                  &&
                  ex.InnerException.Message.Contains("deadlocked on lock resources with another process")))
                {
                    retrycatch = retrycatch + 1;
                    return GetCompleteQueue(req, user);
                }
                else
                {
                    throw ex;
                }
            }
        }


        public string ExportToExcelCompleteQueu(SearchRequestCompletedQueue req, UserProfile profile)
        {
            try
            {
                string fileName = "Completed_Queue_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CurrentPage = 1;
                req.RecordPerPage = 0;
                var CalledFrom = "Completed_Queue";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<CompleteQueue> result = new List<CompleteQueue>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetCompleteQueue(req, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<CompleteQueue>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
