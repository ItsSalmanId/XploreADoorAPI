IF (OBJECT_ID('FOX_PROC_DELETE_NOTIFICATIONS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_DELETE_NOTIFICATIONS
GO 
-- =============================================                          
-- Author:  <Author,Abdur Rafey>                          
-- Create date: <Create Date,07/28/2021>                          
-- DESCRIPTION: <DELETE NOTIFICATIONS>        
      
-- EXEC FOX_PROC_DELETE_NOTIFICATIONS   5651352, 1011163, '07/19/2020', '07/26/2021'           
CREATE PROCEDURE [dbo].[FOX_PROC_DELETE_NOTIFICATIONS]          
@FOX_NOTIFICATION_ID BIGINT,      
@USER VARCHAR(100)      
AS                            
BEGIN              
      
UPDATE Fox_Tbl_Notifications      
SET DELETED = 1, MODIFIED_BY = @USER, MODIFIED_DATE = GETDATE()      
WHERE FOX_NOTIFICATION_ID = @FOX_NOTIFICATION_ID      
      
select * FROM Fox_Tbl_Notifications WHERE FOX_NOTIFICATION_ID = @FOX_NOTIFICATION_ID AND DELETED = 1      
END;   
