IF (OBJECT_ID('FOX_PROC_GET_COUNTIES_BY_STATE_CODE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_COUNTIES_BY_STATE_CODE 
GO  
--****************************************************************************14************************************************************************************  
--EXEC FOX_PROC_GET_COUNTIES_BY_STATE_CODE 1011163, 'wi'      
CREATE PROCEDURE FOX_PROC_GET_COUNTIES_BY_STATE_CODE @PRACTICE_CODE BIGINT,       
                                                    @STATE_CODE    VARCHAR(5)      
AS      
     BEGIN      
         SELECT *      
         FROM FOX_TBL_ZIP_STATE_COUNTY      
         WHERE ZIP_STATE_COUNTY_ID IN      
         (      
             SELECT MAX(ZIP_STATE_COUNTY_ID)      
             FROM FOX_TBL_ZIP_STATE_COUNTY      
             WHERE PRACTICE_CODE = @PRACTICE_CODE      
                   AND STATE = @STATE_CODE      
                   AND COUNTY IS NOT NULL      
                   AND COUNTY != ''      
             GROUP BY COUNTY      
         )      
         ORDER BY COUNTY;      
     END; 