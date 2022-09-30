IF (OBJECT_ID('FOX_PROC_GET_FACILITY_LOCATION_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_FACILITY_LOCATION_LIST 
GO  
--select * from FOX_TBL_ACTIVE_LOCATIONS where ISNULL(deleted,0) = 0                                  
--1011163548501                      
--exec fox_proc_get_facility_location_list_TEST_1422019 '1011163','','','','','','','','','','',1,10,'',''                             
CREATE PROCEDURE [dbo].[fox_proc_get_facility_location_list]-- 1011163,'','al001',1, 2000, 'code' , 'desc'                                                        
@practice_code   BIGINT,         
@search_string   VARCHAR(100),         
@code            VARCHAR(20),         
@description     VARCHAR(100),         
@zip             VARCHAR(100),         
@city            VARCHAR(100),         
@state           VARCHAR(100),         
@address         VARCHAR(100),         
@referral_region VARCHAR(100),         
@country         VARCHAR(50),         
@facility_type   BIGINT NULL,         
@current_page    INT,         
@record_per_page INT,         
@sort_by         VARCHAR(50),         
@sort_order      VARCHAR(5)        
AS        
     BEGIN        
         IF(@SEARCH_STRING = '')        
             BEGIN        
                 SET @SEARCH_STRING = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @SEARCH_STRING = '%'+@SEARCH_STRING+'%';        
             END;        
         IF(@CODE = '')        
             BEGIN        
                 SET @CODE = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @CODE = @CODE+'%';        
             END;        
         IF(@DESCRIPTION = '')        
             BEGIN        
                 SET @DESCRIPTION = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @DESCRIPTION = '%'+@DESCRIPTION+'%';        
             END;        
         IF(@zip = '')        
             BEGIN        
                 SET @zip = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @zip = '%'+@zip+'%';        
             END;        
         IF(@city = '')        
             BEGIN        
                 SET @city = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @city = '%'+@city+'%';        
             END;        
         IF(@state = '')        
             BEGIN        
                 SET @state = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @state = '%'+@state+'%';        
             END;        
         IF(@ADDRESS = '')        
             BEGIN        
                 SET @ADDRESS = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @ADDRESS = '%'+@ADDRESS+'%';        
             END;        
         IF(@REFERRAL_REGION = '')        
             BEGIN        
                 SET @REFERRAL_REGION = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @REFERRAL_REGION = @REFERRAL_REGION;        
             END;        
         IF(@COUNTRY = '')        
             BEGIN        
                 SET @COUNTRY = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @COUNTRY = @COUNTRY;        
             END;        
         IF(@FACILITY_TYPE = '')        
             BEGIN        
                 SET @FACILITY_TYPE = NULL;        
             END;        
             ELSE        
             BEGIN        
                 SET @FACILITY_TYPE = @FACILITY_TYPE;        
             END;                           
         --                      
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;        
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;        
         DECLARE @TOATL_PAGESUDM FLOAT;        
        
         --                      
         SELECT @TOATL_PAGESUDM = COUNT(DISTINCT l.LOC_ID)    
         FROM FOX_TBL_ACTIVE_LOCATIONS AS l        
              LEFT JOIN FOX_TBL_REFERRAL_REGION AS r ON(l.REGION = r.REFERRAL_REGION_CODE OR      
                                                         l.REGION = r.REFERRAL_REGION_NAME)        
                                                       AND ISNULL(r.DELETED, 0) = 0        
                                                       AND r.PRACTICE_CODE = @PRACTICE_CODE        
                                           AND ISNULL(r.IS_ACTIVE, 1) = 1        
              LEFT JOIN FOX_TBL_PROVIDER AS pt_p ON pt_p.FOX_PROVIDER_ID = l.PT_PROVIDER_ID        
AND ISNULL(pt_p.DELETED, 0) = 0        
                                                    AND pt_p.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT JOIN FOX_TBL_PROVIDER AS ot_p ON ot_p.FOX_PROVIDER_ID = l.OT_PROVIDER_ID        
                                                    AND ISNULL(ot_p.DELETED, 0) = 0        
                                                    AND ot_p.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT JOIN FOX_TBL_PROVIDER AS st_p ON st_p.FOX_PROVIDER_ID = l.ST_PROVIDER_ID        
                                                    AND ISNULL(st_p.DELETED, 0) = 0        
                                                    AND st_p.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT JOIN FOX_TBL_PROVIDER AS ep_p ON ep_p.FOX_PROVIDER_ID = l.EP_PROVIDER_ID        
                                                    AND ISNULL(ep_p.DELETED, 0) = 0        
                                                    AND ep_p.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT JOIN FOX_TBL_PROVIDER AS lp ON lp.FOX_PROVIDER_ID = l.LEAD_PROVIDER_ID        
                                                  AND ISNULL(lp.DELETED, 0) = 0        
                                                  AND lp.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT JOIN dbo.FOX_TBL_FACILITY_TYPE AS ftft ON L.FACILITY_TYPE_ID = ftft.FACILITY_TYPE_ID        
                                                             AND ISNULL(ftft.DELETED, 0) = 0        
                                                             AND ftft.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT JOIN FOX_TBL_APPLICATION_USER AS u ON u.USER_NAME = l.CREATED_BY        
                                                         AND ISNULL(u.DELETED, 0) = 0        
                                                         AND u.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT JOIN FOX_TBL_APPLICATION_USER AS us ON us.USER_NAME = l.MODIFIED_BY        
                                                          AND ISNULL(us.DELETED, 0) = 0        
                                                          AND us.PRACTICE_CODE = @PRACTICE_CODE        
              LEFT JOIN FOX_TBL_LOCATION_CORPORATION AS cl ON cl.CODE = l.Parent        
                                                              AND ISNULL(cl.DELETED, 0) = 0        
                                                              AND cl.PRACTICE_CODE = @PRACTICE_CODE        
         WHERE(@SEARCH_STRING IS NULL        
               OR l.CODE LIKE '%'+@SEARCH_STRING+'%'        
               OR r.REFERRAL_REGION_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR r.REFERRAL_REGION_CODE LIKE '%'+@SEARCH_STRING+'%'        
               OR l.NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR l.Address LIKE '%'+@SEARCH_STRING+'%'        
               OR l.City LIKE '%'+@SEARCH_STRING+'%'        
               OR l.STATE LIKE '%'+@SEARCH_STRING+'%'        
               OR l.Zip LIKE '%'+@SEARCH_STRING+'%'        
               OR l.Phone LIKE '%'+@SEARCH_STRING+'%'        
               OR l.Fax LIKE '%'+@SEARCH_STRING+'%'        
               OR l.POS_Code LIKE '%'+@SEARCH_STRING+'%'              
               --OR l.Capacity LIKE '%'+@SEARCH_STRING+'%'              
               --OR l.Census LIKE '%'+@SEARCH_STRING+'%'              
  --OR l.PT LIKE '%'+@SEARCH_STRING+'%'              
               --OR l.OT LIKE '%'+@SEARCH_STRING+'%'              
               --OR l.ST LIKE '%'+@SEARCH_STRING+'%'              
               --OR l.EP LIKE '%'+@SEARCH_STRING+'%'              
               --OR l.Lead LIKE '%'+@SEARCH_STRING+'%'              
              OR pt_p.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR pt_p.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR ot_p.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR ot_p.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR st_p.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR st_p.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR ep_p.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR lp.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR lp.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
OR ep_p.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
               OR (pt_p.FIRST_NAME+' '+pt_p.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
               OR (ot_p.FIRST_NAME+' '+ot_p.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
               OR (st_p.FIRST_NAME+' '+st_p.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
               OR (ep_p.FIRST_NAME+' '+ep_p.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
               OR (lp.FIRST_NAME+' '+lp.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
               OR l.Parent LIKE '%'+@SEARCH_STRING+'%'        
               OR Description LIKE '%'+@SEARCH_STRING+'%'        
               OR l.Country LIKE '%'+@SEARCH_STRING+'%'        
               OR CONVERT(VARCHAR(10), l.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'        
               OR CONVERT(VARCHAR(10), l.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%')                          
              --OR (u.LAST_NAME+', '+u.FIRST_NAME) LIKE '%'+@SEARCH_STRING+'%'                          
              --OR (us.LAST_NAME+', '+us.FIRST_NAME) LIKE '%'+@SEARCH_STRING+'%'                          
              AND ISNULL(l.DELETED, 0) = 0        
              AND l.PRACTICE_CODE = @PRACTICE_CODE        
              AND (@CODE IS NULL        
                   OR l.CODE LIKE '%'+@CODE+'%')        
              AND (@DESCRIPTION IS NULL        
                   OR l.NAME LIKE '%'+@DESCRIPTION+'%')        
              AND (@zip IS NULL        
                   OR l.Zip LIKE '%'+@zip+'%')        
              AND (@city IS NULL        
                   OR l.City LIKE '%'+@city+'%')        
              AND (@state IS NULL        
                   OR l.State LIKE '%'+@state+'%')        
              AND (@ADDRESS IS NULL        
                   OR l.Address LIKE '%'+@ADDRESS+'%')        
              AND (@REFERRAL_REGION IS NULL        
                   OR l.REGION LIKE '%'+@REFERRAL_REGION+'%'        
                   OR r.REFERRAL_REGION_NAME LIKE '%'+@REFERRAL_REGION+'%')        
              AND (@COUNTRY IS NULL        
                   OR l.Country LIKE '%'+@COUNTRY+'%')        
              AND (@FACILITY_TYPE IS NULL        
                   OR l.FACILITY_TYPE_ID = @FACILITY_TYPE);                
         --                          
         IF(@RECORD_PER_PAGE = 0)        
             BEGIN        
                 SET @RECORD_PER_PAGE = @TOATL_PAGESUDM;        
             END;        
             ELSE        
             BEGIN        
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;        
             END;        
        
         --                      
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;        
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);        
        
         --                      
         SELECT *,         
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,         
                @TOTAL_RECORDS AS TOTAL_RECORDS        
         FROM        
         (        
             SELECT DISTINCT         
                    l.LOC_ID,         
                 r.REFERRAL_REGION_CODE +' - '+ r.REFERRAL_REGION_NAME as REFERRAL_REGION_NAME,       
                    --l.REGION AS REFERRAL_REGION_NAME,         
                    l.CODE,         
                    l.NAME,               
                    --(l.Address+', '+l.City+', '+l.STATE+' '+l.Zip) AS COMPLETE_ADDRESS,             
                    (l.Address+', '+l.City+', '+l.STATE+' '+CASE        
                     WHEN LEN(l.Zip) = 9        
                                                                THEN SUBSTRING(l.Zip, 1, 5)+'-'+SUBSTRING(l.Zip, 6, LEN(l.Zip))        
                                                                ELSE l.Zip        
                                                            END) AS COMPLETE_ADDRESS,         
                    l.Zip,         
                    l.City,         
                    l.STATE,         
        l.Address,         
                    r.REFERRAL_REGION_CODE +' - '+ r.REFERRAL_REGION_NAME AS REGION_NAME,        
                    --l.REGION AS REGION_NAME,       
                    --l.REGION,      
     r.REFERRAL_REGION_CODE +' - '+ r.REFERRAL_REGION_NAME AS REGION,      
                    l.Phone,         
                    l.Fax,         
                    l.POS_Code,         
                    l.FACILITY_TYPE_ID,         
                    l.Capacity,         
                    l.Census,                                               
                    --l.PT,                                               
                    --l.OT,                                      
                    --l.ST,                     
              --l.EP,                                               
                    --l.LEAD,               
                    l.Parent,         
                    cl.NAME AS Description,         
                    l.Last_Update,         
                    l.CREATED_BY,         
                    l.CREATED_DATE,         
                    CONVERT(VARCHAR, l.CREATED_DATE) AS Created_Date_Str,         
                    l.MODIFIED_BY,         
                    l.MODIFIED_DATE,         
                    CONVERT(VARCHAR, l.MODIFIED_DATE) AS Modified_Date_Str,         
                    l.DELETED,         
                    ROW_NUMBER() OVER(ORDER BY l.CREATED_DATE DESC) AS ACTIVEROW,         
                    l.Country,         
                    ftft.DISPLAY_NAME AS FACILITY_TYPE_NAME,         
                    l.PT_PROVIDER_ID,         
                    l.OT_PROVIDER_ID,         
                    l.ST_PROVIDER_ID,         
                    l.EP_PROVIDER_ID,         
                    l.LEAD_PROVIDER_ID,         
                    l.IS_ACTIVE,        
                    CASE        
                        WHEN l.IS_ACTIVE = 1        
                        THEN 'NO'        
                        WHEN l.IS_ACTIVE = 0        
                        THEN 'YES'        
                        ELSE 'NO'        
                    END AS Inactive,         
                    CONVERT(VARCHAR(10), l.CREATED_DATE, 101) AS CREATED_DATE_STRING,         
                    CONVERT(VARCHAR(10), l.MODIFIED_DATE, 101) AS MODIFIED_DATE_STRING,         
                    pt_p.FIRST_NAME+' '+pt_p.LAST_NAME AS PT,         
                    ot_p.FIRST_NAME+' '+ot_p.LAST_NAME AS OT,         
                    st_p.FIRST_NAME+' '+st_p.LAST_NAME AS ST,         
      ep_p.FIRST_NAME+' '+ep_p.LAST_NAME AS EP,         
                    lp.FIRST_NAME+' '+lp.LAST_NAME AS LEAD,    
     l.Latitude,    
     l.Longitude        
             FROM FOX_TBL_ACTIVE_LOCATIONS AS l        
                  LEFT JOIN FOX_TBL_REFERRAL_REGION AS r ON(l.REGION = r.REFERRAL_REGION_CODE OR      
                                                             l.REGION = r.REFERRAL_REGION_NAME)        
                                                           AND ISNULL(r.DELETED, 0) = 0        
                           AND r.PRACTICE_CODE = @PRACTICE_CODE        
                                                           AND ISNULL(r.IS_ACTIVE, 1) = 1        
                  LEFT JOIN FOX_TBL_PROVIDER AS pt_p ON pt_p.FOX_PROVIDER_ID = l.PT_PROVIDER_ID        
                                                        AND ISNULL(pt_p.DELETED, 0) = 0        
                                                        AND pt_p.PRACTICE_CODE = @PRACTICE_CODE        
                  LEFT JOIN FOX_TBL_PROVIDER AS ot_p ON ot_p.FOX_PROVIDER_ID = l.OT_PROVIDER_ID        
                                                        AND ISNULL(ot_p.DELETED, 0) = 0        
                                                        AND ot_p.PRACTICE_CODE = @PRACTICE_CODE        
                  LEFT JOIN FOX_TBL_PROVIDER AS st_p ON st_p.FOX_PROVIDER_ID = l.ST_PROVIDER_ID        
                                                        AND ISNULL(st_p.DELETED, 0) = 0        
                                                        AND st_p.PRACTICE_CODE = @PRACTICE_CODE        
                  LEFT JOIN FOX_TBL_PROVIDER AS ep_p ON ep_p.FOX_PROVIDER_ID = l.EP_PROVIDER_ID        
                                            AND ISNULL(ep_p.DELETED, 0) = 0        
                                                        AND ep_p.PRACTICE_CODE = @PRACTICE_CODE        
                  LEFT JOIN FOX_TBL_PROVIDER AS lp ON lp.FOX_PROVIDER_ID = l.LEAD_PROVIDER_ID        
                                                      AND ISNULL(lp.DELETED, 0) = 0        
                                                      AND lp.PRACTICE_CODE = @PRACTICE_CODE        
                  LEFT JOIN dbo.FOX_TBL_FACILITY_TYPE AS ftft ON L.FACILITY_TYPE_ID = ftft.FACILITY_TYPE_ID        
                                                                 AND ISNULL(ftft.DELETED, 0) = 0        
                                                                 AND ftft.PRACTICE_CODE = @PRACTICE_CODE        
 LEFT JOIN FOX_TBL_APPLICATION_USER AS u ON u.USER_NAME = l.CREATED_BY        
                                                             AND ISNULL(u.DELETED, 0) = 0        
                                                             AND u.PRACTICE_CODE = @PRACTICE_CODE        
                  LEFT JOIN FOX_TBL_APPLICATION_USER AS us ON us.USER_NAME = l.MODIFIED_BY        
                                                              AND ISNULL(us.DELETED, 0) = 0        
                                                              AND us.PRACTICE_CODE = @PRACTICE_CODE        
                  LEFT JOIN FOX_TBL_LOCATION_CORPORATION AS cl ON cl.CODE = l.Parent        
                                                                  AND ISNULL(cl.DELETED, 0) = 0        
                                                                  AND cl.PRACTICE_CODE = @PRACTICE_CODE        
             WHERE(@SEARCH_STRING IS NULL        
                   OR l.CODE LIKE '%'+@SEARCH_STRING+'%'        
                   OR r.REFERRAL_REGION_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR r.REFERRAL_REGION_CODE LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.Address LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.City LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.STATE LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.Zip LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.Phone LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.Fax LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.POS_Code LIKE '%'+@SEARCH_STRING+'%'              
                   --OR l.Capacity LIKE '%'+@SEARCH_STRING+'%'              
                   --OR l.Census LIKE '%'+@SEARCH_STRING+'%'              
                   --OR l.PT LIKE '%'+@SEARCH_STRING+'%'              
                   --OR l.OT LIKE '%'+@SEARCH_STRING+'%'              
                   --OR l.ST LIKE '%'+@SEARCH_STRING+'%'              
                   --OR l.EP LIKE '%'+@SEARCH_STRING+'%'              
                   --OR l.Lead LIKE '%'+@SEARCH_STRING+'%'              
                   OR pt_p.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR pt_p.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR ot_p.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR ot_p.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR st_p.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR st_p.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR ep_p.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR lp.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR lp.FIRST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR ep_p.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'        
                   OR (pt_p.FIRST_NAME+' '+pt_p.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
                   OR (ot_p.FIRST_NAME+' '+ot_p.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
                   OR (st_p.FIRST_NAME+' '+st_p.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
                   OR (ep_p.FIRST_NAME+' '+ep_p.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
                   OR (lp.FIRST_NAME+' '+lp.LAST_NAME) LIKE '%'+@SEARCH_STRING+'%'        
OR l.Parent LIKE '%'+@SEARCH_STRING+'%'        
                   OR Description LIKE '%'+@SEARCH_STRING+'%'        
                   OR l.Country LIKE '%'+@SEARCH_STRING+'%'        
                   OR CONVERT(VARCHAR(10), l.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'        
                   OR CONVERT(VARCHAR(10), l.MODIFIED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%')                          
                  --OR (u.LAST_NAME+', '+u.FIRST_NAME) LIKE '%'+@SEARCH_STRING+'%'                          
                  --OR (us.LAST_NAME+', '+us.FIRST_NAME) LIKE '%'+@SEARCH_STRING+'%'                          
                  AND ISNULL(l.DELETED, 0) = 0        
                  AND l.PRACTICE_CODE = @PRACTICE_CODE        
                  AND (@CODE IS NULL        
                       OR l.CODE LIKE '%'+@CODE+'%')        
                  AND (@DESCRIPTION IS NULL        
                       OR l.NAME LIKE '%'+@DESCRIPTION+'%')        
                  AND (@zip IS NULL        
                       OR l.Zip LIKE '%'+@zip+'%')        
                  AND (@city IS NULL        
                       OR l.City LIKE '%'+@city+'%')        
                  AND (@state IS NULL        
                       OR l.State LIKE '%'+@state+'%')        
                  AND (@ADDRESS IS NULL        
                       OR l.Address LIKE '%'+@ADDRESS+'%')        
                  AND (@REFERRAL_REGION IS NULL        
                       OR l.REGION LIKE '%'+@REFERRAL_REGION+'%'        
                       OR r.REFERRAL_REGION_NAME LIKE '%'+@REFERRAL_REGION+'%')        
                  AND (@COUNTRY IS NULL        
                       OR l.Country LIKE '%'+@COUNTRY+'%')        
                  AND (@FACILITY_TYPE IS NULL        
                       OR l.FACILITY_TYPE_ID = @FACILITY_TYPE)        
         ) AS FOX_TBL_ACTIVE_LOCATIONS        
         ORDER BY CASE        
         WHEN @SORT_BY = 'Code'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN CODE        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Code'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN CODE        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Name'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN NAME        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Name'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN NAME        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Address'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN COMPLETE_ADDRESS        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Address'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN COMPLETE_ADDRESS        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'description'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN Description        
                  END ASC,        
CASE        
                      WHEN @SORT_BY = 'description'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN Description        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Country'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN Country        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Country'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN Country        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Phone'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN Phone        
                  END ASC,        
                  CASE        
               WHEN @SORT_BY = 'Phone'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN Phone        
        END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Region'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN REFERRAL_REGION_NAME        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Region'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN REFERRAL_REGION_NAME        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Fax'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN Fax        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Fax'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN Fax        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'POS'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN POS_Code        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'POS'        
                           AND @SORT_ORDER = 'DESC'        
    THEN POS_Code        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Capacity'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN Capacity        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Capacity'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN Capacity        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Census'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN Census        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Census'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN Census        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'PT'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN PT        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'PT'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN PT        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'OT'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN OT        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'OT'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN OT        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'ST'        
                     AND @SORT_ORDER = 'ASC'        
                      THEN ST        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'ST'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN ST        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'EP'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN EP        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'EP'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN EP        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Lead'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN LEAD        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Lead'        
                           AND @SORT_ORDER = 'DESC'        
                    THEN LEAD        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'Parent'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN Parent        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'Parent'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN Parent        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'CreatedDate'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN CREATED_DATE        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'CreatedDate'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN CREATED_DATE        
                  END DESC,        
                  CASE        
                      WHEN @SORT_BY = 'ModifiedDate'        
                           AND @SORT_ORDER = 'ASC'        
                      THEN MODIFIED_DATE        
                  END ASC,        
                  CASE        
                      WHEN @SORT_BY = 'ModifiedDate'        
                           AND @SORT_ORDER = 'DESC'        
                      THEN MODIFIED_DATE        
                  END DESC        
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;        
     END; 