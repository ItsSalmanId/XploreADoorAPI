IF (OBJECT_ID('FOX_PROC_GET_UPDATE_AUTO_RECONCILIATION_CP') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_UPDATE_AUTO_RECONCILIATION_CP  
GO 
-- =============================================                                  
-- Author:  <Muhammad Arqam>                                  
-- Create date: <09/07/2020>                                  
-- Description: <Updated on AutoReconciled>                                  
-- =============================================                                 
 -- FOX_PROC_GET_UPDATE_AUTO_RECONCILIATION_CP  1011163,'1163TESTING',5488718                            
CREATE PROCEDURE [dbo].[FOX_PROC_GET_UPDATE_AUTO_RECONCILIATION_CP]                              
(@PRACTICE_CODE        BIGINT,                          
 @USER_NAME      VARCHAR(70),                           
 @RECONCILIATION_CP_ID BIGINT,                
 @AMOUNT_POSTED  MONEY,  
 @REMARKS VARCHAR(300)    
)                                
AS                                            
BEGIN                                            
  UPDATE FOX_TBL_RECONCILIATION_CP SET                 
   RECONCILIATION_STATUS_ID=(Select RECONCILIATION_STATUS_ID from FOX_TBL_RECONCILIATION_STATUS where STATUS_NAME='Unassigned' AND PRACTICE_CODE=@PRACTICE_CODE),                          
   Modified_Date = GETDATE(),                  
   MODIFIED_BY= @USER_NAME,            
   AMOUNT_POSTED =   @AMOUNT_POSTED ,      
   ASSIGNED_TO = null,      
   ASSIGNED_DATE = null ,  
   REMARKS = @REMARKS  
   where RECONCILIATION_CP_ID=@RECONCILIATION_CP_ID                          
   SELECT * FROM FOX_TBL_RECONCILIATION_CP where RECONCILIATION_CP_ID=@RECONCILIATION_CP_ID                          
END

