IF (OBJECT_ID('FOX_GET_SMART_COUNTRIES') IS NOT NULL ) DROP PROCEDURE FOX_GET_SMART_COUNTRIES  
GO 
--EXEC [FOX_GET_SMART_COUNTRIES] 'nadi',1011163                   
CREATE PROCEDURE [dbo].[FOX_GET_SMART_COUNTRIES] @SEARCHVALUE   VARCHAR(MAX)        
                                              --,@PRACTICE_CODE BIGINT                   
AS        
     BEGIN        
          IF(@SEARCHVALUE = '')        
             BEGIN        
                 SET @SEARCHVALUE = NULL;        
             END;            
         SELECT  FOX_TBL_COUNTRY_ID,        
                 CODE,        
                 NAME,        
                 DESCRIPTION,        
                 CREATED_BY,        
                 CREATED_DATE,        
     MODIFIED_BY,        
     MODIFIED_DATE        
         FROM FOX_TBL_COUNTRY        
         WHERE(CODE = @SEARCHVALUE        
               OR NAME LIKE '%'+@SEARCHVALUE+'%'        
              OR DESCRIPTION LIKE '%'+@SEARCHVALUE+'%')        
              AND ISNULL(DELETED, 0) = 0        
              --AND c.PRACTICE_CODE = @PRACTICE_CODE        
              AND ISNULL(IS_ACTIVE, 1) = 1        
END;  
--*************************************************************************************************1**********************************************************************************************************************  
insert into fox_tbl_financial_class (FINANCIAL_CLASS_ID,PRACTICE_CODE,CODE,NAME,DESCRIPTION,CREATED_BY,CREATED_DATE,MODIFIED_BY,MODIFIED_DATE,DELETED,SHOW_FOR_INSURANCE)  
values(600116,1011163,'SA','SA- Special Account','SA-Special Account','FOX TEAM',getdate(),'FOX TEAM',getdate(),0,0)  
--*************************************************************************************************2**********************************************************************************************************************  
insert into fox_tbl_financial_class (FINANCIAL_CLASS_ID,PRACTICE_CODE,CODE,NAME,DESCRIPTION,CREATED_BY,CREATED_DATE,MODIFIED_BY,MODIFIED_DATE,DELETED,SHOW_FOR_INSURANCE)  
values(600117,1012714,'SA','SA- Special Account','SA-Special Account','FOX TEAM',getdate(),'FOX TEAM',getdate(),0,0)