﻿using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.QualityAsuranceModel;
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

namespace FOX.BusinessOperations.QualityAssuranceService.QAReportService
{
    public class QAReportService : IQAReportService
    {
        private readonly DBContextQualityAssurance _qualityAssuranceContext = new DBContextQualityAssurance();
        private readonly GenericRepository<SurveyAuditScores> _auditScoresRepository;
        private readonly GenericRepository<GradingSetup> _gradingSetupRepository;
        public List<GradingSetup> GradingCriteria = new List<GradingSetup>();
        public QAReportService()
        {
            _auditScoresRepository = new GenericRepository<SurveyAuditScores>(_qualityAssuranceContext);
            _gradingSetupRepository = new GenericRepository<GradingSetup>(_qualityAssuranceContext);
            GradingCriteria = new List<GradingSetup>();
        }
        public List<FeedBackCaller> GetListOfAgents(long practiceCode)
        {
            try
            {
                List<FeedBackCaller> result = new List<FeedBackCaller>();
                var List = _auditScoresRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode).Select(x => x.AUDITOR_NAME).Distinct().ToList();

                List<string> response = new List<string>();
                response = List;

                if (List.Count > 0)
                {
                    for (int i = 0; i < List.Count; i++)
                    {
                        var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                        var _userName = new SqlParameter { ParameterName = "USER_NAME", SqlDbType = SqlDbType.VarChar, Value = List[i] };

                        var  lst = SpRepository<FeedBackCaller>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_AUDITOR_NAME_LIST
                         @PRACTICE_CODE, @USER_NAME", PracticeCode, _userName);
                        result.Add(lst);
                    }
                    return result;
                }
                else
                {
                    return new List<FeedBackCaller>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AuditScoresList> AuditReport(QAReportSearchRequest reg, UserProfile profile)
        {
            List<AuditScoresListTemp> tempList = new List<AuditScoresListTemp>();
            List<AuditScoresList> returList = new List<AuditScoresList>();

            reg.DATE_TO = Helper.GetCurrentDate();
            switch (reg.TIME_FRAME)
            {
                case 1:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(reg.DATE_FROM_STR))
                        reg.DATE_FROM = Convert.ToDateTime(reg.DATE_FROM_STR);
                    else
                        reg.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(reg.DATE_TO_STR))
                        reg.DATE_TO = Convert.ToDateTime(reg.DATE_TO_STR);
                    break;
                default:
                    break;
            }

            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = reg.SEARCH_TEXT };
            SqlParameter _agentName = new SqlParameter { ParameterName = "AGENT_NAME", Value = reg.AGENT_NAME };
            SqlParameter _auditorName = new SqlParameter { ParameterName = "AUDITOR_NAME", Value = reg.AUDITOR_NAME };
            SqlParameter _dateFrom = Helper.getDBNullOrValue("DATE_FROM", reg.DATE_FROM.ToString());
            SqlParameter _dateTos = Helper.getDBNullOrValue("DATE_TO", reg.DATE_TO.ToString());
            SqlParameter _currentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = reg.CURRENT_PAGE };
            SqlParameter _recordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = reg.RECORD_PER_PAGE };
            SqlParameter _sortBy = new SqlParameter { ParameterName = "SORT_BY", Value = reg.SORT_BY };
            SqlParameter _sortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = reg.SORT_ORDER };
            SqlParameter _calltype = new SqlParameter { ParameterName = "CALL_TYPE", Value = reg.CALL_TYPE };



            tempList = SpRepository<AuditScoresListTemp>.GetListWithStoreProcedure(@"exec FOX_PROC_AUDTIT_SCORES_LIST
                            @PRACTICE_CODE, @SEARCH_TEXT, @AGENT_NAME, @AUDITOR_NAME, @DATE_FROM, @DATE_TO, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER, @CALL_TYPE"
                            , _practiceCode, _searchText, _agentName, _auditorName, _dateFrom, _dateTos, _currentPage, _recordPerPage, _sortBy, _sortOrder, _calltype);
            GradingCriteria = GetListOfGradingCriteria(profile.PracticeCode);

            if (reg.AUDITOR_NAME != "")
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    AuditScoresList obj = new AuditScoresList();
                    obj.NAME = tempList[i].NAME;
                    obj.AGENT_NAME = tempList[i].AGENT_NAME;
                    obj.ROW = tempList[i].ROW;
                    obj.TOTAL_POINTS = tempList[i].TOTAL_POINTS + "%    " + getGrade(tempList[i].TOTAL_POINTS) + " /  " + tempList[i].TOTAL_AVG + "%    " + getGrade(tempList[i].TOTAL_AVG);
                    obj.CLIENT_EXPERIENCE_TOTAL = tempList[i].CLIENT_EXPERIENCE_TOTAL + " /  " + tempList[i].CLIENT_EXPERIENCE_AVG_TOTAL;
                    obj.SYSTEM_PROCESS_TOTAL = tempList[i].SYSTEM_PROCESS_TOTAL + " /  " + tempList[i].SYSTEM_PROCESS_AVG_TOTAL;
                    obj.WOW_FACTOR = tempList[i].WOW_FACTOR + " /  " + tempList[i].AVG_WOW_FACTOR;
                    obj.EVALUATIONS = tempList[i].EVALUATIONS + " /  " + tempList[i].AVG_EVALUATIONS ;
                    obj.TOTAL_RECORDS = tempList[i].TOTAL_RECORDS;
                    obj.TOTAL_RECORD_PAGES = tempList[i].TOTAL_RECORD_PAGES;

                    returList.Add(obj);
                }
            }
            else
            {
                for(int i= 0; i< tempList.Count; i++)
                {
                    AuditScoresList obj = new AuditScoresList();
                    obj.NAME = tempList[i].NAME;
                    obj.AGENT_NAME = tempList[i].AGENT_NAME;
                    obj.ROW = tempList[i].ROW;
                    obj.TOTAL_POINTS = tempList[i].TOTAL_POINTS + "%   " + "  "+ getGrade(tempList[i].TOTAL_POINTS);
                    obj.CLIENT_EXPERIENCE_TOTAL = tempList[i].CLIENT_EXPERIENCE_TOTAL.ToString();
                    obj.SYSTEM_PROCESS_TOTAL = tempList[i].SYSTEM_PROCESS_TOTAL.ToString();
                    obj.WOW_FACTOR = tempList[i].WOW_FACTOR.ToString();
                    obj.EVALUATIONS = tempList[i].EVALUATIONS;
                    obj.TOTAL_RECORDS = tempList[i].TOTAL_RECORDS;
                    obj.TOTAL_RECORD_PAGES = tempList[i].TOTAL_RECORD_PAGES;
                
                    returList.Add(obj);
                }
            }
            return returList;
        }
        public string getGrade(decimal? result)
        {
            if (result >= GradingCriteria[0].OVERALL_MIN)
                return "A";
            if (result <= GradingCriteria[1].OVERALL_MAX && result >= GradingCriteria[1].OVERALL_MIN)
                return "B";
            if (result <= GradingCriteria[2].OVERALL_MAX)
                return "U";

            return "";
        }
        public List<GradingSetup> GetListOfGradingCriteria(long practiceCode)
        {
            try
            {
                var response = _gradingSetupRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == practiceCode).ToList();

                if (response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<GradingSetup>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExportToExcelQAReport(QAReportSearchRequest req, UserProfile profile)
        {
            try 
            {
                string fileName = "QA_Report_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CURRENT_PAGE = 1;
                req.RECORD_PER_PAGE = 0;
                var CalledFrom = "";
                if (req.AUDITOR_NAME == "")
                {
                    CalledFrom = "QA_Report";
                }
                else
                {
                    CalledFrom = "QA_Report_AUD";
                }

                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<AuditScoresList> result = new List<AuditScoresList>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = AuditReport(req, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<AuditScoresList>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
