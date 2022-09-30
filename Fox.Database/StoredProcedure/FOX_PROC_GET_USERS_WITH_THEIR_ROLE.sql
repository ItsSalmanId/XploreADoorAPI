IF (OBJECT_ID('FOX_PROC_GET_USERS_WITH_THEIR_ROLE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USERS_WITH_THEIR_ROLE  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_USERS_WITH_THEIR_ROLE]       
@PRACTICE_CODE BIGINT NULL      
--@SEARCH_TEXT VARCHAR(30)      
AS      
     BEGIN      
         SELECT U.USER_ID,       
				U.FIRST_NAME,      
				U.LAST_NAME,      
                U.USER_NAME,       
                R.ROLE_ID,       
                R.ROLE_NAME      
         FROM FOX_TBL_APPLICATION_USER U      
              INNER JOIN FOX_TBL_ROLE R ON U.ROLE_ID = R.ROLE_ID      
                                           AND R.DELETED = 0      
                                           AND (R.PRACTICE_CODE = @PRACTICE_CODE OR R.PRACTICE_CODE IS NULL)      
         WHERE       
   --R.ROLE_ID < 200      
   --            AND       
      U.PRACTICE_CODE = @PRACTICE_CODE      
      AND U.DELETED = 0      
            
      -- U.FIRST_NAME LIKE '%' + @SEARCH_TEXT + '%'          
      --OR U.LAST_NAME LIKE '%' + @SEARCH_TEXT + '%'       
      --OR U.USER_ID LIKE '%' + @SEARCH_TEXT + '%'      
            
            
         ORDER BY R.ROLE_NAME ASC,U.LAST_NAME ;      
   --ORDER BY U.LAST_NAME ASC;      
      
     END;    
