-- AUTHOR:  <MUHAMMAD SALMAN>                                                                                            
-- CREATE DATE: <CREATE DATE, 12/15/2022>                                                                                            
-- DESCRIPTION: <GET LIST OF PATIENT DETAILS>                                                                                      
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INSURANCE_DETAILS]                                                                                                         
(                                                                                                                       
  @PRACTICE_CODE BIGINT                                                                                                                                                  
)                                                                                                              
AS                                                                                                              
BEGIN                                                  
    
SelecT FOX_TBL_INSURANCE_ID AS FoxTblInsurance_Id,INSURANCE_PAYERS_ID As InsurancePayersId,INSURANCE_NAME As InsuranceName  From FOX_TBL_INSURANCE WITH (NOLOCK) where PRACTICE_CODE = @PRACTICE_CODE and DELETED = 0  order by FOX_TBL_INSURANCE_ID ASC                     
              
END 