IF (OBJECT_ID('FOX_PROC_GET_LAST_EXECUTION_DATE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_LAST_EXECUTION_DATE  
GO  
---------------------  
-- =============================================          
-- Author:  <Author,Asad Ejaz>          
-- Create date: <Create Date,05/17/2019>          
-- DESCRIPTION: <UPDATE FOX PHD RECORDING PATH FROM SERVICE>   
CREATE PROC [dbo].[FOX_PROC_GET_LAST_EXECUTION_DATE]      
AS      
     BEGIN      
         SELECT TOP 1 LAST_EXECUTION_DATE      
         FROM [FOX_TBL_PHD_CALLING_UPDATE_SERVICE_CONFIG]      
   WHERE deleted = 0;      
     END; 