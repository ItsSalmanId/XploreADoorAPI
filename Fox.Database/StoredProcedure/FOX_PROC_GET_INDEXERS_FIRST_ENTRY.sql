IF (OBJECT_ID('FOX_PROC_GET_INDEXERS_FIRST_ENTRY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEXERS_FIRST_ENTRY 
GO      
-- =============================================                    
-- Author:  <Author,Abdur Rafay>                    
-- Create date: <Create Date,08/20/2021>                    
-- DESCRIPTION: <GET FIRST ENTRY>                
              
-- [FOX_PROC_GET_INDEXERS_FIRST_ENTRY] 'lksdjflsk_544823'            
              
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INDEXERS_FIRST_ENTRY](              
@INDEXER VARCHAR(100))                
AS                
BEGIN                
        
   SELECT INDEXER FROM FOX_TBL_ACTIVE_INDEXER_HISTORY      
   WHERE INDEXER = @INDEXER      
   AND WORK_ID = 0      
   AND (GETDATE() IS NULL  OR CAST(CREATED_DATE AS DATE) BETWEEN CAST(GETDATE() AS DATE) AND CAST(GETDATE() AS DATE))        
END; 