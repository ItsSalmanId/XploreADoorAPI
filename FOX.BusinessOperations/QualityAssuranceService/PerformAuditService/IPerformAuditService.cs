using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.QualityAssuranceService.PerformAuditService
{
    public interface IPerformAuditService
    {
        TotalNumbers GetTotalNumbersOfCriteria(long practiceCode, RequestModelForCallType request);
        List<FeedBackCaller> GetListOfReps(long practiceCode);
        List<CallLogModel> PostCallList(RequestCallList request, UserProfile profile);
        bool InsertAuditScores(SurveyAuditScores req, UserProfile profile);
        List<SurveyAuditScores> ListAuditedCalls(RequestCallFromQA agentName, UserProfile profile);
    }
}
