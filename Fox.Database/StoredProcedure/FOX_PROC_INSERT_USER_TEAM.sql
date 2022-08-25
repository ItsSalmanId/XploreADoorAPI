IF (OBJECT_ID('FOX_PROC_INSERT_USER_TEAM') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_USER_TEAM  
GO
Create PROCEDURE FOX_PROC_INSERT_USER_TEAM  --5484374,5483053,544110,1011163,0,0            
@USER_TEAM_ID BIGINT,               
@USER_ID BIGINT,               
@PHD_CALL_SCENARIO_ID BIGINT,               
@PRACTICE_CODE BIGINT,               
@COUNTER BIGINT = 0,               
@FILTER VARCHAR(20)  AS               
BEGIN             
           
SET               
  NOCOUNT ON;              
SELECT               
  @COUNTER =  COUNT(*)               
FROM               
  FOX_TBL_USER_TEAMS               
WHERE               
  PHD_CALL_SCENARIO_ID = @PHD_CALL_SCENARIO_ID and ISNULL(deleted,0) = 0              
  AND USER_ID = @USER_ID             
      
  IF @FILTER = 0              
  BEGIN              
  IF @COUNTER = 0               
    BEGIN               
          INSERT INTO FOX_TBL_USER_TEAMS (USER_TEAM_ID, [USER_ID], PHD_CALL_SCENARIO_ID, PRACTICE_CODE, CREATED_DATE, CREATED_BY, MODIFIED_DATE, MODIFIED_BY, DELETED)               
          VALUES (@USER_TEAM_ID, @USER_ID, @PHD_CALL_SCENARIO_ID, @PRACTICE_CODE, GETDATE(), 'FOX_TEAM', GETDATE(), 'FOX_TEAM', 0)               
    END              
  END              
  ELSE IF @FILTER = 1              
  BEGIN              
  IF @COUNTER > 0               
    BEGIN               
          update FOX_TBL_USER_TEAMS set deleted=1 where USER_ID=@USER_ID and PHD_CALL_SCENARIO_ID=@PHD_CALL_SCENARIO_ID               
    END              
  END                
                
END   
  