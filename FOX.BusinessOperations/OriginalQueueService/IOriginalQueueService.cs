using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.OriginalQueueService
{
    public interface IOriginalQueueService
    {
        ResOriginalQueueModel GetOriginalQueue(OriginalQueueRequest req, UserProfile Profile);
        string ExportToExcelOrignalQueue(OriginalQueueRequest req, UserProfile profile);
        long SaveQueueData(OriginalQueue req, UserProfile Profile);
        List<WorkDetails> GetWorkDetails(long req, long practiceCode);
        //List<WorkDetails> GetWorkDetailsUniqueId(string uniqueId, UserProfile Profile);
        List<WorkDetails> GetWorkDetailsUniqueId(ReqOriginalQueueModel reqOriginalQueueModel, UserProfile Profile);
        //string ExportOriginalQueue(OriginalQueueRequest obj, UserProfile userProfile);
    }
}
