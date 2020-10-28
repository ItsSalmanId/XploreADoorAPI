using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.Settings.ReferralSource;
//using FOX.DataModels.Models.Settings.RoleAndRights;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace FOX.BusinessOperations.StoreProcedure.SettingsService.ReferralSourceService
{
    public class ReferralSourceService : IReferralSourceService
    {
        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly DbContextSettings _settings = new DbContextSettings();
        private readonly GenericRepository<ReferralSource> _ReferralSourceRepository;
        private readonly GenericRepository<ReferralSourceInactiveReason> _referralSourceInactiveReasonRepository;
        private readonly GenericRepository<ReferralSourceDeliveryMethod> _referralSourceDeliveryMethodRepository;
        private readonly GenericRepository<FOX_TBL_IDENTIFIER> _fox_tbl_identifier;
        private readonly GenericRepository<PracticeOrganization> _fox_tbl_practice_organization;
        private readonly GenericRepository<Referral_Physicians> _referral_physiciansRepository;

        public ReferralSourceService()
        {
            _ReferralSourceRepository = new GenericRepository<ReferralSource>(security);
            _referralSourceInactiveReasonRepository = new GenericRepository<ReferralSourceInactiveReason>(_settings);
            _referralSourceDeliveryMethodRepository = new GenericRepository<ReferralSourceDeliveryMethod>(_settings);
            _fox_tbl_identifier = new GenericRepository<FOX_TBL_IDENTIFIER>(_settings);
            _fox_tbl_practice_organization = new GenericRepository<PracticeOrganization>(_settings);
            _referral_physiciansRepository = new GenericRepository<Referral_Physicians>(_settings);
        }

        public ReferralSource AddUpdateReferralSource(ReferralSource referralSource, UserProfile profile)
        {
            if (!string.IsNullOrEmpty(referralSource.INACTIVE_DATE_STR))
            {
                referralSource.INACTIVE_DATE = Convert.ToDateTime(referralSource.INACTIVE_DATE_STR);
            }
            else
            {
                referralSource.INACTIVE_DATE = null;
            }
            if (referralSource.PRACTICE_ORGANIZATION_ID == null)
            {
                referralSource.ORGANIZATION = null;
            }
            var dbReferralSource = _ReferralSourceRepository.GetFirst(e => e.SOURCE_ID == referralSource.SOURCE_ID && !e.DELETED);

            //referralSource.PHONE = referralSource.PHONE.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            //referralSource.FAX = referralSource.FAX.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");w

            if (dbReferralSource == null)
            {
                //referralSource.IsPhy = false;
               long REFERRAL_CODE =  InsertUpdateReferralPhy(referralSource, profile);
                // //By Johar removed Idenetity
                referralSource.SOURCE_ID = Helper.getMaximumId("FOX_SOURCE_ID");
                referralSource.REFERRAL_CODE = REFERRAL_CODE;
                referralSource.PRACTICE_CODE = profile.PracticeCode;
                referralSource.CREATED_BY = profile.UserName;
                referralSource.CREATED_DATE = Helper.GetCurrentDate();
                referralSource.MODIFIED_BY = profile.UserName;
                referralSource.MODIFIED_DATE = Helper.GetCurrentDate();
                referralSource.DELETED = false;
                _ReferralSourceRepository.Insert(referralSource);
                _ReferralSourceRepository.Save();
                return referralSource;
            }
            else
            {
                //referralSource.IsPhy = true;
                referralSource.REFERRAL_CODE = dbReferralSource.REFERRAL_CODE;
                InsertUpdateReferralPhy(referralSource, profile);
                dbReferralSource.CODE = referralSource.CODE;
                dbReferralSource.FIRST_NAME = referralSource.FIRST_NAME;
                dbReferralSource.LAST_NAME = referralSource.LAST_NAME;
                dbReferralSource.TITLE = referralSource.TITLE;
                dbReferralSource.ADDRESS = referralSource.ADDRESS;
                dbReferralSource.ADDRESS_2 = referralSource.ADDRESS_2;
                dbReferralSource.ZIP = referralSource.ZIP;
                dbReferralSource.CITY = referralSource.CITY;
                dbReferralSource.STATE = referralSource.STATE;
                dbReferralSource.PHONE = referralSource.PHONE;
                dbReferralSource.FAX = referralSource.FAX;
                dbReferralSource.REFERRAL_REGION = referralSource.REFERRAL_REGION;
                dbReferralSource.NPI = referralSource.NPI;
                dbReferralSource.ORGANIZATION = referralSource.ORGANIZATION;
                dbReferralSource.PRACTICE_ORGANIZATION_ID = referralSource.PRACTICE_ORGANIZATION_ID;
                dbReferralSource.INACTIVE_REASON_ID = referralSource.INACTIVE_REASON_ID;
                dbReferralSource.INACTIVE_DATE = referralSource.INACTIVE_DATE;
                dbReferralSource.SOURCE_DELIVERY_METHOD_ID = referralSource.SOURCE_DELIVERY_METHOD_ID;
                dbReferralSource.ACO_ID = referralSource.ACO_ID;
                dbReferralSource.NOTES = referralSource.NOTES;
                dbReferralSource.MODIFIED_BY = profile.UserName;
                dbReferralSource.MODIFIED_DATE = Helper.GetCurrentDate();
                _ReferralSourceRepository.Update(dbReferralSource);
                _ReferralSourceRepository.Save();
                return dbReferralSource;
            }
        }

        public ReferralSource GetReferralSourceBySourceID(long sourceId)
        {
            try
            {
                var refSrc = _ReferralSourceRepository.GetFirst(x => x.SOURCE_ID == sourceId);
                if (refSrc == null)
                    refSrc = new ReferralSource();

                if (refSrc != null)
                {
                    if (refSrc.ACO_ID != null)
                    {
                        var acoData = _fox_tbl_identifier.GetFirst(e => e.IDENTIFIER_ID == refSrc.ACO_ID && !e.DELETED);
                        if (acoData != null)
                        {
                            System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                            refSrc.ACO_NAME = strConverter.ToTitleCase(acoData.NAME?.ToLower() ?? "");
                        }
                    }

                    if (refSrc.PRACTICE_ORGANIZATION_ID != null)
                    {
                        var poData = _fox_tbl_practice_organization.GetFirst(e => e.PRACTICE_ORGANIZATION_ID == refSrc.PRACTICE_ORGANIZATION_ID && !e.DELETED);
                        if (poData != null)
                        {
                            System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                            refSrc.PRACTICE_ORGANIZATION_NAME = strConverter.ToTitleCase(poData.NAME?.ToLower() ?? "");
                        }
                    }
                }
                return refSrc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ReferralSource GetCurrentUserReferralSource(UserProfile profile)
        //{
        //    try
        //    {
        //        var role = _roleRepository.GetFirst(e => e.ROLE_ID == profile.RoleId);//TO BE ASKED: External user signed up role?
        //        if (role != null && role.ROLE_NAME.ToLower().Contains("external user"))//TO BE ASKED: we cant apply check on role name since it can be changed?
        //        {
        //            var usr = _userRepository.GetFirst(e => e.USER_ID == profile.userID && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
        //            if (usr != null && usr.REFERRAL_SOURCE_ID != null)
        //            {
        //                return _ReferralSourceRepository.Get(x => x.SOURCE_ID == usr.REFERRAL_SOURCE_ID);
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public List<ReferralSource> GetReferralSourceList(ReferralSourceSearch referralSourceSearch, UserProfile profile)
        {
            var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = referralSourceSearch.searchString };
            var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE  ", SqlDbType = SqlDbType.Int, Value = referralSourceSearch.CurrentPage };
            var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = referralSourceSearch.RecordPerPage };
            var SortBy = Helper.getDBNullOrValue("SORT_BY", referralSourceSearch.SortBy);
            var SortOrder = Helper.getDBNullOrValue("SORT_ORDER", referralSourceSearch.SortOrder);
            var Code = new SqlParameter { ParameterName = "CODE", SqlDbType = SqlDbType.VarChar, Value = referralSourceSearch.CODE };
            var FirstName = new SqlParameter { ParameterName = "FIRST_NAME", SqlDbType = SqlDbType.VarChar, Value = referralSourceSearch.FIRST_NAME };
            var LastName = new SqlParameter { ParameterName = "LAST_NAME", SqlDbType = SqlDbType.VarChar, Value = referralSourceSearch.LAST_NAME };
            var Npi = new SqlParameter { ParameterName = "NPI", SqlDbType = SqlDbType.VarChar, Value = referralSourceSearch.NPI };
            var Address = new SqlParameter { ParameterName = "ADDRESS", SqlDbType = SqlDbType.VarChar, Value = referralSourceSearch.ADDRESS };
            var Region = new SqlParameter { ParameterName = "REGION", SqlDbType = SqlDbType.VarChar, Value = referralSourceSearch.REFERRAL_REGION };
            var referralSourceList = SpRepository<ReferralSource>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_REFERRAL_SOURCE_LIST @PRACTICE_CODE, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER,@CODE,@FIRST_NAME,@LAST_NAME,@NPI,@ADDRESS,@REGION",
                practiceCode, searchString, CurrentPage, RecordPerPage, SortBy, SortOrder, Code, FirstName, LastName, Npi, Address, Region);
            return referralSourceList;
        }
        public string ExportToExcelReferralSource(ReferralSourceSearch referralSourceSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Referral_Source_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                referralSourceSearch.CurrentPage = 1;
                referralSourceSearch.RecordPerPage = 0;
                var CalledFrom = "Referral_Source";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<ReferralSource> result = new List<ReferralSource>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetReferralSourceList(referralSourceSearch, profile);
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<ReferralSource>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public InactiveReasonAndDeliveryMethod GetInactiveReasonAndDeliveryMethod(long practiceCode)
        {
            InactiveReasonAndDeliveryMethod _inactiveReasonAndDeliveryMethod = new InactiveReasonAndDeliveryMethod();
            _inactiveReasonAndDeliveryMethod.ReferralSourceInactiveReason = _referralSourceInactiveReasonRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode);
            _inactiveReasonAndDeliveryMethod.ReferralSourceDeliveryMethod = _referralSourceDeliveryMethodRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode);
            return _inactiveReasonAndDeliveryMethod;
        }

        public List<ReferralSource> GetReferralSourceByName(string searchString, long practiceCode)
        {
            return _ReferralSourceRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && (x.FIRST_NAME.Contains(searchString) || x.LAST_NAME.Contains(searchString)));
        }

        public long InsertUpdateReferralPhy(ReferralSource OrsObj, UserProfile profile)
        {
            Referral_Physicians mtbcPhy = new Referral_Physicians();
            var dbReferralPhysn = _referral_physiciansRepository.GetFirst(t => t.REFERRAL_CODE == OrsObj.REFERRAL_CODE && !(t.DELETED ?? false));
            if (dbReferralPhysn != null)
            {
                dbReferralPhysn.REFERRAL_FNAME = OrsObj.FIRST_NAME;
                dbReferralPhysn.REFERRAL_LNAME = OrsObj.LAST_NAME;
                dbReferralPhysn.title = OrsObj.TITLE;
                dbReferralPhysn.REFERRAL_ADDRESS = OrsObj.ADDRESS;
                dbReferralPhysn.REFERRAL_CITY = OrsObj.CITY;
                dbReferralPhysn.REFERRAL_STATE = OrsObj.STATE;
                dbReferralPhysn.REFERRAL_ZIP = OrsObj.ZIP;
                dbReferralPhysn.REFERRAL_PHONE = OrsObj.PHONE;
                dbReferralPhysn.REFERRAL_FAX = OrsObj.FAX;
                dbReferralPhysn.NPI = OrsObj.NPI;
                if (OrsObj.INACTIVE_REASON_ID != null && OrsObj.INACTIVE_REASON_ID != 0)
                {
                    dbReferralPhysn.IN_ACTIVE = true;
                }
                else
                {
                    dbReferralPhysn.IN_ACTIVE = false;
                }

                dbReferralPhysn.Sync_Date = Helper.GetCurrentDate();
                dbReferralPhysn.MODIFIED_DATE = Helper.GetCurrentDate();
                dbReferralPhysn.MODIFIED_BY = profile.UserName;
                _referral_physiciansRepository.Update(dbReferralPhysn);
                _referral_physiciansRepository.Save();
                return dbReferralPhysn.REFERRAL_CODE;
            }
            else
            {
                mtbcPhy.REFERRAL_CODE = Helper.getMaximumId("REFERRAL_CODE");
                mtbcPhy.REFERRAL_FNAME = OrsObj.FIRST_NAME;
                mtbcPhy.REFERRAL_LNAME = OrsObj.LAST_NAME;
                mtbcPhy.title = OrsObj.TITLE;
                mtbcPhy.REFERRAL_ADDRESS = OrsObj.ADDRESS;
                mtbcPhy.REFERRAL_CITY = OrsObj.CITY;
                mtbcPhy.REFERRAL_STATE = OrsObj.STATE;
                mtbcPhy.REFERRAL_ZIP = OrsObj.ZIP;
                mtbcPhy.REFERRAL_PHONE = OrsObj.PHONE;
                mtbcPhy.REFERRAL_FAX = OrsObj.FAX;
                mtbcPhy.NPI = OrsObj.NPI;
                if (OrsObj.INACTIVE_REASON_ID != null && OrsObj.INACTIVE_REASON_ID != 0)
                {
                    mtbcPhy.IN_ACTIVE = true;
                }
                else
                {
                    mtbcPhy.IN_ACTIVE = false;

                }
                mtbcPhy.Sync_Date = Helper.GetCurrentDate();
                mtbcPhy.CREATED_DATE = Helper.GetCurrentDate();
                mtbcPhy.CREATED_BY = profile.UserName;
                _referral_physiciansRepository.Insert(mtbcPhy);
                _referral_physiciansRepository.Save();
                return mtbcPhy.REFERRAL_CODE;
            }


        }
    }
}