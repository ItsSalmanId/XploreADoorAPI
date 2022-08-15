IF (OBJECT_ID('FOX_PROC_GET_IDENTIFIERS_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_IDENTIFIERS_LIST 
GO   
CREATE PROCEDURE dbo.FOX_PROC_GET_IDENTIFIERS_LIST(@PRACTICE_CODE BIGINT)  
AS  
     BEGIN  
         SELECT fti.IDENTIFIER_ID,   
                fti.PRACTICE_CODE,   
                fti.IDENTIFIER_TYPE_ID,   
                fti.NAME,   
                fti.CODE,   
                fti.DESCRIPTION,   
                fti.CODE_NAME,   
                fti.CREATED_BY,   
                fti.CREATED_DATE,   
                fti.MODIFIED_BY,   
                fti.MODIFIED_DATE,   
                fti.IS_ACTIVE  
         FROM dbo.FOX_TBL_IDENTIFIER fti  
         WHERE fti.DELETED = 0  
               AND fti.PRACTICE_CODE = @PRACTICE_CODE;  
     END;