using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Reconciliation;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace FOX.BusinessOperations.ReconciliationService
{
    public class ReconciliationService : IReconciliationService
    {
        private readonly DBContextReconciliations _reconciliationCPContext = new DBContextReconciliations();
        private readonly DBContextHrAutoEmails _hrAutoEmailsContext = new DBContextHrAutoEmails();
        private readonly DbContextPatient _PatientContext = new DbContextPatient();
        private readonly DbContextSecurity _userContext = new DbContextSecurity();
        private readonly GenericRepository<ReconciliationStatus> _reconciliationStatusRepository;
        private readonly GenericRepository<ReconciliationDepositType> _reconciliationDepositTypeRepository;
        private readonly GenericRepository<ReconciliationCategory> _reconciliationCategoryRepository;
        private readonly GenericRepository<ReconciliationFiles> _reconciliationFilesRepository;
        private readonly GenericRepository<ReconciliationCP> _reconciliationCPRepository;
        private readonly GenericRepository<ReconciliationCPLogs> _reconciliationCPLogsRepository;
        private readonly GenericRepository<User> _userRepository;
        private readonly GenericRepository<FoxInsurancePayers> _foxInsuranceRepository;
        private readonly GenericRepository<MTBC_Credentials_Fox_Automation> _foxhrautoemailsRepository;
        public ReconciliationService()
        {
            _reconciliationStatusRepository = new GenericRepository<ReconciliationStatus>(_reconciliationCPContext);
            _reconciliationDepositTypeRepository = new GenericRepository<ReconciliationDepositType>(_reconciliationCPContext);
            _reconciliationCategoryRepository = new GenericRepository<ReconciliationCategory>(_reconciliationCPContext);
            _reconciliationFilesRepository = new GenericRepository<ReconciliationFiles>(_reconciliationCPContext);
            _reconciliationCPRepository = new GenericRepository<ReconciliationCP>(_reconciliationCPContext);
            _reconciliationCPLogsRepository = new GenericRepository<ReconciliationCPLogs>(_reconciliationCPContext);
            _userRepository = new GenericRepository<User>(_userContext);
            _foxInsuranceRepository = new GenericRepository<FoxInsurancePayers>(_PatientContext);
            _foxhrautoemailsRepository = new GenericRepository<MTBC_Credentials_Fox_Automation>(_hrAutoEmailsContext);
        }
       
        public List<ReconciliationStatus> GetReconciliationStatuses(UserProfile profile)
        {
            try
            {
                return SpRepository<ReconciliationStatus>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_STATUSES_TEST @PRACTICE_CODE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<ReconciliationDepositType> GetDepositTypes(UserProfile profile)
        {
            try
            {
                return SpRepository<ReconciliationDepositType>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_DEPOSIT_TYPES_TEST @PRACTICE_CODE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<FOX_TBL_RECONCILIATION_REASON> GetReasons(UserProfile profile)
        {
            try
            {
                return SpRepository<FOX_TBL_RECONCILIATION_REASON>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_REASONS_test @PRACTICE_CODE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<ReconciliationCategory> GetReconciliationCategories(UserProfile profile)
        {
            try
            {
                return SpRepository<ReconciliationCategory>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_CATEGORIES_TEST @PRACTICE_CODE"
                     , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }
        public List<ReconciliationCP> GetReconciliationInsurances(UserProfile profile)
        {
            try
            {
                return SpRepository<ReconciliationCP>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_INSURANCE @PRACTICE_CODE"
                       , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });

            }
            catch (Exception ex) { throw ex; }
        }

        public ReconsiliationCategoryDepositType GetReconsiliationCategoryDepositTypes(UserProfile profile)
        {
            List<ReconciliationCategory> _reconsiliationCategory = GetReconciliationCategories(profile);
            List<ReconciliationDepositType> _reconsiliationDepositType = GetDepositTypes(profile);
            ReconsiliationCategoryDepositType _reconsiliationCategoryDepositType = new ReconsiliationCategoryDepositType { Category = _reconsiliationCategory, DepositType = _reconsiliationDepositType };
            return _reconsiliationCategoryDepositType;
        }

        public List<CheckNoSelectionModel> GetReconciliationCheckNos(UserProfile profile)
        {
            try
            {
                return SpRepository<CheckNoSelectionModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_CHECK_NOS @PRACTICE_CODE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<AmountSelectionModel> GetAmounts(UserProfile profile)
        {
            try
            {
                return SpRepository<AmountSelectionModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_AMOUNTS @PRACTICE_CODE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<AmountPostedSelectionModel> GetPostedAmounts(UserProfile profile)
        {
            try
            {
                return SpRepository<AmountPostedSelectionModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_AMOUNT_POSTED @PRACTICE_CODE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<AmountNotPostedSelectionModel> GetNotPostedAmounts(UserProfile profile)
        {
            try
            {
                return SpRepository<AmountNotPostedSelectionModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_AMOUNT_NOT_POSTED @PRACTICE_CODE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<UsersForDropdown> GetUsersForDD(UserProfile profile)
        {
            try
            {
                return SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USERS_FOR_RECONCILIATION @PRACTICE_CODE, @CURRENT_USER, @ROLE_ID"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter("@CURRENT_USER", SqlDbType.VarChar) { Value = profile.UserName }
                    , new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.BigInt, Value = profile.RoleId }
                    );

            }
            catch (Exception ex) { throw ex; }
        }

        public List<ReconciliationCP> GetReconciliationsCP(ReconciliationCPSearchReq searchReq, UserProfile profile)
        {
            try
            {
                bool hasDOSFromDate = false;
                bool hasDOSToDate = false;
                //if (!string.IsNullOrEmpty(searchReq.DATE_FROM_Str))
                //{
                //    hasDOSFromDate = true;
                //    searchReq.DATE_FROM = Convert.ToDateTime(searchReq.DATE_FROM_Str);
                //}

                //if (!string.IsNullOrEmpty(searchReq.DATE_TO_Str))
                //{
                //    hasDOSToDate = true;
                //    searchReq.DATE_TO = Convert.ToDateTime(searchReq.DATE_TO_Str);
                //}
                DateTime dateTimeTo;
                DateTime dateTimeFrom;
                if (DateTime.TryParse(searchReq.DATE_FROM_Str, out dateTimeFrom))
                {
                    hasDOSFromDate = true;
                    searchReq.DATE_FROM = dateTimeFrom;
                }
                if (DateTime.TryParse(searchReq.DATE_TO_Str, out dateTimeTo))
                {
                    hasDOSToDate = true;
                    searchReq.DATE_TO = dateTimeTo;
                }
                //string cat_Ids = "";
                //string deposit_Type_Ids = "";
                //string check_Nos = "";
                //string amounts = "";
                //string amounts_Posted = "";
                //string amounts_Not_Posted = "";

                //@PRACTICE_CODE, @IS_FOR_REPORT, @IS_DEPOSIT_DATE_SEARCH, @IS_ASSIGNED_DATE_SEARCH, @DATE_FROM, @DATE_TO, @FOX_TBL_INSURANCE_ID, @CATEGORY_IDS, 
                //    @STATUS_ID, @DEPOSIT_TYPE_IDS, @CHECK_NOS, @AMOUNT, @AMOUNT_POSTED, @AMOUNT_NOT_POSTED, @CURRENT_USER, @SEARCH_TEXT, @CURRENT_PAGE, @RECORD_PER_PAGE"
                var result = SpRepository<ReconciliationCP>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATIONS_CP_TEST 
                    @PRACTICE_CODE, @IS_FOR_REPORT, @IS_DEPOSIT_DATE_SEARCH, @IS_ASSIGNED_DATE_SEARCH, @DATE_FROM, @DATE_TO, @FOX_TBL_INSURANCE_NAME, 
                    @STATUS_ID, @CURRENT_USER, @SEARCH_TEXT, @CURRENT_PAGE, @RECORD_PER_PAGE,@SORT_BY ,@SORT_ORDER, @CP_TYPE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "IS_FOR_REPORT", SqlDbType = SqlDbType.Bit, Value = searchReq.IsForReport }
                    , new SqlParameter { ParameterName = "IS_DEPOSIT_DATE_SEARCH", SqlDbType = SqlDbType.Bit, Value = searchReq.IS_DEPOSIT_DATE_SEARCH }
                    , new SqlParameter { ParameterName = "IS_ASSIGNED_DATE_SEARCH", SqlDbType = SqlDbType.Bit, Value = searchReq.IS_ASSIGNED_DATE_SEARCH }
                    , CommonService.Helper.getDBNullOrValue("DATE_FROM", hasDOSFromDate ? (searchReq.DATE_FROM.HasValue ? searchReq.DATE_FROM.Value.ToString("MM/dd/yyyy") : "") : "")
                    , CommonService.Helper.getDBNullOrValue("DATE_TO", hasDOSToDate ? (searchReq.DATE_TO.HasValue ? searchReq.DATE_TO.Value.ToString("MM/dd/yyyy") : "") : "")
                    // , CommonService.Helper.getDBNullOrValue("FOX_TBL_INSURANCE_ID", searchReq.FOX_TBL_INSURANCE_ID.HasValue ? searchReq.FOX_TBL_INSURANCE_ID.ToString() : "")
                    , new SqlParameter("FOX_TBL_INSURANCE_NAME", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.INSURANCE_NAME) ? "" : searchReq.INSURANCE_NAME }
                    //, CommonService.Helper.getDBNullOrValue("CATEGORY_IDS", cat_Ids)
                    //, CommonService.Helper.getDBNullOrValue("STATUS_ID", string.IsNullOrEmpty( searchReq.STATUS_ID) ? "" : searchReq.STATUS_ID)
                    , CommonService.Helper.getDBNullOrValue("STATUS_ID", "")
                    //, CommonService.Helper.getDBNullOrValue("DEPOSIT_TYPE_IDS", deposit_Type_Ids)
                    //, CommonService.Helper.getDBNullOrValue("CHECK_NOS", check_Nos)
                    //, CommonService.Helper.getDBNullOrValue("AMOUNT", check_Nos)
                    , new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = profile.UserName }
                    , new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.SearchString) ? "" : searchReq.SearchString }
                    , new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = searchReq.CurrentPage }
                    , new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = 0 }
                    , new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = searchReq.SORT_BY }
                    , new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = searchReq.SORT_ORDER }
                    , new SqlParameter("CP_TYPE", SqlDbType.VarChar) { Value = searchReq.CP_Type }
                    );

                if (result != null && result.Count > 0)
                {
                    //result.Select(d => new
                    //{
                    //    d.DEPOSIT_DATE.Value.Year,
                    //    d.DEPOSIT_DATE.Value.Month,
                    //    d.DEPOSIT_DATE.Value.Day,
                    //    FormattedDate = d.DEPOSIT_DATE.Value.ToString("yyyy-MMM")
                    //})                    
                    //.OrderByDescending(d => d.Year)
                    //.ThenByDescending(d => d.Month)
                    //.ThenByDescending(d => d.Day)
                    //.Select(d => d.FormattedDate);
                    //result.OrderByDescending(o => o.DEPOSIT_DATE)
                    //    .OrderByDescending(o => o.ye);
                    if (searchReq.CATEGORIES?.Count > 0)
                    {
                        var cats = searchReq.CATEGORIES.Where(e => e.Selected).Select(e=>e.CATEGORY_ID).ToList();
                        if (cats.Count > 0)
                            result = result.FindAll(e => e.CATEGORY_ID.HasValue && cats.Contains(e.CATEGORY_ID.Value));

                        //cat_Ids = string.Join(",", searchReq.CATEGORY_IDS.Select(n => n.ToString()).ToArray());
                        //bool isFirst = true;
                        //foreach (var item in searchReq.CATEGORY_IDS)
                        //{
                        //    if (isFirst)
                        //    {
                        //        isFirst = false;
                        //        cat_Ids += item.ToString();
                        //    }
                        //    else {
                        //        cat_Ids += "," + item.ToString();
                        //    }
                        //}
                    }
                    if (searchReq.FOX_TBL_INSURANCE_NAME?.Count > 0)
                    {
                        var cats = searchReq.FOX_TBL_INSURANCE_NAME.Where(e => e.Selected).Select(e => e.FOX_TBL_INSURANCE_NAME).ToList();
                        if (cats.Count > 0)
                        {
                            result = result.FindAll(e => cats.Contains(e.INSURANCE_NAME));
                        }
                    }
                    if (searchReq.DEPOSIT_TYPES?.Count > 0)
                    {
                        var dts = searchReq.DEPOSIT_TYPES.Where(e => e.Selected).Select(e => e.DEPOSIT_TYPE_ID).ToList();
                        if(dts.Count > 0)
                            result = result.FindAll(e => e.DEPOSIT_TYPE_ID.HasValue && dts.Contains(e.DEPOSIT_TYPE_ID.Value));

                        //deposit_Type_Ids = string.Join(",", searchReq.DEPOSIT_TYPE_IDS.Select(n => n.ToString()).ToArray());
                    }

                    if (searchReq.Statuses?.Count > 0)
                    {
                        var rs = searchReq.Statuses.Where(e => e.Selected).Select(e => e.STATUS_NAME).ToList();
                        if (rs?.Count > 0)
                        {
                            result = result.FindAll(e =>  rs.Contains( e.STATUS_NAME));
                        }
                    }

                    if (searchReq.CHECK_NOS?.Count > 0)
                    {
                        var cnos = searchReq.CHECK_NOS.Where(e => e.Selected).Select(e => e.Value).ToList();
                        if (cnos.Count > 0)
                            result = result.FindAll(e => !string.IsNullOrWhiteSpace(e.CHECK_NO) && cnos.Contains(e.CHECK_NO));

                        //check_Nos = string.Join(",", searchReq.CHECK_NO.Select(n => n).ToArray());
                    }

                    if (searchReq.AMOUNTS?.Count > 0)
                    {
                        var ams = searchReq.AMOUNTS.Where(e => e.Selected).Select(e => e.Value).ToList();
                        if (ams.Count > 0)
                            result = result.FindAll(e => e.AMOUNT.HasValue && ams.Contains(e.AMOUNT.Value));

                        //amounts = string.Join(",", searchReq.AMOUNT.Select(n => n.ToString()).ToArray());
                    }

                    if (searchReq.AMOUNTS_POSTED?.Count > 0)
                    {
                        var amsp = searchReq.AMOUNTS_POSTED.Where(e => e.Selected).Select(e => e.Value).ToList();
                        if (amsp.Count > 0)
                            result = result.FindAll(e => e.AMOUNT_POSTED.HasValue && amsp.Contains(e.AMOUNT_POSTED.Value));

                        //amounts_Posted = string.Join(",", searchReq.AMOUNT_POSTED.Select(n => n.ToString()).ToArray());
                    }

                    if (searchReq.AMOUNTS_NOT_POSTED?.Count > 0)
                    {
                        var amsnp = searchReq.AMOUNTS_NOT_POSTED.Where(e => e.Selected).Select(e => e.Value).ToList();
                        if (amsnp.Count > 0)
                            result = result.FindAll(e => e.AMOUNT_NOT_POSTED.HasValue && amsnp.Contains(e.AMOUNT_NOT_POSTED.Value));

                        //amounts_Not_Posted = string.Join(",", searchReq.AMOUNT_NOT_POSTED.Select(n => n.ToString()).ToArray());
                    }

                    //if (!searchReq.IsForReport)
                    //{
                    //    result = result.FindAll(e => e.STATUS_NAME.ToLower() != "completed");
                    //}

                    // if (profile.ROLE_NAME.ToLower() != 'Administrator' )


                    if (result?.Count > 0)
                    {
                        int  CURRENT_PAGE = searchReq.CurrentPage - 1;
                        int START_FROM  = CURRENT_PAGE * searchReq.RecordPerPage;
                        result[0].TOTAL_RECORDS = result.Count;
                        result.Select(c => { c.TOTAL_RECORDS = result.Count; return c; }).ToList();

                        if (result.Count <= searchReq.RecordPerPage)
                        {
                            result.Select(c => { c.TOTAL_RECORD_PAGES = 1; return c; }).ToList();
                            //result.Select(r => r.TOTAL_RECORDS = 1)
                            //result[0].TOTAL_RECORD_PAGES = 1;
                        }
                        else
                        {
                            result.Select(c => { c.TOTAL_RECORD_PAGES = Math.Ceiling(((double)result.Count / (double)searchReq.RecordPerPage)); return c; }).ToList();
                            //result[0].TOTAL_RECORD_PAGES = Math.Ceiling(((double)result.Count /(double)searchReq.RecordPerPage));
                        }
                        decimal totalAmount = result.Sum(item => item.AMOUNT.HasValue ? item.AMOUNT.Value : 0);
                        decimal totalNotPostedAmount = result.Sum(item => item.AMOUNT_NOT_POSTED.HasValue ? item.AMOUNT_NOT_POSTED.Value : 0);
                        decimal totalPostedAmount = result.Sum(item => item.AMOUNT_POSTED.HasValue ? item.AMOUNT_POSTED.Value : 0);
                        result.Select(c => { c.TOTAL_AMOUNT = totalAmount; return c; }).ToList();
                        //result[0].TOTAL_AMOUNT = result.Sum(item => item.AMOUNT.HasValue ? item.AMOUNT.Value : 0);
                        result.Select(c => { c.TOTAL_POSTED_AMOUNT = totalPostedAmount; return c; }).ToList();
                        //result[0].TOTAL_POSTED_AMOUNT = result.Sum(item => item.AMOUNT_POSTED.HasValue ? item.AMOUNT_POSTED.Value : 0);
                        result.Select(c => { c.TOTAL_UNPOSTED_AMOUNT = totalNotPostedAmount; return c; }).ToList();
                        SetListSerialNumber(result);
                        //result[0].TOTAL_UNPOSTED_AMOUNT = result.Sum(item => item.AMOUNT_NOT_POSTED.HasValue ? item.AMOUNT_NOT_POSTED.Value : 0);
                        //result[0].TOTAL_UNPOSTED_AMOUNT = result.Sum(item => item.AMOUNT.HasValue ? item.AMOUNT.Value : 0) - result.Sum(item => item.AMOUNT_POSTED.HasValue ? item.AMOUNT_POSTED.Value : 0); 
                        result = result.FindAll(e => e.Row_No > START_FROM && e.Row_No <= (searchReq.CurrentPage * searchReq.RecordPerPage));
                    }
                }
                //return result.OrderByDescending(o => o.CREATED_DATE)
                //    .ThenByDescending(o => o.DEPOSIT_DATE)
                //    .ThenByDescending(o => o.ASSIGNED_DATE)
                //    .ThenByDescending(o => o.COMPLETED_DATE).ToList();
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        private void SetListSerialNumber(List<ReconciliationCP> lst)
        {
            int i = 1;
            foreach(ReconciliationCP obj in lst)
            {
                obj.Row_No = i++ ;
            }
        }

        public List<ReconciliationCPLogs> GetReconciliationLogs(ReconciliationCPLogSearchReq searchReq, UserProfile profile)
        {
            try
            {
                var result = SpRepository<ReconciliationCPLogs>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_CP_LOGS_TEST @PRACTICE_CODE, @RECONCILIATION_CP_ID, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE,@LOG_DETAIL,@REMARK_DETAIL",
                    new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "RECONCILIATION_CP_ID", SqlDbType = SqlDbType.BigInt, Value = searchReq.RECONCILIATION_CP_ID }
                    , new SqlParameter("SEARCH_STRING", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.SearchString) ? "" : searchReq.SearchString }
                    , new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = searchReq.CurrentPage }
                    , new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = 0 }
                    , new SqlParameter("LOG_DETAIL", SqlDbType.Bit) { Value = searchReq.LogDetail }
                    , new SqlParameter("REMARK_DETAIL", SqlDbType.Bit) { Value = searchReq.RemarkDetail }
                    );
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<ReconciliationCP> SaveAutoReconciliationCP(ReconciliationCP autoreconciliationToSave, UserProfile profile)
        {
            try
            {
                var dBData = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == autoreconciliationToSave.RECONCILIATION_CP_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                ReconciliationCP prevObj = null;
                prevObj = (ReconciliationCP)dBData.Clone();
                var result = SpRepository<ReconciliationCP>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_AUTO_RECONCILIATION_CP @PRACTICE_CODE,@USER_NAME,@RECONCILIATION_CP_ID,@AMOUNT_POSTED",
                    new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "USER_NAME", SqlDbType = SqlDbType.VarChar, Value = profile.UserName }
                    , new SqlParameter { ParameterName = "RECONCILIATION_CP_ID", SqlDbType = SqlDbType.BigInt, Value = autoreconciliationToSave.RECONCILIATION_CP_ID }
                    , new SqlParameter { ParameterName = "AMOUNT_POSTED", SqlDbType = SqlDbType.BigInt, Value = autoreconciliationToSave.AMOUNT_POSTED }
                    );
                if(result != null)
                {
                    SaveAutoReconciliationLogs(prevObj, autoreconciliationToSave, profile);
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
        public List<ReconciliationCP> SaveManualReconciliationCP(ReconciliationCP manualreconciliationToSave, UserProfile profile)
        {
            try
            {
                if (!string.IsNullOrEmpty(manualreconciliationToSave.POSTED_DATE_STR))
                {
                    manualreconciliationToSave.DATE_POSTED = Convert.ToDateTime(manualreconciliationToSave.POSTED_DATE_STR);
                }
                var dBData = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == manualreconciliationToSave.RECONCILIATION_CP_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                ReconciliationCP prevObj = null;
                prevObj = (ReconciliationCP)dBData.Clone();
                var result = SpRepository<ReconciliationCP>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_AUTO_RECONCILIATION_CP_TEST @PRACTICE_CODE,@USER_NAME,@RECONCILIATION_CP_ID,@AMOUNT_POSTED, @DATE_POSTED",
                    new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "USER_NAME", SqlDbType = SqlDbType.VarChar, Value = profile.UserName }
                    , new SqlParameter { ParameterName = "RECONCILIATION_CP_ID", SqlDbType = SqlDbType.BigInt, Value = manualreconciliationToSave.RECONCILIATION_CP_ID }
                    , new SqlParameter { ParameterName = "AMOUNT_POSTED", SqlDbType = SqlDbType.Decimal, Value = manualreconciliationToSave.AMOUNT_POSTED }
                     , new SqlParameter { ParameterName = "DATE_POSTED", SqlDbType = SqlDbType.VarChar, Value = manualreconciliationToSave.DATE_POSTED.ToString() }
                    );
                if (result != null)
                {
                    SaveManualReconciliationLogs(prevObj, manualreconciliationToSave, profile);
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>Update reconciliation record on Auto Reconcilied Button </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>ReconciliationCP(list of check# which is reconcilied), user(Get Current user info), </param>
        public List<ReconciliationCP> UpdateAutoReconciliationCP(ReconciliationCP autoReconciliationToUpdate, UserProfile profile)
        {
            try
            {
                var dBData = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == autoReconciliationToUpdate.RECONCILIATION_CP_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                ReconciliationCP prevObj = null;
                if(dBData!= null) {
                    prevObj = (ReconciliationCP)dBData.Clone();
                }
                var result = SpRepository<ReconciliationCP>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_UPDATE_AUTO_RECONCILIATION_CP_TEST @PRACTICE_CODE,@USER_NAME,@RECONCILIATION_CP_ID,@AMOUNT_POSTED",
                    new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "USER_NAME", SqlDbType = SqlDbType.VarChar, Value = profile.UserName }
                    , new SqlParameter { ParameterName = "RECONCILIATION_CP_ID", SqlDbType = SqlDbType.BigInt, Value = autoReconciliationToUpdate.RECONCILIATION_CP_ID }
                    , new SqlParameter { ParameterName = "AMOUNT_POSTED", SqlDbType = SqlDbType.BigInt, Value = autoReconciliationToUpdate.AMOUNT_POSTED }
                    );
                if (result != null)
                {
                    UpdateAutoReconciliationLogs(prevObj, autoReconciliationToUpdate, profile);
                }
                else
                {
                   result = new List<ReconciliationCP>();
                }
                return result;
            }
            catch (Exception ex) { throw; }
        }

        /// <summary>Update reconciliation logs remarks </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>ReconciliationCP(get data from reporistry) ,ReconciliationCP(list of check# which is reconcilied), user(Get Current user info), </param>
        public void UpdateAutoReconciliationLogs(ReconciliationCP dbData, ReconciliationCP autoReconciliationToUpdate, UserProfile profile)
        {
            try
            {
                autoReconciliationToUpdate.REMARKS = "Total Posted amount loaded from Websoft against the listed Check# with +/- balance remaining";
                var logsList = new List<ReconciliationCPLogs>();
                if (dbData == null)
                {
                    logsList.Add(new ReconciliationCPLogs() { LOG_MESSAGE = "Reconciliation is created." });
                    dbData = new ReconciliationCP();
                }
                if (dbData.REMARKS != autoReconciliationToUpdate.REMARKS)
                {
                    var log = "";
                    if (dbData.REMARKS != null)
                        log = "REMARKS changed from \"" + dbData.REMARKS + "\" to \"" + dbData.REMARKS + "\".";
                    else
                        log = "REMARKS \"" + autoReconciliationToUpdate.REMARKS + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "REMARKS",
                        PREVIOUS_VALUE = !string.IsNullOrEmpty(dbData.REMARKS) ? dbData.CATEGORY_ID.Value.ToString() : null,
                        NEW_VALUE = !string.IsNullOrEmpty(autoReconciliationToUpdate.REMARKS) ? autoReconciliationToUpdate.REMARKS : null
                    });
                }

                LogReconciliationDetails(logsList, autoReconciliationToUpdate.RECONCILIATION_CP_ID, profile);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DDValues GetDDValues(UserProfile profile)
        {
            try
            {
                var dDValues = new DDValues();
                dDValues.StatusDDValues = GetReconciliationStatuses(profile);
                dDValues.UserDDValues = GetUsersForDD(profile);
                dDValues.Category = GetReconciliationCategories(profile);
                dDValues.DepositType = GetDepositTypes(profile);
                dDValues.Reasons = GetReasons(profile);
                return dDValues;
            }
            catch (Exception ex) { throw ex; }
        }

        public ResponseModel SaveReconciliationCP(ReconciliationCP reconciliationToSave, UserProfile profile)
        {
            bool dBDataIsNull = false;
            ReconciliationCP prevObj = null;
            ResponseModel response = new ResponseModel();
            try
            {
                var reconciliation = new ReconciliationCP();

                if (!string.IsNullOrEmpty(reconciliationToSave.DEPOSIT_DATE_STR))
                {
                    reconciliationToSave.DEPOSIT_DATE = Convert.ToDateTime(reconciliationToSave.DEPOSIT_DATE_STR);
                }

                var dBData = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == reconciliationToSave.RECONCILIATION_CP_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);

                if (dBData == null)
                {
                    dBDataIsNull = true;
                    reconciliation.RECONCILIATION_CP_ID = CommonService.Helper.getMaximumId("FOX_RECONCILIATION_CP_ID");
                    reconciliationToSave.RECONCILIATION_CP_ID = reconciliation.RECONCILIATION_CP_ID;
                    reconciliation.PRACTICE_CODE = profile.PracticeCode;
                    reconciliation.CREATED_BY = reconciliation.MODIFIED_BY = profile.UserName;
                    reconciliation.CREATED_DATE = reconciliation.MODIFIED_DATE = CommonService.Helper.GetCurrentDate();
                    reconciliation.DELETED = false;
                }
                else
                {
                    prevObj = (ReconciliationCP)dBData.Clone();
                    reconciliation = dBData;
                    reconciliation.MODIFIED_BY = profile.UserName;
                    reconciliation.MODIFIED_DATE = CommonService.Helper.GetCurrentDate();
                }

                reconciliation.FOX_TBL_INSURANCE_NAME = reconciliationToSave.FOX_TBL_INSURANCE_NAME;
                reconciliation.DEPOSIT_DATE = reconciliationToSave.DEPOSIT_DATE;
                reconciliation.DEPOSIT_TYPE_ID = reconciliationToSave.DEPOSIT_TYPE_ID;
                reconciliation.CATEGORY_ID = reconciliationToSave.CATEGORY_ID;
                reconciliation.CHECK_NO = reconciliationToSave.CHECK_NO;

                reconciliation.AMOUNT = reconciliationToSave.AMOUNT;
                reconciliation.AMOUNT_POSTED = reconciliationToSave.AMOUNT_POSTED;
                reconciliation.AMOUNT_NOT_POSTED = reconciliationToSave.AMOUNT_NOT_POSTED;
                reconciliation.IS_RECONCILIED = !string.IsNullOrEmpty(reconciliationToSave.REMARKS) && reconciliationToSave.REMARKS.Trim().Length > 0 ? true : false; //reconciliationToSave.AMOUNT_NOT_POSTED.HasValue ? reconciliationToSave.AMOUNT_NOT_POSTED.Value == 0 ? true : false : false;
                reconciliation.REASON = reconciliationToSave.REASON;
                reconciliation.REMARKS = reconciliationToSave.REMARKS;


                //if (prevObj != null && prevObj.RECONCILIATION_STATUS_ID == GetAssignedStatusId(profile))
                //{
                    reconciliation.RECONCILIATION_STATUS_ID = GetClosedStatusId(profile);
                //}

                //if (prevObj != null && prevObj.AMOUNT_NOT_POSTED != 0 && reconciliationToSave.AMOUNT_NOT_POSTED == 0)
                //{
                //    reconciliation.RECONCILIATION_STATUS_ID = GetCompletedStatusId(profile);
                //    reconciliation.COMPLETED_BY = profile.UserName;
                //    reconciliation.COMPLETED_DATE = DateTime.Now;
                //}
                //else if (prevObj != null && prevObj.AMOUNT_NOT_POSTED == 0 && reconciliationToSave.AMOUNT_NOT_POSTED != 0 && !String.IsNullOrEmpty(reconciliationToSave.LEDGER_NAME))
                //{
                //    reconciliation.RECONCILIATION_STATUS_ID = GetPendingStatusId(profile);
                //    reconciliation.MODIFIED_BY = profile.UserName;
                //    reconciliation.MODIFIED_DATE = DateTime.Now;
                //}
                //else if (prevObj != null && prevObj.AMOUNT_NOT_POSTED == 0 && reconciliationToSave.AMOUNT_NOT_POSTED != 0 && String.IsNullOrEmpty(reconciliationToSave.LEDGER_NAME))
                //{
                //    reconciliation.RECONCILIATION_STATUS_ID = GetAssignedStatusId(profile);
                //    reconciliation.MODIFIED_BY = profile.UserName;
                //    reconciliation.MODIFIED_DATE = DateTime.Now;
                //}
                //else if (prevObj == null)
                //{
                //    reconciliation.RECONCILIATION_STATUS_ID = GetUnassignedStatusId(profile);
                //}

                if (dBData == null)
                    _reconciliationCPRepository.Insert(reconciliation);
                else
                    _reconciliationCPRepository.Update(reconciliation);
                _reconciliationCPRepository.Save();

                SaveReconciliationLogs(prevObj, reconciliationToSave, profile);

                response.Success = true;
                response.ErrorMessage = "";
                response.Message = dBData == null ? "Reconciliation added successfully." : "Reconciliation updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = dBDataIsNull ? "Reconciliation couldn't be added." : "Reconciliation couldn't be updated.";
            }
            return response;
        }

        public int? GetUnassignedStatusId(UserProfile profile)
        {
            var unAssignedStatus = _reconciliationStatusRepository.GetFirst(e => e.STATUS_NAME == "Unassigned" && !e.DELETED);
            if (unAssignedStatus != null)
            {
                return unAssignedStatus.RECONCILIATION_STATUS_ID;
            }
            return null;
        }

        public int? GetCompletedStatusId(UserProfile profile)
        {
            var completedStatus = _reconciliationStatusRepository.GetFirst(e => e.STATUS_NAME == "Completed" && !e.DELETED);
            if (completedStatus != null)
            {
                return completedStatus.RECONCILIATION_STATUS_ID;
            }
            return null;
        }

        public int? GetAssignedStatusId(UserProfile profile)
        {
            var assStatus = _reconciliationStatusRepository.GetFirst(e => e.STATUS_NAME == "Assigned" && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);
            if (assStatus != null)
            {
                return assStatus.RECONCILIATION_STATUS_ID;
            }
            return null;
        }

        public int? GetClosedStatusId(UserProfile profile)
        {
            var assStatus = _reconciliationStatusRepository.GetFirst(e => e.STATUS_NAME == "Closed" && !e.DELETED);
            if (assStatus != null)
            {
                return assStatus.RECONCILIATION_STATUS_ID;
            }
            return null;
        }

        public int? GetPendingStatusId(UserProfile profile)
        {
            var penStatus = _reconciliationStatusRepository.GetFirst(e => e.STATUS_NAME == "Pending" && !e.DELETED);
            if (penStatus != null)
            {
                return penStatus.RECONCILIATION_STATUS_ID;
            }
            return null;
        }

        public string GetInsuranceName(long? foxInsId)
        {
            try
            {
                if (foxInsId.HasValue)
                {
                    var insurance = _foxInsuranceRepository.GetFirst(e => e.FOX_TBL_INSURANCE_ID == foxInsId.Value);
                    if (insurance != null)
                    {
                        return insurance.INSURANCE_NAME;
                    }
                }

                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        public string GetDepositTypeName(long? depositTypeId)
        {
            try
            {
                if (depositTypeId.HasValue)
                {
                    var depositType = _reconciliationDepositTypeRepository.GetFirst(e => e.DEPOSIT_TYPE_ID == depositTypeId.Value);
                    if (depositType != null)
                    {
                        return depositType.DEPOSIT_TYPE_NAME;
                    }
                }

                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        public string GetCategoryName(long? catId)
        {
            try
            {
                if (catId.HasValue)
                {
                    var cat = _reconciliationCategoryRepository.GetFirst(e => e.CATEGORY_ID == catId.Value);
                    if (cat != null)
                    {
                        return cat.CATEGORY_NAME;
                    }
                }

                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        public string GetReasonName(long? reasonId)
        {
            try
            {
                if (reasonId.HasValue)
                {
                    SqlParameter reason_Id = new SqlParameter("REASON_ID", reasonId);
                    FOX_TBL_RECONCILIATION_REASON temp = SpRepository<FOX_TBL_RECONCILIATION_REASON>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_RECONCILIATION_REASON_NAME @REASON_ID", reason_Id);
                    return temp != null ? temp.REASON : string.Empty;
                }

                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        public void SaveManualReconciliationLogs(ReconciliationCP dbData, ReconciliationCP manualreconciliationToSave, UserProfile profile)
        {
            try
            {
                manualreconciliationToSave.REMARKS = "Manual Reconciliation";
                var logsList = new List<ReconciliationCPLogs>();
                if (dbData == null)
                {
                    logsList.Add(new ReconciliationCPLogs() { LOG_MESSAGE = "Reconciliation is created." });
                    dbData = new ReconciliationCP();
                }
                if (dbData.REMARKS != manualreconciliationToSave.REMARKS)
                {
                    var log = "";
                    if (dbData.REMARKS != null)
                        //log = "REMARKS changed from \"" + dbData.REMARKS + "\" to \"" + dbData.REMARKS + "\".";
                    log = "REMARKS changed to \"" + dbData.REMARKS + "\".";
                    else
                        log = "REMARKS \"" + manualreconciliationToSave.REMARKS + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "REMARKS",
                        PREVIOUS_VALUE = !string.IsNullOrEmpty(dbData.REMARKS) ? dbData.CATEGORY_ID.Value.ToString() : null,
                        NEW_VALUE = !string.IsNullOrEmpty(manualreconciliationToSave.REMARKS) ? manualreconciliationToSave.REMARKS : null
                    });
                }

                LogReconciliationDetails(logsList, manualreconciliationToSave.RECONCILIATION_CP_ID, profile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveAutoReconciliationLogs(ReconciliationCP dbData, ReconciliationCP autoreconciliationToSave, UserProfile profile)
        {
            try
            {
                autoreconciliationToSave.REMARKS = "Automatically Reconciled";
                var logsList = new List<ReconciliationCPLogs>();
                if (dbData == null)
                {
                    logsList.Add(new ReconciliationCPLogs() { LOG_MESSAGE = "Reconciliation is created." });
                    dbData = new ReconciliationCP();
                }
                if (dbData.REMARKS != autoreconciliationToSave.REMARKS)
                {
                    var log = "";
                    if (dbData.REMARKS != null)
                        log = "REMARKS changed from \"" + dbData.REMARKS + "\" to \"" + dbData.REMARKS + "\".";
                    else
                        log = "REMARKS \"" + autoreconciliationToSave.REMARKS + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "REMARKS",
                        PREVIOUS_VALUE = !string.IsNullOrEmpty(dbData.REMARKS) ? dbData.CATEGORY_ID.Value.ToString() : null,
                        NEW_VALUE = !string.IsNullOrEmpty(autoreconciliationToSave.REMARKS) ? autoreconciliationToSave.REMARKS : null
                    });
                }

                LogReconciliationDetails(logsList, autoreconciliationToSave.RECONCILIATION_CP_ID, profile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveReconciliationLogs(ReconciliationCP dbData, ReconciliationCP reconciliationToSave, UserProfile profile)
        {
            try
            {
                var logsList = new List<ReconciliationCPLogs>();
                //var patAcc = !string.IsNullOrWhiteSpace(reconciliationToSave.PATIENT_ACCOUNT_STR) ? long.Parse(reconciliationToSave.PATIENT_ACCOUNT_STR) : (long?)null;
                if (dbData == null)
                {
                    logsList.Add(new ReconciliationCPLogs() { LOG_MESSAGE = "Reconciliation is created." });
                    dbData = new ReconciliationCP();
                }

                if (dbData.FOX_TBL_INSURANCE_NAME != reconciliationToSave.FOX_TBL_INSURANCE_NAME)
                {
                    var log = "";
                    if (dbData.FOX_TBL_INSURANCE_NAME != null)
                        log = "Insurance changed from \"" + GetInsuranceName(long.Parse(dbData.FOX_TBL_INSURANCE_NAME)) + "\" to \"" + GetInsuranceName(long.Parse(reconciliationToSave.FOX_TBL_INSURANCE_NAME)) + "\".";
                    else
                        log = "Insurance \"" + GetInsuranceName(long.Parse(reconciliationToSave.FOX_TBL_INSURANCE_NAME)) + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "FOX_TBL_INSURANCE_NAME",
                        PREVIOUS_VALUE = string.IsNullOrWhiteSpace(dbData.FOX_TBL_INSURANCE_NAME) ? "" : dbData.FOX_TBL_INSURANCE_NAME,
                        NEW_VALUE = string.IsNullOrWhiteSpace(reconciliationToSave.FOX_TBL_INSURANCE_NAME) ? "" : reconciliationToSave.FOX_TBL_INSURANCE_NAME
                    });
                }

                if (dbData.DEPOSIT_DATE != reconciliationToSave.DEPOSIT_DATE)
                {
                    var log = "";
                    if (dbData.DEPOSIT_DATE != null)
                        log = "Deposit Date changed from \"" + dbData.DEPOSIT_DATE.Value.ToString("MM/dd/yyyy") + "\" to \"" + reconciliationToSave.DEPOSIT_DATE.Value.ToString("MM/dd/yyyy") + "\".";
                    else
                        log = "Deposit Date \"" + reconciliationToSave.DEPOSIT_DATE.Value.ToString("MM/dd/yyyy") + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "DEPOSIT_DATE",
                        PREVIOUS_VALUE = dbData.DEPOSIT_DATE.HasValue ? dbData.DEPOSIT_DATE.Value.ToString("MM/dd/yyyy") : null,
                        NEW_VALUE = reconciliationToSave.DEPOSIT_DATE.HasValue ? reconciliationToSave.DEPOSIT_DATE.Value.ToString("MM/dd/yyyy") : null
                    });
                }

                if (dbData.DEPOSIT_TYPE_ID != reconciliationToSave.DEPOSIT_TYPE_ID)
                {
                    var log = "";
                    if (dbData.DEPOSIT_TYPE_ID != null)
                        log = "Deposit Type changed from \"" + GetDepositTypeName(dbData.DEPOSIT_TYPE_ID) + "\" to \"" + GetDepositTypeName(reconciliationToSave.DEPOSIT_TYPE_ID) + "\".";
                    else
                        log = "Deposit Type \"" + GetDepositTypeName(reconciliationToSave.DEPOSIT_TYPE_ID) + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "DEPOSIT_TYPE_ID",
                        PREVIOUS_VALUE = dbData.DEPOSIT_TYPE_ID.HasValue ? dbData.DEPOSIT_TYPE_ID.Value.ToString() : null,
                        NEW_VALUE = reconciliationToSave.DEPOSIT_TYPE_ID.HasValue ? reconciliationToSave.DEPOSIT_TYPE_ID.Value.ToString() : null
                    });
                }

                if (dbData.CATEGORY_ID != reconciliationToSave.CATEGORY_ID)
                {
                    var log = "";
                    if (dbData.CATEGORY_ID != null)
                        log = "Category changed from \"" + GetCategoryName(dbData.CATEGORY_ID) + "\" to \"" + GetCategoryName(reconciliationToSave.CATEGORY_ID) + "\".";
                    else
                        log = "Category \"" + GetCategoryName(reconciliationToSave.CATEGORY_ID) + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "CATEGORY_ID",
                        PREVIOUS_VALUE = dbData.CATEGORY_ID.HasValue ? dbData.CATEGORY_ID.Value.ToString() : null,
                        NEW_VALUE = reconciliationToSave.CATEGORY_ID.HasValue ? reconciliationToSave.CATEGORY_ID.Value.ToString() : null
                    });
                }

                if (!(dbData.CHECK_NO ?? "").Equals(reconciliationToSave.CHECK_NO ?? ""))
                {
                    var log = "";
                    if (!string.IsNullOrWhiteSpace(dbData.CHECK_NO))
                        log = "Check # changed from\"" + dbData.CHECK_NO + "\" to \"" + reconciliationToSave.CHECK_NO + "\".";
                    else
                        log = "Check # \"" + reconciliationToSave.CHECK_NO + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "CHECK_NO",
                        PREVIOUS_VALUE = dbData.CHECK_NO,
                        NEW_VALUE = reconciliationToSave.CHECK_NO
                    });
                }

                if (dbData.AMOUNT != reconciliationToSave.AMOUNT)
                {
                    var log = "";
                    if (dbData.AMOUNT != null)
                        log = "Amount changed from \"" + dbData.AMOUNT.Value.ToString("C2") + "\" to \"" + reconciliationToSave.AMOUNT.Value.ToString("C2") + "\".";
                    else
                        log = "Amount \"" + reconciliationToSave.AMOUNT.Value.ToString("C2") + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "AMOUNT",
                        PREVIOUS_VALUE = dbData.AMOUNT.HasValue ? dbData.AMOUNT.Value.ToString() : null,
                        NEW_VALUE = reconciliationToSave.AMOUNT.HasValue ? reconciliationToSave.AMOUNT.Value.ToString() : null
                    });
                }

                if (dbData.AMOUNT_POSTED != reconciliationToSave.AMOUNT_POSTED)
                {
                    var log = "";
                    if (dbData.AMOUNT_POSTED != null)
                        log = "Amount Posted changed from \"" + dbData.AMOUNT_POSTED.Value.ToString("C2") + "\" to \"" + reconciliationToSave.AMOUNT_POSTED.Value.ToString("C2") + "\".";
                    else
                        log = "Amount Posted \"" + reconciliationToSave.AMOUNT_POSTED.Value.ToString("C2") + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "AMOUNT_POSTED",
                        PREVIOUS_VALUE = dbData.AMOUNT_POSTED.HasValue ? dbData.AMOUNT_POSTED.Value.ToString() : null,
                        NEW_VALUE = reconciliationToSave.AMOUNT_POSTED.HasValue ? reconciliationToSave.AMOUNT_POSTED.Value.ToString() : null
                    });
                }

                if (dbData.AMOUNT_NOT_POSTED != reconciliationToSave.AMOUNT_NOT_POSTED)
                {
                    var log = "";
                    if (dbData.AMOUNT_NOT_POSTED != null)
                        log = "Amount Not Posted changed from \"" + dbData.AMOUNT_NOT_POSTED.Value.ToString("C2") + "\" to \"" + reconciliationToSave.AMOUNT_NOT_POSTED.Value.ToString("C2") + "\".";
                    else
                        log = "Amount Not Posted \"" + reconciliationToSave.AMOUNT_NOT_POSTED.Value.ToString("C2") + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "AMOUNT_NOT_POSTED",
                        PREVIOUS_VALUE = dbData.AMOUNT_NOT_POSTED.HasValue ? dbData.AMOUNT_NOT_POSTED.Value.ToString() : null,
                        NEW_VALUE = reconciliationToSave.AMOUNT_NOT_POSTED.HasValue ? reconciliationToSave.AMOUNT_NOT_POSTED.Value.ToString() : null
                    });
                }
                if (dbData.REMARKS != reconciliationToSave.REMARKS)
                {
                    var log = "";
                    if (dbData.REMARKS != null)
                        log = "REMARKS changed from \"" + dbData.REMARKS + "\" to \"" + dbData.REMARKS + "\".";
                    else
                        log = "REMARKS \"" + reconciliationToSave.REMARKS + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "REMARKS",
                        PREVIOUS_VALUE = !string.IsNullOrEmpty(dbData.REMARKS) ? dbData.CATEGORY_ID.Value.ToString() : null,
                        NEW_VALUE = !string.IsNullOrEmpty( reconciliationToSave.REMARKS) ? reconciliationToSave.REMARKS : null
                    });
                }

                if (dbData.REASON != reconciliationToSave.REASON)
                {
                    var log = "";
                    if (dbData.REASON != null)
                        log = "Reason changed from \"" + GetReasonName(dbData.REASON) + "\" to \"" + GetReasonName(reconciliationToSave.REASON) + "\".";
                    else
                        log = "Reason \"" + GetReasonName(reconciliationToSave.REASON) + "\" is added to the reconciliation.";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "Reason",
                        PREVIOUS_VALUE = dbData.REASON.HasValue ? dbData.REASON.Value.ToString() : null,
                        NEW_VALUE = reconciliationToSave.REASON.HasValue ? reconciliationToSave.REASON.Value.ToString() : null
                    });
                }


                LogReconciliationDetails(logsList, reconciliationToSave.RECONCILIATION_CP_ID, profile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LogReconciliationDetails(List<ReconciliationCPLogs> reconciliationLogList, long? reconciliation_CP_ID, UserProfile profile)
        {
            try
            {
                foreach (var reconciliationLog in reconciliationLogList)
                {
                    reconciliationLog.RECONCILIATION_LOG_ID = CommonService.Helper.getMaximumId("FOX_RECONCILIATION_CP_LOG_ID");
                    reconciliationLog.RECONCILIATION_CP_ID = reconciliation_CP_ID;

                    reconciliationLog.PRACTICE_CODE = profile.PracticeCode;
                    reconciliationLog.CREATED_BY = reconciliationLog.MODIFIED_BY = profile.UserName;
                    reconciliationLog.CREATED_DATE = reconciliationLog.MODIFIED_DATE = CommonService.Helper.GetCurrentDate();
                    reconciliationLog.DELETED = false;

                    _reconciliationCPLogsRepository.Insert(reconciliationLog);
                    _reconciliationCPLogsRepository.Save();
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public ResponseModel DeleteReconciliationCP(ReconciliationCPToDelete reconciliationToDelete, UserProfile profile)
        {
            var response = new ResponseModel();
            try
            {
                var reconciliation = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == reconciliationToDelete.RECONCILIATION_CP_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                if (reconciliation != null)
                {
                    reconciliation.MODIFIED_BY = profile.UserName;
                    reconciliation.MODIFIED_DATE = CommonService.Helper.GetCurrentDate();
                    reconciliation.DELETED = true;
                    _reconciliationCPRepository.Update(reconciliation);
                    _reconciliationCPRepository.Save();
                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = "Reconciliation deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "";
                    response.Message = "Reconciliation could not be found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = "Reconciliation couldn't be deleted.";
            }
            return response;
        }

        public ResponseModel DeleteReconsiliationLedger(long reconsiliationId, UserProfile profile)
        {
            var response = new ResponseModel();
            try
            {
                var reconciliation = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == reconsiliationId && e.PRACTICE_CODE == profile.PracticeCode );
                if (reconciliation != null)
                {
                    // reconciliation.DELETED = true;
                    string ledgerName = reconciliation.LEDGER_NAME;
                    string ledgerPath = reconciliation.LEDGER_PATH;
                    LogLedgerDelete(ledgerName, ledgerPath, reconsiliationId, profile);
                    reconciliation.LEDGER_NAME = null;
                    reconciliation.LEDGER_PATH = null;
                    // reconciliation.LEDGER_BASE64 = null;
                    reconciliation.RECONCILIATION_STATUS_ID = GetAssignedStatusId(profile);
                    reconciliation.MODIFIED_BY =  profile.UserName;
                    reconciliation.MODIFIED_DATE = DateTime.Now;
                    _reconciliationCPRepository.Update(reconciliation);
                    _reconciliationCPRepository.Save();
                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = "Reconciliation ledger deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "";
                    response.Message = "Reconciliation ledger could not be found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = "Reconciliation ledger couldn't be deleted.";
            }
            return response;
        }

        public void LogLedgerDelete(string ledgerName, string ledgerPath, long reconciliation_CP_ID, UserProfile profile)
        {
            try
            {

                var logsList = new List<ReconciliationCPLogs>();

                var log = "";

                log = "Ledger " + ledgerName + " deleted by \"" + GetUserName(profile.UserName) + "\".";


                logsList.Add(new ReconciliationCPLogs()
                {
                    LOG_MESSAGE = log,
                    FIELD_NAME = "Ledger Name",
                    PREVIOUS_VALUE = ledgerPath,
                    NEW_VALUE = string.Empty
                });


                LogReconciliationDetails(logsList, reconciliation_CP_ID, profile);
            }
            catch (Exception ex) { throw ex; }
        }

        public ResponseModel AssignUserCP(UserAssignmentModel userAssignmentDetails, UserProfile profile)
        {
            var response = new ResponseModel();
            try
            {
                foreach (var reconciliationID in userAssignmentDetails.RECONCILIATION_CP_IDS)
                {
                    bool isReAssign = false;

                    var previousUser = (object)null;
                    var reconciliation = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == reconciliationID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                    if (reconciliation != null)
                    {
                        if (!string.IsNullOrEmpty(reconciliation.ASSIGNED_TO))
                        {
                            previousUser = reconciliation.ASSIGNED_TO.Clone();
                        }

                        reconciliation.ASSIGNED_TO = userAssignmentDetails.UserToAssign.USER_NAME;
                        reconciliation.ASSIGNED_DATE = DateTime.Now;
                        reconciliation.RECONCILIATION_STATUS_ID = GetAssignedStatusId(profile);
                        reconciliation.MODIFIED_BY = profile.UserName;
                        reconciliation.MODIFIED_DATE = DateTime.Now; ;

                        _reconciliationCPRepository.Update(reconciliation);
                        _reconciliationCPRepository.Save();

                        LogUserAssignment(isReAssign, previousUser, userAssignmentDetails.UserToAssign.USER_NAME, reconciliationID, profile);

                        response.Success = true;
                        response.ErrorMessage = "";
                        response.Message = "Reconciliation assigned to user successfully.";
                    }
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = "Error occoured while assigning to user.";
            }
            return response;
        }

        public void LogUserAssignment(bool isReassign, object previousUser, string newUserName, long reconciliation_CP_ID, UserProfile profile)
        {
            try
            {
                string previousUserName = !string.IsNullOrWhiteSpace((string)previousUser) ? (string)previousUser : "";
                var logsList = new List<ReconciliationCPLogs>();
                if (!previousUserName.Equals(newUserName))
                {
                    var log = "";
                    if (!string.IsNullOrWhiteSpace(previousUserName))
                        log = "Adjustment re-assigned from \"" + GetUserName(previousUserName) + "\" to \"" + GetUserName(newUserName) + "\".";
                    else
                        log = "Adjustment assigned to \"" + GetUserName(newUserName) + "\".";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "ASSIGNED_TO",
                        PREVIOUS_VALUE = !string.IsNullOrWhiteSpace(previousUserName) ? previousUserName : null,
                        NEW_VALUE = newUserName
                    });
                }

                LogReconciliationDetails(logsList, reconciliation_CP_ID, profile);
            }
            catch (Exception ex) { throw ex; }
        }

        public string GetUserName(string userName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    var user = _userRepository.GetFirst(e => e.USER_NAME == userName);
                    if (user != null)
                    {
                        string last_Name = user.LAST_NAME?.ToTitleCase();
                        string first_Name = user.LAST_NAME?.ToTitleCase();
                        return last_Name + "," + first_Name;
                    }
                }
                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        public List<ReconciliationCP> GetReconciliationsCPForExcel(ReconciliationCPSearchReq searchReq, UserProfile profile)
        {
            try
            {
                bool hasDOSFromDate = false;
                bool hasDOSToDate = false;

                DateTime dateTimeTo;
                DateTime dateTimeFrom;
                if (DateTime.TryParse(searchReq.DATE_FROM_Str, out dateTimeFrom))
                {
                    hasDOSFromDate = true;
                    searchReq.DATE_FROM = dateTimeFrom;
                }
                if (DateTime.TryParse(searchReq.DATE_TO_Str, out dateTimeTo))
                {
                    hasDOSToDate = true;
                    searchReq.DATE_TO = dateTimeTo;
                }

                var result = SpRepository<ReconciliationCP>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATIONS_CP_TEST 
                    @PRACTICE_CODE, @IS_FOR_REPORT, @IS_DEPOSIT_DATE_SEARCH, @IS_ASSIGNED_DATE_SEARCH, @DATE_FROM, @DATE_TO, @FOX_TBL_INSURANCE_NAME, 
                    @STATUS_ID, @CURRENT_USER, @SEARCH_TEXT, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER, @CP_TYPE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "IS_FOR_REPORT", SqlDbType = SqlDbType.Bit, Value = searchReq.IsForReport }
                    , new SqlParameter { ParameterName = "IS_DEPOSIT_DATE_SEARCH", SqlDbType = SqlDbType.Bit, Value = searchReq.IS_DEPOSIT_DATE_SEARCH }
                    , new SqlParameter { ParameterName = "IS_ASSIGNED_DATE_SEARCH", SqlDbType = SqlDbType.Bit, Value = searchReq.IS_ASSIGNED_DATE_SEARCH }
                    , CommonService.Helper.getDBNullOrValue("DATE_FROM", hasDOSFromDate ? (searchReq.DATE_FROM.HasValue ? searchReq.DATE_FROM.Value.ToString("MM/dd/yyyy") : "") : "")
                    , CommonService.Helper.getDBNullOrValue("DATE_TO", hasDOSToDate ? (searchReq.DATE_TO.HasValue ? searchReq.DATE_TO.Value.ToString("MM/dd/yyyy") : "") : "")
                    // , CommonService.Helper.getDBNullOrValue("FOX_TBL_INSURANCE_ID", searchReq.FOX_TBL_INSURANCE_ID.HasValue ? searchReq.FOX_TBL_INSURANCE_ID.ToString() : "")
                    , new SqlParameter("FOX_TBL_INSURANCE_NAME", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.INSURANCE_NAME) ? "" : searchReq.INSURANCE_NAME }
                    , CommonService.Helper.getDBNullOrValue("STATUS_ID", "")
                    , new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = profile.UserName }
                    , new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.SearchString) ? "" : searchReq.SearchString }
                    , new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = searchReq.CurrentPage }
                    , new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = 0 }
                       , new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = searchReq.SORT_BY }
                    , new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = searchReq.SORT_ORDER }
                    ,new SqlParameter("CP_TYPE", SqlDbType.Int) { Value = searchReq.CP_Type }
                    );

                if (result != null && result.Count > 0)
                {
                    result.OrderByDescending(o => o.DEPOSIT_DATE).ToList();
                    if (searchReq.CATEGORIES?.Count > 0)
                    {
                        var cats = searchReq.CATEGORIES.Where(e => e.Selected).Select(e => e.CATEGORY_ID).ToList();
                        if (cats.Count > 0)
                            result = result.FindAll(e => e.CATEGORY_ID.HasValue && cats.Contains(e.CATEGORY_ID.Value));
                    }
                    if (searchReq.FOX_TBL_INSURANCE_NAME?.Count > 0)
                    {
                        var cats = searchReq.FOX_TBL_INSURANCE_NAME.Where(e => e.Selected).Select(e => e.INSURANCE_NAME).ToList();
                        if (cats.Count > 0)
                            result = result.FindAll(e => e.CATEGORY_ID.HasValue && cats.Contains(e.INSURANCE_NAME));
                    }
                    if (searchReq.DEPOSIT_TYPES?.Count > 0)
                    {
                        var dts = searchReq.DEPOSIT_TYPES.Where(e => e.Selected).Select(e => e.DEPOSIT_TYPE_ID).ToList();
                        if (dts.Count > 0)
                            result = result.FindAll(e => e.DEPOSIT_TYPE_ID.HasValue && dts.Contains(e.DEPOSIT_TYPE_ID.Value));
                    }

                    if (searchReq.Statuses?.Count > 0)
                    {
                        var rs = searchReq.Statuses.Where(e => e.Selected).Select(e => e.STATUS_NAME).ToList();
                        if (rs?.Count > 0)
                        {
                            result = result.FindAll(e => rs.Contains(e.STATUS_NAME));
                        }
                    }

                    if (searchReq.CHECK_NOS?.Count > 0)
                    {
                        var cnos = searchReq.CHECK_NOS.Where(e => e.Selected).Select(e => e.Value).ToList();
                        if (cnos.Count > 0)
                            result = result.FindAll(e => !string.IsNullOrWhiteSpace(e.CHECK_NO) && cnos.Contains(e.CHECK_NO));
                    }

                    if (searchReq.AMOUNTS?.Count > 0)
                    {
                        var ams = searchReq.AMOUNTS.Where(e => e.Selected).Select(e => e.Value).ToList();
                        if (ams.Count > 0)
                            result = result.FindAll(e => e.AMOUNT.HasValue && ams.Contains(e.AMOUNT.Value));
                    }

                    if (searchReq.AMOUNTS_POSTED?.Count > 0)
                    {
                        var amsp = searchReq.AMOUNTS_POSTED.Where(e => e.Selected).Select(e => e.Value).ToList();
                        if (amsp.Count > 0)
                            result = result.FindAll(e => e.AMOUNT_POSTED.HasValue && amsp.Contains(e.AMOUNT_POSTED.Value));
                    }

                    if (searchReq.AMOUNTS_NOT_POSTED?.Count > 0)
                    {
                        var amsnp = searchReq.AMOUNTS_NOT_POSTED.Where(e => e.Selected).Select(e => e.Value).ToList();
                        if (amsnp.Count > 0)
                            result = result.FindAll(e => e.AMOUNT_NOT_POSTED.HasValue && amsnp.Contains(e.AMOUNT_NOT_POSTED.Value));
                    }
                    if (!searchReq.IsForReport)
                    {
                        result = result.FindAll(e => e.STATUS_NAME.ToLower() != "completed");
                    }


                    //if (result?.Count > 0)
                    //{
                    //    int CURRENT_PAGE = searchReq.CurrentPage - 1;
                    //    int START_FROM = CURRENT_PAGE * searchReq.RecordPerPage;
                    //    result[0].TOTAL_RECORDS = result.Count;

                    //    if (result.Count <= searchReq.RecordPerPage)
                    //    {
                    //        result.Select(c => { c.TOTAL_RECORDS = 1; return c; }).ToList();
                    //    }
                    //    else
                    //    {
                    //        result.Select(c => { c.TOTAL_RECORD_PAGES = Math.Ceiling(((double)result.Count / (double)searchReq.RecordPerPage)); return c; }).ToList();
                    //    }
                    //    decimal totalAmount = result.Sum(item => item.AMOUNT.HasValue ? item.AMOUNT.Value : 0);
                    //    decimal totalNotPostedAmount = result.Sum(item => item.AMOUNT_NOT_POSTED.HasValue ? item.AMOUNT_NOT_POSTED.Value : 0);
                    //    decimal totalPostedAmount = result.Sum(item => item.AMOUNT_POSTED.HasValue ? item.AMOUNT_POSTED.Value : 0);
                    //    result.Select(c => { c.TOTAL_AMOUNT = totalAmount; return c; }).ToList();
                    //    result.Select(c => { c.TOTAL_POSTED_AMOUNT = totalPostedAmount; return c; }).ToList();
                    //    result.Select(c => { c.TOTAL_UNPOSTED_AMOUNT = totalNotPostedAmount; return c; }).ToList();

                    //}
                }
                return result;
                //return result.OrderByDescending(o => o.CREATED_DATE)
                //    .ThenByDescending(o => o.DEPOSIT_DATE)
                //    .ThenByDescending(o => o.ASSIGNED_DATE)
                //    .ThenByDescending(o => o.COMPLETED_DATE).ToList();

            }
            catch (Exception ex) { throw ex; }
        }

        public ResponseModel ExportReconciliationsToExcel(ReconciliationCPSearchReq searchReq, UserProfile profile)
        {
            var response = new ResponseModel();
            try
            {
                List<ReconciliationCP> obj = GetReconciliationsCPForExcel(searchReq, profile);
                List<ReconciliationCPExportModel> objToExport = new List<ReconciliationCPExportModel>();
                for (int i = 0; i < obj.Count(); i++)
                {
                    obj[i].ROW = i + 1;

                }
                if (obj != null && obj.Count > 0)
                {
                    PrepareExport(obj, out objToExport);
                    string fileName = "Reconciliations_CP_" + DateTime.Now.Ticks + ".xlsx";
                    string exportPath = string.Empty;
                    bool exported = false;
                    string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                    exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                    if (!Directory.Exists(exportPath))
                    {
                        Directory.CreateDirectory(exportPath);
                    }
                    var pathToWriteFile = exportPath + "\\" + fileName;
                    DataTable dt = ExportToExcel.ListToDataTable(objToExport);
                    dt.TableName = "Reconciliations_CP";
                    //exported = ExportToExcel.CreateExcelDocument<ReconciliationCP>(obj, pathToWriteFile);
                    exported = ExportToExcel.CreateExcelDocument(dt, pathToWriteFile);
                    if (exported)
                    {
                        response.Success = true;
                        response.ErrorMessage = "";
                        response.Message = virtualPath + fileName;
                    }
                    else
                    {
                        response.Success = false;
                        response.ErrorMessage = "Records couldn't be exported. Please try again.";
                        response.Message = "Records couldn't be exported. Please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = "Records couldn't be exported. Please try again.";
            }
            return response;
        }

        public void PrepareExport(List<ReconciliationCP> obj, out List<ReconciliationCPExportModel> recordToExport)
        {
            recordToExport = new List<ReconciliationCPExportModel>();
            foreach (var item in obj)
            {
                var exportModel = new ReconciliationCPExportModel();
                if(item.ROW == null)
                {
                    item.ROW = 0;
                }
                exportModel.ROW = item.ROW;
                exportModel.STATUS_NAME = !string.IsNullOrWhiteSpace(item.STATUS_NAME) ? item.STATUS_NAME : "";
                exportModel.DEPOSIT_DATE = item.DEPOSIT_DATE.HasValue ? item.DEPOSIT_DATE.Value.ToString("MM/dd/yyyy") : "";
                exportModel.DEPOSIT_TYPE_NAME = !string.IsNullOrWhiteSpace(item.DEPOSIT_TYPE_NAME) ? item.DEPOSIT_TYPE_NAME : "";
                exportModel.CATEGORY_NAME = !string.IsNullOrWhiteSpace(item.CATEGORY_NAME) ? item.CATEGORY_NAME : "";
                exportModel.INSURANCE_NAME = !string.IsNullOrWhiteSpace(item.INSURANCE_NAME) ? item.INSURANCE_NAME : "";
                exportModel.CHECK_NO = !string.IsNullOrWhiteSpace(item.CHECK_NO) ? item.CHECK_NO : "";
                exportModel.AMOUNT = item.AMOUNT.HasValue ? item.AMOUNT.Value.ToString("C2") : "";
                exportModel.ASSIGNED_DATE = item.ASSIGNED_DATE.HasValue ? item.ASSIGNED_DATE.Value.ToString("MM/dd/yyyy") : "";
                exportModel.ASSIGNED_TO_NAME = !string.IsNullOrWhiteSpace(item.ASSIGNED_TO_NAME) ? item.ASSIGNED_TO_NAME.ToTitleCase() : "";
                //exportModel.Has_Ledger = !string.IsNullOrWhiteSpace(item.LEDGER_PATH) ? "Yes" : "No";
                exportModel.AMOUNT_POSTED = item.AMOUNT_POSTED.HasValue ? item.AMOUNT_POSTED.Value.ToString("C2") : "";
                exportModel.AMOUNT_NOT_POSTED = item.AMOUNT_NOT_POSTED.HasValue ? item.AMOUNT_NOT_POSTED.Value.ToString("C2") : "";
                //exportModel.COMPLETED_DATE = item.COMPLETED_DATE.HasValue ? item.COMPLETED_DATE.Value.ToString("MM/dd/yyyy") : "";
                exportModel.DATE_POSTED = item.DATE_POSTED.HasValue ? item.DATE_POSTED.Value.ToString("MM/dd/yyyy") : string.Empty;
                exportModel.REASON_NAME = string.IsNullOrEmpty(item.REASON_NAME) ? string.Empty : item.REASON_NAME;
                exportModel.ASSIGNED_GROUP = string.IsNullOrEmpty(item.ASSIGNED_GROUP) ? string.Empty : item.ASSIGNED_GROUP;
                exportModel.ASSIGNED_GROUP_DATE = item.ASSIGNED_GROUP_DATE.HasValue ? item.ASSIGNED_GROUP_DATE.Value.ToString("MM/dd/yyyy") : "";
                recordToExport.Add(exportModel);
            }
        }

        public ResponseModel ExportReconciliationCPLogsToExcel(List<ReconciliationCPLogs> obj, UserProfile profile)
        {
            var response = new ResponseModel();
            try
            {
                List<ReconciliationCPLogExportModel> objToExport = new List<ReconciliationCPLogExportModel>();
                if (obj != null && obj.Count > 0)
                {
                    PrepareLogExport(obj, out objToExport);
                    string fileName = "Reconciliation_CP_Logs_" + obj[0].RECONCILIATION_CP_ID + "_" + DateTime.Now.Ticks + ".xlsx";
                    string exportPath = string.Empty;
                    bool exported = false;
                    string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                    exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                    if (!Directory.Exists(exportPath))
                    {
                        Directory.CreateDirectory(exportPath);
                    }
                    var pathToWriteFile = exportPath + "\\" + fileName;
                    DataTable dt = ExportToExcel.ListToDataTable(objToExport);
                    dt.TableName = "Reconciliation_CP_Logs";
                    exported = ExportToExcel.CreateExcelDocument(dt, pathToWriteFile);
                    if (exported)
                    {
                        response.Success = true;
                        response.ErrorMessage = "";
                        response.Message = virtualPath + fileName;
                    }
                    else
                    {
                        response.Success = false;
                        response.ErrorMessage = "Records couldn't be exported. Please try again.";
                        response.Message = "Records couldn't be exported. Please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = "Records couldn't be exported. Please try again.";
            }
            return response;
        }

        public void PrepareLogExport(List<ReconciliationCPLogs> obj, out List<ReconciliationCPLogExportModel> recordToExport)
        {
            recordToExport = new List<ReconciliationCPLogExportModel>();
            foreach (var item in obj)
            {
                var exportModel = new ReconciliationCPLogExportModel();
                exportModel.CREATED_DATE = item.CREATED_DATE.ToString("MM/dd/yyyy h:mm tt");
                exportModel.LOG_MESSAGE = !string.IsNullOrWhiteSpace(item.LOG_MESSAGE) ? item.LOG_MESSAGE : "";
                exportModel.CREATED_BY_NAME = !string.IsNullOrWhiteSpace(item.CREATED_BY_NAME) ? item.CREATED_BY_NAME : "";
                recordToExport.Add(exportModel);
            }
        }

        //public ResponseModel AttachReconsiliationLedger(LedgerModel ledgerDetails, UserProfile profile)
        //{

        //}



        public ResponseModel AttachLedger(LedgerModel ledgerDetails, UserProfile profile)
        {
            #region Commented Code


            //bool isAttachedFirstTime = true;
            //var response = new ResponseModel();
            //try
            //{
            //    FileRecieverResult result = new FileRecieverResult();

            //    result = ProcessLedgerDocument(ledgerDetails, profile);
            //    if (!string.IsNullOrWhiteSpace(result.FilePath))
            //    {
            //        var reconciliation = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == ledgerDetails.RECONCILIATION_CP_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
            //        if (reconciliation != null)
            //        {

            //            var previousLedger = (object)null;
            //            var previousPath = (object)null;
            //            if (!string.IsNullOrWhiteSpace(reconciliation.LEDGER_BASE64))
            //            {
            //                isAttachedFirstTime = false;
            //                previousLedger = reconciliation.LEDGER_BASE64.Clone();
            //                previousPath = reconciliation.LEDGER_PATH.Clone();
            //            }
            //            //var user = _userRepository.GetFirst(e => e.USER_ID == profile.userID && e.USER_NAME == profile.UserName && !e.DELETED && e.IS_ACTIVE);
            //            reconciliation.LEDGER_PATH = ledgerDetails.FILE_NAME;
            //            reconciliation.LEDGER_NAME = ledgerDetails.FILE_NAME;
            //            reconciliation.LEDGER_PATH = result.FilePath + "\\" + result.FileName;
            //            reconciliation.LEDGER_BASE64 = ledgerDetails.BASE_64_DOCUMENT;
            //            reconciliation.RECONCILIATION_STATUS_ID = GetPendingStatusId(profile);
            //            _reconciliationCPRepository.Update(reconciliation);
            //            _reconciliationCPRepository.Save();
            //            LogLedgerAttached(isAttachedFirstTime, previousLedger, ledgerDetails.BASE_64_DOCUMENT, previousPath, reconciliation.LEDGER_PATH, ledgerDetails.RECONCILIATION_CP_ID, profile);
            //            response.Success = true;
            //            response.ErrorMessage = "";
            //            response.Message = "Ledger attached successfully.";

            //        }
            //        else
            //        {
            //            response.Success = false;
            //            response.ErrorMessage = "";
            //            response.Message = "Ledger attached couldn't be attached. Please try again.";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.Success = false;
            //    response.ErrorMessage = ex.Message;
            //    response.Message = "Ledger attached couldn't be attached. Please try again.";
            //}
            //return response;

            #endregion

            bool isAttachedFirstTime = true;
            var response = new ResponseModel();
            try
            {
                //FileRecieverResult result = new FileRecieverResult();

                //result = ProcessLedgerDocument(ledgerDetails, profile);
                //if (!string.IsNullOrWhiteSpace(result.FilePath))
                //{
                var reconciliation = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == ledgerDetails.RECONCILIATION_CP_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                if (reconciliation != null)
                {

                    var previousLedger = (object)null;
                    var previousPath = (object)null;
                    if (!string.IsNullOrWhiteSpace(reconciliation.LEDGER_PATH))
                    {
                        isAttachedFirstTime = false;
                        previousLedger = reconciliation.LEDGER_NAME;
                        //previousLedger = reconciliation.LEDGER_BASE64.Clone();
                        previousPath = reconciliation.LEDGER_PATH.Clone();
                    }
                    //var user = _userRepository.GetFirst(e => e.USER_ID == profile.userID && e.USER_NAME == profile.UserName && !e.DELETED && e.IS_ACTIVE);
                    //reconciliation.LEDGER_PATH = ledgerDetails.AbsolutePath;
                    reconciliation.LEDGER_NAME = ledgerDetails.FILE_NAME;
                    reconciliation.LEDGER_PATH = ledgerDetails.AbsolutePath;
                    //reconciliation.LEDGER_BASE64 = ledgerDetails.BASE_64_DOCUMENT;
                    reconciliation.RECONCILIATION_STATUS_ID = GetPendingStatusId(profile);
                    _reconciliationCPRepository.Update(reconciliation);
                    _reconciliationCPRepository.Save();
                    LogLedgerAttached(isAttachedFirstTime, previousLedger, ledgerDetails.BASE_64_DOCUMENT, previousPath, reconciliation.LEDGER_PATH, ledgerDetails.RECONCILIATION_CP_ID, profile);
                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = "Ledger attached successfully.";

                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "";
                    response.Message = "Ledger attached couldn't be attached. Please try again.";
                }
                //}
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = "Ledger attached couldn't be attached. Please try again.";
            }
            return response;
        }

        public FileRecieverResult ProcessLedgerDocument(LedgerModel ledgerDetails, UserProfile profile)
        {
            try
            {
                FileRecieverResult result = new FileRecieverResult();
                result = SaveOriginalFile(ledgerDetails, profile);
                if (!string.IsNullOrWhiteSpace(result.FilePath))
                {
                    GenerateAndSaveImagesOfUploadedFiles(ledgerDetails.RECONCILIATION_CP_ID, result.FileName, profile);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileRecieverResult SaveOriginalFile(LedgerModel ledgerDetails, UserProfile profile)
        {
            FileRecieverResult result = new FileRecieverResult();

            try
            {
                if (!string.IsNullOrWhiteSpace(ledgerDetails.BASE_64_DOCUMENT))
                {

                    string directoryPathForOriginalFiles = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.ReconciliationOriginalFilesDirectory );
                    string fileName = string.Empty;
                    var fileExtension = Path.GetExtension(ledgerDetails.FILE_NAME);
                    if (fileExtension == ".jpeg" || fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".tiff" || fileExtension == ".tif")
                    {

                        if (!Directory.Exists(directoryPathForOriginalFiles))
                        {
                            Directory.CreateDirectory(directoryPathForOriginalFiles);
                        }

                        fileName = Path.GetFileNameWithoutExtension(ledgerDetails.FILE_NAME) + "_" + DateTime.Now.ToString("ddMMyyyHHmmssffff") + fileExtension;

                        var pathtowriteimage = directoryPathForOriginalFiles + "\\" + fileName;

                        if (ledgerDetails.BASE_64_DOCUMENT.Contains(","))
                        {

                            ledgerDetails.BASE_64_DOCUMENT = ledgerDetails.BASE_64_DOCUMENT.Substring(ledgerDetails.BASE_64_DOCUMENT.IndexOf(',') + 1);
                        }

                        File.WriteAllBytes(@"" + pathtowriteimage + "", Convert.FromBase64String(ledgerDetails.BASE_64_DOCUMENT));

                        result.FilePath = directoryPathForOriginalFiles;
                        result.FileName = fileName;
                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateAndSaveImagesOfUploadedFiles(long reconciliation_CP_ID, string file_Name, UserProfile profile)
        {
            try
            {
                var orgFileName = file_Name;
                var originalFileDirectory = AppConfiguration.ReconciliationOriginalFilesDirectory;
                var imgDirPath = AppConfiguration.ReconciliationConvertedImagesDirectory;
                var originalFilePathServer = HttpContext.Current.Server.MapPath("~/" + originalFileDirectory);
                var imgPathServer = HttpContext.Current.Server.MapPath("~/" + imgDirPath );
                //var ORIGINAL_FILES_PATH_DB =  HttpContext.Current.Server.MapPath("~/" + AppConfiguration.ReconciliationOriginallyUploadedFiles + "\"")
                //var config = CommonService.Helper.GetServiceConfiguration(profile.PracticeCode);
                //if (config.PRACTICE_CODE != null
                //    && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                //    && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
                //{
                int totalPages = 0;
                int pageCounter = 0;
                //foreach (var filePath1 in FilePathList)
                //{
                if (!Directory.Exists(originalFilePathServer))
                {
                    Directory.CreateDirectory(originalFilePathServer);
                }

                string orgFilePath = HttpContext.Current.Server.MapPath("~/" + originalFileDirectory + @"\" + orgFileName);
                var ext = Path.GetExtension(orgFilePath).ToLower();
                CommonReconciliationService commonReconciliationService = new CommonReconciliationService();
                if (ext == ".tif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif")
                {
                    int numberOfPages = commonReconciliationService.tifToImage(profile, orgFilePath, imgPathServer, imgDirPath, reconciliation_CP_ID, "CP", pageCounter, out pageCounter);
                    totalPages += numberOfPages;
                }
                else if (ext == ".pdf")
                {
                    int numberOfPages = commonReconciliationService.getNumberOfPagesOfPDF(orgFilePath);
                    commonReconciliationService.SavePdfToImages(profile, orgFilePath, reconciliation_CP_ID, "CP", numberOfPages, pageCounter, out pageCounter);
                    totalPages += numberOfPages;
                }
                //}
                AddToDatabase(totalPages, reconciliation_CP_ID, profile);
                //}
                //else
                //{
                //    throw new Exception("DB configuration for file paths not found. See service configuration.");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddToDatabase(int noOfPages, long reconciliation_CP_ID, UserProfile profile)
        {
            try
            {
                ReconciliationCP reconciliation = _reconciliationCPRepository.Get(t => t.RECONCILIATION_CP_ID == reconciliation_CP_ID && t.PRACTICE_CODE == profile.PracticeCode
                    && !t.DELETED);
                if (reconciliation != null)
                {
                    reconciliation.TOTAL_LEDGER_PAGES = noOfPages;
                    reconciliation.MODIFIED_BY = profile.UserName;
                    reconciliation.MODIFIED_DATE = DateTime.Now;
                    _reconciliationCPRepository.Update(reconciliation);
                    _reconciliationCPRepository.Save();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void LogLedgerAttached(bool isAttachedFirstTime, object previousLedger, string newLedger, object previousPath, string newLedgerPath, long reconciliation_CP_ID, UserProfile profile)
        {
            try
            {
                string previousLedgerBas64 = !string.IsNullOrWhiteSpace((string)previousLedger) ? (string)previousLedger : "";
                string previousLedgerPath = !string.IsNullOrWhiteSpace((string)previousPath) ? (string)previousPath : "";
                var logsList = new List<ReconciliationCPLogs>();
                if (!previousLedgerBas64.Equals(newLedger))
                {
                    var log = "";
                    if (!isAttachedFirstTime)
                        log = "Ledger re-attached by \"" + GetUserName(profile.UserName) + "\".";
                    else
                        log = "Ledger attached by \"" + GetUserName(profile.UserName) + "\".";

                    logsList.Add(new ReconciliationCPLogs()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "LEDGER_PATH",
                        PREVIOUS_VALUE = previousLedgerPath,
                        NEW_VALUE = newLedgerPath
                    });
                }

                LogReconciliationDetails(logsList, reconciliation_CP_ID, profile);
            }
            catch (Exception ex) { throw ex; }
        }

        //public List<FilePath> GetReconciliationFiles(ReconciliationFilesSearchReq reconciliationDetails, UserProfile profile)
        //{
        //    try
        //    {
        //        var result = SpRepository<FilePath>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_RECONCILIATION_FILES @RECONCILIATION_ID, @RECONCILIATION_CATEGORY"
        //           , new SqlParameter("RECONCILIATION_ID", SqlDbType.BigInt) { Value = reconciliationDetails.RECONCILIATION_CP_ID }
        //           , new SqlParameter("RECONCILIATION_CATEGORY", SqlDbType.VarChar) { Value = "CP" }
        //           );
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public string DownloadLedger(ReconciliationFilesSearchReq reconciliationDetails, UserProfile profile)
        {
            //    try
            //    {
            //        var reconciliation = _reconciliationCPRepository.GetFirst(e => e.RECONCILIATION_CP_ID == reconciliationDetails.RECONCILIATION_CP_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
            //        if (reconciliation != null)
            //        {
            //            var practiceDocumentDirectory = profile.PracticeDocumentDirectory;
            //            var localPath = practiceDocumentDirectory + "/" + reconciliation.RECONCILIATION_CP_ID + "_" + DateTime.Now.Ticks + ".pdf";
            //            var pathForPDF = Path.Combine(HttpContext.Current.Server.MapPath(@"~/" + practiceDocumentDirectory), reconciliation.RECONCILIATION_CP_ID + "_" + DateTime.Now.Ticks + ".pdf");
            //            ImageHandler imgHandler = new ImageHandler();
            //            var imges = GetReconciliationFiles(reconciliationDetails, profile);
            //            if (imges != null && imges.Count > 0)
            //            {
            //                var imgPaths = imges.Select(x => x.IMAGE_PATH).ToArray();
            //                imgHandler.ImagesToPdf(imgPaths, pathForPDF);
            //                return localPath;
            //            }
            //        }
            //        return "";
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            return null;
        }

        public ReconciliationDDValueResponse AddNewDDValue(ReconciliationDDValue reconciliationDDValue, UserProfile profile)
        {
            try
            {
                long? newId = null;
                if (!string.IsNullOrWhiteSpace(reconciliationDDValue.Value) && !string.IsNullOrWhiteSpace(reconciliationDDValue.ValueType))
                {
                    switch (reconciliationDDValue.ValueType.ToLower())
                    {
                        case "deposittype":
                            newId = AddNewDepositType(reconciliationDDValue.Value.Trim(), profile);
                            break;
                        case "category":
                            newId = AddNewCategory(reconciliationDDValue.Value.Trim(), profile);
                            break;
                        case "status":
                            newId = AddNewStatus(reconciliationDDValue.Value.Trim(), profile);
                            break;
                        default:
                            break;
                    }
                }

                var responseDDValue = new ReconciliationDDValueResponse();
                responseDDValue.NewId = newId;
                responseDDValue.ValueType = reconciliationDDValue.ValueType;

                return responseDDValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long AddNewStatus(string newStatusValue, UserProfile profile)
        {
            try
            {
                var newStatus = new ReconciliationStatus();
                newStatus.RECONCILIATION_STATUS_ID = (int)CommonService.Helper.getMaximumId("FOX_RECONCILIATION_STATUS_ID");
                newStatus.STATUS_NAME = newStatusValue;

                newStatus.PRACTICE_CODE = profile.PracticeCode;
                newStatus.CREATED_BY = newStatus.MODIFIED_BY = profile.UserName;
                newStatus.CREATED_DATE = newStatus.MODIFIED_DATE = DateTime.Now;
                newStatus.DELETED = false;
                _reconciliationStatusRepository.Insert(newStatus);
                _reconciliationStatusRepository.Save();
                return newStatus.RECONCILIATION_STATUS_ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long AddNewDepositType(string newDepositTypeValue, UserProfile profile)
        {
            try
            {
                var newDepositType = new ReconciliationDepositType();
                newDepositType.DEPOSIT_TYPE_ID = CommonService.Helper.getMaximumId("FOX_RECONCILIATION_DEPOSIT_TYPE_ID");
                newDepositType.DEPOSIT_TYPE_NAME = newDepositTypeValue;

                newDepositType.PRACTICE_CODE = profile.PracticeCode;
                newDepositType.CREATED_BY = newDepositType.MODIFIED_BY = profile.UserName;
                newDepositType.CREATED_DATE = newDepositType.MODIFIED_DATE = DateTime.Now;
                newDepositType.DELETED = false;
                _reconciliationDepositTypeRepository.Insert(newDepositType);
                _reconciliationDepositTypeRepository.Save();
                return newDepositType.DEPOSIT_TYPE_ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long AddNewCategory(string newCategoryValue, UserProfile profile)
        {
            try
            {
                var newCat = new ReconciliationCategory();
                newCat.CATEGORY_ID = CommonService.Helper.getMaximumId("FOX_RECONCILIATION_CATEGORY_ID");
                newCat.CATEGORY_NAME = newCategoryValue;

                newCat.PRACTICE_CODE = profile.PracticeCode;
                newCat.CREATED_BY = newCat.MODIFIED_BY = profile.UserName;
                newCat.CREATED_DATE = newCat.MODIFIED_DATE = DateTime.Now;
                newCat.DELETED = false;
                _reconciliationCategoryRepository.Insert(newCat);
                _reconciliationCategoryRepository.Save();
                return newCat.CATEGORY_ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<MTBC_Credentials_Fox_Automation> test = new List<MTBC_Credentials_Fox_Automation>();
        public List<string> HR_Records = new List<string>();
        public HRAutoEmailsUploadResponse ReadExcelForHrEmails(string fileName, UserProfile profile)
        {
            try
            {
                DataTable tbl = new DataTable();
                ResponseModel responseModel = new ResponseModel();
                string path = @"~\FoxDocumentDirectory\HRAutoEmailsUploadedFiles\UploadFiles\" + fileName;
                path = HttpContext.Current.Server.MapPath(path);
                string fileType = path.Substring(path.Length - 3);
                long totalRecordInFile = 0;
                long total_record_updated = 0;
                List<string> Failed_Records = new List<string>();
                HRAutoEmailsUploadResponse responseData = new HRAutoEmailsUploadResponse();


                #region .xlsx File Reader Region


                bool hasHeader = true;
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = System.IO.File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }
                    // Pick Headers from 1st Excel Sheet
                    var wsmain = pck.Workbook.Worksheets[1];
                    for (var i = 1; i < 19; i++)
                    {

                        var temp = wsmain.Cells[1, i, 1, i];
                        tbl.Columns.Add(hasHeader ? temp.Text.Trim() : string.Format("Column {0}", temp.Start.Column));
                    }
                    // Pick Values from All Sheets
                    for (int wb = 1; wb <= pck.Workbook.Worksheets.Count; wb++)
                    {

                        var ws = pck.Workbook.Worksheets[wb];
                        var startRow = hasHeader ? 2 : 1;
                        bool valueExist = false;
                        if (ws.Dimension != null)
                        {
                            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                            {
                                valueExist = false;
                                var wsRow = ws.Cells[rowNum, 1, rowNum, 40];
                                DataRow row = tbl.Rows.Add();
                                foreach (var cell in wsRow)
                                {
                                    if (!string.IsNullOrEmpty(cell.Text)) valueExist = true;
                                    row[cell.Start.Column - 1] = cell.Text;
                                }
                                if (!valueExist)
                                    tbl.Rows.Remove(row);
                            }
                        }

                    }
                }
                totalRecordInFile = tbl.Rows.Count;                            
                foreach (System.Data.DataRow row in tbl.Rows)
                {
                    MTBC_Credentials_Fox_Automation temp = new MTBC_Credentials_Fox_Automation();

                    //if (!string.IsNullOrEmpty(row[""].ToString()) )
                    //   temp.Day = row[0].ToString();
                    if (row.Table.Columns.Contains("Associate ID") && !string.IsNullOrEmpty(row["Associate ID"].ToString()))
                        temp.ASSOCIATION_ID = row["Associate ID"].ToString();
                    if (row.Table.Columns.Contains("First Name") && !string.IsNullOrEmpty(row["First Name"].ToString()))
                        temp.FIRST_NAME = row["First Name"].ToString();
                    if (row.Table.Columns.Contains("Last Name") && !string.IsNullOrEmpty(row["Last Name"].ToString()))
                        temp.LAST_NAME = row["Last Name"].ToString();
                    if (row.Table.Columns.Contains("Work Contact: Work Email") && !string.IsNullOrEmpty(row["Work Contact: Work Email"].ToString()))
                        temp.WORK_EMAIL = row["Work Contact: Work Email"].ToString();
                    if (row.Table.Columns.Contains("Personel Mobile") && !string.IsNullOrEmpty(row["Personel Mobile"].ToString()))
                        temp.PERSONEL_MOBILE = row["Personel Mobile"].ToString();
                    if (row.Table.Columns.Contains("License/Certification Description") && !string.IsNullOrEmpty(row["License/Certification Description"].ToString()))
                        temp.CERTIFICATION_DESCRIPTION = row["License/Certification Description"].ToString();
                    if (row.Table.Columns.Contains("Category Description") && !string.IsNullOrEmpty(row["Category Description"].ToString()))
                        temp.CATEGORY_DESCRIPTION = row["Category Description"].ToString();
                    if (row.Table.Columns.Contains("Effective Date") && !string.IsNullOrEmpty(row["Effective Date"].ToString()))
                    {
                        DateTime date;
                        if (DateTime.TryParse(row["Effective Date"].ToString(), out date))
                            temp.EFFECTIVE_DATE = date;
                    }
                    if (row.Table.Columns.Contains("Expiration Date") && !string.IsNullOrEmpty(row["Expiration Date"].ToString()))
                    {
                        DateTime date;
                        if (DateTime.TryParse(row["Expiration Date"].ToString(), out date))
                            temp.EXPIRATION_DATE = date;
                    }
                    if (row.Table.Columns.Contains("Created Date") && !string.IsNullOrEmpty(row["Created Date"].ToString()))
                    {
                        DateTime date;
                        if (DateTime.TryParse(row["Created Datee"].ToString(), out date))
                            temp.CREATED_DATE = date;
                    }
                    temp.FILE_NAME = fileName;
                    test.Add(temp);
                    InsertExcelValuesOfHrAutoEmailsToDB(temp, profile);
                }
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB5Connection"].ToString()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("FOX_PROC_DELETE_REC_FROM_MTBC_CREDENTIALS", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FILE_NAME", fileName);
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                if (test.Count > 20000)
                {
                    responseData = new HRAutoEmailsUploadResponse();
                    responseData.IsSuccess = false;
                    responseData.ERROR_MESSAGE = "File has more than 20000 records.";
                }
                else if (test.Count > 0 && test.Count < 20000)
                {
                MTBC_Credentials_Fox_Automation last_upload_status = GetLastUploadFileStatusForHrAutoEmails(profile);
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "HRAutoEmailsLogFiles/";
                var exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                var name = fileName.Substring(0, fileName.LastIndexOf('.'));
                var logfileName = DocumentHelper.GenerateSignatureFileName(name) + ".txt";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                var pathtowriteFile = exportPath + "\\" + logfileName;
                if (last_upload_status != null)
                    {

                        responseData.IsSuccess = true;
                        responseData.HR_FILE_NAME = last_upload_status.FILE_NAME;
                        responseData.RECORD_INSERTED = last_upload_status.RECORDS_ADDED_SUCCESSFULLY;
                        responseData.TOTAL_RECORDS = test.Count;
                        responseData.Failled_Record = test.Count - last_upload_status.RECORDS_ADDED_SUCCESSFULLY;
                        responseData.Message = "Record Added";
                        responseData.Last_UPLAOD_DATE = last_upload_status.LAST_UPLOAD_DATE;
                        responseData.Upload_by = profile.UserName;
                        responseData.File_Path = pathtowriteFile;
                    }
                    else
                    {
                        responseData.Last_UPLAOD_DATE = CommonService.Helper.GetCurrentDate();
                    }

                }
                 
                generate_file(responseData, profile);
                return responseData;
                
                #endregion

            }
            catch (Exception exception)
            {
                MTBC_Credentials_Fox_Automation last_upload_status = GetLastUploadFileStatusForHrAutoEmails(profile);
                HRAutoEmailsUploadResponse responseData = new HRAutoEmailsUploadResponse();
                if (HR_Records.Count > 0)
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB5Connection"].ToString()))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("FOX_PROC_DELETE_REC_FROM_MTBC_CREDENTIALS", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FILE_NAME", fileName);
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                        connection.Close();

                    }

                    string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "HRAutoEmailsLogFiles/";
                        var exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                        var name = fileName.Substring(0, fileName.LastIndexOf('.'));
                        var logfileName = DocumentHelper.GenerateSignatureFileName(name) + ".txt";
                        if (!Directory.Exists(exportPath))
                        {
                            Directory.CreateDirectory(exportPath);
                        }
                        var pathtowriteFile = exportPath + "\\" + logfileName;
                        if (last_upload_status != null)
                        {

                            responseData.IsSuccess = true;
                            responseData.HR_FILE_NAME = last_upload_status.FILE_NAME;
                            responseData.RECORD_INSERTED = last_upload_status.RECORDS_ADDED_SUCCESSFULLY;
                            responseData.TOTAL_RECORDS = test.Count;
                            responseData.Failled_Record = test.Count - last_upload_status.RECORDS_ADDED_SUCCESSFULLY;
                            responseData.Message = "Record Added";
                            responseData.Last_UPLAOD_DATE = last_upload_status.CREATED_DATE;
                            responseData.Upload_by = profile.UserName;
                            responseData.File_Path = pathtowriteFile;
                        }
                        else
                        {
                            responseData.Last_UPLAOD_DATE = CommonService.Helper.GetCurrentDate();
                        }

                    

                    generate_file(responseData, profile);
                }                             
                if (last_upload_status != null)
                {
                    return new HRAutoEmailsUploadResponse() { ERROR_MESSAGE = exception.Message, IsSuccess = false, RECORD_EXISTED = 0, RECORD_INSERTED = last_upload_status.RECORDS_ADDED_SUCCESSFULLY, TOTAL_RECORDS = last_upload_status.RECORDS_ADDED_SUCCESSFULLY, Last_UPLAOD_DATE = last_upload_status.LAST_UPLOAD_DATE };
                }
                else
                {
                    return new HRAutoEmailsUploadResponse() { ERROR_MESSAGE = exception.Message, IsSuccess = false, RECORD_EXISTED = 0, RECORD_INSERTED = 0, TOTAL_RECORDS = 0 };
                }

            }
        }


        private void generate_file(HRAutoEmailsUploadResponse log, UserProfile profile)
        {
           
            string exportPath = "";
            string path = string.Empty;
            var filepath = log.File_Path;
            //string fileName = log.HR_FILE_NAME;
            string fileName = Path.GetFileName(filepath);                     
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                //writer.WriteLine("User_Name: " + HttpContext.Current.Request.Headers["UserName"] + Environment.NewLine + "Practice_Code: " + HttpContext.Current.Request.Headers["PracticeCode"] + Environment.NewLine + "URI: " + request.RequestUri.AbsoluteUri + Environment.NewLine +
                //    "Date :" + DateTime.Now.ToString() + Environment.NewLine + "request_status:" + cts.CanBeCanceled + "" + Environment.NewLine + "------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);
                writer.WriteLine("File Upload Status: " + Environment.NewLine + "---------------------" + Environment.NewLine + "Total Records in File: " + log.TOTAL_RECORDS + Environment.NewLine + "Records Successfully Uploaded: " + log.RECORD_INSERTED + Environment.NewLine +
                   "Records Failed: " + log.Failled_Record + Environment.NewLine + Environment.NewLine + "Uploaded On: " + log.Last_UPLAOD_DATE + Environment.NewLine + "Uploaded By: " + log.Upload_by);
            }           
        }

        private void InsertExcelValuesOfHrAutoEmailsToDB(MTBC_Credentials_Fox_Automation lst, UserProfile profile)
        {
            try
            {
                MTBC_Credentials_Fox_Automation record = new MTBC_Credentials_Fox_Automation();
                if (lst.WORK_EMAIL !=null)
                {

                    var key = lst.ASSOCIATION_ID;

                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB5Connection"].ToString()))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();

                        cmd.CommandText = "[INSERT_RECORD_IN_MTBC_Credentials_Fox_Automation]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Association_ID", lst.ASSOCIATION_ID ?? "");
                        cmd.Parameters.AddWithValue("@First_Name", lst.FIRST_NAME ?? "");
                        cmd.Parameters.AddWithValue("@Last_Name", lst.LAST_NAME ?? "");
                        cmd.Parameters.AddWithValue("@Work_Email", lst.WORK_EMAIL ?? "");
                        cmd.Parameters.AddWithValue("@Personel_Mobile", lst.PERSONEL_MOBILE ?? "");
                        cmd.Parameters.AddWithValue("@Certification_Description", lst.CERTIFICATION_DESCRIPTION ?? "");
                        cmd.Parameters.AddWithValue("@Category_Description", lst.CATEGORY_DESCRIPTION ?? "");
                        cmd.Parameters.AddWithValue("@Effective_Date", lst.EFFECTIVE_DATE ?? System.Data.SqlTypes.SqlDateTime.Null);
                        cmd.Parameters.AddWithValue("@Expiration_Date", lst.EXPIRATION_DATE ?? System.Data.SqlTypes.SqlDateTime.Null);
                        cmd.Parameters.AddWithValue("@Created_Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Deleted", false);
                        cmd.Parameters.AddWithValue("@File_Name", lst.FILE_NAME + "(" + profile.UserName + ")");
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                        //record.PRACTICE_CODE = profile.PracticeCode;
                        //record.MTBC_CREDENTIALS_AUTOMATION_ID = CommonService.Helper.getMaximumId("MTBC_CREDENTIALS_AUTOMATION_ID");
                        //    var key = record.ASSOCIATION_ID;
                        //    record.ASSOCIATION_ID = lst.ASSOCIATION_ID;
                        //    record.FIRST_NAME = lst.FIRST_NAME;
                        //    record.LAST_NAME = lst.LAST_NAME;
                        //    record.WORK_EMAIL = lst.WORK_EMAIL;
                        //    record.PERSONEL_MOBILE = lst.PERSONEL_MOBILE;
                        //    record.CERTIFICATION_DESCRIPTION = lst.CERTIFICATION_DESCRIPTION;
                        //    record.CATEGORY_DESCRIPTION = lst.CATEGORY_DESCRIPTION;
                        //    record.EFFECTIVE_DATE = lst.EFFECTIVE_DATE;
                        //    record.EXPIRATION_DATE = lst.EXPIRATION_DATE;
                        //    record.FILE_NAME = lst.FILE_NAME + "(" +profile.UserName + ")";
                        //    //record.CREATED_BY = profile.UserName;
                        //    record.CREATED_DATE = DateTime.Now;
                        //    //record.MODIFIED_BY = profile.UserName;
                        //    //record.MODIFIED_DATE = DateTime.Now;
                        //    record.DELETED = lst.DELETED;
                        //   _foxhrautoemailsRepository.Insert(record);
                        //_foxhrautoemailsRepository.Save();
                        HR_Records.Add(key);
                        connection.Close();
                    }
                }
                }
            catch (Exception ex)
            {

                throw ex;
            }

        }
            public MTBC_Credentials_Fox_Automation GetLastUploadFileStatusForHrAutoEmails(UserProfile profile)
        {
            MTBC_Credentials_Fox_Automation result = new MTBC_Credentials_Fox_Automation();

            var records = _foxhrautoemailsRepository.GetMany(t => !t.DELETED).OrderByDescending(t => t.CREATED_DATE).ToList();
            if (records != null && records.Any()  )
            { 
                var logdetails = _foxhrautoemailsRepository.GetMany(t => !t.DELETED).OrderByDescending(t => t.CREATED_DATE).First();
                result = logdetails;
                result.RECORDS_ADDED_SUCCESSFULLY = records.Count();
                result.LAST_UPLOAD_DATE = logdetails.CREATED_DATE;
                result.CREATED_BY = logdetails.FILE_NAME?.Split('(', ')')[1];
                result.FILE_NAME = logdetails.FILE_NAME?.Substring(0, result.FILE_NAME.IndexOf('('));
               
            }
            return result;
        }
        
        public ReconciliationUploadResponse ReadExcel(string fileName, UserProfile profile)
        {
            try
            {
                DataTable tbl = new DataTable();
                ResponseModel responseModel = new ResponseModel();
                string path = @"~\FoxDocumentDirectory\PatientSurvey\UploadFiles\" + fileName;
                path = HttpContext.Current.Server.MapPath(path);
                string fileType = path.Substring(path.Length - 3);
                long totalRecordInFile = 0;
                long total_record_updated = 0;
                List<string> Failed_Records = new List<string>();
                ReconciliationUploadResponse responseData = new ReconciliationUploadResponse();
                

                #region .xlsx File Reader Region

                
                    bool hasHeader = true;
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = System.IO.File.OpenRead(path))
                        {
                            pck.Load(stream);
                        }
                        // Pick Headers from 1st Excel Sheet
                        var wsmain = pck.Workbook.Worksheets[1];
                        for (var i = 1; i < 19; i++)
                        {

                            var  temp = wsmain.Cells[1, i, 1, i];
                            tbl.Columns.Add(hasHeader ? temp.Text.Trim() : string.Format("Column {0}", temp.Start.Column));
                        }
                        // Pick Values from All Sheets
                        for (int wb = 1; wb <= pck.Workbook.Worksheets.Count; wb++)
                        {

                            var ws = pck.Workbook.Worksheets[wb];
                            var startRow = hasHeader ? 2 : 1;
                            bool valueExist = false;
                            if (ws.Dimension != null)
                            {
                                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                {
                                    valueExist = false;
                                    var wsRow = ws.Cells[rowNum, 1, rowNum, 40];
                                    DataRow row = tbl.Rows.Add();
                                    foreach (var cell in wsRow)
                                    {
                                        if (!string.IsNullOrEmpty(cell.Text)) valueExist = true;
                                        row[cell.Start.Column - 1] = cell.Text;
                                    }
                                    if (!valueExist)
                                        tbl.Rows.Remove(row);
                                }
                            }

                        }
                    }
                    List<ReconcialtionImport> test = new List<ReconcialtionImport>();
                     totalRecordInFile = tbl.Rows.Count;
                    foreach (System.Data.DataRow row in tbl.Rows)
                    {
                        ReconcialtionImport temp = new ReconcialtionImport();

                        //if (!string.IsNullOrEmpty(row[""].ToString()) )
                        //    temp.Day = row[0].ToString();
                        if (row.Table.Columns.Contains("Deposit Date") && !string.IsNullOrEmpty(row["Deposit Date"].ToString()))
                            temp.DepositDate = row["Deposit Date"].ToString();
                        if (row.Table.Columns.Contains("Deposit Type") && !string.IsNullOrEmpty(row["Deposit Type"].ToString()))
                            temp.DepositType = row["Deposit Type"].ToString();
                        if (row.Table.Columns.Contains("Category / Account") && !string.IsNullOrEmpty(row["Category / Account"].ToString()))
                            temp.CategoryAccount = row["Category / Account"].ToString();
                        if (row.Table.Columns.Contains("Category/Acct") && !string.IsNullOrEmpty(row["Category/Acct"].ToString()))
                            temp.CategoryAccount = row["Category/Acct"].ToString();
                        if (row.Table.Columns.Contains("Payer Name") && !string.IsNullOrEmpty(row["Payer Name"].ToString()))
                            temp.PayerName = row["Payer Name"].ToString();
                        if (row.Table.Columns.Contains("Check # / Batch #") && !string.IsNullOrEmpty(row["Check # / Batch #"].ToString()))
                            temp.CheckNoBatchNo = row["Check # / Batch #"].ToString();
                        if (row.Table.Columns.Contains("$$ Amount") && !string.IsNullOrEmpty(row["$$ Amount"].ToString()))
                            temp.Amount = row["$$ Amount"].ToString();
                        if (row.Table.Columns.Contains("Date Assigned") && !string.IsNullOrEmpty(row["Date Assigned"].ToString()))
                            temp.DateAssigned  = row["Date Assigned"].ToString(); 
                        if (row.Table.Columns.Contains("Assigned To") && !string.IsNullOrEmpty(row["Assigned To"].ToString()))
                            temp.AssignedTo = row["Assigned To"].ToString();
                        if (row.Table.Columns.Contains("WS Date Posted") && !string.IsNullOrEmpty(row["WS Date Posted"].ToString()))
                        {
                            temp.DatePosted = row["WS Date Posted"].ToString();
                        }
                        else
                        {
                            temp.DatePosted = DateTime.Now.ToString("MM/dd/yyyy");
                        }                       
                        if (row.Table.Columns.Contains("WS Posted") && !string.IsNullOrEmpty(row["WS Posted"].ToString()))
                        {
                            temp.TotalPosted = row["WS Posted"].ToString();
                        }
                        else
                        {
                            temp.TotalPosted = "0.00";
                        }
                        if (row.Table.Columns.Contains("Not Posted") && !string.IsNullOrEmpty(row["Not Posted"].ToString()))
                            temp.NotPosted = row["Not Posted"].ToString();
                        test.Add(temp);                       
                    }
                    if (test.Count > 20000 )
                    {
                        responseData = new ReconciliationUploadResponse();
                        responseData.IsSuccess = false;
                        responseData.ERROR_MESSAGE = "File has more than 20000 records.";
                    }
                    else if (test.Count > 0 && test.Count < 20000)
                    {
                        responseData = InsertExcelValuesToDB(test, profile);
                        if (responseData != null)
                        {
                            responseData.IsSuccess = true;
                            responseData.TOTAL_RECORDS = test.Count;
                        }

                        FOX_TBL_RECONCILIATION_UPLOAD_LOG last_upload_status = GetLastUploadFileStatus(profile);
                        if (last_upload_status != null)
                        {
                            responseData.IsSuccess = true;
                            responseData.RECORD_INSERTED = last_upload_status.SUCCESSFULLY_ADDED;
                            responseData.TOTAL_RECORDS = last_upload_status.TOTAL_RECORDS;
                            responseData.Last_UPLAOD_DATE = last_upload_status.LAST_UPDATED_DATE; 
                        }
                        else
                        {
                            responseData.Last_UPLAOD_DATE = CommonService.Helper.GetCurrentDate();
                        }                      

                    }
                
                return responseData;
                
                #endregion
               
            }
            catch (Exception exception)
            {
                FOX_TBL_RECONCILIATION_UPLOAD_LOG last_upload_status = GetLastUploadFileStatus(profile);
                if (last_upload_status != null)
                {
                    return new ReconciliationUploadResponse() { ERROR_MESSAGE = exception.Message, IsSuccess = false, RECORD_EXISTED = 0, RECORD_INSERTED = last_upload_status.SUCCESSFULLY_ADDED, TOTAL_RECORDS = last_upload_status.TOTAL_RECORDS, Last_UPLAOD_DATE = last_upload_status.LAST_UPDATED_DATE };
                }
                else
                {
                    return new ReconciliationUploadResponse() { ERROR_MESSAGE = exception.Message, IsSuccess = false, RECORD_EXISTED = 0, RECORD_INSERTED = 0, TOTAL_RECORDS = 0 };
                }
            }
        }

        private ReconciliationUploadResponse InsertExcelValuesToDB(List<ReconcialtionImport> lst, UserProfile profile)
        {          
           
                DataTable dt = GetReconsiliatinTable(lst);

                SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                SqlParameter tempdata = new SqlParameter("RECONCILIATION_DATA", SqlDbType.Structured);
                tempdata.TypeName = "RECONCILIATION_CP_IMPORT";
                tempdata.Value = dt;

            return  SpRepository<ReconciliationUploadResponse>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_RECONCILIATION_EXCEL_DATA_TEST @PRACTICE_CODE, @USER_NAME, @RECONCILIATION_DATA", practiceCode, userName, tempdata);
            
        }

        public FOX_TBL_RECONCILIATION_UPLOAD_LOG GetLastUploadFileStatus(UserProfile profile)
        {
            SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            return SpRepository<FOX_TBL_RECONCILIATION_UPLOAD_LOG>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_RECONCILIATION_UPLOAD_LOG @PRACTICE_CODE", practiceCode);
        }



        public List<SOFT_RECONCILIATION_PAYMENT> GetSoftReconsilitionPayment(SOFT_RECONCILIATION_SERACH_REQUEST obj, UserProfile profile)
        {
            SqlParameter practiceCode = new SqlParameter("STRPRACTICECODE", profile.PracticeCode);
            SqlParameter depositslipid = new SqlParameter("STRDEPOSITID", string.Empty);
            SqlParameter checkNo = new SqlParameter("CHECK_NO", obj.CHECK_NO);
            SqlParameter currentpage = new SqlParameter("CURRENT_PAGE", obj.CurrentPage);
            SqlParameter recordsperpage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = 0 };
            var result = SpRepository<SOFT_RECONCILIATION_PAYMENT>.GetListWithStoreProcedure(@"FOX_PROC_GET_DEPOSITSLIP_CLAIMS_TEST @STRPRACTICECODE,@STRDEPOSITID,@CHECK_NO,@CURRENT_PAGE,@RECORD_PER_PAGE", practiceCode, depositslipid,checkNo,currentpage,recordsperpage);
            if (result?.Count > 0)
            {
                int CURRENT_PAGE = obj.CurrentPage - 1;
                int START_FROM = CURRENT_PAGE * obj.RecordPerPage;
                result[0].TOTAL_RECORDS = result.Count;
                result.Select(c => { c.TOTAL_RECORDS = result.Count; return c; }).ToList();

                if (result.Count <= obj.RecordPerPage)
                {
                    result.Select(c => { c.TOTAL_RECORD_PAGES = 1; return c; }).ToList();
                }
                else
                {
                    result.Select(c => { c.TOTAL_RECORD_PAGES = Math.Ceiling(((double)result.Count / (double)obj.RecordPerPage)); return c; }).ToList();
                }
                SetListSerialNumber(result);
                result = result.FindAll(e => e.Row_No > START_FROM && e.Row_No <= (obj.CurrentPage * obj.RecordPerPage));
                return result;
            }
            else
            {
                return result;
            }
            }


        /// <summary>Get Payment record from Websoft </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>SOFT_RECONCILIATION_SERACH_REQUEST(search check#) , user(Get Current user info) </param>

        public List<SOFT_RECONCILIATION_PAYMENT> GetWebsoftPayment(SOFT_RECONCILIATION_SERACH_REQUEST softRequest, UserProfile profile)
        {
            SqlParameter practiceCode = new SqlParameter("STRPRACTICECODE", profile.PracticeCode);
            SqlParameter checkNo = new SqlParameter("CHECK_NO", softRequest.CHECK_NO);
            var result = SpRepository<SOFT_RECONCILIATION_PAYMENT>.GetListWithStoreProcedure(@" FOX_PROC_GET_DEPOSITSLIP_CLAIMS_AMOUNT_TEST @STRPRACTICECODE,@CHECK_NO", practiceCode, checkNo);
            if(result!= null)
            {
                return result;
            }
            else
            {
                 return new List<SOFT_RECONCILIATION_PAYMENT>();
             }
           
         }

        /// <summary>Get Serail Numbers </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>SOFT_RECONCILIATION_PAYMENT(get list) </param>
        private void SetListSerialNumber(List<SOFT_RECONCILIATION_PAYMENT> softPaymentList)
        {
            int i = 1;
            foreach (SOFT_RECONCILIATION_PAYMENT obj in softPaymentList)
            {
                obj.Row_No = i++;
            }
        }
        private DataTable GetReconsiliatinTable(List<ReconcialtionImport> lst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DEPOSIT_DATE");
            dt.Columns.Add("DEPOSIT_TYPE");
            dt.Columns.Add("CATEGORY_ACCOUNT");
            dt.Columns.Add("INSURANCE_NAME");
            dt.Columns.Add("CHECK_NO");
            dt.Columns.Add("AMOUNT");
            dt.Columns.Add("AMOUNT_POSTED");
            dt.Columns.Add("AMOUNT_NOT_POSTED");
            dt.Columns.Add("DATE_ASSIGNED");
            dt.Columns.Add("ASSIGNED_TO");
            dt.Columns.Add("DATE_ENTERED");
            dt.Columns.Add("BATCH_NO");
            dt.Columns.Add("DATE_POSTED");
            //dt.Columns.Add("ASSIGNED_GROUP");
            //dt.Columns.Add("ASSIGNED_GROUP_DATE");

            foreach (ReconcialtionImport recon in lst)
            {
                //string checkNo = GetCheckNo(recon.CheckNoBatchNo);
                string checkNo = recon.CheckNoBatchNo;
                if (!string.IsNullOrEmpty(checkNo) && checkNo.Length > 2)
                {
                    DataRow dr = dt.Rows.Add();
                    dr["DEPOSIT_DATE"] = string.IsNullOrEmpty( recon.DepositDate) ? string.Empty : recon.DepositDate;
                    dr["DEPOSIT_TYPE"] = string.IsNullOrEmpty( recon.DepositType) ? string.Empty : recon.DepositType;
                    dr["CATEGORY_ACCOUNT"] = string.IsNullOrEmpty( recon.CategoryAccount) ? string.Empty : recon.CategoryAccount;
                    dr["INSURANCE_NAME"] = string.IsNullOrEmpty( recon.PayerName) ? string.Empty : recon.PayerName;
                    dr["CHECK_NO"] = checkNo.Length > 50 ? checkNo.Substring(0,50): checkNo;// recon.CheckNoBatchNo.Substring(0,45);
                    dr["BATCH_NO"] = string.IsNullOrEmpty(recon.CheckNoBatchNo) ? string.Empty : recon.CheckNoBatchNo.Length > 500 ? recon.CheckNoBatchNo.Substring(0,499): recon.CheckNoBatchNo;
                    dr["AMOUNT"] = string.IsNullOrEmpty(recon.Amount) ? "0.00" : recon.Amount.Replace("$", string.Empty);
                    dr["AMOUNT_POSTED"] = string.IsNullOrEmpty(recon.TotalPosted) ? "0.00" : recon.TotalPosted.Replace("$", string.Empty);
                    dr["AMOUNT_NOT_POSTED"] = string.IsNullOrEmpty(recon.NotPosted) ? "0.00" : recon.NotPosted.Replace("$", string.Empty);
                    //dr["DATE_ASSIGNED"] = recon.DateAssigned;
                    //dr["ASSIGNED_TO"] = recon.AssignedTo;
                    dr["DATE_POSTED"] = string.IsNullOrEmpty(recon.DatePosted) ? string.Empty : recon.DatePosted;
                    dr["ASSIGNED_TO"] = string.IsNullOrEmpty(recon.AssignedTo) ? string.Empty : (recon.AssignedTo.Length > 70 ? recon.AssignedTo.Substring(0, 70) : recon.AssignedTo);
                    dr["DATE_ASSIGNED"] = string.IsNullOrEmpty(recon.DateAssigned) ? string.Empty : recon.DateAssigned;
                    dr["DATE_ENTERED"] = string.IsNullOrEmpty(recon.DateEntered) ? string.Empty : recon.DateEntered;
                    //if (!string.IsNullOrEmpty(recon.DateEntered))
                    //{
                    //    dr["DATE_ENTERED"] = recon.DateEntered;
                    //}
                }
                //dt.Rows.Add(dr);
                
            }
            return dt;
        }

        private string GetCheckNo(string checkNo)
        {
            string temp = string.Empty;
            if (string.IsNullOrEmpty(checkNo))
            {
                return string.Empty;
            }
            else if (!checkNo.Contains("*"))
            {
                temp = checkNo;
            }
            else if (checkNo.Contains("****"))
            {
                temp = checkNo.Substring(checkNo.LastIndexOf('*') + 1);
            }
            else
            {
                temp = GetCheckNoBetweenFirstSecondAsterik(checkNo);
            }          
           
            return temp;
        }

        private string GetCheckNoBetweenFirstSecondAsterik(string checkNo)
        {
            StringBuilder temp = new StringBuilder(string.Empty);
            for (int index = 0; index < checkNo.Length; index++)
            {
                bool breakloop = false;
                if (checkNo[index] == '*')
                {

                    index++;
                    for (int i = index; i < checkNo.Length; i++)
                    {
                        if (checkNo[i] == '*')
                        {
                            breakloop = true;
                            break;
                        }
                        temp.Append(checkNo[i]);

                    }
                }
                if (breakloop)
                {
                    break;
                }
            }

            if (temp.ToString().ToLower().StartsWith("eft"))
            {
                temp.Remove(0, 3);
            }
            return temp.ToString();

        }
    }
}
