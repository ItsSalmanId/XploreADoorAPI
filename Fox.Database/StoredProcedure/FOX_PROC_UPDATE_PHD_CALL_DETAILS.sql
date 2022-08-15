IF (OBJECT_ID('FOX_PROC_UPDATE_PHD_CALL_DETAILS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_PHD_CALL_DETAILS  
GO 
-- Author:  <Muhammad, Nouman>                
-- Create date: <05/19/2020>                
-- Description: <Update the patient account for not associated pateint on RFO generation>                
-- =============================================                
CREATE PROCEDURE FOX_PROC_UPDATE_PHD_CALL_DETAILS                
 -- Add the parameters for the stored procedure here                
 @Practice_Code bigint,         
 @FOX_TBL_PHD_CALL_DETAIL_ID bigint,        
 @Patient_Account  bigint        
         
AS                
BEGIN                
 -- SET NOCOUNT ON added to prevent extra result sets from                
 -- interfering with SELECT statements.                
                 
                
   UPDATE [DBO].FOX_TBL_PHD_CALL_DETAILS           
   SET PATIENT_ACCOUNT = @Patient_Account        
   WHERE PRACTICE_CODE = @Practice_Code AND FOX_PHD_CALL_DETAILS_ID =  @FOX_TBL_PHD_CALL_DETAIL_ID         
                
                 
END 