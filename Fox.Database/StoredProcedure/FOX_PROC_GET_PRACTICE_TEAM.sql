IF (OBJECT_ID('FOX_PROC_GET_PRACTICE_TEAM') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PRACTICE_TEAM  
GO  
Create Procedure FOX_PROC_GET_PRACTICE_TEAM          
(                             
 @PRACTICE_CODE BIGINT,              
 @USER_ID BIGINT                 
)                  
AS                  
BEGIN                  
SET NOCOUNT ON;                  
SELECT  PA.PHD_CALL_SCENARIO_ID, au.PRACTICE_CODE, SC.NAME, sc.DESCRIPTION, sc.CREATED_BY, sc.CREATED_DATE, sc.MODIFIED_BY, sc.MODIFIED_DATE, PA.DELETED           
FROM FOX_TBL_USER_TEAMS AS PA              
INNER JOIN FOX_TBL_APPLICATION_USER AS AU ON AU.USER_ID = PA.USER_ID              
INNER JOIN FOX_TBL_PHD_CALL_SCENARIO AS SC ON SC.PHD_CALL_SCENARIO_ID = PA.PHD_CALL_SCENARIO_ID              
WHERE PA.PRACTICE_CODE = @PRACTICE_CODE AND AU.USER_ID = @USER_ID AND ISNULL(PA.DELETED,0) = 0              
END   
   