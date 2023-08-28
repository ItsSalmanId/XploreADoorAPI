using FOX.DataModels.Models.Security;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;

namespace FOX.BusinessOperations.ConsentToCareService
{
    public interface IConsentToCareService
    {
        #region FUNCTIONS
        FoxTblConsentToCare AddUpdateConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        ConsentToCareList GetConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        string GeneratePdfForConcentToCare(FoxTblConsentToCare consentToCareObj, string practiceDocumentDirectory);
        FoxTblConsentToCare DecryptionUrl(FoxTblConsentToCare consentToCareObj);
        bool DobVerificationInvalidAttempt(AddInvalidAttemptRequest addInvalidAttemptRequestObj, UserProfile profile);
        FoxTblConsentToCare SubmitConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        ConsentToCareList GetConsentToCareImagePath(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        #endregion
    }
}
