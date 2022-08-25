IF (OBJECT_ID('FOX_PROC_UPDATE_DOCUMENT_TYPE_BY_WORK_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_DOCUMENT_TYPE_BY_WORK_ID  
GO   
-- =============================================                
-- Author:  <Abdul Sattar>                
-- Create date: <04/08/2021>                
-- Description: <Update document type in work order table when QA code document found by service>                
-- =============================================                            
--FOX_PROC_UPDATE_DOCUMENT_TYPE_BY_WORK_ID   53423713,'Signed Order'          
CREATE PROCEDURE FOX_PROC_UPDATE_DOCUMENT_TYPE_BY_WORK_ID      
 @WORK_ID BIGINT,      
 @DOCUMENT_TYPE_NAME VARCHAR(100)           
AS                
BEGIN       
SET NOCOUNT ON;                        
    update fox_tbl_work_queue       
 set DOCUMENT_TYPE = (SELECT DOCUMENT_TYPE_ID FROM fox_tbl_document_type WHERE LOWER([NAME]) = @DOCUMENT_TYPE_NAME)      
 WHERE WORK_ID = @WORK_ID      
END  
