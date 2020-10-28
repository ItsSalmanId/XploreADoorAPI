using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.HelperClasses;
using FOX.DataModels.Models.AdjustmentApproval;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using HtmlAgilityPack;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace FOX.BusinessOperations.AdjustmentApprovalService
{
    public class AdjustmentApprovalServices : IAdjusmentApprovalServices
    {
        private readonly DBContextAdjusmentApproval _adjustmentApprovalContext = new DBContextAdjusmentApproval();
        private readonly DbContextPatient _patientContext = new DbContextPatient();
        private readonly DbContextSecurity _userContext = new DbContextSecurity();
        private readonly GenericRepository<AdjustmentAmount> _adjustmentAmountRepository;
        private readonly GenericRepository<AdjustmentClaimStatus> _adjustmentClaimStatusRepository;
        private readonly GenericRepository<PatientAdjustmentDetails> _patientAdjustmentDetailsRepository;
        private readonly GenericRepository<AdjustmentLog> _adjustmentLogsRepository;
        private readonly GenericRepository<Patient> _PatientRepository;
        private readonly GenericRepository<User> _userRepository;

        public AdjustmentApprovalServices()
        {
            _adjustmentAmountRepository = new GenericRepository<AdjustmentAmount>(_adjustmentApprovalContext);
            _adjustmentClaimStatusRepository = new GenericRepository<AdjustmentClaimStatus>(_adjustmentApprovalContext);
            _patientAdjustmentDetailsRepository = new GenericRepository<PatientAdjustmentDetails>(_adjustmentApprovalContext);
            _adjustmentLogsRepository = new GenericRepository<AdjustmentLog>(_adjustmentApprovalContext);
            _PatientRepository = new GenericRepository<Patient>(_patientContext);
            _userRepository = new GenericRepository<User>(_userContext);
        }

        public List<AdjustmentAmount> GetAdjustmentAmountsRange(UserProfile profile)
        {
            try
            {
                return SpRepository<AdjustmentAmount>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADJUSTMENT_AMOUNTS @PRACTICE_CODE", new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<AdjustmentClaimStatus> GetAdjustmentStatuses(UserProfile profile)
        {
            try
            {
                return SpRepository<AdjustmentClaimStatus>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADJUSTMENT_STATUSES @PRACTICE_CODE", new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode });
            }
            catch (Exception ex) { throw ex; }
        }

        public List<UsersForDropdown> GetUsersForDD(UserProfile profile)
        {
            try
            {
                return SpRepository<UsersForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USERS_FOR_AJDUSTMENTS @PRACTICE_CODE, @CURRENT_USER, @ROLE_ID"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter("@CURRENT_USER", SqlDbType.VarChar) { Value = profile.UserName }
                    , new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.BigInt, Value = profile.RoleId }
                    );

            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientAdjustmentDetails> GetAdjustments(AdjustmentsSearchReq searchReq, UserProfile profile)
        {
            try
            {
                bool hasDOSFromDate = false;
                bool hasDOSToDate = false;
                if (!string.IsNullOrWhiteSpace(searchReq.DATE_FROM_Str))
                {
                    hasDOSFromDate = true;
                    searchReq.DATE_FROM = Convert.ToDateTime(searchReq.DATE_FROM_Str);
                }

                if (!string.IsNullOrWhiteSpace(searchReq.DATE_TO_Str))
                {
                    hasDOSToDate = true;
                    searchReq.DATE_TO = Convert.ToDateTime(searchReq.DATE_TO_Str);
                }

                var result = SpRepository<PatientAdjustmentDetails>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADJUSMENTS 
                    @PRACTICE_CODE, @IS_FOR_REPORT, @IS_DOS_SEARCH, @DATE_FROM, @DATE_TO, @PATIENT_ACCOUNT, @ADJUSTMENT_AMOUNT_ID, @STATUS_ID, @DISCIPLINE_ID, @CURRENT_USER, @SEARCH_TEXT, @CURRENT_PAGE, @RECORD_PER_PAGE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "IS_FOR_REPORT", SqlDbType = SqlDbType.Bit, Value = searchReq.IsForReport }
                    , new SqlParameter { ParameterName = "IS_DOS_SEARCH", SqlDbType = SqlDbType.Bit, Value = searchReq.IS_ACTIVE_DOS_DATE_TAB }
                    , CommonService.Helper.getDBNullOrValue("DATE_FROM", hasDOSFromDate ? (searchReq.DATE_FROM.HasValue ? searchReq.DATE_FROM.Value.ToString("MM/dd/yyyy") : "") : "")
                    , CommonService.Helper.getDBNullOrValue("DATE_TO", hasDOSToDate ? (searchReq.DATE_TO.HasValue ? searchReq.DATE_TO.Value.ToString("MM/dd/yyyy") : "") : "")
                    , CommonService.Helper.getDBNullOrValue("PATIENT_ACCOUNT", searchReq.PatientAccount)
                    , CommonService.Helper.getDBNullOrValue("ADJUSTMENT_AMOUNT_ID", searchReq.AdjustmentAmountId.HasValue ? searchReq.AdjustmentAmountId.Value.ToString() : "")
                    , CommonService.Helper.getDBNullOrValue("STATUS_ID", searchReq.StatusId.HasValue ? searchReq.StatusId.Value.ToString() : "")
                    , CommonService.Helper.getDBNullOrValue("DISCIPLINE_ID", searchReq.DisciplineId.HasValue ? searchReq.DisciplineId.Value.ToString() : "")
                    , new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = profile.UserName }
                    , new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.SearchString) ? "" : searchReq.SearchString }
                    , new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = searchReq.CurrentPage }
                    , new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = searchReq.RecordPerPage }
                    );

                switch (profile.RoleId)
                {
                    case 105:
                    case 112:
                        //var aMSatuses = new List<string> { "Closed", "Open", "Review Required" };
                        //result = result.FindAll(e => !e.STATUS_NAME.Contains("Closed"));
                        break;
                    case 110:
                    case 111:
                        //var dRSatuses = new List<string> { "Open", "In-Progress", "Pending", "Closed", "Review Reuquired"};
                        //result = result.FindAll(e => dRSatuses.Contains(e.STATUS_NAME));
                        //result = result.FindAll(e => !e.STATUS_NAME.Equals(""));
                        break;
                    default:
                        break;
                }

                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        

        public ResponseModel GetAdjustmentsForExcel(AdjustmentsSearchReq searchReq, UserProfile profile)
        {
            try
            {
                bool hasDOSFromDate = false;
                bool hasDOSToDate = false;
                if (!string.IsNullOrWhiteSpace(searchReq.DATE_FROM_Str))
                {
                    hasDOSFromDate = true;
                    searchReq.DATE_FROM = Convert.ToDateTime(searchReq.DATE_FROM_Str);
                }

                if (!string.IsNullOrWhiteSpace(searchReq.DATE_TO_Str))
                {
                    hasDOSToDate = true;
                    searchReq.DATE_TO = Convert.ToDateTime(searchReq.DATE_TO_Str);
                }

                var result = SpRepository<PatientAdjustmentDetails>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADJUSMENTS 
                    @PRACTICE_CODE, @IS_FOR_REPORT, @IS_DOS_SEARCH, @DATE_FROM, @DATE_TO, @PATIENT_ACCOUNT, @ADJUSTMENT_AMOUNT_ID, @STATUS_ID, @DISCIPLINE_ID, @CURRENT_USER, @SEARCH_TEXT, @CURRENT_PAGE, @RECORD_PER_PAGE"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "IS_FOR_REPORT", SqlDbType = SqlDbType.Bit, Value = searchReq.IsForReport }
                    , new SqlParameter { ParameterName = "IS_DOS_SEARCH", SqlDbType = SqlDbType.Bit, Value = searchReq.IS_ACTIVE_DOS_DATE_TAB }
                    , CommonService.Helper.getDBNullOrValue("DATE_FROM", hasDOSFromDate ? (searchReq.DATE_FROM.HasValue ? searchReq.DATE_FROM.Value.ToString("MM/dd/yyyy") : "") : "")
                    , CommonService.Helper.getDBNullOrValue("DATE_TO", hasDOSToDate ? (searchReq.DATE_TO.HasValue ? searchReq.DATE_TO.Value.ToString("MM/dd/yyyy") : "") : "")
                    , CommonService.Helper.getDBNullOrValue("PATIENT_ACCOUNT", searchReq.PatientAccount)
                    , CommonService.Helper.getDBNullOrValue("ADJUSTMENT_AMOUNT_ID", searchReq.AdjustmentAmountId.HasValue ? searchReq.AdjustmentAmountId.Value.ToString() : "")
                    , CommonService.Helper.getDBNullOrValue("STATUS_ID", searchReq.StatusId.HasValue ? searchReq.StatusId.Value.ToString() : "")
                    , CommonService.Helper.getDBNullOrValue("DISCIPLINE_ID", searchReq.DisciplineId.HasValue ? searchReq.DisciplineId.Value.ToString() : "")
                    , new SqlParameter("CURRENT_USER", SqlDbType.VarChar) { Value = profile.UserName }
                    , new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.SearchString) ? "" : searchReq.SearchString }
                    , new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = searchReq.CurrentPage }
                    , new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = 0 }
                    );

                switch (profile.RoleId)
                {
                    case 105:
                    case 112:
                        //var aMSatuses = new List<string> { "Closed", "Open", "Review Required" };
                        //result = result.FindAll(e => !e.STATUS_NAME.Contains("Closed"));
                        break;
                    case 110:
                    case 111:
                        //var dRSatuses = new List<string> { "Open", "In-Progress", "Pending", "Closed", "Review Reuquired"};
                        //result = result.FindAll(e => dRSatuses.Contains(e.STATUS_NAME));
                        //result = result.FindAll(e => !e.STATUS_NAME.Equals(""));
                        break;
                    default:
                        break;
                }

                return ExportAdjustmentsToExcel(result, profile);
            }
            catch (Exception ex) { throw ex; }
        }

     
        public List<AdjustmentLog> GetAdjustmentLogs(AdjustmentLogSearchReq searchReq, UserProfile profile)
        {
            try
            {
                var result = SpRepository<AdjustmentLog>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADJUSTMENT_LOGS @PRACTICE_CODE, @ADJUSTMENT_DETAIL_ID, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE",
                    new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "ADJUSTMENT_DETAIL_ID", SqlDbType = SqlDbType.BigInt, Value = searchReq.ADJUSTMENT_DETAIL_ID }
                    , new SqlParameter("SEARCH_STRING", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.SearchString) ? "" : searchReq.SearchString }
                    , new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = searchReq.CurrentPage }
                    , new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = searchReq.RecordPerPage }
                    );
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public DDValues GetDDValues(UserProfile profile)
        {
            try
            {
                var dDValues = new DDValues();
                dDValues.AdjustmentAmountDDValues = GetAdjustmentAmountsRange(profile);
                dDValues.AdjustmentStatusDDValues = GetAdjustmentStatuses(profile);
                dDValues.UserDDValues = GetUsersForDD(profile);
                return dDValues;
            }
            catch (Exception ex) { throw ex; }
        }

        public StatusCounter GetStatusCounters(StatusCounterSearch statusCounterSearch, UserProfile profile)
        {
            try
            {
                var statusCounter = new StatusCounter();
                var result = SpRepository<StatusCounter>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_STATUS_COUNTERS @PRACTICE_CODE, @CURRENT_USER, @IS_FOR_REPORT"
                    , new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode }
                    , new SqlParameter { ParameterName = "CURRENT_USER", SqlDbType = SqlDbType.VarChar, Value = profile.UserName }
                    , new SqlParameter { ParameterName = "IS_FOR_REPORT", SqlDbType = SqlDbType.Bit, Value = statusCounterSearch.IsForReport }

                    );
                statusCounter = result[0];

                return statusCounter;

            }
            catch (Exception ex) { throw ex; }
        }

        public ResponseModel SaveAdjustment(PatientAdjustmentDetails adjustmentToSave, UserProfile profile)
        {
            bool dBDataIsNull = false;
            PatientAdjustmentDetails prevObj = null;
            ResponseModel response = new ResponseModel();
            try
            {
                var adjustment = new PatientAdjustmentDetails();

                if (!string.IsNullOrEmpty(adjustmentToSave.REQUESTED_DATE_STR))
                {
                    adjustmentToSave.REQUESTED_DATE = Convert.ToDateTime(adjustmentToSave.REQUESTED_DATE_STR);
                }

                if (!string.IsNullOrEmpty(adjustmentToSave.DOS_FROM_STR))
                {
                    adjustmentToSave.DOS_FROM = Convert.ToDateTime(adjustmentToSave.DOS_FROM_STR);
                }

                if (!string.IsNullOrEmpty(adjustmentToSave.DOS_TO_STR))
                {
                    adjustmentToSave.DOS_TO = Convert.ToDateTime(adjustmentToSave.DOS_TO_STR);
                }

                var dBData = _patientAdjustmentDetailsRepository.GetFirst(e => e.ADJUSTMENT_DETAIL_ID == adjustmentToSave.ADJUSTMENT_DETAIL_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);

                if (dBData == null)
                {
                    dBDataIsNull = true;
                    adjustment.ADJUSTMENT_DETAIL_ID = CommonService.Helper.getMaximumId("FOX_ADJUSTMENT_APPROVAL_DETAIL_ID");
                    adjustmentToSave.ADJUSTMENT_DETAIL_ID = adjustment.ADJUSTMENT_DETAIL_ID;
                    adjustment.PRACTICE_CODE = profile.PracticeCode;
                    adjustment.CREATED_BY = adjustment.MODIFIED_BY = profile.UserName;
                    adjustment.CREATED_DATE = adjustment.MODIFIED_DATE = CommonService.Helper.GetCurrentDate();
                    adjustment.DELETED = false;
                }
                else
                {
                    prevObj = (PatientAdjustmentDetails)dBData.Clone();
                    adjustment = dBData;
                    adjustment.MODIFIED_BY = profile.UserName;
                    adjustment.MODIFIED_DATE = CommonService.Helper.GetCurrentDate();
                }

                adjustment.PATIENT_ACCOUNT = adjustmentToSave.PATIENT_ACCOUNT;
                adjustment.REQUESTED_DATE = adjustmentToSave.REQUESTED_DATE;
                adjustment.REQUESTED_BY = adjustmentToSave.REQUESTED_BY;
                adjustment.DOS_FROM = adjustmentToSave.DOS_FROM;
                adjustment.DOS_TO = adjustmentToSave.DOS_TO;

                adjustment.DISCIPLINE_ID = adjustmentToSave.DISCIPLINE_ID;
                adjustment.ADJUSTMENT_AMOUNT = adjustmentToSave.ADJUSTMENT_AMOUNT;
                adjustment.FLAG_17B9W = adjustmentToSave.FLAG_17B9W;
                adjustment.FLAG_17BAN = adjustmentToSave.FLAG_17BAN;
                adjustment.FLAG_17BDW = adjustmentToSave.FLAG_17BDW;
                adjustment.FLAG_17BEA = adjustmentToSave.FLAG_17BEA;
                adjustment.FLAG_17BER = adjustmentToSave.FLAG_17BER;

                adjustment.FLAG_17CHP = adjustmentToSave.FLAG_17CHP;
                adjustment.FLAG_17CO = adjustmentToSave.FLAG_17CO;
                adjustment.FLAG_17FUA = adjustmentToSave.FLAG_17FUA;
                adjustment.FLAG_17FUO = adjustmentToSave.FLAG_17FUO;
                adjustment.FLAG_17HHE = adjustmentToSave.FLAG_17HHE;
                adjustment.FLAG_17INC = adjustmentToSave.FLAG_17INC;
                adjustment.FLAG_17INS = adjustmentToSave.FLAG_17INS;
                adjustment.FLAG_17LTC = adjustmentToSave.FLAG_17LTC;
                adjustment.FLAG_17MCR = adjustmentToSave.FLAG_17MCR;
                adjustment.FLAG_17MDW = adjustmentToSave.FLAG_17MDW;
                adjustment.FLAG_17MED = adjustmentToSave.FLAG_17MED;
                adjustment.FLAG_17NOA = adjustmentToSave.FLAG_17NOA;
                adjustment.FLAG_17PTF = adjustmentToSave.FLAG_17PTF;
                adjustment.FLAG_17SBW = adjustmentToSave.FLAG_17SBW;
                adjustment.FLAG_17SCW = adjustmentToSave.FLAG_17SCW;
                adjustment.FLAG_17WCW = adjustmentToSave.FLAG_17WCW;
                adjustment.FLAG_17PEC = adjustmentToSave.FLAG_17PEC;
                adjustment.FLAG_17PED = adjustmentToSave.FLAG_17PED;
                adjustment.FLAG_OTHER = adjustmentToSave.FLAG_OTHER;
                adjustment.OTHER_DESCRIPTION = adjustmentToSave.OTHER_DESCRIPTION;
                adjustment.REASON = adjustmentToSave.REASON;
                adjustment.ADJUSTMENT_STATUS_ID = adjustmentToSave.ADJUSTMENT_STATUS_ID;


                if (prevObj != null &&
                    !GetAjustmentStatusName(prevObj.ADJUSTMENT_STATUS_ID.Value).Equals(GetAjustmentStatusName(adjustment.ADJUSTMENT_STATUS_ID.Value)) &&
                    GetAjustmentStatusName(adjustment.ADJUSTMENT_STATUS_ID.Value).Equals("Closed")
                    )
                {
                    adjustment.CLOSED_BY = profile.UserName;
                    adjustment.CLOSED_DATE = DateTime.Now;
                }

                if (dBData == null)
                    _patientAdjustmentDetailsRepository.Insert(adjustment);
                else
                    _patientAdjustmentDetailsRepository.Update(adjustment);
                _patientAdjustmentDetailsRepository.Save();

                SaveAdjustmentLogs(prevObj, adjustmentToSave, profile);

                response.Success = true;
                response.ErrorMessage = "";
                response.Message = dBData == null ? "Adjustment added successfully." : "Adjustment updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = dBDataIsNull ? "Adjustment couldn't be added." : "Adjustment couldn't be updated.";
            }
            return response;
        }

        public void SaveAdjustmentLogs(PatientAdjustmentDetails dbData, PatientAdjustmentDetails adjustmentToSave, UserProfile profile)
        {
            try
            {
                var logsList = new List<AdjustmentLog>();
                //var patAcc = !string.IsNullOrWhiteSpace(adjustmentToSave.PATIENT_ACCOUNT_STR) ? long.Parse(adjustmentToSave.PATIENT_ACCOUNT_STR) : (long?)null;
                if (dbData == null)
                {
                    logsList.Add(new AdjustmentLog() { LOG_MESSAGE = "New adjustment is requested." });
                    dbData = new PatientAdjustmentDetails();
                }

                if (dbData.PATIENT_ACCOUNT != adjustmentToSave.PATIENT_ACCOUNT)
                {
                    var log = "";
                    if (dbData.PATIENT_ACCOUNT != null)
                        log = "Patient changed from \"" + GetPatientName(dbData.PATIENT_ACCOUNT) + "\" to \"" + GetPatientName(adjustmentToSave.PATIENT_ACCOUNT) + "\".";
                    else
                        log = "Patient \"" + GetPatientName(adjustmentToSave.PATIENT_ACCOUNT) + "\" is added to the adjustment.";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "PATIENT_ACCOUNT",
                        PREVIOUS_VALUE = dbData.PATIENT_ACCOUNT.HasValue ? dbData.PATIENT_ACCOUNT.Value.ToString() : null,
                        NEW_VALUE = adjustmentToSave.PATIENT_ACCOUNT.HasValue ? adjustmentToSave.PATIENT_ACCOUNT.Value.ToString() : null
                    });
                }

                if (dbData.DOS_FROM != adjustmentToSave.DOS_FROM)
                {
                    var log = "";
                    if (dbData.DOS_FROM != null)
                        log = "DOS From changed from \"" + dbData.DOS_FROM.Value.ToString("MM/dd/yyyy") + "\" to \"" + adjustmentToSave.DOS_FROM.Value.ToString("MM/dd/yyyy") + "\".";
                    else
                        log = "DOS From \"" + adjustmentToSave.DOS_FROM.Value.ToString("MM/dd/yyyy") + "\" is added to the adjustment.";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "DOS_FROM",
                        PREVIOUS_VALUE = dbData.DOS_FROM.HasValue ? dbData.DOS_FROM.Value.ToString("MM/dd/yyyy") : null,
                        NEW_VALUE = adjustmentToSave.DOS_FROM.HasValue ? adjustmentToSave.DOS_FROM.Value.ToString("MM/dd/yyyy") : null
                    });
                }

                if (dbData.DOS_TO != adjustmentToSave.DOS_TO)
                {
                    var log = "";
                    if (dbData.DOS_TO != null)
                        log = "DOS To changed from \"" + dbData.DOS_TO.Value.ToString("MM/dd/yyyy") + "\" to \"" + adjustmentToSave.DOS_TO.Value.ToString("MM/dd/yyyy") + "\".";
                    else
                        log = "DOS To \"" + adjustmentToSave.DOS_TO.Value.ToString("MM/dd/yyyy") + "\" is added to the adjustment.";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "DOS_TO",
                        PREVIOUS_VALUE = dbData.DOS_TO.HasValue ? dbData.DOS_TO.Value.ToString("MM/dd/yyyy") : null,
                        NEW_VALUE = adjustmentToSave.DOS_TO.HasValue ? adjustmentToSave.DOS_TO.Value.ToString("MM/dd/yyyy") : null
                    });
                }

                if (dbData.DISCIPLINE_ID != adjustmentToSave.DISCIPLINE_ID)
                {
                    var log = "";
                    if (dbData.DISCIPLINE_ID != null)
                        log = "Discipline changed from \"" + CommonService.Helper.GetDepartmentName(dbData.DISCIPLINE_ID) + "\" to \"" + CommonService.Helper.GetDepartmentName(dbData.DISCIPLINE_ID) + "\".";
                    else
                        log = "Discipline \"" + CommonService.Helper.GetDepartmentName(adjustmentToSave.DISCIPLINE_ID) + "\" is added to the adjustment.";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "DISCIPLINE_ID",
                        PREVIOUS_VALUE = dbData.DISCIPLINE_ID.HasValue ? dbData.DISCIPLINE_ID.Value.ToString() : null,
                        NEW_VALUE = adjustmentToSave.DISCIPLINE_ID.HasValue ? adjustmentToSave.DISCIPLINE_ID.Value.ToString() : null
                    });
                }

                if (dbData.ADJUSTMENT_AMOUNT != adjustmentToSave.ADJUSTMENT_AMOUNT)
                {
                    var log = "";
                    if (dbData.ADJUSTMENT_AMOUNT != null)
                        log = "Adjustment amount changed from \"" + dbData.ADJUSTMENT_AMOUNT.Value.ToString("C2") + "\" to \"" + adjustmentToSave.ADJUSTMENT_AMOUNT.Value.ToString("C2") + "\".";
                    else
                        log = "Adjustment amount \"" + adjustmentToSave.ADJUSTMENT_AMOUNT.Value.ToString("C2") + "\" is added to the adjustment.";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "ADJUSTMENT_AMOUNT",
                        PREVIOUS_VALUE = dbData.ADJUSTMENT_AMOUNT.HasValue ? dbData.ADJUSTMENT_AMOUNT.Value.ToString() : null,
                        NEW_VALUE = adjustmentToSave.ADJUSTMENT_AMOUNT.HasValue ? adjustmentToSave.ADJUSTMENT_AMOUNT.Value.ToString() : null
                    });
                }
                if (dbData.FLAG_17B9W != adjustmentToSave.FLAG_17B9W)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17B9W - Medicare Hospice W/O\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17B9W) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17B9W",
                        PREVIOUS_VALUE = dbData.FLAG_17B9W.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17B9W.ToString()
                    });

                if (dbData.FLAG_17BAN != adjustmentToSave.FLAG_17BAN)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17BAN - Bankruptcy Patient\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17BAN) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17BAN",
                        PREVIOUS_VALUE = dbData.FLAG_17BAN.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17BAN.ToString()
                    });

                if (dbData.FLAG_17BDW != adjustmentToSave.FLAG_17BDW)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17BDW - Bad Debt Write Off\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17BDW) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17BDW",
                        PREVIOUS_VALUE = dbData.FLAG_17BDW.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17BDW.ToString()
                    });

                if (dbData.FLAG_17BEA != adjustmentToSave.FLAG_17BEA)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17BEA - Billing Error ADJ\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17BEA) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17BEA",
                        PREVIOUS_VALUE = dbData.FLAG_17BEA.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17BEA.ToString()
                    });

                if (dbData.FLAG_17BER != adjustmentToSave.FLAG_17BER)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17BER - Billing Error Rekej\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17BER) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17BER",
                        PREVIOUS_VALUE = dbData.FLAG_17BER.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17BER.ToString()
                    });

                if (dbData.FLAG_17CHP != adjustmentToSave.FLAG_17CHP)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17CHP - Champ VA Offsets\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17CHP) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17CHP",
                        PREVIOUS_VALUE = dbData.FLAG_17CHP.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17CHP.ToString()
                    });

                if (dbData.FLAG_17CO != adjustmentToSave.FLAG_17CO)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17CO - Contractual Oblijation ADJ\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17CO) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17CO",
                        PREVIOUS_VALUE = dbData.FLAG_17CO.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17CO.ToString()
                    });

                if (dbData.FLAG_17FUA != adjustmentToSave.FLAG_17FUA)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17FUA - No Auth. Requested/Beyond\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17FUA) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17FUA",
                        PREVIOUS_VALUE = dbData.FLAG_17FUA.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17FUA.ToString()
                    });

                if (dbData.FLAG_17FUO != adjustmentToSave.FLAG_17FUO)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17FUO - Fol Write Off\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17FUO) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17FUO",
                        PREVIOUS_VALUE = dbData.FLAG_17FUO.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17FUO.ToString()
                    });

                if (dbData.FLAG_17HHE != adjustmentToSave.FLAG_17HHE)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17HHE - Home Health Episode Write Off\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17HHE) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17HHE",
                        PREVIOUS_VALUE = dbData.FLAG_17HHE.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17HHE.ToString()
                    });

                if (dbData.FLAG_17INC != adjustmentToSave.FLAG_17INC)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17INC - Increase Expected\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17INC) + " for the adjustment.",
                        FIELD_NAME = "FLAG_INC",
                        PREVIOUS_VALUE = dbData.FLAG_17INC.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17INC.ToString()
                    });

                if (dbData.FLAG_17INS != adjustmentToSave.FLAG_17INS)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17INS - Insurance Bankruptcy Adj\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17INS) + " for the adjustment.",
                        FIELD_NAME = "FLAG_INS",
                        PREVIOUS_VALUE = dbData.FLAG_17INS.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17INS.ToString()
                    });

                if (dbData.FLAG_17LTC != adjustmentToSave.FLAG_17LTC)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17LTC - Long term care write off\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17LTC) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17LTC",
                        PREVIOUS_VALUE = dbData.FLAG_17LTC.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17LTC.ToString()
                    });

                if (dbData.FLAG_17MCR != adjustmentToSave.FLAG_17MCR)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17MCR - Management Misc Credit #*\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17MCR) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17MCR",
                        PREVIOUS_VALUE = dbData.FLAG_17MCR.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17MCR.ToString()
                    });

                if (dbData.FLAG_17MDW != adjustmentToSave.FLAG_17MDW)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17MDW - Medicaid Write Off\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17MDW) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17MDW",
                        PREVIOUS_VALUE = dbData.FLAG_17MDW.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17MDW.ToString()
                    });

                if (dbData.FLAG_17MED != adjustmentToSave.FLAG_17MED)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17MED - Medics\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17MED) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17MED",
                        PREVIOUS_VALUE = dbData.FLAG_17MED.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17MED.ToString()
                    });

                if (dbData.FLAG_17NOA != adjustmentToSave.FLAG_17NOA)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17NOA - No Authorization\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17NOA) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17NOA",
                        PREVIOUS_VALUE = dbData.FLAG_17NOA.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17NOA.ToString()
                    });

                if (dbData.FLAG_17PTF != adjustmentToSave.FLAG_17PTF)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17PTF - Past Timely Filing\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17PTF) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17PTF",
                        PREVIOUS_VALUE = dbData.FLAG_17PTF.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17PTF.ToString()
                    });

                if (dbData.FLAG_17SBW != adjustmentToSave.FLAG_17SBW)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17SBW - SM Balance Write Off (Credit)\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17SBW) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17SBW",
                        PREVIOUS_VALUE = dbData.FLAG_17SBW.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17SBW.ToString()
                    });

                if (dbData.FLAG_17SCW != adjustmentToSave.FLAG_17SCW)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17SCW - SM Balance Write Off (Debit)\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17SCW) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17SCW",
                        PREVIOUS_VALUE = dbData.FLAG_17SCW.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17SCW.ToString()
                    });

                if (dbData.FLAG_17WCW != adjustmentToSave.FLAG_17WCW)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17WCW - Work Comp ADJ\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17WCW) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17WCW",
                        PREVIOUS_VALUE = dbData.FLAG_17WCW.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17WCW.ToString()
                    });

                if (dbData.FLAG_17PEC != adjustmentToSave.FLAG_17PEC)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17PEC - 2017 Cash Posting Error Corr Cr#*\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17PEC) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17PEC",
                        PREVIOUS_VALUE = dbData.FLAG_17PEC.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17PEC.ToString()
                    });

                if (dbData.FLAG_17PED != adjustmentToSave.FLAG_17PED)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"17PED - 2017 Cash Posting Error Corr Db#*\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_17PED) + " for the adjustment.",
                        FIELD_NAME = "FLAG_17PED",
                        PREVIOUS_VALUE = dbData.FLAG_17PED.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_17PED.ToString()
                    });

                if (dbData.FLAG_OTHER != adjustmentToSave.FLAG_OTHER)
                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = "Case \"OTHER\" is " + GetMarkUnmarkText(adjustmentToSave.FLAG_OTHER) + " for the adjustment.",
                        FIELD_NAME = "FLAG_OTHER",
                        PREVIOUS_VALUE = dbData.FLAG_OTHER.ToString(),
                        NEW_VALUE = adjustmentToSave.FLAG_OTHER.ToString()
                    });

                if (!(dbData.OTHER_DESCRIPTION ?? "").Equals(adjustmentToSave.OTHER_DESCRIPTION ?? ""))
                {
                    var log = "";
                    if (!string.IsNullOrWhiteSpace(dbData.OTHER_DESCRIPTION))
                        log = "Description for case \"OTHER\" is changed from \"" + dbData.OTHER_DESCRIPTION + "\" to \"" + adjustmentToSave.OTHER_DESCRIPTION + "\".";
                    else
                        log = "Description \"" + adjustmentToSave.OTHER_DESCRIPTION + "\" for case \"OTHER\" is added to the adjustment.";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "OTHER_DESCRIPTION",
                        PREVIOUS_VALUE = dbData.OTHER_DESCRIPTION,
                        NEW_VALUE = adjustmentToSave.OTHER_DESCRIPTION
                    });
                }

                if (!(dbData.REASON ?? "").Equals(adjustmentToSave.REASON ?? ""))
                {
                    var log = "";
                    if (!string.IsNullOrWhiteSpace(dbData.REASON))
                        log = "Reason changed from\"" + dbData.REASON + "\" to \"" + adjustmentToSave.REASON + "\".";
                    else
                        log = "Reason \"" + adjustmentToSave.REASON + "\" is added to the adjustment.";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "REASON",
                        PREVIOUS_VALUE = dbData.REASON,
                        NEW_VALUE = adjustmentToSave.REASON
                    });
                }

                if (dbData.ADJUSTMENT_STATUS_ID != adjustmentToSave.ADJUSTMENT_STATUS_ID)
                {
                    var log = "";
                    if (dbData.ADJUSTMENT_STATUS_ID != null)
                        log = "Adjustment status changed from \"" + GetAjustmentStatusName(dbData.ADJUSTMENT_STATUS_ID) + "\" to \"" + GetAjustmentStatusName(adjustmentToSave.ADJUSTMENT_STATUS_ID) + "\".";
                    else
                        log = "Adjustment is marked as \"" + GetAjustmentStatusName(adjustmentToSave.ADJUSTMENT_STATUS_ID) + "\".";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "ADJUSTMENT_STATUS_ID",
                        PREVIOUS_VALUE = dbData.ADJUSTMENT_STATUS_ID.HasValue ? dbData.ADJUSTMENT_STATUS_ID.Value.ToString() : null,
                        NEW_VALUE = adjustmentToSave.ADJUSTMENT_STATUS_ID.HasValue ? adjustmentToSave.ADJUSTMENT_STATUS_ID.Value.ToString() : null
                    });
                }

                LogAdjustmentDetails(logsList, adjustmentToSave.ADJUSTMENT_DETAIL_ID, profile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetMarkUnmarkText(bool flag)
        {
            try
            {
                if (flag)
                    return "marked";
                else
                    return "unmarked";
            }
            catch (Exception ex) { throw ex; }
        }

        public string GetAjustmentStatusName(int? adjustmentStatusId)
        {
            try
            {
                if (adjustmentStatusId != null)
                {
                    var statusObj = _adjustmentClaimStatusRepository.GetFirst(e => e.STATUS_ID == adjustmentStatusId);
                    if (statusObj != null)
                    {
                        return statusObj.STATUS_NAME;
                    }
                }
                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        public string GetPatientName(long? patientAccount)
        {
            try
            {
                if (patientAccount != null)
                {
                    var patient = _PatientRepository.GetFirst(e => e.Patient_Account == patientAccount);
                    if (patient != null)
                    {
                        return patient.Last_Name + "," + patient.First_Name;
                    }
                }
                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        public void LogAdjustmentDetails(List<AdjustmentLog> adjustmentLogList, long? adjustmentDetailsId, UserProfile profile)
        {
            try
            {
                foreach (var adjustmentLog in adjustmentLogList)
                {
                    adjustmentLog.ADJUSTMENT_LOG_ID = CommonService.Helper.getMaximumId("FOX_ADJUSTMENT_LOG_ID");
                    adjustmentLog.ADJUSTMENT_DETAIL_ID = adjustmentDetailsId;

                    adjustmentLog.PRACTICE_CODE = profile.PracticeCode;
                    adjustmentLog.CREATED_BY = adjustmentLog.MODIFIED_BY = profile.UserName;
                    adjustmentLog.CREATED_DATE = adjustmentLog.MODIFIED_DATE = CommonService.Helper.GetCurrentDate();
                    adjustmentLog.DELETED = false;

                    _adjustmentLogsRepository.Insert(adjustmentLog);
                    _adjustmentLogsRepository.Save();
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public ResponseModel DeleteAdjustment(AdjustmentToDelete adjustmentToDelete, UserProfile profile)
        {
            var response = new ResponseModel();
            try
            {
                var adjustment = _patientAdjustmentDetailsRepository.GetFirst(e => e.ADJUSTMENT_DETAIL_ID == adjustmentToDelete.ADJUSTMENT_DETAIL_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                if (adjustment != null)
                {
                    adjustment.DELETED = true;
                    _patientAdjustmentDetailsRepository.Update(adjustment);
                    _patientAdjustmentDetailsRepository.Save();
                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = "Adjustment deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "";
                    response.Message = "Adjustment could not be found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = "Adjustment couldn't be deleted.";
            }
            return response;
        }

        public ResponseModel AssignUser(UserAssignmentModel userAssignmentDetails, UserProfile profile)
        {
            var response = new ResponseModel();
            bool isReAssign = false;
            try
            {
                var previousUser = (object)null;
                var adjustment = _patientAdjustmentDetailsRepository.GetFirst(e => e.ADJUSTMENT_DETAIL_ID == userAssignmentDetails.ADJUSTMENT_DETAIL_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                if (adjustment != null)
                {
                    if (!string.IsNullOrEmpty(adjustment.ASSIGNED_TO))
                    {
                        isReAssign = true;
                        previousUser = adjustment.ASSIGNED_TO.Clone();
                    }

                    adjustment.ASSIGNED_TO = userAssignmentDetails.UserToAssign.USER_NAME;
                    _patientAdjustmentDetailsRepository.Update(adjustment);
                    _patientAdjustmentDetailsRepository.Save();

                    LogUserAssignment(isReAssign, previousUser, userAssignmentDetails, profile);

                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = isReAssign ? "User re-assigned successfully." : "User assigned successfully.";
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "";
                    response.Message = isReAssign ? "User couldn't be re-assigned." : "User couldn't be assigned.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = isReAssign ? "User couldn't be re-assigned." : "User couldn't be assigned.";
            }
            return response;
        }

        public void LogUserAssignment(bool isReassign, object previousUser, UserAssignmentModel model, UserProfile profile)
        {
            try
            {
                string previousUserName = !string.IsNullOrWhiteSpace((string)previousUser) ? (string)previousUser : "";
                var logsList = new List<AdjustmentLog>();
                if (!previousUserName.Equals(model.UserToAssign.USER_NAME))
                {
                    var log = "";
                    if (isReassign)
                        log = "Adjustment re-assigned from \"" + GetUserName(previousUserName) + "\" to \"" + GetUserName(model.UserToAssign.USER_NAME) + "\".";
                    else
                        log = "Adjustment assigned to \"" + GetUserName(model.UserToAssign.USER_NAME) + "\".";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "ASSIGNED_TO",
                        PREVIOUS_VALUE = !string.IsNullOrWhiteSpace(previousUserName) ? previousUserName : null,
                        NEW_VALUE = model.UserToAssign.USER_NAME
                    });
                }

                LogAdjustmentDetails(logsList, model.ADJUSTMENT_DETAIL_ID, profile);
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

        public ResponseModel ExportAdjustmentsToExcel(List<PatientAdjustmentDetails> obj, UserProfile profile)
        {
            var response = new ResponseModel();
            try
            {
                List<PatientAdjustmentDetailsExportModel> objToExport = new List<PatientAdjustmentDetailsExportModel>();
                if (obj != null && obj.Count > 0)
                {
                    PrepareExport(obj, out objToExport);
                    string fileName = "Adjustment_Report_" + DateTime.Now.Ticks + ".xlsx";
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
                    dt.TableName = "Adjustments";
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

        public void PrepareExport(List<PatientAdjustmentDetails> obj, out List<PatientAdjustmentDetailsExportModel> recordToExport)
        {
            recordToExport = new List<PatientAdjustmentDetailsExportModel>();
            long counter = 0;
            foreach (var item in obj)
            {
                counter++;
                var exportModel = new PatientAdjustmentDetailsExportModel();
                exportModel.SERIAL_NUMBER = counter;
                exportModel.STATUS_NAME = !string.IsNullOrWhiteSpace(item.STATUS_NAME) ? item.STATUS_NAME : "";
                exportModel.PATIENT_NAME = !string.IsNullOrWhiteSpace(item.PATIENT_NAME) ? item.PATIENT_NAME : "";
                exportModel.MRN = !string.IsNullOrWhiteSpace(item.MRN) ? item.MRN : "";
                exportModel.DOS_FROM = item.DOS_FROM.HasValue ? item.DOS_FROM.Value.ToString("MM/dd/yyyy") : "";
                exportModel.DOS_TO = item.DOS_TO.HasValue ? item.DOS_TO.Value.ToString("MM/dd/yyyy") : "";
                exportModel.DISCIPLINE_NAME = !string.IsNullOrWhiteSpace(item.DISCIPLINE_NAME) ? item.DISCIPLINE_NAME : "";
                exportModel.ADJUSTMENT_AMOUNT = item.ADJUSTMENT_AMOUNT.HasValue ? item.ADJUSTMENT_AMOUNT.Value.ToString("C2") : "";
                exportModel.REASON = !string.IsNullOrWhiteSpace(item.REASON) ? item.REASON : "";
                exportModel.REQUESTED_BY = !string.IsNullOrWhiteSpace(item.REQUESTED_BY_NAME) ? item.REQUESTED_BY_NAME : "";
                exportModel.REQUESTED_DATE = item.REQUESTED_DATE.ToString("MM/dd/yyyy");
                exportModel.APPROVED_BY = !string.IsNullOrWhiteSpace(item.APPROVED_BY_NAME) ? item.APPROVED_BY_NAME : "";
                exportModel.APPROVED_DATE = item.APPROVED_DATE.HasValue ? item.APPROVED_DATE.Value.ToString("MM/dd/yyyy") : "";
                exportModel.ASSIGNED_TO = !string.IsNullOrWhiteSpace(item.ASSIGNED_TO_NAME) ? item.ASSIGNED_TO_NAME : "";
                recordToExport.Add(exportModel);
            }
        }

        public ResponseModel ExportAdjustmentLogsToExcel(List<AdjustmentLog> obj, UserProfile profile)
        {
            var response = new ResponseModel();
            try
            {
                List<AdjustmentLogExportModel> objToExport = new List<AdjustmentLogExportModel>();
                if (obj != null && obj.Count > 0)
                {
                    PrepareLogExport(obj, out objToExport);
                    string fileName = "Adjustment_Logs_" + obj[0].ADJUSTMENT_DETAIL_ID + "_" + DateTime.Now.Ticks + ".xlsx";
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
                    dt.TableName = "AdjustmentLogs";
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

        public void PrepareLogExport(List<AdjustmentLog> obj, out List<AdjustmentLogExportModel> recordToExport)
        {
            recordToExport = new List<AdjustmentLogExportModel>();
            long counter = 0;
            foreach (var item in obj)
            {
                counter++;
                var exportModel = new AdjustmentLogExportModel();
                exportModel.SERIAL_NUMBER = counter;
                exportModel.CREATED_DATE = item.CREATED_DATE.ToString("MM/dd/yyyy");
                exportModel.LOG_MESSAGE = !string.IsNullOrWhiteSpace(item.LOG_MESSAGE) ? item.LOG_MESSAGE : "";
                exportModel.CREATED_BY_NAME = !string.IsNullOrWhiteSpace(item.CREATED_BY_NAME) ? item.CREATED_BY_NAME : "";
                recordToExport.Add(exportModel);
            }
        }



        public ResponseModel SignRequest(SignRequestDetails signReqDetails, UserProfile profile)
        {
            bool isSignedFirstTime = true;
            var response = new ResponseModel();
            try
            {
                var adjustment = _patientAdjustmentDetailsRepository.GetFirst(e => e.ADJUSTMENT_DETAIL_ID == signReqDetails.ADJUSTMENT_DETAIL_ID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                if (adjustment != null)
                {
                    if (!string.IsNullOrWhiteSpace(profile.SIGNATURE_PATH))
                    {
                        var previousUser = (object)null;
                        if (!string.IsNullOrWhiteSpace(adjustment.APPROVED_BY))
                        {
                            isSignedFirstTime = false;
                            previousUser = adjustment.APPROVED_BY.Clone();
                        }
                        //var user = _userRepository.GetFirst(e => e.USER_ID == profile.userID && e.USER_NAME == profile.UserName && !e.DELETED && e.IS_ACTIVE);
                        adjustment.APPROVED_BY = profile.UserName;
                        adjustment.APPROVED_DATE = CommonService.Helper.GetCurrentDate();
                        adjustment.APPROVED_SIGN_PATH = profile.SIGNATURE_PATH;
                        _patientAdjustmentDetailsRepository.Update(adjustment);
                        _patientAdjustmentDetailsRepository.Save();
                        LogRequestApproval(isSignedFirstTime, previousUser, signReqDetails.ADJUSTMENT_DETAIL_ID, profile);
                        response.Success = true;
                        response.ErrorMessage = "";
                        response.Message = isSignedFirstTime ? "Adjustment request approved successfully." : "Adjustment request re-approved successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.ErrorMessage = "";
                        response.Message = "Your signature are missing. Please add it first.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "";
                    response.Message = "Adjustment couldn't be found. Please try again.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
                response.Message = isSignedFirstTime ? "Adjustment couldn't be approved. Please try again." : "Adjustment couldn't be re-approved. Please try again.";
            }
            return response;
        }

        public void LogRequestApproval(bool isSignedFirstTime, object previousUser, long adjustment_Detail_ID, UserProfile profile)
        {
            try
            {
                string previousUserName = !string.IsNullOrWhiteSpace((string)previousUser) ? (string)previousUser : "";
                var logsList = new List<AdjustmentLog>();
                if (!previousUserName.Equals(profile.UserName))
                {
                    var log = "";
                    if (!isSignedFirstTime)
                        log = "Adjustment re-approved by \"" + GetUserName(profile.UserName) + "\".";
                    else
                        log = "Adjustment approved by \"" + GetUserName(profile.UserName) + "\".";

                    logsList.Add(new AdjustmentLog()
                    {
                        LOG_MESSAGE = log,
                        FIELD_NAME = "APPROVED_BY",
                        PREVIOUS_VALUE = !string.IsNullOrWhiteSpace(previousUserName) ? previousUserName : null,
                        NEW_VALUE = profile.UserName
                    });
                }

                LogAdjustmentDetails(logsList, adjustment_Detail_ID, profile);
            }
            catch (Exception ex) { throw ex; }
        }

        public ResponseModel DownloadAdjustment(DownloadAdjustmentModel dowloadDetails, UserProfile profile)
        {
            try
            {
                var responseHTMLToPDF = HTMLToPDF(dowloadDetails.ATTACHMENT_HTML, "Adjustment_" + dowloadDetails.ADJUSTMENT_DETAIL_ID.ToString(), profile);
                if (responseHTMLToPDF.Success)
                    return new ResponseModel() { Message = responseHTMLToPDF.FilePath + responseHTMLToPDF.FileName, ErrorMessage = "", Success = true };
                else
                {
                    return new ResponseModel() { Message = "Adjustment couldn't be downloaded. Please try again.", ErrorMessage = responseHTMLToPDF.ErrorMessage, Success = false };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel() { Message = "Adjustment couldn't be downloaded. Please try again.", ErrorMessage = ex.Message, Success = false };
            }
        }

        private ResponseHTMLToPDF HTMLToPDF(string htmlString, string fileName, UserProfile profile, string linkMessage = null)
        {
            try
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlString);
                var htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'print-footer')]");
                if (htmlNode != null)
                {
                    htmlNode.Remove();
                }


                if (!string.IsNullOrWhiteSpace(linkMessage))
                {
                    var htmlNode_link = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'link')]");
                    if (htmlNode_link != null)
                    {
                        htmlNode_link.InnerHtml = linkMessage;
                    }
                }

                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginBottom = 10;
                converter.Options.MarginTop = 10;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.DisplayFooter = false;
                converter.Options.DisplayHeader = false;
                //converter.Options.WebPageWidth = 768;

                converter.Options.CssMediaType = HtmlToPdfCssMediaType.Print;


                //converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                //converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                //PdfTextSection text = new PdfTextSection(10, 10, "Please sign and return to FOX at (800) 597-0848 or email admit@foxrehab.org",
                //    new System.Drawing.Font("Arial", 10));

                // footer settings
                //converter.Options.DisplayFooter = true;
                //converter.Footer.Height = 50;
                //converter.Footer.Add(text);

                PdfDocument doc = converter.ConvertHtmlString(htmlDoc.DocumentNode.OuterHtml);
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                string exportDir = HttpContext.Current.Server.MapPath("~" + virtualPath);

                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
                fileName += DateTime.Now.Ticks + ".pdf";
                string pdfFilePath = exportDir + fileName;


                // save pdf document
                doc.Save(pdfFilePath);

                // close pdf document
                doc.Close();
                return new ResponseHTMLToPDF() { FileName = fileName, FilePath = virtualPath, Success = true, ErrorMessage = "" };
            }
            catch (Exception exception)
            {
                return new ResponseHTMLToPDF() { FileName = "", FilePath = "", Success = false, ErrorMessage = exception.ToString() };
            }
        }
    }
}