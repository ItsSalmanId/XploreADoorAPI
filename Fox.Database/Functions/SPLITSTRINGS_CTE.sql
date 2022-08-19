IF (OBJECT_ID('SPLITSTRINGS_CTE') IS NOT NULL ) DROP PROCEDURE SPLITSTRINGS_CTE
GO 
CREATE FUNCTION [DBO].[SPLITSTRINGS_CTE]  
(  
   @LIST       NVARCHAR(MAX),  
   @DELIMITER  NVARCHAR(255)  
)  
RETURNS @ITEMS TABLE (ITEM NVARCHAR(4000))  
WITH SCHEMABINDING  
AS  
BEGIN  
   DECLARE @LL INT = LEN(@LIST) + 1, @LD INT = LEN(@DELIMITER);  
   
   WITH A AS  
   (  
       SELECT  
           [START] = 1,  
           [END]   = COALESCE(NULLIF(CHARINDEX(@DELIMITER,   
                       @LIST, 1), 0), @LL),  
           [VALUE] = SUBSTRING(@LIST, 1,   
                     COALESCE(NULLIF(CHARINDEX(@DELIMITER,   
                       @LIST, 1), 0), @LL) - 1)  
       UNION ALL  
       SELECT  
           [START] = CONVERT(INT, [END]) + @LD,  
           [END]   = COALESCE(NULLIF(CHARINDEX(@DELIMITER,   
                       @LIST, [END] + @LD), 0), @LL),  
           [VALUE] = SUBSTRING(@LIST, [END] + @LD,   
                     COALESCE(NULLIF(CHARINDEX(@DELIMITER,   
                       @LIST, [END] + @LD), 0), @LL)-[END]-@LD)  
       FROM A  
       WHERE [END] < @LL  
   )  
   INSERT @ITEMS SELECT [VALUE]  
   FROM A  
   WHERE LEN([VALUE]) > 0  
   OPTION (MAXRECURSION 0);  
   
   RETURN;  
END  



