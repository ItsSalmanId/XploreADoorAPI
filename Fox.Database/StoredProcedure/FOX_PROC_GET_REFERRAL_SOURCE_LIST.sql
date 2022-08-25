IF (OBJECT_ID('FOX_PROC_GET_REFERRAL_SOURCE_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_REFERRAL_SOURCE_LIST  
GO  
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------              
CREATE PROCEDURE [dbo].[FOX_PROC_GET_REFERRAL_SOURCE_LIST]  --1011163, '', 1, 10, 'CreatedDate','DESC','','','','','',''   --INDC1 Indiana Central 1                                           
@PRACTICE_CODE   BIGINT,                 
@SEARCH_STRING   VARCHAR(100),                 
@CURRENT_PAGE    INT,                 
@RECORD_PER_PAGE INT,                 
@SORT_BY         VARCHAR(50),                 
@SORT_ORDER      VARCHAR(5),                 
@CODE            VARCHAR(10),                 
@FIRST_NAME      VARCHAR(100),                 
@LAST_NAME       VARCHAR(100),                 
@NPI             VARCHAR(11),                 
@ADDRESS         VARCHAR(500),                 
@REGION          VARCHAR(100)                
AS                
     BEGIN                        
         --                        
         IF(@SEARCH_STRING = '')                
             BEGIN                
                 SET @SEARCH_STRING = NULL;                
             END;                
             ELSE                
             BEGIN                
                 SET @SEARCH_STRING = @SEARCH_STRING+'%';                
             END;                        
         --                        
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                
         DECLARE @TOATL_PAGESUDM FLOAT;                        
         --                        
         SELECT @TOATL_PAGESUDM = COUNT(*)                
         FROM FOX_TBL_ORDERING_REF_SOURCE RS                
              LEFT JOIN fox_tbl_application_user u ON u.USER_NAME = rs.CREATED_BY                
                                                      AND ISNULL(u.DELETED, 0) = 0                
                                                      AND u.PRACTICE_CODE = @PRACTICE_CODE                
              LEFT JOIN fox_tbl_application_user us ON us.USER_NAME = rs.MODIFIED_BY                
                                                       AND ISNULL(us.DELETED, 0) = 0                
                                                       AND us.PRACTICE_CODE = @PRACTICE_CODE                
              LEFT JOIN FOX_TBL_IDENTIFIER i ON i.IDENTIFIER_ID = RS.ACO_ID                
                                                AND ISNULL(i.DELETED, 0) = 0                
                                                AND i.PRACTICE_CODE = @PRACTICE_CODE                
              LEFT JOIN FOX_TBL_PRACTICE_ORGANIZATION po ON po.PRACTICE_ORGANIZATION_ID = RS.PRACTICE_ORGANIZATION_ID                
                                                            AND ISNULL(po.DELETED, 0) = 0                
                                                            AND po.PRACTICE_CODE = @PRACTICE_CODE                
              LEFT JOIN FOX_TBL_REFERRAL_REGION rr ON rr.REFERRAL_REGION_CODE = RS.REFERRAL_REGION                
                                                      AND ISNULL(rr.DELETED, 0) = 0                
                                                      AND rr.PRACTICE_CODE = @PRACTICE_CODE                
              LEFT JOIN FOX_TBL_REFERRAL_SOURCE_INACTIVE_REASON rsai ON rsai.INACTIVE_REASON_ID = RS.INACTIVE_REASON_ID                
                                                                        AND ISNULL(rsai.DELETED, 0) = 0                
                                                                        AND rsai.PRACTICE_CODE = @PRACTICE_CODE                
              LEFT JOIN FOX_TBL_REFERRAL_SOURCE_DELIVERY_METHOD SDM ON SDM.SOURCE_DELIVERY_METHOD_ID = RS.SOURCE_DELIVERY_METHOD_ID                
AND ISNULL(SDM.DELETED, 0) = 0           
                                                                       AND SDM.PRACTICE_CODE = @PRACTICE_CODE              
         WHERE(@SEARCH_STRING IS NULL                
               OR RS.CODE LIKE '%'+@SEARCH_STRING+'%'           
               OR RS.FIRST_NAME LIKE @SEARCH_STRING+'%'                
               OR RS.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'              
               OR RS.TITLE LIKE @SEARCH_STRING+'%'                
               OR RS.REFERRAL_REGION LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.NPI LIKE '%'+@SEARCH_STRING+'%'                
               OR i.CODE LIKE '%'+@SEARCH_STRING+'%'                
               OR i.[NAME] LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.PHONE LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.REFERRAL_REGION LIKE '%'+@SEARCH_STRING+'%'                
               OR po.[NAME] LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.fax LIKE '%'+@SEARCH_STRING+'%'                
               OR CONVERT(VARCHAR, RS.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'                
               OR CONVERT(VARCHAR, RS.CREATED_DATE, 100) LIKE '%'+@SEARCH_STRING+'%'                
               OR (u.LAST_NAME+', '+u.FIRST_NAME) LIKE '%'+@SEARCH_STRING+'%'                
               OR (us.LAST_NAME+', '+us.FIRST_NAME) LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.ADDRESS LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.ADDRESS_2 LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.MI LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.CELL LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.Speciality LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.ADDRESS LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.ADDRESS_2 LIKE '%'+@SEARCH_STRING+'%'        
      OR RS.Email  LIKE '%'+@SEARCH_STRING+'%'               
               OR RS.CITY LIKE '%'+@SEARCH_STRING+'%'                
               OR RS.STATE LIKE '%'+@SEARCH_STRING+'%'                
               OR replace(RS.ZIP, '-', '') LIKE '%'+replace(@SEARCH_STRING, '-', '')+'%'                
               OR ISNULL(rsai.REASON, '') LIKE '%'+@SEARCH_STRING+'%')                
              AND (RS.CODE LIKE '%'+@CODE+'%'                
                   OR i.CODE LIKE '%'+@CODE+'%')                
              AND RS.FIRST_NAME LIKE '%'+@FIRST_NAME+'%'                
              AND RS.LAST_NAME LIKE '%'+@LAST_NAME+'%'                
              AND (RS.CITY LIKE '%'+@ADDRESS+'%'                
                   OR RS.STATE LIKE '%'+@ADDRESS+'%'                
                   OR RS.ZIP LIKE '%'+@ADDRESS+'%'                
                   OR RS.ADDRESS LIKE '%'+@ADDRESS+'%'                
                   OR RS.CITY IS NULL                
                   OR RS.STATE IS NULL                
                   OR RS.ZIP IS NULL                
                   OR RS.ADDRESS IS NULL)                
              AND (RS.REFERRAL_REGION LIKE '%'+@REGION+'%'                
                   OR RS.REFERRAL_REGION IS NULL)                
              AND (RS.NPI LIKE '%'+@NPI+'%'                
                   OR RS.NPI IS NULL)                
              AND (isnulL(rs.DELETED, '0') = 0                
                   AND isnull(u.DELETED, 0) = 0                
                   AND isnull(us.DELETED, 0) = 0                
                   AND RS.PRACTICE_CODE = @PRACTICE_CODE);                
                
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
             SELECT rs.FIRST_NAME,                 
                    RS.LAST_NAME,                 
                    RS.TITLE,                 
                    RS.ADDRESS,                 
   RS.ADDRESS_2,        
     RS.Email,                 
                    RS.CITY,                 
                    RS.STATE,                 
                    RS.ZIP,                 
                    RS.PHONE,                 
                    RS.FAX,                 
                    UPPER(RS.REFERRAL_REGION) AS REFERRAL_REGION,                 
                    '['+rr.REFERRAL_REGION_CODE+'] '+rr.REFERRAL_REGION_NAME AS REFERRAL_REGION_NAME,                                          
                    --,RS.REFERRAL_REGION AS REFERRAL_REGION_NAME                                          
                    RS.NPI,                 
  RS.ORGANIZATION,                 
                    RS.ACO,                 
                    RS.CODE,                 
                    RS.SOURCE_ID,                 
                    RS.PRACTICE_ORGANIZATION_ID,                 
                    RS.INACTIVE_REASON_ID,              
                    CASE                             
                        WHEN RS.INACTIVE_REASON_ID IS NULL                                
                        THEN 'NO'                                
                        ELSE 'YES'                                
                    END AS Inactive,                 
                    RS.INACTIVE_DATE,                 
                    RS.SOURCE_DELIVERY_METHOD_ID,                 
                    SDM.NAME AS SOURCE_DELIVERY_METHOD_NAME,                 
                    RS.NOTES,                 
                    u.LAST_NAME+', '+u.FIRST_NAME CREATED_BY,                 
                    RS.CREATED_DATE,                 
                    CONVERT(VARCHAR, RS.CREATED_DATE) AS Created_Date_Str,                 
                    us.LAST_NAME+', '+us.FIRST_NAME MODIFIED_BY,                 
                    RS.MODIFIED_DATE,                 
                    CONVERT(VARCHAR, RS.MODIFIED_DATE) AS Modified_Date_Str,                 
                    RS.DELETED,                 
                    ROW_NUMBER() OVER(ORDER BY RS.CREATED_DATE DESC) AS ACTIVEROW,                 
                    RS.ACO_ID,                 
                    isnull(i.[CODE_NAME], '') ACO_NAME,                 
                    '['+po.CODE+'] '+po.NAME AS PRACTICE_ORGANIZATION_NAME,                     
                    --isnull(po.[NAME], '') PRACTICE_ORGANIZATION_NAME,                     
                    RS.MI,                 
                    RS.CELL,                 
                    RS.Speciality,                 
                    isnull(rsai.REASON, '') INACTIVE_REASON              
             FROM FOX_TBL_ORDERING_REF_SOURCE RS                
                  LEFT JOIN fox_tbl_application_user u ON u.USER_NAME = rs.CREATED_BY                
                                                          AND ISNULL(u.DELETED, 0) = 0                
                                                          AND u.PRACTICE_CODE = @PRACTICE_CODE                
                  LEFT JOIN fox_tbl_application_user us ON us.USER_NAME = rs.MODIFIED_BY                
                                                           AND ISNULL(us.DELETED, 0) = 0                
                                                           AND us.PRACTICE_CODE = @PRACTICE_CODE               
                  LEFT JOIN FOX_TBL_IDENTIFIER i ON i.IDENTIFIER_ID = RS.ACO_ID                
                                                    AND ISNULL(i.DELETED, 0) = 0                
                                                    AND i.PRACTICE_CODE = @PRACTICE_CODE                
                  LEFT JOIN FOX_TBL_PRACTICE_ORGANIZATION po ON po.PRACTICE_ORGANIZATION_ID = RS.PRACTICE_ORGANIZATION_ID                
                                                                AND ISNULL(po.DELETED, 0) = 0                
                                                                AND po.PRACTICE_CODE = @PRACTICE_CODE              
                  LEFT JOIN FOX_TBL_REFERRAL_REGION rr ON rr.REFERRAL_REGION_CODE = RS.REFERRAL_REGION                
                                                          AND ISNULL(rr.DELETED, 0) = 0                
                                                          AND rr.PRACTICE_CODE = @PRACTICE_CODE                
                  LEFT JOIN FOX_TBL_REFERRAL_SOURCE_INACTIVE_REASON rsai ON rsai.INACTIVE_REASON_ID = RS.INACTIVE_REASON_ID                
                                                                            AND ISNULL(rsai.DELETED, 0) = 0                
                                                                            AND rsai.PRACTICE_CODE = @PRACTICE_CODE                
                  LEFT JOIN FOX_TBL_REFERRAL_SOURCE_DELIVERY_METHOD SDM ON SDM.SOURCE_DELIVERY_METHOD_ID = RS.SOURCE_DELIVERY_METHOD_ID                
                                                                           AND ISNULL(SDM.DELETED, 0) = 0                
                                                                           AND SDM.PRACTICE_CODE = @PRACTICE_CODE                
             WHERE(@SEARCH_STRING IS NULL                
                   OR RS.CODE LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.FIRST_NAME LIKE @SEARCH_STRING+'%'                
                   OR RS.LAST_NAME LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.TITLE LIKE @SEARCH_STRING+'%'                
                   OR RS.REFERRAL_REGION LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.NPI LIKE '%'+@SEARCH_STRING+'%'                
                   OR i.CODE LIKE '%'+@SEARCH_STRING+'%'                
                   OR i.[NAME] LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.PHONE LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.REFERRAL_REGION LIKE '%'+@SEARCH_STRING+'%'                
                   OR po.[NAME] LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.fax LIKE '%'+@SEARCH_STRING+'%'                
                   OR CONVERT(VARCHAR, RS.CREATED_DATE, 101) LIKE '%'+@SEARCH_STRING+'%'                
                   OR CONVERT(VARCHAR, RS.CREATED_DATE, 100) LIKE '%'+@SEARCH_STRING+'%'                
                   OR (u.LAST_NAME+', '+u.FIRST_NAME) LIKE '%'+@SEARCH_STRING+'%'                
                   OR (us.LAST_NAME+', '+us.FIRST_NAME) LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.ADDRESS LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.ADDRESS_2 LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.MI LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.CELL LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.Speciality LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.ADDRESS LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.ADDRESS_2 LIKE '%'+@SEARCH_STRING+'%'        
       OR RS.Email   LIKE '%'+@SEARCH_STRING+'%'               
                   OR RS.CITY LIKE '%'+@SEARCH_STRING+'%'                
                   OR RS.STATE LIKE '%'+@SEARCH_STRING+'%'                
                   OR replace(RS.ZIP, '-', '') LIKE '%'+replace(@SEARCH_STRING, '-', '')+'%'                                   OR ISNULL(rsai.REASON, '') LIKE '%'+@SEARCH_STRING+'%')                
                  AND (RS.CODE LIKE '%'+@CODE+'%'                
                       OR i.CODE LIKE '%'+@CODE+'%')                
                  AND RS.FIRST_NAME LIKE '%'+@FIRST_NAME+'%'                
                  AND RS.LAST_NAME LIKE '%'+@LAST_NAME+'%'                                AND (RS.CITY LIKE '%'+@ADDRESS+'%'                
                       OR RS.STATE LIKE '%'+@ADDRESS+'%'                
                       OR RS.ZIP LIKE '%'+@ADDRESS+'%'                
                       OR RS.ADDRESS LIKE '%'+@ADDRESS+'%'                
                       OR RS.CITY IS NULL                
                       OR RS.STATE IS NULL                
                       OR RS.ZIP IS NULL                
                       OR RS.ADDRESS IS NULL)                
                  AND (RS.REFERRAL_REGION LIKE '%'+@REGION+'%'                
                       OR RS.REFERRAL_REGION IS NULL)                
                  AND (RS.NPI LIKE '%'+@NPI+'%'                
                       OR RS.NPI IS NULL)                
                  AND (isnulL(rs.DELETED, '0') = 0                
                       AND isnull(u.DELETED, 0) = 0                
                       AND isnull(us.DELETED, 0) = 0               
                       AND RS.PRACTICE_CODE = @PRACTICE_CODE)                
         ) AS FOX_TBL_ORDERING_REF_SOURCE                
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
          WHEN @SORT_BY = 'FirstName'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN FIRST_NAME                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'FirstName'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN FIRST_NAME                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'LastName'              
                           AND @SORT_ORDER = 'ASC'                
                      THEN LAST_NAME                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'LastName'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN LAST_NAME                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'Title'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN TITLE                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'Title'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN TITLE                
            END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'ReferralRegion'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN REFERRAL_REGION                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'ReferralRegion'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN REFERRAL_REGION                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'NPI'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN NPI                
                  END ASC,                
                  CASE                
         WHEN @SORT_BY = 'NPI'                
                           AND @SORT_ORDER = 'DESC'                
    THEN NPI                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'Organization'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN PRACTICE_ORGANIZATION_NAME                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'Organization'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN PRACTICE_ORGANIZATION_NAME                
                  END DESC,                
   CASE                
                      WHEN @SORT_BY = 'ACO'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN ACO_NAME                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'ACO'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN ACO_NAME                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'Phone'                
                           AND @SORT_ORDER = 'ASC'                
    THEN PHONE                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'Phone'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN PHONE                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'Fax'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN FAX                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'Fax'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN FAX                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'CreatedBy'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN CREATED_BY                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'CreatedBy'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN CREATED_BY                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'MI'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN MI           
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'MI'              
                           AND @SORT_ORDER = 'DESC'                
                      THEN MI                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'Specialty'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN Speciality                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'Specialty'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN Speciality                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'Cell'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN CELL                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'Cell'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN CELL                
               END DESC,                              
                  --------------------------                              
                
                  CASE                
                      WHEN @SORT_BY = 'Address'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN ADDRESS                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'Address'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN ADDRESS                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'Address_2'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN ADDRESS_2                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'Address_2'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN ADDRESS_2                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'City'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN CITY                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'City'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN CITY                
      END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'State'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN STATE                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'State'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN STATE                
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'InactiveReason'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN INACTIVE_REASON                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'InactiveReason'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN INACTIVE_REASON                
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
                  END DESC,                
                  CASE                
                      WHEN @SORT_BY = 'ZIP'                
                           AND @SORT_ORDER = 'ASC'                
                      THEN ZIP                
                  END ASC,                
                  CASE                
                      WHEN @SORT_BY = 'ZIP'                
                           AND @SORT_ORDER = 'DESC'                
                      THEN ZIP                
                  END DESC                
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;                
     END; 