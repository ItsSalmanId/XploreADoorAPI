-- AUTHOR:  <DEVELOPER, MUHAMMAD SALMAN>
-- CREATE DATE: <CREATE DATE, 10/29/2022>   
-- MODIFY DATE: <CREATE DATE, 10/29/2022>   
-- DESCRIPTION: <FOX_GET_INDEX_ALL_INFO>  
-- [dbo].[FOX_GET_INDEX_ALL_INFO] 54815326                                      
                                              
ALTER PROCEDURE [dbo].[FOX_GET_INDEX_ALL_INFO]                                                              
@WORK_ID BIGINT                                                
--DECLARE @WORK_ID BIGINT= 5486941;                                                              
AS                                   
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED                                        
     BEGIN                                                
         SELECT wq.WORK_ID,                
          FTA.IS_ACQUISITION AS IS_ACQUISITION,                                       
                wq.UNIQUE_ID,                                                 
                wq.DOCUMENT_TYPE,                                                 
                s.SOURCE_ID AS SENDER_ID,                                                 
                (s.FIRST_NAME+' '+s.LAST_NAME) AS SENDER_NAME,                                                 
                LOWER(wq.SORCE_NAME) AS SORCE_NAME,                                                 
                SORCE_TYPE,                                                 
                FACILITY_NAME,                                                 
                FACILITY_ID,                                                 
                wq.DEPARTMENT_ID,                                                 
                IS_EMERGENCY_ORDER,                                                 
                REASON_FOR_VISIT,                                                 
                ACCOUNT_NUMBER,                                                 
                UNIT_CASE_NO,                                                 
                pt.SSN,                                                 
                pt.Chart_Id,                                                 
                wq.PATIENT_ACCOUNT AS PATIENT_ACCOUNT,                                                  
                wq.WORK_STATUS,                                                
                CASE                                                
                    WHEN pt.First_Name IS NULL                                                
                    THEN wtp.First_Name                                                
                    ELSE pt.First_Name                                                
                END AS First_Name,                                                
                CASE                                                
                    WHEN pt.Last_Name IS NULL                                                
                    THEN wtp.Last_Name                                                
                    ELSE pt.Last_Name                                                
                END AS Last_Name,                                                 
                pt.MIDDLE_NAME AS Middle_Name,                                                
                CASE                                                
                    WHEN pt.Date_Of_Birth IS NULL                                                
                    THEN CONVERT(VARCHAR, wtp.DOB, 101)                                                
                    ELSE CONVERT(VARCHAR, pt.Date_Of_Birth, 101)                                                
                END AS Date_Of_Birth,                                                
                CASE                                                
                    WHEN wtp.FOX_Temp_Patient_ID IS NOT NULL                                                
                         AND pt.Patient_Account IS NULL                                                
                    THEN CONVERT(BIT, 1)                                                
                    ELSE CONVERT(BIT, 0)                                     
                END AS Is_Temp_Patient,                   
                pt.Gender,                                              
    pt.Email_Address,                                              
                pt.cell_phone AS Cell_Phone,                                                 
        pt.Business_Phone AS Business_Phone,                                                 
                pt.Home_Phone AS Home_Phone,                                                 
                wq.FOX_TBL_SENDER_TYPE_ID,                                                 
                wq.FOX_TBL_SENDER_NAME_ID,                               
                wq.REASON_FOR_THE_URGENCY,                                                 
    wq.IS_POST_ACUTE,                 
    CONVERT(VARCHAR, wq.Expected_Discharge_Date, 101) AS Expected_Discharge_Date,                                           
                wq.CREATED_BY,                                                 
          wq.CREATED_DATE,                                                 
                wq.MODIFIED_BY,                                                 
               wq.MODIFIED_DATE,                                                 
                wq.IS_EVALUATE_TREAT,                        
                wq.HEALTH_NAME,                                                 
                wq.HEALTH_NUMBER,                                                 
               wq.IS_VERBAL_ORDER,                                                 
                wq.VO_ON_BEHALF_OF,                                                 
                wq.VO_RECIEVED_BY,                                                 
                wq.VO_DATE_TIME,                                              
             wq.RFO_Type,                                                 
                isnull(wq.FILE_PATH,'') AS Zip_File_Path,                                              
                ftp.Title,                                
                ftp.Fax_Number,                                               
    FC.FINANCIAL_CLASS_ID,                                              
    FC.NAME AS FINANCIAL_CLASS_NAME,                                  
 CONVERT(BIT,                                                
     CASE                                                  
    WHEN  ISNULL(ftis.IS_SYNCED, 0) = 0                                               
     THEN 0                                                  
     ELSE 1                                                  
    END) AS IS_SYNC,                                            
                                         
    CONVERT(BIT,                                                
     CASE                                                  
    WHEN  INL.IS_ERROR= 1                                                 
     THEN 1                                                  
     ELSE 0                                                  
    END) AS IS_Error,                                              
 (ISNULL(INL.ERROR,'')) AS ERROR_MSG  ,                                      
                CONVERT(BIT, ISNULL(ftis.IS_SYNCED, 0)) AS IS_WORK_ORDER_INTERFACE_SYNCED,                                                 
--CONVERT(BIT, ISNULL(ftis_p.IS_SYNCED, 0)) AS IS_PATIENT_INTERFACE_SYNCED,                                              
                CASE WHEN ISNULL(ftis_p.IS_SYNCED, 0) = 1 OR (pt.Chart_Id IS NOT NULL AND pt.Chart_Id <> '' ) THEN  CONVERT(BIT,1) ELSE CONVERT(BIT,0) END AS IS_PATIENT_INTERFACE_SYNCED,                                              
                CASE WHEN ISNULL(ftis_p.IS_SYNCED, 0) = 1 OR (pt.Chart_Id IS NOT NULL AND pt.Chart_Id <> '' ) THEN  CONVERT(BIT,1) ELSE CONVERT(BIT,0) END AS IS_PATIENT_OLD_OR_SYNCED,                                              
    (SELECT SUM(amt_due) FROM claims WHERE patient_account = pt.PATIENT_ACCOUNT AND isnull(claims.deleted, 0) = 0 AND pat_status IN('N', 'R', 'B', 'C', 'D')) AS Patient_Balance,                                              
         CONVERT(BIT,                                              
      CASE                                           
     WHEN  ISNULL(RS.FOR_STRATEGIC, 0) = 1                                              
    THEN 1                                  
 WHEN  ISNULL(FC.CODE, '') = 'SA'                                              
    THEN 1                             
    ELSE 0                           
    END) AS IS_STRATEGIC,                                             
 --CONVERT(BIT,                                              
 --     CASE                                               
 --   WHEN  ISNULL(wq.IS_OCR, 0) = 1                                           
 --   THEN 1                                              
 --   ELSE 0                                              
 --   END) AS IS_OCR                                            
 wq.OCR_STATUS_ID,             
 OC.OCR_STATUS AS OCR_STATUS,                        
 wq.FOX_SOURCE_CATEGORY_ID,                    
   CONVERT(BIT,                    
 CASE                     
 WHEN OC.OCR_STATUS = 'OCR performed'                     
 THEN 1                    
 ELSE 0                    
 END  )                  
   AS IS_OCR                                       
   FROM FOX_TBL_WORK_QUEUE AS wq             
              LEFT JOIN  FOX_TBL_ACQUISITION_PATIENT AS FTA with (nolock) ON FTA.Patient_Account = wq.PATIENT_ACCOUNT AND ISNULL(FTA.DELETED, 0)=0                                                     
              LEFT JOIN Patient AS pt with (nolock) ON wq.PATIENT_ACCOUNT = pt.Patient_Account             
                                              
           LEFT JOIN FOX_TBL_INTERFACE_SYNCH INS with (nolock) ON INS.WORK_ID = wq.WORK_ID   AND INS.CREATED_DATE = (SELECT MAX(CREATED_DATE) FROM FOX_TBL_INTERFACE_SYNCH WHERE WORK_ID = wq.WORK_ID)                                              
     LEFT JOIN FOX_TBL_INTERFACE_LOG INL with (nolock) on INL.FOX_INTERFACE_SYNCH_ID=INS.FOX_INTERFACE_SYNCH_ID                                            
            AND ISNULL(pt.Deleted, 0) = 0                               
              LEFT JOIN WS_TBL_FOX_Temp_Patient AS wtp with (nolock) ON wtp.work_id = wq.work_id                                                
             LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE AS s with (nolock) ON s.SOURCE_ID = wq.SENDER_ID                                                
              LEFT JOIN dbo.FOX_TBL_PATIENT AS ftp with (nolock) ON pt.Patient_Account = ftp.Patient_Account                                                
              LEFT JOIN FOX_TBL_INTERFACE_SYNCH AS ftis with (nolock) ON @WORK_ID IS NOT NULL                                                
                                                           AND ftis.PATIENT_ACCOUNT = pt.PATIENT_ACCOUNT                                                
                                                           AND ftis.Work_ID = wq.WORK_ID                                                
                                                           AND ISNULL(ftis.DELETED, 0) = 0                                                
                                                          AND ftis.PRACTICE_CODE = wq.PRACTICE_CODE                                                
                                                           AND wq.WORK_STATUS = 'Completed'                                                
    --LEFT JOIN FOX_TBL_INTERFACE_SYNCH AS ftis_p ON ftis_p.PATIENT_ACCOUNT = pt.PATIENT_ACCOUNT                                                
              --                                               AND ISNULL(ftis_p.DELETED, 0) = 0                                                
              --                                          AND ftis_p.PRACTICE_CODE = wq.PRACTICE_CODE                                                
              --                                               AND ISNULL(ftis_p.IS_SYNCED, 0) = 1                                           
     LEFT JOIN FOX_TBL_OCR_STATUS OC with (nolock) ON OC.OCR_STATUS_ID = wq.OCR_STATUS_ID                                          
    AND ISNULL(OC.DELETED, 0) = 0                       
              LEFT JOIN FOX_TBL_INTERFACE_SYNCH AS ftis_p with (nolock) ON ftis_p.FOX_INTERFACE_SYNCH_ID =                                                
         (                                                
             SELECT TOP 1 FOX_INTERFACE_SYNCH_ID                                    
             FROM FOX_TBL_INTERFACE_SYNCH AS ftis_p_i                                                
             WHERE ftis_p_i.PATIENT_ACCOUNT = pt.PATIENT_ACCOUNT                                                
                   AND ISNULL(ftis_p_i.DELETED, 0) = 0                                              
                   AND ftis_p_i.PRACTICE_CODE = wq.PRACTICE_CODE                                                
                   AND ISNULL(ftis_p_i.IS_SYNCED, 0) = 1                                                
         )  LEFT JOIN FOX_TBL_FINANCIAL_CLASS AS FC with (nolock) ON ftp.FINANCIAL_CLASS_ID = FC.FINANCIAL_CLASS_ID                                              
              AND pt.Practice_Code = FC.PRACTICE_CODE                                              
              AND ISNULL(FC.DELETED, 0) = 0                                              
   LEFT JOIN FOX_TBL_REFERRAL_SENDER RS ON RS.SENDER = wq.SORCE_NAME                                              
                                               and ISNULL(RS.FOR_STRATEGIC, 0) = 1                                              
                    AND ISNULL(RS.DELETED, 0) = 0                                             
                                           
         WHERE wq.work_id = @WORK_ID                                                
               AND isnull(wq.DELETED, 0) = 0;                                                
     END; 