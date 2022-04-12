using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Authorization;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.TasksModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.SettingsService.FacilityLocationService
{
    public class FacilityLocationService : IFacilityLocationService
    {
        private readonly DbContextPatient _dbContextPatient = new DbContextPatient();
        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly GenericRepository<FacilityLocation> _FacilityLocationRepository;
        private readonly GenericRepository<FacilityType> _facilityTypeRepository;
        private readonly GenericRepository<FOX_TBL_GROUP_IDENTIFIER> _IdentifierRepository;
        private readonly GenericRepository<FOX_TBL_LOCATION_CORPORATION> _locatiorRepository;
        private readonly GenericRepository<DataModels.Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER> _identifierrRepository;
        private readonly GenericRepository<FOX_TBL_AUTH_STATUS> _authStatusRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TYPE> _taskTypeRepository;
        private readonly GenericRepository<FOX_TBL_ORDER_STATUS> _orderStatusRepository;
        private readonly GenericRepository<FOX_TBL_SOURCE_OF_REFERRAL> _sourceOfReferralRepository;
        private readonly GenericRepository<AlertType> _alertTypeRepository;
        private readonly GenericRepository<FoxDocumentType> _docTypeRepository;
        private readonly GenericRepository<ContactType> _conTypeRepository;
        private readonly GenericRepository<IdentifierType> _identifierTypeRepository;
        private readonly GenericRepository<PatientAddress> _PatientAddressRepository;
        private readonly GenericRepository<PatientPOSLocation> _PatientPOSLocationRepository;
        private readonly GenericRepository<PatientAddressAdditionalInfo> _PatientAddressAdditionalInfoRepository;
        public FacilityLocationService()
        {
            _FacilityLocationRepository = new GenericRepository<FacilityLocation>(security);
            _facilityTypeRepository = new GenericRepository<FacilityType>(_dbContextPatient);
            _IdentifierRepository = new GenericRepository<FOX_TBL_GROUP_IDENTIFIER>(_dbContextPatient);
            _locatiorRepository = new GenericRepository<FOX_TBL_LOCATION_CORPORATION>(_dbContextPatient);
            _identifierrRepository = new GenericRepository<DataModels.Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER>(_dbContextPatient);
            _authStatusRepository = new GenericRepository<FOX_TBL_AUTH_STATUS>(_dbContextPatient);
            _taskTypeRepository = new GenericRepository<FOX_TBL_TASK_TYPE>(_dbContextPatient);
            _orderStatusRepository = new GenericRepository<FOX_TBL_ORDER_STATUS>(_dbContextPatient);
            _sourceOfReferralRepository = new GenericRepository<FOX_TBL_SOURCE_OF_REFERRAL>(_dbContextPatient);
            _alertTypeRepository = new GenericRepository<AlertType>(_dbContextPatient);
            _docTypeRepository = new GenericRepository<FoxDocumentType>(_dbContextPatient);
            _conTypeRepository = new GenericRepository<ContactType>(_dbContextPatient);
            _identifierTypeRepository = new GenericRepository<IdentifierType>(_dbContextPatient);
            _PatientAddressRepository = new GenericRepository<PatientAddress>(_dbContextPatient);
            _PatientPOSLocationRepository = new GenericRepository<PatientPOSLocation>(_dbContextPatient);
            _PatientAddressAdditionalInfoRepository = new GenericRepository<PatientAddressAdditionalInfo>(_dbContextPatient);

        }

        public FacilityLocation AddUpdateFacilityLocation(FacilityLocation facilityLocation, UserProfile profile)
        {
            var dbFacilityLocation = _FacilityLocationRepository.GetByID(facilityLocation.LOC_ID);
            if (dbFacilityLocation == null)
            {
                facilityLocation.PRACTICE_CODE = profile.PracticeCode;
                facilityLocation.LOC_ID = Convert.ToInt64(profile.PracticeCode.ToString() + Helper.getMaximumId("LOC_ID").ToString());
                facilityLocation.CREATED_BY = profile.UserName;
                facilityLocation.CREATED_DATE = Helper.GetCurrentDate();
                facilityLocation.MODIFIED_BY = profile.UserName;
                facilityLocation.MODIFIED_DATE = Helper.GetCurrentDate();
                facilityLocation.DELETED = false;
                _FacilityLocationRepository.Insert(facilityLocation);
                _FacilityLocationRepository.Save();
            }
            else
            {
                if (dbFacilityLocation.FACILITY_TYPE_ID != facilityLocation.FACILITY_TYPE_ID)
                {
                    facilityLocation.PRACTICE_CODE = profile.PracticeCode;
                    facilityLocation.LOC_ID = Convert.ToInt64(profile.PracticeCode.ToString() + Helper.getMaximumId("LOC_ID").ToString());
                    facilityLocation.CREATED_BY = profile.UserName;
                    facilityLocation.CREATED_DATE = Helper.GetCurrentDate();
                    facilityLocation.MODIFIED_BY = profile.UserName;
                    facilityLocation.MODIFIED_DATE = Helper.GetCurrentDate();
                    facilityLocation.DELETED = false;
                    facilityLocation.IS_ACTIVE = false;
                    _FacilityLocationRepository.Insert(facilityLocation);
                    _FacilityLocationRepository.Save();
                }
                else
                {

                    dbFacilityLocation.NAME = facilityLocation.NAME;
                    dbFacilityLocation.Description = facilityLocation.Description;
                    dbFacilityLocation.Address = facilityLocation.Address;
                    dbFacilityLocation.Zip = facilityLocation.Zip;
                    dbFacilityLocation.City = facilityLocation.City;
                    dbFacilityLocation.State = facilityLocation.State;
                    dbFacilityLocation.Country = facilityLocation.Country;
                    dbFacilityLocation.REGION = facilityLocation.REGION;
                    dbFacilityLocation.Phone = facilityLocation.Phone;
                    dbFacilityLocation.Fax = facilityLocation.Fax;
                    dbFacilityLocation.POS_Code = facilityLocation.POS_Code;
                    dbFacilityLocation.Capacity = facilityLocation.Capacity;
                    dbFacilityLocation.Census = facilityLocation.Census;
                    dbFacilityLocation.FACILITY_TYPE_ID = facilityLocation.FACILITY_TYPE_ID;
                    dbFacilityLocation.PT = facilityLocation.PT;
                    dbFacilityLocation.OT = facilityLocation.OT;
                    dbFacilityLocation.ST = facilityLocation.ST;
                    dbFacilityLocation.EP = facilityLocation.EP;
                    dbFacilityLocation.PT_PROVIDER_ID = facilityLocation.PT_PROVIDER_ID;
                    dbFacilityLocation.OT_PROVIDER_ID = facilityLocation.OT_PROVIDER_ID;
                    dbFacilityLocation.ST_PROVIDER_ID = facilityLocation.ST_PROVIDER_ID;
                    dbFacilityLocation.EP_PROVIDER_ID = facilityLocation.EP_PROVIDER_ID;
                    dbFacilityLocation.LEAD_PROVIDER_ID = facilityLocation.LEAD_PROVIDER_ID;
                    dbFacilityLocation.Lead = facilityLocation.Lead;
                    dbFacilityLocation.Parent = facilityLocation.Parent;
                    dbFacilityLocation.Description = facilityLocation.Description;
                    dbFacilityLocation.MODIFIED_BY = profile.UserName;
                    dbFacilityLocation.MODIFIED_DATE = Helper.GetCurrentDate();
                    _FacilityLocationRepository.Update(dbFacilityLocation);
                    _FacilityLocationRepository.Save();
                }
            }
            var facilityType = new FacilityType();
            if (facilityLocation?.FACILITY_TYPE_ID != null && facilityLocation.FACILITY_TYPE_ID != 0)
            {
                facilityType = _facilityTypeRepository.GetFirst(t => t.FACILITY_TYPE_ID == facilityLocation.FACILITY_TYPE_ID);
                if (facilityType != null)
                {
                    facilityLocation.FACILITY_TYPE_NAME = facilityType.DISPLAY_NAME;
                }
            }
            return facilityLocation;
        }

        public List<FacilityLocation> GetFacilityLocationList(FacilityLocationSearch facilityLocationSearch, UserProfile profile)
        {

            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = facilityLocationSearch.currentPage };
            SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = facilityLocationSearch.recordPerpage };
            SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = facilityLocationSearch.searchString };
            SqlParameter code = new SqlParameter { ParameterName = "CODE", Value = facilityLocationSearch.Code };
            SqlParameter description = new SqlParameter { ParameterName = "DESCRIPTION", Value = facilityLocationSearch.Description };
            SqlParameter zip = new SqlParameter { ParameterName = "ZIP", Value = facilityLocationSearch.zip };
            SqlParameter city = new SqlParameter { ParameterName = "CITY", Value = facilityLocationSearch.city };
            SqlParameter state = new SqlParameter { ParameterName = "STATE", Value = facilityLocationSearch.state };
            SqlParameter address = new SqlParameter { ParameterName = "ADDRESS", Value = facilityLocationSearch.Complete_Address };
            SqlParameter region = new SqlParameter { ParameterName = "REFERRAL_REGION", Value = facilityLocationSearch.Region };
            SqlParameter country = new SqlParameter { ParameterName = "COUNTRY", Value = facilityLocationSearch.Country };
            if (facilityLocationSearch.FacilityType == null)
            {
                facilityLocationSearch.FacilityType = 0;
            }
            SqlParameter facilityType = new SqlParameter { ParameterName = "FACILITY_TYPE", SqlDbType = SqlDbType.BigInt, Value = facilityLocationSearch.FacilityType };
            SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = facilityLocationSearch.sortBy };
            SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = facilityLocationSearch.sortOrder };
            var facilityLocationList = SpRepository<FacilityLocation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_FACILITY_LOCATION_LIST @PRACTICE_CODE, @SEARCH_STRING, @CODE, @DESCRIPTION, @ZIP, @CITY, @STATE, @ADDRESS, @REFERRAL_REGION, @COUNTRY, @FACILITY_TYPE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                parmPracticeCode, searchString, code, description, zip, city, state, address, region, country, facilityType, CurrentPage, RecordPerPage, SortBy, SortOrder);
            return facilityLocationList;
        }

        public FacilityLocation GetFacilityLocationById(LocationPatientAccount location, string practiceCode)
        {
            long PATIENT_ACCOUNT = 0;
            var PracticeCode = Convert.ToInt64(practiceCode);
            if (location != null && !string.IsNullOrWhiteSpace(location.PATIENT_ACCOUNT) )
            {
                PATIENT_ACCOUNT = Convert.ToInt64(location.PATIENT_ACCOUNT);
            }
            var result = _FacilityLocationRepository.Get(x => x.LOC_ID == location.Location_id && x.PRACTICE_CODE == PracticeCode);
            if (result != null)
            {
                //if (result.NAME?.Contains("Private Home") ?? false)
                {
                    var patient_pos = _PatientPOSLocationRepository.GetFirst(t => t.Is_Default == true && t.Loc_ID == location.Location_id && t.Deleted == false && t.Patient_Account == PATIENT_ACCOUNT);
                    if (patient_pos == null)
                    {
                        patient_pos = _PatientPOSLocationRepository.GetMany(t => t.Loc_ID == location.Location_id && t.Deleted == false && t.Patient_Account == PATIENT_ACCOUNT).OrderByDescending(t => t.Created_Date)?.FirstOrDefault();
                    }
                    if (patient_pos != null)
                    {
                        var address_Private_Home = _PatientAddressRepository.GetFirst(t => t.PATIENT_ACCOUNT == PATIENT_ACCOUNT && patient_pos.Patient_POS_ID == t.PATIENT_POS_ID && !(t.DELETED ?? false));

                        //var address_Private_Home = _PatientAddressRepository.GetMany(t => t.PATIENT_ACCOUNT == PATIENT_ACCOUNT && patient_pos.Patient_POS_ID == t.PATIENT_POS_ID && !(t.DELETED ?? false))?.OrderByDescending(T => T.MODIFIED_DATE)?.FirstOrDefault();
                        if (address_Private_Home != null)
                        {
                            if (result.NAME?.Contains("Private Home") ?? false)
                            {
                                result.Address = address_Private_Home.ADDRESS;
                                result.City = address_Private_Home.CITY;
                                result.Country = address_Private_Home.POS_County;
                                result.Phone = address_Private_Home.POS_Phone;
                                result.Fax = address_Private_Home.POS_Fax;
                                result.REGION = address_Private_Home.POS_REGION;
                            }
                            else
                            {
                                if(!String.IsNullOrWhiteSpace(address_Private_Home.ADDRESS))
                                {
                                    result.Address = address_Private_Home.ADDRESS;
                                }
                                if (!String.IsNullOrWhiteSpace(address_Private_Home.CITY))
                                {
                                    result.City = address_Private_Home.CITY;
                                }
                                if (!String.IsNullOrWhiteSpace(address_Private_Home.ZIP))
                                {
                                    result.Zip = address_Private_Home.ZIP;
                                }
                                if (!String.IsNullOrWhiteSpace(address_Private_Home.STATE))
                                {
                                    result.State = address_Private_Home.STATE;
                                }
                                if (!String.IsNullOrWhiteSpace(address_Private_Home.POS_County))
                                {
                                    result.Country = address_Private_Home.POS_County;
                                }

                            }
                        }

                    }
                    else
                    {
                        if (result.NAME?.Contains("Private Home") ?? false)
                        {
                            var additional_address = _PatientAddressAdditionalInfoRepository.GetFirst(t => t.WORK_ID == location.WORK_ID && t.DELETED == false);
                            if (additional_address != null)
                            {
                                var address_Private_Home_additional = _PatientAddressRepository.GetFirst(t => t.PATIENT_ACCOUNT == PATIENT_ACCOUNT && additional_address.PATIENT_ADDRESS_HISTORY_ID == t.PATIENT_ADDRESS_HISTORY_ID && !(t.DELETED ?? false));

                                //var address_Private_Home = _PatientAddressRepository.GetMany(t => t.PATIENT_ACCOUNT == PATIENT_ACCOUNT && patient_pos.Patient_POS_ID == t.PATIENT_POS_ID && !(t.DELETED ?? false))?.OrderByDescending(T => T.MODIFIED_DATE)?.FirstOrDefault();
                                if (address_Private_Home_additional != null)
                                {
                                    result.Address = address_Private_Home_additional.ADDRESS;
                                    result.City = address_Private_Home_additional.CITY;
                                    result.Country = address_Private_Home_additional.POS_County;
                                    result.Phone = address_Private_Home_additional.POS_Phone;
                                    result.Fax = address_Private_Home_additional.POS_Fax;
                                    result.REGION = address_Private_Home_additional.POS_REGION;
                                }
                            }
                        }
                    }

                }
            }

            return result;
        }
        public string ExportToExcelFacilityCreation(FacilityLocationSearch facilityLocationSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Facility_Setup_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                facilityLocationSearch.currentPage = 1;
                facilityLocationSearch.recordPerpage = 0;
                var CalledFrom = "Facility_Setup";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FacilityLocation> result = new List<FacilityLocation>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetFacilityLocationList(facilityLocationSearch, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FacilityLocation>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ProvidersName> GetProviderNamesList(string searchText, long practiceCode)
        {
            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            SqlParameter _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = searchText };
            var providersNameList = SpRepository<ProvidersName>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PROVIDERS_NAME_LIST @PRACTICE_CODE, @SEARCH_TEXT",
                _practiceCode, _searchText);
            return providersNameList;
        }

        public string GetProviderCode(string state, long practiceCode)
        {
            List<int> _codeList = new List<int>();
            var _providerList = _FacilityLocationRepository.GetMany(x => x.CODE.Contains(state) && x.PRACTICE_CODE == practiceCode);
            if (_providerList.Count > 0)
            {
                foreach (var provider in _providerList)
                {
                    var code = new String(provider.CODE.Where(Char.IsDigit).ToArray());
                    if (!string.IsNullOrEmpty(code))
                    {
                        _codeList.Add(Convert.ToInt32(code));
                    }
                }
                return (_codeList.Max() + 1).ToString();
            }
            return "001";
        }

        public string Export(FacilityLocationSearch facilityLocationSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Facility_Creation";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                //obj.CURRENT_PAGE = 1;
                facilityLocationSearch.recordPerpage = 0;
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                var pathtowriteFile = exportPath + "\\" + fileName;
                switch (facilityLocationSearch.CalledFrom)
                {
                    #region RBS Blocked Claims
                    case "Facility_Creation":
                        {
                            var result = GetFacilityLocationList(facilityLocationSearch, profile);
                            exported = ExportToExcel.CreateExcelDocument<FacilityLocation>(result, pathtowriteFile, facilityLocationSearch.CalledFrom.Replace(' ', '_'));
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

        public FacilityType GetFacilityTypeById(long facilityTypeId, long practiceCode)
        {
            return _facilityTypeRepository.GetFirst(t => t.FACILITY_TYPE_ID == facilityTypeId && !t.DELETED && t.PRACTICE_CODE == practiceCode);
        }

        public List<ReferralRegion> GetSmartReferralRegions(string searchText, long practiceCode)
        {
            try
            {
                if (searchText == null)
                    searchText = "";
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
                var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = searchText };
                var result = SpRepository<ReferralRegion>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_REFERRAL_REGION] @PRACTICE_CODE, @SEARCHVALUE",
                    parmPracticeCode, smartvalue).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<ReferralRegion>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FOX_TBL_GROUP_IDENTIFIER> GetGroupIdentifierList(GroupIdentifierSearch groupIdentifierSearch,long practiceCode)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
               
                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = groupIdentifierSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = groupIdentifierSearch.Name };
                SqlParameter description = new SqlParameter { ParameterName = "DESCRIPTION", Value = groupIdentifierSearch.Description };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = groupIdentifierSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = groupIdentifierSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = groupIdentifierSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = groupIdentifierSearch.sortOrder };

                var facilityLocationList = SpRepository<FOX_TBL_GROUP_IDENTIFIER>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_GROUP_IDENTIFIER_LIST @PRACTICE_CODE, @SEARCH_STRING, @NAME, @DESCRIPTION,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                    parmPracticeCode, searchString, name, description,CurrentPage, RecordPerPage, SortBy, SortOrder);
                return facilityLocationList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelGetGroupIdentifier(GroupIdentifierSearch groupIdentifierSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Group_Identifier_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                groupIdentifierSearch.currentPage = 1;
                groupIdentifierSearch.recordPerpage = 0;
                var CalledFrom = "Group_Identifier";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FOX_TBL_GROUP_IDENTIFIER> result = new List<FOX_TBL_GROUP_IDENTIFIER>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetGroupIdentifierList(groupIdentifierSearch, profile.PracticeCode);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FOX_TBL_GROUP_IDENTIFIER>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FOX_TBL_LOCATION_CORPORATION> LocationCorporationList(LocationCorporationSearch locationSearch, long practiceCode)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };

                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = locationSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = locationSearch.Name };
                SqlParameter Code = new SqlParameter { ParameterName = "CODE", Value = locationSearch.Code };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = locationSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = locationSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = locationSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = locationSearch.sortOrder };

                var LocationList = SpRepository<FOX_TBL_LOCATION_CORPORATION>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_LOCATION_CORPORATION_LIST @PRACTICE_CODE, @SEARCH_STRING, @NAME, @CODE,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                    parmPracticeCode, searchString, name, Code, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return LocationList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelLocationCorporation(LocationCorporationSearch locationSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Location_Corporation_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                locationSearch.currentPage = 1;
                locationSearch.recordPerpage = 0;
                var CalledFrom = "Location_Corporation";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FOX_TBL_LOCATION_CORPORATION> result = new List<FOX_TBL_LOCATION_CORPORATION>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = LocationCorporationList(locationSearch, profile.PracticeCode);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FOX_TBL_LOCATION_CORPORATION>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DataModels.Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER> GetIdentifierList(IdentifierSearch identifierSearch, long practiceCode)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };

                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = identifierSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = identifierSearch.Name };
                SqlParameter code = new SqlParameter { ParameterName = "DESCRIPTION", Value = identifierSearch.Code };
                SqlParameter type = new SqlParameter { ParameterName = "TYPE", SqlDbType = SqlDbType.BigInt, Value = identifierSearch.identifier_type };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = identifierSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = identifierSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = identifierSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = identifierSearch.sortOrder };

                var identifierList = SpRepository<DataModels.Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_IDENTIFIER_LIST @PRACTICE_CODE, @SEARCH_STRING, @NAME, @DESCRIPTION,@TYPE,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                    parmPracticeCode, searchString, name, code, type, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return identifierList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelIdentifier(IdentifierSearch identifierSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Identifier_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                identifierSearch.currentPage = 1;
                identifierSearch.recordPerpage = 0;
                var CalledFrom = "Identifier_Type";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<DataModels.Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER> result = new List<DataModels.Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetIdentifierList(identifierSearch, profile.PracticeCode);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<DataModels.Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FOX_TBL_AUTH_STATUS> GetAuthStatusList(AuthStatusSearch authStatusSearch, long practiceCode)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };

                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = authStatusSearch.searchString };
                SqlParameter code = new SqlParameter { ParameterName = "CODE", Value = authStatusSearch.Code };
                SqlParameter description = new SqlParameter { ParameterName = "DESCRIPTION", Value = authStatusSearch.Description };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = authStatusSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = authStatusSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = authStatusSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = authStatusSearch.sortOrder };

                var authStatusList = SpRepository<FOX_TBL_AUTH_STATUS>.GetListWithStoreProcedure(@"exec FOX_PROC_GET__AUTH_STATUS_LIST @PRACTICE_CODE, @SEARCH_STRING, @CODE, @DESCRIPTION,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                    parmPracticeCode, searchString, code, description, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return authStatusList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelAuthStatus(AuthStatusSearch authStatusSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Auth_Status_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                authStatusSearch.currentPage = 1;
                authStatusSearch.recordPerpage = 0;
                var CalledFrom = "Auth_Status";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FOX_TBL_AUTH_STATUS> result = new List<FOX_TBL_AUTH_STATUS>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetAuthStatusList(authStatusSearch, profile.PracticeCode);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FOX_TBL_AUTH_STATUS>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
               throw ex;
            }
        }
        public List<FOX_TBL_TASK_TYPE> GetTaskTypeList(TaskTpyeSearch taskTpyeSearch, long practiceCode)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };

                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = taskTpyeSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = taskTpyeSearch.Name };
                SqlParameter code = new SqlParameter { ParameterName = "DESCRIPTION", Value = taskTpyeSearch.Code };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = taskTpyeSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = taskTpyeSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = taskTpyeSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = taskTpyeSearch.sortOrder };

                var authStatusList = SpRepository<FOX_TBL_TASK_TYPE>.GetListWithStoreProcedure(@"exec FOX_PROC_GET__TASK_TYPE_LIST @PRACTICE_CODE, @SEARCH_STRING, @NAME, @DESCRIPTION,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                    parmPracticeCode, searchString, name, code, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return authStatusList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelTaskType(TaskTpyeSearch taskTpyeSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Task_Type_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                taskTpyeSearch.currentPage = 1;
                taskTpyeSearch.recordPerpage = 0;
                var CalledFrom = "Task_Type";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FOX_TBL_TASK_TYPE> result = new List<FOX_TBL_TASK_TYPE>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetTaskTypeList(taskTpyeSearch, profile.PracticeCode);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FOX_TBL_TASK_TYPE>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FOX_TBL_ORDER_STATUS> GetOrderStatusList(OrderStatusSearch orderStatusSearch, long practiceCode)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };

                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = orderStatusSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = orderStatusSearch.Code };
                SqlParameter description = new SqlParameter { ParameterName = "DESCRIPTION", Value = orderStatusSearch.Description };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = orderStatusSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = orderStatusSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = orderStatusSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = orderStatusSearch.sortOrder };

                var orderStatusList = SpRepository<FOX_TBL_ORDER_STATUS>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ORDER_STATUS_LIST @PRACTICE_CODE, @SEARCH_STRING, @NAME, @DESCRIPTION,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                    parmPracticeCode, searchString, name, description, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return orderStatusList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelOrderStatus(OrderStatusSearch orderStatusSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Order_Status_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                orderStatusSearch.currentPage = 1;
                orderStatusSearch.recordPerpage = 0;
                var CalledFrom = "Order_Status";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FOX_TBL_ORDER_STATUS> result = new List<FOX_TBL_ORDER_STATUS>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetOrderStatusList(orderStatusSearch, profile.PracticeCode);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FOX_TBL_ORDER_STATUS>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FOX_TBL_SOURCE_OF_REFERRAL> GetSourceofReferralList(SourceOfreferralSearch sourceOfreferralSearch, UserProfile profile)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };

                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = sourceOfreferralSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = sourceOfreferralSearch.Code };
                SqlParameter description = new SqlParameter { ParameterName = "DESCRIPTION", Value = sourceOfreferralSearch.Description };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = sourceOfreferralSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = sourceOfreferralSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = sourceOfreferralSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = sourceOfreferralSearch.sortOrder };

                var sourceReferralList = SpRepository<FOX_TBL_SOURCE_OF_REFERRAL>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SOURCE_OF_REFERRAL_LIST @PRACTICE_CODE, @SEARCH_STRING, @NAME, @DESCRIPTION,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                    parmPracticeCode, searchString, name, description, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return sourceReferralList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelSourceOfReferral(SourceOfreferralSearch sourceOfreferralSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Source_of_Referral_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                sourceOfreferralSearch.currentPage = 1;
                sourceOfreferralSearch.recordPerpage = 0;
                var CalledFrom = "Source_of_Referral";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FOX_TBL_SOURCE_OF_REFERRAL> result = new List<FOX_TBL_SOURCE_OF_REFERRAL>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetSourceofReferralList(sourceOfreferralSearch, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FOX_TBL_SOURCE_OF_REFERRAL>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AlertType> GetAlertTypeList(AlertTypeSearch alertTypeSearch, long practiceCode)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };

                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = alertTypeSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = alertTypeSearch.Code };
                SqlParameter description = new SqlParameter { ParameterName = "DESCRIPTION", Value = alertTypeSearch.Description };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = alertTypeSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = alertTypeSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = alertTypeSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = alertTypeSearch.sortOrder };

                var alertTypelList = SpRepository<AlertType>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ALERT_TYPE_LIST @PRACTICE_CODE, @SEARCH_STRING, @NAME, @DESCRIPTION,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                    parmPracticeCode, searchString, name, description, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return alertTypelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelAlertType(AlertTypeSearch alertTypeSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Alert_Type_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                alertTypeSearch.currentPage = 1;
                alertTypeSearch.recordPerpage = 0;
                var CalledFrom = "Alert_Type";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<AlertType> result = new List<AlertType>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetAlertTypeList(alertTypeSearch,profile.PracticeCode);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<AlertType>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FoxDocumentType> GetDocumentTypeList(DocumentTypeSearch documentTypeSearch)
        {

            try
            {
               
                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = documentTypeSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = documentTypeSearch.Name };
                SqlParameter code = new SqlParameter { ParameterName = "CODE", Value = documentTypeSearch.Code };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = documentTypeSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = documentTypeSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = documentTypeSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = documentTypeSearch.sortOrder };

                var ContactTypelList = SpRepository<FoxDocumentType>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_DOCUMENT_TYPE_LIST @SEARCH_STRING, @NAME, @CODE,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                      searchString, name, code, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return ContactTypelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelDocumentType(DocumentTypeSearch documentTypeSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Document_Type_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                documentTypeSearch.currentPage = 1;
                documentTypeSearch.recordPerpage = 0;
                var CalledFrom = "Document_Type";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FoxDocumentType> result = new List<FoxDocumentType>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetDocumentTypeList(documentTypeSearch);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FoxDocumentType>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ContactType> GetPatientContactTypeList(PatientContactTypeSearch patientContactTypeSearch, long practiceCode)
        {

            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };

                SqlParameter searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = patientContactTypeSearch.searchString };
                SqlParameter name = new SqlParameter { ParameterName = "NAME", Value = patientContactTypeSearch.Name };
                SqlParameter code = new SqlParameter { ParameterName = "CODE", Value = patientContactTypeSearch.Code };
                SqlParameter CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = patientContactTypeSearch.currentPage };
                SqlParameter RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = patientContactTypeSearch.recordPerpage };
                SqlParameter SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = patientContactTypeSearch.sortBy };
                SqlParameter SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = patientContactTypeSearch.sortOrder };

                var ConTypelList = SpRepository<ContactType>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_CONTACT_TYPE_LIST @PRACTICE_CODE, @SEARCH_STRING, @NAME, @CODE,@CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                      parmPracticeCode,searchString, name, code, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return ConTypelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string ExportToExcelContactType(PatientContactTypeSearch patientContactTypeSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Contact_Type_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                patientContactTypeSearch.currentPage = 1;
                patientContactTypeSearch.recordPerpage = 0;
                var CalledFrom = "Contact_Type";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<ContactType> result = new List<ContactType>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetPatientContactTypeList(patientContactTypeSearch, profile.PracticeCode);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<ContactType>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<IdentifierType> GetIdentifierTypes(UserProfile profile)
        {
            if (profile.isTalkRehab)
            {
                return _identifierTypeRepository.GetMany(t => !t.DELETED).OrderBy(t => t.NAME).ToList();
            }
            else
            {
                return _identifierTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED).OrderBy(t => t.NAME).ToList();
            }
        }
    }
   
}
