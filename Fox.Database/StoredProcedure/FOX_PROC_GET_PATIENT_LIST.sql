IF (OBJECT_ID('FOX_PROC_GET_PATIENT_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_LIST  
GO                  
--exec [FOX_PROC_GET_PATIENT_LIST] '','', '', '', '', '','', '','', '',1011163, 1, 0, '', 'createdDate', 'ASC','', 0                      
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PATIENT_LIST] --'', '', '', '', '','', 'FEMALE', '','', '', '1011163', 1, 0, '', '', 'ASC', ''                          
 (                          
  @Patient_Account  AS VARCHAR(50)                          
 ,@First_Name AS VARCHAR(50)                          
 ,@Last_Name AS VARCHAR(50)                          
 ,@Middle_Name AS VARCHAR(50)                          
 ,@CHART_ID AS VARCHAR(100)                          
 ,@SSN AS VARCHAR(9)                          
 ,@Gender AS VARCHAR(15)                          
 ,@CreatedDate AS VARCHAR(15)                  
 ,@CreatedBy AS VARCHAR(50)                  
 ,@ModifiedBy AS VARCHAR(50)                  
 ,@PRACTICE_CODE BIGINT                          
 ,@CURRENT_PAGE INT                          
 ,@RECORD_PER_PAGE INT                          
 ,@SEARCH_TEXT VARCHAR(30)                          
 ,@SORT_BY VARCHAR(50)                          
 ,@SORT_ORDER VARCHAR(5)                          
 ,@DOB VARCHAR(50)                
 ,@Patient_Alias BIT                         
 )                          
AS                          
BEGIN          
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED      
 IF (@Patient_Account = '')                          
 BEGIN                          
  SET @Patient_Account = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @Patient_Account = @Patient_Account + '%'                          
 END                          
                          
 IF (@First_Name = '')                          
 BEGIN                          
  SET @First_Name = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @First_Name = @First_Name + '%'                          
 END                          
                          
 IF (@Last_Name = '')                          
 BEGIN                          
  SET @Last_Name = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @Last_Name = @Last_Name + '%'                          
 END                          
                          
 IF (@Middle_Name = '')                          
 BEGIN                          
  SET @Middle_Name = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @Middle_Name = @Middle_Name + '%'                          
 END                          
                          
 IF (@CHART_ID = '')                          
 BEGIN                          
  SET @CHART_ID = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @CHART_ID = @CHART_ID + '%'                          
 END                          
                          
 IF (@SSN = '')                          
 BEGIN                          
  SET @SSN = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @SSN = @SSN + '%'                          
 END                          
                          
 IF (@Gender = '')                          
 BEGIN                          
  SET @Gender = NULL                          
 END                          
 ELSE                          
 --BEGIN                          
 -- SET @Gender = @Gender                          
 --END                          
                          
 IF (@CreatedDate = '')                          
 BEGIN                          
  SET @CreatedDate = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @CreatedDate = @CreatedDate + '%'                          
END                          
                           
 IF (@CreatedBy = '')                          
 BEGIN                          
  SET @CreatedBy = NULL                          
 END                   
 ELSE                          
 BEGIN                          
  SET @CreatedBy = @CreatedBy + '%'                          
 END                          
                          
 IF (@ModifiedBy = '')                          
 BEGIN         
  SET @ModifiedBy = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @ModifiedBy = @ModifiedBy + '%'                          
 END                          
                 
IF (@RECORD_PER_PAGE = 0)                          
 BEGIN                          
  SELECT @RECORD_PER_PAGE = COUNT(*)                          
  FROM PATIENT                          
 END                          
 ELSE                          
 BEGIN             
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                          
 END                          
                          
 IF (@DOB = '')                    
 BEGIN                          
  SET @DOB = NULL                          
 END                          
 ELSE                          
 BEGIN                          
  SET @DOB = @DOB + '%'                          
 END                          
                          
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                          
                          
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                          
 DECLARE @TOATL_PAGESUDM FLOAT                          
                          
 SELECT @TOATL_PAGESUDM = COUNT(*)                
 FROM Patient p  WITH (NOLOCK)                 
 LEFT JOIN  FOX_TBL_PATIENT AS FTP WITH (forceseek,NOLOCK) ON FTP.Patient_Account=p.Patient_Account               
           LEFT JOIN FOX_TBL_PATIENT_INSURANCE AS PI WITH (NOLOCK) ON PI.Patient_Insurance_Id =                  
             (                  
                 SELECT TOP 1 Patient_Insurance_Id                  
                 FROM FOX_TBL_PATIENT_INSURANCE ti WITH (forceseek,NOLOCK)                  
                 WHERE ti.Patient_Account = P.Patient_Account                  
                      AND ti.Pri_Sec_Oth_Type = 'P'                  
                       AND ISNULL(ti.DELETED, 0) = 0                  
                       AND (ti.FOX_INSURANCE_STATUS = 'C')                  
    ORDER BY Modified_Date DESC                  
             )                  
                  LEFT JOIN FOX_TBL_INSURANCE fi WITH (NOLOCK) ON fi.FOX_TBL_INSURANCE_ID = PI.FOX_TBL_INSURANCE_ID                  
                                                    AND fi.PRACTICE_CODE = @PRACTICE_CODE                  
                  LEFT JOIN FOX_TBL_FINANCIAL_CLASS FC WITH (NOLOCK) ON PI.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID                  
                                                          AND ISNULL(FC.DELETED, 0) = 0                  
                                                          AND FC.PRACTICE_CODE = @PRACTICE_CODE                  
   LEFT JOIN fox_tbl_application_user u ON u.USER_NAME = p.Created_By                
   LEFT JOIN fox_tbl_application_user us ON us.USER_NAME = p.Modified_By                
   LEFT JOIN FOX_TBL_PATIENT_ALIAS AS pa ON pa.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT                
              AND ISNULL(pa.DELETED, 0) = 0                
              AND @Patient_Alias = cast(1 as bit)              
LEFT JOIN FOX_TBL_FINANCIAL_CLASS FPC WITH (NOLOCK) ON FTP.FINANCIAL_CLASS_ID = FPC.FINANCIAL_CLASS_ID                  
       AND ISNULL(FPC.DELETED, 0) = 0                  
                                    AND FPC.PRACTICE_CODE = @PRACTICE_CODE                  
 WHERE(p.Practice_Code = @PRACTICE_CODE)                
 AND (          
   (p.Chart_Id is not NULL AND p.Chart_Id <> '')          
   OR          
   ( SELECT COUNT(FOX_INTERFACE_SYNCH_ID) FROM FOX_TBL_INTERFACE_SYNCH WITH (nolock,forceseek)         
     WHERE PATIENT_ACCOUNT = p.PATIENT_ACCOUNT          
     AND isnull(IS_SYNCED,0) = 1          
     AND isnull(DELETED,0) = 0)   > 0           
              
  )          
   AND (p.First_Name LIKE '%'+@SEARCH_TEXT+'%'                
     OR p.Last_Name LIKE '%'+@SEARCH_TEXT+'%'                
     OR p.MIDDLE_NAME LIKE '%'+@SEARCH_TEXT+'%'                
     OR ISNULL(p.Chart_Id, '') LIKE '%'+@SEARCH_TEXT+'%'                
     OR p.SSN LIKE '%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Date_Of_Birth, 101) LIKE+'%'+@SEARCH_TEXT+'%'                
     OR p.Gender LIKE '%'+@SEARCH_TEXT+'%'                
     OR (u.LAST_NAME+', '+u.FIRST_NAME) LIKE '%'+@SEARCH_TEXT+'%'                
     OR (us.LAST_NAME+', '+us.FIRST_NAME) LIKE '%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Created_Date, 101) LIKE+'%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Created_Date, 100) LIKE+'%'+'%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Modified_Date, 101) LIKE+'%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Modified_Date, 100) LIKE+'%'+'%'+@SEARCH_TEXT+'%'                
     OR p.Patient_Account LIKE '%'+@SEARCH_TEXT+'%'                
     OR pa.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                
     OR pa.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                
     OR pa.MIDDLE_INITIALS LIKE '%'+@SEARCH_TEXT+'%')                
   AND (@Patient_Account IS NULL                
     OR p.Patient_Account LIKE '%'+@Patient_Account+'%')                
   AND (@FIRST_NAME IS NULL                
  OR p.First_Name LIKE '%'+@FIRST_NAME+'%'                
     OR pa.FIRST_NAME LIKE '%'+@FIRST_NAME+'%')                
   AND (@LAST_NAME IS NULL                
     OR p.Last_Name LIKE '%'+@LAST_NAME+'%'                
     OR pa.LAST_NAME LIKE '%'+@LAST_NAME+'%')                
   AND (@Middle_Name IS NULL                
     OR p.MIDDLE_NAME LIKE '%'+@Middle_Name+'%'                
     OR pa.MIDDLE_INITIALS LIKE '%'+@Middle_Name+'%')                
   AND (@CHART_ID IS NULL                
     OR ISNULL(p.Chart_Id, '') LIKE '%'+@CHART_ID+'%')                
   AND (@SSN IS NULL                
     OR RIGHT(ISNULL(p.SSN, ''), 4) LIKE '%'+@SSN+'%')                
   AND (@Gender IS NULL                
     OR p.Gender = @Gender)                
   AND (@CreatedBy IS NULL                
     OR p.Created_By LIKE '%'+@CreatedBy+'%')                
   AND (@CreatedDate IS NULL                
     OR CONVERT(VARCHAR(50), ISNULL(p.Created_Date, 0), 101) LIKE '%'+@CreatedDate+'%')                
   AND (@ModifiedBy IS NULL                
     OR p.Modified_By LIKE '%'+@ModifiedBy+'%')                
   AND (@DOB IS NULL                
     OR CONVERT(VARCHAR(50), ISNULL(p.Date_Of_Birth, 0), 101) LIKE '%'+@DOB+'%')                
   AND (ISNULL(p.Deleted, 0) != 1)                          
                          
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM                          
                          
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)                          
                          
 SELECT *                          
  ,@TOATL_PAGESUDM AS TOTAL_RECORD_PAGES                          
  ,@TOTAL_RECORDS TOTAL_RECORDS                          
 FROM (                          
 SELECT p.Patient_Account,                 
     p.First_Name,                 
     p.Last_Name,                 
     p.MIDDLE_NAME,       
     ISNULL(p.Chart_Id, '') AS Chart_Id,                 
     RTRIM(LTRIM(p.SSN)) AS SSN,                 
     p.Date_Of_Birth,              
     p.Gender,                 
     p.Email_Address,                 
     P.Patient_GlobalId,                 
     p.Home_Phone,                 
     p.Business_Phone,                 
     p.cell_phone,        
  CASE                         
     WHEN ISNULL(u.LAST_NAME, '') = '' THEN ISNULL(u.FIRST_NAME, '')                        
     ELSE ISNULL(u.LAST_NAME, '') + ', ' + ISNULL(u.FIRST_NAME, '')                        
     END AS Created_By,          
     --u.LAST_NAME+', '+u.FIRST_NAME Created_By,                 
     p.Created_Date,        
  CASE                         
     WHEN ISNULL(us.LAST_NAME, '') = '' THEN ISNULL(us.FIRST_NAME, '')                        
     ELSE ISNULL(us.LAST_NAME, '') + ', ' + ISNULL(us.FIRST_NAME, '')                        
     END AS Modified_By,                  
     --us.LAST_NAME+', '+us.FIRST_NAME Modified_By,                 
     p.Modified_Date,                 
     p.DELETED,        
            fi.INSURANCE_NAME AS PrimaryInsuranceName,                   
            PI.Patient_Insurance_Id AS PrimaryInsuranceID,                   
            FC.CODE AS FINANCIAL_CLASS,                
     ROW_NUMBER() OVER(ORDER BY p.CREATED_DATE ASC) AS ACTIVEROW,                
     CASE                
      WHEN @Patient_Alias = cast(1 as bit)                 
     AND pa.PATIENT_ALIAS_ID IS NOT NULL                
      THEN 1                
      ELSE 0                
     END AS Is_Patient_Alias,                 
     pa.PATIENT_ALIAS_ID,                 
     pa.ALIAS_TRACKING_NUMBER,                 
     pa.RT_ALIAS_TRACKING,                 
     pa.FIRST_NAME AS FIRST_NAME_ALIAS,                 
     pa.LAST_NAME AS LAST_NAME_ALIAS,                 
     pa.MIDDLE_INITIALS AS MIDDLE_INITIALS_ALIAS,        
  FTP.Patient_Status AS Patient_status,        
  FPC.NAME AS PATIENT_FINANCIAL_CLASS        
 FROM Patient p  WITH (NOLOCK)      
  LEFT JOIN  FOX_TBL_PATIENT AS FTP WITH (NOLOCK) ON FTP.Patient_Account=p.Patient_Account               
           LEFT JOIN FOX_TBL_PATIENT_INSURANCE AS PI WITH (forceseek,NOLOCK) ON PI.Patient_Insurance_Id =                  
             (                  
                 SELECT TOP 1 Patient_Insurance_Id                  
                 FROM FOX_TBL_PATIENT_INSURANCE ti WITH (forceseek,NOLOCK)                
                 WHERE ti.Patient_Account = P.Patient_Account                  
                       AND ti.Pri_Sec_Oth_Type = 'P'                  
                       AND ISNULL(ti.DELETED, 0) = 0                  
                       AND (ti.FOX_INSURANCE_STATUS = 'C')                  
    ORDER BY Modified_Date DESC                  
             )                  
   LEFT JOIN FOX_TBL_INSURANCE fi WITH (NOLOCK) ON fi.FOX_TBL_INSURANCE_ID = PI.FOX_TBL_INSURANCE_ID                  
                                                    AND fi.PRACTICE_CODE = @PRACTICE_CODE                  
                  LEFT JOIN FOX_TBL_FINANCIAL_CLASS FC WITH (NOLOCK) ON PI.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID                  
                                                          AND ISNULL(FC.DELETED, 0) = 0                  
                                                          AND FC.PRACTICE_CODE = @PRACTICE_CODE                  
   LEFT JOIN fox_tbl_application_user u WITH (NOLOCK) ON u.USER_NAME = p.Created_By                
   LEFT JOIN fox_tbl_application_user us WITH (NOLOCK) ON us.USER_NAME = p.Modified_By                
   LEFT JOIN FOX_TBL_PATIENT_ALIAS AS pa WITH (NOLOCK) ON pa.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT                
              AND ISNULL(pa.DELETED, 0) = 0                
              AND @Patient_Alias = cast(1 as bit)                
  LEFT JOIN FOX_TBL_FINANCIAL_CLASS FPC WITH (NOLOCK) ON FTP.FINANCIAL_CLASS_ID = FPC.FINANCIAL_CLASS_ID                  
                                            AND ISNULL(FPC.DELETED, 0) = 0                  
                                            AND FPC.PRACTICE_CODE = @PRACTICE_CODE              
 WHERE(p.Practice_Code = @PRACTICE_CODE)                
 AND (          
   (p.Chart_Id is not NULL AND p.Chart_Id <> '')          
   OR          
   ( SELECT COUNT(FOX_INTERFACE_SYNCH_ID) FROM FOX_TBL_INTERFACE_SYNCH WITH (forceseek,NOLOCK)          
     WHERE PATIENT_ACCOUNT = p.PATIENT_ACCOUNT          
     AND isnull(IS_SYNCED,0) = 1          
     AND isnull(DELETED,0) = 0)   > 0           
              
  )          
   AND (p.First_Name LIKE '%'+@SEARCH_TEXT+'%'                
     OR p.Last_Name LIKE '%'+@SEARCH_TEXT+'%'                
     OR p.MIDDLE_NAME LIKE '%'+@SEARCH_TEXT+'%'                
     OR ISNULL(p.Chart_Id, '') LIKE '%'+@SEARCH_TEXT+'%'                
     OR p.SSN LIKE '%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Date_Of_Birth, 101) LIKE+'%'+@SEARCH_TEXT+'%'                
     OR p.Gender LIKE '%'+@SEARCH_TEXT+'%'                
     OR (u.LAST_NAME+', '+u.FIRST_NAME) LIKE '%'+@SEARCH_TEXT+'%'                
     OR (us.LAST_NAME+', '+us.FIRST_NAME) LIKE '%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Created_Date, 101) LIKE+'%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Created_Date, 100) LIKE+'%'+'%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Modified_Date, 101) LIKE+'%'+@SEARCH_TEXT+'%'                
     OR CONVERT(VARCHAR, p.Modified_Date, 100) LIKE+'%'+'%'+@SEARCH_TEXT+'%'                
     OR p.Patient_Account LIKE '%'+@SEARCH_TEXT+'%'                
     OR pa.FIRST_NAME LIKE '%'+@SEARCH_TEXT+'%'                
     OR pa.LAST_NAME LIKE '%'+@SEARCH_TEXT+'%'                
     OR pa.MIDDLE_INITIALS LIKE '%'+@SEARCH_TEXT+'%')                
   AND (@Patient_Account IS NULL                
     OR p.Patient_Account LIKE '%'+@Patient_Account+'%')                
   AND (@FIRST_NAME IS NULL                
     OR p.First_Name LIKE '%'+@FIRST_NAME+'%'                
     OR pa.FIRST_NAME LIKE '%'+@FIRST_NAME+'%')                
   AND (@LAST_NAME IS NULL                
     OR p.Last_Name LIKE '%'+@LAST_NAME+'%'                
     OR pa.LAST_NAME LIKE '%'+@LAST_NAME+'%')                
   AND (@Middle_Name IS NULL                
     OR p.MIDDLE_NAME LIKE '%'+@Middle_Name+'%'                
     OR pa.MIDDLE_INITIALS LIKE '%'+@Middle_Name+'%')                
   AND (@CHART_ID IS NULL                
     OR ISNULL(p.Chart_Id, '') LIKE '%'+@CHART_ID+'%')                
   AND (@SSN IS NULL                
     OR RIGHT(ISNULL(p.SSN, ''), 4) LIKE '%'+@SSN+'%')                
   AND (@Gender IS NULL                
     OR p.Gender = @Gender)                
   AND (@CreatedBy IS NULL                
     OR p.Created_By LIKE '%'+@CreatedBy+'%')                
   AND (@CreatedDate IS NULL                
     OR CONVERT(VARCHAR(50), ISNULL(p.Created_Date, 0), 101) LIKE '%'+@CreatedDate+'%')                
   AND (@ModifiedBy IS NULL                
     OR p.Modified_By LIKE '%'+@ModifiedBy+'%')                
   AND (@DOB IS NULL                
     OR CONVERT(VARCHAR(50), ISNULL(p.Date_Of_Birth, 0), 101) LIKE '%'+@DOB+'%')                
   AND (ISNULL(p.Deleted, 0) != 1)                         
  ) AS PATIENT                          
 ORDER BY CASE                           
   WHEN @SORT_BY = 'FirstName'                          
    AND @SORT_ORDER = 'ASC'              
    THEN First_Name                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'FirstName'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN First_Name                          
   END DESC                
  ,CASE                           
   WHEN @SORT_BY = 'LastName'                          
    AND @SORT_ORDER = 'ASC'                          
    THEN Last_Name                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'LastName'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN Last_Name                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'MiddleName'                          
    AND @SORT_ORDER = 'ASC'                          
    THEN MIDDLE_NAME                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'MiddleName'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN MIDDLE_NAME                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'ChartId'                          
    AND @SORT_ORDER = 'ASC'                          
    THEN Chart_Id                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'ChartId'                          
    AND @SORT_ORDER = 'DESC'            
    THEN Chart_Id                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'SSN'                          
    AND @SORT_ORDER = 'ASC'                          
    THEN SSN                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'SSN'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN SSN                          
   END DESC                          
  ,CASE           
   WHEN @SORT_BY = 'DOB'                          
    AND @SORT_ORDER = 'ASC'             
    THEN Date_Of_Birth                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'DOB'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN Date_Of_Birth                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'Gender'                          
    AND @SORT_ORDER = 'ASC'                          
    THEN Gender                     
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'Gender'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN Gender                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'CreatedBy'                    
    AND @SORT_ORDER = 'ASC'                          
    THEN Created_By                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'CreatedBy'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN Created_By                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'UpdatedBy'                          
    AND @SORT_ORDER = 'ASC'                          
    THEN Modified_By                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'UpdatedBy'                         
    AND @SORT_ORDER = 'DESC'                          
    THEN Modified_By                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'createdDate'                          
    AND @SORT_ORDER = 'ASC'                        THEN Created_Date                          
   END ASC                          
  ,CASE                     
   WHEN @SORT_BY = 'createdDate' 
    AND @SORT_ORDER = 'DESC'                    
    THEN Created_Date                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'modifiedDate'                          
    AND @SORT_ORDER = 'ASC'                          
    THEN Modified_Date                          
   END ASC                          
  ,CASE                     
   WHEN @SORT_BY = 'modifiedDate'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN Modified_Date                          
   END DESC                          
  ,CASE                                      
   WHEN @SORT_BY = 'FirstNameAlias'                          
    AND @SORT_ORDER = 'ASC'                   
    THEN FIRST_NAME_ALIAS                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'FirstNameAlias'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN FIRST_NAME_ALIAS                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'LastNameAlias'                          
    AND @SORT_ORDER = 'ASC'                       
    THEN LAST_NAME_ALIAS                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'LastNameAlias'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN LAST_NAME_ALIAS                          
   END DESC                          
  ,CASE                           
   WHEN @SORT_BY = 'MiddleNameAlias'                          
    AND @SORT_ORDER = 'ASC'                          
    THEN MIDDLE_INITIALS_ALIAS                          
   END ASC                          
  ,CASE                           
   WHEN @SORT_BY = 'MiddleNameAlias'                          
    AND @SORT_ORDER = 'DESC'                          
    THEN MIDDLE_INITIALS_ALIAS                          
   END DESC                   
  ,CASE                           
   WHEN @SORT_BY = ''                          
    AND @SORT_ORDER = ''      
    THEN CREATED_DATE                          
   END DESC OFFSET @START_FROM ROWS                          
               
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                          
END 
