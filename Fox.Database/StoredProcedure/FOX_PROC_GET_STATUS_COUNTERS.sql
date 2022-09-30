IF (OBJECT_ID('FOX_PROC_GET_STATUS_COUNTERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_STATUS_COUNTERS  
GO 
------------------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_STATUS_COUNTERS]  
(@PRACTICE_CODE BIGINT,   
 @CURRENT_USER  VARCHAR(225),   
 @IS_FOR_REPORT BIT  
)  
AS  
     BEGIN  
         SELECT  
         (  
             SELECT COUNT(*)  
             FROM dbo.FOX_TBL_PATIENT_ADJUSTMENT_DETAILS ftpad  
                  INNER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON @CURRENT_USER = ftau.USER_NAME  
                                                                  AND ftau.IS_ACTIVE = 1  
                                                                  AND ftau.DELETED = 0  
             WHERE ftpad.PRACTICE_CODE = @PRACTICE_CODE  
                   AND ftpad.DELETED = 0  
                   AND (@CURRENT_USER = CASE  
                                            WHEN ftau.ROLE_ID IN(103, 110, 111)  
                        OR @IS_FOR_REPORT = 1  
                                            THEN @CURRENT_USER  
                                            ELSE CASE  
                                                     WHEN ftau.ROLE_ID = 105  
                                                     THEN ftpad.CREATED_BY  
                                                     WHEN ftau.ROLE_ID = 112  
                                                     THEN ftpad.ASSIGNED_TO  
                                                 END  
                                        END)  
         ) AS 'All',   
         (  
             SELECT COUNT(*)  
             FROM dbo.FOX_TBL_PATIENT_ADJUSTMENT_DETAILS ftpad  
                  INNER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON @CURRENT_USER = ftau.USER_NAME  
                                                                  AND ftau.IS_ACTIVE = 1  
                                                                  AND ftau.DELETED = 0  
                  INNER JOIN dbo.FOX_TBL_ADJUSTMENT_CLAIM_STATUS ftacs ON ftpad.ADJUSTMENT_STATUS_ID = ftacs.STATUS_ID  
                                                                          AND ftacs.STATUS_CATEGORY LIKE 'Adjustment Approval'  
                                                                          AND ftacs.STATUS_NAME LIKE 'Open'  
             WHERE ftpad.PRACTICE_CODE = @PRACTICE_CODE  
                   AND ftpad.DELETED = 0  
                   AND (@CURRENT_USER = CASE  
                                            WHEN ftau.ROLE_ID IN(103, 110, 111)  
                        OR @IS_FOR_REPORT = 1  
                                            THEN @CURRENT_USER  
                                            ELSE CASE  
                                                     WHEN ftau.ROLE_ID = 105  
                                                     THEN ftpad.CREATED_BY  
                                                     WHEN ftau.ROLE_ID = 112  
                                                     THEN ftpad.ASSIGNED_TO  
                                                 END  
                                        END)  
         ) AS 'Open',   
         (  
             SELECT COUNT(*)  
             FROM dbo.FOX_TBL_PATIENT_ADJUSTMENT_DETAILS ftpad  
                  INNER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON @CURRENT_USER = ftau.USER_NAME  
                                                                  AND ftau.IS_ACTIVE = 1  
                                                                  AND ftau.DELETED = 0  
                  INNER JOIN dbo.FOX_TBL_ADJUSTMENT_CLAIM_STATUS ftacs ON ftpad.ADJUSTMENT_STATUS_ID = ftacs.STATUS_ID  
                                                                          AND ftacs.STATUS_CATEGORY LIKE 'Adjustment Approval'  
                                                                          AND ftacs.STATUS_NAME LIKE 'In-Progress'  
             WHERE ftpad.PRACTICE_CODE = @PRACTICE_CODE  
                   AND ftpad.DELETED = 0  
                   AND (@CURRENT_USER = CASE  
           WHEN ftau.ROLE_ID IN(103, 110, 111)  
                        OR @IS_FOR_REPORT = 1  
                                            THEN @CURRENT_USER  
                                            ELSE CASE  
                                                     WHEN ftau.ROLE_ID = 105  
                                                     THEN ftpad.CREATED_BY  
                                                     WHEN ftau.ROLE_ID = 112  
                                                     THEN ftpad.ASSIGNED_TO  
                                                 END  
                                        END)  
         ) AS 'InProgress',   
         (  
             SELECT COUNT(*)  
             FROM dbo.FOX_TBL_PATIENT_ADJUSTMENT_DETAILS ftpad  
                  INNER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON @CURRENT_USER = ftau.USER_NAME  
                                                                  AND ftau.IS_ACTIVE = 1  
                                                                  AND ftau.DELETED = 0  
                  INNER JOIN dbo.FOX_TBL_ADJUSTMENT_CLAIM_STATUS ftacs ON ftpad.ADJUSTMENT_STATUS_ID = ftacs.STATUS_ID  
                                                                          AND ftacs.STATUS_CATEGORY LIKE 'Adjustment Approval'  
                                                                          AND ftacs.STATUS_NAME LIKE 'Pending'  
             WHERE ftpad.PRACTICE_CODE = @PRACTICE_CODE  
                   AND ftpad.DELETED = 0  
                   AND (@CURRENT_USER = CASE  
                                            WHEN ftau.ROLE_ID IN(103, 110, 111)  
                        OR @IS_FOR_REPORT = 1  
                                            THEN @CURRENT_USER  
                                            ELSE CASE  
                                                     WHEN ftau.ROLE_ID = 105  
                                                     THEN ftpad.CREATED_BY  
                                                     WHEN ftau.ROLE_ID = 112  
                                                     THEN ftpad.ASSIGNED_TO  
                                                 END  
                                        END)  
         ) AS 'Pending',   
         (  
             SELECT COUNT(*)  
             FROM dbo.FOX_TBL_PATIENT_ADJUSTMENT_DETAILS ftpad  
                  INNER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON @CURRENT_USER = ftau.USER_NAME  
                                                                  AND ftau.IS_ACTIVE = 1  
                                                                  AND ftau.DELETED = 0  
                  INNER JOIN dbo.FOX_TBL_ADJUSTMENT_CLAIM_STATUS ftacs ON ftpad.ADJUSTMENT_STATUS_ID = ftacs.STATUS_ID  
                                                                          AND ftacs.STATUS_CATEGORY LIKE 'Adjustment Approval'  
                                                                          AND ftacs.STATUS_NAME LIKE 'Closed'  
             WHERE ftpad.PRACTICE_CODE = @PRACTICE_CODE  
                   AND ftpad.DELETED = 0  
                   AND (@CURRENT_USER = CASE  
                                            WHEN ftau.ROLE_ID IN(103, 110, 111)  
                        OR @IS_FOR_REPORT = 1  
                                            THEN @CURRENT_USER  
                                            ELSE CASE  
                                                     WHEN ftau.ROLE_ID = 105  
                                                     THEN ftpad.CREATED_BY  
                                                     WHEN ftau.ROLE_ID = 112  
                                                     THEN ftpad.ASSIGNED_TO  
                                                 END  
                                        END)  
         ) AS 'Closed',   
         (  
             SELECT COUNT(*)  
             FROM dbo.FOX_TBL_PATIENT_ADJUSTMENT_DETAILS ftpad  
                  INNER JOIN dbo.FOX_TBL_APPLICATION_USER ftau ON @CURRENT_USER = ftau.USER_NAME  
                                                             AND ftau.IS_ACTIVE = 1  
                                                                  AND ftau.DELETED = 0  
                  INNER JOIN dbo.FOX_TBL_ADJUSTMENT_CLAIM_STATUS ftacs ON ftpad.ADJUSTMENT_STATUS_ID = ftacs.STATUS_ID  
                                                                          AND ftacs.STATUS_CATEGORY LIKE 'Adjustment Approval'  
                                                                          AND ftacs.STATUS_NAME LIKE 'Review Required'  
             WHERE ftpad.PRACTICE_CODE = @PRACTICE_CODE  
                   AND ftpad.DELETED = 0  
                   AND (@CURRENT_USER = CASE  
                                            WHEN ftau.ROLE_ID IN(103, 110, 111)  
                        OR @IS_FOR_REPORT = 1  
                                            THEN @CURRENT_USER  
                                            ELSE CASE  
                                                     WHEN ftau.ROLE_ID = 105  
                                                     THEN ftpad.CREATED_BY  
                                                     WHEN ftau.ROLE_ID = 112  
                                                     THEN ftpad.ASSIGNED_TO  
                                                 END  
                                        END)  
         ) AS 'ReviewRequired';  
     END;  
