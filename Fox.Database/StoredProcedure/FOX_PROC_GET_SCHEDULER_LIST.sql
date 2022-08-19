IF (OBJECT_ID('FOX_PROC_GET_SCHEDULER_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SCHEDULER_LIST  
GO  
-- =============================================                                                                                                                    
-- AUTHOR:  <DEVELOPER, Abdur Rafay>                                                                                                                    
-- CREATE DATE: <CREATE DATE, 20/08/2019>                                                                                                                    
-- DESCRIPTION: <GET SCHEDULER LIST>                                                                                        
-- EXEC FOX_PROC_GET_SCHEDULER_LIST '0', '1011163', '', '08/03/2020', '08/03/2020', '0', '0', 'Cancelled,Completed,Pending,Void,Rescheduled,NULL', '5445', '544104', '0', '0','0','0', '1','10000','AppointmentDate','des'             
                                          
 CREATE PROCEDURE [dbo].[FOX_PROC_GET_SCHEDULER_LIST]             
 @patient_account   VARCHAR(50),                                                     
 @practice_code     BIGINT,                                                     
 @search_string     VARCHAR(100),                                                     
 @date_from         DATETIME,                                                     
 @date_to           DATETIME,                                                     
 @provider          VARCHAR(50),                                                     
 @reason            VARCHAR(100),                                                     
 @status            VARCHAR(100),                                                     
 @restrict_status   VARCHAR(100),                                                     
 @restrict_location VARCHAR(100),                                                     
 @region            VARCHAR(100),                                                     
 @location          VARCHAR(100),                                                     
 @discipline        VARCHAR(100),             
 @insurance        VARCHAR(100),            
 @current_page      INT,                                                     
 @record_per_page   INT,                                                     
 @sort_by           VARCHAR(50),                                                     
 @sort_order        VARCHAR(5)                                                    
AS                                                    
     BEGIN                                                    
         SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;                                                    
         IF(@PROVIDER = '0')                                                    
             BEGIN                                                    
                 SET @PROVIDER = NULL;                                                    
             END;                                                    
             ELSE                                                    
             BEGIN                                                    
                 SET @PROVIDER = @PROVIDER+'%';                                                    
             END;                                                    
         IF(@patient_account = '0')                                                    
             BEGIN                  
                 SET @patient_account = NULL;                             
             END;                                                    
             ELSE                                                    
 BEGIN                                                    
                 SET @patient_account = @patient_account+'%';                                                    
             END;                                                    
         IF(@REASON = '0')                                                    
             BEGIN                   
                 SET @REASON = NULL;                 
             END;                                                    
      ELSE                                      
             BEGIN                    
                 SET @REASON = @REASON+'%';                                       
             END;                                         
         --IF(@STATUS = '0')                  
 --    BEGIN                                                                          
         --        SET @STATUS = NULL;                                                                          
         --    END;                                                                          
         --    ELSE                                                                          
         --    BEGIN                                          
         --        SET @STATUS = @STATUS+'%';                                                                          
         --    END;                                                                          
         IF(@RESTRICT_STATUS = '0')                                                    
             BEGIN                                                    
      SET @RESTRICT_STATUS = NULL;                                       END;                                                    
             ELSE                                                    
             BEGIN                                                    
             SET @RESTRICT_STATUS = @RESTRICT_STATUS;                                                    
             END;                                                    
         IF(@RESTRICT_LOCATION = '0')                                                    
             BEGIN                                                    
                 SET @RESTRICT_LOCATION = NULL;                                                    
             END;                                                    
             ELSE                                                    
             BEGIN                                                    
                 SET @RESTRICT_LOCATION = @RESTRICT_LOCATION;                                                    
     END;                                                    
         IF(@REGION = '0')                                                    
             BEGIN                                                    
                 SET @REGION = NULL;                                                    
             END;                                                    
             ELSE                                                    
            BEGIN                                                    
                 SET @REGION = @REGION+'%';                                                    
             END;                                                    
         IF(@LOCATION = '0')                                                    
             BEGIN                                                    
                SET @LOCATION = NULL;                                                    
             END;                                                    
             ELSE                                                    
             BEGIN                                                    
                 SET @LOCATION = @LOCATION+'%';                                                    
             END;                                                    
         IF(@DISCIPLINE = '0')                                                    
             BEGIN                                                    
                 SET @DISCIPLINE = NULL;                                                    
             END;                                                    
             ELSE              
             BEGIN                                                    
        SET @DISCIPLINE = @DISCIPLINE+'%';                       
    END;             
  IF(@INSURANCE = '0')                                                    
             BEGIN                                                    
             SET @INSURANCE = NULL;                                                    
             END;                                                    
             ELSE                                                 
             BEGIN                                                    
        SET @INSURANCE = @INSURANCE+'%';                                                    
             END;         
         IF(@RECORD_PER_PAGE = 0)                                                 
             BEGIN                                                    
                 SET @RECORD_PER_PAGE = 10000000;                                                    
             END;                                                    
             ELSE                                                    
             BEGIN                                                    
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;                                                    
             END;                                                    
                                                    
         --                                                                                    
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                                                    
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                                   
         DECLARE @TOATL_PAGESUDM FLOAT;                                                    
                                                      
              IF OBJECT_ID('tempdb..#cte') IS NOT NULL                                                  
             DROP TABLE #CTE;                                                    
         WITH CTE                                                    
              AS (SELECT PATIENT_ADDRESS_HISTORY_ID,                                                     
                         PATIENT_ACCOUNT,                                                     
                         CREATED_DATE,                      
       PATIENT_POS_ID,                                                     
                         ROW_NUMBER() OVER(PARTITION BY PATIENT_ACCOUNT ORDER BY CREATED_DATE DESC) RW                                                    
                  FROM FOX_TBL_PATIENT_ADDRESS                                                    
                  WHERE                       
      ADDRESS_TYPE = 'HOME ADDRESS'                                                    
                        AND                       
      ISNULL(DELETED, 0) = 0                                                                      
              --aND PATIENT_ACCOUNT=53428276                                   
              --- AND PATIENT_ACCOUNT=P.PATIENT_ACCOUNT                                                                    
              )                                                    
              SELECT *                                                    
              INTO #CTE                                                    
              FROM CTE                                           
              WHERE RW = 1;                                                    
                                                    
         --------------------------------------------------------------                                                                    
         SELECT @TOATL_PAGESUDM = COUNT(DISTINCT APPT.APPOINTMENT_ID)                                              
         FROM FOX_TBL_APPOINTMENT APPT                                 
              LEFT JOIN FOX_TBL_APPOINTMENT_STATUS A_ST ON APPT.APPOINTMENT_STATUS_ID = A_ST.APPOINTMENT_STATUS_ID                                                    
                                 AND ISNULL(A_ST.DELETED, 0) = 0                                                    
              LEFT JOIN FOX_TBL_VISIT_TYPE VIS ON APPT.VISIT_TYPE_ID = VIS.VISIT_TYPE_ID                                                    
                         AND ISNULL(VIS.DELETED, 0) = 0                                                    
              LEFT JOIN FOX_TBL_PROVIDER PRO ON APPT.PROVIDER_ID = PRO.FOX_PROVIDER_ID                                                    
                    AND PRO.PRACTICE_CODE = @PRACTICE_CODE                                                    
                                                AND ISNULL(PRO.DELETED, 0) = 0                                                    
              LEFT JOIN FOX_TBL_CASE CAS ON APPT.CASE_ID = CAS.CASE_ID                                               
                                            AND ISNULL(CAS.DELETED, 0) = 0                              
                                            AND CAS.PRACTICE_CODE = @PRACTICE_CODE             
     LEFT JOIN FOX_TBL_DISCIPLINE DIS ON DIS.DISCIPLINE_ID = CAS.DISCIPLINE_ID                                                    
                                            AND ISNULL(DIS.DELETED, 0) = 0                              
                                            AND DIS.PRACTICE_CODE = @PRACTICE_CODE             
              LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS POS ON POS.LOC_ID = CAS.POS_ID                                                    
                                              AND ISNULL(POS.DELETED, 0) = 0                                                    
                                                        AND POS.PRACTICE_CODE = @PRACTICE_CODE                                                    
                                                       -- AND POS.FACILITY_TYPE_ID <> @RESTRICT_LOCATION                                                    
              LEFT JOIN FOX_TBL_REFERRAL_REGION REG ON REG.REFERRAL_REGION_ID = CAS.REF_REGION_ID                                                    
                                                       AND ISNULL(REG.DELETED, 0) = 0                                                    
                                                       AND REG.PRACTICE_CODE = @PRACTICE_CODE                                                    
              LEFT JOIN PATIENT PT ON APPT.PATIENT_ACCOUNT = PT.PATIENT_ACCOUNT                                                  
                                      AND PT.PRACTICE_CODE = @PRACTICE_CODE                                                    
                                      AND ISNULL(PT.DELETED, 0) = 0                                                    
              LEFT JOIN FOX_TBL_PATIENT PI ON APPT.PATIENT_ACCOUNT = PI.PATIENT_ACCOUNT                                                    
                                              AND ISNULL(PI.DELETED, 0) = 0                                                    
              LEFT JOIN FOX_TBL_FINANCIAL_CLASS FN_CL ON PI.FINANCIAL_CLASS_ID = FN_CL.FINANCIAL_CLASS_ID                                                    
                                                        AND FN_CL.PRACTICE_CODE = @PRACTICE_CODE                                                    
                                                         AND ISNULL(FN_CL.DELETED, 0) = 0                           
     LEFT JOIN Fox_Tbl_Patient_POS AS pat_pos on  pat_pos.Loc_ID = CAS.POS_ID                      
               AND   pat_pos.PATIENT_ACCOUNT =  PI.PATIENT_ACCOUNT                        
               AND ISNULL(pat_pos.DELETED, 0) = 0                       
      AND pat_pos.Patient_POS_ID = (SELECT TOP 1 pat_pos.Patient_POS_ID WHERE pat_pos.Is_Default = 1 ORDER BY pat_pos.Created_Date DESC)                  
             -- LEFT JOIN #CTE PH ON PH.PATIENT_ACCOUNT = PT.PATIENT_ACCOUNT                                                    
              LEFT JOIN FOX_TBL_PATIENT_ADDRESS PT_ADD ON PT_ADD.PATIENT_POS_ID = pat_pos.Patient_POS_ID                             
               AND  PT_ADD.PATIENT_ACCOUNT =   pat_pos.PATIENT_ACCOUNT                                 
               AND ISNULL(PT_ADD.DELETED, 0) = 0              
   LEFT JOIN FOX_TBL_PATIENT_INSURANCE P_INS ON APPT.PATIENT_ACCOUNT = (SELECT TOP 1 P_INS.PATIENT_ACCOUNT             
                   WHERE P_INS.PRI_SEC_OTH_TYPE = 'P'             
                   AND P_INS.FOX_INSURANCE_STATUS = 'C'            
                   AND ISNULL(P_INS.INACTIVE, 0) = 0            
                   AND ISNULL(P_INS.DELETED, 0) = 0             
                   ORDER BY P_INS.Created_Date DESC)                                                  
   LEFT JOIN FOX_TBL_INSURANCE INS ON INS.FOX_TBL_INSURANCE_ID = P_INS.FOX_TBL_INSURANCE_ID                                                    
                                              AND ISNULL(INS.DELETED, 0) = 0            
         WHERE(@DATE_FROM IS NULL                                           
               OR @DATE_TO IS NULL                                                    
               OR CAST(APPT.APPOINTMENT_DATE AS DATE) BETWEEN CAST(@DATE_FROM AS DATE) AND CAST(@DATE_TO AS DATE))                                                    
              AND APPT.PRACTICE_CODE = @PRACTICE_CODE                                      
              AND ISNULL(APPT.DELETED, 0) = 0                                                                            
              --        AND ISNULL(APPT.ISBLOCKED, 0) = 0                                             
              AND (@patient_account IS NULL                                                    
                   OR appt.PATIENT_ACCOUNT LIKE '%'+@patient_account+'%')                                                    
              AND (@PROVIDER IS NULL                                                    
                   OR APPT.PROVIDER_ID LIKE '%'+@PROVIDER+'%')                                                    
              AND (@REASON IS NULL                                                    
                   OR APPT.VISIT_TYPE_ID LIKE '%'+@REASON+'%')                                                                          
              --AND (@STATUS IS NULL                                                                          
              --     OR APPT.APPOINTMENT_STATUS_ID LIKE '%'+@STATUS+'%')                                  
 AND ((ISNULL(A_ST.DESCRIPTION,'NULL') IN                                                    
         (                                                    
             SELECT Item                                                    
             FROM dbo.SplitStrings_CTE(@STATUS, N',')                                                    
         ))                                                    
         )                                                    
                                                    
              --AND (APPT.APPOINTMENT_STATUS_ID <> @RESTRICT_STATUS)                                                                          
              AND (@REGION IS NULL                                                    
                   OR REG.REFERRAL_REGION_ID LIKE '%'+@REGION+'%')                                            
              AND (@LOCATION IS NULL                                                    
                   OR POS.LOC_ID LIKE '%'+@LOCATION+'%')                                                    
              AND (@DISCIPLINE IS NULL                                                    
                   OR CAS.RT_CASE_NO LIKE '%'+@DISCIPLINE+'%')             
     AND (@INSURANCE IS NULL                                                    
                   OR P_INS.FOX_TBL_INSURANCE_ID LIKE '%'+@INSURANCE+'%')               
              AND (APPT.TIME_FROM LIKE @SEARCH_STRING+'%'                                                    
    OR PT.CHART_ID LIKE @SEARCH_STRING+'%'                                                    
                   OR PT.Patient_Account LIKE @SEARCH_STRING+'%'                                                    
                   OR PT.LAST_NAME+', '+PT.FIRST_NAME LIKE @SEARCH_STRING+'%'                                        
                   OR PT.LAST_NAME LIKE @SEARCH_STRING+'%'                                
                   OR PT.FIRST_NAME LIKE @SEARCH_STRING+'%'                                                    
                   OR FN_CL.CODE LIKE '%'+@SEARCH_STRING+'%'                                                    
                   OR VIS.DESCRIPTION LIKE @SEARCH_STRING+'%'                                                    
                   OR A_ST.DESCRIPTION LIKE @SEARCH_STRING+'%'                                                    
                   OR PRO.PROVIDER_NAME LIKE '%'+@SEARCH_STRING+'%'                                                    
                   OR CAS.RT_CASE_NO LIKE '%'+@SEARCH_STRING+'%'                                                    
                   OR PT_ADD.ADDRESS LIKE @SEARCH_STRING+'%'                                                    
                   OR POS.CODE LIKE @SEARCH_STRING+'%'                                                    
                   OR POS.NAME LIKE @SEARCH_STRING+'%'                                        
                   OR POS.ADDRESS LIKE '%'+@SEARCH_STRING+'%'                                                    
                   OR POS.CITY LIKE @SEARCH_STRING+'%'                                                    
                   OR POS.ZIP LIKE @SEARCH_STRING+'%'                                       
                   OR REG.REFERRAL_REGION_NAME LIKE @SEARCH_STRING+'%');                                                    
         IF(@RECORD_PER_PAGE = 0)                                                    
             BEGIN                                                    
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;                                                    
             END;                                                    
             ELSE                                                    
             BEGIN                                                    
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;                                                    
             END;                                                             
         --SELECT @TOATL_PAGESUDM                                                                    
         --                                                               
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                                                    
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);                                                                            
         --                                                                               
         SELECT *,                                                     
                CAST(@TOATL_PAGESUDM AS INT) AS TOTAL_RECORD_PAGES,                                                     
                CAST(@TOTAL_RECORDS AS INT) AS TOTAL_RECORDS                                                    
         FROM                                                    
         (                                                    
             SELECT DISTINCT             
     P_INS.PATIENT_INSURANCE_ID,            
     P_INS.FOX_TBL_INSURANCE_ID,            
   P_INS.PRI_SEC_OTH_TYPE,            
     P_INS.FOX_INSURANCE_STATUS,            
     INS.INSURANCE_NAME,            
     DIS.DESCRIPTION,            
                    CAST(APPT.TIME_FROM AS DATETIME) AS TIME_FROM_DATE,                                                     
                    CAST(APPT.TIME_TO AS DATETIME) AS TIME_TO_DATE,                                                     
    DATEDIFF(MINUTE, CAST(APPT.TIME_FROM AS TIME),             
     CAST(APPT.TIME_TO AS TIME)) AS LENGTH,                                                     
                    CONVERT(VARCHAR, APPT.APPOINTMENT_DATE) AS APPOINTMENT_DATE_STR,      
     CONVERT(VARCHAR(20), CAST(APPT.APPOINTMENT_COMPLETE_DATE_TIME AS DATE), 0) AS APPOINTMENT_COMPLETE_DATE_STR,      
     CONVERT(VARCHAR(10), CAST(APPT.APPOINTMENT_COMPLETE_DATE_TIME AS TIME), 0) AS APPOINTMENT_COMPLETE_TIME_STR,      
                    PT.CHART_ID AS MRN,                                                    
                    CASE                                                    
      WHEN ISNULL(PT.LAST_NAME, '') = ''                                                    
                        THEN ISNULL(PT.FIRST_NAME, '')                                                    
                        ELSE ISNULL(PT.LAST_NAME, '')+', '+ISNULL(PT.FIRST_NAME, '')                
                    END AS NAME,                                                     
                    PT.HOME_PHONE AS HOME_PHONE,                                                     
                    PT_ADD.PATIENT_ADDRESS_HISTORY_ID AS ADDRESS_ID,                                                
                    PT_ADD.ADDRESS,                            
                    PT_ADD.CITY,                            
     PT_ADD.STATE,                            
                    PT_ADD.ZIP,                                                     
                    FN_CL.FINANCIAL_CLASS_ID AS FC_ID,                                                     
                    FN_CL.CODE AS FC_CODE,                                                     
     FN_CL.DESCRIPTION AS FC_DESCRIPTION,                                                     
                    VIS.VISIT_TYPE_ID AS REASON_ID,                
                    A_ST.APPOINTMENT_STATUS_ID AS STATUS_ID,                                 
                    -- A_ST.DESCRIPTION AS STATUS,                                                      
                    CASE                                                    
                        WHEN A_ST.DESCRIPTION = 'Void'                                                    
      THEN('Pending (Void)')                                                    
                        ELSE A_ST.DESCRIPTION                                                    
                    END AS STATUS,                                                     
                    PRO.PROVIDER_NAME AS PROVIDER,                                                    
                    CASE                                                  
                        WHEN APPT.ISBLOCKED = 1                                                    
                        THEN('Block')                                                    
                        ELSE VIS.DESCRIPTION                                                    
                    END AS REASON,                                                     
                    CAS.RT_CASE_NO,                                                     
                    POS.LOC_ID AS POS_ID,                                                     
                    POS.CODE AS POS_CODE,                     
                    POS.NAME AS POS_NAME,                                                     
                    POS.ADDRESS AS POS_ADDRESS,                       
     POS.CITY AS POS_CITY,                                 
                    POS.STATE AS POS_STATE,                      
     POS.ZIP AS POS_ZIP,                                                     
     POS.Longitude AS AL_LONGITUDE,                                              
                    POS.Latitude AS AL_LATITUDE,                            
     CONVERT (VARCHAR(100), POS.Latitude,128)  AS   AL_LATITUDE_STR,                                        
     CONVERT (VARCHAR(100), POS.Longitude,128)  AS  AL_LONGITUDE_STR,                      
     CONVERT (VARCHAR(100), APPT.LATITUDE,128)  AS APPT_LATITUDE_STR,                                        
     CONVERT (VARCHAR(100), APPT.LONGITUDE,128)  AS APPT_LONGITUDE_STR,                        
     CONVERT (VARCHAR(100),  PT_ADD.Latitude,128)  AS  PATIENT_ADDRESS_LATITUDE_STR,                                        
     CONVERT (VARCHAR(100),  PT_ADD.Longitude,128)  AS  PATIENTA_ADDRESS_LONGITUDE_STR,                             
     CAST(PT_ADD.Longitude AS float) AS PATIENTA_ADDRESS_LONGITUDE,                        
     CAST(PT_ADD.Latitude AS float) AS PATIENT_ADDRESS_LATITUDE,                          
                    REG.REFERRAL_REGION_ID AS REGION_ID,                                                     
                    REG.REFERRAL_REGION_CODE AS REGION_CODE,                                                     
                    REG.REFERRAL_REGION_NAME AS REGION_NAME,                                                     
                    ROW_NUMBER() OVER(ORDER BY APPOINTMENT_DATE DESC) AS ACTIVEROW,                             
                    APPT.*                                        
                                              
             FROM FOX_TBL_APPOINTMENT APPT                                                    
                  LEFT JOIN FOX_TBL_APPOINTMENT_STATUS A_ST ON APPT.APPOINTMENT_STATUS_ID = A_ST.APPOINTMENT_STATUS_ID                                                    
                 AND ISNULL(A_ST.DELETED, 0) = 0                                                    
                 LEFT JOIN FOX_TBL_VISIT_TYPE VIS ON APPT.VISIT_TYPE_ID = VIS.VISIT_TYPE_ID                                                    
                                                     AND ISNULL(VIS.DELETED, 0) = 0                                                    
                 LEFT JOIN FOX_TBL_PROVIDER PRO ON APPT.PROVIDER_ID = PRO.FOX_PROVIDER_ID                                                    
                                                   AND PRO.PRACTICE_CODE = @PRACTICE_CODE                                                    
                                                   AND ISNULL(PRO.DELETED, 0) = 0                                 
                 LEFT JOIN FOX_TBL_CASE CAS ON APPT.CASE_ID = CAS.CASE_ID                                                    
                                               AND ISNULL(CAS.DELETED, 0) = 0                                                    
                    AND CAS.PRACTICE_CODE = @PRACTICE_CODE            
     LEFT JOIN FOX_TBL_DISCIPLINE DIS ON DIS.DISCIPLINE_ID = CAS.DISCIPLINE_ID                                                    
                                            AND ISNULL(DIS.DELETED, 0) = 0                              
                                            AND DIS.PRACTICE_CODE = @PRACTICE_CODE            
                 LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS POS ON POS.LOC_ID = CAS.POS_ID                                                    
                                                            AND ISNULL(POS.DELETED, 0) = 0                                                    
                    AND POS.PRACTICE_CODE = @PRACTICE_CODE                                           
                                                                  
                                                         --   AND POS.FACILITY_TYPE_ID <> @RESTRICT_LOCATION                                           
                                                                  
                 LEFT JOIN FOX_TBL_REFERRAL_REGION REG ON REG.REFERRAL_REGION_ID = CAS.REF_REGION_ID                                                    
                                                           AND ISNULL(REG.DELETED, 0) = 0                                                    
                                                           AND REG.PRACTICE_CODE = @PRACTICE_CODE                      
                 LEFT JOIN PATIENT PT ON APPT.PATIENT_ACCOUNT = PT.PATIENT_ACCOUNT                                         
                                          AND PT.PRACTICE_CODE = @PRACTICE_CODE                                                    
                                          AND ISNULL(PT.DELETED, 0) = 0                                                    
                 LEFT JOIN FOX_TBL_PATIENT PI ON APPT.PATIENT_ACCOUNT = PI.PATIENT_ACCOUNT                                                    
                                                  AND ISNULL(PI.DELETED, 0) = 0                                             
                 LEFT JOIN FOX_TBL_FINANCIAL_CLASS FN_CL ON PI.FINANCIAL_CLASS_ID = FN_CL.FINANCIAL_CLASS_ID                                                    
               AND FN_CL.PRACTICE_CODE = @PRACTICE_CODE                                                    
             LEFT JOIN Fox_Tbl_Patient_POS AS pat_pos on  pat_pos.Loc_ID = CAS.POS_ID                      
               AND   pat_pos.PATIENT_ACCOUNT =  PI.PATIENT_ACCOUNT                       
               AND ISNULL(pat_pos.DELETED, 0) = 0                  
               AND pat_pos.Patient_POS_ID = (SELECT TOP 1 pat_pos.Patient_POS_ID WHERE pat_pos.Is_Default = 1 ORDER BY pat_pos.Created_Date DESC)                             
                 --LEFT JOIN #CTE PH ON PH.PATIENT_ACCOUNT = PT.PATIENT_ACCOUNT                                                    
             LEFT JOIN FOX_TBL_PATIENT_ADDRESS PT_ADD ON PT_ADD.PATIENT_POS_ID = pat_pos.PATIENT_POS_ID                             
               AND  PT_ADD.PATIENT_ACCOUNT =   pat_pos.PATIENT_ACCOUNT                        
               AND ISNULL(PT_ADD.DELETED, 0) = 0                  
       LEFT JOIN FOX_TBL_PATIENT_INSURANCE P_INS ON APPT.PATIENT_ACCOUNT = (SELECT TOP 1 P_INS.PATIENT_ACCOUNT             
                   WHERE P_INS.PRI_SEC_OTH_TYPE = 'P'             
                   AND P_INS.FOX_INSURANCE_STATUS = 'C'            
                   AND ISNULL(P_INS.INACTIVE, 0) = 0            
                   AND ISNULL(P_INS.DELETED, 0) = 0             
                   ORDER BY P_INS.Created_Date DESC)            
          LEFT JOIN FOX_TBL_INSURANCE INS ON INS.FOX_TBL_INSURANCE_ID = P_INS.FOX_TBL_INSURANCE_ID                                                    
                                              AND ISNULL(INS.DELETED, 0) = 0                                      
                                                    
             WHERE(@DATE_FROM IS NULL                                            
                   OR @DATE_TO IS NULL                                                    
                   OR CAST(APPT.APPOINTMENT_DATE AS DATE) BETWEEN CAST(@DATE_FROM AS DATE) AND CAST(@DATE_TO AS DATE))                                                    
                  AND APPT.PRACTICE_CODE = @PRACTICE_CODE                                                    
                  AND ISNULL(APPT.DELETED, 0) = 0                                                                            
                  -- AND ISNULL(APPT.ISBLOCKED, 0) = 0                                                                         
                  AND (@patient_account IS NULL                                                    
                       OR appt.PATIENT_ACCOUNT LIKE '%'+@patient_account+'%')                                                    
                  AND (@PROVIDER IS NULL                                                    
                  OR APPT.PROVIDER_ID LIKE '%'+@PROVIDER+'%')                                                    
                  AND (@REASON IS NULL                                         
                       OR APPT.VISIT_TYPE_ID LIKE '%'+@REASON+'%')                                                                          
                  --AND (@STATUS IS NULL                        
                  --     OR APPT.APPOINTMENT_STATUS_ID LIKE '%'+@STATUS+'%')                                                    
      -- Added 'NULL' to handle Null Case                                                  
                  AND ((ISNULL(A_ST.DESCRIPTION,'NULL') IN                                                    
             (                                                    
                 SELECT Item                                                    
                 FROM dbo.SplitStrings_CTE(@STATUS, N',')                                                    
             ))                                                    
             )                                         
                                                    
                  --AND (APPT.APPOINTMENT_STATUS_ID <> @RESTRICT_STATUS)                                                                          
                  AND (@REGION IS NULL                                                    
                       OR REG.REFERRAL_REGION_ID LIKE '%'+@REGION+'%')                                                    
                  AND (@LOCATION IS NULL                                                    
                       OR POS.LOC_ID LIKE '%'+@LOCATION+'%')                                       
                  AND (@DISCIPLINE IS NULL                                              
                       OR CAS.RT_CASE_NO LIKE '%'+@DISCIPLINE+'%')                  
      AND (@INSURANCE IS NULL                                                    
       OR P_INS.FOX_TBL_INSURANCE_ID LIKE '%'+@INSURANCE+'%')               
                  AND (APPT.APPOINTMENT_DATE LIKE '%'+@SEARCH_STRING+'%'                                                    
                       OR APPT.TIME_FROM LIKE @SEARCH_STRING+'%'                                                    
                       OR PT.CHART_ID LIKE @SEARCH_STRING+'%'                                                    
                       OR APPT.Patient_Account LIKE @SEARCH_STRING+'%'                                                    
                       OR PT.LAST_NAME+', '+PT.FIRST_NAME LIKE @SEARCH_STRING+'%'                                                    
                       OR PT.LAST_NAME LIKE @SEARCH_STRING+'%'                                                    
                       OR PT.FIRST_NAME LIKE @SEARCH_STRING+'%'                                                    
        OR FN_CL.CODE LIKE '%'+@SEARCH_STRING+'%'                                                    
                       OR VIS.DESCRIPTION LIKE @SEARCH_STRING+'%'                                                    
                       OR A_ST.DESCRIPTION LIKE @SEARCH_STRING+'%'                                                    
                       OR PRO.PROVIDER_NAME LIKE '%'+@SEARCH_STRING+'%'                                                    
                       OR CAS.RT_CASE_NO LIKE '%'+@SEARCH_STRING+'%'                                                    
                       OR PT_ADD.ADDRESS LIKE @SEARCH_STRING+'%'                                                
                       OR POS.CODE LIKE @SEARCH_STRING+'%'                                                    
                       OR POS.NAME LIKE @SEARCH_STRING+'%'                                   
                       OR POS.ADDRESS LIKE '%'+@SEARCH_STRING+'%'                                                    
                       OR POS.CITY LIKE @SEARCH_STRING+'%'                                                    
                       OR POS.ZIP LIKE @SEARCH_STRING+'%'                                                    
                       OR REG.REFERRAL_REGION_NAME LIKE @SEARCH_STRING+'%')                                                    
         ) AS FOX_TBL_APPOINTMENT                                                    
         ORDER BY CASE                              
                    WHEN @SORT_BY = 'APPOINTMENTDATE'                                                    
                           AND @SORT_ORDER = 'ASC'                                                    
                      THEN APPOINTMENT_DATE                                                    
                  END ASC,                                                    
                  CASE                                    
                      WHEN @SORT_BY = 'APPOINTMENTDATE'                                             
                           AND @SORT_ORDER = 'DESC'                                                    
                      THEN APPOINTMENT_DATE                                        
  END DESC                                                    
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;                                                    
     END; 