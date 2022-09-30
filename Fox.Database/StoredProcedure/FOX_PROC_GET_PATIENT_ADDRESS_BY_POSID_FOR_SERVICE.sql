IF (OBJECT_ID('FOX_PROC_GET_PATIENT_ADDRESS_BY_POSID_FOR_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_ADDRESS_BY_POSID_FOR_SERVICE  
GO   
-- =============================================              
-- Author:  <Abdul Sattar>              
-- Create date: <05/03/2021>              
-- Description: <Description,Get patient address by PATIENT_POS_ID and PATIENT_ACCOUNT>              
-- =============================================              
--EXEC FOX_PROC_GET_PATIENT_ADDRESS_BY_POSID_FOR_SERVICE  605102       
CREATE PROCEDURE FOX_PROC_GET_PATIENT_ADDRESS_BY_POSID_FOR_SERVICE      
 @PATIENT_ACCOUNT  BIGINT       
 ,@PATIENT_POS_ID BIGINT       
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
  SELECT ZIP      
  FROM FOX_TBL_PATIENT_ADDRESS      
  where       
  PATIENT_ACCOUNT = @PATIENT_ACCOUNT      
  and PATIENT_POS_ID = @PATIENT_POS_ID      
  and isnull(Deleted,0)=0      
END  
