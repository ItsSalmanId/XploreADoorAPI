IF (OBJECT_ID('FOX_PROC_GET_SMART_USERS_OF_REGION_VISIBLE_TO') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_USERS_OF_REGION_VISIBLE_TO  
GO 
create PROCEDURE [FOX_PROC_GET_SMART_USERS_OF_REGION_VISIBLE_TO]              
 @PRACTICE_CODE BIGINT,              
 @SEARCH_TEXT   VARCHAR(50)                   
AS                    
     BEGIN               
  IF OBJECT_ID('tempdb..#TEMP_ROLE') IS NOT NULL  DROP TABLE #TEMP_ROLE              
              
       SELECT ROLE_ID               
    INTO #TEMP_ROLE              
    FROM FOX_TBL_ROLE               
    WHERE RTRIM(LTRIM(ROLE_NAME)) IN ('Regional Director', 'Senior Regional Director','Vice President', 'Regional QA Liaison', 'Regional QA Liaisons', 'Regional Quality Assurance Liaison', 'Regional Quality Assurance Liaisons', 'Senior Regional QA Liaison

  
    
      
', 'Senior Regional Quality Assurance Liaison',         
 'Area Sales Director', 'Practice Liaison')              
    AND PRACTICE_CODE = @PRACTICE_CODE              
            
         SELECT *      , UR.ROLE_NAME              
         FROM FOX_TBL_APPLICATION_USER U              
         INNER JOIN #TEMP_ROLE R ON R.ROLE_ID = U.ROLE_ID               
  LEFT JOIN FOX_TBL_ROLE UR ON UR.ROLE_ID = U.ROLE_ID                       
         WHERE U.PRACTICE_CODE = @PRACTICE_CODE         
           
   --AND ( U.USER_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.LAST_NAME+''+U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.FIRST_NAME+''+U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.LAST_NAME+' '+U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.FIRST_NAME+' '+U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.LAST_NAME+','+U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.FIRST_NAME+','+U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.LAST_NAME+', '+U.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                    
               --OR U.FIRST_NAME+', '+U.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%')                    
                                  
              AND ISNULL(U.DELETED,0) = 0                  
     AND U.ROLE_ID IN (SELECT ROLE_ID FROM #TEMP_ROLE)              
     AND U.IS_ACTIVE=1           
     ORDER BY U.LAST_NAME, U.FIRST_NAME                     
     END; 