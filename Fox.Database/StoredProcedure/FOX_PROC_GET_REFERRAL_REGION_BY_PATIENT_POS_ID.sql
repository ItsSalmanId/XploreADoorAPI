IF (OBJECT_ID('FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_POS_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_POS_ID  
GO  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_REFERRAL_REGION_BY_PATIENT_POS_ID] @PATIENT_POS_ID BIGINT,     
                                                                 @PRACTICE_CODE  BIGINT    
AS    
     BEGIN    
         DECLARE @POS_TYPE NVARCHAR(70);    
         SET @POS_TYPE =    
         (    
             SELECT ftft.NAME    
             FROM dbo.FOX_TBL_ACTIVE_LOCATIONS ftal    
                  LEFT OUTER JOIN dbo.FOX_TBL_FACILITY_TYPE ftft ON ftft.FACILITY_TYPE_ID = ftal.FACILITY_TYPE_ID    
                  LEFT OUTER JOIN dbo.Fox_Tbl_Patient_POS ftpp ON ftpp.Loc_ID = ftal.LOC_ID    
             WHERE ftpp.Patient_POS_ID = @PATIENT_POS_ID    
         );    
         IF @POS_TYPE = 'Private Home'    
             BEGIN    
                 SELECT TOP 1 *    
                 FROM dbo.FOX_TBL_REFERRAL_REGION ftrr    
                      LEFT OUTER JOIN dbo.FOX_TBL_PATIENT_ADDRESS ftpa ON ftrr.REFERRAL_REGION_NAME = ftpa.POS_REGION    
                                                                          AND (ftpa.DELETED = 0    
                                                                               OR ftpa.DELETED IS NULL)    
                      LEFT OUTER JOIN dbo.Fox_Tbl_Patient_POS ftpp ON ftpa.PATIENT_POS_ID = ftpp.Patient_POS_ID    
                                                                      AND (ftpp.DELETED = 0    
                                                                           OR ftpp.DELETED IS NULL)    
                 WHERE ftpp.Patient_POS_ID = @PATIENT_POS_ID    
                       AND ftrr.PRACTICE_CODE = @PRACTICE_CODE    
                       AND (ftrr.DELETED = 0    
                            OR ftrr.DELETED IS NULL)    
                 ORDER BY ftrr.REFERRAL_REGION_ID DESC;    
             END;    
             ELSE    
             BEGIN    
                 SELECT TOP 1 *    
                 FROM dbo.FOX_TBL_REFERRAL_REGION ftrr    
                      LEFT OUTER JOIN dbo.FOX_TBL_ACTIVE_LOCATIONS ftal ON ftrr.REFERRAL_REGION_NAME = ftal.REGION    
                                                                           AND (ftal.DELETED = 0    
                                                                                OR ftal.DELETED IS NULL)    
                                                                           AND ftal.PRACTICE_CODE = @PRACTICE_CODE    
                      LEFT OUTER JOIN dbo.Fox_Tbl_Patient_POS ftpp ON ftal.LOC_ID = ftpp.Loc_ID    
                                                                      AND (ftpp.DELETED = 0    
                                                                           OR ftpp.DELETED IS NULL)    
                 WHERE ftpp.Patient_POS_ID = @PATIENT_POS_ID    
                       AND ftrr.PRACTICE_CODE = @PRACTICE_CODE    
                       AND (ftrr.DELETED = 0    
                            OR ftrr.DELETED IS NULL)    
                 ORDER BY ftrr.REFERRAL_REGION_ID DESC    
             END;    
     END  
