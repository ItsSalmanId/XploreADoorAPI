using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;

namespace FOX.BusinessOperations.PatientSurveyService.UploadDataService
{
    public class UploadDataService: IUploadDataService
    {
        private readonly DbContextPatientSurvey _patientSurveyContext = new DbContextPatientSurvey();
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        public UploadDataService()
        {
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_patientSurveyContext);
        }

        public List<PatientSurvey> GetLastUpload(long practiceCode)
        {
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var PatientSurveyList = SpRepository<PatientSurvey>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_LAST_UPLOAD @PRACTICE_CODE", PracticeCode);
            return PatientSurveyList;
        }

        public ResponseModel ReadExcel(string fileName, UserProfile profile)
        {
            try
            {
                DataTable tbl = new DataTable();
                ResponseModel responseModel = new ResponseModel();
                string path = @"~\FoxDocumentDirectory\PatientSurvey\UploadFiles\" + fileName;
                path = HttpContext.Current.Server.MapPath(path);
                string fileType = path.Substring(path.Length - 3);
                long totalRecordInFile = 0;
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "PatientSurveyLogFiles/";
                var exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                var name = fileName == null ? "Temp" : fileName.Substring(0, fileName.LastIndexOf('.'));
                var logfileName = DocumentHelper.GenerateSignatureFileName(name) + ".txt";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                var pathtowriteFile = exportPath + "\\" + logfileName;
                int rowNumToInsert = 1;

                #region .csv File Reader Region

                if (fileType == "csv") // for .csv file
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(path);
                    //Csv reader reads the stream
                    CsvHelper.CsvReader csvread = new CsvHelper.CsvReader(sr);
                    csvread.Configuration.PrepareHeaderForMatch = header => header.Replace(" ", string.Empty);

                    //csvread will fetch all record in one go to the IEnumerable object record
                    IEnumerable<PatientSurveyExcel> record = csvread.GetRecords<PatientSurveyExcel>().ToList();
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
                        var patientSurvey = new PatientSurvey();
                        patientSurvey.FACILITY_OR_CLIENT_ID = rec.FacilityorClientID;
                        if (!string.IsNullOrEmpty(rec.PatientAccountNumber))
                        {
                            long patientAccountNo;
                            if (long.TryParse(rec.PatientAccountNumber, out patientAccountNo))
                                patientSurvey.PATIENT_ACCOUNT_NUMBER = patientAccountNo;
                        }
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyLastName) && rec.ResponsiblePartyLastName.Length <= 50)
                            patientSurvey.RESPONSIBLE_PARTY_LAST_NAME = rec.ResponsiblePartyLastName;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyFirstName) && rec.ResponsiblePartyFirstName.Length <= 50)
                            patientSurvey.RESPONSIBLE_PARTY_FIRST_NAME = rec.ResponsiblePartyFirstName;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyMiddleInitial) && rec.ResponsiblePartyMiddleInitial.Length <= 1)
                            patientSurvey.RESPONSIBLE_PARTY_MIDDLE_INITIAL = rec.ResponsiblePartyMiddleInitial;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyAddress) && rec.ResponsiblePartyAddress.Length <= 500)
                            patientSurvey.RESPONSIBLE_PARTY_ADDRESS = rec.ResponsiblePartyAddress;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyCity) && rec.ResponsiblePartyCity.Length <= 50)
                            patientSurvey.RESPONSIBLE_PARTY_CITY = rec.ResponsiblePartyCity;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyState) && rec.ResponsiblePartyState.Length <= 2)
                            patientSurvey.RESPONSIBLE_PARTY_STATE = rec.ResponsiblePartyState;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyZipCode) && rec.ResponsiblePartyZipCode.Length <= 20)
                            patientSurvey.RESPONSIBLE_PARTY_ZIP_CODE = rec.ResponsiblePartyZipCode;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyTelephone) && rec.ResponsiblePartyTelephone.Length <= 20)
                            patientSurvey.RESPONSIBLE_PARTY_TELEPHONE = rec.ResponsiblePartyTelephone;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartySSN) && rec.ResponsiblePartySSN.Length <= 20)
                            patientSurvey.RESPONSIBLE_PARTY_SSN = rec.ResponsiblePartySSN;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartySex) && rec.ResponsiblePartySex.Length <= 15)
                            patientSurvey.RESPONSIBLE_PARTY_SEX = rec.ResponsiblePartySex;
                        if (!string.IsNullOrEmpty(rec.ResponsiblePartyDateofBirth))
                        {
                            DateTime date;
                            if (DateTime.TryParse(rec.ResponsiblePartyDateofBirth, out date))
                                patientSurvey.RESPONSIBLE_PARTY_DATE_OF_BIRTH = date;
                        }
                        if (!string.IsNullOrEmpty(rec.PatientLastName) && rec.PatientLastName.Length <= 50)
                            patientSurvey.PATIENT_LAST_NAME = rec.PatientLastName;
                        if (!string.IsNullOrEmpty(rec.PatientFirstName) && rec.PatientFirstName.Length <= 50)
                            patientSurvey.PATIENT_FIRST_NAME = rec.PatientFirstName;
                        if (!string.IsNullOrEmpty(rec.PatientMiddleInitial) && rec.PatientMiddleInitial.Length <= 1)
                            patientSurvey.PATIENT_MIDDLE_INITIAL = rec.PatientMiddleInitial;
                        if (!string.IsNullOrEmpty(rec.PatientAddress) && rec.PatientAddress.Length <= 500)
                            patientSurvey.PATIENT_ADDRESS = rec.PatientAddress;
                        if (!string.IsNullOrEmpty(rec.PatientCity) && rec.PatientCity.Length <= 50)
                            patientSurvey.PATIENT_CITY = rec.PatientCity;
                        if (!string.IsNullOrEmpty(rec.PatientState) && rec.PatientState.Length <= 2)
                            patientSurvey.PATIENT_STATE = rec.PatientState;
                        if (!string.IsNullOrEmpty(rec.PatientZipCode) && rec.PatientZipCode.Length <= 20)
                            patientSurvey.PATIENT_ZIP_CODE = rec.PatientZipCode;
                        if (!string.IsNullOrEmpty(rec.PatientTelephoneNumber) && rec.PatientTelephoneNumber.Length <= 20)
                            patientSurvey.PATIENT_TELEPHONE_NUMBER = rec.PatientTelephoneNumber;
                        if (!string.IsNullOrEmpty(rec.PatientSocialSecurityNumber) && rec.PatientSocialSecurityNumber.Length <= 20)
                            patientSurvey.PATIENT_SOCIAL_SECURITY_NUMBER = rec.PatientSocialSecurityNumber;
                        if (!string.IsNullOrEmpty(rec.PatientGender) && rec.PatientGender.Length <= 15)
                            patientSurvey.PATIENT_GENDER = rec.PatientGender;
                        if (!string.IsNullOrEmpty(rec.PatientDateofBirth))
                        {
                            DateTime date;
                            if (DateTime.TryParse(rec.PatientDateofBirth, out date))
                                patientSurvey.PATIENT_DATE_OF_BIRTH = date;
                        }
                        if (!string.IsNullOrEmpty(rec.AlternateContactLastName) && rec.AlternateContactLastName.Length <= 50)
                            patientSurvey.ALTERNATE_CONTACT_LAST_NAME = rec.AlternateContactLastName;
                        if (!string.IsNullOrEmpty(rec.AlternateContactFirstName) && rec.AlternateContactFirstName.Length <= 50)
                            patientSurvey.ALTERNATE_CONTACT_FIRST_NAME = rec.AlternateContactFirstName;
                        if (!string.IsNullOrEmpty(rec.PatientMiddleInitial) && rec.PatientMiddleInitial.Length <= 1)
                            patientSurvey.ALTERNATE_CONTACT_MIDDLE_INITIAL = rec.PatientMiddleInitial;
                        if (!string.IsNullOrEmpty(rec.AlternateContactTelephone) && rec.AlternateContactTelephone.Length <= 10)
                            patientSurvey.ALTERNATE_CONTACT_TELEPHONE = rec.AlternateContactTelephone;
                        if (!string.IsNullOrEmpty(rec.EMRLocationCode) && rec.EMRLocationCode.Length <= 50)
                            patientSurvey.EMR_LOCATION_CODE = rec.EMRLocationCode;
                        if (!string.IsNullOrEmpty(rec.EMRLocationDescription) && rec.EMRLocationDescription.Length <= 200)
                            patientSurvey.EMR_LOCATION_DESCRIPTION = rec.EMRLocationDescription;
                        if (!string.IsNullOrEmpty(rec.ServiceorPaymentDescription) && rec.ServiceorPaymentDescription.Length <= 200)
                            patientSurvey.SERVICE_OR_PAYMENT_DESCRIPTION = rec.ServiceorPaymentDescription;
                        if (!string.IsNullOrEmpty(rec.Provider) && rec.Provider.Length <= 50)
                            patientSurvey.PROVIDER = rec.Provider;
                        if (!string.IsNullOrEmpty(rec.Region) && rec.Region.Length <= 50)
                            patientSurvey.REGION = rec.Region;
                        if (!string.IsNullOrEmpty(rec.LastVisitDate))
                        {
                            DateTime date;
                            if (DateTime.TryParse(rec.LastVisitDate, out date))
                                patientSurvey.LAST_VISIT_DATE = date;
                        }
                        if (!string.IsNullOrEmpty(rec.DischargeDate))
                        {
                            DateTime date;
                            if (DateTime.TryParse(rec.DischargeDate, out date))
                                patientSurvey.DISCHARGE_DATE = date;
                        }
                        if (!string.IsNullOrEmpty(rec.AttendingDoctorName) && rec.AttendingDoctorName.Length <= 50)
                            patientSurvey.ATTENDING_DOCTOR_NAME = rec.AttendingDoctorName;
                        if (!string.IsNullOrEmpty(rec.PTOTSLP) && rec.PTOTSLP.Length <= 10)
                            patientSurvey.PT_OT_SLP = rec.PTOTSLP;
                        if (!string.IsNullOrEmpty(rec.REFERRALDATEthedatethefilewassenttoSHS))
                        {
                            DateTime date;
                            if (DateTime.TryParse(rec.REFERRALDATEthedatethefilewassenttoSHS, out date))
                                patientSurvey.REFERRAL_DATE = date;
                        }
                        if (!string.IsNullOrEmpty(rec.ProcedureCodeTranCode) && rec.ProcedureCodeTranCode.Length <= 50)
                            patientSurvey.PROCEDURE_OR_TRAN_CODE = rec.ProcedureCodeTranCode;
                        if (!string.IsNullOrEmpty(rec.ServiceorPaymentAmount))
                        {
                            Decimal amount;
                            if (Decimal.TryParse(rec.ServiceorPaymentAmount, out amount))
                                patientSurvey.SERVICE_OR_PAYMENT_AMOUNT = amount;
                        }
                        patientSurvey.FILE_NAME = fileName;
                        patientSurvey.TOTAL_RECORD_IN_FILE = totalRecordInFile;
                        rowNumToInsert = rowNumToInsert + 1;
                        patientSurvey.ROW = rowNumToInsert;
                        AddPatientSurvey(patientSurvey, profile.PracticeCode, profile.UserName);
                    }
                    responseModel.FilePath = pathtowriteFile;
                    sr.Close();
                    generate_file(totalRecordInFile, profile, pathtowriteFile);
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
                        var patientSurvey = new PatientSurvey();
                        patientSurvey.FACILITY_OR_CLIENT_ID = row["Facility or Client ID"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient Account Number"].ToString()))
                        {
                            long patientAccountNo;
                            if (long.TryParse(Convert.ToInt64(row["Patient Account Number"]).ToString(), out patientAccountNo))
                                patientSurvey.PATIENT_ACCOUNT_NUMBER = patientAccountNo;
                        }
                        if (!string.IsNullOrEmpty(row["Responsible Party Last Name"].ToString()) && row["Responsible Party Last Name"].ToString().Length <= 50)
                            patientSurvey.RESPONSIBLE_PARTY_LAST_NAME = row["Responsible Party Last Name"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party First Name"].ToString()) && row["Responsible Party First Name"].ToString().Length <= 50)
                            patientSurvey.RESPONSIBLE_PARTY_FIRST_NAME = row["Responsible Party First Name"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party Middle Initial"].ToString()) && row["Responsible Party Middle Initial"].ToString().Length <= 1)
                            patientSurvey.RESPONSIBLE_PARTY_MIDDLE_INITIAL = row["Responsible Party Middle Initial"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party Address"].ToString()) && row["Responsible Party Address"].ToString().Length <= 500)
                            patientSurvey.RESPONSIBLE_PARTY_ADDRESS = row["Responsible Party Address"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party City"].ToString()) && row["Responsible Party City"].ToString().Length <= 50)
                            patientSurvey.RESPONSIBLE_PARTY_CITY = row["Responsible Party City"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party State"].ToString()) && row["Responsible Party State"].ToString().Length <= 2)
                            patientSurvey.RESPONSIBLE_PARTY_STATE = row["Responsible Party State"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party Zip Code"].ToString()) && row["Responsible Party Zip Code"].ToString().Length <= 20)
                            patientSurvey.RESPONSIBLE_PARTY_ZIP_CODE = row["Responsible Party Zip Code"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party Telephone"].ToString()) && row["Responsible Party Telephone"].ToString().Length <= 20)
                            patientSurvey.RESPONSIBLE_PARTY_TELEPHONE = row["Responsible Party Telephone"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party SSN"].ToString()) && row["Responsible Party SSN"].ToString().Length <= 20)
                            patientSurvey.RESPONSIBLE_PARTY_SSN = row["Responsible Party SSN"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party Sex"].ToString()) && row["Responsible Party Sex"].ToString().Length <= 15)
                            patientSurvey.RESPONSIBLE_PARTY_SEX = row["Responsible Party Sex"].ToString();
                        if (!string.IsNullOrEmpty(row["Responsible Party Date of Birth"].ToString()))
                        {
                            DateTime date;
                            if (DateTime.TryParse(row["Responsible Party Date of Birth"].ToString(), out date))
                                patientSurvey.RESPONSIBLE_PARTY_DATE_OF_BIRTH = date;
                        }
                        if (!string.IsNullOrEmpty(row["Patient Last Name"].ToString()) && row["Patient Last Name"].ToString().Length <= 50)
                            patientSurvey.PATIENT_LAST_NAME = row["Patient Last Name"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient First Name"].ToString()) && row["Patient First Name"].ToString().Length <= 50)
                            patientSurvey.PATIENT_FIRST_NAME = row["Patient First Name"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient Middle Initial"].ToString()) && row["Patient Middle Initial"].ToString().Length <= 1)
                            patientSurvey.PATIENT_MIDDLE_INITIAL = row["Patient Middle Initial"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient Address"].ToString()) && row["Patient Address"].ToString().Length <= 500)
                            patientSurvey.PATIENT_ADDRESS = row["Patient Address"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient City"].ToString()) && row["Patient City"].ToString().Length <= 50)
                            patientSurvey.PATIENT_CITY = row["Patient City"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient State"].ToString()) && row["Patient State"].ToString().Length <= 2)
                            patientSurvey.PATIENT_STATE = row["Patient State"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient Zip Code"].ToString()) && row["Patient Zip Code"].ToString().Length <= 20)
                            patientSurvey.PATIENT_ZIP_CODE = row["Patient Zip Code"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient Telephone Number"].ToString()) && row["Patient Telephone Number"].ToString().Length <= 20)
                            patientSurvey.PATIENT_TELEPHONE_NUMBER = row["Patient Telephone Number"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient Social Security Number"].ToString()) && row["Patient Social Security Number"].ToString().Length <= 20)
                            patientSurvey.PATIENT_SOCIAL_SECURITY_NUMBER = row["Patient Social Security Number"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient Gender"].ToString()) && row["Patient Gender"].ToString().Length <= 15)
                            patientSurvey.PATIENT_GENDER = row["Patient Gender"].ToString();
                        if (!string.IsNullOrEmpty(row["Patient Date of Birth"].ToString()))
                        {
                            DateTime date;
                            if (DateTime.TryParse(row["Patient Date of Birth"].ToString(), out date))
                                patientSurvey.PATIENT_DATE_OF_BIRTH = date;
                        }
                        if (!string.IsNullOrEmpty(row["Alternate Contact Last Name"].ToString()) && row["Alternate Contact Last Name"].ToString().Length <= 50)
                            patientSurvey.ALTERNATE_CONTACT_LAST_NAME = row["Alternate Contact Last Name"].ToString();
                        if (!string.IsNullOrEmpty(row["Alternate Contact First Name"].ToString()) && row["Alternate Contact First Name"].ToString().Length <= 50)
                            patientSurvey.ALTERNATE_CONTACT_FIRST_NAME = row["Alternate Contact First Name"].ToString();
                        if (!string.IsNullOrEmpty(row["Alternate Contact Middle Initial"].ToString()) && row["Alternate Contact Middle Initial"].ToString().Length <= 1)
                            patientSurvey.ALTERNATE_CONTACT_MIDDLE_INITIAL = row["Alternate Contact Middle Initial"].ToString();
                        if (!string.IsNullOrEmpty(row["Alternate Contact Telephone"].ToString()) && row["Alternate Contact Telephone"].ToString().Length <= 10)
                            patientSurvey.ALTERNATE_CONTACT_TELEPHONE = row["Alternate Contact Telephone"].ToString();
                        if (!string.IsNullOrEmpty(row["EMR Location Code"].ToString()) && row["EMR Location Code"].ToString().Length <= 50)
                            patientSurvey.EMR_LOCATION_CODE = row["EMR Location Code"].ToString();
                        if (!string.IsNullOrEmpty(row["EMR Location Description"].ToString()) && row["EMR Location Description"].ToString().Length <= 200)
                            patientSurvey.EMR_LOCATION_DESCRIPTION = row["EMR Location Description"].ToString();
                        if (!string.IsNullOrEmpty(row["Service or Payment Description"].ToString()) && row["Service or Payment Description"].ToString().Length <= 200)
                            patientSurvey.SERVICE_OR_PAYMENT_DESCRIPTION = row["Service or Payment Description"].ToString();
                        if (!string.IsNullOrEmpty(row["Provider"].ToString()) && row["Provider"].ToString().Length <= 50)
                            patientSurvey.PROVIDER = row["Provider"].ToString();
                        if (!string.IsNullOrEmpty(row["Region"].ToString()) && row["Region"].ToString().Length <= 50)
                            patientSurvey.REGION = row["Region"].ToString();
                        if (!string.IsNullOrEmpty(row["Last Visit Date"].ToString()))
                        {
                            DateTime date;
                            if (DateTime.TryParse(row["Last Visit Date"].ToString(), out date))
                                patientSurvey.LAST_VISIT_DATE = date;
                        }
                        if (!string.IsNullOrEmpty(row["Discharge Date"].ToString()))
                        {
                            DateTime date;
                            if (DateTime.TryParse(row["Discharge Date"].ToString(), out date))
                                patientSurvey.DISCHARGE_DATE = date;
                        }
                        if (!string.IsNullOrEmpty(row["Attending Doctor Name"].ToString()) && row["Attending Doctor Name"].ToString().Length <= 50)
                            patientSurvey.ATTENDING_DOCTOR_NAME = row["Attending Doctor Name"].ToString();
                        if (!string.IsNullOrEmpty(row["PTOTSLP"].ToString()) && row["PTOTSLP"].ToString().Length <= 10)
                            patientSurvey.PT_OT_SLP = row["PTOTSLP"].ToString();
                        if (!string.IsNullOrEmpty(row["REFERRAL DATE the date the file was sent to SHS"].ToString()))
                        {
                            DateTime date;
                            if (DateTime.TryParse(row["REFERRAL DATE the date the file was sent to SHS"].ToString(), out date))
                                patientSurvey.REFERRAL_DATE = date;
                        }
                        if (!string.IsNullOrEmpty(row["Procedure Code Tran Code"].ToString()) && row["Procedure Code Tran Code"].ToString().Length <= 50)
                            patientSurvey.PROCEDURE_OR_TRAN_CODE = row["Procedure Code Tran Code"].ToString();
                        decimal amount;
                        if (Decimal.TryParse(row["Service or Payment Amount "].ToString(), out amount))
                            patientSurvey.SERVICE_OR_PAYMENT_AMOUNT = amount;
                        patientSurvey.FILE_NAME = fileName;
                        patientSurvey.TOTAL_RECORD_IN_FILE = totalRecordInFile;
                        rowNumToInsert = rowNumToInsert + 1;
                        patientSurvey.ROW = rowNumToInsert;
                        AddPatientSurvey(patientSurvey, profile.PracticeCode, profile.UserName);
                    }
                    responseModel.FilePath = pathtowriteFile;
                    generate_file(totalRecordInFile, profile, pathtowriteFile);
                }

                #endregion

                return responseModel;
            }
            catch (Exception exception)
            {
                return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }
        List<PatientSurvey> failed_details = new List<PatientSurvey>();

        private void AddPatientSurvey(PatientSurvey patientSurvey, long practiceCode, string userName)
        {
            var dbPatientSurvey = _patientSurveyRepository.GetMany(x => x.PATIENT_ACCOUNT_NUMBER == patientSurvey.PATIENT_ACCOUNT_NUMBER);
            if (dbPatientSurvey.Count > 0) //add
            {
                bool isAlreadyExist = false;
                foreach (var patient in dbPatientSurvey)
                {
                    if (patient.PATIENT_ACCOUNT_NUMBER == patientSurvey.PATIENT_ACCOUNT_NUMBER &&
                        patient.PT_OT_SLP == patientSurvey.PT_OT_SLP && patient.PROVIDER == patientSurvey.PROVIDER &&
                        patient.LAST_VISIT_DATE == patientSurvey.LAST_VISIT_DATE)
                    {
                        isAlreadyExist = true;
                        failed_details.Add(patientSurvey);
                    }
                }
                if (!isAlreadyExist)
                {
                    patientSurvey.SURVEY_ID = Convert.ToInt64(practiceCode.ToString() + Helper.getMaximumId("SURVEY_ID").ToString());
                    patientSurvey.PRACTICE_CODE = practiceCode;
                    patientSurvey.CREATED_BY = patientSurvey.MODIFIED_BY = userName;
                    patientSurvey.CREATED_DATE = patientSurvey.MODIFIED_DATE = Helper.GetCurrentDate();
                    patientSurvey.DELETED = false;
                    patientSurvey.IS_SURVEYED = false;
                    patientSurvey.IS_IMPROVED_SETISFACTION = null;
                    patientSurvey.IS_REFERABLE = null;
                    patientSurvey.IS_CONTACT_HQ = null;
                    patientSurvey.IS_RESPONSED_BY_HQ = null;
                    _patientSurveyRepository.Insert(patientSurvey);
                    _patientSurveyRepository.Save();
                }
            }
            else
            {
                patientSurvey.SURVEY_ID = Convert.ToInt64(practiceCode.ToString() + Helper.getMaximumId("SURVEY_ID").ToString());
                patientSurvey.PRACTICE_CODE = practiceCode;
                patientSurvey.CREATED_BY = patientSurvey.MODIFIED_BY = userName;
                patientSurvey.CREATED_DATE = patientSurvey.MODIFIED_DATE = Helper.GetCurrentDate();
                patientSurvey.DELETED = false;
                patientSurvey.IS_SURVEYED = false;
                patientSurvey.IS_IMPROVED_SETISFACTION = null;
                patientSurvey.IS_REFERABLE = null;
                patientSurvey.IS_CONTACT_HQ = null;
                patientSurvey.IS_RESPONSED_BY_HQ = null;
                _patientSurveyRepository.Insert(patientSurvey);
                _patientSurveyRepository.Save();
            }
        }
        private void generate_file(long totalRecordInFile, UserProfile profile, string pathtowriteFile)
        {
            using (StreamWriter writer = new StreamWriter(pathtowriteFile, true))
            {

                writer.WriteLine("Patient Survey Upload Data - Log Report: " + Environment.NewLine + "----------------------------------------" + Environment.NewLine + "Total Records in File: " + totalRecordInFile + Environment.NewLine + "Records Successfully Uploaded: " + (totalRecordInFile - failed_details.Count()) + "" + Environment.NewLine +
                "Records Failed Due to Duplication Check: " + +(failed_details.Count()) + Environment.NewLine);
                writer.WriteLine("Detail of Failed Records: " + Environment.NewLine + "-------------------------" + Environment.NewLine);
                writer.WriteLine("Excel Row #" + " \t" + "Patient Account Number" + " \t" + "  Responsible Party Last Name" + " \t" + "  Responsible Party First Name" + " \t" 
                    + "  Service or Payment Description" + " \t" + "   Provider  " + " \t" + " \t" + " \t" + " \t" + "   Region" + " \t" + " \t"   + " \t" + " \t" +  "Last Visit Date" + Environment.NewLine);
                foreach (var Data in failed_details)
                {
                    writer.WriteLine(String.Format("{0,-15}  {1,-24}  {2,-30} {3,-31} {4,-40} {5, -39} {6,-36} {7,-33}", Data.ROW, Data.PATIENT_ACCOUNT_NUMBER, Data.PATIENT_FIRST_NAME, Data.PATIENT_LAST_NAME,Data.SERVICE_OR_PAYMENT_DESCRIPTION, Data.PROVIDER,Data.REGION,Data.LAST_VISIT_DATE));
                }

                writer.WriteLine(Environment.NewLine + "Uploaded On: " + Helper.GetCurrentDate() + Environment.NewLine + "Uploaded by: " + profile.UserName);
            }
        }
    }
}