IF (OBJECT_ID('FOX_PROC_GET_MC_SUGGESTED_PAYER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_MC_SUGGESTED_PAYER  
GO    
--dbo.FOX_PROC_GET_MC_SUGGESTED_PAYER 1011163,101116354814399,'',''      
CREATE PROCEDURE dbo.FOX_PROC_GET_MC_SUGGESTED_PAYER      
(@PRACTICE_CODE   BIGINT,       
 @PATIENT_ACCOUNT BIGINT      = NULL,       
 @ZIP             VARCHAR(10) = NULL,       
 @STATE           VARCHAR(10) = NULL      
)      
WITH RECOMPILE      
AS                
    ---------------------------------------------                
    --DECLARE             
    -- @PRACTICE_CODE   BIGINT = 1011163,                 
    -- @PATIENT_ACCOUNT BIGINT = 101116354813128,               
    -- @ZIP  VARCHAR(10) = '07001'              
    ---------------------------------------------                
     BEGIN      
         DECLARE @HASPOSADDRESS BIT= 0;      
         IF OBJECT_ID('tempdb..#TEMPPOSADDRESS') IS NOT NULL      
             DROP TABLE #TEMPPOSADDRESS;      
         IF OBJECT_ID('tempdb..#TEMPFOXINSURANCE') IS NOT NULL      
             DROP TABLE #TEMPFOXINSURANCE;      
         CREATE TABLE #TEMPPOSADDRESS      
         (POS_STATE VARCHAR(10) NULL,       
          POS_ZIP   VARCHAR(10) NULL      
         );      
         CREATE TABLE #TEMPFOXINSURANCE      
         (FOX_TBL_INSURANCE_ID    BIGINT,       
          InsPayer_Id             VARCHAR(10),       
          Insurance_Id            BIGINT,       
          InsPayer_Description    VARCHAR(100) NULL,       
          Insurance_Address       VARCHAR(100) NULL,       
          Insurance_Phone_Number1 VARCHAR(20) NULL,       
          FAX                     VARCHAR(20) NULL,       
          Insurance_Zip           VARCHAR(9) NULL,       
          Insurance_City          VARCHAR(50) NULL,       
          Insurance_State         VARCHAR(2) NULL,       
          FINANCIAL_CLASS_ID      INT NULL      
         );                           
         --#TEMPPATIENTPOSADDRESS                     
         IF(ISNULL(@ZIP, '') = '')      
             BEGIN      
                 INSERT INTO #TEMPPOSADDRESS      
                        SELECT TOP 1 CASE      
                                         WHEN AL.NAME NOT LIKE 'private home'      
                                         THEN AL.State      
                                         ELSE pa.STATE      
                                     END AS POS_STATE,      
                                     CASE      
                                         WHEN AL.NAME NOT LIKE 'private home'      
                                         THEN AL.Zip      
                                         ELSE pa.ZIP      
                                     END AS POS_ZIP      
                        FROM Fox_Tbl_Patient_POS PPOS      
                             INNER JOIN FOX_TBL_ACTIVE_LOCATIONS AL ON AL.LOC_ID = PPOS.Loc_ID      
                                                                       AND AL.DELETED = 0      
                             LEFT JOIN fox_tbl_patient_Address pa ON pa.PATIENT_POS_ID = PPOS.Patient_POS_ID      
                                                                     AND pa.PATIENT_ACCOUNT = PPOS.Patient_Account      
                        WHERE PPOS.PATIENT_ACCOUNT = @PATIENT_ACCOUNT      
                              AND ISNULL(PPOS.DELETED, 0) = 0      
                              AND ISNULL(PPOS.Is_Default, 0) = CASE      
                                                                   WHEN EXISTS      
                        (      
                            SELECT TOP 1 *      
                            FROM Fox_Tbl_Patient_POS      
                            WHERE Patient_Account = @PATIENT_ACCOUNT      
                                  AND ISNULL(PPOS.DELETED, 0) = 0      
                                  AND ISNULL(Is_Default, 0) = 1      
                        )      
                                                                   THEN 1      
                               ELSE 0      
                                                               END;      
             END;      
        ELSE      
             BEGIN      
                 INSERT INTO #TEMPPOSADDRESS      
                 SELECT @STATE AS POS_STATE,       
             @ZIP AS POS_ZIP      
             END;      
         IF(EXISTS      
         (      
             SELECT 1      
             FROM #TEMPPOSADDRESS      
         ))      
           BEGIN      
                 SET @HASPOSADDRESS = 1;      
             END;      
         IF(@HASPOSADDRESS = 1)      
             BEGIN      
                 INSERT INTO #TEMPFOXINSURANCE      
                        SELECT fti.FOX_TBL_INSURANCE_ID,       
                               fti.INSURANCE_PAYERS_ID AS InsPayer_Id,       
                               fti.INSURANCE_ID AS Insurance_Id,       
                               fti.INSURANCE_NAME AS InsPayer_Description,       
                               isnull(fti.ADDRESS, '') Insurance_Address,       
                               isnull(fti.PHONE, '') Insurance_Phone_Number1,       
                               isnull(fti.FAX, '') AS FAX,       
                               isnull(fti.ZIP, '') Insurance_Zip,       
                               isnull(fti.CITY, '') AS Insurance_City,       
                               isnull(fti.STATE, '') Insurance_State,       
                               fti.FINANCIAL_CLASS_ID      
                        FROM #TEMPPOSADDRESS TPA      
                             INNER JOIN FOX_TBL_ZIP_LOCALITY_DATA ld ON ld.State = TPA.POS_STATE      
                                                                        AND ld.ZIP_Code = SUBSTRING(TPA.POS_ZIP, 1, 5)      
                                                                        AND ld.DELETED = 0      
                             INNER JOIN dbo.FOX_TBL_INSURANCE fti ON RIGHT('0'+ISNULL(ld.Locality,''),2) = RIGHT('0'+ISNULL(fti.CARRIER_LOCALITY,''),2)      
                                                                     AND ld.State = fti.CARRIER_STATE      
                                                                     AND ld.Carrier = fti.CARRIER      
                             INNER JOIN FOX_TBL_FINANCIAL_CLASS FC ON FC.FINANCIAL_CLASS_ID = fti.FINANCIAL_CLASS_ID       
                        --INNER JOIN FOX_TBL_PRIMARY_PAYER PP ON RZC.INSURANCE_CODE = PP.INSURANCE_CODE          
                        --                                       AND PP.DELETED = 0          
                        --                                       AND PP.COUNTIES LIKE CASE          
                        --                                                                WHEN isnull(PP.COUNTIES, '') NOT LIKE 'all counties'          
                        --                                                                     AND isnull(PP.COUNTIES, '') NOT LIKE 'All remaining counties'          
                        --                                                                THEN '%'+LTRIM(RTRIM(replace(RZC.COUNTY_CITY, ' County', '')))+'%'          
                        --                                                                ELSE PP.COUNTIES          
                        --                                                            END          
                        --INNER JOIN dbo.FOX_TBL_INSURANCE fti ON PP.INSURANCE_CODE = fti.INSURANCE_PAYERS_ID          
                        --INNER JOIN dbo.FOX_TBL_INSURANCE fti ON PP.PAYER_NAME LIKE '%'+LTRIM(RTRIM(REPLACE(FTI.INSURANCE_NAME, '*', '')))+'%'                
                        --INNER JOIN FOX_TBL_FINANCIAL_CLASS FC ON FC.FINANCIAL_CLASS_ID = fti.FINANCIAL_CLASS_ID          
                        WHERE ISNULL(fti.PRACTICE_CODE, 0) = @PRACTICE_CODE      
                              AND FC.PRACTICE_CODE = @PRACTICE_CODE      
                              AND ISNULL(FC.CODE, '') = 'MC'      
         AND FTI.PAYING_AGENCY NOT LIKE 'RAIL%';      
             END;      
         SELECT TOP 1 *      
         FROM #TEMPFOXINSURANCE;      
     END 