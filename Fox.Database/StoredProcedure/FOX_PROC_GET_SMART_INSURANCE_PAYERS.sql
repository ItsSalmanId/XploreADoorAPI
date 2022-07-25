IF (OBJECT_ID('FOX_PROC_GET_SMART_INSURANCE_PAYERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_INSURANCE_PAYERS  
GO 
CREATE PROCEDURE [DBO].[FOX_PROC_GET_SMART_INSURANCE_PAYERS] --1011163, 0, 'novitas de', 544105,'P', null, 'nj'                                                  
@PRACTICE_CODE      BIGINT,         
@PATIENT_ACCOUNT    BIGINT,         
@SEARCH_VALUE       VARCHAR(100) = NULL,         
@FINANCIAL_CLASS_ID BIGINT       = NULL,         
@PRI_SEC_OTH_TYPE   VARCHAR(50)  = NULL,         
@ZIP                VARCHAR(10)  = NULL,         
@STATE              VARCHAR(10)  = NULL        
WITH RECOMPILE        
AS        
     BEGIN        
         DECLARE @HASPOSADDRESS BIT= 0, @FINANCIAL_CLASS_CODE VARCHAR(20)= ISNULL(        
         (        
             SELECT TOP 1 CODE        
             FROM FOX_TBL_FINANCIAL_CLASS        
             WHERE FINANCIAL_CLASS_ID = @FINANCIAL_CLASS_ID        
                   AND PRACTICE_CODE = @PRACTICE_CODE        
                   AND ISNULL(DELETED, 0) = 0        
         ), '');        
        
         --                                            
         IF(@SEARCH_VALUE = '')        
             BEGIN        
                 SET @SEARCH_VALUE = NULL;        
             END;        
             ELSE        
             BEGIN        
     SET @SEARCH_VALUE = LTRIM(RTRIM(@SEARCH_VALUE));        
                 IF CHARINDEX('[', @SEARCH_VALUE) > 0        
                     BEGIN        
                         SET @SEARCH_VALUE = Replace(@SEARCH_VALUE, '[', '');        
                     END;        
                 IF CHARINDEX(']', @SEARCH_VALUE) > 0        
                     BEGIN        
                         SET @SEARCH_VALUE = Replace(@SEARCH_VALUE, ']', '');        
                     END;        
                 IF CHARINDEX('*', @SEARCH_VALUE) > 0        
                     BEGIN        
                         SET @SEARCH_VALUE = Replace(@SEARCH_VALUE, '*', '');        
                     END;        
     IF CHARINDEX(' ', @SEARCH_VALUE) > 0        
                     BEGIN        
                         SET @SEARCH_VALUE = Replace(@SEARCH_VALUE, ' ', '%');        
                     END;        
                 --SET @SEARCH_VALUE = LTRIM(RTRIM(@SEARCH_VALUE));        
             END;                                                    
         --                                              
         IF OBJECT_ID('tempdb..#TEMP_INSURANCES') IS NOT NULL        
             DROP TABLE #TEMP_INSURANCES;        
         IF OBJECT_ID('tempdb..#TEMPFOXINSURANCES') IS NOT NULL        
             DROP TABLE #TEMPFOXINSURANCES;        
         IF OBJECT_ID('tempdb..#TEMPPATIENTPOSADDRESS') IS NOT NULL        
             DROP TABLE #TEMPPATIENTPOSADDRESS;                                          
         --                                             
         CREATE TABLE #TEMPPATIENTPOSADDRESS        
         (POS_STATE    VARCHAR(10) NULL,         
          POS_ZIP      VARCHAR(10) NULL,         
          REGION_NAME  VARCHAR(100) NULL,         
          REGION_CODE  VARCHAR(100) NULL,         
          REGION_STATE VARCHAR(10) NULL        
         );        
         CREATE TABLE #TEMPFOXINSURANCES        
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
          FINANCIAL_CLASS_ID      INT NULL,         
          CARRIER_STATE           VARCHAR(2) NULL        
         );                                                
         --#TEMPPATIENTPOSADDRESS                  
         IF(ISNULL(@STATE, '') = '')        
             BEGIN        
                 INSERT INTO #TEMPPATIENTPOSADDRESS        
          SELECT TOP 1 CASE        
                                         WHEN AL.NAME NOT LIKE 'private home'        
                                         THEN AL.State        
                                         ELSE pa.STATE        
                                     END AS POS_STATE,        
                                     CASE        
                               WHEN AL.NAME NOT LIKE 'private home'        
                                         THEN AL.Zip        
                                         ELSE pa.ZIP        
                                     END AS POS_ZIP,         
                                 isnull(rr.REFERRAL_REGION_NAME,'') AS REGION_NAME,         
                                     isnull(rr.REFERRAL_REGION_CODE,'') AS REGION_CODE,         
                                     isnull(rr.STATE_CODE,'') AS REGION_STATE        
                        FROM Fox_Tbl_Patient_POS PPOS        
                             INNER JOIN FOX_TBL_ACTIVE_LOCATIONS AL ON AL.LOC_ID = PPOS.Loc_ID        
                                                                       AND ISNULL(AL.DELETED, 0) = 0        
                    AND ISNULL(AL.IS_ACTIVE, 0) = 1        
                                                                       AND AL.PRACTICE_CODE = @PRACTICE_CODE        
                             LEFT JOIN fox_tbl_patient_Address pa ON pa.PATIENT_POS_ID = PPOS.Patient_POS_ID        
                                                                     AND pa.PATIENT_ACCOUNT = PPOS.Patient_Account        
                             LEFT JOIN FOX_TBL_REFERRAL_REGION AS rr ON rr.REFERRAL_REGION_NAME =        
                        (        
                            SELECT TOP 1 REFERRAL_REGION_NAME        
                            FROM FOX_TBL_REFERRAL_REGION AS ftrr        
                            WHERE ftrr.REFERRAL_REGION_NAME = CASE        
                                                                  WHEN AL.NAME NOT LIKE 'private home'        
                                                                  THEN AL.REGION        
                                                                  ELSE pa.POS_REGION        
                                                              END        
                                  AND ISNULL(ftrr.DELETED, 0) = 0        
                                  AND ftrr.PRACTICE_CODE = @PRACTICE_CODE        
                                  AND ISNULL(ftrr.[IS_INACTIVE], 0) = 0        
                        )                
                                                                        --(                
                                                                        --    SELECT TOP 1 REFERRAL_REGION_NAME                
                                                                        --    FROM FOX_TBL_REFERRAL_REGION AS ftrr                
                                                                        --    WHERE ftrr.REFERRAL_REGION_NAME = AL.REGION                
                                                                        --          AND ISNULL(ftrr.DELETED, 0) = 0                
                                                                        --          AND ftrr.PRACTICE_CODE = @PRACTICE_CODE                
                                                                        --          AND ISNULL(ftrr.[IS_INACTIVE], 0) = 0                
                                                                        --)                
        
                                                                        AND ISNULL(RR.DELETED, 0) = 0        
    AND RR.PRACTICE_CODE = @PRACTICE_CODE        
                        WHERE PPOS.PATIENT_ACCOUNT = @PATIENT_ACCOUNT        
                              AND ISNULL(PPOS.DELETED, 0) = 0        
                              AND ISNULL(PPOS.Is_Default, 0) = CASE        
                                                                   WHEN EXISTS        
          (        
                            SELECT TOP 1 *        
                            FROM Fox_Tbl_Patient_POS        
                            WHERE Patient_Account = @PATIENT_ACCOUNT        
                                  AND ISNULL(DELETED, 0) = 0        
                             AND ISNULL(Is_Default, 0) = 1        
                        )        
                                                                   THEN 1        
                                                                   ELSE 0        
                END;        
             END;        
             ELSE        
             BEGIN        
                 INSERT INTO #TEMPPATIENTPOSADDRESS        
                 SELECT @STATE AS POS_STATE,         
                        '' AS POS_ZIP,         
                        '' AS REGION_NAME,         
                        '' AS REGION_CODE,         
                        '' AS REGION_STATE;        
             END;        
         IF(EXISTS        
         (        
             SELECT 1        
             FROM #TEMPPATIENTPOSADDRESS        
         ))        
             BEGIN        
                 SET @HASPOSADDRESS = 1;        
             END;                                            
         --#TEMP_INSURANCES                                              
         SELECT i.FOX_TBL_INSURANCE_ID,         
                i.INSURANCE_PAYERS_ID AS InsPayer_Id,         
                i.INSURANCE_ID AS Insurance_Id,         
                i.INSURANCE_NAME AS InsPayer_Description,         
                isnull(i.ADDRESS, '') Insurance_Address,         
                isnull(i.PHONE, '') Insurance_Phone_Number1,         
                isnull(i.FAX, '') AS FAX,         
                isnull(i.ZIP, '') Insurance_Zip,         
                isnull(i.CITY, '') AS Insurance_City,         
                isnull(i.STATE, '') Insurance_State,         
                i.FINANCIAL_CLASS_ID,         
                isnull(i.CARRIER_STATE, '') CARRIER_STATE        
         INTO #TEMP_INSURANCES        
         FROM FOX_TBL_INSURANCE i        
   left join Insurances AS INS ON INS.Insurance_Id = I.INSURANCE_ID AND isnull(INS.DELETED, 0) = 0                                                                 
         --left join FOX_TBL_FINANCIAL_CLASS fc on fc.FINANCIAL_CLASS_ID = i.FINANCIAL_CLASS_ID                                              
         WHERE i.INSURANCE_NAME LIKE '%'+@SEARCH_VALUE+'%'        
               AND i.PRACTICE_CODE = @PRACTICE_CODE        
               AND isnull(i.DELETED, 0) = 0        
       AND isnull(INS.InActive, 0) = 0     ;                                       
         --FROM INSURANCE_PAYERS ip                                            
         --     JOIN INSURANCES i ON ip.InsPayer_Id = i.InsPayer_Id                                            
         --     JOIN FOX_TBL_INSURANCE ii ON i.Insurance_Id = CONVERT(BIGINT, ii.INSURANCE_ID)                                            
         --                                  AND ii.INSURANCE_NAME LIKE '%'+@SEARCH_VALUE+'%'                                            
         ----------------------------------------------------------------------------------------                                          
         IF(@FINANCIAL_CLASS_CODE IN('PR', 'PP'))        
             BEGIN        
                 IF @FINANCIAL_CLASS_CODE IN('PR')        
                     BEGIN        
                         INSERT INTO #TEMPFOXINSURANCES        
                              SELECT *        
                                FROM #TEMP_INSURANCES FTI        
                                WHERE FTI.InsPayer_Id IN('00003');        
                     END;        
                     ELSE        
                     BEGIN        
                         IF(@HASPOSADDRESS = 1        
                            AND (isnull(        
                         (        
                             SELECT t.REGION_STATE        
                             FROM #TEMPPATIENTPOSADDRESS t        
                         ), '') <> ''))        
                             BEGIN        
                                 IF(ISNULL(        
                                 (        
                                     SELECT t.REGION_STATE        
                                     FROM #TEMPPATIENTPOSADDRESS t        
                                 ), '') IN('NY', 'CT'))        
                                     BEGIN        
                                         INSERT INTO #TEMPFOXINSURANCES        
                                         SELECT *        
                                         FROM #TEMP_INSURANCES FTI        
                                         WHERE FTI.InsPayer_Id IN('00002', '00005', '00006');        
                                     END;        
                                     ELSE        
                                     BEGIN        
      IF(ISNULL(        
                                         (        
                                             SELECT t.REGION_STATE        
                                           FROM #TEMPPATIENTPOSADDRESS t        
                                         ), '') IN('NJ', 'VA')        
                                            AND ISNULL(        
                                         (        
                                             SELECT t.REGION_CODE        
                                             FROM #TEMPPATIENTPOSADDRESS t        
                                         ), '') IN('NJNE1', 'NJN1', 'NJN2', 'VAN', 'VAN2', 'VAN3'))        
                                             BEGIN        
                                                 INSERT INTO #TEMPFOXINSURANCES        
                                                 SELECT *        
                                                 FROM #TEMP_INSURANCES FTI        
                                                 WHERE FTI.InsPayer_Id IN('00002', '00005', '00006');        
                                             END;        
                                             ELSE        
                                             BEGIN        
                                                 INSERT INTO #TEMPFOXINSURANCES        
                                                 SELECT *        
                                                 FROM #TEMP_INSURANCES FTI        
                                                 WHERE FTI.InsPayer_Id IN('00001', '00004', '00007');        
                                             END;        
                                     END;        
                             END;        
                             ELSE        
                             BEGIN        
                                 INSERT INTO #TEMPFOXINSURANCES        
                                 SELECT *        
                                 FROM #TEMP_INSURANCES FTI        
                                 WHERE FTI.InsPayer_Id IN('00001', '00002', '00004', '00005', '00006', '00007');        
                             END;        
                     END;        
             END;        
             ELSE        
             BEGIN        
                 IF @FINANCIAL_CLASS_CODE IN('MC','MD')        
                     BEGIN        
                         IF(@HASPOSADDRESS = 1        
                       AND (        
                         (        
                             SELECT isnull(t.POS_STATE, '')        
                             FROM #TEMPPATIENTPOSADDRESS t        
                         ) <> ''))        
                             BEGIN        
                                 INSERT INTO #TEMPFOXINSURANCES        
                                 SELECT FTI.*        
                                 FROM #TEMP_INSURANCES FTI        
             --INNER JOIN #TEMPPATIENTPOSADDRESS TPA ON TPA.POS_STATE = FTI.Insurance_State        
                                      INNER JOIN #TEMPPATIENTPOSADDRESS TPA ON TPA.POS_STATE = FTI.CARRIER_STATE        
                                 WHERE FTI.FINANCIAL_CLASS_ID = @FINANCIAL_CLASS_ID;        
                             END;        
                             ELSE        
                             BEGIN        
                                 INSERT INTO #TEMPFOXINSURANCES        
                                 SELECT FTI.*        
                                 FROM #TEMP_INSURANCES FTI        
                                 WHERE FTI.FINANCIAL_CLASS_ID = @FINANCIAL_CLASS_ID;        
                             END        
                     END        
                     ELSE        
                     BEGIN        
                         --IF @FINANCIAL_CLASS_CODE IN('MC')        
                         --    BEGIN        
                         --        IF(@HASPOSADDRESS = 1        
                         --           AND (        
                         --        (        
                         --            SELECT isnull(t.POS_STATE, '')        
                         --            FROM #TEMPPATIENTPOSADDRESS t        
                         --        ) <> ''))        
                         --            BEGIN        
                         --                INSERT INTO #TEMPFOXINSURANCES        
                         --          SELECT DISTINCT         
                         --                       FTI.FOX_TBL_INSURANCE_ID,         
--                       FTI.InsPayer_Id,         
                         --                       FTI.Insurance_Id,         
                         --                       FTI.InsPayer_Description,         
                         --      isnull(FTI.Insurance_Address, '') Insurance_Address,         
                         --                       isnull(FTI.Insurance_Phone_Number1, '') Insurance_Phone_Number1,         
                         --                       isnull(FTI.FAX, '') FAX,         
                         --                       isnull(FTI.Insurance_Zip, '') Insurance_Zip,         
                         --                       isnull(FTI.Insurance_City, '') Insurance_City,         
                         --                       isnull(FTI.Insurance_State, '') Insurance_State,         
                         --                       FTI.FINANCIAL_CLASS_ID,         
                         --                       isnull(FTI.CARRIER_STATE, '') CARRIER_STATE        
                         --                FROM #TEMP_INSURANCES FTI        
                         --                     INNER JOIN #TEMPPATIENTPOSADDRESS TPA ON TPA.POS_STATE = FTI.CARRIER_STATE        
                         --                     --INNER JOIN FOX_TBL_ZIP_LOCALITY_DATA ld ON ld.State = TPA.POS_STATE        
                         --                WHERE FTI.FINANCIAL_CLASS_ID = @FINANCIAL_CLASS_ID;        
                         --            END;        
                         --            ELSE        
                         --            BEGIN        
                         --                INSERT INTO #TEMPFOXINSURANCES        
                         --                SELECT FTI.*        
                         --                FROM #TEMP_INSURANCES FTI        
                         --                WHERE FTI.FINANCIAL_CLASS_ID = @FINANCIAL_CLASS_ID;        
                         --            END        
                         --    END        
                         --    ELSE        
                         --    BEGIN        
                                 IF @FINANCIAL_CLASS_CODE IN('CI', 'WC', 'AI', 'MCHMO', 'MA')        
                                     BEGIN        
                                         INSERT INTO #TEMPFOXINSURANCES        
                                         SELECT FTI.*        
                                      FROM #TEMP_INSURANCES FTI        
                                         WHERE FTI.FINANCIAL_CLASS_ID = @FINANCIAL_CLASS_ID;        
                                     END;        
                                     ELSE        
                                     BEGIN        
                                         INSERT INTO #TEMPFOXINSURANCES        
                                         SELECT TOP 100 FTI.*        
    FROM #TEMP_INSURANCES FTI        
                                     END        
                             --END        
                     END        
             END;                                           
         --                                                
         SELECT *        
         FROM #TEMPFOXINSURANCES tfi;        
     END;  
