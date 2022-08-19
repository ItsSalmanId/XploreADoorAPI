IF (OBJECT_ID('FOX_PROC_GET_ADVANCED_EXPORT_REPORT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ADVANCED_EXPORT_REPORT
GO 
---- =============================================                                                  
---- Author:  <Muhammad Arslan Tufail>                                                  
---- Create date: <29/09/2020>                                                  
---- Description: <Get Advanced Search Daily Report of PHD>                                                  
---- =============================================                                    
---- EXEC FOX_PROC_GET_ADVANCED_EXPORT_REPORT '1012714','605101'      
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ADVANCED_EXPORT_REPORT]                                    
@PRACTICE_CODE BIGINT,                  
@CALL_ATTENDED_BY VARCHAR(MAX)                       
AS                                    
BEGIN                                    
 SELECT CONVERT(VARCHAR, PCD.CALL_DATE, 101) AS CALL_DATE_STR,                   
 PCD.PATIENT_ACCOUNT,                   
 PCR.NAME,                   
 PCD.CALL_DETAILS,                   
 PCD.AMOUNT                
 FROM FOX_TBL_PHD_CALL_DETAILS AS PCD                  
 INNER JOIN FOX_TBL_PHD_CALL_REASON AS PCR ON PCR.PHD_CALL_REASON_ID = PCD.CALL_REASON                
 WHERE ISNULL(PCD.DELETED,0)=0                   
 AND PCD.CALL_ATTENDED_BY IN (select val from dbo.f_split(@CALL_ATTENDED_BY, ','))                  
 AND PCD.PRACTICE_CODE = @PRACTICE_CODE                
 AND CAST(PCD.CREATED_DATE AS DATE) = CAST(GETDATE() AS DATE)             
 AND ISNULL(PCD.DELETED, 0) = 0              
 ORDER BY PCD.CREATED_DATE DESC                
END; 