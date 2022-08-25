IF (OBJECT_ID('FOX_SP_VALIDATEEMAIL') IS NOT NULL ) DROP PROCEDURE FOX_SP_VALIDATEEMAIL  
GO 
-- =============================================  
-- Author:  <Muhammad Imran>  
-- Create date: <02/096/2019>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE FOX_SP_VALIDATEEMAIL   
-- Add the parameters for the stored procedure here  
@EMAIL VARCHAR(150)
AS
     BEGIN
         SELECT TOP 1 *
         FROM FOX_TBL_APPLICATION_USER
         WHERE LOWER(ISNULL(EMAIL, '')) = LOWER(@EMAIL)
               AND ISNULL(DELETED, 0) = 0
               AND ISNULL(IS_ACTIVE, 0) = 1;
     END;