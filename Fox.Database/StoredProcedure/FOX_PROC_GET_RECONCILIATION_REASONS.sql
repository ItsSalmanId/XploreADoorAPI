IF (OBJECT_ID('FOX_PROC_GET_RECONCILIATION_REASONS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_RECONCILIATION_REASONS  
GO      
CREATE PROCEDURE [DBO].[FOX_PROC_GET_RECONCILIATION_REASONS]          
 @PRACTICE_CODE BIGINT          
AS              
     BEGIN              
         SELECT *            
         FROM dbo.FOX_TBL_RECONCILIATION_REASON ftrdt              
         WHERE ISNULL(ftrdt.DELETED,0) = 0 and ftrdt.PRACTICE_CODE = @PRACTICE_CODE;              
     END; 