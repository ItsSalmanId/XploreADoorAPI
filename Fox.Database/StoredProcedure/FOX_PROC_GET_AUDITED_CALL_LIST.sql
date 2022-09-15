IF (OBJECT_ID('FOX_PROC_GET_AUDITED_CALL_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_AUDITED_CALL_LIST 
GO
  -- =============================================      
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>      
-- CREATE DATE: <CREATE DATE, 24/04/2018>      
-- DESCRIPTION: <GET AUDITED CALL LIST>      
--   FOX_PROC_GET_AUDITED_CALL_LIST 1011163,'','','12/12/1990','12/12/2020','survey'      
      
CREATE PROCEDURE [dbo].[FOX_PROC_GET_AUDITED_CALL_LIST]     
                @PATIENT_ACCOUNT BIGINT,     
                @PRACTICE_CODE BIGINT,    
                                                       @AGENT_NAME    VARCHAR(100),       
                                                       @AUDITOR_NAME  VARCHAR(100),       
                                                       @DATE_FROM     DATETIME,       
                                                       @DATE_TO       DATETIME,       
                                                       @CALL_TYPE     VARCHAR(100)      
AS      
     BEGIN      
         IF(@AGENT_NAME = '')      
             BEGIN      
                 SET @AGENT_NAME = NULL;      
             END;      
             ELSE      
             BEGIN      
                 SET @AGENT_NAME = @AGENT_NAME;      
             END;      
         IF(@AUDITOR_NAME = '')      
             BEGIN      
                 SET @AUDITOR_NAME = NULL;      
             END;      
             ELSE      
             BEGIN      
                 SET @AUDITOR_NAME = @AUDITOR_NAME;      
             END;    
      IF(@PATIENT_ACCOUNT = 0)            
             BEGIN            
                 SET @PATIENT_ACCOUNT = null;            
             END;    
         IF(@CALL_TYPE = 'all')      
             BEGIN      
                 SELECT *      
                 FROM FOX_TBL_SURVEY_AUDIT_SCORES      
                 WHERE(@AGENT_NAME IS NULL      
                       OR AGENT_NAME = @AGENT_NAME)      
                      AND (@AUDITOR_NAME IS NULL      
                           OR AUDITOR_NAME = @AUDITOR_NAME)      
                      AND ISNULL(DELETED, 0) = 0      
                      AND PRACTICE_CODE = @PRACTICE_CODE      
                      AND (@DATE_FROM IS NULL      
                           OR @DATE_TO IS NULL      
                           OR CONVERT(DATE, CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))       
                 -- AND CALL_TYPE = 'S' OR CALL_TYPE='P'      
                 ORDER BY AGENT_NAME DESC;      
             END;      
             ELSE      
         IF(@CALL_TYPE = 'SURVEY')      
             BEGIN      
                 SELECT *      
                 FROM FOX_TBL_SURVEY_AUDIT_SCORES      
                 WHERE(@AGENT_NAME IS NULL      
                       OR AGENT_NAME = @AGENT_NAME)      
                      AND (@AUDITOR_NAME IS NULL      
                           OR AUDITOR_NAME = @AUDITOR_NAME)      
                      AND ISNULL(DELETED, 0) = 0      
                      AND PRACTICE_CODE = @PRACTICE_CODE      
                      AND (@DATE_FROM IS NULL      
                           OR @DATE_TO IS NULL      
                           OR CONVERT(DATE, CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))      
                      AND CALL_TYPE = 'survey'      
                 ORDER BY AGENT_NAME DESC;      
             END;      
             ELSE      
         IF(@CALL_TYPE = 'phd')      
             BEGIN      
                 SELECT *      
                 FROM FOX_TBL_SURVEY_AUDIT_SCORES      
                 WHERE(@AGENT_NAME IS NULL      
                       OR AGENT_NAME = @AGENT_NAME)      
                      AND (@AUDITOR_NAME IS NULL      
                           OR AUDITOR_NAME = @AUDITOR_NAME)      
                      AND ISNULL(DELETED, 0) = 0     
       AND (@PATIENT_ACCOUNT IS NULL            
                          OR PATIENT_ACCOUNT = @PATIENT_ACCOUNT)    
                     AND PRACTICE_CODE = @PRACTICE_CODE      
                      AND CALL_TYPE = 'phd'      
                      AND (@DATE_FROM IS NULL      
                    OR @DATE_TO IS NULL      
                           OR CONVERT(DATE, CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))      
                 ORDER BY AGENT_NAME DESC;      
             END;      
     END;  
