using System;
using System.Collections.Generic;
using System.Linq;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.BusinessOperations.CommonService;
using System.Data;
using System.Data.SqlClient;
using FOX.DataModels.Models.TasksModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.ClinicianSetup;
using System.Threading;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.CaseServices
{
    public class CaseServices : ICaseServices
    {
        //context
        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly DbContextPatient _PatientContext = new DbContextPatient();
        private readonly DbContextSettings _dbContextSettings = new DbContextSettings();
        private readonly DbContextCommon _DbContextCommon = new DbContextCommon();
        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();
        //respository
        private readonly GenericRepository<FOX_TBL_CASE> _CaseRepository;
        private readonly GenericRepository<FOX_VW_CASE> _vwCaseRepository;
        private readonly GenericRepository<FOX_TBL_CASE_TYPE> _CaseTypeRepository;
        private readonly GenericRepository<FOX_TBL_DISCIPLINE> _CaseDesciplineRepository;
        private readonly GenericRepository<FOX_TBL_CASE_STATUS> _CaseStatusRepository;
        private readonly GenericRepository<FOX_TBL_CASE_SUFFIX> _CaseSuffixRepository;
        private readonly GenericRepository<FOX_TBL_GROUP_IDENTIFIER> _CaseGrpIdentifierRepository;
        private readonly GenericRepository<FOX_TBL_SOURCE_OF_REFERRAL> _CaseSourceofRefRepository;
        private readonly GenericRepository<FOX_TBL_NOTES> _NotesRepository;
        private readonly GenericRepository<FOX_TBL_NOTES_TYPE> _NotesTypeRepository;
        private readonly GenericRepository<FOX_TBL_ORDER_STATUS> _OrderStatusRepository;
        private readonly GenericRepository<FOX_TBL_ORDER_INFORMATION> _OrderInformationRepository;
        private readonly GenericRepository<DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER> _IdentifierRepository;
        private readonly GenericRepository<FOX_TBL_TASK> _TaskRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TYPE> _TaskTypeRepository;
        private readonly GenericRepository<FOX_TBL_TASK_SUB_TYPE> _TaskSubTypeRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TASK_SUB_TYPE> _TaskTaskSubTypeRepository;
        private readonly GenericRepository<PatientInsurance> _PatientInsuranceRepository;
        private readonly GenericRepository<Patient> _PatientRepository;
        private readonly GenericRepository<OriginalQueue> _WorkOrderQueueRepository;
        private readonly GenericRepository<COMMUNICATION_CALL_STATUS> _CallStatusRepository;
        private readonly GenericRepository<COMMUNICATION_STATUS_OF_CARE> _StatusofCareRepository;
        private readonly GenericRepository<ReferralSource> _SourceRepository;
        private readonly GenericRepository<FOX_VW_CALLS_LOG> _vwCallsLogRepository;
        private readonly GenericRepository<FOX_TBL_CALLS_LOG> _CallsLogRepository;
        private readonly GenericRepository<FOX_TBL_COMMUNICATION_CALL_RESULT> _CallResultRepository;
        private readonly GenericRepository<FOX_TBL_COMMUNICATION_CALL_TYPE> _CallTypeRepository;
        private readonly GenericRepository<FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD> _TaskSubTypeIntelTaskFieldRepository;
        private readonly GenericRepository<PatientPOSLocation> _PatientPOSLocationRepository;
        private readonly GenericRepository<FoxInsurancePayers> _foxInsurancePayersRepository;
        private readonly DbContextTasks _TaskContext = new DbContextTasks();
        private readonly GenericRepository<FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING> _TaskRefMapRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT> _FoxTblPatientRepository;
        private readonly GenericRepository<InterfaceSynchModel> __InterfaceSynchModelRepository;
        private readonly CommonServices.CommonServices _commonService;
        private readonly TaskServices.TaskServices _taskService;
        private readonly GenericRepository<ReferralSource> _OrderingRefSourceRepository;
        private readonly GenericRepository<FacilityLocation> _FacilityLocationRepository;
        private readonly GenericRepository<FoxProviderClass> _FoxProviderClassRepository;
        private readonly GenericRepository<FOX_TBL_CASE_TREATMENT_TEAM> _CaseTreatmentTeamRepository;
        private readonly GenericRepository<FOX_TBL_HOLD_NON_REASONS> _foxTblHoldNonRepository;
        private readonly GenericRepository<Provider> _providerRepository;
        private readonly GenericRepository<ReferralSource> _fox_tbl_ordering_ref_source;


        public CaseServices()
        {
            _CaseRepository = new GenericRepository<FOX_TBL_CASE>(_CaseContext);
            _vwCaseRepository = new GenericRepository<FOX_VW_CASE>(_CaseContext);
            _CaseTypeRepository = new GenericRepository<FOX_TBL_CASE_TYPE>(_CaseContext);
            _CaseDesciplineRepository = new GenericRepository<FOX_TBL_DISCIPLINE>(_CaseContext);
            _CaseStatusRepository = new GenericRepository<FOX_TBL_CASE_STATUS>(_CaseContext);
            _CaseSuffixRepository = new GenericRepository<FOX_TBL_CASE_SUFFIX>(_CaseContext);
            _CaseGrpIdentifierRepository = new GenericRepository<FOX_TBL_GROUP_IDENTIFIER>(_CaseContext);
            _CaseSourceofRefRepository = new GenericRepository<FOX_TBL_SOURCE_OF_REFERRAL>(_CaseContext);
            _NotesRepository = new GenericRepository<FOX_TBL_NOTES>(_CaseContext);
            _NotesTypeRepository = new GenericRepository<FOX_TBL_NOTES_TYPE>(_CaseContext);
            _OrderStatusRepository = new GenericRepository<FOX_TBL_ORDER_STATUS>(_CaseContext);
            _OrderInformationRepository = new GenericRepository<FOX_TBL_ORDER_INFORMATION>(_CaseContext);
            _IdentifierRepository = new GenericRepository<DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER>(_CaseContext);
            _TaskRepository = new GenericRepository<FOX_TBL_TASK>(_CaseContext);
            _TaskTypeRepository = new GenericRepository<FOX_TBL_TASK_TYPE>(_CaseContext);
            _TaskSubTypeRepository = new GenericRepository<FOX_TBL_TASK_SUB_TYPE>(_CaseContext);
            _TaskTaskSubTypeRepository = new GenericRepository<FOX_TBL_TASK_TASK_SUB_TYPE>(_CaseContext);
            _PatientInsuranceRepository = new GenericRepository<PatientInsurance>(_CaseContext);
            _PatientRepository = new GenericRepository<Patient>(_CaseContext);
            _WorkOrderQueueRepository = new GenericRepository<DataModels.Models.OriginalQueueModel.OriginalQueue>(_CaseContext);
            _CallStatusRepository = new GenericRepository<COMMUNICATION_CALL_STATUS>(_CaseContext);
            _StatusofCareRepository = new GenericRepository<COMMUNICATION_STATUS_OF_CARE>(_CaseContext);
            _SourceRepository = new GenericRepository<ReferralSource>(_CaseContext);
            _vwCallsLogRepository = new GenericRepository<FOX_VW_CALLS_LOG>(_CaseContext);
            _CallsLogRepository = new GenericRepository<FOX_TBL_CALLS_LOG>(_CaseContext);
            _CallResultRepository = new GenericRepository<FOX_TBL_COMMUNICATION_CALL_RESULT>(_CaseContext);
            _CallTypeRepository = new GenericRepository<FOX_TBL_COMMUNICATION_CALL_TYPE>(_CaseContext);
            _TaskSubTypeIntelTaskFieldRepository = new GenericRepository<FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD>(_CaseContext);
            _PatientPOSLocationRepository = new GenericRepository<PatientPOSLocation>(_CaseContext);
            _TaskRefMapRepository = new GenericRepository<FOX_TBL_TASKSUBTYPE_REFSOURCE_MAPPING>(_TaskContext);
            _FoxTblPatientRepository = new GenericRepository<FOX_TBL_PATIENT>(_PatientContext);
            __InterfaceSynchModelRepository = new GenericRepository<InterfaceSynchModel>(_CaseContext);
            _commonService = new CommonServices.CommonServices();
            _foxInsurancePayersRepository = new GenericRepository<FoxInsurancePayers>(_PatientContext);
            _taskService = new TaskServices.TaskServices();
            _OrderingRefSourceRepository = new GenericRepository<ReferralSource>(_PatientContext);
            _FacilityLocationRepository = new GenericRepository<FacilityLocation>(_PatientContext);
            _FoxProviderClassRepository = new GenericRepository<FoxProviderClass>(_dbContextSettings);
            _CaseTreatmentTeamRepository = new GenericRepository<FOX_TBL_CASE_TREATMENT_TEAM>(_CaseContext);
            _foxTblHoldNonRepository = new GenericRepository<FOX_TBL_HOLD_NON_REASONS>(_CaseContext);
            _providerRepository = new GenericRepository<Provider>(_DbContextCommon);
            _fox_tbl_ordering_ref_source = new GenericRepository<ReferralSource>(_IndexinfoContext);

        }

        public ResponseAddEditCase AddEditCase(string locationName, string certifyState, FOX_TBL_CASE model, UserProfile profile) //Get Case data
        {
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            ResponseAddEditCase responseAddEditCase = new ResponseAddEditCase();
            try
            {
                long? provider_code = 0;
                long? reffral_code = 0;
                if (profile.isTalkRehab)
                {
                    provider_code = getProviderCode(model);
                    reffral_code = getReffralCode(model);

                    if(provider_code == 0)
                    {
                        responseAddEditCase.Message = "Provider not Exist.";
                        return responseAddEditCase;
                    }
                    else if(reffral_code == 0)
                    {
                        responseAddEditCase.Message = "Referring Physicain not Exist.";
                        return responseAddEditCase;
                    }
                }

                var restOfPatientData = new FOX_TBL_PATIENT();
                if (model != null && profile != null)
                {

                    var caseObj = _CaseRepository.GetFirst(t => t.CASE_ID == model.CASE_ID);
                    bool IsEditCase = false;
                    bool IsSameStatus = false;
                    string OldCaseStatus = "";
                    
                    if (caseObj == null)
                    {
                        if (profile.isTalkRehab)
                        {
                            var CaseStatus = _CaseStatusRepository.GetSingle(t => t.CASE_STATUS_ID == model.CASE_STATUS_ID);
                            if(CaseStatus.DESCRIPTION == "Discharged")
                            {
                                responseAddEditCase.Message = "Discharged case cannot be created.";
                                return responseAddEditCase;
                            }
                            else if (CaseStatus.DESCRIPTION == "Non-Admission")
                            {
                                responseAddEditCase.Message = "Refused case cannot be created.";
                                return responseAddEditCase;
                            }
                        }
                        caseObj = new FOX_TBL_CASE();
                        interfaceSynch.CASE_ID = caseObj.CASE_ID = Helper.getMaximumId("FOX_TBL_CASE_ID");
                        caseObj.PRACTICE_CODE = profile.PracticeCode;
                        caseObj.CREATED_BY = caseObj.MODIFIED_BY = profile.UserName;
                        caseObj.CREATED_DATE = caseObj.MODIFIED_DATE = DateTime.Now;
                        IsEditCase = false;
                        responseAddEditCase.Message = "Case inserted successfully.";
                    }
                    else
                    {
                        interfaceSynch.CASE_ID = model.CASE_ID;
                        caseObj.MODIFIED_BY = profile.UserName;
                        caseObj.MODIFIED_DATE = DateTime.Now;
                        IsEditCase = true;
                        responseAddEditCase.Message = "Case edited successfully.";
                        if (profile.isTalkRehab)
                        {
                            if (model.CASE_STATUS_ID == caseObj.CASE_STATUS_ID)
                            {
                                IsSameStatus = true;
                            }
                            else
                            {
                                var OldCaseStatusResponse = _CaseStatusRepository.GetSingle(t => t.CASE_STATUS_ID == caseObj.CASE_STATUS_ID);
                                if(OldCaseStatusResponse.NAME == "Act")
                                {
                                    OldCaseStatus = OldCaseStatusResponse.DESCRIPTION;
                                }
                                else if(OldCaseStatusResponse.NAME == "Dc")
                                {
                                    OldCaseStatus = OldCaseStatusResponse.DESCRIPTION;

                                }
                                else
                                {
                                    OldCaseStatus = OldCaseStatusResponse.NAME;
                                }
                                IsSameStatus = false;
                            }
                            if (!string.IsNullOrWhiteSpace(model.NON_ADMIT_REASON))
                            {
                                caseObj.NON_ADMIT_REASON = model.NON_ADMIT_REASON;
                            }
                        }
                    }
                    /////////////////                                
                    var _patient_Account = Convert.ToInt64(model.PATIENT_ACCOUNT_STR);

                    caseObj.PATIENT_ACCOUNT_STR = model.PATIENT_ACCOUNT_STR;
                    interfaceSynch.PATIENT_ACCOUNT = caseObj.PATIENT_ACCOUNT = _patient_Account;
                    caseObj.CASE_TYPE_ID = model.CASE_TYPE_ID;
                    caseObj.DISCIPLINE_ID = model.DISCIPLINE_ID;
                    caseObj.CASE_STATUS_ID = model.CASE_STATUS_ID;
                    caseObj.IsWellness = model.IsWellness;
                    caseObj.IsSkilled = model.IsSkilled;
                    caseObj.DISCHARGE_DATE = model.DISCHARGE_DATE;
                    caseObj.TREATING_PROVIDER_ID = model.TREATING_PROVIDER_ID;

                    var foxpatientDetail = _FoxTblPatientRepository.GetFirst(e => e.Patient_Account == _patient_Account);
                    var CaseName = _CaseStatusRepository.GetSingle(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CASE_STATUS_ID == model.CASE_STATUS_ID).NAME;
                    var activecases = _vwCaseRepository.GetMany(c => c.PATIENT_ACCOUNT == _patient_Account && c.CASE_ID != model.CASE_ID && c.DELETED == false && c.CASE_STATUS_NAME.ToUpper() == "ACT");
                    if (CaseName.ToUpper() == "ACT")
                    {
                        if (foxpatientDetail != null)
                        {
                            if (string.IsNullOrEmpty(foxpatientDetail.Patient_Status) || foxpatientDetail.Patient_Status == "Inactive")
                            {
                                restOfPatientData = foxpatientDetail;
                                restOfPatientData.Patient_Status = "Active";
                                _FoxTblPatientRepository.Update(restOfPatientData);
                                _FoxTblPatientRepository.Save();
                            }
                        }
                    }
                    else if (CaseName.ToUpper() == "DC")
                    {
                        if (foxpatientDetail != null)
                        {
                            if (!string.IsNullOrEmpty(foxpatientDetail.Patient_Status))
                            {
                                if (string.IsNullOrEmpty(foxpatientDetail.Patient_Status) || foxpatientDetail.Patient_Status == "Active")
                                {
                                    if (activecases.Count == 0)
                                    {
                                        restOfPatientData = foxpatientDetail;
                                        restOfPatientData.Patient_Status = "Inactive";
                                        _FoxTblPatientRepository.Update(restOfPatientData);
                                        _FoxTblPatientRepository.Save();
                                    }
                                }
                            }
                        }
                    }
                    caseObj.CASE_SUFFIX_ID = model.CASE_SUFFIX_ID;
                    caseObj.GROUP_IDENTIFIER_ID = model.GROUP_IDENTIFIER_ID;
                    caseObj.TREATING_PROVIDER_ID = model.TREATING_PROVIDER_ID;
                    caseObj.POS_ID = model.POS_ID;
                    caseObj.TREATING_REGION_ID = model.TREATING_REGION_ID;
                    caseObj.IS_MANUAL_CHANGE_REGION = model.IS_MANUAL_CHANGE_REGION;
                    if (!string.IsNullOrEmpty(model.ADMISSION_DATE_String))
                        caseObj.ADMISSION_DATE = Convert.ToDateTime(model.ADMISSION_DATE_String); //edded by faheem
                    if (!string.IsNullOrEmpty(model.END_CARE_DATE_String))
                        caseObj.END_CARE_DATE = Convert.ToDateTime(model.END_CARE_DATE_String); //edded by faheem
                    else
                        caseObj.END_CARE_DATE = null;

                    if (!string.IsNullOrEmpty(model.START_CARE_DATE_String))
                        caseObj.START_CARE_DATE = Convert.ToDateTime(model.START_CARE_DATE_String); //edded by faheem
                    caseObj.VISIT_PER_WEEK = model.VISIT_PER_WEEK;
                    caseObj.TOTAL_VISITS = null;
                    caseObj.WORK_ID = model.WORK_ID;
                    caseObj.CASE_NO = model.CASE_NO;
                    caseObj.ORDERING_REF_SOURCE_ID = model.ORDERING_REF_SOURCE_ID;
                    caseObj.ACU_IDENTIFIER_ID = model.ACU_IDENTIFIER_ID;
                    caseObj.HOSPITAL_IDENTIFIER_ID = model.HOSPITAL_IDENTIFIER_ID;
                    caseObj.SNF_IDENTIFIER_ID = model.SNF_IDENTIFIER_ID;
                    caseObj.HHH_IDENTIFIER_ID = model.HHH_IDENTIFIER_ID;
                    caseObj.REF_REGION_ID = model.REF_REGION_ID;
                    caseObj.SOURCE_OF_REFERRAL_ID = model.SOURCE_OF_REFERRAL_ID;
                    caseObj.PRIMARY_PHY_ID = model.PRIMARY_PHY_ID;
                    if(caseObj.PRIMARY_PHY_ID!=null && caseObj.PRIMARY_PHY_ID!=0)
                    {
                        var caseStatusList = _CaseStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode);
                        caseStatusList = caseStatusList.FindAll(e => e.NAME.ToLower() == "act" || e.NAME.ToLower() == "open");
                        var CASE_STATUS_ID_ACTIVE = caseStatusList.Find(e => e.NAME.ToLower() == "act").CASE_STATUS_ID;
                        var CASE_STATUS_ID_OPEN = caseStatusList.Find(e => e.NAME.ToLower() == "open").CASE_STATUS_ID;
                        if (caseObj.CASE_STATUS_ID == CASE_STATUS_ID_ACTIVE || caseObj.CASE_STATUS_ID==CASE_STATUS_ID_OPEN)
                        {
                            UpdatePCPInPatientDemographics(caseObj.PRIMARY_PHY_ID, _patient_Account, profile.UserName);
                            UpdatePrimaryPhysicianInActiveandOpenCases(caseObj.PRIMARY_PHY_ID, _patient_Account, profile.PracticeCode);
                        }
                    }
                    caseObj.HEAR_ABOUT_US_ID = model.HEAR_ABOUT_US_ID;
                    caseObj.CERTIFYING_REF_SOURCE_ID = model.CERTIFYING_REF_SOURCE_ID;
                    caseObj.NO_OF_WEEK = model.NO_OF_WEEK;
                    caseObj.PATIENT_RESP_INS_ID = model.PATIENT_RESP_INS_ID;
                    caseObj.HOLD_DATE = model.HOLD_DATE;
                    if (profile.isTalkRehab && model.CASE_STATUS_ID== 600102)
                    {
                        caseObj.HOLD_DURATION = "Pending Assignment";
                    }
                    else
                    {
                        caseObj.HOLD_DURATION = model.HOLD_DURATION;
                    }
                    caseObj.HOLD_TILL_DATE = model.HOLD_TILL_DATE;
                    if (!string.IsNullOrEmpty(model.HOLD_FOLLOW_UP_DATE_String))
                        caseObj.HOLD_FOLLOW_UP_DATE = Convert.ToDateTime(model.HOLD_FOLLOW_UP_DATE_String); //edded by faheem
                    else if(string.IsNullOrEmpty(model.HOLD_FOLLOW_UP_DATE_String))
                    {
                        caseObj.HOLD_FOLLOW_UP_DATE = null;
                    }
                    if (model.OrderInformationList != null && model.OrderInformationList.Count > 0)
                    {
                        AddEditOrderInformation(caseObj.CASE_ID, model.OrderInformationList, profile);
                    }
                    if (model.OrderInformationList_deleted != null && model.OrderInformationList_deleted.Count > 0)
                    {
                        AddEditOrderInformation(caseObj.CASE_ID, model.OrderInformationList_deleted, profile);
                    }
                    if (model.CallsLogList != null && model.CallsLogList.Count > 0)
                    {
                        AddEditCommunicationCalls(caseObj.CASE_ID, model.CallsLogList, profile);
                    }
                    if (model.CallsLogList_deleted != null && model.CallsLogList_deleted.Count > 0)
                    {
                        AddEditCommunicationCalls(caseObj.CASE_ID, model.CallsLogList_deleted, profile);
                    }
                    if (!string.IsNullOrWhiteSpace(model.Comments))
                    {
                        //Case Order Information Comments
                        AddEditNotes(new AddEditNotesViewModel() { NotesType = "Case Order Information Comments", Notes = model.Comments, CASE_ID = caseObj.CASE_ID }, profile);
                    }
                    if (!string.IsNullOrWhiteSpace(model.ImportantNotes))
                    {
                        AddEditNotes(new AddEditNotesViewModel() { NotesType = "Case Important Notes", Notes = model.ImportantNotes, CASE_ID = caseObj.CASE_ID }, profile);
                    }
                    if (!string.IsNullOrWhiteSpace(model.VoidReason))
                    {
                        AddEditNotes(new AddEditNotesViewModel() { NotesType = "Case Void Reason", Notes = model.VoidReason, CASE_ID = caseObj.CASE_ID }, profile);
                    }

                    if (IsEditCase)
                    {
                        _CaseRepository.Update(caseObj);
                        //if (model.Is_Chnage)
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //InsertInterfaceTeamData(interfaceSynch, profile);

                    }
                    else
                    {
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //InsertInterfaceTeamData(interfaceSynch, profile);
                        _CaseRepository.Insert(caseObj);

                    }
                    _CaseContext.SaveChanges();

                    if (profile.isTalkRehab)
                    {
                        InsertDataAdditionalInfo(locationName, certifyState, caseObj, provider_code, reffral_code,IsEditCase, profile, IsSameStatus, OldCaseStatus, model);
                    }

                    //Create tasks
                    if (model.openIssueList != null)
                    {
                        //var checkWeekTaskType = model.openIssueList.Find(e => e.TaskSubTypeList.Exists(t => t.TASK_SUB_TYPE.Contains("Week") && t.IS_CHECKED));
                        //if (checkWeekTaskType == null || !model.openIssueList.Exists(e => e.TaskTypeName.ToLower().Contains("call patient")))
                        //{
                        //AddEditTasks(caseObj.CASE_ID, caseObj.PATIENT_ACCOUNT, model.openIssueList, profile);
                        //}
                        var CASE_STATUS_ID_HOLD = _CaseStatusRepository.GetFirst(e => e.NAME.ToLower() == "hold").CASE_STATUS_ID;
                        if (CASE_STATUS_ID_HOLD != caseObj.CASE_STATUS_ID)
                        {
                            AddEditTasks(caseObj.CASE_ID, caseObj.PATIENT_ACCOUNT, model.openIssueList, profile);
                        }
                    }

                    responseAddEditCase.CASE_ID = caseObj.CASE_ID;
                }
                responseAddEditCase.Success = true;
                responseAddEditCase.ErrorMessage = "Case created successfully.";
            }
            catch (Exception exception)
            {
                //To Do Log Exception Here
                //throw;
                responseAddEditCase.Message = exception.Message;
                responseAddEditCase.Success = false;
                responseAddEditCase.ErrorMessage = exception.ToString();
            }
            return responseAddEditCase;
        }

        public void InsertDataAdditionalInfo(string locationName, string certifyState, FOX_TBL_CASE model, long? provider_code, long? reffral_code, bool isEditcase, UserProfile profile, bool IsSameStatus, string OldCaseStatus, FOX_TBL_CASE updateModel)
        {
            try
            {
                var reasonHistory = "";
                string NewCaseStatus = "";
                string changeReason = "";
                var caseStatus = _CaseStatusRepository.GetSingle(t => t.CASE_STATUS_ID == model.CASE_STATUS_ID);
                if (isEditcase)
                {
                    if (caseStatus.NAME == "Act")
                    {
                        NewCaseStatus = caseStatus.DESCRIPTION;
                    }
                    else if (caseStatus.NAME == "Dc")
                    {
                        NewCaseStatus = caseStatus.DESCRIPTION;

                    }
                    else
                    {
                        NewCaseStatus = caseStatus.NAME;
                    }
                    if (!IsSameStatus)
                    {
                        reasonHistory = "Case status changed from " + OldCaseStatus + " to " + NewCaseStatus + ".";
                    }

                }
                else
                {
                    reasonHistory = "Case# " + model.CASE_NO + " created by " + profile.FirstName + " " + profile.LastName + ".";
                }
                if (caseStatus.NAME == "Pending")
                {
                    foreach (var open in updateModel.openIssueList)
                        if (open.Notes != null)
                        {
                            open.Notes = open.Notes.Replace("<p>", "");
                            open.Notes = open.Notes.Replace("</p>", "");
                            open.Notes = open.Notes.Replace("<br>", "");
                            changeReason += open.Notes + " ";
                        }
                }
                else
                {
                    if (updateModel.NON_ADMIT_REASON != null)
                    {
                        updateModel.NON_ADMIT_REASON = updateModel.NON_ADMIT_REASON.Replace("<br>", "");
                    }
                    changeReason = updateModel.NON_ADMIT_REASON;
                }

                var case_id = new SqlParameter { ParameterName = "case_id", SqlDbType = SqlDbType.BigInt, Value = model.CASE_ID };
                var reason = new SqlParameter { ParameterName = "reason", SqlDbType = SqlDbType.NVarChar, Value = changeReason == null ? " " : changeReason };
                var providerid = new SqlParameter { ParameterName = "providerid", SqlDbType = SqlDbType.BigInt, Value = provider_code };
                var ref_provider_id = new SqlParameter { ParameterName = "ref_provider_id", SqlDbType = SqlDbType.BigInt, Value = reffral_code };
                var pos = new SqlParameter { ParameterName = "pos", SqlDbType = SqlDbType.NVarChar, Value = locationName };
                var region = new SqlParameter { ParameterName = "region", SqlDbType = SqlDbType.NVarChar, Value = certifyState };
                var patient_account = new SqlParameter { ParameterName = "patient_account", SqlDbType = SqlDbType.BigInt, Value = model.PATIENT_ACCOUNT };
                var case_status_id = new SqlParameter { ParameterName = "case_status_id", SqlDbType = SqlDbType.BigInt, Value = model.CASE_STATUS_ID };
                var is_edit_case = new SqlParameter { ParameterName = "is_edit_case", SqlDbType = SqlDbType.BigInt, Value = isEditcase };
                var result = SpRepository<CaseAdditionalInfoRresponce>.GetListWithStoreProcedure(@"exec AF_PROC_CASE_ADDITIONAL_INFO @case_id, @reason, @providerid,@ref_provider_id,@pos,@region,@patient_account,@case_status_id, @is_edit_case",
                case_id, reason, providerid, ref_provider_id, pos, region, patient_account, case_status_id, is_edit_case);
                
                var case_id_Param = new SqlParameter("@Case_Id", SqlDbType.BigInt) { Value = model.CASE_ID };
                var case_no_Param = new SqlParameter("@Case_No", SqlDbType.VarChar) { Value = model.CASE_NO };
                var username_Param = new SqlParameter("@UserName", SqlDbType.VarChar) { Value = profile.UserName };
                var History_Time_Param = new SqlParameter("@History_Time", SqlDbType.VarChar) { Value = DateTime.Now };
                var details_Param = new SqlParameter("@Details", SqlDbType.VarChar) { Value = reasonHistory };
                var reason_Param = new SqlParameter("@CaseReason", SqlDbType.VarChar) { Value = changeReason == null? " " : changeReason };
                var practice_code_Param = new SqlParameter("@Practice_Code", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var insertHistory = SpRepository<CaseAdditionalInfoRresponce>.GetListWithStoreProcedure(@"exec AF_PROC_ADD_CASE_HISTORY @Case_Id, @Case_No, @UserName, @History_Time, @Details, @CaseReason, @Practice_Code",
               case_id_Param, case_no_Param, username_Param, History_Time_Param, details_Param, reason_Param, practice_code_Param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long? getProviderCode(FOX_TBL_CASE model)
        {
            var code = _providerRepository.GetSingle(t => t.FOX_PROVIDER_ID == model.TREATING_PROVIDER_ID);
            if ( code.PROVIDER_CODE == null)
            {
                return 0;
            }
            else
            {
                return code.PROVIDER_CODE;
            }
        }
        
        public long? getReffralCode(FOX_TBL_CASE model)
        {
            var code = _fox_tbl_ordering_ref_source.GetSingle(t => t.SOURCE_ID == model.CERTIFYING_REF_SOURCE_ID);
            if(code.REFERRAL_CODE == null)
            {
                return 0;
            }
            else
            {
                return code.REFERRAL_CODE;
            }
        }

        private List<OpenIssueList> CreateIntelligentTasks(long patientAccount, long caseId, List<OpenIssueList> openIssueList, UserProfile profile)
        {
            try
            {
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var UserId = new SqlParameter { ParameterName = "USER_ID", SqlDbType = SqlDbType.BigInt, Value = profile.userID };
                var _TASK_SUB_TYPE_ID = new SqlParameter { ParameterName = "TASK_SUB_TYPE_ID", SqlDbType = SqlDbType.Int, Value = 0 };
                var result = SpRepository<CatFieldRes>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_INTEL_TEMPL_LIST @PRACTICE_CODE, @USER_ID, @TASK_SUB_TYPE_ID",
                          PracticeCode, UserId, _TASK_SUB_TYPE_ID);


                if (result != null && openIssueList != null)
                {
                    foreach (var taskType in openIssueList)
                    {
                        //taskType.TaskSubTypeList = taskType.TaskSubTypeList.Where(x => !x.IS_CHECKED).ToList();
                        var taskSubTypeList = taskType.TaskSubTypeList.Where(x => !x.IS_CHECKED).ToList();


                        foreach (var taskSubType in taskSubTypeList)
                        {
                            //TASK_CATEGORY = Demographics
                            var demographics = result.Where(t => t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID && t.CATEGORY_NAME.Equals("Demographics"));
                            if (demographics != null && demographics.Any())
                            {
                                //Check in patient table
                                var patient = _PatientRepository.GetFirst(t => !(t.DELETED ?? false) && t.Practice_Code == profile.PracticeCode && t.Patient_Account == patientAccount);
                                if (patient != null)
                                {
                                    //_PatientPOSLocationRepository
                                    var patientPOSLocation = _PatientPOSLocationRepository.GetMany(t => !(t.Deleted ?? false) && t.Patient_Account == patient.Patient_Account);
                                    var patientInsurance = _PatientInsuranceRepository.GetMany(t => !(t.Deleted ?? false) && t.Patient_Account == patient.Patient_Account);
                                    if (
                                        ((patientPOSLocation == null || !patientPOSLocation.Any()) && demographics.Any(t => t.FIELD_NAME.Equals("Place of Service Address")))
                                        || ((patientInsurance == null || !patientInsurance.Any()) && demographics.Any(t => t.FIELD_NAME.Equals("Patient Insurance")))
                                        || (string.IsNullOrWhiteSpace(patient.cell_phone) && demographics.Any(t => t.FIELD_NAME.Equals("Cell Phone")))
                                        || (string.IsNullOrWhiteSpace(patient.Home_Phone) && demographics.Any(t => t.FIELD_NAME.Equals("Home Phone")))
                                        )
                                    {
                                        if (caseId == 0)
                                        {
                                            taskSubType.IS_CHECKED = true;
                                        }
                                    }
                                }
                            }
                            //TASK_CATEGORY = Index Info Source Information
                            var indexInfoSourceInfo = result.Where(t => t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID && t.CATEGORY_NAME.Equals("Index Info Source Information"));
                            if (indexInfoSourceInfo != null && indexInfoSourceInfo.Any())
                            {
                                //Check in WORK_QUEUE table
                                var workQueue = _WorkOrderQueueRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.PATIENT_ACCOUNT == patientAccount);
                                if (workQueue != null)
                                {
                                    var work_id = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = workQueue.WORK_ID };
                                    var patientDiagnosis = SpRepository<FOX_TBL_PATIENT_DIAGNOSIS>.GetListWithStoreProcedure(@"exec [FOX_GET_INDEX_INFO_DIAGNOSIS] @WORK_ID", work_id);
                                    if ((patientDiagnosis == null || !patientDiagnosis.Any()) && indexInfoSourceInfo.Any(t => t.FIELD_NAME.Equals("ICD-10 Diagnosis")))
                                    {
                                        if (caseId == 0)
                                        {
                                            taskSubType.IS_CHECKED = true;
                                        }
                                    }
                                    //Check for Like
                                    var taskRefMapLike = _TaskRefMapRepository
                                        .GetMany(
                                            t => t.CREATED_BY.Equals(profile.UserName)
                                            && t.SOURCE_ID == workQueue.SENDER_ID
                                            && t.CONDITION.Equals("Like")
                                            && !t.DELETED
                                            && t.PRACTICE_CODE == profile.PracticeCode
                                            );
                                    //Check for Not Like
                                    var taskRefMapNotLike = _TaskRefMapRepository
                                        .GetMany(
                                            t => t.CREATED_BY.Equals(profile.UserName)
                                            && t.SOURCE_ID == workQueue.SENDER_ID
                                            && t.CONDITION.Equals("Not Like")
                                            && !t.DELETED
                                            && t.PRACTICE_CODE == profile.PracticeCode
                                            );
                                    if ((taskRefMapLike != null && taskRefMapLike.Any() && (taskRefMapNotLike == null || !taskRefMapNotLike.Any())) && indexInfoSourceInfo.Any(t => t.FIELD_NAME.Equals("Ordering Referral Source")))
                                    {
                                        if (caseId == 0)
                                        {
                                            taskSubType.IS_CHECKED = true;
                                        }
                                    }
                                }
                            }
                            //TASK_CATEGORY = Housecall Case
                            var housecallCase = result.Where(t => t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID && t.CATEGORY_NAME.Equals("Housecall Case"));
                            if (housecallCase != null && housecallCase.Any())
                            {
                                //Check in CASE table
                                var _case = _CaseRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CASE_ID == caseId);
                                if (_case != null)
                                {
                                    var orderInfo = _case.OrderInformationList?.OrderByDescending(t => t.MODIFIED_DATE)?.FirstOrDefault() ?? null;
                                    if (orderInfo != null)
                                    {
                                        //Check for Like
                                        var taskRefMapLike = _TaskRefMapRepository
                                            .GetMany(
                                                t => t.CREATED_BY.Equals(profile.UserName)
                                                && t.ORDER_STATUS_ID == orderInfo.ORDER_STATUS_ID
                                                && t.CONDITION.Equals("Like")
                                                && !t.DELETED
                                                && t.PRACTICE_CODE == profile.PracticeCode
                                                );
                                        //Check for Not Like
                                        var taskRefMapNotLike = _TaskRefMapRepository
                                            .GetMany(
                                                t => t.CREATED_BY.Equals(profile.UserName)
                                                && t.ORDER_STATUS_ID == orderInfo.ORDER_STATUS_ID
                                                && t.CONDITION.Equals("Not Like")
                                                && !t.DELETED
                                                && t.PRACTICE_CODE == profile.PracticeCode
                                                );
                                        if ((taskRefMapLike != null && taskRefMapLike.Any() && (taskRefMapNotLike == null || !taskRefMapNotLike.Any())) && housecallCase.Any(t => t.FIELD_NAME.Equals("Order Status")))
                                        {
                                            if (caseId == 0)
                                            {
                                                taskSubType.IS_CHECKED = true;
                                            }
                                        }
                                    }
                                    if (((_case.TREATING_PROVIDER_ID == null || _case.TREATING_PROVIDER_ID == 0) && housecallCase.Any(t => t.FIELD_NAME.Equals("Treating Provider")))
                                        || ((_case.ORDERING_REF_SOURCE_ID == null || _case.ORDERING_REF_SOURCE_ID == 0) && housecallCase.Any(t => t.FIELD_NAME.Equals("Ordering Referral Source")))
                                        )
                                    {
                                        if (caseId == 0)
                                        {
                                            taskSubType.IS_CHECKED = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //AddEditTasks(caseObj.CASE_ID, openIssueList, profile);
                return openIssueList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddEditTasks(long caseId, long patient_account, List<OpenIssueList> openIssueList, UserProfile profile)
        {
            try
            {
                foreach (var openIssue in openIssueList)
                {
                    var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    var _taskTId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = -1 };
                    var _taskTypeId = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = openIssue.TASK_TYPE_ID };
                    var _isTemplate = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = true };
                    var taskTemplate = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_TASK_BY_TASK_TYPE_ID 
                               @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCode, _taskTId, _taskTypeId, _isTemplate);

                    if (openIssue.TaskSubTypeList.Any(t => t.IS_CHECKED))
                    {
                        var task = _TaskRepository.GetFirst(row => row.CASE_ID == caseId && row.TASK_TYPE_ID == openIssue.TASK_TYPE_ID && row.PRACTICE_CODE == profile.PracticeCode);

                        var _case = _CaseRepository.GetFirst(row => row.PRACTICE_CODE == profile.PracticeCode && row.CASE_ID == caseId);

                        if (task == null)
                        {
                            task = new FOX_TBL_TASK();
                            task.PRACTICE_CODE = profile.PracticeCode;
                            task.CASE_ID = caseId;
                            task.PATIENT_ACCOUNT = patient_account;
                            task.TASK_TYPE_ID = openIssue.TASK_TYPE_ID;
                            if (taskTemplate != null)
                            {
                                task.SEND_TO_ID = taskTemplate.SEND_TO_ID;
                                task.IS_SEND_TO_USER = taskTemplate.IS_SEND_TO_USER;
                                task.FINAL_ROUTE_ID = taskTemplate.FINAL_ROUTE_ID;
                                task.IS_FINAL_ROUTE_USER = taskTemplate.IS_FINAL_ROUTE_USER;
                                task.PRIORITY = taskTemplate.PRIORITY;
                                //task.DUE_DATE_TIME = taskTemplate.DUE_DATE_TIME;
                                if (task.DUE_DATE_TIME == null)
                                {
                                    task.DUE_DATE_TIME = Helper.GetCurrentDate().AddDays(2);
                                    task.DUE_DATE_TIME_str = task.DUE_DATE_TIME?.ToString("MM'/'dd'/'yyyy");
                                }
                                if (taskTemplate.PROVIDER_ID == null)
                                {
                                    task.PROVIDER_ID = _case.TREATING_PROVIDER_ID;
                                }
                                if (taskTemplate.LOC_ID == null)
                                {
                                    task.LOC_ID = _case.POS_ID;
                                }
                            }
                            else if (task.DELETED)
                            {
                                if (taskTemplate != null)
                                {
                                    task.SEND_TO_ID = taskTemplate.SEND_TO_ID;
                                    task.FINAL_ROUTE_ID = taskTemplate.FINAL_ROUTE_ID;
                                    task.PRIORITY = taskTemplate.PRIORITY;
                                    task.DUE_DATE_TIME = taskTemplate.DUE_DATE_TIME;
                                    task.IS_TEMPORARY_DELETED = false;
                                }
                                task.DELETED = false;
                            }

                            if (!string.IsNullOrEmpty(openIssue.Notes))
                            {
                                task.COMMENTS = openIssue.Notes;
                            }
                            else
                            {
                                if(taskTemplate != null)
                                {
                                    var _template = _NotesRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.TASK_ID == taskTemplate.TASK_ID);
                                    if (_template != null && !string.IsNullOrEmpty(_template.NOTES))
                                    {
                                        task.COMMENTS = _template.NOTES;
                                    }
                                }
                            }
                        }
                        task.TASK_ALL_SUB_TYPES_LIST = openIssue.TaskSubTypeList;
                        _taskService.AddUpdateTask(task, profile);
                        _CaseContext.SaveChanges();

                        if (openIssue.TaskSubTypeList.Any(t => !t.IS_CHECKED && t.TASK_ID.HasValue && t.TASK_ID.Value != 0))
                        {
                            var task_Id = openIssue.TaskSubTypeList.Where(e => e.TASK_ID.HasValue && e.TASK_ID.Value != 0).FirstOrDefault().TASK_ID;
                            UnmarkTaskSubtypes(task_Id.Value, openIssue.TaskSubTypeList.Where(e => !e.IS_CHECKED).ToList(), profile);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void AddUpdateTaskNotes(AddEditNotesViewModel addEditNotesViewModel, UserProfile profile)
        {
            var dbNotes = _NotesRepository.GetFirst(x => x.PRACTICE_CODE == profile.PracticeCode && x.TASK_ID == addEditNotesViewModel.TASK_ID);
            if (dbNotes != null)
            {
                dbNotes.NOTES = addEditNotesViewModel.Notes;
                dbNotes.MODIFIED_BY = profile.UserName;
                dbNotes.MODIFIED_DATE = Helper.GetCurrentDate();
                dbNotes.DELETED = false;
                _NotesRepository.Update(dbNotes);
                _NotesRepository.Save();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(addEditNotesViewModel.Notes))
                {
                    FOX_TBL_NOTES notes = new FOX_TBL_NOTES();
                    notes.NOTES_ID = Helper.getMaximumId("FOX_TBL_NOTES_ID");
                    notes.PRACTICE_CODE = profile.PracticeCode;
                    notes.TASK_ID = addEditNotesViewModel.TASK_ID;
                    notes.NOTES_TYPE_ID = _NotesTypeRepository.GetSingle(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.NAME == "Tasks Comments").NOTES_TYPE_ID;
                    notes.NOTES = addEditNotesViewModel.Notes;
                    notes.CREATED_BY = profile.UserName;
                    notes.CREATED_DATE = Helper.GetCurrentDate();
                    notes.MODIFIED_BY = profile.UserName;
                    notes.MODIFIED_DATE = Helper.GetCurrentDate();
                    notes.DELETED = false;
                    _NotesRepository.Insert(notes);
                    _NotesRepository.Save();
                }
            }
        }

        private void UnmarkTaskSubtypes(long task_Id, List<OpenIssueViewModel> uncheckedSubTypeList, UserProfile profile)
        {
            try
            {
                foreach (var taskSubType in uncheckedSubTypeList)
                {
                    var taskTaskSubType = _TaskTaskSubTypeRepository
                       .GetFirst(
                                   t =>
                                       t.TASK_ID == task_Id
                                       && t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID
                                       && !t.DELETED
                                       && t.PRACTICE_CODE == profile.PracticeCode
                               );

                    if (taskTaskSubType != null && taskTaskSubType.TASK_SUB_TYPE_ID.HasValue && !taskTaskSubType.DELETED)
                    {
                        taskTaskSubType.DELETED = !taskSubType.IS_CHECKED;
                        taskTaskSubType.MODIFIED_BY = profile.UserName;
                        taskTaskSubType.MODIFIED_DATE = DateTime.Now;
                        //_TaskTaskSubTypeRepository.Delete(taskTaskSubType);
                        _TaskTaskSubTypeRepository.Update(taskTaskSubType);

                        _CaseContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddEditTask_TaskSubTypes(long tASK_ID, List<OpenIssueViewModel> taskSubTypeList, UserProfile profile)
        {
            try
            {
                foreach (var taskSubType in taskSubTypeList)
                {
                    if (taskSubType.IS_CHECKED)
                    {
                        var taskTaskSubType = _TaskTaskSubTypeRepository.GetFirst(t =>
                                                                                  t.TASK_ID == tASK_ID
                                                                                  && t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID
                                                                                  && !t.DELETED
                                                                                  && t.PRACTICE_CODE == profile.PracticeCode);
                        bool IsEdit = false;

                        if (taskTaskSubType == null && taskSubType.IS_CHECKED)
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
                        else
                        {
                            taskTaskSubType.MODIFIED_BY = profile.UserName;
                            taskTaskSubType.MODIFIED_DATE = DateTime.Now;
                            IsEdit = true;
                        }
                        //taskTaskSubType.DELETED = false;
                        taskTaskSubType.DELETED = !taskSubType.IS_CHECKED;

                        if (IsEdit)
                        {
                            _TaskTaskSubTypeRepository.Update(taskTaskSubType);
                        }
                        else
                        {
                            _TaskTaskSubTypeRepository.Insert(taskTaskSubType);
                        }
                        _CaseContext.SaveChanges();
                    }
                    else
                    {
                        var taskTaskSubType = _TaskTaskSubTypeRepository.GetFirst(t =>
                                                                                  t.TASK_ID == tASK_ID
                                                                                  && t.TASK_SUB_TYPE_ID == taskSubType.TASK_SUB_TYPE_ID
                                                                                  && !t.DELETED
                                                                                  && t.PRACTICE_CODE == profile.PracticeCode);
                        if (taskTaskSubType != null)
                        {
                            taskTaskSubType.DELETED = true;
                            _TaskTaskSubTypeRepository.Update(taskTaskSubType);
                            _CaseContext.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public class AddEditNotesViewModel
        {
            public string NotesType { get; set; }
            public string Notes { get; set; }
            public long CASE_ID { get; set; }
            public long TASK_ID { get; set; }
        }

        private void AddEditNotes(AddEditNotesViewModel addEditNotesViewModel, UserProfile profile)
        {
            //addEditNotesViewModel.NotesType = "Case Order Information Comments";
            //Case Order Information Comments
            var notesType_OrderInfoComments = _NotesTypeRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.NAME.Contains(addEditNotesViewModel.NotesType));
            var notes = _NotesRepository.GetFirst(t => t.CASE_ID == addEditNotesViewModel.CASE_ID && t.NOTES_TYPE_ID == notesType_OrderInfoComments.NOTES_TYPE_ID);
            bool IsEditNotes = false;

            if (notes == null)
            {
                notes = new FOX_TBL_NOTES();

                notes.PRACTICE_CODE = profile.PracticeCode;
                notes.NOTES_ID = Helper.getMaximumId("FOX_TBL_NOTES_ID");
                notes.CREATED_BY = notes.MODIFIED_BY = profile.UserName;
                notes.CREATED_DATE = notes.MODIFIED_DATE = DateTime.Now;
                IsEditNotes = false;
            }
            else
            {
                notes.MODIFIED_BY = profile.UserName;
                notes.MODIFIED_DATE = DateTime.Now;
                IsEditNotes = true;
            }
            notes.CASE_ID = addEditNotesViewModel.CASE_ID;
            notes.NOTES = addEditNotesViewModel.Notes;
            notes.NOTES_TYPE_ID = notesType_OrderInfoComments?.NOTES_TYPE_ID;
            notes.DELETED = false;

            if (IsEditNotes)
            {
                _NotesRepository.Update(notes);
            }
            else
            {
                _NotesRepository.Insert(notes);
            }
            _CaseContext.SaveChanges();
        }

        private void AddEditTaskNotes(AddEditNotesViewModel addEditNotesViewModel, UserProfile profile)
        {
            //addEditNotesViewModel.NotesType = "Case Order Information Comments";
            //Case Order Information Comments
            var notesType_OrderInfoComments = _NotesTypeRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.NAME.Contains(addEditNotesViewModel.NotesType));
            var notes = _NotesRepository.GetFirst(t => t.TASK_ID == addEditNotesViewModel.TASK_ID);
            bool IsEditNotes = false;

            if (notes == null)
            {
                notes = new FOX_TBL_NOTES();

                notes.PRACTICE_CODE = profile.PracticeCode;
                notes.NOTES_ID = Helper.getMaximumId("FOX_TBL_NOTES_ID");
                notes.CREATED_BY = notes.MODIFIED_BY = profile.UserName;
                notes.CREATED_DATE = notes.MODIFIED_DATE = DateTime.Now;
                IsEditNotes = false;
            }
            else
            {
                notes.MODIFIED_BY = profile.UserName;
                notes.MODIFIED_DATE = DateTime.Now;
                IsEditNotes = true;
            }
            notes.TASK_ID = addEditNotesViewModel.TASK_ID;
            notes.NOTES = addEditNotesViewModel.Notes;
            notes.NOTES_TYPE_ID = notesType_OrderInfoComments?.NOTES_TYPE_ID;
            notes.DELETED = false;

            if (IsEditNotes)
            {
                _NotesRepository.Update(notes);
            }
            else
            {
                _NotesRepository.Insert(notes);
            }
            _CaseContext.SaveChanges();
        }

        //OrderInformationList
        private void AddEditOrderInformation(long CASE_ID, List<FOX_TBL_ORDER_INFORMATION> OrderInformationList, UserProfile profile)
        {
            try
            {
                foreach (var orderInformation in OrderInformationList)
                {
                    var orderInfo = _OrderInformationRepository.GetFirst(t => t.CASE_ID == CASE_ID && t.ORDER_INFO_ID == orderInformation.ORDER_INFO_ID);
                    bool IsEditOrderInfo = false;

                    if (orderInfo == null)
                    {
                        orderInfo = new FOX_TBL_ORDER_INFORMATION();
                        orderInfo.ORDER_INFO_ID = Helper.getMaximumId("FOX_TBL_ORDER_INFORMATION_ID");
                        orderInfo.PRACTICE_CODE = profile.PracticeCode;
                        orderInfo.CASE_ID = CASE_ID;
                        orderInfo.CREATED_BY = orderInfo.MODIFIED_BY = profile.UserName;
                        orderInfo.CREATED_DATE = orderInfo.MODIFIED_DATE = DateTime.Now;
                        IsEditOrderInfo = false;
                    }
                    else
                    {
                        orderInfo.MODIFIED_BY = profile.UserName;
                        orderInfo.MODIFIED_DATE = DateTime.Now;
                        IsEditOrderInfo = true;
                    }

                    orderInfo.CASE_TYPE_ID = orderInformation.CASE_TYPE_ID;
                    orderInfo.CODE = orderInformation.CODE;
                    orderInfo.DESCRIPTION = orderInformation.DESCRIPTION;
                    orderInfo.NO_OF_WEEK = orderInformation.NO_OF_WEEK;
                    orderInfo.VISIT_PER_WEEK = orderInformation.VISIT_PER_WEEK;
                    orderInfo.ORDER_STATUS_ID = orderInformation.ORDER_STATUS_ID;
                    orderInfo.PRESC_PROVIDER_ID = orderInformation.PRESC_PROVIDER_ID;
                    if (!string.IsNullOrEmpty(orderInformation.EFFECTIVE_DATE_Stirng))
                        orderInfo.EFFECTIVE_DATE = Convert.ToDateTime(orderInformation.EFFECTIVE_DATE_Stirng);
                    if (!string.IsNullOrEmpty(orderInformation.END_DATE_String))
                        orderInfo.END_DATE = Convert.ToDateTime(orderInformation.END_DATE_String);
                    orderInfo.DELETED = orderInformation.DELETED;
                    if (IsEditOrderInfo)
                    {
                        _OrderInformationRepository.Update(orderInfo);
                    }
                    else
                    {
                        _OrderInformationRepository.Insert(orderInfo);
                    }
                }
                _CaseContext.SaveChanges();
            }
            catch (Exception)
            {
                //To Do Log Exception here
                throw;
            }
        }

        private void AddEditCommunicationCalls(long CASE_ID, List<FOX_TBL_CALLS_LOG> CallsLoglist, UserProfile profile)
        {
            try
            {
                foreach (var callslogObj in CallsLoglist)
                {
                    var callslog = _CallsLogRepository.GetFirst(t => t.CASE_ID == CASE_ID && t.FOX_CALLS_LOG_ID == callslogObj.FOX_CALLS_LOG_ID);
                    bool IsEditCall = false;

                    if (callslog == null)
                    {
                        callslog = new FOX_TBL_CALLS_LOG();
                        callslog.FOX_CALLS_LOG_ID = Helper.getMaximumId("FOX_CALLS_LOG_ID");
                        callslog.PRACTICE_CODE = profile.PracticeCode;
                        callslog.CASE_ID = CASE_ID;
                        callslog.CREATED_BY = callslog.MODIFIED_BY = profile.UserName;
                        callslog.CREATED_DATE = callslog.MODIFIED_DATE = DateTime.Now;
                        IsEditCall = false;
                    }
                    else
                    {
                        callslog.MODIFIED_BY = profile.UserName;
                        callslog.MODIFIED_DATE = DateTime.Now;
                        IsEditCall = true;
                    }
                    if (!string.IsNullOrEmpty(callslogObj.ADMISSION_DATE_String))
                        callslog.ADMISSION_DATE = Convert.ToDateTime(callslogObj.ADMISSION_DATE_String);
                    if (!string.IsNullOrEmpty(callslogObj.CALL_DATE_String))
                        callslog.CALL_DATE = Convert.ToDateTime(callslogObj.CALL_DATE_String);
                    if (!string.IsNullOrEmpty(callslogObj.COMPLETED_DATE_String))
                        callslog.COMPLETED_DATE = Convert.ToDateTime(callslogObj.COMPLETED_DATE_String);
                    if (!string.IsNullOrEmpty(callslogObj.DISCHARGE_DATE_String))
                        callslog.DISCHARGE_DATE = Convert.ToDateTime(callslogObj.DISCHARGE_DATE_String);
                    callslog.FOX_CALL_TYPE_ID = callslogObj.FOX_CALL_TYPE_ID;
                    callslog.CASE_NO = callslogObj.CASE_NO;
                    callslog.CASE_STATUS = callslogObj.CASE_STATUS;
                    callslog.IS_CELL_CALL = callslogObj.IS_CELL_CALL;
                    callslog.COMMENTS = callslogObj.COMMENTS;
                    callslog.DISCHARGE_DATE = callslogObj.DISCHARGE_DATE;
                    callslog.GROUP_IDENTIFIER_ID = callslogObj.GROUP_IDENTIFIER_ID;
                    callslog.IS_HOME_CALL = callslogObj.IS_HOME_CALL;
                    callslog.LOCATION_ID = callslogObj.LOCATION_ID;
                    callslog.PATIENT_STATUS = callslogObj.PATIENT_STATUS;
                    callslog.PROVIDER_ID = callslogObj.PROVIDER_ID;
                    callslog.REGION_ID = callslogObj.REGION_ID;
                    callslog.REMARKABLE_REPORT_COMMENTS = callslogObj.REMARKABLE_REPORT_COMMENTS;
                    callslog.FOX_CALL_STATUS_ID = callslogObj.FOX_CALL_STATUS_ID;
                    callslog.FOX_CARE_STATUS_ID = callslogObj.FOX_CARE_STATUS_ID;
                    callslog.IS_WORK_CALL = callslogObj.IS_WORK_CALL;
                    callslog.FOX_CALL_RESULT_ID = callslogObj.FOX_CALL_RESULT_ID;
                    callslog.RESULT_OF_CALL = callslogObj.RESULT_OF_CALL;
                    callslog.DELETED = callslogObj.DELETED;

                    if (IsEditCall)
                    {
                        _CallsLogRepository.Update(callslog);
                    }
                    else
                    {
                        _CallsLogRepository.Insert(callslog);
                    }
                }
                _CaseContext.SaveChanges();
            }
            catch (Exception exception)
            {
                //To Do Log Exception here
                throw exception;
            }
        }

        public ResponseGetCasesDDL GetCasesDDL(string patient_Account, long practiceCode)
        {
            try
            {
                List<GetTotalDisciplineRes> DiscpilineList = new List<GetTotalDisciplineRes>();
                var _patient_Account = Convert.ToInt64(patient_Account);
                //var FOX_TBL_CASEList = _CaseRepository.GetMany(t => !t.DELETED);
                var FOX_VW_CASEList = _vwCaseRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode && t.PATIENT_ACCOUNT == _patient_Account).OrderByDescending(t => t.CREATED_DATE).ToList();
                if (FOX_VW_CASEList.Count() > 0)
                {
                    foreach (var rec in FOX_VW_CASEList)
                    {
                        if (rec.ORDERING_REF_SOURCE_ID != null)
                        {
                            if (rec.CERTIFYING_REF_SOURCE_ID == 0 || string.IsNullOrEmpty(rec.CERTIFYING_REF_SOURCE_ID.ToString()))
                            {
                                var referralsource = _SourceRepository.GetFirst(r => r.SOURCE_ID == rec.ORDERING_REF_SOURCE_ID && r.DELETED == false);
                                rec.ObjReferralSource = referralsource;
                            }
                        }
                    }
                }
                var CaseTypeResult = _CaseTypeRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var CaseDescplineResult = _CaseDesciplineRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var CaseStatusResult = _CaseStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var CaseSuffixResult = _CaseSuffixRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var CaseGrpIdentifierResult = _CaseGrpIdentifierRepository.GetMany(t => !t.DELETED && (t.IS_ACTIVE ?? true) && t.PRACTICE_CODE == practiceCode);
                var CaseRefSourceResult = _CaseSourceofRefRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var OrderStatusREsult = _OrderStatusRepository.GetMany(t => !t.DELETED && (t.IS_ACTIVE ?? true) && t.PRACTICE_CODE == practiceCode);
                var WorkOrderResult = _WorkOrderQueueRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode && t.PATIENT_ACCOUNT == _patient_Account);
                var StatusOfCall = _CallStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var StatusOfCare = _StatusofCareRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var CallResult = _CallResultRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var PatientData = _PatientRepository.GetSingle(t => !(t.DELETED ?? false) && t.Practice_Code == practiceCode && t.Patient_Account == _patient_Account);
                var CallType = _CallTypeRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);


                var CaseTreatmentTeamList = _CaseTreatmentTeamRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode && t.PATIENT_ACCOUNT.ToString() == patient_Account);
                if (CaseTreatmentTeamList != null && CaseTreatmentTeamList.Count > 0)
                {
                    CaseTreatmentTeamList.ForEach(t=> {
                        var TreatingProvider = _FoxProviderClassRepository.GetFirst(e => e.FOX_PROVIDER_ID == t.TREATING_PROVIDER_ID && !t.DELETED && t.PRACTICE_CODE == practiceCode) ;
                        if(TreatingProvider != null && TreatingProvider.FIRST_NAME != null && TreatingProvider.LAST_NAME != null)
                        {
                            t.TREATING_PROVIDER = TreatingProvider.LAST_NAME +  ", " + TreatingProvider.FIRST_NAME;
                        }
                        var CaseProvider = _FoxProviderClassRepository.GetFirst(e => e.FOX_PROVIDER_ID == t.CASE_PROVIDER_ID && !t.DELETED && t.PRACTICE_CODE == practiceCode);
                        if (CaseProvider != null && CaseProvider.FIRST_NAME != null && CaseProvider.LAST_NAME != null)
                        {
                            t.CASE_PROVIDER = CaseProvider.LAST_NAME + ", " + CaseProvider.FIRST_NAME;
                        }

                    } );
                }
                DiscpilineList = GetTotalDiscipline(_patient_Account, practiceCode);

                var insuranceHistory = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == _patient_Account && (x.Deleted ?? false) == false && x.ELIG_LOADED_ON.HasValue); //History

                if (insuranceHistory != null && insuranceHistory.Count > 0)
                {
                    //insuranceHistory.RemoveAt(0);
                    for (int i = 0; i < insuranceHistory.Count; i++)
                    {
                        if (insuranceHistory[i].CASE_ID != null && insuranceHistory[i].CASE_ID != 0)
                        {
                            insuranceHistory[i].CASE_NO = _vwCaseRepository.GetByID(insuranceHistory[i].CASE_ID).CASE_NO;
                            insuranceHistory[i].RT_CASE_NO = _vwCaseRepository.GetByID(insuranceHistory[i].CASE_ID).RT_CASE_NO;


                        }
                        insuranceHistory[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(insuranceHistory[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";
                    }
                }

                var insuranceList = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == _patient_Account && (x.Deleted ?? false) == false); //insurance List
                if (insuranceList != null && insuranceList.Count > 0)
                {

                    for (int i = 0; i < insuranceList.Count; i++)
                    {
                        insuranceList[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(insuranceList[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";
                    }
                }

                return new ResponseGetCasesDDL()
                {
                    Message = "Get all drop down list values successfully.",
                    Success = true,
                    ErrorMessage = "",
                    FOX_TBL_CASE_TYPEList = CaseTypeResult,
                    FOX_TBL_DISCIPLINEList = CaseDescplineResult,
                    FOX_TBL_CASE_STATUSList = CaseStatusResult,
                    FOX_TBL_CASE_SUFFIXList = CaseSuffixResult,
                    FOX_TBL_GROUP_IDENTIFIERList = CaseGrpIdentifierResult,
                    FOX_TBL_SOURCE_OF_REFList = CaseRefSourceResult,
                    FOX_VW_CASEList = FOX_VW_CASEList,
                    FOX_TBL_ORDER_STATUSList = OrderStatusREsult,
                    GET_TOTAL_DISCIPLINEList = DiscpilineList,
                    InsuranceEligibilityHistoryList = insuranceHistory,
                    WorkOrderQueueList = WorkOrderResult,
                    CallStatusList = StatusOfCall,
                    StatusofCareList = StatusOfCare,
                    PatientObj = PatientData,
                    ResultCallList = CallResult,
                    CallTypeList = CallType,
                    InsuranceList = insuranceList,
                    CaseTreatmentTeamList = CaseTreatmentTeamList

                };
            }
            catch (Exception exception)
            {
                return new ResponseGetCasesDDL()
                {
                    Message = exception.Message,
                    Success = false,
                    ErrorMessage = exception.ToString()
                };
            }
        }
        public ResponseGetCasesDDL GetCasesDDLTalRehab(string patient_Account, long practiceCode)
        {
            try
            {
                List<GetTotalDisciplineRes> DiscpilineList = new List<GetTotalDisciplineRes>();
                var _patient_Account = Convert.ToInt64(patient_Account);
                //var FOX_TBL_CASEList = _CaseRepository.GetMany(t => !t.DELETED);
                var FOX_VW_CASEList = _vwCaseRepository.GetMany(t => !t.DELETED && t.PATIENT_ACCOUNT == _patient_Account && t.PRACTICE_CODE == practiceCode).OrderByDescending(t => t.CREATED_DATE).ToList();  
                if (FOX_VW_CASEList.Count() > 0)
                {
                    foreach (var rec in FOX_VW_CASEList)
                    {
                        if (rec.ORDERING_REF_SOURCE_ID != null)
                        {
                            if (rec.CERTIFYING_REF_SOURCE_ID == 0 || string.IsNullOrEmpty(rec.CERTIFYING_REF_SOURCE_ID.ToString()))
                            {
                                var referralsource = _SourceRepository.GetFirst(r => r.SOURCE_ID == rec.ORDERING_REF_SOURCE_ID && r.DELETED == false && r.PRACTICE_CODE == practiceCode);
                                rec.ObjReferralSource = referralsource;
                            }
                        }
                    }
                }
                var CaseTypeResult = _CaseTypeRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == 1011163);
                var CaseDescplineResult = _CaseDesciplineRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == 1011163);
                var CaseStatusResult = _CaseStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
                var CaseSuffixResult = _CaseSuffixRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == 1011163);
                var CaseGrpIdentifierResult = _CaseGrpIdentifierRepository.GetMany(t => !t.DELETED && (t.IS_ACTIVE ?? true) && t.PRACTICE_CODE == 1011163);
                var OrderStatusREsult = _OrderStatusRepository.GetMany(t => !t.DELETED && (t.IS_ACTIVE ?? true) && t.PRACTICE_CODE == 1011163);

                var CaseRefSourceResult = _CaseSourceofRefRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == 1011163);
                var WorkOrderResult = _WorkOrderQueueRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode && t.PATIENT_ACCOUNT == _patient_Account);
                var StatusOfCall = _CallStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == 1011163);
                var StatusOfCare = _StatusofCareRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == 1011163);
                var CallResult = _CallResultRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == 1011163);
                var PatientData = _PatientRepository.GetSingle(t => !(t.DELETED ?? false) && t.Practice_Code == practiceCode && t.Patient_Account == _patient_Account);
                var CallType = _CallTypeRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == 1011163);


                var CaseTreatmentTeamList = _CaseTreatmentTeamRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode && t.PATIENT_ACCOUNT.ToString() == patient_Account);
                if (CaseTreatmentTeamList != null && CaseTreatmentTeamList.Count > 0)
                {
                    CaseTreatmentTeamList.ForEach(t => {
                        var TreatingProvider = _FoxProviderClassRepository.GetFirst(e => e.FOX_PROVIDER_ID == t.TREATING_PROVIDER_ID && !t.DELETED && t.PRACTICE_CODE == practiceCode);
                        if (TreatingProvider != null && TreatingProvider.FIRST_NAME != null && TreatingProvider.LAST_NAME != null)
                        {
                            t.TREATING_PROVIDER = TreatingProvider.LAST_NAME + ", " + TreatingProvider.FIRST_NAME;
                        }
                        var CaseProvider = _FoxProviderClassRepository.GetFirst(e => e.FOX_PROVIDER_ID == t.CASE_PROVIDER_ID && !t.DELETED && t.PRACTICE_CODE == practiceCode);
                        if (CaseProvider != null && CaseProvider.FIRST_NAME != null && CaseProvider.LAST_NAME != null)
                        {
                            t.CASE_PROVIDER = CaseProvider.LAST_NAME + ", " + CaseProvider.FIRST_NAME;
                        }

                    });
                }
                DiscpilineList = GetTotalDiscipline(_patient_Account, practiceCode);

                var insuranceHistory = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == _patient_Account && (x.Deleted ?? false) == false && x.ELIG_LOADED_ON.HasValue); //History

                if (insuranceHistory != null && insuranceHistory.Count > 0)
                {
                    //insuranceHistory.RemoveAt(0);
                    for (int i = 0; i < insuranceHistory.Count; i++)
                    {
                        if (insuranceHistory[i].CASE_ID != null && insuranceHistory[i].CASE_ID != 0)
                        {
                            insuranceHistory[i].CASE_NO = _vwCaseRepository.GetByID(insuranceHistory[i].CASE_ID).CASE_NO;
                            insuranceHistory[i].RT_CASE_NO = _vwCaseRepository.GetByID(insuranceHistory[i].CASE_ID).RT_CASE_NO;


                        }
                        insuranceHistory[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(insuranceHistory[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";
                    }
                }

                var insuranceList = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == _patient_Account && (x.Deleted ?? false) == false); //insurance List
                if (insuranceList != null && insuranceList.Count > 0)
                {

                    for (int i = 0; i < insuranceList.Count; i++)
                    {
                        insuranceList[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(insuranceList[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";
                    }
                }

                return new ResponseGetCasesDDL()
                {
                    Message = "Get all drop down list values successfully.",
                    Success = true,
                    ErrorMessage = "",
                    FOX_TBL_CASE_TYPEList = CaseTypeResult,
                    FOX_TBL_DISCIPLINEList = CaseDescplineResult,
                    FOX_TBL_CASE_STATUSList = CaseStatusResult,
                    FOX_TBL_CASE_SUFFIXList = CaseSuffixResult,
                    FOX_TBL_GROUP_IDENTIFIERList = CaseGrpIdentifierResult,
                    FOX_TBL_SOURCE_OF_REFList = CaseRefSourceResult,
                    FOX_VW_CASEList = FOX_VW_CASEList,
                    FOX_TBL_ORDER_STATUSList = OrderStatusREsult,
                    GET_TOTAL_DISCIPLINEList = DiscpilineList,
                    InsuranceEligibilityHistoryList = insuranceHistory,
                    WorkOrderQueueList = WorkOrderResult,
                    CallStatusList = StatusOfCall,
                    StatusofCareList = StatusOfCare,
                    PatientObj = PatientData,
                    ResultCallList = CallResult,
                    CallTypeList = CallType,
                    InsuranceList = insuranceList,
                    CaseTreatmentTeamList = CaseTreatmentTeamList

                };
            }
            catch (Exception exception)
            {
                return new ResponseGetCasesDDL()
                {
                    Message = exception.Message,
                    Success = false,
                    ErrorMessage = exception.ToString()
                };
            }
        }
        public ResponseGetCasesDDL GetCasesDDLTalkrehab(CasesSearchRequest casesModel)
        {
            try
            {
                List<Patient> patientsInfo = new List<Patient>();
                long practiceCode = casesModel.PracticeCode;
                long locationCode = casesModel.LocationCode;
                int caseStatusId= casesModel.StatusId;
                long providerId = casesModel.ProviderCode;
                List<GetTotalDisciplineRes> discpilineList = new List<GetTotalDisciplineRes>();
                List<FOX_VW_CASE> foxVWCaseList = _vwCaseRepository.GetMany(cases => !cases.DELETED && cases.PRACTICE_CODE == practiceCode
                && ((locationCode != 0)? locationCode.ToString() == cases.PosLocation: true )
                && ((caseStatusId != 0 ) ? caseStatusId == cases.CASE_STATUS_ID : true )
                && ((providerId != 0)? providerId == cases.TREATING_PROVIDER_ID : true)).OrderByDescending(cases => cases.CREATED_DATE).ToList();

                if (foxVWCaseList.Count() > 0)
                {
                    foreach (var record in foxVWCaseList)
                    {
                        if (record.ORDERING_REF_SOURCE_ID != null)
                        {
                            if (record.CERTIFYING_REF_SOURCE_ID == 0 || string.IsNullOrEmpty(record.CERTIFYING_REF_SOURCE_ID.ToString()))
                            {
                                ReferralSource referralSource = _SourceRepository.GetFirst(referealSource => referealSource.SOURCE_ID == record.ORDERING_REF_SOURCE_ID
                                && referealSource.DELETED == false);
                                record.ObjReferralSource = referralSource;
                            }
                            patientsInfo = _PatientRepository.GetMany(pat => !(pat.DELETED ?? false)
                            && pat.Practice_Code == practiceCode
                            && pat.Patient_Account == record.PATIENT_ACCOUNT);
                        }
                    }
                }
                List<FOX_TBL_CASE_TYPE> caseTypeResult = _CaseTypeRepository.GetMany(caseType => !caseType.DELETED
                && caseType.PRACTICE_CODE == practiceCode
                && caseType.CASE_TYPE_ID == caseStatusId);

                List<FOX_TBL_DISCIPLINE> caseDescplineResult = _CaseDesciplineRepository.GetMany(displine => !displine.DELETED
                && displine.PRACTICE_CODE == practiceCode);

                List<FOX_TBL_CASE_STATUS> caseStatusResult = _CaseStatusRepository.GetMany(caseStatus => !caseStatus.DELETED
                && caseStatus.PRACTICE_CODE == practiceCode
                && ((caseStatusId != 0) ? caseStatusId == caseStatus.CASE_STATUS_ID : true));

                List<FOX_TBL_CASE_SUFFIX> caseSuffixResult = _CaseSuffixRepository.GetMany(caseSuffix => !caseSuffix.DELETED
                && caseSuffix.PRACTICE_CODE == practiceCode);

                List<FOX_TBL_GROUP_IDENTIFIER> caseGrpIdentifierResult = _CaseGrpIdentifierRepository.GetMany(groupIdentifier => !groupIdentifier.DELETED
                && (groupIdentifier.IS_ACTIVE ?? true)
                && groupIdentifier.PRACTICE_CODE == practiceCode);

                List<FOX_TBL_SOURCE_OF_REFERRAL> caseRefSourceResult = _CaseSourceofRefRepository.GetMany(sourceRefferal => !sourceRefferal.DELETED
                && sourceRefferal.PRACTICE_CODE == practiceCode);

                List<FOX_TBL_ORDER_STATUS> orderStatusREsult = _OrderStatusRepository.GetMany(orderStatus => !orderStatus.DELETED
                && (orderStatus.IS_ACTIVE ?? true)
                && orderStatus.PRACTICE_CODE == practiceCode);

                List<OriginalQueue> workOrderResult = _WorkOrderQueueRepository.GetMany(queue => !queue.DELETED
                && queue.PRACTICE_CODE == practiceCode );

                List<COMMUNICATION_CALL_STATUS> statusOfCall = _CallStatusRepository.GetMany(callStatus => !callStatus.DELETED
                && callStatus.PRACTICE_CODE == practiceCode);

                List<COMMUNICATION_STATUS_OF_CARE> statusOfCare = _StatusofCareRepository.GetMany(careStatus => !careStatus.DELETED
                && careStatus.PRACTICE_CODE == practiceCode);

                List<FOX_TBL_COMMUNICATION_CALL_RESULT> callResult = _CallResultRepository.GetMany(callRe => !callRe.DELETED
                && callRe.PRACTICE_CODE == practiceCode);

                List<FOX_TBL_COMMUNICATION_CALL_TYPE> callType = _CallTypeRepository.GetMany(callsType => !callsType.DELETED
                && callsType.PRACTICE_CODE == practiceCode );

                List<FOX_TBL_CASE_TREATMENT_TEAM> caseTreatmentTeamList = _CaseTreatmentTeamRepository.GetMany(treatmentTeam => !treatmentTeam.DELETED
                && treatmentTeam.PRACTICE_CODE == practiceCode
                && treatmentTeam.CASE_PROVIDER_ID == providerId);

                if (caseTreatmentTeamList != null && caseTreatmentTeamList?.Count > 0)
                {
                    caseTreatmentTeamList.ForEach(caseTreatment => {
                        FoxProviderClass TreatingProvider = _FoxProviderClassRepository.GetFirst(providerClass => providerClass.FOX_PROVIDER_ID == caseTreatment.TREATING_PROVIDER_ID
                        && !caseTreatment.DELETED
                        && caseTreatment.PRACTICE_CODE == practiceCode
                        && caseTreatment.CASE_PROVIDER_ID == providerId);
                        if (TreatingProvider != null && TreatingProvider.FIRST_NAME != null && TreatingProvider.LAST_NAME != null)
                        {
                            caseTreatment.TREATING_PROVIDER = TreatingProvider.LAST_NAME + ", " + TreatingProvider.FIRST_NAME;
                        }
                        FoxProviderClass caseProvider = _FoxProviderClassRepository.GetFirst(e => e.FOX_PROVIDER_ID == caseTreatment.CASE_PROVIDER_ID
                        && !caseTreatment.DELETED
                        && caseTreatment.PRACTICE_CODE == practiceCode);
                        if (caseProvider != null && caseProvider.FIRST_NAME != null && caseProvider.LAST_NAME != null)
                        {
                            caseTreatment.CASE_PROVIDER = caseProvider.LAST_NAME + ", " + caseProvider.FIRST_NAME;
                        }

                    });
                }
                discpilineList = GetTotalDisciplineTalkrehab(practiceCode);

                List<PatientInsurance> insuranceHistory = _PatientInsuranceRepository.GetMany(patientIns => (patientIns.Deleted ?? false) == false
                && patientIns.ELIG_LOADED_ON.HasValue);

                if (insuranceHistory != null && insuranceHistory.Count > 0)
                {
                    for (int i = 0; i < insuranceHistory.Count; i++)
                    {
                        if (insuranceHistory[i].CASE_ID != null && insuranceHistory[i].CASE_ID != 0)
                        {
                            insuranceHistory[i].CASE_NO = _vwCaseRepository.GetByID(insuranceHistory[i].CASE_ID).CASE_NO;
                            insuranceHistory[i].RT_CASE_NO = _vwCaseRepository.GetByID(insuranceHistory[i].CASE_ID).RT_CASE_NO;


                        }
                        insuranceHistory[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(insuranceHistory[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";
                    }
                }
                List<PatientInsurance> insuranceList = _PatientInsuranceRepository.GetMany(patientIns => (patientIns.Deleted ?? false) == false);
                if (insuranceList != null && insuranceList.Count > 0)
                {
                    for (int i = 0; i < insuranceList.Count; i++)
                    {
                        insuranceList[i].InsPayer_Description = _foxInsurancePayersRepository.GetByID(insuranceList[i].FOX_TBL_INSURANCE_ID)?.INSURANCE_NAME ?? "";
                    }
                }
                return new ResponseGetCasesDDL()
                {
                    Message = "Get all drop down list values successfully.",
                    Success = true,
                    ErrorMessage = "",
                    FOX_TBL_CASE_TYPEList = caseTypeResult,
                    FOX_TBL_DISCIPLINEList = caseDescplineResult,
                    FOX_TBL_CASE_STATUSList = caseStatusResult,
                    FOX_TBL_CASE_SUFFIXList = caseSuffixResult,
                    FOX_TBL_GROUP_IDENTIFIERList = caseGrpIdentifierResult,
                    FOX_TBL_SOURCE_OF_REFList = caseRefSourceResult,
                    FOX_VW_CASEList = foxVWCaseList,
                    FOX_TBL_ORDER_STATUSList = orderStatusREsult,
                    GET_TOTAL_DISCIPLINEList = discpilineList,
                    InsuranceEligibilityHistoryList = insuranceHistory,
                    WorkOrderQueueList = workOrderResult,
                    CallStatusList = statusOfCall,
                    StatusofCareList = statusOfCare,
                    PatientTalkrehab = patientsInfo,
                    ResultCallList = callResult,
                    CallTypeList = callType,
                    InsuranceList = insuranceList,
                    CaseTreatmentTeamList = caseTreatmentTeamList

                };
            }
            catch (Exception exception)
            {
                return new ResponseGetCasesDDL()
                {
                    Message = exception.Message,
                    Success = false,
                    ErrorMessage = exception.ToString()
                };
            }
        }
        public List<DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER> GetIdentifierList(long practiceCode)
        {

            var practice_code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var result = SpRepository<DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_IDENTIFIERS_LIST] @PRACTICE_CODE",practice_code).ToList();
            if (result.Any())
                return result;
            else
                return new List<DataModels.Models.CasesModel.FOX_TBL_IDENTIFIER>();
            //try
            //{
            //    var Result = _IdentifierRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
            //    return Result;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<SmartIdentifierRes> GetSmartIdentifier(SmartIdentifierReq obj, UserProfile profile)
        {
            try
            {
                if (obj.SEARCHVALUE == null)
                    obj.SEARCHVALUE = "";
                if (obj.SEARCHVALUE.Contains("]"))
                {
                    obj.SEARCHVALUE = obj.SEARCHVALUE.Substring(obj.SEARCHVALUE.IndexOf(' ') + 1).TrimStart();
                }
                var pRACTICE_CODE = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                // var tYPE_ID = new SqlParameter("TYPE_ID", SqlDbType.VarChar) { Value = obj.TYPE_ID };
                var tYPE = new SqlParameter("TYPE", SqlDbType.VarChar) { Value = obj.TYPE };
                var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
                var result = SpRepository<SmartIdentifierRes>.GetListWithStoreProcedure(@"exec [FOX_GET_IDENTIFIER] @PRACTICE_CODE, @TYPE,@SEARCHVALUE",
                    pRACTICE_CODE, tYPE, smartvalue).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<SmartIdentifierRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public InactiveListOfGroupIDNAndSourceOfReferral GetAllIdentifierANDSourceofReferralList(UserProfile profile)
        {
            InactiveListOfGroupIDNAndSourceOfReferral list = new InactiveListOfGroupIDNAndSourceOfReferral();
            List<FOX_TBL_SOURCE_OF_REFERRAL> res = new List<FOX_TBL_SOURCE_OF_REFERRAL>();
            List<FOX_TBL_GROUP_IDENTIFIER> res1 = new List<FOX_TBL_GROUP_IDENTIFIER>();
            try
            {
                //if (profile.isTalkRehab)
                //{
                //    res1 = _CaseGrpIdentifierRepository.GetMany(t => !t.DELETED);
                //    res = _CaseSourceofRefRepository.GetMany(t => !t.DELETED);
                //}
                //else
                //{
                //    res1 = _CaseGrpIdentifierRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode);
                //    res = _CaseSourceofRefRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode);

                //}
                res1 = _CaseGrpIdentifierRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode);
                res = _CaseSourceofRefRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode);

                if (res.Any())
                {
                    list.Spourc_Of_Refferal = res;
                    list.Group_Identifier = res1;
                    return list;
                }
                else
                {
                    return new InactiveListOfGroupIDNAndSourceOfReferral();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FOX_TBL_SOURCE_OF_REFERRAL> GetSourceofReferral(long practiceCode, bool isTalkRehab)
        {
            try
            {
                if (isTalkRehab)
                {
                    var Result = _CaseSourceofRefRepository.GetMany(t => !t.DELETED && (t.IS_ACTIVE ?? true) && t.PRACTICE_CODE == 1011163);
                    if (Result.Any())
                    {
                        return Result;
                    }
                    else
                    {
                        return new List<FOX_TBL_SOURCE_OF_REFERRAL>();
                    }
                }
                else
                {
                    var Result = _CaseSourceofRefRepository.GetMany(t => !t.DELETED && (t.IS_ACTIVE ?? true) && t.PRACTICE_CODE == practiceCode);
                    if (Result.Any())
                    {
                        return Result;
                    }
                    else
                    {
                        return new List<FOX_TBL_SOURCE_OF_REFERRAL>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetNONandHOLDAllListRes GetNONandHOLDIssueList(GetOpenIssueListReq req, UserProfile profile)
        {
            GetNONandHOLDAllListRes response = new GetNONandHOLDAllListRes();
            var pRACTICE_CODE = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };

            var cASE_STATUS_ID = new SqlParameter("@TYPE", SqlDbType.VarChar) { Value = req.TYPE };
            var result = SpRepository<NONandHOLDIssueList>.GetListWithStoreProcedure(@"exec GET_NON_HOLD_ISSUE_LIST @PRACTICE_CODE, @TYPE",
                pRACTICE_CODE, cASE_STATUS_ID).ToList();
            if(result.Any())
            {
                response.NONandHOLDIssueList = result;
                if(req.CASE_ID != 0)
                {
                    long notes_type_id = 0;
                    if(req.TYPE == "NON")
                    {
                        var notes_type = _NotesTypeRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.NAME.ToLower() == "case non admission comments");
                        if (notes_type != null && notes_type.NOTES_TYPE_ID != 0)
                        {
                            notes_type_id = notes_type.NOTES_TYPE_ID;
                        }

                    }
                    else
                    {
                        var notes_type = _NotesTypeRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.NAME.ToLower() == "case hold comments");
                        if (notes_type != null && notes_type.NOTES_TYPE_ID != 0)
                        {
                            notes_type_id = notes_type.NOTES_TYPE_ID;
                        }
                    }

                    if(notes_type_id != 0)
                    {
                        var Notes = _NotesRepository.GetMany(t => t.NOTES_TYPE_ID == notes_type_id && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED && t.CASE_ID == req.CASE_ID).OrderByDescending(t => t.CREATED_DATE).FirstOrDefault();
                        if (Notes != null)
                        {
                            response.Notes = Notes.NOTES ?? "";
                        }
                    }

                }

            }
            return response;

        }

        public GetOpenIssueAllListRes GetOpenIssueList(GetOpenIssueListReq req, UserProfile profile)
        {
            GetOpenIssueAllListRes response = new GetOpenIssueAllListRes();
            try
            {
                var pRACTICE_CODE = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var cASE_ID = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = req.CASE_ID };
                var cASE_STATUS_ID = new SqlParameter("@CASE_STATUS_ID", SqlDbType.BigInt) { Value = req.CASE_STATUS_ID };
                var result = SpRepository<OpenIssueViewModel>.GetListWithStoreProcedure(@"exec GET_OPEN_ISSUE_LIST @PRACTICE_CODE, @CASE_ID, @CASE_STATUS_ID",
                    pRACTICE_CODE, cASE_ID, cASE_STATUS_ID).ToList();

                if (result != null && result.Any())
                {
                    List<OpenIssueList> model = new List<OpenIssueList>();

                    var practicCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                    var caseID = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = req.CASE_ID };
                    var caseStatusId = new SqlParameter("@CASE_STATUS_ID", SqlDbType.BigInt) { Value = req.CASE_STATUS_ID };
                    var taskTypeList = SpRepository<OpenIssueNotes>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_OPEN_ISSUE_LIST @PRACTICE_CODE, @CASE_ID, @CASE_STATUS_ID", practicCode, caseID, caseStatusId);
                    List<FOX_TBL_TASK> taskList = new List<FOX_TBL_TASK>();
                    if (profile.isTalkRehab)
                    {
                        taskList = _TaskRepository.ExecuteCommand("select * from fox_tbl_task where CASE_ID= {0} and PRACTICE_CODE= {1} AND GENERAL_NOTE_ID IS NULL", req.CASE_ID, profile.PracticeCode);
                    }
                    else { 
                        taskList = _TaskRepository.GetMany(t => t.CASE_ID == req.CASE_ID && t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED && t.GENERAL_NOTE_ID == null);
                    }
                    if (taskTypeList != null)
                    {
                        foreach (var item in taskTypeList)
                        {
                            var openIssueList = result.Where(x => x.TASK_TYPE.Contains(item.TaskTypeName)).ToList();
                            if (openIssueList != null)
                            {

                                var totalTasks = _TaskRepository.GetMany(e => e.CASE_ID == req.CASE_ID && e.TASK_TYPE_ID == item.TASK_TYPE_ID && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode);
                                var markCompletedTasks = totalTasks.Where(e => e.IS_FINALROUTE_MARK_COMPLETE && e.IS_SENDTO_MARK_COMPLETE).ToList();
                                model.Add(
                                    new OpenIssueList()
                                    {
                                        TASK_TYPE_ID = item.TASK_TYPE_ID,
                                        TaskTypeName = item.TaskTypeName,
                                        TaskSubTypeList = openIssueList,
                                        //IsRadioBtn = item.TaskTypeName.Contains("Hold/Pending Assignment"),
                                        // Hold Pending Change
                                        IsRadioBtn = item.TaskTypeName.Contains("Call Patient"),
                                        Tasks = taskList,
                                        Is_Green = totalTasks.Count > 0 && totalTasks.Count == markCompletedTasks.Count,
                                        Is_Yellow = totalTasks.Count > 0 && totalTasks.Count != markCompletedTasks.Count && !item.TaskTypeName.Contains("Non Admission Reason(s)"),  /*&& !item.TaskTypeName.Contains("Hold/Pending Assignment")*/
                                        Notes = item.NOTES
                                    }
                                );
                            }
                        }
                    }
                    //response.openIssueList = model;
                    response.openIssueList = CreateIntelligentTasks(req?.PATIENT_ACCOUNT ?? 0, req?.CASE_ID ?? 0, model, profile);
                    response.Message = "";
                    response.Success = true;
                    response.ErrorMessage = "";
                }
                else
                {
                    response.openIssueList = null;
                    response.Message = "No Task_Sub_Type is found.";
                    response.Success = true;
                    response.ErrorMessage = "";
                }
                return response;
            }
            catch (Exception exception)
            {
                //throw ex;
                response.Message = exception.Message;
                response.Success = false;
                response.ErrorMessage = exception.ToString();
                return response;
            }
        }

        public List<GetSmartPoslocRes> GetSmartPosLocation(GetSmartPoslocReq obj, UserProfile Profile)
        {
            try
            {
                if (obj.SEARCHVALUE == null)
                    obj.SEARCHVALUE = "";
                var parmPracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
                var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
                var result = SpRepository<GetSmartPoslocRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_POS_LOCATIONS] @SEARCHVALUE,@PRACTICE_CODE", smartvalue, parmPracticeCode).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<GetSmartPoslocRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Single Object Smart Serach Locations by Johar

        public List<GetSmartPoslocRes> GetSmartPosLocations(string searchText, UserProfile Profile)
        {
            var parmPracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var smartvalue = new SqlParameter("@SEARCHVALUE", SqlDbType.VarChar) { Value = searchText };
            var result = SpRepository<GetSmartPoslocRes>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_POS_LOCATIONS] @SEARCHVALUE,@PRACTICE_CODE", smartvalue, parmPracticeCode).ToList();
            if (result.Any())
                return result;
            else
                return new List<GetSmartPoslocRes>();
        }

        public List<GetTotalDisciplineRes> GetTotalDiscipline(long? Patient_Account, long PracticeCode)
        {
            try
            {
                // var dISCIPLINE_ID = new SqlParameter("DISCIPLINE_ID", SqlDbType.Int) { Value = Discipline_Id };
                var pATIENT_ACCOUNT = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = Patient_Account };
                var pRACTICE_CODE = new SqlParameter("PRACTICE_CODE", SqlDbType.VarChar) { Value = PracticeCode };
                var result = SpRepository<GetTotalDisciplineRes>.GetListWithStoreProcedure(@"exec [FOX_GET_TOTAL_DISCIPLINE] @PATIENT_ACCOUNT,@PRACTICE_CODE", pATIENT_ACCOUNT, pRACTICE_CODE).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<GetTotalDisciplineRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<GetTotalDisciplineRes> GetTotalDiscipline(long? Patient_Account)
        {
            try
            {
                var pATIENT_ACCOUNT = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = Patient_Account };
                var pRACTICE_CODE = new SqlParameter("PRACTICE_CODE", SqlDbType.VarChar) { Value = 1011163 };
                var result = SpRepository<GetTotalDisciplineRes>.GetListWithStoreProcedure(@"exec [FOX_GET_TOTAL_DISCIPLINE] @PATIENT_ACCOUNT,@PRACTICE_CODE", pATIENT_ACCOUNT, pRACTICE_CODE).ToList(); // TO BE CHANGE
                if (result.Any())
                    return result;
                else
                    return new List<GetTotalDisciplineRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetTotalDisciplineRes> GetTotalDisciplineTalkrehab(long practiceCode)
        {
            try
            {
                SqlParameter pracCode = new SqlParameter("PRACTICE_CODE", SqlDbType.VarChar) { Value = practiceCode };
                List<GetTotalDisciplineRes> result = SpRepository<GetTotalDisciplineRes>.GetListWithStoreProcedure(@"exec [FOX_GET_TOTAL_DISCIPLINE_talkrehab] @PRACTICE_CODE", pracCode).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<GetTotalDisciplineRes>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OrderInformationAndNotes GetOrderInformationAndNotes(getOrderInfoReq obj, UserProfile profile)
        {
            try
            {
                List<FOX_TBL_NOTES_TYPE> notesTypeList = new List<FOX_TBL_NOTES_TYPE>();
                List<NotesViewModel> notesList = new List<NotesViewModel>();
                List<FOX_TBL_ORDER_INFORMATION> OrderInfoResult = new List<FOX_TBL_ORDER_INFORMATION>();
                //if (profile.isTalkRehab)
                //{
                //    notesTypeList = _NotesTypeRepository.GetMany(x => !x.DELETED);
                //    notesList = _NotesRepository.GetMany(x => x.CASE_ID == obj.CASE_ID && !x.DELETED).
                //                         Join(
                //                             notesTypeList,
                //                             n => n.NOTES_TYPE_ID,
                //                             nt => nt.NOTES_TYPE_ID,
                //                             (n, nt) => new NotesViewModel
                //                             {
                //                                 NOTES_ID = n.NOTES_ID,
                //                                 NOTES_TYPE_ID = nt.NOTES_TYPE_ID,
                //                                 NotesType = nt.NAME,
                //                                 Notes = n.NOTES
                //                             }
                //                        ).ToList();

                //    var pRACTICE_CODE = new SqlParameter("@PRACTICE_CODE", SqlDbType.VarChar) { Value = profile.PracticeCode };
                //    var cASE_ID = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = obj.CASE_ID };
                //    OrderInfoResult = SpRepository<FOX_TBL_ORDER_INFORMATION>.GetListWithStoreProcedure(@"exec [FOX_GET_ORDER_INFORMATION] @PRACTICE_CODE,@CASE_ID", pRACTICE_CODE, cASE_ID).ToList();  // TO BE CHANGE
                //}
                //else
                //{
                    notesTypeList = _NotesTypeRepository.GetMany(x => x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED);
                    notesList = _NotesRepository.GetMany(x => x.CASE_ID == obj.CASE_ID && x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED).
                                         Join(
                                             notesTypeList,
                                             n => n.NOTES_TYPE_ID,
                                             nt => nt.NOTES_TYPE_ID,
                                             (n, nt) => new NotesViewModel
                                             {
                                                 NOTES_ID = n.NOTES_ID,
                                                 NOTES_TYPE_ID = nt.NOTES_TYPE_ID,
                                                 NotesType = nt.NAME,
                                                 Notes = n.NOTES
                                             }
                                        ).ToList();

                    var pRACTICE_CODE = new SqlParameter("@PRACTICE_CODE", SqlDbType.VarChar) { Value = profile.PracticeCode };
                    var cASE_ID = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = obj.CASE_ID };
                    OrderInfoResult = SpRepository<FOX_TBL_ORDER_INFORMATION>.GetListWithStoreProcedure(@"exec [FOX_GET_ORDER_INFORMATION] @PRACTICE_CODE,@CASE_ID", pRACTICE_CODE, cASE_ID).ToList();
                //}
                return new OrderInformationAndNotes
                {
                    Message = "Get Order Information and Cases Notes Successfully.",
                    Success = true,
                    ErrorMessage = "",
                    OrderInformationList = OrderInfoResult,
                    CasesNotesList = notesList
                };
            }
            catch (Exception ex)
            {
                return new OrderInformationAndNotes
                {
                    Message = "",
                    Success = false,
                    ErrorMessage = "Error Occured"
                };
                throw ex;
            }
        }

        public ResponseModel DeleteOrderInformation(FOX_TBL_ORDER_INFORMATION obj, UserProfile profile)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                var orderInfo = _OrderInformationRepository.GetFirst(t => t.ORDER_INFO_ID == obj.ORDER_INFO_ID && !t.DELETED);

                if (orderInfo == null)
                {
                    response.ErrorMessage = "Record Not Exist";
                    response.Message = "Not Deleted";
                    response.Success = false;

                }
                else
                {
                    orderInfo.DELETED = obj.DELETED;
                    orderInfo.MODIFIED_BY = profile.UserName;
                    orderInfo.MODIFIED_DATE = DateTime.Now;
                    _OrderInformationRepository.Update(orderInfo);
                    _CaseContext.SaveChanges();
                    response.ErrorMessage = "Record Exist";
                    response.Message = "Deleted";
                    response.Success = true;
                }
                return response;
            }
            catch (Exception exception)
            {
                //To Do Log Exception here
                throw exception;
            }
        }

        public List<FOX_VW_CASE> GetAllCases(string patient_Account, long practiceCode)
        {

            try
            {
                var _patient_Account = Convert.ToInt64(patient_Account);
                var Result = _vwCaseRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode && t.PATIENT_ACCOUNT == _patient_Account);
                if (Result.Count() > 0)
                {
                    foreach (var rec in Result)
                    {
                        if (rec.ORDERING_REF_SOURCE_ID != null)
                        {
                            if (rec.CERTIFYING_REF_SOURCE_ID == 0 || string.IsNullOrEmpty(rec.CERTIFYING_REF_SOURCE_ID.ToString()))
                            {
                                var referralsource = _SourceRepository.GetFirst(r => r.SOURCE_ID == rec.ORDERING_REF_SOURCE_ID && r.DELETED == false);
                                rec.ObjReferralSource = referralsource;
                            }
                        }
                    }
                }
                if (Result.Any())
                {
                    return Result;
                }
                else
                {
                    return new List<FOX_VW_CASE>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FOX_VW_CALLS_LOG> GetCallInformation(CallReq obj, UserProfile profile)
        {
            try
            {

                var Result = _vwCallsLogRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CASE_ID == obj.CASE_ID);
                if (Result.Any())
                    return Result;
                else
                    return new List<FOX_VW_CALLS_LOG>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WORK_ORDER_INFO_RES> GetWorkOrderInfo(WORK_ORDER_INFO_REQ obj, UserProfile profile)
        {
            try
            {
                var _patient_Account = Convert.ToInt64(obj.PATIENT_ACCOUNT);
                var pATIENT_ACCOUNT = new SqlParameter("PATIENT_ACCOUNT", SqlDbType.BigInt) { Value = _patient_Account };
                var pRACTICE_CODE = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var result = SpRepository<WORK_ORDER_INFO_RES>.GetListWithStoreProcedure(@"exec [FOX_GET_WORK_ORDER_INFO] @PATIENT_ACCOUNT,@PRACTICE_CODE", pATIENT_ACCOUNT, pRACTICE_CODE).ToList();
                if (result.Any())
                    return result;
                else
                    return new List<WORK_ORDER_INFO_RES>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetOrderingRefSourceinfoRes GetOrdering_Ref_Source_info(GetOrderingRefSourceinfoReq obj, UserProfile profile)
        {
            try
            {

                var cASE_ID = new SqlParameter("CASE_ID", SqlDbType.BigInt) { Value = obj.CASE_ID };
                var pRACTICE_CODE = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var result = SpRepository<GetOrderingRefSourceinfoRes>.GetListWithStoreProcedure(@"exec [GetOrdering_Ref_Source_info] @CASE_ID", cASE_ID).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FOX_VW_CASE> GetSmartCases(SmartSearchCasesRequestModel request, UserProfile Profile)
        {
            try
            {
                var _patient_Account = Convert.ToInt64(request.Patient_Account);
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                Task.Delay(2000).ContinueWith((t) =>
                {
                    var Result = (from c in _CaseContext.vwCase
                                  join tt in _CaseContext.CaseType on c.CASE_TYPE_ID equals tt.CASE_TYPE_ID into ct
                                  from tt in ct.DefaultIfEmpty()
                                  join s in _CaseContext.Status on c.CASE_STATUS_ID equals s.CASE_STATUS_ID into st
                                  from s in st.DefaultIfEmpty()
                                  where c.PRACTICE_CODE == Profile.PracticeCode &&
                                  c.PATIENT_ACCOUNT == _patient_Account &&
                                  c.DELETED == false &&
                                  c.CASE_NO.Contains(request.Keyword) ||
                                  c.RT_CASE_NO.Contains(request.Keyword) ||
                                  s.DESCRIPTION.Contains(request.Keyword) ||
                                  tt.DESCRIPTION.Contains(request.Keyword) ||
                                  tt.NAME.Contains(request.Keyword) ||
                                  s.NAME.Contains(request.Keyword)
                                  select c).ToList();
                    if (Result.Any())
                    {
                        return Result;
                    }
                    else
                    {
                        return new List<FOX_VW_CASE>();
                    }
                }, cancellationToken);
                return new List<FOX_VW_CASE>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseAddEditCase DeleteTask(OpenIssueListToDelete model, UserProfile profile) //Delete subtypes and task
        {
            ResponseAddEditCase responseAddEditCase = new ResponseAddEditCase();
            try
            {
                if (model != null && profile != null)
                {
                    var task_Id = model.TaskSubTypeList.Where(e => e.TASK_ID.HasValue && e.TASK_ID.Value != 0).FirstOrDefault().TASK_ID;
                    UnmarkTaskSubtypes(task_Id.Value, model.TaskSubTypeList.Where(e => !e.IS_CHECKED).ToList(), profile);
                    DeleteTask(task_Id.Value, model.CASE_ID, profile);
                    responseAddEditCase.Success = true;
                    responseAddEditCase.ErrorMessage = "";
                    responseAddEditCase.Message = "";
                    responseAddEditCase.CASE_ID = model.CASE_ID;
                }

            }
            catch (Exception exception)
            {
                //To Do Log Exception Here
                //throw;
                responseAddEditCase.Message = exception.Message;
                responseAddEditCase.Success = false;
                responseAddEditCase.ErrorMessage = exception.ToString();
            }
            return responseAddEditCase;
        }

        public void DeleteTask(long task_Id, long case_Id, UserProfile profile)
        {
            try
            {
                var task = _TaskRepository
                        .GetFirst(
                                    t =>
                                        t.CASE_ID == case_Id
                                        && t.TASK_ID == task_Id
                                        && !t.DELETED
                                        && t.PRACTICE_CODE == profile.PracticeCode
                                );
                if (task != null)
                {
                    task.DELETED = true;
                    task.MODIFIED_BY = profile.UserName;
                    task.MODIFIED_DATE = DateTime.Now;

                    _TaskRepository.Update(task);
                    _CaseContext.SaveChanges();
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public List<FOX_TBL_HEAR_ABOUT_US_OPTIONS> GetSmartHearAboutFox(string searchText, UserProfile profile)
        {
            try
            {
                var result = new List<FOX_TBL_HEAR_ABOUT_US_OPTIONS>();
                var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", SqlDbType = SqlDbType.VarChar, Value = searchText };
                result = SpRepository<FOX_TBL_HEAR_ABOUT_US_OPTIONS>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SMART_HEAR_ABOUT_OPTIONS @PRACTICE_CODE, @SEARCH_TEXT",
                          _practiceCode, _searchText);
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public ReferralRegion GetReferralRegionAginstPosId(long posId, UserProfile userProfile)
        {
            var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = userProfile.PracticeCode };
            var _posId = new SqlParameter { ParameterName = "PATIENT_POS_ID", SqlDbType = SqlDbType.BigInt, Value = posId };
            var result = SpRepository<ReferralRegion>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_POS_ID] @PATIENT_POS_ID, @PRACTICE_CODE", _posId, _practiceCode);
            return result;
        }


        public Referral_Region_View GetReferralRegionAgainstORS(long ORDERING_REF_SOURCE_ID, UserProfile userProfile)
        {
            var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = userProfile.PracticeCode };
            var _ORDERING_REF_SOURCE_ID = new SqlParameter { ParameterName = "ORDERING_REF_SOURCE_ID", SqlDbType = SqlDbType.BigInt, Value = ORDERING_REF_SOURCE_ID };
            var result = SpRepository<Referral_Region_View>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_GET_REFERRAL_REGION_BY_ORDERING_REF_SOURCE_ID] @ORDERING_REF_SOURCE_ID, @PRACTICE_CODE", _ORDERING_REF_SOURCE_ID, _practiceCode);
            return result;
        }


        public List<Provider> GetSmartProviders(string searchValue, int disciplineId, UserProfile Profile)
        {
            var parmPracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var smartvalue = new SqlParameter("@SEARCHVALUE", SqlDbType.VarChar) { Value = searchValue };
            var disciplineId_sqlparm = new SqlParameter("@DISCIPLINE_ID", SqlDbType.VarChar) { Value = disciplineId };
            var result = new List<Provider>();
            if (Profile.isTalkRehab)
            {
                result = SpRepository<Provider>.GetListWithStoreProcedure(@"exec [CCR_GET_SMART_PROVIDER_BY_ID] @PRACTICE_CODE, @SEARCHVALUE, @DISCIPLINE_ID", parmPracticeCode, smartvalue, disciplineId_sqlparm).ToList();
            }
            else
            {
                result = SpRepository<Provider>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_PROVIDER_BY_ID] @PRACTICE_CODE, @SEARCHVALUE, @DISCIPLINE_ID", parmPracticeCode, smartvalue, disciplineId_sqlparm).ToList();
            }
            
            if (result.Any())
                return result;
            else
                return new List<Provider>();
        }
        public List<Provider> GetSmartClinicains(SmartSearchReq obj, UserProfile Profile)
        {
            if (obj.TYPE == "Lead")
                obj.TYPE = "";
            var parmPracticeCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
            var smartvalue = new SqlParameter("@SEARCHVALUE", SqlDbType.VarChar) { Value = obj.Keyword };
            var tYPE = new SqlParameter("@DISCIPLINE_NAME", SqlDbType.VarChar) { Value = obj.TYPE??"" };
            var result = SpRepository<Provider>.GetListWithStoreProcedure(@"exec [FOX_GET_SMART_PROVIDER] @PRACTICE_CODE, @SEARCHVALUE, @DISCIPLINE_NAME", parmPracticeCode, smartvalue, tYPE).ToList();
            if (result.Any())
                return result;
            else
                return new List<Provider>();
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

        public List<FOX_TBL_CASE_STATUS> GetAllCaseStatus(UserProfile profile)
        {
            return _CaseStatusRepository.GetMany(row => !row.DELETED && row.PRACTICE_CODE == profile.PracticeCode);
        }

        public List<FOX_VW_CASE> GetPatientCasesList(long patientAccount, UserProfile profile)
        {
            return _vwCaseRepository.GetMany(row => !row.DELETED && row.PRACTICE_CODE == profile.PracticeCode && row.PATIENT_ACCOUNT == patientAccount);
        }

        public CaseAndOpenIssues GetCasesAndOpenIssues(long caseId, UserProfile profile)
        {
            var _caseAndOpenIssues = new CaseAndOpenIssues();
            var _openIssueRequest = new GetOpenIssueListReq();
            _caseAndOpenIssues.CaseDetail = _vwCaseRepository.GetByID(caseId);
            _openIssueRequest.CASE_ID = _caseAndOpenIssues.CaseDetail?.CASE_ID == null ? 0 : _caseAndOpenIssues.CaseDetail.CASE_ID;
            _openIssueRequest.CASE_STATUS_ID = _caseAndOpenIssues.CaseDetail?.CASE_STATUS_ID.Value == null ? 0 : _caseAndOpenIssues.CaseDetail.CASE_STATUS_ID.Value;
            _openIssueRequest.PATIENT_ACCOUNT_STR = _caseAndOpenIssues.CaseDetail?.PATIENT_ACCOUNT.ToString() == null ? "" : _caseAndOpenIssues.CaseDetail.PATIENT_ACCOUNT.ToString();
            _caseAndOpenIssues.OpenIssues = GetOpenIssueList(_openIssueRequest, profile);
            return _caseAndOpenIssues;
        }
        public void UpdatePCPInPatientDemographics(long? Primary_Physician, long Patient_Account, string username)
        {
            var dbPatient = _PatientRepository.GetFirst(e => e.Patient_Account == Patient_Account);
            var dbPatientFOX = _FoxTblPatientRepository.GetFirst(e => e.Patient_Account == Patient_Account);
            if (dbPatientFOX == null)
            {
                dbPatientFOX = new FOX_TBL_PATIENT();
                dbPatientFOX.FOX_TBL_PATIENT_ID = Helper.getMaximumId("FOX_TBL_PATIENT");
                dbPatientFOX.Patient_Account = dbPatient?.Patient_Account == null ? 0 : dbPatient.Patient_Account;
                dbPatientFOX.Created_By = username;
                dbPatientFOX.Created_Date = Helper.GetCurrentDate();
                dbPatientFOX.DELETED = false;
                dbPatientFOX.Modified_By = username;
                dbPatientFOX.Modified_Date = Helper.GetCurrentDate();
                if (dbPatientFOX.PCP != Primary_Physician)
                {
                    dbPatientFOX.PCP = Primary_Physician;
                    _FoxTblPatientRepository.Insert(dbPatientFOX);
                }
            }
            else
            {
                dbPatientFOX.Modified_By = username;
                dbPatientFOX.Modified_Date = Helper.GetCurrentDate();
                if (dbPatientFOX.PCP != Primary_Physician)
                {
                    dbPatientFOX.PCP = Primary_Physician;
                    _FoxTblPatientRepository.Update(dbPatientFOX);
                }
            }
            if (dbPatient != null && dbPatient.PCP != null && dbPatient.PCP != 0)
            {
                var pcp = _OrderingRefSourceRepository.GetByID(dbPatient.PCP);
                if (pcp != null)
                {
                    dbPatient.Modified_By = username;
                    dbPatient.Modified_Date = Helper.GetCurrentDate();
                    dbPatient.Referring_Physician = pcp.REFERRAL_CODE;
                    _PatientRepository.Update(dbPatient);
                }
            }

            _PatientContext.SaveChanges();
        }

        public void UpdatePrimaryPhysicianInActiveandOpenCases(long? PCP_ID, long Patient_Account, long practiceCode)
        {
            var caseStatusList = _CaseStatusRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == practiceCode);
            caseStatusList = caseStatusList.FindAll(e => e.NAME.ToLower() == "act" || e.NAME.ToLower() == "open");
            var CASE_STATUS_ID_ACTIVE = caseStatusList.Find(e => e.NAME.ToLower() == "act").CASE_STATUS_ID;
            var CASE_STATUS_ID_OPEN = caseStatusList.Find(e => e.NAME.ToLower() == "open").CASE_STATUS_ID;
            var casesList = _CaseRepository.GetMany(e => (e.PATIENT_ACCOUNT == Patient_Account));
            casesList = casesList.FindAll(e => e.CASE_STATUS_ID == CASE_STATUS_ID_ACTIVE || e.CASE_STATUS_ID == CASE_STATUS_ID_OPEN);
            if (casesList != null)
            {
                foreach (var case1 in casesList)
                {
                    if (case1.PRIMARY_PHY_ID != PCP_ID)
                    {
                        case1.PRIMARY_PHY_ID = PCP_ID;
                    }
                    _CaseRepository.Update(case1);

                }
                _CaseContext.SaveChanges();
            }
        }


        public GetTreatingProviderRes PopulateTreatingProviderbasedOnPOS(GetTreatingProviderReq obj, UserProfile profile)
        {
            string PROVIDER_CODE = "";
            string provider_name = "";
            long provider_id = 0;
            switch (obj.CASE_DISCIPLINE_NAME)
            {
                case "OT":
                    var facility = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == obj.POS_ID);
                    if (facility != null && facility.OT != null)
                    {
                        PROVIDER_CODE = facility.OT;
                    }
                    break;
                case "PT":
                    facility = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == obj.POS_ID);
                    if (facility != null && facility.PT != null)
                    {
                        PROVIDER_CODE = facility.PT;
                    }
                    break;
                case "ST":
                    facility = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == obj.POS_ID);
                    if (facility != null && facility.ST != null)
                    {
                        PROVIDER_CODE = facility.ST;
                    }
                    break;
                case "EP":
                    facility = _FacilityLocationRepository.GetFirst(e => e.LOC_ID == obj.POS_ID);
                    if (facility != null && facility.EP != null)
                    {
                        PROVIDER_CODE = facility.EP;
                    }
                    break;
                default:
                    break;
            }
            if (PROVIDER_CODE != null && PROVIDER_CODE !="")
            {
                var provider = _FoxProviderClassRepository.GetFirst(e => e.FOX_PROVIDER_CODE == PROVIDER_CODE);
                if (provider != null)
                {
                    provider_id = provider.FOX_PROVIDER_ID;
                    provider_name = provider.PROVIDER_NAME + " | NPI: " + provider.INDIVIDUAL_NPI;

                }
            }
            GetTreatingProviderRes treating_provider = new GetTreatingProviderRes();
            treating_provider.Treating_Provider_Id = provider_id;
            treating_provider.TreatingProviderName = provider_name ;

            return treating_provider;
        }
    }
}