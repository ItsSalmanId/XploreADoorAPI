IF (OBJECT_ID('FOX_PROC_GET_RECORDING_SERCVICE_CONFIG') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECORDING_SERCVICE_CONFIG  
GO 
---------------------  
-- =============================================          
-- Author:  <Author,Asad Ejaz>          
-- Create date: <Create Date,05/17/2019>          
-- DESCRIPTION: <UPDATE FOX PHD RECORDING PATH FROM SERVICE>  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_RECORDING_SERCVICE_CONFIG]        
AS        
     BEGIN        
         SET NOCOUNT ON;        
         SELECT * FROM [FOX_TBL_PHD_CALLING_UPDATE_SERVICE_CONFIG] WHERE deleted = 0;        
     END; 