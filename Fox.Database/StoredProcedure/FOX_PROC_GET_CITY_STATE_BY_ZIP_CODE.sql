IF (OBJECT_ID('FOX_PROC_GET_CITY_STATE_BY_ZIP_CODE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_CITY_STATE_BY_ZIP_CODE 
GO                  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_CITY_STATE_BY_ZIP_CODE]         
 @zipCode VARCHAR(9)        
AS        
     BEGIN        
    
       IF( (SELECT COUNT(*) FROM Zip_City_State WHERE ZIP_Code = @zipCode)>0 )     
       BEGIN     
       SELECT ZIP_Code AS ZIP_CODE,         
                        City_Name AS CITY_NAME,         
                        State_Code AS STATE_CODE        
                 FROM Zip_City_State        
                 WHERE ISNULL(Deleted, 0) = 0        
                       AND ZIP_Code = @zipCode;    
       END    
    ELSE        
    BEGIN        
    SELECT ZIP_Code AS ZIP_CODE,         
                        City_Name AS CITY_NAME,         
                        State_Code AS STATE_CODE        
                 FROM Zip_City_State        
                 WHERE ISNULL(Deleted, 0) = 0        
                       AND ZIP_Code = LEFT(@zipCode, 5)  ;        
             END;        
     END; 