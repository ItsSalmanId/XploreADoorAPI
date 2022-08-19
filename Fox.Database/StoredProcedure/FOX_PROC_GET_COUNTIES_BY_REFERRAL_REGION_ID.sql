IF (OBJECT_ID('FOX_PROC_GET_COUNTIES_BY_REFERRAL_REGION_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_COUNTIES_BY_REFERRAL_REGION_ID 
GO          
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
--EXEC [FOX_PROC_GET_COUNTIES_BY_REFERRAL_REGION_ID] 1011163, 548516                                   
CREATE PROCEDURE FOX_PROC_GET_COUNTIES_BY_REFERRAL_REGION_ID @PRACTICE_CODE      BIGINT,                                     
                                                            @REFERRAL_REGION_ID BIGINT                                    
AS                                    
                                  
     BEGIN                                  
         SELECT                
    REFERRAL_REGION_ID,COUNTY,*                
    FROM FOX_TBL_ZIP_STATE_COUNTY                
    --WHERE PRACTICE_CODE = @PRACTICE_CODE                
    --               AND STATE = @STATE_CODE                
    --               AND COUNTY IS NOT NULL                
    --               AND COUNTY != ''                
    --         GROUP BY COUNTY                
         WHERE ZIP_STATE_COUNTY_ID IN                
         (                
select max(ZIP_STATE_COUNTY_ID) from  FOX_TBL_ZIP_STATE_COUNTY                
where isnull(deleted,0)  = 0 and PRACTICE_CODE = @PRACTICE_CODE   and REFERRAL_REGION_ID = @REFERRAL_REGION_ID     group by COUNTY                
   )  and isnull(deleted,0)  = 0 and PRACTICE_CODE = @PRACTICE_CODE                 
         ORDER BY MODIFIED_DATE ASC                  
                
                
--------------------------------------------------------------------------------------- send total and maped numbers                 
--IF OBJECT_ID('tempdb..#temp_fox_zcs_data') IS NOT NULL                
--             DROP TABLE #temp_fox_zcs_data;                
                
-- select * into #temp_fox_zcs_data                
--    FROM FOX_TBL_ZIP_STATE_COUNTY                
--         WHERE ZIP_STATE_COUNTY_ID IN                
--         (                
--select max(ZIP_STATE_COUNTY_ID) from  FOX_TBL_ZIP_STATE_COUNTY                
--where isnull(deleted,0)  = 0 and PRACTICE_CODE = @PRACTICE_CODE and REFERRAL_REGION_ID = @REFERRAL_REGION_ID                  
--   group by COUNTY) and isnull(deleted,0) = 0 and practice_code = @PRACTICE_CODE                
                   
                
-- SELECT x.*, y.MAPED_ZIP_COUNT, y.TOTAL_COUNTIES_COUNT                 
-- from                 
-- #temp_fox_zcs_data x                
--   inner join                 
--   (                
--select a.county,a.STATE,isnull(a.MAPED_ZIP_COUNT,0) MAPED_ZIP_COUNT,isnull(b.TOTAL_COUNTIES_COUNT,0) TOTAL_COUNTIES_COUNT                
--from (                
--select zcsx.county,zcsx.state,count(zcsx.ZIP_STATE_COUNTY_ID) MAPED_ZIP_COUNT                
----into #temp_fox_zcd                
--from FOX_TBL_ZIP_STATE_COUNTY zcsx                
--inner join #temp_fox_zcs_data tdx on tdx.COUNTY = zcsx.COUNTY and tdx.STATE = zcsx.STATE and zcsx.REFERRAL_REGION_ID = @REFERRAL_REGION_ID                
--where isnull(zcsx.deleted,0) = 0 and zcsx.practice_code = @PRACTICE_CODE                
--group by zcsx.county,zcsx.state                
--)a                
--left join                
--(                 
--select zcsy.county,zcsy.state,count(zcsy.ZIP_STATE_COUNTY_ID) TOTAL_COUNTIES_COUNT                
----into #temp_fox_zcd                
--from FOX_TBL_ZIP_STATE_COUNTY zcsy                
--inner join #temp_fox_zcs_data tdy on tdy.COUNTY = zcsy.COUNTY and tdy.STATE = zcsy.STATE                 
--where isnull(zcsy.deleted,0) = 0 and zcsy.practice_code = @PRACTICE_CODE ---zcsy.REFERRAL_REGION_ID is null and                
--group by zcsy.county,zcsy.state                 
--)b                
--on a.COUNTY = b.COUNTY and a.STATE = b.state                
--   )y                
--   on x.COUNTY = y.COUNTY and x.STATE = y.state                   
--------------------------------------------------------------------------------------- send total and maped numbers                  
     END; 