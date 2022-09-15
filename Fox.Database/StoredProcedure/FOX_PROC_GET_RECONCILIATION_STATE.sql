IF (OBJECT_ID('FOX_PROC_GET_RECONCILIATION_STATE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECONCILIATION_STATE  
GO  
-- =============================================                          
-- Author:  <Abdur Rafay>                          
-- Create date: <12/16/2020>                          
-- Description: <INSERT RECONCILIATION EXCEL DATA NEW>                          
-- =============================================   
--[FOX_PROC_GET_RECONCILIATION_STATE] 1012714, 1, 0, '11/23/2020','11/23/2021'     
CREATE PROCEDURE [dbo].[FOX_PROC_GET_RECONCILIATION_STATE](      
@PRACTICE_CODE BIGINT,      
 @IS_DEPOSIT_DATE_SEARCH  BIT,                                         
 @IS_ASSIGNED_DATE_SEARCH BIT,       
 @DATE_FROM               VARCHAR(15),                                         
 @DATE_TO                 VARCHAR(15)      
)                  
AS                  
     BEGIN           
  IF(@DATE_FROM = '')                                        
             BEGIN                                        
                 SET @DATE_FROM = NULL;                                        
             END;                                        
             ELSE                                        
             BEGIN                                        
     SET @DATE_FROM = @DATE_FROM+'%';                 
     END;                                        
         IF(@DATE_TO = '')                                        
      BEGIN                              
                 SET @DATE_TO = NULL;                                        
             END;                                        
             ELSE                                        
             BEGIN                                        
                 SET @DATE_TO = @DATE_TO+'%';                                        
             END;      
         SELECT DISTINCT ftrc.STATE                  
         FROM dbo.FOX_TBL_RECONCILIATION_CP ftrc                  
         WHERE ISNULL(ftrc.DELETED,0) = 0               
         AND ftrc.PRACTICE_CODE = @PRACTICE_CODE                 
         AND ftrc.STATE IS NOT NULL         
         
         AND (@DATE_FROM IS NULL                                        
          OR ((CASE                                        
          WHEN @IS_DEPOSIT_DATE_SEARCH = 1                                  
          THEN CAST(ftrc.DEPOSIT_DATE AS DATE)                                        
          WHEN @IS_ASSIGNED_DATE_SEARCH = 1                                        
          THEN CAST(ftrc.ASSIGNED_DATE AS DATE)                                        
          ELSE CAST(ftrc.COMPLETED_DATE AS DATE)                                        
         END) >= CONVERT(DATE, Replace(@DATE_FROM, '%', ''), 101)))                                        
        AND (@DATE_TO IS NULL                                        
          OR ((CASE                                        
          WHEN @IS_DEPOSIT_DATE_SEARCH = 1                                        
          THEN CAST(ftrc.DEPOSIT_DATE AS DATE)                                        
          WHEN @IS_ASSIGNED_DATE_SEARCH = 1                                        
          THEN CAST(ftrc.ASSIGNED_DATE AS DATE)                                        
          ELSE CAST(ftrc.COMPLETED_DATE AS DATE)                                        
         END) <= CONVERT(DATE, Replace(@DATE_TO, '%', ''), 101)))      
     END; 
