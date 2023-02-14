-- =============================================                              
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>                              
-- CREATE DATE: <CREATE DATE, 02/14/2023>                              
-- DESCRIPTION:  THIS PROCEDURE IS GET TEAM MEMBER LIST OF SURVEY                        
-- =============================================    
CREATE PROCEDURE FOX_PROC_GET_QADASHBOARD_SURVEY_TEAM_EMPLOYEE_LIST                        
  @PRACTICE_CODE BIGINT,                      
  @ROLE_ID BIGINT                                 
AS                      
BEGIN                                                                         
   SELECT  DISTINCT SAS.AGENT_NAME AS USER_NAME, DBO.MTBC_TITLECASE(AU.FIRST_NAME + ' '+ AU.LAST_NAME) AS NAME , EMAIL                    
   FROM FOX_TBL_SURVEY_AUDIT_SCORES SAS WITH (NOLOCK)                       
   INNER JOIN FOX_TBL_APPLICATION_USER  AS AU WITH (NOLOCK) ON  SAS.AGENT_NAME = AU.USER_NAME AND AU.PRACTICE_CODE = @PRACTICE_CODE                       
   WHERE                      
      ISNULL(SAS.DELETED, 0) = 0                      
   AND SAS.PRACTICE_CODE = @PRACTICE_CODE     
   AND SAS.MODIFIED_DATE BETWEEN DATEADD(DAY,-98, GETDATE()) AND GETDATE()  -- I have implemented this check for those agents who's audit is performed in last 98 days
    AND ISNULL(AU.DELETED,0)= 0       
   AND AU.ROLE_ID = @ROLE_ID  
  AND AU.IS_ACTIVE = 1                        
 ORDER BY NAME ASC                      
END 