IF (OBJECT_ID('FOX_PROC_GET_PHD_CALL_DETAILS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PHD_CALL_DETAILS  
GO               
--[DBO].[FOX_PROC_GET_PHD_CALL_DETAILS] '11/21/2020 12:00:00 AM', '11/21/2021 11:59:59 PM', '', '', '', '', '', '', '1012714','', 1, 100,'CREATEDDATE', 'DESC'                            
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PHD_CALL_DETAILS]-- '09/01/2020 12:00:00 AM', '11/12/2020 11:59:59 PM', '', '', '', '', '', '', '1011163','', 1, 100,'CREATEDDATE', 'DESC'                                                                     
       
 (                                                                                 
  @CALL_DATE_FROM VARCHAR(50)          
 ,@CALL_DATE_TO VARCHAR(50)                                                                      
 ,@CALL_ATTENDED_BY AS VARCHAR(50)                                                                      
 ,@CALL_REASON AS VARCHAR(50)                      
 ,@CALL_HANDLING AS VARCHAR(50)                
 ,@CS_CASE_STATUS AS VARCHAR(50)                    
 ,@FOLLOW_UP_CALLS AS BIT                                                                
 ,@CHART_ID AS VARCHAR(100)        
 ,@PATIENT_FIRST_NAME VARCHAR(50)           
  ,@PATIENT_LAST_NAME VARCHAR(50)           
 ,@PHONE_NUMBER AS VARCHAR(100)                                                                
 ,@PRACTICE_CODE BIGINT                                                                        
 ,@SEARCH_TEXT VARCHAR(30)                                                                             
 ,@CURRENT_PAGE INT                                                                              
 ,@RECORD_PER_PAGE INT                                                                             
 ,@SORT_BY VARCHAR(50)                                                                              
 ,@SORT_ORDER VARCHAR(5)        
                                                                                
 )                                                                              
AS                                                                              
BEGIN                                                                               
  IF (@CALL_DATE_FROM = '')                                                                              
 BEGIN                                                                              
  SET @CALL_DATE_FROM = NULL                                                                              
 END                                                                              
 ELSE                                                                              
 BEGIN                                                                              
  SET @CALL_DATE_FROM = @CALL_DATE_FROM + '%'                                                                              
 END                                                                       
                                                                                
 IF (@CALL_DATE_TO = '')                                                                              
 BEGIN                                                                              
  SET @CALL_DATE_TO = NULL                                                                              
 END                                                                              
 ELSE                                                                              
 BEGIN                                                                              
  SET @CALL_DATE_TO = @CALL_DATE_TO + '%'                                                                              
 END                                                                              
                                                                                    
 IF (@CALL_ATTENDED_BY = '')                                                                              
 BEGIN           
  SET @CALL_ATTENDED_BY = NULL                           
 END             
 ELSE                                                     
 BEGIN             
  SET @CALL_ATTENDED_BY = @CALL_ATTENDED_BY + '%'                                                                              
 END                          
                                                                       
 IF (@CALL_REASON = '')               
 BEGIN                                                                              
  SET @CALL_REASON = NULL                                                                      
 END                                                                              
 ELSE                                                                              
 BEGIN                                                                              
  SET @CALL_REASON = @CALL_REASON + '%'                                                              
 END                                                
                       
 IF (@CALL_HANDLING = '')                      
 BEGIN                      
 SET @CALL_HANDLING = NULL                      
 END                      
 ELSE                      
 BEGIN                      
 SET @CALL_HANDLING = @CALL_HANDLING + '%'                      
 END                       
                 
 IF (@CS_CASE_STATUS = '')                      
 BEGIN                      
 SET @CS_CASE_STATUS = NULL                      
 END                      
 ELSE                      
 BEGIN                      
 SET @CS_CASE_STATUS = @CS_CASE_STATUS + '%'                      
 END                 
                     
 IF(@FOLLOW_UP_CALLS = '')                    
 BEGIN                     
 SET @FOLLOW_UP_CALLS = NULL                    
 END                    
 ELSE                    
 BEGIN                    
 SET @FOLLOW_UP_CALLS = @FOLLOW_UP_CALLS                    
 END                    
                                                                 
 IF (@CHART_ID = '')                                                                              
 BEGIN                                                                              
  SET @CHART_ID = NULL                                      
 END                                                                              
 ELSE                                               
 BEGIN                                                                              
  SET @CHART_ID = @CHART_ID + '%'                                                                              
 END            
           
  IF (@PHONE_NUMBER = '')                                                                              
 BEGIN                                                                              
  SET @PHONE_NUMBER = NULL                                      
 END                                                                              
 ELSE                                               
 BEGIN                                                                              
  SET @PHONE_NUMBER = @PHONE_NUMBER + '%'                                                                              
 END          
         
   IF (@PATIENT_FIRST_NAME = '')                                                                              
 BEGIN                                                                              
  SET @PATIENT_FIRST_NAME = NULL                                      
 END                                                                              
 ELSE                                               
 BEGIN                                                                              
  SET @PATIENT_FIRST_NAME = @PATIENT_FIRST_NAME + '%'                                                                              
 END          
    IF (@PATIENT_LAST_NAME = '')                                                                              
 BEGIN                                                                              
  SET @PATIENT_LAST_NAME = NULL                                      
 END                                                                              
 ELSE                    
 BEGIN                                                                              
  SET @PATIENT_LAST_NAME = @PATIENT_LAST_NAME + '%'                                                                   
 END                                                                      
                                                                      
 IF (@RECORD_PER_PAGE = 0)                                                                      
 BEGIN                                                                              
  SELECT @RECORD_PER_PAGE = COUNT(*)                                     
  FROM FOX_TBL_PHD_CALL_DETAILS                                                                              
 END                                                                              
ELSE                                                                              
 BEGIN                                                                              
  SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                                                                              
 END                                                                    
                                                                              
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                                                              
                                                 
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                                                                     
 DECLARE @TOATL_PAGESUDM FLOAT                                                                              
                                                                      
 SELECT @TOATL_PAGESUDM = COUNT(*)                                                                              
 FROM FOX_TBL_PHD_CALL_DETAILS P                                      
 INNER JOIN Patient Pat ON Pat.Patient_Account = P.PATIENT_ACCOUNT                                                                     
 LEFT JOIN FOX_TBL_PHD_CALL_SCENARIO PCS ON PCS.PHD_CALL_SCENARIO_ID = P.CALL_SCENARIO                                                                      
 LEFT JOIN FOX_TBL_PHD_CALL_REASON PCR ON PCR.PHD_CALL_REASON_ID = P.CALL_REASON                                                                      
 LEFT JOIN FOX_TBL_PHD_CALL_REQUEST PCRQ ON PCRQ.PHD_CALL_REQUEST_ID = P.REQUEST                                                                             
 LEFT JOIN FOX_TBL_APPLICATION_USER U ON U.USER_ID = P.CALL_ATTENDED_BY                                      
 LEFT JOIN FOX_TBL_PATIENT_PAT_DOCUMENT AS PD ON PD.FOX_PHD_CALL_DETAILS_ID = P.FOX_PHD_CALL_DETAILS_ID                                                                      
 LEFT JOIN FOX_TBL_DOCUMENT_TYPE DY ON DY.DOCUMENT_TYPE_ID = PD.DOCUMENT_TYPE                                       
 --LEFT JOIN FOX_TBL_PATIENT_DOCUMENT_FILE_ALL AS PDFA ON PDFA.PAT_DOCUMENT_ID = PD.PAT_DOCUMENT_ID                                                                                                        
 LEFT JOIN CS_Customer_Support_Info AS CCI ON P.SSCM_CASE_ID = CCI.CS_Case_No                  
 LEFT JOIN CS_Case_Categories AS CCC ON CCI.CS_Case_Category = CCC.CS_Category_ID                  
 LEFT JOIN CS_Case_Progress AS CCP ON P.SSCM_CASE_ID = CCP.CS_Case_No               
                                      
 WHERE (P.PRACTICE_CODE = @PRACTICE_CODE)                                                                      
 AND (               
 isnull(P.PATIENT_ACCOUNT, '') LIKE '%' + @SEARCH_TEXT + '%'                                                                      
 OR isnull(P.MRN, '') LIKE '%' + @SEARCH_TEXT + '%'       
 OR CONVERT(VARCHAR(10), P.DOS, 101) LIKE + '%' + @SEARCH_TEXT + '%'                                                                                  
 OR CONVERT(VARCHAR(10), P.CALL_DATE, 101) LIKE + '%' + @SEARCH_TEXT + '%'                                 
 OR convert(VARCHAR(15),CAST(P.CALL_TIME AS TIME),108) LIKE + '%' + @SEARCH_TEXT + '%'                                                                       
 OR P.CALL_DURATION LIKE + '%' + @SEARCH_TEXT + '%'                                                                       
   OR P.CALLER_NAME LIKE + '%' + @SEARCH_TEXT + '%'                                                                              
   OR P.RELATIONSHIP LIKE '%' + @SEARCH_TEXT + '%'                                       
   OR PCS.NAME LIKE '%' + @SEARCH_TEXT + '%'                                                                     
   OR P.AMOUNT LIKE '%' + @SEARCH_TEXT + '%'                                   
   OR P.SSCM_CASE_ID LIKE '%' + @SEARCH_TEXT + '%'                                   
   OR P.FOLLOW_UP_DATE LIKE '%' + @SEARCH_TEXT + '%'                                   
   OR DY.DOCUMENT_TYPE_ID LIKE '%' + @SEARCH_TEXT + '%'                 
   OR CCP.CS_Case_Status LIKE '%' + @SEARCH_TEXT + '%'                                                             
   OR PCR.NAME LIKE '%' + @SEARCH_TEXT + '%'                        
   OR CCC.CS_Category_Name LIKE '%' + @SEARCH_TEXT +'%'                                                                                                                                   
   OR P.CALL_DETAILS LIKE '%' + @SEARCH_TEXT + '%'                                                                              
   OR PCRQ.NAME LIKE '%' + @SEARCH_TEXT + '%'                                                                              
   OR (U.LAST_NAME+', '+ U.FIRST_NAME) LIKE '%' + @SEARCH_TEXT + '%'                                                                              
   OR P.INCOMING_CALL_NO LIKE '%' + @SEARCH_TEXT + '%'                                                                              
   OR P.PATIENT_EMAIL_ADDRESS LIKE '%' + @SEARCH_TEXT + '%'          
   OR Pat.First_Name LIKE '%' + @SEARCH_TEXT + '%'          
   OR Pat.Last_Name LIKE '%'  + @SEARCH_TEXT + '%'                                                                          
   )                                                    
   AND (@CALL_DATE_FROM IS NULL                                                                                    
   OR (CONVERT(DATE, P.CALL_DATE) >= CONVERT(DATE, Replace(@CALL_DATE_FROM, '%', ''), 101)))                                                                
   AND (@CALL_DATE_TO IS NULL                                                                                    
   OR (CONVERT(DATE, P.CALL_DATE) <= CONVERT(DATE, Replace(@CALL_DATE_TO, '%', ''), 101)))                                                                      
   AND (                                                                             
   @CALL_ATTENDED_BY IS NULL                                                                     
   OR P.CALL_ATTENDED_BY LIKE '%' + @CALL_ATTENDED_BY + '%'              
   )                                                                    
   AND (                                                                             
   @CALL_REASON IS NULL                                                                              
   OR P.CALL_REASON LIKE '%' + @CALL_REASON + '%'                                                                              
   )                           
   AND (                      
   @CALL_HANDLING IS NULL                      
   OR P.CALL_SCENARIO LIKE '%' + @CALL_HANDLING + '%'                      
   )                
   AND (                      
   @CS_CASE_STATUS IS NULL                      
   OR CCP.CS_Case_Status LIKE '%' + @CS_CASE_STATUS + '%'                      
   )                           
   AND (        
   @FOLLOW_UP_CALLS IS NULL                    
   OR P.FOLLOW_UP_DATE != ''                        
   )                                                                              
  AND (                                                           
   @CHART_ID IS NULL                                   
   OR P.MRN LIKE '%' + @CHART_ID + '%'                                                                      
   OR P.PATIENT_ACCOUNT LIKE '%' + @CHART_ID + '%'                                                                              
   )          
   AND (                   
   @PHONE_NUMBER IS NULL                                                                              
   OR P.INCOMING_CALL_NO LIKE '%' + @PHONE_NUMBER + '%'                                                                                                                                        
   )            
    AND (                         
   @PATIENT_FIRST_NAME IS NULL                                                                              
   OR pat.First_Name LIKE '%' + @PATIENT_FIRST_NAME + '%'         
   )          
    AND (                                                           
   @PATIENT_LAST_NAME IS NULL                                                                              
   OR pat.Last_Name LIKE '%' + @PATIENT_LAST_NAME + '%'         
   )                                                               
  AND (ISNULL(P.DELETED, 0) != 1)                                                                              
                                                                              
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM                                                                  
                                                          
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)                                                                            
 SELECT *                                                                              
  ,@TOATL_PAGESUDM AS TOTAL_RECORD_PAGES                                                        
  ,@TOTAL_RECORDS TOTAL_RECORDS                                                                              
 FROM (                             
  SELECT                                                                       
    CD.FOX_PHD_CALL_DETAILS_ID                                                                      
   ,CD.PATIENT_ACCOUNT                                                                     
   ,CD.PRACTICE_CODE                                                                             
   ,ISNULL(CD.MRN, '') AS MRN                                                                      
   ,CD.DOS                                                                      
   ,convert(varchar,CD.DOS,101) AS DOS_STR                                                                             
   ,CD.CALL_SCENARIO                                                                      
   ,PCS.NAME AS CALL_SCENARIO_NAME                                                                  
   ,CD.CALL_DATE                                                                      
   ,convert(varchar,CD.CALL_DATE,101) AS CALL_DATE_STR                                                              
   ,CD.CALL_TIME                                                                      
   ,CD.CALL_DURATION                                                                      
   ,convert(VARCHAR(15),CAST(CD.CALL_TIME AS TIME),100) AS CALL_TIME_STR                                                                                 
 ,CD.CALL_REASON                                                                      
   ,PCR.NAME AS CALL_REASON_NAME                                                                      
   ,CD.AMOUNT                                          
   ,CD.PRIORITY                               
   ,CD.FOLLOW_UP_DATE                  
   ,CAST(CCC.CS_Category_ID AS VARCHAR) AS CS_CASE_CATEGORY                  
   ,CAST(CCC.CS_Category_Name AS VARCHAR) AS CS_CASE_CATEGORY_NAME                
   ,CASE WHEN CCP.CS_Case_Status = 'NC' THEN 'New Case'                
   WHEN CCP.CS_Case_Status = 'CL' THEN 'Closed'                
   WHEN CCP.CS_Case_Status = 'IP' THEN 'In Progress'                
   WHEN CCP.CS_Case_Status = 'RE' THEN 'Re-Opened'                
   WHEN CCP.CS_Case_Status = 'PA' THEN 'Paused'                
 END AS CS_Case_Status                
   ,convert(varchar, CD.FOLLOW_UP_DATE, 101) AS FOLLOW_UP_DATE_STR                               
   ,CD.SSCM_CASE_ID                                      
   ,convert(varchar, DY.DOCUMENT_TYPE_ID, 101) AS DOCUMENT_TYPE                                                 
   ,DY.NAME AS DOCUMENT_TYPE_NAME                                      
   --,PDFA.DOCUMENT_PATH AS ATTACHMENT_NAME              
   ,CD.FILE_PATH AS ATTACHMENT_NAME                                                                                 
   ,CD.CALLER_NAME                                                
   ,CD.RELATIONSHIP                                                                              
   ,CD.INCOMING_CALL_NO                                       
   ,CD.PATIENT_EMAIL_ADDRESS                      
   ,CD.REQUEST                                    
   ,PCRQ.NAME AS REQUEST_NAME                                                                                   
   ,CD.CALL_ATTENDED_BY                                                                              
   ,CD.CALL_DETAILS                                                                      
   ,CD.CALL_RECORDING_PATH                                                                    
   ,CD.GENERAL_NOTE_ID                                                                    
   ,CD.CREATED_BY                                                                      
   ,CD.CREATED_DATE                                                                      
   ,CD.MODIFIED_BY                                                                            
   ,CD.MODIFIED_DATE                                                                         
   ,CD.DELETED          
   ,Pat.First_Name          
   ,Pat.Last_Name                                                                       
   , U.FIRST_NAME + ' ' + U.LAST_NAME AS CALL_ATTENDED_BY_NAME                                                                      
   ,ROW_NUMBER() OVER (                                                                              
    ORDER BY CD.CREATED_DATE ASC                                                                              
    ) AS ACTIVEROW                                                                              
  FROM FOX_TBL_PHD_CALL_DETAILS CD                                                                              
 INNER JOIN Patient Pat ON Pat.Patient_Account = CD.PATIENT_ACCOUNT                                                          
 LEFT JOIN FOX_TBL_PHD_CALL_SCENARIO PCS ON PCS.PHD_CALL_SCENARIO_ID = CD.CALL_SCENARIO                                                                      
 LEFT JOIN FOX_TBL_PHD_CALL_REASON PCR ON PCR.PHD_CALL_REASON_ID = CD.CALL_REASON                                                                      
 LEFT JOIN FOX_TBL_PHD_CALL_REQUEST PCRQ ON PCRQ.PHD_CALL_REQUEST_ID = CD.REQUEST                                                     
 LEFT JOIN FOX_TBL_APPLICATION_USER U ON U.USER_ID = CD.CALL_ATTENDED_BY   
 LEFT JOIN FOX_TBL_PATIENT_PAT_DOCUMENT AS PD ON PD.FOX_PHD_CALL_DETAILS_ID = CD.FOX_PHD_CALL_DETAILS_ID                                                                      
 LEFT JOIN FOX_TBL_DOCUMENT_TYPE DY ON DY.DOCUMENT_TYPE_ID = PD.DOCUMENT_TYPE                                           
 --LEFT JOIN FOX_TBL_PATIENT_DOCUMENT_FILE_ALL AS PDFA ON PDFA.PAT_DOCUMENT_ID = PD.PAT_DOCUMENT_ID     
 LEFT JOIN CS_Customer_Support_Info AS CCI ON CD.SSCM_CASE_ID = CCI.CS_Case_No                  
 LEFT JOIN CS_Case_Categories AS CCC ON CCI.CS_Case_Category = CCC.CS_Category_ID                   
 LEFT JOIN CS_Case_Progress AS CCP ON CD.SSCM_CASE_ID = CCP.CS_Case_No                
                                                                  
  WHERE (CD.PRACTICE_CODE = @PRACTICE_CODE)       
  AND (                                                                       
  isnull(CD.PATIENT_ACCOUNT, '') LIKE '%' + @SEARCH_TEXT + '%'                                                                             
 OR isnull(CD.MRN, '') LIKE '%' + @SEARCH_TEXT + '%'                                                                      
 OR CONVERT(VARCHAR(10), CD.DOS, 101) LIKE + '%' + @SEARCH_TEXT + '%'                      
 OR CONVERT(VARCHAR(10), CD.CALL_DATE, 101) LIKE + '%' + @SEARCH_TEXT + '%'                                                                                  
 OR convert(VARCHAR(15),CAST(CD.CALL_TIME AS TIME),108) LIKE + '%' + @SEARCH_TEXT + '%'                                                                                  
OR CD.CALL_DURATION LIKE + '%' + @SEARCH_TEXT + '%'                                                                  
   OR CD.CALLER_NAME LIKE + '%' + @SEARCH_TEXT + '%'                                     
   OR CD.RELATIONSHIP LIKE '%' + @SEARCH_TEXT + '%'                                                                                    
   OR PCS.NAME LIKE '%' + @SEARCH_TEXT + '%'                                   
   OR CD.AMOUNT LIKE '%' + @SEARCH_TEXT + '%'                                
   OR CD.SSCM_CASE_ID LIKE '%' + @SEARCH_TEXT + '%'                                     
   OR CD.FOLLOW_UP_DATE LIKE '%' + @SEARCH_TEXT + '%'                                        
   OR DY.DOCUMENT_TYPE_ID LIKE '%' + @SEARCH_TEXT + '%'                   
   OR CCP.CS_Case_Status LIKE '%' + @SEARCH_TEXT + '%'                                                                                                                     
   OR PCR.NAME LIKE '%' + @SEARCH_TEXT + '%'                   
   OR CCC.CS_Category_Name LIKE '%' + @SEARCH_TEXT +'%'                                                                             
   OR CD.CALL_DETAILS LIKE '%' + @SEARCH_TEXT + '%'                                                                             
   OR PCRQ.NAME LIKE '%' + @SEARCH_TEXT + '%'                                                                              
   OR (U.FIRST_NAME+', '+ U.LAST_NAME) LIKE '%' + @SEARCH_TEXT + '%'                                                                              
   OR CD.INCOMING_CALL_NO LIKE '%' + @SEARCH_TEXT + '%'                                                                              
   OR CD.PATIENT_EMAIL_ADDRESS LIKE '%' + @SEARCH_TEXT + '%'           
   OR Pat.First_Name LIKE '%' + @SEARCH_TEXT + '%'          
   OR Pat.Last_Name LIKE '%' + @SEARCH_TEXT + '%'        
          
          
                                                                                
   )                                                                      
   AND (@CALL_DATE_FROM IS NULL                                                         
   OR (CONVERT(DATE, CD.CALL_DATE) >= CONVERT(DATE, Replace(@CALL_DATE_FROM, '%', ''), 101)))                                                
   AND (@CALL_DATE_TO IS NULL                                                                                    
OR (CONVERT(DATE, CD.CALL_DATE) <= CONVERT(DATE, Replace(@CALL_DATE_TO, '%', ''), 101)))                                                                        
    AND (                                                                             
   @CALL_ATTENDED_BY IS NULL                                                                       
   OR CD.CALL_ATTENDED_BY LIKE '%' + @CALL_ATTENDED_BY + '%'                                                                      
   )                                                           
   AND (                                                                             
   @CALL_REASON IS NULL                                                                              
   OR CD.CALL_REASON LIKE '%' + @CALL_REASON + '%'                                                                              
   )                          
   AND (                      
   @CALL_HANDLING IS NULL                      
OR CD.CALL_SCENARIO LIKE '%' + @CALL_HANDLING + '%'                      
   )                
   AND (                      
   @CS_CASE_STATUS IS NULL                      
   OR CCP.CS_Case_Status LIKE '%' + @CS_CASE_STATUS + '%'                      
   )                          
   AND (                     
   @FOLLOW_UP_CALLS IS NULL                    
   OR CD.FOLLOW_UP_DATE != ''                        
   )                                        
   AND (                                                                      
   @CHART_ID IS NULL                               
   OR CD.MRN LIKE '%' + @CHART_ID + '%'                                                                      
   OR CD.PATIENT_ACCOUNT LIKE '%' + @CHART_ID + '%'                                                                        
   )           
    AND (                                                                      
   @PHONE_NUMBER IS NULL                                                                      
   OR CD.INCOMING_CALL_NO LIKE '%' + @PHONE_NUMBER + '%'                                                                      
   )          
    AND (                                                                      
   @PATIENT_FIRST_NAME IS NULL                                                                      
   OR pat.First_Name LIKE '%' + @PATIENT_FIRST_NAME + '%'        
   )           
     AND (                                                                      
   @PATIENT_LAST_NAME IS NULL                                                                      
   OR pat.Last_Name LIKE '%' + @PATIENT_LAST_NAME + '%'        
   )                  
   AND (ISNULL(CD.DELETED, 0) != 1)                                                                      
  ) AS FOX_TBL_PHD_CALL_DETAILS                                                                      
 ORDER BY CASE                                                                        
 WHEN @SORT_BY = 'CHARTID'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN MRN                                                                            
 END ASC                                                                            
  ,CASE                                                                               
   WHEN @SORT_BY = 'CHARTID'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN MRN                                                 
   END DESC                                                                      
   ,CASE                                                                        
 WHEN @SORT_BY = 'FIRST_NAME'             
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN First_Name                                                                            
 END ASC ,                                                                           
  CASE                                                                             
   WHEN @SORT_BY = 'FIRST_NAME'                                                                              
    AND @SORT_ORDER = 'DESC'                            
    THEN First_Name                                                 
   END DESC                                                                      
   ,CASE                                                                         
 WHEN @SORT_BY = 'LAST_NAME'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN Last_Name                                                                            
 END ASC ,                                                               
  CASE                                                                             
   WHEN @SORT_BY = 'LAST_NAME'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN Last_Name                                                 
   END DESC                                                                      
   ,CASE                                                                    
   WHEN @SORT_BY = 'DOS'                                            
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN DOS                                                                              
   END ASC                               
  ,CASE                                                                               
   WHEN @SORT_BY = 'DOS'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN DOS                                                                    
   END DESC                                                                       
   ,CASE                                                                            
   WHEN @SORT_BY = 'CALLDATE'                                                                          
    AND @SORT_ORDER = 'ASC'                                                                             
 THEN CALL_DATE                                                                  
   END ASC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'CALLDATE'                                                                              
    AND @SORT_ORDER = 'DESC'                                                   
    THEN CALL_DATE                                    
   END DESC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'CALLTIME'                                                                  
    AND @SORT_ORDER = 'ASC'                          
    THEN CALL_TIME                                                                              
 END ASC                                                                            
  ,CASE                                                                               
WHEN @SORT_BY = 'CALLTIME'    
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN CALL_TIME                                                                              
   END DESC                                    
   ,CASE                                                                     
   WHEN @SORT_BY = 'CALLDURATION'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN CONVERT(int, CALL_DURATION)                                                  
 END ASC                                                                            
  ,CASE                                                                               
   WHEN @SORT_BY = 'CALLDURATION'                                                                        
    AND @SORT_ORDER = 'DESC'                                            
    THEN CONVERT(int, CALL_DURATION)                                                                      
   END DESC                                                                       
   ,CASE                                                                      
   WHEN @SORT_BY = 'CALLERNAME'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                              
   THEN CALLER_NAME                                                                         
  END ASC                                                                           
  ,CASE                                                                
   WHEN @SORT_BY = 'CALLERNAME'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN CALLER_NAME                                           
END DESC                                                                      
   ,CASE                                                                      
   WHEN @SORT_BY = 'RELATIONSHIP'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN RELATIONSHIP                                                                         
  END ASC                                                                           
  ,CASE                                                                               
   WHEN @SORT_BY = 'RELATIONSHIP'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN RELATIONSHIP                                                                              
   END DESC                                                                      
   ,CASE                                                                          
WHEN @SORT_BY = 'CALLSCENARIO'                                                               
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN CALL_SCENARIO_NAME                                                                              
   END ASC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'CALLSCENARIO'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN CALL_SCENARIO_NAME                                                                              
   END DESC                                                                                                 
   ,CASE                                                                          
    WHEN @SORT_BY = 'AMOUNT'                                        
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN AMOUNT                                             
   END ASC                     
   ,CASE                                                                               
   WHEN @SORT_BY = 'CASECATEGORY'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN CALL_SCENARIO_NAME                                                                              
   END DESC                                                                                                      
   ,CASE                                                                          
    WHEN @SORT_BY = 'CASECATEGORY'                                        
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN AMOUNT                                                                              
   END ASC                                                                               
  ,CASE          
   WHEN @SORT_BY = 'AMOUNT'                                                    
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN AMOUNT                                                                              
   END DESC                                    
   ,CASE                                                                          
    WHEN @SORT_BY = 'FOLLOW_UP_DATE'                                        
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN FOLLOW_UP_DATE                                                                              
   END ASC                                                                              
  ,CASE                                                      
   WHEN @SORT_BY = 'FOLLOW_UP_DATE'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN FOLLOW_UP_DATE                                                                              
   END DESC                        
   ,CASE                                                                          
    WHEN @SORT_BY = 'SSCM_CASE_ID'                                        
    AND @SORT_ORDER = 'ASC'                               
    THEN SSCM_CASE_ID                                                                              
   END ASC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'SSCM_CASE_ID'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN SSCM_CASE_ID                                                                              
   END DESC                                   
  ,CASE                                                                          
    WHEN @SORT_BY = 'DOCUMENT_TYPE'                
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN DOCUMENT_TYPE                                                                              
   END ASC                                                                         
  ,CASE                                                                 
   WHEN @SORT_BY = 'DOCUMENT_TYPE'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN DOCUMENT_TYPE                                                                              
   END DESC                                                                
  ,CASE                                                                            
   WHEN @SORT_BY = 'CALLREASON'                                                                              
  AND @SORT_ORDER = 'ASC'                                                                              
    THEN CALL_REASON_NAME                                                                              
   END ASC                       
  ,CASE                                                                               
   WHEN @SORT_BY = 'CALLREASON'                                                                              
    AND @SORT_ORDER = 'DESC'                                                            
    THEN CALL_REASON_NAME                                                                              
   END DESC                                                                      
   ,CASE                                                                      
   WHEN @SORT_BY = 'CALLDETAILS'                                                                              
    AND @SORT_ORDER = 'ASC'                                                
    THEN CALL_DETAILS                                                                       
   END ASC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'CALLDETAILS'                                       
    AND @SORT_ORDER = 'DESC'                                                                         
    THEN CALL_DETAILS                                                                              
   END DESC                      
   ,CASE                                                                      
   WHEN @SORT_BY = 'CASESTATUS'                                                                              
    AND @SORT_ORDER = 'ASC'                        
    THEN CS_Case_Status                                                                       
   END ASC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'CASESTATUS'               
    AND @SORT_ORDER = 'DESC'                                                                         
    THEN CS_Case_Status                                                                              
   END DESC                                                                         
   ,CASE                                                                      
   WHEN @SORT_BY = 'REQUEST'                                                                          
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN REQUEST_NAME                                                                              
   END ASC                                                                 
  ,CASE                                                                               
   WHEN @SORT_BY = 'REQUEST'                                                     
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN REQUEST_NAME                                                                              
   END DESC                          
  ,CASE                                                                   
  WHEN @SORT_BY = 'CALLATTENDEDBY'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN CALL_ATTENDED_BY                                                            
   END ASC                                                                        
  ,CASE                                                                  
   WHEN @SORT_BY = 'CALLATTENDEDBY'                                       
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN CALL_ATTENDED_BY                                                                              
   END DESC                                             
  ,CASE                                          
  WHEN @SORT_BY = 'INCOMINGCALLNUMBER'                                                                 
 AND @SORT_ORDER = 'ASC'                                                                              
    THEN INCOMING_CALL_NO                                                                              
   END ASC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'INCOMINGCALLNUMBER'                                                                              
    AND @SORT_ORDER = 'DESC'                                                                              
    THEN INCOMING_CALL_NO                                          
   END DESC                                                 
  ,CASE                                                                            
  WHEN @SORT_BY = 'PATIENTEMAILADDRESS'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                           
    THEN PATIENT_EMAIL_ADDRESS                                                               
   END ASC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'PATIENTEMAILADDRESS'                                                                              
    AND @SORT_ORDER = 'DESC'                                       
    THEN PATIENT_EMAIL_ADDRESS                                                                              
   END DESC                             
  ,CASE                                                                            
  WHEN @SORT_BY = 'CREATEDDATE'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN CREATED_DATE                                                               
   END ASC                                                                              
  ,CASE                                                                               
   WHEN @SORT_BY = 'CREATEDDATE'                                                                              
 AND @SORT_ORDER = 'DESC'                                                                              
    THEN CREATED_DATE                                                                              
   END DESC                                                                             
  ,CASE                                                                            
  WHEN @SORT_BY = 'MODIFIEDDATE'                                                                              
    AND @SORT_ORDER = 'ASC'                                                                              
    THEN MODIFIED_DATE                                                                   
   END ASC                                                             
 ,CASE                       
   WHEN @SORT_BY = 'MODIFIEDDATE'                                                                              
    AND @SORT_ORDER = 'DESC'                          
    THEN MODIFIED_DATE                                               
   END DESC                                                                             
  ,CASE                                                                           
   WHEN @SORT_BY = ''                                                                              
    AND @SORT_ORDER = ''                                                                              
    THEN CREATED_DATE                                                                              
   END DESC OFFSET @START_FROM ROWS                                
                                                                              
 FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                                                                              
END 