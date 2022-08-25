IF (OBJECT_ID('FOX_GET_SMART_PROVIDER') IS NOT NULL) DROP PROCEDURE FOX_GET_SMART_PROVIDER
GO
--4
CREATE PROCEDURE [dbo].[FOX_GET_SMART_PROVIDER] --1011163,'TIMO', 'PT'                      
(@PRACTICE_CODE   BIGINT,         
 @SEARCHVALUE     VARCHAR(200),         
 @DISCIPLINE_NAME VARCHAR(100) = NULL        
--DECLARE @PRACTICE_CODE BIGINT= 1011163, @SEARCHVALUE VARCHAR(200)= '158', @DISCIPLINE_NAME VARCHAR(100)= 'PT';        
)        
AS        
     BEGIN        
         IF(@SEARCHVALUE = '')        
             BEGIN        
                 SET @SEARCHVALUE = NULL;        
             END;        
         IF(@DISCIPLINE_NAME = '')        
             BEGIN        
                 SET @DISCIPLINE_NAME = NULL;        
             END;          
         --          
         SELECT TOP (100) *        
         FROM FOX_TBL_PROVIDER AS P        
         WHERE(P.FIRST_NAME LIKE '%'+@SEARCHVALUE+'%'        
               OR P.LAST_NAME LIKE '%'+@SEARCHVALUE+'%'        
               OR P.FIRST_NAME+' '+P.LAST_NAME LIKE '%'+@SEARCHVALUE+'%'        
               OR P.LAST_NAME+', '+P.FIRST_NAME LIKE '%'+@SEARCHVALUE+'%'        
               OR P.INDIVIDUAL_NPI LIKE '%'+@SEARCHVALUE+'%')        
              AND ISNULL(P.DELETED, 0) = 0        
              AND P.PRACTICE_CODE = @PRACTICE_CODE        
              --AND P.[STATUS] LIKE 'Active'       
              AND P.IS_INACTIVE = 0  
              AND (DISCIPLINE_ID IN        
					 (        
						 SELECT DISCIPLINE_ID        
						 FROM FOX_TBL_DISCIPLINE        
						 WHERE NAME LIKE @DISCIPLINE_NAME        
							   AND ISNULL(DELETED, 0) = 0        
							   AND PRACTICE_CODE = @PRACTICE_CODE        
					 )        
                   OR DISCIPLINE_ID IS NULL        
                   OR @DISCIPLINE_NAME IS NULL);        
     END;

