using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.StatesModel;
using System.Collections.Generic;

namespace FOX.BusinessOperations.SettingsService.ReferralRegionServices
{
    public interface IReferralRegionService
    {
        List<ReferralRegion> GetReferralRegionByName(string searchString, long practiceCode);
        //List<REGION_ZIPCODE_DATA> GetCountiesListByStateCode(string stateCode, long practiceCode);
        //List<REGION_ZIPCODE_DATA> GetCountiesByReferralRegionId(long referralRegionId, long practiceCode);
        ReferralRegion GetReferralRegionByZipCode(string zipCode, long practiceCode);
        List<ReferralRegion> GetReferralRegionByPatientHomeAddressZipCode(string patient_AccountStr, long practiceCode);
        List<FacilityType> GetFacilityTypes(long practiceCode);
        List<FOX_TBL_ZIP_STATE_COUNTY> GetCountiesListByStateCode(string stateCode, long practiceCode);
        List<FOX_TBL_ZIP_STATE_COUNTY> GetCountiesByReferralRegionId(long referralRegionId, long practiceCode);
        List<FOX_TBL_ZIP_STATE_COUNTY> GetReferralRegionZipCodeData(RegionZipCodeDataReq req, UserProfile profile);
        string SaveMappedCounty(SaveMappCountyReq req, UserProfile profile);
        //List<FOX_TBL_ZIP_STATE_COUNTY> GetCountiesListByStateCode(string stateCode, long practiceCode);
        //List<FOX_TBL_ZIP_STATE_COUNTY> GetCountiesByReferralRegionId(long referralRegionId, long practiceCode);
        //List<REGION_ZIPCODE_DATA> GetCountiesListByStateCode(string stateCode, long practiceCode)
        ResponseModel RemoveReferralRegionCounty(FOX_TBL_ZIP_STATE_COUNTY obj, UserProfile profile);
        string MapAllZipCounties(RegionZipCodeDataReq req, UserProfile profile);
        ZipCityStateCountyRegion GetCityStateCountyRegion(string zipCityState, UserProfile profile);
        ZipCityStateCountyRegion GetCityStateCountyRegionByCity(string zipCityState, UserProfile profile);
        ZipCityStateCountyRegion GetCityStateCountyRegionByCounty(string zipCityState, UserProfile profile);
        List<string> GetSmartCity(string city, UserProfile profile);
        List<string> GetSmartCounty(string county, UserProfile profile);
        List<AdvancedRegionSmartSearch> GetAdvancedRegionSmartSearch(string searchString, long practiceCode);
        List<AdvancedRegionsWithZipCodes> GetAdvancedRegionSearch(AdvanceRegionSearchRequest ObjAdvanceRegionSearchRequest, long practiceCode);
        string ExportAdvancedRegion(showHideAdvancedRegionCol advancedregionreq, UserProfile profile);
        DashBoardUserModal GetRegionDashBoardUsers(long referralRegionId, long pracitce_Code);
    }
}
