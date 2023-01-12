IF (OBJECT_ID('FOX_PROC_GET_TASK_TYPE_AND_SEND_TO_AND_SUB_TYPE_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_TYPE_AND_SEND_TO_ID  
GO 
-- Author:  <IRFAN ULLAH>                                        
-- Create date: <11/02/2022>                                        
-- Description: <GET TASK TYPE AND SEND_TO_ID AND SUB TYPE ID>                                        
-- =============================================            
-- EXEC   FOX_PROC_GET_TASK_TYPE_AND_SEND_TO_ID 1012714          
          
alter PROCEDURE [dbo].[FOX_PROC_GET_TASK_TYPE_AND_SEND_TO_AND_SUB_TYPE_ID]                       
 @PRACTICE_CODE BIGINT,          
 @SUB_TYPE VARCHAR(50)      
 AS           
 BEGIN          
          
 DECLARE @GROUP_ID BIGINT, @TASK_TYPE_ID BIGINT , @TASK_SUB_TYPE_ID BIGINT       
          
 SELECT @GROUP_ID = GROUP_ID           
 FROM FOX_TBL_GROUP WITH(NOLOCK)  
 WHERE PRACTICE_CODE = @PRACTICE_CODE         
 AND GROUP_NAME = '03TL'          
 AND ISNULL(DELETED, 0) = 0        
          
          
SELECT @TASK_TYPE_ID = TASK_TYPE_ID          
FROM FOX_TBL_TASK_TYPE   WITH(NOLOCK)        
WHERE PRACTICE_CODE = @PRACTICE_CODE          
AND LOWER(NAME) = LOWER('Billing Review - High Balance')       
      
      
--  IF OBJECT_ID('TEMPDB..#SUBTYPES') IS NOT NULL       
--     DROP TABLE #SUBTYPES    
    
--select * into #SUBTYPES from FOX_TBL_TASK_SUB_TYPE WITH(NOLOCK) where TASK_TYPE_ID=@TASK_TYPE_ID    
    
    
SELECT @TASK_SUB_TYPE_ID = TASK_SUB_TYPE_ID          
FROM FOX_TBL_TASK_SUB_TYPE  WITH(NOLOCK)     
WHERE PRACTICE_CODE = @PRACTICE_CODE          
AND TASK_TYPE_ID=@TASK_TYPE_ID      
AND LOWER(NAME) = @SUB_TYPE   
 AND ISNULL(DELETED, 0) = 0            
          
 SELECT @TASK_TYPE_ID AS TASK_TYPE_ID,@TASK_SUB_TYPE_ID AS TASK_SUB_TYPE_ID, @GROUP_ID AS SEND_TO_ID          
          
 END
    