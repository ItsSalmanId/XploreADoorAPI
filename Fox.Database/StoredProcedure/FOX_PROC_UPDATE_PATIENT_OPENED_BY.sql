IF (OBJECT_ID('FOX_PROC_UPDATE_PATIENT_OPENED_BY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_PATIENT_OPENED_BY  
GO 
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================      
CREATE PROCEDURE FOX_PROC_UPDATE_PATIENT_OPENED_BY      
 -- Add the parameters for the stored procedure here      
 @Patient_Account BIGINT,      
 @Guest_Patient_Account BIGINT,      
 @USER_NAME VARCHAR(70)      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
   
 SET NOCOUNT ON;      
       
     
 IF EXISTS(SELECT top 1 * FROM FOX_TBL_PATIENT WHERE Patient_Account = @Patient_Account AND ISNULL(DELETED,0) = 0)      
 BEGIN      
  DECLARE @PATIENT_ID BIGINT       
  SET @PATIENT_ID = (SELECT TOP 1 FOX_TBL_PATIENT_ID FROM FOX_TBL_PATIENT WHERE Patient_Account = @Patient_Account AND ISNULL(DELETED,0) = 0)      
  UPDATE FOX_TBL_PATIENT      
  SET IS_OPENED_BY = NULL,      
   Modified_By = @USER_NAME,      
   Modified_Date = GETDATE()     
  WHERE FOX_TBL_PATIENT_ID = @PATIENT_ID      
      
 END      
      
 SELECT TOP 1 * FROM Patient WHERE Patient_Account = @Guest_Patient_Account AND ISNULL(DELETED, 0) = 0      
END   
