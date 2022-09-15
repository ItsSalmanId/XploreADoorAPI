IF (OBJECT_ID('FOX_PROC_GET_DEPOSITSLIP_CLAIMS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_DEPOSITSLIP_CLAIMS 
GO          
-- FOX_PROC_GET_DEPOSITSLIP_CLAIMS_RAFAY 1012714,Null,'5111',1,200Â                          
CREATE PROCEDURE [dbo].[FOX_PROC_GET_DEPOSITSLIP_CLAIMS]                                                     
(                                                              
 @STRPRACTICECODE     VARCHAR(20) = NULL,                                                              
 @STRDEPOSITID        VARCHAR(40) = NULL,                                           
 @CHECK_NO            VARCHAR(50) = NULL,                                     
 @CURRENT_PAGE        INT    = NULL,                                          
 @RECORD_PER_PAGE     INT         = NULL                                          
)                                                              
AS                                                              
BEGIN                            
SET NOCOUNT ON;                            
   SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                            
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                                                                                
 DECLARE @TOATL_PAGESUDM FLOAT              
 DECLARE @TOTAL_AMOUNT_POSTED FLOAT            
            
-- SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                                                                
 SELECT @TOATL_PAGESUDM = count(*) ,   @TOTAL_AMOUNT_POSTED = SUM(ISNULL(CLP.AMOUNT_PAID, 0))                                                            
 FROM MIS_TBL_DWC_DEPOSITSLIP DS                                               
  LEFT JOIN CLAIM_PAYMENTS CLP ON  CLP.Check_No = DS.Check_No                                            
  LEFT JOIN INSURANCES I ON  CLP.INSURANCE_ID = I.INSURANCE_ID AND ISNULL(I.DELETED, 0) = '0'                                                              
  LEFT JOIN INSURANCE_PAYERS IP ON  I.INSPAYER_ID = IP.INSPAYER_ID AND ISNULL(IP.DELETED, 0) = '0'                                                              
  JOIN CLAIMS C ON  C.CLAIM_NO = CLP.CLAIM_NO                                                        
  JOIN PATIENT P ON  P.PATIENT_ACCOUNT = C.PATIENT_ACCOUNT   AND ISNULL(P.DELETED, 0) = '0'                                                               
  WHERE  p.PRACTICE_CODE = @STRPRACTICECODE                                                              
        AND DS.check_No =@CHECK_NO         
  AND DS.PRACTICE_CODE = @STRPRACTICECODE    
        AND ISNULL(DS.DELETED, 0) = '0'        
  --AND  CONVERT(DATE,CLP.Created_Date) >= DATEADD(day,-15, '12/10/2021')        
                                                        
 --SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                                                 
  IF (@RECORD_PER_PAGE = 0)                                                                                
 BEGIN                                                                                
  SET @RECORD_PER_PAGE = @TOATL_PAGESUDM                                                                                
 END                                                                                
 ELSE                                                       
 BEGIN                                                                                
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                                                                                
 END                                   
              IF(@RECORD_PER_PAGE IS NULL OR @RECORD_PER_PAGE = 0)                                    
   BEGIN                                    
  SET @RECORD_PER_PAGE = 1                                    
   END                                                          
   SET @TOATL_PAGESUDM = CEILING(IsNull(@TOATL_PAGESUDM,0) / IsNull(@RECORD_PER_PAGE,1));                                                                              
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM                                                       
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)                                           
  Select @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES                                                                                
  ,@TOTAL_RECORDS TOTAL_RECORDS ,            
  @TOTAL_AMOUNT_POSTED AS TOTAL_AMOUNT_POSTED,* from (                                                            
Select ISNULL(CLP.AMOUNT_PAID, 0) AS AMOUNT_POSTED ,                               
ROW_NUMBER() OVER(ORDER BY DS.CHECK_NO DESC) AS ROW,                                                              
        convert(varchar,P.PATIENT_ACCOUNT) PATIENT_ACCOUNT,                                                              
        (P.Last_Name +',' + P.First_Name) AS PATIENT_NAME,                              
        ISNULL(CLP.CLAIM_NO,0) CLAIM_NO,                 
        IP.INSPAYER_DESCRIPTION,                                                              
        DS.CHECK_NO,                                          
  --ISNULL(CLP.AMOUNT_PAID,0) AS AMOUNT_POSTED,                             
  CONVERT(VARCHAR(10), CLP.DATE_FILING, 101) AS CHECK_DATE,                                    
   --(SELECT SUM(ISNULL(AMOUNT_PAID, 0)) FROM                                  
   --CLAIM_PAYMENTS CP                                  
   --LEFT JOIN MIS_TBL_DWC_DEPOSITSLIP D ON D.check_No = CP.Check_No                                  
   --WHERE CP.Check_No = @CHECK_NO) AS AMOUNT_POSTED                               
                     
   CONVERT(VARCHAR(10), CLP.Created_Date, 101) AS POSTED_DATE              
  FROM MIS_TBL_DWC_DEPOSITSLIP DS                                               
  LEFT JOIN CLAIM_PAYMENTS CLP ON  CLP.Check_No = DS.Check_No                                            
  LEFT JOIN INSURANCES I ON  CLP.INSURANCE_ID = I.INSURANCE_ID AND ISNULL(I.DELETED, 0) = '0'                                                              
  LEFT JOIN INSURANCE_PAYERS IP ON  I.INSPAYER_ID = IP.INSPAYER_ID AND ISNULL(IP.DELETED, 0) = '0'                                                              
  JOIN CLAIMS C ON  C.CLAIM_NO = CLP.CLAIM_NO                                                        
  JOIN PATIENT P ON  P.PATIENT_ACCOUNT = C.PATIENT_ACCOUNT   AND ISNULL(P.DELETED, 0) = '0'                                                               
  WHERE  p.PRACTICE_CODE = @STRPRACTICECODE                                                              
        AND DS.check_No =@CHECK_NO                                                              
        AND ISNULL(DS.DELETED, 0) = '0'         
  --AND  CONVERT(DATE,CLP.Created_Date) >= DATEADD(day,-15, '12/10/2021')        
            ) AS CLAIM_PAYMENT                                                                            
   ORDER BY Check_No DESC                                           
  OFFSET @START_FROM ROWS                                                                           
                                                                                
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                            
END 