IF (OBJECT_ID('FOX_PROC_GET_NOTES_BY_WORKID_FOR_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_NOTES_BY_WORKID_FOR_SERVICE  
GO            
-- =============================================              
-- Author:  <Abdul Sattar>              
-- Create date: <05/03/2021>              
-- Description: <Description,Get notes by work ID,>              
-- =============================================              
--EXEC FOX_PROC_GET_NOTES_BY_WORKID_FOR_SERVICE  605102       
CREATE PROCEDURE FOX_PROC_GET_NOTES_BY_WORKID_FOR_SERVICE      
 @WORK_ID  BIGINT        
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
  SELECT top 1 NOTE_DESC FROM FOX_TBL_NOTES_HISTORY      
  WHERE WORK_ID = @WORK_ID      
  order by MODIFIED_DATE desc      
END      
