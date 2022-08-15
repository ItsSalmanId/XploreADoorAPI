IF (OBJECT_ID('FOX_PROC_GET_PHD_PATIENT_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PHD_PATIENT_LIST  
GO 
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----CREATED BY: ABDULSATTAR----
----------11/21/2018----------     
--TEST PARAMETERES: '101116354812820','', '', '', '','', '', '','', '1011163', 1, 0,'', 'ASC'  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PHD_PATIENT_LIST]    --'101116354812820','', '', '', '','', '', '','', '1011163', 1, 0,'', 'ASC' 
 (
 @PATIENT_ACCOUNT AS VARCHAR(100)
 ,@CHART_ID AS VARCHAR(100)
 ,@LAST_NAME AS VARCHAR(50)
 ,@FIRST_NAME AS VARCHAR(50)                       
 ,@SSN AS VARCHAR(9) 
 ,@DOB VARCHAR(50)
 ,@HOME_PHONE VARCHAR(10) 
 ,@WORK_PHONE VARCHAR(10)
 ,@CELL_PHONE VARCHAR (10)    
 ,@PRACTICE_CODE BIGINT        
 ,@CURRENT_PAGE INT        
 ,@RECORD_PER_PAGE INT       
 ,@SORT_BY VARCHAR(50)        
 ,@SORT_ORDER VARCHAR(5)                
 )        
AS        
BEGIN 
IF (@PATIENT_ACCOUNT = '')        
 BEGIN        
  SET @PATIENT_ACCOUNT = NULL        
 END        
 ELSE        
 BEGIN        
  SET @PATIENT_ACCOUNT = @PATIENT_ACCOUNT + '%'        
 END
         
IF (@CHART_ID = '')        
 BEGIN        
  SET @CHART_ID = NULL        
 END        
 ELSE        
 BEGIN        
  SET @CHART_ID = @CHART_ID + '%'        
 END  
    
  IF (@LAST_NAME = '')        
 BEGIN        
  SET @LAST_NAME = NULL        
 END        
 ELSE        
 BEGIN        
  SET @LAST_NAME = @LAST_NAME + '%'        
 END 
          
 IF (@FIRST_NAME = '')        
 BEGIN        
  SET @FIRST_NAME = NULL        
 END        
 ELSE        
 BEGIN        
  SET @FIRST_NAME = @FIRST_NAME + '%'        
 END        
              
 IF (@SSN = '')        
 BEGIN        
  SET @SSN = NULL        
 END        
 ELSE        
 BEGIN        
  SET @SSN = @SSN + '%'        
 END  
 
 IF (@DOB = '')        
 BEGIN        
  SET @DOB = NULL        
 END        
 ELSE        
 BEGIN        
  SET @DOB = @DOB + '%'        
 END   
 
 IF (@HOME_PHONE = '')        
 BEGIN        
  SET @HOME_PHONE = NULL        
 END        
 ELSE        
 BEGIN        
  SET @HOME_PHONE = @HOME_PHONE + '%'        
 END   
 
 IF (@WORK_PHONE = '')        
 BEGIN        
  SET @WORK_PHONE = NULL        
 END        
 ELSE        
 BEGIN        
  SET @WORK_PHONE = @WORK_PHONE + '%'        
 END   
 
 IF (@CELL_PHONE = '')        
 BEGIN        
  SET @CELL_PHONE = NULL        
 END        
 ELSE        
 BEGIN        
  SET @CELL_PHONE = @CELL_PHONE + '%'        
 END  
            
 IF (@RECORD_PER_PAGE = 0)        
 BEGIN        
  SELECT @RECORD_PER_PAGE = COUNT(*)        
  FROM PATIENT        
 END        
 ELSE        
 BEGIN        
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE        
 END                
        
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1        
        
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE        
 DECLARE @TOATL_PAGESUDM FLOAT        
        
 SELECT @TOATL_PAGESUDM = COUNT(*)        
 FROM PATIENT P        
 LEFT JOIN FOX_TBL_APPLICATION_USER U ON U.USER_NAME = P.CREATED_BY        
 LEFT JOIN FOX_TBL_APPLICATION_USER US ON US.USER_NAME = P.CREATED_BY       
 WHERE (P.PRACTICE_CODE = @PRACTICE_CODE)
 AND P.CHART_ID IS NOT NULL and P.CHART_ID <> ''
 AND (        
   @PATIENT_ACCOUNT IS NULL			
   OR P.Patient_Account LIKE '%' + @PATIENT_ACCOUNT + '%'        
   ) 
   AND (        
   @CHART_ID IS NULL        
   OR P.CHART_ID LIKE '%' + @CHART_ID + '%'        
   ) 
   AND (       
   @LAST_NAME IS NULL        
   OR P.LAST_NAME LIKE '%' + @LAST_NAME + '%'        
   )      
  AND (        
   @FIRST_NAME IS NULL        
   OR P.FIRST_NAME LIKE '%' + @FIRST_NAME + '%'        
   )                   
  AND (        
   @SSN IS NULL               
   OR RIGHT(ISNULL(P.SSN, ''), 4) LIKE '%' + @SSN + '%'        
  )       
  AND (        
   @DOB IS NULL        
   OR CONVERT(VARCHAR(50), ISNULL(P.DATE_OF_BIRTH, 0), 101) LIKE '%' + @DOB + '%'       
   ) 
   AND (        
   @HOME_PHONE IS NULL        
   OR P.HOME_PHONE LIKE '%' + @HOME_PHONE + '%'        
) 
   AND (        
   @WORK_PHONE IS NULL        
   OR P.BUSINESS_PHONE LIKE '%' + @WORK_PHONE + '%'        
   ) 
   AND (        
   @CELL_PHONE IS NULL        
   OR P.CELL_PHONE LIKE '%' + @CELL_PHONE + '%'        
   )        
  AND (ISNULL(P.DELETED, 0) != 1)        
        
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM  
        
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)        
        
 SELECT *        
  ,@TOATL_PAGESUDM AS TOTAL_RECORD_PAGES        
  ,@TOTAL_RECORDS  TOTAL_RECORDS        
 FROM (        
  SELECT 
    P.PATIENT_ACCOUNT        
   ,P.FIRST_NAME        
   ,P.LAST_NAME        
   ,P.MIDDLE_NAME        
   ,ISNULL(P.CHART_ID, '') AS CHART_ID        
   ,RTRIM(LTRIM(P.SSN)) AS SSN        
   ,P.DATE_OF_BIRTH        
   ,P.GENDER        
   ,P.EMAIL_ADDRESS
   ,P.PATIENT_GLOBALID        
   ,P.HOME_PHONE        
   ,P.BUSINESS_PHONE        
   ,P.CELL_PHONE        
   ,U.FIRST_NAME + ' ' + U.LAST_NAME CREATED_BY        
   ,P.CREATED_DATE        
   ,US.FIRST_NAME + ' ' + US.LAST_NAME MODIFIED_BY        
   ,P.MODIFIED_DATE        
   ,P.DELETED        
   ,ROW_NUMBER() OVER (        
    ORDER BY P.CREATED_DATE ASC        
    ) AS ACTIVEROW        
  FROM PATIENT P        
  LEFT JOIN FOX_TBL_APPLICATION_USER U ON U.USER_NAME = P.CREATED_BY        
  LEFT JOIN FOX_TBL_APPLICATION_USER US ON US.USER_NAME = P.CREATED_BY        
  WHERE (P.PRACTICE_CODE = @PRACTICE_CODE)       
   AND P.CHART_ID IS NOT NULL and P.CHART_ID <> ''
	AND   
    (        
    @FIRST_NAME IS NULL        
    OR P.FIRST_NAME LIKE '%' + @FIRST_NAME + '%'        
    )        
   AND (        
    @LAST_NAME IS NULL        
    OR P.LAST_NAME LIKE '%' + @LAST_NAME + '%'        
    )   
	AND (        
    @PATIENT_ACCOUNT IS NULL        
    OR ISNULL(P.Patient_Account , '') LIKE '%' + @PATIENT_ACCOUNT + '%'        
    )     
   AND (        
    @CHART_ID IS NULL        
    OR ISNULL(P.CHART_ID, '') LIKE '%' + @CHART_ID + '%'        
    )        
   AND (        
    @SSN IS NULL        
    OR RIGHT(ISNULL(P.SSN, ''), 4) LIKE '%' + @SSN + '%'        
    )       
   AND (        
    @DOB IS NULL        
    OR CONVERT(VARCHAR(50), ISNULL(P.DATE_OF_BIRTH, 0), 101) LIKE '%' + @DOB + '%'         
    )        
	AND (        
    @HOME_PHONE IS NULL        
    OR P.HOME_PHONE LIKE '%' + @HOME_PHONE + '%'        
    ) 
	AND (        
    @WORK_PHONE IS NULL        
    OR P.BUSINESS_PHONE LIKE '%' + @WORK_PHONE + '%'        
    ) 
	AND (        
    @CELL_PHONE IS NULL        
    OR P.CELL_PHONE LIKE '%' + @CELL_PHONE + '%'        
    ) 
   AND (ISNULL(P.DELETED, 0) != 1)        
  ) AS PATIENT        
 ORDER BY CASE  
 WHEN @SORT_BY = 'PATIENTACCOUNT'        
    AND @SORT_ORDER = 'ASC'        
    THEN Patient_Account        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'PATIENTACCOUNT'        
    AND @SORT_ORDER = 'DESC'        
    THEN Patient_Account        
   END DESC
   ,CASE 
 WHEN @SORT_BY = 'CHARTID'        
    AND @SORT_ORDER = 'ASC'        
    THEN CHART_ID        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'CHARTID'        
    AND @SORT_ORDER = 'DESC'        
    THEN CHART_ID        
   END DESC
   ,CASE 
   WHEN @SORT_BY = 'LASTNAME'        
    AND @SORT_ORDER = 'ASC'        
    THEN LAST_NAME        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'LASTNAME'        
    AND @SORT_ORDER = 'DESC'        
    THEN LAST_NAME        
   END DESC 
   ,CASE      
   WHEN @SORT_BY = 'FIRSTNAME'        
    AND @SORT_ORDER = 'ASC' 
   THEN FIRST_NAME        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'FIRSTNAME'  
    AND @SORT_ORDER = 'DESC'        
    THEN FIRST_NAME        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'SSN'        
    AND @SORT_ORDER = 'ASC'        
    THEN SSN        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'SSN'        
    AND @SORT_ORDER = 'DESC'        
    THEN SSN        
   END DESC        
  ,CASE         
   WHEN @SORT_BY = 'DOB'        
    AND @SORT_ORDER = 'ASC'        
    THEN DATE_OF_BIRTH        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'DOB'        
    AND @SORT_ORDER = 'DESC'        
    THEN DATE_OF_BIRTH        
   END DESC 
   ,CASE 
   WHEN @SORT_BY = 'HOMEPHONE'        
    AND @SORT_ORDER = 'ASC'        
    THEN HOME_PHONE        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'HOMEPHONE'        
    AND @SORT_ORDER = 'DESC'        
    THEN HOME_PHONE        
   END DESC
   ,CASE 
   WHEN @SORT_BY = 'WORKPHONE'        
    AND @SORT_ORDER = 'ASC'        
    THEN BUSINESS_PHONE   
  END ASC     
  ,CASE         
   WHEN @SORT_BY = 'WORKPHONE'        
    AND @SORT_ORDER = 'DESC'        
    THEN BUSINESS_PHONE        
   END DESC
   ,CASE 
   WHEN @SORT_BY = 'CELLPHONE'        
    AND @SORT_ORDER = 'ASC'        
    THEN CELL_PHONE        
   END ASC        
  ,CASE         
   WHEN @SORT_BY = 'CELLPHONE'        
    AND @SORT_ORDER = 'DESC'        
    THEN CELL_PHONE        
   END DESC       
  ,CASE         
   WHEN @SORT_BY = ''        
    AND @SORT_ORDER = ''        
    THEN CREATED_DATE        
   END DESC OFFSET @START_FROM ROWS        
        
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY        
END


