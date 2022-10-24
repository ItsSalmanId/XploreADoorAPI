IF (OBJECT_ID('FOX_PROC_INSERT_UPDATE_SPECIALITY_PROGRAM') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_UPDATE_SPECIALITY_PROGRAM  
GO   
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/25/2019>      
-- Description: <Description,,>      
-- =============================================      
-- FOX_PROC_INSERT_UPDATE_SPECIALITY_PROGRAM 5489021, '', '','1163TESTING'      
      
CREATE PROCEDURE FOX_PROC_INSERT_UPDATE_SPECIALITY_PROGRAM      
 @WORK_ID BIGINT,      
 @SPECIALITY_PROGRAM VARCHAR(500),      
 @PATIENT_ACCOUNT BIGINT NULL,      
 @USER_NAME VARCHAR(70)      
AS      
BEGIN      
 DECLARE @ID   BIGINT       
    IF ( ISNULL(@PATIENT_ACCOUNT,0) <> 0 AND ISNULL(@SPECIALITY_PROGRAM,'0') <>'0')    
 BEGIN    
 IF EXISTS (SELECT TOP 1 * FROM FOX_TBL_PATIENT_PROCEDURE WHERE WORK_ID = @WORK_ID AND ISNULL(DELETED,0)  = 0)      
 BEGIN      
 -- UPDATE HERE      
  DECLARE @PROC_ID BIGINT = (SELECT TOP 1 PAT_PROC_ID FROM FOX_TBL_PATIENT_PROCEDURE WHERE WORK_ID = @WORK_ID AND ISNULL(DELETED,0) = 0)      
      
  UPDATE FOX_TBL_PATIENT_PROCEDURE       
   SET SPECIALITY_PROGRAM = @SPECIALITY_PROGRAM,      
    WORK_ID = @WORK_ID,      
    PATIENT_ACCOUNT = @PATIENT_ACCOUNT,      
    DELETED = 0,      
    MODIFIED_BY = @USER_NAME,      
    MODIFIED_DATE = GETDATE()      
  WHERE PAT_PROC_ID = @PROC_ID      
        
 END      
 ELSE      
 BEGIN      
    
  EXEC DBO.Web_PROC_GetColumnMaxID_Changed 'PAT_PROC_ID', @ID output      
      
  INSERT INTO FOX_TBL_PATIENT_PROCEDURE (PAT_PROC_ID, WORK_ID, PATIENT_ACCOUNT, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE, DELETED,  SPECIALITY_PROGRAM)      
          VALUES (@ID, @WORK_ID, @PATIENT_ACCOUNT, @USER_NAME, GETDATE(), @USER_NAME, GETDATE(), 0, @SPECIALITY_PROGRAM)      
      
  END    
 END    
      
END   