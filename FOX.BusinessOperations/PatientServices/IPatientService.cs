using FOX.DataModels.Models.Authorization;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using System.Collections.Generic;

namespace FOX.BusinessOperations.PatientServices
{
    public interface IPatientService
    {
        Patient AddUpdatePatient(Patient patient, UserProfile profile);

        List<Patient> GetPatientList(PatientSearchRequest patientSearchRequest, UserProfile profile);
        ResponseModel ExportPatientListToExcel(PatientSearchRequest patientSearchRequest, UserProfile profile);
        List<PatientUpdateHistory> GetPatientUpdateHistory(PatientUpdateHistory patientUpdateHistory);
        List<PatientAddress> GetPatientAddress(long patientAccount);
        List<PatientAddress> GetPatientAddressesIncludingPOS(long patientAccount);
        List<PatientAddress> GetPatientAddressesToDisplay(long patientAccount);
        List<PatientInsurance> GetPatientInsurance(long patientAccount, UserProfile profile);
        List<ZipCityState> GetCityStateByZip(string zipCityState);
        List<ZipCityStateAddress> SearchCityStateAddressByAPI(string zipCityState, string address);
        List<PatientInsuranceDetail> GetInsurancePayers(SmartSearchInsuranceReq obj);
        List<string> CheckNecessaryDataForEligibility(long patientAccount);
        List<string> CheckNecessaryDataForLoadEligibility(PatientEligibilitySearchModel searchReq);

        //string CheckPatientInsuranceEligibility(long patientAccount, string userId);
        string CheckPatientInsuranceEligibility(PatientEligibilitySearchModel patientEligibilitySearchModel, string userId);

        Patient GetCurrentPatientDemographics(long patient_Account, UserProfile profile);
        List<PatientPOSLocation> AddUpdatePatientPOSLocation(PatientPOSLocation POSData, UserProfile profile);
        List<ContactTypesForDropdown> GetPatientContactTypes(long practiceCode);
        List<BestTimeToCallForDropdown> GetPatientBestTimeToCall(long practiceCode);     
        List<ContactType> GetAllPatientContactTypes(long practiceCode);
        PatientContact GetPatientContactDetails(long patient_Account);
        PatientContact SaveContact(PatientContact contact, UserProfile profile);
        bool SSNExists(SSNExist request, UserProfile profile);
        bool PatientExists(PatientExist request, UserProfile profile);
        bool PatientDemographicsExists(PatientExist request, UserProfile profile);
        List<PatientContact> GetPatientContacts(long patient_Account);
        List<PatientContact> GetPatientContactsForInsurance(long patient_Account);
        string GetPrimaryInsurance(long patient_Account);
        string GetLatestPrimaryInsurance(long patient_Account);
        PatientInsuranceEligibilityDetail GetCurrentPatientInsurances(long patient_Account, UserProfile profile);
        List<Subscriber> GetSubscribers(SubscriberSearchReq obj, UserProfile Profile);
        List<Employer> GetEmployers(EmployerSearchReq obj, UserProfile Profile);
        ResponseModel SaveInsuranceAndEligibilityDetails(PatientInsuranceEligibilityDetail patient, UserProfile profile,bool FromIndexInfo);
        bool SaveDynamicPatientResponsibilityInsurance(string patient_Account, UserProfile profile);
        ExtractEligibilityDataViewModel FetchEligibilityRecords(PatientEligibilitySearchModel patientEligibilitySearchModel, UserProfile profile);
        PatientInsuranceAuthDetails GetCurrentPatientAuthorizations(long patient_Account, UserProfile profile);
        bool SaveAuthDetails(PatientInsuranceAuthDetails details, UserProfile profile);
        List<MedicareLimitHistory> GetMedicareLimitHistory(MedicareLimitHistorySearchReq req, UserProfile profile);
        splitauthorization getsplitauthorization(long parent_id, UserProfile profile);
        ResponseModel DeletePatPos(PatientPOSLocation obj, UserProfile profile);
        List<FinancialClass> GetFinancialClassDDValues(string practiceCode);
        ReconcileDemographics GetLatestEligibilityRecords(PatientEligibilitySearchModel patientEligibilitySearchModel, UserProfile profile);
        List<AdvanceInsuranceSearch> GetInsurancePayersForAdvanceSearch(AdvanceInsuranceSearch obj);
        List<AdvancePatientSearch> GetPatientsForAdvanceSearch(AdvancePatientSearch obj, UserProfile profile);
        Patient GetPatientDetail(long patient_Account);
        List<SmartPatientRes> GetSmartPatient(string searchText, UserProfile profile);
        List<SmartPatientResForTask> GetSmartPatientForTask(string searchText, UserProfile profile);
        List<SmartModifiedBy> getSmartModifiedBy(string searchText, UserProfile profile);
        PatientInsuranceDetail GetDefaultPrimaryInsurance(DefaultInsuranceParameters defaultInsuranceParameters, long practiceCode);
        SubscriberInformation GetSubscriberInfo(SubscriberInfoRequest subscriberinforequest);
        List<FacilityLocation> GetPatientPrivateHomes(string patientAccount, string stateCode, long practiceCode);
        List<PatientInsuranceDetail> GetSmartInsurancePayers(SmartSearchInsuranceReq obj, UserProfile profile);
        FacilityLocation GetPrivateHomeFacilityByCode(string code, UserProfile profile);
        PatientInsuranceDetail GetSuggestedMCPayer(SuggestedMCPayer suggestedMCPayer, UserProfile profile);
        List<PatientInsuranceDetail> GetPatientInsurancesInIndexInfo(string patientAccountStr, UserProfile profile);
        List<PHR> GetPatientInviteStatus(string PatientAccount, UserProfile profile);
        ResponseModel SendInviteToPatient(PHR obj, UserProfile profile);
        List<Patient> GetInvitedPatient(PHR obj, UserProfile profile);

        ResponseModel BlockPatientFromPHR(PHR obj, UserProfile profile);
        ResponseModel UnBlockPatientFromPHR(PHR obj, UserProfile profile);

        ResponseModel CancelPatientRequestFromPHR(PHR obj, UserProfile profile);

        ResponseModel ResendRequestforPHRToPatient(PHR obj, UserProfile profile);
        List<WorkOrderDocs> GetWorkOrderDocs(string patientAccountStr, UserProfile userProfile);
        PatientInsuranceDetail GetAutoPopulateInsurance(AutoPopulateModel obj, UserProfile userProfile);
        List<WORK_ORDER_INFO_RES> GetWorkOrderInfo(WORK_ORDER_INFO_REQ obj, UserProfile profile);

        bool checkPatientisInterfaced(long PATIENT_ACCOUNT, UserProfile profile);

        ZipRegionIDName GetRegionByZip(string zipCode, UserProfile profile);

        //PatientDeceasedInfo GetPatientDeceaseDate(string pat_Acc, UserProfile profile);

        List<PatientCasesForDD> GetPatientCasesForDD(long patient_Account);

        ResponseModel SaveInsuranceEligibilityFromIndexInfo(PatientInsurance insuranceToCreateUpdate, UserProfile profile);
        ResponseModel PrivateHOMExists(string statecode, UserProfile profile);
        ResponseModel DeleteInsuranceInformation(PatientInsuranceDetail obj, UserProfile profile);
        List<PatientAlias> GetPatientAliasListForSpecificPatient(long patient_Account);
        ResponseModel SavePatientAlias(PatientAlias obj, UserProfile profile);
        List<CountryResponse> getCountries(SmartSearchCountriesReq obj);
        List<CountryResponse> GetAllCountries(long practiceCode);
        ResponseModel SaveEligibiltyDocument(DocumentSaveEligibility documentSaveEligibility, UserProfile profile);
        List<CheckDuplicatePatientsRes> CheckDuplicatePatients(CheckDuplicatePatientsReq documentSaveEligibility, UserProfile profile);
        FoxInsurancePayers GetInsuranc(long ID, UserProfile profile);
    }
}