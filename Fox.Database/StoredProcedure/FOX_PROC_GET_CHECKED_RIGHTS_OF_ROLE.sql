IF (OBJECT_ID('FOX_PROC_GET_CHECKED_RIGHTS_OF_ROLE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_CHECKED_RIGHTS_OF_ROLE 
GO
CREATE PROC [dbo].[FOX_PROC_GET_CHECKED_RIGHTS_OF_ROLE] @PRACTICE_CODE BIGINT,   
                                                @ROLE_ID       BIGINT  
AS  
     BEGIN  
         SELECT ftprr.*  
         FROM dbo.FOX_TBL_ROLE ftr  
              INNER JOIN dbo.FOX_TBL_RIGHTS_OF_ROLE ftror ON ftr.ROLE_ID = ftror.ROLE_ID  
              INNER JOIN dbo.FOX_TBL_PRACTICE_ROLE_RIGHTS ftprr ON ftror.RIGHTS_OF_ROLE_ID = ftprr.RIGHTS_OF_ROLE_ID  
                                                                   AND ftprr.PRACTICE_CODE = @PRACTICE_CODE  
                                                                   AND isnull(ftprr.DELETED, '') = 0  
                                                                   AND ftprr.CHECKED = 1  
         WHERE isnull(ftr.DELETED, '') = 0  
               AND ftr.ROLE_ID = @ROLE_ID;  
     END; 