IF (OBJECT_ID('FOX_PROC_GET_SPECIFIC_ROLE_BY_ROLE_NAME') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SPECIFIC_ROLE_BY_ROLE_NAME  
GO 
CREATE PROCEDURE [FOX_PROC_GET_SPECIFIC_ROLE_BY_ROLE_NAME] @PRACTICE_CODE BIGINT,                    
                                                                     @ROLE_NAME     VARCHAR(200)                    
AS                    
     BEGIN                    
         SELECT *, U.FIRST_NAME, U.LAST_NAME,R.ROLE_NAME                    
         FROM FOX_TBL_APPLICATION_USER U                    
              INNER JOIN dbo.FOX_TBL_ROLE R ON U.ROLE_ID = R.ROLE_ID                    
                                               AND R.ROLE_NAME = @ROLE_NAME                    
         WHERE  U.PRACTICE_CODE = @PRACTICE_CODE                    
               AND ISNULL(U.DELETED,0) = 0                  
   AND U.IS_ACTIVE=1                  
       ORDER BY U.LAST_NAME, U.FIRST_NAME                   
     END; 