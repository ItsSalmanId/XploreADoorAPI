IF (OBJECT_ID('FOX_PROC_GET_USERS_FOR_RECONCILIATION') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_USERS_FOR_RECONCILIATION  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_USERS_FOR_RECONCILIATION]-- 1011163, '1163testing', 103            
(@PRACTICE_CODE BIGINT,         
 @CURRENT_USER  VARCHAR(225),         
 @ROLE_ID       BIGINT        
)        
AS        
     BEGIN        
         --IF(@ROLE_ID = 103)--show admin all users for admin's practice        
         --BEGIN        
         SELECT USER_NAME,         
                UPPER(FIRST_NAME+' - '+r.ROLE_NAME) FIRST_NAME,         
                UPPER(LAST_NAME) LAST_NAME,         
                UPPER(r.ROLE_NAME) ROLE_NAME,         
                r.ROLE_ID ROLE_ID        
         FROM FOX_TBL_APPLICATION_USER au        
              INNER JOIN FOX_TBL_ROLE r ON au.ROLE_ID = r.ROLE_ID        
         WHERE au.PRACTICE_CODE = @PRACTICE_CODE        
               AND au.ROLE_ID IN(select ROLE_ID from FOX_TBL_ROLE where lower(ROLE_NAME) = 'agent' )        
              AND IS_ACTIVE = 1        
              AND USER_NAME NOT LIKE @CURRENT_USER        
              AND ISNULL(AU.DELETED,0) = 0    
     ORDER BY 1;        
         --END;        
         --ELSE        
         --BEGIN        
         --    SELECT USER_NAME,         
         --           UPPER(FIRST_NAME+' - '+r.ROLE_NAME) FIRST_NAME,         
         --           UPPER(LAST_NAME) LAST_NAME,         
         --           UPPER(r.ROLE_NAME) ROLE_NAME,         
         --           r.ROLE_ID ROLE_ID        
         --    FROM FOX_TBL_APPLICATION_USER au        
         --         INNER JOIN FOX_TBL_ROLE r ON au.ROLE_ID = r.ROLE_ID        
         --    WHERE au.PRACTICE_CODE = @PRACTICE_CODE        
         --         --AND AU.ROLE_ID IN(100, 102)        
         --         AND IS_ACTIVE = 1        
         --         AND USER_NAME NOT LIKE @CURRENT_USER        
         --         AND AU.DELETED = 0;        
         --END;        
     END;   
