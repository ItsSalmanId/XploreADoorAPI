IF (OBJECT_ID('FOX_PROC_GET_AUTO_INDEXING_SERCVICE_CONFIG') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_AUTO_INDEXING_SERCVICE_CONFIG 
GO
-- =============================================              
-- Author:  <Author,Abdur Rafay>              
-- Create date: <Create Date,06/22/2021>              
-- DESCRIPTION: <GET INDEXERS> 

  CREATE PROCEDURE [dbo].[FOX_PROC_GET_AUTO_INDEXING_SERCVICE_CONFIG]        
AS        
     BEGIN        
         SET NOCOUNT ON;        
         SELECT * FROM FOX_TBL_AUTO_INDEXING_CONFIG WHERE deleted = 0;        
     END;

