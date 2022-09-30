IF (OBJECT_ID('FOX_PROC_UPDATE_WORK_QUEUE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_WORK_QUEUE  
GO   
----------------------------------------- SP_HELPTEXT FOX_PROC_UPDATE_WORK_QUEUE -----------------------------------  
-- =============================================          
-- Author:  <Muhammad Imran>          
-- Create date: <Create Date,,>          
-- Description: <Description,,>          
-- =============================================     
--EXEC [DBO].[FOX_PROC_UPDATE_WORK_QUEUE] 99911762, '655TESTING', 'Junaid Zahid', 'Index Pending', 'false', 'false', 'false', 'true'  
CREATE PROCEDURE FOX_PROC_UPDATE_WORK_QUEUE      
 -- Add the parameters for the stored procedure here          
 @WORK_ID BIGINT NULL,          
 @USER_NAME VARCHAR(70) NULL,          
 @ASSIGNED_TO VARCHAR(70) NULL,          
 @WORK_STATUS VARCHAR(50) NULL,          
 @UPDATE_WORK_STATUS BIT NULL,          
 @UPDATE_INDEX_DATE BIT NULL,          
 @UPDATE_AGENT_DATE BIT NULL,        
 @SUPERVISOR_STATUS BIT NULL          
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
 IF EXISTS (SELECT * FROM FOX_TBL_WORK_QUEUE WHERE WORK_ID = @WORK_ID)          
 BEGIN          
  UPDATE FOX_TBL_WORK_QUEUE           
   SET          
    MODIFIED_DATE = GETDATE(),          
    MODIFIED_BY = @USER_NAME,          
    ASSIGNED_TO = @ASSIGNED_TO,          
    ASSIGNED_BY = @USER_NAME,          
    ASSIGNED_DATE = GETDATE(),        
 supervisor_status = @SUPERVISOR_STATUS         
    --INDEXER_ASSIGN_DATE = CASE WHEN ISNULL(@UPDATE_INDEX_DATE,0) = CONVERT(BIT,1) THEN GETDATE() END,          
    --AGENT_ASSIGN_DATE = CASE WHEN ISNULL(@UPDATE_AGENT_DATE,0) = CONVERT(BIT,1) THEN GETDATE() END,          
    --WORK_STATUS = CASE WHEN ISNULL(@UPDATE_WORK_STATUS,0) = CONVERT(BIT,1) THEN @WORK_STATUS END,          
    --INDEXER_ASSIGN_DATE = CASE WHEN ISNULL(@UPDATE_WORK_STATUS,0) = CONVERT(BIT,1) THEN GETDATE() END          
   WHERE WORK_ID = @WORK_ID          
          
   IF (@UPDATE_INDEX_DATE = 1)          
   BEGIN          
    UPDATE FOX_TBL_WORK_QUEUE           
    SET INDEXER_ASSIGN_DATE = GETDATE()          
    WHERE WORK_ID = @WORK_ID          
   END          
          
   IF (@UPDATE_AGENT_DATE = 1)          
   BEGIN          
    UPDATE FOX_TBL_WORK_QUEUE           
    SET AGENT_ASSIGN_DATE = GETDATE()          
    WHERE WORK_ID = @WORK_ID          
   END          
          
   IF (@UPDATE_WORK_STATUS = 1)          
   BEGIN          
    UPDATE FOX_TBL_WORK_QUEUE           
    SET WORK_STATUS = @WORK_STATUS,          
     INDEXER_ASSIGN_DATE = GETDATE()          
    WHERE WORK_ID = @WORK_ID          
   END          
 END               
END 