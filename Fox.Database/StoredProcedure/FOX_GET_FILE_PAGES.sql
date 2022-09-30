IF (OBJECT_ID('FOX_GET_File_PAGES') IS NOT NULL) DROP PROCEDURE FOX_GET_File_PAGES
GO
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure [dbo].[FOX_GET_File_PAGES] --544289
(

@WORK_ID bigint

)
AS BEGIN

select  convert(bit,0) as Checked, WORK_ID ,UNIQUE_ID,FILE_PATH,file_path1 from FOX_TBL_WORK_QUEUE_File_All  WHERE WORK_ID = @WORK_ID and isnull(deleted,0)=0

end


--select * from FOX_TBL_WORK_QUEUE_File_All where UNIQUE_ID like'%544289%'



