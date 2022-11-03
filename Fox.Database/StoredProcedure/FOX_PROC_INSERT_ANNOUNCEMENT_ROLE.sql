-- AUTHOR:  <MUHAMMAD SALMAN>      
-- CREATE DATE: <CREATE DATE, 09/26/2022>      
-- DESCRIPTION: <Insert Announcement Roles DETAILS>             
CREATE PROCEDURE [dbo].[FOX_PROC_INSERT_ANNOUNCEMENT_ROLE]            
 @ANNOUNCEMENT_ROLE_ID BIGINT,            
 @ROLE_ID VARCHAR(500),          
 @ROLE_NAME VARCHAR(50),        
 @ANNOUNCEMENT_ID BIGINT,              
 @PRACTICE_CODE BIGINT                
AS                        
BEGIN                     
 INSERT INTO FOX_TBL_ANNOUNCEMENT_ROLE (                    
         ANNOUNCEMENT_ROLE_ID,                    
         ROLE_ID,          
         ROLE_NAME,                    
         ANNOUNCEMENT_ID,                                             
         PRACTICE_CODE    
         )                    
       VALUES (                    
         @ANNOUNCEMENT_ROLE_ID,    
         @ROLE_ID,             
         @ROLE_NAME,               
         @ANNOUNCEMENT_ID,                                                
         @PRACTICE_CODE                                                     
       )                             
END 