IF (OBJECT_ID('FOX_PROC_GET_Duplicate_Referral') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_Duplicate_Referral 
GO  
----------------------------------------------------------------------------------------------------------    
----------------------------- SP_HELPTEXT [FOX_PROC_GET_Duplicate_Referral] --------------------   
---- =============================================                                  
---- Author:  <Muhammad Arslan Tufail>                                  
---- Create date: <08/01/2020>                                  
---- Description: <Return Duplicate Order Referrals>                                  
---- =============================================                    
--exec [FOX_PROC_GET_Duplicate_Referral] 101116354815744, 1011163, 54815474                  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_Duplicate_Referral]                    
@PATIENT_ACCOUNT BIGINT,          
@PRACTICE_CODE BIGINT,      
@ORDER_ID BIGINT      
--@SPLITED_IDS VARCHAR(MAX)      
AS                    
BEGIN                    
 SELECT          
 WQ.WORK_ID,      
 WQ.UNIQUE_ID,           
 ISNULL(dt.NAME,'') DOCUMENT_TYPE,           
 WQ.WORK_STATUS,           
 S.LAST_NAME+','+S.FIRST_NAME AS ORS,              
 WQ.SORCE_NAME AS SENDER_SOURCE,              
 CONVERT(VARCHAR(20), WQ.RECEIVE_DATE, 101) AS RECEIVE_DATE,      
 WQ.DEPARTMENT_ID                     
 FROM FOX_TBL_WORK_QUEUE AS WQ          
 LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE AS S ON WQ.SENDER_ID = S.SOURCE_ID AND ISNULL(S.DELETED, 0) = 0          
 LEFT JOIN FOX_TBL_DOCUMENT_TYPE AS DT ON DT.DOCUMENT_TYPE_ID = WQ.DOCUMENT_TYPE  AND ISNULL(dt.DELETED, 0) = 0             
 WHERE CONVERT(DATE, WQ.CREATED_DATE, 101) = CONVERT(DATE, GETDATE(), 101) AND WQ.PATIENT_ACCOUNT = @PATIENT_ACCOUNT          
 AND ISNULL(WQ.DELETED, 0) = 0 AND WQ.PRACTICE_CODE = @PRACTICE_CODE AND WQ.WORK_ID != @ORDER_ID --AND WQ.WORK_STATUS ='Completed'       
 AND (DT.NAME = 'Forms' OR DT.NAME = 'Insurance Card' OR DT.NAME = 'Signed Order' OR DT.NAME = 'UnSigned Order') --AND WQ.DEPARTMENT_ID IN (select val from dbo.f_split(@SPLITED_IDS, ','))        
END; 