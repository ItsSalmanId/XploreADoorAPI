using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.RequestForOrder.IndexInformation;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.RequestForOrder.IndexInformationServices
{
    public interface IIndexInformationService
    {
        FacilityReferralSource getFacilityReferralSource(long patientAccount, long practiceCode);
        List<FacilityLocation> GetFacilityLocations(string searchText, long practiceCode);
        FacilityLocation GetFacilityByPatientPOS(string patientAccount, long practiceCode);
        ZipRegionIDName GetRegionByZip(string zipCode, UserProfile profile);
    }
}
