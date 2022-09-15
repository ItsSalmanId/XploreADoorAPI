IF (OBJECT_ID('FOX_GET_SMART_PATIENTS') IS NOT NULL ) DROP PROCEDURE FOX_GET_SMART_PATIENTS  
GO 
CREATE PROCEDURE [dbo].[FOX_GET_SMART_PATIENTS] --'1011163', 'test'      
(@PRACTICE_CODE BIGINT,   
 @SEARCHVALUE   VARCHAR(50)  
)  
AS  
     BEGIN  
   IF CHARINDEX(',', @SEARCHVALUE) > 0  
             BEGIN  
                 SET @SEARCHVALUE = Replace(@SEARCHVALUE, ',', '');  
             END;  
         IF(@SEARCHVALUE = '')  
             BEGIN  
                 SET @SEARCHVALUE = NULL;  
             END;  
         SELECT TOP (100) p.Patient_Account,   
                          p.First_Name,   
                          p.Last_Name,   
                          p.MIDDLE_NAME,   
                          isnull(p.Chart_Id, '') AS Chart_Id,   
                          RTRIM(LTRIM(p.SSN)) AS SSN,   
                          p.Gender,   
                          p.Date_Of_Birth,   
                          p.Created_Date  
         FROM dbo.Patient p  
         WHERE(p.PRACTICE_CODE = @PRACTICE_CODE)  
              AND (p.First_Name LIKE '%'+@SEARCHVALUE+'%'  
                   OR p.MIDDLE_NAME LIKE '%'+@SEARCHVALUE+'%'  
                   OR p.Last_Name LIKE+'%'+@SEARCHVALUE+'%'  
                   OR (RTRIM(LTRIM(p.Last_Name))+' '+RTRIM(LTRIM(p.First_Name))) LIKE+'%'+@SEARCHVALUE+'%'  
                   OR (RTRIM(LTRIM(p.Last_Name))+''+RTRIM(LTRIM(p.First_Name))) LIKE+'%'+@SEARCHVALUE+'%'  
                   OR (RTRIM(LTRIM(p.First_Name))+' '+RTRIM(LTRIM(p.Last_Name))) LIKE+'%'+@SEARCHVALUE+'%'  
                   OR (RTRIM(LTRIM(p.First_Name))+''+RTRIM(LTRIM(p.Last_Name))) LIKE+'%'+@SEARCHVALUE+'%'  
                   OR p.Chart_Id LIKE '%'+@SEARCHVALUE+'%'  
                   OR p.SSN LIKE '%'+@SEARCHVALUE+'%'  
                   OR p.Gender LIKE '%'+@SEARCHVALUE+'%'  
                   OR CONVERT(VARCHAR(50), isnull(p.Date_Of_Birth, 0), 101) LIKE '%'+@SEARCHVALUE+'%'  
                   OR CONVERT(VARCHAR(50), isnull(p.Created_Date, 0), 101) LIKE '%'+@SEARCHVALUE+'%')  
              AND ISNULL(p.DELETED, 0) = 0;  
     END;
