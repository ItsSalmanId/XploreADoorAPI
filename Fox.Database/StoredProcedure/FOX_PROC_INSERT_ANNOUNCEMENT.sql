-- AUTHOR:  <MUHAMMAD SALMAN>      
-- CREATE DATE: <CREATE DATE, 09/26/2022>      
-- DESCRIPTION: <Insert Announcement DETAILS>                                                                  
CREATE PROCEDURE [dbo].[FOX_PROC_INSERT_ANNOUNCEMENT]                            
 @ANNOUNCEMENT_ID BIGINT,                            
 @ANNOUNCEMENT_DETAILS VARCHAR(Max),                            
 @ANNOUNCEMENT_TITLE VARCHAR(100),                            
 @ANNOUNCEMENT_DATE_FROM DATETIME,                           
 @ANNOUNCEMENT_DATE_TO DATETIME,                          
 @MODIFIED_DATE DATETIME,         
 @MODIFIED_BY VARCHAR(70),                         
 @PRACTICE_CODE BIGINT,                           
 @DELETED BIT,                              
 @CREATED_BY VARCHAR(70),                       
 @OPERATION VARCHAR(70)                             
                              
AS                                        
BEGIN                
              
 IF(@OPERATION ='ADD')                
 BEGIN                     
   INSERT INTO FOX_TBL_ANNOUNCEMENT           
   (                                    
         ANNOUNCEMENT_ID,                                    
         ANNOUNCEMENT_DETAILS,                                      
         ANNOUNCEMENT_TITLE,                                    
         ANNOUNCEMENT_DATE_FROM,                                             
         ANNOUNCEMENT_DATE_TO,                                    
         MODIFIED_DATE,     
         MODIFIED_BY,     
         PRACTICE_CODE,     
         CREATED_BY,    
         DELETED                                   
         )                                    
       VALUES (                                    
         @ANNOUNCEMENT_ID,                                    
         @ANNOUNCEMENT_DETAILS,                                      
         @ANNOUNCEMENT_TITLE,                                    
         @ANNOUNCEMENT_DATE_FROM,                                             
         @ANNOUNCEMENT_DATE_TO,                                    
         @MODIFIED_DATE,      
         @MODIFIED_BY,       
         @PRACTICE_CODE,      
         @CREATED_BY,                                  
         @DELETED                                 
       )                     
 END                       
 ELSE IF(@OPERATION ='UPDATE')                     
   BEGIN                       
    UPDATE FOX_TBL_ANNOUNCEMENT_ROLE     
 SET DELETED = 1, MODIFIED_BY = @MODIFIED_BY     
 WHERE ANNOUNCEMENT_ID = @ANNOUNCEMENT_ID                 
                       
    UPDATE FOX_TBL_ANNOUNCEMENT           
 SET                       
         ANNOUNCEMENT_DETAILS = @ANNOUNCEMENT_DETAILS,                                      
         ANNOUNCEMENT_TITLE = @ANNOUNCEMENT_TITLE,                                    
         ANNOUNCEMENT_DATE_FROM = @ANNOUNCEMENT_DATE_FROM,                                             
         ANNOUNCEMENT_DATE_TO = @ANNOUNCEMENT_DATE_TO,                                    
         MODIFIED_DATE = @MODIFIED_DATE,        
         MODIFIED_BY  = @MODIFIED_BY,                                          
         PRACTICE_CODE = @PRACTICE_CODE,                                    
         DELETED = @DELETED                                   
   WHERE   ANNOUNCEMENT_ID = @ANNOUNCEMENT_ID                                        
   END                             
END 