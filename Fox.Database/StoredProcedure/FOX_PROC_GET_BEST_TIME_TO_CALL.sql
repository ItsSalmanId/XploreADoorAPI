IF (OBJECT_ID('FOX_PROC_GET_BEST_TIME_TO_CALL') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_BEST_TIME_TO_CALL 
GO
-------------------------------------------------------------------------------------------------------------------------------------------------  
Create Procedure [dbo].[FOX_PROC_GET_BEST_TIME_TO_CALL] --'1011163'  
(  
@PRACTICE_CODE BIGINT  
)    
AS    
BEGIN     
  
select RT_CODE, [DESCRIPTION] from fox_tbl_best_time_to_call   
 where ISNULL (Deleted,0) = 0   
 AND ISNULL(IS_ACTIVE, 1) = 1  
 AND PRACTICE_CODE = @PRACTICE_CODE  
 order by [DESCRIPTION]  
END 