IF (OBJECT_ID('FOX_PROC_GET_ADVANCE_SEARCH_PATIENT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ADVANCE_SEARCH_PATIENT
GO
------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ADVANCE_SEARCH_PATIENT]
(@PRACTICE_CODE   BIGINT, 
 @First_Name      VARCHAR(50), 
 @Middle_Name     VARCHAR(50), 
 @Last_Name       VARCHAR(50), 
 @CHART_ID        VARCHAR(100), 
 @SSN             VARCHAR(9), 
 @Gender          VARCHAR(15), 
 @DOB             VARCHAR(50), 
 @CreatedDate     VARCHAR(15), 
 @SEARCH_STRING   VARCHAR(50), 
 @CURRENT_PAGE    INT, 
 @RECORD_PER_PAGE INT
)
AS  
    ---------------------------------------------  
    --DECLARE   
    --  @PRACTICE_CODE  BIGINT = 1011163    
    -- ,@First_Name   VARCHAR(50) = ''    
    -- ,@Middle_Name     VARCHAR(50) = ''    
    -- ,@Last_Name       VARCHAR(10) = ''    
    -- ,@CHART_ID        VARCHAR(50) = ''    
    -- ,@SSN             VARCHAR(50) = ''    
    -- ,@Gender          VARCHAR(50) = ''    
    -- ,@DOB             VARCHAR(50) = '09/12/2018'    
    -- ,@CreatedDate     VARCHAR(15) = ''   
    -- ,@SEARCH_STRING  VARCHAR(50) = ''  
    -- ,@CURRENT_PAGE  INT = 1  
    -- ,@RECORD_PER_PAGE INT = 10   
    --------------------------------------------  
     BEGIN
         IF CHARINDEX(',', @SEARCH_STRING) > 0
             BEGIN
                 SET @SEARCH_STRING = Replace(@SEARCH_STRING, ',', '');
             END;
         IF(@SEARCH_STRING = '')
             BEGIN
                 SET @SEARCH_STRING = NULL;
             END;
             ELSE
             BEGIN
                 SET @SEARCH_STRING = @SEARCH_STRING+'%';
             END;
         IF(@First_Name = '')
             BEGIN
                 SET @First_Name = NULL;
             END;
             ELSE
             BEGIN
                 SET @First_Name = @First_Name+'%';
             END;
         IF(@Middle_Name = '')
             BEGIN
                 SET @Middle_Name = NULL;
             END;
             ELSE
             BEGIN
                 SET @Middle_Name = @Middle_Name+'%';
             END;
         IF(@Last_Name = '')
             BEGIN
                 SET @Last_Name = NULL;
             END;
             ELSE
             BEGIN
                 SET @Last_Name = @Last_Name+'%';
             END;
         IF(@CHART_ID = '')
             BEGIN
                 SET @CHART_ID = NULL;
             END;
             ELSE
             BEGIN
                 SET @CHART_ID = @CHART_ID+'%';
             END;
         IF(@SSN = '')
             BEGIN
                 SET @SSN = NULL;
             END;
             ELSE
             BEGIN
                 SET @SSN = @SSN;
             END;
         IF(@Gender = '')
             BEGIN
                 SET @Gender = NULL;
             END;
             ELSE
             BEGIN
                 SET @Gender = @Gender;
             END;
         IF(@DOB = '')
             BEGIN
                 SET @DOB = NULL;
             END;
             ELSE
             BEGIN
                 SET @DOB = @DOB+'%';
             END;
         IF(@CreatedDate = '')
             BEGIN
                 SET @CreatedDate = NULL;
             END;
             ELSE
             BEGIN
                 SET @CreatedDate = @CreatedDate+'%';
             END;
         IF(@RECORD_PER_PAGE = 0)
             BEGIN
                 SELECT @RECORD_PER_PAGE = COUNT(*)
                 FROM PATIENT;
             END;
             ELSE
             BEGIN
                 SET @RECORD_PER_PAGE = @RECORD_PER_PAGE;
             END;
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE;
         DECLARE @TOATL_PAGESUDM FLOAT;
         SELECT @TOATL_PAGESUDM = COUNT(*)
         FROM PATIENT p
         WHERE(p.Practice_Code = @PRACTICE_CODE)
              AND (@SEARCH_STRING IS NULL
                   OR (p.First_Name LIKE '%'+@SEARCH_STRING+'%'
                       OR p.MIDDLE_NAME LIKE '%'+@SEARCH_STRING+'%'
                       OR p.Last_Name LIKE+'%'+@SEARCH_STRING+'%'
                       OR (RTRIM(LTRIM(p.Last_Name))+' '+RTRIM(LTRIM(p.First_Name))) LIKE+'%'+@SEARCH_STRING+'%'
                       OR (RTRIM(LTRIM(p.Last_Name))+''+RTRIM(LTRIM(p.First_Name))) LIKE+'%'+@SEARCH_STRING+'%'
                       OR (RTRIM(LTRIM(p.First_Name))+' '+RTRIM(LTRIM(p.Last_Name))) LIKE+'%'+@SEARCH_STRING+'%'
                       OR (RTRIM(LTRIM(p.First_Name))+''+RTRIM(LTRIM(p.Last_Name))) LIKE+'%'+@SEARCH_STRING+'%'
                       OR p.Chart_Id LIKE '%'+@SEARCH_STRING+'%'
                       OR p.SSN LIKE '%'+@SEARCH_STRING+'%'
                       OR p.Gender LIKE '%'+@SEARCH_STRING+'%'
                       OR CONVERT(VARCHAR(50), isnull(p.Date_Of_Birth, 0), 101) LIKE '%'+@SEARCH_STRING+'%'
                       OR CONVERT(VARCHAR(50), isnull(p.Created_Date, 0), 101) LIKE '%'+@SEARCH_STRING+'%'))
              AND (@First_Name IS NULL
                   OR p.First_Name LIKE '%'+@First_Name+'%')
              AND (@MIDDLE_NAME IS NULL
                   OR p.MIDDLE_NAME LIKE '%'+@MIDDLE_NAME+'%'
                   OR p.MI LIKE '%'+@MIDDLE_NAME+'%')
              AND (@Last_Name IS NULL
                   OR p.Last_Name LIKE '%'+@Last_Name+'%')
              AND (@CHART_ID IS NULL
                   OR p.Chart_Id LIKE '%'+@CHART_ID+'%')
              AND (@SSN IS NULL
                   OR RIGHT(isnull(p.SSN, ''), 4) LIKE '%'+@SSN+'%')
              AND (@Gender IS NULL
                   OR p.Gender LIKE '%'+@Gender+'%')
              AND (@DOB IS NULL
                   OR CONVERT(VARCHAR(50), isnull(p.Date_Of_Birth, 0), 101) LIKE '%'+@DOB+'%'        
              --OR replace(left(convert(VARCHAR(50), isnull(p.Date_Of_Birth, 0), 101), 10), '/', '') LIKE  @DOB + '%'        
              )
              AND (@CreatedDate IS NULL
                   OR CONVERT(VARCHAR(50), isnull(p.Created_Date, 0), 101) LIKE '%'+@CreatedDate+'%')
              AND (@SEARCH_STRING IS NOT NULL
                   OR @First_Name IS NOT NULL
                   OR @Middle_Name IS NOT NULL
                   OR @Last_Name IS NOT NULL
                   OR @CHART_ID IS NOT NULL
                   OR @SSN IS NOT NULL
                   OR @Gender IS NOT NULL
                   OR @DOB IS NOT NULL
                   OR @CreatedDate IS NOT NULL)
              AND (isnull(p.Deleted, 0) != 1);
         DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM;
         SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE);
         SELECT *, 
                @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES, 
                @TOTAL_RECORDS TOTAL_RECORDS
         FROM
         (
             SELECT p.Patient_Account, 
                    RTRIM(LTRIM(p.First_Name)) AS First_Name, 
                    RTRIM(LTRIM(p.Last_Name)) AS Last_Name, 
                    p.MIDDLE_NAME, 
                    isnull(p.Chart_Id, '') AS Chart_Id, 
                    RTRIM(LTRIM(p.SSN)) AS SSN, 
                    p.Gender, 
                    p.Date_Of_Birth, 
                    p.Created_Date
             FROM PATIENT p
             WHERE(p.Practice_Code = @PRACTICE_CODE)
                  AND (@SEARCH_STRING IS NULL
                       OR (p.First_Name LIKE '%'+@SEARCH_STRING+'%'
                           OR p.MIDDLE_NAME LIKE '%'+@SEARCH_STRING+'%'
                           OR p.Last_Name LIKE+'%'+@SEARCH_STRING+'%'
                           OR (RTRIM(LTRIM(p.Last_Name))+' '+RTRIM(LTRIM(p.First_Name))) LIKE+'%'+@SEARCH_STRING+'%'
                           OR (RTRIM(LTRIM(p.Last_Name))+''+RTRIM(LTRIM(p.First_Name))) LIKE+'%'+@SEARCH_STRING+'%'
                           OR (RTRIM(LTRIM(p.First_Name))+' '+RTRIM(LTRIM(p.Last_Name))) LIKE+'%'+@SEARCH_STRING+'%'
                           OR (RTRIM(LTRIM(p.First_Name))+''+RTRIM(LTRIM(p.Last_Name))) LIKE+'%'+@SEARCH_STRING+'%'
                           OR p.Chart_Id LIKE '%'+@SEARCH_STRING+'%'
                           OR p.SSN LIKE '%'+@SEARCH_STRING+'%'
                           OR p.Gender LIKE '%'+@SEARCH_STRING+'%'
                           OR CONVERT(VARCHAR(50), isnull(p.Date_Of_Birth, 0), 101) LIKE '%'+@SEARCH_STRING+'%'
                           OR CONVERT(VARCHAR(50), isnull(p.Created_Date, 0), 101) LIKE '%'+@SEARCH_STRING+'%'))
                  AND (@First_Name IS NULL
                       OR p.First_Name LIKE '%'+@First_Name+'%')
                  AND (@MIDDLE_NAME IS NULL
                       OR p.MIDDLE_NAME LIKE '%'+@MIDDLE_NAME+'%'
                       OR p.MI LIKE '%'+@MIDDLE_NAME+'%')
                  AND (@Last_Name IS NULL
                       OR p.Last_Name LIKE '%'+@Last_Name+'%')
                  AND (@CHART_ID IS NULL
                       OR p.Chart_Id LIKE '%'+@CHART_ID+'%')
                  AND (@SSN IS NULL
                       OR RIGHT(isnull(p.SSN, ''), 4) LIKE '%'+@SSN+'%')
                  AND (@Gender IS NULL
                       OR p.Gender LIKE '%'+@Gender+'%')
                  AND (@DOB IS NULL
                       OR CONVERT(VARCHAR(50), isnull(p.Date_Of_Birth, 0), 101) LIKE '%'+@DOB+'%'        
                  --OR replace(left(convert(VARCHAR(50), isnull(p.Date_Of_Birth, 0), 101), 10), '/', '') LIKE  @DOB + '%'        
                  )
                  AND (@CreatedDate IS NULL
                       OR CONVERT(VARCHAR(50), isnull(p.Created_Date, 0), 101) LIKE '%'+@CreatedDate+'%')
                  AND (@SEARCH_STRING IS NOT NULL
                       OR @First_Name IS NOT NULL
                       OR @Middle_Name IS NOT NULL
                       OR @Last_Name IS NOT NULL
                       OR @CHART_ID IS NOT NULL
                       OR @SSN IS NOT NULL
                       OR @Gender IS NOT NULL
                       OR @DOB IS NOT NULL
                       OR @CreatedDate IS NOT NULL)
                  AND (isnull(p.Deleted, 0) != 1)
         ) AS PATIENT
         ORDER BY PATIENT.First_Name ASC, 
                  PATIENT.Last_Name ASC
         OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY;
     END;  

