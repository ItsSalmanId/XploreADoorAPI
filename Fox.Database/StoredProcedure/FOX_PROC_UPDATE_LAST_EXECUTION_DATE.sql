IF (OBJECT_ID('FOX_PROC_UPDATE_LAST_EXECUTION_DATE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_LAST_EXECUTION_DATE  
GO 
---------------------  
-- =============================================          
-- Author:  <Author,Asad Ejaz>          
-- Create date: <Create Date,05/17/2019>          
-- DESCRIPTION: <UPDATE FOX PHD RECORDING PATH FROM SERVICE>   
CREATE PROC [dbo].[FOX_PROC_UPDATE_LAST_EXECUTION_DATE](@DATE DATE)      
AS      
     BEGIN      
         UPDATE [FOX_TBL_PHD_CALLING_UPDATE_SERVICE_CONFIG]      
           SET       
               LAST_EXECUTION_DATE = @DATE;      
     END; 