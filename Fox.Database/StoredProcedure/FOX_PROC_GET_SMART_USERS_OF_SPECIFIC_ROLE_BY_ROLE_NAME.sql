IF (OBJECT_ID('FOX_PROC_GET_SMART_USERS_OF_SPECIFIC_ROLE_BY_ROLE_NAME') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_USERS_OF_SPECIFIC_ROLE_BY_ROLE_NAME  
GO 
CREATE PROCEDURE [FOX_PROC_GET_SMART_USERS_OF_SPECIFIC_ROLE_BY_ROLE_NAME] @PRACTICE_CODE BIGINT,             
                                                                          @SEARCH_TEXT   VARCHAR(50),             
                                                                          @ROLE_NAME     VARCHAR(200)            
AS            
     BEGIN            
         SELECT *            
         FROM FOX_TBL_APPLICATION_USER U            
              INNER JOIN dbo.FOX_TBL_ROLE R ON U.ROLE_ID = R.ROLE_ID            
                                               AND R.ROLE_NAME = @ROLE_NAME            
         WHERE(U.USER_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.LAST_NAME+''+U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.FIRST_NAME+''+U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.LAST_NAME+' '+U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.FIRST_NAME+' '+U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.LAST_NAME+','+U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.FIRST_NAME+','+U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.LAST_NAME+', '+U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'            
               OR U.FIRST_NAME+', '+U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%')            
              AND U.PRACTICE_CODE = @PRACTICE_CODE            
              AND U.DELETED = 0          
   AND U.IS_ACTIVE=1          
     ;            
     END; 