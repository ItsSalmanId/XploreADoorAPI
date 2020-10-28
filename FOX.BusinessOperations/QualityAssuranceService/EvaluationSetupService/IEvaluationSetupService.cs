using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.QualityAssuranceService.EvaluationSetupService
{
    public interface IEvaluationSetupService
    {
        EvaluationSetupResponseModel AllEvaluationCriteria(RequestModelForCallType obj, long practiceCode);
        List<EvaluationCriteria> onSaveOverallWeightageCriteria(RequestModelOverallWeightage obj, UserProfile profile);
        List<EvaluationCriteriaCategories> onSaveClientExperience(RequestModelClientExperience obj, UserProfile profile);
        List<EvaluationCriteriaCategories> onSaveSystemProductprocess(RequestModelSystemProcess obj, UserProfile profile);
        WowFactor onSaveWowFactor(RequestModelWowfactor obj, UserProfile profile);
        List<GradingSetup> onSaveGradingSetup(RequestModelGradingSetup obj, UserProfile profile);
    }
}
