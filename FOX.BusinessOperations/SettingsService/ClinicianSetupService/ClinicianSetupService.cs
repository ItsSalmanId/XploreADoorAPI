using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.ClinicianSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FOX.BusinessOperations.SettingsService.ClinicianSetupService
{
    public class ClinicianSetupService : IClinicianSetupService
    {
        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly DbContextSettings _dbContextSettings = new DbContextSettings();
        private readonly GenericRepository<FoxProviderClass> _FoxProviderClassRepository;
        private readonly GenericRepository<VisitQoutaWeek> _VisitQoutaWeekRepository;
        private readonly GenericRepository<FOX_TBL_DISCIPLINE> _DisciplineRepository;


        public ClinicianSetupService()
        {
            _FoxProviderClassRepository = new GenericRepository<FoxProviderClass>(_dbContextSettings);
            _VisitQoutaWeekRepository = new GenericRepository<VisitQoutaWeek>(_dbContextSettings);
            _DisciplineRepository = new GenericRepository<FOX_TBL_DISCIPLINE>(_CaseContext);
        }
        public GetClinicanRes InsertUpdateClinician(FoxProviderClass obj, UserProfile profile)
        {
            GetClinicanRes response = new GetClinicanRes();
            response.Message = "";
            bool IsEdit = false;
            //var newClinicain = new FoxProviderClass();
            var ClinicianData = _FoxProviderClassRepository.GetFirst(x => x.FOX_PROVIDER_ID == obj.FOX_PROVIDER_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
            if (ClinicianData == null)
            {

                obj.FOX_PROVIDER_ID = Helper.getMaximumId("FOX_PROVIDER_ID");
                obj.CREATED_BY = profile.UserName;
                obj.CREATED_DATE = DateTime.Now;
                obj.PRACTICE_CODE = profile.PracticeCode;
                obj.DELETED = false;
                response.Message = "created";
                IsEdit = false;
                if (obj.STATE == null || obj.STATE.Length > 2)
                    obj.STATE = "";
            }
            else
            {
                IsEdit = true;
                if (obj.STATE == null || obj.STATE.Length > 2)
                    obj.STATE = "";
                ClinicianData.ACTIVE_CASES = obj.ACTIVE_CASES;
                ClinicianData.ADDRESS = obj.ADDRESS;
                ClinicianData.DELETED = obj.DELETED;
                ClinicianData.DISCIPLINE_ID = obj.DISCIPLINE_ID;
                ClinicianData.FIRST_NAME = obj.FIRST_NAME;
                ClinicianData.LAST_NAME = obj.LAST_NAME;
                //ClinicianData.FOX_PROVIDER_CODE = obj.FOX_PROVIDER_CODE;
                //ClinicianData.IS_ACTIVE = obj.IS_ACTIVE;
                ClinicianData.IS_INACTIVE = obj.IS_INACTIVE;
                ClinicianData.INDIVIDUAL_NPI = obj.INDIVIDUAL_NPI;
                ClinicianData.PRACTICE_CODE = profile.PracticeCode;
                ClinicianData.PRIMARY_POS_DISTANCE = obj.PRIMARY_POS_DISTANCE;
                //ClinicianData.PROVIDER_CODE = obj.PROVIDER_CODE;
                ClinicianData.PTO_HRS = obj.PTO_HRS;
                ClinicianData.REFERRAL_REGION_ID = obj.REFERRAL_REGION_ID;
                ClinicianData.SSN = obj.SSN;
                ClinicianData.STATE = obj.STATE;
                ClinicianData.TREATMENT_LOC_ID = obj.TREATMENT_LOC_ID;
                ClinicianData.VISIT_QOUTA_WEEK_ID = obj.VISIT_QOUTA_WEEK_ID;
                ClinicianData.MODIFIED_BY = profile.UserName;
                ClinicianData.MODIFIED_DATE = DateTime.Now;
                ClinicianData.CLR = obj.CLR;
                if (obj.DELETED)
                    response.Message = "deleted";
                else
                    response.Message = "updated";
            }
            if (!IsEdit)
            {
                _FoxProviderClassRepository.Insert(obj);
            }
            else
            {
                _FoxProviderClassRepository.Update(ClinicianData);
            }
            _dbContextSettings.SaveChanges();
            return response;
        }
        public List<VisitQoutaWeek> GetVisitQoutaPerWeek(UserProfile profile)
        {
            var result = _VisitQoutaWeekRepository.GetMany(x => x.PRACTICE_CODE == profile.PracticeCode && !(x.DELETED ?? false)).ToList();
            if (result.Any())
                return result;
            else
                return new List<VisitQoutaWeek>();
        }

        public List<FOX_TBL_DISCIPLINE> GetDisciplines(UserProfile profile)
        {

            var result = _DisciplineRepository.GetMany(x => x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED).ToList();
            if (result.Any())
                return result;
            else
                return new List<FOX_TBL_DISCIPLINE>();
        }
        public List<SmartRefRegion> GetSmartRefRegion(string searchText, UserProfile Profile)
        {

            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = searchText };
            var result = SpRepository<SmartRefRegion>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_REF_REGION] @PRACTICE_CODE, @SEARCHVALUE", parmPracticeCode, smartvalue).ToList();
            if (result.Any())
                return result;
            else
                return new List<SmartRefRegion>();

        }


        public List<FoxProviderClass> GetClinician(GetClinicanReq req, UserProfile Profile)
        {
            if (req.CURRENT_PAGE == 0)
                req.CURRENT_PAGE = 10;
            if (req.RECORD_PER_PAGE == 0)
                req.RECORD_PER_PAGE = 10;
            if (req.SEARCH_STRING == null)
                req.SEARCH_STRING = "";
            if (req.SORT_BY == null)
                req.SORT_BY = "CREATED_DATE";
            if (req.SORT_ORDER == null)
                req.SORT_ORDER = "DESC";

            var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var cURRENT_PAGE = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CURRENT_PAGE };
            var fOX_PROVIDER_ID = new SqlParameter("FOX_PROVIDER_ID", SqlDbType.BigInt) { Value = req.FOX_PROVIDER_ID };
            var rECORD_PER_PAGE = new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = req.RECORD_PER_PAGE };
            var sEARCH_STRING = new SqlParameter("SEARCH_STRING", SqlDbType.VarChar) { Value = req.SEARCH_STRING };
            var sORT_BY = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SORT_BY };
            var sORT_ORDER = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SORT_ORDER };
            var first_Name = new SqlParameter("FIRST_NAME", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(req.First_Name) ? "" : req.First_Name };
            var email = new SqlParameter("EMAIL", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(req.EMAIL) ? "" : req.EMAIL };
            var last_Name = new SqlParameter("LAST_NAME", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(req.Last_Name) ? "" : req.Last_Name };
            var rt_Code = new SqlParameter("RT_CODE", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(req.RT_Code) ? "" : req.RT_Code };
            var npi = new SqlParameter("NPI", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(req.NPI) ? "" : req.NPI };
            var discipline = new SqlParameter("DISCIPLINE", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(req.Discipline) ? "" : req.Discipline };
            var treating_region = new SqlParameter("TREATING_REGION", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(req.Treating_Region) ? "" : req.Treating_Region };
            var treating_location = new SqlParameter("TREATING_LOCATION", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(req.Treating_Location) ? "" : req.Treating_Location };
            var result = SpRepository<FoxProviderClass>.GetListWithStoreProcedure(@"exec [FOX_GET_FOX_PROVIDER] @PRACTICE_CODE, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE,@FOX_PROVIDER_ID,@SORT_BY,@SORT_ORDER,@FIRST_NAME,@LAST_NAME, @EMAIL,@RT_CODE,@NPI,@DISCIPLINE,@TREATING_REGION,@TREATING_LOCATION",
                practiceCode, sEARCH_STRING, cURRENT_PAGE, rECORD_PER_PAGE, fOX_PROVIDER_ID, sORT_BY, sORT_ORDER, first_Name, last_Name, email, rt_Code, npi, discipline, treating_region, treating_location).ToList();
            if (result.Any())
                return result;
            else
                return new List<FoxProviderClass>();

        }

        public string Export(GetClinicanReq obj, UserProfile profile)
        {
            try
            {
                string fileName = "Clinician_Data";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                obj.CURRENT_PAGE = 1;
                obj.RECORD_PER_PAGE = 1000000;
                var CalledFrom = "Clinician_Data";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FoxProviderClass> result = new List<FoxProviderClass>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetClinician(obj, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;
                    //result.CorrectedClaims[i].CHARGE_ENTRY_DATE_str = result.CorrectedClaims[i].CHARGE_ENTRY_DATE?.ToString("MM/dd/yyyy");
                    //result.CorrectedClaims[i].WORK_DATE_Str = result.CorrectedClaims[i].WORK_DATE?.ToString("MM/dd/yyyy");
                    //result.CorrectedClaims[i].REQUESTED_DATE_Str = result.CorrectedClaims[i].REQUESTED_DATE?.ToString("MM/dd/yyyy");
                    //result.CorrectedClaims[i].RESPONSE_DATE_Str = result.CorrectedClaims[i].RESPONSE_DATE?.ToString("MM/dd/yyyy");

                }
                exported = ExportToExcel.CreateExcelDocument<FoxProviderClass>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckNPI(string NPI, UserProfile profile)
        {

            var Result = _FoxProviderClassRepository.GetMany(x => x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED).ToList();
            var matchNpi = Result.Where(t => t.INDIVIDUAL_NPI == NPI).FirstOrDefault();
            if (matchNpi != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckSSN(string SSN, UserProfile profile)
        {

            var Result = _FoxProviderClassRepository.GetMany(x => x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED).ToList();
            var matchSSN = Result.Where(t => t.SSN == SSN).FirstOrDefault();
            if (matchSSN != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<ProviderLocationRes> GetSpecficProviderLocation(ProviderLocationReq obj, UserProfile Profile)
        {
            var pROVIDER_TYPE = new SqlParameter("ROVIDER_TYPE", SqlDbType.VarChar) { Value = obj.ROVIDER_TYPE };
            var fOX_PROVIDER_ID = new SqlParameter("FOX_PROVIDER_ID", SqlDbType.BigInt) { Value = obj.FOX_PROVIDER_ID };
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var result = SpRepository<ProviderLocationRes>.GetListWithStoreProcedure(@"exec [FOX_GET_PROVIDER_LOCATION] @ROVIDER_TYPE,@FOX_PROVIDER_ID,@PRACTICE_CODE", pROVIDER_TYPE, fOX_PROVIDER_ID, parmPracticeCode).ToList();
            if (result.Any())
                return result;
            else
                return new List<ProviderLocationRes>();

        }
        
        public ResponseModel ReadExcel(string fileName, long practiceCode, string userName)
        {
            try
            {
                DataTable tbl = new DataTable();
                ResponseModel responseModel = new ResponseModel();
                string path = @"~\FoxDocumentDirectory\PatientSurvey\UploadFiles\" + fileName;
                path = HttpContext.Current.Server.MapPath(path);
                string fileType = path.Substring(path.Length - 3);
                long totalRecordInFile = 0;
                long total_record_updated = 0;
                List<string>Failed_Records = new List<string>(); 
                #region .csv File Reader Region

                if (fileType == "csv") // for .csv file
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(path);
                    //Csv reader reads the stream
                    CsvHelper.CsvReader csvread = new CsvHelper.CsvReader(sr);
                    csvread.Configuration.PrepareHeaderForMatch = header => header.Replace(" ", string.Empty);
                    csvread.Configuration.PrepareHeaderForMatch = header => header.Replace("%", string.Empty);

                    //csvread will fetch all record in one go to the IEnumerable object record
                    IEnumerable<ProviderExcel> record = csvread.GetRecords<ProviderExcel>().ToList();
                    totalRecordInFile = record.Count();
                    if (totalRecordInFile > 3000)
                    {
                        responseModel.Success = false;
                        responseModel.Message = "Please upload file having maximum of 3000 records";
                        responseModel.ErrorMessage = "Can not upload file, number of records are more than 3000(limit)";
                        return responseModel;
                    }
                    
                    foreach (var rec in record) // Each record will be fetched
                    {
                        var Provider = new FoxProviderClass();
                        if (!string.IsNullOrEmpty(rec.ClinicianCode) && rec.ClinicianCode.Length <= 50)
                            Provider.FOX_PROVIDER_CODE = rec.ClinicianCode;
                        if (!string.IsNullOrEmpty(rec.CL))
                        {
                            Decimal amount;
                            rec.CL = rec.CL.Replace("%", "");
                            if (Decimal.TryParse(rec.CL, out amount))
                                Provider.CLR = amount;
                            else
                            {
                                Provider.CLR = 0;
                            }
                        }
                        bool record_updated = UpdateProviderCLR(Provider, practiceCode);
                        if(record_updated)
                        {
                            total_record_updated++;
                        }
                        else
                        {
                            Failed_Records.Add(Provider.FOX_PROVIDER_CODE);
                        }
                    }
                    sr.Close();
                }

                #endregion

                #region .xlsx File Reader Region

                else // for .xlsx file
                {

                    bool hasHeader = true;
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = System.IO.File.OpenRead(path))
                        {
                            pck.Load(stream);
                        }
                        // Pick Headers from 1st Excel Sheet
                        var wsmain = pck.Workbook.Worksheets[1];
                        for (var i = 1; i < 41; i++)
                        {

                            var temp = wsmain.Cells[1, i, 1, i];
                            tbl.Columns.Add(hasHeader ? temp.Text : string.Format("Column {0}", temp.Start.Column));
                        }
                        // Pick Values from All Sheets
                        for (int wb = 1; wb <= pck.Workbook.Worksheets.Count; wb++)
                        {

                            var ws = pck.Workbook.Worksheets[wb];
                            var startRow = hasHeader ? 2 : 1;
                            bool valueExist = false;
                            if (ws.Dimension != null)
                            {
                                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                {
                                    valueExist = false;
                                    var wsRow = ws.Cells[rowNum, 1, rowNum, 40];
                                    DataRow row = tbl.Rows.Add();
                                    foreach (var cell in wsRow)
                                    {
                                        if (!string.IsNullOrEmpty(cell.Text)) valueExist = true;
                                        row[cell.Start.Column - 1] = cell.Text;
                                    }
                                    if (!valueExist)
                                        tbl.Rows.Remove(row);
                                }
                            }

                        }
                    }
                    totalRecordInFile = tbl.Rows.Count;
                    foreach (System.Data.DataRow row in tbl.Rows)
                    {
                         var Provider = new FoxProviderClass();
                        if (!string.IsNullOrEmpty(row["Clinician Code"].ToString()) && row["Clinician Code"].ToString().Length <= 50)
                            Provider.FOX_PROVIDER_CODE = row["Clinician Code"].ToString();
                        Decimal amount;
                        if (Decimal.TryParse(row["% CL"].ToString().Replace("%",""), out amount))
                        {
                            
                            if (Decimal.TryParse(row["% CL"].ToString(), out amount))
                                Provider.CLR = amount;
                            else
                            {
                                Provider.CLR = 0;
                            }
                        }
                        bool record_updated = UpdateProviderCLR(Provider, practiceCode);
                        if(record_updated)
                        {
                            total_record_updated++;
                        }
                        else
                        {
                            Failed_Records.Add(Provider.FOX_PROVIDER_CODE);
                        }
                    }
                }
                long record_failed = totalRecordInFile - total_record_updated;
                #endregion
                //string Failed_records_string = "";
                
                string[] temp1 = Failed_Records.ToArray();
                string Failed_records_string = String.Join(", ", temp1);
                //foreach(string item in Failed_Records)
                //{
                //    if (!string.IsNullOrEmpty(item))
                //    {
                //        Failed_records_string.Append(item + ", ");
                //    }
                    
                //}
                var directoryPath = "";
                string filePathOther = "";
                directoryPath = HttpContext.Current.Server.MapPath(@"~/FoxDocumentDirectory/Fox/CLR");
                {
                    
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    filePathOther = directoryPath + "//CLR_Log_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + "_" + DateTime.Now.Ticks + ".txt";
                    string textToType = "CLR Log Report: " + Environment.NewLine +
                           "--------------------------" + Environment.NewLine +
                           "Total Records in File: " + totalRecordInFile + Environment.NewLine +
                           "Records successfully uploaded: " + total_record_updated + Environment.NewLine +
                           "Records Failed: " + record_failed + Environment.NewLine + Environment.NewLine +
                           "Failed Record Codes: " + Failed_records_string + Environment.NewLine + Environment.NewLine +
                           "Uploaded on: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + Environment.NewLine +
                           "Uploaded By: " + userName + Environment.NewLine;
                           ;
                    using (StreamWriter writer = new StreamWriter(filePathOther, true))
                    {
                        writer.WriteLine(textToType);
                    }
                }
                if(!String.IsNullOrWhiteSpace(filePathOther))
                {
                    responseModel.Success = true;
                    responseModel.Message = filePathOther;
                }
                else
                {
                    responseModel.Success = false;
                }
               
                return responseModel;
            }
            catch (Exception exception)
            {
                return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }

        public bool UpdateProviderCLR(FoxProviderClass Provider,long practiceCode)
        {
            if(!String.IsNullOrWhiteSpace(Provider.FOX_PROVIDER_CODE))
            {
                var updateProvider = _FoxProviderClassRepository.GetFirst(t => t.FOX_PROVIDER_CODE == Provider.FOX_PROVIDER_CODE && t.PRACTICE_CODE == practiceCode && t.DELETED == false);
                if(updateProvider != null && updateProvider.FOX_PROVIDER_ID != 0)
                {
                    updateProvider.CLR = Provider.CLR;
                    _FoxProviderClassRepository.Update(updateProvider);
                    _dbContextSettings.SaveChanges();
                    return true;

                }
            }
            return false;
        }
        public bool DeleteClinician(DeleteClinicianModel obj, UserProfile profile)
        {
            CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
            TextInfo text_info = culture_info.TextInfo;
            if (obj != null && obj.user != null)
            {
                var ClinicianData = _FoxProviderClassRepository.GetFirst(x => x.FOX_PROVIDER_ID == obj.user.FOX_PROVIDER_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                if (ClinicianData != null)
                {
                    ClinicianData.DELETED = true;
                    ClinicianData.MODIFIED_BY = profile.UserName;
                    ClinicianData.MODIFIED_DATE = DateTime.Now;
                    _FoxProviderClassRepository.Update(ClinicianData);
                    _dbContextSettings.SaveChanges();
                    try
                    {
                        _FoxProviderClassRepository.Update(ClinicianData);
                        _dbContextSettings.SaveChanges();
                        #region email to Carey on Delete

                        string subject = "Clinician profile deleted (" + ClinicianData.EMAIL + ")";
                        obj.user.FIRST_NAME = string.IsNullOrEmpty(obj.user.FIRST_NAME) ? string.Empty : text_info.ToTitleCase(obj.user.FIRST_NAME);
                        obj.user.LAST_NAME = string.IsNullOrEmpty(obj.user.LAST_NAME) ? string.Empty : text_info.ToTitleCase(obj.user.LAST_NAME);
                        profile.FirstName = string.IsNullOrEmpty(profile.FirstName) ? string.Empty : text_info.ToTitleCase(profile.FirstName);
                        profile.LastName = string.IsNullOrEmpty(profile.LastName) ? string.Empty : text_info.ToTitleCase(profile.LastName);

                        var body = "";
                        body += "<body>";

                        body += "<p style='margin: 0px;'>A Clinician profile was deleted with the following specifics:</p><br />";
                        body += "<table width='500'>";
                        body += "<tr>";
                        body += "<td>Profile deleted: </td>";
                        body += "<td>" + obj.user.FIRST_NAME + " " + obj.user.LAST_NAME + (obj.user.FOX_PROVIDER_CODE != null && obj.user.FOX_PROVIDER_CODE != "" ? " - " + obj.user.FOX_PROVIDER_CODE + ". " : ". ") + obj.user.EMAIL + "</td>";
                        body += "</tr>";

                        body += "<tr>";
                        body += "<td>By: </td>";
                        body += "<td>" + profile.FirstName + " " + profile.LastName + ". " + profile.UserEmailAddress + " </td>";
                        body += "</tr>";

                        body += "<tr>";
                        body += "<td>Date/Time: </td>";
                        body += "<td>" + Helper.GetCurrentDate().ToString("MM / dd / yyyy hh: mm tt") + "</td>";
                        body += "</tr>";

                        body += "<tr>";
                        body += "<td>Reason: </td>";
                        body += "<td>" + obj.reason + "</td>";
                        body += "</tr></table><br /><br />";

                        body += "<p style='margin: 0px;'>Regards,</ p><br />";
                        body += "<p style='margin: 0px;'>CareCloud Support team</ p><br />";
                        body += "</body>";

                        string sendTo = string.Empty;
                        List<string> _ccList = new List<string>();

                        if (profile.PracticeCode == 1012714)
                        {
                            sendTo = "Carey.sambogna@foxrehab.org";
                            _ccList.Add("support@foxrehab.org");
                        }
                        else
                        {
                            sendTo = "adnanshah3@carecloud.com";
                            _ccList.Add("abdulsattar@carecloud.com");
                            _ccList.Add("muhammadarslan3@carecloud.com");
                        }
                        Helper.SendEmail(sendTo, subject, body, null, profile, _ccList);
                        #endregion
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
