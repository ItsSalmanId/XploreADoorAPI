IF (OBJECT_ID('FOX_PROC_GET_ASSOCIATED_REAGION_DATA ') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ASSOCIATED_REAGION_DATA 
GO 
-- =============================================                                
-- Author:  <Author,Usama Bin Ahmed>                                
-- Create date: <Create Date,05/09/2020>                                
-- Description: <get associated reagion data >                         
--EXEC FOX_PROC_GET_ASSOCIATED_REAGION_DATA 1011163 ,'Tricky_5481261','Tricky@mtbc.com', 'Regional Director'                              
--EXEC FOX_PROC_GET_ASSOCIATED_REAGION_DATA 1011163 ,'Mubeen_544791','muhammadmubeen1@mtbc.com', 'Account Manager'               
--EXEC FOX_PROC_GET_ASSOCIATED_REAGION_DATA_usama 1011163 ,'Testing_5481889','Test32@test.com', ''                       
                      
                             
CREATE PROCEDURE  [dbo].[FOX_PROC_GET_ASSOCIATED_REAGION_DATA]                      
                      
--104 Regional Director 'Tricky_5481261' 'Tricky@mtbc.com'                      
--105 Account Manager                      
--54810141 Clinician                      
                      
@PRACTICE_CODE BIGINT,                      
--@RT_USER_ID VARCHAR(50),                       
@USER_NAME  VARCHAR(50),                            
@EMAIL VARCHAR (50),                      
@ROLE  VARCHAR (50)                      
                      
AS                      
BEGIN                      
                      
IF(@ROLE = 'Regional Director')                      
BEGIN                      
                      
Select                    
                     
AU.USER_ID,                      
AU.USER_NAME,                      
LOWER(AU.EMAIL) AS USER_EMAIL,                      
LOWER(PRV.EMAIL) AS CLINICIAN_EMAIL,                   
AU.RT_USER_ID,                      
PRV.FOX_PROVIDER_CODE,                      
PRV.FIRST_NAME,                      
PRV.LAST_NAME,                        
RG.REGIONAL_DIRECTOR_ID,                       
RG.REFERRAL_REGION_ID,                        
RG.REFERRAL_REGION_CODE,                      
RG.REFERRAL_REGION_NAME,                    
@ROLE as ROLE_NAME                      
                     
FROM FOX_TBL_APPLICATION_USER as AU                        
                      
JOIN fox_tbl_referral_region AS RG ON (AU.USER_ID = RG.REGIONAL_DIRECTOR_ID AND RG.PRACTICE_CODE = @PRACTICE_CODE)                    
LEFT JOIN FOX_TBL_PROVIDER AS PRV ON (AU.EMAIL = PRV.EMAIL OR AU.RT_USER_ID = PRV.FOX_PROVIDER_CODE) AND PRV.PRACTICE_CODE = @PRACTICE_CODE                       
WHERE  AU.USER_NAME = @USER_NAME                      
AND AU.EMAIL = @EMAIL                    
AND AU.PRACTICE_CODE = @PRACTICE_CODE                    
AND ISNULL(AU.DELETED, 0) = 0                   
AND ISNULL(PRV.DELETED, 0) = 0         
AND ISNULL(RG.DELETED, 0) = 0         
        
UNION                    
                    
Select                    
--DISTINCT                     
AU.USER_ID,                      
AU.USER_NAME,                      
LOWER(AU.EMAIL) AS USER_EMAIL,                      
lOWER(PRV.EMAIL) AS CLINICIAN_EMAIL,                      
AU.RT_USER_ID,                      
PRV.FOX_PROVIDER_CODE,                      
PRV.FIRST_NAME,                      
PRV.LAST_NAME,                        
RG.REGIONAL_DIRECTOR_ID,                       
RG.REFERRAL_REGION_ID,                        
RG.REFERRAL_REGION_CODE,                      
RG.REFERRAL_REGION_NAME,                    
@ROLE as ROLE_NAME                      
                     
FROM FOX_TBL_APPLICATION_USER as AU                     
                       
JOIN FOX_TBL_DASHBOARD_ACCESS AS DB                    
  ON AU.USER_NAME = DB.USER_NAME  
  --and au.ROLE_ID = db.SHOW_AS_ROLE                    
  JOIN fox_tbl_referral_region AS RG                    
  ON RG.REFERRAL_REGION_ID = DB.REFERRAL_REGION_ID  
  AND RG.PRACTICE_CODE = @PRACTICE_CODE  
  --and au.ROLE_ID = db.SHOW_AS_ROLE)   
                    
--ON (AU.USER_NAME = DB.USER_NAME                    
--AND AU.ROLE_ID = DB.SHOW_AS_ROLE)                    
--LEFT JOIN  fox_tbl_referral_region AS RG ON (AU.USER_ID = RG.REGIONAL_DIRECTOR_ID)                    
                    
LEFT JOIN FOX_TBL_PROVIDER AS PRV ON (AU.EMAIL = PRV.EMAIL OR AU.RT_USER_ID = PRV.FOX_PROVIDER_CODE) AND PRV.PRACTICE_CODE = @PRACTICE_CODE                       
WHERE  AU.USER_NAME = @USER_NAME            
AND AU.EMAIL = @EMAIL                    
AND AU.PRACTICE_CODE = @PRACTICE_CODE                    
AND ISNULL(AU.DELETED, 0) = 0                   
AND ISNULL(DB.DELETED, 0) = 0        
AND ISNULL(PRV.DELETED, 0) = 0        
AND ISNULL(RG.DELETED, 0) = 0        
        
END                      
ELSE IF(@ROLE = 'Senior Regional Director')                    
BEGIN                    
Select                    
DISTINCT                     
AU.USER_ID,                       
AU.USER_NAME,                        
RG.SENIOR_REGIONAL_DIRECTOR_ID,                      
RG.REFERRAL_REGION_ID,                       
RG.REFERRAL_REGION_CODE,                      
RG.REFERRAL_REGION_NAME,                      
@ROLE as ROLE_NAME                      
                     
FROM FOX_TBL_APPLICATION_USER as AU                        
                      
JOIN fox_tbl_referral_region AS RG ON (AU.USER_ID = RG.SENIOR_REGIONAL_DIRECTOR_ID)                    
WHERE  AU.USER_NAME = @USER_NAME                      
AND AU.EMAIL = @EMAIL                    
AND AU.PRACTICE_CODE =@PRACTICE_CODE                    
AND ISNULL(AU.DELETED, 0) = 0                      
AND ISNULL(RG.DELETED, 0) = 0                     
UNION                    
                    
Select                    
DISTINCT                     
AU.USER_ID,                       
AU.USER_NAME,                        
RG.SENIOR_REGIONAL_DIRECTOR_ID,                      
RG.REFERRAL_REGION_ID,                       
RG.REFERRAL_REGION_CODE,                      
RG.REFERRAL_REGION_NAME,                     
@ROLE as ROLE_NAME                    
 FROM FOX_TBL_APPLICATION_USER as AU                          
 JOIN FOX_TBL_DASHBOARD_ACCESS AS DB                     
ON AU.USER_NAME = DB.USER_NAME                    
--AND AU.ROLE_ID = DB.SHOW_AS_ROLE)                    
JOIN fox_tbl_referral_region AS RG                    
ON DB.REFERRAL_REGION_ID = RG.REFERRAL_REGION_ID  
AND RG.PRACTICE_CODE =@PRACTICE_CODE  
                      
WHERE  AU.USER_NAME = @USER_NAME                      
AND AU.EMAIL = @EMAIL                    
AND AU.PRACTICE_CODE =@PRACTICE_CODE                    
AND ISNULL(AU.DELETED, 0) = 0                   
AND ISNULL(DB.DELETED, 0) = 0        
AND ISNULL(RG.DELETED, 0) = 0        
END                         
ELSE IF (@ROLE = 'Clinician')                      
BEGIN                      
                      
SELECT                     
DISTINCT                        
  AU.USER_ID,                      
  AU.USER_NAME,                      
   LOWER(AU.EMAIL) AS USER_EMAIL,                      
   LOWER(PRV.EMAIL) AS CLINICIAN_EMAIL,                      
  AU.RT_USER_ID,                      
  PRV.FOX_PROVIDER_CODE,                      
  PRV.FIRST_NAME,                      
  PRV.LAST_NAME,                      
  RG.REFERRAL_REGION_CODE,                      
  RG.REFERRAL_REGION_NAME,                       
  @ROLE AS ROLE_NAME                      
                      
  FROM FOX_TBL_APPLICATION_USER AS AU                      
  JOIN FOX_TBL_PROVIDER AS PRV ON (AU.EMAIL = PRV.EMAIL OR AU.RT_USER_ID = PRV.FOX_PROVIDER_CODE) AND PRV.PRACTICE_CODE = @PRACTICE_CODE                      
  left JOIN fox_tbl_referral_region AS RG ON PRV.REFERRAL_REGION_ID = RG.REFERRAL_REGION_ID AND RG.PRACTICE_CODE = @PRACTICE_CODE                      
  WHERE (AU.USER_NAME = @USER_NAME) AND au.PRACTICE_CODE= @PRACTICE_CODE                   
  AND ISNULL(AU.DELETED, 0) = 0                   
  AND ISNULL(PRV.DELETED, 0) = 0        
  AND ISNULL(RG.DELETED, 0) = 0        
                     
END                      
                      
ELSE IF (@ROLE = 'Account Manager')                      
BEGIN                      
        
SELECT                    
                    
DISTINCT                         
AU.USER_ID,                       
AU.USER_NAME,                      
RG.ACCOUNT_MANAGER_ID,                      
RG.REFERRAL_REGION_CODE,                      
RG.REFERRAL_REGION_NAME,                    
@ROLE AS ROLE_NAME                      
                      
FROM FOX_TBL_APPLICATION_USER AS AU                      
JOIN fox_tbl_referral_region AS RG ON AU.USER_ID = RG.ACCOUNT_MANAGER_ID  
AND RG.PRACTICE_CODE = @PRACTICE_CODE  
WHERE AU.USER_NAME = @USER_NAME                       
AND AU.EMAIL = @EMAIL                     
AND AU.PRACTICE_CODE = @PRACTICE_CODE                                 
AND ISNULL(AU.DELETED, 0) = 0                  
AND ISNULL(RG.DELETED, 0) = 0                        
                    
                    
UNION                    
                     
SELECT                    
                    
DISTINCT                         
AU.USER_ID,                       
AU.USER_NAME,                 
RG.ACCOUNT_MANAGER_ID,                      
RG.REFERRAL_REGION_CODE,                      
RG.REFERRAL_REGION_NAME,                    
@ROLE AS ROLE_NAME                      
                      
FROM FOX_TBL_APPLICATION_USER AS AU                      
JOIN FOX_TBL_DASHBOARD_ACCESS AS DB                     
ON AU.USER_NAME = DB.USER_NAME                    
--AND AU.ROLE_ID = DB.SHOW_AS_ROLE)                    
JOIN fox_tbl_referral_region AS RG ON DB.REFERRAL_REGION_ID = RG.REFERRAL_REGION_ID AND RG.PRACTICE_CODE = @PRACTICE_CODE                   
                    
WHERE AU.USER_NAME = @USER_NAME                       
AND AU.EMAIL = @EMAIL                       
AND AU.PRACTICE_CODE = @PRACTICE_CODE                      
AND AU.EMAIL = @EMAIL                    
AND ISNULL(AU.DELETED, 0) = 0                    
AND ISNULL(DB.DELETED, 0) = 0                    
AND ISNULL(RG.DELETED, 0) = 0                    
--GROUP BY RG.REFERRAL_REGION_NAME                     
END                      
              ELSE                      
BEGIN                      
SELECT                    
DISTINCT                        
AU.USER_ID,                       
AU.USER_NAME,                      
DB.USER_NAME AS DASHBOARD_USER,              
DB.SHOW_AS_ROLE,                      
--RL.ROLE_ID,                      
--RL.ROLE_NAME AS ROLE_NAME_IN_DASHBOARD,                      
RG.REFERRAL_REGION_ID,              
RG.REFERRAL_REGION_CODE,                       
RG.REFERRAL_REGION_NAME,                    
RG2.REFERRAL_REGION_NAME AS DSHBOARD_REGION,                      
@ROLE AS ROLE_NAME                      
               
FROM FOX_TBL_APPLICATION_USER AS AU                      
JOIN FOX_TBL_DASHBOARD_ACCESS AS DB                     
ON AU.USER_NAME = DB.USER_NAME                    
--AND AU.ROLE_ID = DB.SHOW_AS_ROLE)                    
JOIN fox_tbl_referral_region AS RG2                    
ON DB.REFERRAL_REGION_ID = RG2.REFERRAL_REGION_ID  
AND RG2.PRACTICE_CODE = @PRACTICE_CODE  
--JOIN FOX_TBL_ROLE AS RL ON DB.SHOW_AS_ROLE = RL.ROLE_ID                      
left JOIN fox_tbl_referral_region AS RG ON DB.REFERRAL_REGION_ID = RG.REFERRAL_REGION_ID  
AND RG.PRACTICE_CODE = @PRACTICE_CODE  
WHERE                      
AU.USER_NAME  =@USER_NAME                       
AND AU.EMAIL = @EMAIL   
AND AU.PRACTICE_CODE = @PRACTICE_CODE  
AND ISNULL(AU.DELETED, 0) = 0                   
AND ISNULL(DB.DELETED, 0) = 0                   
AND ISNULL(RG.DELETED, 0) = 0        
--AND ISNULL(RL.DELETED, 0) = 0        
        
END                      
                      
END 
