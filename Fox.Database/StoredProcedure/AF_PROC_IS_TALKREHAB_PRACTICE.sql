IF (OBJECT_ID('Af_proc_is_talkrehab_practice') IS NOT NULL ) DROP PROCEDURE Af_proc_is_talkrehab_practice  
GO 
CREATE PROCEDURE Af_proc_is_talkrehab_practice   --'1011163'           
(@Practice_code BIGINT)              
AS              
  BEGIN              
      --DECLARE                                    
      --  @Practice_code BIGINT=1011163                          
      SELECT TOP 1 options              
      FROM   webehr_tbl_user_application_config wtbua              
             JOIN webehr_tbl_application_config wtac              
               ON wtbua.id = wtac.id              
      WHERE  wtbua.practice_code = @Practice_code              
             AND Isnull(wtac.deleted, 0) <> 1              
             AND Isnull(wtbua.deleted, 0) <> 1              
             AND Isnull(description, '') = 'is_talkRehab_practice'              
      ORDER  BY wtbua.modified_date DESC              
  END 