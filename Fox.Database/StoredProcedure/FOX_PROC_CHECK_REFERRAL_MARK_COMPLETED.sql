IF (OBJECT_ID('FOX_PROC_CHECK_REFERRAL_MARK_COMPLETED') IS NOT NULL ) DROP PROCEDURE FOX_PROC_CHECK_REFERRAL_MARK_COMPLETED
GO
  --FOX_PROC_CHECK_REFERRAL_MARK_COMPLETED   53422210 
CREATE PROCEDURE FOX_PROC_CHECK_REFERRAL_MARK_COMPLETED               
 @WORK_ID BIGINT           
AS                
BEGIN                
 -- SET NOCOUNT ON added to prevent extra result sets from                  
 SET NOCOUNT ON;                
 SELECT      
 CASE      
 WHEN LOWER(WORK_STATUS) = 'completed' then cast(1 as bit)      
 else cast(0 as bit)       
 end as IS_MARK_COMPLETED      
  FROM FOX_TBL_WORK_QUEUE      
 WHERE work_id = @WORK_ID              
 AND ISNULL(DELETED,0) = 0                
END  
