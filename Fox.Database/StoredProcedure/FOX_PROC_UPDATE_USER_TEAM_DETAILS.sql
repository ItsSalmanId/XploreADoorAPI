IF (OBJECT_ID('FOX_PROC_UPDATE_USER_TEAM_DETAILS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_USER_TEAM_DETAILS  
GO
-- =============================================         
CREATE PROCEDURE FOX_PROC_UPDATE_USER_TEAM_DETAILS                
 @USER_ID BIGINT                  
AS                 
BEGIN               
UPDATE FOX_TBL_USER_TEAMS       
SET DELETED = 1       
WHERE [USER_ID]  = @USER_ID            
END   
  