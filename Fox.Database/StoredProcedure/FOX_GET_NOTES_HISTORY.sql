IF (OBJECT_ID('FOX_GET_NOTES_HISTORY') IS NOT NULL) DROP PROCEDURE FOX_GET_NOTES_HISTORY
GO
----19
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure [dbo].[FOX_GET_NOTES_HISTORY] --5487033
@WORK_ID bigint
as
begin
SELECT Convert(varchar,CONVERT(DateTime,CREATED_DATE),101 ) +' '+  Convert(varchar,CONVERT(DateTime,CREATED_DATE),108 ) AS CREATED_DATE,
convert(varchar,CREATED_DATE) AS Created_Date_Str,
ROW_NUMBER() OVER ( ORDER BY CREATED_DATE DESC ) AS ACTIVEROW, * 
FROM FOX_TBL_NOTES_HISTORY 
WHERE WORK_ID = @WORK_ID 
AND ISNULL(DELETED,0) = 0 
ORDER BY NOTE_ID DESC 
end



