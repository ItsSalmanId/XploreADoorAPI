IF (OBJECT_ID('FOX_PROC_GET_PATIENT_INFO_BY_WORKID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_INFO_BY_WORKID  
GO  
-- =============================================            
-- Author:  <Abdul Sattar>            
-- Create date: <04/01/2021>            
-- Description: <Get patient information by work ID>            
-- =============================================            
--FOX_PROC_GET_PATIENT_INFO_BY_WORKID  552103          
CREATE PROCEDURE FOX_PROC_GET_PATIENT_INFO_BY_WORKID        
 @WORK_ID BIGINT        
AS            
BEGIN            
 -- SET NOCOUNT ON added to prevent extra result sets from            
 -- interfering with SELECT statements.            
 SET NOCOUNT ON;            
 SELECT TOP 1 * FROM FOX_TBL_WORK_QUEUE             
 WHERE WORK_ID = @WORK_ID         
 AND ISNULL(DELETED,0) = 0            
END   
