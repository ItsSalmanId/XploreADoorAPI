IF (OBJECT_ID('FOX_GET_SMART_MODIFIED_BY') IS NOT NULL ) DROP PROCEDURE FOX_GET_SMART_MODIFIED_BY  
GO   
---- =============================================                  
---- Author:  <Author,,Name>                  
---- Create date: <Create Date,,>                  
---- Description: <Description,,>                  
---- =============================================                           
--FOX_GET_SMART_MODIFIED_BY 1011163, 'ALI'               
  CREATE PROCEDURE [dbo].[FOX_GET_SMART_MODIFIED_BY] --'1011163', '234'                        
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
                          a.First_Name,                         
                          a.Last_Name,                  
      a.USER_NAME,            
      r.ROLE_NAME            
                  
                       
                       
                       
         FROM dbo.FOX_TBL_APPLICATION_USER a             
        left join FOX_TBL_ROLE r ON r.ROLE_ID = a.ROLE_ID             
         WHERE a.PRACTICE_CODE = @PRACTICE_CODE                
      AND a.IS_ACTIVE = 1                   
              AND (                
     (RTRIM(LTRIM(a.Last_Name))+' '+RTRIM(LTRIM(a.First_Name))) LIKE+'%'+@SEARCHVALUE+'%'                
                    --OR (RTRIM(LTRIM(a.Last_Name))+''+RTRIM(LTRIM(a.First_Name))) LIKE+'%'+@SEARCHVALUE +'%'                 
                    OR (RTRIM(LTRIM(a.First_Name))+' '+RTRIM(LTRIM(a.Last_Name))) LIKE+'%'+@SEARCHVALUE +'%'                 
                    --OR (RTRIM(LTRIM(a.First_Name))+''+RTRIM(LTRIM(a.Last_Name))) LIKE+'%'+@SEARCHVALUE +'%'                
     OR a.First_Name LIKE '%'+@SEARCHVALUE  +'%'                
     OR a.Last_Name LIKE+'%'+@SEARCHVALUE+'%'                
                   
       )                                      
              AND ISNULL(a.DELETED, 0) = 0                    
               
     END;                
                
                
                
                
                
