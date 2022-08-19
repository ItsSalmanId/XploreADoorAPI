IF (OBJECT_ID('FOX_GET_SMART_PATIENTS_For_Task') IS NOT NULL ) DROP PROCEDURE FOX_GET_SMART_PATIENTS_For_Task  
GO 
  CREATE PROCEDURE [dbo].[FOX_GET_SMART_PATIENTS_For_Task] --'1011163', '234'                        
(                
 @PRACTICE_CODE BIGINT,                     
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
         SELECT TOP (100)                    
                          p.First_Name,                         
                          p.Last_Name,                   
        p.Patient_Account,                  
        isnull(p.Chart_Id, '') AS Chart_Id,                                                   
                          p.Gender,                     
                          p.Date_Of_Birth,                     
                          p.Created_Date                    
         FROM dbo.Patient p                  
         WHERE p.PRACTICE_CODE = @PRACTICE_CODE                    
              AND (                
      (RTRIM(LTRIM(p.Last_Name))+' '+RTRIM(LTRIM(p.First_Name))) LIKE+'%'+@SEARCHVALUE+'%'                
                    --OR (RTRIM(LTRIM(p.Last_Name))+''+RTRIM(LTRIM(p.First_Name))) LIKE+'%'+@SEARCHVALUE +'%'                 
                    OR  (RTRIM(LTRIM(p.First_Name))+' '+RTRIM(LTRIM(p.Last_Name))) LIKE+'%'+@SEARCHVALUE +'%'                 
                    --OR (RTRIM(LTRIM(p.First_Name))+''+RTRIM(LTRIM(p.Last_Name))) LIKE+'%'+@SEARCHVALUE +'%'                
     OR p.First_Name LIKE '%'+@SEARCHVALUE  +'%'                
     OR p.Last_Name LIKE+'%'+@SEARCHVALUE+'%'                
     OR p.Chart_Id LIKE '%'+@SEARCHVALUE +'%'                
     OR P.Patient_Account LIKE '%' + @SEARCHVALUE +'%'                
     --OR p.Patient_Account LIKE '%'+@SEARCHVALUE+'%'                
       )                                      
              AND ISNULL(p.DELETED, 0) = 0                    
  ORDER BY                
   CASE WHEN ISNUMERIC(@SEARCHVALUE) = 1                 
    THEN                  
     P.CHART_ID                
   END ASC,                
   CASE WHEN ISNUMERIC(@SEARCHVALUE) = 1                
    THEN                
     p.Patient_Account                
   END ASC,                   
   CASE WHEN ISNUMERIC(@SEARCHVALUE) = 0                 
   THEN                
    P.LAST_NAME                
   END ASC,                
   CASE WHEN ISNUMERIC(@SEARCHVALUE) = 0                 
   THEN                
    P.FIRST_NAME                
   END ASC                
     END;                
                
