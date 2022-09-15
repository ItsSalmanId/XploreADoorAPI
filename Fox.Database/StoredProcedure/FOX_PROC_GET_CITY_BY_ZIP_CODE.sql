IF (OBJECT_ID('FOX_PROC_GET_CITY_BY_ZIP_CODE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_CITY_BY_ZIP_CODE 
GO  
CREATE Procedure [dbo].[FOX_PROC_GET_CITY_BY_ZIP_CODE]    
@zipCode varchar(9)    
AS    
BEGIN    
 if((len(isnull(@zipCode, '')) > 5) and (len(isnull(@zipCode, '')) = 9))  
  begin  
   Select City_Name AS CITY_NAME from Zip_City_State where ZIP_Code = LEFT(@zipCode, 5)     
  end  
 else  
  if(len(isnull(@zipCode, '')) = 5)  
   begin  
    Select City_Name AS CITY_NAME from Zip_City_State where ZIP_Code = @zipCode    
   end  
END  
