IF (OBJECT_ID('Web_Proc_GetCountryLong') IS NOT NULL ) DROP PROCEDURE Web_Proc_GetCountryLong  
GO 
Create PROCEDURE [dbo].[Web_Proc_GetCountryLong]          
(           
@UserIP varchar(20)       
)          
AS          
BEGIN          
           
-- SET NOCOUNT ON added to prevent extra result sets from          
SET NOCOUNT ON;          
declare @downloadid bigint        
set @downloadid =256 * 256 * 256 * CAST(PARSENAME(@UserIP, 4) AS float) + 256 * 256 * CAST(PARSENAME(@UserIP, 3) AS float) + 256 * CAST  (PARSENAME(@UserIP, 2) AS float) + CAST(PARSENAME(@UserIP, 1) AS float)                    
SELECT countryLONG FROM IPCountry WHERE  @downloadid BETWEEN ipFROM AND ipTO       
END      
