IF (OBJECT_ID('FOX_PROC_GET_AUTO_INDEXING_SERVICE_LAST_EXECUTION_DATE ') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_AUTO_INDEXING_SERVICE_LAST_EXECUTION_DATE 
GO
-- =============================================              
-- Author:  <Author,Abdur Rafay>              
-- Create date: <Create Date,06/22/2021>              
-- DESCRIPTION: <GET INDEXERS> 

CREATE PROC [dbo].[FOX_PROC_GET_AUTO_INDEXING_SERVICE_LAST_EXECUTION_DATE]      
  AS      
  SELECT LAST_EXECUTION_DATE FROM [FOX_TBL_AUTO_INDEXING_CONFIG] 

