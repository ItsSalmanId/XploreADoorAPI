IF (OBJECT_ID('GET_ALL_LOCATIONS_FOR_SCHEDULER') IS NOT NULL ) DROP PROCEDURE GET_ALL_LOCATIONS_FOR_SCHEDULER  
GO       
CREATE PROCEDURE GET_ALL_LOCATIONS_FOR_SCHEDULER(@PRACTICE_CODE BIGINT)      
AS      
     BEGIN      
         SELECT LOC_ID AS LOCATION_CODE,       
                LTRIM(RTRIM(NAME)) LOCATION_NAME,       
                Address AS LOCATION_ADDRESS,       
                concat(ISNULL(Address, 'address'), ', ', ISNULL(City, 'city'), ' ', ISNULL(State, 'state'), ', ', ISNULL(Zip, 'zip')) AS LOCATION_COMPLETE_ADDRESS,       
                Phone AS PHONE_NUMBER,       
                ISNULL(Fax, '') AS FAX_NUMBER      
         FROM FOX_TBL_ACTIVE_LOCATIONS      
         WHERE PRACTICE_CODE = @PRACTICE_CODE      
               AND ISNULL(DELETED, 0) = 0      
               AND ISNULL(IS_ACTIVE, 1) = 1      
      AND NAME IS NOT NULL      
      AND NAME <> ''      
      ORDER BY LOCATION_NAME ASC       
     END;