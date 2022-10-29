-- AUTHOR:  <MUHAMMAD SALMAN>                                    
-- CREATE DATE: <CREATE DATE, 10/03/2022>                                    
-- DESCRIPTION: <ADD ANNOUNCEMENT SPLASH DETAILS IN HISTORY TABLE>                
CREATE PROCEDURE [dbo].[FOX_PROC_CRUD_ANNOUNCEMENT_HISTORY]        
 @ANNOUNCEMENT_HISTORY_ID BIGINT,              
 @ANNOUNCEMENT_ID BIGINT,                
 @USER_ID VARCHAR(500),                
 @USER_NAME VARCHAR(100),                
 @SHOW_COUNT INT,           
 @CREATED_DATE DATETIME,                
 @MODIFIED_DATE DATETIME,                      
 @PRACTICE_CODE BIGINT,               
 @DELETED BIT,                  
 @CREATED_BY VARCHAR(70),                   
 @OPERATION VARCHAR(70)                 
                  
AS                            
BEGIN                                     
 If(@OPERATION ='ADD')          
  BEGIN                 
    INSERT INTO FOX_TBL_ANNOUNCEMENT_HISTORY (                        
         ANNOUNCEMENT_HISTORY_ID,    
   ANNOUNCEMENT_ID,                        
         USER_ID,                          
         USER_NAME,                        
         SHOW_COUNT,                                 
         CREATED_DATE,                        
         MODIFIED_DATE,                                                
         PRACTICE_CODE,                        
         DELETED,                        
         CREATED_BY                        
         )                        
       VALUES (                        
         @ANNOUNCEMENT_HISTORY_ID,     
   @ANNOUNCEMENT_ID,                       
         @USER_ID,                          
         @USER_NAME,                        
         @SHOW_COUNT,                                 
         @CREATED_DATE,                        
         @MODIFIED_DATE,                                                
         @PRACTICE_CODE,                        
         @DELETED,                        
         @CREATED_BY                                                
       )         
   END               
END 