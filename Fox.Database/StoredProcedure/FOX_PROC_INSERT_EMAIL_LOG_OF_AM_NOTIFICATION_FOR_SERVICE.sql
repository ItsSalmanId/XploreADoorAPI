IF (OBJECT_ID('FOX_PROC_INSERT_EMAIL_LOG_OF_AM_NOTIFICATION_FOR_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_EMAIL_LOG_OF_AM_NOTIFICATION_FOR_SERVICE  
GO 
-- =============================================              
-- Author:  <Abdul Sattar>              
-- Create date: <05/03/2021>              
-- Description: <Description,Insert email log on sending notification to account manager>              
-- =============================================                    
CREATE PROCEDURE [dbo].[FOX_PROC_INSERT_EMAIL_LOG_OF_AM_NOTIFICATION_FOR_SERVICE]       
@FOX_EMAIL_FAX_LOG_ID bigint  
,@PRACTICE_CODE bigint  
,@FROM varchar(100)  
,@TO varchar(100)  
,@CC varchar(500)  
,@BCC varchar(500)  
,@ATTACHMENT_PATH varchar(500)  
,@STATUS varchar(100)  
,@EXCEPTION_SHORT_MSG varchar(1000)  
,@EXCEPTION_TRACE varchar(5000)  
,@WORK_ID bigint  
,@WORK_STATUS varchar(20)             
AS              
BEGIN              
 Insert into FOX_TBL_EMAIL_FAX_LOG    
(FOX_EMAIL_FAX_LOG_ID,PRACTICE_CODE,[TYPE],[FROM],[TO],CC,BCC,ATTACHMENT_PATH,[STATUS],EXCEPTION_SHORT_MSG,EXCEPTION_TRACE,WORK_ID,CREATED_BY,CREATED_DATE,MODIFIED_BY,MODIFIED_DATE,DELETED,WORK_STATUS)  
values(@FOX_EMAIL_FAX_LOG_ID,@PRACTICE_CODE,'Email',@FROM,@TO,@CC,@BCC,@ATTACHMENT_PATH,@STATUS,@EXCEPTION_SHORT_MSG,@EXCEPTION_TRACE,@WORK_ID,'FOX_SERVICE',getdate(),'FOX_SERVICE',getdate(),0,@WORK_STATUS)  
END 
