IF (OBJECT_ID('FOX_PROC_GET_PSR_DETAILED_EXCEPTIONAL_REPORT_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PSR_DETAILED_EXCEPTIONAL_REPORT_SERVICE  
GO 
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PSR_DETAILED_EXCEPTIONAL_REPORT_SERVICE]                                  
(@PRACTICE_CODE   BIGINT,                                   
 @DATE_FROM       DATETIME,                                   
 @DATE_TO         DATETIME,              
 @SURVEY_FLAG     VARCHAR(20)                                   
)                                  
AS                                  
     BEGIN                               
  --SET @DATE_FROM = GETDATE()                         
  --SET @DATE_FROM = GETDATE()                         
                           
SELECT *                                
         FROM                                  
         (                                  
             SELECT PS.IS_SURVEYED,                 
    PS.SURVEY_ID,                            
                    PS.PATIENT_ACCOUNT_NUMBER,                    
     PS.SURVEY_FLAG,                                                  
                    PS.PATIENT_FIRST_NAME,                                   
     PS.PATIENT_MIDDLE_INITIAL,                                   
                    PS.PATIENT_LAST_NAME,                                   
                    PS.PATIENT_STATE,                                   
                    PS.PT_OT_SLP,                                   
                    PS.REGION,                                   
                    PS.PROVIDER,                              
     PS.ATTENDING_DOCTOR_NAME,                                
     PS.FEEDBACK,                                  
                    CASE                                  
                        WHEN ISNULL(PS.SURVEY_STATUS_CHILD, '') = ''                                  
                        THEN 'Pending'                                  
                        ELSE PS.SURVEY_STATUS_CHILD                                  
                    END AS SURVEY_STATUS_CHILD,                                   
                    PS.SURVEY_STATUS_BASE AS SURVEY_STATUS_BASE,                                   
                    AU.FIRST_NAME AS SURVEYED_BY_FNAME,                                   
                    AU.LAST_NAME AS SURVEYED_BY_LNAME,                      
     CLJOIN.FILE_NAME,                                 
                    PS.MODIFIED_DATE,                                   
                    CONVERT(VARCHAR, PS.MODIFIED_DATE) AS Modified_Date_Str,                                   
                    PS.IS_CONTACT_HQ,                                  
                    CASE                                  
                        WHEN PS.IS_CONTACT_HQ = 0                                  
                        THEN 'NO'                                  
                        WHEN PS.IS_CONTACT_HQ = 1                                  
                        THEN 'YES'                                  
                    END AS Is_Contact_HQ_Str,                                   
                    PS.IS_RESPONSED_BY_HQ,                                  
                    CASE                                  
                        WHEN PS.IS_RESPONSED_BY_HQ = 0                                  
                        THEN 'NO'                                  
                        WHEN PS.IS_RESPONSED_BY_HQ = 1                                  
                        THEN 'YES'                                  
                    END AS Is_Responsed_By_HQ_Str,                                   
                    PS.IS_QUESTION_ANSWERED,                                  
                    CASE                                  
      WHEN PS.IS_QUESTION_ANSWERED = 0                                  
                        THEN 'NO'                
                        WHEN PS.IS_QUESTION_ANSWERED = 1                                  
                        THEN 'YES'                          
                    END AS Is_Questioned_Answered_Str,                                   
                PS.IS_REFERABLE,                                  
                    CASE                                  
                        WHEN PS.IS_REFERABLE = 0                       
                        THEN 'NO'                                  
                        WHEN PS.IS_REFERABLE = 1                                  
                        THEN 'YES'                              
                    END AS Is_Referrable_Str,                                   
                    PS.IS_IMPROVED_SETISFACTION,                     
                    CASE                                  
                        WHEN PS.IS_IMPROVED_SETISFACTION = 0                                  
              THEN 'NO'                                  
                        WHEN PS.IS_IMPROVED_SETISFACTION = 1                                  
                        THEN 'YES'                                  
   END AS Is_improved_Satisfaction_Str,                      
     PS.IS_EXCEPTIONAL,                      
     CASE                                        
                        WHEN PS.IS_EXCEPTIONAL = 0                                        
                        THEN 'NO'                                        
                        WHEN PS.IS_EXCEPTIONAL = 1                                        
                        THEN 'YES'                                        
                    END AS Is_Exceptional_Str,                       
     PS.IS_PROTECTIVE_EQUIPMENT,                      
     CASE                                        
                        WHEN PS.IS_PROTECTIVE_EQUIPMENT = 0                                        
                        THEN 'NO'                                        
                        WHEN PS.IS_PROTECTIVE_EQUIPMENT = 1                                        
                        THEN 'YES'                                        
                    END AS Is_Protective_Equipment_Str,                        
                    ROW_NUMBER() OVER(ORDER BY PS.MODIFIED_DATE DESC) AS ACTIVEROW                                  
      FROM FOX_TBL_PATIENT_SURVEY PS                                  
      LEFT JOIN FOX_TBL_APPLICATION_USER AU ON AU.USER_NAME = PS.MODIFIED_BY                         
   --INNER JOIN FOX_TBL_PATIENT_SURVEY_CALL_LOG AS CL ON CL.SURVEY_ID = PS.SURVEY_ID         
   INNER JOIN         
     (        
  SELECT CL.SURVEY_ID , MAX(CL.FILE_NAME) AS [FILE_NAME]        
  FROM FOX_TBL_PATIENT_SURVEY_CALL_LOG AS CL         
  GROUP BY CL.SURVEY_ID        
   ) AS CLJOIN ON CLJOIN.SURVEY_ID = PS.SURVEY_ID            
      WHERE ISNULL(PS.DELETED, 0) = 0                                  
      AND PS.PRACTICE_CODE = @PRACTICE_CODE                                  
      AND PS.REGION <> ''                   
    --AND PS.SURVEY_FLAG <> ''               
   AND PS.SURVEY_FLAG = @SURVEY_FLAG                      
      AND PS.IS_SURVEYED = 1                        
      AND (                        
      @DATE_FROM IS NULL                                  
      OR @DATE_TO IS NULL                                  
      OR CONVERT(DATE, PS.MODIFIED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO)                        
      )                                  
      ) AS SURVEY                                  
      ORDER BY  MODIFIED_DATE DESC                        
     END 