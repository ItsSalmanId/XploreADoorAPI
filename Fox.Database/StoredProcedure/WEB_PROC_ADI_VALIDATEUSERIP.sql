IF (OBJECT_ID('WEB_PROC_ADI_VALIDATEUSERIP') IS NOT NULL ) DROP PROCEDURE WEB_PROC_ADI_VALIDATEUSERIP  
GO 
CREATE PROCEDURE [dbo].[WEB_PROC_ADI_VALIDATEUSERIP]                
-- ADD THE PARAMETERS FOR THE STORED PROCEDURE HERE                        
@USERNAME VARCHAR(50),                        
@USERIP VARCHAR(20)                        
AS                        
BEGIN                        
-- SET NOCOUNT ON ADDED TO PREVENT EXTRA RESULT SETS FROM                        
                    
-- INTERFERING WITH SELECT STATEMENTS.                        
SET NOCOUNT ON;                        
DECLARE @DOWNLOADID BIGINT                        
DECLARE @IPADDRESS VARCHAR(20)                        
DECLARE @RESULT INT                        
SET @IPADDRESS = @USERIP                        
SET @DOWNLOADID =256 * 256 * 256 * CAST(PARSENAME(@IPADDRESS, 4) AS FLOAT) + 256 * 256 * CAST(PARSENAME(@IPADDRESS, 3) AS FLOAT) + 256 * CAST  (PARSENAME(@IPADDRESS, 2) AS FLOAT) + CAST(PARSENAME(@IPADDRESS, 1) AS FLOAT)                        
              
                   
 IF OBJECT_ID('TEMPDB..#ALLOWIP') IS NOT NULL      
       DROP TABLE #ALLOWIP      
      
  SELECT DISTINCT CASE WHEN ISNULL(ALLOWED,0) = 0 THEN                       
  CASE WHEN COUNTRYLONG = '-' THEN '1'ELSE ISNULL(DEFAULT_COUNTRY,0) END                      
  ELSE ALLOWED END AS ALLOWED        
  INTO #ALLOWIP                     
  FROM IPCOUNTRY IPC                       
  LEFT OUTER JOIN ALL_COUNTRIES AC ON AC.COUNTRY_NAME = IPC.COUNTRYLONG                       
  LEFT OUTER JOIN USER_ALLOW_COUNTRIES UAC ON UAC.COUNTRY_NAME = IPC.COUNTRYLONG AND UAC.USER_NAME = @USERNAME                       
  WHERE CONVERT(BIGINT,@DOWNLOADID) BETWEEN IPFROM AND IPTO                       
  AND CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),101)) BETWEEN ISNULL(DATE_FROM,CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),101))) AND ISNULL(DATE_TO,CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),101)))                      
  UNION                  
  SELECT COUNT(*) AS ALLOWED FROM ISBIPCOUNTRY WHERE CONVERT(BIGINT,@DOWNLOADID)  BETWEEN IPNUMBERFROM AND IPNUMBERTO AND IS_ACTIVE = 1        
         
   --TO BE USED FOR FILTERING THE BLOCKED COUNTRIES       
    SET @RESULT =(                  
  SELECT COUNT(*) FROM IPCOUNTRY IPC INNER JOIN ALL_COUNTRIES AC ON IPC.COUNTRYSHORT = AC.COUNTRY_CODE                  
  WHERE @DOWNLOADID BETWEEN IPFROM AND IPTO                  
  AND ISNULL(DELETED, 0) <> 1                  
  AND COUNTRY_FLAG = 'B'                  
 )        
   IF(@RESULT>= 1)                    
 SET @RESULT = '403'                   
                   
ELSE         
--print @DOWNLOADID        
       
        
      
      
      
SELECT @RESULT =  ALLOWED FROM                       
(        
           
  SELECT TOP 1 *      
  FROM #ALLOWIP      
  ORDER BY 1 DESC                     
) AS ISALLOWED           
SELECT @RESULT AS VALIDIP                         
END 
