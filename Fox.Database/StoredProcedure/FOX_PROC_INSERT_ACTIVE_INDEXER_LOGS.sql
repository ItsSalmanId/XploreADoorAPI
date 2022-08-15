IF (OBJECT_ID('FOX_PROC_INSERT_ACTIVE_INDEXER_LOGS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_ACTIVE_INDEXER_LOGS  
GO    
-- =============================================                        
-- Author:  <Abdur Rafay>                        
-- Create date: <07/07/2021>                        
-- Description: <INSERT ACTIVE INDEXER LOGS>                        
-- =============================================                        
-- EXEC FOX_PROC_INSERT_ACTIVE_INDEXER_LOGS 500101, 1011163,'Ahmed_544531','Nisar Ahmed status chnages to inactive','1163testing'       
CREATE PROCEDURE FOX_PROC_INSERT_ACTIVE_INDEXER_LOGS        
 @ID BIGINT,    
 @PRACTICE_CODE BIGINT,     
 @INDEXER VARCHAR(255),                                  
 @LOG_MESSAGE VARCHAR(500),         
 @CREATED_BY VARCHAR(50)         
                            
AS                                  
BEGIN                                  
    
INSERT INTO FOX_TBL_ACTIVE_INDEXER_LOGS ( ACTIVE_INDEXER_LOG_ID, PRACTICE_CODE, INDEXER, LOG_MESSAGE, CREATED_BY , CREATED_DATE, MODIFIED_BY ,MODIFIED_DATE ,DELETED )    
VALUES(  @ID, @PRACTICE_CODE, @INDEXER, @LOG_MESSAGE, @CREATED_BY, GETDATE(), @CREATED_BY, GETDATE(), 0 )    
    
END   
