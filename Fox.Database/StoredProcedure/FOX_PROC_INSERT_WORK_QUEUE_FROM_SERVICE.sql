IF (OBJECT_ID('FOX_PROC_INSERT_WORK_QUEUE_FROM_SERVICE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_INSERT_WORK_QUEUE_FROM_SERVICE  
GO 
CREATE Procedure [dbo].[FOX_PROC_INSERT_WORK_QUEUE_FROM_SERVICE]   
(  
 @WORK_ID BIGINT,  
 @UNIQUE_ID varchar(50),  
 @PRACTICE_CODE bigint,  
 @SORCE_TYPE varchar(50),  
 @SORCE_NAME varchar(100),  
 @WORK_STATUS varchar(100),  
 @RECEIVE_DATE datetime,  
 @TOTAL_PAGES int,  
 @FILE_PATH varchar(max),  
 @FAX_ID varchar(50),  
 @IS_EMERGENCY bit  
  
)  
AS  
BEGIN  
SET NOCOUNT ON;  

declare @is_blacklisted_source bit = 
case when exists (select 1 from FOX_TBL_BLACKLISTED_WHITELISTED_SOURCE where SOURCE_NAME = @SORCE_NAME and PRACTICE_CODE = @PRACTICE_CODE and deleted = 0 and IS_BLACK_LISTED = 1) 
	then cast(1 as bit)
	else cast(0 as bit) 
end;
declare @is_trash bit = 
case when @is_blacklisted_source = 1	
	then cast(1 as bit)
	else cast(0 as bit) 
end;
if not exists (select 1 from FOX_TBL_WORK_QUEUE where WORK_ID=@WORK_ID)  
 begin  
  if not exists (select 1 from FOX_TBL_WORK_QUEUE where fax_id=@FAX_ID)  
   begin  
    insert into FOX_TBL_WORK_QUEUE (WORK_ID,UNIQUE_ID,PRACTICE_CODE,SORCE_NAME,SORCE_TYPE,WORK_STATUS,RECEIVE_DATE,TOTAL_PAGES,FILE_PATH,FAX_ID, IS_EMERGENCY_ORDER,IS_TRASH_REFERRAL)  
    values(@WORK_ID,@UNIQUE_ID,@PRACTICE_CODE,@SORCE_NAME,@SORCE_TYPE,@WORK_STATUS,getdate(),@TOTAL_PAGES,@FILE_PATH,@FAX_ID, @IS_EMERGENCY,@is_trash) 
   end  
 end  
else  
 begin  
  update FOX_TBL_WORK_QUEUE  
  set TOTAL_PAGES = @TOTAL_PAGES  
  where WORK_ID = @WORK_ID;  
 end  

--if(@is_blacklisted_source = cast(1 AS bit))
--	IF not exists(select 1 from FOX_TBL_WORK_ORDER_ADDTIONAL_INFO where WORK_ID=@WORK_ID AND DELETED = 0)
--		BEGIN
--			declare @FOX_WORK_ORDER_ADDTIONAL_INFO_ID nvarchar(max) =''
--			exec Web_GetColumnMaxID_Changed 'FOX_WORK_ORDER_ADDTIONAL_INFO_ID', @FOX_WORK_ORDER_ADDTIONAL_INFO_ID OUTPUT
--			INSERT INTO dbo.FOX_TBL_WORK_ORDER_ADDTIONAL_INFO
--			(
--			    WORK_ORDER_ADDTIONAL_INFO_ID,
--			    WORK_ID,
--			    IS_TRASH_REFERRAL
--			)
--			VALUES
--			(
--			    @FOX_WORK_ORDER_ADDTIONAL_INFO_ID, -- WORK_ORDER_ADDTIONAL_INFO_ID - bigint
--			    @WORK_ID, -- WORK_ID - bigint
--			    cast(1 AS bit) -- IS_TRASH_REFERRAL - bit
--			)
--		END 
--	ELSE
--		BEGIN
--			 update FOX_TBL_WORK_ORDER_ADDTIONAL_INFO  
--			 set IS_TRASH_REFERRAL = cast(1 AS bit)  
--			 where WORK_ID = @WORK_ID;
--		END
END 

