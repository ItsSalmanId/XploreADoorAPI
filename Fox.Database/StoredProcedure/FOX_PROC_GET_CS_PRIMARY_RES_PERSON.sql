IF (OBJECT_ID('FOX_PROC_GET_CS_PRIMARY_RES_PERSON') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_CS_PRIMARY_RES_PERSON 
GO  
-------------------------------------------------------------------------------------------------- FOX_PROC_GET_CS_PRIMARY_RES_PERSON ---------------------------------------------------------------------------------------------  
---- =============================================                                
---- Author:  <Muhammad Arslan Tufail>                                
---- Create date: <10/09/2020>                                
---- Description: <Return CS PRIMARY PERSON CODE>                                
---- =============================================                  
-- EXEC [FOX_PROC_GET_CS_PRIMARY_RES_PERSON] 5001                
CREATE PROCEDURE [dbo].[FOX_PROC_GET_CS_PRIMARY_RES_PERSON]                  
@CS_Category_ID VARCHAR(25)         
AS                  
BEGIN                  
SELECT CS_Pri_Res_Person FROM CS_Case_Categories          
WHERE CS_Category_ID = @CS_Category_ID        
END; 