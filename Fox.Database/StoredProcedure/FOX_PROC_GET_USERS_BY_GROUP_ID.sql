IF (OBJECT_ID('FOX_PROC_GET_USERS_BY_GROUP_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USERS_BY_GROUP_ID  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_USERS_BY_GROUP_ID] @PRACTICE_CODE BIGINT,           
                                                 @GROUP_ID      BIGINT          
AS          
     BEGIN          
         SELECT UG.GROUP_USER_ID,           
                UG.GROUP_ID,        
				U.FIRST_NAME,        
				U.LAST_NAME,           
                UG.USER_NAME,           
                R.ROLE_NAME,          
    R.ROLE_ID,          
    U.USER_ID          
         FROM FOX_TBL_USER_GROUP UG          
              INNER JOIN FOX_TBL_GROUP G ON UG.GROUP_ID = G.GROUP_ID          
                                            AND UG.DELETED = 0          
              INNER JOIN FOX_TBL_APPLICATION_USER U ON UG.USER_NAME = U.USER_NAME          
              INNER JOIN FOX_TBL_ROLE R ON R.ROLE_ID = U.ROLE_ID          
         WHERE UG.GROUP_ID = @GROUP_ID          
               AND G.DELETED = 0          
               AND G.PRACTICE_CODE = @PRACTICE_CODE;          
     END;        
        
        
  
