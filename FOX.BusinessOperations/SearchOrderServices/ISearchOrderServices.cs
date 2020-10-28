using System.Collections.Generic;
using FOX.DataModels.Models.SearchOrderModel;
using FOX.DataModels.Models.Security;

namespace FOX.BusinessOperations.SearchOrderServices
{
    public interface ISearchOrderServices
    {
        List<SearchOrder> GetSearchOrder(SearchOrderRequest searchOrder, UserProfile profile);
        string ExportToExcelSearchOrder(SearchOrderRequest searchOrder, UserProfile profile);
    }
    
}