IF (OBJECT_ID('FOX_PROC_TALKREHAB_DISABLED_MODULES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_TALKREHAB_DISABLED_MODULES  
GO 
CREATE PROCEDURE FOX_PROC_TALKREHAB_DISABLED_MODULES                 
            
AS                
  BEGIN                
  SELECT            
  TALKREHAB_MODULE_ID as TalkrehabModuleId,          
  MODULE_NAME as ModuleName,          
  IS_SHOW as IsShow          
  FROM FOX_TBL_TALKREHAB_DISABLED_MODULES           
  where isnull(deleted, 0) =0                       
  AND IS_SHOW = 0            
  END 