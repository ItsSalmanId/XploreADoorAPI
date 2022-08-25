IF (OBJECT_ID('FOX_PROC_GET_LEAST_ASSIGN_POC_INDEXERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_LEAST_ASSIGN_POC_INDEXERS  
GO
  -- =============================================                    
-- Author:  <Author,Abdur Rafay>                    
-- Create date: <Create Date,07/29/2021>                    
-- DESCRIPTION: <GET LEAST ASSIGN POC INDEXERS>                              
-- [FOX_PROC_GET_LEAST_ASSIGN_POC_INDEXERS] '07/30/2021',1011163 , 'POC Indexer'                   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_LEAST_ASSIGN_POC_INDEXERS](        
@DATE DATETIME,        
@PRACTICE_CODE BIGINT,      
@DEFAULT_VALUE VARCHAR(50))              
AS                
BEGIN                
  IF(@DEFAULT_VALUE = 'Trainee Indexer')      
  BEGIN      
  SELECT TOP 1 HIS.INDEXER      
   FROM FOX_TBL_ACTIVE_INDEXER_HISTORY HIS      
   JOIN FOX_TBL_ACTIVE_INDEXER IND on HIS.INDEXER = IND.INDEXER       
           AND IND.DEFAULT_VALUE = @DEFAULT_VALUE       
           AND ISNULL(IND.IS_ACTIVE, 0) = 1      
   WHERE (CONVERT(DATE, HIS.CREATED_DATE ) BETWEEN CONVERT(DATE, @DATE ) AND CONVERT(DATE, @DATE ))       
   AND HIS.PRACTICE_CODE = @PRACTICE_CODE      
   GROUP BY HIS.INDEXER      
   HAVING COUNT(HIS.INDEXER) < 21      
   ORDER BY  COUNT(HIS.INDEXER) ASC      
  END      
  ELSE       
  BEGIN      
  SELECT TOP 1 HIS.INDEXER      
   FROM FOX_TBL_ACTIVE_INDEXER_HISTORY HIS      
   JOIN FOX_TBL_ACTIVE_INDEXER IND on HIS.INDEXER = IND.INDEXER       
           AND IND.DEFAULT_VALUE = @DEFAULT_VALUE      
           AND ISNULL(IND.IS_ACTIVE, 0) = 1      
   WHERE (CONVERT(DATE, HIS.CREATED_DATE ) BETWEEN CONVERT(DATE, @DATE ) AND CONVERT(DATE, @DATE ))       
   AND HIS.PRACTICE_CODE = @PRACTICE_CODE      
   GROUP BY HIS.INDEXER      
   ORDER BY  COUNT(HIS.INDEXER) ASC      
  END      
END;     
    
