using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.SearchOrderModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace FOX.BusinessOperations.SearchOrderServices
{
    public class SearchOrderServices : ISearchOrderServices
    {
        private long retrycatch = 0;
        private readonly DbContextDashboard _SearchOrderContext = new DbContextDashboard();
        private readonly GenericRepository<SearchOrder> _SearchOrderRepository;

        public SearchOrderServices()
        {
            _SearchOrderRepository = new GenericRepository<SearchOrder>(_SearchOrderContext);
        }

        public List<SearchOrder> GetSearchOrder(SearchOrderRequest searchOrderRequest, UserProfile profile)
        {
            try {
                if (!string.IsNullOrEmpty(searchOrderRequest.Date_Of_Birth_In_String))
                    searchOrderRequest.Date_Of_Birth = Convert.ToDateTime(searchOrderRequest.Date_Of_Birth_In_String);

                if (!string.IsNullOrEmpty(searchOrderRequest.RECEIVE_FROM_In_String))
                    searchOrderRequest.RECEIVE_FROM = Convert.ToDateTime(searchOrderRequest.RECEIVE_FROM_In_String);

                if (!string.IsNullOrEmpty(searchOrderRequest.RECEIVE_TO_In_String))
                    searchOrderRequest.RECEIVE_TO = Convert.ToDateTime(searchOrderRequest.RECEIVE_TO_In_String);

                if (!string.IsNullOrEmpty(searchOrderRequest.RECEIVE_FROM_Time_In_String))
                {
                    var timestr = searchOrderRequest.RECEIVE_FROM_Time_In_String.Split(' ')[0];
                    searchOrderRequest.RECEIVE_FROM_TIME = DateTime.ParseExact(timestr, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);

                    searchOrderRequest.RECEIVE_FROM = searchOrderRequest.RECEIVE_FROM + searchOrderRequest.RECEIVE_FROM_TIME.Value.TimeOfDay;
                }

                if (!string.IsNullOrEmpty(searchOrderRequest.RECEIVE_TO_Time_In_String))
                {
                    var timestr = searchOrderRequest.RECEIVE_TO_Time_In_String.Split(' ')[0];
                    searchOrderRequest.RECEIVE_TO_TIME = DateTime.ParseExact(timestr, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);

                    searchOrderRequest.RECEIVE_TO = searchOrderRequest.RECEIVE_TO + searchOrderRequest.RECEIVE_TO_TIME.Value.TimeOfDay;

                }



                if (searchOrderRequest.RECEIVE_FROM.HasValue)
                {
                    if (searchOrderRequest.RECEIVE_TO.HasValue)
                    {
                        if (String.Equals(searchOrderRequest.RECEIVE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                        {
                            searchOrderRequest.RECEIVE_TO = searchOrderRequest.RECEIVE_TO.Value.AddDays(1).AddSeconds(-1);
                        }
                        else
                        {
                            searchOrderRequest.RECEIVE_TO = searchOrderRequest.RECEIVE_TO.Value.AddSeconds(59);
                        }
                    }
                    else
                        searchOrderRequest.RECEIVE_TO = Helper.GetCurrentDate();
                }
                var patient_account = Helper.getDBNullOrValue("@PATIENT_ACCOUNT", searchOrderRequest.Patient_Account.Trim());
                var mrn = Helper.getDBNullOrValue("@MRN", searchOrderRequest.MRN.Trim());
                var firstName = Helper.getDBNullOrValue("@FIRST_NAME", searchOrderRequest.First_Name.Trim());
                var lastName = Helper.getDBNullOrValue("@LAST_NAME", searchOrderRequest.Last_Name.Trim());
                var SSN = Helper.getDBNullOrValue("@SSN", searchOrderRequest.SSN);
                var dateOfBrith = Helper.getDBNullOrValue("@DATE_OF_BRITH", searchOrderRequest.Date_Of_Birth.ToString());
                var sourceName = Helper.getDBNullOrValue("@SOURCE_NAME", searchOrderRequest.SOURCE_NAME);
                var referralRegion = new SqlParameter { ParameterName = "@REFERRAL_REGION", Value = searchOrderRequest.REGION_CODE ?? "" };
                var senderFirstName = Helper.getDBNullOrValue("@SENDER_FIRST_NAME", searchOrderRequest.SENDER_FIRST_NAME);
                var senderLastName = Helper.getDBNullOrValue("@SENDER_LAST_NAME", searchOrderRequest.SENDER_LAST_NAME);
                var receiveFrom = Helper.getDBNullOrValue("@RECEIVED_FROM", searchOrderRequest.RECEIVE_FROM.ToString());
                var receiveTo = Helper.getDBNullOrValue("@RECEIVED_TO", searchOrderRequest.RECEIVE_TO.ToString());
                var paracticCode = new SqlParameter { ParameterName = "PRACTICE_CODE", Value = profile.PracticeCode };
                var currentPage = Helper.getDBNullOrValue("@CURRENT_PAGE", searchOrderRequest.CurrentPage.ToString());
                var recordPErPage = Helper.getDBNullOrValue("@RECORD_PER_PAGE", searchOrderRequest.RecordPerPage.ToString());
                var searchText = new SqlParameter { ParameterName = "@SEARCH_TEXT", Value = searchOrderRequest.SearchText };
                var documentType = new SqlParameter { ParameterName = "@DOCUMENT_TYPE", Value = searchOrderRequest.DOCUMENT_TYPE };

                var SortBy = Helper.getDBNullOrValue("@SORT_BY", searchOrderRequest.SortBy);
                var SortOrder = Helper.getDBNullOrValue("@SORT_ORDER", searchOrderRequest.SortOrder);

                var result = SpRepository<SearchOrder>.GetListWithStoreProcedure(@"exec FOX_PROC_PATIENT_SEARCH_ORDER @PATIENT_ACCOUNT, @MRN, @FIRST_NAME, @LAST_NAME, @SSN, @DATE_OF_BRITH, @SOURCE_NAME, @REFERRAL_REGION, @SENDER_FIRST_NAME, @SENDER_LAST_NAME, @RECEIVED_FROM, @RECEIVED_TO, @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT,@DOCUMENT_TYPE,@SORT_BY,@SORT_ORDER",
                    patient_account, mrn, firstName, lastName, SSN, dateOfBrith, sourceName, referralRegion, senderFirstName, senderLastName, receiveFrom, receiveTo, paracticCode, currentPage, recordPErPage, searchText, documentType, SortBy, SortOrder);
                return result;

            }
            catch (Exception ex) {
                if (retrycatch <= 2 && (!string.IsNullOrEmpty(ex.Message) &&
                               ex.Message.Contains("deadlocked on lock resources with another process"))
                               || ((ex.InnerException != null) &&
                               !string.IsNullOrEmpty(ex.InnerException.Message)
                               &&
                               ex.InnerException.Message.Contains("deadlocked on lock resources with another process")))
            {
                retrycatch = retrycatch + 1;
                return GetSearchOrder(searchOrderRequest, profile);
            }
                else
            {
                throw ex;
            }
        }
    }

    public string ExportToExcelSearchOrder(SearchOrderRequest req, UserProfile profile)
        {
            try
            {
                string fileName = "Search_Order_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CurrentPage = 1;
                req.RecordPerPage = 0;
                var CalledFrom = "Search_Order";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<SearchOrder> result = new List<SearchOrder>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetSearchOrder(req, profile);
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<SearchOrder>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'), profile.isTalkRehab);
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}