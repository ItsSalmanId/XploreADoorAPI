IF (OBJECT_ID('FOX_PROC_GET_PATIENT_UPDATE_HISTORY_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_UPDATE_HISTORY_LIST  
GO 
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure [dbo].[FOX_PROC_GET_PATIENT_UPDATE_HISTORY_LIST]
  @patientAccount bigint,
  @pageSize int
  as 
  begin
	select top(@pageSize) pl.*,  (au.FIRST_NAME + ' ' + au.LAST_NAME) as USER_NAME
	from FOX_TBL_PATIENT_UPDATE_HISTORY pl join 
		 FOX_TBL_APPLICATION_USER au on pl.updated_by = au.USER_NAME
	where PATIENT_ACCOUNT = @patientAccount
  end


