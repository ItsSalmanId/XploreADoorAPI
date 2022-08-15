IF (OBJECT_ID('FOX_PROC_INSERT_ACTIVE_INDEXER_HISTORY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_ACTIVE_INDEXER_HISTORY  
GO   
-- =============================================                          
-- Author:  <Abdur Rafay>                          
-- Create date: <07/07/2021>                          
-- Description: <INSERT ACTIVE INDEXER HISTORY>                          
-- =============================================                          
-- EXEC FOX_PROC_INSERT_ACTIVE_INDEXER_HISTORY 2, 1011163,'Hanif_544536',548172899          
CREATE PROCEDURE FOX_PROC_INSERT_ACTIVE_INDEXER_HISTORY         
 @ID BIGINT,      
 @PRACTICE_CODE BIGINT,       
 @INDEXER VARCHAR(255),                                    
 @WORK_ID  BIGINT          
                              
AS                           
BEGIN    
DECLARE @RESULT BIGINT;  
SET @RESULT = (SELECT Count(WORK_ID) FROM FOX_TBL_ACTIVE_INDEXER_HISTORY WHERE WORK_ID = @WORK_ID AND WORK_ID <> 0)  
IF(@RESULT = 0 )  
BEGIN  
INSERT INTO FOX_TBL_ACTIVE_INDEXER_HISTORY  ( ACTIVE_INDEXER_HISTORY_ID, PRACTICE_CODE, INDEXER, WORK_ID, CREATED_BY , CREATED_DATE, MODIFIED_BY ,MODIFIED_DATE ,DELETED )      
VALUES(  @ID, @PRACTICE_CODE, @INDEXER, @WORK_ID, 'FOX TEAM', GETDATE(), 'FOX TEAM', GETDATE(), 0 )      
--SELECT  'IF'   
--SELECt @RESULT  
END  
--ELSE   
--BEGIN  
--SELECT 'Else'  
--SELECt @RESULT  
--END  
END     
