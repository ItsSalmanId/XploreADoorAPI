using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.GeneralNotesService
{
    public interface IGeneralNotesServices
    {
        List<FOX_TBL_GENERAL_NOTE> GetGeneralNotes(UserProfile profile, GeneralNotesSearchRequest request);
        List<NoteAlert> GetAlertGeneralNotes(UserProfile profile, string patientAccount);
        GeneralNoteResponseModel GetSingleGeneralNote(UserProfile profile, GeneralNoteRequestModel request);
        FOX_TBL_GENERAL_NOTE GetSingleNoteForUpdate(UserProfile profile, GeneralNoteRequestModel request);
        ResponseModel DeleteGeneralNote(UserProfile profile, GeneralNoteDeleteRequestModel request);
        ResponseModel CreateUpdateNote(GeneralNoteCreateUpdateRequestModel note, UserProfile profile);
        ResponseModel UpdateNote(GeneralNoteCreateUpdateRequestModel note, UserProfile profile);
        GeneralNoteHistoryResponseModel GetGeneralNoteHistory(GeneralNoteHistoryRequestModel request, UserProfile profile);
        List<AlertType> GetAlertTypes(long practiceCode);
        List<AlertType> GetAlertTypeswithInactive(long practiceCode);
        List<PatientCasesForDD> GetPatientCasesList(long patient_Account, UserProfile Profile);
        List<NoteAlert> GetNoteAlert(AlertSearchRequest request, long practiceCode);
        ResponseModel CreateUpdateNoteAlert(NoteAlert request, UserProfile Profile);
        ResponseModel DeleteAlert(long alertId, UserProfile userProfile);
        List<GeneralNotesInterfaceLog> GetGeneralNotesInterfaceLogs(InterfaceLogSearchRequest request, UserProfile profile);
        bool RetryInterfacing(InterfaceLogSearchRequest request, UserProfile profile);
        string ExportToExcelInterfaceLogs(InterfaceLogSearchRequest request, UserProfile profile);
        bool checkPatientisInterfaced(string PATIENT_ACCOUNT, UserProfile profile);
    }
}
