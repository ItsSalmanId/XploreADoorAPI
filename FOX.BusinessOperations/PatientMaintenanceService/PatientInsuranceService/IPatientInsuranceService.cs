using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.PatientMaintenanceService.PatientInsuranceService
{
    public interface IPatientInsuranceService
    {
        int GetUnmappedInsurancesCount(UserProfile profile);
        List<FoxInsurancePayers> GetUnmappedInsurances(UnmappedInsuranceRequest unmappedInsuranceRequest, UserProfile profile);
        string ExportToExcelInsuranceSetup(UnmappedInsuranceRequest unmappedInsuranceRequest, UserProfile profile);
        MTBCInsurancesSearchData GetMTBCInsurancesSearchData();
        List<MTBCInsurances> GetMTBCInsurances(MTBCInsurancesRequest mtbcInsurancesRequest);
        FoxInsurancePayers MapUnmappedInsurance(FoxInsurancePayers foxInsurancePayors);
        List<ClaimInsuranceViewModel> GetUnpaidClaimsForInsurance(ClaimInsuranceSearchReq searchReq, UserProfile profile);
    }
}
