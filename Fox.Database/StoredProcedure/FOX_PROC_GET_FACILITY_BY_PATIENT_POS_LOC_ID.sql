IF (OBJECT_ID('FOX_PROC_GET_FACILITY_BY_PATIENT_POS_LOC_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_FACILITY_BY_PATIENT_POS_LOC_ID 
GO  
CREATE PROCEDURE [FOX_PROC_GET_FACILITY_BY_PATIENT_POS_LOC_ID] @PRACTICE_CODE   BIGINT,     
                                                              @PATIENT_ACCOUNT BIGINT    
AS    
     BEGIN    
         DECLARE @LOC_ID BIGINT;    
         DECLARE @POS_ID BIGINT;    
    IF OBJECT_ID('tempdb..#temp_fox_pos') IS NOT NULL    
             DROP TABLE #temp_fox_pos;    
         SELECT TOP 1 LOC_ID,     
                      Patient_POS_ID    
         INTO #temp_fox_pos    
         FROM FOX_TBL_PATIENT_POS    
         WHERE Patient_Account = @PATIENT_ACCOUNT    
               AND isnull(Deleted, 0) = 0    
               AND isnull(Is_Default, 0) = CASE    
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
         --AND (isnull(Is_Default, 0) = 1    
         --OR Patient_POS_ID = (select top 1 Patient_POS_ID FROM FOX_TBL_PATIENT_POS WHERE Patient_Account = @PATIENT_ACCOUNT AND isnull(Deleted, 0) = 0 ORDER BY Created_Date DESC))    
         --ORDER BY Patient_POS_ID;    
    
         IF(EXISTS    
         (    
             SELECT 1    
             FROM #temp_fox_pos    
         ))    
             BEGIN    
                 SET @LOC_ID =    
                 (    
                     SELECT loc_id    
                     FROM #temp_fox_pos    
                 );    
                 SET @POS_ID =    
                 (    
                     SELECT Patient_POS_ID    
                     FROM #temp_fox_pos    
                 );    
             END;    
         SELECT ftal.LOC_ID,     
                ISNULL(ftal.REGION, (    
             SELECT TOP 1 ftpa.POS_REGION    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         )    
        ) REGION,     
                ftal.CODE,     
                ftal.NAME,     
                ISNULL(  (    
             SELECT TOP 1 ADDRESS    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         ),ftal.Address    
       ) AS Address,     
                ISNULL((    
             SELECT TOP 1 ftpa.CITY    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         ),ftal.City    
         ) City,     
                ISNULL(  (    
             SELECT TOP 1 ftpa.[STATE]    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         ),    
    ftal.State    
       ) State,     
                ISNULL((    
             SELECT TOP 1 ftpa.ZIP    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         ),ftal.Zip    
         ) Zip,     
                ISNULL(ftal.Phone,    
         (    
             SELECT TOP 1 ftpa.POS_Phone    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         )) Phone,     
                ISNULL(ftal.Fax,    
         (    
             SELECT TOP 1 ftpa.POS_Fax    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         )) Fax,     
                ftal.POS_Code,     
                ftal.FOL,     
                ftal.FACILITY_TYPE_ID,     
                ftal.Capacity,     
                ftal.Census,     
                ftal.PT,     
                ftal.OT,     
                ftal.ST,     
            ftal.EP,     
                ftal.LEAD,     
                ftal.Parent,     
                ftal.Description,     
                ftal.Last_Update,     
                ftal.CREATED_BY,     
                ftal.CREATED_DATE,     
                ftal.MODIFIED_BY,     
                ftal.MODIFIED_DATE,     
                ftal.DELETED,     
                ISNULL((    
             SELECT TOP 1 ftpa.POS_County    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         ),ftal.Country    
         ) Country,     
                ftal.PRACTICE_CODE,     
                ISNULL(ftal.Work_Phone,    
         (    
             SELECT TOP 1 ftpa.POS_Work_Phone    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         )) Work_Phone,     
                ISNULL(ftal.Cell_Phone,    
         (    
             SELECT TOP 1 ftpa.POS_Cell_Phone    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         )) Cell_Phone,     
                ISNULL(ftal.Email_Address,    
         (    
             SELECT TOP 1 ftpa.POS_Email_Address    
             FROM FOX_TBL_PATIENT_ADDRESS ftpa    
             WHERE ftpa.PATIENT_POS_ID = @POS_ID    
         )) Email_Address,     
                ftal.IS_PRIVATE_HOME,     
                ftal.IS_ACTIVE,     
               -- ftal.IS_MAP,     
                ftal.PT_PROVIDER_ID,     
                ftal.OT_PROVIDER_ID,     
                ftal.ST_PROVIDER_ID,     
                ftal.EP_PROVIDER_ID    
         FROM dbo.FOX_TBL_ACTIVE_LOCATIONS ftal    
         WHERE ftal.LOC_ID = @LOC_ID    
               AND ftal.PRACTICE_CODE = @PRACTICE_CODE    
               AND isnull(ftal.DELETED, 0) = 0;    
     END;