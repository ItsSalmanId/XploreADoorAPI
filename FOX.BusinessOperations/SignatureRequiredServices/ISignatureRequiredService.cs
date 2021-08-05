using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.SignatureRequired;
using WorkDetails = FOX.DataModels.Models.SignatureRequired.WorkDetails;
using FOX.DataModels.Models.RequestForOrder;

namespace FOX.BusinessOperations.SignatureRequiredServices
{
    public interface ISignatureRequiredService
    {
        List<SignatureRequiredReposne> GetReferralList(SignatureRequiredRequest req, UserProfile Profile);
        List<WorkDetails> GetWorkDetailsUniqueId(ReqsignatureModel reqsignModel, UserProfile Profile);
        object GetRerralList(SignatureRequiredRequest req, UserProfile userProfile);
    }
}
