-- =============================================                        
-- Created By :  Muhammad Salman                        
-- Created date: 02/03/2023   
-- Description: This SP trigger to get existing survey details                       
-- =============================================          
CREATE PROCEDURE [DBO].[FOX_PROC_EXISTING_SURVEY_DETAIL]       
 (@PRACTICE_CODE   BIGINT,                                                                                                                                                                       
  @SURVEY_ID   BIGINT                                                                                       
 )                                                                                                          
 AS                                                                                                                  
   BEGIN        
Select * FROm FOX_TBL_PATIENT_SURVEY PS WITH (NOLOCK)     
left JOIN FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) ON PS.SURVEY_ID = SL.SURVEY_ID       
where SL.SURVEY_ID = @SURVEY_ID        
AND      
SL.SURVEY_ID = @SURVEY_ID       
AND ISNULL(SL.DELETED, 0) = 0         
AND ISNULL(PS.DELETED, 0) = 0         
AND IS_SURVEYED = 1      
and PS.PRACTICE_CODE = @PRACTICE_CODE      
end