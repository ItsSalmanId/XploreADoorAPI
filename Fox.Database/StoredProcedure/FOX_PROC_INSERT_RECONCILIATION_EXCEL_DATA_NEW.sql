IF (OBJECT_ID('FOX_PROC_INSERT_RECONCILIATION_EXCEL_DATA_NEW') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_RECONCILIATION_EXCEL_DATA_NEW  
GO 
-- =============================================                                      
-- Author:  <Abdur Rafay>                                      
-- Create date: <12/16/2020>                                      
-- Description: <INSERT RECONCILIATION EXCEL DATA NEW>                                      
-- =============================================                                      
-- EXEC   FOX_PROC_INSERT_RECONCILIATION_EXCEL_DATA_NEW 1401,1012714,'02/12/2022','UMWA Health Funds','4288********6733','175.47','0.00','0.00','MTBC1','10/25/19','1900-01-01 00:00:00.000','10719902','Admin_5651352','EFT','EFT','NJ'                           
CREATE PROCEDURE FOX_PROC_INSERT_RECONCILIATION_EXCEL_DATA_NEW                     
 @RECONCILIATION_CP_ID BIGINT,                      
 @PRACTICE_CODE BIGINT,                         
 @DEPOSIT_DATE VARCHAR(50),                                 
 @FOX_TBL_INSURANCE_NAME VARCHAR(100),                       
 @CHECK_NO VARCHAR(50),                        
 @AMOUNT VARCHAR(50),                                                 
 @AMOUNT_POSTED VARCHAR(50),                                                 
 @AMOUNT_NOT_POSTED VARCHAR(50),                       
 @ASSIGNED_TO VARCHAR(70),                      
 @ASSIGNED_DATE VARCHAR(50),                          
 @DATE_POSTED VARCHAR(50),                        
 @BATCH_NO VARCHAR(300),                       
 @USER_NAME VARCHAR(70),                      
 @DEPOSIT_TYPE VARCHAR(100),                                                
 @CATEGORY_ACCOUNT VARCHAR(100),                          
 @STATE VARCHAR(100)                                       
AS                                                
BEGIN                                                
                                                
DECLARE                                                 
@CATEGORY_ID BIGINT,                         
@DEPOSIT_TYPE_ID BIGINT,                      
@RECONCILIATION_STATUS_ID BIGINT,                          
@COMPLETED_DATE DATETIME,                      
@COMPLETED_BY VARCHAR(70),                      
@LEDGER_NAME VARCHAR(200),                      
@LEDGER_PATH VARCHAR(500),                      
@TOTAL_LEDGER_PAGES INT,                        
@REASON VARCHAR(4),                      
@REMARKS VARCHAR(300),                       
@ASSIGNED_GROUP VARCHAR(70),                      
@ASSIGNED_GROUP_DATE DATETIME,                      
@SOFTPOSTEDAMOUNT MONEY,                       
@RECORD_EXISTED BIGINT,                                               
@RECORD_INSERTED BIGINT                      
   IF OBJECT_ID('TEMPDB.DBO.#SUMOFCHECK', 'U') IS NOT NULL                              
             DROP TABLE #SUMOFCHECK;                   
   IF OBJECT_ID('TEMPDB.DBO.#POSTEDDATE', 'U') IS NOT NULL                              
             DROP TABLE #POSTEDDATE;                
Select                                                 
   ISNULL(CLP.AMOUNT_PAID, 0) AS AMOUNT_POSTED                  
   INTO #SUMOFCHECK                  
  FROM MIS_TBL_DWC_DEPOSITSLIP DS                                                         
  LEFT JOIN CLAIM_PAYMENTS CLP ON  CLP.Check_No = DS.Check_No                                                      
  LEFT JOIN INSURANCES I ON  CLP.INSURANCE_ID = I.INSURANCE_ID AND ISNULL(I.DELETED, 0) = '0'                                                                        
  LEFT JOIN INSURANCE_PAYERS IP ON  I.INSPAYER_ID = IP.INSPAYER_ID AND ISNULL(IP.DELETED, 0) = '0'                                                                        
  JOIN CLAIMS C ON  C.CLAIM_NO = CLP.CLAIM_NO                                                                  
  JOIN PATIENT P ON  P.PATIENT_ACCOUNT = C.PATIENT_ACCOUNT   AND ISNULL(P.DELETED, 0) = '0'                                                                         
  WHERE  p.PRACTICE_CODE = @PRACTICE_CODE                                                                       
        AND DS.check_No = @CHECK_NO AND DS.Practice_Code = @PRACTICE_CODE             
        AND ISNULL(CLP.DELETED, 0) = '0'     
  AND DS.PRACTICE_CODE = @PRACTICE_CODE    
  AND  CONVERT(DATE,CLP.Created_Date) >= DATEADD(day,-30, @DEPOSIT_DATE)          
          
 SELECT TOP 1  CONVERT(VARCHAR(10), CLP.Created_Date, 101) AS POSTED_DATE           
  INTO #POSTEDDATE          
 FROM MIS_TBL_DWC_DEPOSITSLIP DS                                                 
  LEFT JOIN CLAIM_PAYMENTS CLP ON  CLP.Check_No = DS.Check_No                                              
  LEFT JOIN INSURANCES I ON  CLP.INSURANCE_ID = I.INSURANCE_ID AND ISNULL(I.DELETED, 0) = '0'                                                                
  LEFT JOIN INSURANCE_PAYERS IP ON  I.INSPAYER_ID = IP.INSPAYER_ID AND ISNULL(IP.DELETED, 0) = '0'                                                                
  JOIN CLAIMS C ON  C.CLAIM_NO = CLP.CLAIM_NO                                                          
  JOIN PATIENT P ON  P.PATIENT_ACCOUNT = C.PATIENT_ACCOUNT   AND ISNULL(P.DELETED, 0) = '0'                                                                 
  WHERE  p.PRACTICE_CODE = @PRACTICE_CODE                                                           
        AND DS.check_No = @CHECK_NO                                                               
        AND ISNULL(DS.DELETED, 0) = '0'    
  AND DS.PRACTICE_CODE = @PRACTICE_CODE    
  AND  CONVERT(DATE,CLP.Created_Date) >= DATEADD(day,-30, @DEPOSIT_DATE)          
  ORDER by CLP.Created_Date DESC          
                  
SET @RECORD_EXISTED = 0                                                 
SET @RECORD_INSERTED = 0                                               
SET @DEPOSIT_TYPE_ID = (SELECT TOP 1 DEPOSIT_TYPE_ID FROM FOX_TBL_RECONCILIATION_DEPOSIT_TYPE WHERE  ISNULL(DELETED,0) = 0 AND DEPOSIT_TYPE_NAME = @DEPOSIT_TYPE AND Practice_Code = @PRACTICE_CODE)                                                
SET @CATEGORY_ID = (SELECT TOP 1 CATEGORY_ID FROM FOX_TBL_RECONCILIATION_CATEGORY WHERE  ISNULL(DELETED,0) = 0 AND CATEGORY_NAME = @CATEGORY_ACCOUNT AND Practice_Code = @PRACTICE_CODE  AND ISNULL(DELETED,0) = 0)                                            

 
-- SET @SOFTPOSTEDAMOUNT = (SELECT TOP 1 CONVERT(MONEY,ISNULL(PAID_AMOUNT,0)) FROM  MIS_TBL_DWC_DEPOSITSLIP WHERE check_No = @CHECK_NO AND Practice_Code = @PRACTICE_CODE  AND ISNULL(DELETED,0) = 0 AND CONVERT(DATE, PAYMENT_DATE) = CONVERT(DATE, @DEPOSIT_D
ATE                  
IF(LOWER(@DEPOSIT_TYPE) = LOWER('comlbx') OR LOWER(@DEPOSIT_TYPE) = LOWER('eft'))        
BEGIN        
SET @SOFTPOSTEDAMOUNT = (SELECT TOP 1 CONVERT(MONEY,SUM(AMOUNT_POSTED))  FROM #SUMOFCHECK)          
SET @DATE_POSTED =  (SELECT TOP 1 POSTED_DATE  FROM #POSTEDDATE)          
END        
SET @RECONCILIATION_STATUS_ID = (SELECT TOP 1  RECONCILIATION_STATUS_ID FROM  FOX_TBL_RECONCILIATION_STATUS WHERE LOWER(STATUS_NAME) = 'unassigned' AND PRACTICE_CODE = @PRACTICE_CODE)                         
                      
 IF (@DATE_POSTED = '')                                                 
 BEGIN                                                          
 SET @DATE_POSTED = null                                                          
 END                      
 IF CHARINDEX('(',@AMOUNT) > 0                                                  
 BEGIN                                                          
 SET @AMOUNT = REPLACE(@AMOUNT,'(', '-')                                                          
 END                              
  IF CHARINDEX(')',@AMOUNT) > 0                                                  
 BEGIN                                                          
 SET @AMOUNT = REPLACE(@AMOUNT,')', '')                                                          
 END                              
 IF CHARINDEX('(',@AMOUNT_NOT_POSTED) > 0                                                  
 BEGIN                                                          
 SET @AMOUNT_NOT_POSTED = REPLACE(@AMOUNT_NOT_POSTED,'(', '-')                   
 END                              
  IF CHARINDEX(')',@AMOUNT_NOT_POSTED) > 0                                                  
 BEGIN                                                         
 SET @AMOUNT_NOT_POSTED = REPLACE(@AMOUNT_NOT_POSTED,')', '')                                                          
 END                        
     IF (                      
  (                      
  SELECT COUNT(*)                      
  FROM FOX_TBL_RECONCILIATION_CP                       
  WHERE ISNULL(DELETED, 0) = 0 AND PRACTICE_CODE = @PRACTICE_CODE                       
  AND CHECK_NO = @CHECK_NO                       
  AND CONVERT(DATE, DEPOSIT_DATE) = CONVERT(DATE, @DEPOSIT_DATE)                      
  AND CATEGORY_ID = @CATEGORY_ID                      
  AND ISNULL(DEPOSIT_TYPE_ID,0) = ISNULL(@DEPOSIT_TYPE_ID,0)                       
  AND AMOUNT = @AMOUNT                      
  AND LOWER(FOX_TBL_INSURANCE_NAME) = LOWER(@FOX_TBL_INSURANCE_NAME)                      
  --AND AMOUNT_NOT_POSTED = @AMOUNT_NOT_POSTED                      
  AND LOWER(ASSIGNED_GROUP) = LOWER(@ASSIGNED_TO)                      
  AND AMOUNT_POSTED = ISNULL(@SOFTPOSTEDAMOUNT,0.00)                       
  --AND CONVERT(DATE, DATE_POSTED) = CONVERT(DATE, @DATE_POSTED)                      
  AND CONVERT(DATE, ASSIGNED_GROUP_DATE) = CONVERT(DATE, @ASSIGNED_DATE)                      
  )                       
  > 0)                                                
     BEGIN                                                
     SET @RECORD_EXISTED = @RECORD_EXISTED + 1;                                                 
     END                                                
     ELSE                                                
     BEGIN                                               
                           
                             
                              
INSERT INTO FOX_TBL_RECONCILIATION_CP                       
(RECONCILIATION_CP_ID,                      
PRACTICE_CODE,                      
DEPOSIT_DATE,                      
DEPOSIT_TYPE_ID,                      
CATEGORY_ID,                      
FOX_TBL_INSURANCE_NAME,                      
CHECK_NO,                      
AMOUNT,                      
AMOUNT_POSTED,                      
--AMOUNT_NOT_POSTED,                      
RECONCILIATION_STATUS_ID,                      
ASSIGNED_TO,                      
ASSIGNED_DATE,                      
COMPLETED_DATE,                      
COMPLETED_BY,                      
LEDGER_NAME,                      
LEDGER_PATH,                      
TOTAL_LEDGER_PAGES,                      
CREATED_BY,                      
CREATED_DATE,                      
MODIFIED_BY,                      
MODIFIED_DATE,                      
DELETED,                      
REASON,                      
DATE_POSTED,                      
REMARKS,                      
BATCH_NO,                      
ASSIGNED_GROUP,                      
ASSIGNED_GROUP_DATE,                      
IS_RECONCILIED,              
[STATE])                      
VALUES                      
(                      
@RECONCILIATION_CP_ID,                      
@PRACTICE_CODE,                      
CONVERT(DATE,@DEPOSIT_DATE),                      
@DEPOSIT_TYPE_ID,                      
@CATEGORY_ID,                      
@FOX_TBL_INSURANCE_NAME,                      
@CHECK_NO,                      
ISNULL(CONVERT(MONEY,@AMOUNT),0.00),                  
ISNULL(@SOFTPOSTEDAMOUNT,0.00),                      
 --ISNULL(CONVERT(MONEY,@AMOUNT_NOT_POSTED),0.00),                       
@RECONCILIATION_STATUS_ID,                      
null,  
null,                      
null,                      
null,                      
null,                      
null,                      
null,                      
@USER_NAME,                      
GETDATE(),                      
@USER_NAME,                      
GETDATE(),                      
0,                      
null,                      
cast((case when isnull(@DATE_POSTED,'1900-01-01 00:00:00.000') = '1900-01-01 00:00:00.000' then null else @DATE_POSTED end) as datetime),                      
null,                      
@BATCH_NO,                      
@ASSIGNED_TO,                      
CONVERT(DATE,@ASSIGNED_DATE),                      
0,              
@STATE)                      
                      
SET @RECORD_INSERTED = @RECORD_INSERTED + 1;                                           
   END                                                
                                                
  IF (@RECORD_INSERTED > 0)                                                
  BEGIN                                                
              SELECT CAST(RECONCILIATION_CP_ID AS VARCHAR) FROM FOX_TBL_RECONCILIATION_CP WHERE RECONCILIATION_CP_ID = @RECONCILIATION_CP_ID                  
  END                          
  ELSE                      
   BEGIN                       
   SELECT 'ERROR'                      
   END                      
                                                
END 