IF (OBJECT_ID('FOX_GET_INDEX_INFO_DOCUMNETS') IS NOT NULL ) DROP PROCEDURE FOX_GET_INDEX_INFO_DOCUMNETS  
GO 
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure  [dbo].[FOX_GET_INDEX_INFO_DOCUMNETS] --544127
(
@WORK_ID bigint
)
as begin
 select PAT_DOC_ID,COMENTS,FILE_PATH,FILE_NAME from FOX_TBL_PATIENT_DOCUMENTS where work_id = @WORK_ID and isnull(DELETED,0)= 0 order by PAT_DOC_ID desc
end





