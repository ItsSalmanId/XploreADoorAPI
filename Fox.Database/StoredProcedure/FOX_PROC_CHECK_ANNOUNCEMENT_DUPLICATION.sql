        
   /**************************************************************************        
-- Author: Irfan Ullah        
-- Create date: 10/26/2022        
-- Description: Check the existing announcement enteris against the roles        
*****************************************************************************/                             
                           
CREATE PROCEDURE [dbo].[FOX_PROC_CHECK_ANNOUNCEMENT_DUPLICATION]                                  
 @ANNOUNCEMENT_DATE_FROM DATETIME,                                 
 @ANNOUNCEMENT_DATE_TO DATETIME,                                                    
 @ROLES_IDs varchar(max)                                 
                                    
AS                                              
BEGIN                      
                    
 select top(1) a.* from FOX_TBL_ANNOUNCEMENT a         
 inner join FOX_TBL_ANNOUNCEMENT_ROLE r WITH (NOLOCK) on a.ANNOUNCEMENT_ID=r.ANNOUNCEMENT_ID         
  where a.ANNOUNCEMENT_DATE_FROM between @ANNOUNCEMENT_DATE_FROM and @ANNOUNCEMENT_DATE_TO and        
   r.ROLE_ID in (  SELECT val FROM STRING_SPLIT(@ROLES_IDs, ','))       and      
  isnull(r.DELETED,0)=0 and    
  isnull(a.DELETED,0)=0           
                          
END 