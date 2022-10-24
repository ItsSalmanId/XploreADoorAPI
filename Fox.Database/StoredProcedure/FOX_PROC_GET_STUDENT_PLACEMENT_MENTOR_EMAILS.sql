IF (OBJECT_ID('FOX_PROC_GET_STUDENT_PLACEMENT_MENTOR_EMAILS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_STUDENT_PLACEMENT_MENTOR_EMAILS  
GO 
-- =============================================  
-- AUTHOR: MUHAMMAD ARSLAN TUFAIL  
-- CREATE DATE:     07/26/2022  
-- DESCRIPTION: THIS PROCEDURE IS TRIGGER TO GET EMAIL TEMPLATE DETAILS FOR STUDENT MENTOR   
-- =============================================  
  
CREATE PROCEDURE [FOX_PROC_GET_STUDENT_PLACEMENT_MENTOR_EMAILS]    
@PRACTICE_CODE BIGINT    
AS    
BEGIN    
 SELECT CA.FIRST_NAME AS FirstName, CA.LAST_NAME AS LastName, CA.WORK_EMAIL AS WorkEmail, CA.CATEGORY_DESCRIPTION AS CategoryDescription,     
 CA.UNIVERSITY_DESCRIPTION AS UniversityDescription, CA.EFFECTIVE_DATE AS EffectiveDate, CA.EXPIRATION_DATE AS ExpirationDate    
 FROM FOX_TBL_MTBC_CREDENTIALS_AUTOMATION AS CA    
 INNER JOIN FOX_TBL_HR_AUTOEMAILS_CONFIGURE_ATTACHMENTS AS AA ON CA.CATEGORY_DESCRIPTION = AA.[NAME]    
 WHERE    
 (CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+630)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+600)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+570)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+540)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+510)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+480)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+450)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+420)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+390)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+360)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+330)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+300)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+270)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+240)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+210)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+190)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+150)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+120)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+90)     
 OR CONVERT(DATE,EFFECTIVE_DATE)=CONVERT(DATE, GETDATE()+60))     
 AND AA.IS_ENABLED = 1 AND ISNULL(CA.DELETED, 0) = 0 AND ISNULL(AA.DELETED, 0) = 0 AND CA.PRACTICE_CODE = @PRACTICE_CODE    
 AND CA.CATEGORY_DESCRIPTION LIKE '%Student Placement Mentor%' AND CA.WORK_EMAIL <> ''    
END 