IF (OBJECT_ID('FOX_GET_WORK_ORDER_INFO_FOR_PAT_MAINTENANCE') IS NOT NULL ) DROP PROCEDURE FOX_GET_WORK_ORDER_INFO_FOR_PAT_MAINTENANCE  
GO 
---1 
--FOX_GET_WORK_ORDER_INFO_FOR_PAT_MAINTENANCE_TEST 9090999534105008,1011163    
CREATE PROCEDURE [dbo].[FOX_GET_WORK_ORDER_INFO_FOR_PAT_MAINTENANCE] @PATIENT_ACCOUNT BIGINT,   
                                                            @PRACTICE_CODE   BIGINT  
AS  
     BEGIN
	 
	 
	   
         SELECT DISTINCT   
                W.WORK_ID,   
                W.UNIQUE_ID,   
                W.SORCE_NAME AS EMAIL_ADDRESS,   
                 c.CASE_NO, 
				c.RT_CASE_NO,  
                CONVERT(VARCHAR(20), W.RECEIVE_DATE, 101) AS RECEIVE_DATE,  
				ISNULL(dt.NAME,'') DOCUMENT_TYPE,
				dt.DOCUMENT_TYPE_ID,

                --CASE W.DOCUMENT_TYPE  
                --    WHEN 1  
                --    THEN 'POC'  
                --    WHEN 2  
                --    THEN 'REFERRAL ORDER'  
                --    ELSE 'OTHER'  
                --END AS DOCUMENT_TYPE,   
                S.LAST_NAME+','+S.FIRST_NAME AS SENDER_NAME,   
                W.IS_EVALUATE_TREAT,   
                W.HEALTH_NAME,   
                W.HEALTH_NUMBER,   
                W.UNIT_CASE_NO  
         FROM FOX_TBL_WORK_QUEUE AS W  
              LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE AS S ON W.SENDER_ID = S.SOURCE_ID AND ISNULL(S.DELETED, 0) = 0    
              LEFT JOIN  
    (  
     SELECT WORK_ID, RT_CASE_NO,  
      CASE_NO = STUFF(  (   SELECT DISTINCT   ', '+CASE_NO   FROM FOX_TBL_CASE  
      WHERE WORK_ID = t.WORK_ID FOR XML PATH('')  ), 1, 1, '') FROM FOX_TBL_CASE AS t  
     GROUP BY WORK_ID ,RT_CASE_NO 
    ) AS c ON c.WORK_ID = W.WORK_ID  

	LEFT JOIN FOX_TBL_DOCUMENT_TYPE AS dt ON dt.DOCUMENT_TYPE_ID = W.DOCUMENT_TYPE  AND ISNULL(dt.DELETED, 0) = 0

         WHERE W.PATIENT_ACCOUNT = @PATIENT_ACCOUNT  
               AND W.PRACTICE_CODE = @PRACTICE_CODE  
               AND ISNULL(W.DELETED, 0) = 0
			  
     END;

