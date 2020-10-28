using System;
using FOX.DataModels.Models.CompleteQueueModel;
using System.Collections.Generic;
using FOX.DataModels.Models.Security;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.CompleteQueueService
{
    public interface ICompleteQueueService
    {
        List<CompleteQueue> GetCompleteQueue(SearchRequestCompletedQueue req,UserProfile profile);
        string ExportToExcelCompleteQueu(SearchRequestCompletedQueue req, UserProfile profile);
    }
}