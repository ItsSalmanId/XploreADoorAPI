using FOX.DataModels.Models.CorrectedClaims;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.CorrectedClaimService
{
   public interface ICorrectedClaimService
    {
        List<CORRECTED_CLAIM_TYPE> GetClaimTypes(UserProfile profile);
        CorrectedClaimResponse InsertUpdateCorrectedClaim(CORRECTED_CLAIM obj, UserProfile profile);
        List<AdjustmentClaimStatus> GetAdjustedClaim(UserProfile profile);
        CorrectedClaimData GetCorrectedClaimData(CorrectedClaimSearch correctedClaimSearch, UserProfile profile);
        List<SmartPatientRes> GetSmartPatient(string searchText, UserProfile profile);
        string Export(CorrectedClaimSearch obj, UserProfile profile);
        List<PatientCases> GetPatientCases(string PatientAccount, UserProfile profile);
        List<CorrectedClaimLog> GetCorrectedClaimLog(long CORRECTED_CLAIM_ID, UserProfile profile);
    }
}
