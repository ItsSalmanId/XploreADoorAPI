﻿using FOX.DataModels.Models.Security;
using System.Collections.Generic;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;

namespace FOX.BusinessOperations.ConsentToCareService
{
    public interface IConsentToCareService
    {
        #region FUNCTIONS
        FoxTblConsentToCare AddUpdateConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        ConsentToCareList GetConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        string GeneratePdfForConcentToCare(FoxTblConsentToCare consentToCareObj);
        FoxTblConsentToCare DecryptionUrl(FoxTblConsentToCare consentToCareObj);
        bool DobVerificationInvalidAttempt(AddInvalidAttemptRequest addInvalidAttemptRequestObj, UserProfile profile);
        FoxTblConsentToCare SubmitConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        ConsentToCareList GetConsentToCareImagePath(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        bool CommentOnCallTap(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        ConsentToCareResponse GetInsuranceDetails(FoxTblConsentToCare consentToCareObj, UserProfile profile);
        #endregion
    }
}
