using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.StatesModel;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
using FOX.DataModels.Models.Settings.FacilityLocation;
using System.Linq;
using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Models.CommonModel;
using System.Web;
using FOX.BusinessOperations.CommonServices;
using System.IO;

namespace FOX.BusinessOperations.SettingsService.ReferralRegionServices
{
    public class ReferralRegionService : IReferralRegionService
    {
        private readonly DbContextSecurity _securityContext = new DbContextSecurity();
        private readonly DbContextSettings _settings = new DbContextSettings();
        private readonly GenericRepository<ReferralRegion> _referralRegionRepository;
        private readonly GenericRepository<FacilityType> _facilityTypesRepository;
        public readonly DbContextPatient _dbContextPatient = new DbContextPatient();
        private readonly GenericRepository<REGION_ZIPCODE_DATA> _regionZipCodeRespository;
        private readonly GenericRepository<FOX_TBL_ZIP_STATE_COUNTY> _zipStateCountyRepository;
        private readonly GenericRepository<REFERRAL_REGION_COUNTY> _referralRegionCounty;
        private readonly GenericRepository<FOX_TBL_DASHBOARD_ACCESS> _dashBoardUserRepositry;

        public ReferralRegionService()
        {
            _referralRegionRepository = new GenericRepository<ReferralRegion>(_settings);
            _facilityTypesRepository = new GenericRepository<FacilityType>(_dbContextPatient);
            _facilityTypesRepository = new GenericRepository<FacilityType>(_dbContextPatient);
            _regionZipCodeRespository = new GenericRepository<REGION_ZIPCODE_DATA>(_securityContext);
            _zipStateCountyRepository = new GenericRepository<FOX_TBL_ZIP_STATE_COUNTY>(_settings);
            _referralRegionCounty = new GenericRepository<REFERRAL_REGION_COUNTY>(_securityContext);
            _dashBoardUserRepositry = new GenericRepository<FOX_TBL_DASHBOARD_ACCESS>(_settings);
        }

        public List<ReferralRegion> GetReferralRegionByName(string searchString, long practiceCode)
        {
            return _referralRegionRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && (x.REFERRAL_REGION_NAME.Contains(searchString) || x.REFERRAL_REGION_CODE.Contains(searchString) || ("[" + x.REFERRAL_REGION_CODE + "] " + x.REFERRAL_REGION_NAME).Contains(searchString)));
        }


        //public List<REGION_ZIPCODE_DATA> GetCountiesListByStateCode(string stateCode, long practiceCode)
        //{
        //    var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
        //    var _state_Code = new SqlParameter("STATE_CODE", SqlDbType.VarChar) { Value = stateCode };
        //    return SpRepository<REGION_ZIPCODE_DATA>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_COUNTIES_BY_STATE_CODE] @PRACTICE_CODE ,@STATE_CODE", _paramsPracticeCode, _state_Code);
        //}
        public List<FOX_TBL_ZIP_STATE_COUNTY> GetCountiesListByStateCode(string stateCode, long practiceCode)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _state_Code = new SqlParameter("STATE_CODE", SqlDbType.VarChar) { Value = stateCode };
            var result = SpRepository<FOX_TBL_ZIP_STATE_COUNTY>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_COUNTIES_BY_STATE_CODE] @PRACTICE_CODE ,@STATE_CODE", _paramsPracticeCode, _state_Code);
            if (result != null && result.Count() > 0)
            {
                foreach (FOX_TBL_ZIP_STATE_COUNTY distinctcounty in result)
                {
                    var total_maped_zip_counties = _zipStateCountyRepository.GetMany(c => !c.DELETED && c.PRACTICE_CODE == practiceCode && c.REFERRAL_REGION_ID != null && c.COUNTY.ToLower() == distinctcounty.COUNTY.ToLower() && c.STATE == distinctcounty.STATE);
                    distinctcounty.MAPED_ZIP_COUNT = total_maped_zip_counties.Count().ToString();
                    var total_zip_counties = _zipStateCountyRepository.GetMany(c => !c.DELETED && c.PRACTICE_CODE == practiceCode && c.COUNTY.ToLower() == distinctcounty.COUNTY.ToLower() && c.STATE == distinctcounty.STATE);
                    distinctcounty.TOTAL_COUNTIES_COUNT = total_zip_counties.Count().ToString();
                }
            }
            return result;
        }

        //public List<REGION_ZIPCODE_DATA> GetCountiesByReferralRegionId(long referralRegionId, long practiceCode)
        //{
        //    var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
        //    var _paramsReferralRegionId = new SqlParameter("REFERRAL_REGION_ID", SqlDbType.BigInt) { Value = referralRegionId };
        //    return SpRepository<REGION_ZIPCODE_DATA>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_COUNTIES_BY_REFERRAL_REGION_ID_6906] @PRACTICE_CODE ,@REFERRAL_REGION_ID", _paramsPracticeCode, _paramsReferralRegionId);
        //}

        public List<FOX_TBL_ZIP_STATE_COUNTY> GetCountiesByReferralRegionId(long referralRegionId, long practiceCode)
        {
            try
            {
                var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
                var _paramsReferralRegionId = new SqlParameter("REFERRAL_REGION_ID", SqlDbType.BigInt) { Value = referralRegionId };
                var result = SpRepository<FOX_TBL_ZIP_STATE_COUNTY>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_COUNTIES_BY_REFERRAL_REGION_ID] @PRACTICE_CODE ,@REFERRAL_REGION_ID", _paramsPracticeCode, _paramsReferralRegionId);
                if (result != null || result.Count() > 0)
                {
                    foreach (FOX_TBL_ZIP_STATE_COUNTY distinctcounty in result)
                    {
                        var total_maped_zip_counties = _zipStateCountyRepository.GetMany(c => !c.DELETED && c.PRACTICE_CODE == practiceCode && c.REFERRAL_REGION_ID != null && c.REFERRAL_REGION_ID == referralRegionId && c.COUNTY.ToLower() == distinctcounty.COUNTY.ToLower() && c.STATE == distinctcounty.STATE);
                        distinctcounty.MAPED_ZIP_COUNT = total_maped_zip_counties.Count().ToString();
                        var total_zip_counties = _zipStateCountyRepository.GetMany(c => !c.DELETED && c.PRACTICE_CODE == practiceCode && c.COUNTY.ToLower() == distinctcounty.COUNTY.ToLower() && c.STATE == distinctcounty.STATE);
                        distinctcounty.TOTAL_COUNTIES_COUNT = total_zip_counties.Count().ToString();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReferralRegion GetReferralRegionByZipCode(string zipCode, long practiceCode)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _paramsZipCode = new SqlParameter("ZIP_CODE", SqlDbType.VarChar) { Value = zipCode };
            var res = SpRepository<ReferralRegion>.GetSingleObjectWithStoreProcedure(@"Exec [FOX_PROC_GET_REFERRAL_REGION_BY_ZIP_CODE] @PRACTICE_CODE ,@ZIP_CODE", _paramsPracticeCode, _paramsZipCode);
            return res;
        }

        public List<ReferralRegion> GetReferralRegionByPatientHomeAddressZipCode(string patient_AccountStr, long practiceCode)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _paramsPatientAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = Convert.ToInt64(patient_AccountStr) };
            var res = SpRepository<ReferralRegion>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_HOME_ADDRESS_ZIP_CODE] @PRACTICE_CODE ,@PATIENT_ACCOUNT", _paramsPracticeCode, _paramsPatientAccount);
            return res;
        }

        public List<FacilityType> GetFacilityTypes(long practiceCode)
        {
            return _facilityTypesRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED).OrderBy(t => t.DISPLAY_NAME).ToList();
        }
        public List<FOX_TBL_ZIP_STATE_COUNTY> GetReferralRegionZipCodeData(RegionZipCodeDataReq req, UserProfile profile)
        {
            try
            {
                //var ZipData = _zipStateCountyRepository.GetMany(x => x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED && x.COUNTY == req.COUNTY && x.STATE.ToLower().Equals(req.State.ToLower()) ).ToList();
                var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var _paramscounty = new SqlParameter { ParameterName = "COUNTY", Value = req.COUNTY };
                var _paramsstate = new SqlParameter { ParameterName = "STATE", Value = req.State };
                var ZipData = SpRepository<FOX_TBL_ZIP_STATE_COUNTY>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_REFERRAL_REGION_ZIP_CODE_DATA] @PRACTICE_CODE ,@COUNTY, @STATE", _paramsPracticeCode, _paramscounty, _paramsstate);
                if (ZipData.Any())
                    return ZipData;
                else
                    return new List<FOX_TBL_ZIP_STATE_COUNTY>();

            }
            catch (Exception ex)
            {
                return new List<FOX_TBL_ZIP_STATE_COUNTY>();
            }

        }

        public ResponseModel RemoveReferralRegionCounty(FOX_TBL_ZIP_STATE_COUNTY county, UserProfile profile)
        {
            var _practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var _county = new SqlParameter("COUNTY", SqlDbType.VarChar) { Value = county.COUNTY };
            var _state = new SqlParameter("STATE", SqlDbType.VarChar) { Value = county.STATE };
            var _regionId = county.REFERRAL_REGION_ID.HasValue ? new SqlParameter("REGION_ID", SqlDbType.BigInt) { Value = county.REFERRAL_REGION_ID }
            : new SqlParameter("REGION_ID", SqlDbType.BigInt) { Value = DBNull.Value };

            var res = SpRepository<ReferralRegion>.GetSingleObjectWithStoreProcedure(@"Exec [FOX_PROC_REMOVE_REFERRAL_REGION_COUNTY] @PRACTICE_CODE,@COUNTY,@STATE,@REGION_ID", _practiceCode, _county, _state, _regionId);
            return new ResponseModel()
            {
                Message = "Success",
                ErrorMessage = ""
            };
        }

        private void DeleteStateCountyMapping(long referralRegion, UserProfile profile)
        {
            //var existingCounties = _referralRegionCounty.GetMany(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == referralRegion);
            var existingCounties = _zipStateCountyRepository.GetMany(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == referralRegion);
            //////Commented code after removing county functaionality to procedure
            if (existingCounties != null && existingCounties.Count > 0)
            {
                //foreach (var county in existingCounties)
                //{
                //    county.DELETED = true;
                //    county.MODIFIED_BY = profile.UserName;
                //    county.MODIFIED_DATE = Helper.GetCurrentDate();
                //    _referralRegionCounty.Update(county);
                //}
                //_referralRegionCounty.Save();

                //foreach (REFERRAL_REGION_COUNTY county in existingCounties)
                //{
                //    UpdateCountiesByReferralRegionId(county.REFERRAL_REGION_ID, profile.PracticeCode, county.ZIP_STATE_COUNTY_ID.Value);
                //}
                foreach (FOX_TBL_ZIP_STATE_COUNTY county in existingCounties)
                {
                    UpdateCountiesByReferralRegionId(county.REFERRAL_REGION_ID, profile.PracticeCode, county.ZIP_STATE_COUNTY_ID);
                }

            }
        }

        private void UpdateCountiesByReferralRegionId(long? referralRegionId, long practiceCode, long zip_state_county_id)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _paramsReferralRegionId = new SqlParameter("REFERRAL_REGION_ID", SqlDbType.BigInt) { Value = referralRegionId };
            var _paramsReferralid = new SqlParameter("ZIP_STATE_COUNTY_ID", SqlDbType.BigInt) { Value = zip_state_county_id };

            SpRepository<FOX_TBL_ZIP_STATE_COUNTY>.GetSingleObjectWithStoreProcedure(@"Exec [FOX_PROC_DELETE_COUNTIES_REFERRAL_REGION] @PRACTICE_CODE ,@REFERRAL_REGION_ID, @ZIP_STATE_COUNTY_ID", _paramsPracticeCode, _paramsReferralRegionId, _paramsReferralid);
        }

        public string SaveMappedCounty(SaveMappCountyReq req, UserProfile profile)
        {
            try
            {
                string res = string.Empty;
                string RegionState = string.Empty;
                string ZipStateCountyState = string.Empty;
                var region = _referralRegionRepository.GetByID(req.REFERRAL_REGION_ID);
                if (region != null)
                {
                    RegionState = region.STATE_CODE;
                    if (req.SublistMapped?.Count > 0)
                    {
                        var ZipData = _zipStateCountyRepository.Get(x => x.PRACTICE_CODE == profile.PracticeCode && x.ZIP_STATE_COUNTY_ID == req.SublistMapped[0].ZIP_STATE_COUNTY_ID && !x.DELETED && x.COUNTY?.ToLower() == req.COUNTY?.ToLower());
                        if (ZipData != null)
                        {
                            ZipStateCountyState = ZipData.STATE;
                        }
                        if (!string.IsNullOrEmpty(ZipStateCountyState))     // && !string.IsNullOrEmpty(RegionState)
                        {
                            if ((string.IsNullOrEmpty(RegionState) && !string.IsNullOrEmpty(ZipStateCountyState)) || !RegionState.ToLower().Equals(ZipStateCountyState.ToLower()))
                            {
                                DeleteStateCountyMapping(req.REFERRAL_REGION_ID.Value, profile);
                                region.STATE_CODE = ZipStateCountyState;
                                region.MODIFIED_BY = profile.UserName;
                                region.MODIFIED_DATE = Helper.GetCurrentDate();
                                _referralRegionRepository.Update(region);
                                _referralRegionRepository.Save();
                            }
                        }
                    }
                }



                //var existingCounties = GetCountiesByReferralRegionId(req.REFERRAL_REGION_ID.Value, profile.PracticeCode);
                //var existingCounties = _referralRegionCounty.GetMany(c => c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == req.REFERRAL_REGION_ID );
                //if (existingCounties != null && existingCounties.Count > 0)
                //{
                //    var obj = existingCounties.FirstOrDefault(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.COUNTY == req.COUNTY);
                //    if (obj == null)
                //    {
                //        var referralRegionCounty = new REFERRAL_REGION_COUNTY()
                //        {
                //            CREATED_BY = profile.UserName,
                //            CREATED_DATE = Helper.GetCurrentDate(),
                //            DELETED = false,
                //            MODIFIED_BY = profile.UserName,
                //            MODIFIED_DATE = Helper.GetCurrentDate(),
                //            PRACTICE_CODE = profile.PracticeCode,
                //            REFERRAL_REGION_COUNTY_ID = Helper.getMaximumId("FOX_REFERRAL_REGION_COUNTY_ID"),
                //            REFERRAL_REGION_ID = req.REFERRAL_REGION_ID.Value,
                //            ZIP_STATE_COUNTY_ID = req.SublistMapped[0].ZIP_STATE_COUNTY_ID

                //        };
                //        _referralRegionCounty.Insert(referralRegionCounty);
                //        _referralRegionCounty.Save();
                //    }                                       

                //}
                //else
                //{
                //    var referralRegionCounty = new REFERRAL_REGION_COUNTY()
                //    {
                //        CREATED_BY = profile.UserName,
                //        CREATED_DATE = Helper.GetCurrentDate(),
                //        DELETED = false,
                //        MODIFIED_BY = profile.UserName,
                //        MODIFIED_DATE = Helper.GetCurrentDate(),
                //        PRACTICE_CODE = profile.PracticeCode,
                //        REFERRAL_REGION_COUNTY_ID = Helper.getMaximumId("FOX_REFERRAL_REGION_COUNTY_ID"),
                //        REFERRAL_REGION_ID = req.REFERRAL_REGION_ID.Value,
                //        ZIP_STATE_COUNTY_ID = req.SublistMapped[0].ZIP_STATE_COUNTY_ID

                //    };
                //    _referralRegionCounty.Insert(referralRegionCounty);
                //    _referralRegionCounty.Save();
                //}
                if (req.SublistMapped.Count > 0)
                {
                    foreach (var i in req.SublistMapped)
                    {
                      
                        var ZipData = _zipStateCountyRepository.Get(x => x.PRACTICE_CODE == profile.PracticeCode && x.ZIP_STATE_COUNTY_ID == i.ZIP_STATE_COUNTY_ID && !x.DELETED && x.COUNTY?.ToLower() == req.COUNTY?.ToLower());
                        if (ZipData != null)
                        {
                            ZipStateCountyState = ZipData.STATE;
                            //ZipData.IS_MAP = i.IS_MAP;
                            if (i.IS_MAP == false && i.REFERRAL_REGION_ID == req.REFERRAL_REGION_ID)//&& ZipData.REFERRAL_REGION_ID != null)&& ZipData.REFERRAL_REGION_ID != null
                            {
                                ZipData.REFERRAL_REGION_ID = null;
                            }
                            else if (i.IS_MAP == true && i.REFERRAL_REGION_ID == req.REFERRAL_REGION_ID)
                            {
                                ZipData.REFERRAL_REGION_ID = req.REFERRAL_REGION_ID;
                            }
                            else if (i.IS_MAP == true && i.REFERRAL_REGION_ID != req.REFERRAL_REGION_ID)
                            {
                                ZipData.REFERRAL_REGION_ID = req.REFERRAL_REGION_ID;
                            }
                            else if (i.IS_MAP == true && i.REFERRAL_REGION_ID == null)
                            {
                                ZipData.REFERRAL_REGION_ID = req.REFERRAL_REGION_ID;
                            }
                            ZipData.IS_MAP = true;
                            ZipData.MODIFIED_BY = profile.UserName;
                            ZipData.MODIFIED_DATE = Helper.GetCurrentDate();
                            _zipStateCountyRepository.Update(ZipData);
                            _zipStateCountyRepository.Save();
                        }
                    }
                    res = "Mapped";
                    return res;
                }
                return res;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public string MapAllZipCounties(RegionZipCodeDataReq req, UserProfile profile)
        {
            try
            {
                string res = string.Empty;
                var zipcodecounties = GetReferralRegionZipCodeData(req, profile);
                if (zipcodecounties != null && zipcodecounties.Count() > 0)
                {
                    var ZipData = _zipStateCountyRepository.Get(x => x.PRACTICE_CODE == profile.PracticeCode && x.ZIP_STATE_COUNTY_ID == zipcodecounties[0].ZIP_STATE_COUNTY_ID && !x.DELETED && x.COUNTY?.ToLower() == req.COUNTY ?.ToLower());
                    if (ZipData != null)
                    {
                        if (!string.IsNullOrEmpty(ZipData.STATE) && !string.IsNullOrEmpty(req?.State))
                        {
                            if (req.State.ToLower().Equals(ZipData.STATE.ToLower()))
                            {
                                zipcodecounties = zipcodecounties.FindAll(z => z.REFERRAL_REGION_ID != req.REFERRAL_REGION_ID && !z.DELETED);
                            }
                        }
                    }
                    if (zipcodecounties != null && zipcodecounties.Count() > 0)
                    {
                        foreach (var c in zipcodecounties)
                        {
                            c.IS_MAP = true;
                        }
                        SaveMappCountyReq SaveMappCountyReqObj = new SaveMappCountyReq();
                        SaveMappCountyReqObj.COUNTY = req.COUNTY;
                        SaveMappCountyReqObj.REFERRAL_REGION_ID = req.REFERRAL_REGION_ID;
                        SaveMappCountyReqObj.SublistMapped = zipcodecounties;
                        res = SaveMappedCounty(SaveMappCountyReqObj, profile);
                        return res;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public ZipCityStateCountyRegion GetCityStateCountyRegion(string zipCode, UserProfile profile)
        {
            if (zipCode.Contains("-"))
            {
                zipCode = zipCode.Replace("-", "");
            }
            var _zipCode = new SqlParameter { ParameterName = "@zipCode", Value = zipCode };
            var PracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var result = SpRepository<ZipCityStateCountyRegion>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_CITY_STATE_COUNTY_REGION_BY_ZIP_CODE_NEW @zipCode, @PRACTICE_CODE", _zipCode, PracticeCode);
            return result;
        }
        public ZipCityStateCountyRegion GetCityStateCountyRegionByCity(string city, UserProfile profile)
        {
            var _city = new SqlParameter { ParameterName = "@CITY", Value = city };
            var PracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var result = SpRepository<ZipCityStateCountyRegion>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_CITY_STATE_COUNTY_REGION_BY_CITY_NEW @CITY, @PRACTICE_CODE", _city, PracticeCode);
            return result;
        }
        public ZipCityStateCountyRegion GetCityStateCountyRegionByCounty(string county, UserProfile profile)
        {
            var _county = new SqlParameter { ParameterName = "@COUNTY", Value = county };
            var PracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var result = SpRepository<ZipCityStateCountyRegion>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_CITY_STATE_COUNTY_REGION_BY_COUNTY_NEW @COUNTY, @PRACTICE_CODE", _county, PracticeCode);
            return result;
        }
        public List<string> GetSmartCity(string city, UserProfile profile)
        {
            var _city = new SqlParameter { ParameterName = "@CITY", Value = city };
            var PracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var result = SpRepository<string>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SMART_CITY @CITY, @PRACTICE_CODE", _city, PracticeCode);
            return result;
        }
        public List<string> GetSmartCounty(string county, UserProfile profile)
        {
            var _county = new SqlParameter { ParameterName = "@COUNTY", Value = county };
            var PracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var result = SpRepository<string>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SMART_COUNTY @COUNTY, @PRACTICE_CODE", _county, PracticeCode);
            return result;
        }
        public List<AdvancedRegionSmartSearch> GetAdvancedRegionSmartSearch(string searchString, long practiceCode)
        {
            //var Regionstring = new SqlParameter { ParameterName = "@REGION_STRING", Value = searchString };
            var PracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            //var result = SpRepository<AdvancedRegionSmartSearch>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SMART_SEARCH_REGIONS @REGION_STRING, @PRACTICE_CODE", Regionstring, PracticeCode).ToList();
            var result = SpRepository<AdvancedRegionSmartSearch>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SMART_SEARCH_REGIONS @PRACTICE_CODE", PracticeCode).ToList();
            return result;
        }
        public List<AdvancedRegionsWithZipCodes> GetAdvancedRegionSearch(AdvanceRegionSearchRequest ObjAdvanceRegionSearchRequest, long practiceCode)
        {
            var Regionstring = new SqlParameter { ParameterName = "@REGIONS_STRING", Value = ObjAdvanceRegionSearchRequest.CheckedRegionsIDString };
            var PracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var CurrentPage = new SqlParameter { ParameterName = "@CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = ObjAdvanceRegionSearchRequest.CurrentPage };
            var RecordPerPage = new SqlParameter { ParameterName = "@RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = ObjAdvanceRegionSearchRequest.RecordPerPage };
            var result = SpRepository<AdvancedRegionsWithZipCodes>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADVANCED_EXPORT_REGIONS @REGIONS_STRING, @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE", Regionstring, PracticeCode, CurrentPage, RecordPerPage).ToList();
            return result;
        }
        public string ExportAdvancedRegion(showHideAdvancedRegionCol advancedregionreq, UserProfile profile)
        {
            try
            {
                string fileName = "Region_Full_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                var CalledFrom = "Advanced_region";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<AdvancedRegionsWithZipCodes> result = new List<AdvancedRegionsWithZipCodes>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                if(advancedregionreq != null)
                {
                    if(advancedregionreq.ObjAdvanceRegionSearchRequest != null)
                    {
                        advancedregionreq.ObjAdvanceRegionSearchRequest.RecordPerPage = 0;
                    }
                result = GetAdvancedRegionSearch(advancedregionreq.ObjAdvanceRegionSearchRequest, profile.PracticeCode);
                if(result != null)
                {
                    var counter = 0;
                    foreach(var c in result)
                    {
                        c.ROW_NUM = counter + 1;
                        counter++;
                    }
                }
                exported = ExportToExcel.CreateExcelDocument<AdvancedRegionsWithZipCodes>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                }
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DashBoardUserModal GetRegionDashBoardUsers(long referralRegionId, long pracitce_Code)
        {
            DashBoardUserModal modal = new DashBoardUserModal();
            SqlParameter regionId = new SqlParameter("REFERRAL_REGION_ID", SqlDbType.BigInt) { Value = referralRegionId };
            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = pracitce_Code };
            modal.RegionDashBoardUser = SpRepository<FOX_TBL_DASHBOARD_ACCESS>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_REGION_DASHBOARD_USERS @REFERRAL_REGION_ID, @PRACTICE_CODE", regionId, practice_Code);

            modal.DashBoardUsers = GetSmartUsersOfRegionForVisibleTo(pracitce_Code);

            return modal;

            //return lst == null ?  new List<FOX_TBL_DASHBOARD_ACCESS>() :  lst;
        }

        public List<User> GetSmartUsersOfRegionForVisibleTo(long practiceCode)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _searchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = string.Empty };
            return SpRepository<User>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_SMART_USERS_OF_REGION_VISIBLE_TO] @PRACTICE_CODE, @SEARCH_TEXT", _paramsPracticeCode, _searchText);
        }

    }
}