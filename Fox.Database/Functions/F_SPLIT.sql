IF (OBJECT_ID('F_SPLIT') IS NOT NULL ) DROP PROCEDURE F_SPLIT  
GO 
--**************************************************************************************************************************************************************  
  
CREATE FUNCTION [DBO].[F_SPLIT]  
(  
@PARAM NVARCHAR(MAX),   
@DELIMITER CHAR(1)  
)  
RETURNS @T TABLE (VAL NVARCHAR(MAX), SEQ INT)  
AS  
BEGIN  
SET @PARAM += @DELIMITER  
  
;WITH A AS  
(  
SELECT CAST(1 AS BIGINT) F, CHARINDEX(@DELIMITER, @PARAM) T, 1 SEQ  
UNION ALL  
SELECT T + 1, CHARINDEX(@DELIMITER, @PARAM, T + 1), SEQ + 1  
FROM A  
WHERE CHARINDEX(@DELIMITER, @PARAM, T + 1) > 0  
)  
INSERT @T  
SELECT SUBSTRING(@PARAM, F, T - F), SEQ FROM A  
OPTION (MAXRECURSION 0)  
RETURN  
END  

