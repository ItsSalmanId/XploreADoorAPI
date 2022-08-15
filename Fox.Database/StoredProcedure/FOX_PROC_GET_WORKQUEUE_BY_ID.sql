IF (OBJECT_ID('FOX_PROC_GET_WORKQUEUE_BY_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_WORKQUEUE_BY_ID  
GO     
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <Create Date,,>        
-- Description: <Description,,>        
-- =============================================        
CREATE PROCEDURE FOX_PROC_GET_WORKQUEUE_BY_ID        
 -- Add the parameters for the stored procedure here        
 @WORK_ID BIGINT        
AS        
BEGIN        
      
 SET NOCOUNT ON;        
        
    -- Insert statements for procedure here        
 SELECT * FROM FOX_TBL_WORK_QUEUE WHERE WORK_ID = @WORK_ID AND ISNULL(DELETED,0) = 0      
END   
