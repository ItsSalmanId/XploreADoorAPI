IF (OBJECT_ID('FOX_GET_SMART_POS_LOCATIONS') IS NOT NULL) DROP PROCEDURE FOX_GET_SMART_POS_LOCATIONS
GO
----21
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--EXEC [FOX_GET_SMART_POS_LOCATIONS] 'CT065',1011163         
CREATE PROCEDURE [dbo].[FOX_GET_SMART_POS_LOCATIONS] @SEARCHVALUE   VARCHAR(MAX), 
                                                     @PRACTICE_CODE BIGINT    
--DECLARE @SEARCHVALUE VARCHAR(MAX)= 'CT065', @PRACTICE_CODE BIGINT= 9090992;    
AS
     BEGIN    
         --Drop Table if exists    
         --IF OBJECT_ID('tempdb..#FOX_TBL_PATIENT_POS') IS NOT NULL
         --    DROP TABLE #FOX_TBL_PATIENT_POS;    
         --    
         --    
         IF(@SEARCHVALUE = '')
             BEGIN
                 SET @SEARCHVALUE = NULL;
             END;    
         --    
         --Select DISTINCT LOC_ID from FOX_TBL_PATIENT_POS    
         --SELECT DISTINCT 
         --       LOC_ID
         --INTO #FOX_TBL_PATIENT_POS
         --FROM FOX_TBL_PATIENT_POS
         --WHERE ISNULL(DELETED, 0) = 0;
         --    
         SELECT DISTINCT 
                LOC.LOC_ID, 
                LOC.NAME, 
                LOC.CODE, 
                REG.REFERRAL_REGION_ID, 
                REG.REFERRAL_REGION_CODE, 
                REG.REFERRAL_REGION_NAME
         FROM FOX_TBL_ACTIVE_LOCATIONS AS LOC
              --INNER JOIN #FOX_TBL_PATIENT_POS AS POS ON POS.LOC_ID = LOC.LOC_ID
              LEFT JOIN FOX_TBL_REFERRAL_REGION AS REG ON REG.REFERRAL_REGION_NAME = LOC.REGION
                                                          AND ISNULL(REG.DELETED, 0) = 0      
                                                          --AND reg.IS_ACTIVE = 1      
                                                          AND REG.PRACTICE_CODE = LOC.PRACTICE_CODE
         WHERE(LOC.NAME LIKE '%'+@SEARCHVALUE+'%'
               OR LOC.CODE LIKE '%'+@SEARCHVALUE+'%')
              AND ISNULL(LOC.DELETED, 0) = 0
              AND LOC.PRACTICE_CODE = @PRACTICE_CODE
              AND ISNULL(LOC.IS_ACTIVE, 1) = 1;    
         --OLD    
         --IF(@SEARCHVALUE = '')      
         --    BEGIN      
         --        SET @SEARCHVALUE = NULL;      
         --    END;      
         --SELECT DISTINCT       
         --       LOC.LOC_ID,       
         --       LOC.NAME,       
         --       LOC.CODE,       
         --       isnull(POS.IS_DEFAULT, 0) IS_DEFAULT,       
         --       REG.REFERRAL_REGION_ID,       
         --       REG.REFERRAL_REGION_CODE,       
         --       REG.REFERRAL_REGION_NAME      
         --FROM FOX_TBL_PATIENT_POS POS      
         --     JOIN FOX_TBL_ACTIVE_LOCATIONS LOC ON POS.LOC_ID = LOC.LOC_ID      
         --     LEFT JOIN FOX_TBL_REFERRAL_REGION REG ON REG.REFERRAL_REGION_NAME = LOC.REGION      
         --                                              AND REG.DELETED = 0      
         --                                              --AND reg.IS_ACTIVE = 1      
         --                                              AND (@PRACTICE_CODE IS NULL      
         --                                                   OR LOC.PRACTICE_CODE = @PRACTICE_CODE)      
         --WHERE(LOC.NAME LIKE '%'+@SEARCHVALUE+'%'      
         --      OR LOC.CODE LIKE '%'+@SEARCHVALUE+'%')      
         --     AND ISNULL(POS.DELETED, 0) = 0;    
     END;

