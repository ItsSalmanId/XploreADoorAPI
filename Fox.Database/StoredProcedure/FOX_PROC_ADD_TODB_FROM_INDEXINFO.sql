IF (OBJECT_ID('FOX_PROC_ADD_TODB_FROM_INDEXINFO') IS NOT NULL ) DROP PROCEDURE FOX_PROC_ADD_TODB_FROM_INDEXINFO
GO 
-- =============================================                                                
-- Author:  <Author,Abdur Rafey>                                                
-- Create date: <Create Date,01/11/2021>                                                
-- DESCRIPTION: <ADD TODB FROM INDEX INFO>                                                                   
CREATE PROCEDURE FOX_PROC_ADD_TODB_FROM_INDEXINFO(                                
@PRACTICE_CODE BIGINT NULL,                                
@WORK_ID BIGINT NULL,                                
@USER_NAME VARCHAR(255) NULL,                                
@NO_OF_PAGES INT NULL,                                
@APPROVAL BIT NULL,              
@DECLINE BIT NULL                                
)                                
AS                                
BEGIN                                
                                
--DECLARE @PRACTICE_CODE BIGINT  = 1011163,                                
--@WORK_ID BIGINT  = 1012,                                
--@USER_NAME VARCHAR(255) = '1163testing',                                
--@NO_OF_PAGES INT = 1;                                
                                
DECLARE @DOCUMENT_TYPE VARCHAR(255)                                
DECLARE @USER_ID BIGINT                         
DECLARE @Is_Signed BIT                        
DECLARE @IS_DENIED BIT                              
                        
SET @Is_Signed = (SELECT IsSigned FROM FOX_TBL_WORK_QUEUE WHERE WORK_ID = @WORK_ID)                        
SET @IS_DENIED =  (SELECT IS_DENIED FROM FOX_TBL_WORK_QUEUE WHERE WORK_ID = @WORK_ID)        
                      
IF(@APPROVAL = 1)                                
BEGIN                                
SET @DOCUMENT_TYPE = (SELECT DOCUMENT_TYPE_ID FROM FOX_TBL_DOCUMENT_TYPE WHERE NAME = 'Signed Order' AND ISNULL(DELETED, 0) = 0)                                
SET @USER_ID = (SELECT USER_ID FROM FOX_TBL_APPLICATION_USER WHERE USER_NAME = @USER_NAME AND ISNULL(DELETED, 0) = 0 AND PRACTICE_CODE = @PRACTICE_CODE)                        
SET @Is_Signed = 1                    
SET @IS_DENIED = 0                           
END                       
IF(@APPROVAL = 0 and @DECLINE = 1)                                
BEGIN                                
SET @USER_ID = NULL                                
SET @DOCUMENT_TYPE = (SELECT DOCUMENT_TYPE_ID FROM FOX_TBL_DOCUMENT_TYPE WHERE NAME = 'Unsigned Order' AND ISNULL(DELETED, 0) = 0)                
SET @IS_DENIED = 1                               
END          
IF(@APPROVAL = 1 and @IS_DENIED = 1)                                
BEGIN                                
SET @USER_ID = NULL                                
SET @DOCUMENT_TYPE = (SELECT DOCUMENT_TYPE_ID FROM FOX_TBL_DOCUMENT_TYPE WHERE NAME = 'Signed Order' AND ISNULL(DELETED, 0) = 0)                
SET @IS_DENIED = 0                              
END                                 
ELSE IF(@APPROVAL = 0 and @Is_Signed = 1)                                
BEGIN                                
SET @DOCUMENT_TYPE = (SELECT DOCUMENT_TYPE_ID FROM FOX_TBL_DOCUMENT_TYPE WHERE NAME = 'Signed Order' AND ISNULL(DELETED, 0) = 0)                                
SET @USER_ID = (SELECT USER_ID FROM FOX_TBL_APPLICATION_USER WHERE USER_NAME = @USER_NAME AND ISNULL(DELETED, 0) = 0 AND PRACTICE_CODE = @PRACTICE_CODE)                        
--SET @Is_Signed = 1                 
SET @IS_DENIED = 0                               
END                                
ELSE IF(@APPROVAL = 0 and @Is_Signed = 0)                                
BEGIN                                
SET @USER_ID = NULL                                
SET @DOCUMENT_TYPE = (SELECT DOCUMENT_TYPE_ID FROM FOX_TBL_DOCUMENT_TYPE WHERE NAME = 'Unsigned Order' AND ISNULL(DELETED, 0) = 0)                
--SET @IS_DENIED = 0                               
END               
                                
                                
UPDATE FOX_TBL_WORK_QUEUE              
SET DOCUMENT_TYPE = @DOCUMENT_TYPE,                                
 IsSigned = @Is_Signed,                            
 IS_DENIED = @IS_DENIED,                                
 TOTAL_PAGES = @NO_OF_PAGES,                                
 FAX_ID = '',                         
 SignedBy = @USER_ID,                                
 MODIFIED_BY = @USER_NAME,                                
 MODIFIED_DATE = GETDATE()                                
                                
 WHERE WORK_ID = @WORK_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0                                
                                
 select * from fox_tbl_work_queue WHERE WORK_ID = @WORK_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0                                
END; 