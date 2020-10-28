using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CorrectedClaims;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FOX.BusinessOperations.CorrectedClaimService
{
    public class CorrectedClaimService : ICorrectedClaimService
    {

        private readonly DbContextCorrectedClaims _DbContextCorrectedClaims = new DbContextCorrectedClaims();
        private readonly GenericRepository<FOX_TBL_CASE> _CaseRepository;
        private readonly GenericRepository<CORRECTED_CLAIM> _CorrectedClaimRepository;
        private readonly GenericRepository<CORRECTED_CLAIM_TYPE> _ClaimTypeRepository;
        private readonly GenericRepository<AdjustmentClaimStatus> _AdjClaimStatusRepository;
        private readonly GenericRepository<CorrectedClaimLog> _CorrectedClaimLogRepository;



        public CorrectedClaimService()
        {
            _CorrectedClaimRepository = new GenericRepository<CORRECTED_CLAIM>(_DbContextCorrectedClaims);
            _ClaimTypeRepository = new GenericRepository<CORRECTED_CLAIM_TYPE>(_DbContextCorrectedClaims);
            _AdjClaimStatusRepository = new GenericRepository<AdjustmentClaimStatus>(_DbContextCorrectedClaims);
            _CaseRepository = new GenericRepository<FOX_TBL_CASE>(_DbContextCorrectedClaims);
            _CorrectedClaimLogRepository = new GenericRepository<CorrectedClaimLog>(_DbContextCorrectedClaims);

        }
        public List<CORRECTED_CLAIM_TYPE> GetClaimTypes(UserProfile profile)
        {
            List<CORRECTED_CLAIM_TYPE> Types = new List<CORRECTED_CLAIM_TYPE>();
            try
            {

                Types = _ClaimTypeRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode).ToList();
                if (Types.Any())
                    return Types;
                else
                    return new List<CORRECTED_CLAIM_TYPE>();
            }
            catch (Exception)
            {
                return new List<CORRECTED_CLAIM_TYPE>();
            }

        }
        public List<PatientCases> GetPatientCases(string PatientAccount, UserProfile profile)
        {

            var _PatientAccount = Convert.ToInt64(PatientAccount);
            try
            {
                var lst = _CaseRepository.GetManyQueryable(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.PATIENT_ACCOUNT == _PatientAccount && x.CASE_NO != null)
                    .Select(t => new PatientCases() { CASE_NO = t.CASE_NO, CASE_ID = t.CASE_ID }).ToList();
                if (lst.Any())
                    return lst;
                else
                    return new List<PatientCases>();
            }
            catch (Exception ex)
            {
                return new List<PatientCases>();
            }

        }

        public List<AdjustmentClaimStatus> GetAdjustedClaim(UserProfile profile)
        {
            List<AdjustmentClaimStatus> Types = new List<AdjustmentClaimStatus>();
            try
            {
                Types = _AdjClaimStatusRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.STATUS_CATEGORY == "Corrected Claims").ToList();
                if (Types.Any())
                    return Types;
                else
                    return new List<AdjustmentClaimStatus>();
            }
            catch (Exception)
            {
                return new List<AdjustmentClaimStatus>();
            }

        }
        public List<SmartPatientRes> GetSmartPatient(string searchText, UserProfile profile)
        {
            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = searchText };
                var result = SpRepository<SmartPatientRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_PATIENTS] @PRACTICE_CODE, @SEARCHVALUE",
                    parmPracticeCode, smartvalue).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SmartPatientRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CorrectedClaimResponse InsertUpdateCorrectedClaim(CORRECTED_CLAIM obj, UserProfile profile)
        {

            CorrectedClaimResponse result = new CorrectedClaimResponse();
            result.Message = "";
            try
            {
                List<CorrectedClaimLog> claimLoglist = new List<CorrectedClaimLog>();
                bool isEditCorrectedClaim = false;
                if (obj != null && profile != null)
                {

                    var ClaimObj = _CorrectedClaimRepository.GetFirst(x => x.CORRECTED_CLAIM_ID == obj.CORRECTED_CLAIM_ID && x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED);
                    if (ClaimObj == null)
                    {
                        ClaimObj = new CORRECTED_CLAIM();
                        ClaimObj.CORRECTED_CLAIM_ID = Helper.getMaximumId("FOX_CORRECTED_CLAIM_ID");
                        ClaimObj.REQUESTED_BY = ClaimObj.CREATED_BY = ClaimObj.MODIFIED_BY = profile.UserName;
                        ClaimObj.PRACTICE_CODE = profile.PracticeCode;
                        ClaimObj.CREATED_DATE = ClaimObj.MODIFIED_DATE = DateTime.Now;
                        result.Message = "created";
                        isEditCorrectedClaim = false;
                        claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Corrected Claim Created", ACTION_DETAIL = "Corrected Claim Created against Patient: " + obj.Patient_Name });
                        result.CORRECTED_CLAIM_ID = ClaimObj.CORRECTED_CLAIM_ID;
                    }
                    else
                    {

                        isEditCorrectedClaim = true;
                        if (!string.IsNullOrEmpty(obj.DOS_FROM_Str))
                            obj.DOS_FROM = Convert.ToDateTime(obj.DOS_FROM_Str);
                        if (!string.IsNullOrEmpty(obj.DOS_TO_Str))
                            obj.DOS_TO = Convert.ToDateTime(obj.DOS_TO_Str);
                        if (!string.IsNullOrEmpty(obj.RESPONSE_DATE_Str))
                            obj.RESPONSE_DATE = Convert.ToDateTime(obj.RESPONSE_DATE_Str);
                        if (!string.IsNullOrEmpty(obj.REQUESTED_DATE_Str))
                            obj.REQUESTED_DATE = Convert.ToDateTime(obj.REQUESTED_DATE_Str);
                        if (!string.IsNullOrEmpty(obj.CHARGE_ENTRY_DATE_Str))
                            obj.CHARGE_ENTRY_DATE = Convert.ToDateTime(obj.CHARGE_ENTRY_DATE_Str);
                        if (!string.IsNullOrEmpty(obj.WORK_DATE_Str))
                            obj.WORK_DATE = Convert.ToDateTime(obj.WORK_DATE_Str);

                        if (obj.DELETED != true || !isEditCorrectedClaim)
                        {
                            if (ClaimObj.PATIENT_ACCOUNT.HasValue && ClaimObj.PATIENT_ACCOUNT != obj.PATIENT_ACCOUNT)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.PATIENT_ACCOUNT == ClaimObj.PATIENT_ACCOUNT);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Paitent changed", ACTION_DETAIL = "Paitent Updated: " + obj.Patient_Name });
                            }
                            if (ClaimObj.CHART_ID != null && ClaimObj.CHART_ID != obj.CHART_ID)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.CHART_ID == ClaimObj.CHART_ID);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "MRN changed", ACTION_DETAIL = "MRN Number Updated: " + obj.CHART_ID });
                            }
                            if (ClaimObj.CORRECTED_CLAIM_TYPE_ID.HasValue && ClaimObj.CORRECTED_CLAIM_TYPE_ID != obj.CORRECTED_CLAIM_TYPE_ID)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.CORRECTED_CLAIM_TYPE_ID == obj.CORRECTED_CLAIM_TYPE_ID);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Corrected Claim Type Changed", ACTION_DETAIL = "Corrected Claim Type Updated: " + obj.CORRECTED_CLAIMS_TYPE_DESC });
                            }
                            if (ClaimObj.CASE_NO != null && ClaimObj.CASE_NO.ToLower() != obj.CASE_NO.ToLower())
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.CASE_NO == ClaimObj.CASE_NO);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Case # Changed", ACTION_DETAIL = "Case # Updated: " + obj.CASE_NO });
                            }
                            if (ClaimObj.FOX_TBL_INSURANCE_ID.HasValue && ClaimObj.FOX_TBL_INSURANCE_ID != obj.FOX_TBL_INSURANCE_ID)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.FOX_TBL_INSURANCE_ID == ClaimObj.FOX_TBL_INSURANCE_ID);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Insurance changed", ACTION_DETAIL = "Insurance Updated: " + obj.INSURANCE_NAME });
                            }
                            if (ClaimObj.SOURCE_ID.HasValue && ClaimObj.SOURCE_ID != obj.SOURCE_ID)
                            {
                                //Update 
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.SOURCE_ID == ClaimObj.SOURCE_ID);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Therapist changed", ACTION_DETAIL = "Therapist Updated: " + obj.Theripist });
                            }
                            if (ClaimObj.DOS_FROM.HasValue && ClaimObj.DOS_FROM.Value.Date != obj.DOS_FROM.Value.Date)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.DOS_FROM == ClaimObj.DOS_FROM);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "DOS From changed", ACTION_DETAIL = "DOS From Updated: " + obj.DOS_FROM_Str });
                            }
                            if (ClaimObj.CHARGE_ENTRY_DATE.HasValue && ClaimObj.CHARGE_ENTRY_DATE.Value.Date != obj.CHARGE_ENTRY_DATE.Value.Date)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.CHARGE_ENTRY_DATE == ClaimObj.CHARGE_ENTRY_DATE);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Charge Entry Date changed", ACTION_DETAIL = "Charge Entry Date Updated: " + obj.CHARGE_ENTRY_DATE_Str });
                            }
                            if (ClaimObj.WORK_DATE.HasValue && ClaimObj.WORK_DATE.Value.Date != obj.WORK_DATE.Value.Date)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.WORK_DATE == ClaimObj.WORK_DATE);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Work Date changed", ACTION_DETAIL = "Work Date Updated: " + obj.WORK_DATE_Str });
                            }
                            if (ClaimObj.DOS_TO.HasValue && ClaimObj.DOS_TO.Value.Date != obj.DOS_TO.Value.Date)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.DOS_TO == ClaimObj.DOS_TO);
                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "DOS To changed", ACTION_DETAIL = "DOS To  Updated: " + obj.DOS_TO_Str });
                            }
                            if (ClaimObj.STATUS_ID.HasValue && ClaimObj.STATUS_ID != obj.STATUS_ID)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.STATUS_ID == ClaimObj.STATUS_ID);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Staus changed", ACTION_DETAIL = "Staus Updated: " + obj.STATUS_NAME });
                            }
                            if (obj.REQUESTED_DATE != null && ClaimObj.REQUESTED_DATE.HasValue && ClaimObj.REQUESTED_DATE.Value.Date != obj.REQUESTED_DATE.Value.Date)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.REQUESTED_DATE == ClaimObj.REQUESTED_DATE);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Requested Date changed", ACTION_DETAIL = "Requested Date  Updated: " + obj.REQUESTED_DATE });
                            }
                            if (obj.RESPONSE_DATE != null && ClaimObj.RESPONSE_DATE.HasValue && ClaimObj.RESPONSE_DATE.Value.Date != obj.RESPONSE_DATE.Value.Date)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.RESPONSE_DATE == ClaimObj.RESPONSE_DATE);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Response Date changed", ACTION_DETAIL = "Response Date Updated: " + obj.RESPONSE_DATE });
                            }
                            if (ClaimObj.REMARKS != null && ClaimObj.REMARKS != obj.REMARKS)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.REMARKS == ClaimObj.REMARKS);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Remarks changed", ACTION_DETAIL = "Remarks  Updated: " + obj.REMARKS });
                            }
                            if (ClaimObj.RESPONSE != null && ClaimObj.RESPONSE != obj.RESPONSE)
                            {
                                //Update
                                var cate = _CorrectedClaimRepository.GetFirst(x => !x.DELETED && x.RESPONSE == ClaimObj.RESPONSE);

                                claimLoglist.Add(new CorrectedClaimLog() { ACTION = "Response changed", ACTION_DETAIL = "Response  Updated: " + obj.RESPONSE });
                            }
                            result.Message = "updated";

                        }

                        else
                        {
                            result.Message = "deleted";
                        }

                    }
                    if (obj.RESPONSE_DATE != null)
                    {
                        ClaimObj.RESPONSE_BY = profile.UserName;
                        ClaimObj.RESPONSE = obj.RESPONSE;
                    }
                    if (!string.IsNullOrEmpty(obj.DOS_FROM_Str))
                        ClaimObj.DOS_FROM = Convert.ToDateTime(obj.DOS_FROM_Str);
                    if (!string.IsNullOrEmpty(obj.DOS_TO_Str))
                        ClaimObj.DOS_TO = Convert.ToDateTime(obj.DOS_TO_Str);
                    if (!string.IsNullOrEmpty(obj.RESPONSE_DATE_Str))
                        ClaimObj.RESPONSE_DATE = Convert.ToDateTime(obj.RESPONSE_DATE_Str);
                    if (!string.IsNullOrEmpty(obj.REQUESTED_DATE_Str))
                        ClaimObj.REQUESTED_DATE = Convert.ToDateTime(obj.REQUESTED_DATE_Str);
                    if (!string.IsNullOrEmpty(obj.CHARGE_ENTRY_DATE_Str))
                        ClaimObj.CHARGE_ENTRY_DATE = Convert.ToDateTime(obj.CHARGE_ENTRY_DATE_Str);
                    if (!string.IsNullOrEmpty(obj.WORK_DATE_Str))
                        ClaimObj.WORK_DATE = Convert.ToDateTime(obj.WORK_DATE_Str);
                    if (!string.IsNullOrEmpty(obj.Patient_AccountStr))
                    {
                        ClaimObj.PATIENT_ACCOUNT = Convert.ToInt64(obj.Patient_AccountStr);
                    }
                    else
                    {
                        ClaimObj.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                    }
                    ClaimObj.CASE_NO = obj.CASE_NO;
                    ClaimObj.CHART_ID = obj.CHART_ID;
                    ClaimObj.STATUS_ID = obj.STATUS_ID;
                    ClaimObj.DELETED = obj.DELETED;
                    ClaimObj.CORRECTED_CLAIM_TYPE_ID = obj.CORRECTED_CLAIM_TYPE_ID;
                    ClaimObj.FOX_TBL_INSURANCE_ID = obj.FOX_TBL_INSURANCE_ID;
                    ClaimObj.REMARKS = obj.REMARKS;
                    ClaimObj.SOURCE_ID = obj.SOURCE_ID;
                    ClaimObj.STATUS_ID = obj.STATUS_ID;
                    if (isEditCorrectedClaim)
                    {
                        _CorrectedClaimRepository.Update(ClaimObj);
                    }
                    else
                    {
                        _CorrectedClaimRepository.Insert(ClaimObj);
                    }
                    if (claimLoglist.Count() > 0)
                    {
                        claimLoglist.ForEach(
                            t =>
                            {
                                t.CORRECTED_CLAIM_LOG_ID = Helper.getMaximumId("FOX_CORRECTED_CLAIM_LOG_ID");
                                t.PRACTICE_CODE = profile.PracticeCode;
                                t.CORRECTED_CLAIM_ID = ClaimObj.CORRECTED_CLAIM_ID;
                                t.CREATED_BY = t.MODIFIED_BY = profile.UserName;
                                t.CREATED_DATE = t.MODIFIED_DATE = Helper.GetCurrentDate();
                            }
                            );
                        AddClaimLogs(claimLoglist);
                    }
                    _DbContextCorrectedClaims.SaveChanges();
                }
                return result;

            }
            catch (Exception ex)
            {
                //throw ex;
                return result;
            }

        }

        public CorrectedClaimData GetCorrectedClaimData(CorrectedClaimSearch correctedClaimSearch, UserProfile profile)
        {

            var correctedclaimdata = new CorrectedClaimData();
            //Indexed date mapping....
            if (!string.IsNullOrEmpty(correctedClaimSearch.DOS_FROM_STR))
                correctedClaimSearch.DOS_FROM = Convert.ToDateTime(correctedClaimSearch.DOS_FROM_STR);
            if (!string.IsNullOrEmpty(correctedClaimSearch.DOS_TO_STR))
                correctedClaimSearch.DOS_TO = Convert.ToDateTime(correctedClaimSearch.DOS_TO_STR);
            if (correctedClaimSearch.DOS_FROM.HasValue)
                if (correctedClaimSearch.DOS_TO.HasValue)
                    if (String.Equals(correctedClaimSearch.DOS_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                        correctedClaimSearch.DOS_TO = correctedClaimSearch.DOS_TO.Value.AddDays(1).AddSeconds(-1);
                    else
                        correctedClaimSearch.DOS_TO = correctedClaimSearch.DOS_TO.Value.AddSeconds(59);
                else
                    correctedClaimSearch.DOS_TO = Helper.GetCurrentDate();
            else if (correctedClaimSearch.DOS_TO.HasValue)
            {
                if (String.Equals(correctedClaimSearch.DOS_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                    correctedClaimSearch.DOS_TO = correctedClaimSearch.DOS_TO.Value.AddDays(1).AddSeconds(-1);
                var dateNow = Helper.GetCurrentDate();
                correctedClaimSearch.DOS_FROM = dateNow.AddYears(-100);
            }

            //Received date mapping....
            if (!string.IsNullOrEmpty(correctedClaimSearch.REQ_FROM_STR))
                correctedClaimSearch.REQ_FROM = Convert.ToDateTime(correctedClaimSearch.REQ_FROM_STR);
            if (!string.IsNullOrEmpty(correctedClaimSearch.REQ_TO_STR))
                correctedClaimSearch.REQ_TO = Convert.ToDateTime(correctedClaimSearch.REQ_TO_STR);
            if (correctedClaimSearch.REQ_FROM.HasValue)
                if (correctedClaimSearch.REQ_TO.HasValue)
                    if (String.Equals(correctedClaimSearch.REQ_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                        correctedClaimSearch.REQ_TO = correctedClaimSearch.REQ_TO.Value.AddDays(1).AddSeconds(-1);
                    else
                        correctedClaimSearch.REQ_TO = correctedClaimSearch.REQ_TO.Value.AddSeconds(59);
                else
                    correctedClaimSearch.REQ_TO = Helper.GetCurrentDate();
            else if (correctedClaimSearch.REQ_TO.HasValue)
            {
                if (String.Equals(correctedClaimSearch.REQ_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                    correctedClaimSearch.REQ_TO = correctedClaimSearch.REQ_TO.Value.AddDays(1).AddSeconds(-1);
                var dateNow = Helper.GetCurrentDate();
                correctedClaimSearch.REQ_FROM = dateNow.AddYears(-100);
            }
            if (!string.IsNullOrEmpty(correctedClaimSearch.PATIENT_ACCOUNT_Str))
            {
                correctedClaimSearch.PATIENT_ACCOUNT = Convert.ToInt64(correctedClaimSearch.PATIENT_ACCOUNT_Str);
            }
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = correctedClaimSearch.currentPage };
            var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = correctedClaimSearch.recordPerpage };
            var searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = correctedClaimSearch.searchString };
            var SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = correctedClaimSearch.sortBy };
            var SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = correctedClaimSearch.sortOrder };
            var dOS_FROM = Helper.getDBNullOrValue("DOS_FROM", correctedClaimSearch.DOS_FROM.ToString());
            var dOS_TO = Helper.getDBNullOrValue("DOS_TO", correctedClaimSearch.DOS_TO.ToString());
            var rEQ_FROM = Helper.getDBNullOrValue("REQ_FROM", correctedClaimSearch.REQ_FROM.ToString());
            var rEQ_TO = Helper.getDBNullOrValue("REQ_TO", correctedClaimSearch.REQ_TO.ToString());
            var cORRECTED_CLAIM_TYPE_ID = new SqlParameter("CORRECTED_CLAIM_TYPE_ID", SqlDbType.Int) { Value = correctedClaimSearch.CORRECTED_CLAIM_TYPE_ID };
            var cASE_NO = Helper.getDBNullOrValue("CASE_NO", correctedClaimSearch.CASE_NO);
            var sOURCE_ID = new SqlParameter("SOURCE_ID", SqlDbType.BigInt) { Value = correctedClaimSearch.SOURCE_ID };
            var sTATUS_ID = new SqlParameter("STATUS_ID", SqlDbType.Int) { Value = correctedClaimSearch.STATUS_ID };
            var pATIENT_ACCOUNT = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = correctedClaimSearch.PATIENT_ACCOUNT };
            var CorrectedClaimID = new SqlParameter("CORRECTED_CLAIM_ID", SqlDbType.BigInt) { Value = correctedClaimSearch.CORRECTED_CLAIM_ID };

            correctedclaimdata.CorrectedClaims = SpRepository<CorrectedClaimResponse>.GetListWithStoreProcedure(@"exec FOX_GET_CORRECTED_CLAIM @PRACTICE_CODE, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE, 
      @SORT_BY,@SORT_ORDER, @CORRECTED_CLAIM_TYPE_ID,@CASE_NO,@SOURCE_ID,@DOS_FROM,@DOS_TO,@REQ_FROM,@REQ_TO,@STATUS_ID,@PATIENT_ACCOUNT,@CORRECTED_CLAIM_ID",
                parmPracticeCode, searchString, CurrentPage, RecordPerPage, SortBy, SortOrder,
                cORRECTED_CLAIM_TYPE_ID, cASE_NO, sOURCE_ID, dOS_FROM, dOS_TO, rEQ_FROM, rEQ_TO, sTATUS_ID, pATIENT_ACCOUNT, CorrectedClaimID).ToList();

            correctedclaimdata.CorrectedClaimSummary = GetCorrectedClaimSummary(correctedClaimSearch, profile);
            return correctedclaimdata;

        }


        public List<CorrectedClaimLog> GetCorrectedClaimLog(long CORRECTED_CLAIM_ID, UserProfile profile)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var cORRECTED_CLAIM_ID = new SqlParameter("CORRECTED_CLAIM_ID", SqlDbType.BigInt) { Value = CORRECTED_CLAIM_ID };
                var result = SpRepository<CorrectedClaimLog>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_CORRECTED_CLAIM_LOG @PRACTICE_CODE, @CORRECTED_CLAIM_ID", parmPracticeCode, cORRECTED_CLAIM_ID).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<CorrectedClaimLog>();
            }
            catch (Exception)
            {
                return new List<CorrectedClaimLog>();
            }


        }


        public CorrectedClaimSummaryModel GetCorrectedClaimSummary(CorrectedClaimSearch correctedClaimSearch, UserProfile profile)
         {
            CorrectedClaimSummaryModel obj = new CorrectedClaimSummaryModel();
            obj.ClosedTotal = 0;
            obj.OpenTotal = 0;
            obj.PendingTotal = 0;
            obj.InProgressTotal = 0;
            obj.ReviewTotal = 0;
            obj.Total = 0;

            try
            {


                if (!string.IsNullOrEmpty(correctedClaimSearch.PATIENT_ACCOUNT_Str))
                {
                    correctedClaimSearch.PATIENT_ACCOUNT = Convert.ToInt64(correctedClaimSearch.PATIENT_ACCOUNT_Str);
                }
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var dOS_FROM = Helper.getDBNullOrValue("DOS_FROM", correctedClaimSearch.DOS_FROM.ToString());
                var dOS_TO = Helper.getDBNullOrValue("DOS_TO", correctedClaimSearch.DOS_TO.ToString());
                var rEQ_FROM = Helper.getDBNullOrValue("REQ_FROM", correctedClaimSearch.REQ_FROM.ToString());
                var rEQ_TO = Helper.getDBNullOrValue("REQ_TO", correctedClaimSearch.REQ_TO.ToString());
                var cORRECTED_CLAIM_TYPE_ID = new SqlParameter("CORRECTED_CLAIM_TYPE_ID", SqlDbType.Int) { Value = correctedClaimSearch.CORRECTED_CLAIM_TYPE_ID };
                var cASE_NO = Helper.getDBNullOrValue("CASE_NO", correctedClaimSearch.CASE_NO);
                var sOURCE_ID = new SqlParameter("SOURCE_ID", SqlDbType.BigInt) { Value = correctedClaimSearch.SOURCE_ID };
                var sTATUS_ID = new SqlParameter("STATUS_ID", SqlDbType.Int) { Value = correctedClaimSearch.STATUS_ID };
                var pATIENT_ACCOUNT = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = correctedClaimSearch.PATIENT_ACCOUNT };
                var CorrectedClaimID = new SqlParameter("CORRECTED_CLAIM_ID", SqlDbType.BigInt) { Value = correctedClaimSearch.CORRECTED_CLAIM_ID };
                obj.summary = SpRepository<CorrectedClaimSummary>.GetListWithStoreProcedure(@"exec FOX_GET_CORRECTED_CLAIM_SUMMARY @PRACTICE_CODE, @CORRECTED_CLAIM_TYPE_ID,@CASE_NO,@SOURCE_ID,@DOS_FROM,@DOS_TO,@REQ_FROM,@REQ_TO,@STATUS_ID,@PATIENT_ACCOUNT,@CORRECTED_CLAIM_ID",
               parmPracticeCode, cORRECTED_CLAIM_TYPE_ID, cASE_NO, sOURCE_ID, dOS_FROM, dOS_TO, rEQ_FROM, rEQ_TO, sTATUS_ID, pATIENT_ACCOUNT, CorrectedClaimID).ToList();
                if (obj.summary.Any())
                {

                    for (int i = 0; i < obj.summary.Count(); i++)
                    {
                        if (obj.summary[i].STATUS_NAME.Equals("Open"))
                        {
                            obj.OpenTotal = obj.summary[i].TOTAL;
                        }
                        if (obj.summary[i].STATUS_NAME.Equals("In-Progress"))
                        {
                            obj.InProgressTotal = obj.summary[i].TOTAL;
                        }
                        if (obj.summary[i].STATUS_NAME.Equals("Pending"))
                        {
                            obj.PendingTotal = obj.summary[i].TOTAL;
                        }
                        if (obj.summary[i].STATUS_NAME.Equals("Closed"))
                        {
                            obj.ClosedTotal = obj.summary[i].TOTAL;
                        }
                        if (obj.summary[i].STATUS_NAME.Equals("Review Required"))
                        {
                            obj.ReviewTotal = obj.summary[i].TOTAL;
                        }
                    }
                    obj.Total = obj.summary.Sum(item => item.TOTAL);
                    return obj;
                }
                else
                {
                    return obj;
                }

            }
            catch (Exception ex)
            {
                return new CorrectedClaimSummaryModel();
            }
        }

        private void AddClaimLogs(List<CorrectedClaimLog> claimLoglist)
        {
            try
            {
                foreach (var taskLog in claimLoglist)
                {
                    _CorrectedClaimLogRepository.Insert(taskLog);
                    _CorrectedClaimLogRepository.Save();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public string Export(CorrectedClaimSearch obj, UserProfile profile)
        {
            try
            {
                string fileName = "Corrected_Claim";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                obj.currentPage = 1;
                obj.recordPerpage = 1000000;
                var CalledFrom = "Corrected_Claim";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                CorrectedClaimData result = new CorrectedClaimData();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetCorrectedClaimData(obj, profile);
                for (int i = 0; i < result.CorrectedClaims.Count(); i++)
                {
                    result.CorrectedClaims[i].ROW = i + 1;
                    result.CorrectedClaims[i].CHARGE_ENTRY_DATE_str = result.CorrectedClaims[i].CHARGE_ENTRY_DATE?.ToString("MM/dd/yyyy");
                    result.CorrectedClaims[i].WORK_DATE_Str = result.CorrectedClaims[i].WORK_DATE?.ToString("MM/dd/yyyy");
                    result.CorrectedClaims[i].REQUESTED_DATE_Str = result.CorrectedClaims[i].REQUESTED_DATE?.ToString("MM/dd/yyyy");
                    result.CorrectedClaims[i].RESPONSE_DATE_Str = result.CorrectedClaims[i].RESPONSE_DATE?.ToString("MM/dd/yyyy");

                }
                exported = ExportToExcel.CreateExcelDocument<CorrectedClaimResponse>(result.CorrectedClaims, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
