-- =============================================                        
-- CREATED BY :  Muhammad Salman                        
-- CRFEATED date: 01/31/2022                        
-- =============================================                                                                                                                                                
CREATE PROCEDURE [DBO].[FOX_PROC_GET_AUTOMATED_PERFORM_SURVEY_DETAILS]    
 @SURVEY_ID   BIGINT ,    
 @PRACTICE_CODE   BIGINT                                                                                                                                                                                                                                       
                            
 AS      
 BEGIN      
          
select PATIENT_ACCOUNT from FOX_TBL_PATIENT_SURVEY PS    
Right join FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) on SL.SURVEY_ID = PS.SURVEY_ID     
where IS_SURVEYED = 1    
And ISNULL(PS.Deleted, 0) = 0    
And ISNULL(SL.Deleted, 0) = 0    
And (IS_SMS = 1    
   OR     
   IS_EMAIL = 1)    
   AND    
   PS.SURVEY_ID = @SURVEY_ID    
   AND PS.PRACTICE_CODE = @PRACTICE_CODE    
   AND SL.PRACTICE_CODE = @PRACTICE_CODE    
END