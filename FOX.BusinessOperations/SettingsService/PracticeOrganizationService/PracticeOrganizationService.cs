using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Practice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.SettingsService.PracticeOrganizationService
{
    public class PracticeOrganizationService : IPracticeOrganizationService
    {
        private readonly DbContextSettings _settingContext = new DbContextSettings();
        private readonly GenericRepository<PracticeOrganization> _practiceOrganizationRepository;

        public PracticeOrganizationService()
        {
            _practiceOrganizationRepository = new GenericRepository<PracticeOrganization>(_settingContext);
        }

        public string GetMaxPracticeOrganizationCode(long practiceCode)
        {
            string _code = "00001";
            var praticOrganizations = _practiceOrganizationRepository.GetMany(x => x.PRACTICE_CODE == practiceCode).OrderByDescending(x => x.CODE).ToList();
            if (praticOrganizations.Count == 0)
                return _code;
            _code = praticOrganizations.FirstOrDefault().CODE;
            if (string.IsNullOrEmpty(_code))
                return "00001";
            long cd = Convert.ToInt64(Helper.RemoveSpecialCharacters(_code));//Helper.RemoveSpecialCharacters(_code) added by irfan ullah bg fixation
            cd = ++cd;
            _code = cd.ToString();
            _code = _code.PadLeft(5, '0');
            return _code;
        }

        public PracticeOrganization AddUpdatePracticeOrganization(PracticeOrganization practiceOrganization, UserProfile profile)
        {
            var dbPracticeOrganization = _practiceOrganizationRepository.GetByID(practiceOrganization.PRACTICE_ORGANIZATION_ID);

            if (dbPracticeOrganization == null)
            {
                practiceOrganization.PRACTICE_ORGANIZATION_ID = Helper.getMaximumId("FOX_PRACTICE_ORGANIZATION_ID");
                practiceOrganization.PRACTICE_CODE = profile.PracticeCode;
                practiceOrganization.CREATED_BY = profile.UserName;
                practiceOrganization.CREATED_DATE = Helper.GetCurrentDate();
                practiceOrganization.MODIFIED_BY = profile.UserName;
                practiceOrganization.MODIFIED_DATE = Helper.GetCurrentDate();
                practiceOrganization.DELETED = false;
                practiceOrganization.IS_ACTIVE = true;
                _practiceOrganizationRepository.Insert(practiceOrganization);
                _practiceOrganizationRepository.Save();
                return practiceOrganization;
            }
            else
            {
                dbPracticeOrganization.CODE = practiceOrganization.CODE;
                dbPracticeOrganization.NAME = practiceOrganization.NAME;
                dbPracticeOrganization.DESCRIPTION = practiceOrganization.DESCRIPTION;
                dbPracticeOrganization.ZIP = practiceOrganization.ZIP;
                dbPracticeOrganization.CITY = practiceOrganization.CITY;
                dbPracticeOrganization.STATE = practiceOrganization.STATE;
                dbPracticeOrganization.ADDRESS = practiceOrganization.ADDRESS;
                dbPracticeOrganization.IS_ACTIVE = practiceOrganization.IS_ACTIVE;
                dbPracticeOrganization.MODIFIED_BY = profile.UserName;
                dbPracticeOrganization.MODIFIED_DATE = Helper.GetCurrentDate();
                _practiceOrganizationRepository.Update(dbPracticeOrganization);
                _practiceOrganizationRepository.Save();
                return dbPracticeOrganization;
            }
        }

        public List<PracticeOrganization> GetPracticeOrganizationList(PracticeOrganizationRequest practiceOrganizationRequest, UserProfile profile)
        {
            var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var name = new SqlParameter { ParameterName = "NAME", Value = practiceOrganizationRequest.NAME };
            var searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = practiceOrganizationRequest.SEARCH_STRING };
            var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = practiceOrganizationRequest.CURRENT_PAGE };
            var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = practiceOrganizationRequest.RECORD_PER_PAGE };
            var SortBy = new SqlParameter { ParameterName = "SORT_BY", Value = practiceOrganizationRequest.SORT_BY };
            var SortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = practiceOrganizationRequest.SORT_ORDER };

            var result = SpRepository<PracticeOrganization>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_PRACTICE_ORGANIZATION_LIST
                                       @PRACTICE_CODE, @NAME, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                                       practiceCode, name, searchString, CurrentPage, RecordPerPage, SortBy, SortOrder);
            return result;
        }

        public List<PracticeOrganization> GetPracticeOrganizationByName(string searchText, long practiceCode)
        {
            return _practiceOrganizationRepository.GetMany(x => !x.DELETED && (x.IS_ACTIVE ?? true) && x.PRACTICE_CODE == practiceCode && (x.NAME.Contains(searchText) || x.CODE.Contains(searchText)));
        }

        public string Export(PracticeOrganizationRequest obj, UserProfile profile)
        {
            try
            {
                string fileName = "Practice_Organization";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                //obj.CURRENT_PAGE = 1;
                obj.RECORD_PER_PAGE = 0;
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                var pathtowriteFile = exportPath + "\\" + fileName;
                switch (obj.CalledFrom)
                {
                    #region RBS Blocked Claims
                    case "Practice_Organization":
                        {
                            var result = GetPracticeOrganizationList(obj, profile);
                            //if (result.Count > 0)
                            //{
                            //    var item = result[0];
                            //    var item2 = result[1];
                            //    result.RemoveAt(0);
                            //    result.RemoveAt(0);

                            //    result.Add(item);
                            //    result.Add(item2);
                            //}
                            //for (int i = 0; i < result.Count; i++)
                            //{
                            //    result[i].ROW = i + 1;
                            //}
                            exported = ExportToExcel.CreateExcelDocument<PracticeOrganization>(result, pathtowriteFile, obj.CalledFrom.Replace(' ', '_'));
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
    }
}