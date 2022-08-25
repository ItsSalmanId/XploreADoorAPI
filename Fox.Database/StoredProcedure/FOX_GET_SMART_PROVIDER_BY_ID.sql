IF (OBJECT_ID('FOX_GET_SMART_PROVIDER_BY_ID') IS NOT NULL) DROP PROCEDURE FOX_GET_SMART_PROVIDER_BY_ID
GO
CREATE PROCEDURE [dbo].[FOX_GET_SMART_PROVIDER_BY_ID] --1011163,'TIMO'                                  
(@PRACTICE_CODE BIGINT,                 
 @SEARCHVALUE   VARCHAR(200),                 
 @DISCIPLINE_ID INT          = NULL                    
--DECLARE @PRACTICE_CODE BIGINT= 1011163, @SEARCHVALUE VARCHAR(200)= '158897', @DISCIPLINE_ID INT= 544100                
)                
AS                
BEGIN                
    IF(@SEARCHVALUE = '')                
        BEGIN                
            SET @SEARCHVALUE = NULL                
        END                
    IF(@DISCIPLINE_ID = 0 OR @DISCIPLINE_ID = '')                
        BEGIN                
            SET @DISCIPLINE_ID = NULL                
        END                      
                
 --                
 IF EXISTS (SELECT * FROM FOX_TBL_DISCIPLINE WHERE NAME LIKE 'CS' AND DISCIPLINE_ID = @DISCIPLINE_ID AND ISNULL(DELETED, 0) = 0 AND PRACTICE_CODE = @PRACTICE_CODE)                
 BEGIN                
        SET @DISCIPLINE_ID = NULL                
 END                
    --                      
    SELECT TOP (100) p.*,           
   --rr.REFERRAL_REGION_ID AS REF_REGION_ID,          
   rr.REFERRAL_REGION_CODE,          
   rr.REFERRAL_REGION_NAME          
    FROM FOX_TBL_PROVIDER AS P          
  --LEFT JOIN FOX_TBL_REFERRAL_REGION AS rr ON rr.REFERRAL_REGION_CODE = p.REGION_CODE             
  LEFT JOIN FOX_TBL_REFERRAL_REGION AS rr ON rr.REFERRAL_REGION_ID = p.REFERRAL_REGION_ID                  
    AND ISNULL(rr.DELETED, 0) = 0                
    AND rr.PRACTICE_CODE = @PRACTICE_CODE             
    WHERE(P.FIRST_NAME LIKE '%'+@SEARCHVALUE+'%'                
          OR P.LAST_NAME LIKE '%'+@SEARCHVALUE+'%'                
          OR P.FIRST_NAME+' '+P.LAST_NAME LIKE '%'+@SEARCHVALUE+'%'                
          OR P.LAST_NAME+', '+P.FIRST_NAME LIKE '%'+@SEARCHVALUE+'%'                
          OR P.INDIVIDUAL_NPI LIKE '%'+@SEARCHVALUE+'%')                
         AND ISNULL(P.DELETED, 0) = 0                
         AND P.PRACTICE_CODE = @PRACTICE_CODE                
         AND P.IS_INACTIVE = 0               
         AND (DISCIPLINE_ID = @DISCIPLINE_ID OR @DISCIPLINE_ID IS NULL)                 
         OR (DISCIPLINE_ID IS NULL AND P.PRACTICE_CODE = @PRACTICE_CODE)         
 ORDER BY P.LAST_NAME, p.FIRST_NAME              
END 