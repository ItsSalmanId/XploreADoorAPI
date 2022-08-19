IF (OBJECT_ID('FOX_PROC_GET_SSCM_CASE_DETAILS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_SSCM_CASE_DETAILS  
GO 
-------------------------------------------------------------------------------------------------- FOX_PROC_GET_SSCM_CASE_DETAILS ---------------------------------------------------------------------------------------------   
 ---- =============================================                                      
---- Author:  <Muhammad Arslan Tufail>                                      
---- Create date: <12/09/2020>                                      
---- Description: <Get Case Details>                                      
---- =============================================                        
-- EXEC [FOX_PROC_GET_SSCM_CASE_DETAILS] 1012714                                         
CREATE PROCEDURE [dbo].[FOX_PROC_GET_SSCM_CASE_DETAILS]                        
@PRACTICE_CODE BIGINT            
AS                        
BEGIN                        
 SELECT CSI.CS_CASE_NO,Priority_Level,CCP.CS_Case_Status,  
 CASE when CCP.CS_Rectified_Case_Type = '2' THEN 'Routine' end as CS_Rectified_Case_Type       
 FROM             
 CS_Customer_Support_Info AS CSI            
 INNER JOIN CS_Case_Progress AS CCP ON CCP.CS_CASE_NO = CSI.CS_Case_No            
 INNER JOIN FOX_TBL_PHD_CALL_DETAILS AS PCD ON PCD.SSCM_CASE_ID = CSI.CS_Case_No          
 WHERE CCP.CS_Practice_Code = @PRACTICE_CODE AND PCD.DELETED = 0         
 ORDER BY CSI.CS_CASE_NO DESC            
END; 