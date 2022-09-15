IF (OBJECT_ID('FOX_PROC_GET_PATIENT_POS_BY_PA_FOR_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_POS_BY_PA_FOR_SERVICE  
GO         
-- =============================================              
-- Author:  <Abdul Sattar>              
-- Create date: <05/03/2021>              
-- Description: <Description,Get patient POS by patient acccount>              
-- =============================================              
--EXEC FOX_PROC_GET_PATIENT_POS_BY_PA_FOR_SERVICE  605102       
CREATE PROCEDURE FOX_PROC_GET_PATIENT_POS_BY_PA_FOR_SERVICE      
 @PATIENT_ACCOUNT  BIGINT        
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
  SELECT top 1 Loc_ID,Patient_POS_ID      
  FROM Fox_Tbl_Patient_POS      
  where       
  Patient_Account = @PATIENT_ACCOUNT      
  and isnull(Deleted,0)=0      
  and isnull(Is_Default,0)=1      
  order by Modified_Date desc      
END      
