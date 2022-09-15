IF (OBJECT_ID('Fox_Queue_Split_Document') IS NOT NULL) DROP PROCEDURE Fox_Queue_Split_Document
GO  
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure [dbo].[Fox_Queue_Split_Document] --324234,'FoxDocumentDirectory\\1011163\\Fox\\Logo_1011163_190120182322133060.jpg','','',0,0         
(       
 @FILE_ID bigint,  
 @UNIQUE_ID VARCHAR(200)='',   
 @File_Path_1   VARCHAR(max)='',  
 @File_path    VARCHAR(max)='',  
 @NEW_WORK_ID bigint,  
 @Old_work_id bigint  
             
)            
--AS BEGIN             
--DECLARE @Id BIGINT;            
--DECLARE @STATUS VARCHAR(50);  
--declare @UniID int;     
--declare @like_id varchar(50);   
--declare @without_ varchar(50);   
   
  
  
  
--if CHARINDEX('_',@UNIQUE_ID) > 0  
--begin  
  
-- set  @UniID =(select count(*) as  UNIQUE_ID  from FOX_TBL_WORK_QUEUE_File_All where   UNIQUE_ID like @UNIQUE_ID+'%' and isnull(deleted,0)=0)  
-- set @UniID =(@UniID+1)    
--if (@UniID>1)  
--begin  
--set @UNIQUE_ID=(SELECT LEFT(@UNIQUE_ID, CHARINDEX('_',@UNIQUE_ID)-1) )  
--set @like_id=(@UNIQUE_ID+'_'+convert(varchar,@UniID))  
--END  
  
--end  
--else  
--begin  
  
-- set  @UniID =(select count(*) as  UNIQUE_ID  from FOX_TBL_WORK_QUEUE_File_All where Replace(UNIQUE_ID, '_', '@') like ''+@UNIQUE_ID+'@%'  and isnull(deleted,0)=0)  
-- set @UniID =(@UniID+1)    
--if (@UniID>1)  
--begin  
--if CHARINDEX('_',@UNIQUE_ID) > 0  
--begin  
--set @UNIQUE_ID=(SELECT LEFT(@UNIQUE_ID, CHARINDEX('_',@UNIQUE_ID)-1) )  
--set @like_id=(@UNIQUE_ID+'_'+convert(varchar,@UniID))  
--END   
--else  
--begin  
--set @like_id=(@UNIQUE_ID+'_'+convert(varchar,@UniID))  
--END  
--END  
--else  
--begin  
--set @like_id=(@UNIQUE_ID+'_1')  
--End  
  
--End   
  
----IF OBJECT_ID('tempdb..#tempWorkID') IS NOT NULL DROP TABLE #tempWorkID  
  
----create table #tempWorkID(work_id bigint)  
  
----insert into #tempWorkID execute Web_GetMaxColumnID 'WORK_ID'  
   
----------  
  
-- update FOX_TBL_WORK_QUEUE_File_All set deleted=1 where UNIQUE_ID  =@UNIQUE_ID_1 and File_path1=@File_Path_1  
  
--select @like_id as UNIQUE_ID ,WORK_ID, PRACTICE_CODE ,SORCE_TYPE, SORCE_NAME ,WORK_STATUS ,RECEIVE_DATE  
-- ,TOTAL_PAGES, NO_OF_SPLITS ,FILE_PATH ,ASSIGNED_TO ,ASSIGNED_BY,  
--  ASSIGNED_DATE ,DELETED, PATIENT_ACCOUNT ,INDEXED_BY ,INDEXED_DATE into #temp from FOX_TBL_WORK_QUEUE where UNIQUE_ID  =@UNIQUE_ID   
               
-- --INSERT INTO FOX_TBL_WORK_QUEUE (UNIQUE_ID,WORK_ID, PRACTICE_CODE ,SORCE_TYPE, SORCE_NAME ,WORK_STATUS ,RECEIVE_DATE  
-- --,TOTAL_PAGES, NO_OF_SPLITS ,FILE_PATH ,ASSIGNED_TO ,ASSIGNED_BY,  
-- -- ASSIGNED_DATE ,DELETED, PATIENT_ACCOUNT ,INDEXED_BY ,INDEXED_DATE )             
   
----   (select UNIQUE_ID,(select * from #tempWorkID) as work_Id,  
----PRACTICE_CODE ,SORCE_TYPE, SORCE_NAME ,WORK_STATUS ,RECEIVE_DATE  
---- ,TOTAL_PAGES, NO_OF_SPLITS ,FILE_PATH ,ASSIGNED_TO ,ASSIGNED_BY,  
----  ASSIGNED_DATE ,DELETED, PATIENT_ACCOUNT ,INDEXED_BY ,INDEXED_DATE   
---- from    #temp)  
  
    
--  insert into  FOX_TBL_WORK_QUEUE_File_All (WORK_ID,UNIQUE_ID,FILE_PATH,FILE_PATH1,deleted) values (@NEW_WORK_ID, @like_id,@File_Path,@File_Path_1,'0')  
  
  
--  ----------------  
--  if CHARINDEX('_',@like_id) > 0  
--  begin  
--  set @like_id=(SELECT LEFT(@like_id, CHARINDEX('_',@like_id)-1) )  
--  update FOX_TBL_WORK_QUEUE set NO_OF_SPLITS=(select count(distinct UNIQUE_ID)  from FOX_TBL_WORK_QUEUE_File_All where  ( Replace(UNIQUE_ID, '_', '@') like ''+@like_id+'@%' OR UNIQUE_ID = @like_id) and isnull(deleted,0)=0)     
--  where   
--   Replace(UNIQUE_ID, '_', '@') like ''+@like_id+'@%' OR UNIQUE_ID = @like_id  
--  End  
    
          
-- SET @Id = SCOPE_IDENTITY();            
-- IF(@Id) != 0            
--     BEGIN            
--      SET @STATUS = 'FAILED';       
--     END            
-- ELSE            
--     BEGIN            
--  SELECT @STATUS = 'Successful';            
--  END            
--  SELECT @STATUS AS STATUS            
--END        
     
  
  
   as begin  
  
 --  DECLARE @UNIQUE_ID VARCHAR(50)='5447908_1'  
  
   DECLARE @ORIGINAL_WORK_ID BIGINT=0  
   IF(CHARINDEX('_',@UNIQUE_ID)>0)  
   BEGIN  
   SET @ORIGINAL_WORK_ID = CONVERT(BIGINT,SUBSTRING(@UNIQUE_ID,0,CHARINDEX('_',@UNIQUE_ID)))  
   END  
   ELSE  
   BEGIN  
    SET @ORIGINAL_WORK_ID = CONVERT(BIGINT,@UNIQUE_ID)  
   END  
  
  
   UPDATE FOX_TBL_WORK_QUEUE_FILE_ALL SET DELETED=1 WHERE WORK_ID=@OLD_WORK_ID AND FILE_PATH=@FILE_PATH  
  
   UPDATE FOX_TBL_WORK_QUEUE SET NO_OF_SPLITS = NO_OF_SPLITS + 1 WHERE WORK_ID = @OLD_WORK_ID   
  
   IF(@ORIGINAL_WORK_ID!=@OLD_WORK_ID)  
   BEGIN  
   UPDATE FOX_TBL_WORK_QUEUE SET NO_OF_SPLITS = NO_OF_SPLITS + 1 WHERE WORK_ID = @ORIGINAL_WORK_ID   
   END  
  
   insert into FOX_TBL_WORK_QUEUE_File_All(FILE_ID, UNIQUE_ID,FILE_PATH,FILE_PATH1,deleted,WORK_ID)  
   values(@FILE_ID,@UNIQUE_ID,@File_path,@File_Path_1,0,@NEW_WORK_ID)  
  
  
  
   select 'success'  
   end  
  
  
  
  
