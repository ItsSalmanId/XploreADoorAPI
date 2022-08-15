IF (OBJECT_ID('FOX_PROC_GET_Contact_Types') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_Contact_Types 
GO  
----22   
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
CREATE Procedure [dbo].[FOX_PROC_GET_Contact_Types] --'1011163'  
(  
@PRACTICE_CODE BIGINT  
)    
AS    
BEGIN     
  
select Contact_Type_ID, Type_Name from Fox_Tbl_Patient_Contact_Types   
 where ISNULL (Deleted,0) = 0   
 AND Practice_Code = @PRACTICE_CODE   
 AND ISNULL(IS_ACTIVE, 1) = 1  
 order by Type_Name   
END  
