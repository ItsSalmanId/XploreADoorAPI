IF (OBJECT_ID('FOX_PROC_GET_UNMAPPED_INSURANCES_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_UNMAPPED_INSURANCES_LIST  
GO 
	 -- =============================================    
-- AUTHOR:  <DEVELOPER, YOUSAF>    
-- CREATE DATE: <CREATE DATE, 29/11/2018>    
-- DESCRIPTION: <GET UNMAPPED INSURANCES LIST>    
-- =============================================    
-- EXEC FOX_PROC_GET_UNMAPPED_INSURANCES_LIST 1011163, 1,'', 1, 10  , '', '', '', '', '', '','', '','','','','Insurance_name','ASC'  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_UNMAPPED_INSURANCES_LIST]  
@PRACTICE_CODE BIGINT,  
@INCLUDE_MAPPED BIT,    
@SEARCH_STRING VARCHAR(100),    
@CURRENT_PAGE INT,    
@RECORD_PER_PAGE INT,  
@State varchar(50),  
@FinancialClassId varchar(100),  
@PayerID varchar(10),  
@Name varchar(100),  
@Address varchar(100),  
@ZIP varchar(9),  
@Phone varchar(20),  
@Carrier varchar(10),  
@Carrier_Locality varchar(10),  
@Carrier_State varchar(10),  
@Fee_Redirect varchar(10),       
@SORT_BY VARCHAR(50),       
@SORT_ORDER VARCHAR(5)  
AS   
  
          
     BEGIN   
    
   IF(@FinancialClassId = '')  
   BEGIN  
   SET @FinancialClassId = NULL;  
   END    
     
   IF(@State = '')  
   BEGIN  
   SET @State = NULL;  
   END   
   IF(@PayerID = '')  
   BEGIN  
   SET @PayerID = NULL;  
   END   
   IF(@Name = '')  
   BEGIN  
   SET @Name = NULL;  
   END   
   IF(@Address = '')  
   BEGIN  
   SET @Address = NULL;  
   END   
   IF(@ZIP = '')  
   BEGIN  
   SET @ZIP = NULL;  
   END   
   IF(@Phone = '')  
   BEGIN  
   SET @Phone = NULL;  
   END  
    IF(@Carrier = '')  
   BEGIN  
   SET @Carrier = NULL;  
   END   
    IF(@Carrier_Locality = '')  
   BEGIN  
   SET @Carrier_Locality = NULL;  
   END   
    IF(@Carrier_State = '')  
   BEGIN  
   SET @Carrier_State = NULL;  
   END   
    IF(@Fee_Redirect = '')  
   BEGIN  
   SET @Fee_Redirect = NULL;  
   END      
     
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;            
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;    
         DECLARE @TOATL_PAGESUDM FLOAT;     
              
         SELECT @TOATL_PAGESUDM = COUNT(*)            
         FROM FOX_TBL_INSURANCE i  
   left join fox_tbl_financial_class fc on fc.FINANCIAL_CLASS_ID = i.FINANCIAL_CLASS_ID  
    LEFT JOIN FOX_TBL_INSURANCE AS ii ON ii.INSURANCE_PAYERS_ID = i.FEE_REDIRECT  
  AND ISNULL(ii.DELETED, 0) = 0  
  AND ii.PRACTICE_CODE = @PRACTICE_CODE  
         WHERE  ISNULL(I.DELETED, 0) = 0   
   AND I.PRACTICE_CODE = @PRACTICE_CODE   
   AND (@State IS NULL   
    OR (I.STATE LIKE '%' + @State +'%'))  
  AND (@FinancialClassId IS NULL   
    OR (I.FINANCIAL_CLASS_ID LIKE '%' + @FinancialClassId +'%'))  
  AND (@PayerID IS NULL   
    OR (I.INSURANCE_PAYERS_ID LIKE '%' + @PayerID +'%'))  
  AND (@Name IS NULL   
    OR (I.INSURANCE_NAME LIKE '%' + @Name +'%'))  
  AND (@Address IS NULL   
    OR ((I.ADDRESS LIKE '%' + @Address +'%')  
     OR (I.ADDRESS_1 LIKE '%' + @Address +'%')))  
  --AND (@Address IS NULL   
  --  OR (I.ADDRESS_1 LIKE '%' + @Address +'%'))  
  AND (@ZIP  IS NULL   
    OR (I.ZIP LIKE '%' + @ZIP +'%'))  
  AND (@Phone IS NULL   
    OR (I.PHONE LIKE '%' + @Phone +'%'))  
  AND (@Carrier IS NULL   
    OR (I.CARRIER LIKE '%' + @Carrier +'%'))  
  AND (@Carrier_Locality IS NULL   
    OR (I.CARRIER_LOCALITY LIKE '%' + @Carrier_Locality +'%'))  
  AND (@Carrier_State IS NULL   
    OR (I.CARRIER_STATE LIKE '%' + @Carrier_State +'%'))  
  AND (@Fee_Redirect IS NULL   
    OR ((i.FEE_REDIRECT +' - '+ ii.INSURANCE_NAME) LIKE '%' + @Fee_Redirect +'%'))  
   AND ( (@INCLUDE_MAPPED = 0 AND ISNULL(i.INSURANCE_ID, 0) = 0) OR @INCLUDE_MAPPED = 1 )    
     AND    
     (    
      i.INSURANCE_NAME LIKE '%'+@SEARCH_STRING+'%'    
      OR i.INSURANCE_PAYERS_ID LIKE '%'+@SEARCH_STRING+'%'    
      OR i.ADDRESS LIKE '%'+@SEARCH_STRING+'%'    
      OR i.ADDRESS_1 LIKE '%'+@SEARCH_STRING+'%'   
      OR i.CITY LIKE '%'+@SEARCH_STRING+'%'   
      OR i.STATE LIKE '%'+@SEARCH_STRING+'%'    
      OR i.ZIP LIKE '%'+@SEARCH_STRING+'%'    
      OR i.PHONE LIKE '%'+@SEARCH_STRING+'%'    
      OR fc.NAME LIKE '%'+@SEARCH_STRING+'%'  
      OR i.CARRIER LIKE '%'+@SEARCH_STRING+'%'  
      OR i.CARRIER_LOCALITY LIKE '%'+@SEARCH_STRING+'%'  
      OR i.CARRIER_STATE LIKE '%'+@SEARCH_STRING+'%'  
      OR (i.FEE_REDIRECT +' - '+ ii.INSURANCE_NAME) LIKE '%'+@SEARCH_STRING+'%'      
     )    
            
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
         FROM            
         (            
             SELECT i.*,fc.NAME as FC_NAME  
    ,convert(varchar,i.CREATED_DATE) AS Created_Date_Str  
    ,convert(varchar,i.MODIFIED_DATE) AS Modified_Date_Str  
    ,CASE WHEN i.Is_Authorization_Required = 1  THEN 'YES' WHEN i.Is_Authorization_Required  = 0 THEN 'NO' END as Inactive   
    ,ROW_NUMBER() OVER ( ORDER BY i.INSURANCE_NAME ASC ) AS ACTIVEROW  
    ,(i.FEE_REDIRECT +' - '+ ii.INSURANCE_NAME) AS FEE_PLAN_REDIRECT  
    FROM FOX_TBL_INSURANCE i  
 left join fox_tbl_financial_class fc on fc.FINANCIAL_CLASS_ID = i.FINANCIAL_CLASS_ID  
  AND ISNULL(fc.DELETED, 0) = 0  
  AND fc.PRACTICE_CODE = @PRACTICE_CODE  
 LEFT JOIN FOX_TBL_INSURANCE AS ii ON ii.INSURANCE_PAYERS_ID = i.FEE_REDIRECT  
  AND ISNULL(ii.DELETED, 0) = 0  
  AND ii.PRACTICE_CODE = @PRACTICE_CODE  
    WHERE ISNULL(I.DELETED, 0) = 0   
 AND I.PRACTICE_CODE = @PRACTICE_CODE   
 AND (@State IS NULL   
    OR (I.STATE LIKE '%' + @State +'%'))  
 AND (@FinancialClassId IS NULL   
    OR (I.FINANCIAL_CLASS_ID LIKE '%' + @FinancialClassId +'%'))  
   AND (@PayerID IS NULL   
    OR (I.INSURANCE_PAYERS_ID LIKE '%' + @PayerID +'%'))  
  AND (@Name IS NULL   
    OR (I.INSURANCE_NAME LIKE '%' + @Name +'%'))  
  AND (@Address IS NULL   
    OR ((I.ADDRESS LIKE '%' + @Address +'%')  
     OR (I.ADDRESS_1 LIKE '%' + @Address +'%')))  
  --AND (@Address IS NULL   
  --  OR (I.ADDRESS_1 LIKE '%' + @Address +'%'))  
  AND (@ZIP  IS NULL   
    OR (I.ZIP LIKE '%' + @ZIP +'%'))  
  AND (@Phone IS NULL   
    OR (I.PHONE LIKE '%' + @Phone +'%'))  
  AND (@Carrier IS NULL   
    OR (I.CARRIER LIKE '%' + @Carrier +'%'))  
  AND (@Carrier_Locality IS NULL   
    OR (I.CARRIER_LOCALITY LIKE '%' + @Carrier_Locality +'%'))  
  AND (@Carrier_State IS NULL   
    OR (I.CARRIER_STATE LIKE '%' + @Carrier_State +'%'))  
  AND (@Fee_Redirect IS NULL   
    OR ((i.FEE_REDIRECT +' - '+ ii.INSURANCE_NAME) LIKE '%' + @Fee_Redirect +'%'))  
    AND ( (@INCLUDE_MAPPED = 0 AND ISNULL(i.INSURANCE_ID, 0) = 0) OR @INCLUDE_MAPPED = 1 )    
    AND    
    (    
     i.INSURANCE_NAME LIKE '%'+@SEARCH_STRING+'%'    
     OR i.INSURANCE_PAYERS_ID LIKE '%'+@SEARCH_STRING+'%'    
     OR i.ADDRESS LIKE '%'+@SEARCH_STRING+'%'    
     OR i.ADDRESS_1 LIKE '%'+@SEARCH_STRING+'%'   
     OR i.CITY LIKE '%'+@SEARCH_STRING+'%'   
     OR i.STATE LIKE '%'+@SEARCH_STRING+'%'    
     OR i.ZIP LIKE '%'+@SEARCH_STRING+'%'    
     OR i.PHONE LIKE '%'+@SEARCH_STRING+'%'    
     OR fc.NAME LIKE '%'+@SEARCH_STRING+'%'   
     OR i.CARRIER LIKE '%'+@SEARCH_STRING+'%'  
     OR i.CARRIER_LOCALITY LIKE '%'+@SEARCH_STRING+'%'  
     OR i.CARRIER_STATE LIKE '%'+@SEARCH_STRING+'%'  
     OR (i.FEE_REDIRECT +' - '+ ii.INSURANCE_NAME) LIKE '%'+@SEARCH_STRING+'%'   
    )      
         ) AS FOX_TBL_INSURANCE            
         ORDER BY CASE      
                      WHEN @SORT_BY = 'Insurance_name'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN INSURANCE_NAME      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Insurance_name'      
                           AND @SORT_ORDER = 'DESC'      
                      THEN INSURANCE_NAME      
                END DESC,      
                  CASE      
                      WHEN @SORT_BY = 'Payer_id'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN INSURANCE_PAYERS_ID      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Payer_id'      
                           AND @SORT_ORDER = 'DESC'      
                THEN INSURANCE_PAYERS_ID      
                  END DESC,  
      CASE      
                      WHEN @SORT_BY = 'Adress'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN ADDRESS      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Adress'      
                           AND @SORT_ORDER = 'DESC'      
                      THEN ADDRESS      
                  END DESC,      
                  CASE      
                      WHEN @SORT_BY = 'Adress_1'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN ADDRESS_1      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Adress_1'      
                           AND @SORT_ORDER = 'DESC'      
                THEN ADDRESS_1      
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
     WHEN @SORT_BY = 'ZIP'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN ZIP      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'ZIP'      
                           AND @SORT_ORDER = 'DESC'      
     THEN ZIP      
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
                      WHEN @SORT_BY = 'Name'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN FC_NAME      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Name'      
                           AND @SORT_ORDER = 'DESC'      
                      THEN FC_NAME      
                  END DESC,      
                  CASE      
                      WHEN @SORT_BY = 'Is_Authorization_Required'      
                           AND @SORT_ORDER = 'ASC'     
                      THEN Is_Authorization_Required      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Is_Authorization_Required'      
                           AND @SORT_ORDER = 'DESC'      
                THEN Is_Authorization_Required      
                  END DESC,  
      CASE      
                      WHEN @SORT_BY = 'Locality'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN CARRIER_LOCALITY      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Locality'      
                           AND @SORT_ORDER = 'DESC'      
                THEN CARRIER_LOCALITY      
                  END DESC,  
      CASE      
                      WHEN @SORT_BY = 'Carrier'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN CARRIER      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Carrier'      
                           AND @SORT_ORDER = 'DESC'      
                THEN CARRIER      
                  END DESC,  
      CASE      
                      WHEN @SORT_BY = 'State_2'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN CARRIER_STATE      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'State_2'      
                           AND @SORT_ORDER = 'DESC'      
                THEN CARRIER_STATE      
                  END DESC,  
      CASE      
                      WHEN @SORT_BY = 'Fee_redirect'      
                           AND @SORT_ORDER = 'ASC'      
                      THEN FEE_PLAN_REDIRECT      
                  END ASC,      
                  CASE      
                      WHEN @SORT_BY = 'Fee_redirect'      
                           AND @SORT_ORDER = 'DESC'      
                THEN FEE_PLAN_REDIRECT      
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
     END    
