IF (OBJECT_ID('FOX_PROC_AD_FILES_TO_DB_FROM_RFO') IS NOT NULL) DROP PROCEDURE FOX_PROC_AD_FILES_TO_DB_FROM_RFO
GO
-- =============================================                    
-- Author:  <Author,Abdur Rafey>                    
-- Create date: <Create Date,01/11/2021>                    
-- DESCRIPTION: <ADD FILES TO DB FROM RFO>       
CREATE PROCEDURE [dbo].[FOX_PROC_AD_FILES_TO_DB_FROM_RFO]    
@FILE_ID BIGINT,    
@WORKID BIGINT,         
@FILEPATH VARCHAR(500),      
@LOGOPATH VARCHAR(500),    
@IS_FROM_INDEX_INFO BIT NULL    
AS                      
BEGIN                      
 IF NOT EXISTS(SELECT TOP 1 * FROM FOX_TBL_WORK_QUEUE_File_All WHERE WORK_ID = @WORKID AND FILE_PATH1 = @FILEPATH AND FILE_PATH = @LOGOPATH)     
 BEGIN    
 INSERT INTO FOX_TBL_WORK_QUEUE_File_All     
 (FILE_ID, UNIQUE_ID, FILE_PATH, FILE_PATH1, deleted, WORK_ID)    
 VALUES (@FILE_ID,  CAST(@WORKID AS VARCHAR), @LOGOPATH, @FILEPATH, 0, @WORKID);    
 SELECT * FROM FOX_TBL_WORK_QUEUE_File_All WHERE FILE_ID = @FILE_ID    
 END    
 ELSE    
 BEGIN    
  IF(@IS_FROM_INDEX_INFO = 1)    
  BEGIN    
  UPDATE FOX_TBL_WORK_QUEUE_File_All    
  SET FILE_PATH = @LOGOPATH, FILE_PATH1 = @FILEPATH    
  WHERE UNIQUE_ID = CAST(@WORKID AS VARCHAR)    
  SELECT * FROM FOX_TBL_WORK_QUEUE_File_All  WHERE UNIQUE_ID = CAST(@WORKID AS VARCHAR)    
  END    
 END    
END;         
      
      
      
