IF (OBJECT_ID('FOX_PROC_GET_USERS_DETAILS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USERS_DETAILS  
GO   
-------------------------------------------------------------------------------------------------- FOX_PROC_GET_USERS_DETAILS ---------------------------------------------------------------------------------------------  
---- =============================================                                      
---- Author:  <Muhammad Arslan Tufail>                                      
---- Create date: <12/09/2020>                                      
---- Description: <Get User Details>                                      
---- =============================================                 
 -- EXEC [FOX_PROC_GET_USERS_DETAILS] 'ADMIN992@ADMIN.COM'              
CREATE PROCEDURE [dbo].[FOX_PROC_GET_USERS_DETAILS]                     
@EMAIL VARCHAR(100)           
AS                      
BEGIN       
IF EXISTS(SELECT TOP 1* FROM USERS WHERE Email = @EMAIL AND ISNULL(Is_Active,0)=1)       
BEGIN                    
SELECT DISTINCT U.User_Id as InternalUserID, U.User_FName, U.User_LName, U.Email        
FROM USERS AS U         
WHERE ISNULL(Is_Active,0)=1      
AND Email = @EMAIL      
END      
ELSE      
BEGIN      
SELECT DISTINCT TAU.USER_ID , TAU.FIRST_NAME, TAU.LAST_NAME, TAU.EMAIL      
FROM FOX_TBL_APPLICATION_USER AS TAU      
WHERE EMAIL = @EMAIL      
AND ISNULL(DELETED,0)=0         
END      
END; 