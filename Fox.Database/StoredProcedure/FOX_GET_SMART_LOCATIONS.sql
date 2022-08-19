IF (OBJECT_ID('FOX_GET_SMART_LOCATIONS') IS NOT NULL ) DROP PROCEDURE FOX_GET_SMART_LOCATIONS  
GO 
CREATE PROCEDURE [dbo].[FOX_GET_SMART_LOCATIONS] --'1011163', '50 Ledge Road'                  
(@PRACTICE_CODE BIGINT,                 
 @SEARCHVALUE   VARCHAR(MAX)                
)                
AS                
     BEGIN                
         IF(@SEARCHVALUE = '')                
             BEGIN                
                 SET @SEARCHVALUE = NULL;                
             END;                
         SELECT TOP (100) LOC_ID,                 
                          CODE,                 
                          NAME,                 
                          ISNULL(Address, '') AS 'Address',                 
                          Zip,                 
                          City,                 
                          STATE,                 
                          Phone,                 
                          REGION,              
        IS_ACTIVE,                 
                          ISNULL(Description, '') AS 'Description',                 
                          Country,                 
                          ISNULL(Work_Phone, '') AS 'Work_Phone',                 
                          ISNULL(Cell_Phone, '') AS 'Cell_Phone',                 
                          ISNULL(Email_Address, '') AS 'Email_Address',                 
                          ISNULL(Fax, '') AS 'Fax',                 
                          FACILITY_TYPE_ID                
         FROM FOX_TBL_ACTIVE_LOCATIONS                
         WHERE(NAME LIKE '%'+@SEARCHVALUE+'%'                
               OR Code LIKE '%'+@SEARCHVALUE+'%'            
      OR Address LIKE '%'+@SEARCHVALUE+'%'      
         
   OR Zip LIKE  '%'+@SEARCHVALUE+'%'      
   OR City LIKE '%'+@SEARCHVALUE+'%'      
   OR State LIKE '%'+@SEARCHVALUE+'%'         
   )      
              AND ISNULL(DELETED, 0) = 0               
     AND ISNULL(IS_ACTIVE, 1) = 1                 
              AND FOX_TBL_ACTIVE_LOCATIONS.PRACTICE_CODE = @PRACTICE_CODE;                  
         --New                  
     END; 

