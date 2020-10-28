using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.ClinicianSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.SettingsService.ClinicianSetupService
{
    public interface IClinicianSetupService
    {
        List<VisitQoutaWeek> GetVisitQoutaPerWeek(UserProfile profile);
        List<FOX_TBL_DISCIPLINE> GetDisciplines(UserProfile profile);
        GetClinicanRes InsertUpdateClinician(FoxProviderClass obj, UserProfile profile);
        List<SmartRefRegion> GetSmartRefRegion(string searchText, UserProfile Profile);
        List<FoxProviderClass> GetClinician(GetClinicanReq req, UserProfile Profile);
        string Export(GetClinicanReq obj, UserProfile profile);
        bool CheckNPI(string NPI, UserProfile profile);
        bool CheckSSN(string SSN, UserProfile profile);
        List<ProviderLocationRes> GetSpecficProviderLocation(ProviderLocationReq obj, UserProfile Profile);
        ResponseModel ReadExcel(string filePath, long practiceCode, string userName);
    }
}
