IF (OBJECT_ID('FOX_PROC_GET_TASK_BY_TASK_TYPE_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_BY_TASK_TYPE_ID  
GO 
CREATE PROCEDURE [DBO].[FOX_PROC_GET_TASK_BY_TASK_TYPE_ID]        
(@PRACTICE_CODE BIGINT,           
 @TASK_ID       BIGINT,           
 @TASK_TYPE_ID  INT,           
 @IS_TEMPLATE   BIT          
)          
AS          
     BEGIN          
         SELECT T.*, TT.CATEGORY_CODE,        
                (AU_C.LAST_NAME+', '+AU_C.FIRST_NAME) AS CREATED_BY_FULL_NAME,     
    CASE                 
     WHEN ISNULL(AL.CODE, '') = '' THEN ISNULL(AL.NAME, '')      
     ELSE ISNULL(AL.CODE, '')  + ' - ' + ISNULL(AL.NAME, '')       
  END AS LOCATION_NAME,          
                --AL.NAME AS LOCATION_NAME,           
                AL.CODE AS LOCATION_CODE,           
                (RS.LAST_NAME+', '+RS.FIRST_NAME) AS PROVIDER_FULL_NAME,           
                (AU_TD.LAST_NAME+', '+AU_TD.FIRST_NAME) AS TEMP_DELETED_BY_FULL_NAME,          
      CASE          
       WHEN T.IS_SEND_TO_USER = 1          
       THEN st_usr.LAST_NAME+(CASE          
            WHEN isnull(st_usr.FIRST_NAME, '') <> ''          
            THEN ', '+st_usr.FIRST_NAME          
            ELSE ''          
           END)          
       ELSE         
          G.GROUP_NAME          
      END as SEND_TO_NAME          
      ,          
      CASE           
  WHEN T.IS_FINAL_ROUTE_USER = 1          
         THEN fr_usr.LAST_NAME+(        
   CASE          
              WHEN isnull(fr_usr.FIRST_NAME, '') <> ''          
              THEN ', '+fr_usr.FIRST_NAME          
              ELSE ''          
             END)          
         ELSE          
             G1.GROUP_NAME        
      END as FINAL_ROUTE_NAME          
                
         FROM FOX_TBL_TASK T          
              LEFT JOIN FOX_TBL_TASK_TYPE TT ON T.TASK_TYPE_ID = TT.TASK_TYPE_ID          
              LEFT JOIN FOX_TBL_GROUP G ON T.SEND_TO_ID = G.GROUP_ID          
              LEFT JOIN FOX_TBL_GROUP G1 ON T.FINAL_ROUTE_ID = G1.GROUP_ID          
              LEFT JOIN FOX_TBL_APPLICATION_USER AU_C ON T.CREATED_BY = AU_C.USER_NAME          
              LEFT JOIN FOX_TBL_APPLICATION_USER AU_TD ON T.TEMPORARY_DELETED_BY = AU_TD.USER_NAME          
              LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE RS ON T.PROVIDER_ID = RS.SOURCE_ID          
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
