IF (OBJECT_ID('FOX_TBL_GET_DUPLICATE_PATIENTS') IS NOT NULL ) DROP PROCEDURE FOX_TBL_GET_DUPLICATE_PATIENTS  
GO    
--FOX_TBL_GET_DUPLICATE_PATIENTS_RAFAY 'TEST','TEST','','',1011163,''      
CREATE Procedure [dbo].[FOX_TBL_GET_DUPLICATE_PATIENTS]       
@First_Name VARCHAR(50),      
@Last_Name VARCHAR(50),      
@DOB DATE,      
@gender VARCHAR(50),      
@PRACTICE_CODE BIGINT,      
@Patient_Account BIGINT      
AS BEGIN      
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED    
  IF(@First_Name = '')      
             BEGIN      
                 SET @First_Name = NULL;      
             END;          
     IF(@Last_Name = '')      
             BEGIN      
                 SET @Last_Name = NULL;      
             END;          
     IF(@DOB = '')      
             BEGIN      
                 SET @DOB = NULL;      
             END;          
  IF(@gender = '')      
             BEGIN      
                 SET @gender = NULL;      
             END;          
     IF(@PRACTICE_CODE = '')      
             BEGIN      
                 SET @PRACTICE_CODE = NULL;      
             END;          
      
      
    SELECT       
    ROW_NUMBER() OVER(ORDER BY P.Patient_Account) AS ROW,      
    P.Last_Name,      
    P.First_Name,      
    P.MIDDLE_NAME as MI,      
    P.SSN,      
    P.Gender,      
    CONVERT(DATE, P.Date_Of_Birth, 101) AS DOB,         
    P.Created_By,      
    CONVERT(DATE, P.Created_Date, 101) AS Created_Date,        
    P.Chart_ID,      
    P.Patient_Account,      
    FC.CODE AS FINANCIAL_CLASS,      
    FC.FINANCIAL_CLASS_ID      
    FROM PATIENT P       
    LEFT JOIN FOX_TBL_PATIENT FTP ON P.Patient_Account = FTP.Patient_Account      
    LEFT JOIN FOX_TBL_FINANCIAL_CLASS FC ON  FTP.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID  AND ISNULL(FC.DELETED,0) = 0 AND FC.Practice_Code = @PRACTICE_CODE      
          
    WHERE       
          
    (@First_Name IS NULL OR P.First_Name LIKE @First_Name)      
    AND (@Last_Name IS NULL OR P.Last_Name LIKE @Last_Name)      
    AND (@gender IS NULL OR P.Gender LIKE @gender)      
    and (@DOB IS NULL OR CONVERT(DATE, P.DATE_OF_BIRTH) = @DOB)       
    AND P.Practice_Code = @PRACTICE_CODE      
    AND P.Patient_Account <> @Patient_Account      
    AND ISNULL(P.Deleted,0) = 0       
    AND ISNULL(FTP.Deleted,0) = 0       
      
END;      
