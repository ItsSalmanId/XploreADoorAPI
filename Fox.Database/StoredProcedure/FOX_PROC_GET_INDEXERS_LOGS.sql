IF (OBJECT_ID('FOX_PROC_GET_INDEXERS_LOGS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEXERS_LOGS 
GO  
-- =============================================                
-- Author:  <Author,Abdur Rafay>                
-- Create date: <Create Date,06/22/2021>                
-- DESCRIPTION: <GET INDEXERS LOGS>            
          
-- [FOX_PROC_GET_INDEXERS_LOGS] 'Ahmed_544531',1011163 ,1,10    
          
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INDEXERS_LOGS](    
@INDEXER VARCHAR(100),    
@PRACTICE_CODE BIGINT,    
@CURRENT_PAGE    INT,                                                               
@RECORD_PER_PAGE INT)            
AS            
     BEGIN            
    
SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                                              
DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE                       
DECLARE @TOATL_PAGESUDM FLOAT            
    
SELECT @TOATL_PAGESUDM = COUNT(*)    
  FROM FOX_TBL_ACTIVE_INDEXER_LOGS LOGS     
 Left JOIN FOX_TBL_APPLICATION_USER USR on LOGS.INDEXER = USR.USER_NAME     
            AND USR.PRACTICE_CODE = @PRACTICE_CODE     
            AND ISNULL(USR.DELETED, 0) = 0    
  Left JOIN FOX_TBL_APPLICATION_USER USR1 on LOGS.CREATED_BY = USR1.USER_NAME     
            AND USR.PRACTICE_CODE = @PRACTICE_CODE     
            AND ISNULL(USR.DELETED, 0) = 0    
   WHERE LOGS.INDEXER = @INDEXER    
    AND LOGS.PRACTICE_CODE = @PRACTICE_CODE      
    AND ISNULL(LOGS.DELETED, 0) = 0    
    
IF(@RECORD_PER_PAGE = 0)                                                              
     BEGIN                                                     
      SET @RECORD_PER_PAGE = @TOATL_PAGESUDM                                                              
     END                                                              
     ELSE                                                              
     BEGIN                              
      SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                                                              
     END                                                              
                                                              
    --                                                             
    IF(@RECORD_PER_PAGE <> 0)                                    BEGIN                                                            
    DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM                                                              
    SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)      
    
   SELECT USR.FIRST_NAME, USR.LAST_NAME, USR1.FIRST_NAME+' '+USR1.LAST_NAME AS FULL_NAME ,LOGS.* ,                                                               
     @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                       
     @TOTAL_RECORDS TOTAL_RECORDS     
  FROM FOX_TBL_ACTIVE_INDEXER_LOGS LOGS     
  Left JOIN FOX_TBL_APPLICATION_USER USR on LOGS.INDEXER = USR.USER_NAME     
            AND INDEXER = @INDEXER    
            AND USR.PRACTICE_CODE = @PRACTICE_CODE     
            AND ISNULL(USR.DELETED, 0) = 0    
  Left JOIN FOX_TBL_APPLICATION_USER USR1 on LOGS.CREATED_BY = USR1.USER_NAME     
            AND USR.PRACTICE_CODE = @PRACTICE_CODE     
            AND ISNULL(USR.DELETED, 0) = 0    
   WHERE LOGS.INDEXER = @INDEXER    
   AND LOGS.PRACTICE_CODE = @PRACTICE_CODE      
   AND ISNULL(LOGS.DELETED, 0) = 0    
   ORDER BY LOGS.CREATED_DATE DESC    
    OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY        
   END    
     END;   
