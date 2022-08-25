IF (OBJECT_ID('FOX_PROC_GET_ADVANCE_SEARCH_INSURANCE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ADVANCE_SEARCH_INSURANCE
GO
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ADVANCE_SEARCH_INSURANCE]                  
(@PRACTICE_CODE         BIGINT,                   
 @INSURANCE_DESCRIPTION VARCHAR(50),                   
 @FINANCIAL_CLASS_ID    INT         = NULL,                   
 @INSURANCE_ADDRESS     VARCHAR(50),                   
 @INSURANCE_ZIP         VARCHAR(10),                   
 @INSURANCE_CITY        VARCHAR(50),                   
 @INSURANCE_STATE       VARCHAR(50),                   
 @SEARCH_STRING         VARCHAR(50),                   
 @CURRENT_PAGE          INT,                   
 @RECORD_PER_PAGE       INT                      
--DECLARE @PRACTICE_CODE BIGINT = 1011163                      
-- ,@INSURANCE_DESCRIPTION VARCHAR(50) = ''                    
-- ,@FINANCIAL_CLASS_ID INT = 544100                      
-- ,@INSURANCE_ADDRESS VARCHAR(50) = ''                      
-- ,@INSURANCE_ZIP VARCHAR(10) = ''                      
-- ,@INSURANCE_CITY VARCHAR(50) = ''                      
-- ,@INSURANCE_STATE VARCHAR(50) = ''                      
-- ,@SEARCH_STRING VARCHAR(50) = ''                      
-- ,@CURRENT_PAGE INT = 1                      
-- ,@RECORD_PER_PAGE INT = 10                      
)                  
AS                  
     BEGIN                  
         IF(@SEARCH_STRING = '')                  
             BEGIN                  
                 SET @SEARCH_STRING = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @SEARCH_STRING = @SEARCH_STRING+'%';                  
             END;                  
         IF(@INSURANCE_DESCRIPTION = '')                  
             BEGIN                  
                 SET @INSURANCE_DESCRIPTION = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @INSURANCE_DESCRIPTION = @INSURANCE_DESCRIPTION+'%';                  
             END;                  
         IF(@FINANCIAL_CLASS_ID = 0)                  
             BEGIN                  
                 SET @FINANCIAL_CLASS_ID = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @INSURANCE_DESCRIPTION = @INSURANCE_DESCRIPTION+'%';                  
             END;                  
         IF(@INSURANCE_ADDRESS = '')                  
             BEGIN                  
                 SET @INSURANCE_ADDRESS = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @INSURANCE_ADDRESS = @INSURANCE_ADDRESS+'%';                  
             END;                  
         IF(@INSURANCE_ZIP = '')                  
             BEGIN                  
                 SET @INSURANCE_ZIP = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @INSURANCE_ZIP = @INSURANCE_ZIP+'%';                  
             END;                  
         IF(@INSURANCE_CITY = '')                  
             BEGIN                  
                 SET @INSURANCE_CITY = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @INSURANCE_CITY = @INSURANCE_CITY+'%';                  
             END;                  
         IF(@INSURANCE_STATE = '')                  
             BEGIN                  
                 SET @INSURANCE_STATE = NULL;                  
             END;                  
             ELSE                  
             BEGIN                  
                 SET @INSURANCE_STATE = @INSURANCE_STATE;         
             END;                  
         IF(@RECORD_PER_PAGE = 0)                  
             BEGIN                  
                 SELECT @RECORD_PER_PAGE = COUNT(*)                  
                 FROM FOX_TBL_INSURANCE;                  
             END;                  
             ELSE                  
      BEGIN                  
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;                  
             END;                  
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;                  
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;                  
         DECLARE @TOATL_PAGESUDM FLOAT;                  
         SELECT @TOATL_PAGESUDM = COUNT(*)                  
         FROM FOX_TBL_INSURANCE i                  
              LEFT JOIN dbo.FOX_TBL_FINANCIAL_CLASS ftfc ON ftfc.FINANCIAL_CLASS_ID = i.FINANCIAL_CLASS_ID                  
  WHERE(@SEARCH_STRING IS NULL  AND i.PRACTICE_CODE = @PRACTICE_CODE        
               OR (i.INSURANCE_NAME LIKE '%'+@SEARCH_STRING+'%'                  
                   OR ftfc.NAME LIKE '%'+@SEARCH_STRING+'%'                  
     OR i.ADDRESS LIKE+'%'+@SEARCH_STRING+'%'                  
                   --OR i.ADDRESS_1 LIKE+'%'+@SEARCH_STRING+'%'                  
                   OR i.ZIP LIKE '%'+@SEARCH_STRING+'%'                  
        OR i.City LIKE '%'+@SEARCH_STRING+'%'                  
                   OR i.[STATE] LIKE '%'+@SEARCH_STRING+'%'))                  
              AND (@INSURANCE_DESCRIPTION IS NULL                  
                   OR i.INSURANCE_NAME LIKE '%'+@INSURANCE_DESCRIPTION+'%')                  
              AND (@FINANCIAL_CLASS_ID IS NULL                  
                   OR i.FINANCIAL_CLASS_ID = @FINANCIAL_CLASS_ID)                  
              AND (@INSURANCE_ADDRESS IS NULL                  
          OR i.ADDRESS LIKE '%'+@INSURANCE_ADDRESS+'%'                  
                   --OR i.ADDRESS_2 LIKE '%'+@INSURANCE_ADDRESS+'%'                
       )                  
              AND (@INSURANCE_ZIP IS NULL                  
                   OR i.ZIP LIKE '%'+@INSURANCE_ZIP+'%')                  
              AND (@INSURANCE_CITY IS NULL                  
                   OR i.City LIKE '%'+@INSURANCE_CITY+'%')                  
              AND (@INSURANCE_STATE IS NULL                  
                   OR i.[STATE] LIKE '%'+@INSURANCE_STATE+'%')                  
              AND (@SEARCH_STRING IS NOT NULL                  
                   OR @INSURANCE_DESCRIPTION IS NOT NULL                  
                   OR @FINANCIAL_CLASS_ID IS NOT NULL                  
                   OR @INSURANCE_ADDRESS IS NOT NULL                  
                   OR @INSURANCE_ZIP IS NOT NULL                  
                   OR @INSURANCE_CITY IS NOT NULL                  
                   OR @INSURANCE_STATE IS NOT NULL);                  
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;                  
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);                  
         SELECT *,                   
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                   
                @TOTAL_RECORDS TOTAL_RECORDS                  
         FROM                  
         (                  
             SELECT i.FOX_TBL_INSURANCE_ID,             
					i.PRACTICE_CODE,        
                    i.INSURANCE_ID,                   
                    i.INSURANCE_NAME AS InsPayer_Description,                   
                    isnull(ftfc.FINANCIAL_CLASS_ID, 0) FINANCIAL_CLASS_ID,                   
                    isnull(ftfc.NAME, '') FINANCIAL_CLASS_NAME,                   
                    isnull(i.ADDRESS, '') Insurance_Address,                   
                    --isnull(i.ADDRESS_1, '') ADDRESS_2,       
                    isnull(i.ZIP, '') Insurance_Zip,                   
                    isnull(i.City, '') Insurance_City,                   
                    isnull(i.[STATE], '') Insurance_State,                   
                    isnull(i.PHONE, '') Insurance_Phone_Number1,                   
                    isnull(i.FAX, '') FAX                  
             FROM FOX_TBL_INSURANCE i                  
                  LEFT JOIN dbo.FOX_TBL_FINANCIAL_CLASS ftfc ON ftfc.FINANCIAL_CLASS_ID = i.FINANCIAL_CLASS_ID                  
             WHERE(@SEARCH_STRING IS NULL  AND i.PRACTICE_CODE = @PRACTICE_CODE        
                   OR (i.INSURANCE_NAME LIKE '%'+@SEARCH_STRING+'%'                  
                       OR ftfc.NAME LIKE '%'+@SEARCH_STRING+'%'                  
                       OR i.ADDRESS LIKE+'%'+@SEARCH_STRING+'%'                
                       --OR i.ADDRESS_1 LIKE+'%'+@SEARCH_STRING+'%'                  
                       OR i.ZIP LIKE '%'+@SEARCH_STRING+'%'                  
                       OR i.City LIKE '%'+@SEARCH_STRING+'%'                  
                       OR i.[STATE] LIKE '%'+@SEARCH_STRING+'%'))                  
                  AND (@INSURANCE_DESCRIPTION IS NULL                  
                       OR i.INSURANCE_NAME LIKE '%'+@INSURANCE_DESCRIPTION+'%')                  
                  AND (@FINANCIAL_CLASS_ID IS NULL                  
                       OR i.FINANCIAL_CLASS_ID = @FINANCIAL_CLASS_ID)                  
                  AND (@INSURANCE_ADDRESS IS NULL                  
                       OR i.ADDRESS LIKE '%'+@INSURANCE_ADDRESS+'%'                  
                       --OR i.ADDRESS_1 LIKE '%'+@INSURANCE_ADDRESS+'%'                
        )                  
                  AND (@INSURANCE_ZIP IS NULL                  
                       OR i.ZIP LIKE '%'+@INSURANCE_ZIP+'%')                  
                  AND (@INSURANCE_CITY IS NULL                  
                       OR i.City LIKE '%'+@INSURANCE_CITY+'%')                  
                  AND (@INSURANCE_STATE IS NULL                  
                       OR i.[STATE] LIKE '%'+@INSURANCE_STATE+'%')                  
                  AND (@SEARCH_STRING IS NOT NULL                  
                       OR @INSURANCE_DESCRIPTION IS NOT NULL                  
                       OR @FINANCIAL_CLASS_ID IS NOT NULL                  
                       OR @INSURANCE_ADDRESS IS NOT NULL                  
                       OR @INSURANCE_ZIP IS NOT NULL                  
                       OR @INSURANCE_CITY IS NOT NULL                  
                       OR @INSURANCE_STATE IS NOT NULL)                  
     ) AS INSURANCES                  
         ORDER BY INSURANCES.InsPayer_Description ASC                  
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;                  
     END; 
