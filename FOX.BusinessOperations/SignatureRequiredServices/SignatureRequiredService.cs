using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.SignatureRequired;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkDetails = FOX.DataModels.Models.SignatureRequired.WorkDetails;

namespace FOX.BusinessOperations.SignatureRequiredServices
{
    public class SignatureRequiredService : ISignatureRequiredService
    {
        public List<SignatureRequiredReposne> GetReferralList(SignatureRequiredRequest req, UserProfile Profile)
        {
            List<SignatureRequiredReposne> queue = new List<SignatureRequiredReposne>();
            if(req != null && Profile != null)
            {
                var parmSortBy = new SqlParameter("SORT_BY", SqlDbType.VarChar) { Value = req.SORT_BY };
                var parmRecordsPerPage = new SqlParameter("RECORD_PER_PAGE", SqlDbType.BigInt) { Value = req.RECORD_PER_PAGE };
                var parmSortOrder = new SqlParameter("SORT_ORDER", SqlDbType.VarChar) { Value = req.SORT_ORDER };
                var parmSearchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = req.SEARCH_TEXT };
                var parmCurrentPage = new SqlParameter("CURRENT_PAGE", SqlDbType.Int) { Value = req.CURRENT_PAGE };
                var parmUsertype = new SqlParameter("USER_TYPE", SqlDbType.VarChar) { Value = Profile.UserType };
                var parmUserName = new SqlParameter("USER_NAME", SqlDbType.VarChar) { Value = Profile.UserName };
                var parmIsSignRequired = new SqlParameter("IsSigned", SqlDbType.VarChar) { Value = req.IsSignatureRequired };
                var parmUserEmail = new SqlParameter("UserEmail", SqlDbType.VarChar) { Value = Profile.UserEmailAddress };
                queue = SpRepository<SignatureRequiredReposne>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ORIGINAL_QUEUE_SIGNATURE @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT, @SORT_BY, @SORT_ORDER, @USER_TYPE, @USER_NAME,@IsSigned,@UserEmail", parmCurrentPage, parmRecordsPerPage, parmSearchText, parmSortBy, parmSortOrder, parmUsertype, parmUserName, parmIsSignRequired, parmUserEmail);
                if(queue == null)
                {
                    queue = new List<SignatureRequiredReposne>();
                }
            }
            return queue;
        }

        public object GetRerralList(SignatureRequiredRequest req, UserProfile userProfile)
        {
            throw new NotImplementedException();
        }
        public List<WorkDetails> GetWorkDetailsUniqueId(ReqsignatureModel reqsignModel, UserProfile Profile)
        {
            List<WorkDetails> queue = new List<WorkDetails>();
            if (reqsignModel != null && Profile != null)
            {
                string uniqueId = string.Join("', '", reqsignModel.Unique_IdList);
                queue = SpRepository<WorkDetails>.GetListWithStoreProcedure(@"SELECT WORK_ID, UNIQUE_ID, WORK_STATUS, TOTAL_PAGES NO_OF_PAGES, AT.LAST_NAME + ', ' + AT.FIRST_NAME AS ASSIGNED_TO, CB.LAST_NAME + ', ' + CB.FIRST_NAME AS COMPLETED_BY, FILE_PATH, wq.IS_EMERGENCY_ORDER FROM FOX_TBL_WORK_QUEUE WQ LEFT JOIN FOX_TBL_APPLICATION_USER AT ON WQ.ASSIGNED_TO = AT.USER_NAME LEFT JOIN FOX_TBL_APPLICATION_USER CB ON WQ.COMPLETED_BY = CB.USER_NAME WHERE UNIQUE_ID IN ('" + uniqueId + @"') AND WQ.PRACTICE_CODE = " + Profile.PracticeCode.ToString());
                if(queue == null)
                {
                    queue = new List<WorkDetails>();
                }
            }
            return queue;
        }
    }
}
