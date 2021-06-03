using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.RequestForOrder.IndexInformation;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.ReferralSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.CasesModel;
using FOX.BusinessOperations.CommonService;

namespace FOX.BusinessOperations.RequestForOrder.IndexInformationServices
{
    public class IndexInformationService : IIndexInformationService
    {
        private readonly DbContextIndexinformation _indexinformationContext = new DbContextIndexinformation();
        private readonly DBContextQueue _queueContext = new DBContextQueue();
        private readonly DbContextSecurity _security = new DbContextSecurity();
        private readonly DbContextCases _cases = new DbContextCases();
        private readonly GenericRepository<ReferralSource> _referralSourceRepository;
        private readonly GenericRepository<FacilityLocation> _facilityLocationRepository;
        private readonly GenericRepository<OriginalQueue> _queueRepository;
        private readonly GenericRepository<FOX_TBL_CASE> _caseRepository;
             
        public IndexInformationService()
        {
            _queueRepository = new GenericRepository<OriginalQueue>(_queueContext);
            _referralSourceRepository = new GenericRepository<ReferralSource>(_security);
            _facilityLocationRepository = new GenericRepository<FacilityLocation>(_security);
            _caseRepository = new GenericRepository<FOX_TBL_CASE>(_cases);
        }

        public FacilityReferralSource getFacilityReferralSource(long patientAccount, long practiceCode)
        {
            FOX_TBL_CASE patCases = _caseRepository.GetMany(x => x.PRACTICE_CODE == practiceCode && !x.DELETED && x.PATIENT_ACCOUNT == patientAccount).OrderByDescending(x => x.CREATED_DATE).FirstOrDefault();
            FacilityReferralSource _facilityReferralSource = new FacilityReferralSource();
            if (patCases != null)
            {
                long refSourceId = patCases.ORDERING_REF_SOURCE_ID ?? 0;
                if (refSourceId != 0)
                {
                    _facilityReferralSource.REFERRAL_SOURCE = _referralSourceRepository.GetByID(refSourceId);
                    if (_facilityReferralSource.REFERRAL_SOURCE == null)
                        _facilityReferralSource.REFERRAL_SOURCE = new ReferralSource();
                }

                if (patCases.POS_ID.HasValue)
                {
                    _facilityReferralSource.FACILITY_LOCATION = _facilityLocationRepository.GetByID(patCases.POS_ID);

                    if (_facilityReferralSource.FACILITY_LOCATION == null)
                        _facilityReferralSource.FACILITY_LOCATION = new FacilityLocation();
                }                
            }
            
            //var res = _queueRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.PATIENT_ACCOUNT == patientAccount).OrderByDescending(x => x.CREATED_DATE).FirstOrDefault();
            //if(res != null)
            //{
            //    if (res.SENDER_ID != null)
            //    {
            //        _facilityReferralSource.REFERRAL_SOURCE = _referralSourceRepository.GetByID(res.SENDER_ID);

            //        if (_facilityReferralSource.REFERRAL_SOURCE == null)
            //            _facilityReferralSource.REFERRAL_SOURCE = new ReferralSource();
            //    }
            //    if (!string.IsNullOrEmpty(res.FACILITY_NAME))
            //    {
            //        _facilityReferralSource.FACILITY_LOCATION = _facilityLocationRepository.GetFirst(x => x.NAME == res.FACILITY_NAME);

            //        if (_facilityReferralSource.FACILITY_LOCATION == null)
            //            _facilityReferralSource.FACILITY_LOCATION = new FacilityLocation();
            //    }
            //}
            return _facilityReferralSource;
        }

        public List<FacilityLocation> GetFacilityLocations(string searchText, long practiceCode)
        {
            return _facilityLocationRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && (x.NAME.Contains(searchText) || x.CODE.Contains(searchText)));
        }

        public FacilityLocation GetFacilityByPatientPOS(string patientAccount, long practiceCode)
        {
            Helper.SlownessTrackingExceptionLog("IndexInformationService: In Function  GetFacilityByPracticePOS | Start " + Helper.GetCurrentDate().ToLocalTime());
            var profile = new UserProfile();
            profile.PracticeCode = practiceCode;
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _paramsPatientAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.VarChar) { Value = patientAccount };
            var result = SpRepository<FacilityLocation>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_FACILITY_BY_PATIENT_POS_LOC_ID] @PRACTICE_CODE,@PATIENT_ACCOUNT", _paramsPracticeCode, _paramsPatientAccount);
            if (result != null)
            {
                if (string.IsNullOrWhiteSpace(result.REGION) && !string.IsNullOrWhiteSpace(result.Zip))
                {
                    ZipRegionIDName region = GetRegionByZip(result.Zip, profile);
                    if (region != null)
                    {
                        result.REGION = region.REFERRAL_REGION_NAME;
                    }
                }
            }
            Helper.SlownessTrackingExceptionLog("IndexInformationService: In Function  GetFacilityByPracticePOS | End " + Helper.GetCurrentDate().ToLocalTime());
            return result;
        }
        public ZipRegionIDName GetRegionByZip(string zipCode, UserProfile profile)
        {
            if (zipCode.Contains("-"))
            {
                zipCode = zipCode.Replace("-", "");
            }
            var _zipCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", Value = zipCode };
            var _PracticeCode = new SqlParameter { ParameterName = "@zipCode", Value = profile.PracticeCode };
            var result = SpRepository<ZipRegionIDName>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_REF_REGION_BY_ZIPCODE] @zipCode,@PRACTICE_CODE", _zipCode, _PracticeCode);
            return result;
        }
    }
}