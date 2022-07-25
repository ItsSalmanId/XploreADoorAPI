IF (OBJECT_ID('FOX_GET_INDEX_INFO_PROCEDURES') IS NOT NULL) DROP PROCEDURE FOX_GET_INDEX_INFO_PROCEDURES
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure  [dbo].[FOX_GET_INDEX_INFO_PROCEDURES] --544105
(
@WORK_ID bigint
)
as begin
 select PAT_PROC_ID,PROC_CODE,CPT_DESC,SPECIALITY_PROGRAM from FOX_TBL_PATIENT_PROCEDURE where work_id = @WORK_ID and isnull(DELETED,0)= 0 order by PAT_PROC_ID desc
end





