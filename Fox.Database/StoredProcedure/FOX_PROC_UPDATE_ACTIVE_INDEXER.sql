IF (OBJECT_ID('FOX_PROC_UPDATE_ACTIVE_INDEXER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_ACTIVE_INDEXER  
GO 
-- =============================================                        
-- Author:  <Abdur Rafay>                        
-- Create date: <07/05/2021>                        
-- Description: <UPDATE ACTIVE INDEXER>                        
-- =============================================                        
-- EXEC FOX_PROC_UPDATE_ACTIVE_INDEXER 1011163,'Ahmed_544531','Regular Indexer',0,'1163testing'    
CREATE PROCEDURE FOX_PROC_UPDATE_ACTIVE_INDEXER          
 @PRACTICE_CODE BIGINT,           
 @INDEXER VARCHAR(50),                                  
 @DEFAULT_VALUE VARCHAR(100),         
 @IS_ACTIVE BIT,          
 @MODIFIED_BY VARCHAR(50)           
                            
AS                                  
BEGIN                                  
    
UPDATE FOX_TBL_ACTIVE_INDEXER    
SET DEFAULT_VALUE = @DEFAULT_VALUE,    
IS_ACTIVE = @IS_ACTIVE,    
MODIFIED_BY =  @MODIFIED_BY    
WHERE INDEXER = @INDEXER AND PRACTICE_CODE = @PRACTICE_CODE  AND ISNULL(DELETED, 0) = 0             
    
SELECT * FROM FOX_TBL_ACTIVE_INDEXER WHERE INDEXER = @INDEXER AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0     
END   
