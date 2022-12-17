﻿using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientSurvey;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public class SurveyAutomationService : ISurveyAutomationService
    {
        private readonly DBContextSurveyAutomation _surveyAutomationContext = new DBContextSurveyAutomation();
        private readonly GenericRepository<PatientSurveyHistory> _patientSurveyHistoryRepository;
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        public SurveyAutomationService()
        {
            _patientSurveyHistoryRepository = new GenericRepository<PatientSurveyHistory>(_surveyAutomationContext);
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_surveyAutomationContext);
        }

        public SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation)
        {
            SurveyAutomation surveyAutomation = new SurveyAutomation();
            if (objSurveyAutomation != null)
            {
                SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = objSurveyAutomation.PATIENT_ACCOUNT_STR };
                surveyAutomation = SpRepository<SurveyAutomation>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT", patientAccount);
            }
            return surveyAutomation;
        }
        public List<SurveyQuestions> GetSurveyQuestionDetails(string patinetAccount)
        {
            List<SurveyQuestions> surveyQuestionsList = new List<SurveyQuestions>();
            if (patinetAccount != null)
            {
                SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.VarChar, Value = patinetAccount };
                surveyQuestionsList = SpRepository<SurveyQuestions>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PATIENT_SURVEY_QUESTION  @PATIENT_ACCOUNT", patientAccount);
            }
            return surveyQuestionsList;
        }
        public ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null && objPatientSurvey.PATIENT_ACCOUNT_NUMBER != null)
                {
                    var existingDetailInfo = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.DELETED == false);
                    objPatientSurvey.SURVEY_ID = existingDetailInfo.SURVEY_ID;
                    AddPatientSurvey(objPatientSurvey);
                    PatientSurvey patientSurvey = new PatientSurvey();
                    if (existingDetailInfo != null)
                    {
                        existingDetailInfo.IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ;
                        existingDetailInfo.IS_REFERABLE = objPatientSurvey.IS_REFERABLE;
                        existingDetailInfo.IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION;
                        existingDetailInfo.FEEDBACK = objPatientSurvey.FEEDBACK;
                        existingDetailInfo.SURVEY_STATUS_BASE = "Completed";
                        existingDetailInfo.SURVEY_STATUS_CHILD = "Completed Survey";
                        existingDetailInfo.MODIFIED_BY = "FOX-TEAM";
                        existingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                        existingDetailInfo.DELETED = false;
                        _patientSurveyRepository.Update(existingDetailInfo);
                        _patientSurveyRepository.Save();
                        response.ErrorMessage = "";
                        response.Message = "Suvery completed successfully";
                        response.Success = true;
                    }
                    else
                    {
                        response.ErrorMessage = "";
                        response.Message = "Suvery not completed successfully";
                        response.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        private void AddPatientSurvey(PatientSurvey objPatientSurvey)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null)
                {
                    PatientSurveyHistory patientSurveyHistory = new PatientSurveyHistory();
                    patientSurveyHistory.SURVEY_HISTORY_ID = Helper.getMaximumId("SURVEY_HISTORY_ID");
                    patientSurveyHistory.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                    patientSurveyHistory.PATIENT_ACCOUNT = objPatientSurvey.PATIENT_ACCOUNT_NUMBER;
                    patientSurveyHistory.PRACTICE_CODE = 1011163;
                    patientSurveyHistory.IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ;
                    patientSurveyHistory.IS_REFERABLE = objPatientSurvey.IS_REFERABLE;
                    patientSurveyHistory.IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION;
                    patientSurveyHistory.FEEDBACK = objPatientSurvey.FEEDBACK;
                    patientSurveyHistory.SURVEY_STATUS_BASE = "Completed";
                    patientSurveyHistory.SURVEY_STATUS_CHILD = "Completed Survey";
                    patientSurveyHistory.DELETED = false;
                    patientSurveyHistory.CREATED_BY = "FOX-TEAM";
                    patientSurveyHistory.CREATED_DATE = Helper.GetCurrentDate();
                    _patientSurveyHistoryRepository.Insert(patientSurveyHistory);
                    _patientSurveyHistoryRepository.Save();
                    response.Message = "Suvery completed successfully";
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = "Suvery not completed successfully";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
