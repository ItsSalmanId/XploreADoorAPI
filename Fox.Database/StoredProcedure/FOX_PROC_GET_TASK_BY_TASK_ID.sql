IF (OBJECT_ID('FOX_PROC_GET_TASK_BY_TASK_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_BY_TASK_ID  
GO 
CREATE PROCEDURE [DBO].[FOX_PROC_GET_TASK_BY_TASK_ID]        
(@PRACTICE_CODE BIGINT,           
 @TASK_ID       BIGINT,           
 @TASK_TYPE_ID  INT,           
 @IS_TEMPLATE   BIT          
)          
AS          
     BEGIN          
         SELECT T.*, TT.NAME AS TASK_TYPE, TT.CATEGORY_CODE,        
                (AU_C.LAST_NAME+', '+AU_C.FIRST_NAME) AS CREATED_BY_FULL_NAME,        
    (AU_C1.LAST_NAME+', '+AU_C1.FIRST_NAME) AS MODIFIED_BY_FULL_NAME,         
                CASE                 
     WHEN ISNULL(AL.CODE, '') = '' THEN ISNULL(AL.NAME, '')      
     ELSE ISNULL(AL.CODE, '')  + ' - ' + ISNULL(AL.NAME, '')       
  END AS LOCATION_NAME,      
                AL.CODE AS LOCATION_CODE,        
    P.LAST_NAME + ', ' + P.FIRST_NAME +        
    (        
     CASE        
      WHEN ISNULL(P.INDIVIDUAL_NPI, '') <> '' THEN ' | NPI: ' + P.INDIVIDUAL_NPI        
      ELSE ''        
     END        
    ) AS PROVIDER_FULL_NAME,        
                (AU_TD.LAST_NAME+', '+AU_TD.FIRST_NAME) AS TEMP_DELETED_BY_FULL_NAME,          
    CASE          
       WHEN T.IS_SEND_TO_USER = 1          
       THEN CASE WHEN isnull(st_usr.RT_USER_ID, '') <> '' THEN st_usr.RT_USER_ID + ' - '  ELSE '' END      
   +(CASE          
            WHEN isnull(st_usr.FIRST_NAME, '') <> ''  AND    isnull(st_usr.LAST_NAME, '') <> ''      
            THEN st_usr.LAST_NAME + ', '+st_usr.FIRST_NAME          
            ELSE ''          
           END)          
       ELSE         
          UG_S.GROUP_NAME          
      END as SEND_TO_NAME          
      ,          
      CASE           
  WHEN T.IS_FINAL_ROUTE_USER = 1          
     THEN CASE WHEN isnull(fr_usr.RT_USER_ID, '') <> '' THEN fr_usr.RT_USER_ID + ' - '  ELSE '' END      
   +(CASE          
            WHEN isnull(fr_usr.FIRST_NAME, '') <> ''  AND    isnull(fr_usr.LAST_NAME, '') <> ''      
            THEN fr_usr.LAST_NAME + ', '+fr_usr.FIRST_NAME          
            ELSE ''          
           END)          
       ELSE         
             UG_F.GROUP_NAME        
      END as FINAL_ROUTE_NAME          
         FROM FOX_TBL_TASK T          
              LEFT JOIN FOX_TBL_TASK_TYPE TT ON T.TASK_TYPE_ID = TT.TASK_TYPE_ID          
              LEFT JOIN FOX_TBL_GROUP UG_S ON T.SEND_TO_ID = UG_S.GROUP_ID          
              LEFT JOIN FOX_TBL_GROUP UG_F ON T.FINAL_ROUTE_ID = UG_F.GROUP_ID          
              LEFT JOIN FOX_TBL_APPLICATION_USER AU_C ON T.CREATED_BY = AU_C.USER_NAME      
      LEFT JOIN FOX_TBL_APPLICATION_USER AU_C1 ON T.MODIFIED_BY = AU_C1.USER_NAME            
              LEFT JOIN FOX_TBL_APPLICATION_USER AU_TD ON T.TEMPORARY_DELETED_BY = AU_TD.USER_NAME            
     LEFT JOIN FOX_TBL_PROVIDER P ON T.PROVIDER_ID = P.FOX_PROVIDER_ID          
              LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS AL ON T.LOC_ID = AL.LOC_ID          
     LEFT JOIN FOX_TBL_APPLICATION_USER st_usr ON T.SEND_TO_ID= st_usr.USER_ID          
    LEFT JOIN FOX_TBL_APPLICATION_USER fr_usr ON T.FINAL_ROUTE_ID= fr_usr.USER_ID          
         WHERE ISNULL(T.DELETED, 0) = 0          
               AND ISNULL(T.IS_TEMPLATE, 0) = @IS_TEMPLATE          
               AND T.PRACTICE_CODE = @PRACTICE_CODE          
               AND (@TASK_ID = -1          
                    OR T.TASK_ID = @TASK_ID)          
               AND (@TASK_TYPE_ID = -1          
                    OR T.TASK_TYPE_ID = @TASK_TYPE_ID);          
     END;  
