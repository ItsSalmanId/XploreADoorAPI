IF (OBJECT_ID('FOX_GET_IDENTIFIER') IS NOT NULL) DROP PROCEDURE FOX_GET_IDENTIFIER
GO
CREATE PROCEDURE [dbo].[FOX_GET_IDENTIFIER] --1011163, 'ACO Identifier','Mssp_car'        
@PRACTICE_CODE BIGINT,         
@TYPE          VARCHAR(100),         
@SEARCHVALUE   VARCHAR(MAX)        
AS        
     BEGIN        
         --DECLARE @PRACTICE_CODE BIGINT = 1011163        
         -- ,@TYPE VARCHAR(100) = 'ACO Identifier'        
         -- ,@SEARCHVALUE VARCHAR(MAX) = 'Mssp_car'        
         IF(@SEARCHVALUE = '')        
             BEGIN        
                 SET @SEARCHVALUE = NULL;        
             END;        
         SELECT TOP (100) i.IDENTIFIER_ID,         
                          i.IDENTIFIER_TYPE_ID,         
                          i.CODE,         
                          i.NAME,         
                          i.CODE_NAME,         
                          i.[DESCRIPTION]        
         FROM FOX_TBL_IDENTIFIER AS i        
              JOIN FOX_TBL_IDENTIFIER_TYPE AS it ON i.IDENTIFIER_TYPE_ID = it.IDENTIFIER_TYPE_ID        
                                                    AND ISNULL(it.DELETED, 0) = 0        
                                                    AND it.PRACTICE_CODE = @PRACTICE_CODE        
                                                    AND it.NAME = @TYPE        
         WHERE(i.CODE LIKE '%'+@SEARCHVALUE+'%'        
               OR i.NAME LIKE '%'+@SEARCHVALUE+'%'        
               OR i.CODE_NAME LIKE '%'+@SEARCHVALUE+'%'      
      OR i.DESCRIPTION LIKE '%'+@SEARCHVALUE+'%')        
              AND ISNULL(i.DELETED, 0) = 0        
              AND i.PRACTICE_CODE = @PRACTICE_CODE        
              AND ISNULL(i.IS_ACTIVE, 1) = 1        
         --AND i.IDENTIFIER_TYPE_ID = @TYPE_ID        
     END;

