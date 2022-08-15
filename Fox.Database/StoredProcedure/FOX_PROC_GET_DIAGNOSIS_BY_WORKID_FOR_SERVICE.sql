IF (OBJECT_ID('FOX_PROC_GET_DIAGNOSIS_BY_WORKID_FOR_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_DIAGNOSIS_BY_WORKID_FOR_SERVICE 
GO         
-- =============================================              
-- Author:  <Abdul Sattar>              
-- Create date: <05/03/2021>              
-- Description: <Description,Get diagnosis by work ID,>              
-- =============================================              
--EXEC FOX_PROC_GET_DIAGNOSIS_BY_WORKID_FOR_SERVICE  53422414       
CREATE PROCEDURE FOX_PROC_GET_DIAGNOSIS_BY_WORKID_FOR_SERVICE      
 @WORK_ID  BIGINT        
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
  SELECT DIAG_CODE,DIAG_DESC FROM FOX_TBL_PATIENT_DIAGNOSIS      
  WHERE WORK_ID = @WORK_ID      
END      
