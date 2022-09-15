IF (OBJECT_ID('FOX_PROC_GET_SPECIALITY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SPECIALITY  
GO   
-- =============================================      
-- Author:  <Muhammad Imran>      
-- Create date: <09/26/2019>      
-- Description: <Description,,>      
-- =============================================         
CREATE PROCEDURE FOX_PROC_GET_SPECIALITY    
 @SPECIALITY_ID BIGINT    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 SELECT TOP 1 * FROM FOX_TBL_SPECIALITY    
 WHERE SPECIALITY_ID = @SPECIALITY_ID     
 AND ISNULL(DELETED,0) = 0    
END   
