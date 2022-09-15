IF (OBJECT_ID('FOX_PROC_GET_ADVANCED_EXPORT_REGIONS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ADVANCED_EXPORT_REGIONS
GO          
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ADVANCED_EXPORT_REGIONS] --'',1011163,1,0                  
@REGIONS_STRING VARCHAR(8000),               
@PRACTICE_CODE BIGINT,        
@CURRENT_PAGE         INT,                           
 @RECORD_PER_PAGE      INT        
AS                  
     BEGIN           
  IF OBJECT_ID('TEMPDB..#COUNTY_REGION') IS NOT NULL DROP TABLE #COUNTY_REGION        
                 SELECT          
     ROW_NUMBER() over(ORDER BY ZSC.ZIP_CODE) AS ROW_NUM,           
     ZCS.State_Code AS STATE_CODE,          
     ZCS.City_Name AS CITY,           
     RR.REFERRAL_REGION_CODE,          
     RR.REFERRAL_REGION_NAME,          
     ZSC.COUNTY,          
     ZSC.ZIP_CODE        
  INTO #COUNTY_REGION        
                 FROM   FOX_TBL_ZIP_STATE_COUNTY ZSC          
     INNER JOIN FOX_TBL_REFERRAL_REGION RR ON RR.REFERRAL_REGION_ID = ZSC.REFERRAL_REGION_ID          
                                             AND ISNULL(RR.Deleted, 0) = 0          
                                             AND ISNULL(RR.IS_ACTIVE, 0) = 1          
                                             AND ISNULL(RR.IS_INACTIVE, 0) = 0          
                                             AND RR.PRACTICE_CODE = @PRACTICE_CODE          
     LEFT JOIN ZIP_CITY_STATE ZCS ON LEFT(ZCS.ZIP_Code, 5) = LEFT(ZSC.ZIP_CODE, 5)          
                                             AND ISNULL(ZCS.Deleted, 0) = 0          
                 WHERE         
     ISNULL(ZSC.Deleted, 0) = 0             
     AND ZSC.PRACTICE_CODE = @PRACTICE_CODE             
                 AND ZSC.REFERRAL_REGION_ID IN (select val from dbo.f_split(@REGIONS_STRING, ','))        
   SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                          
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                          
         DECLARE @TOATL_PAGESUDM FLOAT;                          
         SELECT @TOATL_PAGESUDM = COUNT(*)                          
         FROM #COUNTY_REGION        
        
   IF(@RECORD_PER_PAGE = 0)                          
             BEGIN                          
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;                          
             END;                          
             ELSE                          
             BEGIN                          
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;                          
             END;                          
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                          
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);                          
         SELECT *,                           
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                           
                @TOTAL_RECORDS AS TOTAL_RECORDS                          
         FROM  #COUNTY_REGION order by  REFERRAL_REGION_CODE desc                       
   OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;        
     END; 