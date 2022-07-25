IF (OBJECT_ID('FOX_PROC_CHANGE_STATUS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_CHANGE_STATUS
GO
-- =============================================                    
-- Author:  <Muhammad Arqam>                    
-- Create date: 02/01/2020>                    
-- Description: <Description,,>                    
-- =============================================                                         
CREATE PROCEDURE FOX_PROC_CHANGE_STATUS               
 @PRACTICE_CODE BIGINT,      
 @USER_NAME VARCHAR(70),       
 @Work_ID BIGINT             
AS                    
BEGIN                    
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED       
     
 UPDATE FOX_TBL_WORK_QUEUE          
 SET WORK_STATUS = 'Index Pending',          
 COMPLETED_BY=NULL,          
 COMPLETED_DATE=NULL,          
 INDEXED_BY=NULL,          
 INDEXED_DATE=NULL,        
 MODIFIED_DATE= GETDATE(),      
 MODIFIED_BY=@USER_NAME      
 WHERE WORK_ID = @Work_ID AND PRACTICE_CODE = @PRACTICE_CODE           
          
          
END   
