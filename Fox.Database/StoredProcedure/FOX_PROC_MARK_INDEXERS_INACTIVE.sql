IF (OBJECT_ID('FOX_PROC_MARK_INDEXERS_INACTIVE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_MARK_INDEXERS_INACTIVE 
GO  
-- =============================================                      
-- Author:  <Author,Abdur Rafay>                      
-- Create date: <Create Date,08/20/2021>                      
-- DESCRIPTION: <MARK INDEXERS INACTIVE>                  
                
-- [FOX_PROC_MARK_INDEXERS_INACTIVE]  1011163        
                
CREATE PROCEDURE [dbo].[FOX_PROC_MARK_INDEXERS_INACTIVE](          
@PRACTICE_CODE BIGINT)                
AS                  
BEGIN                  
UPDATE FOX_TBL_ACTIVE_INDEXER    
SET IS_ACTIVE = 0    
WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(IS_ACTIVE, 0) = 1    
END;   
  
  