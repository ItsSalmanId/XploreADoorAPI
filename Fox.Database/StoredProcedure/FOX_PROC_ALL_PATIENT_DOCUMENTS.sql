IF (OBJECT_ID('FOX_PROC_ALL_PATIENT_DOCUMENTS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_ALL_PATIENT_DOCUMENTS
GO 
  ----CREATED BY: Usman Nasir----    
 --[FOX_PROC_ALL_PATIENT_DOCUMENTS] '101116354813700','1011163'  

CREATE PROCEDURE [dbo].[FOX_PROC_ALL_PATIENT_DOCUMENTS] --'101116354410152','1011163' ,''    
  (    
  @PATIENT_ACCOUNT AS VARCHAR(100),    
  @PRACTICE_CODE  AS VARCHAR(100)                 
  )    
 AS    
     
   SELECT     
  p.PATIENT_ACCOUNT    
  ,wq.WORK_ID    
  ,wq.UNIQUE_ID    
  ,wqfa.FILE_ID    
  ,wq.SORCE_NAME AS EMAIL_ADDRESS    
   ,dt.DOCUMENT_TYPE_ID
  ,ISNULL(dt.NAME,'') DOCUMENT_TYPE  
  --,CASE wq.DOCUMENT_TYPE    
  -- WHEN 1    
  --  THEN 'POC'    
  -- WHEN 2    
  --  THEN 'REFERRAL ORDER'    
  -- ELSE 'OTHER'    
  -- END AS DOCUMENT_TYPE    
    
  ,wq.WORK_STATUS    
  ,wqfa.FILE_PATH AS src    
  ,wqfa.FILE_PATH1 AS href    
  ,wq.ASSIGNED_TO    
  ,wq.DEPARTMENT_ID    
  ,wq.RECEIVE_DATE    
  ,wq.CREATED_BY     
  ,wq.CREATED_DATE    
  ,S.LAST_NAME + ',' + S.FIRST_NAME AS SENDER_NAME    
  ,wq.MODIFIED_BY     
  ,wq.MODIFIED_DATE    
  ,    
  CASE    
      WHEN isnull(U.LAST_NAME, '') <> ''    
      THEN U.LAST_NAME+(CASE    
           WHEN isnull(U.FIRST_NAME, '') <> ''    
           THEN ', '+U.FIRST_NAME    
           ELSE ''    
          END)    
      ELSE CASE    
         WHEN isnull(U.FIRST_NAME, '') <> ''    
         THEN U.FIRST_NAME    
         ELSE ''    
        END    
     END as Template    
          
   FROM Patient AS p    
   --INNER JOIN FOX_TBL_PATIENT AS ftp ON ftp.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT    
   --         AND ftp.DELETED <> 1    
   INNER JOIN FOX_TBL_WORK_QUEUE AS wq ON wq.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT    
            AND ISNULL(wq.DELETED,0  )=0  
            AND wq.PRACTICE_CODE = @PRACTICE_CODE    
       
   inner JOIN FOX_TBL_WORK_QUEUE_FILE_ALL AS wqfa ON wqfa.WORK_ID = wq.WORK_ID    
           AND  ISNULL(wqfa.DELETED,0  )=0  
   LEFT JOIN FOX_TBL_APPLICATION_USER AS U ON U.USER_NAME = wq.ASSIGNED_TO    
         AND ISNULL(U.DELETED,0  )=0  
   left JOIN FOX_TBL_ORDERING_REF_SOURCE AS S ON wq.SENDER_ID = S.SOURCE_ID  AND ISNULL(S.DELETED, 0) = 0 
       LEFT JOIN FOX_TBL_DOCUMENT_TYPE AS dt ON dt.DOCUMENT_TYPE_ID = wq.DOCUMENT_TYPE AND ISNULL(dt.DELETED, 0) = 0   
 WHERE p.PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
    AND  ISNULL(p.DELETED,0  )=0  
    AND p.PRACTICE_CODE = @PRACTICE_CODE    



 /********************************************************************************************************************************************************************************************************/


