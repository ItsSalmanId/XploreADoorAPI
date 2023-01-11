-- =============================================        
-- Author:  Muhammad Arslan Tufail        
-- Create date: 02/11/2022        
-- Description: This procedure is used to insert record into PHR User.        
-- =============================================        
-- FOX_PROC_INSERT_PHR_USERS_SERVICE 'TEST', 232323111, '232', 'TEST@MAIL.COM', '1SHHD', 10121        
CREATE PROCEDURE FOX_PROC_INSERT_PHR_USERS_SERVICE        
@USER_NAME VARCHAR(255),        
@PATIENT_ACCOUNT BIGINT,        
@USER_PHONE VARCHAR(255),        
@EMAIL_ADDRESS VARCHAR(255),        
@TEMP_PASSWORD VARCHAR(255),        
@PRACTICE_CODE BIGINT        
AS        
BEGIN        
      
  DECLARE @TABLE TABLE (MAXID BIGINT)      
  DECLARE @MAXUSERID BIGINT      
      
  --SET NOCOUNT ON;            
  -- INSERT STATEMENTS FOR PROCEDURE HERE                  
      
  INSERT INTO @TABLE EXEC Web_GetMaxColumnID 'FOX_PHR_USER_ID'      
      
  SET @MAXUSERID = (SELECT MAXID FROM @TABLE)      
           
 INSERT INTO WS_FOX_TBL_PHR_USERS        
 (        
  [USER_ID],        
  [USER_NAME],        
  PATIENT_ACCOUNT,        
  USER_PHONE,        
  EMAIL_ADDRESS,        
  TEMP_PASSWORD,        
  IS_BLOCK,        
  INVITE_STATUS,        
  PRACTICE_CODE,        
  CREATED_DATE,        
  MODIFIED_DATE,        
  CREATED_BY,        
  MODIFIED_BY,        
  DELETED        
 )        
 VALUES        
 (        
  @MAXUSERID,        
  'FP_' + @USER_NAME + '_' + CAST(@MAXUSERID AS VARCHAR),        
  @PATIENT_ACCOUNT,        
  @USER_PHONE,        
  @EMAIL_ADDRESS,        
  @TEMP_PASSWORD,        
  'false',        
  'Response Awaited',        
  @PRACTICE_CODE,        
  GETDATE(),        
  GETDATE(),        
  'FOX SERVICE',        
  'FOX SERVICE',        
  'false'        
 )        
        
 SELECT * FROM WS_FOX_TBL_PHR_USERS WITH (NOLOCK) WHERE USER_ID = @MAXUSERID        
END 