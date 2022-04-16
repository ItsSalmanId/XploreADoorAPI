using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.PatientMaintenanceService.PatientInsuranceService
{
    public class PatientInsuranceService : IPatientInsuranceService
    {
        private readonly DbContextPatientInsurance _patientInsuranceContext = new DbContextPatientInsurance();
        private readonly DbContextPatient _patientContext = new DbContextPatient();
        private readonly GenericRepository<Insurances> _insuranceRepository;
        private readonly GenericRepository<InsurancesPayer> _insurancesPayerRepository;
        private readonly GenericRepository<FoxInsurancePayers> _foxInsurancePayorsRepository;
        private readonly CommonServices.CommonServices _commonService;

        public PatientInsuranceService()
        {
            _insuranceRepository = new GenericRepository<Insurances>(_patientInsuranceContext);
            _insurancesPayerRepository = new GenericRepository<InsurancesPayer>(_patientInsuranceContext);
            _foxInsurancePayorsRepository = new GenericRepository<FoxInsurancePayers>(_patientContext);
            _commonService = new CommonServices.CommonServices();
        }

        public int GetUnmappedInsurancesCount(UserProfile profile)
        {
            if (profile.isTalkRehab)
            {
                return _foxInsurancePayorsRepository.GetMany(row => row.DELETED != true && (row.INSURANCE_ID ?? 0) == 0).Count;
            }
            else
            {
                return _foxInsurancePayorsRepository.GetMany(row => row.DELETED != true && row.PRACTICE_CODE == profile.PracticeCode && (row.INSURANCE_ID ?? 0) == 0).Count;
            }
        }

        public List<FoxInsurancePayers> GetUnmappedInsurances(UnmappedInsuranceRequest unmappedInsuranceRequest, UserProfile profile)
        {

            try
            {
                if (!string.IsNullOrEmpty(unmappedInsuranceRequest.FinancialClassID) && unmappedInsuranceRequest.FinancialClassID.Equals("0"))
                {
                    unmappedInsuranceRequest.FinancialClassID = string.Empty;
                }
                if (!string.IsNullOrEmpty(unmappedInsuranceRequest.State) && unmappedInsuranceRequest.State.Equals("0"))
                {
                    unmappedInsuranceRequest.State = string.Empty;
                }
                if (!string.IsNullOrEmpty(unmappedInsuranceRequest.Carrier_State) && unmappedInsuranceRequest.Carrier_State.Equals("0"))
                {
                    unmappedInsuranceRequest.Carrier_State = string.Empty;
                }
                var _practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var _insludeMapped = new SqlParameter("INCLUDE_MAPPED", SqlDbType.VarChar) { Value = unmappedInsuranceRequest.includeMpped };
                var _searchText = new SqlParameter("SEARCH_STRING", SqlDbType.VarChar) { Value = unmappedInsuranceRequest.SearchText };
                var _currentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = unmappedInsuranceRequest.CurrentPage };
                var _recordPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = unmappedInsuranceRequest.RecordPerPage };
                var _financialClassId = new SqlParameter("FinancialClassId", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(unmappedInsuranceRequest.FinancialClassID ) ? "" : unmappedInsuranceRequest.FinancialClassID };
                var _state = new SqlParameter("State", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.State) ?  "" : unmappedInsuranceRequest.State};
                var _payerId = new SqlParameter("PayerID", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.PayerID) ? "" : unmappedInsuranceRequest.PayerID };
                var _Name = new SqlParameter("Name", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.Name) ? "" : unmappedInsuranceRequest.Name };
                var _Address = new SqlParameter("Address", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.Address) ? "" : unmappedInsuranceRequest.Address };
                var _ZIP = new SqlParameter("ZIP", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.ZIP) ? "" : unmappedInsuranceRequest.ZIP };
                var _Phone = new SqlParameter("Phone", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.Phone) ? "" : unmappedInsuranceRequest.Phone };
                var _Carrier = new SqlParameter("Carrier", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.Carrier) ? "" : unmappedInsuranceRequest.Carrier };
                var _Carrier_Locality = new SqlParameter("Carrier_Locality", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.Carrier_Locality) ? "" : unmappedInsuranceRequest.Carrier_Locality };
                var _Carrier_State = new SqlParameter("Carrier_State", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.Carrier_State) ? "" : unmappedInsuranceRequest.Carrier_State };
                var _Fee_Redirect = new SqlParameter("Fee_Redirect", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.Fee_Redirect) ? "" : unmappedInsuranceRequest.Fee_Redirect };
                var _SORT_BY = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.sortBy) ? "" : unmappedInsuranceRequest.sortBy };
                var _SORT_ORDER = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = String.IsNullOrEmpty(unmappedInsuranceRequest.sortOrder) ? "" : unmappedInsuranceRequest.sortOrder };
                var result = SpRepository<FoxInsurancePayers>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_UNMAPPED_INSURANCES_LIST
                            @PRACTICE_CODE, @INCLUDE_MAPPED, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE, @State, @FinancialClassId, @PayerID, @Name,@Address,@ZIP,@Phone,@Carrier,@Carrier_Locality,@Carrier_State,@Fee_Redirect,@SORT_BY,@SORT_ORDER", _practiceCode, 
                            _insludeMapped, _searchText, _currentPage, _recordPerPage, _state, _financialClassId, _payerId, _Name,_Address, _ZIP, _Phone, _Carrier, _Carrier_Locality, _Carrier_State, _Fee_Redirect,_SORT_BY, _SORT_ORDER);
                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public string ExportToExcelInsuranceSetup(UnmappedInsuranceRequest unmappedInsuranceRequest, UserProfile profile)
        {
            try
            {
                string fileName = "Insurance_Setup_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                unmappedInsuranceRequest.CurrentPage = 1;
                unmappedInsuranceRequest.RecordPerPage = 0;
                var CalledFrom = "Insurance_Setup";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FoxInsurancePayers> result = new List<FoxInsurancePayers>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetUnmappedInsurances(unmappedInsuranceRequest, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FoxInsurancePayers>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MTBCInsurancesSearchData GetMTBCInsurancesSearchData()
        {
            MTBCInsurancesSearchData _mtbcInsurancesSearchData = new MTBCInsurancesSearchData();
            _mtbcInsurancesSearchData.States = _commonService.GetStates().Select(x => x.State_Code).ToList();
            _mtbcInsurancesSearchData.Departments = _insuranceRepository.GetMany(row => !row.Deleted && row.Insurance_Department != null && row.Insurance_Department != "").ConvertAll(x => x.Insurance_Department.ToLower()).Distinct().OrderBy(x => x).ToList();
            _mtbcInsurancesSearchData.PhoneTypes = _insuranceRepository.GetMany(row => !row.Deleted && row.Insurance_Phone_Type1 != null && row.Insurance_Phone_Type1 != "").Select(x => x.Insurance_Phone_Type1).ToList();
            _mtbcInsurancesSearchData.PhoneTypes.AddRange(_insuranceRepository.GetMany(row => !row.Deleted && row.Insurance_Phone_Type2 != null && row.Insurance_Phone_Type2 != "").Select(x => x.Insurance_Phone_Type2).ToList());
            _mtbcInsurancesSearchData.PhoneTypes.AddRange(_insuranceRepository.GetMany(row => !row.Deleted && row.Insurance_Phone_Type3 != null && row.Insurance_Phone_Type3 != "").Select(x => x.Insurance_Phone_Type3).ToList());
            _mtbcInsurancesSearchData.PhoneTypes = _mtbcInsurancesSearchData.PhoneTypes.ConvertAll(x => x.ToLower()).OrderBy(x => x).Distinct().ToList();
            return _mtbcInsurancesSearchData;
        }

        public List<MTBCInsurances> GetMTBCInsurances(MTBCInsurancesRequest mtbcInsurancesRequest)
        {
            try
            {
                var _payerId = Helper.getDBNullOrValue("PAYER_ID", mtbcInsurancesRequest.payerId.ToString());
                var _payerDescription = new SqlParameter("PAYER_DESCRIPTION", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.payerDescription };
                var _insNameId = Helper.getDBNullOrValue("INS_NAME_ID", mtbcInsurancesRequest.insNameId.ToString());
                var _insuranceName = new SqlParameter("INSURANCE_NAME", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.insuranceName };
                var _groupName = new SqlParameter("GROUP_NAME", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.groupName };
                var _insuranceId = Helper.getDBNullOrValue("INSURANCE_ID", mtbcInsurancesRequest.insuranceId.ToString());
                var _insuranceAddress = new SqlParameter("INSURANCE_ADDRESS", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.insuranceAddress };
                var _zip = new SqlParameter("ZIP", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.zip };
                var _city = new SqlParameter("CITY", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.city };
                var _state = new SqlParameter("STATE", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.state };
                var _insuranceState = new SqlParameter("INSURANCE_STATE", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.insuranceState };
                var _payerState = new SqlParameter("PAYER_STATE", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.payerState };
                var _department = new SqlParameter("DEPARTMENT", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.department };
                var _EMC = new SqlParameter("EMC", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.EMC };
                var _phone = new SqlParameter("PHONE", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.phone };
                var _phoneType = new SqlParameter("PHONE_TYPE", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.phoneType };
                var _searchText = new SqlParameter("SEARCH_STRING", SqlDbType.VarChar) { Value = mtbcInsurancesRequest.SearchText };
                var _currentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = mtbcInsurancesRequest.CurrentPage };
                var _recordPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = mtbcInsurancesRequest.RecordPerPage };

                var result = SpRepository<MTBCInsurances>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_MTBC_INSURANCES_LIST
                            @PAYER_ID, @PAYER_DESCRIPTION, @INS_NAME_ID, @INSURANCE_NAME, @GROUP_NAME, @INSURANCE_ID, @INSURANCE_ADDRESS, @ZIP, @CITY, @STATE, @INSURANCE_STATE, @PAYER_STATE, @DEPARTMENT, @EMC, @PHONE, @PHONE_TYPE, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE",
                            _payerId, _payerDescription, _insNameId, _insuranceName, _groupName, _insuranceId, _insuranceAddress, _zip, _city, _state, _insuranceState, _payerState, _department, _EMC, _phone, _phoneType, _searchText, _currentPage, _recordPerPage);
                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public FoxInsurancePayers MapUnmappedInsurance(FoxInsurancePayers foxInsurancePayors, UserProfile profile)
        {
            var dbfoxInsurance = _foxInsurancePayorsRepository.GetByID(foxInsurancePayors.FOX_TBL_INSURANCE_ID);
            if (dbfoxInsurance != null)
            {
                dbfoxInsurance.INSURANCE_ID = foxInsurancePayors.INSURANCE_ID;
                dbfoxInsurance.MODIFIED_BY = profile.UserName;
                _foxInsurancePayorsRepository.Update(dbfoxInsurance);
                _foxInsurancePayorsRepository.Save();
            }
            return dbfoxInsurance;
        }

        public List<ClaimInsuranceViewModel> GetUnpaidClaimsForInsurance(ClaimInsuranceSearchReq searchReq, UserProfile profile)
        {
            try
            {
                DateTime? eDate = null;
                DateTime? tDate = null;

                if (!string.IsNullOrEmpty(searchReq.Effective_Date))
                {
                    eDate = Convert.ToDateTime(searchReq.Effective_Date);
                }

                if (!string.IsNullOrEmpty(searchReq.Termination_Date))
                {
                    tDate = Convert.ToDateTime(searchReq.Termination_Date);
                }
                var patient_insurance_ids_str = "";
                if (searchReq.Patient_Insurance_Ids != null && searchReq.Patient_Insurance_Ids.Count > 0)
                {
                    patient_insurance_ids_str = string.Join(",", searchReq.Patient_Insurance_Ids.Select(item => item.ToString()).ToArray());
                }

                var _practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var _patientAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = long.Parse(searchReq.Patient_Account) };
                var _patientInsuranceIds = new SqlParameter("PATIENT_INSURANCE_ID", SqlDbType.VarChar) { Value = patient_insurance_ids_str };
                var _effectiveDate = !eDate.HasValue ? new SqlParameter("EFFECTIVE_DATE", SqlDbType.VarChar) { Value = DBNull.Value }
                    : new SqlParameter("EFFECTIVE_DATE", SqlDbType.VarChar) { Value = eDate.Value.ToString("MM/dd/yyyy") };
                var _terminationDate = !tDate.HasValue ? new SqlParameter("TERMINATION_DATE", SqlDbType.VarChar) { Value = DBNull.Value }
                    : new SqlParameter("TERMINATION_DATE", SqlDbType.VarChar) { Value = tDate.Value.ToString("MM/dd/yyyy") };


                var result = SpRepository<ClaimInsuranceViewModel>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_UNPAID_INSURANCE_CLAIMS
                            @PRACTICE_CODE, @PATIENT_ACCOUNT, @PATIENT_INSURANCE_ID, @EFFECTIVE_DATE, @TERMINATION_DATE", _practiceCode, _patientAccount, _patientInsuranceIds, _effectiveDate, _terminationDate);
                return result;
            }
            catch (Exception ex)
            {
                return new List<ClaimInsuranceViewModel>();
            }
        }
    }
}
