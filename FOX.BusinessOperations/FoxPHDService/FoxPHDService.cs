using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.FoxPHD;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientDocuments;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace FOX.BusinessOperations.FoxPHDService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FoxPHDService" in both code and config file together.
    public class FoxPHDService : IFoxPHDService
    {
        private readonly DBContextFoxPHD _DBContextFoxPHD = new DBContextFoxPHD();
        private readonly DbContextPatient _PatientContext = new DbContextPatient();
        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly DBContextPatientDocuments _patientDocument = new DBContextPatientDocuments();
        private readonly DBContextPatientDocuments _PatientPATDocumentContext = new DBContextPatientDocuments();
        private readonly GenericRepository<Patient> _PatientRepository;
        private readonly GenericRepository<PHDCallDetail> _PHDDetailRepository;
        private readonly GenericRepository<PatientAddress> _PatientAddressRepository;
        private readonly GenericRepository<PhdCallScenario> _PhdCallScenarioRepository;
        private readonly GenericRepository<PhdCallReason> _PhdCallReasonRepository;
        private readonly GenericRepository<CS_Case_Categories> _caseCategoriesRepository;
        private readonly GenericRepository<PhdCallLogHistory> _phdCallLogHistoryRepository;
        private readonly GenericRepository<PhdCallRequest> _PhdCallRequestRepository;
        private readonly GenericRepository<PhdPatientVerification> _PhdPatientVerificationRepository;
        private readonly GenericRepository<User> _userRepository;
        private readonly GenericRepository<FOX_TBL_GENERAL_NOTE> _generalNotesRepository;
        private readonly GenericRepository<InterfaceSynchModel> __InterfaceSynchModelRepository;
        private readonly GenericRepository<PHDUnmappedCalls> _PHDUnmappedCallsRepository;
        private readonly GenericRepository<FoxDocumentType> _foxDocumentTypeRepository;
        private readonly GenericRepository<PatientPATDocument> _foxPatientPATdocumentRepository;
        private readonly GenericRepository<PatientDocumentFiles> _foxPatientdocumentFilesRepository;
        private readonly GenericRepository<DefaultVauesForPhdUsers> _DefaultVauesForPhdUsersRepository;
        private readonly GenericRepository<PhdFaqsDetail> _phdFaqsDetailRepository;
        public FoxPHDService()
        {
            _PatientRepository = new GenericRepository<Patient>(_PatientContext);
            _PatientAddressRepository = new GenericRepository<PatientAddress>(_PatientContext);
            _PHDDetailRepository = new GenericRepository<PHDCallDetail>(_DBContextFoxPHD);
            _PhdCallScenarioRepository = new GenericRepository<PhdCallScenario>(_DBContextFoxPHD);
            _PhdCallReasonRepository = new GenericRepository<PhdCallReason>(_DBContextFoxPHD);
            _PhdCallRequestRepository = new GenericRepository<PhdCallRequest>(_DBContextFoxPHD);
            _caseCategoriesRepository = new GenericRepository<CS_Case_Categories>(_DBContextFoxPHD);
            _phdCallLogHistoryRepository = new GenericRepository<PhdCallLogHistory>(_DBContextFoxPHD);
            _PhdPatientVerificationRepository = new GenericRepository<PhdPatientVerification>(_DBContextFoxPHD);
            _userRepository = new GenericRepository<User>(_DBContextFoxPHD);
            _generalNotesRepository = new GenericRepository<FOX_TBL_GENERAL_NOTE>(_PatientContext);
            __InterfaceSynchModelRepository = new GenericRepository<InterfaceSynchModel>(_CaseContext);
            _PHDUnmappedCallsRepository = new GenericRepository<PHDUnmappedCalls>(_DBContextFoxPHD);
            _foxDocumentTypeRepository = new GenericRepository<FoxDocumentType>(_patientDocument);
            _foxPatientPATdocumentRepository = new GenericRepository<PatientPATDocument>(_PatientPATDocumentContext);
            _foxPatientdocumentFilesRepository = new GenericRepository<PatientDocumentFiles>(_PatientPATDocumentContext);
            _DefaultVauesForPhdUsersRepository = new GenericRepository<DefaultVauesForPhdUsers>(_DBContextFoxPHD);
            _phdFaqsDetailRepository = new GenericRepository<PhdFaqsDetail>(_DBContextFoxPHD);
        }

        public DropdownLists GetDropdownLists(UserProfile profile)
        {
            try
            {
                DropdownLists ObjDropdownLists = new DropdownLists();
                //if (profile.isTalkRehab)
                //{
                //    ObjDropdownLists.PhdCallScenarios = _PhdCallScenarioRepository.GetMany(s => s.DELETED == false).OrderBy(o => o.NAME).ToList();
                //    ObjDropdownLists.PhdCallReasons = _PhdCallReasonRepository.GetMany(s => s.DELETED == false).OrderBy(o => o.NAME).ToList();
                //    ObjDropdownLists.PhdCallRequests = _PhdCallRequestRepository.GetMany(s => s.DELETED == false).OrderBy(o => o.NAME).ToList();
                //    ObjDropdownLists.CSCaseCategories = _caseCategoriesRepository.GetMany(s => s.CS_Deleted == false).OrderBy(o => o.CS_Category_Name).ToList();
                //    ObjDropdownLists.foxApplicationUsersViewModel = GetPHDCallerDropDownValue(profile);
                //}
                //else
                //{
                    ObjDropdownLists.PhdCallScenarios = _PhdCallScenarioRepository.GetMany(s => s.PRACTICE_CODE == profile.PracticeCode && s.DELETED == false).OrderBy(o => o.NAME).ToList();
                    ObjDropdownLists.PhdCallReasons = _PhdCallReasonRepository.GetMany(s => s.PRACTICE_CODE == profile.PracticeCode && s.DELETED == false).OrderBy(o => o.NAME).ToList();
                    ObjDropdownLists.PhdCallRequests = _PhdCallRequestRepository.GetMany(s => s.PRACTICE_CODE == profile.PracticeCode && s.DELETED == false).OrderBy(o => o.NAME).ToList();
                    ObjDropdownLists.CSCaseCategories = _caseCategoriesRepository.GetMany(s => s.CS_Deleted == false).OrderBy(o => o.CS_Category_Name).ToList();
                    ObjDropdownLists.foxApplicationUsersViewModel = GetPHDCallerDropDownValue(profile);
                //}
                //ObjDropdownLists.foxApplicationUsersViewModel = _userRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode).Select(t => new FoxApplicationUsersViewModel()
                //{string ExportToExcelSourceOfReferral(

                //    FIRST_NAME = t.FIRST_NAME,
                //    LAST_NAME = t.LAST_NAME,
                //    PRACTICE_CODE = t.PRACTICE_CODE,
                //    USER_ID = t.USER_ID,
                //    USER_NAME = t.USER_NAME
                //}).OrderBy(o => o.LAST_NAME).ToList();
                //ObjDropdownLists.foxApplicationUsersViewModel[0].CURRENT_USER_ID = profile.userID.ToString();
                //ObjDropdownLists.foxApplicationUsersViewModel[0].CURRENT_USER_NAME = profile.UserName.ToString();
                return ObjDropdownLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<Patient> GetPatientInformation(PatientsSearchRequest ObjPatientSearchRequest, UserProfile profile)
        {
            try
            {
                if (!string.IsNullOrEmpty(ObjPatientSearchRequest.DATE_OF_BIRTH_STR))
                {
                    ObjPatientSearchRequest.DATE_OF_BIRTH = Convert.ToDateTime(ObjPatientSearchRequest.DATE_OF_BIRTH_STR);
                }
                var AccountNo = Helper.getDBNullOrValue("Patient_Account", ObjPatientSearchRequest.PATIENT_ACCOUNT);
                var MRN = Helper.getDBNullOrValue("CHART_ID", ObjPatientSearchRequest.MRN);
                var LastName = Helper.getDBNullOrValue("LAST_NAME", ObjPatientSearchRequest.LAST_NAME.Trim());
                var FirstName = Helper.getDBNullOrValue("FIRST_NAME", ObjPatientSearchRequest.FIRST_NAME.Trim());
                var SSN = Helper.getDBNullOrValue("SSN", ObjPatientSearchRequest.SSN);
                var dob = Helper.getDBNullOrValue("DOB", ObjPatientSearchRequest.DATE_OF_BIRTH.HasValue ? ObjPatientSearchRequest.DATE_OF_BIRTH.Value.ToString("MM/dd/yyyy") : "");
                var HomePhone = Helper.getDBNullOrValue("HOME_PHONE", ObjPatientSearchRequest.HOME_PHONE);
                var WorkPhone = Helper.getDBNullOrValue("WORK_PHONE", ObjPatientSearchRequest.WORK_PHONE);
                var CellPhone = Helper.getDBNullOrValue("CELL_PHONE", ObjPatientSearchRequest.CELL_PHONE);
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = ObjPatientSearchRequest.CURRENT_PAGE };
                var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = ObjPatientSearchRequest.RECORD_PER_PAGE };
                var SortBy = Helper.getDBNullOrValue("SORT_BY", ObjPatientSearchRequest.SORT_BY);
                var SortOrder = Helper.getDBNullOrValue("SORT_ORDER", ObjPatientSearchRequest.SORT_ORDER);
                var PatientsInfoList = SpRepository<Patient>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PHD_PATIENT_LIST @PATIENT_ACCOUNT,@CHART_ID, @LAST_NAME, @FIRST_NAME, @SSN,@DOB, @HOME_PHONE, @WORK_PHONE, @CELL_PHONE, @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                   AccountNo, MRN, LastName, FirstName, SSN, dob, HomePhone, WorkPhone, CellPhone, PracticeCode, CurrentPage, RecordPerPage, SortBy, SortOrder);
                foreach (var a in PatientsInfoList)
                {
                    var PatientAddress = _PatientAddressRepository.GetFirst(r => r.PATIENT_ACCOUNT == a.Patient_Account && r.DELETED == false && r.ADDRESS_TYPE.ToLower() == "home address");
                    if (PatientAddress != null)
                    {
                        a.Address = PatientAddress.ADDRESS;
                    }
                    var PhdPatientVerificationInfo = _PhdPatientVerificationRepository.GetFirst(s => s.PATIENT_ACCOUNT == a.Patient_Account && s.DELETED == false);
                    if (PhdPatientVerificationInfo != null)
                    {
                        a.PhdpatientverificationObj = PhdPatientVerificationInfo;
                    }
                    a.USER_NAME = profile.UserName;
                    a.USER_ID = profile.userID;
                }
                return PatientsInfoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ResponseModel DeleteCallDetailRecordInformation(PHDCallDetail ObjPHDCallDetailRequest, UserProfile profile)
        {
            try
            {
                var ExistingDetailInfo = _PHDDetailRepository.GetFirst(r => r.FOX_PHD_CALL_DETAILS_ID == ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID && r.DELETED == false);
                if (ExistingDetailInfo != null)
                {
                    ExistingDetailInfo.MODIFIED_BY = profile.UserName;
                    ExistingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                    ExistingDetailInfo.DELETED = true;
                    _PHDDetailRepository.Update(ExistingDetailInfo);
                    _PHDDetailRepository.Save();
                }
                ResponseModel response = new ResponseModel()
                {
                    ErrorMessage = "",
                    Message = "Deleted",
                    Success = true
                };
                return response;
            }
            catch (Exception)
            {
                ResponseModel response = new ResponseModel()
                {
                    ErrorMessage = "",
                    Message = "Not Deleted",
                    Success = false
                };
                return response;
            }
        }
        public List<PHDCallDetail> GetPHDCallDetailsInformation(CallDetailsSearchRequest ObjCallDetailsSearchRequest, UserProfile profile)
        {
            try
            {
             

                if (!string.IsNullOrEmpty(ObjCallDetailsSearchRequest.CALL_DATE_FROM_STR))
                {
                    ObjCallDetailsSearchRequest.CALL_DATE_FROM = Convert.ToDateTime(ObjCallDetailsSearchRequest.CALL_DATE_FROM_STR);
                }
                else
                {
                    ObjCallDetailsSearchRequest.CALL_DATE_FROM = null;
                }
                if (!string.IsNullOrEmpty(ObjCallDetailsSearchRequest.CALL_DATE_TO_STR))
                {
                    ObjCallDetailsSearchRequest.CALL_DATE_TO = Convert.ToDateTime(ObjCallDetailsSearchRequest.CALL_DATE_TO_STR);
                }
                else
                {
                    ObjCallDetailsSearchRequest.CALL_DATE_TO = null;
                }

                //Modified BY Aftab 

                if (!string.IsNullOrEmpty(ObjCallDetailsSearchRequest.CALL_TIME_FROM_STR))
                {
                    var timestr = ObjCallDetailsSearchRequest.CALL_TIME_FROM_STR.Split(' ')[0];
                    ObjCallDetailsSearchRequest.CALL_TIME_FROM = DateTime.ParseExact(timestr, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);

                    ObjCallDetailsSearchRequest.CALL_DATE_FROM = ObjCallDetailsSearchRequest.CALL_DATE_FROM + ObjCallDetailsSearchRequest.CALL_TIME_FROM.Value.TimeOfDay;
                }

                if (!string.IsNullOrEmpty(ObjCallDetailsSearchRequest.CALL_TIME_TO_STR))
                {
                    var timestr = ObjCallDetailsSearchRequest.CALL_TIME_TO_STR.Split(' ')[0];
                    ObjCallDetailsSearchRequest.CALL_TIME_TO = DateTime.ParseExact(timestr, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);

                    ObjCallDetailsSearchRequest.CALL_DATE_TO = ObjCallDetailsSearchRequest.CALL_DATE_TO + ObjCallDetailsSearchRequest.CALL_TIME_TO.Value.TimeOfDay;
                }

                //Close Modification


                if (ObjCallDetailsSearchRequest.CALL_DATE_FROM.HasValue)
                    if (ObjCallDetailsSearchRequest.CALL_DATE_TO.HasValue)
                        if (String.Equals(ObjCallDetailsSearchRequest.CALL_DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                            ObjCallDetailsSearchRequest.CALL_DATE_TO = ObjCallDetailsSearchRequest.CALL_DATE_TO.Value.AddDays(1).AddSeconds(-1);
                        else
                            ObjCallDetailsSearchRequest.CALL_DATE_TO = ObjCallDetailsSearchRequest.CALL_DATE_TO.Value.AddSeconds(59);
                    else
                        ObjCallDetailsSearchRequest.CALL_DATE_TO = Helper.GetCurrentDate();
                else if (ObjCallDetailsSearchRequest.CALL_DATE_TO.HasValue)
                {
                    if (String.Equals(ObjCallDetailsSearchRequest.CALL_DATE_TO.Value.ToString("t"), "12:00 AM", StringComparison.Ordinal))
                        ObjCallDetailsSearchRequest.CALL_DATE_TO = ObjCallDetailsSearchRequest.CALL_DATE_TO.Value.AddDays(1).AddSeconds(-1);
                    var dateNow = Helper.GetCurrentDate();
                    ObjCallDetailsSearchRequest.CALL_DATE_FROM = dateNow.AddYears(-100);
                }
                var CallDateFrom = Helper.getDBNullOrValue("@CALL_DATE_FROM", ObjCallDetailsSearchRequest.CALL_DATE_FROM.ToString());
                var CallDateTo = Helper.getDBNullOrValue("@CALL_DATE_TO", ObjCallDetailsSearchRequest.CALL_DATE_TO.ToString());
                var CallAttendedBy = Helper.getDBNullOrValue("@CALL_ATTENDED_BY", ObjCallDetailsSearchRequest.CALL_ATTENDED_BY.Trim());
                var CallReason = Helper.getDBNullOrValue("@CALL_REASON", ObjCallDetailsSearchRequest.CALL_REASON.Trim());
                var CallHandling = Helper.getDBNullOrValue("@CALL_HANDLING", ObjCallDetailsSearchRequest.CALL_HANDLING.Trim());
                var CsCaseStatus = Helper.getDBNullOrValue("@CS_CASE_STATUS", ObjCallDetailsSearchRequest.CS_CASE_STATUS.Trim());
                var followUpCalls = Helper.getDBNullOrValue("@FOLLOW_UP_CALLS", ObjCallDetailsSearchRequest.FOLLOW_UP_CALLS.ToString());
                var MRN = Helper.getDBNullOrValue("@CHART_ID", ObjCallDetailsSearchRequest.MRN);
                var Patientfirstname = Helper.getDBNullOrValue("@PATIENT_FIRST_NAME", ObjCallDetailsSearchRequest.PATIENT_FIRST_NAME);
                var Patientlastname = Helper.getDBNullOrValue("@PATIENT_LAST_NAME", ObjCallDetailsSearchRequest.PATIENT_LAST_NAME);
                var PhoneNumber = Helper.getDBNullOrValue("@PHONE_NUMBER", ObjCallDetailsSearchRequest.PHONE_NUMBER.ToString());
                var PracticeCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var SearchText = new SqlParameter { ParameterName = "@SEARCH_TEXT", SqlDbType = SqlDbType.VarChar, Value = string.IsNullOrEmpty(ObjCallDetailsSearchRequest.SEARCH_TEXT) ? "" : ObjCallDetailsSearchRequest.SEARCH_TEXT };
                var CurrentPage = new SqlParameter { ParameterName = "@CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = ObjCallDetailsSearchRequest.CURRENT_PAGE };
                var RecordPerPage = new SqlParameter { ParameterName = "@RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = ObjCallDetailsSearchRequest.RECORD_PER_PAGE };
                var SortBy = Helper.getDBNullOrValue("@SORT_BY", ObjCallDetailsSearchRequest.SORT_BY);
                var SortOrder = Helper.getDBNullOrValue("@SORT_ORDER", ObjCallDetailsSearchRequest.SORT_ORDER);
                var PHDDetailsList = SpRepository<PHDCallDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PHD_CALL_DETAILS @CALL_DATE_FROM, @CALL_DATE_TO, @CALL_ATTENDED_BY, @CALL_REASON, @CALL_HANDLING, @CS_CASE_STATUS, @FOLLOW_UP_CALLS, @CHART_ID, @PATIENT_FIRST_NAME, @PATIENT_LAST_NAME, @PHONE_NUMBER, @PRACTICE_CODE, @SEARCH_TEXT, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                   CallDateFrom, CallDateTo, CallAttendedBy, CallReason, CallHandling, CsCaseStatus, followUpCalls, MRN, Patientfirstname, Patientlastname, PhoneNumber, PracticeCode, SearchText, CurrentPage, RecordPerPage, SortBy, SortOrder);


                foreach (var item in PHDDetailsList)
                {
                    List<PHDUnmappedCalls> returList = new List<PHDUnmappedCalls>();
                    UnmappedCallsSearchRequest req = new UnmappedCallsSearchRequest();
                    if (item.CALL_RECORDING_PATH == "" || item.CALL_RECORDING_PATH == null)
                    {
                        req.CALL_DATE = item.CALL_DATE;
                        req.CALL_DATE_STR = item.CALL_DATE_STR;
                        req.CALL_NO = item.INCOMING_CALL_NO;
                        returList = GetUnmappedCalls(req, profile);
                        if (returList != null && returList.Count > 0)
                        {
                            item.IsRecordingMapped = true;
                        }
                        else
                        {
                            item.IsRecordingMapped = false;
                        }
                    }
                }
                return PHDDetailsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// This function is return the PDF Page Number Count
        /// </summary>
        /// <param name="PdfPath"></param>
        /// <returns></returns>
        private int getNumberOfPagesOfPDF(string PdfPath)
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(PdfPath);
            return pdfReader.NumberOfPages;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjPHDCallDetailRequest"></param>
        /// <param name="PdfPath"></param>
        /// <param name="config"></param>
        /// <param name="noOfPages"></param>
        /// <param name="sorcetype"></param>
        /// <param name="Profile"></param>
        /// <param name="patient_account_str"></param>
        /// <param name="NewDocument"></param>
        /// <param name="ticks"></param>
        public void SavePdftoImagesEligibilty(PHDCallDetail ObjPHDCallDetailRequest, string PdfPath, ServiceConfiguration config, int noOfPages, string sorcetype, UserProfile Profile, string patient_account_str, bool NewDocument, long ticks)
        {
            if (!Directory.Exists(config.IMAGES_PATH_SERVER))
            {
                Directory.CreateDirectory(config.IMAGES_PATH_SERVER);
            }
            if (System.IO.File.Exists(PdfPath))
            {
                PatientPATDocument ObjPatientPATDocument = new PatientPATDocument();
                ObjPatientPATDocument.PATIENT_ACCOUNT_str = patient_account_str;
                ObjPatientPATDocument.PATIENT_ACCOUNT = Convert.ToInt64(patient_account_str);
                ObjPatientPATDocument.PRACTICE_CODE = Profile.PracticeCode;
                ObjPatientPATDocument.FOX_PHD_CALL_DETAILS_ID = ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID;

                ObjPatientPATDocument.DOCUMENT_PATH_LIST = new List<PatientDocumentFiles>();
                var document_type = _foxDocumentTypeRepository.GetFirst(d => !d.DELETED && d.DOCUMENT_TYPE_ID.ToString() == ObjPHDCallDetailRequest.DOCUMENT_TYPE).DOCUMENT_TYPE_ID;
                ObjPatientPATDocument.DOCUMENT_TYPE = document_type;
                ObjPatientPATDocument.COMMENTS = "Created_by: " + Profile.UserName + "\n Dated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                for (int i = 0; i < noOfPages; i++)
                {
                    string deliveryReportId = "";
                    System.Drawing.Image img;
                    PdfFocus pdfFocus = new PdfFocus();
                    pdfFocus.Serial = "10261435399";
                    pdfFocus.OpenPdf(PdfPath);
                    if (pdfFocus.PageCount > 0)
                    {
                        //Save all PDF pages to jpeg images
                        pdfFocus.ImageOptions.Dpi = 120;
                        pdfFocus.ImageOptions.ImageFormat = ImageFormat.Jpeg;
                        var image = pdfFocus.ToImage(i + 1);
                        //Next manipulate with Jpeg in memory or save to HDD, open in a viewer
                        using (var memoryStream = new MemoryStream(image))
                        {
                            if (sorcetype?.Split(':')?[0] == "DR")
                            {
                                //deliveryReportId = workId + DateTime.Now.Ticks;
                                using (img = System.Drawing.Image.FromStream(memoryStream))
                                {
                                    img.Save(config.IMAGES_PATH_SERVER + "\\" + deliveryReportId + "_" + i + ".jpg", ImageFormat.Jpeg);
                                    Bitmap bitmap = new Bitmap(img);
                                    ConvertPDFToImages convertPDFToImage = new ConvertPDFToImages();
                                    img.Dispose();
                                    convertPDFToImage.SaveWithNewDimention(bitmap, 115, 150, 100, config.IMAGES_PATH_SERVER + "\\Logo_" + deliveryReportId + "_" + i + ".jpg");
                                    bitmap.Dispose();
                                }
                            }
                            else
                            {
                                using (img = System.Drawing.Image.FromStream(memoryStream))
                                {
                                    img.Save(config.IMAGES_PATH_SERVER + "\\" + patient_account_str + "_" + ticks + "_" + i + ".jpg", ImageFormat.Jpeg);
                                    Bitmap bitmap = new Bitmap(img);
                                    ConvertPDFToImages convertPDFToImage = new ConvertPDFToImages();
                                    img.Dispose();
                                    convertPDFToImage.SaveWithNewDimention(bitmap, 115, 150, 100, config.IMAGES_PATH_SERVER + "\\Logo" + "_" + patient_account_str + "_" + ticks + "_" + i + ".jpg");
                                    bitmap.Dispose();
                                }
                            }
                        }
                    }
                    var imgPath = "";
                    var logoImgPath = "";
                    if (sorcetype?.Split(':')?[0] == "DR")
                    {
                        imgPath = config.IMAGES_PATH_DB + "\\" + deliveryReportId + "_" + i + ".jpg";
                        logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + deliveryReportId + "_" + i + ".jpg";
                    }
                    else
                    {
                        imgPath = config.IMAGES_PATH_DB + "\\" + patient_account_str + "_" + ticks + "_" + i + ".jpg";
                        logoImgPath = config.IMAGES_PATH_DB + "\\Logo" + "_" + patient_account_str + "_" + ticks + "_" + i + ".jpg";
                    }
                    PatientDocumentFiles path = new PatientDocumentFiles();
                    path.DOCUMENT_PATH = imgPath;

                    ObjPatientPATDocument.DOCUMENT_PATH_LIST.Add(path);

                }
                AddUpdateNewDocumentInformation(ObjPatientPATDocument, Profile, NewDocument);
            }
        }
        public ResponseModel AddUpdateNewDocumentInformation(PatientPATDocument objPatientPATDocument, UserProfile profile, bool newDocument)
        {

            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            PatientPATDocument ExistingDocumentInfo = new PatientPATDocument();
            string AddorUpdate = "";
            if (objPatientPATDocument.WORK_ID == 0)
            {
                objPatientPATDocument.WORK_ID = null;
            }
            if (objPatientPATDocument != null)
            {
                var PatientAccount = Convert.ToInt64(objPatientPATDocument.PATIENT_ACCOUNT_str);
                interfaceSynch.PATIENT_ACCOUNT = PatientAccount;
                if (objPatientPATDocument.WORK_ID != null)
                {
                    ExistingDocumentInfo = _foxPatientPATdocumentRepository.GetFirst(r => r.PAT_DOCUMENT_ID == objPatientPATDocument.PAT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.WORK_ID == objPatientPATDocument.WORK_ID && r.DELETED == false);
                }
                else
                {
                    ExistingDocumentInfo = _foxPatientPATdocumentRepository.GetFirst(r => r.PAT_DOCUMENT_ID == objPatientPATDocument.PAT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.DELETED == false);
                }
                if (ExistingDocumentInfo == null)   //Add
                {
                    if (objPatientPATDocument.WORK_ID != null)
                    {
                        if (!string.IsNullOrEmpty(objPatientPATDocument.COMMENTS) || !string.IsNullOrEmpty(objPatientPATDocument.START_DATE.ToString()) || !string.IsNullOrEmpty(objPatientPATDocument.END_DATE.ToString()) || objPatientPATDocument.SHOW_ON_PATIENT_PORTAL)
                        {
                            AddDocument(objPatientPATDocument, profile, newDocument);
                        }
                        else
                        {
                            AddDocument(objPatientPATDocument, profile, newDocument);
                        }
                    }
                    else
                    {
                        AddDocument(objPatientPATDocument, profile, newDocument);
                    }

                    AddorUpdate = "document saved successfully.";
                }

            }
            ResponseModel response = new ResponseModel()
            {
                ErrorMessage = "",
                Message = AddorUpdate,
                Success = true
            };
            return response;
        }
        public long AddDocument(PatientPATDocument objPatientPATDocument, UserProfile profile, bool newDocument)
        {
            PatientPATDocument newObjPatientPATDocument = new PatientPATDocument()
            {
                PATIENT_ACCOUNT = objPatientPATDocument.PATIENT_ACCOUNT,
                PATIENT_ACCOUNT_str = objPatientPATDocument.PATIENT_ACCOUNT_str,
                PARENT_DOCUMENT_ID = objPatientPATDocument.PARENT_DOCUMENT_ID,
                PRACTICE_CODE = objPatientPATDocument.PRACTICE_CODE,
                WORK_ID = objPatientPATDocument.WORK_ID,
                DOCUMENT_TYPE = objPatientPATDocument.DOCUMENT_TYPE,
                CASE_ID = objPatientPATDocument.CASE_ID,
                CASE_LIST = objPatientPATDocument.CASE_LIST,
                START_DATE = objPatientPATDocument.START_DATE,
                END_DATE = objPatientPATDocument.END_DATE,
                SHOW_ON_PATIENT_PORTAL = objPatientPATDocument.SHOW_ON_PATIENT_PORTAL,
                COMMENTS = objPatientPATDocument.COMMENTS,
                DOCUMENT_PATH_LIST = objPatientPATDocument.DOCUMENT_PATH_LIST,
                FOX_PHD_CALL_DETAILS_ID = objPatientPATDocument.FOX_PHD_CALL_DETAILS_ID,
                CREATED_BY = objPatientPATDocument.CREATED_BY,
                CREATED_DATE = objPatientPATDocument.CREATED_DATE,
                MODIFIED_BY = objPatientPATDocument.MODIFIED_BY,
                MODIFIED_DATE = objPatientPATDocument.MODIFIED_DATE,
                DELETED = objPatientPATDocument.DELETED,
            };
            var Parent_ID = newObjPatientPATDocument.PARENT_DOCUMENT_ID;
            if (newDocument)
            {
                newObjPatientPATDocument.PAT_DOCUMENT_ID = Helper.getMaximumId("FOX_PAT_DOCUMENT_ID");
                var firsttimeID = newObjPatientPATDocument.PAT_DOCUMENT_ID;
                newObjPatientPATDocument.PATIENT_ACCOUNT = Convert.ToInt64(newObjPatientPATDocument.PATIENT_ACCOUNT_str);
                newObjPatientPATDocument.WORK_ID = objPatientPATDocument.WORK_ID;
                newObjPatientPATDocument.PARENT_DOCUMENT_ID = !string.IsNullOrEmpty(Parent_ID.ToString()) ? Parent_ID : !string.IsNullOrEmpty(firsttimeID.ToString()) ? firsttimeID : 0;
                newObjPatientPATDocument.PRACTICE_CODE = profile.PracticeCode;
                newObjPatientPATDocument.CREATED_BY = newObjPatientPATDocument.MODIFIED_BY = profile.UserName;
                newObjPatientPATDocument.CREATED_DATE = newObjPatientPATDocument.MODIFIED_DATE = Helper.GetCurrentDate();
                newObjPatientPATDocument.DELETED = false;
                _foxPatientPATdocumentRepository.Insert(newObjPatientPATDocument);
                _foxPatientPATdocumentRepository.Save();
            }
            else
            {
                newObjPatientPATDocument.PAT_DOCUMENT_ID = _foxPatientPATdocumentRepository.GetMany(t => t.PATIENT_ACCOUNT == objPatientPATDocument.PATIENT_ACCOUNT && (t.DELETED == false)).OrderByDescending(t => t.CREATED_DATE).FirstOrDefault().PAT_DOCUMENT_ID;
            }

            if (newObjPatientPATDocument.DOCUMENT_PATH_LIST?.Count > 0)
            {
                foreach (var docPath in newObjPatientPATDocument.DOCUMENT_PATH_LIST)
                {
                    var ExistingImages = _foxPatientdocumentFilesRepository.GetFirst(i => i.PATIENT_DOCUMENT_FILE_ID == docPath.PATIENT_DOCUMENT_FILE_ID && i.PRACTICE_CODE == profile.PracticeCode && !i.DELETED);
                    if (ExistingImages == null)
                    {
                        PatientDocumentFiles ObjPatientDocumentFiles = new PatientDocumentFiles();
                        ObjPatientDocumentFiles.PATIENT_DOCUMENT_FILE_ID = Helper.getMaximumId("PATIENT_DOCUMENT_FILE_ID");
                        ObjPatientDocumentFiles.PRACTICE_CODE = profile.PracticeCode;
                        ObjPatientDocumentFiles.PAT_DOCUMENT_ID = newObjPatientPATDocument.PAT_DOCUMENT_ID;
                        ObjPatientDocumentFiles.DOCUMENT_PATH = docPath.DOCUMENT_PATH;
                        ObjPatientDocumentFiles.DOCUMENT_LOGO_PATH = "";
                        ObjPatientDocumentFiles.CREATED_BY = ObjPatientDocumentFiles.MODIFIED_BY = profile.UserName;
                        ObjPatientDocumentFiles.CREATED_DATE = ObjPatientDocumentFiles.MODIFIED_DATE = Helper.GetCurrentDate();
                        ObjPatientDocumentFiles.DELETED = false;
                        _foxPatientdocumentFilesRepository.Insert(ObjPatientDocumentFiles);
                        _foxPatientdocumentFilesRepository.Save();
                    }
                }
            }
            return newObjPatientPATDocument.PAT_DOCUMENT_ID;
        }

        public ResponseModel AddUpdatePHDCallDetailInformation(PHDCallDetail ObjPHDCallDetailRequest, UserProfile profile)
        {
            try
            {
                string AddorUpdate = "";
                if (!ObjPHDCallDetailRequest.Equals(null))
                {
                    int request = 0;
                    var PatientAccount = Convert.ToInt64(ObjPHDCallDetailRequest.PATIENT_ACCOUNT_STR);
                    int scenario = int.Parse(ObjPHDCallDetailRequest.CALL_SCENARIO);
                    int reason = int.Parse(ObjPHDCallDetailRequest.CALL_REASON);
                    if (ObjPHDCallDetailRequest.REQUEST != "")
                    { request = int.Parse(ObjPHDCallDetailRequest.REQUEST); }
                    long callAttendendName = !string.IsNullOrWhiteSpace(ObjPHDCallDetailRequest.CALL_ATTENDED_BY) ? long.Parse(ObjPHDCallDetailRequest.CALL_ATTENDED_BY) : 0;

                    var callScenario = _PhdCallScenarioRepository.GetFirst(s => s.PHD_CALL_SCENARIO_ID == scenario && s.PRACTICE_CODE == profile.PracticeCode && s.DELETED == false);
                    var CALL_REASON = _PhdCallReasonRepository.GetFirst(s => s.PHD_CALL_REASON_ID == reason && s.PRACTICE_CODE == profile.PracticeCode && s.DELETED == false);
                    var REQUEST = _PhdCallRequestRepository.GetFirst(s => s.PHD_CALL_REQUEST_ID == request && s.PRACTICE_CODE == profile.PracticeCode && s.DELETED == false);
                    var CALL_ATTENDED_BY_NAME = _userRepository.GetFirst(t => t.USER_ID == callAttendendName && !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode);

                    string NOTE = "";
                    if (ObjPHDCallDetailRequest.IsNewPatient == false)
                    {
                        if (ObjPHDCallDetailRequest.INCOMING_CALL_NO != "")
                        {
                            if (ObjPHDCallDetailRequest.REQUEST != "")
                            {
                                string num = getNumberInFormat(ObjPHDCallDetailRequest.INCOMING_CALL_NO);
                                NOTE = "Patient Note – PHD" + " | " + CALL_REASON.DESCRIPTION + " | " + num + " | " + REQUEST.DESCRIPTION + " | " + ObjPHDCallDetailRequest.CALL_DETAILS
                               + "<br>" + CALL_ATTENDED_BY_NAME.FIRST_NAME + " " + CALL_ATTENDED_BY_NAME.LAST_NAME + " | " + Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_DATE_STR).ToString("MM/dd/yyyy") + " (ESD) | " + Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_TIME_STR).ToString("h:mm tt");
                            }
                            else
                            {
                                string num = getNumberInFormat(ObjPHDCallDetailRequest.INCOMING_CALL_NO);
                                NOTE = "Patient Note – PHD" + " | " + CALL_REASON.DESCRIPTION + " | " + num + " | " + ObjPHDCallDetailRequest.CALL_DETAILS
                               + "<br>" + CALL_ATTENDED_BY_NAME.FIRST_NAME + " " + CALL_ATTENDED_BY_NAME.LAST_NAME + " | " + Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_DATE_STR).ToString("MM/dd/yyyy") + " (ESD) | " + Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_TIME_STR).ToString("h:mm tt");
                            }
                        }
                        else
                        {
                            if (ObjPHDCallDetailRequest.REQUEST != "")
                            {
                                NOTE = "Patient Note – PHD" + " | " + CALL_REASON.DESCRIPTION + " | " + REQUEST.DESCRIPTION + " | " + ObjPHDCallDetailRequest.CALL_DETAILS
                           + "<br>" + CALL_ATTENDED_BY_NAME.FIRST_NAME + " " + CALL_ATTENDED_BY_NAME.LAST_NAME + " | " + Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_DATE_STR).ToString("MM/dd/yyyy") + " (ESD) | " + Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_TIME_STR).ToString("h:mm tt");
                            }
                            else
                            {
                                NOTE = "Patient Note – PHD" + " | " + CALL_REASON.DESCRIPTION + " | " + ObjPHDCallDetailRequest.CALL_DETAILS
                          + "<br>" + CALL_ATTENDED_BY_NAME.FIRST_NAME + " " + CALL_ATTENDED_BY_NAME.LAST_NAME + " | " + Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_DATE_STR).ToString("MM/dd/yyyy") + " (ESD) | " + Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_TIME_STR).ToString("h:mm tt");

                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(ObjPHDCallDetailRequest.DOS_STR))
                    {
                        ObjPHDCallDetailRequest.DOS = Convert.ToDateTime(ObjPHDCallDetailRequest.DOS_STR);
                    }
                    if (!string.IsNullOrEmpty(ObjPHDCallDetailRequest.CALL_DATE_STR))
                    {
                        ObjPHDCallDetailRequest.CALL_DATE = Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_DATE_STR);
                    }
                    if (!string.IsNullOrEmpty(ObjPHDCallDetailRequest.CALL_TIME_STR))
                    {
                        ObjPHDCallDetailRequest.CALL_TIME = Convert.ToDateTime(ObjPHDCallDetailRequest.CALL_TIME_STR);
                    }
                    if (!string.IsNullOrEmpty(ObjPHDCallDetailRequest.FOLLOW_UP_DATE_STR))
                    {
                        ObjPHDCallDetailRequest.FOLLOW_UP_DATE = Convert.ToDateTime(ObjPHDCallDetailRequest.FOLLOW_UP_DATE_STR);
                    }
                    var ExistingDetailInfo = _PHDDetailRepository.GetFirst(r => r.FOX_PHD_CALL_DETAILS_ID == ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID && r.DELETED == false);
                    #region For Registered Patients
                    if (ObjPHDCallDetailRequest.IsNewPatient == false)
                    {
                        StringBuilder bldr = new StringBuilder();
                        bldr.AppendFormat(@"<p>{0}</p>", HttpUtility.HtmlEncode(NOTE));
                        NOTE = HttpUtility.HtmlDecode(bldr.ToString());
                        if (ExistingDetailInfo == null && !ObjPHDCallDetailRequest.IS_CALL_DETAIL_EDIT)
                        {
                            string ID;
                            var newNoteId = Helper.getMaximumId("GENERAL_NOTE_ID");
                            var newNote = new FOX_TBL_GENERAL_NOTE()
                            {
                                GENERAL_NOTE_ID = newNoteId,
                                CREATED_BY = profile.UserName,
                                CREATED_DATE = Helper.GetCurrentDate(),
                                DELETED = false,
                                MODIFIED_BY = profile.UserName,
                                MODIFIED_DATE = Helper.GetCurrentDate(),
                                NOTE_DESCRIPTION = getHTML(getRTF(NOTE)),
                                PRACTICE_CODE = profile.PracticeCode,
                                PATIENT_ACCOUNT = PatientAccount,
                                //CASE_ID = null,
                                PARENT_GENERAL_NOTE_ID = newNoteId,
                            };
                            _generalNotesRepository.Insert(newNote);
                            _generalNotesRepository.Save();
                            ID = newNote.GENERAL_NOTE_ID.ToString();
                            var extNote = _generalNotesRepository.GetSingle(x => x.GENERAL_NOTE_ID == newNote.GENERAL_NOTE_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                            if (extNote != null)
                            {
                                GenerateCaseEntries(ObjPHDCallDetailRequest, profile);
                                ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID = Helper.getMaximumId("FOX_PHD_CALL_DETAILS_ID");
                                ObjPHDCallDetailRequest.PATIENT_ACCOUNT = PatientAccount;
                                ObjPHDCallDetailRequest.PRACTICE_CODE = profile.PracticeCode;
                                ObjPHDCallDetailRequest.CREATED_BY = ObjPHDCallDetailRequest.MODIFIED_BY = profile.UserName;
                                ObjPHDCallDetailRequest.CREATED_DATE = ObjPHDCallDetailRequest.MODIFIED_DATE = Helper.GetCurrentDate();
                                ObjPHDCallDetailRequest.DELETED = false;
                                ObjPHDCallDetailRequest.GENERAL_NOTE_ID = extNote.GENERAL_NOTE_ID;
                                HasAttachment(ObjPHDCallDetailRequest, profile);
                                _PHDDetailRepository.Insert(ObjPHDCallDetailRequest);
                                _PHDDetailRepository.Save();
                                AddorUpdate = "Record saved successfully.";

                                if (ObjPHDCallDetailRequest != null)
                                {
                                    if (!string.IsNullOrEmpty(ObjPHDCallDetailRequest.CALL_DETAILS))
                                    {
                                        var LogDetailCn = ObjPHDCallDetailRequest.CALL_DETAILS;
                                        if (LogDetailCn != null)
                                        {
                                            AddPHDLog(ObjPHDCallDetailRequest, "CALL_NOTES", LogDetailCn, profile);
                                        }
                                    }
                                }
                                if (ObjPHDCallDetailRequest != null)
                                {
                                    if (!string.IsNullOrEmpty(ObjPHDCallDetailRequest.FOLLOW_UP_DATE.ToString()))
                                    {
                                        var LogDetailfu = "";
                                        DateTime datetimeStr = Convert.ToDateTime(ObjPHDCallDetailRequest.FOLLOW_UP_DATE);
                                        if (datetimeStr != null)
                                        {
                                            LogDetailfu = "Follow up started. Follow up on " + datetimeStr.ToString("d") + ".";
                                            AddPHDLog(ObjPHDCallDetailRequest, "FOLLOW_UP", LogDetailfu, profile);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ExistingDetailInfo.MRN = ObjPHDCallDetailRequest.MRN;
                            ExistingDetailInfo.DOS = ObjPHDCallDetailRequest.DOS;
                            ExistingDetailInfo.CALL_SCENARIO = ObjPHDCallDetailRequest.CALL_SCENARIO;
                            ExistingDetailInfo.CALL_DATE = ObjPHDCallDetailRequest.CALL_DATE;
                            ExistingDetailInfo.CALL_TIME = ObjPHDCallDetailRequest.CALL_TIME;
                            ExistingDetailInfo.CALL_DURATION = ObjPHDCallDetailRequest.CALL_DURATION;
                            ExistingDetailInfo.CALL_REASON = ObjPHDCallDetailRequest.CALL_REASON;
                            ExistingDetailInfo.AMOUNT = ObjPHDCallDetailRequest.AMOUNT;
                            ExistingDetailInfo.CALLER_NAME = ObjPHDCallDetailRequest.CALLER_NAME;
                            ExistingDetailInfo.RELATIONSHIP = ObjPHDCallDetailRequest.RELATIONSHIP;
                            ExistingDetailInfo.INCOMING_CALL_NO = ObjPHDCallDetailRequest.INCOMING_CALL_NO;
                            ExistingDetailInfo.PATIENT_EMAIL_ADDRESS = ObjPHDCallDetailRequest.PATIENT_EMAIL_ADDRESS;
                            ExistingDetailInfo.REQUEST = ObjPHDCallDetailRequest.REQUEST;
                            ExistingDetailInfo.CALL_ATTENDED_BY = ObjPHDCallDetailRequest.CALL_ATTENDED_BY;
                            ExistingDetailInfo.CALL_DETAILS = ObjPHDCallDetailRequest.CALL_DETAILS;
                            ExistingDetailInfo.MODIFIED_BY = profile.UserName;
                            ExistingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                            ExistingDetailInfo.CURRENT_EXTENSION = ObjPHDCallDetailRequest.CURRENT_EXTENSION;
                            ExistingDetailInfo.PRIORITY = ObjPHDCallDetailRequest.PRIORITY;
                            ExistingDetailInfo.FOLLOW_UP_DATE = ObjPHDCallDetailRequest.FOLLOW_UP_DATE;
                            ExistingDetailInfo.SSCM_CASE_ID = ObjPHDCallDetailRequest.SSCM_CASE_ID;
                            ExistingDetailInfo.CALL_RECORDING_PATH = ObjPHDCallDetailRequest.CALL_RECORDING_PATH;
                            _PHDDetailRepository.Update(ExistingDetailInfo);
                            _PHDDetailRepository.Save();
                            AddorUpdate = "Record updated successfully.";


                            if (ObjPHDCallDetailRequest != null)
                            {
                                if (!string.IsNullOrEmpty(ObjPHDCallDetailRequest.CALL_DETAILS))
                                {
                                    var existingLog = _phdCallLogHistoryRepository.GetMany(r => r.FOX_PHD_CALL_DETAILS_ID == ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID && r.CALL_LOG_OF_TYPE.ToLower() == "call_notes" && !r.DELETED).OrderByDescending(c => c.CREATED_DATE);
                                    if (existingLog != null && existingLog.Count() > 0)
                                    {
                                        if (existingLog.FirstOrDefault().CALL_DETAILS != ObjPHDCallDetailRequest.CALL_DETAILS)
                                        {
                                            var LogDetailCn = ObjPHDCallDetailRequest.CALL_DETAILS;
                                            AddPHDLog(ObjPHDCallDetailRequest, "CALL_NOTES", LogDetailCn, profile);
                                        }
                                    }
                                }
                            }
                            var existingLogfu = _phdCallLogHistoryRepository.GetMany(r => r.FOX_PHD_CALL_DETAILS_ID == ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID && r.CALL_LOG_OF_TYPE.ToLower() == "follow_up" && !r.DELETED).OrderByDescending(c => c.CREATED_DATE);
                            if (!string.IsNullOrEmpty(ObjPHDCallDetailRequest.FOLLOW_UP_DATE.ToString()))
                            {
                                if (existingLogfu != null && existingLogfu.Count() > 0)
                                {
                                    var LogDetailfu = "";
                                    DateTime datetimeStr = Convert.ToDateTime(ObjPHDCallDetailRequest.FOLLOW_UP_DATE);
                                    if (datetimeStr != null)
                                    {
                                        LogDetailfu = "Follow up started. Follow up on " + datetimeStr.ToString("d") + ".";
                                        if (existingLogfu.FirstOrDefault().CALL_DETAILS != LogDetailfu)
                                        {
                                            AddPHDLog(ObjPHDCallDetailRequest, "FOLLOW_UP", LogDetailfu, profile);
                                        }
                                    }
                                }
                                else
                                {
                                    var LogDetailfu = "";
                                    DateTime datetimeStr = Convert.ToDateTime(ObjPHDCallDetailRequest.FOLLOW_UP_DATE);
                                    if (datetimeStr != null)
                                    {
                                        LogDetailfu = "Follow up started. Follow up on " + datetimeStr.ToString("d") + ".";
                                        AddPHDLog(ObjPHDCallDetailRequest, "FOLLOW_UP", LogDetailfu, profile);
                                    }
                                }
                            }
                            else
                            {
                                if (existingLogfu != null && existingLogfu.Count() > 0)
                                {
                                    var LogDetailC = "Follow up cleared.";
                                    if (existingLogfu.FirstOrDefault().CALL_DETAILS != LogDetailC)
                                    {
                                        AddPHDLog(ObjPHDCallDetailRequest, "FOLLOW_UP", LogDetailC, profile);
                                    }
                                }
                            }

                            if (ObjPHDCallDetailRequest.GENERAL_NOTE_ID != null)
                            {
                                var extNote = _generalNotesRepository.GetSingle(x => x.GENERAL_NOTE_ID == ObjPHDCallDetailRequest.GENERAL_NOTE_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                                if (extNote != null)
                                {
                                    extNote.MODIFIED_BY = profile.UserName;
                                    extNote.MODIFIED_DATE = Helper.GetCurrentDate();
                                    extNote.NOTE_DESCRIPTION = getHTML(getRTF(NOTE));

                                    _generalNotesRepository.Update(extNote);
                                    _generalNotesRepository.Save();
                                }
                            }
                        }

                        InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                        interfaceSynch.FOX_INTERFACE_SYNCH_ID = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                        interfaceSynch.PATIENT_ACCOUNT = ObjPHDCallDetailRequest.PATIENT_ACCOUNT;
                        interfaceSynch.PRACTICE_CODE = profile.PracticeCode;
                        interfaceSynch.MODIFIED_BY = interfaceSynch.CREATED_BY = profile.UserName;
                        interfaceSynch.MODIFIED_DATE = interfaceSynch.CREATED_DATE = DateTime.Now;
                        interfaceSynch.DELETED = false;
                        interfaceSynch.IS_SYNCED = false;
                        interfaceSynch.APPLICATION = "PORTAL - PHD Calls";
                        interfaceSynch.GENERAL_NOTE_ID = Convert.ToInt64(ObjPHDCallDetailRequest.GENERAL_NOTE_ID ?? 0);
                        __InterfaceSynchModelRepository.Insert(interfaceSynch);
                        __InterfaceSynchModelRepository.Save();
                    }
                    #endregion End of Registered Patient

                    #region For New Patients
                    else
                    {
                        StringBuilder bldr = new StringBuilder();
                        bldr.AppendFormat(@"<p>{0}</p>", HttpUtility.HtmlEncode(NOTE));
                        NOTE = HttpUtility.HtmlDecode(bldr.ToString());
                        if (ExistingDetailInfo == null && !ObjPHDCallDetailRequest.IS_CALL_DETAIL_EDIT)
                        {
                            GenerateCaseEntries(ObjPHDCallDetailRequest, profile);
                            ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID = Helper.getMaximumId("FOX_PHD_CALL_DETAILS_ID");
                            ObjPHDCallDetailRequest.PATIENT_ACCOUNT = PatientAccount;
                            ObjPHDCallDetailRequest.PRACTICE_CODE = profile.PracticeCode;
                            ObjPHDCallDetailRequest.CREATED_BY = ObjPHDCallDetailRequest.MODIFIED_BY = profile.UserName;
                            ObjPHDCallDetailRequest.CREATED_DATE = ObjPHDCallDetailRequest.MODIFIED_DATE = Helper.GetCurrentDate();
                            ObjPHDCallDetailRequest.DELETED = false;
                            ObjPHDCallDetailRequest.GENERAL_NOTE_ID = null;
                            HasAttachment(ObjPHDCallDetailRequest, profile);
                            _PHDDetailRepository.Insert(ObjPHDCallDetailRequest);
                            _PHDDetailRepository.Save();
                            AddorUpdate = "Record saved successfully.";

                        }
                        else
                        {
                            ExistingDetailInfo.MRN = ObjPHDCallDetailRequest.MRN;
                            ExistingDetailInfo.DOS = ObjPHDCallDetailRequest.DOS;
                            ExistingDetailInfo.CALL_SCENARIO = ObjPHDCallDetailRequest.CALL_SCENARIO;
                            ExistingDetailInfo.CALL_DATE = ObjPHDCallDetailRequest.CALL_DATE;
                            ExistingDetailInfo.CALL_TIME = ObjPHDCallDetailRequest.CALL_TIME;
                            ExistingDetailInfo.CALL_DURATION = ObjPHDCallDetailRequest.CALL_DURATION;
                            ExistingDetailInfo.CALL_REASON = ObjPHDCallDetailRequest.CALL_REASON;
                            ExistingDetailInfo.AMOUNT = ObjPHDCallDetailRequest.AMOUNT;
                            ExistingDetailInfo.CALLER_NAME = ObjPHDCallDetailRequest.CALLER_NAME;
                            ExistingDetailInfo.RELATIONSHIP = ObjPHDCallDetailRequest.RELATIONSHIP;
                            ExistingDetailInfo.INCOMING_CALL_NO = ObjPHDCallDetailRequest.INCOMING_CALL_NO;
                            ExistingDetailInfo.PATIENT_EMAIL_ADDRESS = ObjPHDCallDetailRequest.PATIENT_EMAIL_ADDRESS;
                            ExistingDetailInfo.REQUEST = ObjPHDCallDetailRequest.REQUEST;
                            ExistingDetailInfo.CALL_ATTENDED_BY = ObjPHDCallDetailRequest.CALL_ATTENDED_BY;
                            ExistingDetailInfo.CALL_DETAILS = ObjPHDCallDetailRequest.CALL_DETAILS;
                            ExistingDetailInfo.MODIFIED_BY = profile.UserName;
                            ExistingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                            ExistingDetailInfo.CURRENT_EXTENSION = ObjPHDCallDetailRequest.CURRENT_EXTENSION;
                            ExistingDetailInfo.PRIORITY = ObjPHDCallDetailRequest.PRIORITY;
                            ExistingDetailInfo.FOLLOW_UP_DATE = ObjPHDCallDetailRequest.FOLLOW_UP_DATE;
                            _PHDDetailRepository.Update(ExistingDetailInfo);
                            _PHDDetailRepository.Save();
                            AddorUpdate = "Record updated successfully.";
                        }
                    }
                    #endregion End of Registered Patient
                }
                if (ObjPHDCallDetailRequest.IsNewPatient == true)
                {
                    ResponseModel response = new ResponseModel()
                    {
                        ErrorMessage = "",
                        Message = AddorUpdate,
                        Success = true,
                        ID = ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID.ToString()
                    };
                    return response;
                }
                else
                {
                    ResponseModel response = new ResponseModel()
                    {
                        ErrorMessage = "",
                        Message = AddorUpdate,
                        Success = true
                    };
                    return response;
                }
                //var NewInsertedDetail = _PHDDetailRepository.GetFirst(r => r.FOX_PHD_CALL_DETAILS_ID == ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID && r.DELETED == false);
                //return NewInsertedDetail;
            }
            catch (Exception ex)
            {
                ResponseModel response = new ResponseModel()
                {
                    ErrorMessage = ex.ToString(),
                    Message = "Not saved/updated",
                    Success = false
                };
                return response;
            }
        }
        public ResponseModel AddUpdateVerificationInformation(PhdPatientVerification ObjPhdPatientVerification, UserProfile profile)
        {
            try
            {
                string AddorUpdate = "";
                var PatientAccount = Convert.ToInt64(ObjPhdPatientVerification.PATIENT_ACCOUNT_STR);
                var patient_Detail = _PatientRepository.GetFirst(e => e.Patient_Account == PatientAccount);
                if (!string.IsNullOrEmpty(ObjPhdPatientVerification.LAST_VERIFIED_DATE_STR))
                {
                    ObjPhdPatientVerification.LAST_VERIFIED_DATE = Convert.ToDateTime(ObjPhdPatientVerification.LAST_VERIFIED_DATE_STR);
                }
                if (!ObjPhdPatientVerification.Equals(null))
                {
                    var ExistingPatientVerificationInfo = _PhdPatientVerificationRepository.GetFirst(r => r.FOX_PHD_CALL_PATIENT_VERIFICATION_ID == ObjPhdPatientVerification.FOX_PHD_CALL_PATIENT_VERIFICATION_ID && r.PATIENT_ACCOUNT == PatientAccount && r.DELETED == false);
                    if (ExistingPatientVerificationInfo == null)
                    {
                        ObjPhdPatientVerification.FOX_PHD_CALL_PATIENT_VERIFICATION_ID = Helper.getMaximumId("FOX_PHD_CALL_PATIENT_VERIFICATION_ID");
                        ObjPhdPatientVerification.CREATED_BY = ObjPhdPatientVerification.MODIFIED_BY = profile.UserName;
                        ObjPhdPatientVerification.CREATED_DATE = ObjPhdPatientVerification.MODIFIED_DATE = Helper.GetCurrentDate();
                        ObjPhdPatientVerification.DELETED = false;
                        ObjPhdPatientVerification.PATIENT_ACCOUNT = PatientAccount;
                        _PhdPatientVerificationRepository.Insert(ObjPhdPatientVerification);
                        _PhdPatientVerificationRepository.Save();
                        AddorUpdate = "Call submitted successfully.";
                        if (!ObjPhdPatientVerification.IS_PATIENT_EMAIL_ADDRESS_VERIFIED || !ObjPhdPatientVerification.IS_PATIENT_HOME_PHONE_VERIFIED || !ObjPhdPatientVerification.IS_PATIENT_CELL_PHONE_VERIFIED)
                        {
                            if (!string.IsNullOrEmpty(ObjPhdPatientVerification.NEW_EMAIL_ADDRESS))
                            {
                                patient_Detail.Email_Address = ObjPhdPatientVerification.NEW_EMAIL_ADDRESS;
                                patient_Detail.Modified_By = profile.UserName;
                                patient_Detail.Modified_Date = Helper.GetCurrentDate();
                                _PatientRepository.Update(patient_Detail);
                                _PatientRepository.Save();
                            }
                            if (!string.IsNullOrEmpty(ObjPhdPatientVerification.NEW_HOME_PHONE))
                            {
                                patient_Detail.Home_Phone = ObjPhdPatientVerification.NEW_HOME_PHONE;
                                patient_Detail.Modified_By = profile.UserName;
                                patient_Detail.Modified_Date = Helper.GetCurrentDate();
                                _PatientRepository.Update(patient_Detail);
                                _PatientRepository.Save();
                            }
                            if (!string.IsNullOrEmpty(ObjPhdPatientVerification.NEW_CELL_PHONE))
                            {
                                patient_Detail.cell_phone = ObjPhdPatientVerification.NEW_CELL_PHONE;
                                patient_Detail.Modified_By = profile.UserName;
                                patient_Detail.Modified_Date = Helper.GetCurrentDate();
                                _PatientRepository.Update(patient_Detail);
                                _PatientRepository.Save();
                            }
                        }
                    }
                    else
                    {
                        ExistingPatientVerificationInfo.IS_PATIENT_MRN_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_MRN_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_LAST_NAME_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_LAST_NAME_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_FIRST_NAME_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_FIRST_NAME_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_DOB_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_DOB_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_AGE_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_AGE_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_SSN_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_SSN_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_ADDRESS_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_ADDRESS_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_EMAIL_ADDRESS_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_EMAIL_ADDRESS_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_HOME_PHONE_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_HOME_PHONE_VERIFIED;
                        ExistingPatientVerificationInfo.IS_PATIENT_CELL_PHONE_VERIFIED = ObjPhdPatientVerification.IS_PATIENT_CELL_PHONE_VERIFIED;
                        ExistingPatientVerificationInfo.MODIFIED_BY = profile.UserName;
                        ExistingPatientVerificationInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                        ExistingPatientVerificationInfo.LAST_VERIFIED_DATE = ObjPhdPatientVerification.LAST_VERIFIED_DATE;
                        _PhdPatientVerificationRepository.Update(ExistingPatientVerificationInfo);
                        _PhdPatientVerificationRepository.Save();
                        AddorUpdate = "Call updated successfully.";
                        if (!ObjPhdPatientVerification.IS_PATIENT_EMAIL_ADDRESS_VERIFIED || !ObjPhdPatientVerification.IS_PATIENT_HOME_PHONE_VERIFIED || !ObjPhdPatientVerification.IS_PATIENT_CELL_PHONE_VERIFIED)
                        {
                            if (!string.IsNullOrEmpty(ObjPhdPatientVerification.NEW_EMAIL_ADDRESS))
                            {
                                patient_Detail.Email_Address = ObjPhdPatientVerification.NEW_EMAIL_ADDRESS;
                                patient_Detail.Modified_By = profile.UserName;
                                patient_Detail.Modified_Date = Helper.GetCurrentDate();
                                _PatientRepository.Update(patient_Detail);
                                _PatientRepository.Save();
                            }
                            if (!string.IsNullOrEmpty(ObjPhdPatientVerification.NEW_HOME_PHONE))
                            {
                                patient_Detail.Home_Phone = ObjPhdPatientVerification.NEW_HOME_PHONE;
                                patient_Detail.Modified_By = profile.UserName;
                                patient_Detail.Modified_Date = Helper.GetCurrentDate();
                                _PatientRepository.Update(patient_Detail);
                                _PatientRepository.Save();
                            }
                            if (!string.IsNullOrEmpty(ObjPhdPatientVerification.NEW_CELL_PHONE))
                            {
                                patient_Detail.cell_phone = ObjPhdPatientVerification.NEW_CELL_PHONE;
                                patient_Detail.Modified_By = profile.UserName;
                                patient_Detail.Modified_Date = Helper.GetCurrentDate();
                                _PatientRepository.Update(patient_Detail);
                                _PatientRepository.Save();
                            }
                        }
                    }
                }
                ResponseModel response = new ResponseModel()
                {
                    ErrorMessage = "",
                    Message = AddorUpdate,
                    Success = true
                };
                return response;
            }
            catch (Exception ex)
            {
                ResponseModel response = new ResponseModel()
                {
                    ErrorMessage = ex.ToString(),
                    Message = "Not saved/updated",
                    Success = false
                };
                return response;
            }
        }
        public string ExportToExcelPHD(CallDetailsSearchRequest ObjCallDetailsSearchRequest, UserProfile profile)
        {
            try
            {
                string fileName = "Patient_Helpdesk";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                ObjCallDetailsSearchRequest.CURRENT_PAGE = 1;
                ObjCallDetailsSearchRequest.RECORD_PER_PAGE = 0;
                var CalledFrom = "Patient_Helpdesk";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<PHDCallDetail> result = new List<PHDCallDetail>();

                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetPHDCallDetailsInformation(ObjCallDetailsSearchRequest, profile);
                //Change by Arqam
                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
                TextInfo text_info = culture_info.TextInfo;
                for (int i = 0; i < result.Count(); i++)
                {

                    result[i].CALL_ATTENDED_BY_NAME = text_info.ToTitleCase(result[i].CALL_ATTENDED_BY_NAME);
                    result[i].CALLER_NAME = text_info.ToTitleCase(result[i].CALLER_NAME.ToLower());
                    result[i].CALL_DETAILS = result[i].CALL_DETAILS.First().ToString().ToUpper() + result[i].CALL_DETAILS.Substring(1);
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<PHDCallDetail>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string getNumberInFormat(string number)
        {
            if (number != "" && number != null && number.Length == 10)
            {
                string temp1, temp2, temp3;
                temp1 = number.Substring(0, 3);
                temp2 = number.Substring(3, 3);
                temp3 = number.Substring(6, 4);

                return "(" + temp1 + ") " + temp2 + "-" + temp3;
            }
            else
            {
                return "";
            }
        }
        public string getHTML(string encodedString)
        {
            //Dummy String for test
            //encodedString = "e1xydGYxXGFuc2lccGFwZXJ3MTIyNDBccGFwZXJoMTU4NDBcbWFyZ2w1NzZcbWFyZ3Q1NzZcbWFyZ3I1NzZcbWFyZ2I1NzZ7XCpccnRwYXBlcnNpemUweDRDNjU3NDc0NjU3Mn1cYW5zaWNwZzEyNTJcZGVmZjB7XGZvbnR0Ymx7XGYwXGZzd2lzc1xmcHJxMlxmY2hhcnNldDAgQ2FsaWJyaTt9e1xmMVxmcm9tYW5cZnBycTJcZmNoYXJzZXQyIFN5bWJvbDt9e1xmMlxmbmlsIEFyaWFsO319DQp7XCpcZ2VuZXJhdG9yIE1zZnRlZGl0IDUuNDEuMjEuMjUxMDt9XHZpZXdraW5kNFx1YzFccGFyZFxsYW5nMTAzM1xiXGYwXGZzMjIgQ2hhbmdlIERldGFpbHM6XHBhcg0KXHBhcmRcZmk3MjBcYjAgaWxzIGFyZTpccGFyDQpcdWxcYiBDaGFuZ2UgZGV0YWlscyBhcmU6XHVsbm9uZVxiMFxwYXINClxwYXJkXGZpLTM2MFxsaTE0NTFcZjFcJ2I3XHRhYlxmMCBQbGVhc2UgcHJvY2VlZCBmb3IgVUFUXHBhcg0KXGYxXCdiN1x0YWJcZjJcZnMyMFxwYXINCn0NCgA=";
            if (!string.IsNullOrEmpty(encodedString))
            {
                byte[] data = Convert.FromBase64String(encodedString);
                string decodedString = Encoding.UTF8.GetString(data);
                //Dll for Rtf for Html Conversion
                var hr = new WEBEHRUtil.Generics.RtfConvertor();
                string Html = hr.HrsConvertBuffer(decodedString, WEBEHRUtil.Generics.RtfConvertor.XLATE_RTF_TO_HTML);
                Html = Html.Replace("Arial", "");
                return Html;
            }
            else
                return null;
        }
        public string getRTF(string encodedString)
        {
            //Dummy String for test
            //encodedString = "e1xydGYxXGFuc2lccGFwZXJ3MTIyNDBccGFwZXJoMTU4NDBcbWFyZ2w1NzZcbWFyZ3Q1NzZcbWFyZ3I1NzZcbWFyZ2I1NzZ7XCpccnRwYXBlcnNpemUweDRDNjU3NDc0NjU3Mn1cYW5zaWNwZzEyNTJcZGVmZjB7XGZvbnR0Ymx7XGYwXGZzd2lzc1xmcHJxMlxmY2hhcnNldDAgQ2FsaWJyaTt9e1xmMVxmcm9tYW5cZnBycTJcZmNoYXJzZXQyIFN5bWJvbDt9e1xmMlxmbmlsIEFyaWFsO319DQp7XCpcZ2VuZXJhdG9yIE1zZnRlZGl0IDUuNDEuMjEuMjUxMDt9XHZpZXdraW5kNFx1YzFccGFyZFxsYW5nMTAzM1xiXGYwXGZzMjIgQ2hhbmdlIERldGFpbHM6XHBhcg0KXHBhcmRcZmk3MjBcYjAgaWxzIGFyZTpccGFyDQpcdWxcYiBDaGFuZ2UgZGV0YWlscyBhcmU6XHVsbm9uZVxiMFxwYXINClxwYXJkXGZpLTM2MFxsaTE0NTFcZjFcJ2I3XHRhYlxmMCBQbGVhc2UgcHJvY2VlZCBmb3IgVUFUXHBhcg0KXGYxXCdiN1x0YWJcZjJcZnMyMFxwYXINCn0NCgA=";
            if (!string.IsNullOrEmpty(encodedString))
            {
                var data = Encoding.Default.GetBytes(encodedString);
                string decodedString = Encoding.UTF8.GetString(data);
                decodedString = decodedString.Replace("�", "-");
                //Dll for Rtf for Html Conversion
                var hr = new WEBEHRUtil.Generics.RtfConvertor();
                string RTF = hr.HrsConvertBuffer(decodedString, WEBEHRUtil.Generics.RtfConvertor.XLATE_HTML_TO_RTF);
                RTF = Convert.ToBase64String(Encoding.Default.GetBytes(RTF));
                return RTF;
            }
            else
                return null;
        }
        public bool AddUpdateRecordingName(PHDCallDetail ObjPHDCallDetailRequest, UserProfile profile)
        {
            var ExistingDetailInfo = _PHDDetailRepository.GetFirst(r => r.FOX_PHD_CALL_DETAILS_ID == ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID
            && r.PRACTICE_CODE == profile.PracticeCode && r.DELETED == false);

            ExistingDetailInfo.CALL_RECORDING_PATH = ObjPHDCallDetailRequest.CALL_RECORDING_PATH;
            ExistingDetailInfo.MODIFIED_BY = profile.UserName;
            ExistingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
            _PHDDetailRepository.Update(ExistingDetailInfo);
            _PHDDetailRepository.Save();

            PHDUnmappedCalls callToDelete = new PHDUnmappedCalls();
            callToDelete = _PHDUnmappedCallsRepository.GetFirst(x => x.CALL_RECORDING_PATH == ObjPHDCallDetailRequest.CALL_RECORDING_PATH
             && x.PRACTICE_CODE == profile.PracticeCode && x.DELETED == false);

            callToDelete.DELETED = true;
            callToDelete.MODIFIED_BY = profile.UserName;
            callToDelete.MODIFIED_DATE = Helper.GetCurrentDate();
            _PHDUnmappedCallsRepository.Update(callToDelete);
            _PHDUnmappedCallsRepository.Save();

            return true;
        }

        private List<FoxApplicationUsersViewModel> GetPHDCallerDropDownValue(UserProfile profile)
        {
            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            List<User> users = SpRepository<User>.GetListWithStoreProcedure(@"FOX_PROC_GET_PHD_CALLERS @PRACTICE_CODE", practice_Code);
            List<FoxApplicationUsersViewModel> phdcallers = new List<FoxApplicationUsersViewModel>();
            if (users != null && users.Count > 0)
            {
                phdcallers = users.Select(t => new FoxApplicationUsersViewModel()
                {
                    FIRST_NAME = t.FIRST_NAME,
                    LAST_NAME = t.LAST_NAME,
                    PRACTICE_CODE = t.PRACTICE_CODE,
                    USER_ID = t.USER_ID,
                    USER_NAME = t.USER_NAME,
                    FullName = t.FIRST_NAME + " " + t.LAST_NAME
                }).OrderBy(o => o.FIRST_NAME).ToList();
                phdcallers[0].CURRENT_USER_ID = profile.userID.ToString();
                phdcallers[0].CURRENT_USER_NAME = profile.UserName.ToString();

            }
            return phdcallers;
        }

        public List<PHDUnmappedCalls> GetUnmappedCalls(UnmappedCallsSearchRequest reg, UserProfile profile)
        {
            List<PHDUnmappedCalls> returList = new List<PHDUnmappedCalls>();
            if (!string.IsNullOrEmpty(reg.CALL_DATE_STR))
                reg.CALL_DATE = Convert.ToDateTime(reg.CALL_DATE_STR);
            else
                reg.CALL_DATE = null;



            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _callNo = new SqlParameter { ParameterName = "CALL_NO", Value = reg.CALL_NO };
            SqlParameter _callDate = Helper.getDBNullOrValue("CALL_DATE", reg.CALL_DATE.ToString());

            returList = SpRepository<PHDUnmappedCalls>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_UNMAPPED_CALLS
                            @PRACTICE_CODE, @CALL_DATE, @CALL_NO"
                           , _practiceCode, _callDate, _callNo);
            return returList;
        }
        /// <summary>
        ///  GetFoxDocumentTypes returing the List of Document Type
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public List<FoxDocumentType> GetFoxDocumentTypes(UserProfile userProfile)
        {
            try
            {
                var getDocumentTypeList = _foxDocumentTypeRepository.GetMany(d => !d.DELETED && d.IS_ACTIVE != null).OrderBy(o => o.NAME).ToList();
                if (getDocumentTypeList == null)
                {
                    return getDocumentTypeList = new List<FoxDocumentType>();
                }
                return getDocumentTypeList;
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }
        /// <summary>
        /// This Function is trigger to get list of Follow Up Calls Details.
        /// </summary>
        /// <returns></returns>
        public List<PHDCallDetail> GetFollowUpCalls(UserProfile userProfile)
        {
            try
            {
                SqlParameter UserID = new SqlParameter { ParameterName = "USERID", SqlDbType = SqlDbType.Int, Value = userProfile.userID };
                var getFollowCalls = SpRepository<PHDCallDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PHD_PATIENT_DETAILS @USERID", UserID);
                if (getFollowCalls == null)
                {
                    return getFollowCalls = new List<PHDCallDetail>();
                }
                return getFollowCalls;
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }
        /// <summary>
        /// This Function is used to generate enteries in WEBSOFT Table 
        /// on the basics of Case Number Generation Internally
        /// </summary>
        /// <param name="ObjPHDCallDetailRequest"></param>
        /// <param name="profile"></param>
        public void GenerateCaseEntries(PHDCallDetail objPHDCallDetailRequest, UserProfile profile)
        {
            if (objPHDCallDetailRequest != null && objPHDCallDetailRequest._IsSSCM == true)
            {
                var sscmCaseNum = GenerateCaseNumber();
                objPHDCallDetailRequest.SSCM_CASE_ID = sscmCaseNum;

                var users = GetUserDetails(profile);

                if (users != null && users.Count > 0 && users.Select(s => s.User_FName != null).FirstOrDefault())
                {
                    var UserId = users.Select(u => u.InternalUserID)?.FirstOrDefault().ToUpper();
                    var PracticeCode = profile.PracticeCode;
                    var FullName = users.Select(u => new { u.User_FName, u.User_LName })?.FirstOrDefault();
                    var GetCallReasonList = GetDropdownLists(profile);
                    var CallReasonName = "";
                    var csCaseCategory = "";
                    if (GetCallReasonList != null && GetCallReasonList.PhdCallReasons != null)
                    {
                        CallReasonName = GetCallReasonList.PhdCallReasons.Find(r => r.PHD_CALL_REASON_ID.ToString() == objPHDCallDetailRequest.CALL_REASON).NAME;
                        csCaseCategory = GetCallReasonList.CSCaseCategories.Find(r => r.CS_Category_ID.ToString() == objPHDCallDetailRequest.CS_CASE_CATEGORY).CS_Category_ID.ToString();
                    }
                    var Email = users.Select(e => e.EMAIL)?.FirstOrDefault();
                    if (objPHDCallDetailRequest.PRIORITY == null)
                        objPHDCallDetailRequest.PRIORITY = "";

                    InsertCase(sscmCaseNum, FullName.User_FName + " " + FullName.User_LName, CallReasonName, objPHDCallDetailRequest.CALL_DETAILS,
                    "", csCaseCategory, 0, "2", objPHDCallDetailRequest.PRIORITY, PracticeCode.ToString(), "",
                    objPHDCallDetailRequest.PATIENT_ACCOUNT_STR, "", "", "", objPHDCallDetailRequest.INCOMING_CALL_NO, 0, Email,
                    "NC", "", false, false, UserId.ToString());
                }
                else
                {
                    var UserId = users.Select(u => u.USER_ID)?.FirstOrDefault().ToString().ToUpper();
                    var PracticeCode = profile.PracticeCode;
                    var FullName = users.Select(u => new { u.FIRST_NAME, u.LAST_NAME })?.FirstOrDefault();
                    var GetCallReasonList = GetDropdownLists(profile);
                    var CallReasonName = "";
                    var csCaseCategory = "";
                    if (GetCallReasonList != null && GetCallReasonList.PhdCallReasons != null)
                    {
                        CallReasonName = GetCallReasonList.PhdCallReasons.Find(r => r.PHD_CALL_REASON_ID.ToString() == objPHDCallDetailRequest.CALL_REASON).NAME;
                        csCaseCategory = GetCallReasonList.CSCaseCategories.Find(r => r.CS_Category_ID.ToString() == objPHDCallDetailRequest.CS_CASE_CATEGORY).CS_Category_ID.ToString();
                    }
                    var Email = users.Select(e => e.EMAIL)?.FirstOrDefault();
                    if (objPHDCallDetailRequest.PRIORITY == null)
                        objPHDCallDetailRequest.PRIORITY = "";

                    InsertCase(sscmCaseNum, FullName.FIRST_NAME + " " + FullName.LAST_NAME, CallReasonName, objPHDCallDetailRequest.CALL_DETAILS, "", csCaseCategory, 0, "2",
                    objPHDCallDetailRequest.PRIORITY, PracticeCode.ToString(), "", objPHDCallDetailRequest.PATIENT_ACCOUNT_STR, "", "", "", objPHDCallDetailRequest.INCOMING_CALL_NO, 0, Email,
                    "NC", "", false, false, UserId.ToString());
                }
            }
        }
        /// <summary>
        /// This Function is trigger when PHD Module
        /// conatins the Attachment
        /// </summary>
        /// <param name="ObjPHDCallDetailRequest"></param>
        /// <param name="profile"></param>
        public void HasAttachment(PHDCallDetail ObjPHDCallDetailRequest, UserProfile profile)
        {
            if (ObjPHDCallDetailRequest.ATTACHMENT_NAME != null && ObjPHDCallDetailRequest.ATTACHMENT_NAME != "")
            {
                var ticks = DateTime.Now.Ticks;
                long pageCounter = 0;

                var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                var Attachment_Extension = "." + ObjPHDCallDetailRequest.ATTACHMENT_NAME?.Split('.').Last().ToLower();
                string filePath = ObjPHDCallDetailRequest.ATTACHMENT_NAME;

                if (Attachment_Extension == ".tiff" || Attachment_Extension == ".tif" || Attachment_Extension == ".png" || Attachment_Extension == ".jpg" || Attachment_Extension == ".jpeg" || Attachment_Extension == ".gif")
                {
                    ConvertPDFToImages pdfToImg = new ConvertPDFToImages();
                    int numberOfPages = pdfToImg.tifToImage(filePath, config.IMAGES_PATH_SERVER, ObjPHDCallDetailRequest.PATIENT_ACCOUNT, ticks, config.IMAGES_PATH_DB, out pageCounter, false);
                    SavePdftoImagesEligibilty(ObjPHDCallDetailRequest, filePath, config, numberOfPages, "PHD", profile, ObjPHDCallDetailRequest.PATIENT_ACCOUNT_STR, true, ticks);
                }
                else
                {
                    int numberOfPages = getNumberOfPagesOfPDF(filePath);
                    SavePdftoImagesEligibilty(ObjPHDCallDetailRequest, filePath, config, numberOfPages, "PHD", profile, ObjPHDCallDetailRequest.PATIENT_ACCOUNT_STR, true, ticks);
                }
            }
        }
        /// <summary>
        /// This Function is trigger to generate SSCM Case.
        /// </summary>
        /// <returns></returns>
        public string GenerateCaseNumber()
        {
            using (var db = new DBContextFoxPHD())
            {
                try
                {
                    var maintenanceResult = _DBContextFoxPHD.Maintenance.Select(m => new { m.Case_No, m.Office_Id });
                    var strCaseNo = "";
                    if (maintenanceResult != null)
                    {
                        var caseNumber = maintenanceResult.Select(c => c.Case_No)?.FirstOrDefault();
                        var officeID = maintenanceResult.Select(c => c.Office_Id)?.FirstOrDefault();
                        _DBContextFoxPHD.Database.ExecuteSqlCommand("Update Maintenance Set Case_No = " + Convert.ToString(caseNumber + 1));
                        strCaseNo = officeID + caseNumber.ToString();
                        var isCharCode = GetLocationCharacter("LOC-OTH-01");
                        if (isCharCode != null)
                        {
                            strCaseNo = strCaseNo + "-" + isCharCode;
                        }
                    }
                    return strCaseNo;
                }
                catch (NullReferenceException)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// This Function is returning single Object which is use to get the Location Character 
        /// </summary>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        public string GetLocationCharacter(string LocationID)
        {
            try
            {
                SqlParameter locationID = new SqlParameter { ParameterName = "LOCATION_ID", SqlDbType = SqlDbType.VarChar, Value = LocationID };
                var result = SpRepository<string>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_LOCATION_CHAR_CODE @LOCATION_ID", locationID);
                if (result == null)
                {
                    return result = "";
                }
                return result;
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }
        /// <summary>
        /// This Function is used to insert records on Progress, Customer
        /// Support and History Table. 
        /// </summary>
        /// <param name="strCaseNo"></param>
        /// <param name="strRepName"></param>
        /// <param name="strTitle"></param>
        /// <param name="strDetails"></param>
        /// <param name="strCaseMail"></param>
        /// <param name="strCaseCategory"></param>
        /// <param name="intNCalled"></param>
        /// <param name="strCaseType"></param>
        /// <param name="strPriority"></param>
        /// <param name="strPractice"></param>
        /// <param name="strProvider"></param>
        /// <param name="strPatientAccount"></param>
        /// <param name="strClaimNo"></param>
        /// <param name="strInsCode"></param>
        /// <param name="strResDate"></param>
        /// <param name="strPhone"></param>
        /// <param name="intPhoneType"></param>
        /// <param name="strEmail"></param>
        /// <param name="CaseStatus"></param>
        /// <param name="SendMailTo"></param>
        /// <param name="ShowOnWeb"></param>
        /// <param name="boolTemplate"></param>
        /// <param name="userid"></param>
        private void InsertCase(string strCaseNo, string strRepName, string strTitle, string strDetails, string strCaseMail, string strCaseCategory, int intNCalled, string strCaseType,
        string strPriority, string strPractice, string strProvider, string strPatientAccount, string strClaimNo, string strInsCode, string strResDate, string strPhone, int intPhoneType,
        string strEmail, string CaseStatus, string SendMailTo, bool ShowOnWeb, bool boolTemplate, string userid)
        {
            using (var db = new DBContextFoxPHD())
            {
                try
                {
                    SqlParameter CsCategoryId = new SqlParameter { ParameterName = "CS_Category_ID", SqlDbType = SqlDbType.VarChar, Value = strCaseCategory };
                    var CsPrimaryPerson = SpRepository<string>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_CS_PRIMARY_RES_PERSON @CS_Category_ID", CsCategoryId);

                    if (CsPrimaryPerson != null)
                    {
                        // Insert Records in CustomerSupportInfo Table.

                        CSCustomerSupportInfo customerSupportInfo = new CSCustomerSupportInfo();
                        customerSupportInfo.CS_Case_No = strCaseNo;
                        customerSupportInfo.CS_Case_From_Rep = strRepName;
                        customerSupportInfo.CS_Case_Title = strTitle;
                        customerSupportInfo.CS_Case_Detail = strDetails;
                        customerSupportInfo.CS_REPLY_MAIL_ID = strCaseMail;
                        customerSupportInfo.CS_Case_Category = strCaseCategory;
                        customerSupportInfo.CS_Received_DateTime = DateTime.Now;
                        customerSupportInfo.CS_User_Name = userid.ToString();
                        customerSupportInfo.CS_Created_Date = DateTime.Now;
                        customerSupportInfo.CS_Number_Of_Times_Called = intNCalled;
                        //customerSupportInfo.CS_Case_Type_ID = strCaseType;
                        customerSupportInfo.Priority_Level = strPriority != "" ? strPriority.First().ToString() : "";
                        customerSupportInfo.cs_deleted = false;
                        customerSupportInfo.rowguid = Guid.NewGuid();
                        db.CSCustomerSupportInfos.Add(customerSupportInfo);
                        db.SaveChanges();

                        // Insert Records in CaseProgress Table.

                        CSCaseProgress caseProgress = new CSCaseProgress();
                        caseProgress.CS_Case_No = strCaseNo;
                        caseProgress.CS_Rectified_Case_Type = strCaseType;
                        caseProgress.CS_Practice_Code = Convert.ToInt64(strPractice);
                        caseProgress.CS_Patient_Account = strPatientAccount;
                        caseProgress.CS_Claim_No = strClaimNo;
                        caseProgress.CS_Insurance_Code = strInsCode;
                        caseProgress.CS_Rep_Phone = strPhone;
                        caseProgress.CS_Rep_Phone_Type = intPhoneType;
                        caseProgress.CS_Rep_Email = strEmail;
                        caseProgress.CS_Created_By = "FOX WEB";
                        caseProgress.CS_Created_Date = DateTime.Now;
                        caseProgress.CS_Case_Status = CaseStatus;
                        caseProgress.cs_deleted = false;
                        caseProgress.CS_Pri_Resp_Person = CsPrimaryPerson.ToUpper();
                        caseProgress.Cs_Send_Mail_To = SendMailTo.Trim();
                        caseProgress.Show_on_Web = false;
                        caseProgress.Cs_Authorize = "0";
                        caseProgress.rowguid = Guid.NewGuid();
                        db.CSCaseProgresses.Add(caseProgress);
                        db.SaveChanges();

                        // Insert Records in CaseHistory Table.

                        CSCaseHistory caseHistory = new CSCaseHistory();
                        caseHistory.CS_Case_No = strCaseNo;
                        caseHistory.CS_Case_Status = CaseStatus;
                        caseHistory.CS_Practice_Code = Convert.ToInt64(strPractice);
                        caseHistory.CS_Patient_Account = strPatientAccount;
                        caseHistory.CS_Claim_No = strClaimNo;
                        caseHistory.CS_Insurance_Code = strInsCode;
                        caseHistory.CS_Response_Date = DateTime.Now;
                        caseHistory.CS_Rep_Phone = strPhone;
                        caseHistory.CS_Rep_Phone_Type = intPhoneType;
                        caseHistory.CS_Rep_Email = strEmail;
                        caseHistory.CS_Modified_By = userid.ToString();
                        caseHistory.CS_Pri_Resp_Person = CsPrimaryPerson.ToUpper();
                        caseHistory.CS_Modified_Date = DateTime.Today.Date;
                        caseHistory.cs_deleted = false;
                        caseHistory.rowguid = Guid.NewGuid();
                        db.CSCaseHistories.Add(caseHistory);
                        db.SaveChanges();
                    }
                }
                catch (NullReferenceException nullRef)
                {
                    throw nullRef;
                }
            }
        }
        /// <summary>
        /// This function is return SSCM Case Generation 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public List<SscmCaseDetail> GetCaseDetails(UserProfile profile)
        {
            try
            {
                SqlParameter praticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                List<SscmCaseDetail> result = SpRepository<SscmCaseDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SSCM_CASE_DETAILS @PRACTICE_CODE", praticeCode);
                if (result == null)
                {
                    return result = new List<SscmCaseDetail>();
                }
                return result;
            }
            catch (NullReferenceException)
            {

                throw;
            }
        }
        /// <summary>
        /// This Function is used to Get User Details Internal and External
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public List<FoxUserDetails> GetUserDetails(UserProfile profile)
        {
            try
            {
                SqlParameter email = new SqlParameter { ParameterName = "EMAIL", Value = profile.UserEmailAddress };
                List<FoxUserDetails> users = SpRepository<FoxUserDetails>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USERS_DETAILS @EMAIL", email);
                if (users == null)
                {
                    return users = new List<FoxUserDetails>();
                }
                return users;
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }
        /// <summary>
        /// This function is used to get records of current 
        /// daily report of selected users.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="callerUserID"></param>
        /// <returns></returns>
        public List<ExportAdvancedDailyReport> GetExportAdvancedDailyReports(UserProfile profile, string callerUserID)
        {
            try
            {
                SqlParameter practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                SqlParameter callAttendedBy = new SqlParameter { ParameterName = "CALL_ATTENDED_BY", SqlDbType = SqlDbType.VarChar, Value = callerUserID };
                var result = SpRepository<ExportAdvancedDailyReport>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADVANCED_EXPORT_REPORT @PRACTICE_CODE, @CALL_ATTENDED_BY", practiceCode, callAttendedBy).ToList();
                IEnumerable<ExportAdvancedDailyReport> exportAdvanceReport = null;
                if (result != null)
                {
                    exportAdvanceReport = result.Select(c => new ExportAdvancedDailyReport
                    {
                        CALL_DATE_STR = c.CALL_DATE_STR,
                        NAME = c.NAME,
                        CALL_DETAILS = c.CALL_DETAILS,
                        CALL_USER_ID = c.CALL_USER_ID,
                        PATIENT_ACCOUNT = c.PATIENT_ACCOUNT,
                        CurrencyAmount = c.AMOUNT.HasValue ? c.AMOUNT.Value.ToString("C2") : ""
                    });
                    if (exportAdvanceReport != null)
                    {
                        result = exportAdvanceReport.ToList();
                    }
                }
                else
                {
                    return result = new List<ExportAdvancedDailyReport>();
                }
                return result;
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }
        /// <summary>
        /// This function is used to export selected record 
        /// into Excel sheet.
        /// </summary>
        /// <param name="exportAdvancedDailyReport"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        public string ExportAdvancedDailyReport(ExportAdvancedDailyReport exportAdvancedDailyReport, UserProfile profile)
        {
            try
            {
                string fileName = "Daily_Report_List";
                string exportPath = string.Empty;
                string path = string.Empty;
                bool exported;
                var CalledFrom = "Advanced_Daily_Report";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<ExportAdvancedDailyReport> result = new List<ExportAdvancedDailyReport>();
                var pathToWriteFile = exportPath + "\\" + fileName;
                result = GetExportAdvancedDailyReports(profile, exportAdvancedDailyReport.CALL_USER_ID);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;
                }
                exported = ExportToExcel.CreateExcelDocument<ExportAdvancedDailyReport>(result, pathToWriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (NullReferenceException)
            {
                throw;
            }
        }
        /// <summary>
        /// This Function is used  to return the PHD Call
        /// Log History Details.
        /// </summary>
        /// <param name="phdCallDetailID"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public List<PhdCallLogHistoryDetail> GetPhdCallLogHistoryDetails(string phdCallDetailID, UserProfile userProfile)
        {
            try
            {
                SqlParameter praticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", Value = userProfile.PracticeCode };
                SqlParameter callDetailID = new SqlParameter { ParameterName = "FOX_PHD_CALL_DETAILS_ID", Value = phdCallDetailID };
                List<PhdCallLogHistoryDetail> logCallDetails = SpRepository<PhdCallLogHistoryDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PHD_CALL_HISTORY_DETAILS @PRACTICE_CODE, @FOX_PHD_CALL_DETAILS_ID", praticeCode, callDetailID);
                if (logCallDetails == null)
                {
                    return logCallDetails = new List<PhdCallLogHistoryDetail>();
                }
                return logCallDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void AddPHDLog(PHDCallDetail ObjPHDCallDetailRequest, string LogFor, string LogDetail, UserProfile profile)
        {
            PhdCallLogHistory phdCallLogHistory = new PhdCallLogHistory();
            phdCallLogHistory.PHD_CALL_LOG_ID = Helper.getMaximumId("PHD_CALL_LOG_ID");
            phdCallLogHistory.FOX_PHD_CALL_DETAILS_ID = ObjPHDCallDetailRequest.FOX_PHD_CALL_DETAILS_ID;
            phdCallLogHistory.PRACTICE_CODE = ObjPHDCallDetailRequest.PRACTICE_CODE;
            phdCallLogHistory.PATIENT_ACCOUNT = ObjPHDCallDetailRequest.PATIENT_ACCOUNT;
            phdCallLogHistory.FOLLOW_UP_DATE = LogFor == "CALL_NOTES" ? null : ObjPHDCallDetailRequest.FOLLOW_UP_DATE;
            phdCallLogHistory.CALL_DETAILS = LogDetail;
            phdCallLogHistory.CALL_LOG_OF_TYPE = LogFor;
            phdCallLogHistory.DELETED = false;
            phdCallLogHistory.CREATED_BY = phdCallLogHistory.MODIFIED_BY = profile.UserName;
            phdCallLogHistory.CREATED_DATE = phdCallLogHistory.MODIFIED_DATE = Helper.GetCurrentDate();
            _phdCallLogHistoryRepository.Insert(phdCallLogHistory);
            _phdCallLogHistoryRepository.Save();
        }
        public List<WebSoftCaseStatusResponse> GetWebSoftCaseStatusResponses(string sscmCaseNumber)
        {
            try
            {
                var result = new List<WebSoftCaseStatusResponse>();
                if (!string.IsNullOrEmpty(sscmCaseNumber))
                {
                    SqlParameter sscmCaseNumberStr = new SqlParameter { ParameterName = "Caseno", SqlDbType = SqlDbType.VarChar, Value = sscmCaseNumber };
                    result = SpRepository<WebSoftCaseStatusResponse>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_MTBCSOFT_WEB_CASE_STATUS_PREV_COMMENTS @Caseno", sscmCaseNumberStr).ToList();
                    if (result == null)
                    {
                        return new List<WebSoftCaseStatusResponse>();
                    }
                }
                return result;
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }
        public List<CallHandlingDefaultValues> GetPhdCallScenariosList(string req, UserProfile profile)
        {
            List<CallHandlingDefaultValues> obj = new List<CallHandlingDefaultValues>();
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var SearchText = new SqlParameter { ParameterName = "@SEARCH_TEXT", SqlDbType = SqlDbType.VarChar, Value = string.IsNullOrEmpty(req) ? "" : req };
            obj = SpRepository<CallHandlingDefaultValues>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_CALL_HANDLING_DEFAULT_VALUES @PRACTICE_CODE,@SEARCH_TEXT", PracticeCode, SearchText);
            return obj;
        }
        public List<PhdCallScenario> GetPhdCallScenarios(UserProfile profile)
        {
            List<PhdCallScenario> res = new List<PhdCallScenario>();
            res = _PhdCallScenarioRepository.GetMany(s => s.PRACTICE_CODE == profile.PracticeCode && s.DELETED == false).OrderBy(o => o.NAME).ToList();
            return res;
        }
        public ResponseModel SavePhdScanarios(List<DefaultVauesForPhdUsers> obj, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            DefaultVauesForPhdUsers phdSanariosObj = new DefaultVauesForPhdUsers();
            foreach (var value in obj)
            {
                var phdSanarios = _DefaultVauesForPhdUsersRepository.GetFirst(x => x.USER_ID == value.USER_ID && x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED);
                if (phdSanarios != null)
                {
                    phdSanarios.PHD_CALL_SCENARIO_ID = value.PHD_CALL_SCENARIO_ID;
                    phdSanarios.MODIFIED_BY = profile.UserName;
                    phdSanarios.MODIFIED_DATE = Helper.GetCurrentDate();
                    _DefaultVauesForPhdUsersRepository.Update(phdSanarios);
                    _DefaultVauesForPhdUsersRepository.Save();
                    response.ErrorMessage = "Record updated successfully";
                    response.Success = true;

                }
                else
                {
                    var test = Helper.getMaximumId("DAEAULT_HANDLING_ID");
                    phdSanariosObj.DAEAULT_HANDLING_ID = Helper.getMaximumId("DAEAULT_HANDLING_ID");
                    phdSanariosObj.USER_ID = value.USER_ID;
                    phdSanariosObj.PHD_CALL_SCENARIO_ID = value.PHD_CALL_SCENARIO_ID;
                    phdSanariosObj.PRACTICE_CODE = profile.PracticeCode;
                    phdSanariosObj.DELETED = false;
                    phdSanariosObj.CREATED_BY = profile.UserName;
                    phdSanariosObj.CREATED_DATE = Helper.GetCurrentDate();
                    phdSanariosObj.MODIFIED_BY = profile.UserName;
                    phdSanariosObj.MODIFIED_DATE = Helper.GetCurrentDate();
                    _DefaultVauesForPhdUsersRepository.Insert(phdSanariosObj);
                    _DefaultVauesForPhdUsersRepository.Save();
                    response.ErrorMessage = "Record inserted successfully";
                    response.Success = true;
                }
            }
            return response;
        }

        public DefaultVauesForPhdUsers GetDefaultHandlingValue(UserProfile profile)
        {
            var user = _userRepository.GetFirst(x => x.USER_NAME == profile.UserName && !x.DELETED);
            var phdSanarios = _DefaultVauesForPhdUsersRepository.GetFirst(x => x.USER_ID == user.USER_ID && x.PRACTICE_CODE == user.PRACTICE_CODE && !x.DELETED);
            return phdSanarios;

        }
        //Description:  This function is trigger to Add faqs
        public ResponseModel AddUpdatePhdFaqsDetail(PhdFaqsDetail objPHDFAQsDetail, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                PhdFaqsDetail phdFaqsDetail = new PhdFaqsDetail();
                if (objPHDFAQsDetail != null && !string.IsNullOrEmpty(objPHDFAQsDetail.QUESTIONS.ToString()) && !string.IsNullOrEmpty(objPHDFAQsDetail.ANSWERS.ToString()) && profile.PracticeCode != 0 && !string.IsNullOrEmpty(objPHDFAQsDetail.FAQS_ID.ToString()))
                {
                    var existingDetailInfo = _phdFaqsDetailRepository.GetFirst(r => r.FAQS_ID == objPHDFAQsDetail.FAQS_ID && r.DELETED == false);
                    if (existingDetailInfo == null)
                    {
                        phdFaqsDetail.FAQS_ID = Helper.getMaximumId("FAQS_ID");
                        phdFaqsDetail.QUESTIONS = objPHDFAQsDetail.QUESTIONS.ToString();
                        phdFaqsDetail.ANSWERS = objPHDFAQsDetail.ANSWERS.ToString();
                        phdFaqsDetail.PRACTICE_CODE = profile.PracticeCode;
                        phdFaqsDetail.DELETED = false;
                        phdFaqsDetail.CREATED_BY = phdFaqsDetail.MODIFIED_BY = profile.UserName;
                        phdFaqsDetail.CREATED_DATE = phdFaqsDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                        _phdFaqsDetailRepository.Insert(phdFaqsDetail);
                        _phdFaqsDetailRepository.Save();
                        response.Message = "FAQ inserted successfully";
                        response.Success = true;                     
                    }
                    else
                    {
                        existingDetailInfo.FAQS_ID = objPHDFAQsDetail.FAQS_ID;
                        existingDetailInfo.QUESTIONS = objPHDFAQsDetail.QUESTIONS.ToString();
                        existingDetailInfo.ANSWERS = objPHDFAQsDetail.ANSWERS.ToString();
                        existingDetailInfo.PRACTICE_CODE = profile.PracticeCode;
                        existingDetailInfo.DELETED = false;
                        existingDetailInfo.MODIFIED_BY = profile.UserName;
                        existingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                        _phdFaqsDetailRepository.Update(existingDetailInfo);
                        _phdFaqsDetailRepository.Save();
                        response.Message = "FAQ updated successfully";
                        response.Success = true;
                    }
                }
                else
                {
                    response.ErrorMessage = "Please Enter a Question or Answer";
                    response.Success = false;
                }
               
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return response;
        }
        //Description:  This function is trigger to soft delete faqs
        public ResponseModel DeletePhdFaqs(PhdFaqsDetail objPhdFaqsDetail, UserProfile profile)
        {
                ResponseModel response = new ResponseModel();
                if (objPhdFaqsDetail != null && profile.PracticeCode != 0 && !string.IsNullOrEmpty(objPhdFaqsDetail.FAQS_ID.ToString()))
                { 
                    var existingDetailInfo = _phdFaqsDetailRepository.GetFirst(r => r.FAQS_ID == objPhdFaqsDetail.FAQS_ID && r.DELETED == false);
                    if (existingDetailInfo != null)
                    {
                        existingDetailInfo.MODIFIED_BY = profile.UserName;
                        existingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                        existingDetailInfo.DELETED = true;
                        _phdFaqsDetailRepository.Update(existingDetailInfo);
                        _phdFaqsDetailRepository.Save();
                        response.ErrorMessage = "";
                        response.Message = "FAQ deleted successfully";
                        response.Success = true;
                    }
                    else
                    {
                        response.ErrorMessage = "";
                        response.Message = "FAQ deleted successfully";
                        response.Success = true;
                    }
                }
                return response;
        }
        //Description:  This function is trigger to get dropdown list of faqs
        public List<PhdFaqsDetail> GetDropdownListFaqs(UserProfile profile)
        {
            List <PhdFaqsDetail> faqsInfoList = new List<PhdFaqsDetail>();
            if(profile != null && profile.PracticeCode != 0)
            {
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                faqsInfoList = SpRepository<PhdFaqsDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PHD_FAQS_LIST  @PRACTICE_CODE ", PracticeCode);
            }
            return faqsInfoList;
        }
        //Description:  This function is trigger for smart search 
        public List<PhdFaqsDetail> GetPHDFaqsDetailsInformation(PhdFaqsDetail objPhdFaqsDetail, UserProfile profile)
        {
            List<PhdFaqsDetail> phdDetailsList = new List<PhdFaqsDetail>();
            if (objPhdFaqsDetail != null && !string.IsNullOrEmpty(objPhdFaqsDetail.QUESTIONS?.ToString()) && profile.PracticeCode != 0)
            {
                var PracticeCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var SearchText = new SqlParameter { ParameterName = "@SEARCH_TEXT", SqlDbType = SqlDbType.VarChar, Value = string.IsNullOrEmpty(objPhdFaqsDetail.QUESTIONS) ? "" : objPhdFaqsDetail.QUESTIONS };
                phdDetailsList = SpRepository<PhdFaqsDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PHD_FAQs_DETAILS @PRACTICE_CODE, @SEARCH_TEXT ",
                PracticeCode, SearchText);
            }
            return phdDetailsList;
        }
    }
}

