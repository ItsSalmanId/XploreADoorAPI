IF (OBJECT_ID('FOX_PROC_GET_DEPOSITSLIP_CLAIMS_AMOUNT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_DEPOSITSLIP_CLAIMS_AMOUNT 
GO       
 -- =============================================                                        
-- Author:  <Muhammad Arqam>                                        
-- Create date: <09/24/2020>                                        
-- Description: <Get Deposit Slip Amount>                                        
-- =============================================                  
                
-- [FOX_PROC_GET_DEPOSITSLIP_CLAIMS_AMOUNT_RAFAY]  1012714, 12095953 ,'12/31/2021'           
      
CREATE PROCEDURE [dbo].[FOX_PROC_GET_DEPOSITSLIP_CLAIMS_AMOUNT]                                                           
(                                                                  
 @STRPRACTICECODE     VARCHAR(20) = NULL,                                                                  
 @CHECK_NO            VARCHAR(50) = NULL,      
 @DEPOSIT_DATE    VARCHAR(50) = NULL      
)                                                                  
AS                                                                
BEGIN                          
SET NOCOUNT ON;                          
 SELECT ISNULL(CLP.AMOUNT_PAID, 0) AS AMOUNT_POSTED , CONVERT(VARCHAR(10), CLP.Created_Date, 101) AS POSTED_DATE                     
 FROM MIS_TBL_DWC_DEPOSITSLIP DS                                             
  LEFT JOIN CLAIM_PAYMENTS CLP ON  CLP.Check_No = DS.Check_No                                          
  LEFT JOIN INSURANCES I ON  CLP.INSURANCE_ID = I.INSURANCE_ID AND ISNULL(I.DELETED, 0) = '0'                                                            
  LEFT JOIN INSURANCE_PAYERS IP ON  I.INSPAYER_ID = IP.INSPAYER_ID AND ISNULL(IP.DELETED, 0) = '0'                                                            
  JOIN CLAIMS C ON  C.CLAIM_NO = CLP.CLAIM_NO                                                      
  JOIN PATIENT P ON  P.PATIENT_ACCOUNT = C.PATIENT_ACCOUNT   AND ISNULL(P.DELETED, 0) = '0'                                                             
  WHERE  p.PRACTICE_CODE = @STRPRACTICECODE                                                            
        AND DS.check_No =@CHECK_NO   
  AND DS.PRACTICE_CODE = @STRPRACTICECODE  
        AND ISNULL(CLP.DELETED, 0) = '0'                    
  AND  CONVERT(DATE,CLP.Created_Date) >= DATEADD(day,-30, @DEPOSIT_DATE)                     
END 
