using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace FOX.BusinessOperations.PatientSurveyService.SurveyReportsService
{
    public class SurveyReportsService : ISurveyReportsService
    {
        private readonly DbContextPatientSurvey _patientSurveyContext = new DbContextPatientSurvey();
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;

        public SurveyReportsService()
        {
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_patientSurveyContext);
        }
        public List<PatientSurvey> GetPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            patientSurveySearchRequest.DATE_TO = Helper.GetCurrentDate();
            switch (patientSurveySearchRequest.TIME_FRAME)
            {
                case 1:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_FROM_STR))
                        patientSurveySearchRequest.DATE_FROM = Convert.ToDateTime(patientSurveySearchRequest.DATE_FROM_STR);
                    else
                        patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_TO_STR))
                        patientSurveySearchRequest.DATE_TO = Convert.ToDateTime(patientSurveySearchRequest.DATE_TO_STR);
                    break;
                default:
                    break;
            }
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var dateFrom = Helper.getDBNullOrValue("DATE_FROM", patientSurveySearchRequest.DATE_FROM.ToString());
            var dateTo = Helper.getDBNullOrValue("@DATE_TO", patientSurveySearchRequest.DATE_TO.ToString());
            var provider = new SqlParameter { ParameterName = "PROVIDER", Value = patientSurveySearchRequest.PROVIDER };
            var region = new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            var state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var format = new SqlParameter { ParameterName = "FORMAT", Value = patientSurveySearchRequest.FORMAT };
            var surveyedBy = new SqlParameter { ParameterName = "SURVEYED_BY", Value = patientSurveySearchRequest.SURVEYED_BY };
            var surveyStatus = new SqlParameter { ParameterName = "SURVEYED_STATUS", Value = patientSurveySearchRequest.SURVEYED_STATUS_CHILD };
            var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.CURRENT_PAGE };
            var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.RECORD_PER_PAGE };
            var searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = patientSurveySearchRequest.SEARCH_TEXT };
            var SortBy = Helper.getDBNullOrValue("SORT_BY", patientSurveySearchRequest.SORT_BY);
            var SortOrder = Helper.getDBNullOrValue("SORT_ORDER", patientSurveySearchRequest.SORT_ORDER);

            var patientSurvey = SpRepository<PatientSurvey>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_DETAILED_REPORT
                            @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @PROVIDER, @REGION, @STATE, @FORMAT, @SURVEYED_BY, @SURVEYED_STATUS, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORT_BY, @SORT_ORDER",
                            PracticeCode, dateFrom, dateTo, provider, region, state, format, surveyedBy, surveyStatus, CurrentPage, RecordPerPage, searchText, SortBy, SortOrder);
            return patientSurvey;
        }
        public List<PatientSurvey> GetALLPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            List<PatientSurvey> list = new List<PatientSurvey>();
           
            patientSurveySearchRequest.DATE_TO = Helper.GetCurrentDate();
            switch (patientSurveySearchRequest.TIME_FRAME)
            {
                case 1:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_FROM_STR))
                        patientSurveySearchRequest.DATE_FROM = Convert.ToDateTime(patientSurveySearchRequest.DATE_FROM_STR);
                    else
                        patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_TO_STR))
                        patientSurveySearchRequest.DATE_TO = Convert.ToDateTime(patientSurveySearchRequest.DATE_TO_STR);
                    break;
                default:
                    break;
            }
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var dateFrom = Helper.getDBNullOrValue("DATE_FROM", patientSurveySearchRequest.DATE_FROM.ToString());
            var dateTo = Helper.getDBNullOrValue("@DATE_TO", patientSurveySearchRequest.DATE_TO.ToString());
            var provider = new SqlParameter { ParameterName = "PROVIDER", Value = patientSurveySearchRequest.PROVIDER };
            var region = new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            var state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var format = new SqlParameter { ParameterName = "FORMAT", Value = patientSurveySearchRequest.FORMAT };
            var surveyedBy = new SqlParameter { ParameterName = "SURVEYED_BY", Value = patientSurveySearchRequest.SURVEYED_BY };
            var surveyStatus = new SqlParameter { ParameterName = "SURVEYED_STATUS", Value = patientSurveySearchRequest.SURVEYED_STATUS_CHILD };
            var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.CURRENT_PAGE };
            var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.RECORD_PER_PAGE };
            var searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = patientSurveySearchRequest.SEARCH_TEXT };
            var SortBy = Helper.getDBNullOrValue("SORT_BY", patientSurveySearchRequest.SORT_BY);
            var SortOrder = Helper.getDBNullOrValue("SORT_ORDER", patientSurveySearchRequest.SORT_ORDER);

             list = SpRepository<PatientSurvey>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_DETAILED_REPORT
                            @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @PROVIDER, @REGION, @STATE, @FORMAT, @SURVEYED_BY, @SURVEYED_STATUS, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORT_BY, @SORT_ORDER",
                            PracticeCode, dateFrom, dateTo, provider, region, state, format, surveyedBy, surveyStatus, CurrentPage, RecordPerPage, searchText, SortBy, SortOrder);
         
            return list;
        }
        public PSDRChartData GetALLPendingPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            List<PatientSurvey> list = new List<PatientSurvey>();
            PSDRChartData obj = new PSDRChartData();

            patientSurveySearchRequest.RECORD_PER_PAGE = 0;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Completed Survey ,Deceased,Not Enough Services Provided";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.COMPLETED = list.Count;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Completed Survey";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.COMPLETED_SURVEY = list.Count;

            //patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Not Recommended";
            //list = new List<PatientSurvey>();
            //list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            //obj.NOT_RECOMMENDED = list.Count;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Deceased";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.DECEASED = list.Count;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Not Enough Services Provided";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.NOT_ENOUGH_SERVICES_PROVIDE = list.Count;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Callback,Not Answered";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.INCOMPLETE = list.Count;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Callback";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.CALL_BACK = list.Count;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Not Answered";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.NOT_ANSWERED = list.Count;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Not Interested";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.NOT_INTERESTED = list.Count;

            patientSurveySearchRequest.SURVEYED_STATUS_CHILD = "Pending";
            patientSurveySearchRequest.TIME_FRAME = 4;
            patientSurveySearchRequest.DATE_FROM_STR = "";
            patientSurveySearchRequest.DATE_TO_STR = "";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.PENDING_ALL = list.Count;

            patientSurveySearchRequest.TIME_FRAME = 3;
            patientSurveySearchRequest.DATE_FROM_STR = "";
            patientSurveySearchRequest.DATE_TO_STR = "";
            list = new List<PatientSurvey>();
            list = GetPSRDetailedReport(patientSurveySearchRequest, profile);
            obj.PENDING_30 = list.Count;

            return obj;
        }
        public string ExportToExcelPSRDetailedReport(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            try
            {
                string fileName = "Detailed_Survey_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                patientSurveySearchRequest.CURRENT_PAGE = 1;
                patientSurveySearchRequest.RECORD_PER_PAGE = 0;
                var CalledFrom = "";
                if (patientSurveySearchRequest.FORMAT.Equals("New Format") || patientSurveySearchRequest.FORMAT.Equals("All"))
                {
                     CalledFrom = "Detailed_Survey";
                }
                else
                {
                   CalledFrom = "Detailed_Survey_old";
                }
               
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<PatientSurvey> result = new List<PatientSurvey>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetPSRDetailedReport(patientSurveySearchRequest, profile);
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].ROW = i + 1;
                }
                //for (int i = 0; i < result.Count; i++)
                //{
                //    result[i].MONTH = Convert.ToDateTime(result[i].MODIFIED_DATE).ToString("MMMMM-y");
                //}
                exported = ExportToExcel.CreateExcelDocument<PatientSurvey>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PSRRegionAndQuestionWise> GetPSRRegionAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            patientSurveySearchRequest.DATE_TO = Helper.GetCurrentDate();
            switch (patientSurveySearchRequest.TIME_FRAME)
            {
                case 1:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_FROM_STR))
                        patientSurveySearchRequest.DATE_FROM = Convert.ToDateTime(patientSurveySearchRequest.DATE_FROM_STR);
                    else
                        patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_TO_STR))
                        patientSurveySearchRequest.DATE_TO = Convert.ToDateTime(patientSurveySearchRequest.DATE_TO_STR);
                    break;
                default:
                    break;
            }

            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var _dateFrom = Helper.getDBNullOrValue("DATE_FROM", patientSurveySearchRequest.DATE_FROM.ToString());
            var _dateTo = Helper.getDBNullOrValue("DATE_TO", patientSurveySearchRequest.DATE_TO.ToString());
            var _provider = new SqlParameter { ParameterName = "PROVIDER", Value = patientSurveySearchRequest.PROVIDER };
            var _state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var _format = new SqlParameter { ParameterName = "FORMAT", Value = patientSurveySearchRequest.FORMAT };
            var _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = patientSurveySearchRequest.SEARCH_TEXT };

            var PatientSurveyReport = SpRepository<PSRRegionAndQuestionWise>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_REGION_AND_QUESTION_WISE
                @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @PROVIDER, @STATE, @FORMAT, @SEARCH_TEXT", PracticeCode, _dateFrom, _dateTo, _provider, _state, _format, _searchText);
            if (PatientSurveyReport.Count == 1) { return new List<PSRRegionAndQuestionWise>(); }
            return PatientSurveyReport;
        }
        public string ExportToExcelPSRRegionAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            try
            {
                string fileName = "Question_Survey_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                patientSurveySearchRequest.CURRENT_PAGE = 1;
                patientSurveySearchRequest.RECORD_PER_PAGE = 0;
                var CalledFrom = "Question_Survey";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<PSRRegionAndQuestionWise> result = new List<PSRRegionAndQuestionWise>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetPSRRegionAndQuestionWise(patientSurveySearchRequest, profile);
                //for (int i = 0; i < result.Count; i++)
                //{
                //    result[i].ROW = i + 1;

                //}
                exported = ExportToExcel.CreateExcelDocument<PSRRegionAndQuestionWise>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PSRProviderAndQuestionWise> GetPSRProviderAndQuestionWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            patientSurveySearchRequest.DATE_TO = Helper.GetCurrentDate();
            switch (patientSurveySearchRequest.TIME_FRAME)
            {
                case 1:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_FROM_STR))
                        patientSurveySearchRequest.DATE_FROM = Convert.ToDateTime(patientSurveySearchRequest.DATE_FROM_STR);
                    else
                        patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_TO_STR))
                        patientSurveySearchRequest.DATE_TO = Convert.ToDateTime(patientSurveySearchRequest.DATE_TO_STR);
                    break;
                default:
                    break;
            }

            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var _region = new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            var _dateFrom = Helper.getDBNullOrValue("DATE_FROM", patientSurveySearchRequest.DATE_FROM.ToString());
            var _dateTo = Helper.getDBNullOrValue("DATE_TO", patientSurveySearchRequest.DATE_TO.ToString());
            var _provider = new SqlParameter { ParameterName = "PROVIDER", Value = patientSurveySearchRequest.PROVIDER };
            var _state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var _format = new SqlParameter { ParameterName = "FORMAT", Value = patientSurveySearchRequest.FORMAT };
            var _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = patientSurveySearchRequest.SEARCH_TEXT };

            var PatientSurveyReportProviderAndQuestionWise = SpRepository<PSRProviderAndQuestionWise>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_PROVIDER_AND_QUESTION_WISE
                @PRACTICE_CODE, @REGION, @DATE_FROM, @DATE_TO, @PROVIDER, @STATE, @FORMAT, @SEARCH_TEXT",
                PracticeCode, _region, _dateFrom, _dateTo, _provider, _state, _format, _searchText);
            return PatientSurveyReportProviderAndQuestionWise;
        }

        public List<PSRRegionAndRecommendationWise> GetPSRRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            patientSurveySearchRequest.DATE_TO = Helper.GetCurrentDate();
            switch (patientSurveySearchRequest.TIME_FRAME)
            {
                case 1:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_FROM_STR))
                        patientSurveySearchRequest.DATE_FROM = Convert.ToDateTime(patientSurveySearchRequest.DATE_FROM_STR);
                    else
                        patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_TO_STR))
                        patientSurveySearchRequest.DATE_TO = Convert.ToDateTime(patientSurveySearchRequest.DATE_TO_STR);
                    break;
                default:
                    break;
            }
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var _dateFrom = Helper.getDBNullOrValue("DATE_FROM", patientSurveySearchRequest.DATE_FROM.ToString());
            var _dateTo = Helper.getDBNullOrValue("DATE_TO", patientSurveySearchRequest.DATE_TO.ToString());
            var _provider = new SqlParameter { ParameterName = "PROVIDER", Value = patientSurveySearchRequest.PROVIDER };
            var _state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var _region= new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            var _discipline = new SqlParameter { ParameterName = "DISCIPLINE", Value = patientSurveySearchRequest.DISCIPLINE };
            var _status = new SqlParameter { ParameterName = "STATUS", Value = patientSurveySearchRequest.SURVEYED_STATUS_CHILD };
            var _recommended = new SqlParameter { ParameterName = "RECOMMENDED", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.RECOMMENDED };
            var _notRecommended = new SqlParameter { ParameterName = "NOT_RECOMMENDED", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.NOT_RECOMMENDED };
            var _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = patientSurveySearchRequest.SEARCH_TEXT };

            var PatientSurveyReport = SpRepository<PSRRegionAndRecommendationWise>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_REGION_AND_RECOMMENDATION_WISE
                @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @PROVIDER, @STATE, @REGION, @DISCIPLINE, @STATUS, @RECOMMENDED, @NOT_RECOMMENDED, @SEARCH_TEXT",
                PracticeCode, _dateFrom, _dateTo, _provider, _state, _region, _discipline, _status, _recommended, _notRecommended, _searchText);
            if (PatientSurveyReport.Count == 1) { return new List<PSRRegionAndRecommendationWise>(); }
            return PatientSurveyReport;
        }
        public List<RegionWisePatientData> GetRegionWisePatientData(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            patientSurveySearchRequest.DATE_TO = Helper.GetCurrentDate();
            switch (patientSurveySearchRequest.TIME_FRAME)
            {
                case 1:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_FROM_STR))
                        patientSurveySearchRequest.DATE_FROM = Convert.ToDateTime(patientSurveySearchRequest.DATE_FROM_STR);
                    else
                        patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_TO_STR))
                        patientSurveySearchRequest.DATE_TO = Convert.ToDateTime(patientSurveySearchRequest.DATE_TO_STR);
                    break;
                default:
                    break;
            }
            SqlParameter PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _dateFrom = Helper.getDBNullOrValue("DATE_FROM", patientSurveySearchRequest.DATE_FROM.ToString());
            SqlParameter _dateTo = Helper.getDBNullOrValue("DATE_TO", patientSurveySearchRequest.DATE_TO.ToString());
            SqlParameter _provider = new SqlParameter { ParameterName = "PROVIDER", Value = patientSurveySearchRequest.PROVIDER };
            SqlParameter _state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            SqlParameter _region = new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            SqlParameter _discipline = new SqlParameter { ParameterName = "DISCIPLINE", Value = patientSurveySearchRequest.DISCIPLINE };
            SqlParameter _status = new SqlParameter { ParameterName = "STATUS", Value = patientSurveySearchRequest.SURVEYED_STATUS_CHILD };
            SqlParameter _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = patientSurveySearchRequest.SEARCH_TEXT };
            SqlParameter _currentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.CURRENT_PAGE };
            SqlParameter _recordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.RECORD_PER_PAGE };
            SqlParameter _sortBy = new SqlParameter { ParameterName = "SORT_BY", Value = patientSurveySearchRequest.SORT_BY };
            SqlParameter _sortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = patientSurveySearchRequest.SORT_ORDER };

            var res = SpRepository<RegionWisePatientData>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_REGION_WISE_PATIENT_DATA 
                @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @PROVIDER, @STATE, @REGION, @DISCIPLINE, @STATUS, @SEARCH_TEXT, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                PracticeCode, _dateFrom, _dateTo, _provider, _state, _region, _discipline, _status, _searchText, _currentPage, _recordPerPage, _sortBy, _sortOrder);
          
            return res;
        }
        public string ExportToExcelRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            try
            {
                string fileName = "Region_Wise_Survey_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                patientSurveySearchRequest.CURRENT_PAGE = 1;
                patientSurveySearchRequest.RECORD_PER_PAGE = 0;
                var CalledFrom = "Region_Survey";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<PSRRegionAndRecommendationWise> result = new List<PSRRegionAndRecommendationWise>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetPSRRegionAndRecommendationWise(patientSurveySearchRequest, profile);
                //for (int i = 0; i < result.Count; i++)
                //{
                //    result[i].ROW = i + 1;

                //}
                exported = ExportToExcel.CreateExcelDocument<PSRRegionAndRecommendationWise>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ExportToExcelRegionWisePatientData(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            try
            {
                string fileName = "Region_Wise_Patient_Data";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                patientSurveySearchRequest.CURRENT_PAGE = 1;
                patientSurveySearchRequest.RECORD_PER_PAGE = 0;
                var CalledFrom = "Region_Survey_Patient_Data";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<RegionWisePatientData> result = new List<RegionWisePatientData>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetRegionWisePatientData(patientSurveySearchRequest, profile);
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<RegionWisePatientData>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PSRProviderAndRecommendationWise> GetPSRProviderAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            patientSurveySearchRequest.DATE_TO = Helper.GetCurrentDate();
            switch (patientSurveySearchRequest.TIME_FRAME)
            {
                case 1:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_FROM_STR))
                        patientSurveySearchRequest.DATE_FROM = Convert.ToDateTime(patientSurveySearchRequest.DATE_FROM_STR);
                    else
                        patientSurveySearchRequest.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(patientSurveySearchRequest.DATE_TO_STR))
                        patientSurveySearchRequest.DATE_TO = Convert.ToDateTime(patientSurveySearchRequest.DATE_TO_STR);
                    break;
                default:
                    break;
            }

            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var _region = new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            var _dateFrom = Helper.getDBNullOrValue("DATE_FROM", patientSurveySearchRequest.DATE_FROM.ToString());
            var _dateTo = Helper.getDBNullOrValue("DATE_TO", patientSurveySearchRequest.DATE_TO.ToString());
            var _provider = new SqlParameter { ParameterName = "PROVIDER", Value = patientSurveySearchRequest.PROVIDER };
            var _state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var _discipline = new SqlParameter { ParameterName = "DISCIPLINE", Value = patientSurveySearchRequest.DISCIPLINE };
            var _status = new SqlParameter { ParameterName = "STATUS", Value = patientSurveySearchRequest.SURVEYED_STATUS_CHILD };
            var _recommended = new SqlParameter { ParameterName = "RECOMMENDED", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.RECOMMENDED };
            var _notRecommended = new SqlParameter { ParameterName = "NOT_RECOMMENDED", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.NOT_RECOMMENDED };
            var _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = patientSurveySearchRequest.SEARCH_TEXT };

            var PatientSurveyReport = SpRepository<PSRProviderAndRecommendationWise>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_PROVIDER_AND_RECOMMENDATION_WISE
                @PRACTICE_CODE, @REGION, @DATE_FROM, @DATE_TO, @PROVIDER, @STATE, @DISCIPLINE, @STATUS, @RECOMMENDED,  @NOT_RECOMMENDED, @SEARCH_TEXT",
                PracticeCode, _region, _dateFrom, _dateTo, _provider, _state, _discipline, _status, _recommended, _notRecommended, _searchText);
            return PatientSurveyReport;
        }
    }
}