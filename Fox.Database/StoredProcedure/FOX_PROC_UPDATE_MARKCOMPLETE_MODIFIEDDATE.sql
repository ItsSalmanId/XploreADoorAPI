IF (OBJECT_ID('FOX_PROC_UPDATE_MARKCOMPLETE_MODIFIEDDATE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_MARKCOMPLETE_MODIFIEDDATE  
GO 
-- Author:  <Muhammad Arqam>              
-- Create date: <11/27/2019>              
-- Description: <Description,,>              
-- =============================================             
-- FOX_PROC_UPDATE_MARKCOMPLETE_MODIFIEDDATE  54812804,1011163,101116354813782,'1163TESTING'        
    
create PROCEDURE FOX_PROC_UPDATE_MARKCOMPLETE_MODIFIEDDATE              
 -- Add the parameters for the stored procedure here           
 @Work_ID BIGINT ,          
 @PRACTICE_CODE BIGINT,              
 @PATIENT_ACCOUNT BIGINT,          
 @USER_NAME VARCHAR(70)              
             
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
 SET NOCOUNT ON;              
               
    -- Insert statements for procedure here              
 IF EXISTS(SELECT top 1 * FROM FOX_TBL_INTERFACE_SYNCH  WHERE Work_ID = @Work_ID AND ISNULL(DELETED,0) = 0)              
 BEGIN              
  DECLARE @Interface_ID BIGINT               
  SET @Interface_ID = (SELECT TOP 1 FOX_INTERFACE_SYNCH_ID FROM FOX_TBL_INTERFACE_SYNCH WHERE Work_ID = @Work_ID AND ISNULL(DELETED,0) = 0 order by CREATED_DATE desc)              
  UPDATE FOX_TBL_INTERFACE_SYNCH              
  SET              
   Modified_By = @USER_NAME,              
   Modified_Date = GETDATE()             
  WHERE FOX_INTERFACE_SYNCH_ID = @Interface_ID              
              
 END              
              
 SELECT TOP 1 * FROM FOX_TBL_INTERFACE_SYNCH WHERE FOX_INTERFACE_SYNCH_ID = @Interface_ID             
END 