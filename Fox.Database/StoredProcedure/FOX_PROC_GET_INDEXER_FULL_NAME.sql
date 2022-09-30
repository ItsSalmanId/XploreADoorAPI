IF (OBJECT_ID('FOX_PROC_GET_INDEXER_FULL_NAME') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INDEXER_FULL_NAME 
GO    
-- =============================================                    
-- Author:  <Author,Abdur Rafay>                    
-- Create date: <Create Date,07/30/2021>                    
-- DESCRIPTION: <GET INDEXER FULL NAME>                
              
-- [FOX_PROC_GET_INDEXER_FULL_NAME] 'sdfsdfsdfsdfsdfsdfsdfsdfs_544563'                   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INDEXER_FULL_NAME](        
@INDEXER VARCHAR(50))              
AS                
BEGIN     
SELECT FIRST_NAME +' '+LAST_NAME AS FULL_NAME FROM FOX_TBL_APPLICATION_USER WHERE USER_NAME = @INDEXER    
END;   
