IF (OBJECT_ID('FOX_PROC_GET_SUBSCRIBERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SUBSCRIBERS  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SUBSCRIBERS] --1011163,'todd'  
 (  
 @PRACTICE_CODE BIGINT  
 ,@SEARCHVALUE VARCHAR(MAX)  
 )  
AS  
BEGIN  
 IF (@SEARCHVALUE = '')  
 BEGIN  
  SET @SEARCHVALUE = NULL  
 END  
 if CHARINDEX(',',@SEARCHVALUE) > 0  
   begin  
    set @SEARCHVALUE = Replace(@SEARCHVALUE,',', '')  
   end  
  
 SELECT TOP (100) GUARANTOR_CODE  
  ,GUARANT_FNAME  
  ,GUARANT_MI  
  ,GUARANT_LNAME  
  ,GUARANT_ADDRESS  
  ,GUARANT_ZIP  
  ,GUARANT_CITY  
  ,GUARANT_STATE  
  ,GUARANT_GENDER  
  ,GUARANT_DOB  
  ,GUARANT_HOME_PHONE  
 FROM Guarantors  
 WHERE (  
   GUARANT_FNAME LIKE '%' + @SEARCHVALUE + '%'  
   OR GUARANT_LNAME LIKE '%' + @SEARCHVALUE + '%'  
   OR GUARANT_FNAME + ' ' + GUARANT_LNAME LIKE '%' + @SEARCHVALUE + '%'  
   OR GUARANT_LNAME + ' ' + GUARANT_FNAME LIKE '%' + @SEARCHVALUE + '%'  
  
   )  
  AND ISNULL(deleted, 0) = 0  
END  
  
/**************************************************************************************************************************************************************************************************/ 