-- =============================================                                              
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>                                              
-- CREATE DATE: <CREATE DATE, 22/09/2022>                                              
-- DESCRIPTION:  THIS PROCEDURE ISUSE TO DELETE FRICTIONLESS FILE PATH                                         
-- =============================================      
CREATE PROCEDURE FOX_PROC_DELETE_FRICTIONLESS_WORK_FILE(      
@WORK_ID BIGINT NULL    
)      
AS      
BEGIN      
      
UPDATE FOX_TBL_FRICTIONLESS_WORK_QUEUE_FILE_ALL      
SET  deleted = 1     
WHERE WORK_ID = @WORK_ID    
END; 