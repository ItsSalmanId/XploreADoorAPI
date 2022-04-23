using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.PatientDocumentsService;
using FOX.DataModels;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.PatientDocuments;
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

namespace FOX.BusinessOperations.PatientDocumentsService
{
    public class PatientDocumentsService : IPatientDocumentsService
    {
        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();
        private readonly DBContextPatientDocuments _PatientPATDocumentContext = new DBContextPatientDocuments();
        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<FoxDocumentType> _foxdocumenttypeRepository;
        private readonly GenericRepository<PatientPATDocument> _foxPatientPATdocumentRepository;
        private readonly GenericRepository<FOX_VW_CASE> _casesViewRepository;
        private readonly GenericRepository<PatientDocumentFiles> _foxPatientdocumentFilesRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFilesRepository;
        private readonly GenericRepository<InterfaceSynchModel> __InterfaceSynchModelRepository;
        private readonly GenericRepository<OriginalQueue> _OriginalQueueRepository;
        private readonly GenericRepository<FoxSpecialityProgram> _foxspecialityprogramRepository;

        public PatientDocumentsService()
        {
            _foxdocumenttypeRepository = new GenericRepository<FoxDocumentType>(_IndexinfoContext);
            _foxPatientPATdocumentRepository = new GenericRepository<PatientPATDocument>(_PatientPATDocumentContext);
            _casesViewRepository = new GenericRepository<FOX_VW_CASE>(_CaseContext);
            _foxPatientdocumentFilesRepository = new GenericRepository<PatientDocumentFiles>(_PatientPATDocumentContext);
            _OriginalQueueFilesRepository = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            _OriginalQueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            __InterfaceSynchModelRepository = new GenericRepository<InterfaceSynchModel>(_CaseContext);
            _foxspecialityprogramRepository = new GenericRepository<FoxSpecialityProgram>(_IndexinfoContext);


        }
        public DocumenttypeAndpatientcases getDocumentTypes(string patientAccount, UserProfile profile)
        {
            DocumenttypeAndpatientcases ObjDocumenttypeAndpatientcases = new DocumenttypeAndpatientcases();
            //ObjDocumenttypeAndpatientcases.DocumentTypes = _foxdocumenttypeRepository.GetMany(d => !d.DELETED && !d.IS_ONLINE_ORDER_LIST).OrderBy(o => o.NAME).ToList();
            var AccountNo = Helper.getDBNullOrValue("Patient_Account", patientAccount);
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            ObjDocumenttypeAndpatientcases.DocumentTypes = SpRepository<FoxDocumentType>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_DOCUMENT_TYPES @PATIENT_ACCOUNT, @PRACTICE_CODE", AccountNo, PracticeCode);
            ObjDocumenttypeAndpatientcases.PatientCasesList = _casesViewRepository.GetMany(c => c.PATIENT_ACCOUNT.ToString() == patientAccount && c.PRACTICE_CODE == profile.PracticeCode && !c.DELETED).Select(
                t => new caseViewDataModelView
                {
                    CASE_ID = t.CASE_ID,
                    PRACTICE_CODE = t.PRACTICE_CODE,
                    PATIENT_ACCOUNT = t.PATIENT_ACCOUNT,
                    CASE_TYPE_ID = t.CASE_TYPE_ID,
                    CASE_TYPE_NAME = t.CASE_TYPE_NAME,
                    CASE_NO = t.CASE_NO,
                    RT_CASE_NO = t.RT_CASE_NO,
                    CASE_STATUS_ID = t.CASE_STATUS_ID,
                    CASE_STATUS_NAME = t.CASE_STATUS_NAME,
                    WORK_ID = t.WORK_ID
                }
                ).ToList();
            return ObjDocumenttypeAndpatientcases;
        }
        public List<FoxDocumentType> GetAllDocumentTypes(UserProfile profile)
        {
            var DocumentTypes = _foxdocumenttypeRepository.GetMany(d => !d.DELETED && (d.IS_ACTIVE ?? true)).OrderBy(o => o.NAME).ToList();
            return DocumentTypes;
        }
        public List<FoxSpecialityProgram> GetAllSpecialityProgram(UserProfile profile)
        {
            var SpecialityPrograms = _foxspecialityprogramRepository.GetMany(d => !d.DELETED && d.PRACTICE_CODE == profile.PracticeCode).OrderBy(o => o.PROGRAM_NAME).ToList();          
            return SpecialityPrograms;
        }
        public List<FoxDocumentType> GetAllDocumentTypeswithInactive(UserProfile profile)
        {
            var DocumentTypes = _foxdocumenttypeRepository.GetMany(d => !d.DELETED).OrderBy(o => o.NAME).ToList();
            return DocumentTypes;
        }
        public List<PatientDocument> getDocumentsDataInformation(UserProfile profile, PatientDocumentRequest ObjPatientDocumentRequest)
        {
            try
            {
                var AccountNo = Helper.getDBNullOrValue("Patient_Account", ObjPatientDocumentRequest.PATIENT_ACCOUNT);
                var DocumentTypeID = Helper.getDBNullOrValue("DOCUMENT_TYPE_ID", ObjPatientDocumentRequest.DOCUMENT_TYPE_ID);
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = ObjPatientDocumentRequest.CURRENT_PAGE };
                var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = ObjPatientDocumentRequest.RECORD_PER_PAGE };
                var SortBy = Helper.getDBNullOrValue("SORT_BY", ObjPatientDocumentRequest.SORT_BY);
                var SortOrder = Helper.getDBNullOrValue("SORT_ORDER", ObjPatientDocumentRequest.SORT_ORDER);
                var PatientsInfoList = SpRepository<PatientDocument>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_PAT_DOCUMENTS @PATIENT_ACCOUNT,@DOCUMENT_TYPE_ID, @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                   AccountNo, DocumentTypeID, PracticeCode, CurrentPage, RecordPerPage, SortBy, SortOrder);
                return PatientsInfoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ResponseModel AddUpdateNewDocumentInformation(PatientPATDocument ObjPatientPATDocument, UserProfile profile)
        {
            try
            {
                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                PatientPATDocument ExistingDocumentInfo = new PatientPATDocument();
                string AddorUpdate = "";
                var COUNT = 0;
                if (ObjPatientPATDocument != null)
                {
                    var PatientAccount = Convert.ToInt64(ObjPatientPATDocument.PATIENT_ACCOUNT_str);
                    interfaceSynch.PATIENT_ACCOUNT = PatientAccount;
                    if (!string.IsNullOrEmpty(ObjPatientPATDocument.START_DATE_str))
                    {
                        ObjPatientPATDocument.START_DATE = Convert.ToDateTime(ObjPatientPATDocument.START_DATE_str);
                    }
                    if (!string.IsNullOrEmpty(ObjPatientPATDocument.END_DATE_str))
                    {
                        ObjPatientPATDocument.END_DATE = Convert.ToDateTime(ObjPatientPATDocument.END_DATE_str);
                    }
                    if (ObjPatientPATDocument.WORK_ID != null)
                    {
                        ExistingDocumentInfo = _foxPatientPATdocumentRepository.GetFirst(r => r.PAT_DOCUMENT_ID == ObjPatientPATDocument.PAT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.WORK_ID == ObjPatientPATDocument.WORK_ID && r.DELETED == false);
                    }
                    else
                    {
                        ExistingDocumentInfo = _foxPatientPATdocumentRepository.GetFirst(r => r.PAT_DOCUMENT_ID == ObjPatientPATDocument.PAT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.DELETED == false);
                    }
                    if (ExistingDocumentInfo == null)   //Add
                    {
                        var selectedcases = ObjPatientPATDocument.CASE_LIST.FindAll(c => c.IS_CHECKED == true && c.IS_DISABLED == false);
                        if (selectedcases == null || selectedcases.Count() < 1)
                        {
                            if (ObjPatientPATDocument.WORK_ID != null)
                            {
                                var wqdoctype = _OriginalQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID).DOCUMENT_TYPE;
                                if (!String.Equals(ObjPatientPATDocument.DOCUMENT_TYPE.ToString(), wqdoctype.ToString(), StringComparison.Ordinal))
                                {
                                    var existingreferral = _OriginalQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID);
                                    existingreferral.DOCUMENT_TYPE = ObjPatientPATDocument.DOCUMENT_TYPE;
                                    _OriginalQueueRepository.Update(existingreferral);
                                    _OriginalQueueRepository.Save();
                                    if (!string.IsNullOrEmpty(ObjPatientPATDocument.COMMENTS) || !string.IsNullOrEmpty(ObjPatientPATDocument.START_DATE.ToString()) || !string.IsNullOrEmpty(ObjPatientPATDocument.END_DATE.ToString()) || ObjPatientPATDocument.SHOW_ON_PATIENT_PORTAL)
                                    {
                                        AddDocument(ObjPatientPATDocument, profile);
                                    }
                                }
                                else
                                {
                                    AddDocument(ObjPatientPATDocument, profile);
                                }
                            }
                            else
                            {
                                    AddDocument(ObjPatientPATDocument, profile);
                            }
                        }
                        else
                        {
                            foreach (var singlecase in selectedcases)
                            {
                                PatientPATDocument existingcase = new PatientPATDocument();
                                if (ObjPatientPATDocument.WORK_ID != null)
                                {
                                    var wqdoctype = _OriginalQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID).DOCUMENT_TYPE;
                                    if (!String.Equals(ObjPatientPATDocument.DOCUMENT_TYPE.ToString(), wqdoctype.ToString(), StringComparison.Ordinal))
                                    {
                                        var existingreferral = _OriginalQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID);
                                        existingreferral.DOCUMENT_TYPE = ObjPatientPATDocument.DOCUMENT_TYPE;
                                        _OriginalQueueRepository.Update(existingreferral);
                                        _OriginalQueueRepository.Save();
                                    }
                                    existingcase = _foxPatientPATdocumentRepository.GetFirst(r => r.PARENT_DOCUMENT_ID == ObjPatientPATDocument.PARENT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.PARENT_DOCUMENT_ID.HasValue && r.WORK_ID == ObjPatientPATDocument.WORK_ID && r.CASE_ID == singlecase.CASE_ID && r.DELETED == false);
                                }
                                else
                                {
                                    existingcase = _foxPatientPATdocumentRepository.GetFirst(r => r.PARENT_DOCUMENT_ID == ObjPatientPATDocument.PARENT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.PARENT_DOCUMENT_ID.HasValue && r.CASE_ID == singlecase.CASE_ID && r.DELETED == false);
                                }
                                if (existingcase == null)
                                {
                                    ObjPatientPATDocument.CASE_ID = singlecase.CASE_ID;
                                    if (ObjPatientPATDocument.WORK_ID != null)
                                    {
                                        var DocumentInfo = _foxPatientPATdocumentRepository.GetMany(r => r.WORK_ID == ObjPatientPATDocument.WORK_ID && r.PRACTICE_CODE == profile.PracticeCode && r.DELETED == false);
                                        if (DocumentInfo.Count() == 1)
                                        {
                                            ObjPatientPATDocument.DOCUMENT_PATH_LIST = _OriginalQueueFilesRepository.GetMany(i => i.WORK_ID == ObjPatientPATDocument.WORK_ID && !i.deleted).Select(
                                        t => new PatientDocumentFiles
                                        {
                                            DOCUMENT_PATH = t.FILE_PATH1,
                                            DOCUMENT_LOGO_PATH = t.FILE_PATH
                                        }
                                        ).ToList();
                                        }
                                    }
                                    var SavedDocID = AddDocument(ObjPatientPATDocument, profile);
                                    while (COUNT == 0)
                                    {
                                        ObjPatientPATDocument.PARENT_DOCUMENT_ID = SavedDocID;
                                        COUNT++;
                                    }
                                }
                            }
                        }
                        AddorUpdate = "document saved successfully.";
                    }
                    else  //Update
                    {
                        var selectedcases = ObjPatientPATDocument.CASE_LIST.FindAll(c => c.IS_CHECKED == true && c.IS_DISABLED == false);
                        if (selectedcases == null || selectedcases.Count() < 1)
                        {
                            if (ObjPatientPATDocument.WORK_ID != null)
                            {
                                var wqdoctype = _OriginalQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID).DOCUMENT_TYPE;
                                if (!String.Equals(ObjPatientPATDocument.DOCUMENT_TYPE.ToString(), wqdoctype.ToString(), StringComparison.Ordinal))
                                {
                                    var existingreferral = _OriginalQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID);
                                    existingreferral.DOCUMENT_TYPE = ObjPatientPATDocument.DOCUMENT_TYPE;
                                    existingreferral.MODIFIED_BY = profile.UserName;
                                    existingreferral.MODIFIED_DATE = Helper.GetCurrentDate();
                                    _OriginalQueueRepository.Update(existingreferral);
                                    _OriginalQueueRepository.Save();
                                    var recordswithsameworkID = _foxPatientPATdocumentRepository.GetMany(r => r.PAT_DOCUMENT_ID != ObjPatientPATDocument.PAT_DOCUMENT_ID && r.WORK_ID == ObjPatientPATDocument.WORK_ID && r.DELETED == false);
                                    if (recordswithsameworkID.Count() > 0)
                                    {
                                        foreach (var singlerecord in recordswithsameworkID)
                                        {
                                            singlerecord.DOCUMENT_TYPE = ObjPatientPATDocument.DOCUMENT_TYPE;
                                            singlerecord.MODIFIED_BY = profile.UserName;
                                            singlerecord.MODIFIED_DATE = Helper.GetCurrentDate();
                                            _foxPatientPATdocumentRepository.Update(singlerecord);
                                            _foxPatientPATdocumentRepository.Save();
                                        }
                                    }
                                        UpdateDocument(ExistingDocumentInfo, ObjPatientPATDocument, profile);
                                }
                                else
                                {
                                    UpdateDocument(ExistingDocumentInfo, ObjPatientPATDocument, profile);
                                }
                            }
                            else
                            {
                            UpdateDocument(ExistingDocumentInfo, ObjPatientPATDocument, profile);
                            }
                        }
                        else
                        {
                            UpdateDocument(ExistingDocumentInfo, ObjPatientPATDocument, profile);
                            foreach (var singlecase in selectedcases)
                            {
                                PatientPATDocument existingcase = new PatientPATDocument();
                                if (ObjPatientPATDocument.WORK_ID != null)
                                {
                                    var wqdoctype = _OriginalQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID).DOCUMENT_TYPE;
                                    if (!String.Equals(ObjPatientPATDocument.DOCUMENT_TYPE.ToString(), wqdoctype.ToString(), StringComparison.Ordinal))
                                    {
                                        var existingreferral = _OriginalQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID);
                                        existingreferral.DOCUMENT_TYPE = ObjPatientPATDocument.DOCUMENT_TYPE;
                                        _OriginalQueueRepository.Update(existingreferral);
                                        _OriginalQueueRepository.Save();
                                    }
                                    existingcase = _foxPatientPATdocumentRepository.GetFirst(r => r.PARENT_DOCUMENT_ID == ObjPatientPATDocument.PARENT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.PARENT_DOCUMENT_ID.HasValue && r.WORK_ID == ObjPatientPATDocument.WORK_ID && r.CASE_ID == singlecase.CASE_ID && r.DELETED == false);
                                }
                                else
                                {
                                    existingcase = _foxPatientPATdocumentRepository.GetFirst(r => r.PARENT_DOCUMENT_ID == ObjPatientPATDocument.PARENT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.PARENT_DOCUMENT_ID.HasValue && r.CASE_ID == singlecase.CASE_ID && r.DELETED == false);
                                }
                                if (existingcase == null)
                                {
                                    ObjPatientPATDocument.CASE_ID = singlecase.CASE_ID;
                                    if (ObjPatientPATDocument.WORK_ID != null)
                                    {
                                        var DocumentInfo = _foxPatientPATdocumentRepository.GetMany(r => r.WORK_ID == ObjPatientPATDocument.WORK_ID && r.PRACTICE_CODE == profile.PracticeCode && r.DELETED == false);
                                        if (DocumentInfo.Count() >= 1)
                                        {
                                            ObjPatientPATDocument.DOCUMENT_PATH_LIST = _OriginalQueueFilesRepository.GetMany(i => i.WORK_ID == ObjPatientPATDocument.WORK_ID && !i.deleted).Select(
                                        t => new PatientDocumentFiles
                                        {
                                            DOCUMENT_PATH = t.FILE_PATH1,
                                            DOCUMENT_LOGO_PATH = t.FILE_PATH
                                        }
                                        ).ToList();
                                        }
                                    }
                                    else
                                    {
                                        ObjPatientPATDocument.DOCUMENT_PATH_LIST = _foxPatientdocumentFilesRepository.GetMany(i => i.PAT_DOCUMENT_ID == ObjPatientPATDocument.PAT_DOCUMENT_ID && !i.DELETED && i.PRACTICE_CODE == profile.PracticeCode).Select(
                                    t => new PatientDocumentFiles
                                    {
                                        PRACTICE_CODE = t.PRACTICE_CODE,
                                        DOCUMENT_PATH = t.DOCUMENT_PATH,
                                        DOCUMENT_LOGO_PATH = t.DOCUMENT_LOGO_PATH
                                    }
                                    ).ToList();
                                    }

                                    var SavedDocID = AddDocument(ObjPatientPATDocument, profile);
                                    while (COUNT == 1)
                                    {
                                        ObjPatientPATDocument.PARENT_DOCUMENT_ID = SavedDocID;
                                        COUNT++;
                                    }
                                }
                                else
                                {
                                    //Nothing because already case already exists and not updated again
                                }
                            }
                        }
                        AddorUpdate = "Document updated successfully.";
                    }
                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(interfaceSynch, profile);
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
                    Message = "Not saved/updated Document",
                    Success = false
                };
                return response;
            }
        }
        public long AddDocument(PatientPATDocument ObjPatientPATDocument, UserProfile profile)
        {
            PatientPATDocument newObjPatientPATDocument = new PatientPATDocument()
            {
                PATIENT_ACCOUNT = ObjPatientPATDocument.PATIENT_ACCOUNT,
                PATIENT_ACCOUNT_str = ObjPatientPATDocument.PATIENT_ACCOUNT_str,
                PARENT_DOCUMENT_ID = ObjPatientPATDocument.PARENT_DOCUMENT_ID,
                PRACTICE_CODE = ObjPatientPATDocument.PRACTICE_CODE,
                WORK_ID = ObjPatientPATDocument.WORK_ID,
                DOCUMENT_TYPE = ObjPatientPATDocument.DOCUMENT_TYPE,
                CASE_ID = ObjPatientPATDocument.CASE_ID,
                CASE_LIST = ObjPatientPATDocument.CASE_LIST,
                START_DATE = ObjPatientPATDocument.START_DATE,
                END_DATE = ObjPatientPATDocument.END_DATE,
                SHOW_ON_PATIENT_PORTAL = ObjPatientPATDocument.SHOW_ON_PATIENT_PORTAL,
                COMMENTS = ObjPatientPATDocument.COMMENTS,
                DOCUMENT_PATH_LIST = ObjPatientPATDocument.DOCUMENT_PATH_LIST,
                //DOCUMENT_PATH = ObjPatientPATDocument.DOCUMENT_PATH,
                //DOCUMENT_LOGO_PATH = ObjPatientPATDocument.DOCUMENT_LOGO_PATH,
                CREATED_BY = ObjPatientPATDocument.CREATED_BY,
                CREATED_DATE = ObjPatientPATDocument.CREATED_DATE,
                MODIFIED_BY = ObjPatientPATDocument.MODIFIED_BY,
                MODIFIED_DATE = ObjPatientPATDocument.MODIFIED_DATE,
                DELETED = ObjPatientPATDocument.DELETED,
            };
            var Parent_ID = newObjPatientPATDocument.PARENT_DOCUMENT_ID;
            newObjPatientPATDocument.PAT_DOCUMENT_ID = Helper.getMaximumId("FOX_PAT_DOCUMENT_ID");
            var firsttimeID = newObjPatientPATDocument.PAT_DOCUMENT_ID;
            newObjPatientPATDocument.PATIENT_ACCOUNT = Convert.ToInt64(newObjPatientPATDocument.PATIENT_ACCOUNT_str);
            newObjPatientPATDocument.PARENT_DOCUMENT_ID = !string.IsNullOrEmpty(Parent_ID.ToString()) ? Parent_ID : !string.IsNullOrEmpty(firsttimeID.ToString()) ? firsttimeID : 0;
            newObjPatientPATDocument.PRACTICE_CODE = profile.PracticeCode;
            newObjPatientPATDocument.CREATED_BY = newObjPatientPATDocument.MODIFIED_BY = profile.UserName;
            newObjPatientPATDocument.CREATED_DATE = newObjPatientPATDocument.MODIFIED_DATE = Helper.GetCurrentDate();
            newObjPatientPATDocument.DELETED = false;
            _foxPatientPATdocumentRepository.Insert(newObjPatientPATDocument);
            _foxPatientPATdocumentRepository.Save();
            if (newObjPatientPATDocument.DOCUMENT_PATH_LIST?.Count > 0)
            {
                foreach (var DocPath in newObjPatientPATDocument.DOCUMENT_PATH_LIST)
                {
                    var ExistingImages = _foxPatientdocumentFilesRepository.GetFirst(i => i.PATIENT_DOCUMENT_FILE_ID == DocPath.PATIENT_DOCUMENT_FILE_ID && i.PRACTICE_CODE == profile.PracticeCode && !i.DELETED);
                    if (ExistingImages == null)
                    {
                        PatientDocumentFiles ObjPatientDocumentFiles = new PatientDocumentFiles();
                        ObjPatientDocumentFiles.PATIENT_DOCUMENT_FILE_ID = Helper.getMaximumId("PATIENT_DOCUMENT_FILE_ID");
                        ObjPatientDocumentFiles.PRACTICE_CODE = profile.PracticeCode;
                        ObjPatientDocumentFiles.PAT_DOCUMENT_ID = newObjPatientPATDocument.PAT_DOCUMENT_ID;
                        ObjPatientDocumentFiles.DOCUMENT_PATH = DocPath.DOCUMENT_PATH;
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
        public void UpdateDocument(PatientPATDocument ExistingDocumentInfo, PatientPATDocument ObjPatientPATDocument, UserProfile profile)
        {
                ExistingDocumentInfo.DOCUMENT_TYPE = ObjPatientPATDocument.DOCUMENT_TYPE;
                ExistingDocumentInfo.CASE_ID = ObjPatientPATDocument.CASE_ID;
                ExistingDocumentInfo.START_DATE = ObjPatientPATDocument.START_DATE;
                ExistingDocumentInfo.END_DATE = ObjPatientPATDocument.END_DATE;
                ExistingDocumentInfo.SHOW_ON_PATIENT_PORTAL = ObjPatientPATDocument.SHOW_ON_PATIENT_PORTAL;
                ExistingDocumentInfo.COMMENTS = ObjPatientPATDocument.COMMENTS;
                ExistingDocumentInfo.MODIFIED_BY = profile.UserName;
                ExistingDocumentInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                _foxPatientPATdocumentRepository.Update(ExistingDocumentInfo);
                _foxPatientPATdocumentRepository.Save();
        }
        public string ExportToExcelDocumentInformation(UserProfile profile, PatientDocumentRequest ObjPatientDocumentRequest)
        {
            try
            {
                string fileName = "Documents";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                ObjPatientDocumentRequest.CURRENT_PAGE = 1;
                ObjPatientDocumentRequest.RECORD_PER_PAGE = 0;
                var CalledFrom = "Patient_Documents";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<PatientDocument> result = new List<PatientDocument>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                //var InitialList = getDocumentsDataInformation(profile, ObjPatientDocumentRequest);
                //result = InitialList.FindAll(s => s.DOCUMENT_TYPE == ObjPatientDocumentRequest.SELECTED_DOCUMENT_TYPE);
                result = getDocumentsDataInformation(profile, ObjPatientDocumentRequest);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<PatientDocument>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PatientDocument> getDocumentImagesInformation(UserProfile profile, PatientDocumentRequest ObjPatientDocumentRequest)
        {
            try
            {
                int RECORD_PER_PAGE = 0;
                if(ObjPatientDocumentRequest.IS_DOCUMENT_CLICKED == false)
                {
                    RECORD_PER_PAGE = ObjPatientDocumentRequest.RECORD_PER_PAGE;
                    
                }
                else
                {
                    ObjPatientDocumentRequest.CURRENT_PAGE = 1;
                }
                var AccountNo = Helper.getDBNullOrValue("Patient_Account", ObjPatientDocumentRequest.PATIENT_ACCOUNT);
                var DocumentTypeID = Helper.getDBNullOrValue("DOCUMENT_TYPE_ID", ObjPatientDocumentRequest.DOCUMENT_TYPE_ID);
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = ObjPatientDocumentRequest.CURRENT_PAGE };
                var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = RECORD_PER_PAGE };
                var SortBy = Helper.getDBNullOrValue("SORT_BY", ObjPatientDocumentRequest.SORT_BY);
                var SortOrder = Helper.getDBNullOrValue("SORT_ORDER", ObjPatientDocumentRequest.SORT_ORDER);
                var PatientsInfoList = SpRepository<PatientDocument>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_PAT_DOCUMENT_IMAGES @PATIENT_ACCOUNT,@DOCUMENT_TYPE_ID, @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                   AccountNo, DocumentTypeID, PracticeCode, CurrentPage, RecordPerPage, SortBy, SortOrder);
                foreach (var element in PatientsInfoList)
                {
                    if (element.WORK_ID != null)
                    {
                        var referral = _OriginalQueueRepository.GetFirst(t => t.PRACTICE_CODE == profile.PracticeCode && t.DELETED == false && t.WORK_ID == element.WORK_ID);
                        if (referral != null)
                        {
                            element.SORCE_NAME = referral.SORCE_NAME ?? "";
                            element.SORCE_TYPE = referral.SORCE_TYPE ?? "";
                        }
                    }
                    element.CASE_LIST = _casesViewRepository.GetMany(c => c.PATIENT_ACCOUNT.ToString() == ObjPatientDocumentRequest.PATIENT_ACCOUNT && c.PRACTICE_CODE == profile.PracticeCode && !c.DELETED).Select(
                t => new caseViewDataModelView
                {
                    CASE_ID = t.CASE_ID,
                    PRACTICE_CODE = t.PRACTICE_CODE,
                    PATIENT_ACCOUNT = t.PATIENT_ACCOUNT,
                    CASE_TYPE_ID = t.CASE_TYPE_ID,
                    CASE_TYPE_NAME = t.CASE_TYPE_NAME,
                    CASE_NO = t.CASE_NO,
                    RT_CASE_NO = t.RT_CASE_NO,
                    CASE_STATUS_ID = t.CASE_STATUS_ID,
                    CASE_STATUS_NAME = t.CASE_STATUS_NAME,
                    WORK_ID = t.WORK_ID
                }
                ).ToList();
                    foreach (var childelement in element.CASE_LIST)
                    {
                        PatientPATDocument existingcase = new PatientPATDocument();
                        if (element.WORK_ID != null)
                        {
                            existingcase = _foxPatientPATdocumentRepository.GetFirst(r => r.PARENT_DOCUMENT_ID == element.PARENT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.PARENT_DOCUMENT_ID.HasValue && r.WORK_ID == element.WORK_ID && r.CASE_ID == childelement.CASE_ID && r.DELETED == false);
                        }
                        else
                        {
                            existingcase = _foxPatientPATdocumentRepository.GetFirst(r => r.PARENT_DOCUMENT_ID == element.PARENT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.PARENT_DOCUMENT_ID.HasValue && r.CASE_ID == childelement.CASE_ID && r.DELETED == false);
                        }
                        if (existingcase != null)
                        {
                            childelement.IS_CHECKED = true;
                            childelement.IS_DISABLED = true;
                        }
                    }
                }
                return PatientsInfoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ResponseModel DeleteDocumentFilesInformation(UserProfile profile, PatientDocument objPatientDocument)
        {
            try
            {
                //if (objPatientDocument.WORK_ID != null)
                //{
                //    var ExistingDocumentFile = _OriginalQueueFilesRepository.GetFirst(r => r.WORK_ID == objPatientDocument.WORK_ID && r.FILE_ID == objPatientDocument.IMAGE_FILE_ID && !r.deleted);
                //    if (ExistingDocumentFile != null)
                //    {
                //        ExistingDocumentFile.deleted = true;
                //        _OriginalQueueFilesRepository.Update(ExistingDocumentFile);
                //        _OriginalQueueFilesRepository.Save();
                //    }
                //}
                //else
                //{
                var ExistingDocumentFile = _foxPatientdocumentFilesRepository.GetMany(r => r.PAT_DOCUMENT_ID == objPatientDocument.PAT_DOCUMENT_ID && r.PRACTICE_CODE == profile.PracticeCode && r.PATIENT_DOCUMENT_FILE_ID == objPatientDocument.IMAGE_FILE_ID && !r.DELETED).FirstOrDefault();
                if (ExistingDocumentFile != null)
                {
                    ExistingDocumentFile.DELETED = true;
                    _foxPatientdocumentFilesRepository.Update(ExistingDocumentFile);
                    _foxPatientdocumentFilesRepository.Save();
                }
                //}

                //for record delete if last image deleted
                DeleteDocumentInformation(objPatientDocument);
                ResponseModel response = new ResponseModel()
                {
                    ErrorMessage = "",
                    Message = "File Deleted",
                    Success = true
                };
                return response;
            }
            catch (Exception ex)
            {
                ResponseModel response = new ResponseModel()
                {
                    ErrorMessage = "",
                    Message = "File Not Deleted",
                    Success = false
                };
                return response;
            }
        }
        public void DeleteDocumentInformation(PatientDocument objPatientDocument)
        {
            GenericRepository<PatientDocumentFiles> _foxPatientdocumentFilesRepository = new GenericRepository<PatientDocumentFiles>(_PatientPATDocumentContext);
            var ExistingFiles = _foxPatientdocumentFilesRepository.GetMany(r => r.PAT_DOCUMENT_ID == objPatientDocument.PAT_DOCUMENT_ID && !r.DELETED).FirstOrDefault();
            if (EntityHelper.isTalkRehab)
            {
                if (ExistingFiles != null)
                {
                    var ExistingDocument = _foxPatientPATdocumentRepository.GetFirst(r => r.PAT_DOCUMENT_ID == objPatientDocument.PAT_DOCUMENT_ID && r.DELETED == false);
                    ExistingDocument.DELETED = true;
                    _foxPatientPATdocumentRepository.Update(ExistingDocument);
                    _foxPatientPATdocumentRepository.Save();
                    return;
                }
            }
            else
            {
            if (ExistingFiles == null)
            {
                var ExistingDocument = _foxPatientPATdocumentRepository.GetFirst(r => r.PAT_DOCUMENT_ID == objPatientDocument.PAT_DOCUMENT_ID && r.DELETED == false);
                ExistingDocument.DELETED = true;
                _foxPatientPATdocumentRepository.Update(ExistingDocument);
                _foxPatientPATdocumentRepository.Save();
                return;
            }
            }           
        }
        public void InsertInterfaceTeamData(InterfaceSynchModel obj, UserProfile Profile)
        {
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            if (obj.PATIENT_ACCOUNT == null && obj.CASE_ID == null && obj.TASK_ID == null)
            {

            }
            else
            {
                interfaceSynch.FOX_INTERFACE_SYNCH_ID = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                interfaceSynch.CASE_ID = obj.CASE_ID;
                interfaceSynch.Work_ID = obj.Work_ID;
                interfaceSynch.TASK_ID = obj.TASK_ID;
                interfaceSynch.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                interfaceSynch.PRACTICE_CODE = Profile.PracticeCode;
                interfaceSynch.MODIFIED_BY = interfaceSynch.CREATED_BY = Profile.UserName;
                interfaceSynch.MODIFIED_DATE = interfaceSynch.CREATED_DATE = DateTime.Now;
                interfaceSynch.DELETED = false;
                interfaceSynch.IS_SYNCED = false;
                __InterfaceSynchModelRepository.Insert(interfaceSynch);
                _CaseContext.SaveChanges();
            }
        }
    }
}
