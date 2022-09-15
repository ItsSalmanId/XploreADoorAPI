IF (OBJECT_ID('FOX_SP_Get_Patient_Invite_Status') IS NOT NULL ) DROP PROCEDURE FOX_SP_Get_Patient_Invite_Status  
GO 
/****** Object:  StoredProcedure [dbo].[FOX_SP_Get_Patient_Invite_Status]    Script Date: 12/25/2018 8:50:23 PM ******/  
-- =============================================  
-- Author:  <Imran, Muhammad>  
-- Create date: <12/20/2018>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[FOX_SP_Get_Patient_Invite_Status]  
 -- Add the parameters for the stored procedure here  
 @Patient_Account bigint  
 ,@Practice_Code bigint  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
   
  
    -- Insert statements for procedure here  
 Select top 1 * from [DBO].[WS_FOX_TBL_PHR_USERS]  
 where Practice_code  = @Practice_Code  
 And Patient_Account = @Patient_Account  
 And Deleted = 0  
  
   
END  
