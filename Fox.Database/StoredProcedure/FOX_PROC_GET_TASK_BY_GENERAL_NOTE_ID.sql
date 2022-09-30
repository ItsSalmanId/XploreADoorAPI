IF (OBJECT_ID('FOX_PROC_GET_TASK_BY_GENERAL_NOTE_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_BY_GENERAL_NOTE_ID  
GO 
CREATE PROCEDURE [DBO].[FOX_PROC_GET_TASK_BY_GENERAL_NOTE_ID]        
(@PRACTICE_CODE   BIGINT,         
 @GENERAL_NOTE_ID BIGINT,         
 @TASK_TYPE_ID    INT,         
 @IS_TEMPLATE     BIT        
)        
AS        
     BEGIN        
         SELECT T.*, TT.NAME AS TASK_TYPE, TT.CATEGORY_CODE,        
                (AU_C.LAST_NAME+', '+AU_C.FIRST_NAME) AS CREATED_BY_FULL_NAME,      
  CASE                   
   WHEN ISNULL(AL.CODE, '') = '' THEN ISNULL(AL.NAME, '')        
   ELSE ISNULL(AL.CODE, '')  + ' - ' + ISNULL(AL.NAME, '')         
  END AS LOCATION_NAME,         
                --AL.NAME AS LOCATION_NAME,         
                AL.CODE AS LOCATION_CODE,         
                (P.LAST_NAME+', '+P.FIRST_NAME) AS PROVIDER_FULL_NAME,         
                (AU_TD.LAST_NAME+', '+AU_TD.FIRST_NAME) AS TEMP_DELETED_BY_FULL_NAME,        
                CASE        
                    WHEN T.IS_SEND_TO_USER = 1        
                    THEN st_usr.LAST_NAME+(CASE        
                                               WHEN isnull(st_usr.FIRST_NAME, '') <> ''        
                                               THEN ', '+st_usr.FIRST_NAME        
                                               ELSE ''        
                                           END)        
                    ELSE GU_S.GROUP_NAME        
                END AS SEND_TO_NAME,        
                CASE        
                    WHEN T.IS_FINAL_ROUTE_USER = 1        
                    THEN fr_usr.LAST_NAME+(CASE        
                                               WHEN isnull(fr_usr.FIRST_NAME, '') <> ''        
                                               THEN ', '+fr_usr.FIRST_NAME        
                                               ELSE ''        
                                           END)        
                    ELSE GU_F.GROUP_NAME        
                END AS FINAL_ROUTE_NAME        
         FROM FOX_TBL_TASK T          
              LEFT JOIN FOX_TBL_TASK_TYPE TT ON T.TASK_TYPE_ID = TT.TASK_TYPE_ID        
              LEFT JOIN FOX_TBL_GROUP GU_S ON T.SEND_TO_ID = GU_S.GROUP_ID        
              LEFT JOIN FOX_TBL_GROUP GU_F ON T.FINAL_ROUTE_ID = GU_F.GROUP_ID        
              LEFT JOIN FOX_TBL_APPLICATION_USER AU_C ON T.CREATED_BY = AU_C.USER_NAME        
              LEFT JOIN FOX_TBL_APPLICATION_USER AU_TD ON T.TEMPORARY_DELETED_BY = AU_TD.USER_NAME            
     LEFT JOIN FOX_TBL_PROVIDER P ON T.PROVIDER_ID = P.FOX_PROVIDER_ID        
              LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS AL ON T.LOC_ID = AL.LOC_ID        
              LEFT JOIN FOX_TBL_APPLICATION_USER st_usr ON T.SEND_TO_ID = st_usr.USER_ID        
              LEFT JOIN FOX_TBL_APPLICATION_USER fr_usr ON T.FINAL_ROUTE_ID = fr_usr.USER_ID        
         WHERE ISNULL(T.DELETED, 0) = 0        
               AND ISNULL(T.IS_TEMPLATE, 0) = @IS_TEMPLATE        
               AND T.PRACTICE_CODE = @PRACTICE_CODE        
               AND (@GENERAL_NOTE_ID = -1        
                    OR T.GENERAL_NOTE_ID = @GENERAL_NOTE_ID)        
               AND (@TASK_TYPE_ID = -1        
                    OR T.TASK_TYPE_ID = @TASK_TYPE_ID);        
     END;        
