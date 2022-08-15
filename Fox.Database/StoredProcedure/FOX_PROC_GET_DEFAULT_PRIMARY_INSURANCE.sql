IF (OBJECT_ID('FOX_PROC_GET_DEFAULT_PRIMARY_INSURANCE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_DEFAULT_PRIMARY_INSURANCE 
GO    
-----------------------------------------------------------------------------  
-- =============================================          
-- AUTHOR:  <DEVELOPER, YOUSAF>          
-- CREATE DATE: <CREATE DATE, 10/22/2018>          
-- DESCRIPTION: <GET OPEN ISSUE; GET PATIENT DEFAULT PRIMARY INSURANCE>         
-- FOX_PROC_GET_DEFAULT_PRIMARY_INSURANCE 1011163, 101116354812748, '080571198', 'NJ'    
    
CREATE PROCEDURE [dbo].[FOX_PROC_GET_DEFAULT_PRIMARY_INSURANCE]    
(@PRACTICE_CODE   BIGINT NULL,     
 @PATIENT_ACCOUNT BIGINT NULL,     
 @ZIP             VARCHAR(10) NULL,     
 @STATE           VARCHAR(10) NULL    
)    
AS    
     BEGIN    
         IF OBJECT_ID('TEMPDB..#TEMP_FOX_TBL_PATIENT_ADDRESS') IS NOT NULL    
             DROP TABLE #TEMP_FOX_TBL_PATIENT_ADDRESS;    
         IF OBJECT_ID('TEMPDB..#TEMP_FOX_TBL_INSURANCE') IS NOT NULL    
             DROP TABLE #TEMP_FOX_TBL_INSURANCE;    
    
         CREATE TABLE #TEMP_FOX_TBL_PATIENT_ADDRESS    
         (    
   ZIP   VARCHAR(9) NULL,    
   STATE CHAR(2) NULL    
         );    
    
         IF(@PATIENT_ACCOUNT IS NOT NULL)    
             BEGIN    
                 INSERT INTO #TEMP_FOX_TBL_PATIENT_ADDRESS    
                        SELECT TOP 1 ZIP, STATE    
                        FROM FOX_TBL_PATIENT_ADDRESS    
                        WHERE ISNULL(DELETED, 0) = 0    
                              AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
                              AND ADDRESS_TYPE LIKE '%HOME%';    
             END;    
         ELSE    
             BEGIN    
                 INSERT INTO #TEMP_FOX_TBL_PATIENT_ADDRESS    
                 VALUES (@ZIP, @STATE)    
             END;    
    
         SELECT TOP (1) I.*    
         INTO #TEMP_FOX_TBL_INSURANCE    
         FROM #TEMP_FOX_TBL_PATIENT_ADDRESS TPA    
              LEFT JOIN FOX_TBL_REGION_ZIPCODE_DATA RZC ON SUBSTRING(TPA.ZIP, 1, 5) = RZC.ZIP_CODE    
                                                           AND TPA.STATE = RZC.STATE    
                                                           AND ISNULL(RZC.DELETED, 0) = 0    
              LEFT JOIN FOX_TBL_PRIMARY_PAYER PP ON RZC.INSURANCE_CODE = PP.INSURANCE_CODE    
                                                    AND ISNULL(PP.DELETED, 0) = 0    
              LEFT JOIN FOX_TBL_INSURANCE I ON PP.PAYER_NAME LIKE '%'+I.INSURANCE_NAME+'%'    
         WHERE RZC.PRACTICE_CODE = @PRACTICE_CODE    
               AND PP.PRACTICE_CODE = @PRACTICE_CODE;    
         SELECT II.FOX_TBL_INSURANCE_ID,     
                IP.INSPAYER_ID,     
                I.INSURANCE_ID,     
                II.INSURANCE_NAME AS INSPAYER_DESCRIPTION,     
                I.INSURANCE_ADDRESS,     
                I.INSURANCE_PHONE_NUMBER1,     
                I.INSURANCE_PHONE_NUMBER2 AS FAX,     
                I.INSURANCE_ZIP,     
                I.INSURANCE_CITY,     
                I.INSURANCE_STATE    
         FROM INSURANCE_PAYERS IP    
              JOIN INSURANCES I ON IP.INSPAYER_ID = I.INSPAYER_ID    
              JOIN FOX_TBL_INSURANCE II ON I.INSURANCE_ID = CONVERT(BIGINT, II.INSURANCE_ID)    
              JOIN #TEMP_FOX_TBL_INSURANCE TI ON I.INSURANCE_ID = TI.INSURANCE_ID --AND I.STATE = TI.INSURANCE_ID    
         WHERE II.FOX_TBL_INSURANCE_ID = TI.FOX_TBL_INSURANCE_ID;    
     END; 