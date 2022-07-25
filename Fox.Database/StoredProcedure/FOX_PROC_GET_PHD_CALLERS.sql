IF (OBJECT_ID('FOX_PROC_GET_PHD_CALLERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PHD_CALLERS  
GO 
-- =============================================      
-- Author:  <MUHAMMAD IMRAN>      
-- Create date: <10/12/2019>      
-- Description: <Description,,>      
-- =============================================      
-- FOX_PROC_GET_PHD_CALLERS 1011163      
CREATE PROCEDURE FOX_PROC_GET_PHD_CALLERS      
 -- Add the parameters for the stored procedure here      
 @PRACTICE_CODE BIGINT      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SELECT DISTINCT U.* FROM       
 FOX_TBL_PHD_CALL_DETAILS PHD       
 INNER JOIN FOX_TBL_APPLICATION_USER U ON U.USER_ID = PHD.CALL_ATTENDED_BY AND U.PRACTICE_CODE = @PRACTICE_CODE AND PHD.PRACTICE_CODE = @PRACTICE_CODE      
 WHERE ISNULL(U.DELETED, 0) = 0       
 AND ISNULL(PHD.DELETED, 0) = 0      
       
END   
