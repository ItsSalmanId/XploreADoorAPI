
using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.QualityAssuranceService.EvaluationSetupService
{
    public class EvaluationSetupService : IEvaluationSetupService
    {
        private readonly DBContextQualityAssurance _qualityAssuranceContext = new DBContextQualityAssurance();
        private readonly GenericRepository<EvaluationCriteria> _evaluationCriteriaRepository;
        private readonly GenericRepository<EvaluationCriteriaCategories> _evaluationCriteriaCategoriesRepository;
        private readonly GenericRepository<WowFactor> _wowFactorRepository;
        private readonly GenericRepository<GradingSetup> _gradingSetupRepository;

        public EvaluationSetupService()
        {
            _evaluationCriteriaRepository = new GenericRepository<EvaluationCriteria>(_qualityAssuranceContext);
            _evaluationCriteriaCategoriesRepository = new GenericRepository<EvaluationCriteriaCategories>(_qualityAssuranceContext);
            _wowFactorRepository = new GenericRepository<WowFactor>(_qualityAssuranceContext);
            _gradingSetupRepository = new GenericRepository<GradingSetup>(_qualityAssuranceContext);
        }

        public EvaluationSetupResponseModel AllEvaluationCriteria(RequestModelForCallType obj, long practiceCode)
        {
            EvaluationSetupResponseModel response = new EvaluationSetupResponseModel();
            response.EvaluationCriteria = _evaluationCriteriaRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED && t.CALL_TYPE== obj.Call_Type ).ToList();
            response.EvaluationCriteriaCategories = _evaluationCriteriaCategoriesRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED && t.CALL_TYPE == obj.Call_Type).ToList();
            response.WowFactor = _wowFactorRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED && t.CALL_TYPE == obj.Call_Type).ToList();
            response.GradingSetup = _gradingSetupRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && !t.DELETED && t.CALL_TYPE == obj.Call_Type).ToList();

            return response;

        }

        public List<EvaluationCriteria> onSaveOverallWeightageCriteria(RequestModelOverallWeightage obj, UserProfile profile)
        {

            var tempObj = _evaluationCriteriaRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CALL_TYPE == obj.CALL_TYPE).ToList();
            if (tempObj != null)
            {
                tempObj[0].PERCENTAGE = obj.Client_Experience;
                tempObj[0].MODIFIED_BY = profile.UserName;
                tempObj[0].MODIFIED_DATE = Helper.GetCurrentDate();

                tempObj[1].PERCENTAGE = obj.System_Process;
                tempObj[1].MODIFIED_BY = profile.UserName;
                tempObj[1].MODIFIED_DATE = Helper.GetCurrentDate();

                foreach (var i in tempObj)
                {
                    _evaluationCriteriaRepository.Update(i);
                }
                _evaluationCriteriaRepository.Save();
                return _evaluationCriteriaRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CALL_TYPE == obj.CALL_TYPE).ToList();
            }
            else
            {
                return new List<EvaluationCriteria>();
            }

        }
        public List<EvaluationCriteriaCategories> onSaveClientExperience(RequestModelClientExperience obj, UserProfile profile)
        {
            var tempObj = new List<EvaluationCriteriaCategories>();
            var clientID = _evaluationCriteriaRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CRITERIA_NAME == "Client Experience" && t.CALL_TYPE == obj.CALL_TYPE);
            if (clientID != null && clientID.EVALUATION_CRITERIA_ID != null)
            {
                 tempObj = _evaluationCriteriaCategoriesRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.EVALUATION_CRITERIA_ID == clientID.EVALUATION_CRITERIA_ID && t.CALL_TYPE == obj.CALL_TYPE).ToList();

            }

            if (tempObj.Count > 0)
            {
                tempObj[0].CATEGORIES_POINTS = obj.GREETINGS;
                tempObj[0].MODIFIED_BY = profile.UserName;
                tempObj[0].MODIFIED_DATE = Helper.GetCurrentDate();

                tempObj[1].CATEGORIES_POINTS = obj.CALL_HANDLING_SKILLS;
                tempObj[1].MODIFIED_BY = profile.UserName;
                tempObj[1].MODIFIED_DATE = Helper.GetCurrentDate();

                tempObj[2].CATEGORIES_POINTS = obj.GRAMMER_PRONOUNCIATION_VOCABULARY;
                tempObj[2].MODIFIED_BY = profile.UserName;
                tempObj[2].MODIFIED_DATE = Helper.GetCurrentDate();

                foreach (var i in tempObj)
                {
                    _evaluationCriteriaCategoriesRepository.Update(i);
                }
                _evaluationCriteriaCategoriesRepository.Save();
                return _evaluationCriteriaCategoriesRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.EVALUATION_CRITERIA_ID == clientID.EVALUATION_CRITERIA_ID && t.CALL_TYPE == obj.CALL_TYPE).ToList();
            }
            else
            {
                return new List<EvaluationCriteriaCategories>();
            }

        }

        public List<EvaluationCriteriaCategories> onSaveSystemProductprocess(RequestModelSystemProcess obj, UserProfile profile)
        {
            var tempObj = new List<EvaluationCriteriaCategories>();
            var clientID = _evaluationCriteriaRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CRITERIA_NAME == "System Product and Process" && t.CALL_TYPE == obj.CALL_TYPE);
            if (clientID != null && clientID.EVALUATION_CRITERIA_ID != null)
            {
                tempObj = _evaluationCriteriaCategoriesRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.EVALUATION_CRITERIA_ID == clientID.EVALUATION_CRITERIA_ID && t.CALL_TYPE == obj.CALL_TYPE).ToList();

            }
            if (tempObj.Count > 0)
            {
                if (obj.CALL_TYPE.ToLower() == "phd")
                {
                    tempObj[0].CATEGORIES_POINTS = obj.VERIFIED_PATIENT_ACCOUNT;
                    tempObj[0].MODIFIED_BY = profile.UserName;
                    tempObj[0].MODIFIED_DATE = Helper.GetCurrentDate();

                    tempObj[1].CATEGORIES_POINTS = obj.PRODUCT_KNOWLEDGE;
                    tempObj[1].MODIFIED_BY = profile.UserName;
                    tempObj[1].MODIFIED_DATE = Helper.GetCurrentDate();

                    tempObj[2].CATEGORIES_POINTS = obj.CORRECT_NOTES_AND_TRACKING;
                    tempObj[2].MODIFIED_BY = profile.UserName;
                    tempObj[2].MODIFIED_DATE = Helper.GetCurrentDate();


                    tempObj[3].CATEGORIES_POINTS = obj.EMAIL_TO_STAKE_HOLDERS;
                    tempObj[3].MODIFIED_BY = profile.UserName;
                    tempObj[3].MODIFIED_DATE = Helper.GetCurrentDate();
                }
                else
                {
                    tempObj[0].CATEGORIES_POINTS = obj.VERIFIED_PATIENT_ACCOUNT;
                    tempObj[0].MODIFIED_BY = profile.UserName;
                    tempObj[0].MODIFIED_DATE = Helper.GetCurrentDate();

                    tempObj[1].CATEGORIES_POINTS = obj.ACCOUNT_AND_SCRIPT_READINESS;
                    tempObj[1].MODIFIED_BY = profile.UserName;
                    tempObj[1].MODIFIED_DATE = Helper.GetCurrentDate();

                    tempObj[2].CATEGORIES_POINTS = obj.CASE_HANDLING;
                    tempObj[2].MODIFIED_BY = profile.UserName;
                    tempObj[2].MODIFIED_DATE = Helper.GetCurrentDate();

                    tempObj[3].CATEGORIES_POINTS = obj.EMAIL_TO_STAKE_HOLDERS;
                    tempObj[3].MODIFIED_BY = profile.UserName;
                    tempObj[3].MODIFIED_DATE = Helper.GetCurrentDate();

                    tempObj[4].CATEGORIES_POINTS = obj.ESCALATION;
                    tempObj[4].MODIFIED_BY = profile.UserName;
                    tempObj[4].MODIFIED_DATE = Helper.GetCurrentDate();
                }
               
                //tempObj[5].CATEGORIES_POINTS = obj.ESCALATION;
                //tempObj[5].MODIFIED_BY = profile.UserName;
                //tempObj[5].MODIFIED_DATE = Helper.GetCurrentDate();

                foreach (var i in tempObj)
                {
                    _evaluationCriteriaCategoriesRepository.Update(i);
                }
                _evaluationCriteriaCategoriesRepository.Save();
                return _evaluationCriteriaCategoriesRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.EVALUATION_CRITERIA_ID == clientID.EVALUATION_CRITERIA_ID && t.CALL_TYPE == obj.CALL_TYPE).ToList();
            }
            else
            {
                return new List<EvaluationCriteriaCategories>();
            }

        }

        public WowFactor onSaveWowFactor(RequestModelWowfactor obj, UserProfile profile)
        {
            var tempObj = _wowFactorRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CALL_TYPE == obj.CALL_TYPE);
            if (tempObj != null)
            {
                tempObj.STATUS = obj.STATUS;
                tempObj.BONUS_POINTS = obj.BONUS_POINTS;
                tempObj.PERFORMANCE_KILLER = obj.PERFORMANCE_KILLER;
                tempObj.MODIFIED_BY = profile.UserName;
                tempObj.MODIFIED_DATE = Helper.GetCurrentDate();

                _wowFactorRepository.Update(tempObj);
                _wowFactorRepository.Save();
                return _wowFactorRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CALL_TYPE == obj.CALL_TYPE);
            }
            else
            {
                return new WowFactor();
            }

        }
        public List<GradingSetup> onSaveGradingSetup(RequestModelGradingSetup obj, UserProfile profile)
        {
            var tempObj = _gradingSetupRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CALL_TYPE == obj.CALL_TYPE).ToList();
            if (tempObj != null)
            {
                tempObj[0].OVERALL_MIN = obj.A_MIN;
                tempObj[0].MODIFIED_BY = profile.UserName;
                tempObj[0].MODIFIED_DATE = Helper.GetCurrentDate();

                tempObj[1].OVERALL_MAX = obj.B_MAX;
                tempObj[1].OVERALL_MIN = obj.B_MIN;
                tempObj[1].MODIFIED_BY = profile.UserName;
                tempObj[1].MODIFIED_DATE = Helper.GetCurrentDate();

                tempObj[2].OVERALL_MAX = obj.U_MAX;
                tempObj[2].MODIFIED_BY = profile.UserName;
                tempObj[2].MODIFIED_DATE = Helper.GetCurrentDate();

                foreach (var i in tempObj)
                {
                    _gradingSetupRepository.Update(i);
                }
                _gradingSetupRepository.Save();
                return _gradingSetupRepository.GetMany(t => !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode && t.CALL_TYPE == obj.CALL_TYPE).ToList();
            }
            else
            {
                return new List<GradingSetup>();
            }

        }
    }
}

