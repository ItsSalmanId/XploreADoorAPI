IF (OBJECT_ID('FOX_GET_SMART_PROCEDURES_WEBEHR') IS NOT NULL) DROP PROCEDURE FOX_GET_SMART_PROCEDURES_WEBEHR
GO
-- =============================================                        
-- AUTHOR:  <MUHAMMAD MASHOOD ASHIQ>                        
-- CREATE DATE: <OCT 05, 2012>                        
-- DESCRIPTION: < GET PROCEDURE BY CATEGORY >        
-- ============================================        
CREATE PROCEDURE [dbo].[FOX_GET_SMART_PROCEDURES_WEBEHR] --'','office','a',1011163,0,0,''                     
 @PROCCODE VARCHAR(100)  
 ,@PROCDESCRIPTION VARCHAR(1000)  
 ,@CATEGORY VARCHAR(50)  
 ,@PRACTICE_CODE BIGINT  
 ,@PROVIDER_CODE BIGINT  
 ,@LOCATION_CODE BIGINT  
 ,@DOS VARCHAR(40)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 IF (ISNULL(@DOS, '') = '')  
 BEGIN  
  SET @DOS = CONVERT(DATE, GETDATE(), 101)  
 END  
  
 IF (@CATEGORY = 'm')  
 BEGIN  
  IF (@PROCCODE <> '')  
  BEGIN  
   SELECT DISTINCT (p.proc_code) proc_code  
    ,p.proc_pos_code  
    ,cpt_deleted_expiry_date  
    ,  
    --CASE WHEN CONVERT(VARCHAR,PROC_EFFECTIVE_DATE,101) < CONVERT(VARCHAR,GETDATE(),101)                          
    --CASE WHEN upper(cpt_current_status) = 'DELETED' or CONVERT(VARCHAR,PROC_EFFECTIVE_DATE,101) > CONVERT(VARCHAR,GETDATE(),101)        
    CASE   
     WHEN (  
       (  
        CPT_DELETED_EXPIRY_DATE != '1900-01-01 00:00:00.000'  
        AND CAST(CPT_DELETED_EXPIRY_DATE AS DATE) <= CAST(@DOS AS DATE)  
        )  
       OR CAST(PROC_EFFECTIVE_DATE AS DATE) > CAST(@DOS AS DATE)  
       )  
      THEN REPLACE(proc_description, '(RED)', '') + ' ' + '(RED)'  
     ELSE proc_description  
     END AS proc_description  
    ,ISNULL(LONG_DESCRIPTION, '') LONG_DESCRIPTION  
   FROM [procedures] p  
    ,PROVIDER_PROCEDURES pp  
   LEFT JOIN cpt_current_status cs ON cs.cpt_code = pp.proc_code  
   WHERE p.proc_code = pp.proc_code  
    AND isnull(cs.deleted, 0) = 0  
    AND p.proc_code LIKE '%' + @PROCCODE + '%'  
    AND pp.practice_CODE = @PRACTICE_CODE  
    AND pp.PROVIDER_CODE = @PROVIDER_CODE  
    AND pp.location_code = @LOCATION_CODE  
    AND proc_description NOT LIKE '%(RED)'  
    AND isnull(pp.deleted, 0) = 0  
  END  
  ELSE  
  BEGIN  
   SELECT DISTINCT p.proc_code  
    ,p.proc_pos_code  
    ,cpt_deleted_expiry_date  
    ,  
    --CASE WHEN CONVERT(VARCHAR,PROC_EFFECTIVE_DATE,101) < CONVERT(VARCHAR,GETDATE(),101)                          
    --CASE WHEN upper(cpt_current_status) = 'DELETED' or CONVERT(VARCHAR,PROC_EFFECTIVE_DATE,101) > CONVERT(VARCHAR,GETDATE(),101)                 
    CASE   
     WHEN (  
       (  
        CPT_DELETED_EXPIRY_DATE != '1900-01-01 00:00:00.000'  
        AND CAST(CPT_DELETED_EXPIRY_DATE AS DATE) <= CAST(@DOS AS DATE)  
        )  
       OR CAST(PROC_EFFECTIVE_DATE AS DATE) > CAST(@DOS AS DATE)  
       )  
      THEN REPLACE(proc_description, '(RED)', '') + ' ' + '(RED)'  
     ELSE proc_description  
     END AS proc_description  
    ,ISNULL(LONG_DESCRIPTION, '') LONG_DESCRIPTION  
   FROM [procedures] p  
    ,PROVIDER_PROCEDURES pp  
   LEFT JOIN cpt_current_status cs ON cs.cpt_code = pp.proc_code  
   WHERE p.proc_code = pp.proc_code  
    AND isnull(cs.deleted, 0) = 0  
    --and p.proc_description like '%'+@PROCDESCRIPTION+'%'                            
    AND (  
     p.proc_description LIKE '%' + @PROCDESCRIPTION + '%'  
     OR p.proc_code LIKE '%' + @PROCDESCRIPTION + '%'  
     )  
    AND pp.practice_CODE = @PRACTICE_CODE  
    AND pp.PROVIDER_CODE = @PROVIDER_CODE  
    AND pp.location_code = @LOCATION_CODE  
    AND proc_description NOT LIKE '%(RED)'  
    AND isnull(pp.deleted, 0) = 0  
  END  
 END  
 ELSE  
 BEGIN  
  IF (@PROCCODE <> '')  
  BEGIN  
   SELECT DISTINCT TOP 20 Proc_Code  
    ,proc_pos_code  
    ,cpt_deleted_expiry_date  
    ,  
    --CASE WHEN upper(cpt_current_status) = 'DELETED' or CONVERT(VARCHAR,PROC_EFFECTIVE_DATE,101) > CONVERT(VARCHAR,GETDATE(),101)        
    CASE   
     WHEN (  
       (  
        CPT_DELETED_EXPIRY_DATE != '1900-01-01 00:00:00.000'  
        AND CAST(CPT_DELETED_EXPIRY_DATE AS DATE) <= CAST(@DOS AS DATE)  
        )  
       OR CAST(PROC_EFFECTIVE_DATE AS DATE) > CAST(@DOS AS DATE)  
       )  
      THEN REPLACE(proc_description, '(RED)', '') + ' ' + '(RED)'  
     ELSE proc_description  
     END AS proc_description  
    ,ISNULL(LONG_DESCRIPTION, '') LONG_DESCRIPTION  
   FROM [procedures] p  
   LEFT JOIN cpt_current_status cs ON cs.cpt_code = p.proc_code  
   WHERE isnull(p.deleted, 0) = 0  
    AND isnull(cs.deleted, 0) = 0  
    AND (  
     p.proc_description LIKE '%' + @PROCDESCRIPTION + '%'  
     OR p.proc_code LIKE '%' + @PROCDESCRIPTION + '%'  
     )  
  END  
  ELSE  
  BEGIN  
   SELECT DISTINCT TOP 20 Proc_Code  
    ,proc_pos_code  
    ,cpt_deleted_expiry_date  
    ,  
    --CASE WHEN upper(cpt_current_status) = 'DELETED' or CONVERT(VARCHAR,PROC_EFFECTIVE_DATE,101) > CONVERT(VARCHAR,GETDATE(),101)       
    CASE   
     WHEN (  
       (  
        CPT_DELETED_EXPIRY_DATE != '1900-01-01 00:00:00.000'  
        AND CAST(CPT_DELETED_EXPIRY_DATE AS DATE) <= CAST(@DOS AS DATE)  
        )  
       OR CAST(PROC_EFFECTIVE_DATE AS DATE) > CAST(@DOS AS DATE)  
       )  
      THEN REPLACE(proc_description, '(RED)', '') + ' ' + '(RED)'  
     ELSE proc_description  
     END AS proc_description  
    ,ISNULL(LONG_DESCRIPTION, '') LONG_DESCRIPTION  
   FROM [procedures] p  
   LEFT JOIN cpt_current_status cs ON cs.cpt_code = p.proc_code  
   WHERE isnull(p.deleted, 0) = 0  
    AND isnull(cs.deleted, 0) = 0  
    AND (  
     p.proc_description LIKE '%' + @PROCDESCRIPTION + '%'  
     OR p.proc_code LIKE '%' + @PROCDESCRIPTION + '%'  
     )  
  END  
 END  
END  
