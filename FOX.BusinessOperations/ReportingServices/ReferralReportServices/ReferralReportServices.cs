using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Reporting;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace FOX.BusinessOperations.ReportingServices.ReferralReportServices
{
    public class ReferralReportServices : IReferralReportServices
    {
        private readonly DbContextReporting _reportingContext = new DbContextReporting();
        private readonly GenericRepository<ReferralReport> _reportingRepository;

        public ReferralReportServices()
        {
            _reportingRepository = new GenericRepository<ReferralReport>(_reportingContext);
        }

        public List<ReferralReport> GetReferralReportList(ReferralReportRequest referralReportRequest, UserProfile profile)
        {
            //Indexed date mapping....
            if (!string.IsNullOrEmpty(referralReportRequest.INDEXED_DATE_FROM_STR))
                referralReportRequest.INDEXED_DATE_FROM = Convert.ToDateTime(referralReportRequest.INDEXED_DATE_FROM_STR);
            if (!string.IsNullOrEmpty(referralReportRequest.INDEXED_DATE_TO_STR))
                referralReportRequest.INDEXED_DATE_TO = Convert.ToDateTime(referralReportRequest.INDEXED_DATE_TO_STR);
            if (referralReportRequest.INDEXED_DATE_FROM.HasValue)
                if (referralReportRequest.INDEXED_DATE_TO.HasValue)
                    if (String.Equals(referralReportRequest.INDEXED_DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                        referralReportRequest.INDEXED_DATE_TO = referralReportRequest.INDEXED_DATE_TO.Value.AddDays(1).AddSeconds(-1);
                    else
                        referralReportRequest.INDEXED_DATE_TO = referralReportRequest.INDEXED_DATE_TO.Value.AddSeconds(59);
                else
                    referralReportRequest.INDEXED_DATE_TO = Helper.GetCurrentDate();
            else if (referralReportRequest.INDEXED_DATE_TO.HasValue)
            {
                if (String.Equals(referralReportRequest.INDEXED_DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                    referralReportRequest.INDEXED_DATE_TO = referralReportRequest.INDEXED_DATE_TO.Value.AddDays(1).AddSeconds(-1);
                var dateNow = Helper.GetCurrentDate();
                referralReportRequest.INDEXED_DATE_FROM = dateNow.AddYears(-100);
            }

            //Received date mapping....
            if (!string.IsNullOrEmpty(referralReportRequest.RECEIVED_DATE_FROM_STR))
                referralReportRequest.RECEIVED_DATE_FROM = Convert.ToDateTime(referralReportRequest.RECEIVED_DATE_FROM_STR);
            if (!string.IsNullOrEmpty(referralReportRequest.RECEIVED_DATE_TO_STR))
                referralReportRequest.RECEIVED_DATE_TO = Convert.ToDateTime(referralReportRequest.RECEIVED_DATE_TO_STR);
            if (referralReportRequest.RECEIVED_DATE_FROM.HasValue)
                if (referralReportRequest.RECEIVED_DATE_TO.HasValue)
                    if (String.Equals(referralReportRequest.RECEIVED_DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                        referralReportRequest.RECEIVED_DATE_TO = referralReportRequest.RECEIVED_DATE_TO.Value.AddDays(1).AddSeconds(-1);
                    else
                        referralReportRequest.RECEIVED_DATE_TO = referralReportRequest.RECEIVED_DATE_TO.Value.AddSeconds(59);
                else
                    referralReportRequest.RECEIVED_DATE_TO = Helper.GetCurrentDate();
            else if (referralReportRequest.RECEIVED_DATE_TO.HasValue)
            {
                if (String.Equals(referralReportRequest.RECEIVED_DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                    referralReportRequest.RECEIVED_DATE_TO = referralReportRequest.RECEIVED_DATE_TO.Value.AddDays(1).AddSeconds(-1);
                var dateNow = Helper.GetCurrentDate();
                referralReportRequest.RECEIVED_DATE_FROM = dateNow.AddYears(-100);
            }



            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var indexedDateFrom = Helper.getDBNullOrValue("INDEXED_DATE_FROM", referralReportRequest.INDEXED_DATE_FROM.ToString());
            var indexedDateTo = Helper.getDBNullOrValue("INDEXED_DATE_TO", referralReportRequest.INDEXED_DATE_TO.ToString());
            var receivedDateFrom = Helper.getDBNullOrValue("RECEIVED_DATE_FROM", referralReportRequest.RECEIVED_DATE_FROM.ToString());
            var receivedDateTo = Helper.getDBNullOrValue("RECEIVED_DATE_TO", referralReportRequest.RECEIVED_DATE_TO.ToString());
            var indexedStatus = new SqlParameter { ParameterName = "INDEXED_STATUS", Value = referralReportRequest.INDEXED_STATUS };
            var status = new SqlParameter { ParameterName = "STATUS", Value = referralReportRequest.STATUS };
            var documentType = new SqlParameter { ParameterName = "DOCUMENT_TYPE", Value = referralReportRequest.DOCUMENT_TYPE };
            var sourceType = new SqlParameter { ParameterName = "SOURCE_TYPE", Value = referralReportRequest.SOURCE_TYPE };
            var assignedPersonName = new SqlParameter { ParameterName = "ASSIGNED_PERSON_NAME", Value = referralReportRequest.ASSIGNED_PERSON_NAME };
            var currentPage = Helper.getDBNullOrValue("CURRENT_PAGE", referralReportRequest.CURRENT_PAGE.ToString());
            var recordPErPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", Value = referralReportRequest.RECORD_PER_PAGE };
            var searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = referralReportRequest.SEARCH_TEXT };
            var sortBy = new SqlParameter { ParameterName = "SORT_BY", Value = referralReportRequest.SORT_BY };
            var sortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = referralReportRequest.SORT_ORDER };
            var pRIORITY = new SqlParameter { ParameterName = "PRIORITY", Value = referralReportRequest.PRIORITY };

            var result = SpRepository<ReferralReport>.GetListWithStoreProcedure(@"exec FOX_PROC_REPORT_REFERRAL_LIST @PRACTICE_CODE,@INDEXED_DATE_FROM,
                        @INDEXED_DATE_TO,@RECEIVED_DATE_FROM,@RECEIVED_DATE_TO,@INDEXED_STATUS,@STATUS,@DOCUMENT_TYPE,@SOURCE_TYPE,@ASSIGNED_PERSON_NAME,
                        @CURRENT_PAGE,@RECORD_PER_PAGE,@SEARCH_TEXT,@SORT_BY,@SORT_ORDER,@PRIORITY", PracticeCode, indexedDateFrom, indexedDateTo, receivedDateFrom,
                        receivedDateTo, indexedStatus, status, documentType, sourceType, assignedPersonName, currentPage, recordPErPage, searchText, sortBy, sortOrder, pRIORITY);
            return result;
        }
        public string ExportToExcelReferralReport(ReferralReportRequest referralReportRequest, UserProfile profile)
        {
            try
            {
                string fileName = "Referral_Report_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                referralReportRequest.CURRENT_PAGE = 1;
                referralReportRequest.RECORD_PER_PAGE = 0;
                var CalledFrom = "Referral_Report";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<ReferralReport> result = new List<ReferralReport>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetReferralReportList(referralReportRequest, profile);
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<ReferralReport>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ApplicatinUserName> GetApplicatinUserNameList(string name, long practiceCode)
        {
            var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _name = new SqlParameter { ParameterName = "SEARCH_NAME", Value = name };

            var result = SpRepository<ApplicatinUserName>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_APPLICATION_USERS_NAME @PRACTICE_CODE,@SEARCH_NAME",
                _practiceCode, _name);
            return result;
        }


        public List<TaskListResponse> GetTaskReportList(TaskListRequest obj, UserProfile profile)
         {
            try {

                if (!string.IsNullOrEmpty(obj.DATE_FROM_STR))
                    obj.DATE_FROM = Convert.ToDateTime(obj.DATE_FROM_STR);
                if (!string.IsNullOrEmpty(obj.DATE_TO_STR))
                    obj.DATE_TO = Convert.ToDateTime(obj.DATE_TO_STR);
                if (obj.DATE_FROM.HasValue)
                    if (obj.DATE_TO.HasValue)
                        if (String.Equals(obj.DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                            obj.DATE_TO = obj.DATE_TO.Value.AddDays(1).AddSeconds(-1);
                        else
                            obj.DATE_TO = obj.DATE_TO.Value.AddSeconds(59);
                    else
                        obj.DATE_TO = Helper.GetCurrentDate();
                else if (obj.DATE_TO.HasValue)
                {
                    if (String.Equals(obj.DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                        obj.DATE_TO = obj.DATE_TO.Value.AddDays(1).AddSeconds(-1);
                    var dateNow = Helper.GetCurrentDate();
                    obj.DATE_TO = dateNow.AddYears(-100);
                }
               
                var dATE_FROM = Helper.getDBNullOrValue("DATE_FROM", obj.DATE_FROM.HasValue ? obj.DATE_FROM.Value.ToString() : "");
                var dATE_TO = Helper.getDBNullOrValue("DATE_TO", obj.DATE_TO.HasValue ? obj.DATE_TO.Value.ToString() : "");
                //var dATE_FROM = new SqlParameter { ParameterName = "DATE_FROM", SqlDbType = SqlDbType.DateTime, Value = obj.DATE_FROM };
                //var dATE_TO = new SqlParameter { ParameterName = "DATE_TO", SqlDbType = SqlDbType.DateTime, Value = obj.DATE_TO };
                var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var rOLE = new SqlParameter { ParameterName = "ROLE", SqlDbType = SqlDbType.VarChar, Value = obj.ROLE };
                var cURRENT_PAGE = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = obj.CURRENT_PAGE };
                var rECORD_PER_PAGE = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = obj.RECORD_PER_PAGE };
                var sEARCH_STRING = new SqlParameter { ParameterName = "SEARCH_STRING", SqlDbType = SqlDbType.VarChar, Value = obj.SEARCH_STRING };
                var sORT_ORDER = new SqlParameter { ParameterName = "SORT_ORDER", SqlDbType = SqlDbType.VarChar, Value = obj.SORT_ORDER };
                var sORT_BY = new SqlParameter { ParameterName = "SORT_BY", SqlDbType = SqlDbType.VarChar, Value = obj.SORT_BY };
                var result = SpRepository<TaskListResponse>.GetListWithStoreProcedure(@"exec FOX_GET_TASK_REPORT
                             @DATE_FROM,@DATE_TO,@PRACTICE_CODE,@ROLE,@CURRENT_PAGE,@RECORD_PER_PAGE,@SEARCH_STRING,@SORT_ORDER,@SORT_BY",
                            dATE_FROM, dATE_TO, _practiceCode, rOLE, cURRENT_PAGE, rECORD_PER_PAGE, sEARCH_STRING, sORT_ORDER, sORT_BY);
                if (result != null)
                    return result;
                else
                    return new List<TaskListResponse>();
            }
            catch (Exception) {
                return new List<TaskListResponse>();
            }
        }

        public string Export(TaskListRequest obj, UserProfile profile)
        {
            try
            {
                string fileName = "Task_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                obj.CURRENT_PAGE = 1;
                obj.RECORD_PER_PAGE = 1000000;
                //obj.CalledFrom = "Analysis_Report";
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
                   
                    case "Task_Report":
                        {
                            var result = GetTaskReportList(obj, profile);
                            
                            if (result.Count > 0)
                            {
                                for (int i = 0; i < result.Count; i++) {
                                    result[i].ROW = i+1;
                                    var ROLE_NAME = result[i].ROLE_NAME;
                                    var ASSIGNED_USER = result[i].ASSIGNED_USER;
                                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                                    ROLE_NAME = textInfo.ToTitleCase(ROLE_NAME.ToLower());
                                    ASSIGNED_USER = textInfo.ToTitleCase(ASSIGNED_USER.ToLower());
                                    result[i].ASSIGNED_USER = textInfo.ToTitleCase(ASSIGNED_USER.ToLower());
                                    result[i].ROLE_NAME = ROLE_NAME;
                                    result[i].ASSIGNED_USER = ASSIGNED_USER;
                                }
                            }
                            exported = ExportToExcel.CreateExcelDocument<TaskListResponse>(result, pathtowriteFile, obj.CalledFrom.Replace(' ', '_'));
                            break;
                        }
                     
                }
                return virtualPath + fileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HighBalanceReportRes> getHighBalanceReportList(HighBalanceReportReq obj, UserProfile profile)
        {
            try
            {
                var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var _search_string = new SqlParameter { ParameterName = "search_string", SqlDbType = SqlDbType.VarChar, Value = obj.SEARCH_STRING };
                var _current_page = new SqlParameter { ParameterName = "current_page", SqlDbType = SqlDbType.Int, Value = obj.CurrentPage };
                var _record_per_page = new SqlParameter { ParameterName = "record_per_page", SqlDbType = SqlDbType.Int, Value = obj.RecordPerPage };
                var _sort_by = new SqlParameter { ParameterName = "sort_by", SqlDbType = SqlDbType.VarChar, Value = obj.SORT_BY };
                var _sort_order = new SqlParameter { ParameterName = "sort_order", SqlDbType = SqlDbType.VarChar, Value = obj.SORT_ORDER };


                var result = SpRepository<HighBalanceReportRes>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_PENDING_HIGH_BALANCE_DETAILED_REPORT] @PRACTICE_CODE,@search_string,@current_page,@record_per_page,@sort_by,@sort_order",
                    _practiceCode, _search_string, _current_page, _record_per_page, _sort_by, _sort_order);
                return result;
            }
            catch (Exception EX)
            {

                throw EX;
            }
       
        }
        public string ExportToExcelHighBalanceReport(HighBalanceReportReq obj, UserProfile profile)
        {
            try
            {
                string fileName = "High_Balance_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                obj.CurrentPage = 1;
                obj.RecordPerPage = 0;
                var CalledFrom = "High_Balance_Report";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<HighBalanceReportRes> result = new List<HighBalanceReportRes>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = getHighBalanceReportList(obj, profile);
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].ROW = i + 1;
                    result[i].Patient_Balance = Math.Round(Convert.ToDecimal(result[i].Patient_Balance), 2);

                }
                exported = ExportToExcel.CreateExcelDocument<HighBalanceReportRes>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<InterfaceLogReportRes> getInterfaceLogReportList(InterfaceLogReportReq request, UserProfile profile)
        {
            try
            {
                switch (request.TIME_FRAME)
                {
                    case 1:
                        request.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                        request.DATE_TO = Helper.GetCurrentDate();
                        break;
                    case 2:
                        request.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                        request.DATE_TO = Helper.GetCurrentDate();
                        break;
                    case 3:
                        request.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                        request.DATE_TO = Helper.GetCurrentDate();
                        break;
                    case 4:
                        if (!string.IsNullOrEmpty(request.DateFromInStringDOS))
                            request.DATE_FROM = Convert.ToDateTime(request.DateFromInStringDOS);
                        else
                            request.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                        if (!string.IsNullOrEmpty(request.DateToInStringDOS))
                            request.DATE_TO = Convert.ToDateTime(request.DateToInStringDOS);
                        else
                        {
                            request.DATE_TO = Helper.GetCurrentDate();
                        }
                        break;
                    case 5:
                        request.DATE_FROM = Helper.GetCurrentDate();
                        request.DATE_TO = Helper.GetCurrentDate();
                        break;
                    default:
                        break;
                }
               
                var _paramsPracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = System.Data.SqlDbType.BigInt, Value = profile.PracticeCode };
                var _paramsSearchText = new SqlParameter { ParameterName = "OPTION", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.TYPE };
                var _paramsOption = new SqlParameter { ParameterName = "SEARCH_STRING", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.SEARCH_STRING };
                var _paramsCurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.CurrentPage };
                var _paramsRecordsPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.RecordPerPage };
                var _sortBy = new SqlParameter { ParameterName = "SORT_BY", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.SORT_BY };
                var _soryOrder = new SqlParameter { ParameterName = "SORT_ORDER", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.SORT_ORDER };
                var _paramsSTATUS = new SqlParameter { ParameterName = "STATUS", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.STATUS };
                var _paramsAPPLICATION = new SqlParameter { ParameterName = "APPLICATION", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.Application };
                var dateFrom = Helper.getDBNullOrValue("DATE_FROM", request.DATE_FROM.ToString());
                var dateTo = Helper.getDBNullOrValue("DATE_TO", request.DATE_TO.ToString());

                var res = SpRepository<InterfaceLogReportRes>.GetListWithStoreProcedure(@" exec [FOX_PROC_GET_INTERFACE_LOGS_REPORT]
                      @PRACTICE_CODE, @OPTION, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER, @STATUS,@APPLICATION, @DATE_FROM, @DATE_TO",
                          _paramsPracticeCode, _paramsOption, _paramsSearchText, _paramsCurrentPage, _paramsRecordsPerPage, _sortBy, _soryOrder, _paramsSTATUS, _paramsAPPLICATION, dateFrom, dateTo);
                return res;
            }
            catch (Exception EX)
            {

                throw EX;
            }

        }
        public List<PHRReportRes> getPHRReportList(PHRReportReq request, UserProfile profile)
        {
            try
            {
                var _paramsPracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = System.Data.SqlDbType.BigInt, Value = profile.PracticeCode };
                var _paramsSearchText = new SqlParameter { ParameterName = "PATIENT_STATUS", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.Patient_STATUS };
                var _paramsOption = new SqlParameter { ParameterName = "SEARCH_STRING", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.SEARCH_STRING };
                var _paramsCurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.CurrentPage };
                var _paramsRecordsPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.RecordPerPage };
                var _sortBy = new SqlParameter { ParameterName = "SORT_BY", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.SORT_BY };
                var _soryOrder = new SqlParameter { ParameterName = "SORT_ORDER", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.SORT_ORDER };
                var _paramsSTATUS = new SqlParameter { ParameterName = "INVITATION_STATUS", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.Invitation_STATUS };
               
                var res = SpRepository<PHRReportRes>.GetListWithStoreProcedure(@" exec [FOX_PROC_GET_FOX_PHR_REPORT]
                      @PRACTICE_CODE, @PATIENT_STATUS, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER, @INVITATION_STATUS",
                          _paramsPracticeCode, _paramsOption, _paramsSearchText, _paramsCurrentPage, _paramsRecordsPerPage, _sortBy, _soryOrder, _paramsSTATUS);
                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
                TextInfo text_info = culture_info.TextInfo;
                for (int i = 0; i < res.Count(); i++)
                {

                    res[i].FIRST_NAME = text_info.ToTitleCase(res[i].FIRST_NAME);
                    res[i].LAST_NAME = text_info.ToTitleCase(res[i].LAST_NAME.ToLower());
                    res[i].EMAIL = text_info.ToLower(res[i].EMAIL);
                    res[i].ROW = i + 1;
                }
                return res;
            }
            catch (Exception EX)
            {

                throw EX;
            }
        }

        public string ExportToExcelInterfaceLogReport(InterfaceLogReportReq obj, UserProfile profile)
        {
            try
            {
                string fileName = "Interface_Log_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                obj.CurrentPage = 1;
                obj.RecordPerPage = 0;
                var CalledFrom = "Interface_Log_Report";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<InterfaceLogReportRes> result = new List<InterfaceLogReportRes>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = getInterfaceLogReportList(obj, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;
                }
                exported = ExportToExcel.CreateExcelDocument<InterfaceLogReportRes>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExportToExcelRequestToPHRReport(PHRReportReq obj, UserProfile profile)
        {
            try
            {
                string fileName = "PHR_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                obj.CurrentPage = 1;
                obj.RecordPerPage = 0;
                var CalledFrom = "PHR_Report";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<PHRReportRes> result = new List<PHRReportRes>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = getPHRReportList(obj, profile);
                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
                TextInfo text_info = culture_info.TextInfo;
                for (int i = 0; i < result.Count(); i++)
                {

                    result[i].FIRST_NAME = text_info.ToTitleCase(result[i].FIRST_NAME);
                    result[i].LAST_NAME = text_info.ToTitleCase(result[i].LAST_NAME.ToLower());
                    result[i].EMAIL = text_info.ToLower(result[i].EMAIL);
                     result[i].ROW = i + 1;
                }
                exported = ExportToExcel.CreateExcelDocument<PHRReportRes>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PHRUserLastLoginReponse> GetPHRUsersLoginList(PHRUserLastLoginRequest request, UserProfile profile)
        {
            try
            {
                request.DATE_TO = Helper.GetCurrentDate();

                switch(request.TIME_FRAME)
                {
                    case 1:
                        request.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                        break;
                    case 2:
                        request.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                        break;
                    case 3:
                        request.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                        break;
                    case 4:
                        if (!string.IsNullOrEmpty(request.DATE_FROM_STR))
                            request.DATE_FROM = Convert.ToDateTime(request.DATE_FROM_STR);
                        else
                            request.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                        if (!string.IsNullOrEmpty(request.DATE_TO_STR))
                            request.DATE_TO = Convert.ToDateTime(request.DATE_TO_STR);
                        break;
                    default:
                        break;
                }

                var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var dateFrom = Helper.getDBNullOrValue("DATE_FROM", request.DATE_FROM.ToString());
                var dateTo= Helper.getDBNullOrValue("DATE_TO", request.DATE_TO.ToString());
                var currentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = request.CURRENT_PAGE };
                var recordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = request.RECORD_PER_PAGE };
                var searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", SqlDbType = SqlDbType.VarChar, Value = request.SEARCH_TEXT };
                var sortBy = new SqlParameter { ParameterName = "SORT_BY", Value = request.SORT_BY };
                var sortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = request.SORT_ORDER };
                var result = SpRepository<PHRUserLastLoginReponse>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_PHR_LAST_LOGIN_USER_REPORT  @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORT_BY, @SORT_ORDER", practiceCode, dateFrom, dateTo, currentPage, recordPerPage, searchText, sortBy, sortOrder);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string ExportPHRUserLastLoginReport(PHRUserLastLoginRequest request, UserProfile profile)
        {
            try
            {
                string fileName = "PHR_Users_Last_Login_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                var CalledFrom = "PHR_Users_Last_Login_Report";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<PHRUserLastLoginReponse> result = new List<PHRUserLastLoginReponse>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetPHRUsersLoginList(request, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;
                }
                exported = ExportToExcel.CreateExcelDocument<PHRUserLastLoginReponse>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}