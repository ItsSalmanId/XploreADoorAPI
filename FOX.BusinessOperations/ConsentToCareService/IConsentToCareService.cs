using System.Collections.Generic;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FOX.BusinessOperations.ConsentToCareService
{
    public interface IConsentToCareService
    {
        #region FUNCTIONS
        FoxTblConsentToCare AddUpdateConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        ConsentToCareList GetConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        string GeneratePdfForConcentToCare(FoxTblConsentToCare consentToCareObj, string practiceDocumentDirectory);
        //FoxTblConsentToCare GetConsentToCareDocumentsInfo(FoxTblConsentToCare consentToCareObj, string practiceDocumentDirectory);
        //SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation);
        //List<SurveyQuestions> GetSurveyQuestionDetails(SurveyLink objSurveyLink);
        //ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey);
        //SurveyLink DecryptionUrl(SurveyLink objSurveyLink);
        #endregion
    }
}
