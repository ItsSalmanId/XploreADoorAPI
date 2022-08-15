IF (OBJECT_ID('FOX_PROC_GET_SMART_HEAR_ABOUT_OPTIONS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SMART_HEAR_ABOUT_OPTIONS  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SMART_HEAR_ABOUT_OPTIONS](    
@PRACTICE_CODE BIGINT,    
@SEARCH_TEXT VARCHAR(100)    
)    
AS    
BEGIN    
if(@SEARCH_TEXT = '')    
 begin     
  set @SEARCH_TEXT = null     
 end    
else    
 begin     
  if CHARINDEX('[',@SEARCH_TEXT) > 0    
   begin    
    set @SEARCH_TEXT = Replace(@SEARCH_TEXT,'[', '')    
   end    
  if CHARINDEX(']',@SEARCH_TEXT) > 0    
   begin    
    set @SEARCH_TEXT = Replace(@SEARCH_TEXT,']', '')    
   end    
  set @SEARCH_TEXT = '%' + @SEARCH_TEXT + '%'     
 end    
    
SELECT * FROM     
FOX_TBL_HEAR_ABOUT_US_OPTIONS    
WHERE PRACTICE_CODE = @PRACTICE_CODE AND DELETED = 0    
  AND     
  (    
  CODE LIKE @SEARCH_TEXT    
  OR [NAME] LIKE @SEARCH_TEXT    
  OR (CODE + ' ' + NAME) like @SEARCH_TEXT    
  OR (CODE + '' + NAME) like @SEARCH_TEXT    
  )    
    
END 