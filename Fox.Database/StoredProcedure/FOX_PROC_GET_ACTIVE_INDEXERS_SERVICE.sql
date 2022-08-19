IF (OBJECT_ID('FOX_PROC_GET_ACTIVE_INDEXERS_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ACTIVE_INDEXERS_SERVICE
GO 
  -- =============================================                  
-- Author:  <Author,Abdur Rafay>                  
-- Create date: <Create Date,06/22/2021>                  
-- DESCRIPTION: <GET INDEXERS>                         
-- [FOX_PROC_GET_ACTIVE_INDEXERS_SERVICE] 1011163                   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ACTIVE_INDEXERS_SERVICE](            
@PRACTICE_CODE BIGINT)              
AS              
BEGIN              
SELECT USR.FIRST_NAME, USR.LAST_NAME, IND.INDEXER, IND.DEFAULT_VALUE    
   FROM FOX_TBL_ACTIVE_INDEXER IND      
   Left JOIN FOX_TBL_APPLICATION_USER USR on IND.INDEXER = USR.USER_NAME       
            AND USR.PRACTICE_CODE = @PRACTICE_CODE       
            AND ISNULL(USR.DELETED, 0) = 0      
   WHERE       
   IND.PRACTICE_CODE = @PRACTICE_CODE     
   AND ISNULL(IND.IS_ACTIVE, 0) = 1    
   AND ISNULL(IND.DELETED, 0) = 0      
   ORDER BY IND.CREATED_DATE DESC      
END;   
