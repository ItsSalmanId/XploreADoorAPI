using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.StatesModel;
using FOX.DataModels.Models.WorkOrderHistoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.CommonServices
{
    public interface ICommonServices
    {
        #region Download Images
        string GeneratePdf(long WorkQueue, string practiceDocumentDirectory);//Old
        string GeneratePdfForSplitImages(string unique_Id, string practiceDocumentDirectory);
        string GeneratePdfForAllOriginalImages(string unique_Id, string practiceDocumentDirectory);
        #endregion
        // string GeneratePdfIndexFileInfo(long WorkId, long pat_docID, UserProfile profile);
        #region View Images
        List<CommonFilePath> GetFiles(string uniqueId, long practiceCode);
        List<CommonFilePath> GetAllOriginalFiles(string uniqueId, long practiceCode);
        #endregion
        List<WorkOrderHistory> GetWorkHistory(string uniqueid);
        bool Authenticate(string password, UserProfile profile);
        ResponseGetSenderTypesModel GetSenderTypes(long practiceCode);
        ResponseGetSenderNamesModel GetSenderNames(ReqGetSenderNamesModel model, UserProfile profile = null);
        List<ZipCityState> GetCityStateByZip(string zipCityState);
        SmartLocationRes GetTreatmentLocationByZip(string zip, long practiceCode);
        List<CityStateModel> GetSmartCities(string city);
        List<CityStateModel> GetSmartStates(string stateCode);
        List<States> GetStates();
        Provider GetProvider(long providerId, UserProfile profile);
    }
}
