IF (OBJECT_ID('FOX_PROC_ASSIGN_REFERRAL_TO_INDEXER') IS NOT NULL ) DROP PROCEDURE FOX_PROC_ASSIGN_REFERRAL_TO_INDEXER
GO
  -- =============================================                    
-- Author:  <Author,Abdur Rafay>                    
-- Create date: <Create Date,07/30/2021>                    
-- DESCRIPTION: <ASSIGN REFERRAL TO INDEXER>                
              
-- [FOX_PROC_ASSIGN_REFERRAL_TO_INDEXER] 54817413 ,0000000, 'sdfsdfsdfsdfsdfsdfsdfsdfs_544563'                     
CREATE PROCEDURE [dbo].[FOX_PROC_ASSIGN_REFERRAL_TO_INDEXER](        
@WORK_ID BIGINT,    
@LOG_ID BIGINT,    
@MSG VARCHAR(500),    
@INDEXER VARCHAR(50))              
AS                
BEGIN                
UPDATE FOX_TBL_WORK_QUEUE      
SET ASSIGNED_TO = @INDEXER, ASSIGNED_BY = 'FOX TEAM', ASSIGNED_DATE = GETDATE(), WORK_STATUS = 'Index Pending'      
WHERE WORK_ID = @WORK_ID      
    
INSERT INTO FOX_TBL_WORK_ORDER_HISTORY(LOG_ID, WORK_ID, UNIQUE_ID, LOG_MESSAGE, CREATED_BY, CREATED_ON)                 
VALUES (@LOG_ID, @WORK_ID, @WORK_ID+'',@MSG, 'FOX SERVICE', GETDATE())     
    
END;   
