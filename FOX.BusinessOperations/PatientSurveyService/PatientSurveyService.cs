using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace FOX.BusinessOperations.PatientSurveyService
{
    public class PatientSurveyService : IPatientSurveyService
    {
        private long retrycatch = 0;
        private readonly DbContextPatientSurvey _patientSurveyContext = new DbContextPatientSurvey();
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        private readonly GenericRepository<PatientSurveyHistory> _patientSurveyHistoryRepository;
        private readonly GenericRepository<PatientSurveyCallLog> _patientSurveyCallLogRepository;
        private readonly GenericRepository<PatientSurveyFormatType> _psFormatTypeRepository;
        private readonly GenericRepository<RoleToAdd> _roleRepository;
        private readonly GenericRepository<SurveyServiceLog> _surveyServiceLogRepository;
        public PatientSurveyService()
        {
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_patientSurveyContext);
            _patientSurveyHistoryRepository = new GenericRepository<PatientSurveyHistory>(_patientSurveyContext);
            _patientSurveyCallLogRepository = new GenericRepository<PatientSurveyCallLog>(_patientSurveyContext);
            _psFormatTypeRepository = new GenericRepository<PatientSurveyFormatType>(_patientSurveyContext);
            _roleRepository = new GenericRepository<RoleToAdd>(_patientSurveyContext);
            _surveyServiceLogRepository = new GenericRepository<SurveyServiceLog>(_patientSurveyContext);
        }

        public bool SetSurveytProgress(long patientAccount, bool progressStatus)
        {
            try
            {
                var surveyList = _patientSurveyRepository.GetMany(x => !x.DELETED && x.PATIENT_ACCOUNT_NUMBER == patientAccount);
                if (surveyList.Count > 0)
                {
                    foreach (var survey in surveyList)
                    {
                        survey.IN_PROGRESS = progressStatus;
                        _patientSurveyRepository.Update(survey);
                        _patientSurveyRepository.Save();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public PatientSurvey GetSurveyDetailedFromEmail(string surveyId, long practiceCode)
        {
            long suryid = Convert.ToInt64(surveyId);
            var patientSurvey = _patientSurveyRepository.GetByID(suryid);
            return patientSurvey;

        }
        public ResponseModel UpdatePatientSurvey(PatientSurvey patientSurvey, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            string _body = string.Empty;
            string _subject = string.Empty;
            string sendTo = string.Empty;
            string link = string.Empty;
            List<right> rightList = new List<right>();
            string MRN = "";
            var dbSurvey = _patientSurveyRepository.GetByID(patientSurvey.SURVEY_ID);
            if (dbSurvey != null) //update
            {
                var existingSurveyDetails = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == patientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == patientSurvey.SURVEY_ID && r.IS_SURVEYED == true && r.DELETED == false);
                if (existingSurveyDetails == null)
                {
                    SurveyServiceLog surveyServiceLog = new SurveyServiceLog();
                     surveyServiceLog = _surveyServiceLogRepository.GetFirst(r => r.PATIENT_ACCOUNT == patientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == patientSurvey.SURVEY_ID && r.PRACTICE_CODE == patientSurvey.PRACTICE_CODE && r.DELETED == false);
                    if (surveyServiceLog != null)
                    {
                        surveyServiceLog = _surveyServiceLogRepository.GetByID(surveyServiceLog.SURVEY_AUTOMATION_LOG_ID);
                        surveyServiceLog.DELETED = true;
                        surveyServiceLog.IS_SMS = false;
                        surveyServiceLog.IS_EMAIL = false;
                        _surveyServiceLogRepository.Update(surveyServiceLog);
                        _surveyServiceLogRepository.Save();
                    }
                    //if (patientSurvey.IS_SURVEYED == true)
                    //{
                    //    AddPatientSurveyHistory(dbSurvey, profile);
                    //}
                    if (dbSurvey.IS_SURVEYED == false)
                    {
                        dbSurvey.SURVEY_COMPLETED_DATE = Helper.GetCurrentDate();
                    }
                    dbSurvey.IS_SURVEYED = true;
                    if (!string.IsNullOrEmpty(patientSurvey.SURVEY_STATUS_BASE))
                    {
                        if (patientSurvey.SURVEY_STATUS_BASE.Equals("Completed") && !patientSurvey.SURVEY_STATUS_CHILD.Equals("Deceased") && !patientSurvey.SURVEY_STATUS_CHILD.Equals("Not Interested") && !patientSurvey.SURVEY_STATUS_CHILD.Equals("Unable to Complete Survey"))
                        {
                            dbSurvey.IS_CONTACT_HQ = patientSurvey.IS_CONTACT_HQ;
                            if (dbSurvey.IS_CONTACT_HQ.Value == true)
                            {
                                dbSurvey.IS_RESPONSED_BY_HQ = patientSurvey.IS_RESPONSED_BY_HQ;
                                dbSurvey.IS_QUESTION_ANSWERED = patientSurvey.IS_QUESTION_ANSWERED;
                            }
                            else
                            {
                                dbSurvey.IS_RESPONSED_BY_HQ = null;
                                dbSurvey.IS_QUESTION_ANSWERED = null;
                            }
                            dbSurvey.IS_REFERABLE = patientSurvey.IS_REFERABLE;
                            dbSurvey.IS_IMPROVED_SETISFACTION = patientSurvey.IS_IMPROVED_SETISFACTION;
                            dbSurvey.FEEDBACK = patientSurvey.FEEDBACK;
                            dbSurvey.SURVEY_FLAG = patientSurvey.SURVEY_FLAG;
                            dbSurvey.IS_PROTECTIVE_EQUIPMENT = patientSurvey.IS_PROTECTIVE_EQUIPMENT;
                        }
                        else
                        {
                            dbSurvey.IS_CONTACT_HQ = null;
                            dbSurvey.IS_RESPONSED_BY_HQ = null;
                            dbSurvey.IS_QUESTION_ANSWERED = null;
                            dbSurvey.IS_REFERABLE = null;
                            dbSurvey.IS_IMPROVED_SETISFACTION = null;
                            dbSurvey.FEEDBACK = null;
                            dbSurvey.SURVEY_FLAG = null;
                            dbSurvey.IS_PROTECTIVE_EQUIPMENT = null;
                        }
                        if (patientSurvey.SURVEY_STATUS_BASE.Equals("Incomplete") && (patientSurvey.SURVEY_STATUS_CHILD.Equals("Callback") || patientSurvey.SURVEY_STATUS_CHILD.Equals("New Case Same Discipline")))
                        {
                            dbSurvey.FEEDBACK = patientSurvey.FEEDBACK;
                        }
                        if (patientSurvey.SURVEY_STATUS_BASE.Equals("Completed") && (patientSurvey.SURVEY_STATUS_CHILD.Equals("Unable to Complete Survey") || patientSurvey.SURVEY_STATUS_CHILD.Equals("Not Interested")))
                        {
                            dbSurvey.FEEDBACK = patientSurvey.FEEDBACK;
                            dbSurvey.SURVEY_FLAG = patientSurvey.SURVEY_FLAG;
                        }
                    }
                    if (patientSurvey.ACTIVE_FORMAT == "New Format")
                    {
                        dbSurvey.SURVEY_FORMAT_TYPE = "New Format";
                    }
                    else
                    {
                        dbSurvey.SURVEY_FORMAT_TYPE = "Old Format";
                        dbSurvey.IS_QUESTION_ANSWERED = null;
                    }
                    dbSurvey.SURVEY_STATUS_BASE = patientSurvey.SURVEY_STATUS_BASE;
                    dbSurvey.SURVEY_STATUS_CHILD = patientSurvey.SURVEY_STATUS_CHILD;
                    dbSurvey.MODIFIED_BY = profile.UserName;
                    dbSurvey.MODIFIED_DATE = Helper.GetCurrentDate();
                    dbSurvey.IS_EXCEPTIONAL = patientSurvey.IS_EXCEPTIONAL;
                    AddPatientSurveyHistory(dbSurvey, profile);

                    var surveyId = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = dbSurvey.SURVEY_ID };
                    var practiceCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = dbSurvey.PRACTICE_CODE ?? null };
                    var clientId = new SqlParameter { ParameterName = "@FACILITY_OR_CLIENT_ID", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.FACILITY_OR_CLIENT_ID ?? null };
                    var patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT_NUMBER", SqlDbType = SqlDbType.BigInt, Value = dbSurvey.PATIENT_ACCOUNT_NUMBER ?? null };
                    var resLastName = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_LAST_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.RESPONSIBLE_PARTY_LAST_NAME ?? null };
                    var resFirstName = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_FIRST_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.RESPONSIBLE_PARTY_FIRST_NAME ?? null };
                    var resMidName = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_MIDDLE_INITIAL", SqlDbType = SqlDbType.Char, Value = dbSurvey.RESPONSIBLE_PARTY_MIDDLE_INITIAL ?? null };
                    var resPartyAdd = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_ADDRESS", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.RESPONSIBLE_PARTY_ADDRESS ?? null };
                    var resPartyCity = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_CITY", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.RESPONSIBLE_PARTY_CITY ?? null };
                    var tesPartyStat = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_STATE", SqlDbType = SqlDbType.Char, Value = dbSurvey.RESPONSIBLE_PARTY_STATE ?? null };
                    var restPartyZip = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_ZIP_CODE", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.RESPONSIBLE_PARTY_ZIP_CODE ?? null };
                    var resPartyPhone = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_TELEPHONE", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.RESPONSIBLE_PARTY_TELEPHONE ?? null };
                    var restPartySSN = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_SSN", SqlDbType = SqlDbType.Char, Value = dbSurvey.RESPONSIBLE_PARTY_SSN ?? null };
                    var restPartSex = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_SEX", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.RESPONSIBLE_PARTY_SEX ?? null };
                    var restPartDOB = new SqlParameter { ParameterName = "@RESPONSIBLE_PARTY_DATE_OF_BIRTH", SqlDbType = SqlDbType.DateTime, Value = dbSurvey.RESPONSIBLE_PARTY_DATE_OF_BIRTH ?? null };
                    var patLastName = new SqlParameter { ParameterName = "@PATIENT_LAST_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PATIENT_LAST_NAME ?? null };
                    var patFirstName = new SqlParameter { ParameterName = "@PATIENT_FIRST_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PATIENT_FIRST_NAME ?? null };
                    var patMidName = new SqlParameter { ParameterName = "@PATIENT_MIDDLE_INITIAL", SqlDbType = SqlDbType.Char, Value = dbSurvey.PATIENT_MIDDLE_INITIAL ?? null };
                    var patAddress = new SqlParameter { ParameterName = "@PATIENT_ADDRESS", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PATIENT_ADDRESS ?? null };
                    var patCity = new SqlParameter { ParameterName = "@PATIENT_CITY", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PATIENT_CITY ?? null };
                    var patState = new SqlParameter { ParameterName = "@PATIENT_STATE", SqlDbType = SqlDbType.Char, Value = dbSurvey.PATIENT_STATE ?? null };
                    var patZIP = new SqlParameter { ParameterName = "@PATIENT_ZIP_CODE", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PATIENT_ZIP_CODE ?? null };
                    var patPhone = new SqlParameter { ParameterName = "@PATIENT_TELEPHONE_NUMBER", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PATIENT_TELEPHONE_NUMBER ?? null };
                    var patSSN = new SqlParameter { ParameterName = "@PATIENT_SOCIAL_SECURITY_NUMBER", SqlDbType = SqlDbType.Char, Value = dbSurvey.PATIENT_SOCIAL_SECURITY_NUMBER ?? null };
                    var patGender = new SqlParameter { ParameterName = "@PATIENT_GENDER", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PATIENT_GENDER ?? null };
                    var patDOB = new SqlParameter { ParameterName = "@PATIENT_DATE_OF_BIRTH", SqlDbType = SqlDbType.DateTime, Value = dbSurvey.PATIENT_DATE_OF_BIRTH ?? null };
                    var altLastName = new SqlParameter { ParameterName = "@ALTERNATE_CONTACT_LAST_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.ALTERNATE_CONTACT_LAST_NAME ?? null };
                    var altFirstName = new SqlParameter { ParameterName = "@ALTERNATE_CONTACT_FIRST_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.ALTERNATE_CONTACT_FIRST_NAME ?? null };
                    var altMidName = new SqlParameter { ParameterName = "@ALTERNATE_CONTACT_MIDDLE_INITIAL", SqlDbType = SqlDbType.Char, Value = dbSurvey.ALTERNATE_CONTACT_MIDDLE_INITIAL ?? null };
                    var altPhone = new SqlParameter { ParameterName = "@ALTERNATE_CONTACT_TELEPHONE", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.ALTERNATE_CONTACT_TELEPHONE ?? null };
                    var emrLocCode = new SqlParameter { ParameterName = "@EMR_LOCATION_CODE", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.EMR_LOCATION_CODE ?? null };
                    var emrLocDes = new SqlParameter { ParameterName = "@EMR_LOCATION_DESCRIPTION", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.EMR_LOCATION_DESCRIPTION ?? null };
                    var servicePaymentDesc = new SqlParameter { ParameterName = "@SERVICE_OR_PAYMENT_DESCRIPTION", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.SERVICE_OR_PAYMENT_DESCRIPTION ?? null };
                    var provider = new SqlParameter { ParameterName = "@PROVIDER", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PROVIDER ?? null };
                    var region = new SqlParameter { ParameterName = "@REGION", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.REGION ?? null };
                    var lastVisitDate = new SqlParameter { ParameterName = "@LAST_VISIT_DATE", SqlDbType = SqlDbType.DateTime, Value = dbSurvey.LAST_VISIT_DATE ?? null };
                    var dischargeDate = new SqlParameter { ParameterName = "@DISCHARGE_DATE", SqlDbType = SqlDbType.DateTime, Value = dbSurvey.DISCHARGE_DATE ?? null };
                    var attendingDocName = new SqlParameter { ParameterName = "@ATTENDING_DOCTOR_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.ATTENDING_DOCTOR_NAME ?? null };
                    var ptOtSlp = new SqlParameter { ParameterName = "@PT_OT_SLP", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PT_OT_SLP ?? null };
                    var referralDate = new SqlParameter { ParameterName = "@REFERRAL_DATE", SqlDbType = SqlDbType.DateTime, Value = dbSurvey.REFERRAL_DATE ?? null };
                    var procTranCode = new SqlParameter { ParameterName = "@PROCEDURE_OR_TRAN_CODE", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.PROCEDURE_OR_TRAN_CODE ?? null };
                    var servicePaymentAmnt = new SqlParameter { ParameterName = "@SERVICE_OR_PAYMENT_AMOUNT", SqlDbType = SqlDbType.Money, Value = dbSurvey.SERVICE_OR_PAYMENT_AMOUNT ?? null };
                    var isContactHQ = new SqlParameter { ParameterName = "@IS_CONTACT_HQ", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IS_CONTACT_HQ ?? null };
                    var isResponsedByHq = new SqlParameter { ParameterName = "@IS_RESPONSED_BY_HQ", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IS_RESPONSED_BY_HQ ?? null };
                    var isQuestionAnswered = new SqlParameter { ParameterName = "@IS_QUESTION_ANSWERED", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IS_QUESTION_ANSWERED ?? null };
                    var isReferrable = new SqlParameter { ParameterName = "@IS_REFERABLE", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IS_REFERABLE ?? null };
                    var isImprovedSetisfaction = new SqlParameter { ParameterName = "@IS_IMPROVED_SETISFACTION", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IS_IMPROVED_SETISFACTION ?? null };
                    var feedback = new SqlParameter { ParameterName = "@FEEDBACK", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.FEEDBACK ?? null };
                    var surveyFlag = new SqlParameter { ParameterName = "@SURVEY_FLAG", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.SURVEY_FLAG ?? null };
                    var surveyStatusBase = new SqlParameter { ParameterName = "@SURVEY_STATUS_BASE", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.SURVEY_STATUS_BASE ?? null };
                    var surveyStatusChild = new SqlParameter { ParameterName = "@SURVEY_STATUS_CHILD", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.SURVEY_STATUS_CHILD ?? null };
                    var surveyFormat = new SqlParameter { ParameterName = "@SURVEY_FORMAT_TYPE", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.SURVEY_FORMAT_TYPE ?? null };
                    var isSurveyed = new SqlParameter { ParameterName = "@IS_SURVEYED", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IS_SURVEYED ?? null };
                    var inProgress = new SqlParameter { ParameterName = "@IN_PROGRESS", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IN_PROGRESS ?? null };
                    var fileName = new SqlParameter { ParameterName = "@FILE_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.FILE_NAME ?? null };
                    var sheetName = new SqlParameter { ParameterName = "@SHEET_NAME", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.SHEET_NAME ?? null };
                    var totalRecordInFile = new SqlParameter { ParameterName = "@TOTAL_RECORD_IN_FILE", SqlDbType = SqlDbType.BigInt, Value = dbSurvey.TOTAL_RECORD_IN_FILE ?? null };
                    var createdBy = new SqlParameter { ParameterName = "@CREATED_BY", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.CREATED_BY };
                    var createdDate = new SqlParameter { ParameterName = "@CREATED_DATE", SqlDbType = SqlDbType.DateTime, Value = dbSurvey.@CREATED_DATE };
                    var modifiedBy = new SqlParameter { ParameterName = "@MODIFIED_BY", SqlDbType = SqlDbType.VarChar, Value = dbSurvey.MODIFIED_BY };
                    var modifiedDate = new SqlParameter { ParameterName = "@MODIFIED_DATE", SqlDbType = SqlDbType.DateTime, Value = dbSurvey.MODIFIED_DATE };
                    var delete = new SqlParameter { ParameterName = "@DELETED", SqlDbType = SqlDbType.Bit, Value = dbSurvey.DELETED };
                    var isExceptional = new SqlParameter { ParameterName = "@IS_EXCEPTIONAL", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IS_EXCEPTIONAL };
                    var isprotectiveEquipment = new SqlParameter { ParameterName = "@IS_PROTECTIVE_EQUIPMENT", SqlDbType = SqlDbType.Bit, Value = dbSurvey.IS_PROTECTIVE_EQUIPMENT };
                    var surveyCompletedDate = new SqlParameter { ParameterName = "@SURVEY_COMPLETED_DATE", SqlDbType = SqlDbType.DateTime, Value = dbSurvey.SURVEY_COMPLETED_DATE };

                    if (dbSurvey.PRACTICE_CODE == null)
                    {
                        practiceCode.Value = DBNull.Value;
                    }
                    if (dbSurvey.FACILITY_OR_CLIENT_ID == null)
                    {
                        clientId.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_ACCOUNT_NUMBER == null)
                    {
                        patientAccount.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_LAST_NAME == null)
                    {
                        resLastName.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_FIRST_NAME == null)
                    {
                        resFirstName.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_MIDDLE_INITIAL == null)
                    {
                        resMidName.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_ADDRESS == null)
                    {
                        resPartyAdd.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_CITY == null)
                    {
                        resPartyCity.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_STATE == null)
                    {
                        tesPartyStat.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_ZIP_CODE == null)
                    {
                        restPartyZip.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_TELEPHONE == null)
                    {
                        resPartyPhone.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_SSN == null)
                    {
                        restPartySSN.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_SEX == null)
                    {
                        restPartSex.Value = DBNull.Value;
                    }
                    if (dbSurvey.RESPONSIBLE_PARTY_DATE_OF_BIRTH == null)
                    {
                        restPartDOB.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_LAST_NAME == null)
                    {
                        patLastName.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_FIRST_NAME == null)
                    {
                        patFirstName.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_MIDDLE_INITIAL == null)
                    {
                        patMidName.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_ADDRESS == null)
                    {
                        patAddress.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_CITY == null)
                    {
                        patCity.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_STATE == null)
                    {
                        patState.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_ZIP_CODE == null)
                    {
                        patZIP.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_TELEPHONE_NUMBER == null)
                    {
                        patPhone.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_SOCIAL_SECURITY_NUMBER == null)
                    {
                        patSSN.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_GENDER == null)
                    {
                        patGender.Value = DBNull.Value;
                    }
                    if (dbSurvey.PATIENT_DATE_OF_BIRTH == null)
                    {
                        patDOB.Value = DBNull.Value;
                    }
                    if (dbSurvey.ALTERNATE_CONTACT_LAST_NAME == null)
                    {
                        altLastName.Value = DBNull.Value;
                    }
                    if (dbSurvey.ALTERNATE_CONTACT_FIRST_NAME == null)
                    {
                        altFirstName.Value = DBNull.Value;
                    }
                    if (dbSurvey.ALTERNATE_CONTACT_MIDDLE_INITIAL == null)
                    {
                        altMidName.Value = DBNull.Value;
                    }
                    if (dbSurvey.ALTERNATE_CONTACT_TELEPHONE == null)
                    {
                        altPhone.Value = DBNull.Value;
                    }
                    if (dbSurvey.EMR_LOCATION_CODE == null)
                    {
                        emrLocCode.Value = DBNull.Value;
                    }
                    if (dbSurvey.EMR_LOCATION_DESCRIPTION == null)
                    {
                        emrLocDes.Value = DBNull.Value;
                    }
                    if (dbSurvey.SERVICE_OR_PAYMENT_DESCRIPTION == null)
                    {
                        servicePaymentDesc.Value = DBNull.Value;
                    }
                    if (dbSurvey.PROVIDER == null)
                    {
                        provider.Value = DBNull.Value;
                    }
                    if (dbSurvey.REGION == null)
                    {
                        region.Value = DBNull.Value;
                    }
                    if (dbSurvey.LAST_VISIT_DATE == null)
                    {
                        lastVisitDate.Value = DBNull.Value;
                    }
                    if (dbSurvey.DISCHARGE_DATE == null)
                    {
                        dischargeDate.Value = DBNull.Value;
                    }
                    if (dbSurvey.ATTENDING_DOCTOR_NAME == null)
                    {
                        attendingDocName.Value = DBNull.Value;
                    }
                    if (dbSurvey.PT_OT_SLP == null)
                    {
                        ptOtSlp.Value = DBNull.Value;
                    }
                    if (dbSurvey.REFERRAL_DATE == null)
                    {
                        referralDate.Value = DBNull.Value;
                    }
                    if (dbSurvey.PROCEDURE_OR_TRAN_CODE == null)
                    {
                        procTranCode.Value = DBNull.Value;
                    }
                    if (dbSurvey.SERVICE_OR_PAYMENT_AMOUNT == null)
                    {
                        servicePaymentAmnt.Value = DBNull.Value;
                    }
                    if (dbSurvey.IN_PROGRESS == null)
                    {
                        inProgress.Value = DBNull.Value;
                    }
                    if (dbSurvey.IS_SURVEYED == null)
                    {
                        isSurveyed.Value = DBNull.Value;
                    }
                    if (dbSurvey.IS_CONTACT_HQ == null)
                    {
                        isContactHQ.Value = DBNull.Value;
                    }
                    if (dbSurvey.IS_RESPONSED_BY_HQ == null)
                    {
                        isResponsedByHq.Value = DBNull.Value;
                    }
                    if (dbSurvey.IS_REFERABLE == null)
                    {
                        isReferrable.Value = DBNull.Value;
                    }
                    if (dbSurvey.IS_IMPROVED_SETISFACTION == null)
                    {
                        isImprovedSetisfaction.Value = DBNull.Value;
                    }
                    if (dbSurvey.IS_QUESTION_ANSWERED == null)
                    {
                        isQuestionAnswered.Value = DBNull.Value;
                    }
                    if (dbSurvey.FEEDBACK == null)
                    {
                        feedback.Value = DBNull.Value;
                    }
                    if (dbSurvey.SURVEY_FLAG == null)
                    {
                        surveyFlag.Value = DBNull.Value;
                    }
                    if (dbSurvey.SURVEY_STATUS_BASE == null)
                    {
                        surveyStatusBase.Value = DBNull.Value;
                    }
                    if (dbSurvey.SURVEY_STATUS_CHILD == null)
                    {
                        surveyStatusChild.Value = DBNull.Value;
                    }
                    if (dbSurvey.SURVEY_FORMAT_TYPE == null)
                    {
                        surveyFormat.Value = DBNull.Value;
                    }
                    if (dbSurvey.FILE_NAME == null)
                    {
                        fileName.Value = DBNull.Value;
                    }
                    if (dbSurvey.SHEET_NAME == null)
                    {
                        sheetName.Value = DBNull.Value;
                    }
                    if (dbSurvey.TOTAL_RECORD_IN_FILE == null)
                    {
                        totalRecordInFile.Value = DBNull.Value;
                    }
                    if (dbSurvey.IS_EXCEPTIONAL == null)
                    {
                        isExceptional.Value = DBNull.Value;
                    }
                    if (dbSurvey.IS_PROTECTIVE_EQUIPMENT == null)
                    {
                        isprotectiveEquipment.Value = DBNull.Value;
                    }
                    if (dbSurvey.SURVEY_COMPLETED_DATE == null)
                    {
                        surveyCompletedDate.Value = DBNull.Value;
                    }

                    var PatientSurveyList = SpRepository<PatientSurvey>.GetListWithStoreProcedure(@"exec FOX_PROC_UPDTAE_PATIENT_SURVEY
                 @SURVEY_ID, @PRACTICE_CODE, @FACILITY_OR_CLIENT_ID, @PATIENT_ACCOUNT_NUMBER, @RESPONSIBLE_PARTY_LAST_NAME, @RESPONSIBLE_PARTY_FIRST_NAME, @RESPONSIBLE_PARTY_MIDDLE_INITIAL, @RESPONSIBLE_PARTY_ADDRESS,
                 @RESPONSIBLE_PARTY_CITY, @RESPONSIBLE_PARTY_STATE, @RESPONSIBLE_PARTY_ZIP_CODE, @RESPONSIBLE_PARTY_TELEPHONE, @RESPONSIBLE_PARTY_SSN, @RESPONSIBLE_PARTY_SEX, @RESPONSIBLE_PARTY_DATE_OF_BIRTH, @PATIENT_LAST_NAME, 
                 @PATIENT_FIRST_NAME, @PATIENT_MIDDLE_INITIAL, @PATIENT_ADDRESS, @PATIENT_CITY, @PATIENT_STATE, @PATIENT_ZIP_CODE, @PATIENT_TELEPHONE_NUMBER, @PATIENT_SOCIAL_SECURITY_NUMBER, @PATIENT_GENDER, @PATIENT_DATE_OF_BIRTH,
                 @ALTERNATE_CONTACT_LAST_NAME, @ALTERNATE_CONTACT_FIRST_NAME, @ALTERNATE_CONTACT_MIDDLE_INITIAL, @ALTERNATE_CONTACT_TELEPHONE, @EMR_LOCATION_CODE, @EMR_LOCATION_DESCRIPTION, @SERVICE_OR_PAYMENT_DESCRIPTION, @PROVIDER, 
                 @REGION, @LAST_VISIT_DATE, @DISCHARGE_DATE, @ATTENDING_DOCTOR_NAME, @PT_OT_SLP, @REFERRAL_DATE, @PROCEDURE_OR_TRAN_CODE, @SERVICE_OR_PAYMENT_AMOUNT, @IS_CONTACT_HQ, @IS_RESPONSED_BY_HQ, @IS_QUESTION_ANSWERED,
                 @IS_REFERABLE, @IS_IMPROVED_SETISFACTION, @FEEDBACK, @SURVEY_FLAG, @SURVEY_STATUS_BASE, @SURVEY_STATUS_CHILD, @SURVEY_FORMAT_TYPE, @IS_SURVEYED, @IN_PROGRESS, @FILE_NAME, @SHEET_NAME, @TOTAL_RECORD_IN_FILE, 
                 @CREATED_BY, @CREATED_DATE, @MODIFIED_BY, @MODIFIED_DATE, @DELETED,@IS_EXCEPTIONAL, @IS_PROTECTIVE_EQUIPMENT, @SURVEY_COMPLETED_DATE"
                     , surveyId, practiceCode, clientId, patientAccount, resLastName, resFirstName, resMidName, resPartyAdd, resPartyCity, tesPartyStat, restPartyZip, resPartyPhone, restPartySSN, restPartSex, restPartDOB
                     , patLastName, patFirstName, patMidName, patAddress, patCity, patState, patZIP, patPhone, patSSN, patGender, patDOB, altLastName, altFirstName, altMidName, altPhone, emrLocCode, emrLocDes
                     , servicePaymentDesc, provider, region, lastVisitDate, dischargeDate, attendingDocName, ptOtSlp, referralDate, procTranCode, servicePaymentAmnt, isContactHQ, isResponsedByHq, isQuestionAnswered
                     , isReferrable, isImprovedSetisfaction, feedback, surveyFlag, surveyStatusBase, surveyStatusChild, surveyFormat, isSurveyed, inProgress, fileName, sheetName, totalRecordInFile, createdBy
                     , createdDate, modifiedBy, modifiedDate, delete, isExceptional, isprotectiveEquipment, surveyCompletedDate);

                    if (patientAccount.Value.ToString() != null && practiceCode.Value != null)
                    {
                        CheckDeceasedPatient(patientAccount.Value.ToString(), Convert.ToInt64(practiceCode.Value));
                    }

                    if (patientSurvey.IS_EXCEPTIONAL == true)
                    {

                        if (AppConfiguration.ClientURL.Contains("https://fox.mtbc.com/") && profile.PracticeCode == 1012714)
                        {
                            sendTo = WebConfigurationManager.AppSettings["PatientSurveyEmailAddressForLive"].ToString();
                        }
                        else
                        {
                            sendTo = WebConfigurationManager.AppSettings["PatientSurveyEmailAddressForTest"].ToString();
                        }
                        _subject = "Exceptional feedback ";
                        if (!string.IsNullOrEmpty(patientSurvey.SURVEY_STATUS_CHILD))
                        {
                            if (patientSurvey.SURVEY_STATUS_CHILD.ToLower() == "not recommended")
                            {
                                _subject += "(NR). ";
                            }
                            else if (patientSurvey.SURVEY_STATUS_CHILD.ToLower() == "recommended")
                            {
                                _subject += "(R). ";
                            }
                        }
                        _body = "<b>Body:</b> <br> <p>An exceptional feedback was received with following specifics:</p>";
                        if (!string.IsNullOrEmpty(patientSurvey.PATIENT_FULL_NAME))
                        {
                            _body += "<p> Patient: " + patientSurvey.PATIENT_FULL_NAME + "</p>";
                        }
                        if (patientSurvey.PATIENT_ACCOUNT_NUMBER != null)
                        {
                            if (patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString().Length == 7)
                            {
                                MRN = "0" + patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString();
                            }
                            if (patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString().Length == 6)
                            {
                                MRN = "00" + patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString();
                            }
                            if (patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString().Length == 5)
                            {
                                MRN = "000" + patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString();
                            }
                            if (patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString().Length == 4)
                            {
                                MRN = "0000" + patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString();
                            }
                            if (patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString().Length == 3)
                            {
                                MRN = "00000" + patientSurvey.PATIENT_ACCOUNT_NUMBER.ToString();
                            }

                            _body += "<p>MRN: " + MRN + "</p>";
                            _subject += MRN;
                            _subject += "_" + profile.UserName;
                        }

                        _body += "<p>Surveyed by: " + profile.UserName + "</p>" + "<p>Survey date & time: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "</p> <br>";
                        if (!string.IsNullOrEmpty(patientSurvey.PROVIDER))
                        {
                            _body += "<p>Provider: " + patientSurvey.PROVIDER + "</p> ";
                        }
                        if (!string.IsNullOrEmpty(patientSurvey.ATTENDING_DOCTOR_NAME))
                        {
                            _body += "<p>Attending Doctor: " + patientSurvey.ATTENDING_DOCTOR_NAME + "</p> ";
                        }
                        if (!string.IsNullOrEmpty(patientSurvey.REGION))
                        {
                            _body += "<p>Region: " + patientSurvey.REGION + "</p> ";
                        }

                        link = AppConfiguration.ClientURL + @"#/Reporting/PatientSurveyDetail?value=" + HttpUtility.UrlEncode(dbSurvey.SURVEY_ID.ToString());
                        link += "&name=" + profile.UserEmailAddress;
                        _body += "<p>Please   <a href = " + link + "> " + " click here to login</a>" + " and see the survey details.</p>";
                        //_body += "<h3 style=font-weight:normal;margin:0;><b>Auditor: </b><a href=" +link + " > " + " click here to login </a></h3>";
                        if (!string.IsNullOrEmpty(sendTo))
                        {
                            Helper.Email(sendTo, _subject, _body, profile, null, null, null, null);
                        }
                    }
                    response.Message = "Add/Update successfull";
                    response.Success = true;
                    //_patientSurveyRepository.Update(dbSurvey);
                    //_patientSurveyRepository.Save();
                }
                else
                {
                    response.Message = "Not Add/Update";
                    response.Success = true;
                }
            }
            return response;
        }

        private void AddPatientSurveyHistory(PatientSurvey patientSurvey, UserProfile profile)
        {
            var surveyID = long.Parse(patientSurvey.SURVEY_ID_Str);
            PatientSurveyHistory surveyHistory = new PatientSurveyHistory();
            surveyHistory.SURVEY_HISTORY_ID = Helper.getMaximumId("FOX_SURVEY_HISTORY_ID");
            surveyHistory.PRACTICE_CODE = profile.PracticeCode;
            surveyHistory.SURVEY_ID = surveyID;
            surveyHistory.PATIENT_ACCOUNT = patientSurvey.PATIENT_ACCOUNT_NUMBER;
            surveyHistory.IS_CONTACT_HQ = patientSurvey.IS_CONTACT_HQ;
            surveyHistory.IS_RESPONSED_BY_HQ = patientSurvey.IS_RESPONSED_BY_HQ;
            surveyHistory.IS_REFERABLE = patientSurvey.IS_REFERABLE;
            surveyHistory.IS_IMPROVED_SETISFACTION = patientSurvey.IS_IMPROVED_SETISFACTION;
            surveyHistory.IS_QUESTION_ANSWERED = patientSurvey.IS_QUESTION_ANSWERED;
            surveyHistory.FEEDBACK = patientSurvey.FEEDBACK;
            surveyHistory.SURVEY_FLAG = patientSurvey.SURVEY_FLAG;
            surveyHistory.SURVEY_STATUS_BASE = patientSurvey.SURVEY_STATUS_BASE;
            surveyHistory.SURVEY_STATUS_CHILD = patientSurvey.SURVEY_STATUS_CHILD;
            surveyHistory.SURVEY_BY = patientSurvey.MODIFIED_BY;
            surveyHistory.SURVEY_DATE = patientSurvey.MODIFIED_DATE;
            surveyHistory.CREATED_BY = profile.UserName;
            surveyHistory.CREATED_DATE = Helper.GetCurrentDate();
            surveyHistory.DELETED = false;
            surveyHistory.IS_EXCEPTIONAL = patientSurvey.IS_EXCEPTIONAL;
            //_patientSurveyHistoryRepository.Insert(surveyHistory);
            //_patientSurveyHistoryRepository.Save();

            var surveyHistoryId = new SqlParameter { ParameterName = "@FOX_SURVEY_HISTORY_ID", SqlDbType = SqlDbType.BigInt, Value = surveyHistory.SURVEY_HISTORY_ID };
            var practiceCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = surveyHistory.PRACTICE_CODE };
            var surveyId = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = surveyHistory.SURVEY_ID };
            var patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = surveyHistory.PATIENT_ACCOUNT };
            var isContactHG = new SqlParameter { ParameterName = "@IS_CONTACT_HQ", SqlDbType = SqlDbType.Bit, Value = surveyHistory.IS_CONTACT_HQ };
            var isResponsedByHQ = new SqlParameter { ParameterName = "@IS_RESPONSED_BY_HQ", SqlDbType = SqlDbType.Bit, Value = surveyHistory.IS_RESPONSED_BY_HQ };
            var isReferrable = new SqlParameter { ParameterName = "@IS_REFERABLE", SqlDbType = SqlDbType.Bit, Value = surveyHistory.IS_REFERABLE };
            var isImprovedSetisfaction = new SqlParameter { ParameterName = "@IS_IMPROVED_SETISFACTION", SqlDbType = SqlDbType.Bit, Value = surveyHistory.IS_IMPROVED_SETISFACTION };
            var isQuestionAnswered = new SqlParameter { ParameterName = "@IS_QUESTION_ANSWERED", SqlDbType = SqlDbType.Bit, Value = surveyHistory.IS_QUESTION_ANSWERED };
            var feedback = new SqlParameter { ParameterName = "@FEEDBACK", SqlDbType = SqlDbType.VarChar, Value = surveyHistory.FEEDBACK };
            var surveyFlag = new SqlParameter { ParameterName = "@SURVEY_FLAG", SqlDbType = SqlDbType.VarChar, Value = surveyHistory.SURVEY_FLAG };
            var surveyStatusBase = new SqlParameter { ParameterName = "@SURVEY_STATUS_BASE", SqlDbType = SqlDbType.VarChar, Value = surveyHistory.SURVEY_STATUS_BASE };
            var surveyStatusChild = new SqlParameter { ParameterName = "@SURVEY_STATUS_CHILD", SqlDbType = SqlDbType.VarChar, Value = surveyHistory.SURVEY_STATUS_CHILD };
            var surveyBy = new SqlParameter { ParameterName = "@SURVEY_BY", SqlDbType = SqlDbType.VarChar, Value = surveyHistory.SURVEY_BY };
            var surveyDate = new SqlParameter { ParameterName = "@SURVEY_DATE", SqlDbType = SqlDbType.DateTime, Value = surveyHistory.SURVEY_DATE };
            var createdBy = new SqlParameter { ParameterName = "@CREATED_BY", SqlDbType = SqlDbType.VarChar, Value = surveyHistory.CREATED_BY };
            var createdDate = new SqlParameter { ParameterName = "@CREATED_DATE", SqlDbType = SqlDbType.DateTime, Value = surveyHistory.CREATED_DATE };
            var delete = new SqlParameter { ParameterName = "@DELETED", SqlDbType = SqlDbType.Bit, Value = surveyHistory.DELETED };
            var isExceptional = new SqlParameter { ParameterName = "@IS_EXCEPTIONAL", SqlDbType = SqlDbType.Bit, Value = surveyHistory.IS_EXCEPTIONAL };
            var isprotectiveEquipment = new SqlParameter { ParameterName = "@IS_PROTECTIVE_EQUIPMENT", SqlDbType = SqlDbType.Bit, Value = surveyHistory.IS_PROTECTIVE_EQUIPMENT };

            if (surveyHistory.PRACTICE_CODE == null)
            {
                practiceCode.Value = DBNull.Value;
            }
            if (surveyHistory.SURVEY_ID == null)
            {
                surveyId.Value = DBNull.Value;
            }
            if (surveyHistory.PATIENT_ACCOUNT == null)
            {
                patientAccount.Value = DBNull.Value;
            }
            if (surveyHistory.IS_CONTACT_HQ == null)
            {
                isContactHG.Value = DBNull.Value;
            }
            if (surveyHistory.IS_RESPONSED_BY_HQ == null)
            {
                isResponsedByHQ.Value = DBNull.Value;
            }
            if (surveyHistory.IS_REFERABLE == null)
            {
                isReferrable.Value = DBNull.Value;
            }
            if (surveyHistory.IS_IMPROVED_SETISFACTION == null)
            {
                isImprovedSetisfaction.Value = DBNull.Value;
            }
            if (surveyHistory.IS_QUESTION_ANSWERED == null)
            {
                isQuestionAnswered.Value = DBNull.Value;
            }
            if (surveyHistory.FEEDBACK == null)
            {
                feedback.Value = DBNull.Value;
            }
            if (surveyHistory.SURVEY_FLAG == null)
            {
                surveyFlag.Value = DBNull.Value;
            }
            if (surveyHistory.SURVEY_STATUS_BASE == null)
            {
                surveyStatusBase.Value = DBNull.Value;
            }
            if (surveyHistory.SURVEY_STATUS_CHILD == null)
            {
                surveyStatusChild.Value = DBNull.Value;
            }
            if (surveyHistory.SURVEY_BY == null)
            {
                surveyBy.Value = DBNull.Value;
            }
            if (surveyHistory.SURVEY_DATE == null)
            {
                surveyDate.Value = DBNull.Value;
            }
            if (surveyHistory.CREATED_DATE == null)
            {
                createdDate.Value = DBNull.Value;
            }
            if (surveyHistory.IS_EXCEPTIONAL == null)
            {
                isExceptional.Value = DBNull.Value;
            }
            if (surveyHistory.IS_PROTECTIVE_EQUIPMENT == null)
            {
                isprotectiveEquipment.Value = DBNull.Value;
            }
            var PatientSurveyList = SpRepository<PatientSurvey>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_PATIENT_SURVEY_HISTORY
                                    @FOX_SURVEY_HISTORY_ID, @PRACTICE_CODE, @SURVEY_ID, @PATIENT_ACCOUNT, @IS_CONTACT_HQ, @IS_RESPONSED_BY_HQ, @IS_REFERABLE, @IS_IMPROVED_SETISFACTION, @FEEDBACK, @SURVEY_FLAG, @SURVEY_STATUS_BASE, @SURVEY_STATUS_CHILD, @SURVEY_BY, @SURVEY_DATE, @CREATED_BY, @CREATED_DATE, @DELETED, @IS_QUESTION_ANSWERED,@IS_EXCEPTIONAL,@IS_PROTECTIVE_EQUIPMENT"
                                    , surveyHistoryId, practiceCode, surveyId, patientAccount, isContactHG, isResponsedByHQ, isReferrable, isImprovedSetisfaction, feedback, surveyFlag, surveyStatusBase, surveyStatusChild, surveyBy, surveyDate, createdBy, createdDate, delete, isQuestionAnswered, isExceptional, isprotectiveEquipment);

        }

        public List<PatientSurvey> GetPatientSurveytList(PatientSurveySearchRequest patientSurveySearchRequest, long practiceCode)
        {
            try
            {
                var surveyId = new SqlParameter { ParameterName = "SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = patientSurveySearchRequest.SURVEY_ID };
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = patientSurveySearchRequest.PATIENT_ACCOUNT_NUMBER };
                var SearchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = patientSurveySearchRequest.SEARCH_TEXT };
                var patientFirstName = new SqlParameter { ParameterName = "PATIENT_FIRST_NAME", Value = patientSurveySearchRequest.PATIENT_FIRST_NAME };
                var patientLastName = new SqlParameter { ParameterName = "PATIENT_LAST_NAME", Value = patientSurveySearchRequest.PATIENT_LAST_NAME };
                var patientMiddleInitial = new SqlParameter { ParameterName = "PATIENT_MIDDLE_INITIAL", Value = patientSurveySearchRequest.PATIENT_MIDDLE_INITIAL };
                var isSurveyed = new SqlParameter { ParameterName = "IS_SURVEYED", SqlDbType = SqlDbType.Int, Value = patientSurveySearchRequest.IS_SURVEYED };

                var PatientSurveyList = SpRepository<PatientSurvey>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_LIST
                                    @PRACTICE_CODE, @PATIENT_ACCOUNT, @IS_SURVEYED", PracticeCode, patientAccount, isSurveyed);
                PatientSurveyList.ForEach(x => x.ACTIVE_FORMAT = GetPSFormat(practiceCode));
                return PatientSurveyList;
            }
            catch (Exception ex)
            {
                if (retrycatch <= 2 && (!string.IsNullOrEmpty(ex.Message) &&
                    ex.Message.Contains("deadlocked on lock resources with another process"))
                    || ((ex.InnerException != null) &&
                    !string.IsNullOrEmpty(ex.InnerException.Message)
                    &&
                    ex.InnerException.Message.Contains("deadlocked on lock resources with another process")))
                {
                    retrycatch = retrycatch + 1;
                    return GetPatientSurveytList(patientSurveySearchRequest, practiceCode);


                }
                else
                {
                    throw ex;
                }

            }

        }
        // Description: This function is trigger to get details of survey, performed by patient (Survey Automation)
        public SurveyServiceLog SurveyPerformByUser(SelectiveSurveyList objSelectiveSurveyListlong, long practiceCode)
        {
            SurveyServiceLog surveyServiceLog = new SurveyServiceLog();
            if (objSelectiveSurveyListlong != null && practiceCode != 0)
            {
                SqlParameter surveyId = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = objSelectiveSurveyListlong.SURVEY_ID };
                SqlParameter pracCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                surveyServiceLog = SpRepository<SurveyServiceLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_AUTOMATED_PERFORM_SURVEY_DETAILS  @SURVEY_ID, @PRACTICE_CODE", surveyId, pracCode);
            }
            return surveyServiceLog;
        }
        public List<string> GetPatientSurveytProviderList(long practiceCode)
        {
            return _patientSurveyRepository.GetMany(x => x.PRACTICE_CODE == practiceCode).Select(x => x.PROVIDER).Distinct().ToList();
        }

        public PSDResults GetPSDResults(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile) // PSD => Patient Survey Dashboard.
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
            var _dateFroms = Helper.getDBNullOrValue("DATE_FROM", patientSurveySearchRequest.DATE_FROM.ToString());
            var _dateTos = Helper.getDBNullOrValue("DATE_TO", patientSurveySearchRequest.DATE_TO.ToString());
            var _region = new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            var _state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var _discipline = new SqlParameter { ParameterName = "DISCIPLINE", Value = patientSurveySearchRequest.DISCIPLINE };
            var _format = new SqlParameter { ParameterName = "FORMAT", Value = patientSurveySearchRequest.FORMAT };

            var psdResult = SpRepository<PSDResults>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PSD_RESULTS
                            @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @REGION, @STATE, @DISCIPLINE, @FORMAT", PracticeCode, _dateFroms, _dateTos, _region, _state, _discipline, _format);
            psdResult.recommendationData = GetPSDStateAndRecommendationWise(patientSurveySearchRequest, profile);
            return psdResult;
        }

        public List<string> GetPSRegionList(string searchText, long practiceCode)
        {
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = searchText };

            var stateList = SpRepository<string>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PS_REGION_LIST @PRACTICE_CODE, @SEARCH_TEXT", PracticeCode, _searchText);
            return stateList;
        }

        public List<string> GetPSStateList()
        {
            //var _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = searchText };
            var stateList = SpRepository<string>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PS_STATES_LIST");
            return stateList;
        }

        public List<PSUserList> GetPSUserList(string searchText, long practiceCode)
        {
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = searchText };

            var result = SpRepository<PSUserList>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_USER_NAME_LIST @PRACTICE_CODE, @SEARCH_TEXT", PracticeCode, _searchText);
            return result;
        }

        public List<PatientSurveyHistory> GetSurveyHistoryList(long patientAccount, long practiceCode)
        {
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _surveyId = new SqlParameter { ParameterName = "SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = 0 };
            var _patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = patientAccount };

            var result = SpRepository<PatientSurveyHistory>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_HISTORY_LIST
                         @PRACTICE_CODE, @SURVEY_ID, @PATIENT_ACCOUNT", PracticeCode, _surveyId, _patientAccount);
            return result;
        }

        public string MakeSurveyCall(PatientSurveyCall patientSurveyCall)
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var data = new System.Collections.Specialized.NameValueCollection();

                    data["Number"] = patientSurveyCall.Number;
                    data["Extension"] = patientSurveyCall.Extension;
                    data["FileName"] = patientSurveyCall.FileName;
                    // data["TeamID"] = AppConfiguration.ACUServiceTeamId;
                    if (data["Extension"] == "8381")
                    {
                        data["TeamID"] = AppConfiguration.ACUServiceTeamIdFor8381;
                    }
                    if (data["Extension"] == "8384")
                    {
                        data["TeamID"] = AppConfiguration.ACUServiceTeamIdFor8384;
                    }
                    if (data["Extension"] != "8384" && data["Extension"] != "8381")
                    {
                        data["TeamID"] = AppConfiguration.ACUServiceTeamId;
                    }
                    //data["Number"] = "7328735133";
                    //data["Extension"] = "1111";
                    //data["FileName"] = patientSurveyCall.FileName;
                    //data["TeamID"] = "BC0A6BAC-8536-432E-8A8F-3D1B6310D9BD";

                    var response = client.UploadValues(AppConfiguration.ACUServiceURL, data);
                    string responseString = System.Text.Encoding.Default.GetString(response);
                    responseString = responseString.Replace("\"", "");
                    return responseString;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void AddUpdateSurveyCall(PatientSurveyCallLog patientSurveyCallLog, UserProfile profile)
        {
            var dbSurveyCall = _patientSurveyCallLogRepository.GetByID(patientSurveyCallLog.SURVEY_CALL_ID);
            if (dbSurveyCall == null)
            {
                var surveyID = long.Parse(patientSurveyCallLog.SURVEY_ID_Str);
                patientSurveyCallLog.SURVEY_CALL_ID = Helper.getMaximumId("FOX_SURVEY_CALL_ID");
                patientSurveyCallLog.SURVEY_ID = surveyID;
                patientSurveyCallLog.PRACTICE_CODE = profile.PracticeCode;
                patientSurveyCallLog.CREATED_BY = profile.UserName;
                patientSurveyCallLog.CREATED_DATE = Helper.GetCurrentDate();
                patientSurveyCallLog.MODIFIED_BY = profile.UserName;
                patientSurveyCallLog.MODIFIED_DATE = Helper.GetCurrentDate();
                patientSurveyCallLog.IS_RECEIVED = false;
                patientSurveyCallLog.DELETED = false;

                //_patientSurveyCallLogRepository.Insert(patientSurveyCallLog);
                //_patientSurveyCallLogRepository.Save();


                var surveyCallId = new SqlParameter { ParameterName = "@SURVEY_CALL_ID", SqlDbType = SqlDbType.BigInt, Value = patientSurveyCallLog.SURVEY_CALL_ID };
                var practiceCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = patientSurveyCallLog.PRACTICE_CODE };
                var acuID = new SqlParameter { ParameterName = "@ACU_CALL_ID", SqlDbType = SqlDbType.BigInt, Value = patientSurveyCallLog.ACU_CALL_ID };
                var surveyId = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = patientSurveyCallLog.SURVEY_ID };
                var patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = patientSurveyCallLog.PATIENT_ACCOUNT };
                var fileName = new SqlParameter { ParameterName = "@FILE_NAME", SqlDbType = SqlDbType.VarChar, Value = patientSurveyCallLog.FILE_NAME };
                var isReceived = new SqlParameter { ParameterName = "@IS_RECEIVED", SqlDbType = SqlDbType.Bit, Value = patientSurveyCallLog.IS_RECEIVED };
                var callOutCome = new SqlParameter { ParameterName = "@CALL_OUT_COME", SqlDbType = SqlDbType.VarChar, Value = patientSurveyCallLog.CALL_OUT_COME };
                var callDuration = new SqlParameter { ParameterName = "@CALL_DURATION", SqlDbType = SqlDbType.VarChar, Value = patientSurveyCallLog.CALL_DURATION };
                var isToPatient = new SqlParameter { ParameterName = "@IS_TO_PATIENT", SqlDbType = SqlDbType.Bit, Value = patientSurveyCallLog.IS_TO_PATIENT };
                var createdBy = new SqlParameter { ParameterName = "@CREATED_BY", SqlDbType = SqlDbType.VarChar, Value = patientSurveyCallLog.CREATED_BY };
                var createdDate = new SqlParameter { ParameterName = "@CREATED_DATE", SqlDbType = SqlDbType.DateTime, Value = patientSurveyCallLog.CREATED_DATE };
                var modifiedBy = new SqlParameter { ParameterName = "@MODIFIED_BY", SqlDbType = SqlDbType.VarChar, Value = patientSurveyCallLog.MODIFIED_BY };
                var modifiedDate = new SqlParameter { ParameterName = "@MODIFIED_DATE", SqlDbType = SqlDbType.DateTime, Value = patientSurveyCallLog.MODIFIED_DATE };
                var delete = new SqlParameter { ParameterName = "@DELETED", SqlDbType = SqlDbType.Bit, Value = patientSurveyCallLog.DELETED };


                if (patientSurveyCallLog.PRACTICE_CODE == null)
                {
                    practiceCode.Value = DBNull.Value;
                }
                if (patientSurveyCallLog.ACU_CALL_ID == null)
                {
                    acuID.Value = DBNull.Value;
                }
                if (patientSurveyCallLog.SURVEY_ID == null)
                {
                    surveyId.Value = DBNull.Value;
                }
                if (patientSurveyCallLog.PATIENT_ACCOUNT == null)
                {
                    patientAccount.Value = DBNull.Value;
                }
                if (patientSurveyCallLog.FILE_NAME == null)
                {
                    fileName.Value = DBNull.Value;
                }
                if (patientSurveyCallLog.IS_RECEIVED == null)
                {
                    isReceived.Value = DBNull.Value;
                }
                if (patientSurveyCallLog.CALL_OUT_COME == null)
                {
                    callOutCome.Value = DBNull.Value;
                }
                if (patientSurveyCallLog.CALL_DURATION == null)
                {
                    callDuration.Value = DBNull.Value;
                }
                if (patientSurveyCallLog.IS_TO_PATIENT == null)
                {
                    isToPatient.Value = DBNull.Value;
                }
                var PatientSurveyList = SpRepository<PatientSurveyCallLog>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_PATIENT_SURVEY_CALL_LOG 
                 @SURVEY_CALL_ID, @PRACTICE_CODE, @ACU_CALL_ID, @SURVEY_ID, @PATIENT_ACCOUNT, @FILE_NAME, @IS_RECEIVED, @CALL_OUT_COME, @CALL_DURATION, @IS_TO_PATIENT, @MODIFIED_BY, @MODIFIED_DATE, @CREATED_BY, @CREATED_DATE, @DELETED"
                 , surveyCallId, practiceCode, acuID, surveyId, patientAccount, fileName, isReceived, callOutCome, callDuration, isToPatient, createdBy, createdDate, modifiedBy, modifiedDate, delete);
            }
        }

        public List<PatientSurveyCallLog> GetSurveyCallList(long patientAccount, long practiceCode)
        {
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _surveyId = new SqlParameter { ParameterName = "SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = 0 };
            var _patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = patientAccount };

            var result = SpRepository<PatientSurveyCallLog>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_CALL_LOG_LIST
                         @PRACTICE_CODE, @SURVEY_ID, @PATIENT_ACCOUNT", PracticeCode, _surveyId, _patientAccount);
            return result;
        }

        public PSSearchData GetPSSearchData(long practiceCode) // Dropdown data from patient survey.
        {
            PSSearchData _psSSearchData = new PSSearchData();
            //_psSSearchData.Providers = _patientSurveyRepository.GetMany(x => x.PRACTICE_CODE == practiceCode && x.PROVIDER != null && !string.IsNullOrEmpty(x.PROVIDER)).OrderBy(x => x.PROVIDER).Select(x => x.PROVIDER).Distinct().ToList();
            _psSSearchData.Providers = GetPSProvidersList(practiceCode, string.Empty);
            _psSSearchData.States = GetPSStatesList(practiceCode, string.Empty);
            _psSSearchData.Regions = GetPSRegionsList(practiceCode, string.Empty);
            //_psSSearchData.States = SpRepository<string>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PS_STATES_LIST");
            //_psSSearchData.Regions = _patientSurveyRepository.GetMany(x => x.PRACTICE_CODE == practiceCode).OrderBy(x => x.REGION).Select(x => x.REGION).Distinct().ToList();
            var List = new List<FeedBackCaller>();
            if (EntityHelper.isTalkRehab)
            {
                var rep = _roleRepository.GetSingleOrDefault(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.ROLE_NAME == "Feedback Caller")?.ROLE_ID;
                if (!rep.Equals(null))
                {
                    var _roleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.BigInt, Value = rep };
                    var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };

                    _psSSearchData.Users = SpRepository<PSUserList>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_FEEDBACK_CALLER_NAME_LIST @PRACTICE_CODE, @ROLE_ID", _practiceCode, _roleId);
                }
                else
                {
                    _psSSearchData.Users = new List<PSUserList>();
                }
                return _psSSearchData;
            }
            else
            {
                var rep = _roleRepository.GetSingle(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.ROLE_NAME == "Feedback Caller").ROLE_ID;
                if (!rep.Equals(null))
                {
                    var _roleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.BigInt, Value = rep };
                    var _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };

                    _psSSearchData.Users = SpRepository<PSUserList>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSR_FEEDBACK_CALLER_NAME_LIST @PRACTICE_CODE, @ROLE_ID", _practiceCode, _roleId);
                }
                else
                {
                    _psSSearchData.Users = new List<PSUserList>();
                }
                return _psSSearchData;
            }

        }

        public PSInitialData GetPSInitialData(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
        {
            PSInitialData psInitialData = new PSInitialData();
            psInitialData.psdData = GetPSDResults(patientSurveySearchRequest, profile);
            psInitialData.psSearchDataResult = GetPSSearchData(profile.PracticeCode);
            return psInitialData;
        }

        public List<PSDStateAndRegionRecommendationWise> GetPSDStateAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
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
            var _state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var _region = new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            var _discipline = new SqlParameter { ParameterName = "DISCIPLINE", Value = patientSurveySearchRequest.DISCIPLINE };
            var _format = new SqlParameter { ParameterName = "FORMAT", Value = patientSurveySearchRequest.FORMAT };

            var result = SpRepository<PSDStateAndRegionRecommendationWise>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSD_STATE_AND_RECOMMENDATION_WISE 
                @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @STATE, @REGION, @DISCIPLINE, @FORMAT", PracticeCode, _dateFrom, _dateTo, _state, _region, _discipline, _format);
            return result;
        }

        public List<PSDStateAndRegionRecommendationWise> GetPSDRegionAndRecommendationWise(PatientSurveySearchRequest patientSurveySearchRequest, UserProfile profile)
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
            var _state = new SqlParameter { ParameterName = "STATE", Value = patientSurveySearchRequest.STATE };
            var _region = new SqlParameter { ParameterName = "REGION", Value = patientSurveySearchRequest.REGION };
            var _discipline = new SqlParameter { ParameterName = "DISCIPLINE", Value = patientSurveySearchRequest.DISCIPLINE };
            var _format = new SqlParameter { ParameterName = "FORMAT", Value = patientSurveySearchRequest.FORMAT };

            var result = SpRepository<PSDStateAndRegionRecommendationWise>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PSD_REGION_AND_RECOMMENDATION_WISE 
                @PRACTICE_CODE, @DATE_FROM, @DATE_TO, @STATE, @REGION, @DISCIPLINE, @FORMAT", PracticeCode, _dateFrom, _dateTo, _state, _region, _discipline, _format);
            return result;
        }

        public List<PatientSurveyCallLog> GetPSCallLogList(long patientAccount, long practiceCode)
        {
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
            var _surveyId = new SqlParameter { ParameterName = "SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = 0 };
            var _patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = patientAccount };

            var result = SpRepository<PatientSurveyCallLog>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PS_CALL_LOG_LIST
                         @PRACTICE_CODE, @SURVEY_ID, @PATIENT_ACCOUNT", PracticeCode, _surveyId, _patientAccount);
            return result;
        }
        public List<string> GetPSProvidersList(long practiceCode, string provider)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(provider))
            {
                var practice_code = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                result = SpRepository<string>.GetListWithStoreProcedure(@"FOX_PROC_GET_PATIENT_SURVEY_PROVIDERS @PRACTICE_CODE", practice_code);
                return result.ToList();
            }
            return result;
        }
        public List<string> GetPSStatesList(long practiceCode, string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                var practice_code = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var result = SpRepository<string>.GetListWithStoreProcedure(@"FOX_PROC_GET_PATIENT_SURVEY_STATES @PRACTICE_CODE", practice_code);
                return result.ToList();
                //return _patientSurveyRepository.GetMany(x => x.PRACTICE_CODE == practiceCode && x.PATIENT_STATE != null && !string.IsNullOrEmpty(x.PATIENT_STATE)).OrderBy(x => x.PATIENT_STATE).Select(x => x.PATIENT_STATE).Distinct().ToList();
            }
            return _patientSurveyRepository.GetMany(x => x.PRACTICE_CODE == practiceCode && x.PATIENT_STATE != null && !string.IsNullOrEmpty(x.PATIENT_STATE) && x.REGION == region).OrderBy(x => x.PATIENT_STATE).Select(x => x.PATIENT_STATE).Distinct().ToList();
        }

        public List<string> GetPSRegionsList(long practiceCode, string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                var practice_code = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var result = SpRepository<string>.GetListWithStoreProcedure(@"FOX_PROC_GET_PATIENT_SURVEY_REGIONS @PRACTICE_CODE", practice_code);
                return result.ToList();
            }
            //return _patientSurveyRepository.GetMany(x => x.PRACTICE_CODE == practiceCode && x.REGION != null && !string.IsNullOrEmpty(x.REGION)).OrderBy(x => x.REGION).Select(x => x.REGION).Distinct().ToList();
            return _patientSurveyRepository.GetMany(x => x.PRACTICE_CODE == practiceCode && x.PATIENT_STATE == state && x.REGION != null && !string.IsNullOrEmpty(x.REGION)).OrderBy(x => x.REGION).Select(x => x.REGION).Distinct().ToList();
        }

        public string GetPSFormat(long practiceCode)
        {
            var activeFormat = _psFormatTypeRepository.Get(x => !x.DELETED && x.PRACTICE_CODE == practiceCode && x.IS_ACTIVE == true);
            if (activeFormat != null)
            {
                return activeFormat.FORMAT_TYPE;
            }
            return "";
        }

        public bool UpdatePSFormat(string format, UserProfile profile)
        {
            try
            {
                var allFormat = _psFormatTypeRepository.GetMany(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                foreach (var _format in allFormat)
                {
                    if (_format != null) //set all format active
                    {
                        _format.IS_ACTIVE = false;
                        _format.MODIFIED_BY = profile.UserName;
                        _format.MODIFIED_DATE = Helper.GetCurrentDate();
                        _psFormatTypeRepository.Update(_format);
                        _psFormatTypeRepository.Save();
                    }
                }

                var activeFormat = _psFormatTypeRepository.Get(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && x.FORMAT_TYPE == format);
                if (activeFormat != null) //set this format active
                {
                    activeFormat.IS_ACTIVE = true;
                    activeFormat.MODIFIED_BY = profile.UserName;
                    activeFormat.MODIFIED_DATE = Helper.GetCurrentDate();
                    _psFormatTypeRepository.Update(activeFormat);
                    _psFormatTypeRepository.Save();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public ResponseModel CheckDeceasedPatient(string patientAccount, long practiceCode)
        {
            ResponseModel responseModel = new ResponseModel();
            var result = _patientSurveyRepository.GetMany(x => x.PATIENT_ACCOUNT_NUMBER.ToString() == patientAccount && x.DELETED != true && x.PRACTICE_CODE == practiceCode);
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    if (item.SURVEY_STATUS_CHILD != null && item.SURVEY_STATUS_CHILD.ToLower().Contains("deceased"))
                    {
                        UpdatePatientSurvey(result);
                        responseModel.Message = "Successfully Updated";
                        responseModel.Success = true;
                        responseModel.ErrorMessage = "";
                    }
                    else
                    {
                        responseModel.Message = "Not Updated";
                        responseModel.Success = false;
                        responseModel.ErrorMessage = "";
                    }
                }
            }
            else
            {
                responseModel.Message = "Response is Empty";
                responseModel.Success = false;
                responseModel.ErrorMessage = "";
            }
            return responseModel;
        }
        public void UpdatePatientSurvey(List<PatientSurvey> patientSurveys)
        {
            if (patientSurveys != null)
            {
                foreach (var item in patientSurveys)
                {
                    item.DELETED = true;
                    item.MODIFIED_DATE = DateTime.Now;
                    item.FEEDBACK = "Patient marked as deceased as of " + DateTime.Now;
                    _patientSurveyRepository.Update(item);
                    _patientSurveyRepository.Save();
                }
            }
        }
        /// <summary>
        /// This function get inbound survey call recordings
        /// </summary>
        /// <param name="patientAccountNumber"></param>
        /// <param name="profilePracticeCode"></param>
        /// <returns></returns>
        public List<PatientSurveyInBoundCallResponse> GetPatientSurveyInBoundCalls(long patientAccountNumber, long profilePracticeCode)
        {
            List<PatientSurveyInBoundCallResponse> result = new List<PatientSurveyInBoundCallResponse>();
            if (patientAccountNumber != 0 && profilePracticeCode != 0)
            {
                var patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = patientAccountNumber };
                var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profilePracticeCode };
                result = SpRepository<PatientSurveyInBoundCallResponse>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_INBOUND_CALL_DETAILS @PATIENT_ACCOUNT, @PRACTICE_CODE", patientAccount, practiceCode);
            }
            return result;
        }
    }
}