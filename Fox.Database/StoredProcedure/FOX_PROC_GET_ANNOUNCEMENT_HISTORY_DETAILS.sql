	-- AUTHOR:  <MUHAMMAD SALMAN>                                    
	-- CREATE DATE: <CREATE DATE, 10/03/2022>                                    
	-- DESCRIPTION: <GET LIST OF ANNOUNCEMENT DETAILS WITH HISTORY>                             
	-- [FOX_PROC_GET_ANNOUNCEMENT_HISTORY_DETAILS] 1011163, 548376,103, '1163testing'                           
	ALTER PROCEDURE [DBO].[FOX_PROC_GET_ANNOUNCEMENT_HISTORY_DETAILS]                                                 
	(                                                   
	  @PRACTICE_CODE BIGINT,             
	  @ANNOUNCEMENT_ID BIGINT,        
	  @ROLE_ID BIGINT,    
	  @USER_NAME VARCHAR(50)                                                    
	)                                                      
	AS                                                      
	BEGIN                       
	  SELECT * FROM FOX_TBL_ANNOUNCEMENT_HISTORY AS H WITH (NOLOCK)       
	  WHERE H.ANNOUNCEMENT_ID = @ANNOUNCEMENT_ID          
	  AND H.PRACTICE_CODE = @PRACTICE_CODE  AND H.USER_ID = @ROLE_ID  AND H.USER_NAME = @USER_NAME    
	  AND H.SHOW_COUNT > 0          
	  AND ISNULL(H.DELETED, 0) = 0              
	END 