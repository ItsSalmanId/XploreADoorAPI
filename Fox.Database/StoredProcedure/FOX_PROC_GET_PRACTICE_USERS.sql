IF (OBJECT_ID('FOX_PROC_GET_PRACTICE_USERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PRACTICE_USERS  
GO
   --SP_HELPTEXT FOX_PROC_GET_PRACTICE_USERS                    
-- =============================================                    
-- Author:  <Author,Mehmood ul Hassan>                    
-- Create date: <Create Date,12/10/2017>                    
-- DESCRIPTION: <GET USers for settings>                 
                 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PRACTICE_USERS] --1011163,NULL,0,1,0                    
--PRACTICE CODE CHECK WILL BE DONE LATER                        
 (                        
 @PRACTICE_CODE BIGINT                        
 ,@SEARCH_GO VARCHAR(30)                        
 ,@RECORD_PER_PAGE INT                        
 ,@CURRENT_PAGE INT          
 ,@FILTER_IS_APPROVED bit          
 )                        
AS          
 --DECLARE @PRACTICE_CODE BIGINT = 1012714                     
 --,@SEARCH_GO VARCHAR(30) =    ''            
 --,@RECORD_PER_PAGE INT = 2000                  
 --,@CURRENT_PAGE INT = 1          
 --,@FILTER_IS_APPROVED bit = 1          
BEGIN                        
 SET NOCOUNT ON;                        
                        
 IF (@RECORD_PER_PAGE = 0)                        
 BEGIN                        
  SELECT @RECORD_PER_PAGE = COUNT(*)                        
  FROM FOX_TBL_APPLICATION_USER                        
 END                        
 ELSE                        
 BEGIN                        
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                        
 END                        
                        
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                        
                        
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                        
 DECLARE @TOATL_PAGESUDM FLOAT                        
                        
 SELECT @TOATL_PAGESUDM = COUNT(*)                        
 FROM fox_tbl_application_user u                        
 LEFT JOIN FOX_TBL_ROLE r ON u.ROLE_ID = r.Role_Id                  
 LEFT JOIN FOX_TBL_REFERRAL_REGION rr ON u.REFERRAL_REGION_ID = rr.REFERRAL_REGION_ID   and ISNULL(rr.DELETED, 0) <> 1                  
 LEFT JOIN FOX_TBL_SENDER_TYPE ST ON u.FOX_TBL_SENDER_TYPE_ID = ST.FOX_TBL_SENDER_TYPE_ID   and ISNULL(ST.DELETED, 0) =0                  
 WHERE u.PRACTICE_CODE = @PRACTICE_CODE                     
 AND ISNULL(u.DELETED, 0) <> 1                        
  AND (                        
   @SEARCH_GO IS NULL                        
   --OR ISNULL(USER_NAME, '') LIKE '%' + @SEARCH_GO + '%'                 
                 
   OR  (RTRIM(LTRIM(ISNULL(LAST_NAME, '')))+' '+RTRIM(LTRIM(ISNULL(FIRST_NAME, '')))) LIKE+'%'+ @SEARCH_GO + '%'                
   OR  (RTRIM(LTRIM(ISNULL(FIRST_NAME, '')))+' '+RTRIM(LTRIM(ISNULL(LAST_NAME, '')))) LIKE+'%'+ @SEARCH_GO + '%'                
   OR ISNULL(FIRST_NAME, '') LIKE '%' + @SEARCH_GO + '%'              
   OR ISNULL(MIDDLE_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
   OR ISNULL(LAST_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
   OR ISNULL(USER_DISPLAY_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
   OR ISNULL(EMAIL, '') LIKE '%' + @SEARCH_GO + '%'                        
   OR ISNULL(r.ROLE_NAME, '') LIKE '%' + @SEARCH_GO + '%'                   
   OR ISNULL(rr.REFERRAL_REGION_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
   OR convert(VARCHAR, u.CREATED_DATE, 101) LIKE '%' + @SEARCH_GO + '%'                        
   OR convert(VARCHAR, u.CREATED_DATE, 100) LIKE '%' + @SEARCH_GO + '%'                    
   OR ISNULL(ST.SENDER_TYPE_NAME, '') LIKE '%' + @SEARCH_GO + '%'                   
                         
   )          
   AND ((@FILTER_IS_APPROVED = 1 AND ISNULL(u.IS_APPROVED,0) <> @FILTER_IS_APPROVED) or @FILTER_IS_APPROVED = 0)          
                        
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM                        
                        
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)                        
                        
 SELECT *                        
  ,@TOATL_PAGESUDM AS TOTAL_RECORD_PAGES                        
  ,@TOTAL_RECORDS TOTAL_RECORDS                        
 FROM (                        
  SELECT USER_ID           
   ,u.PRACTICE_CODE                        
   ,USER_NAME                        
   ,PASSWORD                        
   ,PASSWORD_CHANGED_DATE ,        
   FIRST_NAME,        
   LAST_NAME,        
   FIRST_NAME +' ' +LAST_NAME AS Employee_name                               
   ,MIDDLE_NAME                        
   ,USER_DISPLAY_NAME                        
   ,DESIGNATION                        
   ,DATE_OF_BIRTH                        
   ,EMAIL                        
   ,RESET_PASS                        
   ,SECURITY_QUESTION                        
   ,SECURITY_QUESTION_ANSWER                        
   ,LOCKEDBY                        
   ,LAST_LOGIN_DATE                        
   ,isnull(FAILED_PASSWORD_ATTEMPT_COUNT, 0) FAILED_PASSWORD_ATTEMPT_COUNT                        
   ,IS_LOCKED_OUT                        
   ,COMMENTS                        
   ,PASS_RESET_CODE                        
   ,ACTIVATION_CODE                        
   ,u.IS_ACTIVE                        
   ,IS_ADMIN                        
   ,u.ROLE_ID                        
   ,ADDRESS_1                        
   ,ADDRESS_2                        
   ,CITY                        
   ,STATE                     
   ,ZIP        
   ,u.RT_USER_ID                    
   ,u.CREATED_DATE                        
   ,u.CREATED_BY                        
   ,u.MODIFIED_DATE                        
   ,u.MODIFIED_BY                        
   ,u.DELETED                        
   ,MANAGER_ID                        
   ,r.ROLE_NAME                  
   ,ST.SENDER_TYPE_NAME                  
   ,ISNULL(rr.REFERRAL_REGION_NAME, '') AS REGION_NAME                  
   ,ISNULL(u.USER_TYPE, 'Internal') AS USER_TYPE                        
   ,ISNULL(u.PRACTICE_NAME, '') AS PRACTICE_NAME                        
   ,ISNULL(u.SENDER_TYPE, '') AS SENDER_TYPE                        
   ,ISNULL(u.NPI, '') AS NPI                        
   ,ISNULL(u.MOBILE_PHONE, '') AS MOBILE_PHONE                        
   ,ISNULL(u.PHONE_NO, '') AS PHONE_NO                        
   ,ISNULL(u.FAX, '') AS FAX                        
   ,ISNULL(u.LANGUAGE, '') AS LANGUAGE                        
   ,ISNULL(u.TERMINATION_DATE, '') AS TERMINATION_DATE                        
   ,ISNULL(u.SIGNATURE_PATH, '') AS SIGNATURE_PATH                        
   ,ISNULL(u.GENDER, '') AS GENDER                        
   ,ISNULL(u.TIME_ZONE, '') AS TIME_ZONE                        
   ,ISNULL(u.WORK_PHONE, '') AS WORK_PHONE                        
   ,ISNULL(u.NOTE_ID, '') AS NOTE_ID                        
   ,ISNULL(u.ACO, '') AS ACO                        
   ,ISNULL(u.SPECIALITY, '') AS SPECIALITY                        
   ,ISNULL(u.SNF, '') AS SNF                        
   ,ISNULL(u.HHH, '') AS HHH                        
   ,ISNULL(u.HOSPITAL, '') AS HOSPITAL                        
   ,ISNULL(u.THIRD_PARTY_REFERRAL_SOURCE, '') AS THIRD_PARTY_REFERRAL_SOURCE                        
   ,ISNULL(u.IS_APPROVED, '') AS IS_APPROVED                      
   ,ISNULL(u.IS_AD_USER,0) AS IS_AD_USER                      
   ,ROW_NUMBER() OVER (                        
    ORDER BY U.CREATED_DATE ASC                        
    ) AS ACTIVEROW                        
  FROM fox_tbl_application_user u                        
  LEFT JOIN FOX_TBL_ROLE r ON u.ROLE_ID = r.Role_Id                   
  LEFT JOIN FOX_TBL_REFERRAL_REGION rr ON u.REFERRAL_REGION_ID = rr.REFERRAL_REGION_ID   and ISNULL(rr.DELETED, 0) <> 1                       
   LEFT JOIN FOX_TBL_SENDER_TYPE ST ON u.FOX_TBL_SENDER_TYPE_ID = ST.FOX_TBL_SENDER_TYPE_ID   and ISNULL(ST.DELETED, 0) = 0          
  WHERE u.PRACTICE_CODE = @PRACTICE_CODE                      
   AND ISNULL(u.DELETED, 0) <> 1                        
   AND (                        
    @SEARCH_GO IS NULL                        
    --OR ISNULL(USER_NAME, '') LIKE '%' + @SEARCH_GO + '%'           
 OR  (RTRIM(LTRIM(ISNULL(LAST_NAME, '')))+' '+RTRIM(LTRIM(ISNULL(FIRST_NAME, '')))) LIKE+'%'+ @SEARCH_GO + '%'                
    OR  (RTRIM(LTRIM(ISNULL(FIRST_NAME, '')))+' '+RTRIM(LTRIM(ISNULL(LAST_NAME, '')))) LIKE+'%'+ @SEARCH_GO + '%'                
    OR ISNULL(FIRST_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
    OR ISNULL(MIDDLE_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
    OR ISNULL(LAST_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
    OR ISNULL(USER_DISPLAY_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
    OR ISNULL(EMAIL, '') LIKE '%' + @SEARCH_GO + '%'                        
    OR ISNULL(r.ROLE_NAME, '') LIKE '%' + @SEARCH_GO + '%'                  
 OR ISNULL(rr.REFERRAL_REGION_NAME, '') LIKE '%' + @SEARCH_GO + '%'                        
    OR convert(VARCHAR, u.CREATED_DATE, 101) LIKE '%' + @SEARCH_GO + '%'          
    OR convert(VARCHAR, u.CREATED_DATE, 100) LIKE '%' + @SEARCH_GO + '%'                        
 OR ISNULL(ST.SENDER_TYPE_NAME, '') LIKE '%' + @SEARCH_GO + '%'     
  OR ISNULL(u.RT_USER_ID, '') LIKE '%' + @SEARCH_GO + '%'                   
    )          
 AND ((@FILTER_IS_APPROVED = 1 AND ISNULL(u.IS_APPROVED,0) <> @FILTER_IS_APPROVED) or @FILTER_IS_APPROVED = 0)          
  ) AS USERS                        
 ORDER BY           
 CASE WHEN @FILTER_IS_APPROVED = 1 THEN CREATED_DATE END DESC,           
 CASE WHEN @FILTER_IS_APPROVED = 0 THEN CREATED_DATE END DESC           
 OFFSET @START_FROM ROWS                        
                        
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                        
END 
