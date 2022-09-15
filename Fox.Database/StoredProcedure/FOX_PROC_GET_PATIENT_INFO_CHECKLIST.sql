IF (OBJECT_ID('FOX_PROC_GET_PATIENT_INFO_CHECKLIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_INFO_CHECKLIST  
GO    
CREATE PROCEDURE [FOX_PROC_GET_PATIENT_INFO_CHECKLIST] --1012714205377  
@PATIENT_ACCOUNT BIGINT  
AS  
     BEGIN  
         IF OBJECT_ID('TEMPDB..#TEMPFOXPATIENTCHECKLIST') IS NOT NULL  
             BEGIN  
                 DROP TABLE #TEMPFOXPATIENTCHECKLIST;  
             END;  
         SELECT CASE  
                    WHEN ISNULL(P.LAST_NAME, '') = ''  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS [LASTNAME],  
                CASE  
                    WHEN ISNULL(P.FIRST_NAME, '') = ''  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS [FIRSTNAME],  
                CASE  
                    WHEN ISNULL(P.GENDER, '') = ''  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS [GENDER],  
                CASE  
                    WHEN P.DATE_OF_BIRTH IS NULL  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS [DOB],  
                CASE  
                    WHEN ISNULL(P.HOME_PHONE, '') = ''  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS HOMEPHONE,  
                CASE  
                    WHEN ISNULL(P.CELL_PHONE, '') = ''  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS CELLPHONE,  
                CASE  
                    WHEN PPOS.PATIENT_POS_ID IS NULL  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS PLACEOFSERVICE,  
                CASE  
                    WHEN PC.CONTACT_ID IS NULL  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS CONTACTPOA,  
                CASE  
                    WHEN PC.CONTACT_ID IS NULL  
                    THEN CAST(0 AS BIT)  
                    ELSE CAST(1 AS BIT)  
                END AS CHECKELIGIBILITY,  
                CASE  
                    WHEN EXISTS  
         (  
             SELECT PATIENT_INSURANCE_ID  
             FROM FOX_TBL_PATIENT_INSURANCE  
             WHERE PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
                   AND FOX_INSURANCE_STATUS = 'C'  
                   AND ISNULL(PRI_SEC_OTH_TYPE, '') = 'P'  
                   AND ISNULL(DELETED, 0) = 0  
         )  
                    THEN CAST(1 AS BIT)  
                    ELSE CAST(0 AS BIT)  
                END AS PRIMARYINS,  
                CASE  
                    WHEN EXISTS  
         (  
             SELECT PATIENT_INSURANCE_ID  
             FROM FOX_TBL_PATIENT_INSURANCE  
             WHERE PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
                   AND FOX_INSURANCE_STATUS = 'C'  
                   AND ISNULL(PRI_SEC_OTH_TYPE, '') = 'S'  
                   AND ISNULL(DELETED, 0) = 0  
         )  
                    THEN CAST(1 AS BIT)  
                    ELSE CAST(0 AS BIT)  
                END AS SECONDARYINS,  
                CASE  
                    WHEN EXISTS  
         (  
             SELECT PATIENT_INSURANCE_ID  
             FROM FOX_TBL_PATIENT_INSURANCE  
             WHERE PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
                   AND FOX_INSURANCE_STATUS = 'C'  
                   AND ISNULL(PRI_SEC_OTH_TYPE, '') = 'T'  
                   AND ISNULL(DELETED, 0) = 0  
         )  
                    THEN CAST(1 AS BIT)  
                    ELSE CAST(0 AS BIT)  
                END AS TERTIARYINS,  
                CASE  
                    WHEN EXISTS  
         (  
             SELECT PATIENT_INSURANCE_ID  
             FROM FOX_TBL_PATIENT_INSURANCE  
             WHERE PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
                   AND FOX_INSURANCE_STATUS = 'C'  
                   AND ISNULL(PRI_SEC_OTH_TYPE, '') = 'PR' -- AND ISNULL(IS_PRIVATE_PAY,0) = 0        
                   AND ISNULL(DELETED, 0) = 0  
         )  
                    THEN CAST(1 AS BIT)  
                    ELSE CAST(0 AS BIT)  
                END AS PATIENTPAY,  
                CASE  
                    WHEN DATEDIFF(DAY,  
         (  
             SELECT TOP 1 ISNULL(ELIG_LOADED_ON, GETDATE())  
             FROM FOX_TBL_PATIENT_INSURANCE  
             WHERE PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
                   AND ISNULL(DELETED, 0) = 0  
                   AND ISNULL(PRI_SEC_OTH_TYPE, '') = 'P'  
                   AND ISNULL(FOX_INSURANCE_STATUS, '') = 'C'  
                   AND ISNULL(INACTIVE, 0) = 0  
                   AND ELIG_LOADED_ON IS NOT NULL  
         ), GETDATE()) < 14  
                    THEN CAST(1 AS BIT)  
                    ELSE CAST(0 AS BIT)  
                END AS PRIELIGIBILITYCHECKED,  
                CASE  
                    WHEN DATEDIFF(DAY,  
         (  
             SELECT TOP 1 ISNULL(ELIG_LOADED_ON, GETDATE())  
             FROM FOX_TBL_PATIENT_INSURANCE  
             WHERE PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
                   AND ISNULL(DELETED, 0) = 0  
                   AND ISNULL(PRI_SEC_OTH_TYPE, '') = 'S'  
                   AND ISNULL(FOX_INSURANCE_STATUS, '') = 'C'  
                   AND ISNULL(INACTIVE, 0) = 0  
                   AND ELIG_LOADED_ON IS NOT NULL  
         ), GETDATE()) < 14  
                    THEN CAST(1 AS BIT)  
                    ELSE CAST(0 AS BIT)  
                END AS SECELIGIBILITYCHECKED,  
                CASE  
                    WHEN DATEDIFF(DAY,  
         (  
             SELECT TOP 1 ISNULL(ELIG_LOADED_ON, GETDATE())  
             FROM FOX_TBL_PATIENT_INSURANCE  
             WHERE PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
                   AND ISNULL(DELETED, 0) = 0  
                   AND ISNULL(PRI_SEC_OTH_TYPE, '') = 'T'  
                   AND ISNULL(FOX_INSURANCE_STATUS, '') = 'C'  
                   AND ISNULL(INACTIVE, 0) = 0  
                   AND ELIG_LOADED_ON IS NOT NULL  
         ), GETDATE()) < 14  
                    THEN CAST(1 AS BIT)  
                    ELSE CAST(0 AS BIT)  
                END AS TERELIGIBILITYCHECKED  
         INTO #TEMPFOXPATIENTCHECKLIST  
         FROM PATIENT P  
              LEFT JOIN FOX_TBL_PATIENT_POS PPOS ON P.PATIENT_ACCOUNT = PPOS.PATIENT_ACCOUNT  
                                                    AND ISNULL(PPOS.DELETED, 0) = 0  
              LEFT JOIN FOX_TBL_PATIENT_CONTACTS PC ON PC.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
                                                       AND ISNULL(PC.DELETED, 0) = 0  
                                                       AND (PC.FLAG_FINANCIALLY_RESPONSIBLE_PARTY = 1  
                                                            OR FLAG_POWER_OF_ATTORNEY_FINANCIAL = 1)  
         WHERE P.PATIENT_ACCOUNT = @PATIENT_ACCOUNT  
               AND ISNULL(P.DELETED, 0) = 0;  
         SELECT TOP 1 *  
         FROM #TEMPFOXPATIENTCHECKLIST;  
     END;	