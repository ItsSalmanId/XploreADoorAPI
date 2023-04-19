using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.EmailConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FOX.BusinessOperations.CommonServices;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using FOX.DataModels.Models.Settings.FacilityLocation;
using ZXing;
using System.Drawing;
using FOX.BusinessOperations.FaxServices;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.TasksModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.RequestForOrder;
using SautinSoft;
using System.Drawing.Imaging;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.DataModels.Models.GroupsModel;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.ExternalUserModel;
using HtmlAgilityPack;
using SelectPdf;
using System.Globalization;
using FOX.DataModels.Models.FoxPHD;
using FOX.DataModels.Models.SenderType;
using FOX.DataModels.Models.StatesModel;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FOX.BusinessOperations.GroupServices;
using FOX.DataModels;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using System.Net.Mail;
using System.Web.Configuration;

namespace FOX.BusinessOperations.IndexInfoServices
{
    public class IndexInfoService : IIndexInfoService
    {
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFiles;

        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();
        private readonly DbContextPatient _PatientContext = new DbContextPatient();
        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly DbContextFrictionless _dbContextFrictionLess = new DbContextFrictionless();

        private readonly DbContextSettings _settings = new DbContextSettings();
        private readonly DbContextTasks _TaskContext = new DbContextTasks();
        private readonly DBContextFoxPHD _PHDDBContext = new DBContextFoxPHD();
        private readonly DbContextCommon _DbContextCommon = new DbContextCommon();
        //private readonly GenericRepository<FOX_TBL_SENDER> _SenderRepository;
        private readonly GenericRepository<FOX_TBL_NOTES_HISTORY> _NotesRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT_DIAGNOSIS> _InsertDiagnosisRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT_PROCEDURE> _InsertProceuresRepository;
        private readonly GenericRepository<OriginalQueue> _InsertSourceAddRepository;
        private readonly GenericRepository<IndexPatReq> _updatePatAddRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT_DIAGNOSIS> _DeleteDiagnosisRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT_PROCEDURE> _DeleteProcedureRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT_DOCUMENTS> _InsertUpdateRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT_DOCUMENTS> _DeleteDocRepository;
        private readonly GenericRepository<ReferralSource> _InsertUpdateOrderingSourceRepository;
        private readonly GenericRepository<ReferralRegion> _ReferralRegionRepository;
        private readonly GenericRepository<Patient> _PatientRepository;
        private readonly GenericRepository<PatientAddress> _PatientAddressRepository;
        private readonly GenericRepository<EmailConfig> _EmailConfigRepository;
        private readonly GenericRepository<NotesHistory> _NotesHistoryRepository;
        private readonly GenericRepository<User> _User;
        private readonly GenericRepository<FacilityLocation> _FacilityLocationRepository;
        private readonly IFaxService _IFaxService = new FaxService();
        private readonly GenericRepository<PatientPOSLocation> _PatientPOSLocationRepository;
        private readonly GenericRepository<Referral_Physicians> _referral_physiciansRepository;
        private readonly GenericRepository<FoxDocumentType> _foxdocumenttypeRepository;
        private readonly GenericRepository<InterfaceSynchModel> __InterfaceSynchModelRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TYPE> _taskTypeRepository;
        private readonly GenericRepository<FOX_TBL_TASK> _TaskRepository;
        private readonly GenericRepository<TaskLog> _taskLogRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TASK_SUB_TYPE> _TaskTaskSubTypeRepository;
        private readonly GenericRepository<FOX_TBL_TASK_SUB_TYPE> _taskSubTypeRepository;
        private readonly GenericRepository<FrictionLessReferral> _frictionlessReferralRepository;
        private readonly GenericRepository<PatientInsurance> _PatientInsuranceRepository;
        private readonly GenericRepository<FoxInsurancePayers> _foxInsurancePayersRepository;
        private readonly GenericRepository<GROUP> _groupRepository;
        private readonly GenericRepository<PracticeOrganization> _fox_tbl_practice_organization;
        private readonly GenericRepository<DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER> _fox_tbl_identifier;
        private readonly GenericRepository<Speciality> _speciality;
        private readonly GenericRepository<MedicareLimit> _MedicareLimitRepository;
        private readonly GenericRepository<MedicareLimitType> _MedicareLimitTypeRepository;
        private readonly GenericRepository<FinancialClass> _financialClassRepository;
        private readonly GenericRepository<FOX_TBL_ELIG_HTML> _eligHtmlRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT> _FoxTblPatientRepository;
        private readonly GenericRepository<DsFoxOcr> _DsFoxOcrRepository;
        private readonly GenericRepository<FoxOcrStatus> _OcrStatusRepository;
        private readonly GenericRepository<PHDCallDetail> _phdDetailRepository;
        private readonly GenericRepository<FOX_TBL_SENDER_TYPE> _SenderTypeRepository;
        private readonly GenericRepository<ReferralSender> _referralSenderTypeRepository;
        private readonly GenericRepository<FOX_TBL_REFERRAL_SOURCE> _referralSourceTableRepository;
        private readonly GenericRepository<GROUP> _UserGroupseRepository;
        private readonly GenericRepository<FOX_TBL_ZIP_STATE_COUNTY> _zipStateCountyRepository;
        private readonly GenericRepository<RegionCoverLetter> _RegionCoverLetterRepository;
        private readonly GenericRepository<TaskWorkInterfaceMapping> _TaskWorkInterfaceMapping;
        private readonly GenericRepository<AdmissionImportantNotes> _admissionImportantNotes;
        private static List<Thread> threadsList = new List<Thread>();
        private static List<Thread> threadsListForEmail = new List<Thread>();
        private readonly GroupService _groupService;
        private long talkRehabWorkID = 0;
        private long talkRehabInterfaceID = 0;
        private long talkRehabTaskID = 0;
        private long retrycatch = 0;
        public IndexInfoService()
        {
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _OriginalQueueFiles = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);

            //_SenderRepository = new GenericRepository<FOX_TBL_SENDER>(_IndexinfoContext);
            _NotesRepository = new GenericRepository<FOX_TBL_NOTES_HISTORY>(_IndexinfoContext);
            _InsertDiagnosisRepository = new GenericRepository<FOX_TBL_PATIENT_DIAGNOSIS>(_IndexinfoContext);
            _InsertProceuresRepository = new GenericRepository<FOX_TBL_PATIENT_PROCEDURE>(_IndexinfoContext);
            _InsertSourceAddRepository = new GenericRepository<DataModels.Models.OriginalQueueModel.OriginalQueue>(_IndexinfoContext);
            _updatePatAddRepository = new GenericRepository<IndexPatReq>(_IndexinfoContext);
            _DeleteDiagnosisRepository = new GenericRepository<FOX_TBL_PATIENT_DIAGNOSIS>(_IndexinfoContext);
            _DeleteProcedureRepository = new GenericRepository<FOX_TBL_PATIENT_PROCEDURE>(_IndexinfoContext);
            _frictionlessReferralRepository = new GenericRepository<FrictionLessReferral>(_dbContextFrictionLess);
            _InsertUpdateRepository = new GenericRepository<FOX_TBL_PATIENT_DOCUMENTS>(_IndexinfoContext);
            _DeleteDocRepository = new GenericRepository<FOX_TBL_PATIENT_DOCUMENTS>(_IndexinfoContext);
            _InsertUpdateOrderingSourceRepository = new GenericRepository<ReferralSource>(_IndexinfoContext);
            _PatientRepository = new GenericRepository<Patient>(_PatientContext);
            _PatientAddressRepository = new GenericRepository<PatientAddress>(_PatientContext);
            _ReferralRegionRepository = new GenericRepository<ReferralRegion>(security);
            _EmailConfigRepository = new GenericRepository<EmailConfig>(security);
            _NotesHistoryRepository = new GenericRepository<NotesHistory>(security);
            _User = new GenericRepository<User>(security);
            _FacilityLocationRepository = new GenericRepository<FacilityLocation>(security);
            _PatientPOSLocationRepository = new GenericRepository<PatientPOSLocation>(_PatientContext);
            _referral_physiciansRepository = new GenericRepository<Referral_Physicians>(_settings);
            _foxdocumenttypeRepository = new GenericRepository<FoxDocumentType>(_IndexinfoContext);
            _taskTypeRepository = new GenericRepository<FOX_TBL_TASK_TYPE>(_TaskContext);
            __InterfaceSynchModelRepository = new GenericRepository<InterfaceSynchModel>(_CaseContext);
            _TaskRepository = new GenericRepository<FOX_TBL_TASK>(_TaskContext);
            _taskLogRepository = new GenericRepository<TaskLog>(_TaskContext);
            _TaskTaskSubTypeRepository = new GenericRepository<FOX_TBL_TASK_TASK_SUB_TYPE>(_TaskContext);
            _taskSubTypeRepository = new GenericRepository<FOX_TBL_TASK_SUB_TYPE>(_TaskContext);
            _PatientInsuranceRepository = new GenericRepository<PatientInsurance>(_PatientContext);
            _foxInsurancePayersRepository = new GenericRepository<FoxInsurancePayers>(_PatientContext);
            _groupRepository = new GenericRepository<GROUP>(_settings);
            _fox_tbl_practice_organization = new GenericRepository<PracticeOrganization>(_settings);
            _fox_tbl_identifier = new GenericRepository<DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER>(_CaseContext);
            _speciality = new GenericRepository<Speciality>(_CaseContext);
            _MedicareLimitRepository = new GenericRepository<MedicareLimit>(_PatientContext);
            _MedicareLimitTypeRepository = new GenericRepository<MedicareLimitType>(_PatientContext);
            _financialClassRepository = new GenericRepository<FinancialClass>(_PatientContext);
            _eligHtmlRepository = new GenericRepository<FOX_TBL_ELIG_HTML>(_PatientContext);
            _FoxTblPatientRepository = new GenericRepository<FOX_TBL_PATIENT>(_PatientContext);
            _DsFoxOcrRepository = new GenericRepository<DsFoxOcr>(_PatientContext);
            _OcrStatusRepository = new GenericRepository<FoxOcrStatus>(_PatientContext);
            _phdDetailRepository = new GenericRepository<PHDCallDetail>(_PHDDBContext);
            _SenderTypeRepository = new GenericRepository<FOX_TBL_SENDER_TYPE>(_DbContextCommon);
            _referralSenderTypeRepository = new GenericRepository<ReferralSender>(_DbContextCommon);
            _referralSourceTableRepository = new GenericRepository<FOX_TBL_REFERRAL_SOURCE>(_QueueContext);
            _UserGroupseRepository = new GenericRepository<GROUP>(_QueueContext);
            _zipStateCountyRepository = new GenericRepository<FOX_TBL_ZIP_STATE_COUNTY>(_settings);
            _RegionCoverLetterRepository = new GenericRepository<RegionCoverLetter>(security);
            _groupService = new GroupService();
            _TaskWorkInterfaceMapping = new GenericRepository<TaskWorkInterfaceMapping>(_TaskContext);
            _admissionImportantNotes = new GenericRepository<AdmissionImportantNotes>(_QueueContext);
        }
        public void InsertUpdateDocuments(FOX_TBL_PATIENT_DOCUMENTS obj, UserProfile profile)
        {
            var docDetail = _InsertUpdateRepository.GetByID(obj.PAT_DOC_ID);

            if (docDetail != null)
            {
                docDetail.COMENTS = obj.COMENTS;
                docDetail.FILE_NAME = obj.FILE_NAME;
                docDetail.FILE_PATH = obj.FILE_PATH;
                docDetail.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                docDetail.DELETED = obj.DELETED;
                docDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                docDetail.MODIFIED_BY = profile.UserName;
                _InsertUpdateRepository.Update(docDetail);
                _InsertUpdateRepository.Save();
            }
            else
            {

                obj.PAT_DOC_ID = Helper.getMaximumId("PAT_DOC_ID");
                obj.CREATED_BY = profile.UserName;
                obj.CREATED_DATE = Helper.GetCurrentDate();
                obj.DELETED = obj.DELETED;
                obj.MODIFIED_DATE = Helper.GetCurrentDate();
                obj.MODIFIED_BY = profile.UserName;
                obj.PROC_CODE = profile.PracticeCode;
                _InsertUpdateRepository.Insert(obj);
                _InsertUpdateRepository.Save();
            }
        }
        //public void InsertSourceSender(FOX_TBL_SENDER obj, UserProfile profile)
        //{
        //    var senderDetail = _SenderRepository.GetByID(obj.SENDER_ID);

        //    if (senderDetail != null)
        //    {
        //        senderDetail.SENDER_NAME = obj.SENDER_NAME;
        //        senderDetail.DELETED = obj.DELETED;
        //        senderDetail.MODIFIED_DATE = Helper.GetCurrentDate();
        //        senderDetail.MODIFIED_BY = profile.UserName;
        //        _SenderRepository.Update(senderDetail);
        //        _SenderRepository.Save();
        //    }
        //    else
        //    {
        //        // var sender = new FOX_TBL_SENDER();
        //        obj.SENDER_ID = Helper.getMaximumId("SENDER_ID");
        //        obj.CREATED_BY = profile.UserName;
        //        obj.CREATED_DATE = Helper.GetCurrentDate();
        //        obj.DELETED = obj.DELETED;
        //        obj.MODIFIED_DATE = Helper.GetCurrentDate();
        //        obj.MODIFIED_BY = profile.UserName;
        //        obj.PRACTICE_CODE = profile.PracticeCode;
        //        _SenderRepository.Insert(obj);
        //        _SenderRepository.Save();
        //    }
        //}

        public void InsertNotesHistory(FOX_TBL_NOTES_HISTORY obj, UserProfile profile)
        {
            SqlParameter note_Id = new SqlParameter("NOTE_ID", obj.NOTE_ID);
            SqlParameter work_Id = new SqlParameter("WORK_ID", obj.WORK_ID);
            SqlParameter practice_code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter note_desc = new SqlParameter("NOTE_DESC", string.IsNullOrEmpty(obj.NOTE_DESC) ? string.Empty : obj.NOTE_DESC);
            SqlParameter user_name = new SqlParameter("USER_NAME", profile.UserName);
            SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_INSERT_UPDATE_NOTES_HISTORY @NOTE_ID, @WORK_ID, @PRACTICE_CODE, @NOTE_DESC, @USER_NAME", note_Id, work_Id, practice_code, note_desc, user_name);
            //var notesDetail = _NotesRepository.GetByID(obj.NOTE_ID);

            //if (notesDetail != null)
            //{
            //    notesDetail.WORK_ID = obj.WORK_ID;
            //    notesDetail.NOTE_DESC = obj.NOTE_DESC;
            //    notesDetail.DELETED = obj.DELETED;
            //    notesDetail.MODIFIED_DATE = Helper.GetCurrentDate();
            //    notesDetail.MODIFIED_BY = profile.UserName;
            //    _NotesRepository.Update(notesDetail);
            //    _NotesRepository.Save();
            //}
            //else
            //{
            //    obj.NOTE_ID = Helper.getMaximumId("NOTE_ID");
            //    obj.CREATED_BY = profile.UserName;
            //    obj.CREATED_DATE = Helper.GetCurrentDate().ToString();
            //    obj.DELETED = obj.DELETED;
            //    obj.MODIFIED_DATE = Helper.GetCurrentDate();
            //    obj.MODIFIED_BY = profile.UserName;
            //    obj.PRACTICE_CODE = profile.PracticeCode;
            //    _NotesRepository.Insert(obj);
            //    _NotesRepository.Save();
            //}

            //Log Changes
            string logMsg = string.Format("ID: {0} A new Note(s) has been added.", obj.WORK_ID);
            string user = !string.IsNullOrEmpty(profile.FirstName) ? profile.FirstName + " " + profile.LastName : profile.UserName;
            Helper.LogSingleWorkOrderChange(obj.WORK_ID, obj.WORK_ID.ToString(), logMsg, user);
        }
        public void InsertDiagnosisInfo(FOX_TBL_PATIENT_DIAGNOSIS obj, UserProfile profile)
        {

            SqlParameter patDiagId = new SqlParameter("PAT_DIAG_ID", string.IsNullOrEmpty(obj.PAT_DIAG_ID.ToString()) ? 0 : obj.PAT_DIAG_ID); // ZERO CHECK
            SqlParameter workId = new SqlParameter("WORK_ID", string.IsNullOrEmpty(obj.WORK_ID.ToString()) ? 0 : obj.WORK_ID); // ZERO CHECK
            SqlParameter diagCode = new SqlParameter("DIAG_CODE", string.IsNullOrEmpty(obj.DIAG_CODE) ? string.Empty : obj.DIAG_CODE); // ZERO CHECK
            SqlParameter patAccount = new SqlParameter("PATIENT_ACCOUNT", !obj.PATIENT_ACCOUNT.HasValue ? 0 : obj.PATIENT_ACCOUNT.Value); // ZERO CHECK
            SqlParameter diagDesc = new SqlParameter("DIAG_DESC", string.IsNullOrEmpty(obj.DIAG_DESC) ? string.Empty : obj.DIAG_DESC);
            SqlParameter userName = new SqlParameter("USER_NAME", string.IsNullOrEmpty(profile.UserName) ? string.Empty : profile.UserName);
            SpRepository<FOX_TBL_PATIENT_DIAGNOSIS>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_UPDATE_DIAGNOSIS_INFO @PAT_DIAG_ID, @WORK_ID, @DIAG_CODE, @PATIENT_ACCOUNT, @DIAG_DESC, @USER_NAME",
                                                                                                        patDiagId, workId, diagCode, patAccount, diagDesc, userName);
            //var diagnosisDetail = _InsertDiagnosisRepository.GetByID(obj.PAT_DIAG_ID);

            //if (diagnosisDetail != null)
            //{
            //    diagnosisDetail.WORK_ID = obj.WORK_ID;
            //    diagnosisDetail.DIAG_CODE = obj.DIAG_CODE;
            //    diagnosisDetail.DIAG_DESC = obj.DIAG_DESC;
            //    diagnosisDetail.DELETED = obj.DELETED;
            //    diagnosisDetail.MODIFIED_DATE = Helper.GetCurrentDate();
            //    diagnosisDetail.MODIFIED_BY = profile.UserName;
            //    _InsertDiagnosisRepository.Update(diagnosisDetail);
            //    _InsertDiagnosisRepository.Save();
            //}
            //else
            //{//need to make dianamic wok id

            //    obj.PAT_DIAG_ID = Helper.getMaximumId("PAT_DIAG_ID");
            //    obj.CREATED_BY = profile.UserName;
            //    obj.CREATED_DATE = Helper.GetCurrentDate();
            //    obj.DELETED = obj.DELETED;
            //    obj.MODIFIED_DATE = Helper.GetCurrentDate();
            //    obj.MODIFIED_BY = profile.UserName;
            //    _InsertDiagnosisRepository.Insert(obj);
            //    _InsertDiagnosisRepository.Save();
            //}
        }
        public void DeleteDocuments(FOX_TBL_PATIENT_DOCUMENTS obj, UserProfile profile)
        {
            var docDetail = _DeleteDocRepository.GetByID(obj.PAT_DOC_ID);

            if (docDetail != null)
            {
                docDetail.DELETED = obj.DELETED;
                docDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                docDetail.MODIFIED_BY = profile.UserName;
                _DeleteDocRepository.Update(docDetail);
                _DeleteDocRepository.Save();
            }
            else
            {
            }
        }
        public void DeleteDiagnosis(FOX_TBL_PATIENT_DIAGNOSIS obj, UserProfile profile)
        {
            var diagnosisDetail = _DeleteDiagnosisRepository.GetByID(obj.PAT_DIAG_ID);

            if (diagnosisDetail != null)
            {
                diagnosisDetail.DELETED = obj.DELETED;
                diagnosisDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                diagnosisDetail.MODIFIED_BY = profile.UserName;
                _DeleteDiagnosisRepository.Update(diagnosisDetail);
                _DeleteDiagnosisRepository.Save();
            }
            else
            {
            }
        }


        public void DeleteProcedures(FOX_TBL_PATIENT_PROCEDURE obj, UserProfile profile)
        {
            var procedureDetail = _DeleteProcedureRepository.GetByID(obj.PAT_PROC_ID);

            if (procedureDetail != null)
            {
                procedureDetail.DELETED = obj.DELETED;
                procedureDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                procedureDetail.MODIFIED_BY = profile.UserName;
                _DeleteProcedureRepository.Update(procedureDetail);
                _DeleteProcedureRepository.Save();
            }
            else
            {
            }

        }
        public void InsertProceureInfo(FOX_TBL_PATIENT_PROCEDURE obj, UserProfile profile)
        {
            var procedureDetail = _InsertProceuresRepository.GetByID(obj.PAT_PROC_ID);

            if (procedureDetail != null)
            {
                procedureDetail.SPECIALITY_PROGRAM = obj.SPECIALITY_PROGRAM;
                procedureDetail.WORK_ID = obj.WORK_ID;
                procedureDetail.CPT_DESC = obj.CPT_DESC;
                procedureDetail.PROC_CODE = obj.PROC_CODE;
                procedureDetail.DELETED = obj.DELETED;
                procedureDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                procedureDetail.MODIFIED_BY = profile.UserName;
                _InsertProceuresRepository.Update(procedureDetail);
                _InsertProceuresRepository.Save();
            }
            else
            {//need to make dianamic wok id
                obj.PAT_PROC_ID = Helper.getMaximumId("PAT_PROC_ID");
                obj.CREATED_BY = profile.UserName;
                obj.CREATED_DATE = Helper.GetCurrentDate();
                obj.DELETED = obj.DELETED;
                obj.MODIFIED_DATE = Helper.GetCurrentDate();
                obj.MODIFIED_BY = profile.UserName;
                _InsertProceuresRepository.Insert(obj);
                _InsertProceuresRepository.Save();
            }
        }
        public void updataPatientInfo(IndexPatReq obj, UserProfile profile)
        {
            var patDetail = _updatePatAddRepository.GetByID(obj.Patient_Account);
            if (patDetail != null)
            {
                if (!string.IsNullOrEmpty(obj.Date_Of_Birth_In_String))
                    patDetail.Date_Of_Birth = Convert.ToDateTime(obj.Date_Of_Birth_In_String);
                else
                {
                    patDetail.Date_Of_Birth = null;
                }
                patDetail.First_Name = obj.First_Name;
                patDetail.Last_Name = obj.Last_Name;
                patDetail.Middle_Name = obj.Middle_Name;
                patDetail.Gender = obj.Gender;
                //patDetail.Chart_Id = obj.Chart_Id;
                patDetail.SSN = obj.SSN;
                patDetail.DELETED = obj.DELETED;
                patDetail.MODIFIED_BY = profile.UserName;
                patDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                //patDetail.PRACTICE_CODE = profile.PracticeCode;
                //_updatePatAddRepository.Update(patDetail);
                //_updatePatAddRepository.Save();

                var patientAccountNumber = new SqlParameter { ParameterName = "@Patient_Account", SqlDbType = SqlDbType.BigInt, Value = patDetail.Patient_Account };
                var dob = new SqlParameter { ParameterName = "@Date_Of_Birth", SqlDbType = SqlDbType.DateTime, Value = patDetail.Date_Of_Birth };
                var firstName = new SqlParameter { ParameterName = "@First_Name", SqlDbType = SqlDbType.VarChar, Value = patDetail.First_Name };
                var lastName = new SqlParameter { ParameterName = "@Last_Name", SqlDbType = SqlDbType.VarChar, Value = patDetail.Last_Name };
                var midName = new SqlParameter { ParameterName = "@Middle_Name", SqlDbType = SqlDbType.Char, Value = patDetail.Middle_Name };
                var gender = new SqlParameter { ParameterName = "@Gender", SqlDbType = SqlDbType.VarChar, Value = patDetail.Gender };
                var ssn = new SqlParameter { ParameterName = "@SSN", SqlDbType = SqlDbType.Char, Value = patDetail.SSN };
                var modifiedBy = new SqlParameter { ParameterName = "@MODIFIED_BY", SqlDbType = SqlDbType.VarChar, Value = patDetail.MODIFIED_BY };
                var modifiedDate = new SqlParameter { ParameterName = "@MODIFIED_DATE", SqlDbType = SqlDbType.DateTime, Value = patDetail.MODIFIED_DATE };
                var delete = new SqlParameter { ParameterName = "@DELETED", SqlDbType = SqlDbType.Bit, Value = patDetail.DELETED };


                if (patDetail.Date_Of_Birth == null)
                {
                    dob.Value = DBNull.Value;
                }
                if (patDetail.First_Name == null)
                {
                    firstName.Value = DBNull.Value;
                }
                if (patDetail.Last_Name == null)
                {
                    lastName.Value = DBNull.Value;
                }
                if (patDetail.Middle_Name == null)
                {
                    midName.Value = DBNull.Value;
                }
                if (patDetail.Gender == null)
                {
                    gender.Value = DBNull.Value;
                }
                if (patDetail.SSN == null)
                {
                    ssn.Value = DBNull.Value;
                }

                var result = SpRepository<IndexPatReq>.GetListWithStoreProcedure(@"exec FOX_PROC_UPDTAE_PATIENT_INFO 
                                    @Patient_Account, @Date_Of_Birth, @First_Name, @Last_Name, @Middle_Name, @Gender, @SSN, @MODIFIED_BY, @MODIFIED_DATE, @DELETED"
                                    , patientAccountNumber, dob, firstName, lastName, midName, gender, ssn, modifiedBy, modifiedDate, delete);

            }
            else
            {

            }

        }
        public OriginalQueue UpdateSource_AdditionalInfo(DataModels.Models.OriginalQueueModel.OriginalQueue obj, UserProfile profile)
        {
            if (obj.PATIENT_ACCOUNT != 0 && obj.WORK_ID != 0)
            {
                //OriginalQueue sourceAddDetail = null;
                // sourceAddDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                //sourceAddDetail.MODIFIED_BY = profile.UserName;
                SqlParameter workID = new SqlParameter("WORK_ID", obj.WORK_ID);
                SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                SqlParameter pat_account = new SqlParameter("PATIENT_ACCOUNT", obj.PATIENT_ACCOUNT);
                SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                var result = SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_UPDATE_MARKCOMPLETE_MODIFIEDDATE @WORK_ID, @PRACTICE_CODE, @PATIENT_ACCOUNT,@USER_NAME",
                                                                                                                                 workID, practiceCode, pat_account, userName);
                return result;
            }
            return null;
        }
        public OriginalQueue InsertSource_AdditionalInfo(DataModels.Models.OriginalQueueModel.OriginalQueue obj, UserProfile profile)
        {
            //Source Email Validation Vulnerability
            if (!obj.SORCE_NAME.Equals(profile.UserEmailAddress))
            {
                obj.SORCE_NAME = profile.UserEmailAddress;
            }
            // work id null check
            string user = !string.IsNullOrEmpty(profile.FirstName) ? profile.FirstName + " " + profile.LastName : profile.UserName;
            var listLogMsgs = new List<string>();
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            OriginalQueue sourceAddDetail = null;
            long? pat_account = null;
            if (obj.Patient_Account_Str == "0")
            {
                pat_account = null;
            }
            else
            {
                pat_account = long.Parse(obj.Patient_Account_Str);

            }
            if (obj.ISNoAssociate == true && pat_account != null)
            {
                var phdDetailId = long.Parse(obj.FOX_TBL_PHD_CALL_DETAIL_ID);
                var phdPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var phdCallDetailID = new SqlParameter("FOX_TBL_PHD_CALL_DETAIL_ID", SqlDbType.BigInt) { Value = phdDetailId };
                var pateintAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = pat_account };
                SpRepository<PHDCallDetail>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_UPDATE_PHD_CALL_DETAILS @PRACTICE_CODE, @FOX_TBL_PHD_CALL_DETAIL_ID, @PATIENT_ACCOUNT", phdPracticeCode, phdCallDetailID, pateintAccount);
            }
            //if (obj.WORK_ID != 0)
            //{
            //    sourceAddDetail = _InsertSourceAddRepository.GetByID(obj.WORK_ID);
            //}
            //else
            //{
            //    sourceAddDetail = _InsertSourceAddRepository.Get(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.WORK_ID == obj.WORK_ID && obj.WORK_ID != 0);
            //}
            long work_id = 0;
            if (obj.WORK_ID == 0)
            {
                work_id = Helper.getMaximumId("WORK_ID");
            }
            SqlParameter ID = new SqlParameter("ID", work_id);
            SqlParameter workID = new SqlParameter("WORK_ID", obj.WORK_ID);
            SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter createdBy = new SqlParameter("CREATED_BY", profile.UserName);
            SqlParameter isEmergencyOrder = new SqlParameter("IS_EMERGENGENCY_ORDER", false);
            SqlParameter supervisorStatus = new SqlParameter("SUPERVISOR_STATUS", false);
            SqlParameter sourceName = new SqlParameter("SORCE_NAME", profile.UserEmailAddress);
            SqlParameter sourceType = new SqlParameter("SORCE_TYPE", "Email");
            SqlParameter workStatus = new SqlParameter("WORK_STATUS", "Created");
            SqlParameter isVerifiedByRecepient = new SqlParameter("IS_VERIFIED_BY_RECEPIENT", false);
            SqlParameter selectedSource = new SqlParameter("FOX_SOURCE_CATEGORY_ID", obj?.FOX_SOURCE_CATEGORY_ID ?? 0);
            sourceAddDetail = SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_INSERT_ORIGNAL_QUEUE @ID, @WORK_ID, @PRACTICE_CODE, @CREATED_BY,@IS_EMERGENGENCY_ORDER,@SUPERVISOR_STATUS,@SORCE_NAME,@SORCE_TYPE, @WORK_STATUS,@IS_VERIFIED_BY_RECEPIENT,@FOX_SOURCE_CATEGORY_ID",
                                                                                                                                ID, workID, practiceCode, createdBy, isEmergencyOrder, supervisorStatus, sourceName, sourceType, workStatus, isVerifiedByRecepient, selectedSource);

            //if (sourceAddDetail == null)
            //{
            //    sourceAddDetail = new OriginalQueue();
            //    var workId = Helper.getMaximumId("WORK_ID");
            //    sourceAddDetail.WORK_ID = workId;
            //    sourceAddDetail.UNIQUE_ID = workId.ToString();
            //    sourceAddDetail.PRACTICE_CODE = profile.PracticeCode;
            //    sourceAddDetail.CREATED_BY = sourceAddDetail.MODIFIED_BY = profile.UserName;
            //    sourceAddDetail.CREATED_DATE = sourceAddDetail.MODIFIED_DATE = DateTime.Now;
            //    sourceAddDetail.IS_EMERGENCY_ORDER = false;
            //    sourceAddDetail.supervisor_status = false;
            //    sourceAddDetail.DELETED = false;
            //    sourceAddDetail.RECEIVE_DATE = sourceAddDetail.CREATED_DATE;
            //    sourceAddDetail.SORCE_NAME = profile.UserEmailAddress;
            //    sourceAddDetail.SORCE_TYPE = "Email";
            //    sourceAddDetail.WORK_STATUS = "Created";
            //    sourceAddDetail.IS_VERIFIED_BY_RECIPIENT = false;

            //    _InsertSourceAddRepository.Insert(sourceAddDetail);
            //    _InsertSourceAddRepository.Save();
            //}

            sourceAddDetail.FOX_SOURCE_CATEGORY_ID = obj.FOX_SOURCE_CATEGORY_ID ?? 0;
            if (string.IsNullOrEmpty(sourceAddDetail.DEPARTMENT_ID) || sourceAddDetail.DEPARTMENT_ID == "0")
            {
                sourceAddDetail.DEPARTMENT_ID = null;
            }
            if (!sourceAddDetail.DOCUMENT_TYPE.HasValue || sourceAddDetail.DOCUMENT_TYPE.Value == 0)
            {
                sourceAddDetail.DOCUMENT_TYPE = null;
            }
            if (!sourceAddDetail.SENDER_ID.HasValue || sourceAddDetail.SENDER_ID.Value == 0)
            {
                sourceAddDetail.SENDER_ID = null;
            }
            if (string.IsNullOrEmpty(sourceAddDetail.FACILITY_NAME) || sourceAddDetail.FACILITY_NAME == "0")
            {
                sourceAddDetail.FACILITY_NAME = "";
            }
            if (sourceAddDetail != null)
            {
                if (obj.IsSaved == true && obj.IsCompleted == false)
                {
                    //string logMsg = string.Format("ID: {0} has been saved and re-assigned to {1}.", obj.WORK_ID, Helper.GetFullName(obj.ASSIGNED_TO));
                    string logMsg = string.Format("ID: {0} has been saved.", sourceAddDetail.UNIQUE_ID);
                    listLogMsgs.Add(logMsg);

                    //***DO NOT REMOVE THIS***
                    //-----------------------------------------------------------------------------------------------------
                    //Asad: It is commented because index queue is removed and referral will just be saved and not indexed.
                    //sourceAddDetail.INDEXED_BY = profile.UserName;
                    //sourceAddDetail.INDEXED_DATE = Helper.GetCurrentDate();
                    //sourceAddDetail.WORK_STATUS = "Indexed";
                    //-----------------------------------------------------------------------------------------------------
                }
                if (obj.IsSaved == false && obj.IsCompleted == true)
                {
                    string logMsg = string.Format("ID: {0} Order has been Completed.", sourceAddDetail.UNIQUE_ID);
                    listLogMsgs.Add(logMsg);
                    if (sourceAddDetail.INDEXED_DATE == null)
                    {
                        sourceAddDetail.INDEXED_BY = profile.UserName;
                        sourceAddDetail.INDEXED_DATE = Helper.GetCurrentDate();
                    }
                    sourceAddDetail.COMPLETED_BY = profile.UserName;
                    sourceAddDetail.COMPLETED_DATE = Helper.GetCurrentDate();
                    sourceAddDetail.WORK_STATUS = "Completed";
                }
                if (obj.IsSaved == true && obj.IsCompleted == true)
                {
                    string logMsg = string.Format("ID: {0} Order has been Submitted.", sourceAddDetail.UNIQUE_ID);
                    listLogMsgs.Add(logMsg);

                    sourceAddDetail.COMPLETED_BY = profile.UserName;
                    sourceAddDetail.COMPLETED_DATE = Helper.GetCurrentDate();
                    sourceAddDetail.WORK_STATUS = "Completed";
                }
                if (sourceAddDetail.PATIENT_ACCOUNT != pat_account && pat_account != 0)
                {
                    string logMsg = string.Format("ID: {0} Patient '{1}' has been tagged to the order.", sourceAddDetail.UNIQUE_ID, Helper.GetPatientFullName(pat_account));
                    listLogMsgs.Add(logMsg);
                }
                if (sourceAddDetail.ACCOUNT_NUMBER != obj.ACCOUNT_NUMBER)
                {
                    string logMsg = string.Format("ID: {0} Account Number changed from '{1}' to '{2}'.", sourceAddDetail.UNIQUE_ID, !string.IsNullOrEmpty(sourceAddDetail.ACCOUNT_NUMBER) ? sourceAddDetail.ACCOUNT_NUMBER : "Empty"
                        , !string.IsNullOrEmpty(obj.ACCOUNT_NUMBER) ? obj.ACCOUNT_NUMBER : "Empty");
                    listLogMsgs.Add(logMsg);
                }
                sourceAddDetail.PATIENT_ACCOUNT = pat_account;

                sourceAddDetail.ACCOUNT_NUMBER = obj.ACCOUNT_NUMBER;
                if (sourceAddDetail.DOCUMENT_TYPE != obj.DOCUMENT_TYPE)
                {
                    string logMsg = string.Format("ID: {0} Document Type changed from '{1}' to '{2}'.", sourceAddDetail.UNIQUE_ID, Helper.GetDocumentName(sourceAddDetail.DOCUMENT_TYPE), Helper.GetDocumentName(obj.DOCUMENT_TYPE));
                    listLogMsgs.Add(logMsg);
                }
                sourceAddDetail.DOCUMENT_TYPE = obj.DOCUMENT_TYPE;
                //if (sourceAddDetail.FACILITY_NAME != obj.FACILITY_NAME)
                //{
                //    string logMsg = string.Format("ID: {0} Facility Name changed from '{1}' to '{2}'.", obj.UNIQUE_ID, !string.IsNullOrEmpty(sourceAddDetail.FACILITY_NAME) ? sourceAddDetail.FACILITY_NAME : "Empty"
                //        , !string.IsNullOrEmpty(obj.FACILITY_NAME) ? obj.FACILITY_NAME : "Empty");
                //    listLogMsgs.Add(logMsg);
                //}
                //sourceAddDetail.FACILITY_NAME = obj.FACILITY_NAME;
                if (sourceAddDetail.FACILITY_ID != obj.FACILITY_ID)
                {
                    string logMsg = string.Format("ID: {0} Facility Name changed from '{1}' to '{2}'.", sourceAddDetail.UNIQUE_ID, !string.IsNullOrEmpty(sourceAddDetail.FACILITY_NAME) ? sourceAddDetail.FACILITY_NAME : "Empty"
                        , !string.IsNullOrEmpty(obj.FACILITY_NAME) ? obj.FACILITY_NAME : "Empty");
                    listLogMsgs.Add(logMsg);
                }
                sourceAddDetail.FACILITY_NAME = obj.FACILITY_NAME;
                sourceAddDetail.FACILITY_ID = obj.FACILITY_ID;

                sourceAddDetail.IS_EMERGENCY_ORDER = obj.IS_EMERGENCY_ORDER;
                if (sourceAddDetail.REASON_FOR_VISIT != obj.REASON_FOR_VISIT)
                {
                    string logMsg = string.Format("ID: {0} A Reason for Visit has been added.", sourceAddDetail.UNIQUE_ID);
                    listLogMsgs.Add(logMsg);
                }
                sourceAddDetail.REASON_FOR_VISIT = obj.REASON_FOR_VISIT;
                if (sourceAddDetail.SENDER_ID != obj.SENDER_ID)
                {
                    string senderName = "";
                    if (obj.SENDER_ID != null && obj.SENDER_ID != 0)
                    {
                        senderName = GetSenderName(obj.SENDER_ID);
                    }
                    string senderNameOld = "";
                    if (sourceAddDetail.SENDER_ID != null)
                    {
                        senderNameOld = GetSenderName(sourceAddDetail.SENDER_ID);
                    }
                    string logMsg = string.Format("ID: {0} Sender Name changed from '{1}' to '{2}'.", sourceAddDetail.UNIQUE_ID, senderNameOld, senderName);
                }
                sourceAddDetail.SENDER_ID = obj.SENDER_ID;
                sourceAddDetail.UNIT_CASE_NO = obj.UNIT_CASE_NO;
               // if (!String.IsNullOrWhiteSpace(sourceAddDetail.DEPARTMENT_ID) && obj.DEPARTMENT_ID != "0" && sourceAddDetail.DEPARTMENT_ID != obj.DEPARTMENT_ID)
                if (obj.DEPARTMENT_ID != "0" && sourceAddDetail.DEPARTMENT_ID != obj.DEPARTMENT_ID)
                {
                    string logMsg = string.Format("ID: {0} Discipline changed from '{1}' to '{2}'.", sourceAddDetail.UNIQUE_ID, Helper.GetDepartmentNames(sourceAddDetail.DEPARTMENT_ID), Helper.GetDepartmentNames(obj.DEPARTMENT_ID));
                    listLogMsgs.Add(logMsg);
                }
               // if (!String.IsNullOrWhiteSpace(obj.DEPARTMENT_ID) && obj.DEPARTMENT_ID != "0")
                if (obj.DEPARTMENT_ID != "0")
                {
                    sourceAddDetail.DEPARTMENT_ID = obj.DEPARTMENT_ID;
                }

                sourceAddDetail.DELETED = obj.DELETED;
                sourceAddDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                sourceAddDetail.MODIFIED_BY = profile.UserName;

                //Edit By Ali

                //SPECIALITY_PROGRAM
                if (!string.IsNullOrWhiteSpace(obj?.SPECIALITY_PROGRAM))
                {
                    var procedureDetail = InsertUpdateSpecialty(obj, profile, sourceAddDetail, pat_account);

                    //var procedureDetail = _InsertProceuresRepository.Get(t => !t.DELETED && t.WORK_ID == sourceAddDetail.WORK_ID);//obj.WORK_ID;

                    //if (procedureDetail != null)
                    //{
                    //    procedureDetail.SPECIALITY_PROGRAM = obj.SPECIALITY_PROGRAM;
                    //    procedureDetail.WORK_ID = sourceAddDetail.WORK_ID;   //obj.WORK_ID;
                    //    procedureDetail.PATIENT_ACCOUNT = !string.IsNullOrEmpty(pat_account.ToString()) ? pat_account : null;
                    //    procedureDetail.DELETED = false;
                    //    procedureDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                    //    procedureDetail.MODIFIED_BY = profile.UserName;
                    //    _InsertProceuresRepository.Update(procedureDetail);
                    //    _InsertProceuresRepository.Save();
                    //}
                    //else
                    //{
                    //    procedureDetail = new FOX_TBL_PATIENT_PROCEDURE();

                    //    procedureDetail.PAT_PROC_ID = Helper.getMaximumId("PAT_PROC_ID");
                    //    procedureDetail.SPECIALITY_PROGRAM = obj.SPECIALITY_PROGRAM;
                    //    procedureDetail.WORK_ID = sourceAddDetail.WORK_ID;//obj.WORK_ID;
                    //    procedureDetail.PATIENT_ACCOUNT = !string.IsNullOrEmpty(pat_account.ToString()) ? pat_account : null;
                    //    procedureDetail.DELETED = false;
                    //    procedureDetail.CREATED_DATE = procedureDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                    //    procedureDetail.CREATED_BY = procedureDetail.MODIFIED_BY = profile.UserName;

                    //    _InsertProceuresRepository.Insert(procedureDetail);
                    //    _InsertProceuresRepository.Save();
                    //}
                }
                if (obj.IsRequestForOrder)
                {
                    sourceAddDetail.RFO_Type = "Fill_Form";
                    var _user = GetProfileUserById(profile);
                    //var _user = _User.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.USER_ID == profile.userID);//CONVERT TO SP
                    if (_user != null && (obj?.IsSigned ?? false) && (obj.IS_ORS ?? false))
                    {
                        sourceAddDetail.IsSigned = true;
                        sourceAddDetail.SignedBy = profile.userID;
                    }
                    else
                    {
                        sourceAddDetail.IsSigned = false;
                        sourceAddDetail.SignedBy = null;
                    }
                    sourceAddDetail.FOX_TBL_SENDER_TYPE_ID = obj?.FOX_TBL_SENDER_TYPE_ID;
                    sourceAddDetail.FOX_TBL_SENDER_NAME_ID = obj?.FOX_TBL_SENDER_NAME_ID;
                    sourceAddDetail.REASON_FOR_THE_URGENCY = obj?.REASON_FOR_THE_URGENCY;
                    sourceAddDetail.IS_POST_ACUTE = obj.IS_POST_ACUTE;
                    //if (obj?.IS_POST_ACUTE ?? false)
                    //{
                    //    sourceAddDetail.IS_EMERGENCY_ORDER = true;
                    //}
                    //SPECIALITY_PROGRAM
                    if (!string.IsNullOrWhiteSpace(obj?.SPECIALITY_PROGRAM) && obj.SPECIALITY_PROGRAM != "0" && obj.SPECIALITY_PROGRAM.Contains("Home Health, Part A Services"))//SPECIALITY_PROGRAM ZERO CHECK
                    {
                        var _user2 = GetSupervisorUser(profile);
                        //var _user2 = _User.Get(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.EMAIL.Contains(AppConfiguration.SupervisorEmailAddress)); // CONVERT TO SP
                        if (_user2 != null)
                        {
                            sourceAddDetail.ASSIGNED_TO = _user2.USER_NAME;
                            ////***DO NOT REMOVE THIS***
                            ////-----------------------------------------------------------------------------------------------------
                            ////Asad: It is commented because index queue is removed and referral will just be saved and not indexed.
                            ////sourceAddDetail.WORK_STATUS = "Indexed";
                            ////-----------------------------------------------------------------------------------------------------
                            sourceAddDetail.ASSIGNED_BY = profile.UserName;
                            sourceAddDetail.ASSIGNED_DATE = Helper.GetCurrentDate();
                        }
                    }
                    sourceAddDetail.IS_EVALUATE_TREAT = obj.IS_EVALUATE_TREAT;
                    sourceAddDetail.HEALTH_NAME = obj.HEALTH_NAME;
                    sourceAddDetail.HEALTH_NUMBER = obj.HEALTH_NUMBER;

                    //Diagnosis Insertion
                    if (obj.DIAGNOSIS.Count() > 0)
                    {
                        foreach (var dia in obj.DIAGNOSIS.AsEnumerable().Reverse())
                        {
                            dia.WORK_ID = sourceAddDetail.WORK_ID;
                            dia.PATIENT_ACCOUNT = pat_account;
                            InsertDiagnosisInfo(dia, profile);
                        }
                    }


                    if (obj.Is_Manual_ORS)
                    {
                        FOX_TBL_NOTES_HISTORY notes = new FOX_TBL_NOTES_HISTORY();
                        notes.NOTE_DESC = "Custom ordering referral source is added by the user. See the attached referral for details";
                        notes.WORK_ID = sourceAddDetail.WORK_ID;
                        InsertNotesHistory(notes, profile);
                    }
                }

                if (obj.IS_VERBAL_ORDER == true)
                {
                    sourceAddDetail.IS_VERBAL_ORDER = obj.IS_VERBAL_ORDER;
                    sourceAddDetail.VO_ON_BEHALF_OF = obj.VO_ON_BEHALF_OF;
                    sourceAddDetail.VO_RECIEVED_BY = obj.VO_RECIEVED_BY;
                    if (!string.IsNullOrEmpty(obj.VO_DATE_TIME_STR))
                    {
                        sourceAddDetail.VO_DATE_TIME = Helper.ConvertStingToDateTime(obj.VO_DATE_TIME_STR);//Convert.ToDateTime(obj.VO_DATE_TIME_STR);
                    }
                }
                else
                {
                    sourceAddDetail.IS_VERBAL_ORDER = false;
                    sourceAddDetail.VO_ON_BEHALF_OF = null;
                    sourceAddDetail.VO_RECIEVED_BY = null;
                    sourceAddDetail.VO_DATE_TIME = null;
                }


                if (obj.IS_POST_ACUTE == true)
                {
                    sourceAddDetail.IS_POST_ACUTE = obj.IS_POST_ACUTE;
                    sourceAddDetail.REASON_FOR_THE_URGENCY = obj.REASON_FOR_THE_URGENCY;
                    if (!string.IsNullOrEmpty(obj.EXPECTED_DISCHARGE_DATE_STR))
                    {
                        sourceAddDetail.EXPECTED_DISCHARGE_DATE = Helper.ConvertStingToDateTime(obj.EXPECTED_DISCHARGE_DATE_STR);
                    }
                }
                else
                {
                    sourceAddDetail.IS_POST_ACUTE = false;
                    sourceAddDetail.EXPECTED_DISCHARGE_DATE = null;
                }



                //    @VO_DATE_TIME DATETIME

                InsertUpdateAdditionalInfo(sourceAddDetail);

                //_InsertSourceAddRepository.Update(sourceAddDetail);
                //_InsertSourceAddRepository.Save();

                Helper.LogMultipleWorkOrderChanges(sourceAddDetail.WORK_ID, sourceAddDetail.UNIQUE_ID, listLogMsgs, user);
            }
            else
            {   //need to make dianamic wok id
                //obj.WORK_ID = 183710;
                //obj.COMPLETED_BY = profile.UserName;
                //obj.COMPLETED_DATE= Helper.GetCurrentDate();
                //obj.CREATED_BY = profile.UserName;
                //obj.CREATED_DATE = Helper.GetCurrentDate();
                //obj.DELETED = obj.DELETED;
                //obj.PRACTICE_CODE = profile.PracticeCode;
                //obj.MODIFIED_DATE = Helper.GetCurrentDate();
                //obj.MODIFIED_BY = profile.UserName;
                //_InsertSourceAddRepository.Insert(obj);
                //_InsertSourceAddRepository.Save();    
            }

            if (obj.IsSaved == false && obj.IsCompleted == true)
            {
                Patient tempPatient = UpdatePatientStatus(profile, sourceAddDetail, pat_account);
                //var Patient = _FoxTblPatientRepository.GetFirst(t => t.Patient_Account == pat_account && t.DELETED == false);
                //if(Patient !=null)
                //{
                //    Patient.Is_Opened_By = null;
                //    _FoxTblPatientRepository.Update(Patient);
                //    _FoxTblPatientRepository.Save();
                //}
                if (Helper.GetDocumentName(sourceAddDetail.DOCUMENT_TYPE).Equals("Unsigned Order") || Helper.GetDocumentName(sourceAddDetail.DOCUMENT_TYPE).Equals("Signed Order"))
                {
                    sendMail(sourceAddDetail.WORK_ID, profile, false);
                }

                ////Web Services Send Notification

                if (tempPatient != null && !string.IsNullOrWhiteSpace(sourceAddDetail?.GuestID ?? ""))
                {
                    //var PatName = _PatientRepository.GetMany(x => x.Patient_Account == sourceAddDetail.PATIENT_ACCOUNT).Select(t => new
                    //{
                    //    t.Last_Name,
                    //    t.First_Name
                    //}).FirstOrDefault();
                    var _respWSGetAccessToken = WSGetAccessToken();

                    if (_respWSGetAccessToken != null && _respWSGetAccessToken?.Result?.AccessTokenInfo != null)
                    {
                        var _respSendNotification = WSSendNotification(new ReqWSSendNotification()
                        {
                            GuestID = sourceAddDetail.GuestID,
                            Message = "Your referral has been completed for" + tempPatient.Last_Name + ", " + tempPatient.First_Name + "Order ID: " + sourceAddDetail.UNIQUE_ID
                        }, _respWSGetAccessToken.Result.AccessTokenInfo);
                    }
                }
            }
            else
            {
                ///**DO NOT REMOVE THIS***
                ////-----------------------------------------------------------------------------------------------------
                ////Asad: It is commented because index queue is removed and referral will just be saved and not indexed.
                ////sendMail(sourceAddDetail.WORK_ID, profile, true);
                ////-----------------------------------------------------------------------------------------------------
            }
            interfaceSynch.PATIENT_ACCOUNT = pat_account;
            //adding Work id in CASE ID of interface table to interface with correct word id, will add a column later in interface table
            interfaceSynch.Work_ID = obj.WORK_ID;
            var patient = GetInterfaceSynch(profile.PracticeCode, pat_account, obj.WORK_ID);// SpRepository<InterfaceSynchModel>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_INTERFACE_SYNCH @PRACTICE_CODE, @PATIENT_ACCOUNT, @Work_ID", pPracticeCode, pPatAccount, pWork_ID);
                                                                                            //var patient = __InterfaceSynchModelRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.PATIENT_ACCOUNT == pat_account && t.Work_ID == obj.WORK_ID && t.IS_SYNCED == true);
            sourceAddDetail.IS_INTERFACE = true;
            if (patient == null && sourceAddDetail.WORK_STATUS.ToLower() == "completed" && obj.IsSaved == false && obj.IsCompleted == true)
            {
                var IS_TASK_INTERFACED = GetInterfaceSynchList(profile.PracticeCode, pat_account, obj.WORK_ID);// SpRepository<InterfaceSynchModel>.GetListWithStoreProcedure(@"FOX_PROC_GET_INTERFACE_SYNCH @PRACTICE_CODE, @PATIENT_ACCOUNT, @Work_ID", pPracticeCode1, pPatAccount1, pWork_ID1);
                //var IS_TASK_INTERFACED = __InterfaceSynchModelRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode
                //                        && t.PATIENT_ACCOUNT == pat_account
                //                        && t.Work_ID == obj.WORK_ID
                //                        && t.TASK_ID != null);
                string tasktypeHBR = "";
                var pendingBalance = getPendingHighBalance(pat_account);
                FoxDocumentType documentType = GetDocumentType(obj);
                //var documentType = _foxdocumenttypeRepository.GetFirst(t => t.DOCUMENT_TYPE_ID == obj.DOCUMENT_TYPE && t.DELETED == false);

                //MSP
                FOX_TBL_TASK_TYPE MSP_TYPE_ID = GetTaskId(profile);
                //var MSP_TYPE_ID = _taskTypeRepository.GetFirst(t => t.NAME.ToLower() == "block" && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED)?.TASK_TYPE_ID ?? 0;
                //var MSP_TASK_EXIST = _TaskRepository.GetMany(t => t.TASK_TYPE_ID == MSP_TYPE_ID && t.PATIENT_ACCOUNT == pat_account && t.PRACTICE_CODE == profile.PracticeCode && t.DELETED == false).OrderByDescending(t => t.CREATED_DATE).FirstOrDefault();
                List<FOX_TBL_TASK> MSP_TASK_EXIST = GetMSPTaskList(profile, pat_account, MSP_TYPE_ID);
                //var MSP_TASK_EXIST = _TaskRepository.GetMany(t => t.TASK_TYPE_ID == MSP_TYPE_ID.TASK_TYPE_ID && t.PATIENT_ACCOUNT == pat_account && t.PRACTICE_CODE == profile.PracticeCode && t.DELETED == false).OrderByDescending(t => t.CREATED_DATE).ToList();

                if (IS_TASK_INTERFACED?.Count == 0)
                {
                    if (pendingBalance.Patient_Balance != null && pendingBalance.Patient_Balance >= 500 && pendingBalance.Statement_Patient_Balance >= 500 && pendingBalance.NoOfDays > 45 && (documentType.RT_CODE.ToLower() == "00001" || documentType.RT_CODE.ToLower() == "unsig" || documentType.RT_CODE.ToLower() == "forms" || documentType.RT_CODE.ToLower() == "order"))
                    {
                        tasktypeHBR = "BLOCK";
                        var interfaceTaskHBR = setTaskData(profile, pat_account, tasktypeHBR, obj.CURRENT_DATE_STR);
                        var taskInterfacedHBR = AddUpdateTask(interfaceTaskHBR, profile, obj);
                        if (taskInterfacedHBR != null)
                        {
                            interfaceSynch.TASK_ID = taskInterfacedHBR.TASK_ID;
                            InsertInterfaceTeamData(interfaceSynch, profile);
                        }
                    }
                    if (obj.is_strategic_account)
                    {
                        tasktypeHBR = "STRATEGIC";
                    }
                    else
                    {
                        tasktypeHBR = "PORTA";
                    }
                    //Change by arqam
                    if (documentType != null && documentType.RT_CODE != null && (documentType.RT_CODE.ToLower() == "00001" || documentType.RT_CODE.ToLower() == "unsig" || documentType.RT_CODE.ToLower() == "forms" || documentType.RT_CODE.ToLower() == "order"))
                    {
                        var interfaceTaskStrategic = setTaskData(profile, pat_account, tasktypeHBR, obj.CURRENT_DATE_STR);
                        var taskInterfaceStrategic = AddUpdateTask(interfaceTaskStrategic, profile, obj);
                        if (taskInterfaceStrategic != null)
                        {
                            var IS_INTERFACEDOC = 0;
                            interfaceSynch.TASK_ID = taskInterfaceStrategic.TASK_ID;
                            IS_INTERFACEDOC = InsertInterfaceTeamData(interfaceSynch, profile);
                            if (IS_INTERFACEDOC == 0)
                            {
                                sourceAddDetail.IS_INTERFACE = false;
                                SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                                SqlParameter workId = new SqlParameter("Work_ID", obj.WORK_ID);
                                SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_CHANGE_STATUS @PRACTICE_CODE,@USER_NAME,@Work_ID", practice_Code, userName, workId);
                            }
                            else
                            {
                                sourceAddDetail.IS_INTERFACE = true;
                            }
                        }
                    }
                    else
                    {
                        var IS_INTERFACEDOC = 0;
                        interfaceSynch.TASK_ID = null;
                        IS_INTERFACEDOC = InsertInterfaceTeamData(interfaceSynch, profile);
                        if (IS_INTERFACEDOC == 0)
                        {
                            sourceAddDetail.IS_INTERFACE = false;
                            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                            SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                            SqlParameter workId = new SqlParameter("Work_ID", obj.WORK_ID);
                            SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_CHANGE_STATUS @PRACTICE_CODE,@USER_NAME,@Work_ID", practice_Code, userName, workId);
                        }
                        else
                        {
                            sourceAddDetail.IS_INTERFACE = true;
                        }
                    }
                    ////else
                    ////{


                    ////        var interfaceTaskPORTA = setTaskData(profile, pat_account, tasktypeHBR, obj.CURRENT_DATE_STR);
                    ////        var taskInterfacePORTA = AddUpdateTask(interfaceTaskPORTA, profile, obj.WORK_ID);
                    ////        if (taskInterfacePORTA != null)
                    ////        {
                    ////            interfaceSynch.TASK_ID = taskInterfacePORTA.TASK_ID;
                    ////            InsertInterfaceTeamData(interfaceSynch, profile);
                    ////        }
                    ////    }

                    ////}
                }
                else
                {
                    if (IS_TASK_INTERFACED.Count >= 2 && pendingBalance.Patient_Balance != null && pendingBalance.Patient_Balance >= 500 && pendingBalance.Statement_Patient_Balance >= 500 && pendingBalance.NoOfDays > 45)
                    {
                        interfaceSynch.TASK_ID = IS_TASK_INTERFACED[0].TASK_ID;
                        InsertInterfaceTeamData(interfaceSynch, profile);
                        interfaceSynch.TASK_ID = IS_TASK_INTERFACED[1].TASK_ID;
                    }
                    else
                    {
                        interfaceSynch.TASK_ID = IS_TASK_INTERFACED[0].TASK_ID;
                    }
                    var IS_INTERFACETASK = 0;
                    InsertInterfaceTeamData(interfaceSynch, profile);
                    if (IS_INTERFACETASK == 0)
                    {
                        sourceAddDetail.IS_INTERFACE = false;
                        SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                        SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                        SqlParameter workId = new SqlParameter("Work_ID", obj.WORK_ID);
                        SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_CHANGE_STATUS @PRACTICE_CODE,@USER_NAME,@Work_ID", practice_Code, userName, workId);
                    }
                    else
                    {
                        sourceAddDetail.IS_INTERFACE = true;
                    }

                }

                if (MSP_TASK_EXIST != null && MSP_TASK_EXIST.Count > 0)
                {
                    var taskSubType = GetTaskSubType(profile.PracticeCode, MSP_TYPE_ID.TASK_TYPE_ID, "msp");
                    //var taskSubType = _taskSubTypeRepository.GetFirst(t => t.PRACTICE_CODE == profile.PracticeCode && t.DELETED == false && t.TASK_TYPE_ID == MSP_TYPE_ID.TASK_TYPE_ID && t.NAME.ToLower() == "msp");
                    var MSP_SUB_TYPE_EXIST = new FOX_TBL_TASK_TASK_SUB_TYPE();


                    foreach (var msp in MSP_TASK_EXIST)
                    {
                        MSP_SUB_TYPE_EXIST = GetTaskTaskSubType(msp.TASK_ID, profile.PracticeCode, taskSubType.TASK_SUB_TYPE_ID);
                        // MSP_SUB_TYPE_EXIST = _TaskTaskSubTypeRepository.GetFirst(t => t.TASK_ID == msp.TASK_ID && t.PRACTICE_CODE == profile.PracticeCode && t.DELETED == false && t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID);
                        if (MSP_SUB_TYPE_EXIST != null && MSP_SUB_TYPE_EXIST.TASK_ID != null)
                        {
                            break;
                        }
                    }
                    if (MSP_SUB_TYPE_EXIST != null && MSP_SUB_TYPE_EXIST.TASK_ID != null)
                    {
                        var MSP_TASK = MSP_TASK_EXIST.Where(t => t.TASK_ID == MSP_SUB_TYPE_EXIST.TASK_ID).FirstOrDefault();
                        if (MSP_TASK != null)
                        {
                            var IS_MSP_NOT_INTERFACED = GetNotInterfaceTaskList(profile.PracticeCode, pat_account, MSP_TASK.TASK_ID);
                            //var IS_MSP_NOT_INTERFACED = __InterfaceSynchModelRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode
                            //              && t.PATIENT_ACCOUNT == pat_account
                            //&& t.TASK_ID == MSP_TASK.TASK_ID);
                            if (MSP_SUB_TYPE_EXIST != null && MSP_SUB_TYPE_EXIST.TASK_ID != null && IS_MSP_NOT_INTERFACED.Count == 0 && interfaceSynch.TASK_ID != MSP_TASK.TASK_ID)
                            {
                                interfaceSynch.TASK_ID = MSP_SUB_TYPE_EXIST.TASK_ID;
                                InsertInterfaceTeamData(interfaceSynch, profile);
                            }
                            else if (IS_MSP_NOT_INTERFACED.Count > 0)
                            {
                                var interfaced = IS_MSP_NOT_INTERFACED.Where(t => t.IS_SYNCED == false && t.TASK_ID == MSP_SUB_TYPE_EXIST.TASK_ID).FirstOrDefault();
                                if (interfaced != null)
                                {
                                    interfaceSynch.TASK_ID = interfaced.TASK_ID;
                                    InsertInterfaceTeamData(interfaceSynch, profile);
                                }

                            }
                        }
                    }
                }
            }
            //Stop creating POS in RFO of Patient, Usman Nasir
            //MapTreatmentLocation(obj.PATIENT_ACCOUNT, obj.FACILITY_NAME, obj.FACILITY_ID, profile);
            var financial_details = _financialClassRepository.GetFirst(x => x.FINANCIAL_CLASS_ID == obj.FINANCIAL_CLASS_ID);
            if (financial_details != null)
            {
                sourceAddDetail.FINANCIAL_CLASS_NAME = financial_details.NAME.ToString();
            }
            if (profile.isTalkRehab)
            {
                talkRehabWorkID = obj.talkRehabWorkID;
                if (talkRehabTaskID > 0)
                {
                    TaskWorkInterfaceMapping twiMapping = _TaskWorkInterfaceMapping.GetFirst(x => x.TaskId == talkRehabTaskID);
                    if (twiMapping != null)
                    {
                        twiMapping.WorkId = talkRehabWorkID;
                        twiMapping.InterfaceId = talkRehabInterfaceID;
                        twiMapping.ModifiedBy = profile.UserName;
                        twiMapping.ModifiedDate = DateTime.Now;
                        twiMapping.Deleted = false;
                        _TaskWorkInterfaceMapping.Update(twiMapping);
                        _TaskWorkInterfaceMapping.Save();
                    }
                    else
                    {
                        TaskWorkInterfaceMapping mapping = new TaskWorkInterfaceMapping();
                        mapping.TwmID = Helper.getMaximumId("TWM_ID");
                        mapping.TaskId = talkRehabTaskID;
                        mapping.WorkId = talkRehabWorkID;
                        mapping.InterfaceId = talkRehabInterfaceID;
                        mapping.CreatedBy = profile.UserName;
                        mapping.CreatedDate = DateTime.Now;
                        mapping.ModifiedBy = profile.UserName;
                        mapping.ModifiedDate = DateTime.Now;
                        mapping.Deleted = false;
                        _TaskWorkInterfaceMapping.Insert(mapping);
                        _TaskWorkInterfaceMapping.Save();
                    }
                }
            }
            return sourceAddDetail;
        }

        private FOX_TBL_TASK setTaskData(UserProfile profile, long? PATIENT_ACCOUNT, string tasktypeHBR, string CURRENT_DATE_STR)
        {
            var task = new FOX_TBL_TASK();
            SqlParameter pPracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter pTaskTypeName = new SqlParameter();
            pTaskTypeName.ParameterName = "NAME";
            if (tasktypeHBR == "BLOCK")
            {
                pTaskTypeName.Value = "block";
                //task.TASK_TYPE_ID = _taskTypeRepository.GetFirst(t => t.NAME.ToLower() == "block" && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED)?.TASK_TYPE_ID ?? 0;

            }
            else if (tasktypeHBR == "PORTA" || tasktypeHBR == "STRATEGIC")
            {
                pTaskTypeName.Value = "porta";
                //task.TASK_TYPE_ID = _taskTypeRepository.GetFirst(t => t.NAME.ToLower() == "porta" && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED)?.TASK_TYPE_ID ?? 0;

            }
            var Task_type_Id = SpRepository<FOX_TBL_TASK_TYPE>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK_ID @PRACTICE_CODE, @NAME", pPracticeCode, pTaskTypeName);
            task.TASK_TYPE_ID = Task_type_Id?.TASK_TYPE_ID ?? 0;
            //else if (tasktypeHBR == "STRATEGIC")
            //{
            //    task.TASK_TYPE_ID = _taskTypeRepository.GetFirst(t => t.NAME.ToLower() == "strategic accounts" && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED)?.TASK_TYPE_ID ?? 0;
            //}
            task.PRACTICE_CODE = profile.PracticeCode;
            task.PATIENT_ACCOUNT = PATIENT_ACCOUNT;
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var _taskTId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = -1 };
            var _taskTypeId = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = task.TASK_TYPE_ID };
            var _isTemplate = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = true };
            var taskTemplate = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_TASK_BY_TASK_TYPE_ID 
                               @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCode, _taskTId, _taskTypeId, _isTemplate);
            if (taskTemplate != null)
            {
                if (tasktypeHBR == "STRATEGIC")
                {
                    SqlParameter pPractice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                    SqlParameter pGroupName = new SqlParameter("GROUP_NAME", "02CC2");
                    var group02CC2 = SpRepository<GROUP>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_GROUP_ID @PRACTICE_CODE, @GROUP_NAME", pPractice_Code, pGroupName);
                    //var group02CC2 = _groupRepository.GetFirst(t=> t.DELETED == false && t.PRACTICE_CODE == profile.PracticeCode && t.GROUP_NAME == "02CC2");
                    if (group02CC2 != null)
                    {
                        task.SEND_TO_ID = group02CC2.GROUP_ID;
                    }
                }
                else
                {
                    task.SEND_TO_ID = taskTemplate.SEND_TO_ID;
                }
                task.IS_SEND_TO_USER = taskTemplate.IS_SEND_TO_USER;
                task.FINAL_ROUTE_ID = taskTemplate.FINAL_ROUTE_ID;
                task.IS_FINAL_ROUTE_USER = taskTemplate.IS_FINAL_ROUTE_USER;
                task.PRIORITY = taskTemplate.PRIORITY;
                if (task.DUE_DATE_TIME == null)
                {
                    //task.DUE_DATE_TIME = Helper.GetCurrentDate().AddDays(2);
                    //task.DUE_DATE_TIME = CURRENT_DATE_STR;
                    //task.DUE_DATE_TIME_str = task.DUE_DATE_TIME?.ToString("MM'/'dd'/'yyyy");
                    task.DUE_DATE_TIME_str = CURRENT_DATE_STR;
                }
            }
            return task;
        }

        private void MapTreatmentLocation(long? patient_Account, string loc_Name, long? facilityId, UserProfile profile)
        {
            try
            {
                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                if (patient_Account != null && facilityId != null)
                {
                    var location = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == facilityId);
                    if (location != null)
                    {
                        var patientPOSMapping = _PatientPOSLocationRepository.GetFirst(e => e.Patient_Account == patient_Account.Value && e.Loc_ID == location.LOC_ID);
                        if (patientPOSMapping == null)//means mapping does not exist so create one
                        {
                            patientPOSMapping = new PatientPOSLocation();
                            patientPOSMapping.Patient_POS_ID = Helper.getMaximumId("Fox_Patient_POS_ID");
                            interfaceSynch.PATIENT_ACCOUNT = patientPOSMapping.Patient_Account = patient_Account.Value;
                            patientPOSMapping.Loc_ID = location.LOC_ID;

                            patientPOSMapping.Created_By = profile.UserName;
                            patientPOSMapping.Modified_By = profile.UserName;
                            patientPOSMapping.Created_Date = Helper.GetCurrentDate();
                            patientPOSMapping.Modified_Date = Helper.GetCurrentDate();
                            patientPOSMapping.Deleted = false;

                            _PatientPOSLocationRepository.Insert(patientPOSMapping);
                            _PatientPOSLocationRepository.Save();
                            //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                            //InsertInterfaceTeamData(interfaceSynch, profile);
                        }

                        //if (location.IS_PRIVATE_HOME ?? false) {
                        //    SetPatientHomeAddress(location, profile);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void SetPatientHomeAddress(FacilityLocation loc_Data, UserProfile profile)
        //{
        //    var _patientContext = new DbContextPatient();
        //    var _patientAddressRepository = new GenericRepository<PatientAddress>(_patientContext);
        //    if(loc_Data.COMPLETE_ADDRESS)
        //    var dbPatientAddress = _patientAddressRepository.GetFirst(e=>e.);
        //}

        private void sendMail(long wORK_ID, UserProfile profile, bool isSaveIndex)
        {
            try
            {
                //Get Data
                ReferralRegion _accontManagerEmail = new ReferralRegion();
                var wi = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = wORK_ID };
                var emailData = SpRepository<IndexInfoEmail>.GetSingleObjectWithStoreProcedure(@"exec [FOX_GET_INDEX_ALL_INFO_EMAIL] @WORK_ID", wi);
                if (emailData == null) { return; }

                //Basic
                string _body = string.Empty;
                string _subject = string.Empty;
                string sendTo = string.Empty;
                List<string> _bccList = new List<string>();
                if (emailData.IS_EMERGENCY_ORDER != null) { if (emailData.IS_EMERGENCY_ORDER.Value) { _subject = "URGENT: " + _subject; } }
                if (isSaveIndex)
                {
                    _body = "<p>Referral indexed with the following information:</br>";
                    _subject += "Referral indexed for";
                    if (emailData.IS_EMERGENCY_ORDER.HasValue && emailData.IS_EMERGENCY_ORDER.Value)
                    {
                        var urgentEmailsOnCompletion = _EmailConfigRepository.GetMany(x => x.ON_INDEX == true && x.IsUrgentEmail == true && x.DELETED == false);
                        if (urgentEmailsOnCompletion.Count > 0)
                            sendTo = urgentEmailsOnCompletion[0].EMAIL_ADDRESS;
                        for (int i = 1; i < urgentEmailsOnCompletion.Count; i++)
                        {
                            _bccList.Add(urgentEmailsOnCompletion[i].EMAIL_ADDRESS);
                        }
                    }
                    else
                    {
                        var _emailList = _EmailConfigRepository.GetMany(x => x.ON_INDEX == true && x.IsUrgentEmail == false && x.DELETED == false);
                        if (_emailList.Count > 0)
                            sendTo = _emailList[0].EMAIL_ADDRESS;
                        for (int i = 1; i < _emailList.Count; i++)
                        {
                            _bccList.Add(_emailList[i].EMAIL_ADDRESS);
                        }
                    }

                    var patientDetails = _QueueRepository.GetFirst(x => x.WORK_ID == emailData.WORK_ID && x.DELETED == false && x.PRACTICE_CODE == profile.PracticeCode);
                    if (patientDetails != null)
                    {
                        var patientPOSLocation = _PatientPOSLocationRepository.GetMany(x => x.Patient_Account == patientDetails.PATIENT_ACCOUNT && x.Deleted == false && x.Is_Default == true).OrderByDescending(t => t.Modified_Date).FirstOrDefault();
                        if (patientPOSLocation != null)
                        {
                            var activeLocation = _FacilityLocationRepository.GetFirst(x => x.LOC_ID == patientPOSLocation.Loc_ID && x.DELETED == false && x.PRACTICE_CODE == profile.PracticeCode);
                            if (activeLocation != null && activeLocation.NAME.ToLower() == "private home")
                            {
                                var privateHomeDetails = _PatientAddressRepository.GetFirst(x => x.PATIENT_ACCOUNT == patientDetails.PATIENT_ACCOUNT && x.PATIENT_POS_ID == patientPOSLocation.Patient_POS_ID && x.DELETED == false);
                                if (privateHomeDetails != null)
                                {
                                    var referralRegionDetails = _zipStateCountyRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.ZIP_CODE.Substring(0, 5) == privateHomeDetails.ZIP.Substring(0, 5) && !x.DELETED);
                                    if (referralRegionDetails != null)
                                    {
                                        _accontManagerEmail = _ReferralRegionRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.IS_ACTIVE == true && x.ACCOUNT_MANAGER_EMAIL != null && x.REFERRAL_REGION_ID == referralRegionDetails.REFERRAL_REGION_ID && !x.DELETED);
                                    }

                                }


                            }
                            else
                            {
                                if (activeLocation != null && activeLocation.Zip != null)
                                {
                                    var referralRegionDetails = _zipStateCountyRepository.GetFirst(x => x.ZIP_CODE.Substring(0, 5) == activeLocation.Zip.Substring(0, 5) && x.PRACTICE_CODE == profile.PracticeCode);
                                    if (referralRegionDetails != null)
                                    {
                                        _accontManagerEmail = _ReferralRegionRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.IS_ACTIVE == true && x.ACCOUNT_MANAGER_EMAIL != null && x.REFERRAL_REGION_ID == referralRegionDetails.REFERRAL_REGION_ID && !x.DELETED);
                                    }

                                }


                            }
                        }

                    }






                    //var _accontManagerEmail = _ReferralRegionRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.IS_ACTIVE == true && x.ACCOUNT_MANAGER_EMAIL != null && (x.REFERRAL_REGION_NAME == emailData.REFERRAL_REGION_NAME || x.REFERRAL_REGION_CODE == emailData.REFERRAL_REGION_NAME));

                    if (_accontManagerEmail != null)
                    {
                        if (string.IsNullOrEmpty(sendTo))
                        {
                            sendTo = _accontManagerEmail.ACCOUNT_MANAGER_EMAIL;
                        }
                        else
                        {
                            _bccList.Add(_accontManagerEmail.ACCOUNT_MANAGER_EMAIL);
                        }
                    }
                }
                else
                {
                    _body = "<p>Referral completed with the following information:</br>";
                    _subject += "Referral completed for";
                    if (emailData.IS_EMERGENCY_ORDER.HasValue && emailData.IS_EMERGENCY_ORDER.Value)
                    {
                        var urgentEmailsOnCompletion = _EmailConfigRepository.GetMany(x => (x.ON_COMPELETED == true || x.ON_INDEX == true) && x.IsUrgentEmail == true);
                        if (urgentEmailsOnCompletion.Count > 0)
                            sendTo = urgentEmailsOnCompletion[0].EMAIL_ADDRESS;
                        for (int i = 1; i < urgentEmailsOnCompletion.Count; i++)
                        {
                            _bccList.Add(urgentEmailsOnCompletion[i].EMAIL_ADDRESS);
                        }
                    }
                    else
                    {
                        var _emailList = _EmailConfigRepository.GetMany(x => (x.ON_COMPELETED == true || x.ON_INDEX == true) && x.IsUrgentEmail == false);
                        if (_emailList.Count > 0)
                            sendTo = _emailList[0].EMAIL_ADDRESS;
                        for (int i = 1; i < _emailList.Count; i++)
                        {
                            _bccList.Add(_emailList[i].EMAIL_ADDRESS);
                        }
                    }
                    var patientDetails = _QueueRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.WORK_ID == emailData.WORK_ID && x.DELETED == false);
                    if (patientDetails != null)
                    {
                        var patientPOSLocation = _PatientPOSLocationRepository.GetMany(x => x.Patient_Account == patientDetails.PATIENT_ACCOUNT && x.Deleted == false && x.Is_Default == true).OrderByDescending(t => t.Modified_Date).FirstOrDefault();
                        if (patientPOSLocation != null)
                        {
                            var activeLocation = _FacilityLocationRepository.GetFirst(x => x.LOC_ID == patientPOSLocation.Loc_ID && x.DELETED == false && x.PRACTICE_CODE == profile.PracticeCode);
                            if (activeLocation != null && activeLocation.NAME.ToLower() == "private home")
                            {
                                var privateHomeDetails = _PatientAddressRepository.GetFirst(x => x.PATIENT_ACCOUNT == patientDetails.PATIENT_ACCOUNT && x.PATIENT_POS_ID == patientPOSLocation.Patient_POS_ID && x.DELETED == false);
                                if (privateHomeDetails != null)
                                {
                                    var referralRegionDetails = _zipStateCountyRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.ZIP_CODE.Substring(0, 5) == privateHomeDetails.ZIP.Substring(0, 5) && !x.DELETED);
                                    if (referralRegionDetails != null)
                                    {
                                        _accontManagerEmail = _ReferralRegionRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.IS_ACTIVE == true && x.ACCOUNT_MANAGER_EMAIL != null && x.REFERRAL_REGION_ID == referralRegionDetails.REFERRAL_REGION_ID && !x.DELETED);
                                    }

                                }
                            }
                            else
                            {
                                if (activeLocation != null && activeLocation.Zip != null)
                                {
                                    var referralRegionDetails = _zipStateCountyRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.ZIP_CODE.Substring(0, 5) == activeLocation.Zip.Substring(0, 5) && !x.DELETED);
                                    if (referralRegionDetails != null)
                                    {
                                        _accontManagerEmail = _ReferralRegionRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.IS_ACTIVE == true && x.ACCOUNT_MANAGER_EMAIL != null && x.REFERRAL_REGION_ID == referralRegionDetails.REFERRAL_REGION_ID && !x.DELETED);
                                    }

                                }


                            }
                        }

                    }

                    // _accontManagerEmail = _ReferralRegionRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.IS_ACTIVE == true && x.ACCOUNT_MANAGER_EMAIL != null && (x.REFERRAL_REGION_NAME == emailData.REFERRAL_REGION_NAME || x.REFERRAL_REGION_CODE == emailData.REFERRAL_REGION_NAME));
                    if (_accontManagerEmail != null)
                    {
                        if (string.IsNullOrEmpty(sendTo))
                        {
                            sendTo = _accontManagerEmail.ACCOUNT_MANAGER_EMAIL;
                        }
                        else
                        {
                            _bccList.Add(_accontManagerEmail.ACCOUNT_MANAGER_EMAIL);
                        }
                    }
                }
                _subject += " " + Helper.ChangeStringToTitleCase(emailData.PATIENT_LAST_NAME + ", " + emailData.PATIENT_FIRST_NAME) + "for";

                if (emailData.DEPARTMENT_ID != null)
                {
                    if (emailData.DEPARTMENT_ID.Contains("1")) { _subject = _subject + " Occupational Therapy (OT), "; }
                    if (emailData.DEPARTMENT_ID.Contains("2")) { _subject = _subject + " Physical Therapy (PT), "; }
                    if (emailData.DEPARTMENT_ID.Contains("3")) { _subject = _subject + " Speech Therapy (ST), "; }
                    if (emailData.DEPARTMENT_ID == "4") { _subject = _subject + " for Physical/Occupational/Speech Therapy(PT/OT/ST)"; }
                    if (emailData.DEPARTMENT_ID == "5") { _subject = _subject + " for Physical/Occupational Therapy(PT/OT)"; }
                    if (emailData.DEPARTMENT_ID == "6") { _subject = _subject + " for Physical/Speech Therapy(PT/ST)"; }
                    if (emailData.DEPARTMENT_ID == "7") { _subject = _subject + " for Occupational/Speech Therapy(OT/ST)"; }
                    if (emailData.DEPARTMENT_ID.Contains("8")) { _subject = _subject + " Unknown, "; }
                    if (emailData.DEPARTMENT_ID.Contains("9")) { _subject = _subject + " Exercise Physiology (EP), "; }
                    if (_subject.Substring(_subject.Length - 2) == ",")
                    {
                        _subject = _subject.TrimEnd(_subject[_subject.Length - 1]);
                    }
                    //if (emailData.DEPARTMENT_ID == "1") { _subject = _subject + " for Occupational Therapy"; }
                    //else if (emailData.DEPARTMENT_ID == "2") { _subject = _subject + " for Physical Therapy (PT)"; }
                    //else if (emailData.DEPARTMENT_ID == "3") { _subject = _subject + " for Speech Therapy (ST)"; }
                    //else if (emailData.DEPARTMENT_ID == "4") { _subject = _subject + " for Physical/Occupational/Speech Therapy(PT/OT/ST)"; }
                    //else if (emailData.DEPARTMENT_ID == "5") { _subject = _subject + " for Physical/Occupational Therapy(PT/OT)"; }
                    //else if (emailData.DEPARTMENT_ID == "6") { _subject = _subject + " for Physical/Speech Therapy(PT/ST)"; }
                    //else if (emailData.DEPARTMENT_ID == "7") { _subject = _subject + " for Occupational/Speech Therapy(OT/ST)"; }
                }

                //Work Id.
                _body += "<strong>Referral/Work ID</strong>: " + emailData.UNIQUE_ID.ToString() + "</p>";

                //Source Info.
                _body += "<p><strong>Source Info:<br /></strong>";
                if (emailData.DOCUMENT_TYPE != null)
                {
                    _body += "Document Type:&nbsp; " + emailData.DOCUMENT_NAME + "<br />";
                    //if (emailData.DOCUMENT_TYPE == 1) { _body = _body + "POC<br />"; }
                    //else if (emailData.DOCUMENT_TYPE == 2) { _body = _body + "Referral Order<br />"; }
                    //else if (emailData.DOCUMENT_TYPE == 3) { _body = _body + "Other<br />"; }
                }
                if (emailData.SORCE_NAME.Contains("@"))
                {
                    emailData.SORCE_NAME = emailData.SORCE_NAME.ToLower();
                    _body += "Sender Email: <a href=\"Mail To: " + emailData.SORCE_NAME + "\">" + emailData.SORCE_NAME + "<br /></a>";
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(emailData.SORCE_NAME) && emailData.SORCE_NAME.Contains("+1"))
                    {
                        emailData.SORCE_NAME = emailData.SORCE_NAME.Replace("+1", "");
                        if (!string.IsNullOrEmpty(emailData.SORCE_NAME))
                        {
                            emailData.SORCE_NAME = String.Format("{0:(###) ###-####}", Int64.Parse(emailData.SORCE_NAME));
                        }
                    }
                    _body += "Sender Fax: " + emailData.SORCE_NAME + "<br /></a>";
                }
                _body += "Ordering Referral Source: " + Helper.ChangeStringToTitleCase(emailData.REF_SOURCE_LAST_NAME) + " " + Helper.ChangeStringToTitleCase(emailData.REF_SOURCE_FIRST_NAME) + "<br />";
                _body += "Region: " + Helper.ChangeStringToTitleCase(emailData.REFERRAL_REGION_NAME) + "<br />";
                _body += "Treatment Location: " + Helper.ChangeStringToTitleCase(emailData.TREATMENT_LOCATION) + "</p>";

                //Patient Details.
                _body += "<p><strong>Patient Details:<br /></strong>";
                _body += Helper.ChangeStringToTitleCase(emailData.PATIENT_LAST_NAME) + ", " + Helper.ChangeStringToTitleCase(emailData.PATIENT_FIRST_NAME) + " (" + Helper.ChangeStringToTitleCase(emailData.PATIENT_GENDER) + ")<br />";
                _body += "DOB: ";
                if (emailData.PATIENT_DOB != null)
                {
                    _body += emailData.PATIENT_DOB.Value.ToString("MM/dd/yyyy");
                }
                _body += "<br />MRN: ";
                if (!string.IsNullOrEmpty(emailData.PATIENT_MRN))
                {
                    _body += Helper.ChangeStringToTitleCase(emailData.PATIENT_MRN);
                }
                _body += "<br />Home Address: ";
                if (!string.IsNullOrEmpty(emailData.ADDRESS))
                {
                    _body = _body + Helper.ChangeStringToTitleCase(emailData.ADDRESS) + ", " + Helper.ChangeStringToTitleCase(emailData.STATE) + " " + (emailData.ZIP);
                }
                _body += "<br />📞Mobile: ";
                if (!string.IsNullOrEmpty(emailData.PATIENT_PHONE_NO))
                {
                    emailData.PATIENT_PHONE_NO = String.Format("{0:(###) ###-####}", Int64.Parse(emailData.PATIENT_PHONE_NO.Trim()));
                    _body += emailData.PATIENT_PHONE_NO;
                }
                _body += "<br />Pri. Ins: ";
                if (!string.IsNullOrEmpty(emailData.INS_NAME))
                {
                    _body += emailData.INS_NAME;
                }
                _body += "</p>";

                //Diagnosis Information.
                var _diagnosis = _DeleteDiagnosisRepository.GetMany(x => x.WORK_ID == wORK_ID);
                _body += "<p><strong>Diagnosis Information:<br /></strong>";
                if (_diagnosis.Count != 0)
                {
                    for (int i = 0; i < _diagnosis.Count; i++)
                    {
                        _body += "&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + "- [" + _diagnosis[i].DIAG_CODE + "] " + Helper.ChangeStringToTitleCase(_diagnosis[i].DIAG_DESC) + "<br />";
                    }
                    _body += "</p>";
                }

                //Procedure Information.
                var _CPT = _DeleteProcedureRepository.GetMany(x => x.WORK_ID == wORK_ID);
                if (_CPT.Count != 0)
                {
                    _body += "<p><strong>Procedure Information:<br /></strong>";
                    for (int i = 0; i < _CPT.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(_CPT[i].PROC_CODE) && !string.IsNullOrWhiteSpace(_CPT[i].CPT_DESC))
                            _body += "&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + "- [" + _CPT[i].PROC_CODE + "] " + Helper.ChangeStringToTitleCase(_CPT[i].CPT_DESC) + "<br />";
                    }
                    _body += "</p>";
                }

                //Procedure Information.
                _body += "<p><strong>Additional Information:<br /></strong>";
                _body += "Reason for Visit: " + Helper.ChangeStringToTitleCase(emailData.REASON_FOR_VISIT) + "<br />";
                _body += "Account #: " + emailData.ACCOUNT_NUMBER + "<br />";
                _body += "Unit/Case #: " + Helper.ChangeStringToTitleCase(emailData.UNIT_CASE_NO) + "</p>";

                var _notes = _NotesHistoryRepository.GetMany(x => x.WORK_ID == wORK_ID).LastOrDefault();
                _body += "<p><strong >Notes:<br /></strong >";
                _body += "Last Note: ";
                if (_notes != null && !string.IsNullOrWhiteSpace(_notes.NOTE_DESC))
                {
                    _body += Helper.ChangeStringToTitleCase(_notes.NOTE_DESC) + "</p>";
                }
                _body = _body + "</p>";
                //_body += "<br /><br /><p>Please <a href = 'https://fox.mtbc.com/#/OriginalQueue?id=" + emailData.UNIQUE_ID + "' target = '_blank' rel = 'noopener'>click here</a> to see the referral.</p>";
                _body += "<br /><br /><p>Please <a href = '" + AppConfiguration.ClientURL + "#/OriginalQueue?id=" + emailData.UNIQUE_ID + "' target = '_blank' rel = 'noopener'>click here</a> to see the referral.</p>";

                if (!string.IsNullOrEmpty(sendTo))
                {
                    Helper.SendEmail(sendTo, _subject, _body, wORK_ID, profile, null, _bccList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetSenderName(long? senderId)
        {
            var sender = _InsertUpdateOrderingSourceRepository.GetByID(senderId);
            if (sender != null)
            {
                return sender.LAST_NAME + ", " + sender.FIRST_NAME;
            }
            else
            {
                return "";
            }
        }
        public ReferralSource InsertUpdateOrderingSource(ReferralSource obj, UserProfile profile)
        {
            var sourceDetail = _InsertUpdateOrderingSourceRepository.GetFirst(e => e.SOURCE_ID == obj.SOURCE_ID && !e.DELETED);

            if (sourceDetail != null)
            {
                obj.REFERRAL_CODE = sourceDetail.REFERRAL_CODE;
                InsertUpdateReferralPhy(obj, profile);
                sourceDetail.ACO = obj.ACO;
                sourceDetail.ADDRESS = obj.ADDRESS;
                sourceDetail.ADDRESS_2 = obj.ADDRESS_2;
                sourceDetail.CITY = obj.CITY;
                sourceDetail.CODE = obj.CODE;
                sourceDetail.FAX = obj.FAX;
                sourceDetail.TITLE = obj.TITLE;
                sourceDetail.NPI = obj.NPI;
                sourceDetail.ORGANIZATION = obj.ORGANIZATION;
                sourceDetail.PHONE = obj.PHONE;
                sourceDetail.STATE = obj.STATE;
                sourceDetail.ZIP = obj.ZIP;
                sourceDetail.DELETED = obj.DELETED;
                sourceDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                sourceDetail.MODIFIED_BY = profile.UserName;
                _InsertUpdateOrderingSourceRepository.Update(sourceDetail);
                _InsertUpdateOrderingSourceRepository.Save();
                return sourceDetail;
            }
            else
            {
                long REFERRAL_CODE = InsertUpdateReferralPhy(obj, profile);
                //By Johar removed Idenetity
                obj.SOURCE_ID = Helper.getMaximumId("FOX_SOURCE_ID");
                obj.CODE = Helper.getMaximumId("SOURCE_ID").ToString();
                obj.REFERRAL_CODE = REFERRAL_CODE;
                obj.PRACTICE_CODE = profile.PracticeCode;
                obj.CREATED_BY = profile.UserName;
                obj.CREATED_DATE = Helper.GetCurrentDate();
                obj.MODIFIED_DATE = obj.CREATED_DATE;
                obj.MODIFIED_BY = obj.CREATED_BY;
                _InsertUpdateOrderingSourceRepository.Insert(obj);
                _InsertUpdateOrderingSourceRepository.Save();
                return obj;
            }
        }
        public IndexPatRes GetIndexPatInfo(getPatientReq req, UserProfile Profile)
        {
            try
            {
                //if (req.Last_Name == null)
                //    req.Last_Name = "";
                //if (req.First_Name == null)
                //    req.First_Name = "";
                //if (req.Middle_Name == null)
                //    req.Middle_Name = "";
                //if (req.SSN == null)
                //    req.SSN = "";
                //if (req.Gender == null)
                //    req.Gender = "";
                //if (req.Chart_Id == null)
                //    req.Chart_Id = "";
                req.Practice_Code = Profile.PracticeCode;
                var _patientAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = req.Patient_Account };
                var dob = string.IsNullOrEmpty(req.Date_Of_Birth_In_String) ? new DateTime() : Convert.ToDateTime(req.Date_Of_Birth_In_String);
                var dobtemp = dob.ToString("MM/dd/yyyy");
                var Practice_Code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = req.Practice_Code };
                //var last_Name = new SqlParameter("Last_Name", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(req.Last_Name) ? req.Last_Name.Trim() : req.Last_Name };
                //var first_Name = new SqlParameter("First_Name", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(req.First_Name) ? req.First_Name.Trim() : req.First_Name };
                //var middle_Name = new SqlParameter("Middle_Name", SqlDbType.VarChar) { Value = req.Middle_Name };
                //var sSN = new SqlParameter("SSN", SqlDbType.VarChar) { Value = req.SSN };
                //var gender = new SqlParameter("Gender", SqlDbType.VarChar) { Value = req.Gender };
                //var date_Of_Birth = Helper.getDBNullOrValue("Date_Of_Birth", req.Date_Of_Birth_In_String == null ? null : Convert.ToDateTime(req.Date_Of_Birth_In_String).ToShortDateString());// new SqlParameter("Date_Of_Birth", SqlDbType.VarChar) { Value = req.Date_Of_Birth };
                //var chart_Id = new SqlParameter("@Chart_Id", SqlDbType.VarChar) { Value = req.Chart_Id };
                var work_id = new SqlParameter("@WORK_ID", SqlDbType.BigInt) { Value = req.WORK_ID };
                var _PRACTICE_ORGANIZATION_ID = new SqlParameter("@PRACTICE_ORGANIZATION_ID", SqlDbType.BigInt) { Value = Profile.PRACTICE_ORGANIZATION_ID ?? 0 };
                //var result = SpRepository<IndexPatRes>.GetListWithStoreProcedure(@"exec [Fox_Get_Patient_Info] @Last_Name,@First_Name,@Middle_Name,@SSN,@Gender,@Date_Of_Birth,@Chart_Id,@PRACTICE_CODE",
                // last_Name, first_Name, middle_Name, sSN, gender, date_Of_Birth, chart_Id, Practice_Code);
                //var Patient_Alias = new SqlParameter { ParameterName = "Patient_Alias", SqlDbType = SqlDbType.Bit, Value = req.INCLUDE_ALIAS };
                //var _currentPage = new SqlParameter { ParameterName = "@CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = req.CURRENT_PAGE };
                //var _recordPerPage = new SqlParameter { ParameterName = "@RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = req.RECORD_PER_PAGE };
                //var _sortBy = new SqlParameter { ParameterName = "@SORT_BY", Value = req.SORT_BY };
                //var _sortOrder = new SqlParameter { ParameterName = "@SORT_ORDER", Value = req.SORT_ORDER };
                //if (req.SORT_BY == null)
                //{
                //    _sortBy.Value = DBNull.Value;
                //}
                //if (req.SORT_ORDER == null)
                //{
                //    _sortOrder.Value = DBNull.Value;
                //}
                //var result = SpRepository<IndexPatRes>.GetListWithStoreProcedure(@"exec Fox_Get_Patient_Info_Index_Info
                //             @Last_Name, @First_Name, @Middle_Name, @SSN, @Gender, @Date_Of_Birth, @Chart_Id, @PRACTICE_CODE, @PRACTICE_ORGANIZATION_ID,@WORK_ID, @Patient_Alias, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
                //             last_Name, first_Name, middle_Name, sSN, gender, date_Of_Birth, chart_Id, Practice_Code, _PRACTICE_ORGANIZATION_ID, work_id, Patient_Alias, _currentPage, _recordPerPage, _sortBy, _sortOrder);
                var result = SpRepository<IndexPatRes>.GetSingleObjectWithStoreProcedure(@"exec Fox_Get_Patient_Info_Index_Info_single_record @PATIENT_ACCOUNT,@PRACTICE_CODE,@PRACTICE_ORGANIZATION_ID,@WORK_ID", _patientAccount, Practice_Code, _PRACTICE_ORGANIZATION_ID, work_id);
                var res = getPatientsLastORS(req.Patient_Account, req.Practice_Code);

                if (res != null)
                {
                    result.NPI = res.NPI;
                    result.SOURCE_ID = res.SOURCE_ID;
                }

                if (result != null)
                {
                    //if (Profile.PRACTICE_ORGANIZATION_ID != null && ((!string.IsNullOrEmpty(req.Last_Name) && !string.IsNullOrEmpty(req.SSN)) || (!string.IsNullOrEmpty(req.Last_Name) && !string.IsNullOrEmpty(req.Date_Of_Birth))))
                    //{
                    //    var tempSameOrganizationList = result.Where(t => t.PRACTICE_ORGANIZATION_ID == Profile.PRACTICE_ORGANIZATION_ID).AsEnumerable();
                    //    var tempExactMatchList = result.Where(t => (t.PRACTICE_ORGANIZATION_ID != Profile.PRACTICE_ORGANIZATION_ID) && ((t.Last_Name.ToLower().Equals(req.Last_Name.ToLower()) && (t.SSN?.Equals(req.SSN) ?? false))
                    //                                                || (t.Last_Name.ToLower().Equals(req.Last_Name.ToLower()) && (t.Date_Of_Birth?.Equals(dobtemp) ?? false)))).AsEnumerable();
                    //    result = new List<IndexPatRes>();
                    //    if (tempSameOrganizationList.Any())
                    //    {
                    //        result.AddRange(tempSameOrganizationList);
                    //    }
                    //    if (tempExactMatchList.Any())
                    //    {
                    //        result.AddRange(tempExactMatchList);
                    //    }
                    //    var recPerPgae = 0;
                    //    if (req.RECORD_PER_PAGE == 0)
                    //    {
                    //        recPerPgae = result.Count();
                    //    }
                    //    else
                    //    {
                    //        recPerPgae = req.RECORD_PER_PAGE;
                    //    }
                    //    foreach (var item in result)
                    //    {
                    //        item.TOTAL_RECORDS = result.Count();
                    //        item.TOTAL_RECORD_PAGES = (int)Math.Ceiling((decimal)item.TOTAL_RECORDS / (decimal)recPerPgae);
                    //    }
                    //}
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FOX_TBL_PATIENT_DIAGNOSIS> GetDiagnosisInfo(Index_infoReq obj, UserProfile Profile)
        {
            try
            {
                var work_id = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = obj.WORK_ID };
                var result = SpRepository<FOX_TBL_PATIENT_DIAGNOSIS>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_DIAGNOSIS] @WORK_ID", work_id).ToList();
                if (result != null)
                    return result;
                else
                    return new List<FOX_TBL_PATIENT_DIAGNOSIS>();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<GetSmartDiagnosisRes> GetSmartDiagnosisInfo(SmartDiagnosisReq obj, UserProfile Profile)
        {
            try
            {
                if (obj.CATEGORY == null)
                    obj.CATEGORY = "";
                else
                    obj.CATEGORY = obj.CATEGORY.Trim();

                if (obj.CODE == null)
                    obj.CODE = "";
                else
                    obj.CODE = obj.CODE.Trim();

                if (obj.CODE_TYPE == null)
                    obj.CODE_TYPE = "";
                else
                    obj.CODE_TYPE = obj.CODE_TYPE.Trim();

                if (obj.DESCRIPTION == null)
                    obj.DESCRIPTION = "";
                else
                {
                    obj.DESCRIPTION = obj.DESCRIPTION.Trim();
                    if (obj.DESCRIPTION.Contains(@"'"))
                    {
                        obj.DESCRIPTION = obj.DESCRIPTION.Replace(@"'", @"''");
                    }
                }
                if (obj.DOS == null)
                    obj.DOS = "";
                else
                    obj.DOS = obj.DOS.Trim();

                if (obj.OPTIONSEARCH == null)
                    obj.OPTIONSEARCH = "";
                else
                    obj.OPTIONSEARCH = obj.OPTIONSEARCH.Trim();

                if (obj.PROVIDER_CODE == null)
                    obj.PROVIDER_CODE = "";
                else
                    obj.PROVIDER_CODE = obj.PROVIDER_CODE.Trim();

                var cATEGORY = new SqlParameter("@CATEGORY", SqlDbType.VarChar) { Value = obj.CATEGORY };
                var cODE = new SqlParameter("@CODE", SqlDbType.VarChar) { Value = obj.CODE };
                var cODE_TYPE = new SqlParameter("@CODE_TYPE", SqlDbType.VarChar) { Value = obj.CODE_TYPE };
                var dESCRIPTION = new SqlParameter("@DESCRIPTION", SqlDbType.VarChar) { Value = obj.DESCRIPTION };
                var dOS = new SqlParameter("@DOS", SqlDbType.VarChar) { Value = obj.DOS };
                var oPTIONSEARCH = new SqlParameter("@OPTIONSEARCH", SqlDbType.VarChar) { Value = obj.OPTIONSEARCH };
                var pRACTICE_CODE = new SqlParameter("@PRACTICE_CODE", SqlDbType.VarChar) { Value = Profile.PracticeCode };
                var pROVIDER_CODE = new SqlParameter("@PROVIDER_CODE", SqlDbType.VarChar) { Value = obj.PROVIDER_CODE };
                var result = SpRepository<GetSmartDiagnosisRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_DIAGNOSIS_WEBEHR] @CODE,@DESCRIPTION,@OPTIONSEARCH,@PRACTICE_CODE,@PROVIDER_CODE,@CODE_TYPE,@CATEGORY,@DOS", cODE, dESCRIPTION, oPTIONSEARCH, pRACTICE_CODE, pROVIDER_CODE, cODE_TYPE, cATEGORY, dOS).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<GetSmartDiagnosisRes>();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<FOX_TBL_PATIENT_PROCEDURE> GetProceduresInfo(Index_infoReq obj, UserProfile Profile)
        {
            try
            {
                var work_id = new SqlParameter("WORK_ID", SqlDbType.VarChar) { Value = obj.WORK_ID };
                var result = SpRepository<FOX_TBL_PATIENT_PROCEDURE>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_PROCEDURES] @WORK_ID", work_id).ToList();
                if (result != null)
                    return result;
                else
                    return new List<FOX_TBL_PATIENT_PROCEDURE>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<GetSmartProceduresRes> GetSmartProceduresInfo(SmartProceduresReq obj, UserProfile profile)
        {
            try
            {
                if (obj.CATEGORY == null)
                    obj.CATEGORY = "";
                if (obj.DOS == null)
                    obj.DOS = "";
                if (obj.LOCATION_CODE == 0)
                    obj.LOCATION_CODE = 0;
                if (obj.PROCCODE == null)
                    obj.PROCCODE = "";
                if (obj.PROCDESCRIPTION == null)
                    obj.PROCDESCRIPTION = "";
                if (obj.PROVIDER_CODE == 0)
                    obj.PROVIDER_CODE = 0;
                var cATEGORY = new SqlParameter("@CATEGORY", SqlDbType.VarChar) { Value = obj.CATEGORY };
                var dOS = new SqlParameter("@DOS", SqlDbType.VarChar) { Value = obj.DOS };
                var lOCATION_CODE = new SqlParameter("@LOCATION_CODE", SqlDbType.BigInt) { Value = obj.LOCATION_CODE };
                var pRACTICE_CODE = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var pROCCODE = new SqlParameter("@PROCCODE", SqlDbType.VarChar) { Value = obj.PROCCODE };
                var pROCDESCRIPTION = new SqlParameter("@PROCDESCRIPTION", SqlDbType.VarChar) { Value = obj.PROCDESCRIPTION };
                var pROVIDER_CODE = new SqlParameter("@PROVIDER_CODE", SqlDbType.BigInt) { Value = obj.PROVIDER_CODE };
                var result = SpRepository<GetSmartProceduresRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_PROCEDURES_WEBEHR] @PROCCODE,@PROCDESCRIPTION,@CATEGORY,@PRACTICE_CODE,@PROVIDER_CODE,@LOCATION_CODE,@DOS", pROCCODE, pROCDESCRIPTION, cATEGORY, pRACTICE_CODE, pROVIDER_CODE, lOCATION_CODE, dOS).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<GetSmartProceduresRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SourceSenderRes> GetSourceSener(UserProfile Profile)
        //{
        //    try
        //    {
        //        var result = SpRepository<SourceSenderRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SOURCE_SENDERS]").ToList();
        //        if (result != null)
        //            return result;
        //        else
        //            return new List<SourceSenderRes>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<FOX_TBL_NOTES_HISTORY> GetNotes_History(Index_infoReq obj, UserProfile Profile)
        {
            try

            {
                var work_id = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = obj.WORK_ID };
                if (obj.WORK_ID == null)
                {
                    work_id.Value = DBNull.Value;
                }
                var result = SpRepository<FOX_TBL_NOTES_HISTORY>.GetListWithStoreProcedure(@"exec [FOX_GET_NOTES_HISTORY] @WORK_ID", work_id).ToList();
                if (result != null)
                    return result;
                else
                    return new List<FOX_TBL_NOTES_HISTORY>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ExportToExcelNotes_History(Index_infoReq obj, UserProfile profile)
        {
            try
            {
                string fileName = "Notes_History_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                //obj.C = 1;
                //obj.recordPerpage = 0;
                var CalledFrom = "Notes_History";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<FOX_TBL_NOTES_HISTORY> result = new List<FOX_TBL_NOTES_HISTORY>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetNotes_History(obj, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<FOX_TBL_NOTES_HISTORY>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FOX_TBL_PATIENT_DOCUMENTS> GetDocuments(Index_infoReq obj, UserProfile Profile)
        {
            try

            {
                var work_id = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = obj.WORK_ID };
                var result = SpRepository<FOX_TBL_PATIENT_DOCUMENTS>.GetListWithStoreProcedure(@"exec  [FOX_GET_INDEX_INFO_DOCUMNETS] @WORK_ID", work_id).ToList();
                if (result != null)
                    return result;
                else
                    return new List<FOX_TBL_PATIENT_DOCUMENTS>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SmartLocationRes> GetSmartLocations(SmartReq obj, UserProfile profile)
        {
            try
            {
                if (obj.SEARCHVALUE == null)
                    obj.SEARCHVALUE = "";

                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
                var result = SpRepository<SmartLocationRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_LOCATIONS] @PRACTICE_CODE, @SEARCHVALUE",
                    parmPracticeCode, smartvalue).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SmartLocationRes>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //return _FacilityLocationRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && (x.NAME.Contains(obj.SEARCHVALUE) || x.CODE.Contains(obj.SEARCHVALUE)));
        }

        public List<SmartLocationRes> GetSmartLocations(string searchText, UserProfile profile)
        {
            try
            {
                if (searchText == null)
                    searchText = "";
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = searchText };
                var result = SpRepository<SmartLocationRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_LOCATIONS] @PRACTICE_CODE, @SEARCHVALUE",
                    parmPracticeCode, smartvalue).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SmartLocationRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SmartOrderSource> GetSmartOrderingSource(SmartReq obj, UserProfile Profile)
        {
            try
            {
                var parmPracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
                var smartvalue = new SqlParameter("@SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
                var result = new List<SmartOrderSource>();
                if (!obj.Is_From_RFO)
                {
                    result = SpRepository<SmartOrderSource>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_ORDERING_SOURCE] @PRACTICE_CODE, @SEARCHVALUE", parmPracticeCode, smartvalue).ToList();
                }
                else
                {
                    result = SpRepository<SmartOrderSource>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_ORDERING_SOURCE_RFO] @PRACTICE_CODE, @SEARCHVALUE", parmPracticeCode, smartvalue).ToList();
                }

                if (result.Any())
                {
                    return result;
                }
                else
                    return new List<SmartOrderSource>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SmartRefRegion> GetSmartRefRegion(SmartReq obj, UserProfile Profile)
        {
            try
            {
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
                var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
                var result = SpRepository<SmartRefRegion>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_REF_REGION] @PRACTICE_CODE, @SEARCHVALUE", parmPracticeCode, smartvalue).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SmartRefRegion>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GETtAll_IndexifoRes GetAllIndexinfo(Index_infoReq obj, UserProfile Profile)
        {
            try
            {
                var wORK_ID = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = obj.WORK_ID };
                var wORK_IDProc = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = obj.WORK_ID };
                var wORK_IDCpt = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = obj.WORK_ID };
                var wORK_IDDoc = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = obj.WORK_ID };
                var result = SpRepository<GETtAll_IndexifoRes>.GetSingleObjectWithStoreProcedure(@"exec [FOX_GET_INDEX_ALL_INFO] @WORK_ID", wORK_ID);

                if (result != null)
                {
                    result.DIAGNOSIS = SpRepository<FOX_TBL_PATIENT_DIAGNOSIS>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_DIAGNOSIS] @WORK_ID", wORK_IDProc);
                    result.CPTS = SpRepository<FOX_TBL_PATIENT_PROCEDURE>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_PROCEDURES] @WORK_ID", wORK_IDCpt);
                    result.Documents = SpRepository<FOX_TBL_PATIENT_DOCUMENTS>.GetListWithStoreProcedure(@"exec  [FOX_GET_INDEX_INFO_DOCUMNETS] @WORK_ID", wORK_IDDoc);
                    var work_id = new SqlParameter("WORK_ID ", SqlDbType.BigInt) { Value = obj.WORK_ID };
                    var ID = new SqlParameter("id ", SqlDbType.VarChar) { Value = result.UNIQUE_ID };
                    result.FilePaths = SpRepository<FilePath>.GetListWithStoreProcedure(@"exec FOX_GET_File_PAGES  @WORK_ID", work_id);
                    if (result.SORCE_TYPE == "Mobile App")
                    {
                        var PracticeCode2 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = Profile.PracticeCode };
                        var _workId = new SqlParameter { ParameterName = "WORK_ID", SqlDbType = SqlDbType.BigInt, Value = obj.WORK_ID };
                        result.Guest_Login = SpRepository<GuestLogin>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_GUEST_LOGIN @PRACTICE_CODE, @WORK_ID", PracticeCode2, _workId);
                    }
                    if (result.VO_ON_BEHALF_OF != null)
                    {
                        result.OnBehalfOfSource = _InsertUpdateOrderingSourceRepository.GetByID(result.VO_ON_BEHALF_OF);
                    }
                    result.FoxDocumentTypeList = _foxdocumenttypeRepository.GetMany(d => !d.DELETED && (d.IS_ACTIVE ?? true)).OrderBy(o => o.NAME).ToList();
                    //Nouman
                    //Count the file size
                    decimal size = 0;
                    decimal byteCount = 0;
                    if (result.FilePaths != null && result.FilePaths.Any())
                    {
                        foreach (var list in result.FilePaths.ToList())
                        {
                            string virtualPath = @"/" + list.file_path1;
                            string orignalPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                            //list.file_path1;
                            FileInfo file = new FileInfo(orignalPath);
                            bool exists = file.Exists;
                            if (file.Exists)
                            {
                                byteCount = file.Length;
                                size += byteCount;
                            }
                        }
                    }
                    //Convert Bytes into Mbs.
                    result.fileSize = Convert.ToDecimal(string.Format("{0:0.00}", size / 1048576));
                    var usr = _User.GetByID(Profile.userID);
                    FOX_TBL_SENDER_TYPE senderType = new FOX_TBL_SENDER_TYPE();
                    //if (Profile.isTalkRehab)
                    //{
                    //    senderType = _SenderTypeRepository.GetFirst(t => t.FOX_TBL_SENDER_TYPE_ID == usr.FOX_TBL_SENDER_TYPE_ID && !t.DELETED);
                    //}
                    //else
                    //{
                    senderType = _SenderTypeRepository.GetSingleOrDefault(t => t.FOX_TBL_SENDER_TYPE_ID == usr.FOX_TBL_SENDER_TYPE_ID && (t.PRACTICE_CODE == usr.PRACTICE_CODE) && !t.DELETED) ?? null;
                    // }
                    result.SENDER_TYPE = senderType != null ? !string.IsNullOrWhiteSpace(senderType.SENDER_TYPE_NAME ?? "") ? senderType.SENDER_TYPE_NAME : "" : "";
                    return result;

                }
                else
                {
                    return new GETtAll_IndexifoRes();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AnalaysisReportRes> GetAnalysisRPT(AnalaysisReportReq obj, UserProfile Profile)
        {
            try
            {

                if (obj.DATEFROM_In_String == null)
                    obj.DATEFROM_In_String = "";
                if (obj.DATETO_In_String == null)
                    obj.DATETO_In_String = "";
                if (!string.IsNullOrEmpty(obj.DATEFROM_In_String))
                    obj.DATEFROM = Convert.ToDateTime(obj.DATEFROM_In_String);
                if (!string.IsNullOrEmpty(obj.DATETO_In_String))
                    obj.DATETO = Convert.ToDateTime(obj.DATETO_In_String);


                if (obj.TIMEFRAME == "DATERANGE")
                {
                    //var dateFrom = new SqlParameter("DATEFROM", SqlDbType.VarChar) { Value = obj.DATEFROM.Value.ToString("MM/dd/yyyy") };
                    var dateFrom = new SqlParameter("DATEFROM", SqlDbType.VarChar) { Value = obj.DATEFROM.HasValue ? obj.DATEFROM.Value.ToString("MM/dd/yyyy") : "" };
                    //var dateTo = new SqlParameter("DATETO", SqlDbType.VarChar) { Value = obj.DATETO.Value.ToString("MM/dd/yyyy") };
                    var dateTo = new SqlParameter("DATETO", SqlDbType.VarChar) { Value = obj.DATETO.HasValue ? obj.DATETO.Value.ToString("MM/dd/yyyy") : "" };
                    var timeFrame = new SqlParameter("TIMEFRAME", SqlDbType.VarChar) { Value = obj.TIMEFRAME };
                    var BUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                    var BUSINESS_HOURS9A = new SqlParameter("BUSINESS_HOURS9A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS9A };
                    var SATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                    var SUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                    var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                    var pAGEINDEX = new SqlParameter("PAGEINDEX", SqlDbType.BigInt) { Value = obj.PAGEINDEX };
                    var pAGESIZE = new SqlParameter("PAGESIZE", SqlDbType.BigInt) { Value = obj.PAGESIZE };
                    var result = SpRepository<AnalaysisReportRes>.GetListWithStoreProcedure(@"exec [FOX_GET_ANYLYSIS_REPORT] @DATEFROM,@DATETO,@TIMEFRAME,@BUSINESS_HOURS9A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND,@PAGEINDEX,@PAGESIZE", dateFrom, dateTo, timeFrame, BUSINESS_HOURS5A, BUSINESS_HOURS9A, SATURDAYSA, SUNDAYSA, eXCLUDEWEEKEND, pAGEINDEX, pAGESIZE).ToList();
                    if (result.Any())
                        return result;
                    else
                        return new List<AnalaysisReportRes>();
                }
                else
                {
                    var dateFrom = new SqlParameter("DATEFROM", SqlDbType.VarChar) { Value = obj.DATEFROM_In_String };
                    var dateTo = new SqlParameter("DATETO", SqlDbType.VarChar) { Value = obj.DATETO_In_String };
                    var timeFrame = new SqlParameter("TIMEFRAME", SqlDbType.VarChar) { Value = obj.TIMEFRAME };

                    var BUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                    var BUSINESS_HOURS9A = new SqlParameter("BUSINESS_HOURS9A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS9A };
                    var SATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                    var SUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                    var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                    var pAGEINDEX = new SqlParameter("PAGEINDEX", SqlDbType.BigInt) { Value = obj.PAGEINDEX };
                    var pAGESIZE = new SqlParameter("PAGESIZE", SqlDbType.BigInt) { Value = obj.PAGESIZE };
                    var result = SpRepository<AnalaysisReportRes>.GetListWithStoreProcedure(@"exec [FOX_GET_ANYLYSIS_REPORT] @DATEFROM,@DATETO,@TIMEFRAME,@BUSINESS_HOURS9A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND,@PAGEINDEX,@PAGESIZE", dateFrom, dateTo, timeFrame, BUSINESS_HOURS5A, BUSINESS_HOURS9A, SATURDAYSA, SUNDAYSA, eXCLUDEWEEKEND, pAGEINDEX, pAGESIZE).ToList();

                    if (result.Any())
                        return result;
                    else
                        return new List<AnalaysisReportRes>();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }






        public List<SlotAnalysisRes> GetSlotData(SlotAnalysisReq obj, UserProfile Profile)
        {
            try
            {

                var dATE = new SqlParameter("DATE", SqlDbType.VarChar) { Value = obj.DATE.Value.ToString("MM/dd/yyyy") };
                var sTART_VALUE = new SqlParameter("START_VALUE", SqlDbType.VarChar) { Value = obj.START_VALUE };
                var eND_VALUE = new SqlParameter("END_VALUE", SqlDbType.VarChar) { Value = obj.END_VALUE };
                var bUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                var bUSINESS_HOURS8A = new SqlParameter("BUSINESS_HOURS8A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS8A };
                var sATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                var sUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                var result = SpRepository<SlotAnalysisRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SLOT_ANALYSIS_DATA] @DATE,@START_VALUE,@END_VALUE,@BUSINESS_HOURS8A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND", dATE, sTART_VALUE, eND_VALUE, bUSINESS_HOURS8A, bUSINESS_HOURS5A, sATURDAYSA, sUNDAYSA, eXCLUDEWEEKEND).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SlotAnalysisRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<SlotAnalysisRes> GetSlotData16_30(SlotAnalysisReq obj, UserProfile Profile)
        {
            try
            {


                var dATE = new SqlParameter("DATE", SqlDbType.VarChar) { Value = obj.DATE.Value.ToString("MM/dd/yyyy") };
                var sTART_VALUE = new SqlParameter("START_VALUE", SqlDbType.VarChar) { Value = obj.START_VALUE };
                var eND_VALUE = new SqlParameter("END_VALUE", SqlDbType.VarChar) { Value = obj.END_VALUE };
                var bUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                var bUSINESS_HOURS8A = new SqlParameter("BUSINESS_HOURS8A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS8A };
                var sATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                var sUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                var result = SpRepository<SlotAnalysisRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SLOT_ANALYSIS_DATA] @DATE,@START_VALUE,@END_VALUE,@BUSINESS_HOURS8A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND", dATE, sTART_VALUE, eND_VALUE, bUSINESS_HOURS8A, bUSINESS_HOURS5A, sATURDAYSA, sUNDAYSA, eXCLUDEWEEKEND).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SlotAnalysisRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SlotAnalysisRes> GetSlotData0_15(SlotAnalysisReq obj, UserProfile Profile)
        {
            try
            {


                var dATE = new SqlParameter("DATE", SqlDbType.VarChar) { Value = obj.DATE.Value.ToString("MM/dd/yyyy") };
                var sTART_VALUE = new SqlParameter("START_VALUE", SqlDbType.VarChar) { Value = obj.START_VALUE };
                var eND_VALUE = new SqlParameter("END_VALUE", SqlDbType.VarChar) { Value = obj.END_VALUE };
                var bUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                var bUSINESS_HOURS8A = new SqlParameter("BUSINESS_HOURS8A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS8A };
                var sATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                var sUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                var result = SpRepository<SlotAnalysisRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SLOT_ANALYSIS_DATA] @DATE,@START_VALUE,@END_VALUE,@BUSINESS_HOURS8A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND", dATE, sTART_VALUE, eND_VALUE, bUSINESS_HOURS8A, bUSINESS_HOURS5A, sATURDAYSA, sUNDAYSA, eXCLUDEWEEKEND).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SlotAnalysisRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SlotAnalysisRes> GetSlotData31_45(SlotAnalysisReq obj, UserProfile Profile)
        {
            try
            {


                var dATE = new SqlParameter("DATE", SqlDbType.VarChar) { Value = obj.DATE.Value.ToString("MM/dd/yyyy") };
                var sTART_VALUE = new SqlParameter("START_VALUE", SqlDbType.VarChar) { Value = obj.START_VALUE };
                var eND_VALUE = new SqlParameter("END_VALUE", SqlDbType.VarChar) { Value = obj.END_VALUE };
                var bUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                var bUSINESS_HOURS8A = new SqlParameter("BUSINESS_HOURS8A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS8A };
                var sATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                var sUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                var result = SpRepository<SlotAnalysisRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SLOT_ANALYSIS_DATA] @DATE,@START_VALUE,@END_VALUE,@BUSINESS_HOURS8A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND", dATE, sTART_VALUE, eND_VALUE, bUSINESS_HOURS8A, bUSINESS_HOURS5A, sATURDAYSA, sUNDAYSA, eXCLUDEWEEKEND).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SlotAnalysisRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SlotAnalysisRes> GetSlotData46_60(SlotAnalysisReq obj, UserProfile Profile)
        {
            try
            {


                var dATE = new SqlParameter("DATE", SqlDbType.VarChar) { Value = obj.DATE.Value.ToString("MM/dd/yyyy") };
                var sTART_VALUE = new SqlParameter("START_VALUE", SqlDbType.VarChar) { Value = obj.START_VALUE };
                var eND_VALUE = new SqlParameter("END_VALUE", SqlDbType.VarChar) { Value = obj.END_VALUE };
                var bUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                var bUSINESS_HOURS8A = new SqlParameter("BUSINESS_HOURS8A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS8A };
                var sATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                var sUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                var result = SpRepository<SlotAnalysisRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SLOT_ANALYSIS_DATA] @DATE,@START_VALUE,@END_VALUE,@BUSINESS_HOURS8A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND", dATE, sTART_VALUE, eND_VALUE, bUSINESS_HOURS8A, bUSINESS_HOURS5A, sATURDAYSA, sUNDAYSA, eXCLUDEWEEKEND).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SlotAnalysisRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SlotAnalysisRes> GetSlotDataGreater_60(SlotAnalysisReq obj, UserProfile Profile)
        {
            try
            {

                var dATE = new SqlParameter("DATE", SqlDbType.VarChar) { Value = obj.DATE.Value.ToString("MM/dd/yyyy") };
                var sTART_VALUE = new SqlParameter("START_VALUE", SqlDbType.VarChar) { Value = obj.START_VALUE };
                var eND_VALUE = new SqlParameter("END_VALUE", SqlDbType.VarChar) { Value = obj.END_VALUE };
                var bUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                var bUSINESS_HOURS8A = new SqlParameter("BUSINESS_HOURS8A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS8A };
                var sATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                var sUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                var result = SpRepository<SlotAnalysisRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SLOT_ANALYSIS_DATA] @DATE,@START_VALUE,@END_VALUE,@BUSINESS_HOURS8A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND", dATE, sTART_VALUE, eND_VALUE, bUSINESS_HOURS8A, bUSINESS_HOURS5A, sATURDAYSA, sUNDAYSA, eXCLUDEWEEKEND).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SlotAnalysisRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SlotAnalysisRes> GetSlotDataGreater_2HR(SlotAnalysisReq obj, UserProfile Profile)
        {
            try
            {

                var dATE = new SqlParameter("DATE", SqlDbType.VarChar) { Value = obj.DATE.Value.ToString("MM/dd/yyyy") };
                var sTART_VALUE = new SqlParameter("START_VALUE", SqlDbType.VarChar) { Value = obj.START_VALUE };
                var eND_VALUE = new SqlParameter("END_VALUE", SqlDbType.VarChar) { Value = obj.END_VALUE };
                var bUSINESS_HOURS5A = new SqlParameter("BUSINESS_HOURS5A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS5A };
                var bUSINESS_HOURS8A = new SqlParameter("BUSINESS_HOURS8A", SqlDbType.VarChar) { Value = obj.BUSINESS_HOURS8A };
                var sATURDAYSA = new SqlParameter("SATURDAYSA", SqlDbType.VarChar) { Value = obj.SATURDAYSA };
                var sUNDAYSA = new SqlParameter("SUNDAYSA", SqlDbType.VarChar) { Value = obj.SUNDAYSA };
                var eXCLUDEWEEKEND = new SqlParameter("EXCLUDEWEEKEND", SqlDbType.VarChar) { Value = obj.EXCLUDEWEEKEND };
                var result = SpRepository<SlotAnalysisRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SLOT_ANALYSIS_DATA] @DATE,@START_VALUE,@END_VALUE,@BUSINESS_HOURS8A,@BUSINESS_HOURS5A,@SATURDAYSA,@SUNDAYSA,@EXCLUDEWEEKEND", dATE, sTART_VALUE, eND_VALUE, bUSINESS_HOURS8A, bUSINESS_HOURS5A, sATURDAYSA, sUNDAYSA, eXCLUDEWEEKEND).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SlotAnalysisRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Export(AnalaysisReportReq obj, UserProfile profile)
        {
            try
            {
                string fileName = "Analysis_Report";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                obj.PAGEINDEX = 1;
                obj.PAGESIZE = 1000000;
                //obj.CalledFrom = "Analysis_Report";
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
                    case "Analysis_Report":
                        {
                            var result = GetAnalysisRPT(obj, profile);
                            if (result.Count > 0)
                            {
                                var item = result[0];
                                var item2 = result[1];
                                result.RemoveAt(0);
                                result.RemoveAt(0);

                                result.Add(item);
                                result.Add(item2);
                            }
                            for (int i = 0; i < result.Count; i++)
                            {
                                result[i].ROW = i + 1;
                            }
                            exported = ExportToExcel.CreateExcelDocument<AnalaysisReportRes>(result, pathtowriteFile, obj.CalledFrom.Replace(' ', '_'));
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

        public QRCodeModel GenerateQRCode(QRCodeModel obj, UserProfile profile)
        {
            Bitmap result = null;
            string base64Image = "";
            try
            {
                var writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options.Height = 70;
                writer.Options.Width = 70;
                writer.Options.Margin = 0;

                result = writer.Write(obj.WORK_ID.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("QRcode is not generated by library", ex);
            }
            if (!Directory.Exists(obj.AbsolutePath))
            {
                Directory.CreateDirectory(obj.AbsolutePath);
            }

            using (var bitmap = new Bitmap(result))
            {
                result.Dispose();
                string curtime = obj.AbsolutePath + obj.WORK_ID + "_" + DateTime.Now.Ticks.ToString() + ".jpg";
                Bitmap cimage = (Bitmap)bitmap;
                cimage.Save(curtime, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
                cimage.Dispose();
                base64Image = Convert.ToBase64String(File.ReadAllBytes(curtime)); //Get Base64
            }
            string src = "data:image/png;base64," + base64Image;
            string encodedString = HttpUtility.UrlEncode(src);
            obj.ENCODED_IMAGE_BYTES = encodedString;
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + obj.SignPath)))
            {
                obj.SignPath = "";
            }
            return obj;
        }


        public static string GenerateFileName(long PracticeCode, string FileExtention)
        {
            return PracticeCode + "_" + DateTime.Now.ToString("ddMMyyyHHmmssffff") + FileExtention;
        }

        async Task<RespWSGetAccessToken> WSGetAccessToken()
        {
            //GET /api/Fox/GetAccessToken
            //string url = "https://uat-webservices.mtbc.com/Fox/api/Fox/GetAccessToken";
            string url = "https://mhealth.mtbc.com/fox/api/Fox/GetAccessToken";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Foxuser", "fox@700");
                var response = Task.Run(async () => { return await client.GetAsync(url); }).Result;
                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RespWSGetAccessToken>(result);
                }
            }
        }

        async Task<bool> WSSendNotification(ReqWSSendNotification reqRootObject, AccessTokenInfo accessTokenInfo)
        {
            //POST /api/GuestLogin/Send_Notification
            //string url = "https://uat-webservices.mtbc.com/Fox/api/GuestLogin/Send_Notification";
            string url = "https://mhealth.mtbc.com/fox/api/GuestLogin/Send_Notification";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessTokenInfo.token_type, accessTokenInfo.access_token);

                var json = JsonConvert.SerializeObject(reqRootObject);
                var contentType = new StringContent(json, Encoding.UTF8, "application/json");
                var responseMessage = Task.Run(async () => { return await client.PostAsync("", contentType); }).Result;

                using (HttpContent content = responseMessage.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    //result.Wait();

                    if (result.Contains("true"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                //var test = responseMessage.Content.ReadAsStringAsync();
                //test.Wait();
                //if (test.Result.Contains("true"))
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
        }

        public bool SendEmailOrFaxToSender(EmailFaxToSender model, UserProfile profile, string WORK_ID)
        {
            //long Works_id = Convert.ToInt64(WORK_ID);
            //try
            //  {
            //    if ( WORK_ID==null || WORK_ID==""|| WORK_ID=="0" ) {
            //    WORK_ID =Convert.ToString(model.work_id);
            //    model.UNIQUE_ID= Convert.ToString(model.work_id);                          
            //}

            if (model.IS_EMAIL)
            {
                return SendEmailToSender(model, profile, WORK_ID);
            }
            else
            {
                return SendFaxToSender(model, profile);
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public bool SendEmailToSender(EmailFaxToSender data, UserProfile profile, string WORK_ID)
        {
            try
            {
                int threadCounters = 0;
                AttachmentData attachmentPath = new CommonServices.CommonServices().GeneratePdfForEmailToSender(data.UNIQUE_ID.ToString(), profile);
                if (!string.IsNullOrEmpty(attachmentPath.FILE_PATH) && !string.IsNullOrEmpty(attachmentPath.FILE_NAME))
                {
                    string body = GetEmailOrFaxToSenderTemplate(data);
                    var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                    string subject = string.Empty;
                    subject = (!string.IsNullOrWhiteSpace(WORK_ID) ? "Work Order ID " + WORK_ID.ToString() : "") + " - " + (!string.IsNullOrWhiteSpace(data.SUBJECT) ? data.SUBJECT : "Query on the attached referral order");
                    string sendTo = data.EMAIL;

                    //bool sent = Helper.Email(sendTo, subject, body, profile, data.work_id, null, null, new List<string> { Path.Combine(attachmentPath.FILE_PATH, attachmentPath.FILE_NAME) });

                    bool sent = false;
                    if (profile.isTalkRehab)
                    {
                        sent = Helper.Email(sendTo, subject, body, profile, data.work_id, null, null, new List<string> { Path.Combine(attachmentPath.FILE_PATH, attachmentPath.FILE_NAME) }, "NOREPLY@CARECLOUD.COM");
                    }
                    else
                    {
                        List<int> threadCounterForEmail = new List<int>();
                        Thread Thread = new Thread(() => this.newThreadImplementaionForEmail(threadCounters, ref threadCounterForEmail, sendTo, subject, body, profile, data.work_id, null, null, new List<string> { Path.Combine(attachmentPath.FILE_PATH, attachmentPath.FILE_NAME) }));
                        Thread.Start();
                        threadsListForEmail.Add(Thread);
                        sent = true;
                    }

                    if (sent)
                    {
                        var coverFilePath = HTMLToPDFSautinsoft(config, body, "tempcoversletter");
                        SavePdfToImages(coverFilePath, config, WORK_ID, data.work_id, 1, "DR:Fax", "", profile.UserName, true);
                        foreach (var thread in threadsList)
                        {
                            thread.Abort();
                        }
                    }

                    return sent;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                //Helper.LogException(ex, profile);
                throw;
            }
        }
        private string HTMLToPDFSautinsoft(ServiceConfiguration conf, string htmlString, string fileName, string linkMessage = null)
        {
            try
            {
                PdfMetamorphosis p = new PdfMetamorphosis();
                //p.Serial = "10262870570";//server
                p.Serial = "10261942764";//development
                p.PageSettings.Size.A4();
                p.PageSettings.Orientation = PdfMetamorphosis.PageSetting.Orientations.Portrait;
                p.PageSettings.MarginLeft.Inch(0.1f);
                p.PageSettings.MarginRight.Inch(0.1f);
                if (p != null)
                {
                    string pdfFilePath = Path.Combine(conf.ORIGINAL_FILES_PATH_SERVER);
                    //string finalsetpath = conf.ORIGINAL_FILES_PATH_SERVER.Remove(conf.ORIGINAL_FILES_PATH_SERVER.Length - 1);
                    if (!Directory.Exists(pdfFilePath))
                    {
                        Directory.CreateDirectory(pdfFilePath);
                    }
                    fileName = fileName + DateTime.Now.Ticks + ".pdf";
                    string pdfFilePathnew = pdfFilePath + "\\" + fileName;
                    if (p.HtmlToPdfConvertStringToFile(htmlString, pdfFilePathnew) == 0)
                    {
                        return pdfFilePathnew;
                    }
                    else
                    {
                        var ex = p.TraceSettings.ExceptionList.Count > 0 ? p.TraceSettings.ExceptionList[0] : null;
                        var msg = ex != null ? ex.Message + Environment.NewLine + ex.StackTrace : "An error occured during converting HTML to PDF!";
                        return "";
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        public void newThreadImplementaionForEmail(int counter, ref List<int> threadCounterForEmail, string to, string subject, string body, UserProfile profile = null, long? WORK_ID = null, List<string> CC = null, List<string> BCC = null, List<string> AttachmentFilePaths = null, string from = "foxrehab@carecloud.com")
        {
            try
            {
                //bool IsMailSent = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(from);
                        mail.To.Add(new MailAddress(to));
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = Encoding.UTF8;
                        if (CC != null && CC.Count > 0)
                        {
                            foreach (var item in CC) { mail.CC.Add(item); }
                        }
                        if (BCC != null && BCC.Count > 0)
                        {
                            foreach (var item in BCC) { mail.Bcc.Add(item); }
                        }
                        if (AttachmentFilePaths != null && AttachmentFilePaths.Count > 0)
                        {
                            foreach (string filePth in AttachmentFilePaths)
                            {
                                if (File.Exists(filePth)) { mail.Attachments.Add(new Attachment(filePth)); }
                            }
                        }
                        if (profile != null && profile.isTalkRehab)
                        {
                            smtp.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["NoReplyUserName"], WebConfigurationManager.AppSettings["NoReplyPassword"]);
                        }
                        else
                        {
                            smtp.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["FoxRehabUserName"], WebConfigurationManager.AppSettings["FoxRehabPassword"]);
                        }
                        smtp.Send(mail);
                        //IsMailSent = true;
                    }
                }
                counter = 1;
                threadCounterForEmail.Add(1);
                //return IsMailSent;
            }
            catch (Exception)
            {
                threadCounterForEmail.Add(1);
            }
        }
        //New Thread Implementation
        public void SavePdfToImages(string PdfPath, ServiceConfiguration config, string workId, long lworkid, int noOfPages, string sorcetype, string sorceName, string userName, bool approval = true)
        {
            approval = false;
            var decline = false;
            List<int> threadCounter = new List<int>();
            if (!string.IsNullOrEmpty(PdfPath) && PdfPath.Contains("Signed"))
            {
                approval = true;
            }
            if (!string.IsNullOrEmpty(PdfPath) && PdfPath.Contains("Unsigned"))
            {
                decline = true;
            }
            if (!Directory.Exists(config.IMAGES_PATH_SERVER))
            {
                Directory.CreateDirectory(config.IMAGES_PATH_SERVER);
            }

            if (System.IO.File.Exists(PdfPath))
            {
                for (int i = 0; i < noOfPages; i++)
                {
                    //End
                    //string ImgDirPath = "FoxDocumentDirectory\\Fox\\Images";
                    var imgPath = "";
                    var logoImgPath = "";
                    var imgPathServer = "";
                    var logoImgPathServer = "";
                    Random random = new Random();

                    if (sorcetype.Split(':')?[0] == "DR")
                    {
                        var randomString = random.Next();
                        imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                        imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + i + "_" + randomString + ".jpg";

                        randomString = random.Next();
                        logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                        logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                    }
                    else
                    {
                        //imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + ".jpg";
                        //if (System.IO.File.Exists(imgPath))
                        //{
                        var randomString = random.Next();
                        imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                        imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + i + "_" + randomString + ".jpg";

                        //}
                        //logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + ".jpg";
                        //if (System.IO.File.Exists(imgPath))
                        //{
                        randomString = random.Next();
                        logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                        logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg"; logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                        //}
                        //imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + i + ".jpg";
                        //if (System.IO.File.Exists(imgPath))
                        //{
                        //randomString = random.Next();
                        //}
                        //logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + ".jpg";
                        //if (System.IO.File.Exists(imgPath))
                        //{
                        //randomString = random.Next();

                        //}
                    }

                    Thread myThread = new Thread(() => this.newThreadImplementaion(ref threadCounter, PdfPath, i, imgPathServer, logoImgPathServer));
                    myThread.Start();
                    if (workId != null && workId.Contains("_"))
                    {
                        var uWorkId = _OriginalQueueFiles.GetFirst(t => t.UNIQUE_ID == workId);
                        if (uWorkId != null)
                        {
                            workId = Convert.ToString(uWorkId.WORK_ID);
                        }
                    }
                    threadsList.Add(myThread);
                    AddFilesToDatabase(imgPath, workId, lworkid, logoImgPath);
                }
                while (noOfPages > threadCounter.Count)
                {
                    //loop untill record complete
                }
                foreach (var thread in threadsList)
                {
                    thread.Abort();
                }
                //noOfPages = noOfPages + 1;
                long ConvertedWorkID = Convert.ToInt64(workId);
                noOfPages = _OriginalQueueFiles.GetMany(t => t.WORK_ID == ConvertedWorkID && !t.deleted)?.Count() ?? 0;
                AddToDatabase(PdfPath, noOfPages, workId, sorcetype, sorceName, userName, approval, config.PRACTICE_CODE, decline);
            }
        }
        public void newThreadImplementaion(ref List<int> threadCounter, string PdfPath, int i, string imgPath, string logoImgPath)
        {
            try
            {
                System.Drawing.Image img;
                PdfFocus f = new PdfFocus();
               // f.Serial = "10261435399";
                f.Serial = "80033727929";
                f.OpenPdf(PdfPath);

                if (f.PageCount > 0)
                {
                    //Save all PDF pages to jpeg images
                    f.ImageOptions.Dpi = 120;
                    f.ImageOptions.ImageFormat = ImageFormat.Jpeg;

                    var image = f.ToImage(i + 1);
                    //Next manipulate with Jpeg in memory or save to HDD, open in a viewer
                    using (var ms = new MemoryStream(image))
                    {
                        img = System.Drawing.Image.FromStream(ms);
                        img.Save(imgPath, ImageFormat.Jpeg);
                        Bitmap bmp = new Bitmap(img);
                        ConvertPDFToImages ctp = new ConvertPDFToImages();
                        img.Dispose();
                        ctp.SaveWithNewDimention(bmp, 115, 150, 100, logoImgPath);
                        bmp.Dispose();
                    }
                }
                threadCounter.Add(1);
            }
            catch (Exception)
            {
                threadCounter.Add(1);
            }
        }
        private void AddFilesToDatabase(string filePath, string workId, long lworkid, string logoPath)
        {
            try
            {
                //OriginalQueueFiles originalQueueFiles = _OriginalQueueFiles.GetFirst(t => t.UNIQUE_ID == workId && !t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));

                //if (originalQueueFiles == null)
                //{
                //    //If Work Order files is deleted
                //    originalQueueFiles = _OriginalQueueFiles.Get(t => t.UNIQUE_ID == workId && t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));
                //    if (originalQueueFiles == null)
                //    {
                //        originalQueueFiles = new OriginalQueueFiles();

                //        originalQueueFiles.FILE_ID = Helper.getMaximumId("FOXREHAB_FILE_ID");
                //        originalQueueFiles.WORK_ID = lworkid;
                //        originalQueueFiles.UNIQUE_ID = workId.ToString();
                //        originalQueueFiles.FILE_PATH1 = filePath;
                //        originalQueueFiles.FILE_PATH = logoPath;
                //        originalQueueFiles.deleted = false;

                //        //_OriginalQueueFiles.Insert(originalQueueFiles);
                //        //_OriginalQueueFiles.Save();
                //    }
                //}
                //else
                //{
                //    originalQueueFiles.FILE_PATH1 = filePath;
                //    originalQueueFiles.FILE_PATH = logoPath;

                //    //_OriginalQueueFiles.Update(originalQueueFiles);
                //    //_OriginalQueueFiles.Save();
                //}


                long iD = Helper.getMaximumId("FOXREHAB_FILE_ID");
                var fileId = new SqlParameter("FILE_ID", SqlDbType.BigInt) { Value = iD };
                var parmWorkID = new SqlParameter("WORKID", SqlDbType.BigInt) { Value = workId };
                var parmFilePath = new SqlParameter("FILEPATH", SqlDbType.VarChar) { Value = filePath };
                var parmLogoPath = new SqlParameter("LOGOPATH", SqlDbType.VarChar) { Value = logoPath };
                var _isFromIndexInfo = new SqlParameter("IS_FROM_INDEX_INFO", SqlDbType.Bit) { Value = true };

                var result = SpRepository<OriginalQueueFiles>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_AD_FILES_TO_DB_FROM_RFO @FILE_ID, @WORKID, @FILEPATH, @LOGOPATH, @IS_FROM_INDEX_INFO",
                    fileId, parmWorkID, parmFilePath, parmLogoPath, _isFromIndexInfo);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void AddToDatabase(string filePath, int noOfPages, string workId, string sorcetype, string sorceName, string userName, bool approval, long? practice_code, bool decline)
        {
            try
            {
                //OriginalQueue originalQueue = _QueueRepository.Get(t => t.UNIQUE_ID == workId && !t.DELETED);
                //var documentType = _foxdocumenttypeRepository.GetFirst(t => !t.DELETED && t.NAME == "Signed Order");
                //var userId = _User.GetFirst(x => x.USER_NAME == userName && !x.DELETED).USER_ID;
                //if (originalQueue != null)
                //{
                //    //TO DO
                //    //originalQueue.SORCE_TYPE = sorcetype;
                //    //originalQueue.SORCE_NAME = sorceName;
                //    //originalQueue.WORK_STATUS = "Indexed";
                //    if (approval)
                //    {
                //        originalQueue.DOCUMENT_TYPE = documentType.DOCUMENT_TYPE_ID;
                //        originalQueue.IsSigned = true;
                //    }
                //    originalQueue.TOTAL_PAGES = noOfPages;
                //    //originalQueue.FILE_PATH = filePath;
                //    originalQueue.FAX_ID = "";

                //    originalQueue.SignedBy = userId;
                //    originalQueue.MODIFIED_BY = userName;
                //    originalQueue.MODIFIED_DATE = DateTime.Now;

                ////_QueueRepository.Update(originalQueue);
                ////_QueueRepository.Save();
                //}


                var PracticeCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practice_code };
                var workid = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };
                var username = new SqlParameter { ParameterName = "@USER_NAME", SqlDbType = SqlDbType.VarChar, Value = userName };
                var noofpages = new SqlParameter { ParameterName = "@NO_OF_PAGES", SqlDbType = SqlDbType.Int, Value = noOfPages };
                var approve = new SqlParameter { ParameterName = "@APPROVAL", SqlDbType = SqlDbType.Bit, Value = approval };
                var declined = new SqlParameter { ParameterName = "@DECLINE", SqlDbType = SqlDbType.Bit, Value = decline };

                var result = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_ADD_TODB_FROM_INDEXINFO @PRACTICE_CODE,@WORK_ID,@USER_NAME,@NO_OF_PAGES,@APPROVAL,@DECLINE",
                    PracticeCode, workid, username, noofpages, approve, declined);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SendFaxToSender(EmailFaxToSender data, UserProfile profile)
        {
            var commonService = new CommonServices.CommonServices();
            AttachmentData attachmentPath = commonService.GeneratePdfForEmailToSender(data.UNIQUE_ID.ToString(), profile);
            try
            {
                if (!string.IsNullOrEmpty(attachmentPath.FILE_PATH) && !string.IsNullOrEmpty(attachmentPath.FILE_NAME))
                {
                    string coverLetterTemplate = GetEmailOrFaxToSenderTemplate(data);

                    var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                    var coverFilePath = HTMLToPDFSautinsoft(config, coverLetterTemplate, "tempcoversletter");
                    SavePdfToImages(coverFilePath, config, data.UNIQUE_ID, data.work_id, 1, "DR:Fax", "", profile.UserName, true);
                    string newFileName = commonService.AddCoverPageForFax(attachmentPath.FILE_PATH, attachmentPath.FILE_NAME, coverLetterTemplate);

                    if (!attachmentPath.FILE_PATH.EndsWith("\\"))
                    {
                        attachmentPath.FILE_PATH = attachmentPath.FILE_PATH + "\\";
                    }

                    string subject = !string.IsNullOrWhiteSpace(data.SUBJECT) ? data.SUBJECT : "Query on the attached referral order";

                    //var fax = _IFaxService.SendFax(new string[] { data.FAX }, new string[] { "" }, new string[] { coverLetterTemplate }, attachmentPath.FILE_NAME, attachmentPath.FILE_PATH, subject, false, profile);

                    var fax = _IFaxService.SendFax(new string[] { data.FAX }, new string[] { "" }, new string[] { }, newFileName, attachmentPath.FILE_PATH, subject, false, profile);

                    string[] result = fax.Split(',');
                    if ((result[1].Equals("True") || result[1].Equals("true")) && (!result[2].Equals("false") || !result[2].Equals("False")))
                    {
                        Helper.LogFaxData(to: data.FAX, status: "Success", profile: profile, ex: null, work_id: data.work_id, attachments: new List<string> { Path.Combine(string.IsNullOrEmpty(attachmentPath.FILE_PATH) ? string.Empty : attachmentPath.FILE_PATH, string.IsNullOrEmpty(attachmentPath.FILE_NAME) ? string.Empty : attachmentPath.FILE_NAME) });
                        return true;
                    }
                    else
                    {
                        Helper.LogFaxData(to: data.FAX, status: "Failed", profile: profile, ex: "FAX could not be sent.", work_id: data.work_id, attachments: new List<string> { Path.Combine(string.IsNullOrEmpty(attachmentPath.FILE_PATH) ? string.Empty : attachmentPath.FILE_PATH, string.IsNullOrEmpty(attachmentPath.FILE_NAME) ? string.Empty : attachmentPath.FILE_NAME) });

                        throw new Exception(fax);
                        //return false;
                    }
                }
                else
                {
                    //Helper.LogFaxData(to: data.FAX, status: "Failed", profile: profile,   ex: "PDF could not be generated.", work_id: data.work_id, attachments: new List<string> { Path.Combine(string.IsNullOrEmpty(attachmentPath.FILE_PATH) ? string.Empty : attachmentPath.FILE_PATH, string.IsNullOrEmpty(attachmentPath.FILE_NAME) ? string.Empty : attachmentPath.FILE_NAME) });
                    //throw new Exception("PDF could not be generated.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Helper.LogFaxData(to: data.FAX, status: "Failed", profile: profile, ex: ex.Message, work_id: data.work_id, attachments: new List<string> { Path.Combine(string.IsNullOrEmpty(attachmentPath.FILE_PATH) ? string.Empty : attachmentPath.FILE_PATH, string.IsNullOrEmpty(attachmentPath.FILE_NAME) ? string.Empty : attachmentPath.FILE_NAME) });
                throw;
            }
        }

        public string GetEmailOrFaxToSenderTemplate(EmailFaxToSender data)
        {
            string templatePathOfSenderEmail = string.Empty;
            string body = string.Empty;
            if (EntityHelper.isTalkRehab)
            {
                templatePathOfSenderEmail = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/email_fax_to_sender_template_ccremote.html");
            }
            else
            {
                templatePathOfSenderEmail = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/email_fax_to_sender_template.html");
            }
            if (File.Exists(templatePathOfSenderEmail))
            {
                body = File.ReadAllText(templatePathOfSenderEmail);
                string receivedDate = string.Empty;
                string receivedTime = string.Empty;
                if (data.From_Index_Info)
                {
                    var workorder = _InsertSourceAddRepository.GetFirst(e => e.UNIQUE_ID == data.UNIQUE_ID);
                    if (workorder != null)
                    {
                        receivedDate = workorder.RECEIVE_DATE.HasValue ? workorder.RECEIVE_DATE.Value.ToString("MM/dd/yyyy") : "N/A";
                        receivedTime = workorder.RECEIVE_DATE.HasValue ? workorder.RECEIVE_DATE.Value.ToString("hh:mm tt") : "N/A";
                    }
                    else
                    {
                        workorder = _InsertSourceAddRepository.GetFirst(e => e.WORK_ID == data.work_id);
                        if (workorder != null)
                        {
                            receivedDate = workorder.RECEIVE_DATE.HasValue ? workorder.RECEIVE_DATE.Value.ToString("MM/dd/yyyy") : "N/A";
                            receivedTime = workorder.RECEIVE_DATE.HasValue ? workorder.RECEIVE_DATE.Value.ToString("hh:mm tt") : "N/A";
                        }
                    }
                    body = body.Replace("[[SENT_TO]]", (EntityHelper.isTalkRehab) ? "CareCloud Remote" : "FOX Rehab");
                }
                else
                {
                    receivedDate = Helper.GetCurrentDate().ToString("MM/dd/yyyy");
                    receivedTime = Helper.GetCurrentDate().ToString("hh:mm tt");
                    if (data.IS_EMAIL)
                    {
                        body = body.Replace("[[SENT_TO]]", data.EMAIL ?? ((EntityHelper.isTalkRehab) ? "CareCloud Remote" : "FOX Rehab"));
                    }
                    else
                    {
                        body = body.Replace("[[SENT_TO]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(data.FAX) ?? ((EntityHelper.isTalkRehab) ? "CareCloud Remote" : "FOX Rehab"));
                    }
                }
                var sendingDate = Helper.GetCurrentDate().ToString("MM/dd/yyyy") + " " + Helper.GetCurrentDate().ToString("hh:mm tt");
                body = body.Replace("[[RECEIVED_DATE]]", receivedDate);
                body = body.Replace("[[RECEIVED_TIME]]", receivedTime);
                body = body.Replace("[[BODY]]", data.BODY);
                body = body.Replace("[[SENDING_DATE]]", sendingDate);
                if (data != null && !string.IsNullOrEmpty(data.EMAIL))
                {
                    body = body.Replace("[[EMAIL]]", data.EMAIL);
                }
                else
                {
                    body = body.Replace("[[EMAIL]]", data.FAX);
                }
            }
            return body;
        }

        public IndexInfoInitialData GetIndexInfoInitialData(long workId, long _practiceCode)
        {
            IndexInfoInitialData _indexInfoInitialData = new IndexInfoInitialData();

            var _workIdRes = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = workId };
            var _workIdProc = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = workId };
            var _workIdCpt = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = workId };
            var workIdDoc = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = workId };
            var _indexInfo = SpRepository<GETtAll_IndexifoRes>.GetSingleObjectWithStoreProcedure(@"exec [FOX_GET_INDEX_ALL_INFO] @WORK_ID", _workIdRes);
            if (_indexInfo != null)
            {
                _indexInfo.DIAGNOSIS = SpRepository<FOX_TBL_PATIENT_DIAGNOSIS>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_DIAGNOSIS] @WORK_ID", _workIdProc);
                _indexInfo.CPTS = SpRepository<FOX_TBL_PATIENT_PROCEDURE>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_PROCEDURES] @WORK_ID", _workIdCpt);
                _indexInfo.Documents = SpRepository<FOX_TBL_PATIENT_DOCUMENTS>.GetListWithStoreProcedure(@"exec  [FOX_GET_INDEX_INFO_DOCUMNETS] @WORK_ID", workIdDoc);
                var workIdPage = new SqlParameter("WORK_ID ", SqlDbType.BigInt) { Value = workId };
                var ID = new SqlParameter("id ", SqlDbType.VarChar) { Value = _indexInfo.UNIQUE_ID };
                _indexInfo.FilePaths = SpRepository<FilePath>.GetListWithStoreProcedure(@"exec FOX_GET_File_PAGES  @WORK_ID", workIdPage);
                if (_indexInfo.SORCE_TYPE == "Mobile App")
                {
                    var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = _practiceCode };
                    var _workIdLog = new SqlParameter { ParameterName = "WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };

                    _indexInfo.Guest_Login = SpRepository<GuestLogin>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_GUEST_LOGIN @PRACTICE_CODE, @WORK_ID", practiceCode, _workIdLog);
                }
                _indexInfoInitialData.IndexIfoRes = _indexInfo;
            }
            else
                _indexInfoInitialData.IndexIfoRes = new GETtAll_IndexifoRes();

            //Notes History.
            var practiceHis = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = workId };
            var noteshistory = SpRepository<FOX_TBL_NOTES_HISTORY>.GetListWithStoreProcedure(@"exec [FOX_GET_NOTES_HISTORY] @WORK_ID", practiceHis).ToList();
            if (noteshistory != null)
                _indexInfoInitialData.NotesHistory = noteshistory;
            else
                _indexInfoInitialData.NotesHistory = new List<FOX_TBL_NOTES_HISTORY>();

            //Procedures.
            var _workIdProc2 = new SqlParameter("WORK_ID", SqlDbType.VarChar) { Value = workId };
            var _procedures = SpRepository<FOX_TBL_PATIENT_PROCEDURE>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_PROCEDURES] @WORK_ID", _workIdProc2).ToList();
            if (_procedures != null)
                _indexInfoInitialData.proceduresList = _procedures;
            else
                _indexInfoInitialData.proceduresList = new List<FOX_TBL_PATIENT_PROCEDURE>();

            //Diagnosis.
            var _workIdDiag = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = workId };
            var _diagnosis = SpRepository<FOX_TBL_PATIENT_DIAGNOSIS>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_DIAGNOSIS] @WORK_ID", _workIdDiag).ToList();
            if (_diagnosis != null)
                _indexInfoInitialData.DiagnosisList = _diagnosis;
            else
                _indexInfoInitialData.DiagnosisList = new List<FOX_TBL_PATIENT_DIAGNOSIS>();

            return _indexInfoInitialData;
        }

        public FacilityLocation GetFacilityIfNoAlreadyExist(long id, UserProfile userProfile, string patientAccount)
        {
            long patientAccountOut;
            long patientAccountLong;
            if (Int64.TryParse(patientAccount, out patientAccountOut))
            {
                patientAccountLong = patientAccountOut;
                if (_PatientPOSLocationRepository.GetFirst(t => t.Loc_ID == id && t.Patient_Account == patientAccountLong && !(t.Deleted ?? false)) != null)
                {
                    return null;
                }
            }
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = userProfile.PracticeCode };
            var _paramsLocId = new SqlParameter("LOC_ID", SqlDbType.BigInt) { Value = id };
            return SpRepository<FacilityLocation>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_FACILITY_BY_ID] @PRACTICE_CODE, @LOC_ID", _paramsPracticeCode, _paramsLocId);
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

        public List<FoxDocumentType> getDocumentTypes(UserProfile profile)
        {
            var DocumentTypes = _foxdocumenttypeRepository.GetMany(d => !d.DELETED && (d.IS_ACTIVE ?? true) && d.IS_ONLINE_ORDER_LIST == true);
            //var DocumentTypes = (from row in _IndexinfoContext.FoxDocumentType orderby row.NAME == "other" ? 1 : 0 select row).ToList();
            return DocumentTypes;
        }
        public int InsertInterfaceTeamData(InterfaceSynchModel obj, UserProfile Profile)
        {
            try
            {
                bool isSync = false;
                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();

                if (obj.PATIENT_ACCOUNT == null && obj.CASE_ID == null && obj.TASK_ID == null)
                {

                }
                else
                {
                    //               CREATE PROCEDURE FOX_PROC_INSERT_INTERFACE_DATA

                    //@CASE_ID BIGINT,
                    //   @Work_ID BIGINT,
                    //@TASK_ID BIGINT,
                    //   @PATIENT_ACCOUNT BIGINT,
                    //@USER_NAME VARCHAR(70),
                    obj.APPLICATION = "PORTAL";
                    //   @IS_SYNCED BIT

                    SqlParameter pracCode = new SqlParameter("@Practice_code", Profile.PracticeCode);
                    var response = SpRepository<string>.GetSingleObjectWithStoreProcedure(@"Exec Af_proc_is_talkrehab_practice @Practice_code", pracCode);
                    if (response == null)
                    {
                        isSync = false;
                    }
                    else
                    {
                        isSync = true;
                    }
                    long Pid = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                    SqlParameter id = new SqlParameter("ID", Pid);

                    SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", Profile.PracticeCode);
                    SqlParameter caseId = new SqlParameter("CASE_ID", obj.CASE_ID ?? (object)DBNull.Value);
                    SqlParameter workId = new SqlParameter("Work_ID", obj.Work_ID ?? (object)DBNull.Value);
                    SqlParameter taskId = new SqlParameter("TASK_ID", obj.TASK_ID ?? (object)DBNull.Value);
                    SqlParameter patientAccount = new SqlParameter("PATIENT_ACCOUNT", obj.PATIENT_ACCOUNT);
                    SqlParameter userName = new SqlParameter("USER_NAME", Profile.UserName);
                    SqlParameter isSynced = new SqlParameter("IS_SYNCED", isSync);
                    SqlParameter application = new SqlParameter("APPLICATION", string.IsNullOrEmpty(obj.APPLICATION) ? string.Empty : obj.APPLICATION);
                    var result = SpRepository<InterfaceSynchModel>.GetListWithStoreProcedure(@"FOX_PROC_INSERT_INTERFACE_DATA @ID, @PRACTICE_CODE,@CASE_ID,@Work_ID,@TASK_ID,@PATIENT_ACCOUNT,@USER_NAME,@IS_SYNCED,@APPLICATION",
                                                                                                           id, practiceCode, caseId, workId, taskId, patientAccount, userName, isSynced, application);
                    if (Profile.isTalkRehab)
                    {
                        talkRehabInterfaceID = Pid;
                    }
                    else
                    {
                        talkRehabInterfaceID = 0;
                    }
                    return result.Count;
                    //interfaceSynch.FOX_INTERFACE_SYNCH_ID = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                    //interfaceSynch.CASE_ID = obj.CASE_ID;
                    //interfaceSynch.Work_ID = obj.Work_ID;
                    //interfaceSynch.TASK_ID = obj.TASK_ID;
                    //interfaceSynch.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                    //interfaceSynch.PRACTICE_CODE = Profile.PracticeCode;
                    //interfaceSynch.MODIFIED_BY = interfaceSynch.CREATED_BY = Profile.UserName;
                    //interfaceSynch.MODIFIED_DATE = interfaceSynch.CREATED_DATE = DateTime.Now;
                    //interfaceSynch.DELETED = false;
                    //interfaceSynch.IS_SYNCED = false;
                    //__InterfaceSynchModelRepository.Insert(interfaceSynch);
                    //_CaseContext.SaveChanges();
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public PatientInfoChecklist GetPatientInfoChecklist(long patientAccount)
        {
            var _patientAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = patientAccount };
            var _procedures = SpRepository<PatientInfoChecklist>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_PATIENT_INFO_CHECKLIST] @PATIENT_ACCOUNT", _patientAccount);
            return _procedures;
        }

        public FOX_TBL_TASK AddUpdateTask(FOX_TBL_TASK task, UserProfile profile, OriginalQueue WORK_QUEUE)
        {

            if (!string.IsNullOrEmpty(task.PATIENT_ACCOUNT_STR))
            {
                task.PATIENT_ACCOUNT = Convert.ToInt64(task.PATIENT_ACCOUNT_STR);
            }
            if (task != null && profile != null)
            {
                FOX_TBL_TASK dbTask = GetTask(profile.PracticeCode, task.TASK_ID);
                //FOX_TBL_TASK dbTask = _TaskRepository.GetFirst(t => t.PRACTICE_CODE == profile.PracticeCode && t.TASK_ID == task.TASK_ID);
                if (dbTask == null)
                {
                    SqlParameter sendToId;
                    if (profile.isTalkRehab)
                    {
                        try
                        {
                            long talkRehabGroupID = AddTalkRehabGroup(profile);
                            sendToId = new SqlParameter("SEND_TO_ID", talkRehabGroupID);
                        }
                        catch (Exception)
                        {
                            sendToId = new SqlParameter("SEND_TO_ID", task.SEND_TO_ID ?? (object)DBNull.Value);
                        }
                    }
                    else
                    {
                        sendToId = new SqlParameter("SEND_TO_ID", task.SEND_TO_ID ?? (object)DBNull.Value);
                    }
                    long primaryKey = Helper.getMaximumId("FOX_TASK_ID");
                    SqlParameter id = new SqlParameter("ID", primaryKey);
                    SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                    SqlParameter patientAccount = new SqlParameter("PATIENT_ACCOUNT", task.PATIENT_ACCOUNT);
                    SqlParameter isCompletedInt = new SqlParameter("IS_COMPLETED_INT", SqlDbType.Int);
                    isCompletedInt.Value = 0;// new SqlParameter("IS_COMPLETED_INT", 0);/*0: Initiated 1:Sender Completed 2:Final Route Completed*/
                    SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                    SqlParameter isTemplate = new SqlParameter("IS_TEMPLATE", task.IS_TEMPLATE);
                    SqlParameter taskTypeId = new SqlParameter("TASK_TYPE_ID", task.TASK_TYPE_ID);
                    SqlParameter finalRouteId = new SqlParameter("FINAL_ROUTE_ID", task.FINAL_ROUTE_ID ?? (object)DBNull.Value);
                    SqlParameter priority = new SqlParameter("PRIORITY", string.IsNullOrEmpty(task.PRIORITY) ? string.Empty : task.PRIORITY);
                    SqlParameter dueDateTime = new SqlParameter("DUE_DATE_TIME", string.IsNullOrEmpty(task.DUE_DATE_TIME_str) ? (object)DBNull.Value : Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str) ?? Helper.GetCurrentDate());
                    task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                    SqlParameter categoryID = new SqlParameter("CATEGORY_ID", task.CATEGORY_ID ?? (object)DBNull.Value);
                    SqlParameter isReqSignedoff = new SqlParameter("IS_REQ_SIGNOFF", task.IS_REQ_SIGNOFF ?? (object)DBNull.Value);
                    SqlParameter isSendingRouteDetails = new SqlParameter("IS_SENDING_ROUTE_DETAILS", task.IS_SENDING_ROUTE_DETAILS ?? (object)DBNull.Value);
                    SqlParameter sendContextId = new SqlParameter("SEND_CONTEXT_ID", task.SEND_CONTEXT_ID ?? (object)DBNull.Value);
                    SqlParameter contextInfo = new SqlParameter("CONTEXT_INFO", task.CONTEXT_INFO ?? (object)DBNull.Value);
                    SqlParameter deliveryId = new SqlParameter("DEVELIVERY_ID", task.DEVELIVERY_ID ?? (object)DBNull.Value);
                    SqlParameter destinations = new SqlParameter("DESTINATIONS", task.DESTINATIONS ?? (object)DBNull.Value);
                    SqlParameter loc_Id = new SqlParameter("LOC_ID", task.LOC_ID ?? (object)DBNull.Value);
                    SqlParameter provider_ID = new SqlParameter("PROVIDER_ID", task.PROVIDER_ID ?? (object)DBNull.Value);
                    SqlParameter isSendMailAuto = new SqlParameter("IS_SEND_EMAIL_AUTO", task.IS_SEND_EMAIL_AUTO ?? (object)DBNull.Value);
                    SqlParameter deleted = new SqlParameter("DELETED", task.DELETED);
                    SqlParameter isSendToUser = new SqlParameter("IS_SEND_TO_USER", task.IS_SEND_TO_USER);
                    SqlParameter isFinalRouteUser = new SqlParameter("IS_FINAL_ROUTE_USER", task.IS_FINAL_ROUTE_USER);
                    SqlParameter isFinalRouteMarkComplete = new SqlParameter("IS_FINALROUTE_MARK_COMPLETE", task.IS_FINALROUTE_MARK_COMPLETE);
                    SqlParameter isSendToMarkComplete = new SqlParameter("IS_SENDTO_MARK_COMPLETE", task.IS_SENDTO_MARK_COMPLETE);

                    dbTask = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_TASK @ID, @PRACTICE_CODE,@PATIENT_ACCOUNT ,@IS_COMPLETED_INT ,@USER_NAME,@IS_TEMPLATE,@TASK_TYPE_ID,@SEND_TO_ID,@FINAL_ROUTE_ID,@PRIORITY,@DUE_DATE_TIME,@CATEGORY_ID,@IS_REQ_SIGNOFF,@IS_SENDING_ROUTE_DETAILS,@SEND_CONTEXT_ID,@CONTEXT_INFO,@DEVELIVERY_ID,@DESTINATIONS,@LOC_ID,@PROVIDER_ID,@IS_SEND_EMAIL_AUTO,@DELETED,@IS_SEND_TO_USER,@IS_FINAL_ROUTE_USER,@IS_FINALROUTE_MARK_COMPLETE,@IS_SENDTO_MARK_COMPLETE",
                                                                                                                    id, practiceCode, patientAccount, isCompletedInt, userName, isTemplate, taskTypeId, sendToId, finalRouteId, priority, dueDateTime, categoryID, isReqSignedoff, isSendingRouteDetails, sendContextId, contextInfo, deliveryId, destinations, loc_Id, provider_ID, isSendMailAuto, deleted, isSendToUser, isFinalRouteUser, isFinalRouteMarkComplete, isSendToMarkComplete);
                    if (profile.isTalkRehab)
                    {
                        talkRehabTaskID = primaryKey;
                    }
                    else
                    {
                        talkRehabTaskID = 0;
                    }

                    //dbTask = new FOX_TBL_TASK();
                    //dbTask.TASK_ID = Helper.getMaximumId("FOX_TASK_ID");
                    //dbTask.PRACTICE_CODE = profile.PracticeCode;
                    //dbTask.PATIENT_ACCOUNT = task.PATIENT_ACCOUNT;
                    //dbTask.IS_COMPLETED_INT = 0;/*0: Initiated 1:Sender Completed 2:Final Route Completed*/
                    //dbTask.CREATED_BY = dbTask.MODIFIED_BY = profile.UserName;
                    //dbTask.CREATED_DATE = dbTask.MODIFIED_DATE = Helper.GetCurrentDate();
                    //dbTask.IS_TEMPLATE = task.IS_TEMPLATE;
                    if (WORK_QUEUE != null)
                    {
                        AddTaskLogs(dbTask, task, profile, WORK_QUEUE);
                    }
                    //if (task.DUE_DATE_TIME != null)
                    //{
                    //    if (dbTask.DUE_DATE_TIME.HasValue && dbTask.DUE_DATE_TIME.Value.Date != task.DUE_DATE_TIME.Value.Date)
                    //    {
                    //        task.Is_Change = true;
                    //    }
                    //}
                    //dbTask.TASK_TYPE_ID = task.TASK_TYPE_ID;
                    //dbTask.SEND_TO_ID = task.SEND_TO_ID;
                    //dbTask.FINAL_ROUTE_ID = task.FINAL_ROUTE_ID;
                    //dbTask.PRIORITY = task.PRIORITY;
                    //dbTask.DUE_DATE_TIME = task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                    //dbTask.CATEGORY_ID = task.CATEGORY_ID;
                    //dbTask.IS_REQ_SIGNOFF = task.IS_REQ_SIGNOFF;
                    //dbTask.IS_SENDING_ROUTE_DETAILS = task.IS_SENDING_ROUTE_DETAILS;
                    //dbTask.SEND_CONTEXT_ID = task.SEND_CONTEXT_ID;
                    //dbTask.CONTEXT_INFO = task.CONTEXT_INFO;
                    //dbTask.DEVELIVERY_ID = task.DEVELIVERY_ID;
                    //dbTask.DESTINATIONS = task.DESTINATIONS;
                    //dbTask.LOC_ID = task.LOC_ID;
                    //dbTask.PROVIDER_ID = task.PROVIDER_ID;
                    //dbTask.IS_SEND_EMAIL_AUTO = task.IS_SEND_EMAIL_AUTO;
                    //dbTask.DELETED = task.DELETED;
                    //dbTask.IS_TEMPLATE = task.IS_TEMPLATE;
                    //dbTask.IS_SEND_TO_USER = task.IS_SEND_TO_USER;
                    //dbTask.IS_FINAL_ROUTE_USER = task.IS_FINAL_ROUTE_USER;
                    //dbTask.IS_FINALROUTE_MARK_COMPLETE = task.IS_FINALROUTE_MARK_COMPLETE;
                    //dbTask.IS_SENDTO_MARK_COMPLETE = task.IS_SENDTO_MARK_COMPLETE;
                    AddEditTask_TaskSubTypesforPORTA(dbTask.TASK_ID, task.TASK_TYPE_ID, profile, task.PATIENT_ACCOUNT, WORK_QUEUE.FOX_SOURCE_CATEGORY_ID);

                    //if (dbTask.DUE_DATE_TIME == null)
                    //{
                    //    dbTask.DUE_DATE_TIME = task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                    //}                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(InterfaceSynch, profile);
                    dbTask.dbChangeMsg = "TaskInsertSuccessed";
                    dbTask.CATEGORY_CODE = _taskTypeRepository.GetByID(dbTask?.TASK_TYPE_ID)?.CATEGORY_CODE;
                    //_TaskRepository.Insert(dbTask);
                    //_TaskRepository.Save();
                }

                return dbTask;
            }
            return null;
        }

        public bool checkPatientisInterfaced(long? PATIENT_ACCOUNT, UserProfile profile)
        {
            var Patient_interfaced = GetSynchedPatient(profile.PracticeCode, PATIENT_ACCOUNT);
            //var Patient_interfaced = __InterfaceSynchModelRepository.GetFirst(t => (t.DELETED == false) && t.PRACTICE_CODE == profile.PracticeCode && t.PATIENT_ACCOUNT == PATIENT_ACCOUNT && (t.IS_SYNCED ?? false));
            var patient = GetPatient(PATIENT_ACCOUNT);
            //var patient = _PatientRepository.GetFirst(t => t.Patient_Account == PATIENT_ACCOUNT && !(t.DELETED ?? false));
            bool IS_PATIENT_INTERFACE_SYNCED = false;
            if ((patient != null && !String.IsNullOrWhiteSpace(patient.Chart_Id)) || Patient_interfaced != null)
            {
                IS_PATIENT_INTERFACE_SYNCED = true;
            }
            else
            {
                IS_PATIENT_INTERFACE_SYNCED = false;
            }
            return IS_PATIENT_INTERFACE_SYNCED;
        }

        private void AddEditTask_TaskSubTypesforPORTA(long tASK_ID, int? task_type_id, UserProfile profile, long? PATIENT_ACCOUNT, long? FOX_SOURCE_CATEGORY_ID)
        {
            bool IS_CHECKED = false;
            bool patientInterfaced = checkPatientisInterfaced(PATIENT_ACCOUNT, profile);
            var taskSubTypeList = GetTaskSubTypes(profile.PracticeCode, task_type_id);
            //var taskSubTypeList = _taskSubTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && t.DELETED == false && t.TASK_TYPE_ID == task_type_id);
            var taskTypes = GetTaskTypes(profile.PracticeCode);
            //var taskTypes = _taskTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);
            var referralSource = _referralSourceTableRepository.GetFirst(t => t.FOX_SOURCE_CATEGORY_ID == FOX_SOURCE_CATEGORY_ID && t.DELETED == false);
            //var referralSource2 = _speciality.GetFirst(x => x.SPECIALITY_ID.ToString() == FOX_SOURCE_CATEGORY_ID.ToString());
            var currentTaskType = taskTypes.Find(e => e.TASK_TYPE_ID == task_type_id);
            foreach (var taskSubType in taskSubTypeList)
            {
                if (FOX_SOURCE_CATEGORY_ID == null && taskSubType.NAME.ToLower() == "new client" && patientInterfaced == false)
                {
                    IS_CHECKED = true;
                }
                else if (FOX_SOURCE_CATEGORY_ID == null && !string.IsNullOrEmpty(taskSubType.NAME) && taskSubType.NAME.ToLower() == "existing client" && patientInterfaced == true)
                {
                    IS_CHECKED = true;
                }
                else if (FOX_SOURCE_CATEGORY_ID != null && referralSource != null && !string.IsNullOrEmpty(referralSource.DESCRIPTION) && !string.IsNullOrEmpty(taskSubType.NAME)
                    && referralSource.DESCRIPTION.ToLower() == taskSubType.NAME.ToLower())
                {
                    IS_CHECKED = true;
                }
                //else if (FOX_SOURCE_CATEGORY_ID != null && referralSource != null || referralSource2 != null && !string.IsNullOrEmpty(referralSource.DESCRIPTION) || !string.IsNullOrEmpty(referralSource2.NAME) && !string.IsNullOrEmpty(taskSubType.NAME)
                //  && referralSource.DESCRIPTION.ToLower() == taskSubType.NAME.ToLower() || referralSource2.NAME.ToLower() == taskSubType.NAME.ToLower())
                //{
                //    IS_CHECKED = true;
                //}
                else if (currentTaskType != null && !string.IsNullOrEmpty(taskSubType.NAME) && currentTaskType.RT_CODE.ToLower() == "block" && taskSubType.NAME.ToLower() == "hpb")
                {
                    IS_CHECKED = true;
                }
                else
                {
                    IS_CHECKED = false;
                }

                if (IS_CHECKED)
                {
                    InsertTaskTaskSubType(profile, tASK_ID, taskSubType.TASK_SUB_TYPE_ID);
                    //var taskTaskSubType = _TaskTaskSubTypeRepository.GetFirst(t =>
                    //                                                          t.TASK_ID == tASK_ID
                    //                                                          && t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID
                    //                                                          && t.PRACTICE_CODE == profile.PracticeCode);
                    //bool IsEdit = false;
                    //if (taskTaskSubType == null && IS_CHECKED)
                    //{
                    //    taskTaskSubType = new FOX_TBL_TASK_TASK_SUB_TYPE();
                    //    taskTaskSubType.TASK_TASK_SUB_TYPE_ID = Helper.getMaximumId("FOX_TBL_TASK_TASK_SUB_TYPE_ID");
                    //    taskTaskSubType.PRACTICE_CODE = profile.PracticeCode;
                    //    taskTaskSubType.TASK_ID = tASK_ID;
                    //    taskTaskSubType.TASK_SUB_TYPE_ID = taskSubType.TASK_SUB_TYPE_ID;
                    //    taskTaskSubType.CREATED_BY = taskTaskSubType.MODIFIED_BY = profile.UserName;
                    //    taskTaskSubType.CREATED_DATE = taskTaskSubType.MODIFIED_DATE = DateTime.Now;
                    //    IsEdit = false;
                    //}
                    //taskTaskSubType.DELETED = !IS_CHECKED;
                    //if (!IsEdit)
                    //{
                    //    _TaskTaskSubTypeRepository.Insert(taskTaskSubType);
                    //}
                    //_TaskTaskSubTypeRepository.Save();
                }
            }
        }

        private void AddTaskLogs(FOX_TBL_TASK dbtask, FOX_TBL_TASK task, UserProfile profile, OriginalQueue WORK_QUEUE)
        {
            List<TaskLog> taskLoglist = new List<TaskLog>();
            List<string> porta_logs = new List<string>();

            if (task.TASK_TYPE_ID != null)
            {
                var treatmentLocation = new FacilityLocation();
                var obj = new Index_infoReq();
                obj.WORK_ID = WORK_QUEUE.WORK_ID;
                var patientInterfaced = checkPatientisInterfaced(task.PATIENT_ACCOUNT, profile);
                var patient = GetTaskPatient(task.PATIENT_ACCOUNT, profile.PracticeCode);
                //var patient = _PatientRepository.GetFirst(x => x.Patient_Account == task.PATIENT_ACCOUNT && x.Practice_Code == profile.PracticeCode && x.DELETED == false);
                var sourceDetail = GetSourceDetail(profile.PracticeCode, WORK_QUEUE.WORK_ID);
                //var sourceDetail = _InsertSourceAddRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.WORK_ID == WORK_QUEUE.WORK_ID && WORK_QUEUE.WORK_ID != 0);
                var documentType = GetDocumentType(WORK_QUEUE);
                //var documentType = _foxdocumenttypeRepository.GetFirst(t => t.DOCUMENT_TYPE_ID == sourceDetail.DOCUMENT_TYPE);
                var ORS = new ReferralSource();
                if (sourceDetail != null)
                {
                    ORS = GetOrderingRefrralSource(sourceDetail.SENDER_ID ?? 0);
                }
                //var ORS = _InsertUpdateOrderingSourceRepository.GetFirst(t => t.SOURCE_ID == sourceDetail.SENDER_ID);
                var Indexer = new User();
                if (sourceDetail != null)
                {
                    Indexer = GetIndexer(sourceDetail.COMPLETED_BY, profile.PracticeCode);
                }
                //var Indexer = _User.GetFirst(T => T.USER_NAME == sourceDetail.COMPLETED_BY);
                var notes = GetNotes_History(obj, profile);
                var app_user = new User();
                var curr_insurances = GetPatientCurrentInsurance(task.PATIENT_ACCOUNT); //Current
                //var curr_insurances = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == task.PATIENT_ACCOUNT && (x.Deleted ?? false) == false && (string.IsNullOrEmpty(x.FOX_INSURANCE_STATUS) || x.FOX_INSURANCE_STATUS == "C")); //Current
                if (WORK_QUEUE != null && !String.IsNullOrWhiteSpace(WORK_QUEUE.SORCE_NAME))
                {
                    app_user = GetReferralUser(WORK_QUEUE.SORCE_TYPE, WORK_QUEUE.SORCE_NAME);
                    //if (WORK_QUEUE.SORCE_TYPE.ToLower() == "email")
                    //{                    
                    //    app_user = _User.GetFirst(t => t.EMAIL == WORK_QUEUE.SORCE_NAME && t.DELETED == false && t.IS_ACTIVE == true && t.IS_APPROVED == true);
                    //}
                    //else if (WORK_QUEUE.SORCE_TYPE.ToLower() == "fax")
                    //{
                    //    app_user = _User.GetFirst(t => (t.FAX == WORK_QUEUE.SORCE_NAME || t.FAX_2 == WORK_QUEUE.SORCE_NAME || t.FAX_3 == WORK_QUEUE.SORCE_NAME) && t.DELETED == false && t.IS_ACTIVE == true && t.IS_APPROVED == true);
                    //}
                }
                else
                {
                    app_user = null;
                }
                if (sourceDetail != null && sourceDetail.FACILITY_ID != null)
                {
                    treatmentLocation = GetReferralSourceFacilities(sourceDetail.FACILITY_ID, profile.PracticeCode);
                    //treatmentLocation = _FacilityLocationRepository.GetFirst(t => t.LOC_ID == sourceDetail.FACILITY_ID && t.DELETED == false && t.PRACTICE_CODE == profile.PracticeCode);
                }
                int count = notes.Count;
                var discipline = "";
                if (sourceDetail != null)
                {
                    if (!string.IsNullOrEmpty(sourceDetail?.DEPARTMENT_ID))
                    {
                        if (sourceDetail.DEPARTMENT_ID.Contains("1"))
                        {
                            discipline = discipline + " Occupational Therapy (OT), ";
                        }
                        if (sourceDetail.DEPARTMENT_ID.Contains("2"))
                        {
                            discipline = discipline + " Physical Therapy (PT), ";
                        }
                        if (sourceDetail.DEPARTMENT_ID.Contains("3"))
                        {
                            discipline = discipline + " Speech Therapy (ST), ";
                        }
                        if (sourceDetail.DEPARTMENT_ID == "4")
                        {
                            discipline = discipline + "Physical/Occupational/Speech Therapy(PT/OT/ST)";
                        }
                        if (sourceDetail.DEPARTMENT_ID == "5")
                        {
                            discipline = discipline + "Physical/Occupational Therapy(PT/OT)";
                        }
                        if (sourceDetail.DEPARTMENT_ID == "6")
                        {
                            discipline = discipline + "Physical/Speech Therapy(PT/ST)";
                        }
                        if (sourceDetail.DEPARTMENT_ID == "7")
                        {
                            discipline = discipline + "Occupational/Speech Therapy(OT/ST)";
                        }
                        if (sourceDetail.DEPARTMENT_ID.Contains("8"))
                        {
                            discipline = discipline + " Unknown, ";
                        }
                        if (sourceDetail.DEPARTMENT_ID.Contains("9"))
                        {
                            discipline = discipline + " Exercise Physiology (EP), ";
                        }
                    }
                    else
                    {
                        discipline = "none";
                    }
                    if (discipline.Substring(discipline.Length - 2) == ",")
                    {
                        discipline = discipline.TrimEnd(discipline[discipline.Length - 1]);
                    }
                    //switch (sourceDetail.DEPARTMENT_ID)
                    //{
                    //    case "1":
                    //        {
                    //            discipline = "Occupational Therapy(OT)";
                    //            break;
                    //        }
                    //    case "2":
                    //        {
                    //            discipline = "Physical Therapy (PT)";
                    //            break;
                    //        }
                    //    case "3":
                    //        {
                    //            discipline = "Speech Therapy (ST)";
                    //            break;
                    //        }
                    //    case "1,2,3":
                    //        {
                    //            discipline = "Physical/Occupational/Speech Therapy(PT/OT/ST)";
                    //            break;
                    //        }
                    //    case "1,2":
                    //        {
                    //            discipline = "Physical/Occupational Therapy(PT/OT)";
                    //            break;
                    //        }
                    //    case "2,3":
                    //        {
                    //            discipline = "Physical/Speech Therapy(PT/ST)";
                    //            break;
                    //        }
                    //    case "1,3":
                    //        {
                    //            discipline = "Occupational/Speech Therapy(OT/ST)";
                    //            break;
                    //        }
                    //    case "8":
                    //        {
                    //            discipline = "Unknown";
                    //            break;
                    //        }
                    //    default:
                    //        {
                    //            discipline = "none";
                    //            break;
                    //        }
                    //}


                    //taskLoglist.Add(new TaskLog() { ACTION = "Completed Date & Time :", ACTION_DETAIL = "Completed Date & Time :" + sourceDetail.COMPLETED_DATE });
                    //taskLoglist.Add(new TaskLog() { ACTION = "Username :", ACTION_DETAIL = "Username : " + Indexer.LAST_NAME + ", " + Indexer.FIRST_NAME });
                    //taskLoglist.Add(new TaskLog() { ACTION = "indexer :", ACTION_DETAIL = "Indexer >" });

                    porta_logs.Add("Completed Date & Time :" + sourceDetail.COMPLETED_DATE);
                    if (Indexer != null)
                    {
                        string str = "";
                        if (String.IsNullOrEmpty(Indexer.FIRST_NAME) && String.IsNullOrEmpty(Indexer.FIRST_NAME))
                        {
                            str = "Username : ";
                        }
                        else if (!String.IsNullOrEmpty(Indexer.FIRST_NAME) && !String.IsNullOrEmpty(Indexer.FIRST_NAME))
                        {
                            str = "Username : " + Indexer.LAST_NAME + "," + Indexer.FIRST_NAME;
                        }
                        else if (String.IsNullOrEmpty(Indexer.LAST_NAME))
                        {
                            str = "Username :" + Indexer.FIRST_NAME;
                        }
                        else if (String.IsNullOrEmpty(Indexer.FIRST_NAME))
                        {
                            str = "Username :" + Indexer.LAST_NAME;
                        }
                        porta_logs.Add(str);
                    }
                    porta_logs.Add("Indexer >");


                    PatientInsurance medicare_insurance = new PatientInsurance();
                    var medicare_financial_class = GetFinancialClass("mc", profile.PracticeCode);
                    //var medicare_financial_class = _financialClassRepository.GetFirst(t => t.CODE.ToLower() == "mc" && t.DELETED == false && t.PRACTICE_CODE == profile.PracticeCode);
                    bool Procedure_information = false;

                    if (WORK_QUEUE.IS_VERBAL_ORDER != null && WORK_QUEUE.IS_VERBAL_ORDER == true)
                    {
                        if (WORK_QUEUE.VO_DATE_TIME != null)
                        {
                            //taskLoglist.Add(new TaskLog() { ACTION = "Date & Time :", ACTION_DETAIL = "Date & Time :" + WORK_QUEUE.VO_DATE_TIME });
                            porta_logs.Add("Date & Time :" + WORK_QUEUE.VO_DATE_TIME);

                        }

                        if (!String.IsNullOrWhiteSpace(WORK_QUEUE.VO_RECIEVED_BY))
                        {
                            //taskLoglist.Add(new TaskLog() { ACTION = "Received By :", ACTION_DETAIL = "Received By :" + WORK_QUEUE.VO_RECIEVED_BY });
                            porta_logs.Add("Received By :" + WORK_QUEUE.VO_RECIEVED_BY);

                        }
                        if (WORK_QUEUE.VO_ON_BEHALF_OF != null)
                        {


                            var on_behalf_of = GetOrderingRefrralSource(WORK_QUEUE.VO_ON_BEHALF_OF);
                            //var on_behalf_of = _InsertUpdateOrderingSourceRepository.GetFirst(t => t.DELETED == false && t.SOURCE_ID == WORK_QUEUE.VO_ON_BEHALF_OF);
                            if (on_behalf_of != null)
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "On Behalf of : ", ACTION_DETAIL = "On Behalf of : " + on_behalf_of.FIRST_NAME ?? ""  + " " + on_behalf_of.LAST_NAME ?? "" + " - " + on_behalf_of.REFERRAL_REGION ?? "" });
                                porta_logs.Add("On Behalf of : " + on_behalf_of.FIRST_NAME ?? "" + " " + on_behalf_of.LAST_NAME ?? "" + " - " + on_behalf_of.REFERRAL_REGION ?? "");

                            }
                            Procedure_information = true;
                        }
                        //taskLoglist.Add(new TaskLog() { ACTION = "Verbal Order :", ACTION_DETAIL = "Verbal Order: Yes" });
                        porta_logs.Add("Verbal Order: Yes");


                        Procedure_information = true;
                    }
                    else
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Verbal Order :", ACTION_DETAIL = "Verbal Order: No" });
                        porta_logs.Add("Verbal Order: No");

                    }

                    if (WORK_QUEUE.IS_EVALUATE_TREAT != null && WORK_QUEUE.IS_EVALUATE_TREAT == true)
                    {

                        if (!String.IsNullOrWhiteSpace(WORK_QUEUE.HEALTH_NUMBER))
                        {
                            //taskLoglist.Add(new TaskLog() { ACTION = "Contact Number :", ACTION_DETAIL = "Contact Number :" + WORK_QUEUE.HEALTH_NUMBER });
                            porta_logs.Add("Contact Number :" + WORK_QUEUE.HEALTH_NUMBER);
                        }
                        if (!String.IsNullOrWhiteSpace(WORK_QUEUE.HEALTH_NAME))
                        {
                            //taskLoglist.Add(new TaskLog() { ACTION = "Home Health Name :", ACTION_DETAIL = "Home Health Name :" + WORK_QUEUE.HEALTH_NAME });
                            porta_logs.Add("Home Health Name :" + WORK_QUEUE.HEALTH_NAME);
                        }
                        Procedure_information = true;
                    }
                    if (WORK_QUEUE.IS_POST_ACUTE != null && WORK_QUEUE.IS_POST_ACUTE == true)
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Post-acute Referral", ACTION_DETAIL = "Post-acute Referral: Yes" });
                        porta_logs.Add("Post-acute Referral: Yes");
                        Procedure_information = true;
                    }
                    else
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Post-acute Referral", ACTION_DETAIL = "Post-acute Referral: No" });
                        porta_logs.Add("Post-acute Referral: No");
                    }
                    if (!String.IsNullOrWhiteSpace(WORK_QUEUE.REASON_FOR_THE_URGENCY))
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Reason for the Urgency", ACTION_DETAIL = "Reason for the Urgency : " + WORK_QUEUE.REASON_FOR_THE_URGENCY });
                        porta_logs.Add("Reason for the Urgency : " + WORK_QUEUE.REASON_FOR_THE_URGENCY);
                        Procedure_information = true;
                    }
                    if (WORK_QUEUE.IS_EMERGENCY_ORDER == true)
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Urgent Referral", ACTION_DETAIL = "Urgent Referral: Yes" });
                        porta_logs.Add("Urgent Referral: Yes");
                        Procedure_information = true;
                    }
                    else
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Urgent Referral", ACTION_DETAIL = "Urgent Referral: No" });
                        porta_logs.Add("Urgent Referral: No");

                    }

                    var speciality_program = GetROProcedure(WORK_QUEUE.WORK_ID);
                    //var speciality_program = _InsertProceuresRepository.GetFirst(t=> t.DELETED == false && t.WORK_ID == WORK_QUEUE.WORK_ID);

                    if (speciality_program != null && speciality_program.SPECIALITY_PROGRAM != null && speciality_program.SPECIALITY_PROGRAM != "0")
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Speciality Program :", ACTION_DETAIL = "Speciality Program : " + speciality_program.SPECIALITY_PROGRAM });
                        porta_logs.Add("Speciality Program : " + speciality_program.SPECIALITY_PROGRAM);
                        Procedure_information = true;
                    }
                    if (Procedure_information)
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Procedure Information", ACTION_DETAIL = "Procedure Information >" });
                        porta_logs.Add("Procedure Information > ");
                    }

                    if (medicare_financial_class != null && curr_insurances != null && curr_insurances.Count > 0)
                    {
                        medicare_insurance = curr_insurances.Find(t => t.FINANCIAL_CLASS_ID == medicare_financial_class.FINANCIAL_CLASS_ID);
                    }
                    FOX_TBL_ELIG_HTML eligibility = new FOX_TBL_ELIG_HTML();
                    if (medicare_insurance != null)
                    {
                        eligibility = GetPatienEligibilityDetail(profile.PracticeCode, medicare_insurance.Patient_Insurance_Id, task.PATIENT_ACCOUNT);
                        //eligibility = _eligHtmlRepository.GetFirst(t => t.PRACTICE_CODE == profile.PracticeCode && t.DELETED == false && t.PATIENT_INSURANCE_ID == medicare_insurance.Patient_Insurance_Id && t.PATIENT_ACCOUNT == task.PATIENT_ACCOUNT);
                    }
                    if (medicare_insurance != null && eligibility != null && eligibility.ELIG_HTML_ID != 0)
                    {
                        if (medicare_insurance.HOSPICE_LIMIT_ID != null)
                        {
                            var medicare_limit_hospice = GetMedicareLimit(medicare_insurance.HOSPICE_LIMIT_ID, task.PATIENT_ACCOUNT, profile.PracticeCode);
                            //var medicare_limit_hospice = _MedicareLimitRepository.GetFirst(t => t.MEDICARE_LIMIT_ID == medicare_insurance.HOSPICE_LIMIT_ID && t.Patient_Account == task.PATIENT_ACCOUNT  && t.DELETED == false && t.PRACTICE_CODE == profile.PracticeCode);
                            if (medicare_limit_hospice != null)
                            {
                                var current_date = Helper.GetCurrentDate();
                                if (
                                    (
                                        (medicare_limit_hospice.EFFECTIVE_DATE != null && medicare_limit_hospice.EFFECTIVE_DATE <= current_date)
                                        &&
                                        (medicare_limit_hospice.END_DATE != null && medicare_limit_hospice.END_DATE >= current_date)
                                    )

                                    ||
                                     (medicare_limit_hospice.END_DATE == null && medicare_limit_hospice.EFFECTIVE_DATE != null
                                     && medicare_limit_hospice.EFFECTIVE_DATE <= current_date
                                     )
                                )
                                {
                                    string hospice_string = "";
                                    if (medicare_limit_hospice.NPI != null)
                                    {
                                        hospice_string = " - NPI: " + medicare_limit_hospice.NPI;
                                    }

                                    if (medicare_limit_hospice.EFFECTIVE_DATE != null)
                                    {
                                        hospice_string = hospice_string + " - Effective Date: " + Convert.ToDateTime(medicare_limit_hospice.EFFECTIVE_DATE).ToShortDateString();
                                    }
                                    if (medicare_limit_hospice.END_DATE != null)
                                    {
                                        hospice_string = hospice_string + " - End Date: " + Convert.ToDateTime(medicare_limit_hospice.END_DATE).ToShortDateString();
                                    }

                                    //taskLoglist.Add(new TaskLog() { ACTION = "Medicare Limit :", ACTION_DETAIL = "Active Hospice: " + hospice_string });
                                    porta_logs.Add("Active Hospice: " + hospice_string);
                                }


                            }
                        }

                        if (medicare_insurance.HOME_HEALTH_LIMIT_ID != null)
                        {
                            var medicare_limit_hhh = GetMedicareLimit(medicare_insurance.HOME_HEALTH_LIMIT_ID, task.PATIENT_ACCOUNT, profile.PracticeCode);
                            //var medicare_limit_hhh = _MedicareLimitRepository.GetFirst(t => t.MEDICARE_LIMIT_ID == medicare_insurance.HOME_HEALTH_LIMIT_ID && t.Patient_Account == task.PATIENT_ACCOUNT  && t.DELETED == false && t.PRACTICE_CODE == profile.PracticeCode);

                            if (medicare_limit_hhh != null)
                            {
                                var current_date = Helper.GetCurrentDate();
                                if (
                                    (
                                        (medicare_limit_hhh.EFFECTIVE_DATE != null && medicare_limit_hhh.EFFECTIVE_DATE <= current_date)
                                        &&
                                        (medicare_limit_hhh.END_DATE != null && medicare_limit_hhh.END_DATE >= current_date)
                                    )

                                    ||
                                     (medicare_limit_hhh.END_DATE == null && medicare_limit_hhh.EFFECTIVE_DATE != null
                                     && medicare_limit_hhh.EFFECTIVE_DATE >= current_date
                                     )
                                )
                                {
                                    string hhe_string = "";

                                    if (medicare_limit_hhh.NPI != null)
                                    {
                                        hhe_string = " - NPI: " + medicare_limit_hhh.NPI;
                                    }

                                    if (medicare_limit_hhh.EFFECTIVE_DATE != null)
                                    {
                                        hhe_string = hhe_string + " - Effective Date: " + Convert.ToDateTime(medicare_limit_hhh.EFFECTIVE_DATE).ToShortDateString();
                                    }
                                    if (medicare_limit_hhh.END_DATE != null)
                                    {
                                        hhe_string = hhe_string + " - End Date: " + Convert.ToDateTime(medicare_limit_hhh.END_DATE).ToShortDateString();
                                    }

                                    //taskLoglist.Add(new TaskLog() { ACTION = "Medicare Limit :", ACTION_DETAIL = "Active Home Health Episode: " + hhe_string });
                                    porta_logs.Add("Active Home Health Episode: " + hhe_string);
                                }
                            }
                        }
                    }

                    foreach (var note in notes)
                    {

                        //taskLoglist.Add(new TaskLog() { ACTION = "Notes: ", ACTION_DETAIL = "Note #" + count + ") " + note.NOTE_DESC });
                        porta_logs.Add("Note #" + count + ") " + note.NOTE_DESC);
                        count--;
                    }
                    if (notes.Count != 0)
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Additional Information: ", ACTION_DETAIL = "Additional Information ->" });
                        porta_logs.Add("Additional Information -> ");
                    }
                    //taskLoglist.Add(new TaskLog() { ACTION = "Discipline : ", ACTION_DETAIL = "Discipline : " + discipline });
                    porta_logs.Add("Discipline : " + discipline);
                    if (treatmentLocation != null && treatmentLocation.REGION != null && treatmentLocation.REGION != "")
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Treatment Region: ", ACTION_DETAIL = "Treatment Region: " + treatmentLocation.REGION });
                        porta_logs.Add("Treatment Region: " + treatmentLocation.REGION);
                    }
                    else if (treatmentLocation != null && treatmentLocation.NAME != null && treatmentLocation.NAME.Contains("Private Home"))
                    {
                        var patient_pos = GetPatientPOS(sourceDetail.FACILITY_ID, task.PATIENT_ACCOUNT);
                        //var patient_pos = _PatientPOSLocationRepository.GetFirst(t => t.Is_Default == true && t.Loc_ID == sourceDetail.FACILITY_ID && t.Deleted == false && t.Patient_Account == task.PATIENT_ACCOUNT);
                        if (patient_pos == null)
                        {
                            patient_pos = _PatientPOSLocationRepository.GetMany(t => t.Loc_ID == sourceDetail.FACILITY_ID && t.Deleted == false && t.Patient_Account == task.PATIENT_ACCOUNT).OrderByDescending(t => t.Created_Date)?.FirstOrDefault();
                        }
                        if (patient_pos != null)
                        {
                            var address_Private_Home = _PatientAddressRepository.GetFirst(t => t.PATIENT_ACCOUNT == task.PATIENT_ACCOUNT && patient_pos.Patient_POS_ID == t.PATIENT_POS_ID && !(t.DELETED ?? false));
                            if (address_Private_Home != null && !String.IsNullOrWhiteSpace(address_Private_Home.POS_REGION))
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "Treatment Region: ", ACTION_DETAIL = "Treatment Region: " + address_Private_Home.POS_REGION });
                                porta_logs.Add("Treatment Region: " + address_Private_Home.POS_REGION);
                            }
                        }
                    }
                    if (sourceDetail.FACILITY_NAME != null)
                    {
                        //taskLoglist.Add(new TaskLog() { ACTION = "Treatment location: ", ACTION_DETAIL = "Treatment location: " + sourceDetail.FACILITY_NAME });
                        porta_logs.Add("Treatment location: " + sourceDetail.FACILITY_NAME);
                    }
                    if (app_user != null)
                    {
                        if (app_user.HHH != null)
                        {
                            var HHH = GetCaseIdentifier(app_user.HHH);
                            //var HHH = _fox_tbl_identifier.GetFirst(t => t.IDENTIFIER_ID == app_user.HHH);
                            if (HHH != null && !String.IsNullOrWhiteSpace(HHH.NAME))
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "HH&H: ", ACTION_DETAIL = "HH&H: " + HHH.NAME });
                                porta_logs.Add("HH&H: " + HHH.NAME);
                            }
                        }
                        if (app_user.HOSPITAL != null)
                        {
                            var hospital = GetCaseIdentifier(app_user.HOSPITAL);
                            //var hospital = _fox_tbl_identifier.GetFirst(t => t.IDENTIFIER_ID == app_user.HOSPITAL);
                            if (hospital != null && !String.IsNullOrWhiteSpace(hospital.NAME))
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "Hospital: ", ACTION_DETAIL = "Hospital: " + hospital.NAME });
                                porta_logs.Add("Hospital: " + hospital.NAME);
                            }
                        }
                        if (app_user.SNF != null)
                        {
                            var SNF = GetCaseIdentifier(app_user.SNF);
                            //var SNF = _fox_tbl_identifier.GetFirst(t => t.IDENTIFIER_ID == app_user.SNF);
                            if (SNF != null && !String.IsNullOrWhiteSpace(SNF.NAME))
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "SNF: ", ACTION_DETAIL = "SNF: " + SNF.NAME });
                                porta_logs.Add("SNF: " + SNF.NAME);
                            }
                        }
                        if (app_user.SPECIALITY != null)
                        {
                            var speciality = GetSpeciality(app_user.SPECIALITY);
                            //var speciality = _speciality.GetFirst(t => t.SPECIALITY_ID == app_user.SPECIALITY);
                            if (speciality != null && !String.IsNullOrWhiteSpace(speciality.NAME))
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "Practice Speciality: ", ACTION_DETAIL = "Practice Speciality: " + speciality.NAME });
                                porta_logs.Add("Practice Speciality: " + speciality.NAME);
                            }
                        }
                        if (app_user.ACO != null)
                        {
                            var ACO = GetCaseIdentifier(app_user.ACO);
                            //var ACO = _fox_tbl_identifier.GetFirst(t => t.IDENTIFIER_ID == app_user.ACO);
                            if (ACO != null && !String.IsNullOrWhiteSpace(ACO.NAME))
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "ACO: ", ACTION_DETAIL = "ACO: " + ACO.NAME });
                                porta_logs.Add("ACO: " + ACO.NAME);

                            }
                        }
                        if (app_user.PRACTICE_ORGANIZATION_ID != null)
                        {
                            var practice_organization = GetPracticeOrganization(app_user.PRACTICE_ORGANIZATION_ID);
                            //var practice_organization = _fox_tbl_practice_organization.GetFirst(t => t.PRACTICE_ORGANIZATION_ID == app_user.PRACTICE_ORGANIZATION_ID);
                            if (practice_organization != null && !String.IsNullOrWhiteSpace(practice_organization.NAME))
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "Practice/Organization Name: ", ACTION_DETAIL = "Practice/Organization Name: " + practice_organization.NAME });
                                porta_logs.Add("Practice/Organization Name: " + practice_organization.NAME);

                            }
                        }
                    }
                    //taskLoglist.Add(new TaskLog() { ACTION = "Ordering Referral Source : ", ACTION_DETAIL = "Ordering Referral Source: " + ORS.CODE + " - " + ORS.LAST_NAME + ", " + ORS.FIRST_NAME + " - " + ORS.REFERRAL_REGION + " - NPI: " + ORS.NPI });
                    //taskLoglist.Add(new TaskLog() { ACTION = "Document Type : ", ACTION_DETAIL = "Document Type : " + documentType.NAME });
                    //taskLoglist.Add(new TaskLog() { ACTION = "Sender Email/FAX : ", ACTION_DETAIL = "Sender Email/FAX : " + sourceDetail.SORCE_NAME });
                    //taskLoglist.Add(new TaskLog() { ACTION = "Source Information", ACTION_DETAIL = "Source Information >" });
                    porta_logs.Add("Ordering Referral Source: " + ORS.CODE + " - " + ORS.LAST_NAME + ", " + ORS.FIRST_NAME + " - " + ORS.REFERRAL_REGION + " - NPI: " + ORS.NPI);
                    porta_logs.Add("Document Type : " + documentType.NAME);
                    porta_logs.Add("Sender Email/FAX : " + sourceDetail.SORCE_NAME);
                    porta_logs.Add("Source Information >");
                }
                if (patient != null)
                {
                    var taskTypes = GetTaskTypes(profile.PracticeCode);
                    //var taskTypes = _taskTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);

                    if (taskTypes != null && taskTypes.Count > 0)
                    {
                        var taskTypeIdPorta = taskTypes.Find(t => t.RT_CODE?.ToLower() == "porta" && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED)?.TASK_TYPE_ID ?? 0;
                        var taskTypeIdHBR = taskTypes.Find(t => t.RT_CODE?.ToLower() == "block" && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED)?.TASK_TYPE_ID ?? 0;
                        var taskTypeIdStrategic = taskTypes.Find(t => t.NAME.ToLower() == "strategic accounts" && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED)?.TASK_TYPE_ID ?? 0;
                        if (task.TASK_TYPE_ID == taskTypeIdHBR)
                        {

                            if (curr_insurances != null)
                            {
                                curr_insurances.Where(t => t.Pri_Sec_Oth_Type == "P" || t.Pri_Sec_Oth_Type == "S");
                                foreach (var ins in curr_insurances)
                                {
                                    var ins_name = GetInsurance(ins.FOX_TBL_INSURANCE_ID);
                                    //var ins_name = _foxInsurancePayersRepository.GetFirst(t => t.DELETED == false && t.FOX_TBL_INSURANCE_ID == ins.FOX_TBL_INSURANCE_ID).INSURANCE_NAME ?? "";
                                    if (ins.Pri_Sec_Oth_Type == "S" && ins_name != "")
                                    {
                                        //taskLoglist.Add(new TaskLog() { ACTION = "Secondary Insurance ", ACTION_DETAIL = "Secondary Insurance: " + ' ' + ins_name });
                                        porta_logs.Add("Secondary Insurance: " + ' ' + ins_name);
                                    }
                                }
                                foreach (var ins in curr_insurances)
                                {
                                    var ins_name = GetInsurance(ins.FOX_TBL_INSURANCE_ID);
                                    //var ins_name = _foxInsurancePayersRepository.GetFirst(t => t.DELETED == false && t.FOX_TBL_INSURANCE_ID == ins.FOX_TBL_INSURANCE_ID).INSURANCE_NAME ?? "";
                                    if (ins.Pri_Sec_Oth_Type == "P" && ins_name != "")
                                    {
                                        //taskLoglist.Add(new TaskLog() { ACTION = "Primary Insurance ", ACTION_DETAIL = "Primary Insurance: " + ' ' + ins_name });
                                        porta_logs.Add("Primary Insurance: " + ' ' + ins_name);
                                    }
                                }
                            }
                            var amount = getPendingHighBalance(task.PATIENT_ACCOUNT);
                            //taskLoglist.Add(new TaskLog() { ACTION = "Due Amount ", ACTION_DETAIL = "Due Amount: " + " $ " + Math.Round(Convert.ToDecimal(amount.Patient_Balance), 2) });
                            //porta_logs.Add("Due Amount: " + " $ " + Math.Round(Convert.ToDecimal(amount.Patient_Balance), 2) );
                            porta_logs.Add("Due Amount: " + " $ " + Math.Round(Convert.ToDecimal(amount.Statement_Patient_Balance), 2));

                        }

                        if (task.TASK_TYPE_ID == taskTypeIdPorta || task.TASK_TYPE_ID == taskTypeIdStrategic)
                        {
                            if (patientInterfaced == true)
                            {
                                // taskLoglist.Add(new TaskLog() { ACTION = "Interface Status ", ACTION_DETAIL = "Interface Status: Existing Patient" });
                                porta_logs.Add("Interface Status: Existing Patient");
                            }
                            else
                            {
                                //taskLoglist.Add(new TaskLog() { ACTION = "Interface Status ", ACTION_DETAIL = "Interface Status: New Patient" });
                                porta_logs.Add("Interface Status: New Patient");
                            }
                        }
                    }


                    //taskLoglist.Add(new TaskLog() { ACTION = "Name ", ACTION_DETAIL = " Name : " + patient.First_Name + ' ' + patient.Last_Name });
                    porta_logs.Add(" Name : " + patient.First_Name + ' ' + patient.Last_Name);
                    //taskLoglist.Add(new TaskLog() { ACTION = "+ Patient Information", ACTION_DETAIL = "Patient Information >" });
                    porta_logs.Add(" <br>Patient Information >");
                }
            }
            porta_logs.Reverse();
            StringBuilder portalog = new StringBuilder();

            foreach (string str in porta_logs)
            {
                portalog.Append(str + "<br>");
            }
            taskLoglist.Add(new TaskLog()
            {
                ACTION = "Porta Logs",
                ACTION_DETAIL = portalog.ToString()
            }
                        );

            if (taskLoglist.Count() > 0)
            {
                //foreach (var taskLog in taskLoglist)
                //{
                InsertTaskLog(dbtask.TASK_ID, taskLoglist, profile);
                //}
            }

            //    taskLoglist.ForEach(row =>
            //    {
            //        row.TASK_LOG_ID = Helper.getMaximumId("FOX_TASK_LOG_ID");
            //        row.PRACTICE_CODE = profile.PracticeCode;
            //        row.TASK_ID = dbtask.TASK_ID;
            //        row.CREATED_BY = row.MODIFIED_BY = profile.UserName;
            //        row.CREATED_DATE = row.MODIFIED_DATE = Helper.GetCurrentDate();
            //    });
            //}

            //try
            //{
            //    foreach (var taskLog in taskLoglist)
            //    {
            //        _taskLogRepository.Insert(taskLog);
            //        _taskLogRepository.Save();
            //    }
            //}
            //catch (Exception exception)
            //{
            //    throw exception;
            //}
            //_TaskContext.SaveChanges();
        }
        public pendingBalanceAmount getPendingHighBalance(long? PATIENT_ACCOUNT)
        {
            try
            {
                var _patientAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = PATIENT_ACCOUNT };
                //if (PATIENT_ACCOUNT == null)
                //{
                //    _patientAccount.Value = DBNull.Value;
                //}
                var result = SpRepository<pendingBalanceAmount>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_PENDING_BALANCE @PATIENT_ACCOUNT", _patientAccount);
                if (result == null)
                {
                    return new pendingBalanceAmount();
                }
                return result;
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public AttachmentData GeneratePdfforSignatureUpdate(string unique_Id)
        {
            try
            {
                var queue = _QueueRepository.GetFirst(e => e.UNIQUE_ID == unique_Id);
                if (queue != null)
                {
                    string file_Name = queue.UNIQUE_ID + " __" + DateTime.Now.Ticks + ".pdf";
                    string folder = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.ExportedFilesPath);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var localPath = "FoxDocumentDirectory" + "/" + file_Name;
                    var pathForPDF = Path.Combine(folder, file_Name);
                    ImageHandler imgHandler = new ImageHandler();
                    var imges = _OriginalQueueFiles.GetMany(x => x.UNIQUE_ID == unique_Id);
                    if (imges != null && imges.Count > 0)
                    {
                        var imgPaths = (from x in imges select x.FILE_PATH1).ToArray();
                        imgHandler.ImagesToPdf(imgPaths, pathForPDF);
                        AttachmentData attachmentData = new AttachmentData();
                        attachmentData.FILE_PATH = folder;
                        attachmentData.FILE_NAME = file_Name;
                        return attachmentData;
                    }
                }
                return new AttachmentData();
            }
            catch (Exception)
            {
                return new AttachmentData();
            }
        }

        private int getNumberOfPagesOfPDF(string PdfPath)
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(PdfPath);
            return pdfReader.NumberOfPages;
        }

        private ResponseHTMLToPDF HTMLToPDF(ServiceConfiguration config, string htmlString, string fileName, string linkMessage = null)
        {
            try
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlString);
                //htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'print-footer')]").Remove();


                if (!string.IsNullOrWhiteSpace(linkMessage))
                {
                    var htmlNode_link = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'link')]");
                    if (htmlNode_link != null)
                    {
                        htmlNode_link.InnerHtml = linkMessage;
                    }
                }

                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginBottom = 10;
                converter.Options.MarginTop = 30;
                converter.Options.MarginLeft = 20;
                converter.Options.MarginRight = 0;
                converter.Options.DisplayHeader = false;
                converter.Options.WebPageWidth = 768;

                //PdfTextSection text = new PdfTextSection(10, 10, "Please sign and return to FOX at +1 (800) 597 - 0848 or email admit@foxrehab.org",
                //    new Font("Arial", 10));

                //footer settings
                //converter.Options.DisplayFooter = true;
                //converter.Footer.Height = 50;
                //converter.Footer.Add(text);

                PdfDocument doc = converter.ConvertHtmlString(htmlDoc.DocumentNode.OuterHtml);

                //string pdfPath = HttpContext.Current.Server.MapPath("~/" + @"FoxDocumentDirectory\RequestForOrderPDF\");
                string pdfPath = config.ORIGINAL_FILES_PATH_SERVER;

                if (!Directory.Exists(pdfPath))
                {
                    Directory.CreateDirectory(pdfPath);
                }
                fileName += DateTime.Now.Ticks + ".pdf";
                string pdfFilePath = pdfPath + fileName;


                // save pdf document
                doc.Save(pdfFilePath);

                // close pdf document
                doc.Close();
                return new ResponseHTMLToPDF() { FileName = fileName, FilePath = pdfPath, Success = true, ErrorMessage = "" };
            }
            catch (Exception exception)
            {
                return new ResponseHTMLToPDF() { FileName = "", FilePath = "", Success = true, ErrorMessage = exception.ToString() };
            }

        }
        public List<PreviousEmailInfo> getPreviousEmailInformation(string WORK_ID, UserProfile profile)
        {
            var work_id = long.Parse(WORK_ID);
            var _workId = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = work_id };
            var _practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var previous_emails = SpRepository<PreviousEmailInfo>.GetListWithStoreProcedure(@"exec FOX_TBL_GET_PREVIOUS_EMAILS 
                               @WORK_ID,@PRACTICE_CODE", _workId, _practiceCode);
            return previous_emails;

        }

        public string setPatientOpenedBy(long patientAccount, UserProfile profile)
        {
            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter user_Name = new SqlParameter("USER_NAME", profile.UserName);
            SqlParameter patient_Account = new SqlParameter("PATIENT_ACCOUNT", patientAccount);
            //SqlParameter open_By = new SqlParameter();
            //open_By.ParameterName = "OPEN_BY";
            //open_By.SqlDbType = SqlDbType.VarChar;
            //open_By.Direction = ParameterDirection.Output;
            //open_By.Size = 150;
            //SpRepository<string>.GetSingleObjectWithStoreProcedure(string.Format( "exec FOX_PROC_SET_PATIENT_OPENBY {0}, {1}, {2}, {3}", patient_Account, user_Name, practice_Code, open_By));
            string temp = SpRepository<string>.GetListWithStoreProcedure(@" FOX_PROC_SET_PATIENT_OPENBY @PATIENT_ACCOUNT, @USER_NAME, @PRACTICE_CODE", patient_Account, user_Name, practice_Code).SingleOrDefault();
            return string.IsNullOrEmpty(temp) ? null : temp;



            //var patient = _FoxTblPatientRepository.GetFirst(t => t.Patient_Account == patientAccount && t.DELETED == false);
            //if (patient != null && String.IsNullOrWhiteSpace(patient.Is_Opened_By))
            //{
            //    patient.Is_Opened_By = profile.UserName;
            //    _FoxTblPatientRepository.Update(patient);
            //    _FoxTblPatientRepository.Save();
            //    return null;

            //}
            //else if(patient != null && !String.IsNullOrWhiteSpace(patient.Is_Opened_By))
            //{
            //    if(patient.Is_Opened_By == profile.UserName)
            //    {
            //        return null;
            //    }
            //    else
            //    {
            //        var user = _User.GetFirst(t=> t.USER_NAME == patient.Is_Opened_By && t.DELETED == false && t.PRACTICE_CODE == profile.PracticeCode);
            //        if(user != null)
            //        {
            //            return user.LAST_NAME + ", " + user.FIRST_NAME;
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //    }
            //}
            //return null;

        }

        public void ClearPatientOpenedBy(long patientAccount, UserProfile profile)
        {
            var patient = _FoxTblPatientRepository.GetFirst(t => t.Patient_Account == patientAccount && t.DELETED == false);
            if (patient != null)
            {
                patient.Is_Opened_By = null;
                _FoxTblPatientRepository.Update(patient);
                _FoxTblPatientRepository.Save();
            }


        }

        public bool updateWorkOrderSignature(SubmitSignatureImageWithData obj, UserProfile profile)
        {
            if (obj._isSignaturePresent)
            {
                if (File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + profile.SIGNATURE_PATH)))
                {
                    obj.base64textString = "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/" + profile.SIGNATURE_PATH)));
                }
            }
            var work_id = long.Parse(obj.workId);
            var documentType = "";
            var work_order = _QueueRepository.GetFirst(t => t.WORK_ID == work_id && t.DELETED == false);
            var practiceCode = work_order.PRACTICE_CODE.Value.ToString();
            OriginalQueueFiles originalQueueFiles = _OriginalQueueFiles.GetFirst(t => t.UNIQUE_ID == work_order.UNIQUE_ID && !t.deleted);

            var patient = _PatientRepository.GetFirst(x => x.Patient_Account == work_order.PATIENT_ACCOUNT && x.Practice_Code == work_order.PRACTICE_CODE && x.DELETED == false);
            var sourceDetail = _InsertSourceAddRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == work_order.PRACTICE_CODE && t.WORK_ID == work_order.WORK_ID && work_order.WORK_ID != 0);
            if (obj._approval)
            {
                documentType = _foxdocumenttypeRepository.GetFirst(t => t.NAME == "Signed Order").NAME ?? "";
            }
            else
            {
                documentType = _foxdocumenttypeRepository.GetFirst(t => t.NAME == "Unsigned Order").NAME ?? "";
            }
            var ORS = _InsertUpdateOrderingSourceRepository.GetFirst(t => t.SOURCE_ID == sourceDetail.SENDER_ID);
            var address = _PatientAddressRepository.GetMany(t => t.PATIENT_ACCOUNT == work_order.PATIENT_ACCOUNT && t.ADDRESS_TYPE == "Home Address" && !(t.DELETED ?? false)).OrderByDescending(t => t.MODIFIED_DATE).FirstOrDefault();
            var diagnosis_string = "";
            var diagnosis = _InsertDiagnosisRepository.GetMany(t => t.DELETED == false && t.WORK_ID == work_id);
            var specialty = _InsertProceuresRepository.GetFirst(t => t.DELETED == false && t.WORK_ID == work_id);
            var referralSender = _referralSenderTypeRepository.GetMany(t => t.DELETED == false);

            if (referralSender != null && referralSender.Any())
            {
                foreach (var item in referralSender)
                {
                    if (!string.IsNullOrWhiteSpace(work_order.SORCE_NAME) && !string.IsNullOrWhiteSpace(work_order.SORCE_TYPE)
                        && ((work_order.SORCE_TYPE.ToLower() == "fax" && work_order.SORCE_NAME == item.SENDER) || (work_order.SORCE_TYPE.ToLower() == "email" && work_order.SORCE_NAME == item.SENDER)))
                    {
                        work_order.is_strategic_account = true;
                    }
                }
            }

            var fClass = new FinancialClass();

            if (work_order != null && work_order.PATIENT_ACCOUNT != null && work_order.PATIENT_ACCOUNT != 0)
            {
                var pat = _FoxTblPatientRepository.GetFirst(x => x.Patient_Account == work_order.PATIENT_ACCOUNT);
                if (pat != null && pat.FINANCIAL_CLASS_ID != null)
                {
                    fClass = _financialClassRepository.GetFirst(x => x.FINANCIAL_CLASS_ID == pat.FINANCIAL_CLASS_ID);
                    if (fClass != null && !string.IsNullOrEmpty(fClass.NAME) && fClass.NAME.ToLower().Equals("sa- special account"))
                    {
                        work_order.is_strategic_account = true;
                    }
                }
            }

            if (diagnosis != null && diagnosis.Count > 0)
            {
                foreach (var item in diagnosis)
                {
                    diagnosis_string = diagnosis_string + item.DIAG_CODE + " " + item.DIAG_DESC + " <br />";
                }
            }
            //var speciality = _speciality.GetFirst(t=> t.SPECIALITY_ID == work_order.SPECIALITY_PROGRAM);
            var pri_insurance = "";
            var curr_insurances = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == work_order.PATIENT_ACCOUNT && x.Pri_Sec_Oth_Type == "P" && (x.Deleted ?? false) == false && (string.IsNullOrEmpty(x.FOX_INSURANCE_STATUS) || x.FOX_INSURANCE_STATUS == "C")).OrderByDescending(t => t.Modified_Date).FirstOrDefault(); //Current
            var ins_name = "";
            if (curr_insurances != null)
            {
                ins_name = _foxInsurancePayersRepository.GetFirst(t => (t.DELETED == null || t.DELETED == false) && t.FOX_TBL_INSURANCE_ID == curr_insurances.FOX_TBL_INSURANCE_ID).INSURANCE_NAME ?? "";
            }
            var frictionLessReferralData = _frictionlessReferralRepository.GetFirst(t => t.DELETED == false && t.WORK_ID == work_id);
            if (frictionLessReferralData != null && !string.IsNullOrEmpty(frictionLessReferralData.PATIENT_INSURANCE_PAYER_ID))
            {
                ins_name = _foxInsurancePayersRepository.GetFirst(t => (t.DELETED == null || t.DELETED == false) && t.INSURANCE_PAYERS_ID == frictionLessReferralData.PATIENT_INSURANCE_PAYER_ID).INSURANCE_NAME ?? "";
            }

            if (!String.IsNullOrWhiteSpace(ins_name))
            {
                pri_insurance = ins_name;
            }
            var file_name = "";
            if (frictionLessReferralData != null)
            {
                file_name = frictionLessReferralData.PATIENT_LAST_NAME + "_" + documentType;
            }
            else
            {
                file_name = patient.Last_Name + "_" + documentType;
            }

            var Sender = _User.GetFirst(T => T.USER_NAME == sourceDetail.CREATED_BY);
            if (Sender == null && sourceDetail != null && !string.IsNullOrEmpty(sourceDetail.CREATED_BY) && sourceDetail.CREATED_BY.Equals("FOX TEAM"))
            {
                Sender = _User.GetFirst(T => T.USER_NAME == profile.UserName);
            }
            var discipline = "";
            if (sourceDetail != null)
            {
                if (!string.IsNullOrEmpty(sourceDetail?.DEPARTMENT_ID))
                {
                    if (sourceDetail.DEPARTMENT_ID.EndsWith("1"))
                    {
                        sourceDetail.DEPARTMENT_ID = sourceDetail.DEPARTMENT_ID + ",";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("1,"))
                    {
                        discipline = discipline + " Occupational Therapy (OT), ";
                        if (sourceDetail.DEPARTMENT_ID.EndsWith(","))
                        {
                            sourceDetail.DEPARTMENT_ID = sourceDetail.DEPARTMENT_ID.Remove(sourceDetail.DEPARTMENT_ID.Length - 1, 1);
                        }
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("10"))
                    {
                        discipline = discipline + " Skilled Nursing (SN), ";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("2"))
                    {
                        discipline = discipline + " Physical Therapy (PT), ";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("3"))
                    {
                        discipline = discipline + " Speech Therapy (ST), ";
                    }
                    if (sourceDetail.DEPARTMENT_ID == "4")
                    {
                        discipline = discipline + "Physical/Occupational/Speech Therapy(PT/OT/ST)";
                    }
                    if (sourceDetail.DEPARTMENT_ID == "5")
                    {
                        discipline = discipline + "Physical/Occupational Therapy(PT/OT)";
                    }
                    if (sourceDetail.DEPARTMENT_ID == "6")
                    {
                        discipline = discipline + "Physical/Speech Therapy(PT/ST)";
                    }
                    if (sourceDetail.DEPARTMENT_ID == "7")
                    {
                        discipline = discipline + "Occupational/Speech Therapy(OT/ST)";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("8"))
                    {
                        discipline = discipline + " Unknown, ";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("9"))
                    {
                        discipline = discipline + " Exercise Physiology (EP), ";
                    }
                }
                else
                {
                    discipline = "";
                }
                if (discipline.Substring(discipline.Length - 2) == ",")
                {
                    discipline = discipline.TrimEnd(discipline[discipline.Length - 1]);
                }
            }
            QRCodeModel qr = new QRCodeModel();
            //qr.SignPath = GetProfile().SIGNATURE_PATH;
            qr.AbsolutePath = System.Web.HttpContext.Current.Server.MapPath("~/" + AppConfiguration.QRCodeTempPath);
            qr.WORK_ID = work_id;
            var qrCode = GenerateQRCode(qr);
            string body = string.Empty;
            string templatePathOfSenderEmail = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/print-send-submit-order.html");

            if (File.Exists(templatePathOfSenderEmail))
            {
                string receivedDate = string.Empty;
                string receivedTime = string.Empty;
                body = File.ReadAllText(templatePathOfSenderEmail);
                HtmlDocument htmldoc = new HtmlDocument();
                htmldoc.LoadHtml(body);
                if (work_order.IS_VERBAL_ORDER == false && work_order.is_strategic_account == true)
                {
                    var VerbalOrder = htmldoc.DocumentNode.SelectSingleNode("//span[@id='VerbalOrder']");
                    VerbalOrder.Remove();
                }
                if (work_order.IS_VERBAL_ORDER == null || work_order.IS_VERBAL_ORDER == false)
                {
                    var Verbal = htmldoc.DocumentNode.SelectSingleNode("//span[@id='VERBAL']");
                    Verbal.Remove();
                }
                if (work_order.is_strategic_account)
                {
                    var Insurance = htmldoc.DocumentNode.SelectSingleNode("//span[@id='PRIMARY_INSURANCE']");
                    Insurance.Remove();
                }
                if (work_order.IS_EVALUATE_TREAT == null || work_order.IS_EVALUATE_TREAT == false)
                {
                    var evaluate = htmldoc.DocumentNode.SelectSingleNode("//span[@id='EVALUATE_TREAT']");
                    evaluate.Remove();
                }
                if (work_order.IS_EMERGENCY_ORDER == false || frictionLessReferralData != null)
                {
                    var URGENT = htmldoc.DocumentNode.SelectSingleNode("//span[@id='URGENT']");
                    URGENT.Remove();
                }
                body = htmldoc.DocumentNode.OuterHtml;

                if (frictionLessReferralData != null)
                {
                    patient = new Patient();
                    body = body.Replace("[[PATIENT_NAME]]", frictionLessReferralData.PATIENT_LAST_NAME + ", " + frictionLessReferralData.PATIENT_FIRST_NAME);
                    var date = Convert.ToDateTime(frictionLessReferralData.PATIENT_DOB);
                    frictionLessReferralData.PATIENT_DOB_STRING = date.ToShortDateString();
                    body = body.Replace("[[PATIENT_DOB]]", frictionLessReferralData.PATIENT_DOB_STRING ?? "");
                    body = body.Replace("[[PATIENT_MRN]]", patient.Chart_Id ?? "");
                }
                else
                {
                    body = body.Replace("[[PATIENT_NAME]]", patient.Last_Name + ", " + patient.First_Name + " (" + patient.Gender + ")");
                    body = body.Replace("[[PATIENT_DOB]]", patient.Date_Of_Birth.ToString() ?? "");
                    body = body.Replace("[[PATIENT_MRN]]", patient.Chart_Id ?? "");
                }
                if (address != null)
                {
                    body = body.Replace("[[PATIENT_HOME_ADDRESS]]", address.ADDRESS ?? "");
                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                    body = body.Replace("[[PATIENT_HOME_ADDRESS_2]]", ti.ToTitleCase(address.CITY) + ", " + address.STATE + " " + address.ZIP);

                }
                else
                {
                    body = body.Replace("[[PATIENT_HOME_ADDRESS]]", "");
                    body = body.Replace("[[PATIENT_HOME_ADDRESS_2]]", "");
                }
                body = body.Replace("[[PATIENT_PRI_INS]]", pri_insurance ?? "");


                if (qrCode != null)
                {
                    body = body.Replace("[[QRCode]]", qrCode.ENCODED_IMAGE_BYTES ?? "");
                }
                body = body.Replace("[[DOCUMENT_TYPE]]", documentType ?? "");
                if (ORS != null)
                {
                    body = body.Replace("[[ORS]]", ORS.LAST_NAME + ", " + ORS.FIRST_NAME ?? "");
                }
                if (frictionLessReferralData != null && !string.IsNullOrEmpty(frictionLessReferralData.PROVIDER_LAST_NAME))
                {
                    body = body.Replace("[[ORS]]", frictionLessReferralData.PROVIDER_LAST_NAME + ", " + frictionLessReferralData.PROVIDER_FIRST_NAME ?? "" + "|" + frictionLessReferralData.PROVIDER_REGION ?? "");
                }

                if (frictionLessReferralData != null)
                {
                    body = body.Replace("[[SENDER]]", frictionLessReferralData.SUBMITER_FIRST_NAME == null ? "" : frictionLessReferralData.SUBMITTER_LAST_NAME + ", " + frictionLessReferralData.SUBMITER_FIRST_NAME ?? "");
                }
                else
                {
                    body = body.Replace("[[SENDER]]", Sender == null ? "" : Sender.LAST_NAME + ", " + Sender.FIRST_NAME ?? "");
                }

                body = body.Replace("[[TREATMENT_LOCATION]]", sourceDetail.FACILITY_NAME ?? "");
                if (fClass != null && !string.IsNullOrEmpty(fClass.NAME) && fClass.NAME.ToLower().Equals("sa- special account"))
                {
                    var Title = "Home Health";
                    var Description = "Home Health Strategic Account";
                    body = body.Replace("[[Home_Health]]", Title ?? "");
                    body = body.Replace("[[Home_Health_Description]]", Description ?? "");
                }
                else
                {
                    body = body.Replace("[[Home_Health]]", "");
                    body = body.Replace("[[Home_Health_Description]]", "");
                }

                body = body.Replace("[[Diagnosis_Information]]", diagnosis_string);

                body = body.Replace("[[discipline]]", discipline ?? "");

                if (specialty != null && !String.IsNullOrWhiteSpace(specialty.SPECIALITY_PROGRAM))
                {
                    body = body.Replace("[[specialty_program]]", specialty.SPECIALITY_PROGRAM ?? "");
                }
                else
                {
                    body = body.Replace("[[specialty_program]]", "");
                }

                if (work_order.IS_EMERGENCY_ORDER)
                {
                    body = body.Replace("[[is_urgent_Referral]]", "Yes");
                    body = body.Replace("[[reason_urgency]]", work_order.REASON_FOR_THE_URGENCY ?? "");

                }
                else
                {
                    body = body.Replace("[[is_urgent_Referral]]", "No");
                }

                if (work_order.IS_EVALUATE_TREAT != null && work_order.IS_EVALUATE_TREAT == true)
                {
                    body = body.Replace("[[HHH_NAME]]", work_order.HEALTH_NAME ?? "");
                    body = body.Replace("[[HHH_NUMBER]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(work_order.HEALTH_NUMBER) ?? "");
                }



                if (work_order.IS_VERBAL_ORDER != null && work_order.IS_VERBAL_ORDER == true)
                {
                    body = body.Replace("[[is_verbal_order]]", "Yes");
                    body = body.Replace("[[on_behalf_of]]", work_order.VO_ON_BEHALF_OF.ToString());
                    body = body.Replace("[[received_by]]", work_order.VO_RECIEVED_BY ?? "");
                    body = body.Replace("[[verbal_date]]", work_order.VO_DATE_TIME.Value.ToString("MM/dd/yyyy") ?? "");
                    body = body.Replace("[[verbal_time]]", work_order.VO_DATE_TIME.Value.ToString("hh:mm tt") ?? "");
                }
                else
                {
                    body = body.Replace("[[is_verbal_order]]", "No");
                }

                body = body.Replace("[[additional_notes]]", work_order.REASON_FOR_VISIT ?? "");
                var provider = _InsertUpdateOrderingSourceRepository.GetFirst(t => t.SOURCE_ID == sourceDetail.SENDER_ID);
                if (provider != null)
                {
                    if (provider.FAX != null)
                    {
                        var splitedFaxIndex = provider.FAX.Substring(0);
                        if (splitedFaxIndex != null && splitedFaxIndex.Length >= 10)
                        {
                            var newFormatFax = 1 + " " + "(" + splitedFaxIndex[0] + splitedFaxIndex[1] + splitedFaxIndex[2] + ")" + " " + splitedFaxIndex[3] + splitedFaxIndex[4] + splitedFaxIndex[5] + " " + "<span> &#8208;</span>" + " " + splitedFaxIndex[6] +
                            splitedFaxIndex[7] + splitedFaxIndex[8] + splitedFaxIndex[9];
                            body = body.Replace("[[provider_fax]]", "+" + newFormatFax ?? "");
                        }
                    }
                    body = body.Replace("[[provider_name]]", provider.LAST_NAME + ", " + provider.FIRST_NAME + " " + provider.REFERRAL_REGION);
                    body = body.Replace("[[provider_NPI]]", provider.NPI ?? "");
                    body = body.Replace("[[provider_phone]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(provider.PHONE) ?? "");
                    body = body.Replace("[[provider_date]]", Helper.GetCurrentDate().ToShortDateString() ?? "");
                }
                else
                {
                    if (frictionLessReferralData != null)
                    {
                        body = body.Replace("[[provider_name]]", frictionLessReferralData.PROVIDER_LAST_NAME + ", " + frictionLessReferralData.PROVIDER_FIRST_NAME + " " + frictionLessReferralData.PROVIDER_REGION);
                        body = body.Replace("[[provider_NPI]]", frictionLessReferralData.PROVIDER_NPI ?? "");
                        body = body.Replace("[[provider_phone]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(frictionLessReferralData.SUBMITTER_PHONE) ?? "");
                        body = body.Replace("[[provider_date]]", Helper.GetCurrentDate().ToShortDateString() ?? "");
                        if (frictionLessReferralData.PROVIDER_FAX != null)
                        {
                            var splitedFaxIndex = frictionLessReferralData.PROVIDER_FAX.Substring(0);
                            if (splitedFaxIndex != null && splitedFaxIndex.Length >= 10)
                            {
                                var newFormatFax = 1 + " " + "(" + splitedFaxIndex[0] + splitedFaxIndex[1] + splitedFaxIndex[2] + ")" + " " + splitedFaxIndex[3] + splitedFaxIndex[4] + splitedFaxIndex[5] + " " + "<span> &#8208;</span>" + " " + splitedFaxIndex[6] +
                                splitedFaxIndex[7] + splitedFaxIndex[8] + splitedFaxIndex[9];
                                body = body.Replace("[[provider_fax]]", "+" + newFormatFax ?? "");
                            }
                        }
                        else
                        {
                            body = body.Replace("[[provider_fax]]", "");
                        }
                    }
                    else
                    {
                        body = body.Replace("[[provider_name]]", "");
                        body = body.Replace("[[provider_NPI]]", "");
                        body = body.Replace("[[provider_phone]]", "");
                        body = body.Replace("[[provider_fax]]", "");
                        body = body.Replace("[[provider_date]]", "");
                    }
                }
                if (obj._approval)
                {
                    body = body.Replace("[[Signature]]", obj.base64textString ?? "");
                    body = body.Replace("[[current_Date]]", Helper.GetCurrentDate().ToString("MM/dd/yyyy h:mm tt") ?? "");
                }
                else
                {
                    body = body.Replace("<img style=\"width:30%; height: 60px;margin:6px;\" src=\"[[Signature]]\" alt=\"Signature\">", "________");
                    body = body.Replace("[[current_Date]]", Helper.GetCurrentDate().ToString("MM/dd/yyyy h:mm tt") ?? "");
                }

                //"<p style=\"display: inline - block; \"><span style = \"display:inline-block;border-bottom:1px solid #12222E;width:100px;\" ></ span ></ p > "
                //body = body.Replace("[[current_Date]]", Helper.GetCurrentDate().ToShortDateString() ?? "");
                body = body.Replace("[[Comments]]", obj.comments ?? "");
            }

            var config = Helper.GetServiceConfiguration(long.Parse(practiceCode));
            if (config.PRACTICE_CODE != null
                && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
            {
                ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, body, file_name.Replace(' ', '_'));
                if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                {
                    string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                    int numberOfPages = getNumberOfPagesOfPDF(filePath);
                    //string imagesPath = HttpContext.Current.Server.MapPath("~/" + ImgDirPath);
                    //SavePdfToImages(filePath, imagesPath, reqAddDocument_SignOrder.WorkId, numberOfPages, "Email", Profile.UserEmailAddress, Profile.UserName);
                    SavePdfToImages(filePath, config, work_order.WORK_ID.ToString(), work_order.WORK_ID, numberOfPages, "Email", work_order.SORCE_NAME, profile.UserName, obj._approval);

                    if (work_order != null && work_order.WORK_STATUS != null && work_order.WORK_STATUS.ToLower().Equals("completed"))
                    {
                        CreatePORTA(profile, work_order);
                    }
                    return true;

                }
                else
                {
                    return false;
                }

            }
            return false;
        }

        public QRCodeModel GenerateQRCode(QRCodeModel obj)
        {
            Bitmap result = null;
            string base64Image = "";
            try
            {
                var writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options.Height = 70;
                writer.Options.Width = 70;
                writer.Options.Margin = 0;

                result = writer.Write(obj.WORK_ID.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("QRcode is not generated by library", ex);
            }
            if (!Directory.Exists(obj.AbsolutePath))
            {
                Directory.CreateDirectory(obj.AbsolutePath);
            }

            using (var bitmap = new Bitmap(result))
            {
                result.Dispose();
                string curtime = obj.AbsolutePath + obj.WORK_ID + "_" + DateTime.Now.Ticks.ToString() + ".jpg";
                Bitmap cimage = (Bitmap)bitmap;
                cimage.Save(curtime, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
                cimage.Dispose();
                base64Image = Convert.ToBase64String(File.ReadAllBytes(curtime)); //Get Base64
            }
            string src = "data:image/png;base64," + base64Image;
            obj.ENCODED_IMAGE_BYTES = src;
            return obj;
        }

        private FOX_TBL_TASK GetTask(long Practice_Code, long Task_Id)
        {
            SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", Practice_Code);
            SqlParameter TaskID = new SqlParameter("TASK_ID", Task_Id);
            return SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK @PRACTICE_CODE, @TASK_ID", practiceCode, TaskID);

        }

        private FOX_TBL_TASK_SUB_TYPE GetTaskSubType(long Practice_Code, int Task_Type_Id, string Sub_Type_Name)
        {
            SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", Practice_Code);
            SqlParameter taskTypeId = new SqlParameter("TASK_TYPE_ID", Task_Type_Id);
            SqlParameter name = new SqlParameter("NAME", Sub_Type_Name);
            return SpRepository<FOX_TBL_TASK_SUB_TYPE>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK_SUB_TYPE @PRACTICE_CODE, @TASK_TYPE_ID, @NAME", practiceCode, taskTypeId, name);
        }


        private FOX_TBL_TASK_TASK_SUB_TYPE GetTaskTaskSubType(long task_id, long practice_code, int task_SubType_Id)
        {
            SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", practice_code);
            SqlParameter taskId = new SqlParameter("TASK_ID", task_id);
            SqlParameter taskSubtypeId = new SqlParameter("TASK_SUB_TYPE_ID", task_SubType_Id);
            return SpRepository<FOX_TBL_TASK_TASK_SUB_TYPE>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK_TASK_SUB_TYPE @TASK_ID, @PRACTICE_CODE, @TASK_SUB_TYPE_ID", taskId, practiceCode, taskSubtypeId);

        }

        private InterfaceSynchModel GetInterfaceSynch(long practice_Code, long? patient_Account, long work_Id)
        {
            if (patient_Account != null && patient_Account != 0)
            {
                SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", practice_Code);
                SqlParameter PatAccount = new SqlParameter("PATIENT_ACCOUNT", patient_Account ?? 0);
                SqlParameter WorkId = new SqlParameter("Work_ID", work_Id);
                return SpRepository<InterfaceSynchModel>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_INTERFACE_SYNCH @PRACTICE_CODE, @PATIENT_ACCOUNT, @Work_ID", PracticeCode, PatAccount, WorkId);
            }
            return null;
        }

        private List<InterfaceSynchModel> GetInterfaceSynchList(long practice_Code, long? patient_Account, long work_Id)
        {
            if (patient_Account != null && patient_Account != 0)
            {
                SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", practice_Code);
                SqlParameter PatAccount = new SqlParameter("PATIENT_ACCOUNT", patient_Account ?? 0);
                SqlParameter WorkId = new SqlParameter("Work_ID", work_Id);
                return SpRepository<InterfaceSynchModel>.GetListWithStoreProcedure(@"FOX_PROC_GET_INTERFACE_TASKED @PRACTICE_CODE, @PATIENT_ACCOUNT, @Work_ID", PracticeCode, PatAccount, WorkId);
            }
            return null;
        }

        private List<InterfaceSynchModel> GetNotInterfaceTaskList(long practice_Code, long? patient_Account, long taskId)
        {
            if (patient_Account != null && patient_Account != 0)
            {
                SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", practice_Code);
                SqlParameter PatAccount = new SqlParameter("PATIENT_ACCOUNT", patient_Account ?? 0);
                SqlParameter TaskId = new SqlParameter("TASK_ID", taskId);
                return SpRepository<InterfaceSynchModel>.GetListWithStoreProcedure(@"FOX_PROC_GET_INTERFACE_TASKED @PRACTICE_CODE, @PATIENT_ACCOUNT, @TASK_ID", PracticeCode, PatAccount, TaskId);
            }
            return null;
        }

        private InterfaceSynchModel GetSynchedPatient(long practice_Code, long? patient_Account)
        {
            if (patient_Account != null && patient_Account != 0)
            {
                SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", practice_Code);
                SqlParameter PatAccount = new SqlParameter("PATIENT_ACCOUNT", patient_Account ?? 0);
                return SpRepository<InterfaceSynchModel>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_INTERFACE_PATIENT @PRACTICE_CODE, @PATIENT_ACCOUNT", PracticeCode, PatAccount);
            }
            return null;
        }

        private Patient GetPatient(long? patient_Account)
        {
            if (patient_Account != null && patient_Account != 0)
            {
                SqlParameter PatAccount = new SqlParameter("PATIENT_ACCOUNT", patient_Account ?? 0);
                return SpRepository<Patient>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_PATIENT  @PATIENT_ACCOUNT", PatAccount);
            }
            return null;
        }

        private List<FOX_TBL_TASK_SUB_TYPE> GetTaskSubTypes(long practice_Code, int? task_type_id)
        {
            SqlParameter TaskTypeId = new SqlParameter("TASK_TYPE_ID", task_type_id ?? 0);
            SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", practice_Code);
            return SpRepository<FOX_TBL_TASK_SUB_TYPE>.GetListWithStoreProcedure(@"FOX_PROC_GET_TASK_SUB_TYPES @PRACTICE_CODE, @TASK_TYPE_ID", PracticeCode, TaskTypeId);
        }

        private List<FOX_TBL_TASK_TYPE> GetTaskTypes(long practice_Code)
        {
            SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", practice_Code);
            return SpRepository<FOX_TBL_TASK_TYPE>.GetListWithStoreProcedure(@"FOX_PROC_GET_TASK_TYPES @PRACTICE_CODE", PracticeCode);
        }
        private void InsertTaskTaskSubType(UserProfile profile, long Task_Id, int Task_Sub_Type_Id)
        {
            long Pid = Helper.getMaximumId("FOX_TBL_TASK_TASK_SUB_TYPE_ID");
            SqlParameter id = new SqlParameter("ID", Pid);
            SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter TaskId = new SqlParameter("TASK_ID", Task_Id);
            SqlParameter TaskSubTypeId = new SqlParameter("TASK_SUB_TYPE_ID", Task_Sub_Type_Id);
            SqlParameter UserName = new SqlParameter("USER_NAME", profile.UserName);

            SpRepository<FOX_TBL_TASK_TASK_SUB_TYPE>.GetListWithStoreProcedure(@"FOX_PROC_INSERT_TASK_TASK_SUB_TYPE @ID, @TASK_ID, @TASK_SUB_TYPE_ID, @PRACTICE_CODE, @USER_NAME", id, TaskId, TaskSubTypeId, PracticeCode, UserName);
        }

        private OriginalQueue InsertUpdateSpecialty(OriginalQueue obj, UserProfile profile, OriginalQueue sourceAddDetail, long? pat_account)
        {
            // speciality program zero/null value check
            //patient accout null
            if (!string.IsNullOrEmpty(obj.SPECIALITY_PROGRAM) && obj.SPECIALITY_PROGRAM != "0" && pat_account != null && pat_account != 0)
            {
                SqlParameter workId = new SqlParameter("WORK_ID", sourceAddDetail.WORK_ID);
                SqlParameter specialityProgram = new SqlParameter("SPECIALITY_PROGRAM", obj.SPECIALITY_PROGRAM);
                SqlParameter patAccount = new SqlParameter("PATIENT_ACCOUNT", pat_account.Value);
                SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                return SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_INSERT_UPDATE_SPECIALITY_PROGRAM @WORK_ID, @SPECIALITY_PROGRAM, @PATIENT_ACCOUNT,@USER_NAME",
                                                                                                                                    workId, specialityProgram, patAccount, userName);
            }
            return null;
        }

        private void InsertUpdateAdditionalInfo(OriginalQueue sourceAddDetail)
        {
            SqlParameter pWorkId = new SqlParameter("WORK_ID", sourceAddDetail.WORK_ID);
            SqlParameter pDepartmentID = new SqlParameter("DEPARTMENT_ID", string.IsNullOrEmpty(sourceAddDetail.DEPARTMENT_ID) ? string.Empty : sourceAddDetail.DEPARTMENT_ID);
            SqlParameter pDocumentType = new SqlParameter("DOCUMENT_TYPE", !sourceAddDetail.DOCUMENT_TYPE.HasValue ? 0 : sourceAddDetail.DOCUMENT_TYPE);
            SqlParameter pSenderId = new SqlParameter("SENDER_ID", !sourceAddDetail.SENDER_ID.HasValue ? 0 : sourceAddDetail.SENDER_ID);
            SqlParameter pFacilityName = new SqlParameter("FACILITY_NAME", string.IsNullOrEmpty(sourceAddDetail.FACILITY_NAME) ? string.Empty : sourceAddDetail.FACILITY_NAME);
            SqlParameter pIndexedBy = new SqlParameter("INDEXED_BY", string.IsNullOrEmpty(sourceAddDetail.INDEXED_BY) ? string.Empty : sourceAddDetail.INDEXED_BY);
            SqlParameter pIndexedDate = new SqlParameter("INDEXED_DATE", !sourceAddDetail.INDEXED_DATE.HasValue ? (object)DBNull.Value : sourceAddDetail.INDEXED_DATE);
            SqlParameter pCompletedBy = new SqlParameter("COMPLETED_BY", string.IsNullOrEmpty(sourceAddDetail.COMPLETED_BY) ? string.Empty : sourceAddDetail.COMPLETED_BY);
            SqlParameter pCompletedDate = new SqlParameter("COMPLETED_DATE", !sourceAddDetail.COMPLETED_DATE.HasValue ? (object)DBNull.Value : sourceAddDetail.COMPLETED_DATE);
            SqlParameter pWorkStatus = new SqlParameter("WORK_STATUS", string.IsNullOrEmpty(sourceAddDetail.WORK_STATUS) ? string.Empty : sourceAddDetail.WORK_STATUS);
            SqlParameter pPatAccount23 = new SqlParameter("PATIENT_ACCOUNT", !sourceAddDetail.PATIENT_ACCOUNT.HasValue ? 0 : sourceAddDetail.PATIENT_ACCOUNT);
            SqlParameter pAccountNumber = new SqlParameter("ACCOUNT_NUMBER", string.IsNullOrEmpty(sourceAddDetail.ACCOUNT_NUMBER) ? string.Empty : sourceAddDetail.ACCOUNT_NUMBER);
            SqlParameter pFacilityId = new SqlParameter("FACILITY_ID", !sourceAddDetail.FACILITY_ID.HasValue ? 0 : sourceAddDetail.FACILITY_ID);
            SqlParameter pIsEmergencyOrder = new SqlParameter("IS_EMERGENCY_ORDER", sourceAddDetail.IS_EMERGENCY_ORDER);
            SqlParameter pReasonForVisit = new SqlParameter("REASON_FOR_VISIT", string.IsNullOrEmpty(sourceAddDetail.REASON_FOR_VISIT) ? string.Empty : sourceAddDetail.REASON_FOR_VISIT);
            SqlParameter pUnitCaseNo = new SqlParameter("UNIT_CASE_NO", string.IsNullOrEmpty(sourceAddDetail.UNIT_CASE_NO) ? string.Empty : sourceAddDetail.UNIT_CASE_NO);
            SqlParameter pDeleted = new SqlParameter("DELETED", sourceAddDetail.DELETED);
            SqlParameter pModifiedBy = new SqlParameter("MODIFIED_BY", string.IsNullOrEmpty(sourceAddDetail.MODIFIED_BY) ? string.Empty : sourceAddDetail.MODIFIED_BY);
            SqlParameter pRFOType = new SqlParameter("RFO_Type", string.IsNullOrEmpty(sourceAddDetail.RFO_Type) ? string.Empty : sourceAddDetail.RFO_Type);
            SqlParameter pIsSigned = new SqlParameter("IsSigned", sourceAddDetail.IsSigned ?? false);
            SqlParameter pSignedBy = new SqlParameter("SignedBy", !sourceAddDetail.SignedBy.HasValue ? 0 : sourceAddDetail.SignedBy);
            SqlParameter pSenderTypeId = new SqlParameter("FOX_TBL_SENDER_TYPE_ID", sourceAddDetail.FOX_TBL_SENDER_TYPE_ID ?? 0);
            SqlParameter pSenderTypeNameId = new SqlParameter("FOX_TBL_SENDER_NAME_ID", sourceAddDetail.FOX_TBL_SENDER_NAME_ID ?? 0);
            SqlParameter pReasonForUrgency = new SqlParameter("REASON_FOR_THE_URGENCY", string.IsNullOrEmpty(sourceAddDetail.REASON_FOR_THE_URGENCY) ? string.Empty : sourceAddDetail.REASON_FOR_THE_URGENCY);
            SqlParameter pIsPostAcute = new SqlParameter("IS_POST_ACUTE", sourceAddDetail.IS_POST_ACUTE ?? false);
            SqlParameter pAssignedTo = new SqlParameter("ASSIGNED_TO", string.IsNullOrEmpty(sourceAddDetail.ASSIGNED_TO) ? string.Empty : sourceAddDetail.ASSIGNED_TO);
            SqlParameter pAssignedBy = new SqlParameter("ASSIGNED_BY", string.IsNullOrEmpty(sourceAddDetail.ASSIGNED_BY) ? string.Empty : sourceAddDetail.ASSIGNED_BY);
            SqlParameter pAssignedDate = new SqlParameter("ASSIGNED_DATE", !sourceAddDetail.ASSIGNED_DATE.HasValue ? (object)DBNull.Value : sourceAddDetail.ASSIGNED_DATE);
            SqlParameter pIsEvaluateTreat = new SqlParameter("IS_EVALUATE_TREAT", sourceAddDetail.IS_EVALUATE_TREAT ?? false);
            SqlParameter pHealthName = new SqlParameter("HEALTH_NAME", string.IsNullOrEmpty(sourceAddDetail.HEALTH_NAME) ? string.Empty : sourceAddDetail.HEALTH_NAME);
            SqlParameter pHealthNumber = new SqlParameter("HEALTH_NUMBER", string.IsNullOrEmpty(sourceAddDetail.HEALTH_NUMBER) ? string.Empty : sourceAddDetail.HEALTH_NUMBER);
            SqlParameter pIsVerbalOrder = new SqlParameter("IS_VERBAL_ORDER", sourceAddDetail.IS_VERBAL_ORDER ?? false);
            SqlParameter pVoOnBehalfOf = new SqlParameter("VO_ON_BEHALF_OF", sourceAddDetail.VO_ON_BEHALF_OF ?? 0);
            SqlParameter pVoReceivedBy = new SqlParameter("VO_RECIEVED_BY", string.IsNullOrEmpty(sourceAddDetail.VO_RECIEVED_BY) ? string.Empty : sourceAddDetail.VO_RECIEVED_BY);
            SqlParameter pVoDateTime = new SqlParameter("VO_DATE_TIME", sourceAddDetail.VO_DATE_TIME ?? (object)DBNull.Value);
            SqlParameter dischargedDate = new SqlParameter("EXPECTED_DISCHARGE_DATE", sourceAddDetail.EXPECTED_DISCHARGE_DATE ?? (object)DBNull.Value);
            SqlParameter selectedSource = new SqlParameter("@FOX_SOURCE_CATEGORY_ID", sourceAddDetail?.FOX_SOURCE_CATEGORY_ID ?? 0);
            SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_INSERT_ORIGNAL_QUEUE_ADDITIONAL_INFO @WORK_ID,@DEPARTMENT_ID,@DOCUMENT_TYPE,@SENDER_ID,@FACILITY_NAME,@INDEXED_BY,@INDEXED_DATE,@COMPLETED_BY,@COMPLETED_DATE,@WORK_STATUS,@PATIENT_ACCOUNT,@ACCOUNT_NUMBER,@FACILITY_ID,@IS_EMERGENCY_ORDER,@REASON_FOR_VISIT,@UNIT_CASE_NO,@DELETED,@MODIFIED_BY,@RFO_Type,@IsSigned,@SignedBy,@FOX_TBL_SENDER_TYPE_ID,@FOX_TBL_SENDER_NAME_ID,@REASON_FOR_THE_URGENCY,@IS_POST_ACUTE,@ASSIGNED_TO,@ASSIGNED_BY,@ASSIGNED_DATE,@IS_EVALUATE_TREAT,@HEALTH_NAME,@HEALTH_NUMBER,@IS_VERBAL_ORDER,@VO_ON_BEHALF_OF,@VO_RECIEVED_BY,@VO_DATE_TIME,@EXPECTED_DISCHARGE_DATE ,@FOX_SOURCE_CATEGORY_ID",
                                                                                                                                pWorkId, pDepartmentID, pDocumentType, pSenderId, pFacilityName, pIndexedBy, pIndexedDate, pCompletedBy, pCompletedDate, pWorkStatus, pPatAccount23, pAccountNumber, pFacilityId, pIsEmergencyOrder, pReasonForVisit, pUnitCaseNo, pDeleted, pModifiedBy, pRFOType, pIsSigned, pSignedBy, pSenderTypeId, pSenderTypeNameId, pReasonForUrgency, pIsPostAcute, pAssignedTo, pAssignedBy, pAssignedDate, pIsEvaluateTreat, pHealthName, pHealthNumber, pIsVerbalOrder, pVoOnBehalfOf, pVoReceivedBy, pVoDateTime, dischargedDate, selectedSource);
        }

        private Patient UpdatePatientStatus(UserProfile profile, OriginalQueue sourceAddDetail, long? pat_account)//PATIENT ZERO CHECK
        {
            if (pat_account != null && pat_account != 0)
            {
                SqlParameter pPat_Account = new SqlParameter("Patient_Account", pat_account ?? 0);
                SqlParameter pGuest_Pat_Account = new SqlParameter("Guest_Patient_Account", sourceAddDetail.PATIENT_ACCOUNT ?? 0);
                SqlParameter pUser_Name = new SqlParameter("USER_NAME", profile.UserName);
                var tempPatient = SpRepository<Patient>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_UPDATE_PATIENT_OPENED_BY @Patient_Account, @Guest_Patient_Account,@USER_NAME", pPat_Account, pGuest_Pat_Account, pUser_Name);
                return tempPatient;
            }
            return null;
        }

        private FoxDocumentType GetDocumentType(OriginalQueue obj)
        {
            SqlParameter pDocumentTypeId = new SqlParameter("DOCUMENT_TYPE_ID", obj.DOCUMENT_TYPE ?? 0);
            var documentType = SpRepository<FoxDocumentType>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_DOCUMENT_TYPE @DOCUMENT_TYPE_ID", pDocumentTypeId);
            return documentType;
        }

        private FOX_TBL_TASK_TYPE GetTaskId(UserProfile profile)
        {
            SqlParameter pPracticeCode2 = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter pTaskTypeName = new SqlParameter("NAME", "block");
            var MSP_TYPE_ID = SpRepository<FOX_TBL_TASK_TYPE>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK_ID @PRACTICE_CODE, @NAME", pPracticeCode2, pTaskTypeName);
            return MSP_TYPE_ID;
        }

        private List<FOX_TBL_TASK> GetMSPTaskList(UserProfile profile, long? pat_account, FOX_TBL_TASK_TYPE MSP_TYPE_ID)
        {
            SqlParameter pTaskTypeId = new SqlParameter("TASK_TYPE_ID", MSP_TYPE_ID?.TASK_TYPE_ID ?? 0);
            SqlParameter pPatAccount3 = new SqlParameter("PATIENT_ACCOUNT", pat_account ?? 0);
            SqlParameter pPracticeCode3 = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            var MSP_TASK_EXIST = SpRepository<FOX_TBL_TASK>.GetListWithStoreProcedure(@"FOX_PROC_GET_MSP_TASK_LIST @TASK_TYPE_ID, @PATIENT_ACCOUNT, @PRACTICE_CODE", pTaskTypeId, pPatAccount3, pPracticeCode3);
            return MSP_TASK_EXIST;
        }

        private Patient GetTaskPatient(long? patientAccount, long practiceCode)
        {
            SqlParameter patAccount = new SqlParameter("PATIENT_ACCOUNT", patientAccount);
            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", practiceCode);
            return SpRepository<Patient>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_TASK_PATIENT @PATIENT_ACCOUNT, @PRACTICE_CODE", patAccount, practice_Code);
        }

        private OriginalQueue GetSourceDetail(long practiceCode, long workId)
        {
            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", practiceCode);
            SqlParameter work_Id = new SqlParameter("WORK_ID", workId);
            return SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_SOURCE_DETAIL @PRACTICE_CODE, @WORK_ID", practice_Code, work_Id);
        }

        private ReferralSource GetOrderingRefrralSource(long? SenderId)
        {
            SqlParameter source_Id = new SqlParameter("SOURCE_ID", SenderId);
            return SpRepository<ReferralSource>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_ORDERING_REFERRAL_SOURCE @SOURCE_ID", source_Id);

        }

        private User GetIndexer(string userName, long practiceCode)
        {
            SqlParameter user_Name = new SqlParameter("USER_NAME", userName);
            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", practiceCode);
            if (userName == null)
            {
                user_Name.Value = DBNull.Value;
            }
            return SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_INDEX_INFO_INDEXER @USER_NAME, @PRACTICE_CODE", user_Name, practice_Code);
        }

        private List<PatientInsurance> GetPatientCurrentInsurance(long? patient_Account)
        {
            if (patient_Account != null && patient_Account != 0)
            {
                SqlParameter patAccount = new SqlParameter("PATIENT_ACCOUNT", patient_Account);
                return SpRepository<PatientInsurance>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_CURRENT_INSURANCES @PATIENT_ACCOUNT", patAccount);
            }
            return null;
        }

        private User GetReferralUser(string sourceType, string sourceName)
        {
            SqlParameter Source_Type = new SqlParameter("SORCE_TYPE", sourceType);
            SqlParameter Source_Name = new SqlParameter("EMAIL", sourceName);
            return SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_REFERRAL_USER @SORCE_TYPE, @EMAIL", Source_Type, Source_Name);
        }

        private FacilityLocation GetReferralSourceFacilities(long? FacilityId, long practiceCode)
        {
            if (FacilityId != null && FacilityId != 0)
            {
                SqlParameter Facility_Id = new SqlParameter("LOC_ID", FacilityId);
                SqlParameter Practice_Code = new SqlParameter("PRACTICE_CODE", practiceCode);
                return SpRepository<FacilityLocation>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_REFERRAL_SOURCE_FACILITIES @LOC_ID, @PRACTICE_CODE", Facility_Id, Practice_Code);
            }
            return null;

        }

        private FinancialClass GetFinancialClass(string fcCode, long practiceCode)
        {
            SqlParameter Fc_Code = new SqlParameter("FC_CODE", fcCode);
            SqlParameter Practice_Code = new SqlParameter("PRACTICE_CODE", practiceCode);
            return SpRepository<FinancialClass>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_FINANCIAL_CLASS @FC_CODE, @PRACTICE_CODE", Fc_Code, Practice_Code);
        }

        private FOX_TBL_PATIENT_PROCEDURE GetROProcedure(long workId)
        {
            SqlParameter Work_Id = new SqlParameter("WORK_ID", workId);
            return SpRepository<FOX_TBL_PATIENT_PROCEDURE>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_RO_REFERRAL_PROCEDURE @WORK_ID", Work_Id);

        }

        private FOX_TBL_ELIG_HTML GetPatienEligibilityDetail(long practiceCode, long patientInsuranceId, long? patientAccount)
        {
            if (patientAccount != null && patientAccount != 0)
            {
                SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", practiceCode);
                SqlParameter patient_Insurance_Id = new SqlParameter("PATIENT_INSURACE_ID", patientInsuranceId);
                SqlParameter Patient_Account = new SqlParameter("PATIENT_ACCOUNT", patientAccount);
                return SpRepository<FOX_TBL_ELIG_HTML>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_ELIG_DETAIL @PRACTICE_CODE, @PATIENT_INSURACE_ID, @PATIENT_ACCOUNT", practice_Code, patient_Insurance_Id, Patient_Account);
            }
            return null;
        }

        private MedicareLimit GetMedicareLimit(long? hospiceLimitID, long? patientAccount, long practiceCode)
        {
            if (patientAccount != null && patientAccount != 0)
            {
                SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", practiceCode);
                SqlParameter patient_Insurance_Id = new SqlParameter("MEDICARE_LIMIT_ID", hospiceLimitID);
                SqlParameter Patient_Account = new SqlParameter("PATIENT_ACCOUNT", patientAccount);
                return SpRepository<MedicareLimit>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_ELIG_DETAIL @PRACTICE_CODE, @MEDICARE_LIMIT_ID, @PATIENT_ACCOUNT", practice_Code, patient_Insurance_Id, Patient_Account);
            }
            return null;

        }

        private PatientPOSLocation GetPatientPOS(long? facilityId, long? patientAccount)
        {
            if (facilityId != null && facilityId != 0)
            {
                SqlParameter loc_Id = new SqlParameter("FACILITY_ID", facilityId);
                SqlParameter patient_Account = new SqlParameter("PATIENT_ACCOUNT", patientAccount);
                return SpRepository<PatientPOSLocation>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_POS @PATIENT_ACCOUNT, @FACILITY_ID", patient_Account, loc_Id);
            }
            return null;
        }

        private DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER GetCaseIdentifier(long? identifierID)
        {
            if (identifierID != null && identifierID != 0)
            {
                SqlParameter Identifier_Id = new SqlParameter("IDENTIFIER_ID", identifierID);
                return SpRepository<DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_IDENTIFIER @IDENTIFIER_ID", Identifier_Id);
            }
            return null;
        }

        private Speciality GetSpeciality(long? specialityId)
        {

            if (specialityId != null && specialityId != 0)
            {
                SqlParameter Speciality_Id = new SqlParameter("SPECIALITY_ID", specialityId);
                return SpRepository<Speciality>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_SPECIALITY @SPECIALITY_ID", Speciality_Id);
            }
            return null;
        }

        private PracticeOrganization GetPracticeOrganization(long? PracticeOrganizaitonID)
        {
            if (PracticeOrganizaitonID != null && PracticeOrganizaitonID != 0)
            {
                SqlParameter Practice_Organiation_ID = new SqlParameter("PRACTICE_ORGANIZATION_ID", PracticeOrganizaitonID);
                return SpRepository<PracticeOrganization>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PRACTICE_ORGANIZATION @PRACTICE_ORGANIZATION_ID", Practice_Organiation_ID);
            }
            return null;
        }

        private string GetInsurance(long InsuranceId)
        {
            SqlParameter Insurance_ID = new SqlParameter("INSURANCE_ID", InsuranceId);
            FoxInsurancePayers val = SpRepository<FoxInsurancePayers>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_INSURANCE @INSURANCE_ID", Insurance_ID);
            if (val != null)
            {
                return string.IsNullOrEmpty(val.INSURANCE_NAME) ? string.Empty : val.INSURANCE_NAME;
            }
            return string.Empty;
        }

        private void InsertTaskLog(long? taskId, List<TaskLog> tasklog, UserProfile profile)
        {
            try
            {


                if (taskId != null && taskId.Value > 0)
                {
                    foreach (var item in tasklog)
                    {
                        List<TaskLog> lstTaskLog = new List<TaskLog>();
                        lstTaskLog.Add(item);
                        DataTable _dataTable = GetTaskLogTable(lstTaskLog);

                        if (_dataTable.Rows.Count > 0)
                        {
                            long primaryKey = Helper.getMaximumId("FOX_TASK_LOG_ID");
                            SqlParameter id = new SqlParameter("ID", primaryKey);
                            SqlParameter task_log = new SqlParameter("TASK_LOG", SqlDbType.Structured);
                            task_log.TypeName = "TASK_LOG_HISTORY";
                            task_log.Value = _dataTable;
                            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                            SqlParameter Task_Id = new SqlParameter("TASK_ID", taskId);
                            //SqlParameter pAction = new SqlParameter("ACTION", string.IsNullOrEmpty(Action) ? string.Empty : Action);
                            //SqlParameter Action_Detail = new SqlParameter("ACTION_DETAIL", string.IsNullOrEmpty(ActionDetail) ? string.Empty : ActionDetail);
                            SqlParameter user_Name = new SqlParameter("USER_NAME", profile.UserName);
                            SpRepository<TaskLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_INSERT_TASK_LOG @ID, @PRACTICE_CODE, @TASK_ID, @TASK_LOG, @USER_NAME", id, practice_Code, Task_Id, task_log, user_Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (retrycatch <= 2 && !string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    retrycatch = retrycatch + 1;
                    InsertTaskLog(taskId, tasklog, profile);
                }
                else
                {
                    throw ex;
                }

            }
        }

        private DataTable GetTaskLogTable(List<TaskLog> lst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ACTION", typeof(string));
            dt.Columns.Add("ACTION_DETAIL", typeof(string));
            foreach (TaskLog task in lst)
            {
                dt.Rows.Add(task.ACTION, task.ACTION_DETAIL);
            }
            return dt;
        }
        private User GetProfileUserById(UserProfile profile)
        {
            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter user_ID = new SqlParameter("USER_ID", profile.userID);
            return SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PROFILE_USER_BY_ID @PRACTICE_CODE, @USER_ID", practice_Code, user_ID);
        }
        private User GetSupervisorUser(UserProfile profile)
        {
            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter email = new SqlParameter("EMAIL", AppConfiguration.SupervisorEmailAddress);
            return SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_SUPERVISOR @PRACTICE_CODE, @EMAIL", practice_Code, email);
        }
        public DsFoxOcr GetOCRData(Index_infoReq obj, UserProfile profile)
        {
            var patient = _DsFoxOcrRepository.GetFirst(t => t.work_id == obj.WORK_ID && !(t.Deleted ?? false));
            //if (patient != null)
            //{
            return patient;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public FacilityLocation GetLocationByID(long? loc_id, UserProfile profile)
        {
            if (loc_id != null)
            {
                var location = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == loc_id && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                if (location != null)
                {
                    return location;
                }
                else
                {
                    return new FacilityLocation();
                }
            }
            else
            {
                return new FacilityLocation();
            }
        }

        public bool UpdateOCRValue(long? work_id, UserProfile profile)
        {
            var ocrStatus = _OcrStatusRepository.GetFirst(x => x.OCR_STATUS.ToLower() == "Data pulled into referral" && !(x.DELETED ?? false)).OCR_STATUS_ID;
            if (ocrStatus != 0)
            {
                var que = _QueueRepository.GetFirst(x => x.WORK_ID == work_id && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);

                if (que != null)
                {
                    que.OCR_STATUS_ID = ocrStatus;
                    que.MODIFIED_BY = profile.UserName;
                    que.MODIFIED_DATE = Helper.GetCurrentDate();
                    _QueueRepository.Update(que);
                    _QueueRepository.Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public ReferralPatientInfo getPatientReferralDetail(long work_id, UserProfile profile)
        {
            ReferralPatientInfo result = new ReferralPatientInfo();
            result.originalQueue = new OriginalQueue();
            result.patientData = new Patient();
            var patientResponse = new Patient();

            var referralResponse = _QueueRepository.GetFirst(x => x.WORK_ID == work_id && x.PRACTICE_CODE == profile.PracticeCode && !(x.DELETED));
            if (referralResponse != null && (referralResponse.PATIENT_ACCOUNT != null || referralResponse.PATIENT_ACCOUNT != 0))
            {
                patientResponse = _PatientRepository.GetFirst(x => x.Patient_Account == referralResponse.PATIENT_ACCOUNT && !(x.DELETED ?? false));
            }
            if (referralResponse != null)
            {
                //result.DocumentTypeName = _foxdocumenttypeRepository.GetFirst(x => x.DOCUMENT_TYPE_ID == referralResponse.DOCUMENT_TYPE && !(x.DELETED)).NAME;
                if (referralResponse.DOCUMENT_TYPE != null)
                {
                    var DocumentTypeName = _foxdocumenttypeRepository.GetFirst(x => x.DOCUMENT_TYPE_ID == referralResponse.DOCUMENT_TYPE && !(x.DELETED));
                    if (DocumentTypeName != null && DocumentTypeName.NAME != null)
                    {
                        result.DocumentTypeName = DocumentTypeName.NAME;
                    }
                }
                var User = _User.GetFirst(x => x.USER_NAME == referralResponse.CREATED_BY && !(x.DELETED));
                if (User != null)
                {
                    result.SenderName = (!string.IsNullOrEmpty(User.LAST_NAME) ? User.LAST_NAME + ", " : "") + (!string.IsNullOrEmpty(User.FIRST_NAME) ? User.FIRST_NAME + ", " : "");
                }
                else
                {
                    result.SenderName = string.Empty;
                }
                if (referralResponse.SENDER_ID != null)
                {
                    result.referralSource = _InsertUpdateOrderingSourceRepository.GetFirst(x => x.SOURCE_ID == referralResponse.SENDER_ID && x.PRACTICE_CODE == profile.PracticeCode && !(x.DELETED));
                }
                result.originalQueue = referralResponse;
            }
            if (referralResponse != null)
            {
                result.patientData = patientResponse;
            }
            return result;
        }

        public void CreatePORTA(UserProfile profile, OriginalQueue obj)
        {
            var interfaceTaskStrategic = setTaskDataForPORTA(profile, obj);
            var taskInterfaceStrategic = AddUpdateTaskForPORTA(interfaceTaskStrategic, profile, obj);

            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            if (taskInterfaceStrategic != null)
            {
                var IS_INTERFACEDOC = 0;
                interfaceSynch.TASK_ID = taskInterfaceStrategic.TASK_ID;
                interfaceSynch.Work_ID = obj.WORK_ID;
                interfaceSynch.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                IS_INTERFACEDOC = InsertInterfaceTeamData(interfaceSynch, profile);
                if (IS_INTERFACEDOC == 0)
                {
                    IS_INTERFACEDOC = InsertInterfaceTeamData(interfaceSynch, profile);
                }
            }
        }
        private FOX_TBL_TASK setTaskDataForPORTA(UserProfile profile, OriginalQueue obj)
        {
            var task = new FOX_TBL_TASK();
            SqlParameter pPracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter pTaskTypeName = new SqlParameter();
            pTaskTypeName.ParameterName = "NAME";
            pTaskTypeName.Value = "porta";

            var Task_type_Id = SpRepository<FOX_TBL_TASK_TYPE>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK_ID @PRACTICE_CODE, @NAME", pPracticeCode, pTaskTypeName);
            task.TASK_TYPE_ID = Task_type_Id?.TASK_TYPE_ID ?? 0;

            task.PRACTICE_CODE = profile.PracticeCode;
            task.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;

            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var _taskTId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = -1 };
            var _taskTypeId = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = task.TASK_TYPE_ID };
            var _isTemplate = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = true };
            var taskTemplate = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_TASK_BY_TASK_TYPE_ID 
                               @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCode, _taskTId, _taskTypeId, _isTemplate);
            if (taskTemplate != null)
            {
                if (obj.is_strategic_account)
                {
                    SqlParameter pPractice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                    SqlParameter pGroupName = new SqlParameter("GROUP_NAME", "02CC2");
                    var group02CC2 = SpRepository<GROUP>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_GROUP_ID @PRACTICE_CODE, @GROUP_NAME", pPractice_Code, pGroupName);

                    if (group02CC2 != null)
                    {
                        task.SEND_TO_ID = group02CC2.GROUP_ID;
                    }
                }
                else
                {
                    task.SEND_TO_ID = taskTemplate.SEND_TO_ID;
                }
                task.IS_SEND_TO_USER = taskTemplate.IS_SEND_TO_USER;
                task.FINAL_ROUTE_ID = taskTemplate.FINAL_ROUTE_ID;
                task.IS_FINAL_ROUTE_USER = taskTemplate.IS_FINAL_ROUTE_USER;
                task.PRIORITY = "MEDIUM";
                task.DUE_DATE_TIME = Helper.GetCurrentDate();
            }
            return task;
        }
        public FOX_TBL_TASK AddUpdateTaskForPORTA(FOX_TBL_TASK task, UserProfile profile, OriginalQueue WORK_QUEUE)
        {
            if (!string.IsNullOrEmpty(task.PATIENT_ACCOUNT_STR))
            {
                task.PATIENT_ACCOUNT = Convert.ToInt64(task.PATIENT_ACCOUNT_STR);
            }
            if (task != null && profile != null)
            {
                FOX_TBL_TASK dbTask = GetTask(profile.PracticeCode, task.TASK_ID);

                if (dbTask == null)
                {
                    long primaryKey = Helper.getMaximumId("FOX_TASK_ID");
                    SqlParameter id = new SqlParameter("ID", primaryKey);
                    SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                    SqlParameter patientAccount = new SqlParameter("PATIENT_ACCOUNT", task.PATIENT_ACCOUNT);
                    SqlParameter isCompletedInt = new SqlParameter("IS_COMPLETED_INT", SqlDbType.Int);
                    isCompletedInt.Value = 0;// 0: Initiated 1:Sender Completed 2:Final Route Completed
                    SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                    SqlParameter isTemplate = new SqlParameter("IS_TEMPLATE", task.IS_TEMPLATE);
                    SqlParameter taskTypeId = new SqlParameter("TASK_TYPE_ID", task.TASK_TYPE_ID);
                    SqlParameter sendToId = new SqlParameter("SEND_TO_ID", task.SEND_TO_ID ?? (object)DBNull.Value);
                    SqlParameter finalRouteId = new SqlParameter("FINAL_ROUTE_ID", task.FINAL_ROUTE_ID ?? (object)DBNull.Value);
                    SqlParameter priority = new SqlParameter("PRIORITY", string.IsNullOrEmpty(task.PRIORITY) ? string.Empty : task.PRIORITY);
                    SqlParameter dueDateTime = new SqlParameter("DUE_DATE_TIME", string.IsNullOrEmpty(task.DUE_DATE_TIME_str) ? (object)DBNull.Value : Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str) ?? Helper.GetCurrentDate());
                    task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                    SqlParameter categoryID = new SqlParameter("CATEGORY_ID", task.CATEGORY_ID ?? (object)DBNull.Value);
                    SqlParameter isReqSignedoff = new SqlParameter("IS_REQ_SIGNOFF", task.IS_REQ_SIGNOFF ?? (object)DBNull.Value);
                    SqlParameter isSendingRouteDetails = new SqlParameter("IS_SENDING_ROUTE_DETAILS", task.IS_SENDING_ROUTE_DETAILS ?? (object)DBNull.Value);
                    SqlParameter sendContextId = new SqlParameter("SEND_CONTEXT_ID", task.SEND_CONTEXT_ID ?? (object)DBNull.Value);
                    SqlParameter contextInfo = new SqlParameter("CONTEXT_INFO", task.CONTEXT_INFO ?? (object)DBNull.Value);
                    SqlParameter deliveryId = new SqlParameter("DEVELIVERY_ID", task.DEVELIVERY_ID ?? (object)DBNull.Value);
                    SqlParameter destinations = new SqlParameter("DESTINATIONS", task.DESTINATIONS ?? (object)DBNull.Value);
                    SqlParameter loc_Id = new SqlParameter("LOC_ID", task.LOC_ID ?? (object)DBNull.Value);
                    SqlParameter provider_ID = new SqlParameter("PROVIDER_ID", task.PROVIDER_ID ?? (object)DBNull.Value);
                    SqlParameter isSendMailAuto = new SqlParameter("IS_SEND_EMAIL_AUTO", task.IS_SEND_EMAIL_AUTO ?? (object)DBNull.Value);
                    SqlParameter deleted = new SqlParameter("DELETED", task.DELETED);
                    SqlParameter isSendToUser = new SqlParameter("IS_SEND_TO_USER", task.IS_SEND_TO_USER);
                    SqlParameter isFinalRouteUser = new SqlParameter("IS_FINAL_ROUTE_USER", task.IS_FINAL_ROUTE_USER);
                    SqlParameter isFinalRouteMarkComplete = new SqlParameter("IS_FINALROUTE_MARK_COMPLETE", task.IS_FINALROUTE_MARK_COMPLETE);
                    SqlParameter isSendToMarkComplete = new SqlParameter("IS_SENDTO_MARK_COMPLETE", task.IS_SENDTO_MARK_COMPLETE);

                    dbTask = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_TASK @ID, @PRACTICE_CODE,@PATIENT_ACCOUNT ,@IS_COMPLETED_INT ,@USER_NAME,@IS_TEMPLATE,@TASK_TYPE_ID,@SEND_TO_ID,@FINAL_ROUTE_ID,@PRIORITY,@DUE_DATE_TIME,@CATEGORY_ID,@IS_REQ_SIGNOFF,@IS_SENDING_ROUTE_DETAILS,@SEND_CONTEXT_ID,@CONTEXT_INFO,@DEVELIVERY_ID,@DESTINATIONS,@LOC_ID,@PROVIDER_ID,@IS_SEND_EMAIL_AUTO,@DELETED,@IS_SEND_TO_USER,@IS_FINAL_ROUTE_USER,@IS_FINALROUTE_MARK_COMPLETE,@IS_SENDTO_MARK_COMPLETE",
                    id, practiceCode, patientAccount, isCompletedInt, userName, isTemplate, taskTypeId, sendToId, finalRouteId, priority, dueDateTime, categoryID, isReqSignedoff, isSendingRouteDetails, sendContextId, contextInfo, deliveryId, destinations, loc_Id, provider_ID, isSendMailAuto, deleted, isSendToUser, isFinalRouteUser, isFinalRouteMarkComplete, isSendToMarkComplete);

                    if (WORK_QUEUE != null)
                    {
                        AddTaskLogsForPORTA(dbTask, task, profile, WORK_QUEUE);
                    }

                    var taskSubTypeList = GetTaskSubTypes(profile.PracticeCode, task.TASK_TYPE_ID);
                    foreach (var taskSubType in taskSubTypeList)
                    {
                        if (!string.IsNullOrEmpty(taskSubType.NAME) && taskSubType.NAME.ToLower() == "existing client")
                        {
                            InsertTaskTaskSubType(profile, dbTask.TASK_ID, taskSubType.TASK_SUB_TYPE_ID);
                        }
                    }
                    dbTask.dbChangeMsg = "TaskInsertSuccessed";
                    dbTask.CATEGORY_CODE = _taskTypeRepository.GetByID(dbTask?.TASK_TYPE_ID)?.CATEGORY_CODE;

                }
                return dbTask;
            }
            return null;
        }
        private void AddTaskLogsForPORTA(FOX_TBL_TASK dbtask, FOX_TBL_TASK task, UserProfile profile, OriginalQueue WORK_QUEUE)
        {
            List<TaskLog> taskLoglist = new List<TaskLog>();
            List<string> porta_logs = new List<string>();

            if (task.TASK_TYPE_ID != null)
            {
                var sourceDetail = GetSourceDetail(profile.PracticeCode, WORK_QUEUE.WORK_ID);
                var ORS = GetOrderingRefrralSource(sourceDetail.SENDER_ID ?? 0);
                var discipline = "";
                if (sourceDetail != null)
                {
                    if (!string.IsNullOrEmpty(sourceDetail?.DEPARTMENT_ID))
                    {
                        if (sourceDetail.DEPARTMENT_ID.Contains("1"))
                        {
                            discipline = discipline + " Occupational Therapy (OT), ";
                        }
                        if (sourceDetail.DEPARTMENT_ID.Contains("2"))
                        {
                            discipline = discipline + " Physical Therapy (PT), ";
                        }
                        if (sourceDetail.DEPARTMENT_ID.Contains("3"))
                        {
                            discipline = discipline + " Speech Therapy (ST), ";
                        }
                        if (sourceDetail.DEPARTMENT_ID == "4")
                        {
                            discipline = discipline + "Physical/Occupational/Speech Therapy(PT/OT/ST)";
                        }
                        if (sourceDetail.DEPARTMENT_ID == "5")
                        {
                            discipline = discipline + "Physical/Occupational Therapy(PT/OT)";
                        }
                        if (sourceDetail.DEPARTMENT_ID == "6")
                        {
                            discipline = discipline + "Physical/Speech Therapy(PT/ST)";
                        }
                        if (sourceDetail.DEPARTMENT_ID == "7")
                        {
                            discipline = discipline + "Occupational/Speech Therapy(OT/ST)";
                        }
                        if (sourceDetail.DEPARTMENT_ID.Contains("8"))
                        {
                            discipline = discipline + " Unknown, ";
                        }
                        if (sourceDetail.DEPARTMENT_ID.Contains("9"))
                        {
                            discipline = discipline + " Exercise Physiology (EP), ";
                        }
                    }
                    else
                    {
                        discipline = "none";
                    }
                    if (discipline.Substring(discipline.Length - 2) == ",")
                    {
                        discipline = discipline.TrimEnd(discipline[discipline.Length - 1]);
                    }
                }
                porta_logs.Add("Signed on :" + Helper.GetCurrentDate());
                porta_logs.Add("Ordering Referral Source: " + ORS.CODE + " - " + ORS.LAST_NAME + ", " + ORS.FIRST_NAME + " - " + ORS.REFERRAL_REGION + " - NPI: " + ORS.NPI);
                porta_logs.Add("Discipline : " + discipline);
                porta_logs.Add(" <br>Signed Referral Received and Interfaced to Scanning with the following details: >");
            }
            porta_logs.Reverse();
            StringBuilder portalog = new StringBuilder();

            foreach (string str in porta_logs)
            {
                portalog.Append(str + "<br>");
            }
            taskLoglist.Add(new TaskLog()
            {
                ACTION = "Porta Logs",
                ACTION_DETAIL = portalog.ToString()
            }
                        );

            if (taskLoglist.Count() > 0)
            {
                InsertTaskLog(dbtask.TASK_ID, taskLoglist, profile);
            }
        }
        public ReferralSourceAndGroups getAllReferralSourceAndGroups(UserProfile profile)
        {
            ReferralSourceAndGroups response = new ReferralSourceAndGroups();
            if (profile.isTalkRehab)
            {
                response.ReferralSource = _referralSourceTableRepository.GetMany(x => !(x.DELETED));
                response.Groups = _groupRepository.GetMany(x => !(x.DELETED));
            }
            else
            {
                response.ReferralSource = _referralSourceTableRepository.GetMany(x => !(x.DELETED) && (x.PRACTICE_CODE == profile.PracticeCode));
                response.Groups = _groupRepository.GetMany(x => !(x.DELETED) && (x.PRACTICE_CODE == profile.PracticeCode));
            }

            return response;
        }

        public pendingBalanceAmount GetPatientBalance(long? patientAccount)
        {
            return getPendingHighBalance(patientAccount);
        }
        public List<PatientListResponse> GetpatientsList(getPatientReq req, UserProfile Profile)
        {
            if (req.Last_Name == null)
                req.Last_Name = "";
            if (req.First_Name == null)
                req.First_Name = "";
            if (req.Middle_Name == null)
                req.Middle_Name = "";
            if (req.SSN == null)
                req.SSN = "";
            if (req.Gender == null)
                req.Gender = "";
            if (req.Chart_Id == null)
                req.Chart_Id = "";
            if (!string.IsNullOrEmpty(req.Date_Of_Birth_In_String))
                req.Date_Of_Birth = Convert.ToDateTime(req.Date_Of_Birth_In_String);
            else
            {
                req.Date_Of_Birth = null;
            }
            req.First_Name = string.IsNullOrEmpty(req.First_Name) ? " " : Helper.RemoveSpecialCharacters(req.First_Name.Trim());
            req.Last_Name = string.IsNullOrEmpty(req.First_Name) ? " " : Helper.RemoveSpecialCharacters(req.Last_Name.Trim());
            req.Middle_Name = string.IsNullOrEmpty(req.First_Name) ? " " : Helper.RemoveSpecialCharacters(req.Middle_Name.Trim());
            var first_Name = new SqlParameter("@First_Name", SqlDbType.VarChar) { Value = req.First_Name };
            var last_Name = new SqlParameter("@Last_Name", SqlDbType.VarChar) { Value = req.Last_Name };
            var middle_Name = new SqlParameter("@Middle_Name", SqlDbType.VarChar) { Value = req.Middle_Name };
            var SSN = new SqlParameter("@SSN", SqlDbType.VarChar) { Value = req.SSN };
            var gender = new SqlParameter("@Gender", SqlDbType.VarChar) { Value = req.Gender };
            var Practice_Code = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var _currentPage = new SqlParameter { ParameterName = "@CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = req.CURRENT_PAGE };
            var _recordPerPage = new SqlParameter { ParameterName = "@RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = req.RECORD_PER_PAGE };
            var date_Of_Birth = new SqlParameter { ParameterName = "@Date_Of_Birth", SqlDbType = SqlDbType.VarChar, Value = req.Date_Of_Birth == null ? "" : req.Date_Of_Birth?.ToString("MM/dd/yyyy") };
            var Patient_Alias = new SqlParameter { ParameterName = "@Patient_Alias", SqlDbType = SqlDbType.Bit, Value = req.INCLUDE_ALIAS };
            var _PRACTICE_ORGANIZATION_ID = new SqlParameter("@PRACTICE_ORGANIZATION_ID", SqlDbType.BigInt) { Value = Profile.PRACTICE_ORGANIZATION_ID ?? 0 };
            var result = new List<PatientListResponse>();
            if (Profile.isTalkRehab)
            {
                var chart_Id = new SqlParameter("@Chart_Id", SqlDbType.VarChar) { Value = req.Patient_Account };
                result = SpRepository<PatientListResponse>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_FOR_INDEX_INFO_FOR_CCR
                             @First_Name,@Last_Name,@Middle_Name,@Chart_Id,@SSN,@Gender,@PRACTICE_CODE,@CURRENT_PAGE,@RECORD_PER_PAGE,@PRACTICE_ORGANIZATION_ID,@Date_Of_Birth,@Patient_Alias",
                             first_Name, last_Name, middle_Name, chart_Id, SSN, gender, Practice_Code, _currentPage, _recordPerPage, _PRACTICE_ORGANIZATION_ID, date_Of_Birth, Patient_Alias);
            }
            else
            {
                var chart_Id = new SqlParameter("@Chart_Id", SqlDbType.VarChar) { Value = req.Chart_Id };
                result = SpRepository<PatientListResponse>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_FOR_INDEX_INFO
                             @First_Name,@Last_Name,@Middle_Name,@Chart_Id,@SSN,@Gender,@PRACTICE_CODE,@CURRENT_PAGE,@RECORD_PER_PAGE,@PRACTICE_ORGANIZATION_ID,@Date_Of_Birth,@Patient_Alias",
                             first_Name, last_Name, middle_Name, chart_Id, SSN, gender, Practice_Code, _currentPage, _recordPerPage, _PRACTICE_ORGANIZATION_ID, date_Of_Birth, Patient_Alias);
            }
            if (result.Any())
            {
                var dob = string.IsNullOrEmpty(req.Date_Of_Birth_In_String) ? new DateTime() : Convert.ToDateTime(req.Date_Of_Birth_In_String);
                var dobtemp = dob.ToString("MM/dd/yyyy");
                if (Profile.PRACTICE_ORGANIZATION_ID != null && ((!string.IsNullOrEmpty(req.Last_Name) && !string.IsNullOrEmpty(req.SSN)) || (!string.IsNullOrEmpty(req.Last_Name) && req.Date_Of_Birth != null)))
                {
                    var tempSameOrganizationList = result.Where(t => t.PRACTICE_ORGANIZATION_ID == Profile.PRACTICE_ORGANIZATION_ID).AsEnumerable();
                    var tempExactMatchList = result.Where(t => (t.PRACTICE_ORGANIZATION_ID != Profile.PRACTICE_ORGANIZATION_ID) && ((t.Last_Name.ToLower().Equals(req.Last_Name.ToLower()) && (t.SSN?.Equals(req.SSN) ?? false))
                                                                || (t.Last_Name.ToLower().Equals(req.Last_Name.ToLower()) && (t.Date_Of_Birth?.Equals(dobtemp) ?? false)))).AsEnumerable();
                    result = new List<PatientListResponse>();
                    if (tempSameOrganizationList.Any())
                    {
                        result.AddRange(tempSameOrganizationList);
                    }
                    if (tempExactMatchList.Any())
                    {
                        result.AddRange(tempExactMatchList);
                    }
                    var recPerPgae = 0;
                    if (req.RECORD_PER_PAGE == 0)
                    {
                        recPerPgae = result.Count();
                    }
                    else
                    {
                        recPerPgae = req.RECORD_PER_PAGE;
                    }
                    foreach (var item in result)
                    {
                        item.TOTAL_RECORDS = result.Count();
                        item.TOTAL_RECORD_PAGES = (int)Math.Ceiling((decimal)item.TOTAL_RECORDS / (decimal)recPerPgae);
                    }
                }

                return result;
            }
            else
            {
                return new List<PatientListResponse>();
            }
        }
        /// <summary>
        /// This Function is used to Get Duplicate Referral Info of Patient
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public List<DuplicateReferralInfo> GetDuplicateReferralInformation(checkDuplicateReferralRequest checkDuplicateReferral, UserProfile userProfile)
        {
            try
            {
                if (checkDuplicateReferral != null)
                {
                    var wORK_ID = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = checkDuplicateReferral.workID };
                    var getWorkID = SpRepository<GETtAll_IndexifoRes>.GetSingleObjectWithStoreProcedure(@"exec [FOX_GET_INDEX_ALL_INFO] @WORK_ID", wORK_ID);

                    if (getWorkID != null && getWorkID.PATIENT_ACCOUNT != null)
                    {
                        var Patient_Account = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = getWorkID.PATIENT_ACCOUNT };
                        var Practice_Code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = userProfile.PracticeCode };
                        var order_Id = new SqlParameter("ORDER_ID", SqlDbType.BigInt) { Value = getWorkID.WORK_ID };
                        var result = SpRepository<DuplicateReferralInfo>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_Duplicate_Referral] @PATIENT_ACCOUNT, @PRACTICE_CODE, @ORDER_ID", Patient_Account, Practice_Code, order_Id);
                        if (result != null && result.Count() > 0)
                        {
                            List<DuplicateReferralInfo> MainList = new List<DuplicateReferralInfo>();
                            if (!string.IsNullOrEmpty(checkDuplicateReferral.splitedIDs))
                            {
                                var splitedIDs = checkDuplicateReferral.splitedIDs.Split(',');
                                if (splitedIDs != null && splitedIDs.Length > 0)
                                {
                                    foreach (var item in splitedIDs)
                                    {
                                        foreach (var ite in result)
                                        {
                                            if (item != null)
                                            {
                                                //var DepartIDs = ite.DEPARTMENT_ID.Split(',');
                                                if (!string.IsNullOrEmpty(ite.DEPARTMENT_ID) && ite.DEPARTMENT_ID.Contains(item.ToString()))
                                                {
                                                    if (MainList != null)
                                                    {
                                                        if (MainList.Find(e => e.WORK_ID == ite.WORK_ID) == null)
                                                        {
                                                            MainList.Add(ite);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                return MainList;
                            }
                            else
                                return new List<DuplicateReferralInfo>();
                        }
                    }
                }
                return new List<DuplicateReferralInfo>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Get All Patient Docs.
        /// </summary>
        /// <param name="patientAccountStr"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public List<WorkOrderDocs> GetWorkOrderDocs(string patientAccountStr, UserProfile userProfile)
        {
            List<WorkOrderDocs> list = new List<WorkOrderDocs>();
            var _parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = userProfile.PracticeCode };
            var _patientAccount = !string.IsNullOrWhiteSpace(patientAccountStr) ? new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = long.Parse(patientAccountStr) } : new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = DBNull.Value };
            list = SpRepository<WorkOrderDocs>.GetListWithStoreProcedure(@"exec [FOX_PROC_ALL_PATIENT_DOCUMENTS]  @PATIENT_ACCOUNT, @PRACTICE_CODE", _parmPracticeCode, _patientAccount);
            if (list != null)
            {
                return list;
            }
            return list = new List<WorkOrderDocs>();
        }
        /// <summary>
        /// This Function is used to save Log History
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="userProfile"></param>
        public void SaveLogMessage(Index_infoReq workId, UserProfile userProfile)
        {
            if (workId != null)
            {
                Helper.LogSingleWorkOrderChange(Convert.ToInt64(workId.WORK_ID), workId.WORK_ID.ToString(), "Duplicate referral warning overridden and marked complete", userProfile.FirstName + " " + userProfile.LastName);
            }
        }

        public PatLastORS getPatientsLastORS(long patient_Account, long practice_Code)
        {
            var _patientAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = patient_Account };
            var _practice_Code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practice_Code };

            return SpRepository<PatLastORS>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_ORS_BY_PATIENT_ACCOUNT @PATIENT_ACCOUNT, @PRACTICE_CODE ", _patientAccount, _practice_Code);

        }
        /// <summary>
        /// This Function is used to get Cover Letter File Name
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        public string GetRegionCoverLetterAttachment(string regionCode)
        {
            try
            {
                string result = string.Empty;
                if (!string.IsNullOrEmpty(regionCode))
                {
                    var response = _RegionCoverLetterRepository.Get(x => x.REFERRAL_REGION_CODE == regionCode && x.IS_FAX_COVER_LETTER == true);
                    if (response != null && !string.IsNullOrEmpty(response.FILE_PATH))
                    {
                        result = response.FILE_PATH;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private long AddTalkRehabGroup(UserProfile profile)
        {
            long groupID = 0;
            var checkGroupExists = _groupRepository.GetFirst(x => x.GROUP_NAME == "Default");
            if (checkGroupExists == null)
            {
                GROUP model = new GROUP();
                model.GROUP_NAME = "Default";
                var groupDetail = _groupService.AddUpdateGroup(model, profile);
                if (groupDetail != null)
                {
                    groupID = long.Parse(groupDetail.ID);
                }
                var _parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var allAdminUsers = SpRepository<User>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ALL_ADIM_USER @PRACTICE_CODE", _parmPracticeCode);
                if (allAdminUsers != null && allAdminUsers.Count() > 0)
                {
                    GroupUsersCreateViewModel groupUsersCreateViewModel = new GroupUsersCreateViewModel();
                    List<UserWithRoles> groupUserList = new List<UserWithRoles>();
                    foreach (var user in allAdminUsers)
                    {
                        UserWithRoles groupUser = new UserWithRoles
                        {
                            FIRST_NAME = user.FIRST_NAME,
                            GROUP_ID = groupID,
                            GROUP_USER_ID = 0,
                            LAST_NAME = user.LAST_NAME,
                            ROLE_ID = (long)user.ROLE_ID,
                            ROLE_NAME = user.ROLE_NAME,
                            USER_ID = user.USER_ID,
                            USER_NAME = user.USER_NAME
                        };
                        groupUserList.Add(groupUser);
                    }
                    groupUsersCreateViewModel.USERS = groupUserList.ToArray();
                    _groupService.AddUsersInGroup(groupUsersCreateViewModel, profile);
                }
            }
            else
            {
                groupID = checkGroupExists.GROUP_ID;
            }

            return groupID;
        }
        public long GetTalkRehabTaskWorkID(long taskId, UserProfile profile)
        {
            TaskWorkInterfaceMapping mappingDetail = _TaskWorkInterfaceMapping.GetFirst(x => x.TaskId == taskId);
            if (mappingDetail != null)
            {
                InterfaceSynchModel taskInterface = __InterfaceSynchModelRepository.GetFirst(x => x.TASK_ID == taskId);
                if (taskInterface != null) //if (taskInterface != null && taskInterface.IS_SYNCED == true)
                {
                    return mappingDetail.WorkId;
                }
                else
                    return 0;
            }
            return 0;
        }
        public long MarkTaskAsComplete(long taskId, UserProfile profile)
        {
            try
            {
                FOX_TBL_TASK taskDetail = _TaskRepository.GetFirst(x => x.TASK_ID == taskId);
                if (taskDetail != null)
                {
                    taskDetail.IS_SENDTO_MARK_COMPLETE = true;
                    _TaskRepository.Update(taskDetail);
                    _TaskRepository.Save();
                }
                //InterfaceSynchModel taskInterface = __InterfaceSynchModelRepository.GetFirst(x => x.TASK_ID == taskId);
                //if(taskInterface != null)
                //{
                //    taskInterface.IS_SYNCED = true;
                //    __InterfaceSynchModelRepository.Update(taskInterface);
                //    __InterfaceSynchModelRepository.Save();
                //}
                TaskWorkInterfaceMapping mappingDetail = _TaskWorkInterfaceMapping.GetFirst(x => x.TaskId == taskId);
                if (mappingDetail != null)
                {
                    return mappingDetail.WorkId;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AdmissionImportantNotes AddAdmissionImportantNotes(AdmissionImportantNotes objAdmissionImportantNotes, UserProfile userProfile)
        {
            if (!string.IsNullOrEmpty(objAdmissionImportantNotes.NOTES))
            {
                long generalNotId = 0;
                if (objAdmissionImportantNotes.ADMISSION_IMPORTANT_NOTES_ID == 0)
                {
                    generalNotId = Helper.getMaximumId("ADMISSION_IMPORTANT_NOTES_ID");
                }
                if (objAdmissionImportantNotes != null && generalNotId != 0)
                {
                    objAdmissionImportantNotes.ADMISSION_IMPORTANT_NOTES_ID = generalNotId;
                    objAdmissionImportantNotes.CREATED_FROM = "FOX PORTAL";
                    objAdmissionImportantNotes.PRACTICE_CODE = userProfile.PracticeCode;
                    objAdmissionImportantNotes.CREATED_BY = userProfile.UserName;
                    objAdmissionImportantNotes.CREATED_DATE = Helper.GetCurrentDate();
                    objAdmissionImportantNotes.MODIFIED_BY = userProfile.UserName;
                    objAdmissionImportantNotes.MODIFIED_DATE = Helper.GetCurrentDate();
                    objAdmissionImportantNotes.DELETED = false;
                    _admissionImportantNotes.Insert(objAdmissionImportantNotes);
                    _admissionImportantNotes.Save();
                }
                else
                {
                    objAdmissionImportantNotes.PRACTICE_CODE = userProfile.PracticeCode;
                    objAdmissionImportantNotes.MODIFIED_BY = userProfile.UserName;
                    objAdmissionImportantNotes.MODIFIED_DATE = Helper.GetCurrentDate();
                    objAdmissionImportantNotes.DELETED = false;
                    _admissionImportantNotes.Update(objAdmissionImportantNotes);
                    _admissionImportantNotes.Save();
                }
            }
            return objAdmissionImportantNotes;
        }

        public AdmissionImportantNotes GetAdmissionImportantNotes(AdmissionImportantNotes objAdmissionImportantNotes, UserProfile userProfile)
        {
            AdmissionImportantNotes getAdmissionImportantNotes = new AdmissionImportantNotes();
            if (objAdmissionImportantNotes != null)
            {
                getAdmissionImportantNotes = _admissionImportantNotes.GetFirst(r => r.WORK_ID == objAdmissionImportantNotes.WORK_ID && r.PRACTICE_CODE == userProfile.PracticeCode && r.DELETED == false);
            }
            else
            {
                getAdmissionImportantNotes = null;
            }
            return getAdmissionImportantNotes;
        }
    }
}