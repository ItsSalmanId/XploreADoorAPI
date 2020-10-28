using FOX.DataModels.Models.Authorization;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.TasksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.SettingsService.FacilityLocationService
{
    public interface IFacilityLocationService
    {
        FacilityLocation AddUpdateFacilityLocation(FacilityLocation facilityLocation, UserProfile profile);
        List<FacilityLocation> GetFacilityLocationList(FacilityLocationSearch facilityLocationSearch, UserProfile profile);
        string ExportToExcelFacilityCreation(FacilityLocationSearch facilityLocationSearch, UserProfile profile);
        List<ProvidersName> GetProviderNamesList(string searchText, long practiceCode);
        string GetProviderCode(string state, long practiceCode);
        FacilityLocation GetFacilityLocationById(LocationPatientAccount location, string practiceCode);
        string Export(FacilityLocationSearch facilityLocationSearch, UserProfile profile);
        FacilityType GetFacilityTypeById(long facilityTypeId, long practiceCode);
        List<ReferralRegion> GetSmartReferralRegions(string searchText, long practiceCode);
        List<FOX_TBL_GROUP_IDENTIFIER> GetGroupIdentifierList(GroupIdentifierSearch groupIdentifierSearch, long practiceCode);
        string ExportToExcelGetGroupIdentifier(GroupIdentifierSearch groupIdentifierSearch, UserProfile profile);
        List<FOX_TBL_LOCATION_CORPORATION> LocationCorporationList(LocationCorporationSearch locationCorporationSearch, long practiceCode);
        string ExportToExcelLocationCorporation(LocationCorporationSearch locationCorporationSearch, UserProfile profile);
        List<DataModels.Models.Settings.FacilityLocation.FOX_TBL_IDENTIFIER> GetIdentifierList(IdentifierSearch identifierSearch, long practiceCode);
        string ExportToExcelIdentifier(IdentifierSearch identifierSearch, UserProfile profile);
        List<FOX_TBL_AUTH_STATUS> GetAuthStatusList(AuthStatusSearch authStatusSearch, long practiceCode);
        string ExportToExcelAuthStatus(AuthStatusSearch authStatusSearch, UserProfile profile);
        List<FOX_TBL_TASK_TYPE> GetTaskTypeList(TaskTpyeSearch taskTpyeSearch, long practiceCode);
        string ExportToExcelTaskType(TaskTpyeSearch taskTpyeSearch, UserProfile profile);
        List<FOX_TBL_ORDER_STATUS> GetOrderStatusList(OrderStatusSearch orderStatusSearch, long practiceCode);
        string ExportToExcelOrderStatus(OrderStatusSearch orderStatusSearch, UserProfile profile);
        List<FOX_TBL_SOURCE_OF_REFERRAL> GetSourceofReferralList(SourceOfreferralSearch sourceOfreferralSearch, long practiceCode);
        string ExportToExcelSourceOfReferral(SourceOfreferralSearch sourceOfreferralSearch, UserProfile profile);
        List<AlertType> GetAlertTypeList(AlertTypeSearch alertTypeSearch, long practiceCode);
        string ExportToExcelAlertType(AlertTypeSearch alertTypeSearch, UserProfile profile);
        List<FoxDocumentType> GetDocumentTypeList(DocumentTypeSearch documentTypeSearch);
        string ExportToExcelDocumentType(DocumentTypeSearch documentTypeSearch, UserProfile profile);
        List<ContactType> GetPatientContactTypeList(PatientContactTypeSearch patientContactTypeSearch, long practiceCode);
        string ExportToExcelContactType(PatientContactTypeSearch patientContactTypeSearch, UserProfile profile);
        List<IdentifierType> GetIdentifierTypes(long practiceCode);

    }
}