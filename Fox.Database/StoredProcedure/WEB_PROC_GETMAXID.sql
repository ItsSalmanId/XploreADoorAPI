IF (OBJECT_ID('Web_Proc_GetMaxID') IS NOT NULL ) DROP PROCEDURE Web_Proc_GetMaxID  
GO 
-- =============================================          
-- Author:  <Abdur Rehman & ARSHAD MAHMOOD>          
-- Create date: <05/04/2009>          
-- Description: <GET MAX COLUMN ID by providing table and column name>          
-- Modified By/Date: Arshad Mahmood on 01/23/2014
-- Modification Description: Added practice code check only for patient and appointments tables to enhance efficiency of procedure
-- Web_Proc_GetMaxID 'Patient','Patient_account','9090999'

-- =============================================          
CREATE procedure [dbo].[Web_Proc_GetMaxID]          
	-- Add the parameters for the stored procedure here          
	@TableName varchar(100),       
	@colName varchar(100),      
	@Practice_code varchar(16)          
AS          
BEGIN          
	-- SET NOCOUNT ON added to prevent extra result sets from          
	-- interfering with SELECT statements.          
	DECLARE @MaxID   bigint;                
	SET NOCOUNT ON;          
	-- Insert statements for procedure here          
	Declare @Office_id varchar(10)      
	select @Office_id = office_id from Maintenance      
	Declare  @Query nvarchar(500)      
	if(@TableName = 'PATIENT' or @TableName = 'Appointments' )
	begin
		set @Query = 'select convert(varchar,'+ @Practice_code +''+ @Office_id + ')' + '+convert(varchar,convert(bigint,substring(convert(varchar,MaxColumnID),len('+ @Practice_code +''+ @Office_id +')+1,len(convert(varchar,MaxColumnID)))) + 1) as MaxColumnID 
		from (Select isNull(MAX(convert(bigint,' + @colName + ')),'+ @Practice_code +''+ @Office_id + '10000) as MaxColumnID From '+ @TableName +' Where practice_code='+@Practice_code+' and '+ @colName +' Like '''+ @Practice_code + @Office_id + '%'') a'      
	end
	else
	begin
		set @Query = 'select convert(varchar,'+ @Practice_code +''+ @Office_id + ')' + '+convert(varchar,convert(bigint,substring(convert(varchar,MaxColumnID),len('+ @Practice_code +''+ @Office_id +')+1,len(convert(varchar,MaxColumnID)))) + 1) as MaxColumnID 
		from (Select isNull(MAX(convert(bigint,' + @colName + ')),'+ @Practice_code +''+ @Office_id + '10000) as MaxColumnID From '+ @TableName +' Where '+ @colName +' Like '''+ @Practice_code + @Office_id + '%'') a'    
	end
	--print @Query    
	execute sp_executesql @Query      
End 

