-- =============================================                              
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>                              
-- CREATE DATE: <CREATE DATE, 02/14/2023>                              
-- DESCRIPTION:  THIS PROCEDURE IS GET TEAM MEMBER LIST OF PHD                         
-- =============================================    
Create PROCEDURE FOX_PROC_GET_TEAM_EMPLOYEE_LIST --1012714,',605122,605121,605128,605129,605127,605120,605130,605113,605123,54410118'                                        
  @PRACTICE_CODE BIGINT,                                          
  @CALL_SCANRIO_ID VARCHAR(200)                                                                              
AS                              
BEGIN                               
   SELECT  DISTINCT SAS.AGENT_NAME AS USER_NAME, DBO.MTBC_TITLECASE(AU.FIRST_NAME + ' '+ AU.LAST_NAME) AS NAME , EMAIL                                    
   FROM FOX_TBL_SURVEY_AUDIT_SCORES  SAS  WITH (NOLOCK)                                    
   INNER JOIN FOX_TBL_APPLICATION_USER  AS AU WITH (NOLOCK) ON  SAS.AGENT_NAME = AU.USER_NAME AND AU.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(AU.DELETED,0)= 0                                       
   WHERE                            
   SAS.PHD_CALL_SCENARIO_ID IN (SELECT * FROM FOXSPLITSTRING(@CALL_SCANRIO_ID,',')) AND                      
   ISNULL(SAS.DELETED, 0) = 0                                      
   AND SAS.PRACTICE_CODE = @PRACTICE_CODE    
   AND ISNULL(AU.FIRST_NAME, '') <> ''  AND ISNULL(AU.LAST_NAME, '') <> ''                             
   AND ISNULL(AU.DELETED,0)= 0  AND AU.IS_ACTIVE = 1       
   AND SAS.MODIFIED_DATE BETWEEN DATEADD(DAY,-98, GETDATE()) AND GETDATE()    
  ORDER BY NAME ASC                                               
END 