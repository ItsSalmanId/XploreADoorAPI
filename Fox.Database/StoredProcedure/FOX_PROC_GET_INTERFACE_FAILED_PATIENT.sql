IF (OBJECT_ID('FOX_PROC_GET_INTERFACE_FAILED_PATIENT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INTERFACE_FAILED_PATIENT  
GO  
-- =============================================        
-- AUTHOR:  <ABDUR RAFAY>        
-- CREATE DATE: <28/1/2019>        
-- DESCRIPTION: <GET INTERFACE FAILED PATIENT LIST>        
-- [FOX_PROC_GET_INTERFACE_FAILED_PATIENT] '1012714','ADMIN_5651352'      
--CREATE PROCEDURE [DBO].[FOX_PROC_GET_INTERFACE_FAILED_PATIENT] --'1011163','1163TESTING'      
  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_INTERFACE_FAILED_PATIENT] --[FOX_PROC_GET_INTERFACE_FAILED_PATIENT]'1011163','1163testing'      
@PRACTICE_CODE BIGINT,   
@CURRENT_USER  VARCHAR(70)     
AS  
--DECLARE @PRACTICE_CODE BIGINT = 1012714,         
--@CURRENT_USER  VARCHAR(70) = 'ELINOR_552458'       
BEGIN  
WITH CTE AS  
(  
 SELECT DISTINCT   
 P.LAST_NAME,   
 P.FIRST_NAME,   
 P.PATIENT_ACCOUNT,  
 FTIL.MODIFIED_DATE,  
 ROW_NUMBER() OVER (PARTITION BY P.PATIENT_ACCOUNT ORDER BY FTIL.MODIFIED_DATE DESC) AS RN   
FROM PATIENT AS P  
 JOIN FOX_TBL_INTERFACE_LOG AS FTIL ON FTIL.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
 AND ISNULL(FTIL.DELETED, 0) = 0  
 AND FTIL.PRACTICE_CODE = @PRACTICE_CODE  
 AND FTIL.IS_ERROR = 1  
 AND FTIL.ERROR <> 'Didn`t Receive ACK from RT.'  
 AND FTIL.IS_OUTGOING = 1   
 JOIN FOX_TBL_WORK_QUEUE AS FTWQ ON FTWQ.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT  
 AND ISNULL(FTWQ.DELETED, 0) = 0  
 AND FTWQ.PRACTICE_CODE = @PRACTICE_CODE  
 --JOIN FOX_TBL_INTERFACE_SYNCH AS FTIS ON FTIS.WORK_ID = FTWQ.WORK_ID  
 --AND ISNULL(FTIS.DELETED, 0) = 0  
 --AND FTIS.PRACTICE_CODE = @PRACTICE_CODE  
WHERE ISNULL(P.DELETED, 0) = 0  
AND P.PRACTICE_CODE = @PRACTICE_CODE  
AND  
 (  
 (SELECT COUNT (USER_NAME) FROM FOX_TBL_APPLICATION_USER U JOIN FOX_TBL_ROLE R ON U.ROLE_ID = R.ROLE_ID   
 WHERE USER_NAME LIKE @CURRENT_USER   
 AND R.ROLE_NAME LIKE 'ADMINISTRATOR'   
 AND ISNULL(U.DELETED,0) = 0  
 AND ISNULL(R.DELETED,0) = 0  
 ) > 0  
 OR FTWQ.COMPLETED_BY = @CURRENT_USER  
 )  
AND   
 (  
 SELECT COUNT(FOX_INTERFACE_SYNCH_ID)    
 FROM FOX_TBL_INTERFACE_SYNCH   
 WHERE PATIENT_ACCOUNT = P.PATIENT_ACCOUNT   
 AND DELETED = 0   
 AND IS_SYNCED = 0 ) > 0   
 )  
SELECT LAST_NAME,FIRST_NAME,PATIENT_ACCOUNT FROM CTE WHERE RN = 1 ORDER BY MODIFIED_DATE DESC  
END; 