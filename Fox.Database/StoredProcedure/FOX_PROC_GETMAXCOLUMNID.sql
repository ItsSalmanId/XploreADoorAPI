IF (OBJECT_ID('FOX_PROC_GETMAXCOLUMNID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GETMAXCOLUMNID  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GETMAXCOLUMNID]  
-- Add the parameters for the stored procedure here
@Col_Name NVARCHAR(50)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
DECLARE @counterValue BIGINT;
SET NOCOUNT ON;
-- Insert statements for procedure here
BEGIN
UPDATE Maintenance_Counter
SET Col_Counter = Col_Counter + 1,
@counterValue = Col_Counter --+ 1
WHERE COL_NAME = @Col_Name
END
SELECT (
CONVERT(
VARCHAR,
(
SELECT office_id
FROM Maintenance
)
) + '' + CONVERT(VARCHAR, (@counterValue))
) AS MaxID
END

