IF (OBJECT_ID('FOX_PROC_INSERT_SURVEY_SERVICE_LOG') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_SURVEY_SERVICE_LOG  
GO   
-- =============================================                  
-- Author:  <Author,Abdur Rafey>                  
-- Create date: <Create Date,01/11/2021>                  
-- DESCRIPTION: <INSERT FOX SURVEY SERVICE LOG>                  
              
--   [dbo].[FOX_PROC_INSERT_SURVEY_SERVICE_LOG] 1011163, '01/11/2021'            
              
CREATE PROCEDURE [FOX_PROC_INSERT_SURVEY_SERVICE_LOG]              
(@PRACTICE_CODE BIGINT            
,@DATE VARCHAR(20))              
AS              
BEGIN              
        
DECLARE @TOTAL_CALLS INT        
SET     @TOTAL_CALLS = 0;    
    
DECLARE @LOG_ID BIGINT                                  
SET @LOG_ID = (SELECT ISNULL(MAX(FOX_TBL_SURVEY_SERVICE_LOG_ID),500100) FROM FOX_TBL_SURVEY_SERVICE_LOG)     
    
SELECT @TOTAL_CALLS = COUNT(*) FROM FOX_TBL_PATIENT_SURVEY        
WHERE        
cast(MODIFIED_DATE as date) = @DATE        
AND PRACTICE_CODE = @PRACTICE_CODE        
AND DELETED = 0        
        
    
INSERT INTO FOX_TBL_SURVEY_SERVICE_LOG(    
FOX_TBL_SURVEY_SERVICE_LOG_ID    
,SURVEY_ID_COUNT    
,IS_SUCCESSFUL    
,CREATED_BY    
,CREATED_DATE    
,MODIFIED_BY    
,MODIFIED_DATE    
,DELETED    
)    
VALUES    
(    
@LOG_ID + 1    
,@TOTAL_CALLS    
,1    
,'FOX SURVEY SERVICE'    
,GETDATE()    
,'FOX SURVEY SERVICE'    
,GETDATE()    
,0    
);    
    
    
END;  
