IF (OBJECT_ID('FOX_PROC_GET_PR_STATES_AND_REGIONS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PR_STATES_AND_REGIONS  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PR_STATES_AND_REGIONS] @PRACTICE_CODE    BIGINT, 
                                                           @PATIENT_ACCOUNT  BIGINT, 
                                                           @CASE_ID          BIGINT      = NULL, 
                                                           @PRI_SEC_OTH_TYPE VARCHAR(50) = NULL, 
                                                           @IS_WELLNESS      BIT
WITH RECOMPILE
AS
     BEGIN
         DECLARE @HASPOSADDRESS BIT= 0, @POSSTATE VARCHAR(5) = '', @POSREGIONCODE VARCHAR(5) = '', @DISCIPLINE_CODE VARCHAR(20)= ISNULL(
         (
             SELECT D.NAME
             FROM FOX_TBL_DISCIPLINE D
                  INNER JOIN FOX_TBL_CASE C ON C.DISCIPLINE_ID = D.DISCIPLINE_ID
                                               AND C.DELETED = 0
                                               AND C.PRACTICE_CODE = @PRACTICE_CODE
             WHERE C.CASE_ID = @CASE_ID
                   AND D.PRACTICE_CODE = @PRACTICE_CODE
                   AND D.DELETED = 0
         ), '');          
         --                      
         IF OBJECT_ID('tempdb..#TEMPFOXINSURANCES') IS NOT NULL
             DROP TABLE #TEMPFOXINSURANCES;
         IF OBJECT_ID('tempdb..#TEMPPATIENTPOSADDRESS') IS NOT NULL
             DROP TABLE #TEMPPATIENTPOSADDRESS;                  
         --                     
         CREATE TABLE #TEMPPATIENTPOSADDRESS
         (POS_STATE    VARCHAR(50) NULL, 
          POS_ZIP      VARCHAR(50) NULL, 
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
          FINANCIAL_CLASS_ID      INT NULL
         );                     
         --#TEMPPATIENTPOSADDRESS                  
         INSERT INTO #TEMPPATIENTPOSADDRESS
                SELECT TOP 1 AL.State AS POS_STATE, 
                             AL.Zip AS POS_ZIP, 
                             rr.REFERRAL_REGION_NAME AS REGION_NAME, 
                             rr.REFERRAL_REGION_CODE AS REGION_CODE, 
                             rr.STATE_CODE AS REGION_STATE
                FROM FOX_TBL_CASE c
                     INNER JOIN FOX_TBL_ACTIVE_LOCATIONS AL ON AL.LOC_ID = c.POS_ID
                                                               AND AL.DELETED = 0
                     LEFT JOIN FOX_TBL_REFERRAL_REGION AS rr ON rr.REFERRAL_REGION_NAME =
                (
                    SELECT TOP 1 REFERRAL_REGION_NAME
                    FROM FOX_TBL_REFERRAL_REGION ftrr
                    WHERE ftrr.REFERRAL_REGION_NAME = AL.REGION
                          AND ftrr.DELETED = 0
                          AND RR.[IS_INACTIVE ] = 0
                          AND RR.PRACTICE_CODE = @PRACTICE_CODE
                )
                WHERE c.CASE_ID = @CASE_ID
                      AND ISNULL(c.DELETED, 0) = 0;
         IF(EXISTS
         (
             SELECT 1
             FROM #TEMPPATIENTPOSADDRESS
         ))
             BEGIN
                 SET @HASPOSADDRESS = 1;
				 SET @POSSTATE = isnull((SELECT t.POS_STATE FROM #TEMPPATIENTPOSADDRESS t), '');
				 SET @POSREGIONCODE = isnull((SELECT t.REGION_CODE FROM #TEMPPATIENTPOSADDRESS t), '');
             END;

         ----------------------------------------------------------------------------------------                  
    IF(@PRI_SEC_OTH_TYPE = 'PR')
             BEGIN
                 IF(@HASPOSADDRESS = 1)
                     BEGIN
                         IF @DISCIPLINE_CODE IN('EP')
                             BEGIN
                                 IF(
								 (@POSSTATE IN ('NY', 'CT', 'MA', 'NH', 'RI'))
                                    OR (@POSREGIONCODE LIKE 'NJN%' OR @POSREGIONCODE LIKE 'VAN%')
								   )
                                     BEGIN
                                         INSERT INTO #TEMPFOXINSURANCES
                                                SELECT FOX_TBL_INSURANCE_ID, 
                                                       INSURANCE_PAYERS_ID AS InsPayer_Id, 
                                                       INSURANCE_ID AS Insurance_Id, 
                                                       INSURANCE_NAME AS InsPayer_Description, 
                                                       isnull(ADDRESS, '') Insurance_Address, 
                                                       isnull(PHONE, '') Insurance_Phone_Number1, 
                                                       isnull(FAX, '') AS FAX, 
                                                       isnull(ZIP, '') Insurance_Zip, 
                                                       isnull(CITY, '') AS Insurance_City, 
                                                       isnull(STATE, '') Insurance_State, 
                                                       FINANCIAL_CLASS_ID
                                                FROM fox_tbl_insurance FTI
                                                WHERE FTI.INSURANCE_PAYERS_ID IN('00007', '7');
                                     END;
                                     ELSE
                                     BEGIN
                                         INSERT INTO #TEMPFOXINSURANCES
                                         SELECT FOX_TBL_INSURANCE_ID, 
                                                INSURANCE_PAYERS_ID AS InsPayer_Id, 
                                                INSURANCE_ID AS Insurance_Id, 
                                                INSURANCE_NAME AS InsPayer_Description, 
                                                isnull(ADDRESS, '') Insurance_Address, 
                                                isnull(PHONE, '') Insurance_Phone_Number1, 
                                                isnull(FAX, '') AS FAX, 
                                                isnull(ZIP, '') Insurance_Zip, 
                                                isnull(CITY, '') AS Insurance_City, 
                                                isnull(STATE, '') Insurance_State, 
                                                FINANCIAL_CLASS_ID
                                         FROM fox_tbl_insurance FTI
                                         WHERE FTI.INSURANCE_PAYERS_ID IN('00006', '6');
                                     END;
                             END;
                             ELSE
                         IF @DISCIPLINE_CODE IN('PT', 'OT', 'ST')
                             BEGIN
                                 IF(@IS_WELLNESS = 1)
                                     BEGIN
                                         IF((@POSSTATE IN('NY', 'CT', 'MA', 'NH', 'RI'))
                                            OR (@POSREGIONCODE LIKE 'NJN%' OR @POSREGIONCODE LIKE 'VAN%'))
                                             BEGIN
                                                 INSERT INTO #TEMPFOXINSURANCES
                                                        SELECT FOX_TBL_INSURANCE_ID, 
                                                               INSURANCE_PAYERS_ID AS InsPayer_Id, 
                                                               INSURANCE_ID AS Insurance_Id, 
                                                  INSURANCE_NAME AS InsPayer_Description, 
                                          isnull(ADDRESS, '') Insurance_Address, 
                                                               isnull(PHONE, '') Insurance_Phone_Number1, 
                                                               isnull(FAX, '') AS FAX, 
                                                               isnull(ZIP, '') Insurance_Zip, 
                                                               isnull(CITY, '') AS Insurance_City, 
                                                               isnull(STATE, '') Insurance_State, 
                                                               FINANCIAL_CLASS_ID
                                                        FROM fox_tbl_insurance FTI
                                                        WHERE FTI.INSURANCE_PAYERS_ID IN('00005', '5');
                                             END;
                                             ELSE
                                             BEGIN
                                                 INSERT INTO #TEMPFOXINSURANCES
                                                 SELECT FOX_TBL_INSURANCE_ID, 
                                                        INSURANCE_PAYERS_ID AS InsPayer_Id, 
                                                        INSURANCE_ID AS Insurance_Id, 
                                                        INSURANCE_NAME AS InsPayer_Description, 
                                                        isnull(ADDRESS, '') Insurance_Address, 
                                                        isnull(PHONE, '') Insurance_Phone_Number1, 
                                                        isnull(FAX, '') AS FAX, 
                                                        isnull(ZIP, '') Insurance_Zip, 
                                                        isnull(CITY, '') AS Insurance_City, 
                                                        isnull(STATE, '') Insurance_State, 
                                                        FINANCIAL_CLASS_ID
                                                 FROM fox_tbl_insurance FTI
                                                 WHERE FTI.INSURANCE_PAYERS_ID IN('00004', '4');
                                             END;
                                     END;
                                     ELSE
                                     BEGIN
                                         IF @IS_WELLNESS = 0
                                             BEGIN
                                                 IF((@POSSTATE IN('NY', 'CT', 'MA', 'NH', 'RI'))
                                            OR (@POSREGIONCODE LIKE 'NJN%' OR @POSREGIONCODE LIKE 'VAN%'))
                                                     BEGIN
                                                         INSERT INTO #TEMPFOXINSURANCES
                                                         SELECT FOX_TBL_INSURANCE_ID, 
                                                                INSURANCE_PAYERS_ID AS InsPayer_Id, 
                                                                INSURANCE_ID AS Insurance_Id, 
                                                                INSURANCE_NAME AS InsPayer_Description, 
                                                                isnull(ADDRESS, '') Insurance_Address, 
                                                                isnull(PHONE, '') Insurance_Phone_Number1, 
                                                                isnull(FAX, '') AS FAX, 
                                                                isnull(ZIP, '') Insurance_Zip, 
                                                                isnull(CITY, '') AS Insurance_City, 
                                                                isnull(STATE, '') Insurance_State, 
                                                                FINANCIAL_CLASS_ID
                                      FROM fox_tbl_insurance FTI
                                                         WHERE FTI.INSURANCE_PAYERS_ID IN('00002', '2');
                                                     END;
                                                     ELSE
                                                     BEGIN
                                                         INSERT INTO #TEMPFOXINSURANCES
                                                         SELECT FOX_TBL_INSURANCE_ID, 
                                                                INSURANCE_PAYERS_ID AS InsPayer_Id, 
                                                                INSURANCE_ID AS Insurance_Id, 
                                                                INSURANCE_NAME AS InsPayer_Description, 
                                                                isnull(ADDRESS, '') Insurance_Address, 
                                                                isnull(PHONE, '') Insurance_Phone_Number1, 
                                                                isnull(FAX, '') AS FAX, 
                                                                isnull(ZIP, '') Insurance_Zip, 
                                                                isnull(CITY, '') AS Insurance_City, 
                                                                isnull(STATE, '') Insurance_State, 
                                                                FINANCIAL_CLASS_ID
                                                         FROM fox_tbl_insurance FTI
                                                         WHERE FTI.INSURANCE_PAYERS_ID IN('00001', '1');
                                                     END;
                                             END;
                                     END;
                             END;
                             ELSE
                             BEGIN
                                 INSERT INTO #TEMPFOXINSURANCES
                                 SELECT FOX_TBL_INSURANCE_ID, 
                                        INSURANCE_PAYERS_ID AS InsPayer_Id, 
                                        INSURANCE_ID AS Insurance_Id, 
                                        INSURANCE_NAME AS InsPayer_Description, 
                                        isnull(ADDRESS, '') Insurance_Address, 
                                        isnull(PHONE, '') Insurance_Phone_Number1, 
                                        isnull(FAX, '') AS FAX, 
                                        isnull(ZIP, '') Insurance_Zip, 
                                        isnull(CITY, '') AS Insurance_City, 
                                        isnull(STATE, '') Insurance_State, 
                                        FINANCIAL_CLASS_ID
                                 FROM fox_tbl_insurance FTI
                                 WHERE FTI.INSURANCE_PAYERS_ID IN('00007', '7', '00006', '6', '00007', '7', '00007', '7', '00007', '7', '00007', '7');
                             END;
                     END;
             END;                   
         --                        
         --=============================================

         SELECT TOP 1 *
         FROM #TEMPFOXINSURANCES tfi;
     END;


