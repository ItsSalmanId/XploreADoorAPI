-- =============================================                          
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>                          
-- CREATE DATE: <CREATE DATE, 23/06/2022>                          
-- Description:  This procedure is Get Team member list                  
-- =============================================     
CREATE PROCEDURE FOX_PROC_GET_TEAM_EMPLOYEE_LIST --1011163,'544110,544109,544114,544112,544113'                                    
  @PRACTICE_CODE BIGINT,                                      
  @CALL_SCANRIO_ID varchar(200)                                                                          
AS                          
BEGIN                           
   ( SELECT  TBL_USER.USER_NAME, DBO.MTBC_TITLECASE(TBL_USER.FIRST_NAME + ' '+ TBL_USER.LAST_NAME) AS NAME, TBL_USER.EMAIL  FROM  FOX_TBL_APPLICATION_USER TBL_USER   with (nolock)                            
  JOIN FOX_TBL_USER_TEAMS TBL_USER_TEAM with (nolock) ON TBL_USER.USER_ID = TBL_USER_TEAM.USER_ID                              
  WHERE TBL_USER_TEAM.PHD_CALL_SCENARIO_ID IN  (select * from foxsplitstring(@CALL_SCANRIO_ID,','))                      
  AND ISNULL(TBL_USER.DELETED, 0) = 0 AND ISNULL(TBL_USER_TEAM.DELETED, 0) = 0   AND TBL_USER_TEAM.PRACTICE_CODE = @PRACTICE_CODE AND TBL_USER.IS_ACTIVE = 1                     
    UNION                            
   SELECT  DISTINCT PHD.CREATED_BY AS USER_NAME, DBO.MTBC_TITLECASE(AU.FIRST_NAME + ' '+ AU.LAST_NAME) AS NAME , EMAIL                                
   FROM FOX_TBL_PHD_CALL_DETAILS PHD  with (nolock)                                
   LEFT JOIN FOX_TBL_APPLICATION_USER AS AU with (nolock) ON  PHD.CREATED_BY = AU.USER_NAME AND AU.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(AU.DELETED,0)= 0                                   
   WHERE                        
   phd.CALL_SCENARIO in (select * from foxsplitstring(@CALL_SCANRIO_ID,',')) and                  
   ISNULL(PHD.DELETED, 0) = 0                                  
   AND PHD.PRACTICE_CODE = @PRACTICE_CODE  AND ISNULL(AU.FIRST_NAME, '') <> ''  AND ISNULL(AU.LAST_NAME, '') <> ''                         
   AND ISNULL(AU.DELETED,0)= 0  AND AU.IS_ACTIVE = 1)                      
  ORDER BY NAME ASC                                           
END 