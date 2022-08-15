IF (OBJECT_ID('FOX_PROC_GET_RECONCILIATION_REASON_NAME') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECONCILIATION_REASON_NAME  
GO 
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE FOX_PROC_GET_RECONCILIATION_REASON_NAME    
 -- Add the parameters for the stored procedure here    
 @REASON_ID BIGINT NULL    
AS    
BEGIN    
 SELECT TOP 1 * FROM FOX_TBL_RECONCILIATION_REASON WHERE FOX_TBL_RECONCILIATION_REASON_ID = @REASON_ID    
END    
