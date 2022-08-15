IF (OBJECT_ID('FOX_PROC_CHECK_DUPLICATE_PATIENT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_CHECK_DUPLICATE_PATIENT
GO
-- =============================================              
-- Author:  <Muhammad, Nouman>              
-- Create date: <04/10/2020>              
-- Description: <Select the patient if patient is already have activated phr>              
-- ============================================= 
CREATE PROCEDURE FOX_PROC_CHECK_DUPLICATE_PATIENT              
 -- Add the parameters for the stored procedure here              
 @Practice_Code bigint,            
 @Patient_Account  bigint            
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
               
              
    -- Insert statements for procedure here              
 Select top 1 * from [DBO].[Patient]              
 where Practice_code  = @Practice_Code              
 And Patient_Account = @Patient_Account              
 And isnull(Deleted,0) = 0              
              
               
END   
