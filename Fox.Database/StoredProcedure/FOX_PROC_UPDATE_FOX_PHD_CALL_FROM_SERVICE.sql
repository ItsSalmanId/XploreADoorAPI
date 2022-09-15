IF (OBJECT_ID('FOX_PROC_UPDATE_FOX_PHD_CALL_FROM_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDATE_FOX_PHD_CALL_FROM_SERVICE  
GO 
---------------------  
-- =============================================          
-- Author:  <Author,Asad Ejaz>          
-- Create date: <Create Date,05/17/2019>          
-- DESCRIPTION: <UPDATE FOX PHD RECORDING PATH FROM SERVICE>          
CREATE PROCEDURE [dbo].[FOX_PROC_UPDATE_FOX_PHD_CALL_FROM_SERVICE]        
(@FOX_PHD_CALL_DETAILS_ID BIGINT,         
 @CALL_RECORDING_PATH     VARCHAR(500)        
)        
AS        
     BEGIN        
         SET NOCOUNT ON;        
         UPDATE FOX_TBL_PHD_CALL_DETAILS        
           SET         
               CALL_RECORDING_PATH = @CALL_RECORDING_PATH        
         WHERE FOX_PHD_CALL_DETAILS_ID = @FOX_PHD_CALL_DETAILS_ID;        
     END; 