IF (OBJECT_ID('FOX_PROC_INSERT_RECONCILIATION_EXCEL_DATA') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_RECONCILIATION_EXCEL_DATA  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_INSERT_RECONCILIATION_EXCEL_DATA]                  
 @PRACTICE_CODE BIGINT,                              
 @USER_NAME VARCHAR(70),                              
 @RECONCILIATION_DATA RECONCILIATION_CP_IMPORT READONLY                              
AS                              
BEGIN                              
                              
  DECLARE                               
   @DEPOSIT_DATE DATETIME,                               
   @DEPOSIT_TYPE_ID BIGINT,                               
   @CATEGORY_ID BIGINT,                               
   @INSURANCE VARCHAR(100),                               
   @CHECK_NO VARCHAR(50),                               
   @AMOUNT MONEY,                               
   @AMOUNT_POSTED MONEY,                               
   @AMOUNT_NOT_POSTED MONEY,                              
   @DATE_ASSIGNED DATETIME,                               
   @ASSIGNED_TO VARCHAR(70),                               
   @DATE_ENTERED DATETIME,                               
   @BATCH_NO VARCHAR(300),                              
   @ID BIGINT,                              
   @INSURANCE_NAME VARCHAR(100),                              
   @DEPOSIT_TYPE VARCHAR(100),                              
   @CATEGORY_ACCOUNT VARCHAR(100),                              
   @AMOUNT1 VARCHAR(10),                              
   @AMOUNT_POSTED1 VARCHAR(10),                              
   @AMOUNT_NOT_POSTED1 VARCHAR(10),                              
   @DATE_ENTERED1 VARCHAR(15),                              
   @RECORD_EXISTED BIGINT,                              
   @RECORD_INSERTED BIGINT,                              
   @DEPOSIT_DATE1 VARCHAR(15),                              
   @TOTAL_RECORDS BIGINT  ,                            
   @UNASSIGNED_STATUSID BIGINT,                             
   @POSTED_DATE DATETIME,                          
   @DATE_ASSIGNED1 VARCHAR(15),                          
   @SOFTPOSTEDAMOUNT MONEY                          
                             
             
            
  SET @RECORD_EXISTED = 0                               
  SET @RECORD_INSERTED = 0                             
                               
                              
                              
                              
  SET @TOTAL_RECORDS = (SELECT COUNT(*) FROM @RECONCILIATION_DATA)                              
                               
 DECLARE @TEMP TABLE(ID BIGINT, DEPOSIT_DATE DATETIME, DEPOSIT_TYPE_ID BIGINT, CATEGORY_ID BIGINT, INSURANCE VARCHAR(100), CHECK_NO VARCHAR(50), AMOUNT MONEY,                               
   AMOUNT_POSTED MONEY, AMOUNT_NOT_POSTED MONEY,DATE_ASSIGNED DATETIME, ASSIGNED_TO VARCHAR(70), DATE_ENTERED DATETIME, BATCH_NO VARCHAR(300), POSTED_DATE DATETIME)                              
                            
   SET @UNASSIGNED_STATUSID = (SELECT TOP 1  RECONCILIATION_STATUS_ID FROM  FOX_TBL_RECONCILIATION_STATUS WHERE LOWER(STATUS_NAME) = 'unassigned')                            
                                 
  DECLARE RECONSILITION_TEMP CURSOR FOR                                
    SELECT DEPOSIT_DATE, DEPOSIT_TYPE,CATEGORY_ACCOUNT, INSURANCE_NAME,CHECK_NO,BATCH_NO, AMOUNT,AMOUNT_POSTED, DATE_POSTED, DATE_ASSIGNED , ASSIGNED_TO                               
    FROM   @RECONCILIATION_DATA                                
                                
  OPEN RECONSILITION_TEMP;                                
                                
  FETCH NEXT FROM RECONSILITION_TEMP INTO @DEPOSIT_DATE1, @DEPOSIT_TYPE,@CATEGORY_ACCOUNT, @INSURANCE_NAME, @CHECK_NO, @BATCH_NO, @AMOUNT1, @AMOUNT_POSTED1, @POSTED_DATE, @DATE_ASSIGNED1, @ASSIGNED_TO                            
                                
  WHILE @@FETCH_STATUS = 0                      
    BEGIN                                
                             
     IF ((SELECT COUNT(*) FROM FOX_TBL_RECONCILIATION_CP WHERE ISNULL(DELETED, 0) = 0 AND PRACTICE_CODE = @PRACTICE_CODE AND CHECK_NO = @CHECK_NO AND CONVERT(DATE, DEPOSIT_DATE) = CONVERT(DATE, @DEPOSIT_DATE1)) > 0)                              
     BEGIN                              
  SET @RECORD_EXISTED = @RECORD_EXISTED + 1;                           END                              
     ELSE                              
     BEGIN                         EXEC DBO.Web_PROC_GetColumnMaxID_Changed 'FOX_RECONCILIATION_CP_ID', @ID output                                
                                 
     --SET @INSURANCE = (SELECT TOP 1 INSURANCE_NAME FROM FOX_TBL_INSURANCE WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED,0) = 0 AND INSURANCE_NAME = @INSURANCE_NAME)                               
     SET @DEPOSIT_TYPE_ID = (SELECT TOP 1 DEPOSIT_TYPE_ID FROM FOX_TBL_RECONCILIATION_DEPOSIT_TYPE WHERE  ISNULL(DELETED,0) = 0 AND DEPOSIT_TYPE_NAME = @DEPOSIT_TYPE AND Practice_Code = @PRACTICE_CODE)                              
     SET @CATEGORY_ID = (SELECT TOP 1 CATEGORY_ID FROM FOX_TBL_RECONCILIATION_CATEGORY WHERE  ISNULL(DELETED,0) = 0 AND CATEGORY_NAME = @CATEGORY_ACCOUNT AND Practice_Code = @PRACTICE_CODE)                           
     SET @SOFTPOSTEDAMOUNT = (SELECT TOP 1 CONVERT(MONEY,ISNULL(PAID_AMOUNT,0)) FROM  MIS_TBL_DWC_DEPOSITSLIP WHERE check_No = @CHECK_NO AND Practice_Code = @PRACTICE_CODE  AND ISNULL(DELETED,0) = 0 AND CONVERT(DATE, PAYMENT_DATE) = CONVERT(DATE, @DEPOSIT
_DATE1))                          
               
    IF CHARINDEX('(',@AMOUNT1) > 0                                
 BEGIN                                        
 SET @AMOUNT1 = REPLACE(@AMOUNT1,'(', '-')                                        
 END            
  IF CHARINDEX(')',@AMOUNT1) > 0                                
 BEGIN                                        
 SET @AMOUNT1 = REPLACE(@AMOUNT1,')', '')                                        
 END            
            
 IF CHARINDEX('(',@AMOUNT_NOT_POSTED1) > 0                                
 BEGIN                                        
 SET @AMOUNT_NOT_POSTED1 = REPLACE(@AMOUNT_NOT_POSTED1,'(', '-')                                        
 END            
  IF CHARINDEX(')',@AMOUNT_NOT_POSTED1) > 0                                
 BEGIN                                        
 SET @AMOUNT_NOT_POSTED1 = REPLACE(@AMOUNT_NOT_POSTED1,')', '')                                        
 END            
            
     INSERT INTO @TEMP (ID,DEPOSIT_DATE, DEPOSIT_TYPE_ID, CATEGORY_ID, INSURANCE, CHECK_NO, AMOUNT, AMOUNT_POSTED, AMOUNT_NOT_POSTED,   BATCH_NO, POSTED_DATE, ASSIGNED_TO, DATE_ASSIGNED)                              
       VALUES (@ID, CONVERT(DATE,@DEPOSIT_DATE1), @DEPOSIT_TYPE_ID, @CATEGORY_ID, @INSURANCE_NAME, @CHECK_NO, ISNULL(CONVERT(MONEY,@AMOUNT1),0.00), ISNULL(@SOFTPOSTEDAMOUNT,0.00),                               
       ISNULL(CONVERT(MONEY,@AMOUNT_NOT_POSTED1),0.00), @BATCH_NO, cast((case when isnull(@POSTED_DATE,'1900-01-01 00:00:00.000') = '1900-01-01 00:00:00.000' then null else @POSTED_DATE end) as datetime), @ASSIGNED_TO, CONVERT(DATE,@DATE_ASSIGNED1))      

  
    
                          
                                
                                     
                                  
    SET @RECORD_INSERTED = @RECORD_INSERTED + 1;                              
                                    
                                  
   END                              
   FETCH NEXT FROM RECONSILITION_TEMP INTO @DEPOSIT_DATE1, @DEPOSIT_TYPE,@CATEGORY_ACCOUNT, @INSURANCE_NAME, @CHECK_NO, @BATCH_NO, @AMOUNT1, @AMOUNT_POSTED1, @POSTED_DATE, @DATE_ASSIGNED1, @ASSIGNED_TO                              
    END                                
     
  CLOSE RECONSILITION_TEMP;                                
                                
  DEALLOCATE RECONSILITION_TEMP;                                
                       
  IF (@RECORD_INSERTED > 0)                              
  BEGIN                              
   INSERT INTO FOX_TBL_RECONCILIATION_CP (RECONCILIATION_CP_ID,DEPOSIT_DATE, DEPOSIT_TYPE_ID, CATEGORY_ID, FOX_TBL_INSURANCE_NAME, CHECK_NO, AMOUNT, AMOUNT_POSTED,                                
    BATCH_NO, CREATED_BY, MODIFIED_BY, CREATED_DATE, MODIFIED_DATE, PRACTICE_CODE, RECONCILIATION_STATUS_ID, DATE_POSTED, ASSIGNED_GROUP, ASSIGNED_GROUP_DATE, IS_RECONCILIED)                                 
   SELECT ID,DEPOSIT_DATE, DEPOSIT_TYPE_ID, CATEGORY_ID, INSURANCE, CHECK_NO, AMOUNT, AMOUNT_POSTED,  BATCH_NO, @USER_NAME, @USER_NAME, GETDATE(), GETDATE(), @PRACTICE_CODE,                           
   @UNASSIGNED_STATUSID, POSTED_DATE ,ASSIGNED_TO  ,DATE_ASSIGNED, CONVERT(BIT, 0)  FROM @TEMP                              
                                 
   DECLARE @LOG_ID BIGINT                              
   SET @LOG_ID = (SELECT ISNULL(MAX(ID),500100) FROM FOX_TBL_RECONCILIATION_UPLOAD_LOG)                              
                              
   INSERT INTO FOX_TBL_RECONCILIATION_UPLOAD_LOG (ID, LAST_UPDATED_DATE, TOTAL_RECORDS,CREATED_DATE, CREATED_BY, MODIFIED_BY, MODIFIED_DATE, PRACTICE_CODE, SUCCESSFULLY_ADDED)                              
           VALUES(@LOG_ID+1, GETDATE(), @TOTAL_RECORDS, GETDATE(), @USER_NAME, @USER_NAME, GETDATE(), @PRACTICE_CODE, @RECORD_INSERTED)                              
                                 
  END                              
                              
  SELECT @RECORD_EXISTED AS RECORD_EXISTED, @RECORD_INSERTED AS RECORD_INSERTED                              
                                    
                               
                              
END     
