IF (OBJECT_ID('Fox_Get_Patient_Info_Index_Info_single_record') IS NOT NULL) DROP PROCEDURE Fox_Get_Patient_Info_Index_Info_single_record
GO                      
CREATE PROCEDURE [dbo].[Fox_Get_Patient_Info_Index_Info_single_record]       
      
--MODIFIED BY ADNAN ASGHAR       
-- DATED 05192022                             
@PATIENT_ACCOUNT                 VARCHAR(100),                         
@PRACTICE_CODE            BIGINT,                         
@PRACTICE_ORGANIZATION_ID BIGINT,                         
@WORK_ID                  BIGINT                  
AS        
      
      
      
--DECLARE                                   
--@PATIENT_ACCOUNT          VARCHAR(100) = '1012714600151798',                         
--@PRACTICE_CODE            BIGINT = 1012714,                         
--@PRACTICE_ORGANIZATION_ID BIGINT = NULL,                         
--@WORK_ID                  BIGINT = ''        
      
     IF(@PRACTICE_ORGANIZATION_ID = 0)                        
         BEGIN                        
             SET @PRACTICE_ORGANIZATION_ID = NULL;                        
         END;                                                                                                                                         
     IF(@WORK_ID = 0)                        
         BEGIN                        
             SET @WORK_ID = NULL;                        
         END;                                                                      
   BEGIN              
         IF OBJECT_ID('tempdb..#cte') IS NOT NULL                        
             DROP TABLE #CTE;                        
         WITH CTE                        
              AS       
     (SELECT FOX_INTERFACE_SYNCH_ID,                         
                         PATIENT_ACCOUNT,                         
                         ROW_NUMBER() OVER(PARTITION BY PATIENT_ACCOUNT ORDER BY FOX_INTERFACE_SYNCH_ID DESC) RW                        
                  FROM FOX_TBL_INTERFACE_SYNCH AS ftis_p_i   WITH (NOLOCK)                     
                  WHERE ISNULL(ftis_p_i.DELETED, 0) = 0                        
                        AND ftis_p_i.PRACTICE_CODE = @PRACTICE_CODE        
      and PATIENT_ACCOUNT=@PATIENT_ACCOUNT   --- ADDED by ADNAN ASGHAR 05192022      
                        AND ISNULL(ftis_p_i.IS_SYNCED, 0) = 1)              
              SELECT *                        
              INTO #CTE                        
              FROM CTE                        
              WHERE RW = 1;         
        
         IF OBJECT_ID('tempdb..#cte2') IS NOT NULL                        
             DROP TABLE #CTE2;                        
         WITH CTE                        
              AS (SELECT Patient_POS_ID,                         
                         Patient_Account,                         
                         ROW_NUMBER() OVER(PARTITION BY PATIENT_ACCOUNT ORDER BY Created_Date DESC) RW                        
                  FROM Fox_Tbl_Patient_POS    WITH (NOLOCK)                       
                  WHERE loc_id <> 0                        
                        AND isnull(deleted, 0) = 0                        
                        AND ISNULL(Patient_Account, '') <> '')       
           
      
              SELECT *                        
              INTO #CTE2                        
              FROM CTE                        
              WHERE RW = 1;                        
               
       
      
         -----------------------------------------------------                          
   SELECT DISTINCT                         
                    ROW_NUMBER() OVER(ORDER BY p.SSN) AS ROW,                         
                    p.First_Name,                         
                    p.Last_Name,                         
                    CONVERT(VARCHAR(100), p.Patient_Account) AS Patient_Account,                         
                    RTRIM(LTRIM(SSN)) AS SSN,                      
                    CASE UPPER(p.Gender)                        
                        WHEN 'M'                        
                        THEN 'Male'                  
                        WHEN 'MALE'                        
                        THEN 'Male'                        
     WHEN 'F'                        
                        THEN 'Female'                        
                        WHEN 'FEMALE'          
                  THEN 'Female'                        
     END AS GENDER,                         
                    CONVERT(VARCHAR, p.Date_Of_Birth, 101) AS Date_Of_Birth,                         
                    Chart_Id AS MRN,                         
                    isnull(loc.ZIP, '') AS ZIP,                    
                    isnull(loc.City, '') AS City,                         
                    isnull(loc.STATE, '') AS STATE,                         
                    isnull(loc.Address, '') AS HomeAddress,                         
     p.MIDDLE_NAME,                         
                    p.Cell_Phone,                         
                    Business_Phone,                         
                    p.Home_Phone,                         
                    p.Email_Address,                         
                    fp.Title,                         
                    fp.Fax_Number,                         
                    fp.PRACTICE_ORGANIZATION_ID,                         
                    FC.NAME AS FINANCIAL_CLASS_NAME,                         
                    FC.FINANCIAL_CLASS_ID,                                                     
                    CONVERT(BIT, ISNULL(ftis.IS_SYNCED, 0)) AS IS_WORK_ORDER_INTERFACE_SYNCED,                                     
                    CASE                        
                        WHEN ISNULL(ftis_p.IS_SYNCED, 0) = 1                        
                             OR (p.Chart_Id IS NOT NULL                        
                                 AND p.Chart_Id <> '')                        
                        THEN CONVERT(BIT, 1)                        
                        ELSE CONVERT(BIT, 0)                        
                    END AS IS_PATIENT_INTERFACE_SYNCED,                        
        CASE                        
                        WHEN ISNULL(ftis_p.IS_SYNCED, 0) = 1                        
                             OR (p.Chart_Id IS NOT NULL                        
                                 AND p.Chart_Id <> '')                        
                        THEN CONVERT(BIT, 1)                        
                        ELSE CONVERT(BIT, 0)                        
                    END AS IS_PATIENT_OLD_OR_SYNCED,                                                            
             (                        
                 SELECT SUM(amt_due)                        
                 FROM claims  WITH (NOLOCK)                         
                 WHERE patient_account = p.PATIENT_ACCOUNT                        
                       AND isnull(claims.deleted, 0) = 0                        
                       AND pat_status IN('N', 'R', 'B', 'C', 'D')                        
             ) AS Patient_Balance              
    --,                        
    --                CASE                        
    --                    WHEN @Patient_Alias = CAST(1 AS BIT)                        
    --                         AND pal.PATIENT_ALIAS_ID IS NOT NULL                        
    --                    THEN CAST(1 AS BIT)                        
    --                    ELSE CAST(0 AS BIT)                        
    --                END AS Is_Patient_Alias,                         
                    --pal.PATIENT_ALIAS_ID,                         
                    --pal.ALIAS_TRACKING_NUMBER,                         
                    --pal.RT_ALIAS_TRACKING,                         
                    --pal.FIRST_NAME AS FIRST_NAME_ALIAS,                         
                    --pal.LAST_NAME AS LAST_NAME_ALIAS,                         
                    --pal.MIDDLE_INITIALS AS MIDDLE_INITIALS_ALIAS               
             FROM patient AS p WITH(NOLOCK)                        
                  --LEFT JOIN FOX_TBL_PATIENT_ALIAS AS pal ON pal.PATIENT_ACCOUNT = p.Patient_Account                        
                  --                             AND ISNULL(pal.DELETED, 0) = 0                        
                  --                                          AND @Patient_Alias = CAST(1 AS BIT)                        
                  --LEFT JOIN FOX_TBL_PATIENT_ADDRESS AS pa ON pa.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT                        
                  --                                      AND pa.ADDRESS_TYPE LIKE 'Private Home'                        
                  --              AND ISNULL(pa.DELETED, 0) = 0                        
                  LEFT JOIN #CTE2 cte1 ON cte1.PATIENT_ACCOUNT = p.Patient_Account                        
                  LEFT JOIN Fox_Tbl_Patient_POS AS pos  WITH (NOLOCK)   ON pos.Patient_POS_ID = cte1.Patient_POS_ID               
                  --LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS AS loc ON loc.Loc_ID = pos.Loc_ID                                                                                  
                                                               --AND ISNULL(loc.DELETED, 0) = 0                        
                                      --AND loc.PRACTICE_CODE = @PRACTICE_CODE'            
                   LEFT JOIN FOX_TBL_PATIENT_ADDRESS AS loc  WITH (NOLOCK)   ON loc.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT                        
                                                       --AND pa.ADDRESS_TYPE LIKE 'Private Home'                        
                                                       AND ISNULL(loc.DELETED, 0) = 0  AND loc.ADDRESS_TYPE  = 'Home Address'                                          
            
                  LEFT JOIN FOX_TBL_PATIENT AS fp  WITH (NOLOCK)   ON fp.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT                        
                  AND ISNULL(fp.DELETED, 0) = 0                        
                  LEFT JOIN FOX_TBL_WORK_QUEUE AS wq  WITH (NOLOCK)   ON @WORK_ID IS NOT NULL                        
                                                               AND wq.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT                        
                                                               AND wq.WORK_ID = @WORK_ID                        
                                                               AND wq.WORK_STATUS = 'Completed'                        
                                                               AND ISNULL(wq.DELETED, 0) = 0                        
                     AND wq.PRACTICE_CODE = @PRACTICE_CODE                        
                  LEFT JOIN FOX_TBL_INTERFACE_SYNCH AS ftis  WITH (NOLOCK)  ON @WORK_ID IS NOT NULL                        
                                                               AND ftis.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT                        
                                                               AND ftis.Work_ID = wq.WORK_ID                        
                                                               AND ISNULL(ftis.DELETED, 0) = 0                        
                                                               AND ftis.PRACTICE_CODE = @PRACTICE_CODE                    
                  LEFT JOIN #CTE cte ON cte.PATIENT_ACCOUNT = p.Patient_Account                                  
                  LEFT JOIN FOX_TBL_INTERFACE_SYNCH AS ftis_p  WITH (NOLOCK)   ON ftis_p.FOX_INTERFACE_SYNCH_ID = cte.FOX_INTERFACE_SYNCH_ID                
      LEFT JOIN FOX_TBL_FINANCIAL_CLASS AS FC  WITH (NOLOCK)   ON fp.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID                        
                  AND p.Practice_Code = FC.PRACTICE_CODE                        
                                                               AND ISNULL(FC.DELETED, 0) = 0                        
             WHERE p.practice_code = @PRACTICE_CODE                                                          
                   AND (@PRACTICE_ORGANIZATION_ID IS NULL                                      
                        OR fp.PRACTICE_ORGANIZATION_ID = @PRACTICE_ORGANIZATION_ID                        
                        OR ISNULL(fp.PRACTICE_ORGANIZATION_ID, 1) = 1)                        
   AND ISNULL(p.Deleted, 0) = 0                        
                   AND (Chart_Id > '00000999'                        
                        OR Chart_Id = ''                        
                        OR Chart_Id IS NULL)                        
                   AND (fp.Patient_status IS NULL                        
                        OR fp.Patient_status NOT LIKE 'error')              
      and P.Patient_Account = @PATIENT_ACCOUNT              
     END;   
  
