IF (OBJECT_ID('FOX_PROC_SET_PATIENT_OPENBY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_SET_PATIENT_OPENBY  
GO       
-- =============================================          
-- Author:  <Muhammad Imran>          
-- Create date: <Create Date,,>          
-- Description: <Description,,>          
-- =============================================          
CREATE PROCEDURE FOX_PROC_SET_PATIENT_OPENBY  -- 101116354813875, '1163testing', 1011163, null        
 @PATIENT_ACCOUNT BIGINT,          
 @USER_NAME VARCHAR(70),          
 @PRACTICE_CODE BIGINT        
         
AS          
BEGIN          
  -- SET NOCOUNT ON added to prevent extra result sets from          
  -- interfering with SELECT statements.          
  SET NOCOUNT ON;          
  DECLARE @OPEN_BY VARCHAR(150)        
  IF EXISTS(SELECT * FROM FOX_TBL_PATIENT WHERE ISNULL(DELETED,0) = 0 AND Patient_Account = @PATIENT_ACCOUNT AND ISNULL(IS_OPENED_BY,'') = '')          
  BEGIN          
   UPDATE FOX_TBL_PATIENT          
    SET IS_OPENED_BY = @USER_NAME,          
     Modified_By = @USER_NAME,          
     Modified_Date = GETDATE()          
    WHERE ISNULL(Deleted , 0) = 0          
     AND Patient_Account = @PATIENT_ACCOUNT          
     AND ISNULL(IS_OPENED_BY , '' ) = ''          
    SET @OPEN_BY = NULL          
  END          
  ELSE IF EXISTS(SELECT * FROM FOX_TBL_PATIENT WHERE ISNULL(DELETED,0) = 0 AND Patient_Account = @PATIENT_ACCOUNT AND IS_OPENED_BY = @USER_NAME)          
  BEGIN          
   SET @OPEN_BY = NULL          
  END          
  ELSE IF EXISTS (SELECT * FROM FOX_TBL_PATIENT WHERE ISNULL(DELETED,0) = 0 AND Patient_Account = @PATIENT_ACCOUNT AND IS_OPENED_BY <> @USER_NAME)          
  BEGIN          
   DECLARE           
    @FIRST_NAME VARCHAR(50),          
    @LAST_NAME VARCHAR(50)          
          
    SELECT @LAST_NAME = LAST_NAME, @FIRST_NAME = FIRST_NAME         
 FROM FOX_TBL_APPLICATION_USER         
 WHERE USER_NAME = (SELECT TOP 1 IS_OPENED_BY FROM FOX_TBL_PATIENT WHERE ISNULL(DELETED,0) = 0 AND Patient_Account = @PATIENT_ACCOUNT AND ISNULL(DELETED,0) = 0)         
 AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0          
          
    SET @OPEN_BY = CASE WHEN ISNULL(@LAST_NAME, '') = '' THEN @FIRST_NAME ELSE @LAST_NAME + ', ' + @FIRST_NAME END          
  END        
        
  SELECT @OPEN_BY        
          
  RETURN         
END       
  
