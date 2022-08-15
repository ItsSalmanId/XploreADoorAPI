IF (OBJECT_ID('FOX_PROC_GET_FOX_PHR_REPORT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_FOX_PHR_REPORT 
GO                                        
-- =============================================                                          
-- AUTHOR:  <DEVELOPER, MUHAMMAD ARQAM>                                          
-- CREATE DATE: <CREATE DATE, 5/16/2020>                                          
-- DESCRIPTION: <GET ALL FOX PHR LIST>                                          
                                
--EXEC [FOX_PROC_GET_FOX_PHR_REPORT] 1012714,'ALL','',1,20, 'CREATED_DATE', 'DESC','ALL'                                                   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_FOX_PHR_REPORT]                                           
 @PRACTICE_CODE        BIGINT,                                          
 @PATIENT_STATUS  VARCHAR(20),                                                                
 @SEARCH_STRING        VARCHAR(100),                                                       
 @CURRENT_PAGE         INT,                                                       
 @RECORD_PER_PAGE      INT,                                          
 @SORT_BY VARCHAR(20),                                          
 @SORT_ORDER VARCHAR(10),                                        
 @INVITATION_STATUS VARCHAR(20)                                    
AS                                          
BEGIN                                          
                                          
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                          
                                          
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                                          
 DECLARE @TOATL_PAGESUDM FLOAT                                          
 DECLARE @Active INT                                          
 DECLARE @Awaited INT                                          
                                        
--ACTIVE                                         
select @Active = COUNT(*)                                        
FROM Patient p    with(nolock)                                
join FOX_TBL_PATIENT ftp  with(nolock) on ftp.Patient_Account=p.Patient_Account                                     
left join WS_FOX_TBL_PHR_USERS phr  with(nolock) on phr.Patient_Account=p.PATIENT_ACCOUNT                                      
where phr.INVITE_STATUS='Active'                                    
AND P.PRACTICE_CODE = @PRACTICE_CODE                                  
AND p.Deleted = 0                                     
AND isnull(ftp.Deleted,0)=0                                  
AND isnull(phr.deleted,0)=0                                            
AND (p.Email_Address is not null  and p.Email_Address !=' ')                                  
AND (p.Chart_Id is not null  and p.Chart_Id != ' ')                                           
AND (p.Gender is not null  and p.Gender != ' ')                      
                      
 -- -- Invitation Status                                        
     AND                                          
     (                               
                                
  phr.INVITE_STATUS=@INVITATION_STATUS                        
     OR                        
  isnull(phr.INVITE_STATUS, 'notsent')=@INVITATION_STATUS                        
  OR                                    
  (                                        
    @INVITATION_STATUS = 'ALL'                                      
  )                                  
  )                                          
                                                 
  -- Patient Status                                        
     AND                                          
     (                                      
   ftp.Patient_Status=@PATIENT_STATUS                                    
   OR                                    
   (                                      
       @PATIENT_STATUS = 'ALL'                                      
) 
     )                                     
                                     
         
  AND                                        
  (@SEARCH_STRING = '' OR(                                        
   CONVERT(VARCHAR(100),p.PATIENT_ACCOUNT) LIKE '%' + @SEARCH_STRING + '%'                                      
   OR P.Chart_Id LIKE '%' + @SEARCH_STRING + '%'                                            
   OR P.First_Name LIKE  '%' + @SEARCH_STRING + '%'                         
   OR P.Last_Name LIKE  '%' + @SEARCH_STRING + '%'                                                           
   OR P.Gender LIKE  '%' + @SEARCH_STRING + '%'                                     
   OR P.Email_Address LIKE  '%' + @SEARCH_STRING + '%'             
   OR P.Email_Address LIKE  '%' + @SEARCH_STRING + '%'                
   OR isnull(phr.INVITE_STATUS, 'not sent') LIKE  '%' + @SEARCH_STRING + '%'                          
   OR replace(phr.INVITE_STATUS,'Response Awaited', 'Pending') LIKE  '%' + @SEARCH_STRING + '%'                                    
   OR ftp.Patient_Status LIKE  '%' + @SEARCH_STRING + '%'                                 )     )                                      
                                     
 --PENDING                                           
select @Awaited = COUNT(*) from                                     
Patient p   with(nolock)                                  
join FOX_TBL_PATIENT ftp  with(nolock) on ftp.Patient_Account=p.Patient_Account                                    
left join WS_FOX_TBL_PHR_USERS phr  with(nolock) on phr.Patient_Account=p.PATIENT_ACCOUNT                
where phr.INVITE_STATUS='Response Awaited'                                    
AND P.PRACTICE_CODE = @PRACTICE_CODE                         
AND p.Deleted = 0                                     
AND isnull(ftp.Deleted,0)=0                                  
AND isnull(phr.deleted,0)=0                                      
AND (p.Email_Address is not null  and p.Email_Address !=' ')                                  
AND (p.Chart_Id is not null  and p.Chart_Id != ' ')                                               
AND (p.Gender is not null  and p.Gender != ' ')                       
                      
 -- -- Invitation Status                                        
     AND                                          
     (                               
                                
  phr.INVITE_STATUS=@INVITATION_STATUS                        
     OR                        
  isnull(phr.INVITE_STATUS, 'notsent')=@INVITATION_STATUS                        
  OR                                    
  (                                        
    @INVITATION_STATUS = 'ALL'                                      
  )                                  
  )                                          
                                                 
  -- Patient Status                                        
     AND                                          
     (                                      
   ftp.Patient_Status=@PATIENT_STATUS                                    
   OR                                    
   (                                      
       @PATIENT_STATUS = 'ALL'                                      
   )                                          
     )                                     
                                     
                                                 
  AND                                        
  (@SEARCH_STRING = '' OR(                                        
   CONVERT(VARCHAR(100),p.PATIENT_ACCOUNT) LIKE '%' + @SEARCH_STRING + '%'                                      
   OR P.Chart_Id LIKE '%' + @SEARCH_STRING + '%'                                            
   OR P.First_Name LIKE  '%' + @SEARCH_STRING + '%'                                                         
   OR P.Last_Name LIKE  '%' + @SEARCH_STRING + '%'                    
   OR P.Gender LIKE  '%' + @SEARCH_STRING + '%'                                     
   OR P.Email_Address LIKE  '%' + @SEARCH_STRING + '%'                                  
   OR isnull(phr.INVITE_STATUS, 'not sent') LIKE  '%' + @SEARCH_STRING + '%'                          
   OR replace(phr.INVITE_STATUS,'Response Awaited', 'Pending') LIKE  '%' + @SEARCH_STRING + '%'                                    
   OR ftp.Patient_Status LIKE  '%' + @SEARCH_STRING + '%'                                           
    )     )                                     
                                     
--TOTAL                                    
 SELECT @TOATL_PAGESUDM = COUNT(*)             
 FROM Patient p  with(nolock)                                   
join FOX_TBL_PATIENT ftp  with(nolock) on ftp.Patient_Account=p.Patient_Account                                    
left join WS_FOX_TBL_PHR_USERS phr  with(nolock) on phr.Patient_Account=p.PATIENT_ACCOUNT           
left join fox_tbl_case cas  with(nolock) on cas.Patient_Account = p.PATIENT_ACCOUNT            
left join FOX_TBL_REFERRAL_REGION reg  with(nolock) on reg.REFERRAL_REGION_ID = cas.treating_region_id                                
where                                   
P.PRACTICE_CODE = @PRACTICE_CODE                      
AND p.Deleted = 0                                  
AND isnull(ftp.Deleted,0)=0                                  
AND isnull(phr.deleted,0)=0              
AND isnull(cas.deleted,0)=0                                 
AND isnull(reg.deleted,0)=0                                     
AND (p.Email_Address is not null  and p.Email_Address !=' ')                                  
AND (p.Chart_Id is not null  and p.Chart_Id != ' ')                                        
AND (p.Gender is not null  and p.Gender != ' ')                      
AND  
cas.case_id  =   
(select MAX(case_id) from fox_tbl_case   with(nolock) where PATIENT_ACCOUNT = cas.Patient_Account and PRACTICE_CODE = @PRACTICE_CODE and DELETED =0  
)  
                               
 -- -- Invitation Status                                        
     AND                                          
     (                               
                                
  phr.INVITE_STATUS=@INVITATION_STATUS                        
     OR                        
  isnull(phr.INVITE_STATUS, 'notsent')=@INVITATION_STATUS                        
  OR                                    
  (                                       
    @INVITATION_STATUS = 'ALL'                                      
  )                                  
  )                                          
                                                 
  -- Patient Status                                        
     AND                                          
     (                                      
   ftp.Patient_Status=@PATIENT_STATUS                                    
   OR                                    
   (                                      
       @PATIENT_STATUS = 'ALL'                                      
   )                                          
     )                                     
                                     
                                                 
  AND                                        
  (@SEARCH_STRING = '' OR(                         
   CONVERT(VARCHAR(100),p.PATIENT_ACCOUNT) LIKE '%' + @SEARCH_STRING + '%'                                      
   OR P.Chart_Id LIKE '%' + @SEARCH_STRING + '%'                                            
   OR P.First_Name LIKE  '%' + @SEARCH_STRING + '%'                                                         
   OR P.Last_Name LIKE  '%' + @SEARCH_STRING + '%'                                          
   OR P.Gender LIKE  '%' + @SEARCH_STRING + '%'                                     
   OR P.Email_Address LIKE  '%' + @SEARCH_STRING + '%'     
   OR reg.REFERRAL_REGION_CODE LIKE  '%' + @SEARCH_STRING + '%'            
   OR reg.REFERRAL_REGION_NAME LIKE  '%' + @SEARCH_STRING + '%'                                 
   OR isnull(phr.INVITE_STATUS, 'not sent') LIKE  '%' + @SEARCH_STRING + '%'                          
   OR replace(phr.INVITE_STATUS,'Response Awaited', 'Pending') LIKE  '%' + @SEARCH_STRING + '%'                                    
 OR ftp.Patient_Status LIKE  '%' + @SEARCH_STRING + '%'                                           
    )     )                                        
                                        
                                           
                                          
 IF (@RECORD_PER_PAGE = 0)                                          
  BEGIN               
   SET @RECORD_PER_PAGE = @TOATL_PAGESUDM                                          
  END                                          
 ELSE                                          
  BEGIN                                          
   SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                                          
END                                          
                                  
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM                                          
                                          
  IF (@RECORD_PER_PAGE <> 0)                                          
  BEGIN                                        
 SET @TOATL_PAGESUDM = CEILING(IsNull(@TOATL_PAGESUDM,0) / IsNull(@RECORD_PER_PAGE,1))                    
                                          
 SELECT *,                                                       
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                                       
                @TOTAL_RECORDS AS TOTAL_RECORDS,                                        
    @Awaited AS Response_Awaited,                                        
    @Active AS Active                    
                                                          
         FROM                                                      
         (                                                      
             SELECT                                         
    phr.USER_PHONE,                        
 phr.USER_ID,                                    
    p.Patient_Account  AS PATIENT_ACCOUNT,                                    
    CONVERT(VARCHAR(30),p.Patient_Account) AS PATIENT_ACCOUNT_str,                                    
    p.Chart_Id  AS MRN,                                    
    p.First_Name AS FIRST_NAME,                                    
    p.Last_Name AS LAST_NAME,                                    
    p.Gender AS GENDER,                                    
    p.Email_Address  AS EMAIL,            
 reg.REFERRAL_REGION_CODE + ' - ' +reg.REFERRAL_REGION_NAME AS REGION,                                    
    ftp.Patient_Status  AS PATIENT_STATUS,                          
 --phr.INVITE_STATUS AS INVITATION_STATUS,                                   
 CASE                                                                               
 WHEN ISNULL(phr.INVITE_STATUS,'') =''                                                          
    THEN 'Not Sent'                          
WHEN phr.INVITE_STATUS='Response Awaited'                                                         
    THEN 'Pending'                                    
 ELSE                                   
 phr.INVITE_STATUS                                   
 END AS INVITATION_STATUS,                                   
    P.DELETED,                                    
    P.Created_Date,               
    CONVERT(BIT, CASE                                                                                 
    WHEN  phr.INVITE_STATUS='Active'                                                                                
     OR ftp.Patient_Status='Expired'                                                             
     THEN 1                                                                  
    ELSE 0                                                                                
    END) AS IS_ACTIVE ,                                        
   ROW_NUMBER() OVER ( ORDER BY P.CREATED_DATE DESC ) AS ACTIVEROW                                        
   FROM  Patient p   with(nolock)                                  
join FOX_TBL_PATIENT ftp  with(nolock) on ftp.Patient_Account=p.Patient_Account                                    
left join WS_FOX_TBL_PHR_USERS phr  with(nolock) on phr.Patient_Account=p.PATIENT_ACCOUNT                                   
LEFT join fox_tbl_case cas  with(nolock) on cas.Patient_Account = p.PATIENT_ACCOUNT            
LEFT join FOX_TBL_REFERRAL_REGION reg with(nolock) on reg.REFERRAL_REGION_ID = cas.treating_region_id                                
where                                   
P.PRACTICE_CODE = @PRACTICE_CODE                                  
AND p.Deleted = 0                                     
AND isnull(ftp.Deleted,0)=0                                  
AND isnull(phr.deleted,0)=0              
AND isnull(cas.deleted,0)=0                                 
AND isnull(reg.deleted,0)=0                                      
AND (p.Email_Address is not null  and p.Email_Address !=' ')                                  
AND (p.Chart_Id is not null  and p.Chart_Id != ' ')                                        
AND (p.Gender is not null  and p.Gender != ' ')                      
AND  
cas.case_id  =   
(select MAX(case_id) from fox_tbl_case  with(nolock) where PATIENT_ACCOUNT = cas.Patient_Account and PRACTICE_CODE = @PRACTICE_CODE and DELETED =0  
)  
    
                                    
   -- -- Invitation Status                                        
     AND                                          
     (                                      
  phr.INVITE_STATUS=@INVITATION_STATUS                         
   OR                        
  isnull(phr.INVITE_STATUS, 'notsent')=@INVITATION_STATUS                                   
  OR                                    
  (                                        
    @INVITATION_STATUS = 'ALL'                              
  )                                   
  )                                             
  -- Patient Status                                        
     AND                                          
     (                                      
   ftp.Patient_Status=@PATIENT_STATUS                                    
   OR                                    
   (                             
       @PATIENT_STATUS = 'ALL'                            
   )                                          
     )                                                
  AND                                        
  (@SEARCH_STRING = '' OR(                                        
   CONVERT(VARCHAR(100),p.PATIENT_ACCOUNT) LIKE '%' + @SEARCH_STRING + '%'                                      
   OR P.Chart_Id LIKE '%' + @SEARCH_STRING + '%'                                            
   OR P.First_Name LIKE  '%' + @SEARCH_STRING + '%'                                                  
   OR P.Last_Name LIKE  '%' + @SEARCH_STRING + '%'                                                           
   OR P.Gender LIKE  '%' + @SEARCH_STRING + '%'                                     
   OR P.Email_Address LIKE  '%' + @SEARCH_STRING + '%'            
   OR reg.REFERRAL_REGION_CODE LIKE  '%' + @SEARCH_STRING + '%'            
   OR reg.REFERRAL_REGION_NAME LIKE  '%' + @SEARCH_STRING + '%'                                    
   OR isnull(phr.INVITE_STATUS, 'not sent') LIKE  '%' + @SEARCH_STRING + '%'                           
   OR replace(phr.INVITE_STATUS,'Response Awaited', 'Pending') LIKE  '%' + @SEARCH_STRING + '%'                
   OR ftp.Patient_Status LIKE  '%' + @SEARCH_STRING + '%'                                           
   )     )              
                                        
                                        
         ) AS WS_FOX_TBL_PHR_USERS                                          
                                               
 ORDER BY                                          
  CASE WHEN @SORT_BY LIKE 'PATIENT_ACCOUNT' AND @SORT_ORDER = 'ASC' THEN PATIENT_ACCOUNT END ASC,                                          
  CASE WHEN @SORT_BY LIKE 'PATIENT_ACCOUNT' AND @SORT_ORDER = 'DESC' THEN PATIENT_ACCOUNT END DESC,                                      
  CASE WHEN @SORT_BY LIKE 'MRN' AND @SORT_ORDER = 'ASC' THEN MRN END ASC,                                          
  CASE WHEN @SORT_BY LIKE 'MRN' AND @SORT_ORDER = 'DESC' THEN MRN END DESC,                                        
  CASE WHEN @SORT_BY LIKE 'FIRST_NAME' AND @SORT_ORDER = 'ASC' THEN FIRST_NAME END ASC,                                          
  CASE WHEN @SORT_BY LIKE 'FIRST_NAME' AND @SORT_ORDER = 'DESC' THEN FIRST_NAME END DESC,                                        
  CASE WHEN @SORT_BY LIKE 'LAST_NAME' AND @SORT_ORDER = 'ASC' THEN LAST_NAME END ASC,                        
  CASE WHEN @SORT_BY LIKE 'LAST_NAME' AND @SORT_ORDER = 'DESC' THEN LAST_NAME END DESC ,                                        
  CASE WHEN @SORT_BY LIKE 'GENDER' AND @SORT_ORDER = 'ASC' THEN GENDER END ASC,                                        
  CASE WHEN @SORT_BY LIKE 'GENDER' AND @SORT_ORDER = 'DESC' THEN GENDER END DESC ,                                        
  CASE WHEN @SORT_BY LIKE 'EMAIL' AND @SORT_ORDER = 'ASC' THEN EMAIL END ASC,                                        
  CASE WHEN @SORT_BY LIKE 'EMAIL' AND @SORT_ORDER = 'DESC' THEN EMAIL END DESC ,                        
  CASE WHEN @SORT_BY LIKE 'INVITATION_STATUS' AND @SORT_ORDER = 'ASC' THEN INVITATION_STATUS END ASC,                                        
  CASE WHEN @SORT_BY LIKE 'INVITATION_STATUS' AND @SORT_ORDER = 'DESC' THEN INVITATION_STATUS END DESC ,            
  CASE WHEN @SORT_BY LIKE 'REGION' AND @SORT_ORDER = 'ASC' THEN REGION END ASC,                                        
  CASE WHEN @SORT_BY LIKE 'REGION' AND @SORT_ORDER = 'DESC' THEN REGION END DESC ,                                    
  CASE WHEN @SORT_BY LIKE 'PATIENT_STATUS' AND @SORT_ORDER = 'ASC' THEN PATIENT_STATUS END ASC,                                        
  CASE WHEN @SORT_BY LIKE 'PATIENT_STATUS' AND @SORT_ORDER = 'DESC' THEN PATIENT_STATUS END                                           
                                     
 DESC OFFSET @START_FROM ROWS                                          
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                                          
                                          
END                                       
END 

