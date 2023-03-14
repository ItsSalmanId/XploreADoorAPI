using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Authorization;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientDocuments;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.DataModels.Models.Settings.ClinicianSetup;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.TasksModel;
using HtmlAgilityPack;
using Newtonsoft.Json;
using SautinSoft;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace FOX.BusinessOperations.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly DbContextPatient _PatientContext = new DbContextPatient();
        private readonly DbContextPatientNew _NewPatientContext = new DbContextPatientNew();
        private readonly DbContextClaim _PatientClaimContext = new DbContextClaim();
        private readonly DbContextSecurity _DbContextSecurity = new DbContextSecurity();
        private readonly DbContextSettings _dbContextSettings = new DbContextSettings();
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();
        private readonly DBContextMTBCPatientInsurance _DBContextMTBCPatientInsurance = new DBContextMTBCPatientInsurance();
        private readonly DbContextTasks _TaskContext = new DbContextTasks();
        private readonly GenericRepository<Patient> _PatientRepository;
        private readonly GenericRepository<Patient> _NewPatientRepository;
        private readonly GenericRepository<PHR> _PatientPHR;
        private readonly GenericRepository<PatientUpdateHistory> _PatientUpdateHistoryRepository;
        private readonly GenericRepository<PatientAddress> _PatientAddressRepository;
        private readonly GenericRepository<PatientInsurance> _PatientInsuranceRepository;
        private readonly GenericRepository<PatientPOSLocation> _PatientPOSLocationRepository;
        private readonly GenericRepository<FacilityLocation> _FacilityLocationRepository;
        private readonly GenericRepository<FacilityType> _FacilityTypeRepository;
        private readonly GenericRepository<ReferralSource> _OrderingRefSourceRepository;
        private readonly GenericRepository<PatientContact> _PatientContactRepository;
        private readonly GenericRepository<AF_TBL_PATIENT_NEXT_OF_KIN> _AF_TBL_PATIENT_NEXT_OF_KINRepository;
        private readonly GenericRepository<WebehrTblPatientCareTeams> _WebehrTblPatientCareTeamsRepository;
        private readonly GenericRepository<ContactType> _ContactTypeRepository;
        private readonly GenericRepository<Subscriber> _SubscriberRepository;
        private readonly GenericRepository<MedicareLimitType> _MedicareLimitTypeRepository;
        private readonly GenericRepository<MedicareLimit> _MedicareLimitRepository;
        private readonly GenericRepository<Employer> _EmployerRepository;
        //private readonly GenericRepository<FoxInsurancePayors> _FoxInsurancePayorsRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT> _FoxTblPatientRepository;
        private readonly GenericRepository<FOX_TBL_AUTH> _PatientAuthorizationRepository;
        private readonly GenericRepository<FOX_TBL_APPT_TYPE> _AppointmentTypeRepository;
        private readonly GenericRepository<FOX_TBL_AUTH_APPT_TYPE> _AuthorizationAppointmentRepository;
        private readonly GenericRepository<FOX_TBL_AUTH_CHARGES> _AuthorizationChargesRepository;
        private readonly GenericRepository<FOX_TBL_AUTH_DOC> _AuthorizationDocumentRepository;
        private readonly GenericRepository<FOX_TBL_AUTH_STATUS> _AuthorizationStatusRepository;
        private readonly GenericRepository<FOX_TBL_AUTH_VALUE_TYPE> _AuthorizationValueTypeRepository;
        //Case
        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly GenericRepository<FOX_VW_CASE> _vwPatientCaseRepository;
        private readonly GenericRepository<FOX_TBL_NOTES> _NotesRepository;
        private readonly GenericRepository<FOX_TBL_NOTES_TYPE> _NotesTypeRepository;
        private readonly GenericRepository<OriginalQueue> _workQueueRepository;
        private readonly GenericRepository<FOX_TBL_ELIG_HTML> _eligHtmlRepository;
        private readonly GenericRepository<FinancialClass> _financialClassRepository;
        private readonly GenericRepository<ReconcileDemographics> _ReconcileDemoRepository;
        private readonly GenericRepository<InterfaceSynchModel> __InterfaceSynchModelRepository;
        private readonly GenericRepository<FoxInsurancePayers> _foxInsurancePayersRepository;
        private readonly GenericRepository<FOX_TBL_CASE> _caseRepository;
        //TASK
        private readonly GenericRepository<FOX_TBL_TASK_TYPE> _taskTypeRepository;
        private readonly GenericRepository<FOX_TBL_TASK> _TaskRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TASK_SUB_TYPE> _TaskTaskSubTypeRepository;
        private readonly GenericRepository<TaskLog> _taskLogRepository;
        private readonly GenericRepository<FOX_TBL_TASK_SUB_TYPE> _taskSubTypeRepository;
        private readonly GenericRepository<ClaimInsurance> _claimInsuranceRepository;
        private readonly GenericRepository<ClaimNotes> _claimNotesRepository;
        //DISCOUNT,PERIOD
        private readonly GenericRepository<PatientPRDiscount> _PatientPRDiscountRepository;
        private readonly GenericRepository<PatientPRPeriod> _PatientPRPeriodRepository;
        private readonly CommonServices.CommonServices _commonService;
        private readonly GenericRepository<FOX_TBL_CASE_STATUS> _caseStatusRepository;
        //MTBC Table Patient_Insurance
        private readonly GenericRepository<MTBCPatientInsurance> _MTBCPatientInsuranceRepository;
        private readonly GenericRepository<FoxProviderClass> _FoxProviderClassRepository;
        private readonly GenericRepository<ReferralRegion> _FoxReferralRegionRepository;
        private readonly GenericRepository<Patient_Additional_Info_TalkEHR> _talkEHRTblAdditionalPatientInfoRepository;
        private readonly GenericRepository<Patient_Address_History_WebEHR> _webEHRTblPatientAddressRepository;
        private readonly GenericRepository<NoteAlert> _noteAlertRepository;
        private readonly GenericRepository<AlertType> _alertTypeRepository;
        private readonly GenericRepository<PatientAlias> _patientAliasRepository;
        private readonly GenericRepository<CountryResponse> _CountryRepository;
        //
        private readonly DBContextPatientDocuments _PatientPATDocumentContext = new DBContextPatientDocuments();
        private readonly GenericRepository<PatientPATDocument> _foxPatientPATdocumentRepository;
        private readonly GenericRepository<PatientDocumentFiles> _foxPatientdocumentFilesRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFilesRepository;
        private readonly GenericRepository<FoxDocumentType> _foxdocumenttypeRepository;
        private readonly GenericRepository<Zip_City_State> _zipCityStateRepository;
        private readonly DbContextCommon _DbContextCommon = new DbContextCommon();
        public PatientService()
        {
            _NewPatientRepository = new GenericRepository<Patient>(_NewPatientContext);
            _PatientRepository = new GenericRepository<Patient>(_PatientContext);
            _PatientPHR = new GenericRepository<PHR>(_PatientContext);
            _PatientAddressRepository = new GenericRepository<PatientAddress>(_PatientContext);
            _PatientInsuranceRepository = new GenericRepository<PatientInsurance>(_PatientContext);
            _PatientUpdateHistoryRepository = new GenericRepository<PatientUpdateHistory>(_PatientContext);
            _PatientPOSLocationRepository = new GenericRepository<PatientPOSLocation>(_PatientContext);
            _FacilityLocationRepository = new GenericRepository<FacilityLocation>(_PatientContext);
            _FacilityTypeRepository = new GenericRepository<FacilityType>(_PatientContext);
            _OrderingRefSourceRepository = new GenericRepository<ReferralSource>(_PatientContext);
            _PatientContactRepository = new GenericRepository<PatientContact>(_PatientContext);
            _AF_TBL_PATIENT_NEXT_OF_KINRepository = new GenericRepository<AF_TBL_PATIENT_NEXT_OF_KIN>(_PatientContext);
            _WebehrTblPatientCareTeamsRepository = new GenericRepository<WebehrTblPatientCareTeams>(_PatientContext);
            _ContactTypeRepository = new GenericRepository<ContactType>(_PatientContext);
            _SubscriberRepository = new GenericRepository<Subscriber>(_PatientContext);
            _MedicareLimitTypeRepository = new GenericRepository<MedicareLimitType>(_PatientContext);
            _MedicareLimitRepository = new GenericRepository<MedicareLimit>(_PatientContext);
            _EmployerRepository = new GenericRepository<Employer>(_PatientContext);
            //_FoxInsurancePayorsRepository = new GenericRepository<FoxInsurancePayors>(_PatientContext);
            _vwPatientCaseRepository = new GenericRepository<FOX_VW_CASE>(_CaseContext);
            _FoxTblPatientRepository = new GenericRepository<FOX_TBL_PATIENT>(_PatientContext);
            _PatientAuthorizationRepository = new GenericRepository<FOX_TBL_AUTH>(_PatientContext);
            _AppointmentTypeRepository = new GenericRepository<FOX_TBL_APPT_TYPE>(_PatientContext);
            _AuthorizationAppointmentRepository = new GenericRepository<FOX_TBL_AUTH_APPT_TYPE>(_PatientContext);
            _AuthorizationChargesRepository = new GenericRepository<FOX_TBL_AUTH_CHARGES>(_PatientContext);
            _AuthorizationDocumentRepository = new GenericRepository<FOX_TBL_AUTH_DOC>(_PatientContext);
            _AuthorizationStatusRepository = new GenericRepository<FOX_TBL_AUTH_STATUS>(_PatientContext);
            _AuthorizationValueTypeRepository = new GenericRepository<FOX_TBL_AUTH_VALUE_TYPE>(_PatientContext);
            _NotesRepository = new GenericRepository<FOX_TBL_NOTES>(_PatientContext);
            _NotesTypeRepository = new GenericRepository<FOX_TBL_NOTES_TYPE>(_PatientContext);
            _workQueueRepository = new GenericRepository<OriginalQueue>(_PatientContext);
            _eligHtmlRepository = new GenericRepository<FOX_TBL_ELIG_HTML>(_PatientContext);
            _financialClassRepository = new GenericRepository<FinancialClass>(_PatientContext);
            _ReconcileDemoRepository = new GenericRepository<ReconcileDemographics>(_PatientContext);
            __InterfaceSynchModelRepository = new GenericRepository<InterfaceSynchModel>(_CaseContext);
            _foxInsurancePayersRepository = new GenericRepository<FoxInsurancePayers>(_PatientContext);
            _caseRepository = new GenericRepository<FOX_TBL_CASE>(_CaseContext);
            _claimInsuranceRepository = new GenericRepository<ClaimInsurance>(_PatientClaimContext);
            _claimNotesRepository = new GenericRepository<ClaimNotes>(_PatientClaimContext);
            _PatientPRDiscountRepository = new GenericRepository<PatientPRDiscount>(_PatientContext);
            _PatientPRPeriodRepository = new GenericRepository<PatientPRPeriod>(_PatientContext);
            _commonService = new CommonServices.CommonServices();
            _caseStatusRepository = new GenericRepository<FOX_TBL_CASE_STATUS>(_CaseContext);
            _taskTypeRepository = new GenericRepository<FOX_TBL_TASK_TYPE>(_TaskContext);
            _TaskRepository = new GenericRepository<FOX_TBL_TASK>(_TaskContext);
            _TaskTaskSubTypeRepository = new GenericRepository<FOX_TBL_TASK_TASK_SUB_TYPE>(_TaskContext);
            _taskLogRepository = new GenericRepository<TaskLog>(_TaskContext);
            _taskSubTypeRepository = new GenericRepository<FOX_TBL_TASK_SUB_TYPE>(_TaskContext);
            //MTBC Table Patient_Insurance
            _MTBCPatientInsuranceRepository = new GenericRepository<MTBCPatientInsurance>(_DBContextMTBCPatientInsurance);
            _FoxProviderClassRepository = new GenericRepository<FoxProviderClass>(_dbContextSettings);
            _FoxReferralRegionRepository = new GenericRepository<ReferralRegion>(_dbContextSettings);
            _talkEHRTblAdditionalPatientInfoRepository = new GenericRepository<Patient_Additional_Info_TalkEHR>(_PatientContext);
            _webEHRTblPatientAddressRepository = new GenericRepository<Patient_Address_History_WebEHR>(_PatientContext);
            //Notes & Alerts
            _noteAlertRepository = new GenericRepository<NoteAlert>(_PatientContext);
            _alertTypeRepository = new GenericRepository<AlertType>(_PatientContext);
            //Alias
            _patientAliasRepository = new GenericRepository<PatientAlias>(_PatientContext);
            _CountryRepository = new GenericRepository<CountryResponse>(_PatientContext);
            _foxPatientPATdocumentRepository = new GenericRepository<PatientPATDocument>(_PatientPATDocumentContext);
            _foxPatientdocumentFilesRepository = new GenericRepository<PatientDocumentFiles>(_PatientPATDocumentContext);
            _OriginalQueueFilesRepository = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            _foxdocumenttypeRepository = new GenericRepository<FoxDocumentType>(_IndexinfoContext);
            _zipCityStateRepository = new GenericRepository<Zip_City_State>(_DbContextCommon);
        }

        public Patient AddUpdatePatient(Patient patient, UserProfile profile)
        {
            if (patient.PCP != null && patient.PCP != 0)
            {
                var pcp = _OrderingRefSourceRepository.GetByID(patient.PCP);
                if (pcp != null)
                {
                    patient.Referring_Physician = pcp.REFERRAL_CODE;
                }
            }
            //-------------
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            if (!string.IsNullOrEmpty(patient.Date_Of_Birth_In_String))
            {
                patient.Date_Of_Birth = Convert.ToDateTime(patient.Date_Of_Birth_In_String);
            }

            if (!string.IsNullOrEmpty(patient.Expiry_Date_In_Str))
            {
                patient.Expiry_Date = Convert.ToDateTime(patient.Expiry_Date_In_Str);
            }

            var dbPatient = _PatientRepository.GetByID(patient.Patient_Account);
            var dbFoxPatient = _FoxTblPatientRepository.GetFirst(x => x.Patient_Account == patient.Patient_Account && !(x.DELETED ?? false));
            if (dbPatient == null) //add
            {
                interfaceSynch.PATIENT_ACCOUNT = patient.Patient_Account = Convert.ToInt64(profile.PracticeCode.ToString() + Helper.getMaximumId("Patient_Account").ToString());
                patient.Practice_Code = profile.PracticeCode;
                patient.Created_By = patient.Modified_By = profile.UserName;
                patient.Created_Date = patient.Modified_Date = Helper.GetCurrentDate();
                patient.DELETED = false;
                patient.cell_phone = String.IsNullOrEmpty(patient.cell_phone) ? "" : patient.cell_phone.Trim();
                _PatientRepository.Insert(patient);
                if (patient.PCP != null && profile.isTalkRehab)
                {
                    if (patient.PCP_Name != null)
                    {
                        CreateUpdateCareTeam(patient, profile, "", "");
                    }
                }
                _PatientRepository.Save();
                //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                //if(patient.InterfacePatient==true)
                //{
                //    InsertInterfaceTeamData(interfaceSynch, profile);
                //}
                if (patient.Patient_POS_Location_List != null && patient.Patient_POS_Location_List.Count > 0 && patient.FROM_INDEXINFO)
                    AddPatientPOS(patient.Patient_POS_Location_List, profile, patient.Patient_Account);
                //add Alias Patient
                if (patient?.Patient_Alias_List?.Count > 0)
                {
                    for (int i = 0; i < patient.Patient_Alias_List.Count; i++)
                    {
                        patient.Patient_Alias_List[i].PATIENT_ACCOUNT_STR = patient.Patient_Account.ToString();
                        SavePatientAlias(patient.Patient_Alias_List[i], profile);
                    }
                }

                if (patient.Patient_Contacts_List.Count > 0)
                {
                    for (int i = 0; i < patient.Patient_Contacts_List.Count; i++)
                    {
                        patient.Patient_Contacts_List[i].Patient_Account = patient.Patient_Account;
                        SaveContact(patient.Patient_Contacts_List[i], profile);
                    }
                }
                if (patient.IsRegister != true)
                {
                    AddUpdatePatientAddress(patient.Patient_Address, profile, patient.Patient_Account);
                    AddUpdatePatientInsurance(patient.PatientInsurance, profile, patient.Patient_Account);
                }

                SaveRestOfPatientDetails(patient, profile.UserName);
            }
            else //Update
            {
                interfaceSynch.PATIENT_ACCOUNT = long.Parse(patient.Patient_AccountStr);

                if (patient.First_Name == null)
                    patient.First_Name = "";
                if (dbPatient.First_Name == null)
                    dbPatient.First_Name = "";
                if (!String.Equals(patient.First_Name.Trim(), dbPatient.First_Name.Trim(), StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "First Name", dbPatient.First_Name, patient.First_Name, profile.UserName);
                    dbPatient.First_Name = patient.First_Name;
                    patient.is_Change = true;
                }
                if (patient.First_Name == null)
                    patient.First_Name = "";
                if (dbPatient.First_Name == null)
                    dbPatient.First_Name = "";
                if (!String.Equals(patient.Last_Name.Trim(), dbPatient.Last_Name.Trim(), StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Last Name", dbPatient.Last_Name, patient.Last_Name, profile.UserName);
                    dbPatient.Last_Name = patient.Last_Name;
                    patient.is_Change = true;
                }
                if (patient.MIDDLE_NAME == null)
                    patient.MIDDLE_NAME = "";
                if (dbPatient.MIDDLE_NAME == null)
                    dbPatient.MIDDLE_NAME = "";
                if (!String.Equals(patient.MIDDLE_NAME.Trim(), dbPatient.MIDDLE_NAME.Trim(), StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Middle Name", dbPatient.MIDDLE_NAME, patient.MIDDLE_NAME, profile.UserName);
                    dbPatient.MIDDLE_NAME = patient.MIDDLE_NAME;
                    patient.is_Change = true;
                }
                if (patient.SSN == null)
                    patient.SSN = "";
                if (dbPatient.SSN == null)
                    dbPatient.SSN = "";
                if (!String.Equals(patient.SSN.Trim(), dbPatient.SSN.Trim(), StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "SSN", dbPatient.SSN, patient.SSN, profile.UserName);
                    dbPatient.SSN = patient.SSN;
                    patient.is_Change = true;
                }
                if (patient.Date_Of_Birth != dbPatient.Date_Of_Birth)
                {
                    var olddob = dbPatient.Date_Of_Birth.HasValue ? dbPatient.Date_Of_Birth.Value.Date.ToString("MM/dd/yyyy") : "";
                    var newdob = patient.Date_Of_Birth.HasValue ? patient.Date_Of_Birth.Value.Date.ToString("MM/dd/yyyy") : "";
                    CreatePatientUpdateHistory(patient.Patient_Account, "Date Of Brith", olddob, newdob, profile.UserName);
                    dbPatient.Date_Of_Birth = patient.Date_Of_Birth;
                    patient.is_Change = true;
                }
                if (patient.Expiry_Date != dbPatient.Expiry_Date)
                {
                    var oldd = dbPatient.Expiry_Date.HasValue ? dbPatient.Expiry_Date.Value.Date.ToString("MM/dd/yyyy") : "";
                    var newd = patient.Expiry_Date.HasValue ? patient.Expiry_Date.Value.Date.ToString("MM/dd/yyyy") : "";
                    CreatePatientUpdateHistory(patient.Patient_Account, "Expiry_Date", oldd, newd, profile.UserName);
                    dbPatient.Date_Of_Birth = patient.Date_Of_Birth;
                    patient.is_Change = true;
                }
                if (patient.Gender == null)
                    patient.Gender = "";
                if (dbPatient.Gender == null)
                    dbPatient.Gender = "";
                if (!String.Equals(patient.Gender.Trim(), dbPatient.Gender.Trim(), StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Gender", dbPatient.Gender, patient.Gender, profile.UserName);
                    dbPatient.Gender = patient.Gender;
                    patient.is_Change = true;
                }
                if (patient.Home_Phone == null)
                    patient.Home_Phone = "";
                if (dbPatient.Home_Phone == null)
                    dbPatient.Home_Phone = "";
                if (!String.Equals(patient.Home_Phone, dbPatient.Home_Phone, StringComparison.Ordinal))  //&& !patient.IsHomePhoneFromSLC
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Home Phone", dbPatient.Home_Phone, patient.Home_Phone, profile.UserName);
                    dbPatient.Home_Phone = patient.Home_Phone;
                    patient.is_Change = true;
                }
                if (patient.Business_Phone == null)
                    patient.Business_Phone = "";
                if (dbPatient.Business_Phone == null)
                    dbPatient.Business_Phone = "";
                if (!String.Equals(patient.Business_Phone, dbPatient.Business_Phone, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Work Phone", dbPatient.Business_Phone, patient.Business_Phone, profile.UserName);
                    dbPatient.Business_Phone = patient.Business_Phone;
                    patient.is_Change = true;
                }
                if (patient.cell_phone == null)
                    patient.cell_phone = "";
                if (dbPatient.cell_phone == null)
                    dbPatient.cell_phone = "";
                if (!String.Equals(patient.cell_phone, dbPatient.cell_phone, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Mobile Phone", dbPatient.cell_phone, patient.cell_phone, profile.UserName);
                    dbPatient.cell_phone = patient.cell_phone;
                    patient.is_Change = true;
                }
                if (patient.Chart_Id == null)
                    patient.Chart_Id = "";
                if (dbPatient.Chart_Id == null)
                    dbPatient.Chart_Id = "";
                if (!String.Equals(patient.Chart_Id, dbPatient.Chart_Id, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Medical Record Number", dbPatient.Chart_Id, patient.Chart_Id, profile.UserName);
                    dbPatient.Chart_Id = patient.Chart_Id;
                    patient.is_Change = true;
                }
                if (patient.Email_Address == null)
                    patient.Email_Address = "";
                if (dbPatient.Email_Address == null)
                    dbPatient.Email_Address = "";
                if (!String.Equals(patient.Email_Address, dbPatient.Email_Address, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Email", dbPatient.Email_Address, patient.Email_Address, profile.UserName);
                    dbPatient.Email_Address = patient.Email_Address;
                    patient.is_Change = true;
                }

                if (!String.Equals(patient.Best_Time_of_Call_Cell, dbPatient.Best_Time_of_Call_Cell, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Best_Time_of_Call_Cell", dbPatient.Best_Time_of_Call_Cell, patient.Best_Time_of_Call_Cell, profile.UserName);
                    dbPatient.Best_Time_of_Call_Cell = patient.Best_Time_of_Call_Cell;
                }

                if (!String.Equals(patient.Best_Time_of_Call_Work, dbPatient.Best_Time_of_Call_Work, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Best_Time_of_Call_Work", dbPatient.Best_Time_of_Call_Work, patient.Best_Time_of_Call_Work, profile.UserName);
                    dbPatient.Best_Time_of_Call_Work = patient.Best_Time_of_Call_Work;
                }

                if (!String.Equals(patient.Best_Time_of_Call_Home, dbPatient.Best_Time_of_Call_Home, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Best_Time_of_Call_Home", dbPatient.Best_Time_of_Call_Home, patient.Best_Time_of_Call_Home, profile.UserName);
                    dbPatient.Best_Time_of_Call_Home = patient.Best_Time_of_Call_Home;
                }
                if (dbFoxPatient != null)
                {
                    if (patient.Fax_Number == null)
                        patient.Fax_Number = "";
                    if (dbFoxPatient.Fax_Number == null)
                        dbFoxPatient.Fax_Number = "";
                    if (!String.Equals(patient.Fax_Number.Trim(), dbFoxPatient.Fax_Number.Trim(), StringComparison.Ordinal))
                    {
                        CreatePatientUpdateHistory(patient.Patient_Account, "Fax_Number", dbPatient.Fax_Number, patient.Fax_Number, profile.UserName);
                        dbPatient.Fax_Number = dbFoxPatient.Fax_Number = patient.Fax_Number;
                        patient.is_Change = true;
                    }
                }
                if (dbFoxPatient != null)
                {
                    if (patient.Title == null)
                        patient.Title = "";
                    if (dbFoxPatient.Title == null)
                        dbFoxPatient.Title = "";
                    if (!String.Equals(patient.Title.Trim(), dbFoxPatient.Title.Trim(), StringComparison.Ordinal))
                    {
                        CreatePatientUpdateHistory(patient.Patient_Account, "Title", dbPatient.Title, patient.Title, profile.UserName);
                        dbPatient.Title = dbFoxPatient.Title = patient.Title;
                        patient.is_Change = true;
                    }
                }

                if (patient.PCP != dbPatient.PCP)
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "PCP", (dbPatient.PCP != null ? dbPatient.PCP.ToString() : ""), (patient.PCP != null ? patient.PCP.ToString() : ""), profile.UserName);
                    dbPatient.PCP = patient.PCP == 0 ? null : patient.PCP;
                }

                if (!String.Equals(patient.Employment_Status, dbPatient.Employment_Status, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Employment_Status", dbPatient.Employment_Status, patient.Employment_Status, profile.UserName);
                    dbPatient.Employment_Status = patient.Employment_Status;
                }

                if (!String.Equals(patient.Student_Status, dbPatient.Student_Status, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Student_Status", dbPatient.Student_Status, patient.Student_Status, profile.UserName);
                    dbPatient.Student_Status = patient.Student_Status;
                }

                if (!String.Equals(patient.Marital_Status, dbPatient.Marital_Status, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Marital_Status", dbPatient.Marital_Status, patient.Marital_Status, profile.UserName);
                    dbPatient.Marital_Status = patient.Marital_Status;
                }

                if (patient.FINANCIAL_CLASS_ID != dbPatient.FINANCIAL_CLASS_ID)
                {
                    var oldFinClassCode = "";
                    var newFinClassCode = "";
                    if (dbPatient.FINANCIAL_CLASS_ID.HasValue && dbPatient.FINANCIAL_CLASS_ID.Value != 0)
                    {
                        oldFinClassCode = _financialClassRepository.GetByID(dbPatient.FINANCIAL_CLASS_ID).CODE;
                    }
                    if (patient.FINANCIAL_CLASS_ID.HasValue && patient.FINANCIAL_CLASS_ID.Value != 0)
                    {
                        newFinClassCode = _financialClassRepository.GetByID(patient.FINANCIAL_CLASS_ID).CODE;
                    }
                    CreatePatientUpdateHistory(patient.Patient_Account, "Financial_Class", oldFinClassCode, newFinClassCode, profile.UserName);
                    dbPatient.FINANCIAL_CLASS_ID = patient.FINANCIAL_CLASS_ID;
                }

                if (dbPatient.Expired != patient.Expired)
                {
                    var dbVal = dbPatient.Expired ?? false;
                    var newVal = patient.Expired ?? false;
                    CreatePatientUpdateHistory(patient.Patient_Account, "Expired", dbVal ? "True" : "False", newVal ? "True" : "False", profile.UserName);
                    if ((patient.Expired ?? false))
                    {
                        dbPatient.Expired = true;
                        dbPatient.Expiry_Date = Helper.GetCurrentDate();
                    }
                    else
                    {
                        dbPatient.Expired = false;
                        dbPatient.Expiry_Date = null;
                    }
                }

                if (patient.Expired ?? false)
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Patient_Status", dbPatient.Patient_Status, "Expired", profile.UserName);
                    dbPatient.Patient_Status = "Expired";
                }
                else
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Patient_Status", dbPatient.Patient_Status, "Inactive", profile.UserName);
                    dbPatient.Patient_Status = "Inactive";
                }

                dbPatient.City = patient.City;
                dbPatient.State = patient.State;
                dbPatient.ZIP = patient.ZIP;
                dbPatient.Address = patient.Address;
                dbPatient.Date_Of_Birth = patient.Date_Of_Birth;
                dbPatient.Expiry_Date = patient.Expiry_Date;

                dbPatient.Modified_By = profile.UserName;
                dbPatient.Modified_Date = Helper.GetCurrentDate();
                //  if (patient.is_Change)
                //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                //if(patient.InterfacePatient==true)
                //{
                //    InsertInterfaceTeamData(interfaceSynch, profile);
                //}

                _PatientRepository.Update(dbPatient);
                patient.is_Change = false;

                if (dbPatient.Referring_Physician != patient.Referring_Physician)
                {
                    CreatePatientUpdateHistory(patient.Patient_Account, "Referring_Physician", (dbPatient.Referring_Physician.HasValue ? dbPatient.Referring_Physician.ToString() : ""), (patient.Referring_Physician.HasValue ? patient.Referring_Physician.ToString() : ""), profile.UserName);
                    dbPatient.Referring_Physician = patient.Referring_Physician;
                }
                if (patient.PCP != null && profile.isTalkRehab)
                {
                    if (patient.PCP_Name != null)
                    {
                        var splitPCPName = patient.PCP_Name.Split(',');
                        var splitPCPFName = splitPCPName[0];
                        var splitPCPLName = splitPCPName[1].Split('|')[0].Trim();

                        CreateUpdateCareTeam(patient, profile, splitPCPFName, splitPCPLName);
                    }
                }
                _PatientRepository.Save();

                if (patient.Patient_POS_Location_List != null && patient.Patient_POS_Location_List.Count > 0 && patient.FROM_INDEXINFO)
                    AddPatientPOS(patient.Patient_POS_Location_List, profile, patient.Patient_Account);
                //edit Alias Patient
                if (patient?.Patient_Alias_List != null && patient?.Patient_Alias_List.Count > 0)
                {
                    for (int i = 0; i < patient.Patient_Alias_List.Count; i++)
                    {
                        patient.Patient_Alias_List[i].PATIENT_ACCOUNT_STR = patient.Patient_Account.ToString();
                        SavePatientAlias(patient.Patient_Alias_List[i], profile);
                    }
                }
                if (patient.Patient_Contacts_List != null && patient.Patient_Contacts_List.Count > 0)
                {
                    for (int i = 0; i < patient.Patient_Contacts_List.Count; i++)
                    {
                        patient.Patient_Contacts_List[i].Patient_Account = patient.Patient_Account;
                        SaveContact(patient.Patient_Contacts_List[i], profile);
                    }
                }

                if (patient.Patient_Address != null && patient.Patient_Address.Count > 0)
                    AddUpdatePatientAddress(patient.Patient_Address, profile, patient.Patient_Account);

                if (patient.PatientInsurance != null && patient.PatientInsurance.Count > 0)
                    AddUpdatePatientInsurance(patient.PatientInsurance, profile, patient.Patient_Account);

                if (patient.PlaceOfServicesToDeleteIds != null && patient.PlaceOfServicesToDeleteIds.Count > 0)
                {
                    foreach (var patientPos in patient.PlaceOfServicesToDeleteIds)
                    {
                        var patientPosLocId = Convert.ToInt64(patientPos);
                        var patientPosToDelete = _PatientPOSLocationRepository.GetFirst(p => p.Patient_Account == patient.Patient_Account && !(p.Deleted ?? false) && p.Loc_ID == patientPosLocId);
                        if (patientPosToDelete != null)
                        {
                            patientPosToDelete.Deleted = true;
                            DeletePatPos(patientPosToDelete, profile);
                        }
                    }
                }

                SaveRestOfPatientDetails(patient, profile.UserName);
            }

            return patient;
        }

        private void SaveRestOfPatientDetails(Patient patient, string username)
        {
            var restOfPatientData = new FOX_TBL_PATIENT();
            var data = _FoxTblPatientRepository.GetFirst(e => e.Patient_Account == patient.Patient_Account);
            if (data == null)
            {
                restOfPatientData.FOX_TBL_PATIENT_ID = Helper.getMaximumId("FOX_TBL_PATIENT");
                restOfPatientData.Patient_Account = patient.Patient_Account;
                restOfPatientData.Created_By = restOfPatientData.Modified_By = username;
                restOfPatientData.Created_Date = restOfPatientData.Modified_Date = Helper.GetCurrentDate();
                restOfPatientData.DELETED = false;
                restOfPatientData.PRACTICE_ORGANIZATION_ID = patient?.PRACTICE_ORGANIZATION_ID;
            }
            else
            {
                restOfPatientData = data;
                restOfPatientData.Modified_By = username;
                restOfPatientData.Modified_Date = Helper.GetCurrentDate();
            }

            restOfPatientData.Title = patient.Title;
            restOfPatientData.Best_Time_of_Call_Home = patient.Best_Time_of_Call_Home;
            restOfPatientData.Best_Time_of_Call_Work = patient.Best_Time_of_Call_Work;
            restOfPatientData.Best_Time_of_Call_Cell = patient.Best_Time_of_Call_Cell;
            restOfPatientData.Fax_Number = patient.Fax_Number;
            restOfPatientData.PCP = patient.PCP;
            if (restOfPatientData.PCP != null && restOfPatientData.PCP != 0)
            {
                UpdatePrimaryPhysicianInCases(restOfPatientData.PCP, patient.Patient_Account, patient.Practice_Code);
            }
            restOfPatientData.Employment_Status = patient.Employment_Status;
            restOfPatientData.Patient_Status = patient.Patient_Status;
            restOfPatientData.Student_Status = patient.Student_Status;
            restOfPatientData.FINANCIAL_CLASS_ID = patient.FINANCIAL_CLASS_ID;
            restOfPatientData.Expired = patient.Expired;
            restOfPatientData.POA_EMERGENCY_CONTACT = patient.POA_EMERGENCY_CONTACT;

            if (data == null)
            {
                _FoxTblPatientRepository.Insert(restOfPatientData);
            }
            else
            {
                _FoxTblPatientRepository.Update(restOfPatientData);
            }

            _FoxTblPatientRepository.Save();

            SaveAdditionalInfoInTalkEHR(patient, username);
            //UpdateClaimsExpiry();
        }

        private void UpdateClaimsExpiry(long pat_Acc, string username)
        {
            //Wating for Shan's reply
        }

        private void SaveAdditionalInfoInTalkEHR(Patient patient, string username)
        {
            var additionalPatientData = new Patient_Additional_Info_TalkEHR();
            var talkEHRTablePatientdata = _talkEHRTblAdditionalPatientInfoRepository.GetFirst(e => e.Patient_Account == patient.Patient_Account);
            if (talkEHRTablePatientdata == null)
            {
                additionalPatientData.PATIENT_ADDITIONAL_INFO_ID = Helper.getMaximumId("PATIENT_ADDITIONAL_INFO_ID");
                additionalPatientData.Patient_Account = patient.Patient_Account;
                additionalPatientData.CREATED_BY = additionalPatientData.MODIFIED_BY = username;
                additionalPatientData.CREATED_DATE = additionalPatientData.MODIFIED_DATE = Helper.GetCurrentDate();
                additionalPatientData.DELETED = false;
                additionalPatientData.PRACTICE_ORGANIZATION_ID = patient?.PRACTICE_ORGANIZATION_ID;
            }
            else
            {
                additionalPatientData = talkEHRTablePatientdata;
                additionalPatientData.MODIFIED_BY = username;
                additionalPatientData.MODIFIED_DATE = Helper.GetCurrentDate();
            }

            additionalPatientData.Title = patient.Title;
            additionalPatientData.Best_Time_of_Call_Home = patient.Best_Time_of_Call_Home;
            additionalPatientData.Best_Time_of_Call_Work = patient.Best_Time_of_Call_Work;
            additionalPatientData.Best_Time_of_Call_Cell = patient.Best_Time_of_Call_Cell;
            additionalPatientData.Fax_Number = patient.Fax_Number;
            additionalPatientData.Employment_Status = patient.Employment_Status;
            additionalPatientData.Patient_Status = patient.Patient_Status;
            additionalPatientData.Student_Status = patient.Student_Status;
            additionalPatientData.FINANCIAL_CLASS_ID = patient.FINANCIAL_CLASS_ID;
            additionalPatientData.Expired = patient.Expired;


            if (talkEHRTablePatientdata == null)
            {
                _talkEHRTblAdditionalPatientInfoRepository.Insert(additionalPatientData);
            }
            else
            {
                _talkEHRTblAdditionalPatientInfoRepository.Update(additionalPatientData);
            }

            _talkEHRTblAdditionalPatientInfoRepository.Save();
        }


        private void AddPatientPOS(List<PatientPOSLocation> posLocationList, UserProfile profile, long patientAccount)
        {
            if (posLocationList != null && posLocationList.Count > 0)
            {
                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                interfaceSynch.PATIENT_ACCOUNT = patientAccount;
                var existingPOSLocations = _PatientPOSLocationRepository.GetMany(e => e.Patient_Account == patientAccount && !(e.Deleted ?? false)).ToList();
                foreach (var loc in posLocationList)
                {
                    if (loc.Loc_ID != 0)
                    {
                        DateTime dateTimeTo;
                        DateTime dateTimeFrom;
                        if (DateTime.TryParse(loc.Effective_To_In_String, out dateTimeTo))
                        {
                            loc.Effective_To = dateTimeTo;
                        }
                        if (DateTime.TryParse(loc.Effective_From_In_String, out dateTimeFrom))
                        {
                            loc.Effective_From = dateTimeFrom;
                        }
                        //this commented temporarygiving error in this method 03/06/2019
                        UpdateCoordinates(loc, profile);
                        int index = existingPOSLocations.FindIndex(item => item.Loc_ID == loc.Loc_ID);
                        if (loc.Patient_POS_Details != null && loc.Patient_POS_Details.CODE != null)
                        {
                            if (index < 0 || (loc.Patient_POS_Details.CODE.ToLower().Contains("hom")) && index < 0)//Insert
                            {
                                loc.Patient_POS_ID = Helper.getMaximumId("Fox_Patient_POS_ID");
                                loc.Patient_Account = patientAccount;
                                loc.Created_By = profile.UserName;
                                loc.Modified_By = profile.UserName;
                                loc.Created_Date = Helper.GetCurrentDate();
                                loc.Modified_Date = Helper.GetCurrentDate();
                                loc.Deleted = false;
                                _PatientPOSLocationRepository.Insert(loc);
                                _PatientPOSLocationRepository.Save();

                                var facilityType = (from f in _PatientContext.FacilityLocation
                                                    join ft in _PatientContext.FaclityTypes on f.FACILITY_TYPE_ID equals ft.FACILITY_TYPE_ID
                                                    //join ftt in _PatientContext.FaclityTypes on f.PRACTICE_CODE equals ftt.PRACTICE_CODE
                                                    where f.LOC_ID == loc.Loc_ID && ft.PRACTICE_CODE == profile.PracticeCode && !ft.DELETED
                                                    select ft).FirstOrDefault();

                                if (facilityType != null && facilityType.NAME?.ToLower() == "private home")
                                {
                                    //this commented temporarygiving error in this method 03/06/2019
                                    POSCoordinates coordinates = GetCoordinates(loc.Patient_POS_Details);
                                    PatientAddress address = new PatientAddress()
                                    {
                                        PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID"),
                                        PATIENT_ACCOUNT = patientAccount,
                                        ADDRESS = loc.Patient_POS_Details.Address,
                                        ZIP = loc.Patient_POS_Details.Zip == null ? null : loc.Patient_POS_Details.Zip.Replace(" - ", ""),
                                        CITY = loc.Patient_POS_Details.City,
                                        STATE = loc.Patient_POS_Details.State,
                                        CREATED_BY = profile.UserName,
                                        CREATED_DATE = Helper.GetCurrentDate(),
                                        MODIFIED_BY = profile.UserName,
                                        MODIFIED_DATE = Helper.GetCurrentDate(),
                                        DELETED = false,
                                        PATIENT_POS_ID = loc.Patient_POS_ID,
                                        POS_Phone = loc.Patient_POS_Details.Phone,
                                        POS_Work_Phone = loc.Patient_POS_Details.Work_Phone,
                                        POS_Cell_Phone = loc.Patient_POS_Details.Cell_Phone,
                                        POS_Fax = loc.Patient_POS_Details.Fax,
                                        POS_Email_Address = loc.Patient_POS_Details.Email_Address,
                                        POS_REGION = loc.Patient_POS_Details.REGION,
                                        POS_County = loc.Patient_POS_Details.Country
                                        //ngitude = coordinates.Longitude
                                    };
                                    //this commented temporarygiving error in this method 03/06/2019
                                    if (coordinates != null)
                                    {
                                        if (coordinates.Latitude != null)
                                        {
                                            address.Latitude = Convert.ToSingle(coordinates.Latitude);
                                        }
                                        if (coordinates.Longitude != null)
                                        {
                                            address.Longitude = Convert.ToSingle(coordinates.Longitude);
                                        }
                                    }

                                    address = SaveAddressInWebEHRTable(true, address);

                                    _PatientAddressRepository.Insert(address);
                                    _PatientAddressRepository.Save();

                                }
                                else if (facilityType != null)
                                {
                                    PatientAddress address = new PatientAddress()
                                    {
                                        PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID"),
                                        PATIENT_ACCOUNT = patientAccount,
                                        ADDRESS = loc.Patient_POS_Details.Address,
                                        ZIP = loc.Patient_POS_Details.Zip == null ? null : loc.Patient_POS_Details.Zip.Replace("-", ""),
                                        CITY = loc.Patient_POS_Details.City,
                                        STATE = loc.Patient_POS_Details.State,
                                        CREATED_BY = profile.UserName,
                                        CREATED_DATE = Helper.GetCurrentDate(),
                                        MODIFIED_BY = profile.UserName,
                                        MODIFIED_DATE = Helper.GetCurrentDate(),
                                        DELETED = false,
                                        PATIENT_POS_ID = loc.Patient_POS_ID,
                                        //POS_Phone = loc.Patient_POS_Details.Phone,
                                        //POS_Work_Phone = loc.Patient_POS_Details.Work_Phone,
                                        //POS_Cell_Phone = loc.Patient_POS_Details.Cell_Phone,
                                        //POS_Fax = loc.Patient_POS_Details.Fax,
                                        //POS_Email_Address = loc.Patient_POS_Details.Email_Address,
                                        //POS_REGION = loc.Patient_POS_Details.REGION,
                                        //POS_County = loc.Patient_POS_Details.Country
                                        //ngitude = coordinates.Longitude
                                    };
                                    _PatientAddressRepository.Insert(address);
                                    _PatientAddressRepository.Save();
                                }
                                UpdateDefaultLocations(loc.Loc_ID, patientAccount, loc.Patient_POS_ID);
                                //if (facilityType.NAME?.ToLower())
                                SaveGuarantorForPatientFromPOS(loc, profile);
                            }
                            if (index == 0 && (loc.Patient_POS_Details.CODE.ToLower().Contains("hom"))) // update
                            {
                                var posT = existingPOSLocations.Find(item => item.Loc_ID == loc.Loc_ID);

                                posT.Modified_By = profile.UserName;
                                posT.Modified_Date = Helper.GetCurrentDate();
                                posT.Deleted = false;

                                _PatientPOSLocationRepository.Update(posT);
                                _PatientPOSLocationRepository.Save();

                                var AddressT = _PatientAddressRepository.GetFirst(x => x.PATIENT_ACCOUNT == patientAccount && x.PATIENT_POS_ID == posT.Patient_POS_ID && !(x.DELETED ?? false));
                                if (AddressT != null)
                                {
                                    AddressT = SaveAddressInWebEHRTable(false, AddressT);

                                    AddressT.ADDRESS = loc.Patient_POS_Details.Address;
                                    AddressT.ZIP = loc.Patient_POS_Details.Zip.Replace("-", "") == null ? null : loc.Patient_POS_Details.Zip.Replace("-", "");
                                    AddressT.CITY = loc.Patient_POS_Details.City;
                                    AddressT.STATE = loc.Patient_POS_Details.State;
                                    AddressT.POS_REGION = loc.Patient_POS_Details.REGION;
                                    AddressT.POS_County = loc.Patient_POS_Details.Country;

                                    _PatientAddressRepository.Update(AddressT);
                                    _PatientAddressRepository.Save();
                                }
                                loc.Patient_Account = patientAccount;
                                UpdateDefaultLocations(loc.Loc_ID, patientAccount, posT.Patient_POS_ID);
                                SaveGuarantorForPatientFromPOS(loc, profile);
                            }
                        }
                    }
                }
            }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        private void SaveGuarantorForPatientFromPOS(PatientPOSLocation loc, UserProfile profile)
        {
            var patient = _PatientRepository.GetFirst(e => e.Patient_Account == loc.Patient_Account && (e.DELETED ?? false) == false);
            if (!string.IsNullOrWhiteSpace(loc.Patient_POS_Details.NAME) && !loc.Patient_POS_Details.NAME.ToLower().Equals("private home"))
            {
                if (patient != null && !string.IsNullOrWhiteSpace(loc.Patient_POS_Details.Address))
                {
                    bool isEdit = true;
                    var guarantor = _SubscriberRepository.GetFirst(e => e.GUARANTOR_CODE == patient.Financial_Guarantor && (e.Deleted ?? false) == false);
                    if (guarantor == null)
                    {
                        isEdit = false;
                        guarantor = new Subscriber();
                        guarantor.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                        guarantor.guarant_practice_code = profile.PracticeCode;
                        guarantor.created_by = guarantor.modified_by = profile.UserName;
                        guarantor.created_date = guarantor.modified_date = Helper.GetCurrentDate();
                        guarantor.Deleted = false;
                    }
                    else
                    {
                        guarantor.modified_by = profile.UserName;
                        guarantor.modified_date = Helper.GetCurrentDate();
                    }

                    guarantor.GUARANT_FNAME = null;
                    guarantor.GUARANT_LNAME = loc.Patient_POS_Details.NAME;
                    guarantor.GUARANT_ADDRESS = loc.Patient_POS_Details.Address;
                    guarantor.GUARANT_ZIP = loc.Patient_POS_Details.Zip;
                    guarantor.GUARANT_CITY = loc.Patient_POS_Details.City;
                    guarantor.GUARANT_STATE = loc.Patient_POS_Details.State;
                    guarantor.GUARANT_HOME_PHONE = loc.Patient_POS_Details.Phone;
                    guarantor.GUARANT_WORK_PHONE = loc.Patient_POS_Details.Work_Phone;
                    guarantor.guarant_practice_code = profile.PracticeCode;
                    guarantor.Guarant_Type = "G";

                    if (isEdit)
                    {
                        _SubscriberRepository.Update(guarantor);
                        _SubscriberRepository.Save();
                    }
                    else
                    {
                        _SubscriberRepository.Insert(guarantor);
                        _SubscriberRepository.Save();
                        patient.Financial_Guarantor = guarantor.GUARANTOR_CODE;
                        patient.Address_To_Guarantor = true;
                        patient.ModifiedBy = profile.UserName;
                        patient.Modified_Date = Helper.GetCurrentDate();
                        _PatientRepository.Update(patient);
                        _PatientRepository.Save();
                    }
                }
            }
            else
            {
                if (patient != null)
                {
                    patient.Financial_Guarantor = null;
                    patient.Address_To_Guarantor = false;
                    patient.ModifiedBy = profile.UserName;
                    patient.Modified_Date = Helper.GetCurrentDate();
                    _PatientRepository.Update(patient);
                    _PatientRepository.Save();
                }
            }
        }

        private void SaveGuarantorForPatientFromContact(PatientContact con, UserProfile profile)
        {
            //if (con != null && con.STATEMENT_ADDRESS_MARKED && ((con.Flag_Financially_Responsible_Party ?? false) || (con.Flag_Power_Of_Attorney_Financial ?? false)))
            //Change By Arqam
            if (con != null && con.STATEMENT_ADDRESS_MARKED && (con.Flag_Financially_Responsible_Party ?? false))
            {
                var patient = _PatientRepository.GetFirst(e => e.Patient_Account == con.Patient_Account && (e.DELETED ?? false) == false);

                if (patient != null)
                {
                    bool isEdit = true;
                    var guarantor = _SubscriberRepository.GetFirst(e => e.GUARANTOR_CODE == patient.Financial_Guarantor && (e.Deleted ?? false) == false);
                    if (guarantor == null)
                    {
                        isEdit = false;
                        guarantor = new Subscriber();
                        guarantor.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                        guarantor.guarant_practice_code = profile.PracticeCode;
                        guarantor.created_by = guarantor.modified_by = profile.UserName;
                        guarantor.created_date = guarantor.modified_date = Helper.GetCurrentDate();
                        guarantor.Deleted = false;
                    }
                    else
                    {
                        guarantor.modified_by = profile.UserName;
                        guarantor.modified_date = Helper.GetCurrentDate();
                    }

                    guarantor.GUARANT_FNAME = con.First_Name;
                    guarantor.GUARANT_LNAME = con.Last_Name;
                    guarantor.GUARANT_ADDRESS = con.Address;
                    guarantor.GUARANT_ZIP = con.Zip;
                    guarantor.GUARANT_CITY = con.City;
                    guarantor.GUARANT_STATE = con.State;
                    guarantor.GUARANT_HOME_PHONE = con.Home_Phone;
                    guarantor.GUARANT_WORK_PHONE = con.Work_Phone;
                    guarantor.guarant_practice_code = profile.PracticeCode;
                    guarantor.Guarant_Type = "G";

                    if (isEdit)
                    {
                        _SubscriberRepository.Update(guarantor);
                        _SubscriberRepository.Save();
                    }
                    else
                    {
                        _SubscriberRepository.Insert(guarantor);
                        _SubscriberRepository.Save();
                        patient.Financial_Guarantor = guarantor.GUARANTOR_CODE;
                        patient.Address_To_Guarantor = true;
                        patient.ModifiedBy = profile.UserName;
                        patient.Modified_Date = Helper.GetCurrentDate();
                        _PatientRepository.Update(patient);
                        _PatientRepository.Save();
                    }
                }
            }
        }

        private void SaveHomeAddressInPatient(PatientAddress address)
        {
            var patient = _NewPatientRepository.GetFirst(e => e.Patient_Account == address.PATIENT_ACCOUNT.Value && (e.DELETED ?? false) == false);
            if (patient != null)
            {
                patient.Address = address.ADDRESS;
                patient.ZIP = address.ZIP;
                patient.City = address.CITY;
                patient.State = address.STATE;
                patient.Modified_By = address.MODIFIED_BY;
                patient.Modified_Date = address.MODIFIED_DATE;
                if (EntityHelper.isTalkRehab)
                {
                    patient.Address_Type = "RESIDENTIAL ADDRESS";
                }
                _NewPatientRepository.Update(patient);
                _NewPatientRepository.Save();
            }
        }

        private PatientAddress SaveAddressInWebEHRTable(bool is_Insert, PatientAddress address)
        {
            var webEHR_Address = new Patient_Address_History_WebEHR();
            if (is_Insert)
            {
                webEHR_Address = new Patient_Address_History_WebEHR()
                {
                    PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID"),
                    PATIENT_ACCOUNT = address.PATIENT_ACCOUNT,
                    ADDRESS = address.ADDRESS,
                    ADDRESS_TYPE = address.ADDRESS_TYPE,
                    ZIP = address.ZIP,
                    CITY = address.CITY,
                    STATE = address.STATE,
                    CREATED_BY = address.CREATED_BY,
                    CREATED_DATE = address.CREATED_DATE,
                    MODIFIED_BY = address.MODIFIED_BY,
                    MODIFIED_DATE = address.MODIFIED_DATE,
                    DELETED = address.DELETED,
                    PATIENT_POS_ID = address.PATIENT_POS_ID,
                    Same_As_POS = address.Same_As_POS,
                    Phone = address.POS_Phone,
                    Work_Phone = address.POS_Work_Phone,
                    Cell_Phone = address.POS_Cell_Phone,
                    Fax = address.POS_Fax,
                    Email_Address = address.POS_Email_Address,
                    REGION = address.POS_REGION,
                    County = address.COUNTRY,
                    Latitude = address.Latitude,
                    Longitude = address.Longitude
                };

                _webEHRTblPatientAddressRepository.Insert(webEHR_Address);
                _webEHRTblPatientAddressRepository.Save();
                address.WEBEHR_PATIENT_ADDRESS_ID = webEHR_Address.PATIENT_ADDRESS_HISTORY_ID;
            }
            else
            {
                webEHR_Address = _webEHRTblPatientAddressRepository.GetFirst(e => e.PATIENT_ADDRESS_HISTORY_ID == address.WEBEHR_PATIENT_ADDRESS_ID && (e.DELETED ?? false) == false);
                if (webEHR_Address == null)
                {
                    webEHR_Address = new Patient_Address_History_WebEHR()
                    {
                        PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID"),
                        PATIENT_ACCOUNT = address.PATIENT_ACCOUNT,
                        ADDRESS = address.ADDRESS,
                        ADDRESS_TYPE = address.ADDRESS_TYPE,
                        ZIP = address.ZIP,
                        CITY = address.CITY,
                        STATE = address.STATE,
                        CREATED_BY = address.CREATED_BY,
                        CREATED_DATE = address.CREATED_DATE,
                        MODIFIED_BY = address.MODIFIED_BY,
                        MODIFIED_DATE = address.MODIFIED_DATE,
                        DELETED = address.DELETED,
                        PATIENT_POS_ID = address.PATIENT_POS_ID,
                        Same_As_POS = address.Same_As_POS,
                        Phone = address.POS_Phone,
                        Work_Phone = address.POS_Work_Phone,
                        Cell_Phone = address.POS_Cell_Phone,
                        Fax = address.POS_Fax,
                        Email_Address = address.POS_Email_Address,
                        REGION = address.POS_REGION,
                        County = address.COUNTRY,
                        Latitude = address.Latitude,
                        Longitude = address.Longitude
                    };

                    _webEHRTblPatientAddressRepository.Insert(webEHR_Address);
                    _webEHRTblPatientAddressRepository.Save();
                    address.WEBEHR_PATIENT_ADDRESS_ID = webEHR_Address.PATIENT_ADDRESS_HISTORY_ID;
                }
                else
                {
                    webEHR_Address.PATIENT_ACCOUNT = address.PATIENT_ACCOUNT;
                    webEHR_Address.ADDRESS = address.ADDRESS;
                    webEHR_Address.ZIP = address.ZIP;
                    webEHR_Address.CITY = address.CITY;
                    webEHR_Address.STATE = address.STATE;
                    webEHR_Address.CREATED_BY = address.CREATED_BY;
                    webEHR_Address.CREATED_DATE = address.CREATED_DATE;
                    webEHR_Address.MODIFIED_BY = address.MODIFIED_BY;
                    webEHR_Address.MODIFIED_DATE = address.MODIFIED_DATE;
                    webEHR_Address.DELETED = address.DELETED;
                    webEHR_Address.PATIENT_POS_ID = address.PATIENT_POS_ID;
                    webEHR_Address.Same_As_POS = address.Same_As_POS;
                    webEHR_Address.Phone = address.POS_Phone;
                    webEHR_Address.Work_Phone = address.POS_Work_Phone;
                    webEHR_Address.Cell_Phone = address.POS_Cell_Phone;
                    webEHR_Address.Fax = address.POS_Fax;
                    webEHR_Address.Email_Address = address.POS_Email_Address;
                    webEHR_Address.REGION = address.POS_REGION;
                    webEHR_Address.County = address.COUNTRY;
                    webEHR_Address.Latitude = address.Latitude;
                    webEHR_Address.Longitude = address.Longitude;

                    _webEHRTblPatientAddressRepository.Update(webEHR_Address);
                    _webEHRTblPatientAddressRepository.Save();
                }
            }
            if (address.ADDRESS_TYPE == "Home Address")
                SaveHomeAddressInPatient(address);

            return address;
        }

        private void AddUpdatePatientAddress(List<PatientAddress> patientAddress, UserProfile profile, long patientAccount)
        {
            //var _patientContext = new DbContextPatient();
            //var _patientAddressRepository = new GenericRepository<PatientAddress>(_patientContext);
            for (int i = 0; i < patientAddress.Count; i++)
            {
                var dbPatientAddress = _PatientAddressRepository.GetByID(patientAddress[i].PATIENT_ADDRESS_HISTORY_ID);
                if (dbPatientAddress == null) //add
                {
                    patientAddress[i].PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID");
                    patientAddress[i].ZIP = patientAddress[i].ZIP == null ? null : patientAddress[i].ZIP.Replace("-", "");
                    patientAddress[i].PATIENT_ACCOUNT = patientAccount;
                    patientAddress[i].CREATED_BY = patientAddress[i].MODIFIED_BY = profile.UserName;
                    patientAddress[i].CREATED_DATE = patientAddress[i].MODIFIED_DATE = Helper.GetCurrentDate();
                    patientAddress[i].DELETED = false;

                    patientAddress[i] = SaveAddressInWebEHRTable(true, patientAddress[i]);
                    _PatientAddressRepository.Insert(patientAddress[i]);
                    _PatientAddressRepository.Save();
                    if (patientAddress[i].ADDRESS_TYPE != null && patientAddress[i].ADDRESS_TYPE.ToLower() == "home address")
                    {
                        CheckAndUpdatePRSubscriber(patientAccount.ToString(), profile);
                        UpdateSubscriberForAllOtherInsurancesIfRelationSelfExceptPR(patientAccount.ToString(), profile);
                    }
                }
                else //Update (or delete home address)
                {
                    if (patientAddress[i].To_Be_Deleted)//delete
                    {
                        dbPatientAddress.DELETED = true;
                        dbPatientAddress.MODIFIED_BY = profile.UserName;
                        dbPatientAddress.MODIFIED_DATE = Helper.GetCurrentDate();
                        _PatientAddressRepository.Update(dbPatientAddress);
                        _PatientAddressRepository.Save();
                        SaveAddressInWebEHRTable(false, dbPatientAddress);
                        if (patientAddress[i].ADDRESS_TYPE != null && patientAddress[i].ADDRESS_TYPE.ToLower() == "home address")
                        {
                            UpdateResidualInsuranceSubscriberOnRemoveHomeAddress(patientAccount.ToString(), profile);
                            UpdateHomeAddressInPatient(patientAccount, profile);
                        }
                    }
                    else
                    {   //update
                        if (!String.Equals(patientAddress[i].ADDRESS_TYPE, dbPatientAddress.ADDRESS_TYPE, StringComparison.Ordinal)) { CreatePatientUpdateHistory(dbPatientAddress.PATIENT_ACCOUNT.Value, "Address Type", dbPatientAddress.ADDRESS_TYPE, patientAddress[i].ADDRESS_TYPE, profile.UserName); dbPatientAddress.ADDRESS_TYPE = patientAddress[i].ADDRESS_TYPE; }
                        if (!String.Equals(patientAddress[i].CITY, dbPatientAddress.CITY, StringComparison.Ordinal)) { CreatePatientUpdateHistory(dbPatientAddress.PATIENT_ACCOUNT.Value, "City", dbPatientAddress.CITY, patientAddress[i].CITY, profile.UserName); dbPatientAddress.CITY = patientAddress[i].CITY; }
                        if (!String.Equals(patientAddress[i].STATE, dbPatientAddress.STATE, StringComparison.Ordinal)) { CreatePatientUpdateHistory(dbPatientAddress.PATIENT_ACCOUNT.Value, "State", dbPatientAddress.STATE, patientAddress[i].STATE, profile.UserName); dbPatientAddress.STATE = patientAddress[i].STATE; }
                        if (!String.Equals(patientAddress[i].ADDRESS, dbPatientAddress.ADDRESS, StringComparison.Ordinal)) { CreatePatientUpdateHistory(dbPatientAddress.PATIENT_ACCOUNT.Value, "Address", dbPatientAddress.ADDRESS, patientAddress[i].ADDRESS, profile.UserName); dbPatientAddress.ADDRESS = patientAddress[i].ADDRESS; }
                        if (!String.Equals(patientAddress[i].ZIP, dbPatientAddress.ZIP, StringComparison.Ordinal))
                        {
                            patientAddress[i].ZIP = patientAddress[i].ZIP == null ? null : patientAddress[i].ZIP.Replace("-", "");
                            dbPatientAddress.ZIP = dbPatientAddress.ZIP.Replace("-", "");

                            CreatePatientUpdateHistory(dbPatientAddress.PATIENT_ACCOUNT.Value, "ZIP", dbPatientAddress.ZIP, patientAddress[i].ZIP, profile.UserName);
                            dbPatientAddress.ZIP = patientAddress[i].ZIP;
                        }

                        dbPatientAddress.ADDRESS_TYPE = patientAddress[i].ADDRESS_TYPE;
                        dbPatientAddress.MODIFIED_BY = profile.UserName;
                        dbPatientAddress.MODIFIED_DATE = Helper.GetCurrentDate();
                        _PatientAddressRepository.Update(dbPatientAddress);
                        _PatientAddressRepository.Save();
                        SaveAddressInWebEHRTable(false, dbPatientAddress);
                        if (patientAddress[i].ADDRESS_TYPE != null && patientAddress[i].ADDRESS_TYPE.ToLower() == "home address")
                        {
                            CheckAndUpdatePRSubscriber(patientAccount.ToString(), profile);
                            UpdateSubscriberForAllOtherInsurancesIfRelationSelfExceptPR(patientAccount.ToString(), profile);
                        }
                    }
                }
            }
        }

        private void UpdateHomeAddressInPatient(long? patient_account, UserProfile profile)
        {
            if (patient_account.HasValue && patient_account.Value != 0)
            {
                var patient = _PatientRepository.GetFirst(e => e.Patient_Account == patient_account.Value && (e.DELETED ?? false) == false);
                if (patient != null)
                {
                    patient.Address = patient.ZIP = patient.City = patient.State = null;
                    patient.ModifiedBy = profile.UserName;
                    patient.Modified_Date = Helper.GetCurrentDate();
                    _PatientRepository.Update(patient);
                    _PatientRepository.Save();
                }
            }
        }

        private void DeleteAddress(long addressId)
        {
            if (addressId != 0)
            {
                var addressToBeDeleted = _PatientAddressRepository.GetByID(addressId);
                if (addressToBeDeleted != null)
                {
                    _PatientAddressRepository.Delete(addressToBeDeleted);
                    _PatientAddressRepository.Save();
                }
            }
        }

        private void AddUpdatePatientInsurance(List<PatientInsurance> patientInsurance, UserProfile profile, long patientAccount)
        {
            //var medicareIds =
            //      new List<long> {
            //                        0,
            //                        0,
            //                        0
            //  };
            for (int i = 0; i < patientInsurance?.Count; i++)
            {
                PatientInsuranceEligibilityDetail ObjPatientInsuranceEligibilityDetail = new PatientInsuranceEligibilityDetail();
                ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate = new PatientInsurance();
                ObjPatientInsuranceEligibilityDetail.Patient_Account_Str = patientAccount.ToString();
                //List<MedicareLimit> medicarelistObj = new List<MedicareLimit>();
                ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate = patientInsurance[i];

                //var currMedLimList = _MedicareLimitRepository.GetMany(x => medicareIds.Contains(x.MEDICARE_LIMIT_ID));

                //var medicareLimitTypes = _MedicareLimitTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);
                //ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.CurrentMedicareLimitList = medicareLimitTypes.GroupJoin(
                //    currMedLimList,
                //    foo => foo.MEDICARE_LIMIT_TYPE_ID,
                //    bar => bar.MEDICARE_LIMIT_TYPE_ID,
                //    (x, y) => new { lim_type = x, currMedLim = y }).SelectMany(
                //    x => x.currMedLim.DefaultIfEmpty(),
                //    //(x, y) => new { lim_type = x, currMedLim = y },
                //    (x, y) => new MedicareLimit
                //    {
                //        MEDICARE_LIMIT_ID = y?.MEDICARE_LIMIT_ID ?? 0,
                //        PRACTICE_CODE = y?.PRACTICE_CODE,
                //        Patient_Account = y?.Patient_Account,
                //        EFFECTIVE_DATE = y?.EFFECTIVE_DATE,
                //        EFFECTIVE_DATE_IN_STRING = y?.EFFECTIVE_DATE_IN_STRING,
                //        END_DATE = y?.END_DATE,
                //        END_DATE_IN_STRING = y?.END_DATE_IN_STRING,
                //        ABN_EST_WK_COST = y?.ABN_EST_WK_COST,
                //        ABN_COMMENTS = y?.ABN_COMMENTS,
                //        NPI = y?.NPI,
                //        MEDICARE_LIMIT_TYPE_ID = x?.lim_type.MEDICARE_LIMIT_TYPE_ID,
                //        MEDICARE_LIMIT_STATUS = y?.MEDICARE_LIMIT_STATUS,
                //        MEDICARE_LIMIT_TYPE_NAME = x?.lim_type.NAME,
                //        DISPLAY_ORDER = x.lim_type.DISPLAY_ORDER
                //    }
                //).OrderBy(w => w.DISPLAY_ORDER).ToList();
                //ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.CurrentMedicareLimitList = medicarelistObj;
                SaveInsuranceAndEligibilityDetails(ObjPatientInsuranceEligibilityDetail, profile, true);

                //if (!string.IsNullOrEmpty(patientInsurance[i].Effective_Date_In_String))
                //{
                //    patientInsurance[i].Effective_Date = Convert.ToDateTime(patientInsurance[i].Effective_Date_In_String);
                //}
                //if (!string.IsNullOrEmpty(patientInsurance[i].Termination_Date_In_String))
                //{
                //    patientInsurance[i].Termination_Date = Convert.ToDateTime(patientInsurance[i].Termination_Date_In_String);
                //}
                //if (!string.IsNullOrEmpty(patientInsurance[i].SUPRESS_BILLING_UNTIL_DATE_IN_STRING))
                //{
                //    patientInsurance[i].SUPRESS_BILLING_UNTIL = Convert.ToDateTime(patientInsurance[i].SUPRESS_BILLING_UNTIL_DATE_IN_STRING);
                //}

                //patientInsurance[i] = CreateUpdateInsuranceInMTBC(patientInsurance[i], patientAccount, profile);

                //var dbPatientInsurance = _PatientInsuranceRepository.GetByID(patientInsurance[i].Patient_Insurance_Id);
                //if (dbPatientInsurance == null) //add
                //{
                //    patientInsurance[i].Patient_Insurance_Id = Helper.getMaximumId("Fox_Patient_Insurance_Id");
                //    patientInsurance[i].Parent_Patient_insurance_Id = patientInsurance[i].Patient_Insurance_Id;
                //    patientInsurance[i].FOX_INSURANCE_STATUS = "C";
                //    patientInsurance[i].Patient_Account = patientAccount;
                //    patientInsurance[i].Created_By = patientInsurance[i].Modified_By = profile.UserName;
                //    patientInsurance[i].Created_Date = patientInsurance[i].Modified_Date = Helper.GetCurrentDate();
                //    patientInsurance[i].Policy_Number = !string.IsNullOrEmpty(patientInsurance[i].Policy_Number) ? patientInsurance[i].Policy_Number : "";
                //    patientInsurance[i].Deleted = false;
                //    //-------------------------------------------------Subscriber Info-------------------------------------------------
                //    //if (patientInsurance[i].SUBSCRIBER_DETAILS != null)
                //    //{
                //    //    if (!patientInsurance[i].Relationship.ToLower().Equals("s") && patientInsurance[i].InsPayer_Description != "/Pt Pays after Insu")
                //    //    {
                //    //        SavePatientContactfromInsuranceSubscriber(patientInsurance[i].SUBSCRIBER_DETAILS, patientInsurance[i].Relationship, patientAccount, profile);
                //    //    }
                //    //    if (patientInsurance[i].SUBSCRIBER_DETAILS.IS_NEW_SUBSCRIBER) //If new subscriber created, it will contain subscriber data.
                //    //    {
                //    //        //Create new subscriber and add map its key with new insurance obj
                //    //        patientInsurance[i].SUBSCRIBER_DETAILS.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                //    //        patientInsurance[i].SUBSCRIBER_DETAILS.guarant_practice_code = profile.PracticeCode;
                //    //        patientInsurance[i].SUBSCRIBER_DETAILS.created_by = patientInsurance[i].SUBSCRIBER_DETAILS.modified_by = profile.UserName;
                //    //        patientInsurance[i].SUBSCRIBER_DETAILS.created_date = patientInsurance[i].SUBSCRIBER_DETAILS.modified_date = Helper.GetCurrentDate();
                //    //        patientInsurance[i].SUBSCRIBER_DETAILS.Deleted = false;
                //    //        if (!string.IsNullOrEmpty(patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING))
                //    //        {
                //    //            patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_DOB = Convert.ToDateTime(patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING);
                //    //        }
                //    //        //InsertInterfaceTeamData(interfaceSynch, profile);
                //    //        _SubscriberRepository.Insert(patientInsurance[i].SUBSCRIBER_DETAILS);
                //    //        //Map new subscriber with new insurance obj as well once subscriber created sucessfully
                //    //        patientInsurance[i].Subscriber = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANTOR_CODE;
                //    //    }
                //    //    else
                //    //    {
                //    //        //If any other subscriber has been allocated, then it is already mapped with InsuranceToCreateUpdate obj on client side upon subscriber selection.
                //    //    }
                //    //}
                //    _PatientInsuranceRepository.Insert(patientInsurance[i]);
                //    _PatientInsuranceRepository.Save();
                //}
                //else //Update
                //{
                //    var insuranceType = string.Empty;
                //    if (patientInsurance[i].Pri_Sec_Oth_Type == "P")
                //    {
                //        insuranceType = "Primary Insurance";
                //    }
                //    else if (patientInsurance[i].Pri_Sec_Oth_Type == "S")
                //    {
                //        insuranceType = "Secondary Insurance";
                //    }
                //    else
                //    {
                //        insuranceType = "Tertiary Insurance";
                //    }
                //    if (patientInsurance[i].Insurance_Id != dbPatientInsurance.Insurance_Id)
                //    {
                //        CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Insurance_Id", dbPatientInsurance.Insurance_Id.ToString(), patientInsurance[i].Insurance_Id.ToString(), profile.UserName);
                //        dbPatientInsurance.Insurance_Id = patientInsurance[i].Insurance_Id;
                //    }
                //    //if (patientInsurance[i].Co_Payment != dbPatientInsurance.Co_Payment) { CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Co Payment", dbPatientInsurance.Co_Payment.ToString(), patientInsurance[i].Co_Payment.ToString(), profile.UserName); dbPatientInsurance.Co_Payment = patientInsurance[i].Co_Payment; }
                //    //if (patientInsurance[i].Deductions != dbPatientInsurance.Deductions) { CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Deductions", dbPatientInsurance.Deductions.ToString(), patientInsurance[i].Deductions.ToString(), profile.UserName); dbPatientInsurance.Deductions = patientInsurance[i].Deductions; }
                //    if (!String.Equals(patientInsurance[i].Policy_Number, dbPatientInsurance.Policy_Number, StringComparison.Ordinal))
                //    {
                //        CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Policy No", dbPatientInsurance.Policy_Number, patientInsurance[i].Policy_Number, profile.UserName);
                //        dbPatientInsurance.Policy_Number = patientInsurance[i].Policy_Number;
                //    }
                //    if (!String.Equals(patientInsurance[i].Group_Number, dbPatientInsurance.Group_Number, StringComparison.Ordinal))
                //    {
                //        CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Group No", dbPatientInsurance.Group_Number, patientInsurance[i].Group_Number, profile.UserName);
                //        dbPatientInsurance.Group_Number = patientInsurance[i].Group_Number;
                //    }
                //    if (!String.Equals(patientInsurance[i].Relationship, dbPatientInsurance.Relationship, StringComparison.Ordinal))
                //    {
                //        CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Relationship", dbPatientInsurance.Relationship, patientInsurance[i].Relationship, profile.UserName);
                //        dbPatientInsurance.Relationship = patientInsurance[i].Relationship;
                //    }
                //    if (!String.Equals(patientInsurance[i].Plan_Name, dbPatientInsurance.Plan_Name, StringComparison.Ordinal))
                //    {
                //        CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Plan Name", dbPatientInsurance.Plan_Name, patientInsurance[i].Plan_Name, profile.UserName);
                //        dbPatientInsurance.Plan_Name = patientInsurance[i].Plan_Name;
                //    }

                //    if (patientInsurance[i]?.Effective_Date != null && dbPatientInsurance.Effective_Date != null && patientInsurance[i]?.Effective_Date.Value.Date != dbPatientInsurance.Effective_Date.Value.Date)
                //    {
                //        CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Effective Date", dbPatientInsurance.Effective_Date.Value.Date.ToString("dd/MM/yyyy"), patientInsurance[i].Effective_Date.Value.Date.ToString("dd/MM/yyyy"), profile.UserName);
                //        dbPatientInsurance.Effective_Date = patientInsurance[i].Effective_Date;
                //    }

                //    if (patientInsurance[i]?.Termination_Date != null && dbPatientInsurance.Termination_Date != null && patientInsurance[i].Termination_Date.Value.Date != dbPatientInsurance.Termination_Date.Value.Date)
                //    {
                //        CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + " Termination Date", dbPatientInsurance.Termination_Date.Value.Date.ToString("dd/MM/yyyy"), patientInsurance[i].Termination_Date.Value.Date.ToString("dd/MM/yyyy"), profile.UserName);
                //        dbPatientInsurance.Termination_Date = patientInsurance[i].Termination_Date;
                //    }
                //    if (patientInsurance[i]?.SUPRESS_BILLING_UNTIL != null && dbPatientInsurance.SUPRESS_BILLING_UNTIL != null && patientInsurance[i].SUPRESS_BILLING_UNTIL.Value.Date != dbPatientInsurance.SUPRESS_BILLING_UNTIL.Value.Date)
                //    {
                //        CreatePatientUpdateHistory(dbPatientInsurance.Patient_Account, insuranceType + "Supress billing until", dbPatientInsurance.SUPRESS_BILLING_UNTIL.Value.Date.ToString("dd/MM/yyyy"), patientInsurance[i].SUPRESS_BILLING_UNTIL.Value.Date.ToString("dd/MM/yyyy"), profile.UserName);
                //        dbPatientInsurance.SUPRESS_BILLING_UNTIL = patientInsurance[i].SUPRESS_BILLING_UNTIL;
                //    }
                //    dbPatientInsurance.Pri_Sec_Oth_Type = patientInsurance[i].Pri_Sec_Oth_Type;
                //    dbPatientInsurance.FOX_TBL_INSURANCE_ID = patientInsurance[i].FOX_TBL_INSURANCE_ID;
                //    dbPatientInsurance.Patient_Account = dbPatientInsurance.Patient_Account;
                //    dbPatientInsurance.Effective_Date = patientInsurance[i].Effective_Date;
                //    dbPatientInsurance.Termination_Date = patientInsurance[i].Termination_Date;
                //    dbPatientInsurance.INACTIVE = patientInsurance[i].INACTIVE;
                //    dbPatientInsurance.IsWellness = patientInsurance[i].IsWellness;
                //    dbPatientInsurance.IsSkilled = patientInsurance[i].IsSkilled;
                //    dbPatientInsurance.SUPRESS_BILLING_UNTIL = patientInsurance[i].SUPRESS_BILLING_UNTIL;
                //    dbPatientInsurance.Modified_By = profile.UserName;
                //    dbPatientInsurance.Modified_Date = Helper.GetCurrentDate();
                //    dbPatientInsurance.MTBC_Patient_Insurance_Id = patientInsurance[i].MTBC_Patient_Insurance_Id;

                //    if (patientInsurance[i].SUBSCRIBER_DETAILS != null)
                //    {
                //        if (!patientInsurance[i].Relationship.ToLower().Equals("s") && patientInsurance[i].InsPayer_Description != "/Pt Pays after Insu")
                //        {
                //            SavePatientContactfromInsuranceSubscriber(patientInsurance[i].SUBSCRIBER_DETAILS, patientInsurance[i].Relationship, patientAccount, profile);
                //        }
                //        if (patientInsurance[i].SUBSCRIBER_DETAILS.IS_NEW_SUBSCRIBER) //If new subscriber created, it will contain subscriber data.
                //        {
                //            //Create new subscriber and add map its key with new insurance obj
                //            patientInsurance[i].SUBSCRIBER_DETAILS.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                //            patientInsurance[i].SUBSCRIBER_DETAILS.guarant_practice_code = profile.PracticeCode;
                //            patientInsurance[i].SUBSCRIBER_DETAILS.created_by = patientInsurance[i].SUBSCRIBER_DETAILS.modified_by = profile.UserName;
                //            patientInsurance[i].SUBSCRIBER_DETAILS.created_date = patientInsurance[i].SUBSCRIBER_DETAILS.modified_date = Helper.GetCurrentDate();
                //            patientInsurance[i].SUBSCRIBER_DETAILS.Deleted = false;
                //            _SubscriberRepository.Insert(patientInsurance[i].SUBSCRIBER_DETAILS);
                //            _SubscriberRepository.Save();
                //            //Map new subscriber with new insurance obj as well once subscriber created sucessfully
                //            dbPatientInsurance.Subscriber = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANTOR_CODE;
                //        }
                //        else
                //        {
                //            //If any other subscriber has been reallocated, then it is already mapped with InsuranceToCreateUpdate obj on client side upon subscriber selection. 
                //            //So just assign it.
                //            //dbPatientInsurance.Subscriber = patientInsurance[i].Subscriber;
                //            if (patientInsurance[i].Pri_Sec_Oth_Type.ToLower().Equals("pr"))

                //            {
                //                var existingsub = _SubscriberRepository.GetByID(patientInsurance[i].Subscriber);
                //                if (existingsub != null)
                //                {
                //                    existingsub.GUARANT_FNAME = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_FNAME;
                //                    existingsub.GUARANT_LNAME = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_LNAME;
                //                    existingsub.GUARANT_MI = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_MI;
                //                    existingsub.GUARANT_ADDRESS = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_ADDRESS;
                //                    existingsub.GUARANT_ZIP = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_ZIP;
                //                    existingsub.GUARANT_CITY = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_CITY;
                //                    existingsub.GUARANT_STATE = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_STATE;
                //                    existingsub.GUARANT_HOME_PHONE = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_HOME_PHONE;
                //                    existingsub.GUARANT_DOB = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_DOB;
                //                    existingsub.GUARANT_GENDER = patientInsurance[i].SUBSCRIBER_DETAILS.GUARANT_GENDER;

                //                    existingsub.modified_by = profile.UserName;
                //                    existingsub.modified_date = Helper.GetCurrentDate();
                //                    _SubscriberRepository.Update(existingsub);
                //                    _SubscriberRepository.Save();

                //                    patientInsurance[i].Subscriber = existingsub.GUARANTOR_CODE;

                //                }

                //                //details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE = (long)details.InsuranceToCreateUpdate.Subscriber;

                //                //details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.Deleted = false;
                //            }
                //        }
                //    }
                //    _PatientInsuranceRepository.Update(dbPatientInsurance);
                //    _PatientInsuranceRepository.Save();
                //}
                ////if (!patientInsurance[i].Pri_Sec_Oth_Type.ToLower().Equals("pr") && patientInsurance[i].Patient_Insurancs_Ids_With_Overlapping_Dates != null && patientInsurance[i].Patient_Insurancs_Ids_With_Overlapping_Dates.Count > 0)
                ////{
                ////    SetTerminationDateForExistingPayers(patientInsurance[i], profile);
                ////}

                //if (patientInsurance[i].ClaimsToMapToNewInsurance != null && patientInsurance[i].ClaimsToMapToNewInsurance.Count > 0)
                //{
                //    MapClaimsToNewInsurance(patientInsurance[i], profile);
                //}
            }
        }

        public void CreatePatientUpdateHistory(long patientAccount, string fieldName, string previous_value, string newValue, string updatedBy)
        {
            var _patientContext = new DbContextPatientNew();
            var _patientUpdateHistoryRepository = new GenericRepository<PatientUpdateHistory>(_patientContext);
            var _patientUpdateHistory = new PatientUpdateHistory();
            long patientUpdateHistoryId = Helper.getMaximumId("patient_update_history_id");
            _patientUpdateHistory.patient_update_history_id = patientUpdateHistoryId;
            _patientUpdateHistory.PATIENT_ACCOUNT = patientAccount;
            _patientUpdateHistory.field_name = fieldName;
            //_patientUpdateHistory.previous_value = previous_value;
            //_patientUpdateHistory.new_value = string.IsNullOrEmpty(newValue) ? string.Empty : newValue.Replace("-", " ");
            _patientUpdateHistory.new_value = !string.IsNullOrWhiteSpace(newValue) ? newValue.Replace("-", " ") : "";
            _patientUpdateHistory.previous_value = !string.IsNullOrWhiteSpace(previous_value) ? previous_value : "";
            _patientUpdateHistory.ip_address = Helper.GetIpAddress() == "::1" ? "Server" : Helper.GetIpAddress();
            _patientUpdateHistory.updated_by = updatedBy;
            _patientUpdateHistory.updated_time = Helper.GetCurrentDate();
            _patientUpdateHistory.isActive = true;
            _patientUpdateHistory.isDeleted = false;
            _patientUpdateHistoryRepository.Insert(_patientUpdateHistory);
            _patientUpdateHistoryRepository.Save();
        }

        public List<Patient> GetPatientList(PatientSearchRequest patientSearchRequest, UserProfile profile)
        {
            string spName = string.Empty;
            if (!string.IsNullOrEmpty(patientSearchRequest.DOBInString))
            {
                patientSearchRequest.DOB = Convert.ToDateTime(patientSearchRequest.DOBInString);
            }

            if (!string.IsNullOrEmpty(patientSearchRequest.CreatedDateInString))
            {
                patientSearchRequest.CreatedDate = Convert.ToDateTime(patientSearchRequest.CreatedDateInString);
            }

            var accountNo = Helper.getDBNullOrValue("Patient_Account", patientSearchRequest.Patient_Account);
            var FirstName = Helper.getDBNullOrValue("First_Name ", patientSearchRequest.FirstName.Trim());
            var MiddleName = Helper.getDBNullOrValue("Middle_Name", patientSearchRequest.MiddleName.Trim());
            var LastName = Helper.getDBNullOrValue("Last_Name", patientSearchRequest.LastName.Trim());
            var MRN = Helper.getDBNullOrValue("CHART_ID", patientSearchRequest.MRN);
            var SSN = Helper.getDBNullOrValue("SSN", patientSearchRequest.SSN);
            var dob = Helper.getDBNullOrValue("DOB", patientSearchRequest.DOB.HasValue ? patientSearchRequest.DOB.Value.ToString("MM/dd/yyyy") : "");
            var Gender = Helper.getDBNullOrValue("Gender", patientSearchRequest.Gender);
            var CreatedDate = Helper.getDBNullOrValue("Created_Date", patientSearchRequest.CreatedDate.HasValue ? patientSearchRequest.CreatedDate.Value.ToString("MM/dd/yyyy") : "");
            var CreatedBy = Helper.getDBNullOrValue("CreatedBy", patientSearchRequest.CreatedBy);
            var ModifiedBy = Helper.getDBNullOrValue("ModifiedBy", patientSearchRequest.ModifiedBy);
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = patientSearchRequest.CurrentPage };
            var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = patientSearchRequest.RecordPerPage };
            var SearchText = new SqlParameter { ParameterName = "SEARCH_TEXT", SqlDbType = SqlDbType.VarChar, Value = string.IsNullOrEmpty(patientSearchRequest.SearchText) ? "" : patientSearchRequest.SearchText };
            var SortBy = Helper.getDBNullOrValue("SORT_BY", patientSearchRequest.SortBy);
            var SortOrder = Helper.getDBNullOrValue("SORT_ORDER", patientSearchRequest.SortOrder);
            var Patient_Alias = new SqlParameter { ParameterName = "Patient_Alias", SqlDbType = SqlDbType.Bit, Value = patientSearchRequest.INCLUDE_ALIAS };

            spName = profile.isTalkRehab ? "FOX_PROC_GET_PATIENT_LIST_TALKREHAB" : "FOX_PROC_GET_PATIENT_LIST";

            var PatientList = SpRepository<Patient>.GetListWithStoreProcedure(@"exec " + spName + " @Patient_Account, @First_Name, @Last_Name, @Middle_Name, @CHART_ID, @SSN, @Gender, @Created_Date, @CreatedBy, @ModifiedBy, @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORT_BY, @SORT_ORDER,@DOB, @Patient_Alias",
                accountNo, FirstName, LastName, MiddleName, MRN, SSN, Gender, CreatedDate, CreatedBy, ModifiedBy, PracticeCode, CurrentPage, RecordPerPage, SearchText, SortBy, SortOrder, dob, Patient_Alias);

            return PatientList;
        }

        public List<PatientAddress> GetPatientAddress(long patientAccount)
        {
            return _PatientAddressRepository.GetMany(x => x.PATIENT_ACCOUNT == patientAccount && x.DELETED == false && x.ADDRESS_TYPE != null).ToList();
        }

        public List<PatientAddress> GetPatientAddressesIncludingPOS(long patientAccount)
        {
            var regularAddresses = _PatientAddressRepository.GetMany(x => x.PATIENT_ACCOUNT == patientAccount && x.DELETED == false).ToList();
            var posAddress = _PatientPOSLocationRepository.GetFirst(e => e.Patient_Account == patientAccount && e.Loc_ID != 0 && e.Deleted == false);
            if (posAddress != null)
            {
                var loc_datails = _FacilityLocationRepository.GetByID(posAddress.Loc_ID);
                if (loc_datails != null)
                {
                    var obj = new PatientAddress();
                    obj.ADDRESS = loc_datails.Address;
                    obj.CITY = loc_datails.City;
                    obj.STATE = loc_datails.State;
                    obj.ZIP = loc_datails.Zip;
                    obj.ADDRESS_TYPE = "pos";
                    regularAddresses.Add(obj);
                }
            }
            return regularAddresses;
        }

        public List<PatientAddress> GetPatientAddressesToDisplay(long patientAccount)
        {
            return _PatientAddressRepository.GetMany(x => x.PATIENT_ACCOUNT == patientAccount && x.DELETED == false).ToList();
        }

        public List<PatientInsurance> GetPatientInsurance(long patientAccount, UserProfile profile)
        {

            UpdateMedicareCheckboxes(patientAccount, profile);
            var PatientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = patientAccount };
            var result = SpRepository<PatientInsurance>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES @PATIENT_ACCOUNT", PatientAccount);

            foreach (var ins in result)
            {
                long subsId = ins.Subscriber.HasValue ? ins.Subscriber.Value : 0;
                if (subsId > 0)
                {
                    ins.SUBSCRIBER_DETAILS = _SubscriberRepository.GetByID(subsId);
                }
                ins.CurrentMedicareLimitList = new List<MedicareLimit>();
                var medicareIds =
                    new List<long> {
                                    0,
                                    0,
                                    0
                };
                var currMedLimList = _MedicareLimitRepository.GetMany(x => medicareIds.Contains(x.MEDICARE_LIMIT_ID));

                var medicareLimitTypes = _MedicareLimitTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);
                ins.CurrentMedicareLimitList = medicareLimitTypes.GroupJoin(
                    currMedLimList,
                    foo => foo.MEDICARE_LIMIT_TYPE_ID,
                    bar => bar.MEDICARE_LIMIT_TYPE_ID,
                    (x, y) => new { lim_type = x, currMedLim = y }).SelectMany(
                    x => x.currMedLim.DefaultIfEmpty(),
                    //(x, y) => new { lim_type = x, currMedLim = y },
                    (x, y) => new MedicareLimit
                    {
                        MEDICARE_LIMIT_ID = y?.MEDICARE_LIMIT_ID ?? 0,
                        PRACTICE_CODE = y?.PRACTICE_CODE,
                        Patient_Account = y?.Patient_Account,
                        EFFECTIVE_DATE = y?.EFFECTIVE_DATE,
                        EFFECTIVE_DATE_IN_STRING = y?.EFFECTIVE_DATE_IN_STRING,
                        END_DATE = y?.END_DATE,
                        END_DATE_IN_STRING = y?.END_DATE_IN_STRING,
                        ABN_EST_WK_COST = y?.ABN_EST_WK_COST,
                        ABN_COMMENTS = y?.ABN_COMMENTS,
                        NPI = y?.NPI,
                        MEDICARE_LIMIT_TYPE_ID = x?.lim_type.MEDICARE_LIMIT_TYPE_ID,
                        MEDICARE_LIMIT_STATUS = y?.MEDICARE_LIMIT_STATUS,
                        MEDICARE_LIMIT_TYPE_NAME = x?.lim_type.NAME,
                        DISPLAY_ORDER = x.lim_type.DISPLAY_ORDER
                    }
                ).OrderBy(w => w.DISPLAY_ORDER).ToList();
            }
            return result;
        }

        public List<PatientUpdateHistory> GetPatientUpdateHistory(PatientUpdateHistory patientUpdateHistory)
        {
            var patientAccount = new SqlParameter { ParameterName = "@patientAccount", Value = patientUpdateHistory.PATIENT_ACCOUNT };
            var pageSize = new SqlParameter { ParameterName = "@pageSize", Value = 100 };
            var result = SpRepository<PatientUpdateHistory>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_UPDATE_HISTORY_LIST @patientAccount, @pageSize", patientAccount, pageSize);
            return result;
        }

        public List<ZipCityState> GetCityStateByZip(string zipCode)
        {
            if (zipCode.Contains("-"))
            {
                zipCode = zipCode.Replace("-", "");
            }
            var _zipCode = new SqlParameter { ParameterName = "@zipCode", Value = zipCode };
            var result = SpRepository<ZipCityState>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_CITY_STATE_BY_ZIP_CODE @zipCode", _zipCode);
            return result;
        }

        public List<ZipCityStateAddress> SearchCityStateAddressByAPI(string zipCode, string address)
        {
            address = String.IsNullOrEmpty(address) ? " " : address;
            zipCode = String.IsNullOrEmpty(zipCode) ? " " : zipCode;
            if (zipCode.Contains("-"))
            {
                zipCode = zipCode.Replace("-", "");
            }
            var _zipCode = new SqlParameter { ParameterName = "@zipCode", Value = zipCode };
            var result = SpRepository<ZipCityState>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_CITY_STATE_BY_ZIP_CODE @zipCode", _zipCode);
            //Above Line will return City and State with respect to ZIP Code

            List<ZipCityStateAddress> _zipCodeTemp = new List<ZipCityStateAddress>();
            if (result != null && result.Any() && result.Count() > 0)
            {
                var _USStreetApiClass = new USStreetApiClass();

                var lookup = new SmartyStreets.USStreetApi.Lookup
                {
                    Street = address,
                    City = result[0].CITY_NAME,
                    State = result[0].STATE_CODE,
                    ZipCode = result[0].ZIP_CODE
                };
                _zipCodeTemp = _USStreetApiClass.GetDataUSStreetApi(lookup);
                //Above Line will return an Object of ZipCityStateAddress Class with addition of 9 digit ZIPC Code
            }
            else
            {
                var _USStreetApiClass = new USStreetApiClass();

                var lookup = new SmartyStreets.USStreetApi.Lookup
                {
                    Street = address,
                    City = "",
                    State = "",
                    ZipCode = zipCode
                };
                _zipCodeTemp = _USStreetApiClass.GetDataUSStreetApi(lookup);
                string fullZIP = _zipCodeTemp[0].ZIP_CODE;
                if (_zipCodeTemp[0].ZIP_CODE.Contains("-"))
                {
                    _zipCodeTemp[0].ZIP_CODE = _zipCodeTemp[0].ZIP_CODE.Replace("-", "");
                }
                if (_zipCodeTemp.Count > 0 && _zipCodeTemp[0].ZIP_CODE != "" && _zipCodeTemp[0].CITY_NAME != "" && _zipCodeTemp[0].STATE_CODE != "")
                {
                    Zip_City_State isAlreadyPresent = new Zip_City_State();
                    try
                    {
                        string zip = _zipCodeTemp[0].ZIP_CODE;
                        isAlreadyPresent = _zipCityStateRepository.GetFirst(x => x.ZIP_Code == zip && !x.Deleted);
                        if (isAlreadyPresent == null)
                        {
                            Zip_City_State objToAdd = new Zip_City_State();
                            objToAdd.ZIP_Code = _zipCodeTemp[0].ZIP_CODE;
                            objToAdd.City_Name = _zipCodeTemp[0].CITY_NAME;
                            objToAdd.State_Code = _zipCodeTemp[0].STATE_CODE;
                            objToAdd.Deleted = false;
                            objToAdd.Created_By = "Fox Team";
                            objToAdd.Created_Date = Helper.GetCurrentDate();
                            objToAdd.Modified_By = "Fox Team";
                            objToAdd.Modified_Date = Helper.GetCurrentDate();
                            objToAdd.Time_Zone = GetTimeZoneName(_zipCodeTemp[0].TIME_ZONE);
                            _zipCityStateRepository.Insert(objToAdd);
                            _zipCityStateRepository.Save();
                        }
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }




                }
                _zipCodeTemp[0].ZIP_CODE = fullZIP;
            }
            return _zipCodeTemp;
        }
        public string GetTimeZoneName(string timeZone)
        {
            string zoneName = string.Empty;
            switch (timeZone)
            {
                case "Alaska":
                    zoneName = "AKST -  Alaska Standard Time";
                    break;
                case "Eastern":
                    zoneName = "EST - Eastern Standard Time";
                    break;
                case "Chamorro":
                    zoneName = "CHST -  Chamorro Standard Time";
                    break;
                case "Marshall":
                    zoneName = "MHT -  Marshall Islands Time";
                    break;
                case "Pacific":
                    zoneName = "PST - Pacific Standard Time";
                    break;
                case "Atlantic":
                    zoneName = "AST -  Atlantic Standard Time";
                    break;
                case "Central":
                    zoneName = "CST - Central Standard Time";
                    break;
                case "Mountain":
                    zoneName = "MST - Mountain Standard Time";
                    break;
                case "Pohnpei":
                    zoneName = "PONT - Pohnpei Standard Time";
                    break;

                default:
                    zoneName = "No Time Zone Applicable";
                    return zoneName;
            }
            return zoneName;
        }
        public ZipRegionIDName GetRegionByZip(string zipCode, UserProfile profile)
        {
            if (!string.IsNullOrWhiteSpace(zipCode))
            {
                if (zipCode.Contains("-"))
                {
                    zipCode = zipCode.Replace("-", "");
                }
                var _zipCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", Value = zipCode };
                var _PracticeCode = new SqlParameter { ParameterName = "@zipCode", Value = profile.PracticeCode };
                var result = SpRepository<ZipRegionIDName>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_REF_REGION_BY_ZIPCODE] @zipCode,@PRACTICE_CODE", _zipCode, _PracticeCode);
                if (result != null)
                    return result;
                else
                    return new ZipRegionIDName();
            }
            else
                return new ZipRegionIDName();
        }

        public List<string> GetCitiesByZip(string zipCode)
        {
            List<string> cities = new List<string>();
            if (!string.IsNullOrWhiteSpace(zipCode))
            {
                if (zipCode.Contains("-"))
                {
                    zipCode = zipCode.Replace("-", "");
                }
                var _zipCode = new SqlParameter { ParameterName = "@zipCode", Value = zipCode };
                cities = SpRepository<string>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_CITY_BY_ZIP_CODE @zipCode", _zipCode);
            }
            return cities;
        }

        public List<PatientInsuranceDetail> GetInsurancePayers(SmartSearchInsuranceReq obj)
        {
            string state = "";
            if (!string.IsNullOrWhiteSpace(obj?.Patient_Account ?? ""))
            {
                var patient_Account = long.Parse(obj.Patient_Account);
                var _POSData = _PatientPOSLocationRepository.GetFirst(x => x.Patient_Account == patient_Account && !(x.Deleted ?? false));
                if (_POSData != null)
                {
                    var locData = _FacilityLocationRepository.GetFirst(x => x.LOC_ID == _POSData.Loc_ID && !x.DELETED);
                    state = locData?.State ?? "";
                }
            }

            var _patientState = new SqlParameter { ParameterName = "@PatientState", Value = state };
            var _insuranceName = new SqlParameter { ParameterName = "Insurance_Description", Value = obj.Insurance_Name };
            var result = SpRepository<PatientInsuranceDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_INSURANCE_PAYERS @PatientState,@Insurance_Description ", _patientState, _insuranceName);
            return result;
        }

        public List<string> CheckNecessaryDataForEligibility(long patientAccount)
        {
            List<string> missingDataList = new List<string>();
            var _patientAccount = new SqlParameter { ParameterName = "PATIENTACCOUNT", Value = patientAccount };
            var patientInsuranceInformation = SpRepository<PatientInsuranceInformation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES_INFO @PATIENTACCOUNT", _patientAccount).Where(x => x.INS_TYPE == 1).FirstOrDefault();
            if (patientInsuranceInformation == null) { missingDataList.Add("No insurance found. Please add insurance first."); return missingDataList; }
            if (string.IsNullOrWhiteSpace(patientInsuranceInformation.FIRST_NAME)) { missingDataList.Add("First Name."); }
            if (string.IsNullOrWhiteSpace(patientInsuranceInformation.LAST_NAME)) { missingDataList.Add("Last Name."); }
            if (string.IsNullOrWhiteSpace(patientInsuranceInformation.POLICY_NUMBER)) { missingDataList.Add("Policy Number."); }
            if (!patientInsuranceInformation.DATE_OF_BIRTH.HasValue) { missingDataList.Add("Date of Birth."); }
            if (string.IsNullOrWhiteSpace(patientInsuranceInformation.GENDER)) { missingDataList.Add("Gender."); }
            return missingDataList;
        }

        public List<string> CheckNecessaryDataForLoadEligibility(PatientEligibilitySearchModel searchReq)
        {
            var patientAccount = searchReq.Patient_Account_Str;
            var patient_Insurance_Id = searchReq.Patient_Insurance_id;

            List<string> missingDataList = new List<string>();
            var _patientAccount = new SqlParameter { ParameterName = "PATIENTACCOUNT", Value = long.Parse(patientAccount) };
            var _result = SpRepository<PatientInsuranceInformation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES_INFO @PATIENTACCOUNT", _patientAccount);
            PatientInsuranceInformation patientInsuranceInformation = new PatientInsuranceInformation();
            if (patient_Insurance_Id.HasValue)
            {
                patientInsuranceInformation = _result.Where(x => x.PATIENT_INSURANCE_ID == patient_Insurance_Id.Value).FirstOrDefault();
            }
            else
            {
                patientInsuranceInformation = _result.Where(x => x.INS_TYPE == 1).FirstOrDefault();
            }
            if (patientInsuranceInformation == null) { missingDataList.Add("No insurance found. Please add insurance first."); return missingDataList; }
            if (string.IsNullOrWhiteSpace(patientInsuranceInformation.FIRST_NAME)) { missingDataList.Add("First Name."); }
            if (string.IsNullOrWhiteSpace(patientInsuranceInformation.LAST_NAME)) { missingDataList.Add("Last Name."); }
            if (string.IsNullOrWhiteSpace(patientInsuranceInformation.POLICY_NUMBER)) { missingDataList.Add("Policy Number."); }
            if (!patientInsuranceInformation.DATE_OF_BIRTH.HasValue) { missingDataList.Add("Date of Birth."); }
            if (string.IsNullOrWhiteSpace(patientInsuranceInformation.GENDER)) { missingDataList.Add("Gender."); }
            return missingDataList;
        }

        public string CheckPatientInsuranceEligibility(PatientEligibilitySearchModel patientEligibilitySearchModel, string userId)
        {
            var patientAccount = patientEligibilitySearchModel.Patient_Account_Str;
            var patient_Insurance_Id = patientEligibilitySearchModel.Patient_Insurance_id;

            //var patientAccount = new SqlParameter { ParameterName = "PATIENTACCOUNT", Value = 10011092 };
            var _patientAccount = new SqlParameter { ParameterName = "PATIENTACCOUNT", Value = long.Parse(patientAccount) };
            var _result = SpRepository<PatientInsuranceInformation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES_INFO @PATIENTACCOUNT", _patientAccount);
            PatientInsuranceInformation patientInsuranceInformation = new PatientInsuranceInformation();
            if (patient_Insurance_Id.HasValue)
            {
                patientInsuranceInformation = _result.Where(x => x.PATIENT_INSURANCE_ID == patient_Insurance_Id.Value).FirstOrDefault();
            }
            else
            {
                patientInsuranceInformation = _result.Where(x => x.INS_TYPE == 1).FirstOrDefault();
            }
            #region Old Implementation to check Eligiblity with WCF Service
            //Eligibility....
            //ExternalServices.PatientEligibilityService.MTBCData mtbcData = new ExternalServices.PatientEligibilityService.MTBCData();
            //mtbcData.ViewType = patientEligibilitySearchModel.IS_MVP_VIEW ? "MVP" : "";
            //mtbcData.ClientID = "FOXREHAB"; //  Client TalkEHR, WebSoft, EDI, WebEHR,foxrehab etc. For fox it will be FOXREHAB.
            //mtbcData.ClientType = "PATIENT";    //  ClientType can be CLAIM, PATIENT, APPOINTMENT or any other like FOX_CLIENT, from patient form it will be PATIENT.
            //mtbcData.ServerName = "10.10.30.76";    // Database Server IP
            //mtbcData.UserID = userId.ToString();    //  User that is using WebSoft. User like IA32, MS147 etc.
            //mtbcData.InsuranceID = patientInsuranceInformation?.INSURANCE_ID.ToString() ?? ""; //  Id of Insurance table in MTBC system
            //mtbcData.insPayerID = patientInsuranceInformation?.INSPAYER_ID.ToString() ?? "";   //  Optional
            //mtbcData.PayerType = patientInsuranceInformation?.INS_TYPE.ToString() ?? "";   //  Payer type in MTBC's system. Primary, Secondary or OTHER etc.
            //mtbcData.PatientAccount = patientInsuranceInformation.PATIENT_ACCOUNT.ToString();  //  Required value
            //mtbcData.ClaimNo = string.Empty;    //  Optional
            //mtbcData.AppointmentID = string.Empty;  //  Optional

            //ExternalServices.PatientEligibilityService.MTBC270RequestData requestData = new ExternalServices.PatientEligibilityService.MTBC270RequestData();

            //// requestData.FIRST_NAME = result.FIRST_NAME;
            ////  requestData.LAST_NAME = result.LAST_NAME;
            //requestData.Address = patientInsuranceInformation.ADDRESS;
            //requestData.City = patientInsuranceInformation.CITY;

            //requestData.DateOfService = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(DateTime.Now));
            //requestData.PayerName = patientInsuranceInformation.PAYER_NAME;
            //requestData.ProviderFirstName = patientInsuranceInformation.PROVIDER_FNAME; // Table name providers -- code -- patient-- provider 
            //requestData.ProviderLastName = patientInsuranceInformation.PROVIDER_LNAME; // Table name providers
            //requestData.ProviderNPI = "1326092503";  // Table name providers
            //requestData.ProviderSSN = patientInsuranceInformation.PROVIDER_SSN; // Table name providers
            //requestData.Relationship = patientInsuranceInformation.RELATIONSHIP;    // S
            //requestData.Zip = patientInsuranceInformation.ZIP;
            //requestData.State = patientInsuranceInformation.STATE;

            //if (patientInsuranceInformation.RELATIONSHIP == "S")
            //{
            //    requestData.SubscriberDateOfBirth = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.DATE_OF_BIRTH.ToString()));
            //    requestData.SubscriberFirstName = patientInsuranceInformation.FIRST_NAME;
            //    requestData.SubscriberGender = patientInsuranceInformation.GENDER;
            //    requestData.SubscriberGroupNumber = patientInsuranceInformation.GROUP_NUMBER;
            //    requestData.SubscriberLastName = patientInsuranceInformation.LAST_NAME;
            //    requestData.SubscriberSSN = patientInsuranceInformation.SSN;    //from patient table
            //    requestData.DependentDOB = "";
            //    requestData.DependentFirstName = "";
            //    requestData.DependentGender = "";
            //    requestData.DependentLastName = "";
            //}
            //else
            //{
            //    requestData.SubscriberDateOfBirth = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.GUARANTOR_DOB.ToString()));
            //    requestData.SubscriberFirstName = patientInsuranceInformation.GURANTOR_FNAME;
            //    requestData.SubscriberGender = patientInsuranceInformation.GUARANTOR_GENDER;
            //    requestData.SubscriberGroupNumber = patientInsuranceInformation.GROUP_NUMBER;
            //    requestData.SubscriberLastName = patientInsuranceInformation.GURANTOR_LNAME;
            //    requestData.SubscriberSSN = patientInsuranceInformation.GUARANTOR_SSN;
            //    requestData.DependentDOB = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.DATE_OF_BIRTH.ToString()));
            //    requestData.DependentFirstName = patientInsuranceInformation.FIRST_NAME;
            //    requestData.DependentGender = patientInsuranceInformation.GENDER;
            //    requestData.DependentLastName = patientInsuranceInformation.GROUP_NUMBER;
            //}
            //if (patientInsuranceInformation.PRAC_TYPE == "G")   //from practices by practice_code
            //{
            //    requestData.OrganizationType = "2";
            //    // requestData.ProviderNPI = // Group_NPI from table provider_payer
            //    requestData.OrganizationNPI = "1326092503";
            //}
            //else
            //{
            //    requestData.OrganizationType = "1";
            //    // requestData.ProviderNPI =  //indiv_NPI from table provider_payer
            //    requestData.OrganizationNPI = "1326092503";
            //}
            //requestData.ProviderSSN = patientInsuranceInformation.PROVIDER_SSN;
            //requestData.TaxID = patientInsuranceInformation.PRACTICE_TAX_ID;
            //requestData.PayerID = patientInsuranceInformation.INSPAYER_ELIGIBILITY_ID;
            //requestData.OrganizationName = patientInsuranceInformation.PRACTICE_NAME;
            //requestData.SubscriberMemberID = patientInsuranceInformation.POLICY_NUMBER;
            //ExternalServices.PatientEligibilityService.Service objService = new ExternalServices.PatientEligibilityService.Service();
            //string htmlStr = objService.MTBCResponse(requestData, mtbcData);
            #endregion
            #region New Implementation to check Eligiblity with RestFull API
            //To See Detailed Documentation Please have the documents path  : FoxRehabilitationAPI\About Project\Eligibity-documents\
            EligibilityModelNew objEligibilityNew = new EligibilityModelNew();
            String htmlStr;
            //*********************************** MTBCData ***************************************
            objEligibilityNew.ViewType = patientEligibilitySearchModel.IS_MVP_VIEW ? "MVP" : "FOX";
            objEligibilityNew.ClientID = "FOXREHAB";
            //objEligibilityNew.ClientID = "8781";
            objEligibilityNew.ClientType = "PATIENT";
            objEligibilityNew.ServerName = "10.10.30.76";
            objEligibilityNew.UserID = userId.ToString();
            objEligibilityNew.InsuranceID = patientInsuranceInformation.INSURANCE_ID.ToString() ?? "";
            objEligibilityNew.insPayerID = patientInsuranceInformation.INSPAYER_ID.ToString() ?? "";
            //objEligibilityNew.PayerType = patientInsuranceInformation.INS_TYPE.ToString();
            objEligibilityNew.PayerType = "P";
            objEligibilityNew.PatientAccount = patientInsuranceInformation.PATIENT_ACCOUNT.ToString();
            objEligibilityNew.ClaimNo = string.Empty;
            objEligibilityNew.AppointmentID = string.Empty;
            //objEligibilityNew.InsPayerDescriptionName = eligibilityModel.Inspayer_Description;
            //*********************************** Payer Information ***************************************
            objEligibilityNew.PayerName = patientInsuranceInformation.PAYER_NAME;
            objEligibilityNew.PayerID = patientInsuranceInformation.INSPAYER_ELIGIBILITY_ID;
            //************************************ Practice Information ************************************
            objEligibilityNew.Address = patientInsuranceInformation.ADDRESS;
            objEligibilityNew.City = patientInsuranceInformation.CITY;
            objEligibilityNew.DateOfService = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(DateTime.Now));
            //objElig.InquiryDate;    //  Required value
            objEligibilityNew.ProviderFirstName = patientInsuranceInformation.PROVIDER_FNAME;
            objEligibilityNew.ProviderLastName = patientInsuranceInformation.PROVIDER_LNAME;
            objEligibilityNew.ProviderNPI = "1326092503";  // Table name providers
            objEligibilityNew.ProviderSSN = patientInsuranceInformation.PROVIDER_SSN;
            objEligibilityNew.Relationship = patientInsuranceInformation.RELATIONSHIP;
            objEligibilityNew.Zip = patientInsuranceInformation.ZIP;
            objEligibilityNew.State = patientInsuranceInformation.STATE;
            if (!string.IsNullOrEmpty(patientInsuranceInformation.RELATIONSHIP) && patientInsuranceInformation.RELATIONSHIP.Contains("S"))
            {
                objEligibilityNew.SubscriberDateOfBirth = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.DATE_OF_BIRTH.ToString()));
                objEligibilityNew.SubscriberFirstName = patientInsuranceInformation.FIRST_NAME;
                objEligibilityNew.SubscriberGender = patientInsuranceInformation.GENDER;
                objEligibilityNew.SubscriberGroupNumber = patientInsuranceInformation.GROUP_NUMBER;
                objEligibilityNew.SubscriberLastName = patientInsuranceInformation.LAST_NAME;
                objEligibilityNew.SubscriberSSN = patientInsuranceInformation.SSN;    //from patient table                                              
                //*********************************** Dependent Level *****************************************
                objEligibilityNew.DependentDOB = string.Empty;
                objEligibilityNew.DependentFirstName = string.Empty;
                objEligibilityNew.DependentGender = string.Empty;
                objEligibilityNew.DependentLastName = string.Empty;
            }
            else
            {
                objEligibilityNew.SubscriberDateOfBirth = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.GUARANTOR_DOB.ToString()));
                objEligibilityNew.SubscriberFirstName = patientInsuranceInformation.GURANTOR_FNAME;
                objEligibilityNew.SubscriberGender = patientInsuranceInformation.GUARANTOR_GENDER;
                objEligibilityNew.SubscriberGroupNumber = patientInsuranceInformation.GROUP_NUMBER;
                objEligibilityNew.SubscriberLastName = patientInsuranceInformation.GURANTOR_LNAME;
                objEligibilityNew.SubscriberDateOfDeath = string.Empty;
                objEligibilityNew.SubscriberSSN = patientInsuranceInformation.GUARANTOR_SSN;
                //*********************************** Dependent Level *****************************************
                objEligibilityNew.DependentDOB = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.DATE_OF_BIRTH.ToString()));
                objEligibilityNew.DependentFirstName = patientInsuranceInformation.FIRST_NAME;
                objEligibilityNew.DependentGender = patientInsuranceInformation.GENDER;
                objEligibilityNew.DependentLastName = patientInsuranceInformation.GROUP_NUMBER;
            }
            if (patientInsuranceInformation.PRAC_TYPE == "G")
            {
                objEligibilityNew.OrganizationType = "2";  //Required value(if "I" then send 1 and if "G" then send 2 )
                objEligibilityNew.ProviderNPI = string.Empty; //if OrganizationType is "I" then you assign individual_npi
                objEligibilityNew.OrganizationNPI = "1326092503";  //if OrganizationType is "G" then you assign group_npi            
            }
            else
            {
                objEligibilityNew.OrganizationType = "1";     // Required value(if "I" then send 1 and if "G" then send 2 )
                objEligibilityNew.OrganizationNPI = string.Empty; //if OrganizationType is "G" then you assign group_npi            
                objEligibilityNew.ProviderNPI = "1326092503"; //if OrganizationType is "I" then you assign individual_npi
            }
            objEligibilityNew.TaxID = patientInsuranceInformation.PRACTICE_TAX_ID;     //  Required value
            objEligibilityNew.OrganizationName = patientInsuranceInformation.PRACTICE_NAME;
            objEligibilityNew.SubscriberMemberID = patientInsuranceInformation.POLICY_NUMBER; //  Required value
            //********************************** Service Level Type **************************************
            objEligibilityNew.ServiceType = "30";

            var result = "";
            string URl = WebConfigurationManager.AppSettings["EligibilityURL"].ToString();
            HtmlDocument doc = new HtmlDocument();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(URl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(objEligibilityNew);
                var stringContent = new StringContent(json, System.Text.UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = client.PostAsync("", stringContent).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync();
                    result = jsonString.Result;
                }
                htmlStr = result; //added by aftab
            }
            #endregion

            if (patientEligibilitySearchModel.IS_MVP_VIEW)
            {
                htmlStr = htmlStr.Replace(@"id=""container""", @"id =""main-container-eligibility""");
                htmlStr = htmlStr.Replace(@"table-heading", @"table-heading-orange");
                htmlStr = htmlStr.Replace(@"document.getElementById('1').style.display = ''", "document.getElementById('1')==null?'':document.getElementById('1').style.display = ''");
                //htmlStr = htmlStr.Replace(@"table-heading", @"table-heading-dark-grey");
                //htmlStr = htmlStr.Replace(@"border-b", @"border-b margin-b10");
                htmlStr = RemoveStyleNodeFromHtmlForMVP(htmlStr);
            }
            else
            {
                htmlStr = htmlStr.Replace(@"id=""main-container""", @"id=""main-container-eligibility""");
                htmlStr = htmlStr.Replace(@"document.getElementById('1').style.display = ''", "document.getElementById('1')==null?'':document.getElementById('1').style.display = ''");
                if (string.IsNullOrEmpty(patientInsuranceInformation?.Fox_Insurance_Name) && patientInsuranceInformation?.FOX_TBL_INSURANCE_ID != null)
                {
                    patientInsuranceInformation.Fox_Insurance_Name = _foxInsurancePayersRepository.GetFirst(p => p.FOX_TBL_INSURANCE_ID == patientInsuranceInformation.FOX_TBL_INSURANCE_ID && (p.DELETED ?? false) == false)?.INSURANCE_NAME;
                }
                htmlStr = UpdatePayerNameInHTML(htmlStr, patientInsuranceInformation);
            }

            return htmlStr;
        }

        public string RemoveStyleNodeFromHtmlForMVP(string htmlStr)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlStr);
            var eligiblitycont = doc.DocumentNode.SelectSingleNode("*//div[contains(@class,'container')]");
            if (eligiblitycont != null)
                htmlStr = eligiblitycont.OuterHtml;
            return htmlStr;
        }

        public string UpdatePayerNameInHTML(string htmlStr, PatientInsuranceInformation patientInsuranceInformation)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            //htmlDoc.Load(path);
            htmlDoc.LoadHtml(htmlStr);
            //htmlDoc.DocumentNode.OuterHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\b", "").Replace("&nbsp;", " ");
            var headingNode = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@id,'main-container-eligibility')]//div[contains(@class,'custom-panel-head')][1]//h3[1]");
            if (headingNode != null)
            {
                if (!string.IsNullOrWhiteSpace(headingNode.InnerText))
                {
                    var text = headingNode.InnerText;
                    var regInput = new Regex("(.*?)(\\<input[^\\>]*\\>)(.*?)");
                    var elig_payer_name = regInput.Replace(text, "");
                    if (!string.IsNullOrWhiteSpace(elig_payer_name))
                    {
                        var regex = new Regex(Regex.Escape(elig_payer_name));
                        htmlStr = regex.Replace(htmlStr, patientInsuranceInformation.Fox_Insurance_Name, 1);
                        //htmlStr = htmlStr.Replace(elig_payer_name, patientInsuranceInformation.Fox_Insurance_Name);
                    }
                }
            }

            return htmlStr;
        }

        public Patient GetCurrentPatientDemographics(long patient_Account, UserProfile profile)
        {
            Patient patient_Details = new Patient();
            var restOfPatientData = new FOX_TBL_PATIENT();
            patient_Details = _PatientRepository.GetSingleOrDefault(e => e.Patient_Account == patient_Account);
            var restOfDetails = _FoxTblPatientRepository.GetSingleOrDefault(e => e.Patient_Account == patient_Details.Patient_Account);
            var activecases = _vwPatientCaseRepository.GetMany(c => c.PATIENT_ACCOUNT == patient_Account && (c.CASE_STATUS_NAME.ToUpper() == "ACT" || c.CASE_STATUS_NAME.ToUpper() == "HOLD") && c.DELETED == false);
            if (patient_Details != null)// if patient is not null, get rest of the info
            {
                if (restOfDetails != null)
                {
                    if (string.IsNullOrEmpty(restOfDetails.Patient_Status) || restOfDetails.Patient_Status == "Inactive")
                    {
                        if (activecases.Count() > 0)
                        {
                            restOfDetails.Patient_Status = "Active";
                            _FoxTblPatientRepository.Update(restOfDetails);
                            _FoxTblPatientRepository.Save();
                        }
                        else if (string.IsNullOrEmpty(restOfDetails.Patient_Status))
                        {
                            restOfDetails.Patient_Status = "Inactive";
                            _FoxTblPatientRepository.Update(restOfDetails);
                            _FoxTblPatientRepository.Save();
                        }
                    }
                }
                patient_Details.FinancialClassList = GetFinancialClassDDValues(profile.PracticeCode.ToString());
                GetRestOfPatientData(patient_Details);
                if (patient_Details.PCP != null && patient_Details.PCP != 0)
                {
                    var pcp = _OrderingRefSourceRepository.GetByID(patient_Details.PCP);
                    if (pcp != null)
                    {
                        patient_Details.PCP_Name = pcp.LAST_NAME + ", " + pcp.FIRST_NAME + " | " + pcp.REFERRAL_REGION;
                        patient_Details.PCP_Notes = pcp.NOTES;
                    }


                    //var pcp = _FoxProviderClassRepository.GetFirst(e => e.FOX_PROVIDER_ID == patient_Details.PCP && !e.DELETED && (e.IS_INACTIVE ?? false) == false);
                    //if (pcp != null)
                    //{
                    //    patient_Details.PCP_Name = pcp.LAST_NAME + ", " + pcp.FIRST_NAME;

                    //    if (!string.IsNullOrWhiteSpace(pcp.INDIVIDUAL_NPI))
                    //    {
                    //        patient_Details.PCP_Name += " | NPI: " + pcp.INDIVIDUAL_NPI;
                    //    }
                    //}
                    //else
                    //{
                    //    patient_Details.PCP = null;
                    //}
                }

                //Get Addresses
                var patient_adresses = _PatientAddressRepository.GetMany(x => x.PATIENT_ACCOUNT == patient_Account && x.DELETED == false).ToList();
                if (patient_adresses != null && patient_adresses.Count() > 0)
                {
                    patient_Details.Patient_Address = new List<PatientAddress>();
                    foreach (var address in patient_adresses)
                    {
                        address.CitiesList = new List<string>();
                        address.CitiesList = GetCitiesByZip(address.ZIP);
                        address.Same_As_POS = address.Same_As_POS ?? false;
                        patient_Details.Patient_Address.Add(address);
                    }
                }

                //POS Details
                bool isFirst = false;
                var profileUser = new UserProfile();
                var locIdList = _PatientPOSLocationRepository.GetManyQueryable(x => x.Patient_Account == patient_Account && x.Deleted == false && x.Loc_ID != 0).OrderByDescending(t => t.Patient_POS_ID).ToList();
                if (locIdList != null && locIdList.Count() > 0)
                {
                    patient_Details.Patient_POS_Location_List = new List<PatientPOSLocation>();
                    foreach (var item in locIdList)
                    {
                        using (var context = new DbContextPatient())
                        {
                            var loc = context.FacilityLocation.Where(i => i.LOC_ID == item.Loc_ID).FirstOrDefault();
                            FacilityType facilityType = new FacilityType();
                            if (loc != null && (loc.FACILITY_TYPE_ID != null && loc.FACILITY_TYPE_ID != 0))
                            {
                                facilityType = _FacilityTypeRepository.GetByID(loc.FACILITY_TYPE_ID);
                                var patientPrivateHomeAddress = _PatientAddressRepository.GetFirst(a => a.PATIENT_POS_ID == item.Patient_POS_ID && a.DELETED == false);
                                if (facilityType != null && facilityType.NAME.ToLower() == "private home")
                                {
                                    if (patientPrivateHomeAddress != null)
                                    {
                                        loc.Address = patientPrivateHomeAddress.ADDRESS;
                                        loc.Zip = patientPrivateHomeAddress.ZIP;
                                        loc.Cell_Phone = patientPrivateHomeAddress.POS_Cell_Phone;
                                        loc.Work_Phone = patientPrivateHomeAddress.POS_Work_Phone;
                                        loc.Phone = patientPrivateHomeAddress.POS_Phone;
                                        loc.Email_Address = patientPrivateHomeAddress.POS_Email_Address;
                                        loc.REGION = patientPrivateHomeAddress.POS_REGION;
                                        loc.Country = patientPrivateHomeAddress.POS_County;
                                        loc.REGION_NAME = patientPrivateHomeAddress.POS_REGION;
                                        loc.City = patientPrivateHomeAddress.CITY;
                                        loc.State = patientPrivateHomeAddress.STATE;
                                        loc.Fax = patientPrivateHomeAddress.POS_Fax;
                                        loc.Latitude = patientPrivateHomeAddress.Latitude;
                                        loc.Longitude = patientPrivateHomeAddress.Longitude;
                                        loc.Country = patientPrivateHomeAddress.COUNTRY;
                                    }
                                }
                                else
                                {
                                    if (patientPrivateHomeAddress != null)
                                    {
                                        loc.Address = patientPrivateHomeAddress.ADDRESS;
                                        loc.Zip = patientPrivateHomeAddress.ZIP;
                                        loc.City = patientPrivateHomeAddress.CITY;
                                        loc.State = patientPrivateHomeAddress.STATE;
                                        loc.Email_Address = patientPrivateHomeAddress.POS_Email_Address;
                                        //loc.Country = patientPrivateHomeAddress.COUNTRY;
                                        loc.Country = patientPrivateHomeAddress.POS_County;
                                    }
                                }
                                if (facilityType != null)
                                {
                                    loc.FACILITY_TYPE_NAME = !string.IsNullOrWhiteSpace(facilityType.DISPLAY_NAME) ? facilityType.DISPLAY_NAME : "";
                                }
                                else
                                {
                                    loc.FACILITY_TYPE_NAME = "";
                                }

                                if (string.IsNullOrWhiteSpace(loc.REGION))
                                {
                                    profileUser.PracticeCode = profile.PracticeCode;
                                    var region = GetRegionByZip(loc.Zip, profileUser);
                                    if (region != null)
                                    {
                                        loc.REGION = region.REFERRAL_REGION_NAME;
                                    }
                                }
                                item.Patient_POS_Details = loc;
                            }

                            patient_Details.Patient_POS_Location_List.Add(item);

                            if (!isFirst && loc != null)
                            {
                                isFirst = true;
                                var obj = new PatientAddress();
                                obj.ADDRESS = loc?.Address ?? "";
                                obj.CITY = loc?.City ?? "";
                                obj.STATE = loc?.State ?? "";
                                obj.ZIP = loc?.Zip ?? "";
                                obj.ADDRESS_TYPE = "pos";
                                if (patient_Details.Patient_Address == null)
                                {
                                    patient_Details.Patient_Address = new List<PatientAddress>();
                                }
                                patient_Details.Patient_Address.Add(obj);
                            }
                        }
                    }
                }
                if (locIdList != null && locIdList.Count > 0)
                {
                    var homePhoneOfPatientPOS = (from pos in _PatientContext.PatientPOSLocation
                                                 join f in _PatientContext.FacilityLocation on pos.Loc_ID equals f.LOC_ID
                                                 join ft in _PatientContext.FaclityTypes on f.FACILITY_TYPE_ID equals ft.FACILITY_TYPE_ID
                                                 where pos.Patient_Account == patient_Account && pos.Deleted == false && pos.Loc_ID != 0 && !string.IsNullOrEmpty(f.Phone)
                                                 && f.PRACTICE_CODE == profile.PracticeCode && f.DELETED && ft.PRACTICE_CODE == profile.PracticeCode && ft.DELETED &&
                                                 (f.POS_Code != "11")
                                                 orderby pos.Created_Date descending
                                                 select f.Phone).FirstOrDefault();
                    //patient_Details.Home_Phone = !string.IsNullOrEmpty(homePhoneOfPatientPOS) && homePhoneOfPatientPOS == patient_Details.Home_Phone ? homePhoneOfPatientPOS : patient_Details.Home_Phone;
                    patient_Details.Home_Phone = patient_Details.Home_Phone;
                    patient_Details.IsHomePhoneFromSLC = !string.IsNullOrEmpty(homePhoneOfPatientPOS) ? true : false;

                }

                //Contact Details
                var conList = _PatientContactRepository.GetManyQueryable(x => x.Patient_Account == patient_Account && x.Deleted == false).ToList();
                if (conList != null && conList.Count() > 0)
                {
                    patient_Details.Patient_Contacts_List = new List<PatientContact>();
                    var typelist = _ContactTypeRepository.GetMany(e => e.Practice_Code == patient_Details.Practice_Code).Select(w => new { w.Contact_Type_ID, w.Type_Name }).ToList();
                    foreach (var item in conList)
                    {
                        item.Flags = SetFlags(item);
                        item.Contact_Type_Name = typelist.Find(e => e.Contact_Type_ID == item.Contact_Type_Id)?.Type_Name ?? "";
                        if (item.Country != null)
                        {
                            var countryres = _CountryRepository.GetFirst(c => c.FOX_TBL_COUNTRY_ID.ToString() == item.Country && !c.DELETED && (c.IS_ACTIVE ?? false));
                            if (countryres != null)
                            {
                                item.CODE = countryres.CODE;
                                item.NAME = countryres.NAME;
                                item.DESCRIPTION = countryres.DESCRIPTION;
                            }
                        }
                        patient_Details.Patient_Contacts_List.Add(item);
                    }
                }

                //Insurance Details
                patient_Details.Current_Patient_Insurances = new List<PatientInsurance>();
                var curr_insurances = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == patient_Account && (x.Deleted ?? false) == false && (string.IsNullOrEmpty(x.FOX_INSURANCE_STATUS) || x.FOX_INSURANCE_STATUS == "C")); //Current
                if (curr_insurances != null && curr_insurances.Count > 0)
                {
                    patient_Details.Current_Patient_Insurances = curr_insurances;

                    for (int i = 0; i < patient_Details.Current_Patient_Insurances.Count; i++)
                    {
                        var insType = patient_Details.Current_Patient_Insurances[i].Pri_Sec_Oth_Type;
                        if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("p"))
                        {
                            patient_Details.Current_Patient_Insurances[i].DisplayOrder = 1;//To display in proper order at fornt-end
                            patient_Details.Current_Patient_Insurances[i].Ins_Type = "Primary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("s"))
                        {
                            patient_Details.Current_Patient_Insurances[i].DisplayOrder = 2;//To display in proper order at fornt-ends
                            patient_Details.Current_Patient_Insurances[i].Ins_Type = "Secondary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("t"))
                        {
                            patient_Details.Current_Patient_Insurances[i].DisplayOrder = 3;//To display in proper order at fornt-end
                            patient_Details.Current_Patient_Insurances[i].Ins_Type = "Tertiary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("q"))
                        {
                            patient_Details.Current_Patient_Insurances[i].DisplayOrder = 4;//To display in proper order at fornt-end
                            patient_Details.Current_Patient_Insurances[i].Ins_Type = "Quaternary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("pr"))
                        {
                            if (patient_Details.Current_Patient_Insurances[i].IS_PRIVATE_PAY ?? false)
                            {
                                patient_Details.Current_Patient_Insurances[i].DisplayOrder = 5;//To display in proper order at fornt-end
                                patient_Details.Current_Patient_Insurances[i].Ins_Type = "Private Pay";
                            }
                            else
                            {
                                patient_Details.Current_Patient_Insurances[i].DisplayOrder = 6;//To display in proper order at fornt-end
                                patient_Details.Current_Patient_Insurances[i].Ins_Type = "Residual Balance";
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("x"))
                        {
                            patient_Details.Current_Patient_Insurances[i].DisplayOrder = 7;//To display in proper order at fornt-end
                            patient_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Primary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("y"))
                        {
                            patient_Details.Current_Patient_Insurances[i].DisplayOrder = 8;//To display in proper order at fornt-end
                            patient_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Secondary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("z"))
                        {
                            patient_Details.Current_Patient_Insurances[i].DisplayOrder = 9;//To display in proper order at fornt-end
                            patient_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Other";
                        }

                        patient_Details.Current_Patient_Insurances[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(patient_Details.Current_Patient_Insurances[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME;

                        if (patient_Details.Current_Patient_Insurances[i].Subscriber.HasValue && patient_Details.Current_Patient_Insurances[i].Subscriber.Value != 0)
                        {
                            var subscriberInfo = _SubscriberRepository.GetByID(curr_insurances[i].Subscriber.Value);
                            if (subscriberInfo != null)
                            {
                                patient_Details.Current_Patient_Insurances[i].SUBSCRIBER_DETAILS = subscriberInfo;
                            }
                        }
                        //patient_Details.Current_Patient_Insurances[i].SUBSCRIBER_DETAILS = _FoxInsurancePayorsRepository.GetByID(patient_Details.Current_Patient_Insurances[i])
                        //patient_Details.Current_Patient_Insurances[i].InsPayer_Description = payorNameList
                        //    .Find(e => e.Patient_Insurance_Id == patient_Details.Current_Patient_Insurances[i].Patient_Insurance_Id).InsPayer_Description;
                    }

                    //To display in proper order on fornt-end
                    patient_Details.Current_Patient_Insurances = patient_Details.Current_Patient_Insurances.OrderBy(e => e.DisplayOrder).ToList();
                }
            }
            patient_Details.IS_PATIENT_INTERFACE_SYNCED = checkPatientisInterfaced(patient_Account, profile);


            return patient_Details;
        }


        public bool checkPatientisInterfaced(long PATIENT_ACCOUNT, UserProfile profile)
        {
            var Patient_interfaced = __InterfaceSynchModelRepository.GetFirst(t => (t.DELETED == false) && t.PRACTICE_CODE == profile.PracticeCode && t.PATIENT_ACCOUNT == PATIENT_ACCOUNT && (t.IS_SYNCED ?? false));
            var patient = _PatientRepository.GetFirst(t => t.Patient_Account == PATIENT_ACCOUNT && !(t.DELETED ?? false));
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
        public Patient GetRestOfPatientData(Patient patient_Details)
        {
            var restOfDetails = _FoxTblPatientRepository.GetFirst(e => e.Patient_Account == patient_Details.Patient_Account);
            if (restOfDetails != null)
            {
                patient_Details.Title = restOfDetails.Title;
                patient_Details.Best_Time_of_Call_Home = restOfDetails.Best_Time_of_Call_Home;
                patient_Details.Best_Time_of_Call_Work = restOfDetails.Best_Time_of_Call_Work;
                patient_Details.Best_Time_of_Call_Cell = restOfDetails.Best_Time_of_Call_Cell;
                patient_Details.Fax_Number = restOfDetails.Fax_Number;
                patient_Details.PCP = restOfDetails.PCP;
                patient_Details.Employment_Status = restOfDetails.Employment_Status;
                patient_Details.Patient_Status = restOfDetails.Patient_Status;
                patient_Details.Student_Status = restOfDetails.Student_Status;
                patient_Details.FINANCIAL_CLASS_ID = restOfDetails.FINANCIAL_CLASS_ID;
                patient_Details.Expired = restOfDetails.Expired;
            }

            return patient_Details;
        }

        public string SetFlags(PatientContact con)
        {
            string flags = "";
            if (con.Flag_Financially_Responsible_Party ?? false)
                flags += "Financially Responsible Party, ";
            if (con.Flag_Preferred_Contact ?? false)
                flags += "Preferred Contact, ";
            if (con.Flag_Emergency_Contact ?? false)
                flags += "Emergency Contact, ";
            if (con.Flag_Power_Of_Attorney ?? false)
                flags += "Power of Attorney, ";
            if (con.Flag_Power_Of_Attorney_Financial ?? false)
                flags += "Power of Attorney Financial, ";
            if (con.Flag_Power_Of_Attorney_Medical ?? false)
                flags += "Power of Attorney Medical, ";
            if (con.Flag_Lives_In_Household_SLC ?? false)
                flags += "Lives in Household or SLC, ";
            if (con.Flag_Service_Location ?? false)
                flags += "Service Location, ";

            if (!string.IsNullOrWhiteSpace(flags))
            {
                flags = flags.Remove(flags.Length - 2);
            }

            return flags;
        }

        async Task<List<POSCoordinates>> GetLatiLan(string address, AccessTokenInfo accessTokenInfo)
        {
            string url = AppConfiguration.AppLatLongLink + "{" + address + "}";
            //string url = $"https://mhealth.mtbc.com/Fox/api/User/GetLatLong?Address={address}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessTokenInfo.token_type, accessTokenInfo.access_token);

                var response = Task.Run(async () => { return await client.GetAsync(url); }).Result;

                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<POSCoordinates>>(result);
                }
            }
        }

        async Task<RespWSGetAccessToken> WSGetAccessToken()
        {
            //GET /api/Fox/GetAccessToken
            //string url = "https://mhealth.mtbc.com/Fox/api/Fox/GetAccessToken";
            string url = AppConfiguration.AppAccessTokenLink;
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



        POSCoordinates GetCoordinates(FacilityLocation loc)
        {
            string add = string.IsNullOrEmpty(loc.Address) ? "" : loc.Address;
            string city = string.IsNullOrEmpty(loc.City) ? "" : loc.City;
            string state = string.IsNullOrEmpty(loc.State) ? "" : loc.State;
            string zipCode = string.Empty;
            if (!string.IsNullOrEmpty(loc.Zip))
            {
                if (loc.Zip.Length > 5)
                {
                    zipCode = loc.Zip.Insert(5, "-");
                }
                else
                {
                    zipCode = loc.Zip;
                }
            }
            //string zipCode = string.IsNullOrEmpty(loc.Zip) ? "" : loc.Zip;

            string address = add + " " + city + " " + state + " " + zipCode;

            if (!string.IsNullOrEmpty(address.Trim()))
            {
                var token = WSGetAccessToken();
                if (token != null && token.Result?.AccessTokenInfo != null)
                {
                    var bb = GetLatiLan(address, token.Result?.AccessTokenInfo);
                    if (bb.Result?.Count > 0)
                    {
                        return bb.Result[0];
                    }
                }
            }
            return null;
        }

        void UpdateCoordinates(FacilityLocation loc, UserProfile profile)
        {
            if (loc != null)
            {
                FacilityLocation pos = _FacilityLocationRepository.GetByID(loc.LOC_ID);
                if (pos != null)
                {
                    if (pos.Latitude == null || pos.Longitude == null)
                    {
                        POSCoordinates coordinates = GetCoordinates(loc);
                        if (coordinates != null)
                        {
                            pos.Longitude = Convert.ToDouble(coordinates.Longitude);
                            pos.Latitude = Convert.ToDouble(coordinates.Latitude);
                            pos.MODIFIED_BY = profile.UserName;
                            pos.MODIFIED_DATE = Helper.GetCurrentDate();
                            _FacilityLocationRepository.Update(pos);
                            _FacilityLocationRepository.Save();
                        }

                    }
                }
            }
        }

        void UpdateCoordinates(PatientPOSLocation loc, UserProfile profile)
        {
            if (loc != null)
            {
                FacilityLocation pos = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == loc.Loc_ID && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);
                if (pos != null)
                {
                    if (pos.Latitude == null || pos.Longitude == null)
                    {
                        POSCoordinates coordinates = GetCoordinates(pos);
                        if (coordinates != null)
                        {
                            pos.Longitude = Convert.ToDouble(coordinates.Longitude);
                            pos.Latitude = Convert.ToDouble(coordinates.Latitude);
                        }
                        pos.MODIFIED_BY = profile.UserName;
                        pos.MODIFIED_DATE = Helper.GetCurrentDate();
                        _FacilityLocationRepository.Update(pos);
                        _FacilityLocationRepository.Save();
                    }
                }

            }
        }
        POSCoordinates AddContactCoordinates(string address)
        {
            var token = WSGetAccessToken();
            if (token != null && token.Result?.AccessTokenInfo != null)
            {
                var bb = GetLatiLan(address, token.Result?.AccessTokenInfo);
                if (bb.Result?.Count > 0)
                {
                    return bb.Result[0];
                }
            }
            return null;
        }




        public List<PatientPOSLocation> AddUpdatePatientPOSLocation(PatientPOSLocation loc, UserProfile profile)
        {
            FOX_TBL_PATIENT foxObj = new FOX_TBL_PATIENT();
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            interfaceSynch.PATIENT_ACCOUNT = Convert.ToInt64(loc.Patient_Account_Str);
            if (loc != null && loc.Patient_POS_Details != null)
            {
                //Set dates first
                if (!string.IsNullOrEmpty(loc.Effective_From_In_String))
                {
                    loc.Effective_From = Convert.ToDateTime(loc.Effective_From_In_String);
                }

                if (!string.IsNullOrEmpty(loc.Effective_To_In_String))
                {
                    loc.Effective_To = Convert.ToDateTime(loc.Effective_To_In_String);
                }

                loc.Patient_Account = Convert.ToInt64(loc.Patient_Account_Str);
                //Update patient home home
                if (loc.Patient_Home_Phone != loc.Patient_POS_Details.Phone)
                {
                    var pat_Rec = _PatientRepository.GetFirst(p => p.Patient_Account == loc.Patient_Account);
                    if (pat_Rec != null)
                    {
                        pat_Rec.Modified_By = profile.UserName;
                        pat_Rec.Modified_Date = Helper.GetCurrentDate();
                        pat_Rec.Home_Phone = loc.Patient_POS_Details.Phone;
                        _PatientRepository.Update(pat_Rec);
                        _PatientRepository.Save();
                    }
                }
                if (loc.IsNew)//If new location added which does not exists in Fox_tbl_active_locations
                {
                    if (loc.Patient_POS_Details.NAME.ToLower() != "private home")
                    {
                        var newCreatedLocId = AddFacilityLocation(loc.Patient_POS_Details, profile);
                        if (newCreatedLocId != 0)//!0 means created successfully and its a bigint value
                        {
                            loc.Patient_POS_ID = Helper.getMaximumId("Fox_Patient_POS_ID");
                            loc.Loc_ID = newCreatedLocId;
                            loc.Created_By = profile.UserName;
                            loc.Created_Date = Helper.GetCurrentDate();
                            loc.Modified_By = profile.UserName;
                            loc.Modified_Date = Helper.GetCurrentDate();
                            loc.Deleted = false;
                            _PatientPOSLocationRepository.Insert(loc);
                            _PatientPOSLocationRepository.Save();
                            SaveGuarantorForPatientFromPOS(loc, profile);
                            //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO
                            //InsertInterfaceTeamData(interfaceSynch, profile);
                        }
                    }
                    else
                    {
                        var facilityOfPrivateHome = _FacilityLocationRepository.GetFirst(f => f.CODE == loc.Patient_POS_Details.CODE && f.REGION == null && loc.Deleted == false && f.PRACTICE_CODE == profile.PracticeCode);
                        if (facilityOfPrivateHome == null)
                        {
                            var privateHomeFacilityType = _FacilityTypeRepository.GetFirst(t => t.NAME.ToLower() == "private home");

                            facilityOfPrivateHome = new FacilityLocation()
                            {
                                LOC_ID = Convert.ToInt64(profile.PracticeCode.ToString() + Helper.getMaximumId("LOC_ID").ToString()),
                                CODE = loc.Patient_POS_Details.CODE,
                                NAME = "Private Home",
                                FACILITY_TYPE_ID = privateHomeFacilityType.FACILITY_TYPE_ID,
                                PRACTICE_CODE = profile.PracticeCode,
                                CREATED_BY = profile.UserName,
                                CREATED_DATE = Helper.GetCurrentDate(),
                                MODIFIED_BY = profile.UserName,
                                MODIFIED_DATE = Helper.GetCurrentDate(),
                                DELETED = false
                            };
                            UpdateCoordinates(facilityOfPrivateHome, profile);
                            _FacilityLocationRepository.Insert(facilityOfPrivateHome);
                            _FacilityLocationRepository.Save();
                            //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO
                            //InsertInterfaceTeamData(interfaceSynch, profile);
                        }
                        loc.Patient_POS_ID = Helper.getMaximumId("Fox_Patient_POS_ID");
                        loc.Loc_ID = facilityOfPrivateHome.LOC_ID;
                        loc.Created_By = profile.UserName;
                        loc.Created_Date = Helper.GetCurrentDate();
                        loc.Modified_By = profile.UserName;
                        loc.Modified_Date = Helper.GetCurrentDate();
                        loc.Deleted = false;

                        _PatientPOSLocationRepository.Insert(loc);
                        _PatientPOSLocationRepository.Save();
                        //this commented temporarygiving error in this method 03/06/2019
                        UpdateCoordinates(facilityOfPrivateHome, profile);
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO
                        //InsertInterfaceTeamData(interfaceSynch, profile);
                        var zip = loc.Patient_POS_Details.Zip.Replace("-", string.Empty);
                        var address = new PatientAddress()
                        {
                            PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID"),
                            ADDRESS_TYPE = null,
                            CITY = loc.Patient_POS_Details.City,
                            ADDRESS = loc.Patient_POS_Details.Address,
                            COUNTRY = loc.Patient_POS_Details.Country,
                            PATIENT_ACCOUNT = Convert.ToInt64(loc.Patient_Account_Str),
                            STATE = loc.Patient_POS_Details.State,
                            ZIP = zip,
                            CREATED_BY = profile.UserName,
                            CREATED_DATE = Helper.GetCurrentDate(),
                            MODIFIED_BY = profile.UserName,
                            MODIFIED_DATE = Helper.GetCurrentDate(),
                            PATIENT_POS_ID = loc.Patient_POS_ID,
                            DELETED = false,
                            POS_Cell_Phone = loc.Patient_POS_Details.Cell_Phone,
                            POS_Email_Address = loc.Patient_POS_Details.Email_Address,
                            POS_Fax = loc.Patient_POS_Details.Fax,
                            POS_Work_Phone = loc.Patient_POS_Details.Work_Phone,
                            POS_Phone = loc.Patient_POS_Details.Phone,
                            POS_REGION = loc.Patient_POS_Details.REGION,
                            POS_County = loc.Patient_POS_Details.Country,
                            Latitude = Convert.ToSingle(facilityOfPrivateHome.Latitude),
                            Longitude = Convert.ToSingle(facilityOfPrivateHome.Longitude)
                        };
                        address = SaveAddressInWebEHRTable(true, address);
                        _PatientAddressRepository.Insert(address);
                        _PatientAddressRepository.Save();
                    }


                }
                else if (loc.IsAddition)
                {
                    loc.Patient_POS_ID = Helper.getMaximumId("Fox_Patient_POS_ID");
                    loc.Created_By = profile.UserName;
                    loc.Created_Date = Helper.GetCurrentDate();
                    loc.Modified_By = profile.UserName;
                    loc.Modified_Date = Helper.GetCurrentDate();
                    loc.Deleted = false;

                    _PatientPOSLocationRepository.Insert(loc);
                    _PatientPOSLocationRepository.Save();

                    // pos entry in patient address

                    var zip = loc.Patient_POS_Details.Zip.Replace("-", string.Empty);
                    var address = new PatientAddress()
                    {
                        PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID"),
                        ADDRESS_TYPE = null,
                        CITY = loc.Patient_POS_Details.City,
                        ADDRESS = loc.Patient_POS_Details.Address,
                        COUNTRY = loc.Patient_POS_Details.Country,
                        PATIENT_ACCOUNT = Convert.ToInt64(loc.Patient_Account_Str),
                        STATE = loc.Patient_POS_Details.State,
                        ZIP = zip,
                        CREATED_BY = profile.UserName,
                        CREATED_DATE = Helper.GetCurrentDate(),
                        MODIFIED_BY = profile.UserName,
                        MODIFIED_DATE = Helper.GetCurrentDate(),
                        PATIENT_POS_ID = loc.Patient_POS_ID,
                        DELETED = false,
                        //POS_Cell_Phone = loc.Patient_POS_Details.Cell_Phone,
                        //POS_Email_Address = loc.Patient_POS_Details.Email_Address,
                        //POS_Fax = loc.Patient_POS_Details.Fax,
                        //POS_Work_Phone = loc.Patient_POS_Details.Work_Phone,
                        //POS_Phone = loc.Patient_POS_Details.Phone,
                        //POS_REGION = loc.Patient_POS_Details.REGION,
                        //POS_County = loc.Patient_POS_Details.Country,
                        //Latitude = facilityOfPrivateHome.Latitude,
                        //Longitude = facilityOfPrivateHome.Longitude
                    };
                    //address = SaveAddressInWebEHRTable(true, address);
                    _PatientAddressRepository.Insert(address);
                    _PatientAddressRepository.Save();

                    // pos entry in patient address

                    //this commented temporarygiving error in this method 03/06/2019
                    UpdateCoordinates(loc.Patient_POS_Details, profile);
                    SaveGuarantorForPatientFromPOS(loc, profile);
                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO
                    //InsertInterfaceTeamData(interfaceSynch, profile);
                }
                else if (loc.IsUpdation)
                {

                    var existingLoc = _PatientPOSLocationRepository.GetByID(loc.Patient_POS_ID);
                    if (existingLoc != null)
                    {
                        if (loc.Patient_POS_Details.NAME.ToLower() == "private home")
                        {
                            var facilityOfPrivateHome = _FacilityLocationRepository.GetFirst(f => f.CODE == loc.Patient_POS_Details.CODE && f.REGION == null && loc.Deleted == false && f.PRACTICE_CODE == profile.PracticeCode);
                            if (facilityOfPrivateHome != null)
                            {
                                loc.Loc_ID = facilityOfPrivateHome.LOC_ID;
                            }
                        }
                        existingLoc.Loc_ID = loc.Loc_ID;
                        //existingLoc.Patient_Account = loc.Loc_ID;

                        existingLoc.Is_Default = loc.Is_Default;
                        existingLoc.Is_Void = loc.Is_Void;
                        existingLoc.Effective_From = loc.Effective_From;
                        existingLoc.Effective_To = loc.Effective_To;
                        existingLoc.Deleted = loc.Deleted ?? false; //In case of delete checkbox in future

                        existingLoc.Modified_By = profile.UserName;
                        existingLoc.Modified_Date = Helper.GetCurrentDate();

                        //if (existingLoc.Patient_POS_Details.Longitude == null && existingLoc.Patient_POS_Details.Latitude == null)
                        //{
                        //    POSCoordinates coordinates = GetCoordinates(existingLoc.Patient_POS_Details);
                        //    if (coordinates != null)
                        //    {
                        //        existingLoc.Patient_POS_Details.Latitude = Convert.ToDouble(coordinates.Latitude);
                        //        existingLoc.Patient_POS_Details.Longitude = Convert.ToDouble(coordinates.Longitude);
                        //    }
                        //}                            
                        _PatientPOSLocationRepository.Update(existingLoc);
                        _PatientPOSLocationRepository.Save();
                        //this commented temporarygiving error in this method 03/06/2019
                        UpdateCoordinates(loc.Patient_POS_Details, profile);
                        SaveGuarantorForPatientFromPOS(loc, profile);
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO
                        //InsertInterfaceTeamData(interfaceSynch, profile);

                        var zip = loc.Patient_POS_Details.Zip.Replace("-", string.Empty);

                        var existingAddress = _PatientAddressRepository.GetFirst(t => t.PATIENT_ACCOUNT == loc.Patient_Account && loc.Patient_POS_ID == t.PATIENT_POS_ID && t.DELETED == false);
                        if (existingAddress != null)
                        {
                            existingAddress.ADDRESS_TYPE = null;
                            existingAddress.CITY = loc.Patient_POS_Details.City;
                            existingAddress.ADDRESS = loc.Patient_POS_Details.Address;
                            existingAddress.COUNTRY = loc.Patient_POS_Details.Country;
                            existingAddress.PATIENT_ACCOUNT = Convert.ToInt64(loc.Patient_Account_Str);
                            existingAddress.STATE = loc.Patient_POS_Details.State;
                            existingAddress.ZIP = zip;
                            existingAddress.CREATED_BY = profile.UserName;
                            existingAddress.CREATED_DATE = Helper.GetCurrentDate();
                            existingAddress.MODIFIED_BY = profile.UserName;
                            existingAddress.MODIFIED_DATE = Helper.GetCurrentDate();
                            existingAddress.PATIENT_POS_ID = loc.Patient_POS_ID;
                            existingAddress.DELETED = false;
                            //existingAddress.POS_Cell_Phone = loc.Patient_POS_Details.Cell_Phone;
                            //existingAddress.POS_Email_Address = loc.Patient_POS_Details.Email_Address;
                            //existingAddress.POS_Fax = loc.Patient_POS_Details.Fax;
                            //existingAddress.POS_Work_Phone = loc.Patient_POS_Details.Work_Phone;
                            //existingAddress.POS_Phone = loc.Patient_POS_Details.Phone;
                            if (loc.Patient_POS_Details.NAME.ToLower() == "private home")
                            {
                                existingAddress.POS_REGION = loc.Patient_POS_Details.REGION;
                                existingAddress.POS_County = loc.Patient_POS_Details.Country;
                            }

                            //existingAddress.POS_County = loc.Patient_POS_Details.Country;

                            //address = SaveAddressInWebEHRTable(true, address);
                            _PatientAddressRepository.Update(existingAddress);
                            _PatientAddressRepository.Save();
                        }

                    }
                }
                //unmark other defaults
                if (loc.Is_Default ?? false)
                {
                    UpdateDefaultLocations(loc.Loc_ID, loc.Patient_Account, loc.Patient_POS_ID);
                    //if default then update the home address address in patient demographics
                    var res = (from f in _PatientContext.FacilityLocation
                               join ft in _PatientContext.FaclityTypes on f.FACILITY_TYPE_ID equals ft.FACILITY_TYPE_ID into fft
                               from ft in fft.DefaultIfEmpty()
                               join pos in _PatientContext.PatientPOSLocation on f.LOC_ID equals pos.Loc_ID
                               join a in _PatientContext.PatientAddress on pos.Patient_POS_ID equals a.PATIENT_POS_ID into posa
                               from a in posa.DefaultIfEmpty()
                               where pos.Patient_POS_ID == loc.Patient_POS_ID && !(pos.Deleted ?? false) && ft.NAME.ToLower() == "private home"
                               select new FacilityLocationViewModel()
                               {
                                   Address = !string.IsNullOrEmpty(f.Address) ? f.Address : a.ADDRESS,
                                   Zip = !string.IsNullOrEmpty(f.Zip) ? f.Zip : a.ZIP,
                                   City = !string.IsNullOrEmpty(f.City) ? f.City : a.CITY,
                                   State = !string.IsNullOrEmpty(f.State) ? f.State : a.STATE
                               }).FirstOrDefault();
                    if (res != null)
                    {
                        var homeAddress = _PatientAddressRepository.GetFirst(a => a.ADDRESS_TYPE == "Home Address" && !(a.DELETED ?? false) && a.PATIENT_ACCOUNT == loc.Patient_Account);
                        if (homeAddress == null)
                        {
                            var address = new PatientAddress()
                            {
                                PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID"),
                                STATE = res.State,
                                ADDRESS = res.Address,
                                ADDRESS_TYPE = "Home Address",
                                CITY = res.City,
                                DELETED = false,
                                CREATED_BY = profile.UserName,
                                CREATED_DATE = Helper.GetCurrentDate(),
                                MODIFIED_BY = profile.UserName,
                                MODIFIED_DATE = Helper.GetCurrentDate(),
                                ZIP = res.Zip,
                                PATIENT_ACCOUNT = loc.Patient_Account
                            };
                            address = SaveAddressInWebEHRTable(true, address);
                            _PatientAddressRepository.Insert(address);
                            _PatientAddressRepository.Save();
                        }
                        else
                        {
                            homeAddress.CITY = res.City;
                            homeAddress.ADDRESS = res.Address;
                            homeAddress.STATE = res.State;
                            homeAddress.ZIP = res.Zip;
                            homeAddress.MODIFIED_BY = profile.UserName;
                            homeAddress.MODIFIED_DATE = Helper.GetCurrentDate();
                            _PatientAddressRepository.Update(homeAddress);
                            _PatientAddressRepository.Save();
                            SaveAddressInWebEHRTable(false, homeAddress);
                        }
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //InsertInterfaceTeamData(interfaceSynch, profile);
                    }
                    var openHoldCases = (from c in _CaseContext.Case
                                         join ct in _CaseContext.CaseType on c.CASE_TYPE_ID equals ct.CASE_TYPE_ID into cct
                                         from ct in cct.DefaultIfEmpty()
                                         join cs in _CaseContext.Status on c.CASE_STATUS_ID equals cs.CASE_STATUS_ID into ccs
                                         from cs in ccs.DefaultIfEmpty()
                                         where (cs.NAME.ToLower() == "open" || cs.NAME.ToLower() == "hold") && (c.PATIENT_ACCOUNT == loc.Patient_Account) && c.DELETED == false && c.PRACTICE_CODE == profile.PracticeCode
                                         select c).ToList();
                    var region = _DbContextSecurity.ReferralRegions.Where(r => r.REFERRAL_REGION_NAME.ToLower() == loc.Patient_POS_Details.REGION.ToLower()).FirstOrDefault();
                    foreach (var c in openHoldCases)
                    {
                        c.TREATING_REGION_ID = region != null && (c.TREATING_PROVIDER_ID == null || c.TREATING_PROVIDER_ID == 0) && c.IS_MANUAL_CHANGE_REGION == false ? region.REFERRAL_REGION_ID : c.TREATING_REGION_ID;
                        c.POS_ID = loc.Loc_ID;
                        c.MODIFIED_BY = profile.UserName;
                        c.MODIFIED_DATE = Helper.GetCurrentDate();
                        _caseRepository.Update(c);
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //InsertInterfaceTeamData(interfaceSynch, profile);
                    }
                    _caseRepository.Save();
                }
            }

            //Get Latest POS Details
            var list = new List<PatientPOSLocation>();
            var locIdList = _PatientPOSLocationRepository.GetManyQueryable(x => x.Patient_Account == loc.Patient_Account && (x.Deleted ?? false) == false && x.Loc_ID != 0).ToList();
            if (locIdList != null && locIdList.Count() > 0)
            {
                foreach (var item in locIdList)
                {
                    FacilityLocationViewModel location = _FacilityLocationRepository.GetMany(t => t.LOC_ID == item.Loc_ID)
                                                            .Select(t => new FacilityLocationViewModel()
                                                            {
                                                                LOC_ID = t.LOC_ID,
                                                                ROW = t.ROW,
                                                                CODE = t.CODE,
                                                                NAME = t.NAME,
                                                                Address = t.Address,
                                                                COMPLETE_ADDRESS = t.COMPLETE_ADDRESS,
                                                                Country = t.Country,
                                                                REGION = t.REGION,
                                                                REGION_NAME = t.REGION_NAME,
                                                                Phone = t.Phone,
                                                                Fax = t.Fax,
                                                                POS_Code = t.POS_Code,
                                                                Capacity = t.Capacity,
                                                                Census = t.Census,
                                                                PT = t.PT,
                                                                OT = t.OT,
                                                                ST = t.ST,
                                                                EP = t.EP,
                                                                Lead = t.Lead,
                                                                Parent = t.Parent,
                                                                Description = t.Description,
                                                                City = t.City,
                                                                State = t.State,
                                                                Zip = t.Zip,
                                                                FACILITY_TYPE_ID = t.FACILITY_TYPE_ID,
                                                                Last_Update = t.Last_Update,
                                                                CREATED_BY = t.CREATED_BY,
                                                                CREATED_DATE = t.CREATED_DATE,
                                                                Created_Date_Str = t.Created_Date_Str,
                                                                CREATED_DATE_STRING = t.CREATED_DATE_STRING,
                                                                MODIFIED_BY = t.MODIFIED_BY,
                                                                MODIFIED_DATE = t.MODIFIED_DATE,
                                                                Modified_Date_Str = t.Modified_Date_Str,
                                                                MODIFIED_DATE_STRING = t.MODIFIED_DATE_STRING,
                                                                DELETED = t.DELETED,
                                                                TOTAL_RECORD_PAGES = t.TOTAL_RECORD_PAGES,
                                                                TOTAL_RECORDS = t.TOTAL_RECORDS,
                                                                PRACTICE_CODE = t.PRACTICE_CODE,
                                                                Work_Phone = t.Work_Phone,
                                                                Cell_Phone = t.Cell_Phone,
                                                                Email_Address = t.Email_Address,
                                                                AddressType = t.AddressType,
                                                                Inactive = t.Inactive,
                                                                IS_ACTIVE = t.IS_ACTIVE,
                                                                FACILITY_TYPE_NAME = t.FACILITY_TYPE_NAME,
                                                                PT_PROVIDER_ID = t.PT_PROVIDER_ID,
                                                                OT_PROVIDER_ID = t.OT_PROVIDER_ID,
                                                                ST_PROVIDER_ID = t.ST_PROVIDER_ID,
                                                                EP_PROVIDER_ID = t.EP_PROVIDER_ID,
                                                                LEAD_PROVIDER_ID = t.LEAD_PROVIDER_ID,
                                                                PATIENT_POS_ID = t.PATIENT_POS_ID,
                                                                Is_Void = t.Is_Void,
                                                                Is_Default = t.Is_Default,
                                                                Effective_From = t.Effective_From,
                                                                Effective_To = t.Effective_To,
                                                                Longitude = t.Longitude,
                                                                Latitude = t.Latitude
                                                            })
                                                            .FirstOrDefault();

                    //if (location != null && location.CODE != null && location.CODE.ToLower().Contains("hom"))
                    if (location != null)
                    {
                        if (location.CODE != null && location.CODE.ToLower().Contains("hom"))
                        {
                            var PrivatelocationAddress = _PatientAddressRepository.GetFirst(t => t.PATIENT_ACCOUNT == loc.Patient_Account && t.PATIENT_POS_ID == item.Patient_POS_ID && t.DELETED == false);
                            if (PrivatelocationAddress != null)
                            {
                                location.Address = PrivatelocationAddress.ADDRESS;
                                location.Zip = PrivatelocationAddress.ZIP;
                                location.City = PrivatelocationAddress.CITY;
                                location.State = PrivatelocationAddress.STATE;
                                location.Phone = PrivatelocationAddress.POS_Phone;
                                location.Work_Phone = PrivatelocationAddress.POS_Work_Phone;
                                location.Cell_Phone = PrivatelocationAddress.POS_Cell_Phone;
                                location.Fax = PrivatelocationAddress.POS_Fax;
                                location.Email_Address = PrivatelocationAddress.POS_Email_Address;
                            }
                            else
                            {
                                location.Address = PrivatelocationAddress.ADDRESS;
                                location.Zip = PrivatelocationAddress.ZIP;
                                location.City = PrivatelocationAddress.CITY;
                                location.State = PrivatelocationAddress.STATE;
                                location.Country = PrivatelocationAddress.POS_County;
                            }
                        }
                    }
                    if (item.Patient_POS_Details != null)
                    {
                        item.Patient_POS_Details.LOC_ID = location.LOC_ID;
                        item.Patient_POS_Details.NAME = location.NAME;
                        item.Patient_POS_Details.CODE = location.CODE;
                        item.Patient_POS_Details.COMPLETE_ADDRESS = location.COMPLETE_ADDRESS;
                        item.Patient_POS_Details.Country = location.Country;
                        item.Patient_POS_Details.REGION = location.REGION;
                        item.Patient_POS_Details.POS_Code = location.POS_Code;
                        item.Patient_POS_Details.Capacity = location.Capacity;
                        item.Patient_POS_Details.Census = location.Census;
                        item.Patient_POS_Details.PT = location.PT;
                        item.Patient_POS_Details.OT = location.OT;
                        item.Patient_POS_Details.ST = location.ST;
                        item.Patient_POS_Details.EP = location.EP;
                        item.Patient_POS_Details.Lead = location.Lead;
                        item.Patient_POS_Details.Parent = location.Parent;
                        item.Patient_POS_Details.Description = location.Description;
                        item.Patient_POS_Details.FACILITY_TYPE_ID = location.FACILITY_TYPE_ID;
                        item.Patient_POS_Details.FACILITY_TYPE_NAME = location.FACILITY_TYPE_NAME;
                        item.Patient_POS_Details.PATIENT_POS_ID = location.PATIENT_POS_ID;
                        item.Patient_POS_Details.Is_Void = location.Is_Void;
                        item.Patient_POS_Details.Is_Default = location.Is_Default;
                        item.Patient_POS_Details.Effective_From = location.Effective_From;
                        item.Patient_POS_Details.Effective_To = location.Effective_To;
                        item.Patient_POS_Details.IS_ACTIVE = location.IS_ACTIVE;
                        item.Patient_POS_Details.DELETED = location.DELETED;
                        item.Patient_POS_Details.Inactive = location.Inactive;
                        item.Patient_POS_Details.Address = location.Address;
                        item.Patient_POS_Details.Zip = location.Zip;
                        item.Patient_POS_Details.City = location.City;
                        item.Patient_POS_Details.State = location.State;
                        item.Patient_POS_Details.Phone = location.Phone;
                        item.Patient_POS_Details.Work_Phone = location.Work_Phone;
                        item.Patient_POS_Details.Cell_Phone = location.Cell_Phone;
                        item.Patient_POS_Details.Fax = location.Fax;
                        item.Patient_POS_Details.Email_Address = location.Email_Address;
                        item.Patient_POS_Details.Longitude = location.Longitude;
                        item.Patient_POS_Details.Latitude = location.Latitude;
                        item.Patient_POS_Details.LEAD_PROVIDER_ID = location.LEAD_PROVIDER_ID;
                        item.Patient_POS_Details.PT_PROVIDER_ID = location.PT_PROVIDER_ID;
                        item.Patient_POS_Details.OT_PROVIDER_ID = location.OT_PROVIDER_ID;
                        item.Patient_POS_Details.ST_PROVIDER_ID = location.ST_PROVIDER_ID;
                        item.Patient_POS_Details.EP_PROVIDER_ID = location.EP_PROVIDER_ID;
                        item.Patient_POS_Details.AddressType = location.AddressType;
                        item.Patient_POS_Details.PRACTICE_CODE = location.PRACTICE_CODE;
                        item.Patient_POS_Details.TOTAL_RECORD_PAGES = location.TOTAL_RECORD_PAGES;
                        item.Patient_POS_Details.TOTAL_RECORDS = location.TOTAL_RECORDS;
                        item.Patient_POS_Details.MODIFIED_BY = location.MODIFIED_BY;
                        item.Patient_POS_Details.MODIFIED_DATE = location.MODIFIED_DATE;
                        item.Patient_POS_Details.CREATED_DATE = location.CREATED_DATE;
                        item.Patient_POS_Details.CREATED_BY = location.CREATED_BY;
                        item.Patient_POS_Details.REGION_NAME = location.REGION_NAME;
                    }

                    //item.Patient_POS_Details_List.Add(loc);
                    list.Add(item);
                }
            }

            if (loc.Is_Default ?? false)
            {
                var statementAddress = _PatientAddressRepository.GetFirst(a => a.ADDRESS_TYPE == "Statement Address" && !(a.DELETED ?? false) && a.PATIENT_ACCOUNT == loc.Patient_Account);
                if (statementAddress == null)
                {
                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(interfaceSynch, profile);
                    var zip = loc.Patient_POS_Details.Zip?.Replace("-", string.Empty) ?? "";
                    var address = new PatientAddress()
                    {
                        PATIENT_ACCOUNT = loc.Patient_Account,
                        ADDRESS_TYPE = "Statement Address",
                        PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID"),
                        CREATED_BY = profile.UserName,
                        CREATED_DATE = Helper.GetCurrentDate(),
                        MODIFIED_BY = profile.UserName,
                        MODIFIED_DATE = Helper.GetCurrentDate(),
                        DELETED = false,
                        ADDRESS = loc.Patient_POS_Details?.Address ?? "",
                        CITY = (loc.Patient_POS_Details?.City ?? "").ToUpper(),
                        Same_As_POS = true,
                        STATE = loc.Patient_POS_Details?.State ?? "",
                        ZIP = zip
                    };
                    address = SaveAddressInWebEHRTable(true, address);
                    _PatientAddressRepository.Insert(address);
                    _PatientAddressRepository.Save();
                }
                else
                {
                    var zip = loc.Patient_POS_Details.Zip?.Replace("-", string.Empty) ?? "";
                    statementAddress.MODIFIED_BY = profile.UserName;
                    statementAddress.MODIFIED_DATE = Helper.GetCurrentDate();
                    statementAddress.DELETED = false;
                    statementAddress.ADDRESS = loc.Patient_POS_Details?.Address ?? "";
                    statementAddress.CITY = (loc.Patient_POS_Details?.City ?? "").ToUpper();
                    statementAddress.STATE = loc.Patient_POS_Details?.State ?? "";
                    statementAddress.ZIP = zip;
                    _PatientAddressRepository.Update(statementAddress);
                    _PatientAddressRepository.Save();
                    SaveAddressInWebEHRTable(false, statementAddress);
                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(interfaceSynch, profile);
                }
            }
            CheckAndUpdatePRSubscriber(loc.Patient_Account_Str, profile);
            UpdateSubscriberForAllOtherInsurancesIfRelationSelfExceptPR(loc.Patient_Account_Str, profile);
            return list;
        }

        public void UpdateDefaultLocations(long defaultLocId, long patAcc, long POS_id)
        {
            var locIdList = _PatientPOSLocationRepository.GetManyQueryable(x => x.Patient_Account == patAcc && x.Patient_POS_ID != POS_id && x.Is_Default.HasValue && x.Is_Default.Value).ToList();//Get other defaults
            if (locIdList != null && locIdList.Count() > 0)
            {
                foreach (var item in locIdList)
                {
                    item.Is_Default = false;
                    item.Effective_To = Helper.GetCurrentDate();
                    _PatientPOSLocationRepository.Update(item);
                    _PatientPOSLocationRepository.Save();
                }
            }
        }

        public long AddFacilityLocation(FacilityLocation facilityLocation, UserProfile profile)
        {
            FOX_TBL_PATIENT foxObj = new FOX_TBL_PATIENT();
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            var dbFacilityLocation = _FacilityLocationRepository.GetByID(facilityLocation.LOC_ID);
            if (dbFacilityLocation == null)
            {
                //POSCoordinates coordinates = GetCoordinates(facilityLocation);
                //if (coordinates != null)
                //{
                //    facilityLocation.Latitude = Convert.ToDouble(coordinates.Latitude);
                //    facilityLocation.Longitude = Convert.ToDouble(coordinates.Longitude);
                //}                    
                facilityLocation.PRACTICE_CODE = profile.PracticeCode;
                facilityLocation.LOC_ID = Convert.ToInt64(profile.PracticeCode.ToString() + Helper.getMaximumId("LOC_ID").ToString());
                facilityLocation.CREATED_BY = profile.UserName;
                facilityLocation.CREATED_DATE = Helper.GetCurrentDate();
                facilityLocation.MODIFIED_BY = profile.UserName;
                facilityLocation.MODIFIED_DATE = Helper.GetCurrentDate();
                facilityLocation.DELETED = false;
                _FacilityLocationRepository.Insert(facilityLocation);
                _FacilityLocationRepository.Save();
                //this commented temporarygiving error in this method 03/06/2019
                UpdateCoordinates(facilityLocation, profile);
            }
            return facilityLocation.LOC_ID;
        }

        public List<ContactTypesForDropdown> GetPatientContactTypes(long practiceCode)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var conTypes = SpRepository<ContactTypesForDropdown>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_Contact_Types @PRACTICE_CODE", parmPracticeCode);
            return conTypes;
        }
        public List<BestTimeToCallForDropdown> GetPatientBestTimeToCall(long practiceCode)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var bestTimes = SpRepository<BestTimeToCallForDropdown>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_BEST_TIME_TO_CALL] @PRACTICE_CODE", parmPracticeCode);
            return bestTimes;
        }

        public List<ContactType> GetAllPatientContactTypes(UserProfile profile)
        {
            List<ContactType> conTypes = new List<ContactType>();
            //if (profile.isTalkRehab)
            //{
            //    conTypes = _ContactTypeRepository.GetMany(x => !(x.Deleted ?? false)).ToList();
            //}
            //else
            //{
            conTypes = _ContactTypeRepository.GetMany(x => !(x.Deleted ?? false) && x.Practice_Code == profile.PracticeCode).ToList();
            // }

            if (conTypes.Any())
            {
                return conTypes;
            }
            else
            {
                return new List<ContactType>();
            }
        }
        public PatientContact GetPatientContactDetails(long contactid)
        {
            PatientContact contact = new PatientContact();
            contact = _PatientContactRepository.GetSingle(e => e.Contact_ID == contactid);

            if (contact.Country != null)
            {
                var countryres = _CountryRepository.GetFirst(c => c.FOX_TBL_COUNTRY_ID.ToString() == contact.Country && !c.DELETED && (c.IS_ACTIVE ?? false));
                if (countryres != null)
                {
                    contact.CODE = countryres.CODE;
                    contact.NAME = countryres.NAME;
                    contact.DESCRIPTION = countryres.DESCRIPTION;
                }
            }

            if (contact != null)
            {
                contact.CitiesList = new List<string>();
                contact.CitiesList = !string.IsNullOrWhiteSpace(contact.Zip) ? GetCitiesByZip(contact.Zip) : contact.CitiesList;
                return contact;
            }

            return null;
        }

        public PatientContact SaveContact(PatientContact contact, UserProfile profile)
        {
            string oldFname = string.Empty;
            string oldLname = string.Empty;
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            interfaceSynch.PATIENT_ACCOUNT = long.Parse(contact.Patient_Account_Str);

            if (!string.IsNullOrEmpty(contact.Start_Date_In_String))
            {
                contact.Start_Date = Convert.ToDateTime(contact.Start_Date_In_String);
            }

            if (!string.IsNullOrEmpty(contact.End_Date_In_String))
            {
                contact.End_Date = Convert.ToDateTime(contact.End_Date_In_String);
            }

            //  if (contact.STATEMENT_ADDRESS_MARKED && (contact.Flag_Financially_Responsible_Party.HasValue ? contact.Flag_Financially_Responsible_Party.Value : false || contact.Flag_Power_Of_Attorney_Financial.HasValue ? contact.Flag_Power_Of_Attorney_Financial.Value : false))
            //Change By arqam
            if (contact.STATEMENT_ADDRESS_MARKED && (contact.Flag_Financially_Responsible_Party.HasValue ? contact.Flag_Financially_Responsible_Party.Value : false))
            {
                contact.PopulateStatementAddress = true;
            }
            else
            {
                contact.PopulateStatementAddress = false;
            }

            var dbContact = _PatientContactRepository.GetByID(contact.Contact_ID);
            if (dbContact != null)
            {
                oldFname = dbContact.First_Name;
                oldLname = dbContact.Last_Name;
            }

            if (dbContact == null) //Add
            {
                dbContact = contact;
                contact.Contact_ID = Helper.getMaximumId("Fox_Patient_Contact_ID");
                contact.Created_By = contact.Modified_By = profile.UserName;
                contact.Created_On = contact.Modified_On = Helper.GetCurrentDate();

                contact.Deleted = false;
                _PatientContactRepository.Insert(contact);
                _PatientContactRepository.Save();
                CheckAndUpdatePRSubscriber(contact.Patient_Account_Str, profile);
            }
            else
            {
                UpdateFinancialGuarantorInPatientFromContact(dbContact, contact, profile);
                dbContact.Contact_Type_Id = contact.Contact_Type_Id;
                dbContact.First_Name = contact.First_Name;
                dbContact.MI = contact.MI;
                dbContact.Last_Name = contact.Last_Name;
                dbContact.Room = contact.Room;
                dbContact.Address = contact.Address;
                dbContact.Is_Same_Patient_Address = contact.Is_Same_Patient_Address;
                dbContact.Zip = contact.Zip;
                dbContact.City = contact.City;
                dbContact.State = contact.State;
                dbContact.Country = contact.Country;
                dbContact.Email_Address = contact.Email_Address;
                dbContact.Fax_Number = contact.Fax_Number;
                dbContact.Home_Phone = contact.Home_Phone;
                dbContact.Call_Home = contact.Call_Home;
                dbContact.Best_Time_Call_Home = contact.Best_Time_Call_Home;
                dbContact.Preferred_Contact = contact.Preferred_Contact;
                dbContact.Work_Phone = contact.Work_Phone;
                dbContact.EXT_WORK_PHONE = contact.EXT_WORK_PHONE;
                dbContact.Call_Work = contact.Call_Work;
                dbContact.Best_Time_Call_Work = contact.Best_Time_Call_Work;
                dbContact.Cell_Phone = contact.Cell_Phone;
                dbContact.Call_Cell = contact.Call_Cell;
                dbContact.Best_Time_Call_Cell = contact.Best_Time_Call_Cell;
                dbContact.Preferred_Delivery_Method = contact.Preferred_Delivery_Method;

                dbContact.Start_Date = contact.Start_Date;
                dbContact.End_Date = contact.End_Date;
                dbContact.Marketing_Referral_Source = contact.Marketing_Referral_Source;
                dbContact.Flag_Financially_Responsible_Party = contact.Flag_Financially_Responsible_Party;
                dbContact.Flag_Preferred_Contact = contact.Flag_Preferred_Contact;
                dbContact.Flag_Emergency_Contact = contact.Flag_Emergency_Contact;
                dbContact.Flag_Power_Of_Attorney = contact.Flag_Power_Of_Attorney;
                dbContact.Flag_Power_Of_Attorney_Financial = contact.Flag_Power_Of_Attorney_Financial;
                dbContact.Flag_Power_Of_Attorney_Medical = contact.Flag_Power_Of_Attorney_Medical;

                dbContact.Flag_Lives_In_Household_SLC = contact.Flag_Lives_In_Household_SLC;
                dbContact.Flag_Service_Location = contact.Flag_Service_Location;
                dbContact.Deleted = contact.Deleted;
                dbContact.STATEMENT_ADDRESS_MARKED = contact.STATEMENT_ADDRESS_MARKED;

                dbContact.Modified_By = profile.UserName;
                dbContact.Modified_On = Helper.GetCurrentDate();
                _PatientContactRepository.Update(dbContact);
                _PatientContactRepository.Save();

                CheckAndUpdatePRSubscriber(contact.Patient_Account_Str, profile);
            }
            if (profile.isTalkRehab)
            {
                if (contact.Contact_Type_Id == 600109)
                {
                    CreateUpdateTalkRehabContactsGuarantor(contact, profile, oldFname, oldLname);
                }
                else if (contact.Contact_Type_Id == 600115)
                {
                    CreateUpdateCareTeam(contact, profile, oldFname, oldLname);
                }
                else
                {
                    CreateUpdateTalkRehabContactsNextOfKin(contact, profile, oldFname, oldLname);
                }
            }

            //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir  
            //InsertInterfaceTeamData(interfaceSynch, profile);
            if (contact.PopulateStatementAddress)
            {
                PopulateStatementAddress(contact, profile);
                SaveGuarantorForPatientFromContact(contact, profile);
            }

            return contact;
        }
        private void CreateUpdateCareTeam(PatientContact patientContactObj, UserProfile profile, string oldFname, string oldLname)
        {
            WebehrTblPatientCareTeams dbContact = _WebehrTblPatientCareTeamsRepository.GetFirst(patientContact => patientContact.Patient_Account == patientContactObj.Patient_Account && patientContact.FirstName == oldFname && patientContact.LastName == oldLname);

            if (dbContact == null)
            {
                WebehrTblPatientCareTeams patientCareObj = new WebehrTblPatientCareTeams();
                patientCareObj.PatientCareTeamID = Helper.getMaximumId("PatientCareTeamID");

                patientCareObj.FirstName = patientContactObj.First_Name;
                patientCareObj.LastName = patientContactObj.Last_Name;
                patientCareObj.Patient_Account = patientContactObj.Patient_Account;

                if (patientContactObj.Home_Phone != null)
                {
                    patientCareObj.Phone = patientContactObj.Home_Phone;
                }
                else
                {
                    if (patientContactObj.Work_Phone != null)
                    {
                        patientCareObj.Phone = patientContactObj.Work_Phone;
                    }
                    else
                    {
                        patientCareObj.Phone = patientContactObj.Cell_Phone;
                    }
                }

                patientCareObj.Address = patientContactObj.Address;
                patientCareObj.Zip = patientContactObj.Zip;
                patientCareObj.City = patientContactObj.City;
                patientCareObj.State = patientContactObj.State;


                if (patientContactObj.Contact_Type_Id == 600115)
                {
                    patientCareObj.REFERRINGPROVIDER = true;
                }
                patientCareObj.Created_Date = DateTime.Now.Date;
                patientCareObj.Modified_Date = DateTime.Now;
                patientCareObj.Created_By = profile.UserName;
                patientCareObj.Modified_By = profile.UserName;
                patientCareObj.Practice_Code = profile.PracticeCode;
                patientCareObj.Deleted = false;
                _WebehrTblPatientCareTeamsRepository.Insert(patientCareObj);
            }
            else
            {
                dbContact.FirstName = patientContactObj.First_Name;
                dbContact.LastName = patientContactObj.Last_Name;
                dbContact.Patient_Account = patientContactObj.Patient_Account;

                if (patientContactObj.Home_Phone != null)
                {
                    dbContact.Phone = patientContactObj.Home_Phone;
                }
                else
                {
                    if (patientContactObj.Work_Phone != null)
                    {
                        dbContact.Phone = patientContactObj.Work_Phone;
                    }
                    else
                    {
                        dbContact.Phone = patientContactObj.Cell_Phone;
                    }
                }

                dbContact.Address = patientContactObj.Address;
                dbContact.Zip = patientContactObj.Zip;
                dbContact.City = patientContactObj.City;
                dbContact.State = patientContactObj.State;

                if (patientContactObj.Contact_Type_Id == 600115)
                {
                    dbContact.REFERRINGPROVIDER = true;
                }
                dbContact.Created_Date = DateTime.Now.Date;
                dbContact.Modified_Date = DateTime.Now;
                dbContact.Created_By = profile.UserName;
                dbContact.Modified_By = profile.UserName;
                _WebehrTblPatientCareTeamsRepository.Update(dbContact);
            }
            _WebehrTblPatientCareTeamsRepository.Save();
        }
        private void CreateUpdateCareTeam(Patient patientContactObj, UserProfile profile, string oldFname = "", string oldLname = "")
        {
            WebehrTblPatientCareTeams dbContact = _WebehrTblPatientCareTeamsRepository.GetFirst(patientContact => patientContact.Patient_Account == patientContactObj.Patient_Account && patientContact.FirstName == oldFname && patientContact.LastName == oldLname);

            if (dbContact == null && patientContactObj.SmartOrderSource != null)
            {
                WebehrTblPatientCareTeams patientCareObj = new WebehrTblPatientCareTeams();
                patientCareObj.PatientCareTeamID = Helper.getMaximumId("PatientCareTeamID");

                patientCareObj.FirstName = patientContactObj.SmartOrderSource.FIRST_NAME;
                patientCareObj.LastName = patientContactObj.SmartOrderSource.LAST_NAME;
                patientCareObj.Patient_Account = patientContactObj.Patient_Account;

                patientCareObj.Phone = patientContactObj.SmartOrderSource.PHONE;
                patientCareObj.EMAIL = patientContactObj.SmartOrderSource.Email;

                patientCareObj.Address = patientContactObj.SmartOrderSource.ADDRESS;
                patientCareObj.Zip = patientContactObj.SmartOrderSource.ZIP;
                patientCareObj.City = patientContactObj.SmartOrderSource.CITY;
                patientCareObj.State = patientContactObj.SmartOrderSource.STATE;
                patientCareObj.NPI = patientContactObj.SmartOrderSource.NPI;

                if (patientContactObj.PCP != null)
                {
                    patientCareObj.PCP = true;
                }
                patientCareObj.Created_Date = DateTime.Now.Date;
                patientCareObj.Modified_Date = DateTime.Now;
                patientCareObj.Created_By = profile.UserName;
                patientCareObj.Modified_By = profile.UserName;
                patientCareObj.Practice_Code = profile.PracticeCode;
                patientCareObj.Deleted = false;
                _WebehrTblPatientCareTeamsRepository.Insert(patientCareObj);
            }
            else
            {
                dbContact.FirstName = patientContactObj.First_Name;
                dbContact.LastName = patientContactObj.Last_Name;
                dbContact.Patient_Account = patientContactObj.Patient_Account;

                if (patientContactObj.Home_Phone != null)
                {
                    dbContact.Phone = patientContactObj.Home_Phone;
                }
                else
                {
                    if (patientContactObj.Business_Phone != null)
                    {
                        dbContact.Phone = patientContactObj.Business_Phone;
                    }
                    else
                    {
                        dbContact.Phone = patientContactObj.cell_phone;
                    }
                }

                dbContact.Address = patientContactObj.Address;
                dbContact.Zip = patientContactObj.ZIP;
                dbContact.City = patientContactObj.City;
                dbContact.State = patientContactObj.State;
                dbContact.Fax = patientContactObj.Fax_Number;
                dbContact.EMAIL = patientContactObj.Email_Address;



                if (patientContactObj.PCP != null)
                {
                    dbContact.PCP = true;
                }
                dbContact.Modified_Date = DateTime.Now;
                dbContact.Modified_By = profile.UserName;
                _WebehrTblPatientCareTeamsRepository.Update(dbContact);
            }
            _WebehrTblPatientCareTeamsRepository.Save();
        }
        private bool CreateUpdateTalkRehabContactsNextOfKin(PatientContact patientContactObj, UserProfile profile, string oldFname, string oldLname)
        {
            var currentDate = Helper.GetCurrentDate();

            AF_TBL_PATIENT_NEXT_OF_KIN nextOfKin = new AF_TBL_PATIENT_NEXT_OF_KIN();
            var dbContact = _AF_TBL_PATIENT_NEXT_OF_KINRepository.GetFirst(patientContact => patientContact.PATIENT_ACCOUNT == patientContactObj.Patient_Account && patientContact.FIRSTNAME == oldFname && patientContact.LASTNAME == oldLname);
            if (dbContact == null)
            {
                nextOfKin.PATIENT_NEXT_OF_KIN_ID = Helper.getMaximumId("PATIENT_NEXT_OF_KIN_ID");

                nextOfKin.FIRSTNAME = patientContactObj.First_Name;
                nextOfKin.LASTNAME = patientContactObj.Last_Name;
                //nextOfKin.MI = nextOfKin.MI;
                nextOfKin.PATIENT_ACCOUNT = patientContactObj.Patient_Account;
                if (patientContactObj.Flag_Emergency_Contact == true)
                {
                    nextOfKin.RELATIONTOPATIENT = "EMC";
                }
                else
                {
                    nextOfKin.RELATIONTOPATIENT = FieldWiseComparison(patientContactObj.Contact_Type_Id);
                }

                nextOfKin.ADDRESS1 = patientContactObj.Address;
                nextOfKin.ZIP = patientContactObj.Zip;
                nextOfKin.CITY = patientContactObj.City;
                nextOfKin.STATE = patientContactObj.State;
                //nextOfKin.PHONE = patientContactObj.Home_Phone;
                if (patientContactObj.Home_Phone != null)
                {
                    nextOfKin.PHONE = patientContactObj.Home_Phone;
                    nextOfKin.NOK_PHONE_TYPE = "Home Phone";
                }
                else
                {
                    if (patientContactObj.Work_Phone != null)
                    {
                        nextOfKin.PHONE = patientContactObj.Work_Phone;
                        nextOfKin.NOK_PHONE_TYPE = "Office Phone";
                    }
                    else
                    {
                        nextOfKin.PHONE = patientContactObj.Cell_Phone;
                        nextOfKin.NOK_PHONE_TYPE = "Cell Phone";
                    }

                }

                nextOfKin.CREATED_DATE = currentDate;
                nextOfKin.MODIFIED_DATE = currentDate;
                nextOfKin.CREATED_BY = profile.UserName;
                nextOfKin.MODIFIED_BY = profile.UserName;
                nextOfKin.DELETED = false;
                _AF_TBL_PATIENT_NEXT_OF_KINRepository.Insert(nextOfKin);
            }
            else
            {
                dbContact.FIRSTNAME = patientContactObj.First_Name;
                dbContact.LASTNAME = patientContactObj.Last_Name;
                //dbContact.MI = nextOfKin.MI;
                dbContact.PATIENT_ACCOUNT = patientContactObj.Patient_Account;

                if (patientContactObj.Flag_Emergency_Contact == true)
                {
                    dbContact.RELATIONTOPATIENT = "EMC";
                }
                else
                {
                    dbContact.RELATIONTOPATIENT = FieldWiseComparison(patientContactObj.Contact_Type_Id);
                }

                dbContact.ADDRESS1 = patientContactObj.Address;
                dbContact.ZIP = patientContactObj.Zip;
                dbContact.CITY = patientContactObj.City;
                dbContact.STATE = patientContactObj.State;
                dbContact.PHONE = patientContactObj.Home_Phone;

                if (patientContactObj.Home_Phone != null)
                {
                    dbContact.PHONE = patientContactObj.Home_Phone;
                    dbContact.NOK_PHONE_TYPE = "Home Phone";
                }
                else
                {
                    if (patientContactObj.Work_Phone != null)
                    {
                        dbContact.PHONE = patientContactObj.Work_Phone;
                        dbContact.NOK_PHONE_TYPE = "Office Phone";
                    }
                    else
                    {
                        dbContact.PHONE = patientContactObj.Cell_Phone;
                        dbContact.NOK_PHONE_TYPE = "Cell Phone";
                    }

                }

                dbContact.MODIFIED_DATE = currentDate;
                dbContact.MODIFIED_BY = profile.UserName;
                _AF_TBL_PATIENT_NEXT_OF_KINRepository.Update(dbContact);
            }
            _AF_TBL_PATIENT_NEXT_OF_KINRepository.Save();
            return true;
        }
        private bool CreateUpdateTalkRehabContactsGuarantor(PatientContact patientContactObj, UserProfile profile, string oldFname, string oldLname)
        {
            var currentDate = Helper.GetCurrentDate();
            Subscriber guarantorObj = new Subscriber();
            var dbContact = _SubscriberRepository.GetFirst(patientContact => patientContact.GUARANT_FNAME == oldFname && patientContact.GUARANT_LNAME == oldLname);
            if (dbContact == null || oldFname == "")
            {
                guarantorObj.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");

                guarantorObj.GUARANT_FNAME = patientContactObj.First_Name;
                guarantorObj.GUARANT_LNAME = patientContactObj.Last_Name;
                //guarantorObj.MIDDLE_NAME = patientContactObj.MI; 
                guarantorObj.Guarant_Relation = "OTH";
                guarantorObj.GUARANT_ADDRESS = patientContactObj.Address;
                //guarantorObj.Email_Address = patientContactObj.EMAIL;
                guarantorObj.GUARANT_ZIP = patientContactObj.Zip;
                guarantorObj.GUARANT_CITY = patientContactObj.City;
                guarantorObj.GUARANT_STATE = patientContactObj.State;
                if (patientContactObj.Home_Phone != null)
                {
                    guarantorObj.GUARANT_HOME_PHONE = patientContactObj.Home_Phone;
                    guarantorObj.GUARANT_PHONE_TYPE = "Home Phone";
                }
                else
                {
                    if (patientContactObj.Work_Phone != null)
                    {
                        guarantorObj.GUARANT_HOME_PHONE = patientContactObj.Work_Phone;
                        guarantorObj.GUARANT_PHONE_TYPE = "Office Phone";
                    }
                    else
                    {
                        guarantorObj.GUARANT_HOME_PHONE = patientContactObj.Cell_Phone;
                        guarantorObj.GUARANT_PHONE_TYPE = "Cell Phone";
                    }

                }

                guarantorObj.created_date = currentDate;
                guarantorObj.modified_date = currentDate;
                guarantorObj.created_by = profile.UserName;
                guarantorObj.modified_by = profile.UserName;
                guarantorObj.Deleted = false;
                _SubscriberRepository.Insert(guarantorObj);
                SaveGuarantorCodeInPatient(patientContactObj.Patient_Account, guarantorObj.GUARANTOR_CODE);
            }
            else
            {

                dbContact.GUARANT_FNAME = patientContactObj.First_Name;
                dbContact.GUARANT_LNAME = patientContactObj.Last_Name;
                //guarantorObj.MIDDLE_NAME = patientContactObj.MI;
                //dbContact.Contact_Type_Id = 600109;
                dbContact.Guarant_Relation = "OTH";
                dbContact.GUARANT_ADDRESS = patientContactObj.Address;
                //dbContact.Email_Address = patientContactObj.EMAIL;
                dbContact.GUARANT_ZIP = patientContactObj.Zip;
                dbContact.GUARANT_CITY = patientContactObj.City;
                dbContact.GUARANT_STATE = patientContactObj.State;
                //dbContact.Country = patientContactObj.Country;
                dbContact.GUARANT_HOME_PHONE = patientContactObj.Home_Phone;

                if (patientContactObj.Home_Phone != null)
                {
                    dbContact.GUARANT_HOME_PHONE = patientContactObj.Home_Phone;
                    dbContact.GUARANT_PHONE_TYPE = "Home Phone";
                }
                else
                {
                    if (patientContactObj.Work_Phone != null)
                    {
                        dbContact.GUARANT_HOME_PHONE = patientContactObj.Work_Phone;
                        dbContact.GUARANT_PHONE_TYPE = "Office Phone";
                    }
                    else
                    {
                        dbContact.GUARANT_HOME_PHONE = patientContactObj.Cell_Phone;
                        dbContact.GUARANT_PHONE_TYPE = "Cell Phone";
                    }

                }
                dbContact.modified_date = currentDate;
                dbContact.modified_by = profile.UserName;
                _SubscriberRepository.Update(dbContact);

            }
            _SubscriberRepository.Save();
            return true;
        }
        private void SaveGuarantorCodeInPatient(long patientAccount, long guarantorCode)
        {
            var patient = _NewPatientRepository.GetFirst(e => e.Patient_Account == patientAccount && (e.DELETED ?? false) == false);
            if (patient != null)
            {
                patient.Address_To_Guarantor = true;
                patient.Financial_Guarantor = guarantorCode;
                _NewPatientRepository.Update(patient);
                _NewPatientRepository.Save();
            }
        }
        private string FieldWiseComparison(long? contactType)
        {
            string typeId = string.Empty;

            if (contactType == 600104)
            {
                typeId = "OTH";
            }
            else if (contactType == 600105)
            {
                typeId = "CHD";
            }
            else if (contactType == 600106)
            {
                typeId = "OTH";
            }
            else if (contactType == 600108)
            {
                typeId = "FND";
            }
            else if (contactType == 600110)
            {
                typeId = "GRD";
            }
            else if (contactType == 600113)
            {
                typeId = "OTH";
            }
            else if (contactType == 600114)
            {
                typeId = "PAR";
            }
            else if (contactType == 600117)
            {
                typeId = "SEL";
            }
            else if (contactType == 600118)
            {
                typeId = "SIB";
            }
            else if (contactType == 600119)
            {
                typeId = "CHD";
            }
            else if (contactType == 600120)
            {
                typeId = "SPO";
            }
            else
            {
                typeId = "OTH";
            }

            return typeId;
        }
        public void CheckAndUpdatePRSubscriber(string Patient_Account, UserProfile profile)
        {
            var Pat_acct = long.Parse(Patient_Account);
            SubscriberInfoRequest req = new SubscriberInfoRequest();
            Subscriber ObjSubscriber = new Subscriber();
            PatientContact con = null;
            PatientInsurance PRpayInsurance = null;
            req.patientAccount = Pat_acct;
            req.Practice_Code = profile.PracticeCode;
            var patient_misc_data = GetSubscriberInfo(req);
            string con_Type = string.Empty; string con_Type_Code = string.Empty;
            var PRFinancialClassId = _financialClassRepository.GetFirst(e => e.PRACTICE_CODE == profile.PracticeCode && e.CODE == "PR" && !e.DELETED)?.FINANCIAL_CLASS_ID ?? 0;
            if (PRFinancialClassId != 0)
            {
                PRpayInsurance = _PatientInsuranceRepository.GetFirst(i => i.Pri_Sec_Oth_Type == "PR" && (i.IS_PRIVATE_PAY ?? false) == false && i.FINANCIAL_CLASS_ID == PRFinancialClassId && i.Patient_Account == Pat_acct && !(i.Deleted ?? true));
            }
            if (PRpayInsurance != null)
            {
                //if (PRpayInsurance.Relationship == "S")
                //{
                //    var homeaddress_Address = patient_misc_data.PatientAddress.Where(a => a.ADDRESS_TYPE != null && a.ADDRESS_TYPE.ToLower() == "home address").OrderByDescending(e => e.MODIFIED_DATE).FirstOrDefault();
                //    if (homeaddress_Address != null && patient_misc_data.patientinfo != null)
                //    {
                //        PRpayInsurance.Relationship = "S";
                //    }
                //    else
                //    {
                //        PRpayInsurance.Relationship = "S";
                //    }
                //}
                //else
                //{
                if (patient_misc_data.PatientContactsList.Count > 0)
                {
                    //var financial_Con = patient_misc_data.PatientContactsList.Where(e => (e.Flag_Financially_Responsible_Party ?? false) || (e.Flag_Power_Of_Attorney_Financial ?? false)).OrderByDescending(e => e.Modified_On).FirstOrDefault();
                    //Change By arqam
                    var financial_Con = patient_misc_data.PatientContactsList.Where(e => (e.Flag_Financially_Responsible_Party ?? false)).OrderByDescending(e => e.Modified_On).FirstOrDefault();

                    if (financial_Con != null)
                    {
                        con = financial_Con;
                        if (con != null)
                        {
                            var con_type = patient_misc_data.ContactTypeList.Where(e => e.Contact_Type_ID == con.Contact_Type_Id).FirstOrDefault();
                            if (con_type != null)
                            {
                                con_Type = con_type.Type_Name;
                            }
                            switch (con_Type)
                            {
                                case "Son":
                                case "Daughter":
                                    PRpayInsurance.Relationship = "C";
                                    break;
                                case "Self":
                                    PRpayInsurance.Relationship = "S";
                                    break;
                                case "Spouse":
                                    PRpayInsurance.Relationship = "SP";
                                    break;
                                default:
                                    PRpayInsurance.Relationship = "O";
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    PRpayInsurance.Relationship = "S";
                }
                //}
                long? SavedsubscriberId = CheckAndCreateSubscriberForPR(con, patient_misc_data, profile, PRpayInsurance.Relationship);
                if (SavedsubscriberId != null)
                {
                    PRpayInsurance.Subscriber = SavedsubscriberId;
                }
                else
                {
                    PRpayInsurance.Deleted = true;
                }
                PRpayInsurance.Modified_By = profile.UserName;
                PRpayInsurance.Modified_Date = Helper.GetCurrentDate();
                _PatientInsuranceRepository.Update(PRpayInsurance);
                _PatientInsuranceRepository.Save();
            }
        }
        public void UpdateSubscriberForAllOtherInsurancesIfRelationSelfExceptPR(string Patient_Account, UserProfile profile)
        {
            var Pat_acct = long.Parse(Patient_Account);
            SubscriberInfoRequest req = new SubscriberInfoRequest();
            Subscriber ObjSubscriber = new Subscriber();
            PatientContact con = null;
            List<PatientInsurance> OtherInsurances = null;
            req.patientAccount = Pat_acct;
            req.Practice_Code = profile.PracticeCode;
            var patient_misc_data = GetSubscriberInfo(req);
            var PRFinancialClassId = _financialClassRepository.GetFirst(e => e.PRACTICE_CODE == profile.PracticeCode && e.CODE == "PR" && !e.DELETED)?.FINANCIAL_CLASS_ID ?? 0;
            if (PRFinancialClassId != 0)
            {
                OtherInsurances = _PatientInsuranceRepository.GetMany(i => i.FINANCIAL_CLASS_ID != PRFinancialClassId && i.Patient_Account == Pat_acct && !(i.Deleted ?? true) && i.FOX_INSURANCE_STATUS.ToLower() == "c");
            }
            if (OtherInsurances != null && OtherInsurances.Count() > 0)
            {
                for (int i = 0; i < OtherInsurances.Count; i++)
                {
                    if (!string.IsNullOrEmpty(OtherInsurances[i].Relationship) && OtherInsurances[i].Relationship.ToLower() == "s")
                    {
                        if (OtherInsurances[i].Subscriber != null)
                        {
                            long? SavedsubscriberId = CheckAndCreateSubscriberForPR(con, patient_misc_data, profile, OtherInsurances[i].Relationship);
                            if (SavedsubscriberId != null)
                            {
                                OtherInsurances[i].Subscriber = SavedsubscriberId;
                            }
                            OtherInsurances[i].Modified_By = profile.UserName;
                            OtherInsurances[i].Modified_Date = Helper.GetCurrentDate();
                            _PatientInsuranceRepository.Update(OtherInsurances[i]);
                            _PatientInsuranceRepository.Save();
                        }
                    }
                }
            }
        }
        public void UpdateResidualInsuranceSubscriberOnRemoveHomeAddress(string Patient_Account, UserProfile profile) //Set Subscriber Hard Coded Address
        {
            SubscriberInfoRequest req = new SubscriberInfoRequest();
            Subscriber ObjSubscriber = new Subscriber();
            PatientInsurance PRpayInsurance = null;
            var Pat_acct = long.Parse(Patient_Account);
            req.patientAccount = Pat_acct;
            req.Practice_Code = profile.PracticeCode;
            var patient_misc_data = GetSubscriberInfo(req);
            long? subscriberId = null;
            var PRFinancialClassId = _financialClassRepository.GetFirst(e => e.PRACTICE_CODE == profile.PracticeCode && e.CODE == "PR" && !e.DELETED)?.FINANCIAL_CLASS_ID ?? 0;
            if (PRFinancialClassId != 0)
            {
                PRpayInsurance = _PatientInsuranceRepository.GetFirst(i => i.Pri_Sec_Oth_Type == "PR" && (i.IS_PRIVATE_PAY ?? false) == false && i.FINANCIAL_CLASS_ID == PRFinancialClassId && i.Patient_Account == Pat_acct && !(i.Deleted ?? true));
            }
            if (PRpayInsurance != null)
            {
                PRpayInsurance.Relationship = "S";
                ObjSubscriber.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                subscriberId = ObjSubscriber.GUARANTOR_CODE;
                ObjSubscriber.guarant_practice_code = profile.PracticeCode;
                ObjSubscriber.GUARANT_FNAME = patient_misc_data.patientinfo.FirstName;
                ObjSubscriber.GUARANT_MI = patient_misc_data.patientinfo.MIDDLE_NAME;
                ObjSubscriber.GUARANT_LNAME = patient_misc_data.patientinfo.LastName;
                ObjSubscriber.GUARANT_DOB = patient_misc_data.patientinfo.Date_Of_Birth;
                ObjSubscriber.GUARANT_GENDER = patient_misc_data.patientinfo.Gender;
                ObjSubscriber.GUARANT_ADDRESS = "7 Carnegie Plaza";
                ObjSubscriber.GUARANT_ZIP = "080031000";
                ObjSubscriber.GUARANT_CITY = "Cherry Hill";
                ObjSubscriber.GUARANT_STATE = "NJ";
                ObjSubscriber.Guarant_Type = "S";

                ObjSubscriber.created_by = ObjSubscriber.modified_by = profile.UserName;
                ObjSubscriber.created_date = ObjSubscriber.modified_date = Helper.GetCurrentDate();
                ObjSubscriber.Deleted = false;
                _SubscriberRepository.Insert(ObjSubscriber);
                _SubscriberRepository.Save();

                long? SavedsubscriberId = subscriberId;
                if (SavedsubscriberId != null)
                {
                    PRpayInsurance.Subscriber = SavedsubscriberId;
                }
                else
                {
                    PRpayInsurance.Deleted = true;
                }
                PRpayInsurance.Modified_By = profile.UserName;
                PRpayInsurance.Modified_Date = Helper.GetCurrentDate();
                _PatientInsuranceRepository.Update(PRpayInsurance);
                _PatientInsuranceRepository.Save();

            }
        }
        public void PopulateStatementAddress(PatientContact contact, UserProfile profile)
        {
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            interfaceSynch.PATIENT_ACCOUNT = long.Parse(contact.Patient_Account_Str);
            var dbPatientStatementAddress = _PatientAddressRepository.GetFirst(e => e.ADDRESS_TYPE.ToLower().Equals("statement address") && e.PATIENT_ACCOUNT == contact.Patient_Account && !(e.DELETED ?? false));
            if (dbPatientStatementAddress == null) //add
            {
                dbPatientStatementAddress = new PatientAddress();
                dbPatientStatementAddress.PATIENT_ADDRESS_HISTORY_ID = Helper.getMaximumId("PATIENT_ADDRESS_HISTORY_ID");
                dbPatientStatementAddress.PATIENT_ACCOUNT = contact.Patient_Account;
                dbPatientStatementAddress.CREATED_BY = dbPatientStatementAddress.MODIFIED_BY = profile.UserName;
                dbPatientStatementAddress.CREATED_DATE = dbPatientStatementAddress.MODIFIED_DATE = Helper.GetCurrentDate();
                dbPatientStatementAddress.DELETED = false;

                dbPatientStatementAddress.ADDRESS_TYPE = "Statement Address";
                dbPatientStatementAddress.ADDRESS = contact.Address;
                dbPatientStatementAddress.CITY = contact.City;
                dbPatientStatementAddress.STATE = contact.State;
                dbPatientStatementAddress.ZIP = !string.IsNullOrWhiteSpace(contact.Zip) ? contact.Zip.Replace("-", "") : contact.Zip;
                dbPatientStatementAddress.Same_As_POS = false;
                dbPatientStatementAddress = SaveAddressInWebEHRTable(true, dbPatientStatementAddress);
                _PatientAddressRepository.Insert(dbPatientStatementAddress);
                _PatientAddressRepository.Save();
            }
            else //Update (or delete home address)
            {
                dbPatientStatementAddress.Same_As_POS = false;
                dbPatientStatementAddress.MODIFIED_BY = profile.UserName;
                dbPatientStatementAddress.MODIFIED_DATE = Helper.GetCurrentDate();

                if (!String.Equals(contact.Address, dbPatientStatementAddress.ADDRESS, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(dbPatientStatementAddress.PATIENT_ACCOUNT.Value, "Address", dbPatientStatementAddress.ADDRESS, contact.Address, profile.UserName);
                    dbPatientStatementAddress.ADDRESS = contact.Address;
                }

                if (!String.Equals(contact.City, dbPatientStatementAddress.CITY, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(dbPatientStatementAddress.PATIENT_ACCOUNT.Value, "City", dbPatientStatementAddress.CITY, contact.City, profile.UserName);
                    dbPatientStatementAddress.CITY = contact.City;
                }
                if (!String.Equals(contact.State, dbPatientStatementAddress.STATE, StringComparison.Ordinal))
                {
                    CreatePatientUpdateHistory(dbPatientStatementAddress.PATIENT_ACCOUNT.Value, "State", dbPatientStatementAddress.STATE, contact.State, profile.UserName);
                    dbPatientStatementAddress.STATE = contact.State;
                }

                if (!String.Equals(contact.Zip, dbPatientStatementAddress.ZIP, StringComparison.Ordinal))
                {
                    contact.Zip = !string.IsNullOrWhiteSpace(contact.Zip) ? contact.Zip.Replace("-", "") : contact.Zip;
                    dbPatientStatementAddress.ZIP = !string.IsNullOrWhiteSpace(dbPatientStatementAddress.ZIP) ? dbPatientStatementAddress.ZIP.Replace("-", "") : dbPatientStatementAddress.ZIP;

                    CreatePatientUpdateHistory(dbPatientStatementAddress.PATIENT_ACCOUNT.Value, "ZIP", dbPatientStatementAddress.ZIP, contact.Zip, profile.UserName);
                    dbPatientStatementAddress.ZIP = contact.Zip;
                }

                dbPatientStatementAddress.MODIFIED_BY = profile.UserName;
                dbPatientStatementAddress.MODIFIED_DATE = Helper.GetCurrentDate();
                _PatientAddressRepository.Update(dbPatientStatementAddress);
                _PatientAddressRepository.Save();
                SaveAddressInWebEHRTable(false, dbPatientStatementAddress);
            }
        }

        public bool SSNExists(SSNExist request, UserProfile profile)
        {
            long pat_Acc = !string.IsNullOrEmpty(request.Patient_Account) ? long.Parse(request.Patient_Account) : 0;
            var pat = _PatientRepository.GetFirst(x => x.SSN == request.SSN && x.Patient_Account != pat_Acc && x.Practice_Code == profile.PracticeCode);
            return pat != null ? true : false;
        }

        public bool PatientExists(PatientExist request, UserProfile profile)
        {
            long patient_Account = !string.IsNullOrEmpty(request.Patient_Account) ? long.Parse(request.Patient_Account) : 0;
            string firstName = !string.IsNullOrEmpty(request.First_Name) ? request.First_Name.ToLower().Trim() : "";
            string MiddleName = !string.IsNullOrEmpty(request.Middle_Name) ? request.Middle_Name.ToLower().Trim() : "";
            string lastName = !string.IsNullOrEmpty(request.Last_Name) ? request.Last_Name.ToLower().Trim() : "";
            DateTime? dob = !string.IsNullOrEmpty(request.Date_of_Birth) ? Convert.ToDateTime(request.Date_of_Birth) : (DateTime?)null;
            long practiceCode = profile.PracticeCode;
            DateTime datecheck = new DateTime(2019, 6, 8, 0, 0, 0); // June 8, 2019
            //string SSN = string.IsNullOrEmpty(request.SSN) ? "" : request.SSN;
            //if (!string.IsNullOrEmpty(f_Name) && !string.IsNullOrEmpty(l_Name) && !string.IsNullOrEmpty(gender) && dob.HasValue)
            //{
            var patients = _PatientRepository.GetFirst(e => e.First_Name.ToLower().Trim().Equals(firstName.ToLower().Trim())
                && e.Last_Name.ToLower().Trim().Equals(lastName.ToLower().Trim())
                && e.Date_Of_Birth.HasValue && DbFunctions.TruncateTime(e.Date_Of_Birth.Value) == DbFunctions.TruncateTime(dob.Value)
                //&& e.Patient_Account != patient_Account
                && e.Practice_Code == practiceCode
                && e.DELETED == false && e.Created_Date.HasValue && DbFunctions.TruncateTime(e.Created_Date.Value) >= DbFunctions.TruncateTime(datecheck));
            //&& e.Created_Date.HasValue ? (e.Created_Date.Value.Date >= new DateTime(2019, 6, 8).Date) : false
            return patients != null ? true : false;
            //}
            //else {
            //    return false;
            //}
        }

        public bool PatientDemographicsExists(PatientExist request, UserProfile profile)
        {
            long patient_Account = !string.IsNullOrEmpty(request.Patient_Account) ? long.Parse(request.Patient_Account) : 0;
            string firstName = !string.IsNullOrEmpty(request.First_Name) ? request.First_Name.ToLower().Trim() : "";
            string MiddleName = !string.IsNullOrEmpty(request.Middle_Name) ? request.Middle_Name.ToLower().Trim() : "";
            string lastName = !string.IsNullOrEmpty(request.Last_Name) ? request.Last_Name.ToLower().Trim() : "";
            DateTime? dob = !string.IsNullOrEmpty(request.Date_of_Birth) ? Convert.ToDateTime(request.Date_of_Birth) : (DateTime?)null;
            long practiceCode = profile.PracticeCode;
            //string SSN = string.IsNullOrEmpty(request.SSN) ? "" : request.SSN;
            //if (!string.IsNullOrEmpty(f_Name) && !string.IsNullOrEmpty(l_Name) && !string.IsNullOrEmpty(gender) && dob.HasValue)
            //{
            var patients = _PatientRepository.GetMany(e => e.First_Name.ToLower().Trim().Equals(firstName.ToLower().Trim())
                && e.Last_Name.ToLower().Trim().Equals(lastName.ToLower().Trim())
                && e.Date_Of_Birth.HasValue && DbFunctions.TruncateTime(e.Date_Of_Birth.Value) == DbFunctions.TruncateTime(dob.Value)
                && e.Patient_Account != patient_Account
                && e.Practice_Code == practiceCode
                && e.DELETED == false);
            return patients?.Count > 0 ? true : false;
        }

        public List<PatientContact> GetPatientContacts(long patient_Account)
        {
            var list = new List<PatientContact>();
            var conList = _PatientContactRepository.GetManyQueryable(x => x.Patient_Account == patient_Account && x.Deleted == false).ToList();

            if (conList != null && conList.Count() > 0)
            {
                var pract_Code = _PatientRepository.GetFirst(x => x.Patient_Account == patient_Account && !(x.DELETED ?? false))?.Practice_Code;

                var typelist = _ContactTypeRepository.GetMany(e => e.Practice_Code == pract_Code).Select(w => new { w.Contact_Type_ID, w.Type_Name }).ToList();
                foreach (var item in conList)
                {
                    item.Flags = SetFlags(item);
                    var con = typelist.FirstOrDefault(e => e.Contact_Type_ID == item.Contact_Type_Id);
                    if (con != null)
                    {
                        item.Contact_Type_Name = con.Type_Name;
                    }
                    else
                    {
                        item.Contact_Type_Name = "";
                    }

                    if (item.Country != null)
                    {
                        var countryres = _CountryRepository.GetFirst(c => c.FOX_TBL_COUNTRY_ID.ToString() == item.Country && !c.DELETED && (c.IS_ACTIVE ?? false));
                        if (countryres != null)
                        {
                            item.CODE = countryres.CODE;
                            item.NAME = countryres.NAME;
                            item.DESCRIPTION = countryres.DESCRIPTION;
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public List<PatientContact> GetPatientContactsForInsurance(long patient_Account)
        {
            var list = new List<PatientContact>();
            var conList = _PatientContactRepository.GetMany(x => x.Patient_Account == patient_Account && x.Deleted == false && x.Contact_Type_Id != 0).ToList();
            if (conList != null && conList.Count() > 0)
            {
                var pract_Code = _PatientRepository.GetFirst(x => x.Patient_Account == patient_Account && !(x.DELETED ?? false))?.Practice_Code;
                list = new List<PatientContact>();
                var typelist = _ContactTypeRepository.GetMany(e => e.Practice_Code == pract_Code).Select(w => new { w.Contact_Type_ID, w.Type_Name }).ToList();
                foreach (var item in conList)
                {
                    item.Flags = SetFlags(item);
                    item.Contact_Type_Name = typelist.Find(e => e.Contact_Type_ID == item.Contact_Type_Id)?.Type_Name ?? "";
                    if (item.Country != null)
                    {
                        var countryres = _CountryRepository.GetFirst(c => c.FOX_TBL_COUNTRY_ID.ToString() == item.Country && !c.DELETED && (c.IS_ACTIVE ?? false));
                        if (countryres != null)
                        {
                            item.CODE = countryres.CODE;
                            item.NAME = countryres.NAME;
                            item.DESCRIPTION = countryres.DESCRIPTION;
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }


        public void UpdateMedicareCheckboxes(long patient_Account, UserProfile profile)
        {
            bool updateIns = false;
            var insurances = _PatientInsuranceRepository.GetManyQueryable(x => x.Patient_Account == patient_Account && (x.Deleted ?? false) == false && x.Pri_Sec_Oth_Type != "PR").ToList();
            foreach (var insurance in insurances)
            {
                updateIns = false;
                if (insurance.CHK_ABN.HasValue && insurance.CHK_ABN.Value && insurance.ABN_LIMIT_ID.HasValue)
                {
                    var abnLim = _MedicareLimitRepository.GetFirst(e => e.MEDICARE_LIMIT_ID == insurance.ABN_LIMIT_ID.Value && !e.DELETED);
                    if (abnLim != null && abnLim.END_DATE.HasValue && abnLim.END_DATE.Value.Date < DateTime.Now.Date)
                    {
                        insurance.CHK_ABN = false;
                        updateIns = true;
                    }
                }

                if (insurance.CHK_HOSPICE.HasValue && insurance.CHK_HOSPICE.Value && insurance.HOSPICE_LIMIT_ID.HasValue)
                {
                    var hosLim = _MedicareLimitRepository.GetFirst(e => e.MEDICARE_LIMIT_ID == insurance.HOSPICE_LIMIT_ID.Value && !e.DELETED);
                    if (hosLim != null && hosLim.END_DATE.HasValue && hosLim.END_DATE.Value.Date < DateTime.Now.Date)
                    {
                        insurance.CHK_HOSPICE = false;
                        updateIns = true;
                    }
                }

                if (insurance.CHK_HOME_HEALTH_EPISODE.HasValue && insurance.CHK_HOME_HEALTH_EPISODE.Value && insurance.HOME_HEALTH_LIMIT_ID.HasValue)
                {
                    var hheLim = _MedicareLimitRepository.GetFirst(e => e.MEDICARE_LIMIT_ID == insurance.HOME_HEALTH_LIMIT_ID.Value && !e.DELETED);
                    if (hheLim != null && hheLim.END_DATE.HasValue && hheLim.END_DATE.Value.Date < DateTime.Now.Date)
                    {
                        insurance.CHK_HOME_HEALTH_EPISODE = false;
                        updateIns = true;
                    }
                }

                if (updateIns)
                {
                    insurance.Modified_By = profile.UserName;
                    insurance.Modified_Date = Helper.GetCurrentDate();
                    _PatientInsuranceRepository.Update(insurance);
                    _PatientInsuranceRepository.Save();
                }
            }
        }

        public PatientInsuranceEligibilityDetail GetCurrentPatientInsurances(long patient_Account, UserProfile profile)
        {
            //Fetch Patient Details
            var patient = _PatientRepository.GetFirst(e => e.Patient_Account == patient_Account && (e.DELETED ?? false) == false);
            if (patient != null)
            {
                UpdateMedicareCheckboxes(patient_Account, profile);

                PatientInsuranceEligibilityDetail patient_Ins_Elig_Details = new PatientInsuranceEligibilityDetail();

                //Medicare checks
                //var restOfPatientData = _FoxTblPatientRepository.GetFirst(e => e.Patient_Account == patient_Account && e.DELETED == false);
                //if (restOfPatientData == null)
                //{
                //    patient_Ins_Elig_Details.CHK_ABN = false;
                //    patient_Ins_Elig_Details.CHK_HOME_HEALTH_EPISODE = false;
                //}
                //else
                //{
                //    patient_Ins_Elig_Details.CHK_ABN = (restOfPatientData.CHK_ABN ?? false) ? true : false;
                //    patient_Ins_Elig_Details.CHK_HOME_HEALTH_EPISODE = (restOfPatientData.CHK_HOME_HEALTH_EPISODE ?? false) ? true : false;
                //}
                //patient_Ins_Elig_Details.chk_Hospice = (patient.chk_Hospice ?? false) ? true : false;

                //Fetch Current Insurance Details
                patient_Ins_Elig_Details.Current_Patient_Insurances = new List<PatientInsurance>();
                //var curr_insurances = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == patient_Account && (x.Deleted ?? false) == false && (string.IsNullOrEmpty(x.FOX_INSURANCE_STATUS) || x.FOX_INSURANCE_STATUS == "C")).OrderByDescending(q => q.Created_Date).ToList(); //Current

                var curr_insurances = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == patient_Account && (x.Deleted ?? false) == false && (string.IsNullOrEmpty(x.FOX_INSURANCE_STATUS) || x.FOX_INSURANCE_STATUS == "C")); //Current
                if (curr_insurances != null && curr_insurances.Count > 0)
                {
                    patient_Ins_Elig_Details.Current_Patient_Insurances = curr_insurances;

                    for (int i = 0; i < patient_Ins_Elig_Details.Current_Patient_Insurances.Count; i++)
                    {
                        if (patient_Ins_Elig_Details.Current_Patient_Insurances[i].MTBC_Patient_Insurance_Id.HasValue)
                        {
                            var mtbcInsId = patient_Ins_Elig_Details.Current_Patient_Insurances[i].MTBC_Patient_Insurance_Id.Value;
                            var mtbcIns = _MTBCPatientInsuranceRepository.GetFirst(e => e.Patient_Insurance_Id == mtbcInsId && (e.Deleted ?? false) == false);
                            if (mtbcIns != null)
                            {
                                patient_Ins_Elig_Details.Current_Patient_Insurances[i].Eligibility_Status = mtbcIns.Eligibility_Status;
                            }
                        }

                        var insType = patient_Ins_Elig_Details.Current_Patient_Insurances[i].Pri_Sec_Oth_Type;
                        if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("p"))
                        {
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 1;//To display in proper order at fornt-end
                            patient_Ins_Elig_Details.hasPrimaryIns = true;
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Primary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("s"))
                        {
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 2;//To display in proper order at fornt-end
                            patient_Ins_Elig_Details.hasSecondaryIns = true;
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Secondary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("t"))
                        {
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 3;//To display in proper order at fornt-end
                            patient_Ins_Elig_Details.hasTertiaryIns = true;
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Tertiary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("q"))
                        {
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 4;//To display in proper order at fornt-end
                            patient_Ins_Elig_Details.hasQuaternaryIns = true;
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Quaternary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("pr"))
                        {
                            patient_Ins_Elig_Details.hasPatientResponsibilityIns = true;
                            if (patient_Ins_Elig_Details.Current_Patient_Insurances[i].IS_PRIVATE_PAY ?? false)
                            {
                                patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 5;//To display in proper order at fornt-end
                                patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Private Pay";
                            }
                            else
                            {
                                patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 6;//To display in proper order at fornt-end
                                patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Residual Balance";
                            }



                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("x"))
                        {
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 7;//To display in proper order at fornt-end
                            patient_Ins_Elig_Details.hasInvalidPrimaryIns = true;
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Primary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("y"))
                        {
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 8;//To display in proper order at fornt-end
                            patient_Ins_Elig_Details.hasInvalidSecondaryIns = true;
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Secondary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("z"))
                        {
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].DisplayOrder = 9;//To display in proper order at fornt-end
                            patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Other";
                        }

                        patient_Ins_Elig_Details.Current_Patient_Insurances[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(patient_Ins_Elig_Details.Current_Patient_Insurances[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";

                        //Fetch Subscriber Info (if any)
                        if (patient_Ins_Elig_Details.Current_Patient_Insurances[i].Subscriber.HasValue && patient_Ins_Elig_Details.Current_Patient_Insurances[i].Subscriber.Value != 0)
                        {
                            var subscriberInfo = _SubscriberRepository.GetByID(curr_insurances[i].Subscriber.Value);
                            if (subscriberInfo != null)
                            {
                                patient_Ins_Elig_Details.Current_Patient_Insurances[i].SUBSCRIBER_DETAILS = subscriberInfo;
                            }
                        }
                        //Fetch Case Name
                        if (patient_Ins_Elig_Details.Current_Patient_Insurances[i].CASE_ID != null && patient_Ins_Elig_Details.Current_Patient_Insurances[i].CASE_ID != 0)
                        {
                            var caseObj = _vwPatientCaseRepository.GetByID(patient_Ins_Elig_Details.Current_Patient_Insurances[i].CASE_ID);
                            if (caseObj != null)
                            {
                                patient_Ins_Elig_Details.Current_Patient_Insurances[i].CASE_NO = caseObj.CASE_NO;
                                patient_Ins_Elig_Details.Current_Patient_Insurances[i].RT_CASE_NO = caseObj.RT_CASE_NO;
                            }
                        }

                        //Medicare Limit
                        patient_Ins_Elig_Details.Current_Patient_Insurances[i].CurrentMedicareLimitList = new List<MedicareLimit>();
                        var medicareIds =
                            new List<long> {
                                    patient_Ins_Elig_Details.Current_Patient_Insurances[i].ABN_LIMIT_ID ?? 0,
                                    patient_Ins_Elig_Details.Current_Patient_Insurances[i].HOSPICE_LIMIT_ID ?? 0,
                                    patient_Ins_Elig_Details.Current_Patient_Insurances[i].HOME_HEALTH_LIMIT_ID ?? 0
                        };
                        var currMedLimList = _MedicareLimitRepository.GetMany(x => medicareIds.Contains(x.MEDICARE_LIMIT_ID));

                        var medicareLimitTypes = _MedicareLimitTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);
                        patient_Ins_Elig_Details.Current_Patient_Insurances[i].CurrentMedicareLimitList = medicareLimitTypes.GroupJoin(
                         currMedLimList,
                         foo => foo.MEDICARE_LIMIT_TYPE_ID,
                         bar => bar.MEDICARE_LIMIT_TYPE_ID,
                         (x, y) => new { lim_type = x, currMedLim = y })
                   .SelectMany(
                         x => x.currMedLim.DefaultIfEmpty(),
                         //(x, y) => new { lim_type = x, currMedLim = y },
                         (x, y) => new MedicareLimit
                         {
                             MEDICARE_LIMIT_ID = y?.MEDICARE_LIMIT_ID ?? 0,
                             PRACTICE_CODE = y?.PRACTICE_CODE,
                             Patient_Account = y?.Patient_Account,
                             EFFECTIVE_DATE = y?.EFFECTIVE_DATE,
                             EFFECTIVE_DATE_IN_STRING = y?.EFFECTIVE_DATE_IN_STRING,
                             END_DATE = y?.END_DATE,
                             END_DATE_IN_STRING = y?.END_DATE_IN_STRING,
                             ABN_EST_WK_COST = y?.ABN_EST_WK_COST,
                             ABN_COMMENTS = y?.ABN_COMMENTS,
                             NPI = y?.NPI,
                             MEDICARE_LIMIT_TYPE_ID = x?.lim_type.MEDICARE_LIMIT_TYPE_ID,
                             MEDICARE_LIMIT_STATUS = y?.MEDICARE_LIMIT_STATUS,
                             MEDICARE_LIMIT_TYPE_NAME = x?.lim_type.NAME,
                             DISPLAY_ORDER = x.lim_type.DISPLAY_ORDER
                         }
                    )
                   .OrderBy(w => w.DISPLAY_ORDER)
                   .ToList();
                    }

                    //To display in proper order at front-end
                    patient_Ins_Elig_Details.Current_Patient_Insurances = patient_Ins_Elig_Details.Current_Patient_Insurances.OrderBy(e => e.DisplayOrder).ThenBy(e => e.Created_Date).ToList();
                }

                //var insuranceHistory = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == patient_Account && (x.Deleted ?? false) == false).OrderByDescending(w => w.Patient_Insurance_Id).ToList(); //History
                var insuranceHistory = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == patient_Account && (x.Deleted ?? false) == false && x.ELIG_LOADED_ON.HasValue).OrderByDescending(w => w.ELIG_LOADED_ON).ToList(); //History

                if (insuranceHistory != null && insuranceHistory.Count > 0)
                {
                    //insuranceHistory.RemoveAt(insuranceHistory.Count-1);
                    patient_Ins_Elig_Details.Old_Patient_Insurances = insuranceHistory;
                    for (int i = 0; i < patient_Ins_Elig_Details.Old_Patient_Insurances.Count; i++)
                    {
                        //patient_Ins_Elig_Details.Old_Patient_Insurances[i].InsPayer_Description = _FoxInsurancePayorsRepository.GetByID(patient_Ins_Elig_Details.Old_Patient_Insurances[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";
                        patient_Ins_Elig_Details.Old_Patient_Insurances[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(patient_Ins_Elig_Details.Old_Patient_Insurances[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";

                        //Fetch Case Name
                        if (patient_Ins_Elig_Details.Old_Patient_Insurances[i].CASE_ID != null && patient_Ins_Elig_Details.Old_Patient_Insurances[i].CASE_ID != 0)
                        {
                            var caseObj = _vwPatientCaseRepository.GetByID(patient_Ins_Elig_Details.Old_Patient_Insurances[i].CASE_ID);
                            if (caseObj != null)
                            {
                                patient_Ins_Elig_Details.Old_Patient_Insurances[i].CASE_NO = caseObj.CASE_NO;
                                patient_Ins_Elig_Details.Old_Patient_Insurances[i].RT_CASE_NO = caseObj.RT_CASE_NO;
                            }
                        }
                        //Fetch Subscriber Info (if any)
                        if (patient_Ins_Elig_Details.Old_Patient_Insurances[i].Subscriber.HasValue && patient_Ins_Elig_Details.Old_Patient_Insurances[i].Subscriber.Value != 0)
                        {
                            var subscriberInfo = _SubscriberRepository.GetByID(insuranceHistory[i].Subscriber.Value);
                            if (subscriberInfo != null)
                            {
                                patient_Ins_Elig_Details.Old_Patient_Insurances[i].SUBSCRIBER_DETAILS = subscriberInfo;
                            }
                        }

                        patient_Ins_Elig_Details.Old_Patient_Insurances[i].CurrentMedicareLimitList = new List<MedicareLimit>();
                        var medicareIds =
                            new List<long> {
                                    patient_Ins_Elig_Details.Old_Patient_Insurances[i].ABN_LIMIT_ID ?? 0,
                                    patient_Ins_Elig_Details.Old_Patient_Insurances[i].HOSPICE_LIMIT_ID ?? 0,
                                    patient_Ins_Elig_Details.Old_Patient_Insurances[i].HOME_HEALTH_LIMIT_ID ?? 0
                        };
                        var currMedLimList = _MedicareLimitRepository.GetMany(x => medicareIds.Contains(x.MEDICARE_LIMIT_ID));

                        var medicareLimitTypes = _MedicareLimitTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);
                        patient_Ins_Elig_Details.Old_Patient_Insurances[i].CurrentMedicareLimitList = medicareLimitTypes.GroupJoin(
                         currMedLimList,
                         foo => foo.MEDICARE_LIMIT_TYPE_ID,
                         bar => bar.MEDICARE_LIMIT_TYPE_ID,
                         (x, y) => new { lim_type = x, currMedLim = y })
                   .SelectMany(
                         x => x.currMedLim.DefaultIfEmpty(),
                         //(x, y) => new { lim_type = x, currMedLim = y },
                         (x, y) => new MedicareLimit
                         {
                             MEDICARE_LIMIT_ID = y?.MEDICARE_LIMIT_ID ?? 0,
                             PRACTICE_CODE = y?.PRACTICE_CODE,
                             Patient_Account = y?.Patient_Account,
                             EFFECTIVE_DATE = y?.EFFECTIVE_DATE,
                             EFFECTIVE_DATE_IN_STRING = y?.EFFECTIVE_DATE_IN_STRING,
                             END_DATE = y?.END_DATE,
                             END_DATE_IN_STRING = y?.END_DATE_IN_STRING,
                             ABN_EST_WK_COST = y?.ABN_EST_WK_COST,
                             ABN_COMMENTS = y?.ABN_COMMENTS,
                             NPI = y?.NPI,
                             MEDICARE_LIMIT_TYPE_ID = x?.lim_type.MEDICARE_LIMIT_TYPE_ID,
                             MEDICARE_LIMIT_STATUS = y?.MEDICARE_LIMIT_STATUS,
                             MEDICARE_LIMIT_TYPE_NAME = x?.lim_type.NAME,
                             DISPLAY_ORDER = x.lim_type.DISPLAY_ORDER
                         }
                    )
                   .OrderBy(w => w.DISPLAY_ORDER)
                   .ToList();
                    }
                }
                //usman
                //if pr and case_id != null
                //if (patient_Ins_Elig_Details.Current_Patient_Insurances[i].Ins_Type == "Patient Responsibility")
                patient_Ins_Elig_Details.PR_CASES_LIST = GetPatientCasesForPR(patient_Account);
                //_vwPatientCaseRepository.GetMany(e => e.PATIENT_ACCOUNT == patient.Patient_Account && e.CASE_SUFFIX_NAME.Equals("PP"));

                //Fetch Medicare Limit Types Details
                //patient_Ins_Elig_Details.CurrentMedicareLimitList = new List<MedicareLimit>();
                //var currMedLimList = _MedicareLimitRepository.GetMany(x => x.Patient_Account == patient_Account && x.MEDICARE_LIMIT_STATUS == "C");

                //var medicareLimitTypes = _MedicareLimitTypeRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED);

                //patient_Ins_Elig_Details.CurrentMedicareLimitList = medicareLimitTypes.GroupJoin(
                //              currMedLimList,
                //              foo => foo.MEDICARE_LIMIT_TYPE_ID,
                //              bar => bar.MEDICARE_LIMIT_TYPE_ID,
                //              (x, y) => new { lim_type = x, currMedLim = y })
                //        .SelectMany(
                //              x => x.currMedLim.DefaultIfEmpty(),
                //              //(x, y) => new { lim_type = x, currMedLim = y },
                //              (x, y) => new MedicareLimit
                //              {
                //                  MEDICARE_LIMIT_ID = y?.MEDICARE_LIMIT_ID ?? 0,
                //                  PRACTICE_CODE = y?.PRACTICE_CODE,
                //                  Patient_Account = y?.Patient_Account,
                //                  EFFECTIVE_DATE = y?.EFFECTIVE_DATE,
                //                  EFFECTIVE_DATE_IN_STRING = y?.EFFECTIVE_DATE_IN_STRING,
                //                  END_DATE = y?.END_DATE,
                //                  END_DATE_IN_STRING = y?.END_DATE_IN_STRING,
                //                  ABN_EST_WK_COST = y?.ABN_EST_WK_COST,
                //                  ABN_COMMENTS = y?.ABN_COMMENTS,
                //                  NPI = y?.NPI,
                //                  MEDICARE_LIMIT_TYPE_ID = x?.lim_type.MEDICARE_LIMIT_TYPE_ID,
                //                  MEDICARE_LIMIT_STATUS = y?.MEDICARE_LIMIT_STATUS,
                //                  MEDICARE_LIMIT_TYPE_NAME = x?.lim_type.NAME,
                //                  DISPLAY_ORDER = x.lim_type.DISPLAY_ORDER
                //              }
                //         )
                //        .OrderBy(w => w.DISPLAY_ORDER)
                //        .ToList();

                //Fetch Employer Details
                patient_Ins_Elig_Details.Employer_Details = new Employer();
                if (patient != null && (patient.EMPLOYER_CODE != null && patient.EMPLOYER_CODE != 0))
                {
                    var employer_info = _EmployerRepository.GetFirst(e => e.Employer_Code == patient.EMPLOYER_CODE);
                    if (employer_info != null)
                    {
                        patient_Ins_Elig_Details.Employer_Details = employer_info;
                        if ((employer_info.IS_REFERRAL_REQUIRED ?? false) && (employer_info.REFERRAL_PROVIDER_ID != null && employer_info.REFERRAL_PROVIDER_ID != 0))
                        {
                            var pcp = _OrderingRefSourceRepository.GetByID(employer_info.REFERRAL_PROVIDER_ID);
                            if (pcp != null)
                            {
                                patient_Ins_Elig_Details.Employer_Details.REFERRAL_PROVIDER_NAME = pcp.FIRST_NAME + " " + pcp.LAST_NAME + " | " + pcp.REFERRAL_REGION;
                            }
                        }
                    }
                }

                //Get Suggested MC Payer
                var _parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var _patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = patient.Patient_Account };
                var _zip = new SqlParameter { ParameterName = "ZIP", Value = DBNull.Value };
                var result = SpRepository<PatientInsuranceDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_MC_SUGGESTED_PAYER @PRACTICE_CODE, @PATIENT_ACCOUNT, @ZIP", _parmPracticeCode, _patientAccount, _zip);
                if (result != null && result.Count > 0)
                {
                    patient_Ins_Elig_Details.Suggested_MC_Payer = result[0].InsPayer_Description;
                }
                else
                {
                    patient_Ins_Elig_Details.Suggested_MC_Payer = "";
                }

                //fetch cases for dropdown
                patient_Ins_Elig_Details.PatientCasesList = GetPatientCasesForDD(patient_Account);
                var fcList = GetFinancialClassDDValues(profile.PracticeCode.ToString());
                if (fcList != null && fcList.Count > 0)
                {
                    patient_Ins_Elig_Details.FinancialClassList = fcList.Where(e => e.SHOW_FOR_INSURANCE).ToList();
                }
                else
                {
                    patient_Ins_Elig_Details.FinancialClassList = new List<FinancialClass>();
                }

                patient_Ins_Elig_Details.PatientPRDiscountList = _PatientPRDiscountRepository.GetMany(e => e.PRACTICE_CODE == profile.PracticeCode).ToList();
                patient_Ins_Elig_Details.PatientPRPeriodList = _PatientPRPeriodRepository.GetMany(e => e.PRACTICE_CODE == profile.PracticeCode).ToList();

                return patient_Ins_Elig_Details;
            }
            return new PatientInsuranceEligibilityDetail();
        }

        public List<PatientInsuranceDetail> GetPatientInsurancePayorName(long patientAccount)
        {
            var PatientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = patientAccount };
            var result = SpRepository<PatientInsuranceDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES @PATIENT_ACCOUNT", PatientAccount);
            return result;
        }

        public string GetFoxPayorName(long insurance_Id)
        {
            var insur_Id = insurance_Id.ToString();
            //var ins = _FoxInsurancePayorsRepository.GetFirst(x => x.INSURANCE_ID == insur_Id);
            var ins = _foxInsurancePayersRepository.GetFirst(x => x.INSURANCE_ID == insurance_Id);

            if (ins != null)
            {
                //return ins.INSURANCE_NAME;
                return ins.INSURANCE_NAME;

            }
            return "";
        }

        public string GetPrimaryInsurance(long patient_Account)
        {
            string name = "";

            //Get Primary Insurance Name
            var priInsu = _PatientInsuranceRepository.GetFirst(e => e.Patient_Account == patient_Account && e.Pri_Sec_Oth_Type == "P" && (string.IsNullOrEmpty(e.FOX_INSURANCE_STATUS) || e.FOX_INSURANCE_STATUS == "C"));
            if (priInsu != null)
            {
                //name = _FoxInsurancePayorsRepository.GetByID(priInsu.FOX_TBL_INSURANCE_ID).INSURANCE_NAME;
                name = _foxInsurancePayersRepository.GetByID(priInsu.FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME;

            }
            return name;
        }

        public string GetLatestPrimaryInsurance(long patient_Account)
        {
            string name = "";
            var PatientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = patient_Account };
            var result = SpRepository<PatientInsurance>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_LATEST_PRIMARY_INSURANCE @PATIENT_ACCOUNT", PatientAccount);
            if (result != null)
            {
                var priInsu = result.FOX_TBL_INSURANCE_ID;
                if (priInsu != 0)
                {
                    //name = _FoxInsurancePayorsRepository.GetByID(priInsu.FOX_TBL_INSURANCE_ID).INSURANCE_NAME;
                    name = _foxInsurancePayersRepository.GetByID(priInsu)?.INSURANCE_NAME;

                }
            }
            return name;
        }

        public List<Subscriber> GetSubscribers(SubscriberSearchReq obj, UserProfile Profile)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
            var result = SpRepository<Subscriber>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_SUBSCRIBERS] @PRACTICE_CODE, @SEARCHVALUE", parmPracticeCode, smartvalue).ToList();
            if (result.Any())
                return result;
            else
                return new List<Subscriber>();
        }

        public List<Employer> GetEmployers(EmployerSearchReq obj, UserProfile Profile)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
            var result = SpRepository<Employer>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_EMPLOYERS] @PRACTICE_CODE, @SEARCHVALUE", parmPracticeCode, smartvalue).ToList();
            if (result.Any())
                return result;
            else
                return new List<Employer>();
        }

        public List<PatientCasesForDD> GetPatientCasesForDD(long patient_Account)
        {
            List<PatientCasesForDD> caseList = _vwPatientCaseRepository.GetMany(e => e.PATIENT_ACCOUNT == patient_Account)
                .Select(w => new PatientCasesForDD()
                {
                    CASE_ID = w.CASE_ID,
                    CASE_NO = w.CASE_NO,
                    RT_CASE_NO = w.RT_CASE_NO,
                    CASE_STATUS_NAME = w.CASE_STATUS_NAME,
                    DISCIPLINE_NAME = w.DISCIPLINE_NAME,
                    CREATED_DATE = w.CREATED_DATE,
                    CASE_SUFFIX_NAME = w.CASE_SUFFIX_NAME,

                }).ToList();
            return caseList;
        }

        public List<PR_VIEW_CASES> GetPatientCasesForPR(long patient_Account)
        {
            List<PR_VIEW_CASES> caseList = _vwPatientCaseRepository.GetMany(e => e.PATIENT_ACCOUNT == patient_Account && e.CASE_SUFFIX_NAME == "PP")
                .Select(w => new PR_VIEW_CASES()
                {
                    CASE_ID = w.CASE_ID,
                    CASE_NO = w.CASE_NO,
                    RT_CASE_NO = w.RT_CASE_NO,
                    CASE_STATUS_NAME = w.CASE_STATUS_NAME,
                    ADMISSION_DATE = w.ADMISSION_DATE,
                    PosLocation = w.PosLocation,
                    TreatingProviderName = w.TreatingProviderName,
                    POS_ID = w.POS_ID,
                    IsWellness = w.IsWellness,
                    IsSkilled = w.IsSkilled


                }).ToList();
            return caseList;
        }

        public bool EligibilityDataChanged(PatientInsurance oldInsuranceData, PatientInsurance newInsuranceData, out bool abnchanged, out bool hoschanged, out bool hhchanged)
        {
            abnchanged = false;
            hoschanged = false;
            hhchanged = false;
            ABN_MedicareLimitDataChanged(oldInsuranceData.ABN_LIMIT_ID, newInsuranceData.CurrentMedicareLimitList, out abnchanged);
            HOS_MedicareLimitDataChanged(oldInsuranceData.HOSPICE_LIMIT_ID, newInsuranceData.CurrentMedicareLimitList, out hoschanged);
            HH_MedicareLimitDataChanged(oldInsuranceData.HOME_HEALTH_LIMIT_ID, newInsuranceData.CurrentMedicareLimitList, out hhchanged);

            if ((oldInsuranceData.Co_Payment ?? 0) != (newInsuranceData.Co_Payment ?? 0)) { return true; }
            if ((oldInsuranceData.IS_COPAY_PER ?? false) != (newInsuranceData.IS_COPAY_PER ?? false)) { return true; }
            if ((oldInsuranceData.IS_COPAY_PER_VISIT ?? false) != (newInsuranceData.IS_COPAY_PER_VISIT ?? false)) { return true; }
            if (oldInsuranceData.DED_AMT_VERIFIED_ON != newInsuranceData.DED_AMT_VERIFIED_ON) { return true; }
            if (!String.Equals(oldInsuranceData.DED_POLICY_LIMIT_RESET_ON ?? "", newInsuranceData.DED_POLICY_LIMIT_RESET_ON ?? "", StringComparison.Ordinal)) { return true; }
            if (oldInsuranceData.YEARLY_DED_AMT != newInsuranceData.YEARLY_DED_AMT) { return true; }
            if (oldInsuranceData.DED_MET != newInsuranceData.DED_MET) { return true; }
            if (oldInsuranceData.DED_MET_AS_OF != newInsuranceData.DED_MET_AS_OF) { return true; }
            if (oldInsuranceData.DED_REMAINING != newInsuranceData.DED_REMAINING) { return true; }
            if ((oldInsuranceData.IS_PT_ST_THRESHOLD_REACHED ?? false) != (newInsuranceData.IS_PT_ST_THRESHOLD_REACHED ?? false)) { return true; }
            if ((oldInsuranceData.IS_OT_THRESHOLD_REACHED ?? false) != (newInsuranceData.IS_OT_THRESHOLD_REACHED ?? false)) { return true; }

            if (oldInsuranceData.PT_ST_TOT_AMT_USED != newInsuranceData.PT_ST_TOT_AMT_USED) { return true; }
            if (oldInsuranceData.PT_ST_RT_AMT != newInsuranceData.PT_ST_RT_AMT) { return true; }

            if (oldInsuranceData.OT_TOT_AMT_USED != newInsuranceData.OT_TOT_AMT_USED) { return true; }
            if (oldInsuranceData.OT_RT_AMT != newInsuranceData.OT_RT_AMT) { return true; }

            if (oldInsuranceData.PT_ST_YTD_AMT != newInsuranceData.PT_ST_YTD_AMT) { return true; }
            if (oldInsuranceData.OT_YTD_AMT != newInsuranceData.OT_YTD_AMT) { return true; }

            if (oldInsuranceData.BENEFIT_AMT_VERIFIED_ON != newInsuranceData.BENEFIT_AMT_VERIFIED_ON) { return true; }
            if (!String.Equals(oldInsuranceData.BENEFIT_POLICY_LIMIT_RESET_ON ?? "", newInsuranceData.BENEFIT_POLICY_LIMIT_RESET_ON ?? "", StringComparison.Ordinal)) { return true; }

            if (oldInsuranceData.MYB_LIMIT_DOLLARS != newInsuranceData.MYB_LIMIT_DOLLARS) { return true; }
            if (oldInsuranceData.MYB_LIMIT_VISIT != newInsuranceData.MYB_LIMIT_VISIT) { return true; }
            if (oldInsuranceData.MYB_USED_OUTSIDE_DOLLARS != newInsuranceData.MYB_USED_OUTSIDE_DOLLARS) { return true; }
            if (oldInsuranceData.MYB_USED_OUTSIDE_VISIT != newInsuranceData.MYB_USED_OUTSIDE_VISIT) { return true; }

            if (oldInsuranceData.MYB_USED_DOLLARS != newInsuranceData.MYB_USED_DOLLARS) { return true; }
            if (oldInsuranceData.MYB_USED_VISIT != newInsuranceData.MYB_USED_VISIT) { return true; }
            if (oldInsuranceData.MYB_REMAINING_DOLLARS != newInsuranceData.MYB_REMAINING_DOLLARS) { return true; }
            if (oldInsuranceData.MYB_REMAINING_VISIT != newInsuranceData.MYB_REMAINING_VISIT) { return true; }

            if (oldInsuranceData.MOP_AMT != newInsuranceData.MOP_AMT) { return true; }
            if (oldInsuranceData.MOP_USED_OUTSIDE_RT != newInsuranceData.MOP_USED_OUTSIDE_RT) { return true; }
            if (oldInsuranceData.MOP_USED != newInsuranceData.MOP_USED) { return true; }
            if (oldInsuranceData.MOP_REMAINING != newInsuranceData.MOP_REMAINING) { return true; }

            if (!String.Equals(oldInsuranceData.SPOKE_TO ?? "", newInsuranceData.SPOKE_TO ?? "", StringComparison.Ordinal)) { return true; }
            if (oldInsuranceData.CASE_ID != newInsuranceData.CASE_ID) { return true; }

            if (!String.Equals(oldInsuranceData.BENEFIT_COMMENTS ?? "", newInsuranceData.BENEFIT_COMMENTS ?? "", StringComparison.Ordinal)) { return true; }
            if (!String.Equals(oldInsuranceData.GENERAL_COMMENTS ?? "", newInsuranceData.GENERAL_COMMENTS ?? "", StringComparison.Ordinal)) { return true; }

            if (oldInsuranceData.CHK_ABN != newInsuranceData.CHK_ABN) { return true; }
            if (oldInsuranceData.CHK_HOME_HEALTH_EPISODE != newInsuranceData.CHK_HOME_HEALTH_EPISODE) { return true; }
            if (oldInsuranceData.CHK_HOSPICE != newInsuranceData.CHK_HOSPICE) { return true; }

            //if not any of above returns true, then return false means data has not changed
            if (abnchanged || hoschanged || hhchanged)
            {
                return true;
            }
            else return false;
        }

        //public void SaveRestOfPatientDataForInsurance(PatientInsuranceEligibilityDetail details, long patAccount, string userName)
        //{
        //    var restOfPatientData = new FOX_TBL_PATIENT();
        //    var data = _FoxTblPatientRepository.GetFirst(e => e.Patient_Account == patAccount);
        //    if (data == null)
        //    {
        //        restOfPatientData.FOX_TBL_PATIENT_ID = Helper.getMaximumId("FOX_TBL_PATIENT");
        //        restOfPatientData.Patient_Account = patAccount;
        //        restOfPatientData.Created_By = restOfPatientData.Modified_By = userName;
        //        restOfPatientData.Created_Date = restOfPatientData.Modified_Date = Helper.GetCurrentDate();
        //        restOfPatientData.DELETED = false;
        //    }
        //    else
        //    {
        //        restOfPatientData = data;
        //        restOfPatientData.Modified_By = userName;
        //        restOfPatientData.Modified_Date = Helper.GetCurrentDate();
        //    }

        //    //restOfPatientData.CHK_ABN = details.CHK_ABN;
        //    //restOfPatientData.CHK_HOME_HEALTH_EPISODE = details.CHK_HOME_HEALTH_EPISODE;

        //    if (data == null)
        //    {
        //        _FoxTblPatientRepository.Insert(restOfPatientData);
        //    }
        //    else
        //    {
        //        _FoxTblPatientRepository.Update(restOfPatientData);
        //    }

        //    _PatientContext.SaveChanges();
        //}

        public PatientInsurance CreateUpdateInsuranceInMTBC(PatientInsurance insuranceToCreateUpdate, long patient_Account, UserProfile profile)
        {
            bool isEdit = true;
            var MTBCIns = _MTBCPatientInsuranceRepository.GetFirst(e => e.Patient_Insurance_Id == insuranceToCreateUpdate.MTBC_Patient_Insurance_Id);
            if (MTBCIns == null)
            {
                isEdit = false;
                MTBCIns = new MTBCPatientInsurance();
                MTBCIns.Patient_Insurance_Id = Helper.getMaximumId("Patient_Insurance_Id");
                MTBCIns.Created_By = profile.UserName;
                MTBCIns.Created_Date = MTBCIns.Modified_Date = Helper.GetCurrentDate();
                MTBCIns.Deleted = false;
            }

            MTBCIns.Modified_By = profile.UserName;
            MTBCIns.Modified_Date = Helper.GetCurrentDate();

            MTBCIns.Patient_Account = patient_Account;
            MTBCIns.Effective_Date = insuranceToCreateUpdate.Effective_Date;
            MTBCIns.Termination_Date = insuranceToCreateUpdate.Termination_Date;
            MTBCIns.Policy_Number = insuranceToCreateUpdate.Policy_Number;
            MTBCIns.Group_Number = insuranceToCreateUpdate.Group_Number;
            MTBCIns.Plan_Name = insuranceToCreateUpdate.Plan_Name;
            MTBCIns.Relationship = insuranceToCreateUpdate.Relationship;
            if (!string.IsNullOrWhiteSpace(insuranceToCreateUpdate.Eligibility_Status) && insuranceToCreateUpdate.Eligibility_Status.Length > 50)
            {
                insuranceToCreateUpdate.Eligibility_Status = insuranceToCreateUpdate.Eligibility_Status.Substring(0, 40).Trim() + "...";
            }
            MTBCIns.Eligibility_Status = insuranceToCreateUpdate.Eligibility_Status;
            insuranceToCreateUpdate.MTBC_Patient_Insurance_Id = MTBCIns.Patient_Insurance_Id;

            var foxInsurance = _foxInsurancePayersRepository.GetFirst(e => e.FOX_TBL_INSURANCE_ID == insuranceToCreateUpdate.FOX_TBL_INSURANCE_ID);
            if (foxInsurance != null)
            {
                insuranceToCreateUpdate.Insurance_Id = foxInsurance.INSURANCE_ID.HasValue ? foxInsurance.INSURANCE_ID.Value : 0;
                MTBCIns.Insurance_Id = insuranceToCreateUpdate.Insurance_Id;
            }

            switch (insuranceToCreateUpdate.Pri_Sec_Oth_Type)
            {
                case "PR":
                    MTBCIns.Pri_Sec_Oth_Type = "R";
                    break;
                case "T":
                case "Q":
                    MTBCIns.Pri_Sec_Oth_Type = "O";
                    break;
                default:
                    MTBCIns.Pri_Sec_Oth_Type = insuranceToCreateUpdate.Pri_Sec_Oth_Type;
                    break;
            }

            if (!insuranceToCreateUpdate.Pri_Sec_Oth_Type.Equals("PR"))
            {
                MTBCIns.Co_Payment = insuranceToCreateUpdate.Co_Payment;
                MTBCIns.Deductions = insuranceToCreateUpdate.DED_REMAINING;
            }

            if ((insuranceToCreateUpdate.Subscriber.HasValue && insuranceToCreateUpdate.Subscriber.Value != 0)
                || (insuranceToCreateUpdate.SUBSCRIBER_DETAILS != null && !string.IsNullOrWhiteSpace(insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_FNAME)))
            {
                if (!insuranceToCreateUpdate.Relationship.ToLower().Equals("s") && (!insuranceToCreateUpdate.Pri_Sec_Oth_Type.Equals("PR") || (insuranceToCreateUpdate.IS_PRIVATE_PAY ?? false)))
                {
                    SavePatientContactfromInsuranceSubscriber(insuranceToCreateUpdate.SUBSCRIBER_DETAILS, insuranceToCreateUpdate.Relationship, patient_Account, profile);
                }
                if (insuranceToCreateUpdate.Patient_Insurance_Id == 0)
                {
                    if ((!insuranceToCreateUpdate.Subscriber.HasValue || insuranceToCreateUpdate.Subscriber.Value != 0)
                        && insuranceToCreateUpdate.SUBSCRIBER_DETAILS != null && !string.IsNullOrWhiteSpace(insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_FNAME)
                        //&& insuranceToCreateUpdate.SUBSCRIBER_DETAILS.IS_NEW_SUBSCRIBER
                        ) //If new subscriber created, it will contain subscriber data.
                    {
                        //Create new subscriber and add map its key with new insurance obj
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.guarant_practice_code = profile.PracticeCode;
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_by = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_by = profile.UserName;
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_date = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_date = Helper.GetCurrentDate();
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.Deleted = false;

                        if (!string.IsNullOrEmpty(insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING))
                        {
                            insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB = Convert.ToDateTime(insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING);
                        }

                        _SubscriberRepository.Insert(insuranceToCreateUpdate.SUBSCRIBER_DETAILS);
                        _SubscriberRepository.Save();
                        //Map new subscriber with new insurance obj as well once subscriber created sucessfully
                        insuranceToCreateUpdate.Subscriber = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE;
                    }
                    else
                    {
                        //If any other subscriber has been allocated, then it is already mapped with InsuranceToCreateUpdate obj on client side upon subscriber selection.
                    }
                }
                else
                {
                    if (insuranceToCreateUpdate.SUBSCRIBER_DETAILS != null && insuranceToCreateUpdate.SUBSCRIBER_DETAILS.IS_NEW_SUBSCRIBER) //If new subscriber created, it will contain subscriber data.
                    {
                        //Create new subscriber and add map its key with new insurance obj
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.guarant_practice_code = profile.PracticeCode;
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_by = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_by = profile.UserName;
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_date = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_date = Helper.GetCurrentDate();
                        insuranceToCreateUpdate.SUBSCRIBER_DETAILS.Deleted = false;

                        if (!string.IsNullOrEmpty(insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING))
                        {
                            insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB = Convert.ToDateTime(insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING);
                        }

                        _SubscriberRepository.Insert(insuranceToCreateUpdate.SUBSCRIBER_DETAILS);
                        _SubscriberRepository.Save();
                        //Map new subscriber with new insurance obj as well once subscriber created sucessfully
                        insuranceToCreateUpdate.Subscriber = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE;
                    }
                    else
                    {
                        //For other than PR, subscriber is added only and can't be updated.
                        if (insuranceToCreateUpdate.Pri_Sec_Oth_Type.ToLower().Equals("pr"))
                        {
                            var existingsub = _SubscriberRepository.GetFirst(e => e.GUARANTOR_CODE == insuranceToCreateUpdate.Subscriber && (e.Deleted ?? false) == false);
                            if (existingsub != null)
                            {
                                existingsub.GUARANT_FNAME = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_FNAME;
                                existingsub.GUARANT_LNAME = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_LNAME;
                                existingsub.GUARANT_MI = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_MI;
                                existingsub.GUARANT_ADDRESS = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_ADDRESS;
                                existingsub.GUARANT_ZIP = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_ZIP;
                                existingsub.GUARANT_CITY = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_CITY;
                                existingsub.GUARANT_STATE = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_STATE;
                                existingsub.GUARANT_HOME_PHONE = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_HOME_PHONE;
                                existingsub.GUARANT_DOB = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB;
                                existingsub.GUARANT_GENDER = insuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_GENDER;

                                existingsub.modified_by = profile.UserName;
                                existingsub.modified_date = Helper.GetCurrentDate();
                                _SubscriberRepository.Update(existingsub);
                                _SubscriberRepository.Save();
                            }
                        }
                    }
                }
                MTBCIns.Subscriber = insuranceToCreateUpdate.Subscriber != null && insuranceToCreateUpdate.Subscriber == 0 ? null : insuranceToCreateUpdate.Subscriber;
            }

            if (isEdit)
                _MTBCPatientInsuranceRepository.Update(MTBCIns);
            else
                _MTBCPatientInsuranceRepository.Insert(MTBCIns);

            _MTBCPatientInsuranceRepository.Save();
            //}
            return insuranceToCreateUpdate;
        }

        public ResponseModel SaveInsuranceAndEligibilityDetails(PatientInsuranceEligibilityDetail details, UserProfile profile, bool FromIndexInfo)
        {
            ResponseModel resp = new ResponseModel();
            resp.Success = false;
            if (!string.IsNullOrEmpty(details.Patient_Account_Str))
            {
                //InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                var patAccount = long.Parse(details.Patient_Account_Str);
                //interfaceSynch.PATIENT_ACCOUNT = patAccount;
                //Medicare checks
                var paientDetails = _PatientRepository.GetByID(patAccount);
                if (paientDetails != null)
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Employer
                    if (details.Employer_Details != null && details.Employer_Details.Employer_Code != 0)
                    {
                        details.Employer_Details.Modified_By = profile.UserName;
                        details.Employer_Details.Modified_Date = Helper.GetCurrentDate();
                        _EmployerRepository.Update(details.Employer_Details);

                        paientDetails.EMPLOYER_CODE = details.Employer_Details.Employer_Code;
                        _PatientRepository.Update(paientDetails);
                    }
                    //if (details.is_Change)
                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(interfaceSynch, profile);
                    //_PatientRepository.Update(paientDetails);
                    details.is_Change = false;
                }

                //Insurance, Subscriber & Eligibility
                if (details.InsuranceToCreateUpdate != null)
                {
                    //Dates need to be set in any case, so setting them first
                    //Effective From Date
                    if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.Effective_Date_In_String)) details.InsuranceToCreateUpdate.Effective_Date = Convert.ToDateTime(details.InsuranceToCreateUpdate.Effective_Date_In_String);
                    //Effective To Date
                    if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.Termination_Date_In_String)) details.InsuranceToCreateUpdate.Termination_Date = Convert.ToDateTime(details.InsuranceToCreateUpdate.Termination_Date_In_String);
                    //SUPRESS_BILLING_UNTIL Date
                    if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.SUPRESS_BILLING_UNTIL_DATE_IN_STRING)) details.InsuranceToCreateUpdate.SUPRESS_BILLING_UNTIL = Convert.ToDateTime(details.InsuranceToCreateUpdate.SUPRESS_BILLING_UNTIL_DATE_IN_STRING);
                    //DED_AMT_VERIFIED_ON Date
                    if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.DED_AMT_VERIFIED_ON_DATE_IN_STRING))
                    {
                        var datetimeUtc = Convert.ToDateTime(details.InsuranceToCreateUpdate.DED_MET_AS_OF_IN_STRING);
                        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        var estDate = TimeZoneInfo.ConvertTimeFromUtc(datetimeUtc, easternZone);
                        details.InsuranceToCreateUpdate.DED_AMT_VERIFIED_ON = Convert.ToDateTime(estDate);
                    }
                    //DED_MET_AS_OF Date
                    if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.DED_MET_AS_OF_IN_STRING))
                    {
                        var datetimeUtc = Convert.ToDateTime(details.InsuranceToCreateUpdate.DED_MET_AS_OF_IN_STRING);
                        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        var estDate = TimeZoneInfo.ConvertTimeFromUtc(datetimeUtc, easternZone);
                        details.InsuranceToCreateUpdate.DED_MET_AS_OF = Convert.ToDateTime(estDate);
                    }
                    //BENEFIT_AMT_VERIFIED_ON Date
                    //if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.BENEFIT_AMT_VERIFIED_ON_DATE_IN_STRING)) details.InsuranceToCreateUpdate.BENEFIT_AMT_VERIFIED_ON = Convert.ToDateTime(details.InsuranceToCreateUpdate.BENEFIT_AMT_VERIFIED_ON_DATE_IN_STRING);
                    if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.BENEFIT_AMT_VERIFIED_ON_DATE_IN_STRING))
                    {
                        var timeUtc = Convert.ToDateTime(details.InsuranceToCreateUpdate.BENEFIT_AMT_VERIFIED_ON_DATE_IN_STRING);
                        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        var estDate = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                        details.InsuranceToCreateUpdate.BENEFIT_AMT_VERIFIED_ON = Convert.ToDateTime(estDate);
                    }

                    //Deceased Date
                    if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.Deceased_Date_In_String)) details.InsuranceToCreateUpdate.Deceased_Date = Convert.ToDateTime(details.InsuranceToCreateUpdate.Deceased_Date_In_String);

                    //Set Insurance Subscriber DOB
                    if (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS != null && (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.IS_NEW_SUBSCRIBER || details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type.ToLower().Equals("pr")))
                    {
                        if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING))
                            details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB =
                                Convert.ToDateTime(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING);
                    }

                    details.InsuranceToCreateUpdate = CreateUpdateInsuranceInMTBC(details.InsuranceToCreateUpdate, patAccount, profile);

                    //var dbPatientInsurance = details.InsuranceToCreateUpdate.Patient_Insurance_Id == 0 ? null : _PatientInsuranceRepository.GetByID(details.InsuranceToCreateUpdate.Patient_Insurance_Id);
                    var dbPatientInsurance = details.InsuranceToCreateUpdate.Patient_Insurance_Id == 0 ? null
                        : _PatientInsuranceRepository.GetFirst(e => e.Patient_Insurance_Id == details.InsuranceToCreateUpdate.Patient_Insurance_Id && e.Patient_Account == patAccount && !(e.Deleted ?? false));


                    if (dbPatientInsurance == null)
                    {
                        //Basic Info
                        details.InsuranceToCreateUpdate.Patient_Insurance_Id = Helper.getMaximumId("Fox_Patient_Insurance_Id");
                        details.InsuranceToCreateUpdate.Parent_Patient_insurance_Id = details.InsuranceToCreateUpdate.Patient_Insurance_Id;
                        details.InsuranceToCreateUpdate.Patient_Account = patAccount;
                        details.InsuranceToCreateUpdate.FOX_INSURANCE_STATUS = "C";
                        details.InsuranceToCreateUpdate.Created_By = details.InsuranceToCreateUpdate.Modified_By = profile.UserName;
                        details.InsuranceToCreateUpdate.Created_Date = details.InsuranceToCreateUpdate.Modified_Date = Helper.GetCurrentDate();
                        details.InsuranceToCreateUpdate.Deleted = false;
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Subscriber Info
                        //if (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS != null)
                        //{
                        //    if (!details.InsuranceToCreateUpdate.Relationship.ToLower().Equals("s"))
                        //    {
                        //        SavePatientContactfromInsuranceSubscriber(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS, details.InsuranceToCreateUpdate.Relationship, patAccount, profile);
                        //    }
                        //    if (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.IS_NEW_SUBSCRIBER) //If new subscriber created, it will contain subscriber data.
                        //    {
                        //        //Create new subscriber and add map its key with new insurance obj
                        //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                        //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.guarant_practice_code = profile.PracticeCode;
                        //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_by = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_by = profile.UserName;
                        //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_date = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_date = Helper.GetCurrentDate();
                        //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.Deleted = false;
                        //        if (!string.IsNullOrEmpty(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING))
                        //        {
                        //            details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB = Convert.ToDateTime(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB_IN_STRING);
                        //        }
                        //        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //        //InsertInterfaceTeamData(interfaceSynch, profile);

                        //        _SubscriberRepository.Insert(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS);

                        //        //Map new subscriber with new insurance obj as well once subscriber created sucessfully
                        //        details.InsuranceToCreateUpdate.Subscriber = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE;
                        //    }
                        //    else
                        //    {
                        //        //If any other subscriber has been allocated, then it is already mapped with InsuranceToCreateUpdate obj on client side upon subscriber selection.
                        //    }
                        //}
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //InsertInterfaceTeamData(interfaceSynch, profile);
                        _PatientInsuranceRepository.Insert(details.InsuranceToCreateUpdate);
                        resp.Message = "Insurance created successfully.";
                        resp.Success = true;
                    }
                    else //Update
                    {
                        //Detect changes first
                        bool abnInfoChanged = false;
                        bool hosInfoChanged = false;
                        bool hhInfoChanged = false;

                        if (!FromIndexInfo && !details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type.ToLower().Equals("pr") && EligibilityDataChanged(dbPatientInsurance, details.InsuranceToCreateUpdate, out abnInfoChanged, out hosInfoChanged, out hhInfoChanged))
                        {
                            if (EligibilityAlreadyLoadedInThisMonth(dbPatientInsurance))
                            {
                                resp.Message = "Eligibility can only be entered after span of 14 days.";
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(dbPatientInsurance.FOX_ELIGIBILITY_GUID))
                                {
                                    var newInsObj = new PatientInsurance();
                                    newInsObj.Patient_Insurance_Id = Helper.getMaximumId("Fox_Patient_Insurance_Id");
                                    newInsObj.Parent_Patient_insurance_Id = dbPatientInsurance.Patient_Insurance_Id;
                                    //newInsObj.Parent_Patient_insurance_Id = dbPatientInsurance.Parent_Patient_insurance_Id;
                                    newInsObj.FINANCIAL_CLASS_ID = dbPatientInsurance.FINANCIAL_CLASS_ID;
                                    newInsObj.MTBC_Patient_Insurance_Id = dbPatientInsurance.MTBC_Patient_Insurance_Id;
                                    newInsObj.FOX_INSURANCE_STATUS = "C"; //Create new insurance with current status
                                    newInsObj.Created_By = dbPatientInsurance.Created_By; //Keep created by value same as it was for the first insurance created
                                    newInsObj.Modified_By = profile.UserName;
                                    newInsObj.Created_Date = dbPatientInsurance.Created_Date; //Keep created date same as it was for the first insurance created
                                    newInsObj.Modified_Date = Helper.GetCurrentDate();
                                    newInsObj.Deleted = false;
                                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Payor Info
                                    newInsObj.Pri_Sec_Oth_Type = details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type;
                                    newInsObj.Patient_Account = dbPatientInsurance.Patient_Account;
                                    newInsObj.Insurance_Id = details.InsuranceToCreateUpdate.Insurance_Id;
                                    newInsObj.FOX_TBL_INSURANCE_ID = details.InsuranceToCreateUpdate.FOX_TBL_INSURANCE_ID;
                                    newInsObj.Effective_Date = details.InsuranceToCreateUpdate.Effective_Date;
                                    newInsObj.Termination_Date = details.InsuranceToCreateUpdate.Termination_Date;
                                    newInsObj.INACTIVE = details.InsuranceToCreateUpdate.INACTIVE;
                                    newInsObj.Relationship = details.InsuranceToCreateUpdate.Relationship;
                                    newInsObj.Policy_Number = details.InsuranceToCreateUpdate.Policy_Number;
                                    newInsObj.Group_Number = details.InsuranceToCreateUpdate.Group_Number;
                                    newInsObj.Plan_Name = details.InsuranceToCreateUpdate.Plan_Name;
                                    newInsObj.SUPRESS_BILLING_UNTIL = details.InsuranceToCreateUpdate.SUPRESS_BILLING_UNTIL;
                                    newInsObj.CASE_ID = details.InsuranceToCreateUpdate.CASE_ID;
                                    newInsObj.PR_DISCOUNT_ID = details.InsuranceToCreateUpdate.PR_DISCOUNT_ID;
                                    newInsObj.PR_PERIOD_ID = details.InsuranceToCreateUpdate.PR_PERIOD_ID;
                                    newInsObj.PERIODIC_PAYMENT = details.InsuranceToCreateUpdate.PERIODIC_PAYMENT;
                                    newInsObj.Is_Authorization_Required = details.InsuranceToCreateUpdate.Is_Authorization_Required;
                                    newInsObj.IsWellness = details.InsuranceToCreateUpdate.IsWellness;
                                    newInsObj.IsSkilled = details.InsuranceToCreateUpdate.IsSkilled;
                                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Subscriber Info
                                    //if (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS != null)
                                    //{
                                    //    if (!details.InsuranceToCreateUpdate.Relationship.ToLower().Equals("s"))
                                    //    {
                                    //        SavePatientContactfromInsuranceSubscriber(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS, details.InsuranceToCreateUpdate.Relationship, patAccount, profile);
                                    //    }
                                    //    if (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.IS_NEW_SUBSCRIBER) //If new subscriber created, it will contain subscriber data.
                                    //    {
                                    //        //Create new subscriber and add map its key with new insurance obj
                                    //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                                    //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.guarant_practice_code = profile.PracticeCode;
                                    //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_by = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_by = profile.UserName;
                                    //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_date = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_date = Helper.GetCurrentDate();
                                    //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.Deleted = false;
                                    //        _SubscriberRepository.Insert(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS);

                                    //        //Map new subscriber with new insurance obj as well once subscriber created sucessfully
                                    //        newInsObj.Subscriber = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE;
                                    //    }
                                    //    else
                                    //    {
                                    //        //If any other subscriber has been reallocated, then it is already mapped with InsuranceToCreateUpdate obj on client side upon subscriber selection. 
                                    //        //So just assign it.
                                    //        newInsObj.Subscriber = details.InsuranceToCreateUpdate.Subscriber;
                                    //    }
                                    //}
                                    newInsObj.Subscriber = details.InsuranceToCreateUpdate.Subscriber;
                                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Eligibility Info
                                    newInsObj.Co_Payment = details.InsuranceToCreateUpdate.Co_Payment;
                                    newInsObj.IS_COPAY_PER_VISIT = details.InsuranceToCreateUpdate.IS_COPAY_PER_VISIT;
                                    newInsObj.DED_AMT_VERIFIED_ON = details.InsuranceToCreateUpdate.DED_AMT_VERIFIED_ON;
                                    newInsObj.DED_POLICY_LIMIT_RESET_ON = details.InsuranceToCreateUpdate.DED_POLICY_LIMIT_RESET_ON;
                                    newInsObj.YEARLY_DED_AMT = details.InsuranceToCreateUpdate.YEARLY_DED_AMT;
                                    newInsObj.DED_MET = details.InsuranceToCreateUpdate.DED_MET;
                                    newInsObj.DED_MET_AS_OF = details.InsuranceToCreateUpdate.DED_MET_AS_OF;
                                    newInsObj.DED_REMAINING = details.InsuranceToCreateUpdate.DED_REMAINING;
                                    newInsObj.IS_PT_ST_THRESHOLD_REACHED = details.InsuranceToCreateUpdate.IS_PT_ST_THRESHOLD_REACHED;
                                    newInsObj.IS_OT_THRESHOLD_REACHED = details.InsuranceToCreateUpdate.IS_OT_THRESHOLD_REACHED;
                                    newInsObj.PT_ST_TOT_AMT_USED = details.InsuranceToCreateUpdate.PT_ST_TOT_AMT_USED;
                                    newInsObj.PT_ST_RT_AMT = details.InsuranceToCreateUpdate.PT_ST_RT_AMT;
                                    //newInsObj.PT_ST_OUTSIDE_AMT_USED = details.InsuranceToCreateUpdate.PT_ST_OUTSIDE_AMT_USED;
                                    newInsObj.OT_TOT_AMT_USED = details.InsuranceToCreateUpdate.OT_TOT_AMT_USED;
                                    newInsObj.OT_RT_AMT = details.InsuranceToCreateUpdate.OT_RT_AMT;
                                    //newInsObj.OT_OUTSIDE_AMT_USED = details.InsuranceToCreateUpdate.OT_OUTSIDE_AMT_USED;
                                    newInsObj.PT_ST_YTD_AMT = details.InsuranceToCreateUpdate.PT_ST_YTD_AMT;
                                    newInsObj.OT_YTD_AMT = details.InsuranceToCreateUpdate.OT_YTD_AMT;
                                    newInsObj.BENEFIT_AMT_VERIFIED_ON = details.InsuranceToCreateUpdate.BENEFIT_AMT_VERIFIED_ON;
                                    newInsObj.BENEFIT_POLICY_LIMIT_RESET_ON = details.InsuranceToCreateUpdate.BENEFIT_POLICY_LIMIT_RESET_ON;
                                    newInsObj.MYB_LIMIT_DOLLARS = details.InsuranceToCreateUpdate.MYB_LIMIT_DOLLARS;
                                    newInsObj.MYB_LIMIT_VISIT = details.InsuranceToCreateUpdate.MYB_LIMIT_VISIT;
                                    newInsObj.MYB_USED_OUTSIDE_DOLLARS = details.InsuranceToCreateUpdate.MYB_USED_OUTSIDE_DOLLARS;
                                    newInsObj.MYB_USED_OUTSIDE_VISIT = details.InsuranceToCreateUpdate.MYB_USED_OUTSIDE_VISIT;
                                    newInsObj.MYB_USED_DOLLARS = details.InsuranceToCreateUpdate.MYB_USED_DOLLARS;
                                    newInsObj.MYB_USED_VISIT = details.InsuranceToCreateUpdate.MYB_USED_VISIT;
                                    newInsObj.MYB_REMAINING_DOLLARS = details.InsuranceToCreateUpdate.MYB_REMAINING_DOLLARS;
                                    newInsObj.MYB_REMAINING_VISIT = details.InsuranceToCreateUpdate.MYB_REMAINING_VISIT;
                                    newInsObj.MOP_AMT = details.InsuranceToCreateUpdate.MOP_AMT;
                                    newInsObj.MOP_USED_OUTSIDE_RT = details.InsuranceToCreateUpdate.MOP_USED_OUTSIDE_RT;
                                    newInsObj.MOP_USED = details.InsuranceToCreateUpdate.MOP_USED;
                                    newInsObj.MOP_REMAINING = details.InsuranceToCreateUpdate.MOP_REMAINING;
                                    newInsObj.SPOKE_TO = details.InsuranceToCreateUpdate.SPOKE_TO;
                                    newInsObj.BENEFIT_COMMENTS = details.InsuranceToCreateUpdate.BENEFIT_COMMENTS;
                                    newInsObj.GENERAL_COMMENTS = details.InsuranceToCreateUpdate.GENERAL_COMMENTS;
                                    newInsObj.IS_COPAY_PER = details.InsuranceToCreateUpdate.IS_COPAY_PER ?? false;
                                    //newInsObj.IS_VERIFIED = details.InsuranceToCreateUpdate.IS_VERIFIED;
                                    //if (newInsObj.IS_VERIFIED ?? false)
                                    //{
                                    //    newInsObj.VERIFIED_BY = profile.UserName;
                                    //    newInsObj.VERIFIED_DATE = Helper.GetCurrentDate();
                                    //}
                                    newInsObj.IS_VERIFIED = true;
                                    newInsObj.VERIFIED_BY = profile.UserName;
                                    newInsObj.VERIFIED_DATE = Helper.GetCurrentDate();
                                    newInsObj.CHK_ABN = details.InsuranceToCreateUpdate.CHK_ABN;
                                    newInsObj.CHK_HOME_HEALTH_EPISODE = details.InsuranceToCreateUpdate.CHK_HOME_HEALTH_EPISODE;
                                    newInsObj.CHK_HOSPICE = details.InsuranceToCreateUpdate.CHK_HOSPICE;
                                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Medicare Info
                                    MedicareLimit newABNData = details.InsuranceToCreateUpdate.CurrentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "ABN").FirstOrDefault();
                                    newABNData.Patient_Account = patAccount;
                                    newInsObj.ABN_LIMIT_ID = abnInfoChanged ? CreateNewLimit(dbPatientInsurance.ABN_LIMIT_ID, newABNData, "ABN", profile) : dbPatientInsurance.ABN_LIMIT_ID;

                                    MedicareLimit newHOSData = details.InsuranceToCreateUpdate.CurrentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "Hospice").FirstOrDefault();
                                    newHOSData.Patient_Account = patAccount;
                                    newInsObj.HOSPICE_LIMIT_ID = hosInfoChanged ? CreateNewLimit(dbPatientInsurance.HOSPICE_LIMIT_ID, newHOSData, "Hospice", profile) : dbPatientInsurance.HOSPICE_LIMIT_ID;

                                    MedicareLimit newHHData = details.InsuranceToCreateUpdate.CurrentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "Home Health Episode").FirstOrDefault();
                                    newHHData.Patient_Account = patAccount;
                                    newInsObj.HOME_HEALTH_LIMIT_ID = hhInfoChanged ? CreateNewLimit(dbPatientInsurance.HOME_HEALTH_LIMIT_ID, newHHData, "Home Health Episode", profile) : dbPatientInsurance.HOME_HEALTH_LIMIT_ID;
                                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Insert newInsObj with Current status into db
                                    newInsObj.ELIG_LOADED_ON = Helper.GetCurrentDate();
                                    _PatientInsuranceRepository.Insert(newInsObj);
                                    _PatientContext.SaveChanges();
                                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //Update previous insurance status to Old
                                    dbPatientInsurance.FOX_INSURANCE_STATUS = "O";
                                    dbPatientInsurance.Modified_By = profile.UserName;
                                    dbPatientInsurance.Modified_Date = Helper.GetCurrentDate();
                                    _PatientInsuranceRepository.Update(dbPatientInsurance);
                                    _PatientContext.SaveChanges();
                                    //if (details.is_Change)
                                    //Task 149402:Dev Task: FOX - RT 105.Disabling editing of patient info. from RFO, Usman Nasir
                                    //InsertInterfaceTeamData(interfaceSynch, profile);
                                    details.is_Change = false;
                                    if (!string.IsNullOrWhiteSpace(details.InsuranceToCreateUpdate.Eligibility_MSP_Data))
                                    {
                                        string tasktype = "BLOCK";

                                        InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                                        var interfaceTask = setTaskData(profile, details.InsuranceToCreateUpdate.Patient_Account, tasktype, details.InsuranceToCreateUpdate.CURRENT_DATE_STR);
                                        var taskInterfaced = AddUpdateTask(interfaceTask, profile, details.InsuranceToCreateUpdate.Eligibility_MSP_Data);
                                        //if (taskInterfaced != null)
                                        //{
                                        //    interfaceSynch.PATIENT_ACCOUNT = details.InsuranceToCreateUpdate.Patient_Account;
                                        //    interfaceSynch.TASK_ID = taskInterfaced.TASK_ID;
                                        //    InsertInterfaceTeamData(interfaceSynch, profile);
                                        //}
                                    }
                                    if (!string.IsNullOrWhiteSpace(details.InsuranceToCreateUpdate.Deceased_Date_In_String))
                                    {
                                        CreateAlertforDeceasedPatient(dbPatientInsurance.Patient_Account, details.InsuranceToCreateUpdate.Deceased_Date.Value, profile);
                                    }
                                    resp.Message = "Eligibility added successfully.";
                                    resp.Success = true;
                                }
                                else
                                {
                                    dbPatientInsurance = UpdateInsuraneAndEligibility(dbPatientInsurance, details, abnInfoChanged, hosInfoChanged, hhInfoChanged, profile, out resp);
                                }
                            }
                        }
                        else
                        {
                            if (!string.Equals(dbPatientInsurance.Pri_Sec_Oth_Type ?? "", details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type ?? "", StringComparison.Ordinal))
                            {
                                if (dbPatientInsurance.MTBC_Patient_Insurance_Id != null && dbPatientInsurance.MTBC_Patient_Insurance_Id != 0)
                                {
                                    var allinsurancesofsametype = _PatientInsuranceRepository.GetMany(i => i.MTBC_Patient_Insurance_Id == dbPatientInsurance.MTBC_Patient_Insurance_Id && i.FOX_INSURANCE_STATUS.ToLower() == "o" && i.Patient_Insurance_Id != dbPatientInsurance.Patient_Insurance_Id && (i.Deleted ?? false) == false);
                                    if (allinsurancesofsametype.Count() > 0)
                                    {
                                        foreach (var singleinsurance in allinsurancesofsametype)
                                        {
                                            if (singleinsurance != null && singleinsurance.Patient_Insurance_Id != 0)
                                            {
                                                singleinsurance.Pri_Sec_Oth_Type = details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type;
                                                singleinsurance.Modified_By = profile.UserName;
                                                singleinsurance.Modified_Date = Helper.GetCurrentDate();
                                                _PatientInsuranceRepository.Update(singleinsurance);
                                                _PatientInsuranceRepository.Save();
                                            }
                                        }
                                    }
                                }
                            }
                            dbPatientInsurance.Pri_Sec_Oth_Type = details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type;
                            dbPatientInsurance.FINANCIAL_CLASS_ID = details.InsuranceToCreateUpdate.FINANCIAL_CLASS_ID;
                            dbPatientInsurance.Patient_Account = dbPatientInsurance.Patient_Account;
                            dbPatientInsurance.Insurance_Id = details.InsuranceToCreateUpdate.Insurance_Id;
                            dbPatientInsurance.FOX_TBL_INSURANCE_ID = details.InsuranceToCreateUpdate.FOX_TBL_INSURANCE_ID;
                            dbPatientInsurance.Effective_Date = details.InsuranceToCreateUpdate.Effective_Date;
                            dbPatientInsurance.Termination_Date = details.InsuranceToCreateUpdate.Termination_Date;
                            dbPatientInsurance.INACTIVE = details.InsuranceToCreateUpdate.INACTIVE;
                            dbPatientInsurance.Relationship = details.InsuranceToCreateUpdate.Relationship;
                            dbPatientInsurance.Policy_Number = details.InsuranceToCreateUpdate.Policy_Number;
                            dbPatientInsurance.Group_Number = details.InsuranceToCreateUpdate.Group_Number;
                            dbPatientInsurance.Plan_Name = details.InsuranceToCreateUpdate.Plan_Name;
                            dbPatientInsurance.SUPRESS_BILLING_UNTIL = details.InsuranceToCreateUpdate.SUPRESS_BILLING_UNTIL;
                            if (!FromIndexInfo)
                            {
                                dbPatientInsurance.Is_Authorization_Required = details.InsuranceToCreateUpdate.Is_Authorization_Required;
                                dbPatientInsurance.CASE_ID = details.InsuranceToCreateUpdate.CASE_ID;
                                dbPatientInsurance.PR_DISCOUNT_ID = details.InsuranceToCreateUpdate.PR_DISCOUNT_ID;
                                dbPatientInsurance.PR_PERIOD_ID = details.InsuranceToCreateUpdate.PR_PERIOD_ID;
                                dbPatientInsurance.PERIODIC_PAYMENT = details.InsuranceToCreateUpdate.PERIODIC_PAYMENT;
                                dbPatientInsurance.IsWellness = details.InsuranceToCreateUpdate.IsWellness;
                                dbPatientInsurance.IsSkilled = details.InsuranceToCreateUpdate.IsSkilled;
                            }

                            //if (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS != null)
                            //{
                            //    if (!details.InsuranceToCreateUpdate.Relationship.ToLower().Equals("s"))
                            //    {
                            //        SavePatientContactfromInsuranceSubscriber(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS, details.InsuranceToCreateUpdate.Relationship, patAccount, profile);
                            //    }
                            //    if (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.IS_NEW_SUBSCRIBER) //If new subscriber created, it will contain subscriber data.
                            //    {
                            //        //Create new subscriber and add map its key with new insurance obj
                            //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                            //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.guarant_practice_code = profile.PracticeCode;
                            //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_by = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_by = profile.UserName;
                            //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.created_date = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.modified_date = Helper.GetCurrentDate();
                            //        details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.Deleted = false;
                            //        _SubscriberRepository.Insert(details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS);

                            //        //Map new subscriber with new insurance obj as well once subscriber created sucessfully
                            //        dbPatientInsurance.Subscriber = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE;
                            //    }
                            //    else
                            //    {
                            //        //If any other subscriber has been reallocated, then it is already mapped with InsuranceToCreateUpdate obj on client side upon subscriber selection. 
                            //        //So just assign it.
                            //        //if (details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.Flag_PERSONAL_INFORMATION)
                            //        if (details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type.ToLower().Equals("pr"))

                            //        {
                            //            var existingsub = _SubscriberRepository.GetFirst(e => e.GUARANTOR_CODE == details.InsuranceToCreateUpdate.Subscriber && (e.Deleted ?? false) == false);
                            //            if (existingsub != null)
                            //            {
                            //                existingsub.GUARANT_FNAME = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_FNAME;
                            //                existingsub.GUARANT_LNAME = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_LNAME;
                            //                existingsub.GUARANT_MI = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_MI;
                            //                existingsub.GUARANT_ADDRESS = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_ADDRESS;
                            //                existingsub.GUARANT_ZIP = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_ZIP;
                            //                existingsub.GUARANT_CITY = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_CITY;
                            //                existingsub.GUARANT_STATE = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_STATE;
                            //                existingsub.GUARANT_HOME_PHONE = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_HOME_PHONE;
                            //                existingsub.GUARANT_DOB = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_DOB;
                            //                existingsub.GUARANT_GENDER = details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANT_GENDER;

                            //                existingsub.modified_by = profile.UserName;
                            //                existingsub.modified_date = Helper.GetCurrentDate();
                            //                _SubscriberRepository.Update(existingsub);
                            //                _SubscriberRepository.Save();

                            //            }

                            //            //details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.GUARANTOR_CODE = (long)details.InsuranceToCreateUpdate.Subscriber;

                            //            //details.InsuranceToCreateUpdate.SUBSCRIBER_DETAILS.Deleted = false;
                            //        }
                            //        dbPatientInsurance.Subscriber = details.InsuranceToCreateUpdate.Subscriber;
                            //    }
                            //}

                            dbPatientInsurance.Subscriber = details.InsuranceToCreateUpdate.Subscriber;

                            //dbPatientInsurance.IS_VERIFIED = details.InsuranceToCreateUpdate.IS_VERIFIED;
                            //if (dbPatientInsurance.IS_VERIFIED ?? false)
                            //{
                            //    dbPatientInsurance.VERIFIED_BY = profile.UserName;
                            //    dbPatientInsurance.VERIFIED_DATE = Helper.GetCurrentDate();
                            //}
                            //else
                            //{
                            //    dbPatientInsurance.VERIFIED_DATE = null;
                            //}

                            dbPatientInsurance.Modified_By = profile.UserName;
                            dbPatientInsurance.Modified_Date = Helper.GetCurrentDate();
                            _PatientInsuranceRepository.Update(dbPatientInsurance);
                            //if (details.is_Change)
                            //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                            //InsertInterfaceTeamData(interfaceSynch, profile);
                            details.is_Change = false;
                            resp.Message = "Insurance updated successfully.";
                            resp.Success = true;
                        }
                    }

                    if (!details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type.ToLower().Equals("pr") && details.InsuranceToCreateUpdate.Patient_Insurancs_Ids_With_Overlapping_Dates != null
                        && details.InsuranceToCreateUpdate.Patient_Insurancs_Ids_With_Overlapping_Dates.Count > 0)
                    {
                        //Update MTBC Insurance in this case as well? Question for SA team.
                        SetTerminationDateForExistingPayers(details.InsuranceToCreateUpdate, profile);
                    }

                    if (details.InsuranceToCreateUpdate.ClaimsToMapToNewInsurance != null && details.InsuranceToCreateUpdate.ClaimsToMapToNewInsurance.Count > 0)
                    {
                        MapClaimsToNewInsurance(details.InsuranceToCreateUpdate, profile);
                    }
                    if (!FromIndexInfo)
                    {
                        if (!string.IsNullOrWhiteSpace(details.InsuranceToCreateUpdate.Deceased_Date_In_String))
                        {
                            SaveDeceasedDateInPatient(patAccount, details.InsuranceToCreateUpdate.Deceased_Date, profile);
                        }
                    }
                }

                //If execution pointer reaches here, it means context needs to be saved now.
                _PatientContext.SaveChanges();

                //return true;
                //resp.Message = "Eligibility added successfully.";
                //resp.Success = true;
                return resp;
            }
            //return false;
            //resp.Message = "An error occurred while adding eligibility. Please try again.";
            //resp.Success = false;
            return resp;
        }

        private PatientInsurance UpdateInsuraneAndEligibility(PatientInsurance dbPatientInsurance, PatientInsuranceEligibilityDetail details, bool abnInfoChanged, bool hosInfoChanged, bool hhInfoChanged, UserProfile profile, out ResponseModel resp)
        {
            resp = new ResponseModel();
            resp.Success = false;
            dbPatientInsurance.FINANCIAL_CLASS_ID = details.InsuranceToCreateUpdate.FINANCIAL_CLASS_ID;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Payor Info
            dbPatientInsurance.Pri_Sec_Oth_Type = details.InsuranceToCreateUpdate.Pri_Sec_Oth_Type;
            dbPatientInsurance.Insurance_Id = details.InsuranceToCreateUpdate.Insurance_Id;
            dbPatientInsurance.FOX_TBL_INSURANCE_ID = details.InsuranceToCreateUpdate.FOX_TBL_INSURANCE_ID;
            dbPatientInsurance.Effective_Date = details.InsuranceToCreateUpdate.Effective_Date;
            dbPatientInsurance.Termination_Date = details.InsuranceToCreateUpdate.Termination_Date;
            dbPatientInsurance.INACTIVE = details.InsuranceToCreateUpdate.INACTIVE;
            dbPatientInsurance.Relationship = details.InsuranceToCreateUpdate.Relationship;
            dbPatientInsurance.Policy_Number = details.InsuranceToCreateUpdate.Policy_Number;
            dbPatientInsurance.Group_Number = details.InsuranceToCreateUpdate.Group_Number;
            dbPatientInsurance.Plan_Name = details.InsuranceToCreateUpdate.Plan_Name;
            dbPatientInsurance.SUPRESS_BILLING_UNTIL = details.InsuranceToCreateUpdate.SUPRESS_BILLING_UNTIL;
            dbPatientInsurance.CASE_ID = details.InsuranceToCreateUpdate.CASE_ID;
            dbPatientInsurance.PR_DISCOUNT_ID = details.InsuranceToCreateUpdate.PR_DISCOUNT_ID;
            dbPatientInsurance.PR_PERIOD_ID = details.InsuranceToCreateUpdate.PR_PERIOD_ID;
            dbPatientInsurance.PERIODIC_PAYMENT = details.InsuranceToCreateUpdate.PERIODIC_PAYMENT;
            dbPatientInsurance.Is_Authorization_Required = details.InsuranceToCreateUpdate.Is_Authorization_Required;
            dbPatientInsurance.IsWellness = details.InsuranceToCreateUpdate.IsWellness;
            dbPatientInsurance.IsSkilled = details.InsuranceToCreateUpdate.IsSkilled;
            dbPatientInsurance.Subscriber = details.InsuranceToCreateUpdate.Subscriber;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Eligibility Info
            dbPatientInsurance.Co_Payment = details.InsuranceToCreateUpdate.Co_Payment;
            dbPatientInsurance.IS_COPAY_PER_VISIT = details.InsuranceToCreateUpdate.IS_COPAY_PER_VISIT;
            dbPatientInsurance.DED_AMT_VERIFIED_ON = details.InsuranceToCreateUpdate.DED_AMT_VERIFIED_ON;
            dbPatientInsurance.DED_POLICY_LIMIT_RESET_ON = details.InsuranceToCreateUpdate.DED_POLICY_LIMIT_RESET_ON;
            dbPatientInsurance.YEARLY_DED_AMT = details.InsuranceToCreateUpdate.YEARLY_DED_AMT;
            dbPatientInsurance.DED_MET = details.InsuranceToCreateUpdate.DED_MET;
            dbPatientInsurance.DED_MET_AS_OF = details.InsuranceToCreateUpdate.DED_MET_AS_OF;
            dbPatientInsurance.DED_REMAINING = details.InsuranceToCreateUpdate.DED_REMAINING;
            dbPatientInsurance.IS_PT_ST_THRESHOLD_REACHED = details.InsuranceToCreateUpdate.IS_PT_ST_THRESHOLD_REACHED;
            dbPatientInsurance.IS_OT_THRESHOLD_REACHED = details.InsuranceToCreateUpdate.IS_OT_THRESHOLD_REACHED;
            dbPatientInsurance.PT_ST_TOT_AMT_USED = details.InsuranceToCreateUpdate.PT_ST_TOT_AMT_USED;
            dbPatientInsurance.PT_ST_RT_AMT = details.InsuranceToCreateUpdate.PT_ST_RT_AMT;
            dbPatientInsurance.OT_TOT_AMT_USED = details.InsuranceToCreateUpdate.OT_TOT_AMT_USED;
            dbPatientInsurance.OT_RT_AMT = details.InsuranceToCreateUpdate.OT_RT_AMT;
            dbPatientInsurance.PT_ST_YTD_AMT = details.InsuranceToCreateUpdate.PT_ST_YTD_AMT;
            dbPatientInsurance.OT_YTD_AMT = details.InsuranceToCreateUpdate.OT_YTD_AMT;
            dbPatientInsurance.BENEFIT_AMT_VERIFIED_ON = details.InsuranceToCreateUpdate.BENEFIT_AMT_VERIFIED_ON;
            dbPatientInsurance.BENEFIT_POLICY_LIMIT_RESET_ON = details.InsuranceToCreateUpdate.BENEFIT_POLICY_LIMIT_RESET_ON;
            dbPatientInsurance.MYB_LIMIT_DOLLARS = details.InsuranceToCreateUpdate.MYB_LIMIT_DOLLARS;
            dbPatientInsurance.MYB_LIMIT_VISIT = details.InsuranceToCreateUpdate.MYB_LIMIT_VISIT;
            dbPatientInsurance.MYB_USED_OUTSIDE_DOLLARS = details.InsuranceToCreateUpdate.MYB_USED_OUTSIDE_DOLLARS;
            dbPatientInsurance.MYB_USED_OUTSIDE_VISIT = details.InsuranceToCreateUpdate.MYB_USED_OUTSIDE_VISIT;
            dbPatientInsurance.MYB_USED_DOLLARS = details.InsuranceToCreateUpdate.MYB_USED_DOLLARS;
            dbPatientInsurance.MYB_USED_VISIT = details.InsuranceToCreateUpdate.MYB_USED_VISIT;
            dbPatientInsurance.MYB_REMAINING_DOLLARS = details.InsuranceToCreateUpdate.MYB_REMAINING_DOLLARS;
            dbPatientInsurance.MYB_REMAINING_VISIT = details.InsuranceToCreateUpdate.MYB_REMAINING_VISIT;
            dbPatientInsurance.MOP_AMT = details.InsuranceToCreateUpdate.MOP_AMT;
            dbPatientInsurance.MOP_USED_OUTSIDE_RT = details.InsuranceToCreateUpdate.MOP_USED_OUTSIDE_RT;
            dbPatientInsurance.MOP_USED = details.InsuranceToCreateUpdate.MOP_USED;
            dbPatientInsurance.MOP_REMAINING = details.InsuranceToCreateUpdate.MOP_REMAINING;
            dbPatientInsurance.SPOKE_TO = details.InsuranceToCreateUpdate.SPOKE_TO;
            dbPatientInsurance.BENEFIT_COMMENTS = details.InsuranceToCreateUpdate.BENEFIT_COMMENTS;
            dbPatientInsurance.GENERAL_COMMENTS = details.InsuranceToCreateUpdate.GENERAL_COMMENTS;
            dbPatientInsurance.IS_COPAY_PER = details.InsuranceToCreateUpdate.IS_COPAY_PER ?? false;
            //dbPatientInsurance.IS_VERIFIED = details.InsuranceToCreateUpdate.IS_VERIFIED;
            //if (dbPatientInsurance.IS_VERIFIED ?? false)
            //{
            //    dbPatientInsurance.VERIFIED_BY = profile.UserName;
            //    dbPatientInsurance.VERIFIED_DATE = Helper.GetCurrentDate();
            //}
            dbPatientInsurance.IS_VERIFIED = true;
            dbPatientInsurance.VERIFIED_BY = profile.UserName;
            dbPatientInsurance.VERIFIED_DATE = Helper.GetCurrentDate();
            dbPatientInsurance.CHK_ABN = details.InsuranceToCreateUpdate.CHK_ABN;
            dbPatientInsurance.CHK_HOME_HEALTH_EPISODE = details.InsuranceToCreateUpdate.CHK_HOME_HEALTH_EPISODE;
            dbPatientInsurance.CHK_HOSPICE = details.InsuranceToCreateUpdate.CHK_HOSPICE;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Medicare Info

            if (details.InsuranceToCreateUpdate.CurrentMedicareLimitList.Count != 0)
            {
                MedicareLimit newABNData = details.InsuranceToCreateUpdate.CurrentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "ABN").FirstOrDefault();
                newABNData.Patient_Account = dbPatientInsurance.Patient_Account;
                //UpdateABNData(newABNData, dbPatientInsurance.ABN_LIMIT_ID);
                dbPatientInsurance.ABN_LIMIT_ID = abnInfoChanged ? CreateNewLimit(dbPatientInsurance.ABN_LIMIT_ID, newABNData, "ABN", profile) : dbPatientInsurance.ABN_LIMIT_ID;

                MedicareLimit newHOSData = details.InsuranceToCreateUpdate.CurrentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "Hospice").FirstOrDefault();
                newHOSData.Patient_Account = dbPatientInsurance.Patient_Account;
                //UpdateHOSData(newHOSData, dbPatientInsurance.HOSPICE_LIMIT_ID);
                dbPatientInsurance.HOSPICE_LIMIT_ID = hosInfoChanged ? CreateNewLimit(dbPatientInsurance.HOSPICE_LIMIT_ID, newHOSData, "Hospice", profile) : dbPatientInsurance.HOSPICE_LIMIT_ID;

                MedicareLimit newHHData = details.InsuranceToCreateUpdate.CurrentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "Home Health Episode").FirstOrDefault();
                newHHData.Patient_Account = dbPatientInsurance.Patient_Account;
                //UpdateHHData(newHHData, dbPatientInsurance.HOME_HEALTH_LIMIT_ID);
                dbPatientInsurance.HOME_HEALTH_LIMIT_ID = hhInfoChanged ? CreateNewLimit(dbPatientInsurance.HOME_HEALTH_LIMIT_ID, newHHData, "Home Health Episode", profile) : dbPatientInsurance.HOME_HEALTH_LIMIT_ID;
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            dbPatientInsurance.ELIG_LOADED_ON = Helper.GetCurrentDate();
            dbPatientInsurance.Modified_By = profile.UserName;
            dbPatientInsurance.Modified_Date = Helper.GetCurrentDate();
            dbPatientInsurance.Deleted = false;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _PatientInsuranceRepository.Update(dbPatientInsurance);
            _PatientContext.SaveChanges();
            //if (details.is_Change)
            //Task 149402:Dev Task: FOX - RT 105.Disabling editing of patient info. from RFO, Usman Nasir
            //InsertInterfaceTeamData(interfaceSynch, profile);
            details.is_Change = false;
            if (!string.IsNullOrWhiteSpace(details.InsuranceToCreateUpdate.Eligibility_MSP_Data))
            {
                string tasktype = "BLOCK";

                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                var interfaceTask = setTaskData(profile, details.InsuranceToCreateUpdate.Patient_Account, tasktype, details.InsuranceToCreateUpdate.CURRENT_DATE_STR);
                var taskInterfaced = AddUpdateTask(interfaceTask, profile, details.InsuranceToCreateUpdate.Eligibility_MSP_Data);
            }
            if (!string.IsNullOrWhiteSpace(details.InsuranceToCreateUpdate.Deceased_Date_In_String))
            {
                CreateAlertforDeceasedPatient(dbPatientInsurance.Patient_Account, details.InsuranceToCreateUpdate.Deceased_Date.Value, profile);
            }
            resp.Message = "Eligibility updated successfully.";
            resp.Success = true;
            return dbPatientInsurance;
        }

        private void SaveDeceasedDateInPatient(long patAccount, DateTime? deceased_Date, UserProfile profile)
        {
            var patient = _PatientRepository.GetFirst(e => e.Patient_Account == patAccount && (e.DELETED ?? false) == false);
            if (patient != null)
            {
                patient.Expiry_Date = deceased_Date;
                patient.Modified_By = profile.UserName;
                patient.Modified_Date = Helper.GetCurrentDate();
                _PatientRepository.Update(patient);
                _PatientRepository.Save();

                SaveUpdateExpiredBit(patAccount, profile);

            }
        }

        private void CreateAlertforDeceasedPatient(long patAccount, DateTime? deceased_Date, UserProfile profile)
        {
            var alerttypeID = _alertTypeRepository.GetSingle(a => a.PRACTICE_CODE == profile.PracticeCode && !a.DELETED && a.DESCRIPTION == "Expired")?.ALERT_TYPE_ID;
            if (alerttypeID.HasValue)
            {
                NoteAlert noteAlert = new NoteAlert();
                noteAlert.FOX_TBL_ALERT_ID = Helper.getMaximumId("FOX_TBL_ALERT_ID");
                noteAlert.ALERT_TYPE_ID = alerttypeID.Value;
                noteAlert.NOTE_DETAIL = "Patient expired on " + deceased_Date.Value.ToString("MM/dd/yyyy");
                noteAlert.EFFECTIVE_TILL = DateTime.Now.AddDays(14);
                noteAlert.PATIENT_ACCOUNT = patAccount;
                noteAlert.PRACTICE_CODE = profile.PracticeCode;
                noteAlert.CREATED_BY = profile.UserName;
                noteAlert.CREATED_DATE = Helper.GetCurrentDate();
                noteAlert.MODIFIED_BY = profile.UserName;
                noteAlert.MODIFIED_DATE = Helper.GetCurrentDate();
                noteAlert.DELETED = false;
                _noteAlertRepository.Insert(noteAlert);
                _noteAlertRepository.Save();
            }
        }

        private void SaveUpdateExpiredBit(long patAccount, UserProfile profile)
        {
            var restofpatient = _FoxTblPatientRepository.GetFirst(e => e.Patient_Account == patAccount && (e.DELETED ?? false) == false);
            if (restofpatient != null)
            {
                restofpatient.Expired = true;
                restofpatient.Patient_Status = "Expired";
                _FoxTblPatientRepository.Update(restofpatient);
            }
            else
            {
                restofpatient.FOX_TBL_PATIENT_ID = Helper.getMaximumId("FOX_TBL_PATIENT");
                restofpatient.Patient_Account = patAccount;
                restofpatient.Created_By = restofpatient.Modified_By = profile.UserName;
                restofpatient.Created_Date = restofpatient.Modified_Date = Helper.GetCurrentDate();
                restofpatient.DELETED = false;
                restofpatient.Expired = true;
                restofpatient.Patient_Status = "Expired";
                _FoxTblPatientRepository.Insert(restofpatient);
            }
            _FoxTblPatientRepository.Save();
            SaveUpdateExpiredBitInTalkEHR(patAccount, profile);
        }

        private void SaveUpdateExpiredBitInTalkEHR(long patAccount, UserProfile profile)
        {
            var restofpatient = _talkEHRTblAdditionalPatientInfoRepository.GetFirst(e => e.Patient_Account == patAccount && (e.DELETED ?? false) == false);
            if (restofpatient != null)
            {
                restofpatient.Expired = true;
                restofpatient.Patient_Status = "Expired";
                _talkEHRTblAdditionalPatientInfoRepository.Update(restofpatient);
            }
            else
            {
                restofpatient.PATIENT_ADDITIONAL_INFO_ID = Helper.getMaximumId("PATIENT_ADDITIONAL_INFO_ID");
                restofpatient.Patient_Account = patAccount;
                restofpatient.CREATED_BY = restofpatient.MODIFIED_BY = profile.UserName;
                restofpatient.CREATED_DATE = restofpatient.MODIFIED_DATE = Helper.GetCurrentDate();
                restofpatient.Expired = true;
                restofpatient.Patient_Status = "Expired";
                restofpatient.DELETED = false;
                _talkEHRTblAdditionalPatientInfoRepository.Insert(restofpatient);
            }
            _talkEHRTblAdditionalPatientInfoRepository.Save();
        }

        public bool SaveDynamicPatientResponsibilityInsurance(string patient_Account, UserProfile profile)
        {
            //PatientInsuranceDetail ObjPatientInsuranceDetail = new PatientInsuranceDetail();
            //ObjPatientInsuranceDetail = GetSmartInsurancePayers();
            //var PracticeCode = Convert.ToInt64(profile.PracticeCode);
            PatientInsuranceEligibilityDetail ObjPatientInsuranceEligibilityDetail = new PatientInsuranceEligibilityDetail();
            ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate = new PatientInsurance();
            ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.IS_PRIVATE_PAY = false;

            if (!string.IsNullOrWhiteSpace(patient_Account.ToString()))
            {
                ObjPatientInsuranceEligibilityDetail.Patient_Account_Str = patient_Account;
                ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Pri_Sec_Oth_Type = "PR";
                ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Policy_Number = "";

                long pat_acc = long.Parse(patient_Account.ToString());
                var fcObj = _financialClassRepository.GetFirst(e => e.PRACTICE_CODE == profile.PracticeCode && e.SHOW_FOR_INSURANCE && e.CODE.ToLower() == "pr" && !e.DELETED);
                if (fcObj != null)
                {
                    ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.FINANCIAL_CLASS_ID = fcObj.FINANCIAL_CLASS_ID;
                }
                var foxIns = _foxInsurancePayersRepository.GetFirst(e => e.INSURANCE_PAYERS_ID == "00003" && (e.DELETED ?? false) == false && e.PRACTICE_CODE == profile.PracticeCode);
                if (foxIns != null)
                {
                    ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.FOX_TBL_INSURANCE_ID = foxIns.FOX_TBL_INSURANCE_ID;
                    ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.InsPayer_Description = foxIns.INSURANCE_NAME;
                    //var contact = GetPatientContactInfoForPR(details.Patient_Account_Str, profile);
                    SubscriberInfoRequest req = new SubscriberInfoRequest();
                    req.patientAccount = pat_acc;
                    req.Practice_Code = profile.PracticeCode;
                    PatientContact con = null;
                    var patient_misc_data = GetSubscriberInfo(req);
                    //var homeaddress_Address = patient_misc_data.PatientAddress.Where(a => a.ADDRESS_TYPE != null && a.ADDRESS_TYPE.ToLower() == "home address").OrderByDescending(e => e.MODIFIED_DATE).FirstOrDefault();
                    //if (homeaddress_Address != null && patient_misc_data.patientinfo != null)
                    //{
                    //    ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship = "S";
                    //}
                    //else
                    //{
                    if (patient_misc_data.PatientContactsList.Count > 0)
                    {
                        string con_Type = string.Empty; string con_Type_Code = string.Empty;
                        //var financial_Con = patient_misc_data.PatientContactsList.Where(e => (e.Flag_Financially_Responsible_Party ?? false) || (e.Flag_Power_Of_Attorney_Financial ?? false)).OrderByDescending(e => e.Contact_ID).FirstOrDefault();
                        //Change by arqam
                        var financial_Con = patient_misc_data.PatientContactsList.Where(e => (e.Flag_Financially_Responsible_Party ?? false)).OrderByDescending(e => e.Contact_ID).FirstOrDefault();

                        if (financial_Con != null)
                        {
                            con = financial_Con;
                            if (con != null)
                            {
                                var con_type = patient_misc_data.ContactTypeList.Where(e => e.Contact_Type_ID == con.Contact_Type_Id).FirstOrDefault();
                                if (con_type != null)
                                {
                                    con_Type = con_type.Type_Name;
                                }
                                switch (con_Type)
                                {
                                    case "Son":
                                    case "Daughter":
                                        ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship = "C";
                                        break;
                                    case "Self":
                                        ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship = "S";
                                        break;
                                    case "Spouse":
                                        ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship = "SP";
                                        break;
                                    default:
                                        ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship = "O";
                                        break;
                                }
                            }
                            else
                            {
                                ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship = "S";
                            }
                        }
                        else
                        {
                            ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship = "S";
                        }

                    }
                    else
                    {
                        ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship = "S";
                    }
                    //}

                    ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Effective_Date_In_String = String.Format("{0:ddd MMM dd yyyy}", DateTime.Now);
                    ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Subscriber = CheckAndCreateSubscriberForPR(con, patient_misc_data, profile, ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Relationship);
                    if (ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate.Subscriber != null)
                    {
                        SaveInsuranceAndEligibilityDetails(ObjPatientInsuranceEligibilityDetail, profile, false);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private long? CheckAndCreateSubscriberForPR(PatientContact con, SubscriberInformation patient_misc_data, UserProfile profile, string Relation)
        {
            Subscriber ObjSubscriber = new Subscriber();
            long? subscriberId = null;
            //if (Relation == "S")
            //{
            if (con != null && con.Contact_ID != 0 && con.Contact_Type_Id != 0)
            {
                ObjSubscriber.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                subscriberId = ObjSubscriber.GUARANTOR_CODE;
                ObjSubscriber.guarant_practice_code = profile.PracticeCode;
                ObjSubscriber.GUARANT_FNAME = con.First_Name;
                ObjSubscriber.GUARANT_MI = con.MI;
                ObjSubscriber.GUARANT_LNAME = con.Last_Name;
                ObjSubscriber.GUARANT_ADDRESS = con.Address;
                ObjSubscriber.GUARANT_ZIP = con.Zip;
                ObjSubscriber.GUARANT_CITY = con.City;
                ObjSubscriber.GUARANT_STATE = con.State;
                ObjSubscriber.GUARANT_HOME_PHONE = con.Home_Phone;
                ObjSubscriber.created_by = ObjSubscriber.modified_by = profile.UserName;
                ObjSubscriber.created_date = ObjSubscriber.modified_date = Helper.GetCurrentDate();
                ObjSubscriber.Guarant_Type = "G";
                ObjSubscriber.Deleted = false;
                _SubscriberRepository.Insert(ObjSubscriber);
                _SubscriberRepository.Save();
            }
            else
            {
                var homeaddress_Address = patient_misc_data.PatientAddress.Where(a => a.ADDRESS_TYPE != null && a.ADDRESS_TYPE.ToLower() == "home address").OrderByDescending(e => e.MODIFIED_DATE).FirstOrDefault();
                if (homeaddress_Address != null && patient_misc_data.patientinfo != null)
                {
                    ObjSubscriber.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                    subscriberId = ObjSubscriber.GUARANTOR_CODE;
                    ObjSubscriber.guarant_practice_code = profile.PracticeCode;
                    ObjSubscriber.GUARANT_FNAME = patient_misc_data.patientinfo.FirstName;
                    ObjSubscriber.GUARANT_MI = patient_misc_data.patientinfo.MIDDLE_NAME;
                    ObjSubscriber.GUARANT_LNAME = patient_misc_data.patientinfo.LastName;
                    ObjSubscriber.GUARANT_DOB = patient_misc_data.patientinfo.Date_Of_Birth;
                    ObjSubscriber.GUARANT_GENDER = patient_misc_data.patientinfo.Gender;
                    ObjSubscriber.GUARANT_ADDRESS = homeaddress_Address.ADDRESS;
                    ObjSubscriber.GUARANT_ZIP = homeaddress_Address.ZIP;
                    ObjSubscriber.GUARANT_CITY = homeaddress_Address.CITY;
                    ObjSubscriber.GUARANT_STATE = homeaddress_Address.STATE;
                    ObjSubscriber.Guarant_Type = "H";

                    ObjSubscriber.created_by = ObjSubscriber.modified_by = profile.UserName;
                    ObjSubscriber.created_date = ObjSubscriber.modified_date = Helper.GetCurrentDate();
                    ObjSubscriber.Deleted = false;
                    _SubscriberRepository.Insert(ObjSubscriber);
                    _SubscriberRepository.Save();
                }
                else
                {
                    ObjSubscriber.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
                    subscriberId = ObjSubscriber.GUARANTOR_CODE;
                    ObjSubscriber.guarant_practice_code = profile.PracticeCode;
                    ObjSubscriber.GUARANT_FNAME = patient_misc_data.patientinfo.FirstName;
                    ObjSubscriber.GUARANT_MI = patient_misc_data.patientinfo.MIDDLE_NAME;
                    ObjSubscriber.GUARANT_LNAME = patient_misc_data.patientinfo.LastName;
                    ObjSubscriber.GUARANT_DOB = patient_misc_data.patientinfo.Date_Of_Birth;
                    ObjSubscriber.GUARANT_GENDER = patient_misc_data.patientinfo.Gender;
                    ObjSubscriber.GUARANT_ADDRESS = "7 Carnegie Plaza";
                    ObjSubscriber.GUARANT_ZIP = "080031000";
                    ObjSubscriber.GUARANT_CITY = "Cherry Hill";
                    ObjSubscriber.GUARANT_STATE = "NJ";
                    ObjSubscriber.Guarant_Type = "S";

                    ObjSubscriber.created_by = ObjSubscriber.modified_by = profile.UserName;
                    ObjSubscriber.created_date = ObjSubscriber.modified_date = Helper.GetCurrentDate();
                    ObjSubscriber.Deleted = false;
                    _SubscriberRepository.Insert(ObjSubscriber);
                    _SubscriberRepository.Save();
                }
            }
            //else
            //{
            //    if (con != null && con.Contact_ID != 0 && con.Contact_Type_Id != 0)
            //    {
            //        ObjSubscriber.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
            //        subscriberId = ObjSubscriber.GUARANTOR_CODE;
            //        ObjSubscriber.guarant_practice_code = profile.PracticeCode;
            //        ObjSubscriber.GUARANT_FNAME = con.First_Name;
            //        ObjSubscriber.GUARANT_MI = con.MI;
            //        ObjSubscriber.GUARANT_LNAME = con.Last_Name;
            //        ObjSubscriber.GUARANT_ADDRESS = con.Address;
            //        ObjSubscriber.GUARANT_ZIP = con.Zip;
            //        ObjSubscriber.GUARANT_CITY = con.City;
            //        ObjSubscriber.GUARANT_STATE = con.State;
            //        ObjSubscriber.GUARANT_HOME_PHONE = con.Home_Phone;
            //        ObjSubscriber.created_by = ObjSubscriber.modified_by = profile.UserName;
            //        ObjSubscriber.created_date = ObjSubscriber.modified_date = Helper.GetCurrentDate();
            //        ObjSubscriber.Guarant_Type = "G";
            //        ObjSubscriber.Deleted = false;
            //        _SubscriberRepository.Insert(ObjSubscriber);
            //        _SubscriberRepository.Save();
            //    }
            //    else
            //    {
            //        var statement_Address = patient_misc_data.PatientAddress.Where(a => a.ADDRESS_TYPE != null && a.ADDRESS_TYPE.ToLower() == "statement address").OrderByDescending(e => e.MODIFIED_DATE).FirstOrDefault();
            //        if (statement_Address != null && patient_misc_data.patientinfo != null)
            //        {
            //            ObjSubscriber.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
            //            subscriberId = ObjSubscriber.GUARANTOR_CODE;
            //            ObjSubscriber.guarant_practice_code = profile.PracticeCode;
            //            ObjSubscriber.GUARANT_FNAME = patient_misc_data.patientinfo.FirstName;
            //            ObjSubscriber.GUARANT_MI = patient_misc_data.patientinfo.MIDDLE_NAME;
            //            ObjSubscriber.GUARANT_LNAME = patient_misc_data.patientinfo.LastName;
            //            ObjSubscriber.GUARANT_DOB = patient_misc_data.patientinfo.Date_Of_Birth;
            //            ObjSubscriber.GUARANT_GENDER = patient_misc_data.patientinfo.Gender;
            //            ObjSubscriber.GUARANT_ADDRESS = statement_Address.ADDRESS;
            //            ObjSubscriber.GUARANT_ZIP = statement_Address.ZIP;
            //            ObjSubscriber.GUARANT_CITY = statement_Address.CITY;
            //            ObjSubscriber.GUARANT_STATE = statement_Address.STATE;
            //            ObjSubscriber.Guarant_Type = "S";

            //            ObjSubscriber.created_by = ObjSubscriber.modified_by = profile.UserName;
            //            ObjSubscriber.created_date = ObjSubscriber.modified_date = Helper.GetCurrentDate();
            //            ObjSubscriber.Deleted = false;
            //            _SubscriberRepository.Insert(ObjSubscriber);
            //            _SubscriberRepository.Save();
            //        }
            //        else
            //        {
            //            ObjSubscriber.GUARANTOR_CODE = Helper.getMaximumId("GUARANTOR_CODE");
            //            subscriberId = ObjSubscriber.GUARANTOR_CODE;
            //            ObjSubscriber.guarant_practice_code = profile.PracticeCode;
            //            ObjSubscriber.GUARANT_FNAME = patient_misc_data.patientinfo.FirstName;
            //            ObjSubscriber.GUARANT_MI = patient_misc_data.patientinfo.MIDDLE_NAME;
            //            ObjSubscriber.GUARANT_LNAME = patient_misc_data.patientinfo.LastName;
            //            ObjSubscriber.GUARANT_DOB = patient_misc_data.patientinfo.Date_Of_Birth;
            //            ObjSubscriber.GUARANT_GENDER = patient_misc_data.patientinfo.Gender;
            //            ObjSubscriber.GUARANT_ADDRESS = "7 Carnegie Plaza";
            //            ObjSubscriber.GUARANT_ZIP = "080031000";
            //            ObjSubscriber.GUARANT_CITY = "Cherry Hill";
            //            ObjSubscriber.GUARANT_STATE = "NJ";
            //            ObjSubscriber.Guarant_Type = "H";

            //            ObjSubscriber.created_by = ObjSubscriber.modified_by = profile.UserName;
            //            ObjSubscriber.created_date = ObjSubscriber.modified_date = Helper.GetCurrentDate();
            //            ObjSubscriber.Deleted = false;
            //            _SubscriberRepository.Insert(ObjSubscriber);
            //            _SubscriberRepository.Save();
            //        }
            //    }
            //}
            return subscriberId;
        }

        public void SetTerminationDateForExistingPayers(PatientInsurance recentlyCreatedInsurance, UserProfile profile)
        {
            if (recentlyCreatedInsurance.Effective_Date.HasValue)
            {
                foreach (var exisingPayer_PatientInsuranceId in recentlyCreatedInsurance.Patient_Insurancs_Ids_With_Overlapping_Dates)
                {
                    var ins = _PatientInsuranceRepository.GetFirst(e => e.Patient_Insurance_Id == exisingPayer_PatientInsuranceId);
                    if (ins != null)
                    {
                        ins.Termination_Date = recentlyCreatedInsurance.Effective_Date.Value.AddDays(-1);
                        ins.Modified_By = "FOX TEAM";
                        ins.Modified_Date = Helper.GetCurrentDate();
                        _PatientInsuranceRepository.Update(ins);
                        _PatientInsuranceRepository.Save();
                        UpdateMTBCInsuranceTerminationDate(ins, profile);
                    }
                }
            }
        }

        public void UpdateMTBCInsuranceTerminationDate(PatientInsurance ins, UserProfile profile)
        {
            var mtbcInsurance = _MTBCPatientInsuranceRepository.GetFirst(e => e.Patient_Insurance_Id == ins.MTBC_Patient_Insurance_Id);
            if (mtbcInsurance != null)
            {
                mtbcInsurance.Termination_Date = ins.Termination_Date;
                mtbcInsurance.Modified_By = "FOX TEAM";
                mtbcInsurance.Modified_Date = ins.Modified_Date;
                _MTBCPatientInsuranceRepository.Update(mtbcInsurance);
                _MTBCPatientInsuranceRepository.Save();
            }
        }

        public void MapClaimsToNewInsurance(PatientInsurance recentlyCreatedInsurance, UserProfile profile)
        {
            FOX_TBL_PATIENT foxObj = new FOX_TBL_PATIENT();
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            interfaceSynch.PATIENT_ACCOUNT = recentlyCreatedInsurance.Patient_Account;
            foreach (var claim in recentlyCreatedInsurance.ClaimsToMapToNewInsurance)
            {
                var claimIns = _claimInsuranceRepository.GetFirst(e => e.Claim_Insurance_Id == claim.CLAIM_INSURANCE_ID && e.Insurance_Id == claim.INSURANCE_ID && e.Patient_Account == claim.PATIENT_ACCOUNT
                    && (e.Deleted ?? false) == false && e.Pri_Sec_Oth_Type == recentlyCreatedInsurance.Pri_Sec_Oth_Type);

                if (claimIns != null)
                {
                    var foxIns = _foxInsurancePayersRepository.GetFirst(e => e.FOX_TBL_INSURANCE_ID == recentlyCreatedInsurance.FOX_TBL_INSURANCE_ID && (e.DELETED ?? false) == false);
                    if (foxIns != null && foxIns.INSURANCE_ID.HasValue && foxIns.INSURANCE_ID.Value != 0)
                    {
                        claimIns.Insurance_Id = foxIns.INSURANCE_ID.Value;
                        claimIns.Modified_By = "FOX_" + profile.UserName;
                        claimIns.Modified_Date = Helper.GetCurrentDate();

                        _claimInsuranceRepository.Update(claimIns);
                        _claimInsuranceRepository.Save();
                        AddClaimNotes(recentlyCreatedInsurance, claim.CLAIM_NO.Value, profile);
                    }
                }
            }
        }

        public void AddClaimNotes(PatientInsurance recentlyCreatedInsurance, long claim_No, UserProfile profile)
        {
            var claimNote = new ClaimNotes();
            claimNote.Claim_Notes_Id = Helper.getMaximumId("Claim_Notes_Id");
            claimNote.Claim_No = claim_No;
            claimNote.Note_Id = 500430;//GENERAL - Its same id on vsub, uat and mis_db
            claimNote.Note_Detail = "FOX-" + profile.UserName + " | " +
                (recentlyCreatedInsurance.Created_Date.HasValue ? recentlyCreatedInsurance.Created_Date.Value.ToString("MM/dd/yyyy") : Helper.GetCurrentDate().ToString("MM/dd/yyyy")) + " | " +
                recentlyCreatedInsurance.InsPayer_Description + " added as " + GetInsuraneTypeName(recentlyCreatedInsurance.Pri_Sec_Oth_Type, (recentlyCreatedInsurance.IS_PRIVATE_PAY ?? false)).ToLower() + " insurance.";

            claimNote.Created_By = claimNote.Modified_By = profile.UserName;
            claimNote.Created_Date = Helper.GetCurrentDate();
            claimNote.Modified_Date = Helper.GetCurrentDate();
            claimNote.created_From = "FOX Portal";
            _claimNotesRepository.Insert(claimNote);
            _claimNotesRepository.Save();
        }

        public string GetInsuraneTypeName(string pri_Sec_Oth_Type, bool is_PP)
        {
            string insuraneName = string.Empty;
            switch (pri_Sec_Oth_Type)
            {
                case "P":
                    insuraneName = "Primary";
                    break;
                case "S":
                    insuraneName = "Secondary";
                    break;
                case "T":
                    insuraneName = "Tertiary";
                    break;
                case "Q":
                    insuraneName = "Quarternary";
                    break;
                case "PR":
                    if (is_PP)
                        insuraneName = "Private Pay";
                    else
                        insuraneName = "Residual Balance";
                    break;
                default:
                    break;
            }
            return insuraneName;
        }

        public long? CreateNewLimit(long? old_Lim_ID, MedicareLimit newABNData, string lim_Type, UserProfile profile)
        {
            FOX_TBL_PATIENT foxObj = new FOX_TBL_PATIENT();
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            interfaceSynch.PATIENT_ACCOUNT = newABNData.Patient_Account;
            interfaceSynch.CASE_ID = newABNData.CASE_ID;
            var medicareLimObj = new MedicareLimit();
            long? new_lim_Id = null;
            var dbLim = _MedicareLimitRepository.GetByID(old_Lim_ID);
            if (!string.IsNullOrEmpty(newABNData.EFFECTIVE_DATE_IN_STRING))
                newABNData.EFFECTIVE_DATE = Convert.ToDateTime(newABNData.EFFECTIVE_DATE_IN_STRING);
            if (!string.IsNullOrEmpty(newABNData.END_DATE_IN_STRING))
                newABNData.END_DATE = Convert.ToDateTime(newABNData.END_DATE_IN_STRING);
            if (dbLim == null)//Added First time
            {
                if (lim_Type.Equals("ABN"))
                {
                    if (newABNData.EFFECTIVE_DATE != null || newABNData.END_DATE != null || newABNData.ABN_EST_WK_COST != null || !string.IsNullOrWhiteSpace(newABNData.ABN_COMMENTS))
                    {
                        if (!string.IsNullOrEmpty(newABNData.EFFECTIVE_DATE_IN_STRING))
                            newABNData.EFFECTIVE_DATE = Convert.ToDateTime(newABNData.EFFECTIVE_DATE_IN_STRING);
                        if (!string.IsNullOrEmpty(newABNData.END_DATE_IN_STRING))
                            newABNData.END_DATE = Convert.ToDateTime(newABNData.END_DATE_IN_STRING);
                        //has info to save
                        new_lim_Id = Helper.getMaximumId("FOX_REHAB_MEDICARE_LIMIT_ID");

                        newABNData.MEDICARE_LIMIT_ID = new_lim_Id.Value;
                        newABNData.PRACTICE_CODE = profile.PracticeCode;
                        //newABNData.CASE_ID = details.InsuranceToCreateUpdate.CASE_ID;
                        newABNData.MEDICARE_LIMIT_STATUS = "C";
                        newABNData.CREATED_BY = newABNData.MODIFIED_BY = profile.UserName;
                        newABNData.CREATED_DATE = newABNData.MODIFIED_DATE = Helper.GetCurrentDate();
                        newABNData.DELETED = false;
                        _MedicareLimitRepository.Insert(newABNData);
                        _PatientContext.SaveChanges();
                    }
                }
                else if (lim_Type.Equals("Hospice") || lim_Type.Equals("Home Health Episode"))
                {
                    if (newABNData.EFFECTIVE_DATE != null || newABNData.END_DATE != null || !string.IsNullOrWhiteSpace(newABNData.NPI))
                    {
                        //has info to save
                        new_lim_Id = Helper.getMaximumId("FOX_REHAB_MEDICARE_LIMIT_ID");
                        newABNData.MEDICARE_LIMIT_ID = new_lim_Id.Value;
                        newABNData.PRACTICE_CODE = profile.PracticeCode;
                        //newABNData.Patient_Account = patAccount;
                        //newABNData.CASE_ID = details.InsuranceToCreateUpdate.CASE_ID;
                        newABNData.MEDICARE_LIMIT_STATUS = "C";
                        newABNData.CREATED_BY = newABNData.MODIFIED_BY = profile.UserName;
                        newABNData.CREATED_DATE = newABNData.MODIFIED_DATE = Helper.GetCurrentDate();
                        newABNData.DELETED = false;
                        _MedicareLimitRepository.Insert(newABNData);
                        _PatientContext.SaveChanges();
                    }
                }
            }
            else
            {
                new_lim_Id = Helper.getMaximumId("FOX_REHAB_MEDICARE_LIMIT_ID");
                medicareLimObj.MEDICARE_LIMIT_ID = new_lim_Id.Value;
                medicareLimObj.MEDICARE_LIMIT_STATUS = "C"; //Create new limit with current status
                medicareLimObj.CREATED_BY = dbLim.CREATED_BY; //Keep created by value same as it was for the first insurance created
                medicareLimObj.MODIFIED_BY = profile.UserName;
                medicareLimObj.CREATED_DATE = dbLim.CREATED_DATE; //Keep created date same as it was for the first insurance created
                medicareLimObj.MODIFIED_DATE = Helper.GetCurrentDate();
                medicareLimObj.DELETED = false;
                //medicareLimObj.Patient_Account = dbLim.Patient_Account;
                //medicareLimObj.CASE_ID = details.InsuranceToCreateUpdate.CASE_ID;
                medicareLimObj.PRACTICE_CODE = dbLim.PRACTICE_CODE;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                medicareLimObj.EFFECTIVE_DATE = newABNData.EFFECTIVE_DATE;
                medicareLimObj.END_DATE = newABNData.END_DATE;
                medicareLimObj.Patient_Account = dbLim.Patient_Account;
                if (lim_Type.Equals("ABN"))
                {
                    medicareLimObj.ABN_EST_WK_COST = newABNData.ABN_EST_WK_COST;
                    medicareLimObj.ABN_COMMENTS = newABNData.ABN_COMMENTS;
                }
                else
                {
                    medicareLimObj.NPI = newABNData.NPI;
                }

                medicareLimObj.MEDICARE_LIMIT_TYPE_ID = dbLim.MEDICARE_LIMIT_TYPE_ID;
                _MedicareLimitRepository.Insert(medicareLimObj);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Update previous limit status to Old
                dbLim.MEDICARE_LIMIT_STATUS = "O";
                dbLim.MODIFIED_BY = profile.UserName;
                dbLim.MODIFIED_DATE = Helper.GetCurrentDate();
                _MedicareLimitRepository.Update(dbLim);
                _PatientContext.SaveChanges();
            }

            return new_lim_Id;
        }

        public bool ABN_MedicareLimitDataChanged(long? abn_Id, List<MedicareLimit> currentMedicareLimitList, out bool abnInfoChanged)
        {
            abnInfoChanged = false;
            var newData = currentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "ABN").FirstOrDefault();
            if (abn_Id != null)
            {
                var oldData = _MedicareLimitRepository.GetByID(abn_Id ?? 0);

                if (oldData.EFFECTIVE_DATE != newData.EFFECTIVE_DATE) { abnInfoChanged = true; return true; }
                if (oldData.END_DATE != newData.END_DATE) { abnInfoChanged = true; return true; }
                if ((oldData.ABN_EST_WK_COST ?? 0) != (newData.ABN_EST_WK_COST ?? 0)) { abnInfoChanged = true; return true; }
                if (!String.Equals(oldData.ABN_COMMENTS ?? "", newData.ABN_COMMENTS ?? "", StringComparison.Ordinal)) { abnInfoChanged = true; return true; }
            }
            else
            {
                if (newData != null)
                {
                    if (newData.EFFECTIVE_DATE != null || newData.END_DATE != null || newData.ABN_EST_WK_COST != null || !string.IsNullOrWhiteSpace(newData.ABN_COMMENTS))
                    {
                        abnInfoChanged = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HOS_MedicareLimitDataChanged(long? hos_Id, List<MedicareLimit> currentMedicareLimitList, out bool hosInfoChanged)
        {
            hosInfoChanged = false;
            var newData = currentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "Hospice").FirstOrDefault();
            if (hos_Id != null)
            {
                //var oldData = _MedicareLimitRepository.GetByID(hos_Id.Value);
                var oldData = _MedicareLimitRepository.GetFirst(l => l.MEDICARE_LIMIT_ID == hos_Id.Value);
                if (oldData != null)
                {
                    if (oldData.EFFECTIVE_DATE != newData.EFFECTIVE_DATE) { hosInfoChanged = true; return true; }
                    if (oldData.EFFECTIVE_DATE != newData.EFFECTIVE_DATE) { hosInfoChanged = true; return true; }
                    if (oldData.END_DATE != newData.END_DATE) { hosInfoChanged = true; return true; }
                    if (!String.Equals(oldData.NPI ?? "", newData.NPI ?? "", StringComparison.Ordinal)) { hosInfoChanged = true; return true; }
                }
            }
            else
            {
                if (newData != null)
                {
                    if (newData.EFFECTIVE_DATE != null || newData.END_DATE != null || !string.IsNullOrWhiteSpace(newData.NPI))
                    {
                        hosInfoChanged = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HH_MedicareLimitDataChanged(long? hh_Id, List<MedicareLimit> currentMedicareLimitList, out bool hhInfoChanged)
        {
            hhInfoChanged = false;
            var newData = currentMedicareLimitList.Where(e => e.MEDICARE_LIMIT_TYPE_NAME == "Home Health Episode").FirstOrDefault();
            if (hh_Id != null)
            {
                // var oldData = _MedicareLimitRepository.GetByID(hh_Id ?? 0);
                var oldData = _MedicareLimitRepository.GetFirst(l => l.MEDICARE_LIMIT_ID == hh_Id);
                if (oldData != null)
                {
                    if (oldData.EFFECTIVE_DATE != newData.EFFECTIVE_DATE) { hhInfoChanged = true; return true; }
                    if (oldData.END_DATE != newData.END_DATE) { hhInfoChanged = true; return true; }
                    if (!String.Equals(oldData.NPI ?? "", newData.NPI ?? "", StringComparison.Ordinal)) { hhInfoChanged = true; return true; }
                }
            }
            else
            {
                if (newData != null)
                {
                    if (newData.EFFECTIVE_DATE != null || newData.END_DATE != null || !string.IsNullOrWhiteSpace(newData.NPI))
                    {
                        hhInfoChanged = true;
                        return true;
                    }
                }
            }
            return false;
        }
        //Medicare Info
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool MedicareDataChanged(MedicareLimit oldData, MedicareLimit newData, long? newCaseId)
        {
            if (newData.MEDICARE_LIMIT_TYPE_NAME == "ABN")
            {
                if (oldData.CASE_ID != newCaseId) { return true; }
                if (oldData.EFFECTIVE_DATE != newData.EFFECTIVE_DATE) { return true; }
                if (oldData.END_DATE != newData.END_DATE) { return true; }
                if ((oldData.ABN_EST_WK_COST ?? 0) != (newData.ABN_EST_WK_COST ?? 0)) { return true; }
                if (!String.Equals(oldData.ABN_COMMENTS ?? "", newData.ABN_COMMENTS ?? "", StringComparison.Ordinal)) { return true; }
                return false;
            }
            else if ((oldData.MEDICARE_LIMIT_TYPE_NAME == "Hospice") || newData.MEDICARE_LIMIT_TYPE_NAME.Equals("Home Health Episode"))
            {
                if (oldData.CASE_ID != newCaseId) { return true; }
                if (oldData.EFFECTIVE_DATE != newData.EFFECTIVE_DATE) { return true; }
                if (oldData.END_DATE != newData.END_DATE) { return true; }
                if (!String.Equals(oldData.NPI ?? "", newData.NPI ?? "", StringComparison.Ordinal)) { return true; }
                return false;
            }
            return false;
        }

        public string GetElgibilityDetails(long patientAccount, PatientInsuranceInformation patientInsuranceInformation, UserProfile profile, bool isMVP = false)
        {
            #region Old Implementation to check Eligiblity with WCF Service
            //Eligibility....
            //ExternalServices.PatientEligibilityService.MTBCData mtbcData = new ExternalServices.PatientEligibilityService.MTBCData();
            //mtbcData.ViewType = isMVP ? "MVP" : "";
            //mtbcData.ClientID = "FOXREHAB"; //  Client TalkEHR, WebSoft, EDI, WebEHR,foxrehab etc. For fox it will be FOXREHAB.
            //mtbcData.ClientType = "PATIENT";    //  ClientType can be CLAIM, PATIENT, APPOINTMENT or any other like FOX_CLIENT, from patient form it will be PATIENT.
            //mtbcData.ServerName = "10.10.30.76";    // Database Server IP
            //mtbcData.UserID = profile.userID.ToString();    //  User that is using WebSoft. User like IA32, MS147 etc.
            //mtbcData.InsuranceID = patientInsuranceInformation.INSURANCE_ID.ToString(); //  Id of Insurance table in MTBC system
            //mtbcData.insPayerID = patientInsuranceInformation.INSPAYER_ID.ToString();   //  Optional
            //mtbcData.PayerType = patientInsuranceInformation.INS_TYPE.ToString();   //  Payer type in MTBC's system. Primary, Secondary or OTHER etc.
            //mtbcData.PatientAccount = patientInsuranceInformation.PATIENT_ACCOUNT.ToString();  //  Required value
            //mtbcData.ClaimNo = string.Empty;    //  Optional
            //mtbcData.AppointmentID = string.Empty;  //  Optional

            //ExternalServices.PatientEligibilityService.MTBC270RequestData requestData = new ExternalServices.PatientEligibilityService.MTBC270RequestData();

            //// requestData.FIRST_NAME = result.FIRST_NAME;
            ////  requestData.LAST_NAME = result.LAST_NAME;
            //requestData.Address = patientInsuranceInformation.ADDRESS;
            //requestData.City = patientInsuranceInformation.CITY;

            ////DOS Range: start date-end date
            ////requestData.DateOfService = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(DateTime.Now))+"-"+ Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(DateTime.Now));
            //requestData.DateOfService = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(DateTime.Now));
            //requestData.PayerName = patientInsuranceInformation.PAYER_NAME;
            //requestData.ProviderFirstName = patientInsuranceInformation.PROVIDER_FNAME; // Table name providers -- code -- patient-- provider 
            //requestData.ProviderLastName = patientInsuranceInformation.PROVIDER_LNAME;  // Table name providers
            //requestData.ProviderNPI = "1326092503"; // Table name providers
            //requestData.ProviderSSN = patientInsuranceInformation.PROVIDER_SSN; // Table name providers
            //requestData.Relationship = patientInsuranceInformation.RELATIONSHIP;    // S
            //requestData.Zip = patientInsuranceInformation.ZIP;
            //requestData.State = patientInsuranceInformation.STATE;

            //if (patientInsuranceInformation.RELATIONSHIP == "S")
            //{
            //    if (patientInsuranceInformation.DATE_OF_BIRTH != null)
            //        requestData.SubscriberDateOfBirth = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.DATE_OF_BIRTH.ToString()));
            //    requestData.SubscriberFirstName = patientInsuranceInformation.FIRST_NAME;
            //    requestData.SubscriberGender = patientInsuranceInformation.GENDER;
            //    requestData.SubscriberGroupNumber = patientInsuranceInformation.GROUP_NUMBER;
            //    requestData.SubscriberLastName = patientInsuranceInformation.LAST_NAME;
            //    requestData.SubscriberSSN = patientInsuranceInformation.SSN;    //from patient table
            //    requestData.DependentDOB = "";
            //    requestData.DependentFirstName = "";
            //    requestData.DependentGender = "";
            //    requestData.DependentLastName = "";
            //}
            //else
            //{
            //    requestData.SubscriberDateOfBirth = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.GUARANTOR_DOB.ToString()));
            //    requestData.SubscriberFirstName = patientInsuranceInformation.GURANTOR_FNAME;
            //    requestData.SubscriberGender = patientInsuranceInformation.GUARANTOR_GENDER;
            //    requestData.SubscriberGroupNumber = patientInsuranceInformation.GROUP_NUMBER;
            //    requestData.SubscriberLastName = patientInsuranceInformation.GURANTOR_LNAME;
            //    requestData.SubscriberSSN = patientInsuranceInformation.GUARANTOR_SSN;
            //    requestData.DependentDOB = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.DATE_OF_BIRTH.ToString()));
            //    requestData.DependentFirstName = patientInsuranceInformation.FIRST_NAME;
            //    requestData.DependentGender = patientInsuranceInformation.GENDER;
            //    requestData.DependentLastName = patientInsuranceInformation.GROUP_NUMBER;
            //}
            //if (patientInsuranceInformation.PRAC_TYPE == "G")   //from practices by practice_code
            //{
            //    requestData.OrganizationType = "2";
            //    // requestData.ProviderNPI = // Group_NPI from table provider_payer
            //    requestData.OrganizationNPI = "1326092503";
            //}
            //else
            //{
            //    requestData.OrganizationType = "1";
            //    // requestData.ProviderNPI =  //indiv_NPI from table provider_payer
            //    requestData.OrganizationNPI = "1326092503";
            //}
            //HtmlDocument htmlDoc = new HtmlDocument();
            //requestData.ProviderSSN = patientInsuranceInformation.PROVIDER_SSN;
            //requestData.TaxID = patientInsuranceInformation.PRACTICE_TAX_ID;
            //requestData.PayerID = patientInsuranceInformation.INSPAYER_ELIGIBILITY_ID;
            //requestData.OrganizationName = patientInsuranceInformation.PRACTICE_NAME;
            //requestData.SubscriberMemberID = patientInsuranceInformation.POLICY_NUMBER;
            //ExternalServices.PatientEligibilityService.Service objService = new ExternalServices.PatientEligibilityService.Service();
            //string htmlStr = objService.MTBCResponse(requestData, mtbcData);
            #endregion Old Implementation to check Eligiblity with WCF Service
            //*********************************** New Implementation Starts ***************************************
            #region New Implementation to check Eligiblity with RestFull API
            ExternalServices.PatientEligibilityService.MTBCData mtbcData = new ExternalServices.PatientEligibilityService.MTBCData();
            String htmlStr;
            EligibilityModelNew objEligibilityNew = new EligibilityModelNew();
            //*********************************** MTBCData ***************************************
            objEligibilityNew.ViewType = mtbcData.ViewType = isMVP ? "MVP" : "FOX";
            objEligibilityNew.ClientID = "FOXREHAB";
            //objEligibilityNew.ClientID = "8781";
            objEligibilityNew.ClientType = "PATIENT";
            objEligibilityNew.ServerName = "10.10.30.76";
            objEligibilityNew.UserID = profile.userID.ToString();
            objEligibilityNew.InsuranceID = patientInsuranceInformation.INSURANCE_ID.ToString() ?? "";
            objEligibilityNew.insPayerID = patientInsuranceInformation.INSPAYER_ID.ToString() ?? "";
            //objEligibilityNew.PayerType = patientInsuranceInformation.INS_TYPE.ToString();
            objEligibilityNew.PayerType = "P";
            objEligibilityNew.PatientAccount = patientInsuranceInformation.PATIENT_ACCOUNT.ToString();
            objEligibilityNew.ClaimNo = string.Empty;
            objEligibilityNew.AppointmentID = string.Empty;
            //objEligibilityNew.InsPayerDescriptionName = eligibilityModel.Inspayer_Description;
            //*********************************** Payer Information ***************************************
            objEligibilityNew.PayerName = patientInsuranceInformation.PAYER_NAME;
            objEligibilityNew.PayerID = patientInsuranceInformation.INSPAYER_ELIGIBILITY_ID;
            //************************************ Practice Information ************************************
            objEligibilityNew.Address = patientInsuranceInformation.ADDRESS;
            objEligibilityNew.City = patientInsuranceInformation.CITY;
            objEligibilityNew.DateOfService = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(DateTime.Now));
            objEligibilityNew.ProviderFirstName = patientInsuranceInformation.PROVIDER_FNAME;
            objEligibilityNew.ProviderLastName = patientInsuranceInformation.PROVIDER_LNAME;
            objEligibilityNew.ProviderNPI = "1326092503";  // Table name providers
            objEligibilityNew.ProviderSSN = patientInsuranceInformation.PROVIDER_SSN;
            objEligibilityNew.Relationship = patientInsuranceInformation.RELATIONSHIP;
            objEligibilityNew.Zip = patientInsuranceInformation.ZIP;
            objEligibilityNew.State = patientInsuranceInformation.STATE;
            if (!string.IsNullOrEmpty(patientInsuranceInformation.RELATIONSHIP) && patientInsuranceInformation.RELATIONSHIP.Contains("S"))
            {
                objEligibilityNew.SubscriberDateOfBirth = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.DATE_OF_BIRTH.ToString()));
                objEligibilityNew.SubscriberFirstName = patientInsuranceInformation.FIRST_NAME;
                objEligibilityNew.SubscriberGender = patientInsuranceInformation.GENDER;
                objEligibilityNew.SubscriberGroupNumber = patientInsuranceInformation.GROUP_NUMBER;
                objEligibilityNew.SubscriberLastName = patientInsuranceInformation.LAST_NAME;
                objEligibilityNew.SubscriberSSN = patientInsuranceInformation.SSN;
                //*********************************** Dependent Level *****************************************
                objEligibilityNew.DependentDOB = string.Empty;
                objEligibilityNew.DependentFirstName = string.Empty;
                objEligibilityNew.DependentGender = string.Empty;
                objEligibilityNew.DependentLastName = string.Empty;
            }
            else
            {
                objEligibilityNew.SubscriberDateOfBirth = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.GUARANTOR_DOB.ToString()));
                objEligibilityNew.SubscriberFirstName = patientInsuranceInformation.GURANTOR_FNAME;
                objEligibilityNew.SubscriberGender = patientInsuranceInformation.GUARANTOR_GENDER;
                objEligibilityNew.SubscriberGroupNumber = patientInsuranceInformation.GROUP_NUMBER;
                objEligibilityNew.SubscriberLastName = patientInsuranceInformation.GURANTOR_LNAME;
                objEligibilityNew.SubscriberDateOfDeath = string.Empty;
                objEligibilityNew.SubscriberSSN = patientInsuranceInformation.GUARANTOR_SSN;
                //*********************************** Dependent Level *****************************************
                objEligibilityNew.DependentDOB = Helper.DateFormateForInsuranceEligibility(Convert.ToDateTime(patientInsuranceInformation.DATE_OF_BIRTH.ToString()));
                objEligibilityNew.DependentFirstName = patientInsuranceInformation.FIRST_NAME;
                objEligibilityNew.DependentGender = patientInsuranceInformation.GENDER;
                objEligibilityNew.DependentLastName = patientInsuranceInformation.GROUP_NUMBER;
            }
            if (patientInsuranceInformation.PRAC_TYPE == "G")
            {
                objEligibilityNew.OrganizationType = "2"; //Required value(if "I" then send 1 and if "G" then send 2 )
                objEligibilityNew.ProviderNPI = string.Empty; //if OrganizationType is "I" then you assign individual_npi
                objEligibilityNew.OrganizationNPI = "1326092503"; //if OrganizationType is "G" then you assign group_npi            
            }
            else
            {
                objEligibilityNew.OrganizationType = "1";     //  Required value(if "I" then send 1 and if "G" then send 2 )
                objEligibilityNew.OrganizationNPI = string.Empty; //if OrganizationType is "G" then you assign group_npi            
                objEligibilityNew.ProviderNPI = "1326092503"; //if OrganizationType is "I" then you assign individual_npi
            }
            objEligibilityNew.TaxID = patientInsuranceInformation.PRACTICE_TAX_ID;
            objEligibilityNew.OrganizationName = patientInsuranceInformation.PRACTICE_NAME;
            objEligibilityNew.SubscriberMemberID = patientInsuranceInformation.POLICY_NUMBER;
            //********************************** Service Level Type **************************************
            objEligibilityNew.ServiceType = "30";
            var result = "";
            string URl = WebConfigurationManager.AppSettings["EligibilityURL"].ToString();
            HtmlDocument doc = new HtmlDocument();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(URl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(objEligibilityNew);
                var stringContent = new StringContent(json, System.Text.UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = client.PostAsync("", stringContent).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync();
                    result = jsonString.Result;
                }
                htmlStr = result;
            }
            #endregion End New Implementation to check Eligiblity with RestFull API 
            htmlStr = htmlStr.Replace(@"id=""main-container""", @"id =""main-container-eligibility""");
            htmlStr = htmlStr.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\b", "").Replace("&nbsp;", " ");
            SaveEligibilityHtml(patientAccount, patientInsuranceInformation.PATIENT_INSURANCE_ID, htmlStr, profile);
            return htmlStr;
        }

        public ExtractEligibilityDataViewModel FetchEligibilityRecords(PatientEligibilitySearchModel patientEligibilitySearchModel, UserProfile profile)
        {
            long patientAccount = long.Parse(patientEligibilitySearchModel.Patient_Account_Str);

            var _patientAccount = new SqlParameter { ParameterName = "PATIENTACCOUNT", Value = patientAccount };
            var _result = SpRepository<PatientInsuranceInformation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES_INFO @PATIENTACCOUNT", _patientAccount);

            PatientInsuranceInformation patientInsuranceInformation = new PatientInsuranceInformation();
            if (patientEligibilitySearchModel.Patient_Insurance_id.HasValue)
            {
                patientInsuranceInformation = _result.Where(x => x.PATIENT_INSURANCE_ID == patientEligibilitySearchModel.Patient_Insurance_id.Value).FirstOrDefault();
            }
            else
            {
                patientInsuranceInformation = _result.Where(x => x.INS_TYPE == 1).FirstOrDefault();
            }

            string htmlStr = GetElgibilityDetails(patientAccount, patientInsuranceInformation, profile);

            /////////////////////////////////////////
            //string htmlStr = System.IO.File.ReadAllText(@"E:\Asad Ejaz\Profile Data\Desktop\42610117210.html");
            //string htmlStr = System.IO.File.ReadAllText(@"E:\Asad Ejaz\Profile Data\Desktop\42610118278.html");
            //htmlStr = htmlStr.Replace(@"id=""main-container""", @"id =""main-container-eligibility""");
            //htmlStr = htmlStr.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\b", "").Replace("&nbsp;", " ");
            /////////////////////////////////////////
            //if (htmlStr != null && htmlStr != "")
            //GetEligibilityInformation(htmlStr, patientAccount,profile);

            ExtractEligibilityDataViewModel eligRecords = new ExtractEligibilityDataViewModel();
            //eligRecords = ExtractEligibilityData(htmlStr, patientInsuranceInformation.PATIENT_INSURANCE_ID);
            eligRecords = ExtractEligibilityData(htmlStr, patientInsuranceInformation.PATIENT_INSURANCE_ID, profile, patientEligibilitySearchModel.Patient_Account_Str);
            return eligRecords;
        }

        public string gethtml()
        {
            string html = "";
            string path = @"E:\Asad Ejaz\Profile Data\Desktop\42610117210.html";

            // This text is added only once to the file.
            if (File.Exists(path))
            {
                html = File.ReadAllText(path);
            }
            return html;
        }

        public ReconcileDemographics GetLatestEligibilityRecords(PatientEligibilitySearchModel patientEligibilitySearchModel, UserProfile profile)
        {
            ReconcileDemographics reconcileDemographics;
            long patientAccount = long.Parse(patientEligibilitySearchModel.Patient_Account_Str);
            //long insType = eligibilitySearchReq.INS_TYPE;
            var _patientAccount = new SqlParameter { ParameterName = "PATIENTACCOUNT", Value = patientAccount };
            var _result = SpRepository<PatientInsuranceInformation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES_INFO @PATIENTACCOUNT", _patientAccount);
            if (_result.Count() > 0 && _result != null)
            {
                //var patientInsuranceInformation = _result.Where(x => x.INS_TYPE == insType).FirstOrDefault();
                var patientInsuranceInformation = _result.Where(x => x.INS_TYPE == 1).FirstOrDefault();
                if (patientInsuranceInformation != null)
                {
                    return GetEligibilityInformation(GetElgibilityDetails(patientAccount, patientInsuranceInformation, profile), patientAccount, profile);
                }
            }
            return reconcileDemographics = new ReconcileDemographics();
        }

        public ReconcileDemographics GetEligibilityInformation(string ElgString, long patientAccount, UserProfile profile)
        {
            if (string.IsNullOrWhiteSpace(ElgString))
                return new ReconcileDemographics();
            ReconcileDemographics obj2 = new ReconcileDemographics();
            HtmlDocument HtmlDoc = new HtmlDocument();
            PayorDataModel obj = new PayorDataModel();

            HtmlDoc.LoadHtml(ElgString);
            obj.PayorAdress = HtmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'EDIPatInsAddress')]")?.InnerHtml ?? "";
            obj.PayorCity = HtmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'EDIPatInsCity')]")?.InnerHtml ?? "";
            obj.PayorDOB = HtmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'EDIPatInsDOB')]")?.InnerHtml ?? "";
            obj.PayorGender = HtmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'EDIPatInsGender')]")?.InnerHtml ?? "";
            obj.PayorState = HtmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'EDIPatInsState')]")?.InnerHtml ?? "";
            obj.PayorZip = HtmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'EDIPatInsZIP')]")?.InnerHtml ?? "";
            string FullName = HtmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'EDIPatInsName')]")?.InnerHtml ?? "";
            obj.Patient_Account = patientAccount;
            obj2.Error = "";
            if (FullName.Contains(','))
            {
                obj.PayorFirstName = FullName.Split(',')[1];
                obj.PayorLastName = FullName.Split(',')[0];
            }
            else
            {
                obj.PayorFirstName = FullName;
            }

            if (!string.IsNullOrWhiteSpace(obj.PayorAdress)
                || !string.IsNullOrWhiteSpace(obj.PayorCity)
                || !string.IsNullOrWhiteSpace(obj.PayorDOB)
                || !string.IsNullOrWhiteSpace(obj.PayorGender)
                || !string.IsNullOrWhiteSpace(obj.PayorState)
                || !string.IsNullOrWhiteSpace(obj.PayorZip)
                || !string.IsNullOrWhiteSpace(obj.PayorFirstName)
                || !string.IsNullOrWhiteSpace(obj.PayorLastName)
                )
            {
                SaveReconcileLatestData(obj, profile);
                obj2 = GetRecncileDemo(patientAccount, profile);
                return obj2;
            }
            else
                return new ReconcileDemographics();
        }

        public void SaveReconcileLatestData(PayorDataModel obj, UserProfile profile)
        {
            ReconcileDemographics reconcileObj = new ReconcileDemographics();
            reconcileObj.FOX_REC_DEM_ID = Helper.getMaximumId("FOX_REC_DEM_ID");
            reconcileObj.ADDRESS = obj.PayorAdress;
            reconcileObj.CITY = obj.PayorCity;
            reconcileObj.ZIP = obj.PayorZip;
            if (reconcileObj.ZIP != null && reconcileObj.ZIP.Contains("-"))
            {
                reconcileObj.ZIP = reconcileObj.ZIP.Replace("-", "");
            }
            if (obj.PayorDOB == string.Empty)
            {
                reconcileObj.DOB = null;
            }
            else
            {
                reconcileObj.DOB = Convert.ToDateTime(obj.PayorDOB);
            }
            reconcileObj.FIRST_NAME = obj.PayorFirstName;
            reconcileObj.GENDER = obj.PayorGender;
            reconcileObj.LAST_NAME = obj.PayorLastName;
            reconcileObj.PATIENT_ACCOUNT = obj.Patient_Account;
            reconcileObj.PRACTICE_CODE = profile.PracticeCode;
            reconcileObj.STATE = obj.PayorState;
            reconcileObj.CREATED_BY = profile.UserName;
            reconcileObj.CREATED_DATE = Helper.GetCurrentDate();
            reconcileObj.MODIFIED_BY = profile.UserName;
            reconcileObj.MODIFIED_DATE = Helper.GetCurrentDate();
            reconcileObj.DELETED = false;
            _ReconcileDemoRepository.Insert(reconcileObj);
            _ReconcileDemoRepository.Save();
        }

        private ReconcileDemographics GetRecncileDemo(long Patient_Account, UserProfile profile)
        {
            ReconcileDemographics model = new ReconcileDemographics();
            model = _ReconcileDemoRepository.GetMany(t => t.PATIENT_ACCOUNT == Patient_Account && !t.DELETED)?.OrderByDescending(x => x.CREATED_DATE)?.FirstOrDefault();
            if (model != null)
                return model;
            else
                return model = new ReconcileDemographics();
        }

        public void SaveEligibilityHtml(long patient_account, long patientInsuranceId, string html, UserProfile profile)
        {
            FOX_TBL_ELIG_HTML eligHTMLObj = new FOX_TBL_ELIG_HTML();
            eligHTMLObj.ELIG_HTML_ID = Helper.getMaximumId("FOX_ELIG_HTML");
            eligHTMLObj.CREATED_BY = eligHTMLObj.MODIFIED_BY = profile.UserName;
            eligHTMLObj.CREATED_DATE = eligHTMLObj.MODIFIED_DATE = Helper.GetCurrentDate();
            eligHTMLObj.PRACTICE_CODE = profile.PracticeCode;
            eligHTMLObj.PATIENT_ACCOUNT = patient_account;
            eligHTMLObj.PATIENT_INSURANCE_ID = patientInsuranceId;
            eligHTMLObj.ELIG_HTML = html;

            _eligHtmlRepository.Insert(eligHTMLObj);
            _eligHtmlRepository.Save();
        }

        private ExtractEligibilityDataViewModel ExtractEligibilityData(string ediHTMLString, long patientInsuranceId, UserProfile Profile, string Patient_Account_Str)
        {
            /*benefits                 
                1. HBPC_Plan_MB
                2. PT
                3. ST
                4. OT

            classes
                1. Fox_Out_of_Pocket_....benefits
                2. Fox_Deductible_....benefits
                3. Fox_Copay_....benefits
                4. Fox_Cov_Plan_....benefits
                5. Fox_Network_.

                6. Fox_CO_Insurance_
                7. Fox_Benefit_Description_*/

            HtmlDocument htmlDoc = new HtmlDocument();
            //htmlDoc.Load(path);
            htmlDoc.LoadHtml(ediHTMLString);
            htmlDoc.DocumentNode.OuterHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\b", "").Replace("&nbsp;", " ");
            List<string> benefitNameList = new List<string>() { "HBPC_Plan_MB", "HBPC", "PT", "ST", "OT", "HBPC_Plan" };
            List<string> applicableBenefitNameList = new List<string>();
            var patInsurance = new PatientInsurance();
            if (patientInsuranceId != 0)
            {
                patInsurance= _PatientInsuranceRepository.GetByID(patientInsuranceId);
            }
            string caseNo = "";
            if (patInsurance.CASE_ID != null)
            {
                caseNo = _vwPatientCaseRepository.GetByID(patInsurance.CASE_ID)?.CASE_NO ?? "";
            }

            bool isMedicare = false;
            var fcObj = _financialClassRepository.GetByID(patInsurance.FINANCIAL_CLASS_ID);
            if (fcObj != null && !string.IsNullOrWhiteSpace(fcObj.CODE) && fcObj.CODE.Equals("MC"))
            {
                isMedicare = true;
            }

            bool lookForINNetwork = false;
            var fox_Ins = _foxInsurancePayersRepository.GetFirst(e => e.FOX_TBL_INSURANCE_ID == patInsurance.FOX_TBL_INSURANCE_ID && (e.DELETED ?? false) == false);
            if (fox_Ins != null)
            {
                //00585 => BCBS PA Highmark/Capital
                //00571 => BCBS DE (Local and Bluecard)

                if (fox_Ins.INSURANCE_PAYERS_ID.Equals("00585") || fox_Ins.INSURANCE_PAYERS_ID.Equals("00571"))
                {
                    lookForINNetwork = true;
                }
            }

            if (isMedicare)
            {
                //applicableBenefitNameList = benefitNameList.Where(e => e.Contains("HBPC_Plan_MB")).ToList();
                //As per M. Ali, use all available benefits
                applicableBenefitNameList = benefitNameList.Where(e => !e.Equals("HBPC") && !e.Equals("HBPC_Plan")).ToList();

                //if (!string.IsNullOrEmpty(caseNo))
                //{
                //    applicableBenefitNameList.AddRange(benefitNameList.Where(e => caseNo.Contains(e)).ToList());
                //}
            }
            else
            {
                //if (!string.IsNullOrEmpty(caseNo))
                //{
                //    applicableBenefitNameList = benefitNameList.Where(e => caseNo.Contains(e)).ToList();
                //}
                //else
                //{
                //As per M. Ali, use all available benefits
                //Remove HBPC_Plan_MB because its for Medicare
                applicableBenefitNameList = benefitNameList.Where(e => !e.Contains("HBPC_Plan_MB")).ToList();
                //}
            }

            //if (isMedicare)
            //{
            //    benefitNameList = benefitNameList.Where(e => e.Contains("HBPC_Plan_MB")).ToList();
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(caseNo))
            //    {
            //        benefitNameList = benefitNameList.Where(e => caseNo.Contains(e)).ToList();
            //    }
            //    else
            //    {
            //        //Remove HBPC_Plan_MB
            //        benefitNameList = benefitNameList.Where(e => !e.Contains("HBPC_Plan_MB")).ToList();
            //    }
            //}


            //string Co_Payment = "";
            //bool IS_COPAY_PER;
            //bool? IS_COPAY_PER_VISIT;
            //string DED_POLICY_LIMIT_RESET_ON = "";
            //string YEARLY_DED_AMT = "";
            //string DED_MET = "";
            //string DED_MET_AS_OF = "";
            //string DED_REMAINING = "";
            //string PT_ST_TOT_AMT_USED = "";      //PHYSICAL THERAPY > BENEFIT DESCRIPTION > $2,258.80: : Used Amount  
            //string OT_TOT_AMT_USED = "";         //OCCUPATIONAL THERAPY > BENEFIT DESCRIPTION > $2,258.80: : Used Amount  

            ////CHECK OUT OF NETWORK FOR ALL NON-MEDICARE INSURANCES
            ////CHECK INDIVIDUAL COVERAGE PLAN

            //string BENEFIT_POLICY_LIMIT_RESET_ON = "";  //01/01
            //string MYB_LIMIT_DOLLARS = "";              //Out of pocket (stop loss)         $13,700.00 Per Calendar Year
            //string MYB_LIMIT_VISIT = "";                // IDK how the visits would be given?
            //string MYB_USED_OUTSIDE_DOLLARS = "";       // USED  = Total - Remaining.       $12,971.75 Remaining  
            //string MYB_USED_OUTSIDE_VISIT = "";         //
            //string MOP_AMT = "";                        // See in LIMITATIONS               $750.00 Per Calenda
            //string MOP_USED_OUTSIDE_RT = "";            //USED  = Total - Remaining         $750.00 Remaining
            //string Network = "";                        //OUT
            //string CoveragePlan = "";                   //Individual

            ExtractEligibilityDataViewModel model = new ExtractEligibilityDataViewModel();
            model.Allow_Save = DocumentHasRestrictedNodes(htmlDoc);
            if (model.Allow_Save && isMedicare)
            {
                model.Allow_Save = HasOnlyMedicarePartA(htmlDoc);
                if (!model.Allow_Save)
                {
                    model.Not_Allow_Save_Reason = "npb";//Reason is Not Part B
                }
            }
            else
            {
                model.Not_Allow_Save_Reason = "map";//Reason is MAP plans
            }

            if (isMedicare)
            {
                model = ExtractDataForMedicare(applicableBenefitNameList, htmlDoc, model, Profile, Patient_Account_Str);
            }
            else
            {
                model = ExtractDataForCommercial(applicableBenefitNameList, htmlDoc, model, lookForINNetwork);
            }

            return model;
        }

        private bool HasOnlyMedicarePartA(HtmlDocument htmlDoc)
        {
            bool allowSave = true;
            string partA = "HBPC_Plan_MA";
            var Deductible_Nodes_PartA = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Deductible_" + partA + "')]");
            var CO_Insurance_Nodes_PartA = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_CO_Insurance_" + partA + "')]");
            var Copay_Nodes_PartA = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Copay_" + partA + "')]");
            var Benefit_Description_Nodes_PartA = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Benefit_Description_" + partA + "')]");
            var Active_Cov_Nodes_PartA = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Active_Coverage_" + partA + "')]");
            if (Deductible_Nodes_PartA != null || CO_Insurance_Nodes_PartA != null || Copay_Nodes_PartA != null || Benefit_Description_Nodes_PartA != null || Active_Cov_Nodes_PartA != null)
            {
                var bn = "HBPC_Plan_MB";
                var Deductible_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Deductible_" + bn + "')]");
                var CO_Insurance_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_CO_Insurance_" + bn + "')]");
                var Copay_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Copay_" + bn + "')]");
                var Benefit_Description_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Benefit_Description_" + bn + "')]");
                var Active_Cov_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Active_Coverage_" + bn + "')]");

                if (Deductible_Nodes == null && CO_Insurance_Nodes == null && Copay_Nodes == null && Benefit_Description_Nodes == null && Active_Cov_Nodes == null)
                {
                    allowSave = false;
                }
            }
            return allowSave;
        }

        private bool DocumentHasRestrictedNodes(HtmlDocument htmlDoc)
        {
            bool allowSave = true;
            bool isValidDate = true;

            string[] plans = { "HN", "IN", "PR", "PS", "HM" };
            string[] codes = { "30", "CQ" };

            foreach (var plan in plans)
            {
                foreach (var code in codes)
                {
                    var node = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'Fox_\"" + code + "_" + plan + "\"')]/tbody/tr/td[3]");
                    if (node != null)
                    {
                        isValidDate = CheckMCEligiblityEffectiveDate(node); //MC - Medicare
                        if (!isValidDate)
                        {
                            allowSave = false;
                            break;
                        }
                    }
                }
                if (!allowSave)
                {
                    break;
                }
            }
            return allowSave;
        }

        private bool CheckMCEligiblityEffectiveDate(HtmlNode htmlDoc)
        {

            var date_range_node = htmlDoc;
            bool isValidDate = true;
            if (date_range_node != null)
            {
                List<DateTime> dateTimeList = new List<DateTime>();
                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                MatchCollection mCollection = rx.Matches(date_range_node.InnerText);
                foreach (var item in mCollection)
                {
                    Match m = item as Match;
                    if (m.Success)
                    {
                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                            m.Groups["month"].Value,
                            m.Groups["day"].Value,
                            m.Groups["year"].Value), "MM/dd/yyyy", null);

                        dateTimeList.Add(date);
                    }
                }
                if (dateTimeList.Count >= 2)
                {
                    IEnumerable<DateTime> IdateTimeList = dateTimeList.Take(2);
                    if (IdateTimeList.Count() > 0 && Helper.GetCurrentDate() >= IdateTimeList.ElementAt(0) && Helper.GetCurrentDate() <= IdateTimeList.ElementAt(1))
                    {
                        isValidDate = false;
                    }

                }
                else if (dateTimeList.Count == 1)
                {
                    IEnumerable<DateTime> IdateTimeList = dateTimeList.Take(1);
                    if (IdateTimeList.Count() > 0 && Helper.GetCurrentDate() >= IdateTimeList.ElementAt(0))
                    {
                        isValidDate = false;
                    }
                }

            }
            return isValidDate;
        }

        private bool checkIsRailroadMedicare(HtmlDocument htmlDoc)
        {
            var railroad = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@class,'custom-panel-head')][h3//text()[contains(.,'30-HEALTH BENEFIT PLAN COVERAGE (MA-MEDICARE PART A)')]]/following-sibling::table");
            if (railroad != null && railroad.LastChild != null && railroad.LastChild.ChildNodes.Count >= 1)
            {
                var childdata = railroad.LastChild.ChildNodes;
                if (childdata != null && childdata.Count > 0)
                {
                    foreach (var node in childdata)
                    {
                        if (node != null && !string.IsNullOrWhiteSpace(node.InnerText))
                        {
                            if (node.LastChild != null && node.InnerText.ToLower().Contains("railroad"))
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }

        private bool checkIsPlanExpired(HtmlDocument htmlDoc)
        {
            string termination_date = "";
            //string text_second = "";
            var eligibility = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@class,'custom-panel-head')][h3//text()[contains(.,'30-HEALTH BENEFIT PLAN COVERAGE (PR-PREFERRED PROVIDER OGRANIZATION (PPO))')]]/following-sibling::table");
            if (eligibility != null && eligibility.LastChild != null && eligibility.LastChild.ChildNodes.Count >= 1)
            {
                var childdata = eligibility.LastChild.ChildNodes;
                if (childdata != null && childdata.Count > 0)
                {
                    foreach (var node in childdata)
                    {
                        if (node != null && !string.IsNullOrWhiteSpace(node.InnerText))
                        {
                            string text = @"";
                            if (node.LastChild != null && node.InnerText.Contains("Coordination of Benefit"))
                            {
                                text = node.InnerText;
                                text = text.Substring(text.IndexOf("Coordination of Benefit"));
                                if (text.IndexOf('-') > text.IndexOf("Coordination of Benefit"))
                                {
                                    int n = text.IndexOf('-');
                                    if (text.Length > n + 11)
                                    {
                                        termination_date = text.Substring(n + 1, 10);
                                    }
                                }
                                //termination_date = "sfjiasgfi323#2";
                                DateTime termination_date_Correct;
                                if (!String.IsNullOrWhiteSpace(termination_date) && DateTime.TryParse(termination_date, out termination_date_Correct))
                                {
                                    var date = Convert.ToDateTime(termination_date_Correct);
                                    if (date != null && date < Helper.GetCurrentDate())
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return false;
        }

        private bool checkIsPPO(HtmlDocument htmlDoc)
        {
            var eligibility = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@class,'custom-panel-head')][h3//text()[contains(.,'30-HEALTH BENEFIT PLAN COVERAGE (PR-PREFERRED PROVIDER OGRANIZATION (PPO))')]]/following-sibling::table");
            if (eligibility != null)
            {
                return true;

            }
            return false;
        }

        private ExtractEligibilityDataViewModel GetMSPData(HtmlDocument htmlDoc, ExtractEligibilityDataViewModel model)
        {
            //var mspData = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@class,'custom-panel-head')][h3//text()[contains(.,'-MSP-')]]/following-sibling::table/tbody/tr/td[count(//table/thead/tr/th[.='OTHER OR ADDITIONAL PAYOR'])+0]");

            //var mspData = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@class,'custom-panel-head')][h3//text()[contains(.,'-MSP-')]]/following-sibling::table/tbody/tr/td[count(//table/thead/tr/th[.='OTHER OR ADDITIONAL PAYOR']/preceding-sibling::*)+1]");

            HtmlNodeCollection FinalMspData;
            var mspData = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@class,'custom-panel-head')][h3//text()[contains(.,'14-MSP-')]]/following-sibling::table");
            if (mspData == null)
            {
                mspData = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@class,'custom-panel-head')][h3//text()[contains(.,'15-MSP-')]]/following-sibling::table");
            }
            if (mspData == null)
            {
                mspData = htmlDoc.DocumentNode.SelectSingleNode("*//div[contains(@class,'custom-panel-head')][h3//text()[contains(.,'47-MSP-')]]/following-sibling::table");
            }
            if (mspData != null && mspData.LastChild.ChildNodes.Count >= 1)
            {
                FinalMspData = mspData.LastChild.ChildNodes;
                foreach (var msp in FinalMspData)
                {
                    if (msp != null && !string.IsNullOrWhiteSpace(msp.InnerText))
                    {

                        if (msp.LastChild != null && msp.LastChild.InnerHtml.Contains("<br>"))
                        {
                            var mspcomments = msp.LastChild.InnerHtml.Replace("<br>", "-").Replace("--", "-");
                            if (!string.IsNullOrWhiteSpace(mspcomments))
                            {
                                model.Eligibility_MSP_Data = model.Eligibility_MSP_Data + mspcomments;
                            }
                            //model.Eligibility_MSP_Data =  !string.IsNullOrWhiteSpace(mspcomments) ? mspcomments : "";
                        }
                        if (msp.NextSibling != null)
                        {
                            model.Eligibility_MSP_Data = model.Eligibility_MSP_Data + '-';
                        }

                    }
                }
            }


            return model;
        }

        public ExtractEligibilityDataViewModel ExtractDataForCommercial(List<string> benefitNameList, HtmlDocument htmlDoc, ExtractEligibilityDataViewModel model, bool look_For_IN_Network)
        {
            model = GetHosAndHHEDataFromEligibility(htmlDoc, model);
            model = GetPatientDeceasedDate(htmlDoc, model);
            model = GetEligibilityStatus(htmlDoc, model);

            bool oopNodeRead = false;
            bool dedNodeRead = false;
            Copay _copay = new Copay();
            var currentYear = DateTime.Now.Year.ToString();
            foreach (var bn in benefitNameList)
            {
                HtmlNodeCollection Out_of_PocketNodes = null;
                HtmlNodeCollection Deductible_Nodes = null;
                HtmlNodeCollection Copay_Nodes = null;
                HtmlNodeCollection CO_Insurance_Nodes = null;
                HtmlNodeCollection Limitations_Nodes = null;
                HtmlNodeCollection Benefit_Description_Nodes = null;
                HtmlNodeCollection Active_Cov_Nodes = null;

                //HtmlNodeCollection Cov_Plan_Nodes = null;

                //xPath ex: person[contains(firstname, 'Kerr') and contains(lastname, 'och')]
                //var Out_of_PocketNodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Out_of_Pocket_" + bn + "')]");
                //var Deductible_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Deductible_" + bn + "')]");
                //var Copay_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Copay_" + bn + "')]");
                //var CO_Insurance_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_CO_Insurance_" + bn + "')]");
                //var Cov_Plan_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Cov_Plan_" + bn + "')]");
                //var Network_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Network_" + bn + "')]");
                //var Limitations_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Limitations_" + bn + "')]");
                //var Benefit_Description_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Benefit_Description_" + bn + "')]");

                if (bn.Equals("HBPC"))
                {
                    if (look_For_IN_Network)
                        Out_of_PocketNodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Out_of_Pocket_HBPC_Plan')) and contains(@class,'Fox_Out_of_Pocket_" + bn + "_In_Individual" + "')]");
                    else
                        Out_of_PocketNodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Out_of_Pocket_HBPC_Plan')) and contains(@class,'Fox_Out_of_Pocket_" + bn + "_Out_Individual" + "')]");
                }
                else
                {
                    if (look_For_IN_Network)
                        Out_of_PocketNodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Out_of_Pocket_" + bn + "') and contains(@class,'_In_Individual')]");
                    else
                        Out_of_PocketNodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Out_of_Pocket_" + bn + "') and contains(@class,'_Out_Individual')]");
                }

                if (bn.Equals("HBPC"))
                {
                    if (look_For_IN_Network)
                        Deductible_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Deductible_HBPC_Plan')) and contains(@class,'Fox_Deductible_" + bn + "_In_Individual" + "')]");
                    else
                        Deductible_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Deductible_HBPC_Plan')) and contains(@class,'Fox_Deductible_" + bn + "_Out_Individual" + "')]");
                }
                else
                {
                    if (look_For_IN_Network)
                        Deductible_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Deductible_" + bn + "') and contains(@class, '_In_Individual')]");
                    else
                        Deductible_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Deductible_" + bn + "') and contains(@class, '_Out_Individual')]");
                }

                if (bn.Equals("HBPC"))
                {
                    if (look_For_IN_Network)
                        Copay_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Copay_HBPC_Plan')) and contains(@class,'Fox_Copay_" + bn + "_In_Individual" + "')]");
                    else
                        Copay_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Copay_HBPC_Plan')) and contains(@class,'Fox_Copay_" + bn + "_Out_Individual" + "')]");
                }
                else
                {
                    if (look_For_IN_Network)
                        Copay_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Copay_" + bn + "') and contains(@class, '_In_Individual')]");
                    else
                        Copay_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Copay_" + bn + "') and contains(@class, '_Out_Individual')]");
                }

                if (bn.Equals("HBPC"))
                {
                    if (look_For_IN_Network)
                        CO_Insurance_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_CO_Insurance_HBPC_Plan')) and contains(@class,'Fox_CO_Insurance_" + bn + "_In_Individual" + "')]");
                    else
                        CO_Insurance_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_CO_Insurance_HBPC_Plan')) and contains(@class,'Fox_CO_Insurance_" + bn + "_Out_Individual" + "')]");
                }
                else
                {
                    if (look_For_IN_Network)
                        CO_Insurance_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_CO_Insurance_" + bn + "') and contains(@class, '_In_Individual')]");
                    else
                        CO_Insurance_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_CO_Insurance_" + bn + "') and contains(@class, '_Out_Individual')]");
                }

                //if (look_For_IN_Network) {
                //    Cov_Plan_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Cov_Plan_" + bn + "') and contains(@class,'Fox_Cov_Plan_" + bn + "')]");
                //}
                //else {
                //    Cov_Plan_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Cov_Plan_" + bn + "')]");
                //}

                if (look_For_IN_Network)
                    Benefit_Description_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Benefit_Description_" + bn + "') and contains(@class, '_In_Individual')]");
                else
                    Benefit_Description_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Benefit_Description_" + bn + "') and contains(@class, '_Out_Individual')]");

                if (bn.Equals("HBPC"))
                {
                    if (look_For_IN_Network)
                        Limitations_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Limitations_HBPC_Plan')) and contains(@class,'Fox_Limitations_" + bn + "_In_Individual" + "')]");
                    else
                        Limitations_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Limitations_HBPC_Plan')) and contains(@class,'Fox_Limitations_" + bn + "_Out_Individual" + "')]");
                }
                else
                {
                    if (look_For_IN_Network)
                        Limitations_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Limitations_" + bn + "') and contains(@class, '_In_Individual')]");
                    else
                        Limitations_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Limitations_" + bn + "') and contains(@class, '_Out_Individual')]");
                }

                if (bn.Equals("HBPC"))
                {
                    if (look_For_IN_Network)
                        Active_Cov_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Active_Coverage_HBPC_Plan')) and contains(@class,'Fox_Active_Coverage_" + bn + "_In_Individual" + "')]");
                    else
                        Active_Cov_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[not(contains(@class,'Fox_Active_Coverage_HBPC_Plan')) and contains(@class,'Fox_Active_Coverage_" + bn + "_Out_Individual" + "')]");
                }
                else
                {
                    if (look_For_IN_Network)
                        Active_Cov_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Active_Coverage_" + bn + "') and contains(@class, '_In_Individual')]");
                    else
                        Active_Cov_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Active_Coverage_" + bn + "') and contains(@class, '_Out_Individual')]");
                }


                if (Out_of_PocketNodes != null || Deductible_Nodes != null || CO_Insurance_Nodes != null || Copay_Nodes != null
                    //|| Cov_Plan_Nodes != null
                    //|| Network_Nodes != null 
                    || Benefit_Description_Nodes != null || Limitations_Nodes != null)
                {

                    //switch (bn)
                    //{
                    //    case "HBPC":
                    //        _copay.Type = "HBPC";
                    //        break;
                    //    case "HBPC_Plan":
                    //        _copay.Type = "HBPC_Plan";
                    //        break;
                    //    case "PT":
                    //        _copay.Type = "PT";
                    //        break;
                    //    case "ST":
                    //        _copay.Type = "ST";
                    //        break;
                    //    case "OT":
                    //        _copay.Type = "OT";
                    //        break;
                    //    default:
                    //        break;
                    //}
                    #region Out_of_Pocket

                    if (Out_of_PocketNodes != null)
                    {
                        foreach (var htmlNode in Out_of_PocketNodes)
                        {
                            if (htmlNode.InnerText.Contains(currentYear))
                            {
                                if (htmlNode.InnerText.Contains("$") && !model.MOP_AMT.HasValue && !oopNodeRead)
                                {
                                    model.MOP_AMT = htmlNode.InnerText.ExtractAmountFromString();
                                }
                                if (htmlNode.InnerText.ToLower().Contains("remaining") && htmlNode.InnerText.Contains("$") && !model.MOP_AMT_Remaining.HasValue && !oopNodeRead)
                                {
                                    model.MOP_AMT_Remaining = htmlNode.InnerText.ExtractAmountFromString();
                                }
                                else if (htmlNode.InnerText.ToLower().Contains("year to date") && htmlNode.InnerText.Contains("$") && !model.MOP_AMT_Remaining.HasValue && !oopNodeRead)
                                {
                                    if (model.MOP_AMT.HasValue)
                                    {
                                        var used_amt = htmlNode.InnerText.ExtractAmountFromString();
                                        model.MOP_AMT_Remaining = model.MOP_AMT - used_amt;
                                    }
                                }

                                //if (htmlNode.InnerText.ToLower().Contains("year to date") && htmlNode.InnerText.Contains("$") && !model.mo.HasValue)
                                //{
                                //    model.MOP_AMT_Remaining = htmlNode.InnerText.ExtractAmountFromString();
                                //}
                            }
                            else
                            {
                                if ((htmlNode.InnerText.ToLower().Contains("per calendar year") || htmlNode.InnerText.ToLower().Contains("per service year")) && htmlNode.InnerText.Contains("$") && !model.MOP_AMT.HasValue && !oopNodeRead)
                                {
                                    model.MOP_AMT = htmlNode.InnerText.ExtractAmountFromString();
                                }
                                if (htmlNode.InnerText.ToLower().Contains("remaining") && htmlNode.InnerText.Contains("$") && !model.MOP_AMT_Remaining.HasValue && !oopNodeRead)
                                {
                                    model.MOP_AMT_Remaining = htmlNode.InnerText.ExtractAmountFromString();
                                }
                                else if (htmlNode.InnerText.ToLower().Contains("year to date") && htmlNode.InnerText.Contains("$") && !model.MOP_AMT_Remaining.HasValue && !oopNodeRead)
                                {
                                    if (model.MOP_AMT.HasValue)
                                    {
                                        var used_amt = htmlNode.InnerText.ExtractAmountFromString();
                                        model.MOP_AMT_Remaining = model.MOP_AMT - used_amt;
                                    }
                                }
                            }
                        }
                        if (model.MOP_AMT.HasValue && !oopNodeRead)
                        {
                            oopNodeRead = true;
                        }
                    }
                    #endregion

                    #region Deductible
                    if (Deductible_Nodes != null)
                    {
                        foreach (var htmlNode in Deductible_Nodes)
                        {
                            if ((htmlNode.InnerText.ToLower().Contains("per calendar year") || htmlNode.InnerText.ToLower().Contains("per service year")) && htmlNode.InnerText.Contains("$") && !model.YEARLY_DED_AMT.HasValue && !dedNodeRead)
                            {
                                model.YEARLY_DED_AMT = htmlNode.InnerText.ExtractAmountFromString();

                                List<DateTime> dateTimeList = new List<DateTime>();
                                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                                MatchCollection mCollection = rx.Matches(htmlNode.InnerText);
                                foreach (var item in mCollection)
                                {
                                    Match m = item as Match;
                                    if (m.Success)
                                    {
                                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                              m.Groups["month"].Value,
                                              m.Groups["day"].Value,
                                              m.Groups["year"].Value), "MM/dd/yyyy", null);

                                        dateTimeList.Add(date);
                                    }
                                }

                                DateTime? dateTimeFrom = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                                DateTime? dateTimeTo = dateTimeList.OrderByDescending(t => t.Date).FirstOrDefault();

                                model.DED_POLICY_LIMIT_RESET_ON = dateTimeFrom.HasValue ? dateTimeFrom.Value.ToString("MM/dd") : "01/01";
                            }

                            if (htmlNode.InnerText.ToLower().Contains("remaining") && htmlNode.InnerText.Contains("$") && !model.DED_REMAINING.HasValue && !dedNodeRead)
                            {
                                model.DED_REMAINING = htmlNode.InnerText.ExtractAmountFromString();
                            }
                            else if (htmlNode.InnerText.ToLower().Contains("year to date") && htmlNode.InnerText.Contains("$") && !model.DED_REMAINING.HasValue && !dedNodeRead)
                            {
                                if (model.YEARLY_DED_AMT.HasValue)
                                {
                                    var used_amt = htmlNode.InnerText.ExtractAmountFromString();
                                    model.DED_REMAINING = model.YEARLY_DED_AMT - used_amt;
                                }
                            }
                        }
                        if (model.YEARLY_DED_AMT.HasValue)
                        {
                            dedNodeRead = true;
                        }
                    }
                    #endregion

                    #region Copay
                    if (Copay_Nodes != null)
                    {
                        foreach (var htmlNode in Copay_Nodes)
                        {
                            if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                            {
                                if (htmlNode.InnerText.ToLower().Contains("per visit"))
                                {
                                    _copay.IS_COPAY_PER_VISIT = true;
                                }
                                else if (htmlNode.InnerText.ToLower().Contains("per day") || htmlNode.InnerText.ToLower().Contains("episode"))
                                {
                                    _copay.IS_COPAY_PER_VISIT = false;
                                }
                                else
                                {
                                    _copay.IS_COPAY_PER_VISIT = null;
                                }
                                if (htmlNode.InnerText.Contains("$"))
                                {
                                    _copay.Co_Payment = htmlNode.InnerText.ExtractAmountFromString();
                                    _copay.IS_COPAY_PER = false;
                                }
                                if (htmlNode.InnerText.Contains("%"))
                                {
                                    string _str = htmlNode.InnerText;
                                    _str = _str.Replace(":", "");

                                    var tempAry = _str.Split(' ');
                                    //_copay.Co_Payment = tempAry.FirstOrDefault(t => t.Contains("%"))?.ExtractAmountFromString();

                                    decimal _dec;
                                    decimal.TryParse(tempAry.FirstOrDefault(t => t.Contains("%"))?.Replace("%", ""), out _dec);

                                    //_copay.Co_Payment = _dec * 100;
                                    _copay.Co_Payment = _dec;
                                    _copay.IS_COPAY_PER = true;
                                }
                            }
                        }
                    }
                    #endregion
                }
                #region CO_Insurance
                if (CO_Insurance_Nodes != null && (!_copay.Co_Payment.HasValue || _copay.Co_Payment.Value == 0))
                {
                    bool? isVisit = null;
                    bool? isPer = null;
                    decimal? co_ins = null;
                    foreach (var htmlNode in CO_Insurance_Nodes)
                    {
                        if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                        {
                            if (htmlNode.InnerText.ToLower().Contains("per visit"))
                            {
                                //_copay.IS_COPAY_PER_VISIT = true;
                                isVisit = true;
                            }
                            else if (htmlNode.InnerText.ToLower().Contains("per day") || htmlNode.InnerText.ToLower().Contains("episode"))
                            {
                                //_copay.IS_COPAY_PER_VISIT = false;
                                isVisit = false;
                            }
                            else
                            {
                                //_copay.IS_COPAY_PER_VISIT = null;
                                isVisit = null;
                            }
                            if (htmlNode.InnerText.Contains("$"))
                            {
                                //_copay.Co_Payment = htmlNode.InnerText.ExtractAmountFromString();
                                //_copay.IS_COPAY_PER = false;
                                co_ins = htmlNode.InnerText.ExtractAmountFromString();
                                isPer = false;
                            }
                            if (htmlNode.InnerText.Contains("%"))
                            {
                                string _str = htmlNode.InnerText;
                                _str = _str.Replace(":", "");
                                var tempAry = _str.Split(' ');
                                //_copay.Co_Payment = tempAry.FirstOrDefault(t => t.Contains("%"))?.ExtractAmountFromString();
                                decimal _dec;
                                decimal.TryParse(tempAry.FirstOrDefault(t => t.Contains("%"))?.Replace("%", ""), out _dec);

                                //_copay.Co_Payment = _dec * 100;

                                //_copay.Co_Payment = _dec;

                                co_ins = _dec;

                                //_copay.IS_COPAY_PER = true;
                                isPer = true;
                            }
                        }
                    }
                    if (co_ins.HasValue)
                    {
                        if (!_copay.Co_Payment.HasValue || (_copay.Co_Payment.Value == 0 && co_ins > _copay.Co_Payment))
                        {
                            _copay.Co_Payment = co_ins;
                            _copay.IS_COPAY_PER_VISIT = isVisit;
                            _copay.IS_COPAY_PER = isPer;
                        }
                    }
                }
                #endregion

                #region Cov_Plan
                //if (Cov_Plan_Nodes != null)
                //{
                //    foreach (var htmlNode in Cov_Plan_Nodes)
                //    {
                //        if (!string.IsNullOrWhiteSpace(htmlNode.InnerText) && string.IsNullOrWhiteSpace(model.CoveragePlan))
                //        {
                //            //Individual
                //            model.CoveragePlan = htmlNode.InnerText;
                //        }
                //    }
                //}
                #endregion

                #region Network
                //if (Network_Nodes != null)
                //{
                //    foreach (var htmlNode in Network_Nodes)
                //    {
                //        if (!string.IsNullOrWhiteSpace(htmlNode.InnerText) && string.IsNullOrWhiteSpace(model.Network))
                //        {
                //            //OUT
                //            model.Network = htmlNode.InnerText;
                //        }
                //    }
                //}
                #endregion

                #region Benefit_Description_PT
                if (Benefit_Description_Nodes != null && (bn.Equals("PT") || bn.Equals("ST")))
                {
                    foreach (var htmlNode in Benefit_Description_Nodes)
                    {
                        if (!model.PT_ST_TOT_AMT_USED.HasValue)
                        {
                            if (htmlNode.InnerText.Contains("$") && !model.PT_ST_TOT_AMT_USED.HasValue) //htmlNode.InnerText.ToLower().Contains("used amount") && 
                            {
                                model.PT_ST_TOT_AMT_USED = htmlNode.InnerText.ExtractAmountFromString();
                            }
                        }
                    }
                }
                #endregion

                #region Benefit_Description_OT
                if (Benefit_Description_Nodes != null && bn.Equals("OT"))
                {
                    foreach (var htmlNode in Benefit_Description_Nodes)
                    {
                        if (!model.OT_TOT_AMT_USED.HasValue)
                        {
                            if (htmlNode.InnerText.Contains("$") && !model.OT_TOT_AMT_USED.HasValue) //htmlNode.InnerText.ToLower().Contains("used amount") && 
                            {
                                model.OT_TOT_AMT_USED = htmlNode.InnerText.ExtractAmountFromString();
                            }
                        }
                    }
                }
                #endregion

                #region Limitations
                if (Limitations_Nodes != null)
                {
                    foreach (var htmlNode in Limitations_Nodes)
                    {
                        if (!model.MYB_LIMIT_DOLLARS.HasValue)
                        {
                            if (htmlNode.InnerText.ToLower().Contains("per calendar year") && htmlNode.InnerText.Contains("$"))
                            {
                                model.MYB_LIMIT_DOLLARS = htmlNode.InnerText.ExtractAmountFromString();
                            }
                        }
                        //if (!htmlNode.InnerText.ToLower().Contains("per calendar year") && htmlNode.InnerText.Contains("$"))
                        //{
                        //    // How we can check this
                        //    model.MYB_LIMIT_VISIT = htmlNode.InnerText.ExtractAmountFromString();
                        //}
                        if (!model.MYB_LIMIT_Remaining_DOLLARS.HasValue)
                        {
                            if (htmlNode.InnerText.ToLower().Contains("remaining") && htmlNode.InnerText.Contains("$"))
                            {
                                model.MYB_LIMIT_Remaining_DOLLARS = htmlNode.InnerText.ExtractAmountFromString();
                            }
                        }
                        //if (!htmlNode.InnerText.ToLower().Contains("remaining") && htmlNode.InnerText.Contains("$"))
                        //{
                        //    // How we can check this
                        //    model.MYB_LIMIT_Remaining_VISIT = htmlNode.InnerText.ExtractAmountFromString();
                        //}
                    }
                }
                #endregion

                #region Effective_Date
                if (Active_Cov_Nodes != null)
                {
                    foreach (var htmlNode in Active_Cov_Nodes)
                    {
                        if (!string.IsNullOrWhiteSpace(htmlNode.InnerText) && !model.Active_Coverage_From.HasValue)
                        {
                            bool hasEndDate = false;
                            if (htmlNode.InnerText.ToLower().Contains("-"))
                            {
                                hasEndDate = true;
                            }
                            List<DateTime> dateTimeList = new List<DateTime>();
                            Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                            MatchCollection mCollection = rx.Matches(htmlNode.InnerText);
                            foreach (var item in mCollection)
                            {
                                Match m = item as Match;
                                if (m.Success)
                                {
                                    DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                               m.Groups["month"].Value,
                                               m.Groups["day"].Value,
                                               m.Groups["year"].Value), "MM/dd/yyyy", null);

                                    dateTimeList.Add(date);
                                }
                            }

                            DateTime? dateTimeFrom = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                            model.Active_Coverage_From = dateTimeFrom;
                            if (hasEndDate)
                            {
                                DateTime? dateTimeTo = dateTimeList.OrderByDescending(t => t.Date).FirstOrDefault();
                                model.Active_Coverage_To = dateTimeTo;
                            }
                        }
                    }
                }
                #endregion

                //break;
            }
            model.CopayList.Add(_copay);

            return model;
        }

        private ResponseHTMLToPDF HTMLToPDF(ServiceConfiguration config, string htmlString, string fileName, bool IS_MVP, string linkMessage = null)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);


            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginTop = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.DisplayHeader = false;
            converter.Options.WebPageWidth = 1000;

            //// footer settings
            //converter.Options.DisplayFooter = true;
            //converter.Footer.Height = 50;
            //converter.Footer.Add(text);
            List<PdfPage> list = new List<PdfPage>();
            PdfDocument doc = converter.ConvertHtmlString(htmlDoc.DocumentNode.OuterHtml);

            //string pdfPath = HttpContext.Current.Server.MapPath("~/" + @"FoxDocumentDirectory\RequestForOrderPDF\");
            string pdfPath = config.ORIGINAL_FILES_PATH_SERVER;

            if (!Directory.Exists(pdfPath))
            {
                Directory.CreateDirectory(pdfPath);
            }
            fileName += DateTime.Now.Ticks + ".pdf";
            string pdfFilePath = pdfPath + fileName;

            var pages = doc.Pages.Count;
            // save pdf document
            doc.Save(pdfFilePath);


            // close pdf document
            doc.Close();
            return new ResponseHTMLToPDF() { FileName = fileName, FilePath = pdfPath, Success = true, ErrorMessage = "" };
        }
        public void SavePdftoImagesEligibilty(string PdfPath, ServiceConfiguration config, int noOfPages, string sorcetype, UserProfile Profile, string patient_account_str, long PATIENT_INSURANCE_ID, bool NewDocument, long work_id = 0)
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
                if (work_id != 0)
                {
                    ObjPatientPATDocument.WORK_ID = work_id;
                }

                ObjPatientPATDocument.DOCUMENT_PATH_LIST = new List<PatientDocumentFiles>();
                var document_type = _foxdocumenttypeRepository.GetFirst(d => !d.DELETED && (d.IS_ACTIVE ?? true) && d.NAME.ToLower() == "forms").DOCUMENT_TYPE_ID;
                ObjPatientPATDocument.DOCUMENT_TYPE = document_type;
                ObjPatientPATDocument.COMMENTS = "Created_by: " + Profile.UserName + "\n Dated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                for (int i = 0; i < noOfPages; i++)
                {
                    string deliveryReportId = "";
                    System.Drawing.Image img;
                    PdfFocus f = new PdfFocus();
                    //f.Serial = "10261435399";
                    f.Serial = "80033727929";
                    f.OpenPdf(PdfPath);
                    var ticks = DateTime.Now.Ticks;
                    if (f.PageCount > 0)
                    {
                        //Save all PDF pages to jpeg images
                        f.ImageOptions.Dpi = 120;
                        f.ImageOptions.ImageFormat = ImageFormat.Jpeg;
                        var image = f.ToImage(i + 1);
                        //Next manipulate with Jpeg in memory or save to HDD, open in a viewer
                        using (var ms = new MemoryStream(image))
                        {
                            if (sorcetype.Split(':')?[0] == "DR")
                            {
                                //deliveryReportId = workId + DateTime.Now.Ticks;
                                using (img = System.Drawing.Image.FromStream(ms))
                                {
                                    //img = System.Drawing.Image.FromStream(ms);
                                    img.Save(config.IMAGES_PATH_SERVER + "\\" + deliveryReportId + "_" + i + ".jpg", ImageFormat.Jpeg);
                                    Bitmap bmp = new Bitmap(img);
                                    ConvertPDFToImages ctp = new ConvertPDFToImages();
                                    img.Dispose();
                                    ctp.SaveWithNewDimention(bmp, 115, 150, 100, config.IMAGES_PATH_SERVER + "\\Logo_" + deliveryReportId + "_" + i + ".jpg");
                                    bmp.Dispose();
                                }
                            }
                            else
                            {
                                using (img = System.Drawing.Image.FromStream(ms))
                                {
                                    //img = System.Drawing.Image.FromStream(ms);
                                    img.Save(config.IMAGES_PATH_SERVER + "\\" + patient_account_str + "_" + ticks + "_" + i + ".jpg", ImageFormat.Jpeg);
                                    Bitmap bmp = new Bitmap(img);
                                    ConvertPDFToImages ctp = new ConvertPDFToImages();
                                    img.Dispose();
                                    ctp.SaveWithNewDimention(bmp, 115, 150, 100, config.IMAGES_PATH_SERVER + "\\Logo" + "_" + patient_account_str + "_" + ticks + "_" + i + ".jpg");
                                    bmp.Dispose();
                                }
                            }
                        }
                    }
                    //End
                    //string ImgDirPath = "FoxDocumentDirectory\\Fox\\Images";
                    var imgPath = "";
                    var logoImgPath = "";
                    if (sorcetype.Split(':')?[0] == "DR")
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

        public ResponseModel AddUpdateNewDocumentInformation(PatientPATDocument ObjPatientPATDocument, UserProfile profile, bool NewDocument)
        {

            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            PatientPATDocument ExistingDocumentInfo = new PatientPATDocument();
            string AddorUpdate = "";
            if (ObjPatientPATDocument.WORK_ID == 0)
            {
                ObjPatientPATDocument.WORK_ID = null;
            }
            if (ObjPatientPATDocument != null)
            {
                var PatientAccount = Convert.ToInt64(ObjPatientPATDocument.PATIENT_ACCOUNT_str);
                interfaceSynch.PATIENT_ACCOUNT = PatientAccount;
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
                    if (ObjPatientPATDocument.WORK_ID != null)
                    {
                        var wqdoctype = _workQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID).DOCUMENT_TYPE;
                        if (!String.Equals(ObjPatientPATDocument.DOCUMENT_TYPE.ToString(), wqdoctype.ToString(), StringComparison.Ordinal))
                        {
                            var existingreferral = _workQueueRepository.GetFirst(q => q.WORK_ID == ObjPatientPATDocument.WORK_ID);
                            existingreferral.DOCUMENT_TYPE = ObjPatientPATDocument.DOCUMENT_TYPE;
                            _workQueueRepository.Update(existingreferral);
                            _workQueueRepository.Save();
                            if (!string.IsNullOrEmpty(ObjPatientPATDocument.COMMENTS) || !string.IsNullOrEmpty(ObjPatientPATDocument.START_DATE.ToString()) || !string.IsNullOrEmpty(ObjPatientPATDocument.END_DATE.ToString()) || ObjPatientPATDocument.SHOW_ON_PATIENT_PORTAL)
                            {
                                AddDocument(ObjPatientPATDocument, profile, NewDocument);
                            }
                        }
                        else
                        {
                            AddDocument(ObjPatientPATDocument, profile, NewDocument);
                        }
                    }
                    else
                    {
                        AddDocument(ObjPatientPATDocument, profile, NewDocument);
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

        public long AddDocument(PatientPATDocument ObjPatientPATDocument, UserProfile profile, bool NewDocument)
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
            if (NewDocument)
            {
                newObjPatientPATDocument.PAT_DOCUMENT_ID = Helper.getMaximumId("FOX_PAT_DOCUMENT_ID");
                var firsttimeID = newObjPatientPATDocument.PAT_DOCUMENT_ID;
                newObjPatientPATDocument.PATIENT_ACCOUNT = Convert.ToInt64(newObjPatientPATDocument.PATIENT_ACCOUNT_str);
                newObjPatientPATDocument.WORK_ID = ObjPatientPATDocument.WORK_ID;
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
                newObjPatientPATDocument.PAT_DOCUMENT_ID = _foxPatientPATdocumentRepository.GetMany(t => t.PATIENT_ACCOUNT == ObjPatientPATDocument.PATIENT_ACCOUNT && (t.DELETED == false)).OrderByDescending(t => t.CREATED_DATE).FirstOrDefault().PAT_DOCUMENT_ID;
            }

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

        private int getNumberOfPagesOfPDF(string PdfPath)
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(PdfPath);
            return pdfReader.NumberOfPages;
        }


        public ExtractEligibilityDataViewModel ExtractDataForMedicare(List<string> benefitNameList, HtmlDocument htmlDoc, ExtractEligibilityDataViewModel model, UserProfile Profile, string patient_account_str)
        {
            Copay _copay = new Copay();
            var currentYear = DateTime.Now.Year.ToString();
            //model = GetEffectiveDates(model);
            model = GetHosAndHHEDataFromEligibility(htmlDoc, model);
            model = GetPatientDeceasedDate(htmlDoc, model);
            model = GetEligibilityStatus(htmlDoc, model);
            model = GetMSPData(htmlDoc, model);
            model.Is_Railroad_Medicare = checkIsRailroadMedicare(htmlDoc);
            model.Is_PPO = checkIsPPO(htmlDoc);
            model.Is_Plan_Expired = checkIsPlanExpired(htmlDoc);

            foreach (var bn in benefitNameList)
            {
                //var Out_of_PocketNodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Out_of_Pocket_" + bn + "')]");
                var Deductible_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Deductible_" + bn + "')]");
                var CO_Insurance_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_CO_Insurance_" + bn + "')]");
                var Copay_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Copay_" + bn + "')]");
                //var Cov_Plan_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Cov_Plan_" + bn + "')]");
                //var Network_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Network_" + bn + "')]");
                var Benefit_Description_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Benefit_Description_" + bn + "')]");
                //var Limitations_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Limitations_" + bn + "')]");
                var Active_Cov_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Active_Coverage_" + bn + "')]");
                //if (bn.Equals("HBPC_Plan_MB"))
                //{
                //    var active_Cov_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Network_" + bn + "')]/ancestor::table[/tbody/tr/td[count(//table/thead/tr/th[.='ACTIVE COVERAGE']/preceding-sibling::th)+1]]");
                //}

                switch (bn)
                {
                    case "HBPC_Plan_MB":
                        _copay.Type = "Medicare";
                        break;
                    case "PT":
                        _copay.Type = "PT";
                        break;
                    case "ST":
                        _copay.Type = "ST";
                        break;
                    case "OT":
                        _copay.Type = "OT";
                        break;
                    default:
                        break;
                }

                #region Out_of_Pocket
                //if (Out_of_PocketNodes != null)
                //{
                //    foreach (var htmlNode in Out_of_PocketNodes)
                //    {
                //        if (htmlNode.InnerText.Contains(currentYear) && !model.MOP_AMT.HasValue)
                //        {
                //            if (htmlNode.InnerText.ToLower().Contains("per calendar year") && htmlNode.InnerText.Contains("$"))
                //            {
                //                model.MOP_AMT = htmlNode.InnerText.ExtractAmountFromString();
                //            }
                //        }
                //        if (htmlNode.InnerText.Contains(currentYear) && !model.MOP_AMT_Remaining.HasValue)
                //        {
                //            if (htmlNode.InnerText.ToLower().Contains("remaining") && htmlNode.InnerText.Contains("$"))
                //            {
                //                model.MOP_AMT_Remaining = htmlNode.InnerText.ExtractAmountFromString();
                //            }
                //        }
                //    }
                //}
                #endregion

                #region Deductible
                if (Deductible_Nodes != null)
                {
                    foreach (var htmlNode in Deductible_Nodes)
                    {
                        if (htmlNode.InnerText.Contains(currentYear))
                        {
                            if (htmlNode.InnerText.ToLower().Contains("per calendar year") && htmlNode.InnerText.Contains("$") && !model.YEARLY_DED_AMT.HasValue)
                            {
                                model.YEARLY_DED_AMT = htmlNode.InnerText.ExtractAmountFromString();

                                List<DateTime> dateTimeList = new List<DateTime>();
                                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                                MatchCollection mCollection = rx.Matches(htmlNode.InnerText);
                                foreach (var item in mCollection)
                                {
                                    Match m = item as Match;
                                    if (m.Success)
                                    {
                                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                        m.Groups["month"].Value,
                                        m.Groups["day"].Value,
                                        m.Groups["year"].Value), "MM/dd/yyyy", null);
                                        //DateTime date = DateTime.Parse(String.Format("{0}/{1}/{2} {3}",
                                        //    m.Groups["month"].Value,
                                        //    m.Groups["day"].Value,
                                        //    m.Groups["year"].Value,
                                        //    m.Groups["time"].Value));

                                        dateTimeList.Add(date);
                                    }
                                }

                                DateTime dateTimeFrom = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                                DateTime dateTimeTo = dateTimeList.OrderByDescending(t => t.Date).FirstOrDefault();

                                model.DED_POLICY_LIMIT_RESET_ON = dateTimeFrom.ToString("MM/dd");
                            }

                            if (htmlNode.InnerText.ToLower().Contains("remaining") && htmlNode.InnerText.Contains("$") && !model.DED_REMAINING.HasValue)
                            {
                                model.DED_REMAINING = htmlNode.InnerText.ExtractAmountFromString();
                            }
                        }
                    }
                }
                #endregion

                #region CO_Insurance
                if (CO_Insurance_Nodes != null)
                {
                    foreach (var htmlNode in CO_Insurance_Nodes)
                    {
                        if (htmlNode.InnerText.Contains(currentYear))
                        {
                            if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                            {
                                if (htmlNode.InnerText.ToLower().Contains("per visit"))
                                {
                                    _copay.IS_COPAY_PER_VISIT = true;
                                }
                                else if (htmlNode.InnerText.ToLower().Contains("per day") || htmlNode.InnerText.ToLower().Contains("episode"))
                                {
                                    _copay.IS_COPAY_PER_VISIT = false;
                                }
                                else
                                {
                                    _copay.IS_COPAY_PER_VISIT = null;
                                }
                                if (htmlNode.InnerText.Contains("$"))
                                {
                                    _copay.Co_Payment = htmlNode.InnerText.ExtractAmountFromString();
                                    _copay.IS_COPAY_PER = false;
                                }
                                if (htmlNode.InnerText.Contains("%"))
                                {
                                    string _str = htmlNode.InnerText;

                                    var tempAry = _str.Split(' ');
                                    //_copay.Co_Payment = tempAry.FirstOrDefault(t => t.Contains("%"))?.ExtractAmountFromString();
                                    decimal _dec;
                                    decimal.TryParse(tempAry.FirstOrDefault(t => t.Contains("%"))?.Replace("%", ""), out _dec);

                                    //_copay.Co_Payment = _dec * 100;
                                    _copay.Co_Payment = _dec;


                                    _copay.IS_COPAY_PER = true;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Copay
                if (Copay_Nodes != null)
                {
                    foreach (var htmlNode in Copay_Nodes)
                    {
                        if (htmlNode.InnerText.Contains(currentYear))
                        {
                            if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                            {
                                if (htmlNode.InnerText.ToLower().Contains("per visit"))
                                {
                                    _copay.IS_COPAY_PER_VISIT = true;
                                }
                                else if (htmlNode.InnerText.ToLower().Contains("per day") || htmlNode.InnerText.ToLower().Contains("episode"))
                                {
                                    _copay.IS_COPAY_PER_VISIT = false;
                                }
                                else
                                {
                                    _copay.IS_COPAY_PER_VISIT = null;
                                }
                                if (htmlNode.InnerText.Contains("$"))
                                {
                                    _copay.Co_Payment = htmlNode.InnerText.ExtractAmountFromString();
                                    _copay.IS_COPAY_PER = false;
                                }
                                if (htmlNode.InnerText.Contains("%"))
                                {
                                    string _str = htmlNode.InnerText;

                                    var tempAry = _str.Split(' ');
                                    //_copay.Co_Payment = tempAry.FirstOrDefault(t => t.Contains("%"))?.ExtractAmountFromString();

                                    decimal _dec;
                                    decimal.TryParse(tempAry.FirstOrDefault(t => t.Contains("%"))?.Replace("%", ""), out _dec);

                                    //_copay.Co_Payment = _dec * 100;
                                    _copay.Co_Payment = _dec;
                                    _copay.IS_COPAY_PER = true;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Cov_Plan
                //if (Cov_Plan_Nodes != null)
                //{
                //    foreach (var htmlNode in Cov_Plan_Nodes)
                //    {
                //        if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                //        {
                //            //Individual
                //            if (string.IsNullOrWhiteSpace(model.CoveragePlan))
                //                model.CoveragePlan = htmlNode.InnerText;
                //        }
                //    }
                //}
                //#endregion

                //#region Network
                //if (Network_Nodes != null)
                //{
                //    foreach (var htmlNode in Network_Nodes)
                //    {
                //        if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                //        {
                //            //OUT
                //            if (string.IsNullOrWhiteSpace(model.Network))
                //                model.Network = htmlNode.InnerText;
                //        }
                //    }
                //}
                #endregion

                #region Benefit_Description_PT
                if (Benefit_Description_Nodes != null && (bn.Equals("PT") || bn.Equals("ST")))
                {
                    foreach (var htmlNode in Benefit_Description_Nodes)
                    {
                        if (htmlNode.InnerText.Contains(currentYear) && !model.PT_ST_TOT_AMT_USED.HasValue)
                        {
                            if (htmlNode.InnerText.Contains("$") && !model.PT_ST_TOT_AMT_USED.HasValue) //htmlNode.InnerText.ToLower().Contains("used amount") && 
                            {
                                model.PT_ST_TOT_AMT_USED = htmlNode.InnerText.ExtractAmountFromString();
                            }
                        }
                    }
                }
                #endregion

                #region Benefit_Description_OT
                if (Benefit_Description_Nodes != null && bn.Equals("OT"))
                {
                    foreach (var htmlNode in Benefit_Description_Nodes)
                    {
                        if (htmlNode.InnerText.Contains(currentYear) && !model.OT_TOT_AMT_USED.HasValue)
                        {
                            if (htmlNode.InnerText.Contains(currentYear))
                            {
                                if (htmlNode.InnerText.Contains("$") && !model.OT_TOT_AMT_USED.HasValue) //htmlNode.InnerText.ToLower().Contains("used amount") && 
                                {
                                    model.OT_TOT_AMT_USED = htmlNode.InnerText.ExtractAmountFromString();
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Limitations
                //if (Limitations_Nodes != null)
                //{
                //    foreach (var htmlNode in Limitations_Nodes)
                //    {
                //        if (htmlNode.InnerText.Contains(currentYear) && !model.MYB_LIMIT_DOLLARS.HasValue)
                //        {
                //            if (htmlNode.InnerText.ToLower().Contains("per calendar year") && htmlNode.InnerText.Contains("$"))
                //            {
                //                model.MYB_LIMIT_DOLLARS = htmlNode.InnerText.ExtractAmountFromString();
                //            }
                //        }
                //        if (htmlNode.InnerText.Contains(currentYear) && !model.MYB_LIMIT_Remaining_DOLLARS.HasValue)
                //        {
                //            if (htmlNode.InnerText.ToLower().Contains("remaining") && htmlNode.InnerText.Contains("$"))
                //            {
                //                model.MYB_LIMIT_Remaining_DOLLARS = htmlNode.InnerText.ExtractAmountFromString();
                //            }
                //        }
                //    }
                //}
                #endregion

                #region Effective_Date
                if (Active_Cov_Nodes != null && bn.Equals("HBPC_Plan_MB"))
                {
                    foreach (var htmlNode in Active_Cov_Nodes)
                    {
                        if (!string.IsNullOrWhiteSpace(htmlNode.InnerText) && !model.Active_Coverage_From.HasValue)
                        {
                            bool hasEndDate = false;
                            if (htmlNode.InnerText.ToLower().Contains("-"))
                            {
                                hasEndDate = true;
                            }

                            List<DateTime> dateTimeList = new List<DateTime>();
                            Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                            MatchCollection mCollection = rx.Matches(htmlNode.InnerText);
                            foreach (var item in mCollection)
                            {
                                Match m = item as Match;
                                if (m.Success)
                                {
                                    DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                                  m.Groups["month"].Value,
                                                  m.Groups["day"].Value,
                                                  m.Groups["year"].Value), "MM/dd/yyyy", null);

                                    dateTimeList.Add(date);
                                }
                            }

                            DateTime? dateTimeFrom = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                            model.Active_Coverage_From = dateTimeFrom;
                            if (hasEndDate)
                            {
                                //DateTime? dateTimeTo = dateTimeList.OrderByDescending(t => t.Date).FirstOrDefault();
                                //model.Active_Coverage_To = dateTimeTo;
                            }
                        }
                    }
                }
                #endregion
            }

            model.CopayList.Add(_copay);
            return model;
        }

        private ExtractEligibilityDataViewModel GetEligibilityStatus(HtmlDocument htmlDoc, ExtractEligibilityDataViewModel model)
        {
            var general_elig_info_node = htmlDoc.DocumentNode.SelectNodes("*//table[contains(@id,'Elig_tbl')]//tr[td//text()[contains(., 'Status')]]");
            if (general_elig_info_node != null)
            {
                //if (general_elig_info_node.Count > 1)
                //{
                foreach (var statustr in general_elig_info_node)
                {
                    //var statusValueTD = statustr.SelectSingleNode("//td[1]");
                    var statusValueTD = statustr.LastChild;

                    if (statusValueTD != null && !string.IsNullOrWhiteSpace(statusValueTD.InnerText) && string.IsNullOrWhiteSpace(model.Eligibility_Status))
                    {
                        if (statusValueTD.InnerText.ToLower().Contains("medicare"))
                        {
                            if (!statusValueTD.InnerText.ToLower().Contains("part a"))
                            {
                                model.Eligibility_Status = statusValueTD.InnerText.Trim();
                            }
                        }
                        else
                        {
                            model.Eligibility_Status = statusValueTD.InnerText.Trim();
                        }
                    }
                }
            }

            return model;
        }

        public ExtractEligibilityDataViewModel GetHosAndHHEDataFromEligibility(HtmlDocument htmlDoc, ExtractEligibilityDataViewModel model)
        {
            var Home_Health_Care_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Health_care_facility_HBPC')]");
            var Home_Hospice_Nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Health_care_facility_HO')]");
            if (Home_Health_Care_Nodes != null)
            {
                foreach (var htmlNode in Home_Health_Care_Nodes)
                {
                    if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                    {
                        var nodedata = htmlNode.InnerHtml.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (nodedata.Count() > 0)
                        {
                            if (htmlNode.InnerText.Contains("NPI") && string.IsNullOrWhiteSpace(model.HHE_NPI))
                            {
                                var npi_str = nodedata.Where(e => e.Contains("NPI")).FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(npi_str))
                                {
                                    if (npi_str.Contains(":"))
                                    {
                                        var npi_data = npi_str.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        model.HHE_NPI = npi_data.LastOrDefault();
                                        if (!string.IsNullOrWhiteSpace(model.HHE_NPI))
                                        {
                                            model.HHE_NPI = model.HHE_NPI.Trim();
                                        }
                                    }
                                }
                            }

                            //if (htmlNode.InnerText.Contains("Period Start") && !model.HHE_Effective_Date.HasValue)
                            //{
                            //    var start_Date_str = nodedata.Where(e => e.ToLower().Contains("period start")).FirstOrDefault();
                            //    List<DateTime> dateTimeList = new List<DateTime>();
                            //    Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                            //    MatchCollection mCollection = rx.Matches(start_Date_str);
                            //    foreach (var item in mCollection)
                            //    {
                            //        Match m = item as Match;
                            //        if (m.Success)
                            //        {
                            //            DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                            //                m.Groups["month"].Value,
                            //                m.Groups["day"].Value,
                            //                m.Groups["year"].Value), "MM/dd/yyyy", null);

                            //            dateTimeList.Add(date);
                            //        }
                            //    }

                            //    model.HHE_Effective_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                            //}

                            //if (htmlNode.InnerText.Contains("Period End") && !model.HHE_End_Date.HasValue)
                            //{
                            //    var end_date_str = nodedata.Where(e => e.ToLower().Contains("period end")).FirstOrDefault();
                            //    List<DateTime> dateTimeList = new List<DateTime>();
                            //    Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                            //    MatchCollection mCollection = rx.Matches(end_date_str);
                            //    foreach (var item in mCollection)
                            //    {
                            //        Match m = item as Match;
                            //        if (m.Success)
                            //        {
                            //            DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                            //                m.Groups["month"].Value,
                            //                m.Groups["day"].Value,
                            //                m.Groups["year"].Value), "MM/dd/yyyy", null);

                            //            dateTimeList.Add(date);
                            //        }
                            //    }

                            //    model.HHE_End_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                            //}

                            //if (htmlNode.InnerText.Contains("Plan") || htmlNode.InnerText.Contains("Benefit"))
                            //{
                            //    string start_Date_str = "";
                            //    if (htmlNode.InnerText.Contains("Plan"))
                            //        start_Date_str = nodedata.Where(e => e.ToLower().Contains("Plan")).FirstOrDefault();
                            //    else
                            //        start_Date_str = nodedata.Where(e => e.ToLower().Contains("Benefit")).FirstOrDefault();

                            //    List<DateTime> dateTimeList = new List<DateTime>();
                            //    Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                            //    MatchCollection mCollection = rx.Matches(start_Date_str);
                            //    foreach (var item in mCollection)
                            //    {
                            //        Match m = item as Match;
                            //        if (m.Success)
                            //        {
                            //            DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                            //                m.Groups["month"].Value,
                            //                m.Groups["day"].Value,
                            //                m.Groups["year"].Value), "MM/dd/yyyy", null);

                            //            dateTimeList.Add(date);
                            //        }
                            //    }

                            //    model.HHE_Effective_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault(); ;
                            //    if (dateTimeList.Count() > 1)
                            //    {
                            //        model.HHE_End_Date = dateTimeList.OrderByDescending(t => t.Date).FirstOrDefault();
                            //    }
                            //}

                            if (htmlNode.InnerText.ToLower().Contains("date of service"))
                            {
                                string start_Date_str = nodedata.Where(e => e.ToLower().Contains("date of service")).FirstOrDefault();
                                //if (htmlNode.InnerText.Contains("Plan"))
                                //    start_Date_str = nodedata.Where(e => e.ToLower().Contains("Plan")).FirstOrDefault();
                                //else
                                //    start_Date_str = nodedata.Where(e => e.ToLower().Contains("Benefit")).FirstOrDefault();

                                List<DateTime> dateTimeList = new List<DateTime>();
                                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                                MatchCollection mCollection = rx.Matches(start_Date_str);
                                foreach (var item in mCollection)
                                {
                                    Match m = item as Match;
                                    if (m.Success)
                                    {
                                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                            m.Groups["month"].Value,
                                            m.Groups["day"].Value,
                                            m.Groups["year"].Value), "MM/dd/yyyy", null);

                                        dateTimeList.Add(date);
                                    }
                                }
                                if (!model.HHE_Effective_Date.HasValue)
                                    model.HHE_Effective_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                                if (dateTimeList.Count() > 1 && !model.HHE_End_Date.HasValue)
                                {
                                    model.HHE_End_Date = dateTimeList.OrderByDescending(t => t.Date).FirstOrDefault();
                                }
                            }
                        }
                    }
                }
            }

            if (Home_Hospice_Nodes != null)
            {
                foreach (var htmlNode in Home_Hospice_Nodes)
                {
                    if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                    {
                        var nodedata = htmlNode.InnerHtml.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (nodedata.Count() > 0)
                        {
                            if (htmlNode.InnerText.Contains("NPI") && string.IsNullOrWhiteSpace(model.HOS_NPI))
                            {
                                var npi_str = nodedata.Where(e => e.Contains("NPI")).FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(npi_str))
                                {
                                    if (npi_str.Contains(":"))
                                    {
                                        var npi_data = npi_str.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        model.HOS_NPI = npi_data.LastOrDefault();
                                        if (!string.IsNullOrWhiteSpace(model.HOS_NPI))
                                        {
                                            model.HOS_NPI = model.HOS_NPI.Trim();
                                        }
                                    }
                                }
                            }

                            if (htmlNode.InnerText.Contains("Period Start") && !model.HOS_Effective_Date.HasValue)
                            {
                                var start_Date_str = nodedata.Where(e => e.ToLower().Contains("period start")).FirstOrDefault();
                                List<DateTime> dateTimeList = new List<DateTime>();
                                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                                MatchCollection mCollection = rx.Matches(start_Date_str);
                                foreach (var item in mCollection)
                                {
                                    Match m = item as Match;
                                    if (m.Success)
                                    {
                                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                            m.Groups["month"].Value,
                                            m.Groups["day"].Value,
                                            m.Groups["year"].Value), "MM/dd/yyyy", null);

                                        dateTimeList.Add(date);
                                    }
                                }

                                model.HOS_Effective_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                            }

                            if (htmlNode.InnerText.Contains("Period End") && !model.HOS_End_Date.HasValue)
                            {
                                var start_Date_str = nodedata.Where(e => e.ToLower().Contains("period end")).FirstOrDefault();
                                List<DateTime> dateTimeList = new List<DateTime>();
                                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                                MatchCollection mCollection = rx.Matches(start_Date_str);
                                foreach (var item in mCollection)
                                {
                                    Match m = item as Match;
                                    if (m.Success)
                                    {
                                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                            m.Groups["month"].Value,
                                            m.Groups["day"].Value,
                                            m.Groups["year"].Value), "MM/dd/yyyy", null);

                                        dateTimeList.Add(date);
                                    }
                                }

                                model.HOS_End_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                            }

                            if ((htmlNode.InnerText.Contains("Plan") || htmlNode.InnerText.Contains("Benefit")) && !model.HOS_Effective_Date.HasValue && !model.HOS_End_Date.HasValue)
                            {
                                string start_Date_str = "";
                                if (htmlNode.InnerText.Contains("Plan"))
                                    start_Date_str = nodedata.Where(e => e.ToLower().Contains("plan")).FirstOrDefault();
                                else
                                    start_Date_str = nodedata.Where(e => e.ToLower().Contains("benefit")).FirstOrDefault();

                                List<DateTime> dateTimeList = new List<DateTime>();
                                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                                MatchCollection mCollection = rx.Matches(start_Date_str);
                                foreach (var item in mCollection)
                                {
                                    Match m = item as Match;
                                    if (m.Success)
                                    {
                                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                            m.Groups["month"].Value,
                                            m.Groups["day"].Value,
                                            m.Groups["year"].Value), "MM/dd/yyyy", null);

                                        dateTimeList.Add(date);
                                    }
                                }

                                model.HOS_Effective_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
                                if (dateTimeList.Count() > 1)
                                {
                                    model.HOS_End_Date = dateTimeList.OrderByDescending(t => t.Date).FirstOrDefault();
                                }
                            }

                            if (htmlNode.InnerText.ToLower().Contains("date of service") && !model.HOS_Effective_Date.HasValue && !model.HOS_End_Date.HasValue)
                            {
                                string start_Date_str = "";
                                //if (htmlNode.InnerText.Contains("Plan"))
                                //    start_Date_str = nodedata.Where(e => e.ToLower().Contains("Plan")).FirstOrDefault();
                                //else
                                //    start_Date_str = nodedata.Where(e => e.ToLower().Contains("Benefit")).FirstOrDefault();

                                List<DateTime> dateTimeList = new List<DateTime>();
                                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                                MatchCollection mCollection = rx.Matches(start_Date_str);
                                foreach (var item in mCollection)
                                {
                                    Match m = item as Match;
                                    if (m.Success)
                                    {
                                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                                            m.Groups["month"].Value,
                                            m.Groups["day"].Value,
                                            m.Groups["year"].Value), "MM/dd/yyyy", null);

                                        dateTimeList.Add(date);
                                    }
                                }

                                model.HOS_Effective_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault(); ;
                                if (dateTimeList.Count() > 1)
                                {
                                    model.HOS_End_Date = dateTimeList.OrderByDescending(t => t.Date).FirstOrDefault();
                                }
                            }
                        }
                    }
                }

                //Jaime's request to add hospice end date only if its in the past (https://www.wrike.com/workspace.htm?acc=2335832#path=inbox&ei=276750653&et=task)
                if (model.HOS_End_Date.HasValue && model.HOS_End_Date.Value.Date >= DateTime.Now.Date)
                    model.HOS_End_Date = null;
            }

            return model;
        }

        private ExtractEligibilityDataViewModel GetPatientDeceasedDate(HtmlDocument htmlDoc, ExtractEligibilityDataViewModel model)
        {
            var decease_date_node = htmlDoc.DocumentNode.SelectSingleNode("//tr[contains(@class,'Fox_Pat_Deceased_Date')]/td[2]");
            if (decease_date_node != null)
            {
                List<DateTime> dateTimeList = new List<DateTime>();
                Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
                MatchCollection mCollection = rx.Matches(decease_date_node.InnerText);
                foreach (var item in mCollection)
                {
                    Match m = item as Match;
                    if (m.Success)
                    {
                        DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
                            m.Groups["month"].Value,
                            m.Groups["day"].Value,
                            m.Groups["year"].Value), "MM/dd/yyyy", null);

                        dateTimeList.Add(date);
                    }
                }

                model.Deceased_Date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
            }

            return model;
        }

        public PatientInsuranceAuthDetails GetCurrentPatientAuthorizations(long patient_Account, UserProfile profile)
        {
            bool calculateTotal = false;
            bool calculateTotalUsed = false;
            bool calculateTotalRemaining = false;
            bool calculateSimpleRemaining = false;
            Regex regex = new Regex(@"^[-+]?\d+\.?\d*$", RegexOptions.Compiled);
            PatientInsuranceAuthDetails patient_Ins_Auth_Details = new PatientInsuranceAuthDetails();
            //Fetch Patient Details
            var patient = _PatientRepository.GetFirst(e => e.Patient_Account == patient_Account && (e.DELETED ?? false) == false);
            if (patient != null)
            {
                //Fetch Current Insurance Details
                patient_Ins_Auth_Details.Current_Patient_Insurances = new List<PatientInsurance>();
                var curr_insurances = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == patient_Account && (x.Deleted ?? false) == false && (string.IsNullOrEmpty(x.FOX_INSURANCE_STATUS) || x.FOX_INSURANCE_STATUS == "C")); //Current  && (x.Pri_Sec_Oth_Type.ToUpper() != "PR")
                if (curr_insurances != null && curr_insurances.Count > 0)
                {
                    //Fetch value types
                    patient_Ins_Auth_Details.ValueTypeList = new List<FOX_TBL_AUTH_VALUE_TYPE>();
                    var valueTypeList = _AuthorizationValueTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);
                    if (valueTypeList != null && valueTypeList.Count > 0)
                    {
                        patient_Ins_Auth_Details.ValueTypeList = valueTypeList;
                    }
                    patient_Ins_Auth_Details.Current_Patient_Insurances = curr_insurances;

                    for (int i = 0; i < patient_Ins_Auth_Details.Current_Patient_Insurances.Count; i++)
                    {
                        var insType = patient_Ins_Auth_Details.Current_Patient_Insurances[i].Pri_Sec_Oth_Type;
                        if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("p"))
                        {
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 1;//To display in proper order at fornt-end
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Primary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("s"))
                        {
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 2;//To display in proper order at fornt-end
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Secondary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("t"))
                        {
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 3;//To display in proper order at fornt-end
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Tertiary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("q"))
                        {
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 4;//To display in proper order at fornt-end
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Quaternary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("pr"))
                        {
                            if (patient_Ins_Auth_Details.Current_Patient_Insurances[i].IS_PRIVATE_PAY ?? false)
                            {
                                patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 5;//To display in proper order at fornt-end
                                patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Private Pay";
                            }
                            else
                            {
                                patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 6;//To display in proper order at fornt-end
                                patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Residual Balance";
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("x"))
                        {
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 7;//To display in proper order at fornt-end
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Primary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("y"))
                        {
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 8;//To display in proper order at fornt-end
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Secondary";
                        }
                        else if (!string.IsNullOrWhiteSpace(insType) && insType.ToLower().Equals("z"))
                        {
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].DisplayOrder = 9;//To display in proper order at fornt-end
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].Ins_Type = "Invalid Other";
                        }

                        FoxInsurancePayers obj = _foxInsurancePayersRepository.GetByID(patient_Ins_Auth_Details.Current_Patient_Insurances[i].FOX_TBL_INSURANCE_ID);

                        if (obj != null)
                        {
                            //patient_Ins_Auth_Details.Current_Patient_Insurances[i].InsPayer_Description = obj.INSURANCE_NAME;
                            patient_Ins_Auth_Details.Current_Patient_Insurances[i].InsPayer_Description = obj.INSURANCE_NAME;
                        }
                    }

                    //To display in proper order at fornt-end
                    patient_Ins_Auth_Details.Current_Patient_Insurances = patient_Ins_Auth_Details.Current_Patient_Insurances.OrderBy(e => e.DisplayOrder).ToList();
                }

                //Fetch Authorization List Details
                patient_Ins_Auth_Details.PatientAuthorizationList = new List<FOX_TBL_AUTH>();
                foreach (var ins in patient_Ins_Auth_Details.Current_Patient_Insurances)
                {
                    var authorizations = _PatientAuthorizationRepository.GetMany(x => x.Parent_Patient_insurance_Id == ins.Parent_Patient_insurance_Id && !x.DELETED);
                    if (authorizations != null && authorizations.Count > 0)
                    {
                        foreach (var auth in authorizations)
                        {
                            if (!string.IsNullOrEmpty(auth.AUTH_UNIQUE_ID) && auth.AUTH_UNIQUE_ID.Contains("_"))
                            {
                                auth.IS_SPLIT_AUTH = true;
                            }
                            else
                            {
                                auth.IS_SPLIT_AUTH = false;
                            }

                            //Fetch Case Name
                            if (auth.CASE_ID != null && auth.CASE_ID != 0)
                            {
                                var caseObj = _vwPatientCaseRepository.GetByID(auth.CASE_ID);
                                if (caseObj != null)
                                {
                                    auth.CASE_NO = caseObj.CASE_NO ?? "";
                                    auth.RT_CASE_NO = caseObj.RT_CASE_NO ?? "";
                                }
                            }

                            //Billing Provider Name
                            if (auth.BILLING_PROVIDER_ID != null && auth.BILLING_PROVIDER_ID != 0)
                            {
                                var ordSrcBP = _OrderingRefSourceRepository.GetByID(auth.BILLING_PROVIDER_ID);
                                if (ordSrcBP != null)
                                {
                                    auth.BILLING_PROVIDER_NAME = ordSrcBP.FIRST_NAME + " " + ordSrcBP.LAST_NAME;
                                    auth.BILLING_PROVIDER_NOTES = ordSrcBP.NOTES;
                                    if (!string.IsNullOrEmpty(ordSrcBP.REFERRAL_REGION))
                                    {
                                        auth.BILLING_PROVIDER_NAME += " | " + ordSrcBP.REFERRAL_REGION;
                                    }
                                }
                            }

                            if (auth.REFERRED_BY_ID != null && auth.REFERRED_BY_ID != 0)
                            {
                                var ordSrcRB = _OrderingRefSourceRepository.GetByID(auth.REFERRED_BY_ID);
                                if (ordSrcRB != null)
                                {
                                    auth.REFERRED_BY_NAME = ordSrcRB.FIRST_NAME + " " + ordSrcRB.LAST_NAME;
                                    auth.REFERRED_BY_NOTES = ordSrcRB.NOTES;
                                    if (!string.IsNullOrEmpty(ordSrcRB.REFERRAL_REGION))
                                    {
                                        auth.REFERRED_BY_NAME += " | " + ordSrcRB.REFERRAL_REGION;
                                    }
                                }
                            }
                            if (patient_Ins_Auth_Details.ValueTypeList != null && patient_Ins_Auth_Details.ValueTypeList.Count > 0 && auth.MULT_VALUE_TYPE_ID != null)
                            {
                                auth.MULT_VALUE_TYPE_NAME = patient_Ins_Auth_Details.ValueTypeList.Where(v => v.AUTH_VALUE_TYPE_ID == auth.MULT_VALUE_TYPE_ID).FirstOrDefault()?.NAME;
                            }


                            if (auth.LOC_ID != null && auth.LOC_ID != 0)
                            {
                                var locData = _FacilityLocationRepository.GetByID(auth.LOC_ID);
                                if (locData != null)
                                {
                                    auth.LOC_NAME = locData.NAME;
                                    //if (!string.IsNullOrEmpty(locData.REGION_NAME))
                                    //{
                                    //    auth.REFERRED_BY_NAME += " | " + locData.REFERRAL_REGION;
                                    //}
                                }
                            }

                            //mapped appoinments data
                            auth.AuthAppointmentIds = new List<int>();
                            var authApps = _AuthorizationAppointmentRepository.GetMany(e => e.AUTH_ID == auth.AUTH_ID && e.APPT_TYPE_ID.HasValue && !e.DELETED);
                            if (authApps != null && authApps.Count > 0)
                            {
                                var ids = authApps.Select(e => e.APPT_TYPE_ID.Value).ToList();
                                auth.AuthAppointmentIds = ids;
                                //foreach (var authAppointment in auth.AuthorizationAppointments)
                                //{
                                //    var apptType = _AppointmentTypeRepository.GetFirst(e => e.APPT_TYPE_ID == authAppointment.APPT_TYPE_ID && !e.DELETED);
                                //    if (apptType != null)
                                //    {
                                //        authAppointment.APPT_CODE = apptType.APPT_CODE;
                                //        authAppointment.NAME = apptType.NAME;
                                //    }
                                //}
                            }

                            //mapped charges data
                            auth.AuthorizationChargesList = new List<FOX_TBL_AUTH_CHARGES>();
                            for (int i = 0; i < 6; i++)
                            {
                                auth.AuthorizationChargesList.Add(new FOX_TBL_AUTH_CHARGES());
                            }
                            if (auth.TOTAL == null || auth.TOTAL == 0)
                            {
                                auth.TOTAL = 0;
                                calculateTotal = true;
                            }
                            if (auth.AuthTotalUsed == null || auth.AuthTotalUsed == 0)
                            {
                                auth.AuthTotalUsed = 0;
                                calculateTotalUsed = true;
                            }
                            if (auth.AuthTotalRemaining == null || auth.AuthTotalRemaining == 0)
                            {
                                auth.AuthTotalRemaining = 0;
                                calculateTotalRemaining = true;
                            }
                            if (auth.IsSimple != null && auth.IsSimple == true && auth.SimpleAllowded != null && auth.SimpleAllowded > 0 && auth.SimpleUsed != null && auth.SimpleUsed > 0)
                            {
                                calculateSimpleRemaining = true;
                            }

                            var authCharges = _AuthorizationChargesRepository.GetMany(e => e.AUTH_ID == auth.AUTH_ID && !e.DELETED).OrderByDescending(e => e.MODIFIED_DATE).ToList();
                            if (authCharges != null && authCharges.Count > 0)
                            {
                                for (int i = 0; i < authCharges.Count(); i++)
                                {
                                    if (i == 6)
                                    {
                                        break;
                                    }
                                    auth.AuthorizationChargesList[i] = authCharges[i];
                                    if (!string.IsNullOrWhiteSpace(auth.AuthorizationChargesList[i].CHARGES))
                                    {
                                        auth.AuthorizationChargesList[i].Disable = true;
                                    }
                                    if (calculateTotal && authCharges[i].VALUE.HasValue)
                                    {
                                        //if (regex.IsMatch(authCharges[i].VALUE))
                                        //{
                                        //    auth.TOTAL = (auth.TOTAL ?? 0) + decimal.Parse(authCharges[i].VALUE);
                                        //}
                                        auth.TOTAL = (auth.TOTAL ?? 0) + authCharges[i].VALUE;
                                    }
                                    if (calculateTotalUsed && authCharges[i].USED != null && authCharges[i].USED != 0)
                                    {
                                        auth.AuthTotalUsed = (auth.AuthTotalUsed ?? 0) + authCharges[i].USED;
                                    }
                                    if (calculateTotalRemaining && authCharges[i].REMAINING != null && authCharges[i].REMAINING != 0)
                                    {
                                        auth.AuthTotalRemaining = (auth.AuthTotalRemaining ?? 0) + authCharges[i].REMAINING;
                                    }
                                    if (authCharges[i].VALUE_TYPE_ID != null)
                                    {
                                        authCharges[i].ValueTypeName = patient_Ins_Auth_Details.ValueTypeList.Where(v => v.AUTH_VALUE_TYPE_ID == authCharges[i].VALUE_TYPE_ID).FirstOrDefault()?.NAME;
                                        if (auth.ChargesMultiple == false)
                                        {
                                            if (authCharges[i].ValueTypeName.ToLower() == "multiple")
                                            {
                                                auth.ChargesMultiple = true;
                                            }
                                        }
                                    }
                                }

                                //auth.AuthorizationChargesList = authCharges;
                            }

                            if (auth.TOTAL != null && calculateTotal)
                            {
                                auth.TOTAL = auth.TOTAL + (auth.MULT_VALUE ?? 0);
                            }
                            if (auth.AuthTotalUsed != null && calculateTotalUsed)
                            {
                                auth.AuthTotalUsed = auth.AuthTotalUsed + (auth.MULT_USED ?? 0);
                            }
                            if (auth.AuthTotalRemaining != null && calculateTotalRemaining)
                            {
                                auth.AuthTotalRemaining = auth.AuthTotalRemaining + (auth.MULT_REMAINING ?? 0);
                            }
                            if (calculateSimpleRemaining)
                            {
                                auth.SimpleRemaining = (auth.SimpleAllowded ?? 0) - (auth.SimpleUsed ?? 0);
                            }
                            if (auth.MULT_VALUE != null)
                            {
                                auth.TOTAL = auth.MULT_VALUE;
                                auth.AuthTotalUsed = auth.MULT_USED ?? 0;
                                auth.AuthTotalRemaining = auth.MULT_REMAINING ?? 0;
                                if (auth.AuthTotalUsed == null)
                                {
                                    auth.AuthTotalUsed = 0;
                                }
                                if (auth.AuthTotalRemaining == null)
                                {
                                    auth.AuthTotalRemaining = 0;
                                }
                                if (auth.MULT_REMAINING == null)
                                {
                                    if (auth.MULT_USED == null)
                                    {
                                        auth.MULT_USED = 0;
                                    }
                                    auth.MULT_REMAINING = auth.MULT_VALUE - auth.MULT_USED;
                                }
                                else if (auth.MULT_USED == null)
                                {
                                    auth.MULT_USED = 0;
                                    auth.MULT_USED = auth.MULT_VALUE - auth.MULT_REMAINING;
                                }
                            }
                            if (auth.TOTAL != null)
                            {
                                ins.AuthGrandTotal = (ins.AuthGrandTotal ?? 0) + auth.TOTAL;
                            }
                            if (auth.AuthTotalUsed != null)
                            {
                                ins.AuthGrandTotalUsed = (ins.AuthGrandTotalUsed ?? 0) + auth.AuthTotalUsed;
                            }
                            if (auth.AuthTotalRemaining != null)
                            {
                                ins.AuthGrandTotalRemaining = (ins.AuthGrandTotalRemaining ?? 0) + auth.AuthTotalRemaining;
                            }

                            //mapped docs data
                            auth.AuthorizationDocuments = new List<FOX_TBL_AUTH_DOC>();
                            var authDocs = _AuthorizationDocumentRepository.GetMany(e => e.AUTH_ID == auth.AUTH_ID && !e.DELETED);
                            if (authDocs != null && authDocs.Count > 0)
                            {
                                auth.AuthorizationDocuments = authDocs;
                            }

                            //comments
                            auth.AuthComments = _NotesRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.AUTH_ID == auth.AUTH_PARENT_ID)?.NOTES;

                            if (auth.BILLING_PROVIDER_ID != null)
                            {
                                var providerDetail = _commonService.GetProvider(auth.BILLING_PROVIDER_ID.Value, profile);
                                if (providerDetail != null)
                                {
                                    auth.BILLING_PROVIDER_NAME = providerDetail.LAST_NAME + ", " + providerDetail.FIRST_NAME + (string.IsNullOrEmpty(providerDetail.INDIVIDUAL_NPI) ? "" : " | NPI: " + providerDetail.INDIVIDUAL_NPI);
                                }
                            }

                            patient_Ins_Auth_Details.PatientAuthorizationList.Add(auth);

                            //get Auth comments
                            //ins.AuthComments = _NotesRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.Parent_Patient_insurance_Id == ins.Patient_Insurance_Id)?.NOTES;
                            calculateTotal = false;
                            calculateTotalUsed = false;
                            calculateTotalRemaining = false;
                        }
                    }
                }

                //Fetch Appointment Types
                patient_Ins_Auth_Details.AppointmentTypes = new List<FOX_TBL_APPT_TYPE>();
                var appointmentTypes = _AppointmentTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);
                if (appointmentTypes != null && appointmentTypes.Count > 0)
                {
                    patient_Ins_Auth_Details.AppointmentTypes = appointmentTypes;
                }

                //Fetch Appointment Types
                patient_Ins_Auth_Details.AuthorizationStatuses = new List<FOX_TBL_AUTH_STATUS>();
                var authStatuses = _AuthorizationStatusRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && (t.IS_ACTIVE ?? true) && !t.DELETED);
                if (authStatuses != null && authStatuses.Count > 0)
                {
                    patient_Ins_Auth_Details.AuthorizationStatuses = authStatuses;
                }

                //fetch cases for dropdown
                patient_Ins_Auth_Details.PatientCasesList = GetPatientCasesForDD(patient_Account);

            }
            return patient_Ins_Auth_Details;
        }

        public bool SaveAuthDetails(PatientInsuranceAuthDetails details, UserProfile profile)
        {
            if (!string.IsNullOrEmpty(details.Patient_Account_Str))
            {
                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                var patAccount = long.Parse(details.Patient_Account_Str);
                interfaceSynch.PATIENT_ACCOUNT = patAccount;
                //Insurance, Subscriber & Eligibility
                if (details.AuthToCreateUpdate != null)
                {
                    //Dates need to be set in any case, so setting them first
                    //EFFECTIVE_FROM
                    if (!string.IsNullOrEmpty(details.AuthToCreateUpdate.EFFECTIVE_FROM_IN_STR)) details.AuthToCreateUpdate.EFFECTIVE_FROM = Convert.ToDateTime(details.AuthToCreateUpdate.EFFECTIVE_FROM_IN_STR);
                    //EFFECTIVE_TO
                    if (!string.IsNullOrEmpty(details.AuthToCreateUpdate.EFFECTIVE_TO_IN_STR)) details.AuthToCreateUpdate.EFFECTIVE_TO = Convert.ToDateTime(details.AuthToCreateUpdate.EFFECTIVE_TO_IN_STR);
                    //RECORD_DATE
                    if (!string.IsNullOrEmpty(details.AuthToCreateUpdate.RECORD_DATE_IN_STR)) details.AuthToCreateUpdate.RECORD_DATE = Convert.ToDateTime(details.AuthToCreateUpdate.RECORD_DATE_IN_STR);
                    //RECORD_TIME
                    if (!string.IsNullOrEmpty(details.AuthToCreateUpdate.RECORD_TIME_IN_STR))
                    {
                        var timestr = details.AuthToCreateUpdate.RECORD_TIME_IN_STR.Split(' ')[0];
                        details.AuthToCreateUpdate.RECORD_TIME = DateTime.ParseExact(timestr, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                    }
                    //REQUESTED_ON
                    if (!string.IsNullOrEmpty(details.AuthToCreateUpdate.REQUESTED_ON_IN_STR)) details.AuthToCreateUpdate.REQUESTED_ON = Convert.ToDateTime(details.AuthToCreateUpdate.REQUESTED_ON_IN_STR);
                    //RECEIVED_ON
                    if (!string.IsNullOrEmpty(details.AuthToCreateUpdate.RECEIVED_ON_IN_STR)) details.AuthToCreateUpdate.RECEIVED_ON = Convert.ToDateTime(details.AuthToCreateUpdate.RECEIVED_ON_IN_STR);

                    var dbPatientAuth = _PatientAuthorizationRepository.GetByID(details.AuthToCreateUpdate.AUTH_ID);

                    if (dbPatientAuth == null)
                    {
                        details.AuthToCreateUpdate.AUTH_ID = Helper.getMaximumId("FOX_REHAB_AUTH_ID");
                        if (details.AuthToCreateUpdate.IS_SPLIT_AUTH)
                        {
                            var newUniqueId = "";
                            var parentUniqueId = _PatientAuthorizationRepository.GetMany(e => e.AUTH_PARENT_ID == details.AuthToCreateUpdate.AUTH_PARENT_ID).LastOrDefault().AUTH_UNIQUE_ID;

                            if (parentUniqueId.Contains("_"))
                            {
                                var oldCounter = parentUniqueId.Split('_')[1];
                                if (!string.IsNullOrEmpty(oldCounter))
                                {
                                    var newCounter = Convert.ToInt16(oldCounter) + 1;
                                    newUniqueId = details.AuthToCreateUpdate.AUTH_PARENT_ID + "_" + newCounter;
                                }
                            }
                            else
                            {
                                newUniqueId = parentUniqueId + "_0";
                            }

                            details.AuthToCreateUpdate.AUTH_UNIQUE_ID = newUniqueId;
                        }
                        else
                        {
                            details.AuthToCreateUpdate.AUTH_UNIQUE_ID = details.AuthToCreateUpdate.AUTH_ID.ToString();
                            details.AuthToCreateUpdate.AUTH_PARENT_ID = details.AuthToCreateUpdate.AUTH_ID;
                        }

                        details.AuthToCreateUpdate.CREATED_BY = details.AuthToCreateUpdate.MODIFIED_BY = profile.UserName;
                        details.AuthToCreateUpdate.CREATED_DATE = details.AuthToCreateUpdate.MODIFIED_DATE = Helper.GetCurrentDate();
                        details.AuthToCreateUpdate.PRACTICE_CODE = profile.PracticeCode;
                        details.AuthToCreateUpdate.VERIFIED_BY = profile.UserName;
                        details.AuthToCreateUpdate.DELETED = false;
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //InsertInterfaceTeamData(interfaceSynch, profile);
                        _PatientAuthorizationRepository.Insert(details.AuthToCreateUpdate);
                        //_PatientContext.SaveChanges();
                    }
                    else //Update
                    {
                        dbPatientAuth.RECORD_DATE = details.AuthToCreateUpdate.RECORD_DATE;
                        dbPatientAuth.RECORD_TIME = details.AuthToCreateUpdate.RECORD_TIME;
                        dbPatientAuth.REQUESTED_ON = details.AuthToCreateUpdate.REQUESTED_ON;
                        dbPatientAuth.RECEIVED_ON = details.AuthToCreateUpdate.RECEIVED_ON;
                        dbPatientAuth.REQUESTED_VISITS = details.AuthToCreateUpdate.REQUESTED_VISITS;
                        dbPatientAuth.EFFECTIVE_FROM = details.AuthToCreateUpdate.EFFECTIVE_FROM;
                        dbPatientAuth.EFFECTIVE_TO = details.AuthToCreateUpdate.EFFECTIVE_TO;
                        dbPatientAuth.AUTH_STATUS_ID = details.AuthToCreateUpdate.AUTH_STATUS_ID;
                        dbPatientAuth.CASE_ID = details.AuthToCreateUpdate.CASE_ID;
                        dbPatientAuth.BILLING_PROVIDER_ID = details.AuthToCreateUpdate.BILLING_PROVIDER_ID;
                        dbPatientAuth.AUTH_NUMBER = details.AuthToCreateUpdate.AUTH_NUMBER;

                        dbPatientAuth.REFERRED_BY_ID = details.AuthToCreateUpdate.REFERRED_BY_ID;
                        dbPatientAuth.VERIFIED_BY = profile.UserName;
                        dbPatientAuth.LOC_ID = details.AuthToCreateUpdate.LOC_ID;
                        dbPatientAuth.MULT_VALUE = details.AuthToCreateUpdate.MULT_VALUE;
                        dbPatientAuth.MULT_VALUE_TYPE_ID = details.AuthToCreateUpdate.MULT_VALUE_TYPE_ID;
                        dbPatientAuth.MULT_USED = details.AuthToCreateUpdate.MULT_USED;
                        dbPatientAuth.MULT_REMAINING = details.AuthToCreateUpdate.MULT_REMAINING;
                        dbPatientAuth.IS_AS_ONE = details.AuthToCreateUpdate.IS_AS_ONE;
                        dbPatientAuth.TOTAL = details.AuthToCreateUpdate.TOTAL;
                        dbPatientAuth.IsSimple = details.AuthToCreateUpdate.IsSimple;
                        dbPatientAuth.BillCode = details.AuthToCreateUpdate.BillCode;
                        dbPatientAuth.SimpleAllowded = details.AuthToCreateUpdate.SimpleAllowded;
                        dbPatientAuth.SimpleUsed = details.AuthToCreateUpdate.SimpleUsed;

                        dbPatientAuth.MODIFIED_BY = profile.UserName;
                        dbPatientAuth.MODIFIED_DATE = Helper.GetCurrentDate();
                        //if (details.is_Change)
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //InsertInterfaceTeamData(interfaceSynch, profile);
                        _PatientAuthorizationRepository.Update(dbPatientAuth);
                        details.is_Change = false;
                        //_PatientContext.SaveChanges();
                    }
                    _PatientContext.SaveChanges();

                    SaveAuthAppointments(details.AuthToCreateUpdate.AuthAppointmentIds, details.AuthToCreateUpdate.AUTH_ID, profile);
                    SaveAuthCharges(details.AuthToCreateUpdate.AuthorizationChargesList, details.AuthToCreateUpdate.AUTH_ID, details.AuthToCreateUpdate.IsSimple, profile);
                    SaveAuthComments(details, profile);
                    return true;
                }
            }
            return false;
        }

        public void SaveAuthAppointments(List<int> AuthAppointmentIds, long auth_Id, UserProfile profile)
        {
            //Delete existing mappings (if any)
            var existingappts = _AuthorizationAppointmentRepository.GetMany(e => e.AUTH_ID == auth_Id);
            if (existingappts != null && existingappts.Count > 0)
            {
                foreach (var oldAppt in existingappts)
                {
                    _AuthorizationAppointmentRepository.Delete(oldAppt);
                }
                _PatientContext.SaveChanges();
            }


            //create new mappings (if any)
            if (AuthAppointmentIds != null && AuthAppointmentIds.Count() > 0)
            {
                foreach (var newApptId in AuthAppointmentIds)
                {
                    var authApptObj = new FOX_TBL_AUTH_APPT_TYPE();
                    authApptObj.AUTH_APPT_TYPE_ID = Helper.getMaximumId("FOX_REHAB_AUTH_APPT_TYPE_ID");
                    authApptObj.PRACTICE_CODE = profile.PracticeCode;
                    authApptObj.AUTH_ID = auth_Id;
                    authApptObj.APPT_TYPE_ID = newApptId;

                    authApptObj.CREATED_BY = authApptObj.MODIFIED_BY = profile.UserName;
                    authApptObj.CREATED_DATE = authApptObj.MODIFIED_DATE = Helper.GetCurrentDate();
                    authApptObj.DELETED = false;

                    _AuthorizationAppointmentRepository.Insert(authApptObj);
                }
                _PatientContext.SaveChanges();
            }
        }

        public void SaveAuthCharges(List<FOX_TBL_AUTH_CHARGES> authorizationChargesList, long auth_Id, bool? isSimpleAuth, UserProfile profile)
        {
            if (authorizationChargesList != null && authorizationChargesList.Count() > 0)
            {
                bool isSimpleAuthorization = isSimpleAuth.HasValue ? isSimpleAuth.Value : false;
                foreach (var charge in authorizationChargesList)
                {
                    if (!isSimpleAuthorization)
                    {
                        var dbCharge = _AuthorizationChargesRepository.GetByID(charge.AUTH_CHARGES_ID);

                        if (dbCharge == null)
                        {
                            if (ChargeObjHasData(charge))
                            {
                                charge.AUTH_CHARGES_ID = Helper.getMaximumId("FOX_REHAB_AUTH_CHARGES_ID");
                                charge.AUTH_ID = auth_Id;
                                charge.CHARGES = charge.CHARGES;
                                charge.PRACTICE_CODE = profile.PracticeCode;
                                charge.CREATED_BY = charge.MODIFIED_BY = profile.UserName;
                                charge.CREATED_DATE = charge.MODIFIED_DATE = Helper.GetCurrentDate();
                                charge.DELETED = false;
                                _AuthorizationChargesRepository.Insert(charge);
                                _PatientContext.SaveChanges();
                            }
                        }
                        else
                        {
                            if (ChargeObjHasData(charge))
                            {
                                //dbCharge.AUTH_ID = auth_Id;
                                dbCharge.CPT_RANGE_FROM_CODE = charge.CPT_RANGE_FROM_CODE;
                                dbCharge.CPT_RANGE_TO_CODE = charge.CPT_RANGE_TO_CODE;
                                dbCharge.IS_EXEMPT = charge.IS_EXEMPT;
                                //dbCharge.CHARGES = charge.CHARGES;
                                dbCharge.DIAGNOSIS_CODE = charge.DIAGNOSIS_CODE;
                                dbCharge.VALUE = charge.VALUE;
                                dbCharge.VALUE_TYPE_ID = charge.VALUE_TYPE_ID;
                                dbCharge.USED = charge.USED;
                                dbCharge.MODIFIED_BY = charge.MODIFIED_BY = profile.UserName;
                                dbCharge.MODIFIED_DATE = charge.MODIFIED_DATE = Helper.GetCurrentDate();
                                dbCharge.CHARGES = charge.CHARGES;
                                _AuthorizationChargesRepository.Update(dbCharge);
                                _PatientContext.SaveChanges();
                            }
                            else
                            {
                                dbCharge.MODIFIED_DATE = Helper.GetCurrentDate();
                                dbCharge.MODIFIED_BY = profile.UserName;
                                dbCharge.DELETED = true;
                                _AuthorizationChargesRepository.Update(dbCharge);
                                _PatientContext.SaveChanges();
                            }
                        }
                    }
                    else //if simple authorization than delete its enteries
                    {
                        var dbCharge = _AuthorizationChargesRepository.GetByID(charge.AUTH_CHARGES_ID);
                        if (dbCharge != null)
                        {
                            dbCharge.CPT_RANGE_FROM_CODE = null;
                            dbCharge.CPT_RANGE_TO_CODE = null;
                            dbCharge.IS_EXEMPT = null;
                            //dbCharge.CHARGES = charge.CHARGES;
                            dbCharge.DIAGNOSIS_CODE = null;
                            dbCharge.VALUE = null;
                            dbCharge.VALUE_TYPE_ID = null;
                            dbCharge.USED = null;
                            charge.MODIFIED_BY = null;
                            charge.MODIFIED_DATE = Helper.GetCurrentDate();
                            dbCharge.CHARGES = null;
                            _AuthorizationChargesRepository.Update(dbCharge);
                            _PatientContext.SaveChanges();
                        }
                    }
                }
            }
        }

        public void SaveAuthComments(PatientInsuranceAuthDetails details, UserProfile profile)
        {
            //var parentPattInsuranceid = details.AuthToCreateUpdate.Parent_Patient_insurance_Id;
            var authId = details.AuthToCreateUpdate.AUTH_PARENT_ID;
            //var comments = details.Current_Patient_Insurances.Find(e => e.Parent_Patient_insurance_Id == parentPattInsuranceid)?.AuthComments;
            var comments = details.AuthToCreateUpdate.AuthComments ?? "";

            if (!string.IsNullOrEmpty(comments))
            {
                var commentsObj = new FOX_TBL_NOTES();
                var notesTypeObj = _NotesTypeRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.NAME.ToLower().Contains("auth"));
                //var dbComments = _NotesRepository.GetFirst(t => t.Parent_Patient_insurance_Id == (parentPattInsuranceid ?? 0));
                var dbComments = _NotesRepository.GetFirst(t => t.AUTH_ID == authId);

                if (dbComments == null)
                {
                    commentsObj = new FOX_TBL_NOTES();
                    commentsObj.PRACTICE_CODE = profile.PracticeCode;
                    commentsObj.NOTES_ID = Helper.getMaximumId("FOX_TBL_NOTES_ID");
                    commentsObj.CREATED_BY = commentsObj.MODIFIED_BY = profile.UserName;
                    commentsObj.CREATED_DATE = commentsObj.MODIFIED_DATE = DateTime.Now;
                    commentsObj.DELETED = false;
                }
                else
                {
                    commentsObj = dbComments;
                    commentsObj.MODIFIED_BY = profile.UserName;
                    commentsObj.MODIFIED_DATE = DateTime.Now;
                }
                commentsObj.NOTES_TYPE_ID = notesTypeObj?.NOTES_TYPE_ID;
                //commentsObj.Parent_Patient_insurance_Id = parentPattInsuranceid;
                commentsObj.AUTH_ID = authId;
                commentsObj.NOTES = comments;

                if (dbComments == null)
                {
                    _NotesRepository.Insert(commentsObj);
                }
                else
                {
                    _NotesRepository.Update(commentsObj);
                }

                _PatientContext.SaveChanges();
            }
        }

        public bool ChargeObjHasData(FOX_TBL_AUTH_CHARGES chargeDetails)
        {
            if (!string.IsNullOrEmpty(chargeDetails.CPT_RANGE_FROM_CODE)
                || !string.IsNullOrEmpty(chargeDetails.CPT_RANGE_TO_CODE)
                || !string.IsNullOrEmpty(chargeDetails.CHARGES)
                || (chargeDetails.IS_EXEMPT ?? false)
                || !string.IsNullOrEmpty(chargeDetails.DIAGNOSIS_CODE)
                || chargeDetails.VALUE.HasValue && chargeDetails.VALUE > 0
                || (chargeDetails.USED.HasValue && chargeDetails.USED.Value != 0)
                || (chargeDetails.USED.HasValue && chargeDetails.USED.Value != 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<MedicareLimitHistory> GetMedicareLimitHistory(MedicareLimitHistorySearchReq req, UserProfile profile)
        {
            List<MedicareLimitHistory> mLimitHistory = new List<MedicareLimitHistory>();

            if (!string.IsNullOrEmpty(req.Patient_Account) && req.MedicareLimitTypeId != null)
            {
                var patient_Acc = long.Parse(req.Patient_Account);
                var lim_Type_Id = req.MedicareLimitTypeId;
                var historyList = _MedicareLimitRepository.GetMany(e => e.Patient_Account == patient_Acc && e.MEDICARE_LIMIT_TYPE_ID == lim_Type_Id && e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED);
                if (historyList != null)
                {
                    var caseNo = "";


                    foreach (var historyRecord in historyList)
                    {
                        var history = new MedicareLimitHistory();
                        history.Owner = historyRecord.CREATED_BY ?? "";
                        if (historyRecord.CASE_ID != null && historyRecord.CASE_ID != 0)
                        {
                            var caseInfo = _vwPatientCaseRepository.GetByID(historyRecord.CASE_ID);
                            if (caseInfo != null)
                                caseNo = caseInfo.CASE_NO ?? "";
                        }

                        history.CaseNo = caseNo;
                        history.Effective_From = historyRecord.EFFECTIVE_DATE;
                        history.Effective_To = historyRecord.END_DATE;
                        mLimitHistory.Add(history);
                    }
                }
            }
            return mLimitHistory;
        }

        public splitauthorization getsplitauthorization(long parent_id, UserProfile profile)
        {
            var commentsObjs = new FOX_TBL_NOTES();
            splitauthorization patient_Ins_Auth_Details = new splitauthorization();
            Regex regex = new Regex(@"^[-+]?\d+\.?\d*$", RegexOptions.Compiled);
            //Fetch Authorization List Details
            patient_Ins_Auth_Details.splitauthorizationList = new List<FOX_TBL_AUTH>();
            var authorizations = _PatientAuthorizationRepository.GetMany(x => x.AUTH_PARENT_ID == parent_id && !x.DELETED);

            patient_Ins_Auth_Details.splitauthorizationList = authorizations;
            //Retrieve Minimum Date
            var MinDate = (from d in authorizations select d.EFFECTIVE_FROM).FirstOrDefault();
            if (MinDate != null)
            {
                patient_Ins_Auth_Details.StartDate = MinDate.Value;
            }

            //Retrieve Maximum Date
            var MaxDate = (from d in authorizations orderby d.CREATED_DATE descending select d.EFFECTIVE_TO).FirstOrDefault();
            if (MaxDate != null)
            {
                patient_Ins_Auth_Details.EndDate = MaxDate.Value;
            }

            decimal? total = 0;

            if (authorizations != null && authorizations.Count > 0)
            {
                foreach (var auth in authorizations)
                {
                    auth.AuthorizationChargesList = new List<FOX_TBL_AUTH_CHARGES>();
                    for (int i = 0; i < 6; i++)
                    {
                        auth.AuthorizationChargesList.Add(new FOX_TBL_AUTH_CHARGES());
                    }
                    if (auth != null && auth.MULT_VALUE != null)
                    {
                        if (auth.AuthTotalUsed == null)
                        {
                            auth.AuthTotalUsed = 0;
                        }
                        if (auth.AuthTotalRemaining == null)
                        {
                            auth.AuthTotalRemaining = 0;
                        }
                        auth.TOTAL = auth.MULT_VALUE;
                        auth.AuthTotalUsed = auth.MULT_USED ?? 0;
                        auth.AuthTotalRemaining = auth.MULT_REMAINING ?? 0;

                        if (auth.MULT_REMAINING == null)
                        {
                            if (auth.MULT_USED == null)
                            {
                                auth.MULT_USED = 0;

                            }
                            auth.MULT_REMAINING = auth.MULT_VALUE - auth.MULT_USED;
                            auth.AuthTotalRemaining = auth.MULT_REMAINING;
                        }
                        else if (auth.MULT_USED == null)
                        {
                            auth.MULT_USED = 0;
                            auth.MULT_USED = auth.MULT_VALUE - auth.MULT_REMAINING;

                        }
                        if (auth.TOTAL != null)
                        {
                            if (patient_Ins_Auth_Details.GRANDTOTAL == null)
                            {
                                patient_Ins_Auth_Details.GRANDTOTAL = 0;
                            }
                            patient_Ins_Auth_Details.GRANDTOTAL = (patient_Ins_Auth_Details.GRANDTOTAL ?? 0) + auth.TOTAL;
                        }
                        if (auth.AuthTotalUsed != null)
                        {
                            patient_Ins_Auth_Details.AuthGrandTotalUsed = (patient_Ins_Auth_Details.AuthGrandTotalUsed ?? 0) + auth.AuthTotalUsed;
                        }

                        if (auth.AuthTotalRemaining != null)
                        {
                            patient_Ins_Auth_Details.AuthGrandTotalRemaining = (patient_Ins_Auth_Details.AuthGrandTotalRemaining ?? 0) + auth.AuthTotalRemaining;
                        }
                    }
                    else
                    {
                        var authCharges = _AuthorizationChargesRepository.GetMany(e => e.AUTH_ID == auth.AUTH_ID && !e.DELETED);
                        if (authCharges != null && authCharges.Count > 0)
                        {
                            for (int i = 0; i < authCharges.Count(); i++)
                            {
                                auth.AuthorizationChargesList[i] = authCharges[i];
                                total = total + (authCharges[i].VALUE.HasValue ? authCharges[i].VALUE : 0);
                                //if (authCharges[i].VALUE.HasValue)
                                //{

                                //    //if (authCharges[i].VALUE.Value > 0  )
                                //    //{
                                //    //    total +=   authCharges[i].VALUE;
                                //    //}
                                //}
                                if (authCharges[i].USED != null && authCharges[i].USED != 0)
                                {
                                    auth.AuthTotalUsed = authCharges[i].USED;
                                }
                                if (authCharges[i].REMAINING != null && authCharges[i].REMAINING != 0)
                                {
                                    auth.AuthTotalRemaining = authCharges[i].REMAINING;
                                }
                                //authCharges[i].REMAINING = total - authCharges[i].USED ?? 0;
                                ////if (authCharges[i].REMAINING != null && authCharges[i].REMAINING != 0)
                                //{
                                //    //auth.AuthTotalRemaining = authCharges[i].REMAINING;
                                //    auth.AuthTotalRemaining = total - auth.AuthTotalUsed;
                                //}
                                if (auth.TOTAL != null)
                                {
                                    patient_Ins_Auth_Details.GRANDTOTAL = (patient_Ins_Auth_Details.GRANDTOTAL ?? 0) + auth.TOTAL;
                                }
                                //if (auth.TOTAL != null)
                                //{
                                //    if (patient_Ins_Auth_Details.GRANDTOTAL == null)
                                //    {
                                //        patient_Ins_Auth_Details.GRANDTOTAL = 0;
                                //    }
                                //    patient_Ins_Auth_Details.GRANDTOTAL = (patient_Ins_Auth_Details.GRANDTOTAL ?? 0) + auth.TOTAL;
                                //}
                                if (auth.AuthTotalUsed != null)
                                {
                                    patient_Ins_Auth_Details.AuthGrandTotalUsed = (patient_Ins_Auth_Details.AuthGrandTotalUsed ?? 0) + auth.AuthTotalUsed;
                                }
                                if (auth.AuthTotalRemaining != null)
                                {
                                    patient_Ins_Auth_Details.AuthGrandTotalRemaining = (patient_Ins_Auth_Details.AuthGrandTotalRemaining ?? 0) + auth.AuthTotalRemaining;
                                }

                            }
                            //if (auth.AuthTotalUsed == null)
                            //{
                            //    auth.AuthTotalUsed = 0;
                            //}
                            //if (auth.AuthTotalRemaining == null)
                            //{
                            //    auth.AuthTotalRemaining = 0;
                            //}
                            auth.TOTAL = total;
                        }
                    }
                }
            }
            return patient_Ins_Auth_Details;
        }

        private long? ActivePOSExists(long patient_account)
        {
            long? loc_Id = null;
            var posData = _PatientPOSLocationRepository.GetMany(e => e.Patient_Account == patient_account && (e.Deleted ?? false) == false && ((e.Effective_To.HasValue && DbFunctions.TruncateTime(e.Effective_To.Value) > DbFunctions.TruncateTime(DateTime.Now)) || ((e.Is_Void ?? false) == false))).ToList();
            if (posData.Count > 0)
            {
                foreach (var pos in posData)
                {
                    var loc = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == pos.Loc_ID && !e.DELETED && e.IS_ACTIVE);
                    if (loc != null && loc.NAME.ToLower() != "private home")
                    {
                        loc_Id = loc.LOC_ID;
                        break;
                    }
                }
            }
            return loc_Id;
        }

        private void UpdateFinancialGuarantorInPatientFromPOS(PatientPOSLocation obj, UserProfile profile)
        {
            var patient = _PatientRepository.GetFirst(e => e.Patient_Account == obj.Patient_Account && (e.DELETED ?? false) == false);
            if (patient != null && patient.Financial_Guarantor.HasValue)
            {
                long? active_loc_Id = ActivePOSExists(obj.Patient_Account);
                if (active_loc_Id.HasValue && active_loc_Id.Value != 0)
                {
                    var loc = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == active_loc_Id.Value && !e.DELETED && e.IS_ACTIVE);
                    if (loc != null)
                    {
                        PatientPOSLocation posdata = new PatientPOSLocation();
                        posdata.Patient_Account = obj.Patient_Account;
                        posdata.Loc_ID = loc.LOC_ID;
                        posdata.Patient_POS_Details = loc;
                        SaveGuarantorForPatientFromPOS(posdata, profile);
                    }
                    else
                    {
                        var con = ActiveFinancialContactExists(patient.Patient_Account);
                        if (con != null)
                        {
                            SaveGuarantorForPatientFromContact(con, profile);
                        }
                        else
                        {
                            patient.Financial_Guarantor = null;
                            patient.Address_To_Guarantor = false;
                            patient.ModifiedBy = profile.UserName;
                            patient.Modified_Date = Helper.GetCurrentDate();
                            _PatientRepository.Update(patient);
                            _PatientRepository.Save();
                        }
                    }
                }
                else
                {
                    var con = ActiveFinancialContactExists(patient.Patient_Account);
                    if (con != null)
                    {
                        SaveGuarantorForPatientFromContact(con, profile);
                    }
                    else
                    {
                        patient.Financial_Guarantor = null;
                        patient.Address_To_Guarantor = false;
                        patient.ModifiedBy = profile.UserName;
                        patient.Modified_Date = Helper.GetCurrentDate();
                        _PatientRepository.Update(patient);
                        _PatientRepository.Save();
                    }
                }
            }
        }

        private bool ContactIsSameAsFinancialGuarantor(PatientContact con, Patient patient)
        {
            var fg = _SubscriberRepository.GetFirst(e => e.GUARANTOR_CODE == patient.Financial_Guarantor && (e.Deleted ?? false) == false);
            if (fg != null && (!string.IsNullOrWhiteSpace(fg.GUARANT_FNAME) && fg.GUARANT_FNAME.Equals(con.First_Name)) && fg.GUARANT_LNAME.Equals(con.Last_Name) && fg.GUARANT_ADDRESS.Equals(con.Address) && fg.GUARANT_CITY.Equals(con.City) && fg.GUARANT_STATE == con.State)
            {
                return true;
            }
            return false;
        }

        private void UpdateFinancialGuarantorInPatientFromContact(PatientContact dbcon, PatientContact updatedcon, UserProfile profile)
        {
            if (dbcon != null && updatedcon != null)
            {
                var patient = _PatientRepository.GetFirst(e => e.Patient_Account == dbcon.Patient_Account && (e.DELETED ?? false) == false && e.Financial_Guarantor.HasValue && e.Financial_Guarantor.Value != 0);
                if (patient != null)
                {
                    //if (dbcon.STATEMENT_ADDRESS_MARKED && (((dbcon.Flag_Financially_Responsible_Party ?? false) == true && (updatedcon.Flag_Financially_Responsible_Party ?? false) == false)
                    //     || ((dbcon.Flag_Power_Of_Attorney_Financial ?? false) == true && (updatedcon.Flag_Power_Of_Attorney_Financial ?? false) == false)))
                    if (dbcon.STATEMENT_ADDRESS_MARKED && ((dbcon.Flag_Financially_Responsible_Party ?? false) == true && (updatedcon.Flag_Financially_Responsible_Party ?? false) == false))

                    {
                        if (ContactIsSameAsFinancialGuarantor(updatedcon, patient))
                        {
                            var con = ActiveFinancialContactExists(patient.Patient_Account);
                            if (con != null && con.Contact_ID != updatedcon.Contact_ID)
                            {
                                SaveGuarantorForPatientFromContact(con, profile);
                            }
                            else
                            {
                                long? active_loc_Id = ActivePOSExists(patient.Patient_Account);
                                if (active_loc_Id.HasValue && active_loc_Id.Value != 0)
                                {
                                    var loc = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == active_loc_Id.Value && !e.DELETED && e.IS_ACTIVE);
                                    if (loc != null)
                                    {
                                        PatientPOSLocation posdata = new PatientPOSLocation();
                                        posdata.Patient_Account = patient.Patient_Account;
                                        posdata.Loc_ID = loc.LOC_ID;
                                        posdata.Patient_POS_Details = loc;
                                        SaveGuarantorForPatientFromPOS(posdata, profile);
                                    }
                                    else
                                    {
                                        patient.Financial_Guarantor = null;
                                        patient.Address_To_Guarantor = false;
                                        patient.ModifiedBy = profile.UserName;
                                        patient.Modified_Date = Helper.GetCurrentDate();
                                        _PatientRepository.Update(patient);
                                        _PatientRepository.Save();
                                    }
                                }
                                else
                                {
                                    patient.Financial_Guarantor = null;
                                    patient.Address_To_Guarantor = false;
                                    patient.ModifiedBy = profile.UserName;
                                    patient.Modified_Date = Helper.GetCurrentDate();
                                    _PatientRepository.Update(patient);
                                    _PatientRepository.Save();
                                }
                            }
                        }
                    }
                }
            }
        }

        private PatientContact ActiveFinancialContactExists(long pat_account)
        {
            var con = _PatientContactRepository.GetFirst(e => e.Patient_Account == pat_account && (e.Deleted ?? false) == false && e.STATEMENT_ADDRESS_MARKED
                && ((e.Flag_Financially_Responsible_Party ?? false) || (e.Flag_Financially_Responsible_Party ?? false)));
            return con;
        }

        public ResponseModel DeletePatPos(PatientPOSLocation obj, UserProfile profile)
        {
            //try
            //{

            var PatPosData = _PatientPOSLocationRepository.GetFirst(e => e.Patient_Account == obj.Patient_Account && !(e.Deleted ?? false) && e.Patient_POS_ID == obj.Patient_POS_ID);

            if (PatPosData != null)
            {
                PatPosData.Modified_By = profile.UserName;
                PatPosData.Modified_Date = Helper.GetCurrentDate();
                PatPosData.Deleted = obj.Deleted;
                _PatientPOSLocationRepository.Update(PatPosData);
                _PatientPOSLocationRepository.Save();

                UpdateFinancialGuarantorInPatientFromPOS(obj, profile);

                CheckAndUpdatePRSubscriber(obj.Patient_Account_Str, profile);

                //Update patient home home
                if (obj != null && obj.Patient_POS_Details != null && obj.Patient_Home_Phone == obj.Patient_POS_Details.Phone)
                {
                    var pat_Rec = _PatientRepository.GetFirst(p => p.Patient_Account == obj.Patient_Account);
                    if (pat_Rec != null)
                    {
                        pat_Rec.Modified_By = profile.UserName;
                        pat_Rec.Modified_Date = Helper.GetCurrentDate();
                        pat_Rec.Home_Phone = "";
                        _PatientRepository.Update(pat_Rec);
                        _PatientRepository.Save();
                    }
                }
                else
                {
                    var pat_Rec = _PatientRepository.GetFirst(p => p.Patient_Account == obj.Patient_Account);
                    if (pat_Rec != null)
                    {
                        pat_Rec.Modified_By = profile.UserName;
                        pat_Rec.Modified_Date = Helper.GetCurrentDate();
                        pat_Rec.Home_Phone = obj.Patient_Home_Phone;
                        _PatientRepository.Update(pat_Rec);
                        _PatientRepository.Save();
                    }
                }
            }
            ResponseModel resp = new ResponseModel()
            {
                ErrorMessage = "",
                Message = "Deleted",
                Success = true
            };
            return resp;
            //}
            //catch (Exception ex)
            //{
            //    ResponseModel resp = new ResponseModel()
            //    {
            //        ErrorMessage = "",
            //        Message = "Not Deleted",
            //        Success = false
            //    };
            //    return resp;
            //    //Log Exception
            //}
        }

        public List<FinancialClass> GetFinancialClassDDValues(string practiceCode)
        {
            if (practiceCode == "0")
            {
                return _financialClassRepository.GetMany(e => !e.DELETED);
            }
            else
            {
                var PracticeCode = Convert.ToInt64(practiceCode);
                return _financialClassRepository.GetMany(e => e.PRACTICE_CODE == PracticeCode && !e.DELETED);
            }
        }

        public List<AdvanceInsuranceSearch> GetInsurancePayersForAdvanceSearch(AdvanceInsuranceSearch searchReq)
        {
            var _practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = searchReq.Practice_Code };
            var _insuranceName = Helper.getDBNullOrValue("INSURANCE_DESCRIPTION", searchReq.InsPayer_Description);
            var _financial_Class_ID = searchReq.FINANCIAL_CLASS_ID.HasValue ? new SqlParameter("FINANCIAL_CLASS_ID", SqlDbType.Int) { Value = searchReq.FINANCIAL_CLASS_ID }
                : new SqlParameter("FINANCIAL_CLASS_ID", SqlDbType.Int) { Value = DBNull.Value };
            var _insuranceaddress = Helper.getDBNullOrValue("INSURANCE_ADDRESS", searchReq.Insurance_Address);
            var _insuranceZip = Helper.getDBNullOrValue("INSURANCE_ZIP", searchReq.Insurance_Zip);
            var _insuranceCity = Helper.getDBNullOrValue("INSURANCE_CITY", searchReq.Insurance_City);
            var _insuranceState = Helper.getDBNullOrValue("INSURANCE_STATE", searchReq.Insurance_State);
            var _searchString = new SqlParameter("SEARCH_STRING", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.SearchString) ? "" : searchReq.SearchString };
            var _currentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = searchReq.CurrentPage };
            var _recordPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = searchReq.RecordPerPage };

            var result = SpRepository<AdvanceInsuranceSearch>.
                GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADVANCE_SEARCH_INSURANCE 
                        @PRACTICE_CODE,@INSURANCE_DESCRIPTION,@FINANCIAL_CLASS_ID,@INSURANCE_ADDRESS,@INSURANCE_ZIP,@INSURANCE_CITY,@INSURANCE_STATE,@SEARCH_STRING,@CURRENT_PAGE,@RECORD_PER_PAGE",
                    _practiceCode, _insuranceName, _financial_Class_ID, _insuranceaddress, _insuranceZip, _insuranceCity, _insuranceState, _searchString, _currentPage, _recordPerPage);
            return result;
        }

        public List<AdvancePatientSearch> GetPatientsForAdvanceSearch(AdvancePatientSearch searchReq, UserProfile profile)
        {
            if (!string.IsNullOrEmpty(searchReq.Date_Of_Birth_In_String))
            {
                searchReq.Date_Of_Birth = Convert.ToDateTime(searchReq.Date_Of_Birth_In_String);
            }

            if (!string.IsNullOrEmpty(searchReq.Created_Date_Str))
            {
                searchReq.Created_Date = Convert.ToDateTime(searchReq.Created_Date_Str);
            }

            var _practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var _firstName = Helper.getDBNullOrValue("First_Name", searchReq.First_Name);
            var _middleName = Helper.getDBNullOrValue("Middle_Name", searchReq.MIDDLE_NAME);
            var _lastName = Helper.getDBNullOrValue("Last_Name", searchReq.Last_Name);
            var _MRN = Helper.getDBNullOrValue("CHART_ID", searchReq.Chart_Id);
            var _SSN = Helper.getDBNullOrValue("SSN", searchReq.SSN);
            var _gender = Helper.getDBNullOrValue("Gender", searchReq.Gender);
            var _dob = Helper.getDBNullOrValue("DOB", searchReq.Date_Of_Birth.HasValue ? searchReq.Date_Of_Birth.Value.ToString("MM/dd/yyyy") : "");
            var _createdDate = Helper.getDBNullOrValue("CreatedDate", searchReq.Created_Date.HasValue ? searchReq.Created_Date.Value.ToString("MM/dd/yyyy") : "");
            var _searchString = new SqlParameter("SEARCH_STRING", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(searchReq.SearchString) ? "" : searchReq.SearchString };
            var _currentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = searchReq.CurrentPage };
            var _recordPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.Int) { Value = searchReq.RecordPerPage };

            var result = SpRepository<AdvancePatientSearch>.
                GetListWithStoreProcedure(@"exec FOX_PROC_GET_ADVANCE_SEARCH_PATIENT 
                        @PRACTICE_CODE, @First_Name, @Middle_Name, @Last_Name, @CHART_ID, @SSN, @Gender, @DOB, @CreatedDate, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE",
                    _practiceCode, _firstName, _middleName, _lastName, _MRN, _SSN, _gender, _dob, _createdDate, _searchString, _currentPage, _recordPerPage);
            return result;
        }

        public Patient GetPatientDetail(long patient_Account)
        {
            return _PatientRepository.GetByID(patient_Account);
        }

        public List<SmartPatientRes> GetSmartPatient(string searchText, UserProfile profile)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(searchText) ? searchText.Trim() : searchText };
            var result = SpRepository<SmartPatientRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_PATIENTS] @PRACTICE_CODE, @SEARCHVALUE",
                parmPracticeCode, smartvalue).ToList();
            if (result.Any())
                return result;
            else
                return new List<SmartPatientRes>();
        }
        public List<SmartPatientResForTask> GetSmartPatientForTask(string searchText, UserProfile profile)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(searchText) ? searchText.Trim() : searchText };
            var result = SpRepository<SmartPatientResForTask>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_PATIENTS_For_Task] @PRACTICE_CODE, @SEARCHVALUE",
                parmPracticeCode, smartvalue).ToList();
            if (result.Count > 0)
            {
                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
                TextInfo text_info = culture_info.TextInfo;
                int i = 1;
                foreach (SmartPatientResForTask obj in result)
                {

                    obj.FIRST_NAME = string.IsNullOrEmpty(obj.FIRST_NAME) ? string.Empty : text_info.ToTitleCase(obj.FIRST_NAME);
                    obj.LAST_NAME = string.IsNullOrEmpty(obj.LAST_NAME) ? string.Empty : text_info.ToTitleCase(obj.LAST_NAME);
                    obj.ROW = i++;
                }
                return result;
            }

            else
                return new List<SmartPatientResForTask>();
        }
        public List<SmartModifiedBy> getSmartModifiedBy(string searchText, UserProfile profile)
        {
            if (!string.IsNullOrEmpty(searchText) && searchText.Contains(" - "))
            {
                searchText = searchText.Replace(searchText.Substring(searchText.IndexOf('-') - 1, searchText.Length - searchText.IndexOf("-") + 1), string.Empty);
            }
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(searchText) ? searchText.Trim() : searchText };
            var result = SpRepository<SmartModifiedBy>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_MODIFIED_BY] @PRACTICE_CODE, @SEARCHVALUE",
                 parmPracticeCode, smartvalue).ToList();
            if (result.Any())
            {
                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
                TextInfo text_info = culture_info.TextInfo;
                int i = 1;
                foreach (SmartModifiedBy obj in result)
                {

                    obj.FIRST_NAME = string.IsNullOrEmpty(obj.FIRST_NAME) ? string.Empty : text_info.ToTitleCase(obj.FIRST_NAME);
                    obj.LAST_NAME = string.IsNullOrEmpty(obj.LAST_NAME) ? string.Empty : text_info.ToTitleCase(obj.LAST_NAME);
                    obj.ROW = i++;
                }
                return result;
            }
            else
                return new List<SmartModifiedBy>();

        }

        public PatientInsuranceDetail GetDefaultPrimaryInsurance(DefaultInsuranceParameters defaultInsuranceParameters, long practiceCode)
        {
            var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", Value = practiceCode };
            var _patientAccount = Helper.getDBNullOrValue("PATIENT_ACCOUNT", defaultInsuranceParameters.patientAccount.ToString());
            var _ZIP = Helper.getDBNullOrValue("ZIP ", defaultInsuranceParameters.ZIP);
            var _state = Helper.getDBNullOrValue("STATE ", defaultInsuranceParameters.State);
            var result = SpRepository<PatientInsuranceDetail>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_DEFAULT_PRIMARY_INSURANCE @PRACTICE_CODE,@PATIENT_ACCOUNT, @ZIP, @STATE", _practiceCode, _patientAccount, _ZIP, _state);
            return result;
        }
        public SubscriberInformation GetSubscriberInfo(SubscriberInfoRequest subscriberinforequest)
        {
            //Patient Details
            SubscriberInformation ObjSubscriberInformation = new SubscriberInformation();
            ObjSubscriberInformation.patientinfo = new Patient();
            ObjSubscriberInformation.PatientAddress = new List<PatientAddress>();
            ObjSubscriberInformation.PatientContactsList = new List<PatientContact>();
            ObjSubscriberInformation.patientinfo = _PatientRepository.GetFirst(e => e.Patient_Account == subscriberinforequest.patientAccount && (e.DELETED ?? false) == false);
            //Contact Details
            ObjSubscriberInformation.PatientContactsList = _PatientContactRepository.GetMany(x => x.Patient_Account == subscriberinforequest.patientAccount && (x.Deleted ?? false) == false).ToList();
            //Patient Address
            ObjSubscriberInformation.PatientAddress = _PatientAddressRepository.GetMany(x => x.PATIENT_ACCOUNT == subscriberinforequest.patientAccount && (x.DELETED ?? false) == false).ToList();
            //Contact Type
            ObjSubscriberInformation.ContactTypeList = _ContactTypeRepository.GetMany(e => e.Practice_Code == subscriberinforequest.Practice_Code && (e.Deleted ?? false) == false).ToList();
            return ObjSubscriberInformation;
        }
        //public List<PatientInsuranceDetail> GetSmartInsurancePayers(SmartInsurancePayersReq obj, UserProfile profile)        
        public List<PatientInsuranceDetail> GetSmartInsurancePayers(SmartSearchInsuranceReq obj, UserProfile profile)
        {
            var _practice_Code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var _pat_Acc = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = string.IsNullOrWhiteSpace(obj.Patient_Account) ? 0 : long.Parse(obj.Patient_Account) };
            var _financialClassId = new SqlParameter("FINANCIAL_CLASS_ID", SqlDbType.Int) { Value = obj.FINANCIAL_CLASS_ID ?? 0 };
            var _searchVal = new SqlParameter("SEARCH_VALUE", SqlDbType.VarChar) { Value = obj.Insurance_Name };
            var _pri_sec_oth_type = new SqlParameter("PRI_SEC_OTH_TYPE", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(obj.Pri_Sec_Oth_Type) ? obj.Pri_Sec_Oth_Type : "" };

            var _parmZipCode = !string.IsNullOrWhiteSpace(obj.Zip) ? new SqlParameter("ZIP", SqlDbType.VarChar) { Value = obj.Zip } : new SqlParameter { ParameterName = "ZIP", Value = DBNull.Value };
            var _parmState = !string.IsNullOrWhiteSpace(obj.State) ? new SqlParameter("STATE", SqlDbType.VarChar) { Value = obj.State } : new SqlParameter { ParameterName = "STATE", Value = DBNull.Value };

            var result = SpRepository<PatientInsuranceDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SMART_INSURANCE_PAYERS 
                       @PRACTICE_CODE,@PATIENT_ACCOUNT,@SEARCH_VALUE,@FINANCIAL_CLASS_ID,@PRI_SEC_OTH_TYPE,@ZIP,@STATE",
                   _practice_Code, _pat_Acc, _searchVal, _financialClassId, _pri_sec_oth_type, _parmZipCode, _parmState);
            return result;
        }

        public PatientInsuranceDetail GetAutoPopulateInsurance(AutoPopulateModel obj, UserProfile profile)
        {
            var _practice_Code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var _pat_Acc = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = string.IsNullOrWhiteSpace(obj.patientAccount) ? 0 : long.Parse(obj.patientAccount) };
            var _pri_sec_oth_type = new SqlParameter("PRI_SEC_OTH_TYPE", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(obj.Pri_Sec_Oth_Type) ? obj.Pri_Sec_Oth_Type : "" };
            var _CASE_ID = new SqlParameter("CASE_ID", SqlDbType.BigInt) { Value = obj.CASE_ID };
            var IS_WELLNESS = new SqlParameter("IS_WELLNESS", SqlDbType.Bit) { Value = obj.IS_WELLNESS };
            var result = SpRepository<PatientInsuranceDetail>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PR_STATES_AND_REGIONS
                       @PRACTICE_CODE,@PATIENT_ACCOUNT,@CASE_ID,@PRI_SEC_OTH_TYPE,@IS_WELLNESS",
                   _practice_Code, _pat_Acc, _CASE_ID, _pri_sec_oth_type, IS_WELLNESS);
            return result;
        }

        public List<FacilityLocation> GetPatientPrivateHomes(string patientAccount, string stateCode, long practiceCode)
        {
            long lPatientAccount;
            if (long.TryParse(patientAccount, out lPatientAccount))
            {
                var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
                var _paramsStateCode = new SqlParameter("STATE", SqlDbType.VarChar) { Value = stateCode };
                var _paramsPatientAccount = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = lPatientAccount };
                return SpRepository<FacilityLocation>.GetListWithStoreProcedure(@"EXEC [FOX_PROC_GET_PRIVATE_HOMES_BY_STATE] @STATE, @PRACTICE_CODE, @PATIENT_ACCOUNT", _paramsStateCode, _paramsPracticeCode, _paramsPatientAccount);
            }
            else
            {
                return new List<FacilityLocation>();
            }
        }

        public FacilityLocation GetPrivateHomeFacilityByCode(string code, UserProfile profile)
        {

            var facilityOfPrivateHome = (from f in _PatientContext.FacilityLocation
                                         join ft in _PatientContext.FaclityTypes on f.FACILITY_TYPE_ID equals ft.FACILITY_TYPE_ID
                                         where ft.NAME.ToLower().Equals("private home")
                                                && f.CODE == code
                                                && f.DELETED == false
                                                && f.PRACTICE_CODE == profile.PracticeCode
                                                && ft.DELETED == false
                                                && ft.PRACTICE_CODE == profile.PracticeCode
                                         select f).FirstOrDefault();
            if (facilityOfPrivateHome == null)
            {
                var privateHomeFacilityType = _FacilityTypeRepository.GetFirst(t => t.NAME.ToLower() == "private home" && t.PRACTICE_CODE == profile.PracticeCode);
                facilityOfPrivateHome = new FacilityLocation()
                {
                    LOC_ID = Convert.ToInt64(profile.PracticeCode.ToString() + Helper.getMaximumId("LOC_ID").ToString()),
                    CODE = code,
                    NAME = "Private Home",
                    FACILITY_TYPE_ID = privateHomeFacilityType.FACILITY_TYPE_ID,
                    PRACTICE_CODE = profile.PracticeCode,
                    CREATED_BY = profile.UserName,
                    CREATED_DATE = Helper.GetCurrentDate(),
                    MODIFIED_BY = profile.UserName,
                    MODIFIED_DATE = Helper.GetCurrentDate(),
                    DELETED = false
                };
                _FacilityLocationRepository.Insert(facilityOfPrivateHome);
                _FacilityLocationRepository.Save();
            }
            return facilityOfPrivateHome;
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

        public PatientInsuranceDetail GetSuggestedMCPayer(SuggestedMCPayer suggestedMCPayer, UserProfile profile)
        {
            var _parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var _patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = DBNull.Value };
            var _parmZipCode = !string.IsNullOrWhiteSpace(suggestedMCPayer.Zip) ? new SqlParameter("ZIP", SqlDbType.VarChar) { Value = suggestedMCPayer.Zip } : new SqlParameter { ParameterName = "ZIP", Value = DBNull.Value };
            var _parmState = !string.IsNullOrWhiteSpace(suggestedMCPayer.State) ? new SqlParameter("STATE", SqlDbType.VarChar) { Value = suggestedMCPayer.State } : new SqlParameter { ParameterName = "STATE", Value = DBNull.Value };

            var result = SpRepository<PatientInsuranceDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_MC_SUGGESTED_PAYER @PRACTICE_CODE, @PATIENT_ACCOUNT, @ZIP, @STATE", _parmPracticeCode, _patientAccount, _parmZipCode, _parmState);
            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return new PatientInsuranceDetail();
            }
        }

        public List<PatientInsuranceDetail> GetPatientInsurancesInIndexInfo(string patientAccountStr, UserProfile profile)
        {
            var _parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var _patientAccount = !string.IsNullOrWhiteSpace(patientAccountStr) ? new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = long.Parse(patientAccountStr) }
                : new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = DBNull.Value };

            return SpRepository<PatientInsuranceDetail>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_INSURANCES_IN_INDEX_INFO @PRACTICE_CODE, @PATIENT_ACCOUNT", _parmPracticeCode, _patientAccount);
        }

        public List<WorkOrderDocs> GetWorkOrderDocs(string patientAccountStr, UserProfile userProfile)
        {
            var _parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = userProfile.PracticeCode };
            var _patientAccount = !string.IsNullOrWhiteSpace(patientAccountStr) ? new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = long.Parse(patientAccountStr) }
                : new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = DBNull.Value };
            var list = SpRepository<WorkOrderDocs>.GetListWithStoreProcedure(@"exec [FOX_PROC_ALL_PATIENT_DOCUMENTS]  @PATIENT_ACCOUNT, @PRACTICE_CODE", _parmPracticeCode, _patientAccount);
            return list;
        }

        public void SavePatientContactfromInsuranceSubscriber(Subscriber SubscriberDetail, string Relation, long PatientAccount, UserProfile profile)
        {
            List<PatientContact> ContactsList = new List<PatientContact>();
            //PatientContact SingleContact = new PatientContact();
            var SingleContact = (PatientContact)null;
            ContactsList = GetPatientContacts(PatientAccount);
            Dictionary<string, string> selectedrelation = new Dictionary<string, string>
                    {
                        {"B","brother"},
                        {"C","child"},
                        {"F","father"},
                        {"GF","grandfather"},
                        {"GM","grandmother"},
                        {"MO","mother"},
                        {"S","self"},
                        {"SI","sister"},
                        {"SP","spouse"},
                        {"O","other contact"},
                    };
            string rel = selectedrelation[Relation];
            if (!string.IsNullOrEmpty(rel))
            {
                if (ContactsList.Count() > 0)
                {
                    if (rel.ToLower() == "child")
                    {
                        SingleContact = ContactsList.Where(c => c.Contact_Type_Name.ToLower() == "son").FirstOrDefault();
                        if (SingleContact == null)
                            SingleContact = ContactsList.Where(c => c.Contact_Type_Name.ToLower() == "daughter").FirstOrDefault();
                    }
                    else
                    {
                        SingleContact = ContactsList.Where(c => c.Contact_Type_Name.ToLower() == rel).FirstOrDefault();
                    }
                }
                if (SingleContact == null)
                {
                    PatientContact ObjPatientContact = new PatientContact();
                    ObjPatientContact.Patient_Account = PatientAccount;
                    ObjPatientContact.First_Name = SubscriberDetail.GUARANT_FNAME;
                    ObjPatientContact.MI = SubscriberDetail.GUARANT_MI;
                    ObjPatientContact.Last_Name = SubscriberDetail.GUARANT_LNAME;
                    ObjPatientContact.Address = SubscriberDetail.GUARANT_ADDRESS;
                    ObjPatientContact.Zip = SubscriberDetail.GUARANT_ZIP;
                    ObjPatientContact.City = SubscriberDetail.GUARANT_CITY;
                    ObjPatientContact.State = SubscriberDetail.GUARANT_STATE;
                    ObjPatientContact.Home_Phone = SubscriberDetail.GUARANT_HOME_PHONE;
                    if (rel.ToLower() == "child")
                    {
                        if (!string.IsNullOrEmpty(SubscriberDetail.GUARANT_GENDER))
                        {
                            if (SubscriberDetail.GUARANT_GENDER.ToLower() == "male")
                            {
                                ObjPatientContact.Contact_Type_Id = _ContactTypeRepository.GetSingle(e => e.Practice_Code == profile.PracticeCode && e.Type_Name.ToLower() == "son").Contact_Type_ID;
                            }
                            else if (SubscriberDetail.GUARANT_GENDER.ToLower() == "female")
                            {
                                ObjPatientContact.Contact_Type_Id = _ContactTypeRepository.GetSingle(e => e.Practice_Code == profile.PracticeCode && e.Type_Name.ToLower() == "daughter").Contact_Type_ID;
                            }
                            else
                            {
                                ObjPatientContact.Contact_Type_Id = 0;
                            }
                        }
                        else
                        {
                            ObjPatientContact.Contact_Type_Id = 0;
                        }
                    }
                    else if (rel.ToLower() == "other")
                    {
                        ObjPatientContact.Contact_Type_Id = _ContactTypeRepository.GetSingle(e => e.Practice_Code == profile.PracticeCode && e.Type_Name.ToLower() == "other contact").Contact_Type_ID;
                    }
                    else
                    {
                        ObjPatientContact.Contact_Type_Id = _ContactTypeRepository.GetSingle(e => e.Practice_Code == profile.PracticeCode && e.Type_Name.ToLower() == rel).Contact_Type_ID;
                    }
                    ObjPatientContact.Country = "US";
                    SaveContact(ObjPatientContact, profile);
                }
            }
        }

        public List<WORK_ORDER_INFO_RES> GetWorkOrderInfo(WORK_ORDER_INFO_REQ obj, UserProfile profile)
        {
            var _patient_Account = Convert.ToInt64(obj.PATIENT_ACCOUNT);
            var pATIENT_ACCOUNT = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = _patient_Account };
            var pRACTICE_CODE = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var result = SpRepository<WORK_ORDER_INFO_RES>.GetListWithStoreProcedure(@"exec [FOX_GET_WORK_ORDER_INFO_FOR_PAT_MAINTENANCE] @PATIENT_ACCOUNT,@PRACTICE_CODE", pATIENT_ACCOUNT, pRACTICE_CODE).ToList();
            if (result.Any())
                return result;
            else
                return new List<WORK_ORDER_INFO_RES>();
        }

        string ReplaceSSN(string ssn)
        {
            if (ssn.Length >= 5)
            {
                //string temp = ssn.Substring(0, 5);
                return "*****" + ssn.Substring(5);
            }
            else
            {
                return ssn;
            }

        }

        public void PrepareLogExport(List<Patient> obj, out List<PatientExportToExcelModel> recordToExport)
        {
            recordToExport = new List<PatientExportToExcelModel>();
            foreach (var item in obj)
            {
                var exportModel = new PatientExportToExcelModel();
                if (item.ROW == 0)
                    item.ROW = 0;
                exportModel.ROW = item.ROW;
                exportModel.Patient_Account = !string.IsNullOrEmpty(item.Patient_Account.ToString()) ? item.Patient_Account.ToString() : "";
                exportModel.First_Name = !string.IsNullOrWhiteSpace(item.First_Name) ? item.First_Name : "";
                exportModel.Last_Name = !string.IsNullOrWhiteSpace(item.Last_Name) ? item.Last_Name : ""; //item.DEPOSIT_DATE.HasValue ? item.DEPOSIT_DATE.Value.ToString("MM/dd/yyyy") : "";
                exportModel.MI = !string.IsNullOrEmpty(item.MIDDLE_NAME) ? item.MIDDLE_NAME : "";

                exportModel.FIRST_NAME_ALIAS = !string.IsNullOrEmpty(item.FIRST_NAME_ALIAS) ? item.FIRST_NAME_ALIAS : "";
                exportModel.LAST_NAME_ALIAS = !string.IsNullOrEmpty(item.LAST_NAME_ALIAS) ? item.LAST_NAME_ALIAS : "";
                exportModel.MIDDLE_INITIALS_ALIAS = !string.IsNullOrEmpty(item.MIDDLE_INITIALS_ALIAS) ? item.MIDDLE_INITIALS_ALIAS : "";

                exportModel.MRN = !string.IsNullOrEmpty(item.Chart_Id) ? item.Chart_Id : "";
                exportModel.SSN = !string.IsNullOrEmpty(item.SSN) ? ReplaceSSN(item.SSN) : "";
                exportModel.DOB = item.Date_Of_Birth.HasValue ? item.Date_Of_Birth.Value.ToString("MM/dd/yyyy") : "";
                exportModel.Gender = !string.IsNullOrEmpty(item.Gender) ? item.Gender : "";
                exportModel.Created_By = !string.IsNullOrEmpty(item.Created_By) ? item.Created_By : "";
                exportModel.Updated_By = !string.IsNullOrEmpty(item.Modified_By) ? item.Modified_By : "";
                exportModel.Created_Date_Time = item.Created_Date.HasValue ? item.Created_Date : null;
                exportModel.Update_Date_Time = item.Modified_Date.HasValue ? item.Modified_Date : null;


                recordToExport.Add(exportModel);
            }
        }

        public ResponseModel ExportPatientListToExcel(PatientSearchRequest request, UserProfile profile)
        {
            ResponseModel resp;
            List<Patient> _patientList = GetPatientList(request, profile);
            resp = ExportPatientListToExcel(_patientList, profile);

            return resp;
        }

        public ResponseModel ExportPatientListToExcel(List<Patient> obj, UserProfile profile)
        {
            var response = new ResponseModel();

            List<PatientExportToExcelModel> objToExport = new List<PatientExportToExcelModel>();
            if (obj != null && obj.Count > 0)
            {
                for (int i = 0; i < obj.Count(); i++)
                {
                    obj[i].ROW = i + 1;

                }
                PrepareLogExport(obj, out objToExport);
                string fileName = "Patients_List_" + DateTime.Now.Ticks + ".xlsx";
                string exportPath = string.Empty;
                bool exported = false;
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                var pathToWriteFile = exportPath + "\\" + fileName;
                DataTable dt = ExportToExcel.ListToDataTable(objToExport);
                dt.TableName = "Patient_List";
                exported = ExportToExcel.CreateExcelDocument(dt, pathToWriteFile);
                if (exported)
                {
                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = virtualPath + fileName;
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "Records couldn't be exported. Please try again.";
                    response.Message = "Records couldn't be exported. Please try again.";
                }
            }
            return response;
        }

        public List<PHR> GetPatientInviteStatus(string PatientAccount, UserProfile profile)
        {
            var _parmPracticeCode = new SqlParameter("Practice_Code", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var _patientAccount = new SqlParameter { ParameterName = "Patient_Account", Value = long.Parse(PatientAccount) };
            return SpRepository<PHR>.GetListWithStoreProcedure(@"exec FOX_SP_Get_Patient_Invite_Status @Patient_Account, @Practice_Code", _patientAccount, _parmPracticeCode);
        }

        public string GetEmailOrFaxToSenderTemplate(string firstName, string LastName, string link, string pin, string practiceName)
        {
            string body = string.Empty;
            string templatePathOfSenderEmail = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/Invitation_To_PHR_Template.html");
            if (File.Exists(templatePathOfSenderEmail))
            {
                string receivedDate = string.Empty;
                string receivedTime = string.Empty;
                body = File.ReadAllText(templatePathOfSenderEmail);
                body = body.Replace("[[FIRST_NAME]]", firstName);
                body = body.Replace("[[LAST_NAME]]", LastName);
                body = body.Replace("[[Link]]", link);
                body = body.Replace("[[PIN_Code]]", pin);
                body = body.Replace("[[Practice_Code]]", practiceName);
            }
            return body;
        }

        private bool SendEmail(string emailAddress, string pin, string link, string firstName, string lastName, UserProfile profile)
        {
            string body = GetEmailOrFaxToSenderTemplate(firstName: firstName, LastName: lastName, link: link, pin: pin, practiceName: profile.PracticeName);
            List<string> BCC = new List<string>();
            BCC.Add("muhammadarslan3@carecloud.com");
            bool sent = Helper.Email(to: emailAddress, subject: "Fox Patient Portal", body: body, profile: profile, CC: null, BCC: BCC);
            return sent;
        }

        private bool SendInvitationSMS(string cellNumber, string pin, string link, string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(cellNumber) && !string.IsNullOrEmpty(pin) && !string.IsNullOrEmpty(link))
            {
                StringBuilder body = new StringBuilder();
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    body.Append("Hi " + firstName + " " + lastName + "," + Environment.NewLine);
                }
                else if (!string.IsNullOrEmpty(firstName))
                {
                    body.Append("Hi " + firstName + "," + Environment.NewLine);
                }
                else if (!string.IsNullOrEmpty(lastName))
                {
                    body.Append("Hi " + lastName + "," + Environment.NewLine);
                }

                body.AppendLine("FOX REHABILITATION SERVICES, INC.would like to connect with you via the client portal. Fox Client Portal provides updates, and allow convenient access to your healthcare information.");
                body.AppendLine("Click on the link below to create your account.");
                body.AppendLine(link);
                string status = ExternalServices.SmsService.NJSmsService(CellPhone: cellNumber, SmsBody: body.ToString());
                if (status.Equals("Error"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private string GetLink(string email, string phone, string patientAccount, string pinCode)
        {
            EncryptionDecryption encrypt = new EncryptionDecryption();
            string queryString = encrypt.Encrypt(email + "|" + phone + "|" + patientAccount + "|" + pinCode).Replace('+', '!');
            //string link = AppConfiguration.PHRRoutingLink + queryString;
            string link = WebConfigurationManager.AppSettings["PHRPortalURL"].ToString() + queryString;
            return link;
        }


        public ResponseModel SendInviteToPatient(PHR obj, UserProfile profile)
        {
            ResponseModel resp;
            PHR patientInviteData;

            //if (string.IsNullOrEmpty(obj.EMAIL_ADDRESS) && !string.IsNullOrEmpty(obj.USER_PHONE))
            //{
            //    patientInviteData = _PatientPHR.Get(e => e.USER_PHONE == obj.USER_PHONE && e.PRACTICE_CODE == profile.PracticeCode && !(e.DELETED.HasValue ? e.DELETED.Value : false));
            //}
            //else if (string.IsNullOrEmpty(obj.USER_PHONE) && !string.IsNullOrEmpty(obj.EMAIL_ADDRESS))
            //{
            //    patientInviteData = _PatientPHR.Get(e => e.EMAIL_ADDRESS == obj.EMAIL_ADDRESS && e.PRACTICE_CODE == profile.PracticeCode && !(e.DELETED.HasValue ? e.DELETED.Value : false));
            //}
            ////else
            ////{
            ////    patientInviteData = _PatientPHR.Get(e => (e.EMAIL_ADDRESS == obj.EMAIL_ADDRESS || e.USER_PHONE == obj.USER_PHONE) && e.PRACTICE_CODE == profile.PracticeCode && !(e.DELETED.HasValue ? e.DELETED.Value : false));
            ////}

            if (!string.IsNullOrWhiteSpace(obj.EMAIL_ADDRESS))
            {
                patientInviteData = _PatientPHR.Get(e => e.EMAIL_ADDRESS == obj.EMAIL_ADDRESS && e.PRACTICE_CODE == profile.PracticeCode && !(e.DELETED.HasValue ? e.DELETED.Value : false));
            }
            else
            {
                patientInviteData = null;
            }


            var dbPatient = _PatientRepository.GetByID(obj.PATIENT_ACCOUNT);
            if (patientInviteData != null)
            {
                string msg = "";
                PHR phoneCheck = _PatientPHR.Get(e => e.USER_PHONE == obj.USER_PHONE && e.PRACTICE_CODE == profile.PracticeCode && !(e.DELETED.HasValue ? e.DELETED.Value : false) && e.USER_PHONE != null);
                PHR emailCheck = _PatientPHR.Get(e => e.EMAIL_ADDRESS == obj.EMAIL_ADDRESS && e.PRACTICE_CODE == profile.PracticeCode && !(e.DELETED.HasValue ? e.DELETED.Value : false) && e.EMAIL_ADDRESS != null);
                if (phoneCheck != null && emailCheck != null)
                {
                    msg = "A user already been registered with this email address | cell number";
                }
                if (phoneCheck != null)
                {
                    msg = "A user already been registered with this cell number";
                }
                else
                {
                    msg = "A user already been registered with this email address";
                }
                resp = new ResponseModel()
                {
                    ErrorMessage = "",
                    Message = msg,
                    Success = false
                };
            }
            else
            {
                if (obj.ShouldUpdatePatient)
                {
                    dbPatient.Email_Address = obj.EMAIL_ADDRESS;
                    dbPatient.cell_phone = obj.USER_PHONE;
                    dbPatient.Modified_Date = Helper.GetCurrentDate();
                    dbPatient.Modified_By = profile.UserName;
                }


                int pin = Helper.GetRandomPin();
                string link = GetLink(obj.EMAIL_ADDRESS, obj.USER_PHONE, obj.PATIENT_ACCOUNT.ToString(), pin.ToString());

                Patient pat = GetPatientDetail(obj.PATIENT_ACCOUNT);

                //bool SMSStatus = false;
                bool EmailStatus = false;
                //if (!string.IsNullOrEmpty(obj.USER_PHONE))
                //{
                //    SMSStatus = SendInvitationSMS(cellNumber: obj.USER_PHONE, pin: pin.ToString(), link: link, firstName: pat.FirstName, lastName: pat.LastName);
                //}

                if (!string.IsNullOrEmpty(obj.EMAIL_ADDRESS))
                {
                    EmailStatus = SendEmail(obj.EMAIL_ADDRESS, pin.ToString(), link, pat.First_Name, pat.Last_Name, profile);
                }

                if (EmailStatus)
                {
                    patientInviteData = new PHR();
                    CommonServices.EncryptionDecryption encrypt = new CommonServices.EncryptionDecryption();
                    long maxId = Helper.getMaximumId("FOX_PHR_USER_ID");
                    patientInviteData.USER_ID = maxId;
                    patientInviteData.USER_NAME = "FP_" + obj.USER_NAME + "_" + maxId;
                    patientInviteData.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                    patientInviteData.USER_PHONE = obj.USER_PHONE;
                    patientInviteData.EMAIL_ADDRESS = obj.EMAIL_ADDRESS;
                    patientInviteData.TEMP_PASSWORD = encrypt.Encrypt(pin.ToString());
                    // patientInviteData.IS_ACTIVATED = true;
                    patientInviteData.IS_BLOCK = false;
                    patientInviteData.INVITE_STATUS = "Response Awaited";
                    patientInviteData.PRACTICE_CODE = profile.PracticeCode;
                    patientInviteData.CREATED_DATE = Helper.GetCurrentDate();
                    patientInviteData.MODIFIED_DATE = Helper.GetCurrentDate();
                    patientInviteData.CREATED_BY = patientInviteData.MODIFIED_BY = profile.UserName;
                    patientInviteData.DELETED = false;
                    _PatientPHR.Insert(patientInviteData);
                    _PatientPHR.Save();
                    resp = new ResponseModel()
                    {
                        ErrorMessage = "",
                        Message = "Invitation sent to patient successfully.",
                        Success = true
                    };
                }
                else
                {
                    resp = new ResponseModel()
                    {
                        Success = false,
                        Message = "Unable to send request to patient.",
                        ErrorMessage = ""
                    };
                }
            }

            return resp;
        }
        public List<Patient> GetInvitedPatient(PHR obj, UserProfile profile)
        {
            try
            {
                PHR invitedPatientData;
                List<Patient> invitedPatient = new List<Patient>();
                if (!string.IsNullOrWhiteSpace(obj.EMAIL_ADDRESS))
                {
                    invitedPatientData = _PatientPHR.Get(e => e.EMAIL_ADDRESS == obj.EMAIL_ADDRESS && e.PRACTICE_CODE == profile.PracticeCode && !(e.DELETED.HasValue ? e.DELETED.Value : false));
                    if (invitedPatientData != null)
                    {
                        var _parmPracticeCode = new SqlParameter("Practice_Code", SqlDbType.BigInt) { Value = profile.PracticeCode };
                        var _patientAccount = new SqlParameter("Patient_Account", SqlDbType.BigInt) { Value = invitedPatientData.PATIENT_ACCOUNT };
                        invitedPatient = SpRepository<Patient>.GetListWithStoreProcedure(@"exec FOX_PROC_CHECK_DUPLICATE_PATIENT @Practice_Code,  @Patient_Account", _parmPracticeCode, _patientAccount);
                    }
                }
                return invitedPatient;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ResponseModel UnBlockPatientFromPHR(PHR obj, UserProfile profile)
        {
            ResponseModel resp;
            PHR patientInviteData = _PatientPHR.Get(e => e.USER_ID == obj.USER_ID && e.PRACTICE_CODE == profile.PracticeCode && !(e.DELETED.HasValue ? e.DELETED.Value : false) && (e.IS_BLOCK.HasValue ? e.IS_BLOCK.Value : false));
            if (patientInviteData != null)
            {
                patientInviteData.IS_BLOCK = false;
                patientInviteData.INVITE_STATUS = "Active";
                patientInviteData.MODIFIED_DATE = Helper.GetCurrentDate();
                patientInviteData.MODIFIED_BY = profile.UserName;
                _PatientPHR.Update(patientInviteData);
                _PatientPHR.Save();
                resp = new ResponseModel()
                {
                    Success = true,
                    Message = "Patient unblocked successfully.",
                    ErrorMessage = ""
                };
            }
            else
            {
                resp = new ResponseModel()
                {
                    Success = false,
                    Message = "Error occoured while unblocking the patient.",
                    ErrorMessage = ""
                };
            }
            return resp;
        }

        public ResponseModel BlockPatientFromPHR(PHR obj, UserProfile profile)
        {
            ResponseModel resp;
            PHR patientInviteData = _PatientPHR.Get(e => e.USER_ID == obj.USER_ID && !(e.DELETED.HasValue ? e.DELETED.Value : false) && e.PRACTICE_CODE == profile.PracticeCode);
            if (patientInviteData != null)
            {
                patientInviteData.IS_BLOCK = true;
                patientInviteData.INVITE_STATUS = "Blocked";
                patientInviteData.MODIFIED_DATE = Helper.GetCurrentDate();
                patientInviteData.MODIFIED_BY = profile.UserName;
                _PatientPHR.Update(patientInviteData);
                _PatientPHR.Save();
                resp = new ResponseModel()
                {
                    Success = true,
                    Message = "Patient blocked successfully.",
                    ErrorMessage = ""
                };
            }
            else
            {
                resp = new ResponseModel()
                {
                    Success = false,
                    Message = "Error occoured while blocking the patient.",
                    ErrorMessage = ""
                };
            }
            return resp;
        }

        public ResponseModel CancelPatientRequestFromPHR(PHR obj, UserProfile profile)
        {
            ResponseModel resp;
            PHR patientInviteData = _PatientPHR.Get(e => e.USER_ID == obj.USER_ID && !(e.DELETED.HasValue ? e.DELETED.Value : false) && e.PRACTICE_CODE == profile.PracticeCode);
            if (patientInviteData != null)
            {
                patientInviteData.DELETED = true;
                patientInviteData.INVITE_STATUS = "Blocked";
                patientInviteData.MODIFIED_DATE = Helper.GetCurrentDate();
                patientInviteData.MODIFIED_BY = profile.UserName;
                _PatientPHR.Update(patientInviteData);
                _PatientPHR.Save();
                resp = new ResponseModel()
                {
                    Success = true,
                    Message = "Request to patient canceled successfully.",
                    ErrorMessage = ""
                };
            }
            else
            {
                resp = new ResponseModel()
                {
                    Success = false,
                    Message = "Error occoured while canceling the request.",
                    ErrorMessage = ""
                };
            }
            return resp;
        }

        public ResponseModel ResendRequestforPHRToPatient(PHR obj, UserProfile profile)
        {
            ResponseModel resp;
            PHR patientInviteData = _PatientPHR.GetByID(obj.USER_ID);
            if (patientInviteData != null)
            {
                //if (SendEmail())
                int pin = Helper.GetRandomPin();

                //CommonServices.EncryptionDecryption encrypt = new CommonServices.EncryptionDecryption();
                string link = GetLink(email: obj.EMAIL_ADDRESS, phone: obj.USER_PHONE, patientAccount: obj.PATIENT_ACCOUNT.ToString(), pinCode: pin.ToString());
                Patient pat = GetPatientDetail(obj.PATIENT_ACCOUNT);
                //if ((!string.IsNullOrEmpty(obj.EMAIL_ADDRESS) && SendEmail(obj.EMAIL_ADDRESS, pin.ToString(), link, pat.First_Name, pat.Last_Name, profile))
                //    || (!string.IsNullOrEmpty(obj.USER_PHONE) && SendInvitationSMS(cellNumber: obj.USER_PHONE, pin: pin.ToString(), link: link, firstName: pat.FirstName, lastName: pat.LastName)))
                if (!string.IsNullOrEmpty(obj.EMAIL_ADDRESS) && SendEmail(obj.EMAIL_ADDRESS, pin.ToString(), link, pat.First_Name, pat.Last_Name, profile))
                {
                    CommonServices.EncryptionDecryption encrypt = new CommonServices.EncryptionDecryption();
                    patientInviteData.USER_PHONE = obj.USER_PHONE;
                    patientInviteData.EMAIL_ADDRESS = obj.EMAIL_ADDRESS;
                    patientInviteData.TEMP_PASSWORD = encrypt.Encrypt(pin.ToString());
                    patientInviteData.MODIFIED_DATE = patientInviteData.CREATED_DATE = Helper.GetCurrentDate();
                    patientInviteData.MODIFIED_BY = patientInviteData.CREATED_BY = profile.UserName;
                    _PatientPHR.Update(patientInviteData);
                    _PatientPHR.Save();
                    resp = new ResponseModel()
                    {
                        Success = true,
                        Message = "Request sent to patient successfully.",
                        ErrorMessage = ""
                    };
                }
                else
                {
                    resp = new ResponseModel()
                    {
                        Success = false,
                        Message = "Unable to send request to patient.",
                        ErrorMessage = ""
                    };
                }
            }
            else
            {
                resp = new ResponseModel()
                {
                    Success = false,
                    Message = "Error occoured while sending request to patient.",
                    ErrorMessage = ""
                };
            }
            return resp;
        }

        public void UpdatePrimaryPhysicianInCases(long? PCP_ID, long Patient_Account, long practiceCode)
        {
            var caseStatusList = _caseStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
            caseStatusList = caseStatusList.FindAll(e => e.NAME.ToLower() == "act" || e.NAME.ToLower() == "open");
            var CASE_STATUS_ID_ACTIVE = caseStatusList.Find(e => e.NAME.ToLower() == "act").CASE_STATUS_ID;
            var CASE_STATUS_ID_OPEN = caseStatusList.Find(e => e.NAME.ToLower() == "open").CASE_STATUS_ID;
            var casesList = _caseRepository.GetMany(e => (e.PATIENT_ACCOUNT == Patient_Account));
            casesList = casesList.FindAll(e => e.CASE_STATUS_ID == CASE_STATUS_ID_ACTIVE || e.CASE_STATUS_ID == CASE_STATUS_ID_OPEN);
            if (casesList != null)
            {
                foreach (var case1 in casesList)
                {
                    if (case1.PRIMARY_PHY_ID != PCP_ID)
                    {
                        case1.PRIMARY_PHY_ID = PCP_ID;
                    }
                    _caseRepository.Update(case1);

                }
                _CaseContext.SaveChanges();
            }
        }

        //public PatientDeceasedInfo GetPatientDeceaseDate(string pat_Acc, UserProfile profile)
        //{
        //    PatientDeceasedInfo decease_info = new PatientDeceasedInfo();
        //    if (!string.IsNullOrWhiteSpace(pat_Acc))
        //    {
        //        decease_info.Patient_Account_Str = pat_Acc;
        //        PatientEligibilitySearchModel model = new PatientEligibilitySearchModel();
        //        model.Patient_Account_Str = pat_Acc;
        //        decease_info.Deceased_Date = GetDeceaseDateFromEligibility(model, profile);
        //    }

        //    return decease_info;
        //}

        //private DateTime? GetDeceaseDateFromEligibility(PatientEligibilitySearchModel patientEligibilitySearchModel, UserProfile profile)
        //{
        //    DateTime? dec_date = null;
        //    long patientAccount = long.Parse(patientEligibilitySearchModel.Patient_Account_Str);
        //    //long insType = eligibilitySearchReq.INS_TYPE;
        //    var _patientAccount = new SqlParameter { ParameterName = "PATIENTACCOUNT", Value = patientAccount };
        //    var _result = SpRepository<PatientInsuranceInformation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES_INFO @PATIENTACCOUNT", _patientAccount);
        //    var patientInsuranceInformation = _result.Where(x => x.INS_TYPE == 1).FirstOrDefault();
        //    //string htmlStr = GetElgibilityDetails(patientAccount, patientInsuranceInformation, profile);
        //    string htmlStr = gethtml();

        //    HtmlDocument htmlDoc = new HtmlDocument();
        //    //htmlDoc.Load(path);
        //    htmlDoc.LoadHtml(htmlStr);
        //    htmlDoc.DocumentNode.OuterHtml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\b", "").Replace("&nbsp;", " ");
        //    //var decease_date_nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'Fox_Pat_Deceased_Date')]");
        //    var decease_date_node = htmlDoc.DocumentNode.SelectSingleNode("//tr[contains(@class,'Fox_Pat_Deceased_Date')]/td[2]");

        //    if (decease_date_node != null)
        //    {
        //        List<DateTime> dateTimeList = new List<DateTime>();
        //        Regex rx = new Regex(@"(?<month>\d{2})\/(?<day>\d{2})\/(?<year>\d{4})");
        //        MatchCollection mCollection = rx.Matches(decease_date_node.InnerText);
        //        foreach (var item in mCollection)
        //        {
        //            Match m = item as Match;
        //            if (m.Success)
        //            {
        //                DateTime date = DateTime.ParseExact(String.Format("{0}/{1}/{2}",
        //                    m.Groups["month"].Value,
        //                    m.Groups["day"].Value,
        //                    m.Groups["year"].Value), "MM/dd/yyyy", null);

        //                dateTimeList.Add(date);
        //            }
        //        }

        //        dec_date = dateTimeList.OrderBy(t => t.Date).FirstOrDefault();
        //    }

        //    return dec_date;
        //}

        public ResponseModel SaveInsuranceEligibilityFromIndexInfo(PatientInsurance insuranceToCreateUpdate, UserProfile profile)
        {
            ResponseModel resp = new ResponseModel();
            resp.Success = false;
            var dbInsurance = _PatientInsuranceRepository.GetFirst(e => e.Patient_Insurance_Id == insuranceToCreateUpdate.Patient_Insurance_Id && (e.Deleted ?? false) == false);
            if (dbInsurance != null)
            {
                if (EligibilityAlreadyLoadedInThisMonth(dbInsurance))
                {
                    resp.Message = "Eligibility can only be entered after span of 14 days.";
                }
                else
                {
                    insuranceToCreateUpdate.Parent_Patient_insurance_Id = dbInsurance.Parent_Patient_insurance_Id;
                    insuranceToCreateUpdate.MTBC_Patient_Insurance_Id = dbInsurance.MTBC_Patient_Insurance_Id;

                    if (!string.IsNullOrEmpty(insuranceToCreateUpdate.Effective_Date_In_String))
                        insuranceToCreateUpdate.Effective_Date = Convert.ToDateTime(insuranceToCreateUpdate.Effective_Date_In_String);
                    else
                    {
                        insuranceToCreateUpdate.Effective_Date = dbInsurance.Effective_Date;
                    }

                    if (!string.IsNullOrEmpty(insuranceToCreateUpdate.Termination_Date_In_String))
                        insuranceToCreateUpdate.Termination_Date = Convert.ToDateTime(insuranceToCreateUpdate.Termination_Date_In_String);
                    else
                    {
                        insuranceToCreateUpdate.Termination_Date = dbInsurance.Termination_Date;
                    }

                    insuranceToCreateUpdate.SUPRESS_BILLING_UNTIL = dbInsurance.SUPRESS_BILLING_UNTIL;
                    insuranceToCreateUpdate.Subscriber = dbInsurance.Subscriber;
                    insuranceToCreateUpdate.Pri_Sec_Oth_Type = dbInsurance.Pri_Sec_Oth_Type;
                    insuranceToCreateUpdate.Patient_Account = dbInsurance.Patient_Account;
                    insuranceToCreateUpdate.Insurance_Id = dbInsurance.Insurance_Id;
                    insuranceToCreateUpdate.FOX_TBL_INSURANCE_ID = dbInsurance.FOX_TBL_INSURANCE_ID;
                    insuranceToCreateUpdate.INACTIVE = dbInsurance.INACTIVE;
                    insuranceToCreateUpdate.Relationship = dbInsurance.Relationship;
                    insuranceToCreateUpdate.Policy_Number = dbInsurance.Policy_Number;
                    insuranceToCreateUpdate.Group_Number = dbInsurance.Group_Number;
                    insuranceToCreateUpdate.Plan_Name = dbInsurance.Plan_Name;
                    insuranceToCreateUpdate.PR_DISCOUNT_ID = dbInsurance.PR_DISCOUNT_ID;
                    insuranceToCreateUpdate.PR_PERIOD_ID = dbInsurance.PR_PERIOD_ID;
                    insuranceToCreateUpdate.PERIODIC_PAYMENT = dbInsurance.PERIODIC_PAYMENT;
                    insuranceToCreateUpdate.Is_Authorization_Required = dbInsurance.Is_Authorization_Required;
                    insuranceToCreateUpdate.IsWellness = dbInsurance.IsWellness;
                    insuranceToCreateUpdate.IsSkilled = dbInsurance.IsSkilled;

                    PatientInsuranceEligibilityDetail ObjPatientInsuranceEligibilityDetail = new PatientInsuranceEligibilityDetail();
                    ObjPatientInsuranceEligibilityDetail.InsuranceToCreateUpdate = insuranceToCreateUpdate;
                    ObjPatientInsuranceEligibilityDetail.Patient_Account_Str = insuranceToCreateUpdate.Patient_Account.ToString();
                    resp.Success = SaveInsuranceAndEligibilityDetails(ObjPatientInsuranceEligibilityDetail, profile, false).Success;
                    if (resp.Success)
                    {
                        resp.Message = "Eligibility added successfully.";
                        long patientAccount = long.Parse(ObjPatientInsuranceEligibilityDetail.Patient_Account_Str);
                        var _patientAccount = new SqlParameter { ParameterName = "PATIENTACCOUNT", Value = patientAccount };
                        var _result = SpRepository<PatientInsuranceInformation>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_INSURANCES_INFO @PATIENTACCOUNT", _patientAccount);

                        PatientInsuranceInformation patientInsuranceInformation = new PatientInsuranceInformation();

                        if (insuranceToCreateUpdate.Patient_Insurance_Id != 0)
                        {
                            //patientInsuranceInformation = _result.Where(x => x.PATIENT_INSURANCE_ID == insuranceToCreateUpdate.Patient_Insurance_Id).FirstOrDefault();
                            patientInsuranceInformation = _result.Where(x => x.MTBC_Patient_Insurance_Id == insuranceToCreateUpdate.MTBC_Patient_Insurance_Id).FirstOrDefault();
                        }
                        else
                        {
                            patientInsuranceInformation = _result.Where(x => x.INS_TYPE == 1).FirstOrDefault();
                        }

                        if (patientInsuranceInformation != null)
                        {

                            string eligibility_html_string = GetElgibilityDetails(patientAccount, patientInsuranceInformation, profile, insuranceToCreateUpdate.IS_MVP_VIEW);

                            if (insuranceToCreateUpdate.IS_MVP_VIEW)
                            {
                                eligibility_html_string = eligibility_html_string.Replace(@"id=""container""", @"id =""main-container-eligibility""");
                                eligibility_html_string = eligibility_html_string.Replace(@"table-heading", @"table-heading-orange");
                                eligibility_html_string = RemoveStyleNodeFromHtmlForMVP(eligibility_html_string);
                            }
                            else
                            {
                                eligibility_html_string = eligibility_html_string.Replace(@"id=""main-container""", @"id=""main-container-eligibility""");
                                if (string.IsNullOrEmpty(patientInsuranceInformation?.Fox_Insurance_Name) && patientInsuranceInformation?.FOX_TBL_INSURANCE_ID != null)
                                {
                                    patientInsuranceInformation.Fox_Insurance_Name = _foxInsurancePayersRepository.GetFirst(p => p.FOX_TBL_INSURANCE_ID == patientInsuranceInformation.FOX_TBL_INSURANCE_ID && (p.DELETED ?? false) == false)?.INSURANCE_NAME;
                                }
                                eligibility_html_string = UpdatePayerNameInHTML(eligibility_html_string, patientInsuranceInformation);
                            }

                            HtmlDocument eligibility = new HtmlDocument();
                            eligibility.LoadHtml(eligibility_html_string);
                            var main_conatiner = eligibility.DocumentNode.SelectSingleNode("//*[contains(@id,'container')]");
                            if (insuranceToCreateUpdate.IS_MVP_VIEW)
                            {
                                main_conatiner = eligibility.DocumentNode.SelectSingleNode("//*[contains(@class,'container')]");
                            }
                            if (main_conatiner != null)
                            {
                                eligibility_html_string = main_conatiner.InnerHtml;
                            }
                            //eligibility_html_string = eligibility.DocumentNode.SelectSingleNode("//*[contains(@id,'container')]").InnerHtml;

                            string template_html = string.Empty;
                            if (insuranceToCreateUpdate.IS_MVP_VIEW)
                            {
                                template_html = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/Eligibility_MVP_View_Template.html");
                            }
                            else
                            {
                                template_html = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/Eligibilty_View_Template.html");
                            }

                            template_html = File.ReadAllText(template_html);
                            HtmlDocument template = new HtmlDocument();
                            template.LoadHtml(template_html);
                            var container = template.GetElementbyId("eligibility-container");
                            if (container != null && main_conatiner != null)
                            {
                                //template.DocumentNode.SelectSingleNode("//*[contains(@id,'eligibility-container')]").AppendChild(container1);
                                template.GetElementbyId("eligibility-container").AppendChild(main_conatiner);
                            }
                            var print_button = template.GetElementbyId("Elig_print");
                            if (print_button != null)
                            {
                                template.GetElementbyId("Elig_print").Remove();
                            }
                            string htmlStr = template.DocumentNode.OuterHtml;

                            if (insuranceToCreateUpdate.IS_MVP_VIEW == false)
                            {
                                var custom_panel = template.DocumentNode.SelectSingleNode("//*[contains(@class,'col-12')]");
                                if (custom_panel != null)
                                {
                                    var custom_panel_nodes = custom_panel.LastChild.ChildNodes;
                                    var total_custom_panel = custom_panel_nodes.Count;
                                    List<HtmlNodeCollection> list = new List<HtmlNodeCollection>();
                                    HtmlNodeCollection nodeCollection = new HtmlNodeCollection(null);
                                    if (total_custom_panel > 32)
                                    {
                                        for (int i = 0; i < custom_panel_nodes.Count; i++)
                                        {

                                            nodeCollection.Add(custom_panel_nodes[i]);
                                            if ((i % 32 == 0) && i != 0)
                                            {
                                                list.Add(nodeCollection);
                                                nodeCollection = new HtmlNodeCollection(null);
                                            }
                                        }
                                        list.Add(nodeCollection);

                                        bool NewDocument = true;
                                        for (int i = 0; i < list.Count; i++)
                                        {
                                            custom_panel.LastChild.ChildNodes.Clear();
                                            custom_panel.LastChild.AppendChildren(list[i]);
                                            htmlStr = template.DocumentNode.OuterHtml;
                                            if (i == 0)
                                            {
                                                var top_header = template.DocumentNode.SelectNodes("//*[contains(@class,'col-6 margin-b10')]");
                                                var top_header_date = template.DocumentNode.SelectSingleNode("//*[contains(@class,'custom-panel-head')]");
                                                top_header_date.Remove();
                                                top_header[0].Remove();
                                                top_header[1].Remove();
                                            }
                                            var file_name = insuranceToCreateUpdate.Patient_Account.ToString() + "_" + DateTime.Now.Ticks;
                                            var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                                            ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, htmlStr, file_name.Replace(' ', '_'), insuranceToCreateUpdate.IS_MVP_VIEW);
                                            if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                                            {
                                                string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                                                int numberOfPages = getNumberOfPagesOfPDF(filePath);
                                                SavePdftoImagesEligibilty(filePath, config, numberOfPages, "Email", profile, insuranceToCreateUpdate.Patient_Account.ToString(), insuranceToCreateUpdate.Patient_Insurance_Id, NewDocument, insuranceToCreateUpdate.Work_ID);
                                            }
                                            NewDocument = false;
                                        }
                                    }
                                    else
                                    {
                                        var file_name = insuranceToCreateUpdate.Patient_Account.ToString() + "_" + DateTime.Now.Ticks;
                                        var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                                        ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, htmlStr, file_name.Replace(' ', '_'), insuranceToCreateUpdate.IS_MVP_VIEW);


                                        if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                                        {
                                            string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                                            int numberOfPages = getNumberOfPagesOfPDF(filePath);
                                            SavePdftoImagesEligibilty(filePath, config, numberOfPages, "Email", profile, insuranceToCreateUpdate.Patient_Account.ToString(), insuranceToCreateUpdate.Patient_Insurance_Id, true, insuranceToCreateUpdate.Work_ID);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var file_name = insuranceToCreateUpdate.Patient_Account.ToString() + "_" + DateTime.Now.Ticks;
                                var config = Helper.GetServiceConfiguration(profile.PracticeCode);

                                ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, htmlStr, file_name.Replace(' ', '_'), insuranceToCreateUpdate.IS_MVP_VIEW);

                                if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                                {
                                    string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                                    int numberOfPages = getNumberOfPagesOfPDF(filePath);
                                    SavePdftoImagesEligibilty(filePath, config, numberOfPages, "Email", profile, ObjPatientInsuranceEligibilityDetail.Patient_Account_Str, patientInsuranceInformation.PATIENT_INSURANCE_ID, true, insuranceToCreateUpdate.Work_ID);
                                }
                            }
                        }
                    }
                    else
                    {
                        resp.Message = "An error occurred while adding eligibility. Please try again.";
                    }
                }
            }
            return resp;
        }

        public bool EligibilityAlreadyLoadedInThisMonth(PatientInsurance dbInsurance)
        {
            if (dbInsurance != null)
            {
                var last_elig_record = _PatientInsuranceRepository.GetFirst(e => e.Patient_Account == dbInsurance.Patient_Account && e.Pri_Sec_Oth_Type == dbInsurance.Pri_Sec_Oth_Type
                        && e.FOX_INSURANCE_STATUS == "C" && (e.Deleted ?? false) == false && e.ELIG_LOADED_ON.HasValue);

                if (last_elig_record != null && last_elig_record.ELIG_LOADED_ON != null)
                {
                    DateTime d1 = last_elig_record.ELIG_LOADED_ON.Value;
                    DateTime d2 = DateTime.Now;
                    TimeSpan span = d2 - d1;
                    if (span.TotalDays <= 14)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public FOX_TBL_TASK AddUpdateTask(FOX_TBL_TASK task, UserProfile profile, string Eligibility_MSP_Data)
        {
            if (!string.IsNullOrEmpty(task.PATIENT_ACCOUNT_STR))
            {
                task.PATIENT_ACCOUNT = Convert.ToInt64(task.PATIENT_ACCOUNT_STR);
            }
            if (task != null && profile != null)
            {
                FOX_TBL_TASK dbTask = _TaskRepository.GetFirst(t => t.PRACTICE_CODE == profile.PracticeCode && t.TASK_ID == task.TASK_ID);
                if (dbTask == null)
                {
                    dbTask = new FOX_TBL_TASK();
                    dbTask.TASK_ID = Helper.getMaximumId("FOX_TASK_ID");
                    dbTask.PRACTICE_CODE = profile.PracticeCode;
                    dbTask.PATIENT_ACCOUNT = task.PATIENT_ACCOUNT;
                    dbTask.IS_COMPLETED_INT = 0;/*0: Initiated 1:Sender Completed 2:Final Route Completed*/
                    dbTask.CREATED_BY = dbTask.MODIFIED_BY = profile.UserName;
                    dbTask.CREATED_DATE = dbTask.MODIFIED_DATE = Helper.GetCurrentDate();
                    dbTask.IS_TEMPLATE = task.IS_TEMPLATE;
                    AddTaskLogs(dbTask, task, profile, Eligibility_MSP_Data);
                    if (task.DUE_DATE_TIME != null)
                    {
                        if (dbTask.DUE_DATE_TIME.HasValue && dbTask.DUE_DATE_TIME.Value.Date != task.DUE_DATE_TIME.Value.Date)
                        {
                            task.Is_Change = true;
                        }
                    }
                    dbTask.TASK_TYPE_ID = task.TASK_TYPE_ID;
                    dbTask.SEND_TO_ID = task.SEND_TO_ID;
                    dbTask.FINAL_ROUTE_ID = task.FINAL_ROUTE_ID;
                    dbTask.PRIORITY = task.PRIORITY;
                    dbTask.DUE_DATE_TIME = task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                    dbTask.CATEGORY_ID = task.CATEGORY_ID;
                    dbTask.IS_REQ_SIGNOFF = task.IS_REQ_SIGNOFF;
                    dbTask.IS_SENDING_ROUTE_DETAILS = task.IS_SENDING_ROUTE_DETAILS;
                    dbTask.SEND_CONTEXT_ID = task.SEND_CONTEXT_ID;
                    dbTask.CONTEXT_INFO = task.CONTEXT_INFO;
                    dbTask.DEVELIVERY_ID = task.DEVELIVERY_ID;
                    dbTask.DESTINATIONS = task.DESTINATIONS;
                    dbTask.LOC_ID = task.LOC_ID;
                    dbTask.PROVIDER_ID = task.PROVIDER_ID;
                    dbTask.IS_SEND_EMAIL_AUTO = task.IS_SEND_EMAIL_AUTO;
                    dbTask.DELETED = task.DELETED;
                    dbTask.IS_TEMPLATE = task.IS_TEMPLATE;
                    dbTask.IS_SEND_TO_USER = task.IS_SEND_TO_USER;
                    dbTask.IS_FINAL_ROUTE_USER = task.IS_FINAL_ROUTE_USER;
                    dbTask.IS_FINALROUTE_MARK_COMPLETE = task.IS_FINALROUTE_MARK_COMPLETE;
                    dbTask.IS_SENDTO_MARK_COMPLETE = task.IS_SENDTO_MARK_COMPLETE;
                    AddEditTask_TaskSubTypesforPORTA(dbTask.TASK_ID, task.TASK_TYPE_ID, profile, task.PATIENT_ACCOUNT);

                    if (dbTask.DUE_DATE_TIME == null)
                    {
                        dbTask.DUE_DATE_TIME = task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                    }                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(InterfaceSynch, profile);
                    dbTask.dbChangeMsg = "TaskInsertSuccessed";
                    dbTask.CATEGORY_CODE = _taskTypeRepository.GetByID(dbTask?.TASK_TYPE_ID)?.CATEGORY_CODE;
                    _TaskRepository.Insert(dbTask);
                    _TaskRepository.Save();
                }

                return dbTask;
            }
            return null;
        }


        private void AddEditTask_TaskSubTypesforPORTA(long tASK_ID, int? task_type_id, UserProfile profile, long? PATIENT_ACCOUNT)
        {
            bool IS_CHECKED = false;
            var patientInterfaced = __InterfaceSynchModelRepository.GetFirst(t => (t.DELETED == false) && t.PRACTICE_CODE == profile.PracticeCode && t.PATIENT_ACCOUNT == PATIENT_ACCOUNT && t.IS_SYNCED == true);
            var taskSubTypeList = _taskSubTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && t.DELETED == false && t.TASK_TYPE_ID == task_type_id);
            var taskTypes = _taskTypeRepository.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED);
            var currentTaskType = taskTypes.Find(e => e.TASK_TYPE_ID == task_type_id);
            foreach (var taskSubType in taskSubTypeList)
            {
                if (currentTaskType != null && currentTaskType.RT_CODE != null && currentTaskType.RT_CODE.ToLower() == "block" && taskSubType.NAME.ToLower() == "msp")
                {
                    IS_CHECKED = true;
                }
                else
                {
                    IS_CHECKED = false;
                }

                if (IS_CHECKED)
                {
                    var taskTaskSubType = _TaskTaskSubTypeRepository.GetFirst(t =>
                                                                              t.TASK_ID == tASK_ID
                                                                              && t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID
                                                                              && t.PRACTICE_CODE == profile.PracticeCode);
                    bool IsEdit = false;
                    if (taskTaskSubType == null && IS_CHECKED)
                    {
                        taskTaskSubType = new FOX_TBL_TASK_TASK_SUB_TYPE();
                        taskTaskSubType.TASK_TASK_SUB_TYPE_ID = Helper.getMaximumId("FOX_TBL_TASK_TASK_SUB_TYPE_ID");
                        taskTaskSubType.PRACTICE_CODE = profile.PracticeCode;
                        taskTaskSubType.TASK_ID = tASK_ID;
                        taskTaskSubType.TASK_SUB_TYPE_ID = taskSubType.TASK_SUB_TYPE_ID;
                        taskTaskSubType.CREATED_BY = taskTaskSubType.MODIFIED_BY = profile.UserName;
                        taskTaskSubType.CREATED_DATE = taskTaskSubType.MODIFIED_DATE = DateTime.Now;
                        IsEdit = false;
                    }
                    taskTaskSubType.DELETED = !IS_CHECKED;
                    if (!IsEdit)
                    {
                        _TaskTaskSubTypeRepository.Insert(taskTaskSubType);
                    }
                    _TaskTaskSubTypeRepository.Save();
                }
            }
        }

        private void AddTaskLogs(FOX_TBL_TASK dbtask, FOX_TBL_TASK task, UserProfile profile, string Eligibility_MSP_Data)
        {
            List<TaskLog> taskLoglist = new List<TaskLog>();
            StringBuilder taklog = new StringBuilder("<br>");
            if (task.TASK_TYPE_ID != null)
            {
                if (!string.IsNullOrWhiteSpace(Eligibility_MSP_Data))
                {
                    var MSP = Eligibility_MSP_Data.Split('-');
                    foreach (var arr in MSP)
                    {
                        if (arr != null && arr != "")
                        {
                            taklog.Append(arr + "<br>");
                        }
                    }


                }
            }
            //}

            taskLoglist.Add(new TaskLog() { ACTION = "+ Patient Information", ACTION_DETAIL = taklog.ToString() });

            if (taskLoglist.Count() > 0)
            {
                taskLoglist.ForEach(row =>
                {
                    row.TASK_LOG_ID = Helper.getMaximumId("FOX_TASK_LOG_ID");
                    row.PRACTICE_CODE = profile.PracticeCode;
                    row.TASK_ID = dbtask.TASK_ID;
                    row.CREATED_BY = row.MODIFIED_BY = profile.UserName;
                    row.CREATED_DATE = row.MODIFIED_DATE = Helper.GetCurrentDate();
                });
            }

            foreach (var taskLog in taskLoglist)
            {
                _taskLogRepository.Insert(taskLog);
                _taskLogRepository.Save();
            }
            _TaskContext.SaveChanges();
        }


        private FOX_TBL_TASK setTaskData(UserProfile profile, long? PATIENT_ACCOUNT, string tasktype, string CURRENT_DATE_STR)
        {
            var task = new FOX_TBL_TASK();
            if (tasktype == "BLOCK")
            {
                task.TASK_TYPE_ID = _taskTypeRepository.GetFirst(t => t.RT_CODE.ToLower() == "block" && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED)?.TASK_TYPE_ID ?? 0;
            }
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
                task.SEND_TO_ID = taskTemplate.SEND_TO_ID;
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

        public ResponseModel PrivateHOMExists(string statecode, UserProfile profile)
        {
            var response = new ResponseModel();
            response.Success = false;

            if (!string.IsNullOrWhiteSpace(statecode))
            {
                var hom = _FacilityLocationRepository.GetFirst(e => e.NAME.ToLower().Contains("private home") && e.CODE.ToLower().Contains(statecode.ToLower()) && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);
                if (hom != null)
                {
                    response.Success = true;
                    return response;
                }
            }
            return response;
        }

        public ResponseModel DeleteInsuranceInformation(PatientInsuranceDetail obj, UserProfile profile)
        {
            var response = new ResponseModel();
            response.Success = false;
            if (obj != null && obj.Patient_Insurance_Id != null)
            {
                if (obj.MTBC_Patient_Insurance_Id != 0)
                {
                    //Delete From Fox Insurance Table
                    var allinsurancesofsametype = _PatientInsuranceRepository.GetMany(i => i.MTBC_Patient_Insurance_Id == obj.MTBC_Patient_Insurance_Id && (i.Deleted ?? false) == false);
                    if (allinsurancesofsametype.Count() > 0)
                    {
                        foreach (var insurance in allinsurancesofsametype)
                        {
                            if (insurance != null && insurance.Patient_Insurance_Id != 0)
                            {
                                insurance.Deleted = true;
                                insurance.Modified_By = profile.UserName;
                                insurance.Modified_Date = Helper.GetCurrentDate();
                                _PatientInsuranceRepository.Update(insurance);
                                _PatientInsuranceRepository.Save();
                                response.Success = true;
                            }
                        }
                    }
                    //Delete From MTBC Insurance Table
                    var mtbcInsurance = _MTBCPatientInsuranceRepository.GetByID(obj.MTBC_Patient_Insurance_Id);
                    if (mtbcInsurance != null)
                    {
                        mtbcInsurance.Deleted = true;
                        mtbcInsurance.Modified_By = profile.UserName;
                        mtbcInsurance.Modified_Date = Helper.GetCurrentDate();
                        _MTBCPatientInsuranceRepository.Update(mtbcInsurance);
                        _MTBCPatientInsuranceRepository.Save();
                        response.Success = true;
                    }
                }
                return response;
            }
            else
            {
                return response;
            }
        }

        public List<PatientAlias> GetPatientAliasListForSpecificPatient(long patient_Account)
        {
            List<PatientAlias> aliasList = _patientAliasRepository.GetMany(e => e.PATIENT_ACCOUNT == patient_Account && e.Deleted == false);
            return aliasList;
        }

        public ResponseModel SavePatientAlias(PatientAlias aliasToCreateUpdate, UserProfile profile)
        {
            var response = new ResponseModel();
            response.Success = false;
            bool isEdit = false;
            if (aliasToCreateUpdate != null)
            {
                long patAcc = long.Parse(aliasToCreateUpdate.PATIENT_ACCOUNT_STR);
                var aliasInfo = new PatientAlias();
                if (aliasToCreateUpdate.PATIENT_ALIAS_ID != 0)  //aliasToCreateUpdate.Is_Update && 
                {
                    var dbAlias = _patientAliasRepository.GetFirst(e => e.PATIENT_ALIAS_ID == aliasToCreateUpdate.PATIENT_ALIAS_ID && !e.Deleted && e.PATIENT_ACCOUNT == patAcc);
                    if (dbAlias != null)
                    {
                        isEdit = true;
                        aliasInfo = dbAlias;
                        aliasInfo.Modified_By = profile.UserName;
                        aliasInfo.Modified_Date = Helper.GetCurrentDate();
                    }
                    else
                    {
                        response.ErrorMessage = "Alias information could not be found.";
                        return response;
                    }
                }
                else
                {
                    aliasInfo.PATIENT_ALIAS_ID = Helper.getMaximumId("FOX_PATIENT_ALIAS_ID");
                    aliasInfo.PATIENT_ACCOUNT = aliasToCreateUpdate.PATIENT_ACCOUNT;
                    aliasInfo.Created_By = aliasInfo.Modified_By = profile.UserName;
                    aliasInfo.Created_Date = aliasInfo.Modified_Date = Helper.GetCurrentDate();
                    aliasInfo.Deleted = false;
                    aliasInfo.ALIAS_TRACKING_NUMBER = aliasInfo.PATIENT_ACCOUNT_STR + "-" + (GetPatientAliasListForSpecificPatient(aliasToCreateUpdate.PATIENT_ACCOUNT.Value).Count + 1);
                }

                aliasInfo.FIRST_NAME = aliasToCreateUpdate.FIRST_NAME;
                aliasInfo.LAST_NAME = aliasToCreateUpdate.LAST_NAME;
                aliasInfo.MIDDLE_INITIALS = aliasToCreateUpdate.MIDDLE_INITIALS;

                if (isEdit)  //aliasToCreateUpdate.Is_Update
                    _patientAliasRepository.Update(aliasInfo);
                else
                    _patientAliasRepository.Insert(aliasInfo);

                _patientAliasRepository.Save();
                response.Success = true;
                return response;
            }
            else
            {
                return response;
            }
        }
        public List<CountryResponse> getCountries(SmartSearchCountriesReq obj)
        {
            if (obj.SEARCHVALUE == null)
                obj.SEARCHVALUE = "";
            if (obj.SEARCHVALUE.Contains("-") || obj.SEARCHVALUE.Contains("]"))
            {
                obj.SEARCHVALUE = obj.SEARCHVALUE.Substring(6).TrimStart();
            }
            //var parmPracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
            //var result = SpRepository<GetSmartPoslocRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_COUNTRIES] @SEARCHVALUE,@PRACTICE_CODE", smartvalue, parmPracticeCode).ToList();
            var result = SpRepository<CountryResponse>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_COUNTRIES] @SEARCHVALUE", smartvalue).ToList();
            if (result.Any())
                return result;
            else
                return new List<CountryResponse>();
        }
        public List<CountryResponse> GetAllCountries(long practiceCode)
        {
            var countriesresult = _CountryRepository.GetMany(x => !x.DELETED && (x.IS_ACTIVE ?? false)).ToList();

            if (countriesresult.Any())
            {
                return countriesresult;
            }
            else
            {
                return new List<CountryResponse>();
            }
        }


        public List<CheckDuplicatePatientsRes> CheckDuplicatePatients(CheckDuplicatePatientsReq CheckDuplicatePatientsReq, UserProfile profile)
        {
            var Patient_Account = Convert.ToInt64(CheckDuplicatePatientsReq.PATIENT_ACCOUNT);
            var _first_Name = new SqlParameter("First_Name", SqlDbType.VarChar) { Value = CheckDuplicatePatientsReq.First_Name ?? "" };
            var _Last_Name = new SqlParameter("Last_Name", SqlDbType.VarChar) { Value = CheckDuplicatePatientsReq.Last_Name ?? "" };
            var date_Of_Birth = Helper.getDBNullOrValue("DOB", CheckDuplicatePatientsReq.Date_Of_Birth_In_String == null ? null : Convert.ToDateTime(CheckDuplicatePatientsReq.Date_Of_Birth_In_String).ToShortDateString());
            var gender = new SqlParameter("gender", SqlDbType.VarChar) { Value = CheckDuplicatePatientsReq.Gender ?? "" };
            var _Patient_account = new SqlParameter("Patient_Account", SqlDbType.BigInt) { Value = Patient_Account };
            var _practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var List = SpRepository<CheckDuplicatePatientsRes>.GetListWithStoreProcedure(@"exec FOX_TBL_GET_DUPLICATE_PATIENTS 
                               @First_Name,@Last_Name,@DOB,@gender,@PRACTICE_CODE,@Patient_Account", _first_Name, _Last_Name, date_Of_Birth, gender, _practiceCode, _Patient_account);
            if (List != null && List.Count > 0)
            {
                foreach (var item in List)
                {
                    item.IS_PATIENT_INTERFACE_SYNCED = checkPatientisInterfaced(item.Patient_Account, profile);
                    var default_POS = _PatientPOSLocationRepository.GetFirst(t => t.Is_Default == true && t.Deleted == false && t.Patient_Account == item.Patient_Account);
                    if (default_POS != null)
                    {
                        var activelocation = _FacilityLocationRepository.GetFirst(t => t.LOC_ID == default_POS.Loc_ID && t.DELETED == false && t.PRACTICE_CODE == profile.PracticeCode);
                        if (activelocation != null)
                        {
                            if (activelocation.NAME != null && activelocation.NAME == "Private Home")
                            {
                                var address = _PatientAddressRepository.GetFirst(t => t.DELETED == false && t.PATIENT_POS_ID == default_POS.Patient_POS_ID);
                                if (address != null)
                                {
                                    item.POS = activelocation.CODE + ",   " + address.ADDRESS + ", " + address.CITY + ", " + address.STATE + activelocation.Zip;
                                }
                            }
                            else
                            {
                                item.POS = activelocation.CODE + ",   " + activelocation.Address + ", " + activelocation.City + ", " + activelocation.State + " " + activelocation.Zip;
                            }
                        }
                    }
                    var primary_insurances = _PatientInsuranceRepository.GetMany(t => t.Pri_Sec_Oth_Type == "P" && t.Deleted == false && t.Patient_Account == item.Patient_Account);
                    if (primary_insurances != null)
                    {

                        item.Primary_INS = "";
                        FinancialClass financial_class = new FinancialClass();
                        foreach (var pri in primary_insurances)
                        {

                            financial_class = _financialClassRepository.GetFirst(t => t.DELETED == false && t.FINANCIAL_CLASS_ID == pri.FINANCIAL_CLASS_ID && t.PRACTICE_CODE == profile.PracticeCode);
                            if (financial_class != null)
                            {
                                if (item.Primary_INS != "")
                                {
                                    item.Primary_INS = item.Primary_INS + ", ";
                                }
                                item.Primary_INS = item.Primary_INS + financial_class.CODE + ": " + pri.Policy_Number;
                            }


                        }
                    }
                    var referral = _workQueueRepository.GetMany(t => item.Patient_Account == t.PATIENT_ACCOUNT && t.DELETED == false && t.WORK_STATUS == "Completed" && t.PRACTICE_CODE == profile.PracticeCode).OrderByDescending(t => t.COMPLETED_DATE).FirstOrDefault();
                    if (referral != null)
                    {
                        item.referral_Completed_By = referral.COMPLETED_BY;
                        item.referral_Completed_On = referral.COMPLETED_DATE;
                        item.Sender_Source = referral.SORCE_NAME;
                        item.Work_ID = referral.WORK_ID;
                        var document_type = _foxdocumenttypeRepository.GetFirst(t => t.DOCUMENT_TYPE_ID == referral.DOCUMENT_TYPE && t.DELETED == false);
                        if (document_type != null)
                        {
                            item.Document_Type = document_type.NAME;
                        }
                        var discipline = "";
                        if (referral != null)
                        {
                            if (!string.IsNullOrEmpty(referral?.DEPARTMENT_ID))
                            {
                                if (referral.DEPARTMENT_ID.Contains("1"))
                                {
                                    discipline = discipline + "(OT), ";
                                }
                                if (referral.DEPARTMENT_ID.Contains("2"))
                                {
                                    discipline = discipline + "(PT), ";
                                }
                                if (referral.DEPARTMENT_ID.Contains("3"))
                                {
                                    discipline = discipline + "(ST), ";
                                }
                                if (referral.DEPARTMENT_ID == "4")
                                {
                                    discipline = discipline + "(PT/OT/ST)";
                                }
                                if (referral.DEPARTMENT_ID == "5")
                                {
                                    discipline = discipline + "Physical/Occupational Therapy(PT/OT)";
                                }
                                if (referral.DEPARTMENT_ID == "6")
                                {
                                    discipline = discipline + "(PT/ST)";
                                }
                                if (referral.DEPARTMENT_ID == "7")
                                {
                                    discipline = discipline + "(OT/ST)";
                                }
                                if (referral.DEPARTMENT_ID.Contains("8"))
                                {
                                    discipline = discipline + " Unknown, ";
                                }
                                if (referral.DEPARTMENT_ID.Contains("9"))
                                {
                                    discipline = discipline + "(EP), ";
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
                            item.Discipline = discipline;
                        }
                    }
                    else
                    {
                        item.referral_Completed_On = null;
                        item.Work_ID = null;
                    }

                }
                return List;

            }
            return null;
        }

        public ResponseModel SaveEligibiltyDocument(DocumentSaveEligibility documentSaveEligibility, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            HtmlDocument eligibility = new HtmlDocument();
            eligibility.LoadHtml(documentSaveEligibility.document_html);
            var main_conatiner = eligibility.DocumentNode.SelectSingleNode("//*[contains(@id,'container')]");
            if (documentSaveEligibility.IS_MVP_VIEW)
            {
                main_conatiner = eligibility.DocumentNode.SelectSingleNode("//*[contains(@class,'container')]");
            }
            if (main_conatiner != null)
            {
                documentSaveEligibility.document_html = main_conatiner.InnerHtml;
            }
            //eligibility_html_string = eligibility.DocumentNode.SelectSingleNode("//*[contains(@id,'container')]").InnerHtml;



            string template_html = string.Empty;
            if (documentSaveEligibility.IS_MVP_VIEW)
            {
                template_html = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/Eligibility_MVP_View_Template.html");
            }
            else
            {
                template_html = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/Eligibilty_View_Template.html");
            }

            template_html = File.ReadAllText(template_html);
            HtmlDocument template = new HtmlDocument();
            template.LoadHtml(template_html);
            var container = template.GetElementbyId("eligibility-container");
            if (container != null && main_conatiner != null)
            {
                //template.DocumentNode.SelectSingleNode("//*[contains(@id,'eligibility-container')]").AppendChild(container1);
                template.GetElementbyId("eligibility-container").AppendChild(main_conatiner);
            }
            var print_button = template.GetElementbyId("Elig_print");
            if (print_button != null)
            {
                template.GetElementbyId("Elig_print").Remove();
            }
            string htmlStr = template.DocumentNode.OuterHtml;

            if (documentSaveEligibility.IS_MVP_VIEW == false)
            {
                var custom_panel = template.DocumentNode.SelectSingleNode("//*[contains(@class,'col-12')]");
                if (custom_panel != null)
                {
                    var custom_panel_nodes = custom_panel.LastChild.ChildNodes;
                    var total_custom_panel = custom_panel_nodes.Count;
                    List<HtmlNodeCollection> list = new List<HtmlNodeCollection>();
                    HtmlNodeCollection nodeCollection = new HtmlNodeCollection(null);
                    if (total_custom_panel > 32)
                    {
                        bool NewDocument = true;
                        for (int i = 0; i < custom_panel_nodes.Count; i++)
                        {

                            nodeCollection.Add(custom_panel_nodes[i]);
                            if ((i % 32 == 0) && i != 0)
                            {
                                list.Add(nodeCollection);
                                nodeCollection = new HtmlNodeCollection(null);
                            }
                        }
                        list.Add(nodeCollection);

                        for (int i = 0; i < list.Count; i++)
                        {

                            custom_panel.LastChild.ChildNodes.Clear();
                            custom_panel.LastChild.AppendChildren(list[i]);
                            htmlStr = template.DocumentNode.OuterHtml;
                            if (i == 0)
                            {
                                var top_header = template.DocumentNode.SelectNodes("//*[contains(@class,'col-6 margin-b10')]");
                                var top_header_date = template.DocumentNode.SelectSingleNode("//*[contains(@class,'custom-panel-head')]");
                                top_header_date.Remove();
                                top_header[0].Remove();
                                top_header[1].Remove();
                            }
                            var file_name = documentSaveEligibility.Patient_Account_Str + "_" + DateTime.Now.Ticks;
                            var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                            ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, htmlStr, file_name.Replace(' ', '_'), documentSaveEligibility.IS_MVP_VIEW);
                            if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                            {
                                string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                                int numberOfPages = getNumberOfPagesOfPDF(filePath);
                                SavePdftoImagesEligibilty(filePath, config, numberOfPages, "Email", profile, documentSaveEligibility.Patient_Account_Str, documentSaveEligibility.Patient_Insurance_Id, NewDocument, documentSaveEligibility.WORK_ID);
                            }
                            NewDocument = false;
                        }
                    }
                    else
                    {
                        var file_name = documentSaveEligibility.Patient_Account_Str + "_" + DateTime.Now.Ticks;
                        var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                        ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, htmlStr, file_name.Replace(' ', '_'), documentSaveEligibility.IS_MVP_VIEW);


                        if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                        {
                            string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                            int numberOfPages = getNumberOfPagesOfPDF(filePath);
                            SavePdftoImagesEligibilty(filePath, config, numberOfPages, "Email", profile, documentSaveEligibility.Patient_Account_Str, documentSaveEligibility.Patient_Insurance_Id, true, documentSaveEligibility.WORK_ID);
                        }
                    }
                    response.Success = true;
                }
            }
            else
            {


                var file_name = documentSaveEligibility.Patient_Account_Str + "_" + DateTime.Now.Ticks;
                var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, htmlStr, file_name.Replace(' ', '_'), documentSaveEligibility.IS_MVP_VIEW);


                if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                {
                    string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                    int numberOfPages = getNumberOfPagesOfPDF(filePath);
                    SavePdftoImagesEligibilty(filePath, config, numberOfPages, "Email", profile, documentSaveEligibility.Patient_Account_Str, documentSaveEligibility.Patient_Insurance_Id, true, documentSaveEligibility.WORK_ID);
                }
                response.Success = true;
            }



            return response;
        }

        public FoxInsurancePayers GetInsuranc(long ID, UserProfile profile)
        {
            var insurance = _foxInsurancePayersRepository.GetFirst(p => p.FOX_TBL_INSURANCE_ID == ID && (p.DELETED ?? false) == false);
            return insurance;
        }

        public POSCoordinates ResetCoordinates(FacilityLocation loc, UserProfile profile)
        {
            POSCoordinates response = new POSCoordinates();
            if (loc != null)
            {
                //try
                //{
                loc.FACILITY_TYPE_NAME = String.IsNullOrEmpty(loc.FACILITY_TYPE_NAME) ? "" : loc.FACILITY_TYPE_NAME;
                if (loc.UpdatePatientAddress == true && loc.FACILITY_TYPE_NAME?.ToLower() == "private home")
                {
                    FacilityLocation patientAddress = new FacilityLocation();

                    var patientPos = _PatientPOSLocationRepository.GetMany(e => e.Patient_Account == loc.PATIENT_ACCOUNT && e.Is_Default == true && e.Loc_ID != 0 && e.Deleted == false).OrderByDescending(t => t.Modified_Date).FirstOrDefault();
                    if (patientPos != null)
                    {
                        PatientAddress address = _PatientAddressRepository.GetFirst(e => e.PATIENT_ACCOUNT == patientPos.Patient_Account && e.PATIENT_POS_ID == patientPos.Patient_POS_ID && e.DELETED == false);
                        if (address != null)
                        {
                            patientAddress.Zip = address.ZIP;
                            patientAddress.City = address.CITY;
                            patientAddress.State = address.STATE;
                            patientAddress.Address = address.ADDRESS;
                            if (loc.SetCoordinatesManually == false)
                            {
                                POSCoordinates coordinates = GetCoordinates(patientAddress);
                                if (coordinates != null)
                                {
                                    address.Latitude = Convert.ToSingle(coordinates.Latitude);
                                    address.Longitude = Convert.ToSingle(coordinates.Longitude);
                                    //address.ADDRESS = coordinates.Address;
                                }

                            }
                            else
                            {
                                address.Latitude = Convert.ToSingle(loc.Latitude);
                                address.Longitude = Convert.ToSingle(loc.Longitude);
                                //address.ADDRESS = loc.Address;
                                //address.CITY = loc.City;
                                //address.STATE = loc.State;
                                //address.ZIP = loc.Zip;
                                //address.COUNTRY = loc.Country;


                            }
                            address.MODIFIED_BY = profile.UserName;
                            address.MODIFIED_DATE = Helper.GetCurrentDate();

                            _PatientAddressRepository.Update(address);
                            _PatientAddressRepository.Save();
                            response.Latitude = address.Latitude.ToString();
                            response.Longitude = address.Longitude.ToString();
                            response.Address = address.ADDRESS;

                        }
                    }
                }
                else
                {
                    FacilityLocation pos = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == loc.LOC_ID && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);
                    if (pos != null)
                    {
                        if (loc.SetCoordinatesManually == false)
                        {
                            POSCoordinates coordinates = GetCoordinates(pos);
                            if (coordinates != null)
                            {
                                pos.Longitude = Convert.ToDouble(coordinates.Longitude);
                                pos.Latitude = Convert.ToDouble(coordinates.Latitude);
                                pos.Address = coordinates.Address;
                            }
                        }
                        else
                        {
                            pos.Longitude = Convert.ToDouble(loc.Longitude);
                            pos.Latitude = Convert.ToDouble(loc.Latitude);
                            //pos.Address = loc.Address;
                            //pos.City = loc.City;
                            //pos.State = loc.State;
                            //pos.Zip = loc.Zip;
                            //pos.Country = loc.Country;


                        }
                        pos.MODIFIED_BY = profile.UserName;
                        pos.MODIFIED_DATE = Helper.GetCurrentDate();
                        _FacilityLocationRepository.Update(pos);
                        _FacilityLocationRepository.Save();
                        response.Latitude = pos.Latitude.ToString();
                        response.Longitude = pos.Longitude.ToString();
                    }
                }
                //}
                //catch (Exception ex)
                //{
                //    return response = new POSCoordinates();
                //}

            }
            return response;
        }
    }
}