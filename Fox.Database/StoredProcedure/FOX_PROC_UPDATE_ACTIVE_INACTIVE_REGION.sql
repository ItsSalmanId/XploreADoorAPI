IF (OBJECT_ID('FOX_PROC_UPDATE_ACTIVE_INACTIVE_REGION') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_ACTIVE_INACTIVE_REGION  
GO   
-- =============================================                          
-- AUTHOR:  <DEVELOPER, ABDUL SATTAR>                          
-- CREATE DATE: <CREATE DATE, 09/13/2019>                          
-- DESCRIPTION: <UPDATE REGION ACTIVE/INACTIVE FLAG ON THE BASE OF EFFECTIVE DATE>                          
-- =============================================                          
-- EXEC [DBO].[FOX_PROC_UPDATE_ACTIVE_INACTIVE_REGION]                  
CREATE PROCEDURE FOX_PROC_UPDATE_ACTIVE_INACTIVE_REGION                
--@PRACTICE_CODE BIGINT                  
AS            
     BEGIN            
         SET NOCOUNT ON;            
         IF OBJECT_ID('TEMPDB..#FOXTEMPREGIONDATA') IS NOT NULL            
             DROP TABLE #FOXTEMPREGIONDATA;            
         SELECT *            
         INTO #FOXTEMPREGIONDATA            
         FROM FOX_TBL_REFERRAL_REGION            
         WHERE PRACTICE_CODE = 1012714            
               AND ISNULL(DELETED, 0) = 0;            
         IF(            
         (            
             SELECT COUNT(*)            
             FROM #FOXTEMPREGIONDATA            
         ) > 0)            
             BEGIN            
                 IF(            
                 (            
                     SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE [START_DATE] IS NOT NULL            
                           AND [END_DATE] IS NULL            
                           AND CAST([START_DATE] AS DATE) <= CAST(GETDATE() AS DATE)            
                 ) > 0)            
                     BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                               IS_ACTIVE = 1,             
                               IS_INACTIVE = 0,        
                               MODIFIED_DATE = MODIFIED_DATE,        
                               MODIFIED_BY= MODIFIED_BY         
                         WHERE [START_DATE] IS NOT NULL            
                               AND [END_DATE] IS NULL            
                               AND CAST([START_DATE] AS DATE) <= CAST(GETDATE() AS DATE);            
                     END;            
                 IF(            
                 (            
                     SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE [START_DATE] IS NOT NULL            
                           AND [END_DATE] IS NOT NULL            
                           AND CAST(GETDATE() AS DATE) BETWEEN CAST([START_DATE] AS DATE) AND CAST([END_DATE] AS DATE)            
                 ) > 0)            
                     BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                               IS_ACTIVE = 1,             
                               IS_INACTIVE = 0,        
                               MODIFIED_DATE = MODIFIED_DATE,        
                               MODIFIED_BY = MODIFIED_BY            
                         WHERE [START_DATE] IS NOT NULL            
                               AND [END_DATE] IS NOT NULL            
                               AND CAST(GETDATE() AS DATE) BETWEEN CAST([START_DATE] AS DATE) AND CAST([END_DATE] AS DATE);            
                     END;            
                 IF(            
                 (            
                     SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE [START_DATE] IS NULL            
                           AND [END_DATE] IS NOT NULL            
                           AND CAST([END_DATE] AS DATE) > CAST(GETDATE() AS DATE)            
                 ) > 0)            
            BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                       IS_ACTIVE = 1,             
                               IS_INACTIVE = 0,        
                                MODIFIED_DATE = MODIFIED_DATE,        
                                MODIFIED_BY= MODIFIED_BY            
                         WHERE [START_DATE] IS NULL           
                               AND [END_DATE] IS NOT NULL            
                               AND CAST([END_DATE] AS DATE) > CAST(GETDATE() AS DATE);            
                     END;         
              
      --Modified Part by Aftab        
          IF(            
                 (            
       SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE [START_DATE] IS NULL            
                           AND [END_DATE] IS NOT NULL            
                           AND CAST([END_DATE] AS DATE) = CAST(GETDATE() AS DATE)            
                 ) > 0)            
                     BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                               IS_ACTIVE = 0,             
                               IS_INACTIVE = 1,        
                               MODIFIED_DATE = GETDATE(),        
                               MODIFIED_BY = 'FOXDEV SERVICE'          
                         WHERE [START_DATE] IS NULL            
                               AND [END_DATE] IS NOT NULL            
                               AND CAST([END_DATE] AS DATE) = CAST(GETDATE() AS DATE);            
                     END;        
          IF(            
                 (            
                     SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE [START_DATE] IS NOT NULL            
                           AND [END_DATE] IS NULL            
                           AND CAST([START_DATE] AS DATE) = CAST(GETDATE() AS DATE)            
                 ) > 0)            
                     BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                               IS_ACTIVE = 1,             
                               IS_INACTIVE = 0,        
           MODIFIED_DATE = GETDATE(),        
        MODIFIED_BY = 'FOXDEV SERVICE'          
                         WHERE [START_DATE] IS NULL            
                               AND [END_DATE] IS NOT NULL            
                               AND CAST([START_DATE] AS DATE) = CAST(GETDATE() AS DATE);            
                     END;        
        
          IF(            
                 (            
                     SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE [START_DATE] IS NOT NULL            
                           AND [END_DATE] IS NOT NULL            
                           AND CAST([START_DATE] AS DATE) = CAST(GETDATE() AS DATE)            
                 ) > 0)            
                     BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                               IS_ACTIVE = 1,             
                               IS_INACTIVE = 0,        
                               MODIFIED_DATE = GETDATE(),        
                               MODIFIED_BY = 'FOXDEV SERVICE'          
                         WHERE [START_DATE] IS NOT NULL            
                               AND [END_DATE] IS NOT NULL            
                               AND CAST([START_DATE] AS DATE) = CAST(GETDATE() AS DATE);            
                     END;        
         IF(            
                 (            
                     SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE [START_DATE] IS NOT NULL            
                           AND [END_DATE] IS NOT NULL           
                           AND CAST([END_DATE] AS DATE) = CAST(GETDATE() AS DATE)            
                 ) > 0)            
                     BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                    IS_ACTIVE = 0,             
                               IS_INACTIVE = 1,        
                                MODIFIED_DATE = GETDATE(),        
                                MODIFIED_BY = 'FOXDEV SERVICE'          
                         WHERE [START_DATE] IS NOT NULL            
               AND [END_DATE] IS NOT NULL            
                               AND CAST([END_DATE] AS DATE) = CAST(GETDATE() AS DATE);            
                     END;        
             
              
      --Modified Close by aftab           
                 IF(            
                 (            
                     SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE([START_DATE] IS NOT NULL            
                           AND [END_DATE] IS NULL            
                           AND CAST([START_DATE] AS DATE) > CAST(GETDATE() AS DATE))            
                          OR ([START_DATE] IS NOT NULL            
                              AND [END_DATE] IS NOT NULL            
                              AND CAST(GETDATE() AS DATE) NOT BETWEEN CAST([START_DATE] AS DATE) AND CAST([END_DATE] AS DATE))            
                          OR ([START_DATE] IS NULL            
 AND [END_DATE] IS NOT NULL            
                              AND CAST([END_DATE] AS DATE) < CAST(GETDATE() AS DATE))            
                 ) > 0)            
                     BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                               IS_ACTIVE = 0,             
                               IS_INACTIVE = 1,        
                               MODIFIED_DATE = MODIFIED_DATE,        
                                 MODIFIED_BY= MODIFIED_BY            
                         WHERE([START_DATE] IS NOT NULL            
                               AND [END_DATE] IS NULL            
                               AND CAST([START_DATE] AS DATE) > CAST(GETDATE() AS DATE))            
                              OR ([START_DATE] IS NOT NULL            
                                  AND [END_DATE] IS NOT NULL            
                                  AND CAST(GETDATE() AS DATE) NOT BETWEEN CAST([START_DATE] AS DATE) AND CAST([END_DATE] AS DATE))            
                              OR ([START_DATE] IS NULL            
                                  AND [END_DATE] IS NOT NULL            
                                  AND CAST([END_DATE] AS DATE) < CAST(GETDATE() AS DATE));            
                     END;            
                 IF(            
                 (            
                     SELECT COUNT(*)            
                     FROM #FOXTEMPREGIONDATA            
                     WHERE([START_DATE] IS NULL            
                           AND [END_DATE] IS NULL)            
                 ) > 0)            
                     BEGIN            
                         UPDATE #FOXTEMPREGIONDATA            
                           SET             
                               IS_ACTIVE = 1,             
                               IS_INACTIVE = 0,        
       MODIFIED_DATE = MODIFIED_DATE,        
                           MODIFIED_BY = MODIFIED_BY            
                         WHERE [START_DATE] IS NULL            
                               AND [END_DATE] IS NULL;            
                     END;            
             END;            
         UPDATE RR            
           SET             
               RR.IS_ACTIVE = TR.IS_ACTIVE,             
               RR.IS_INACTIVE = TR.IS_INACTIVE,             
               RR.MODIFIED_BY = TR.MODIFIED_BY,             
               RR.MODIFIED_DATE = TR.MODIFIED_DATE         
         FROM FOX_TBL_REFERRAL_REGION RR            
              JOIN #FOXTEMPREGIONDATA TR ON TR.REFERRAL_REGION_ID = RR.REFERRAL_REGION_ID            
                                            AND TR.REFERRAL_REGION_CODE = RR.REFERRAL_REGION_CODE            
         WHERE RR.PRACTICE_CODE = 1012714            
  AND ISNULL(RR.DELETED, 0) = 0;            
     END;