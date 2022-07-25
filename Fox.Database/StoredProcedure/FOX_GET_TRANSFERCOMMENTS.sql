IF (OBJECT_ID('FOX_GET_TRANSFERCOMMENTS') IS NOT NULL ) DROP PROCEDURE FOX_GET_TRANSFERCOMMENTS  
GO 
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure [dbo].[FOX_GET_TRANSFERCOMMENTS]  
(  
@WORK_ID bigint  
)  
as  
BEGIN  
select top 1 * from FOX_TBL_WORK_TRANSFER where WORK_ID = @WORK_ID order by 1 desc  
end  
  
  
