ALTER PROCEDURE [DBO].[FOX_PROC_GET_PATIENT_SURVEY_LIST] --1011163, 816631, 0  
 @PRACTICE_CODE BIGINT  
 ,@PATIENT_ACCOUNT BIGINT  
 ,@IS_SURVEYED INT  
AS  
 --declare                                                                        
 --@PRACTICE_CODE BIGINT   = 1011163
 --,@PATIENT_ACCOUNT BIGINT  = 816631
 --,@IS_SURVEYED INT     = 1
BEGIN  
 DECLARE @SURVEY_STATUS BIT;   
 IF ( @IS_SURVEYED = 1)  
        BEGIN   
            SET @SURVEY_STATUS = 1;  
        END  
 ELSE  
  BEGIN   
            SET @SURVEY_STATUS = 0;  
        END  
   
 SELECT DISTINCT PS.PATIENT_ACCOUNT_NUMBER,ISNULL(PS.PATIENT_MIDDLE_INITIAL,'') AS PATIENT_MIDDLE_INITIAL,PS.*,  
                 CONVERT(INT,ROUND(DATEDIFF(HOUR,PS.PATIENT_DATE_OF_BIRTH,GETDATE())/8766.0,0)) AS PATIENT_AGE,  
      CONVERT(INT,ROUND(DATEDIFF(HOUR,PS.RESPONSIBLE_PARTY_DATE_OF_BIRTH,GETDATE())/8766.0,0)) AS RESPONSIBLE_PARTY_AGE,
 CASE                                                                         
       WHEN SL.IS_EMAIL = 1                                                                                               
       THEN 'Email'       
        WHEN SL.IS_SMS = 1                                                                                               
       THEN 'SMS'       
       Else      
      AU.FIRST_NAME                                                                                                                                                 
      END AS SURVEYED_BY_FNAME ,         
             
    CASE                                                                         
       WHEN SL.IS_EMAIL = 1                                                                                                 
       THEN ''       
       WHEN SL.IS_SMS = 1                                                                                               
       THEN ''      
       Else      
      AU.LAST_NAME + ','                                                                                                                                                 
      END AS SURVEYED_BY_LNAME,    
     -- AU.FIRST_NAME AS SURVEYED_BY_FNAME,   
     -- AU.LAST_NAME AS SURVEYED_BY_LNAME,   
      ISNULL(PS.SURVEY_STATUS_CHILD, '') AS SURVEY_STATUS_CHILD,  
      PS.SURVEY_FORMAT_TYPE   
 FROM FOX_TBL_PATIENT_SURVEY PS  
   LEFT JOIN FOX_TBL_APPLICATION_USER AU WITH (NOLOCK) ON AU.USER_NAME = PS.MODIFIED_BY 
   LEFT JOIN FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) ON PS.SURVEY_ID = SL.SURVEY_ID AND ISNULL(SL.DELETED, 0) = 0 
 WHERE ISNULL(PS.DELETED, 0) = 0   
         AND PS.PRACTICE_CODE = @PRACTICE_CODE  
   AND IS_SURVEYED = @SURVEY_STATUS  
   AND PS.PATIENT_ACCOUNT_NUMBER = @PATIENT_ACCOUNT  
END  
  