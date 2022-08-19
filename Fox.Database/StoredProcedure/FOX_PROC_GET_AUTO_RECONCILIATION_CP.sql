IF (OBJECT_ID('FOX_PROC_GET_AUTO_RECONCILIATION_CP') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_AUTO_RECONCILIATION_CP 
GO   
-- =============================================                              
-- Author:  <Muhammad Arqam>                              
-- Create date: <02/12/2020>                              
-- Description: <Updated on AutoReconciled>                              
-- =============================================                             
 -- FOX_PROC_GET_AUTO_RECONCILIATION_CP_TEST  1012714,'Admin_5651352',53422145,'22.31', 'Wed Nov 04 2020' ,''                       
CREATE PROCEDURE [dbo].[FOX_PROC_GET_AUTO_RECONCILIATION_CP]                          
(@PRACTICE_CODE        BIGINT,                      
 @USER_NAME      VARCHAR(70),                       
 @RECONCILIATION_CP_ID BIGINT,                
 @AMOUNT_POSTED MONEY,           
 @DATE_POSTED    VARCHAR(50),    
 @REMARKS VARCHAR(300)    
)                            
AS                                        
BEGIN                                        
  UPDATE FOX_TBL_RECONCILIATION_CP SET                      
   IS_RECONCILIED = 1,                      
   RECONCILIATION_STATUS_ID = (Select RECONCILIATION_STATUS_ID from FOX_TBL_RECONCILIATION_STATUS where STATUS_NAME='Closed' AND PRACTICE_CODE=@PRACTICE_CODE),                      
   Modified_Date = GETDATE(),              
   MODIFIED_BY= @USER_NAME,            
   AMOUNT_POSTED = @AMOUNT_POSTED,          
   DATE_POSTED = CONVERT(DATETIME, @DATE_POSTED),    
   REMARKS = @REMARKS    
   where RECONCILIATION_CP_ID = @RECONCILIATION_CP_ID                      
   SELECT * FROM FOX_TBL_RECONCILIATION_CP where RECONCILIATION_CP_ID = @RECONCILIATION_CP_ID                      
END   
