IF (OBJECT_ID('FOX_PROC_GET_UNASSIGNED_REFERRALS_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_UNASSIGNED_REFERRALS_SERVICE  
GO 
-- =============================================                  
-- Author:  <Author,Abdur Rafay>                  
-- Create date: <Create Date,06/22/2021>                  
-- DESCRIPTION: <GET INDEXERS>                
-- EXEC [dbo].[FOX_PROC_GET_UNASSIGNED_REFERRALS_SERVICE] 1011163                            
CREATE PROCEDURE [dbo].[FOX_PROC_GET_UNASSIGNED_REFERRALS_SERVICE]                          
  (                            
 @PRACTICE_CODE BIGINT                            
 )                          
WITH RECOMPILE                          
AS                            
BEGIN                            
 SET NOCOUNT ON;                            
 SELECT WORK_ID, SORCE_NAME                            
                   
  FROM FOX_TBL_WORK_QUEUE WQ                          
  WHERE WQ.PRACTICE_CODE = @PRACTICE_CODE                            
   AND ISNULL(WQ.DELETED, 0) = 0                            
   AND WQ.ASSIGNED_TO IS NULL    
    
ORDER BY CREATED_DATE DESC    
END           
