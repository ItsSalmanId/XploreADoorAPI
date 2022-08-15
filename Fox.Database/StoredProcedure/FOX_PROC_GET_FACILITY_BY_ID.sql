IF (OBJECT_ID('FOX_PROC_GET_FACILITY_BY_ID') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_FACILITY_BY_ID 
GO  
CREATE PROC [dbo].[FOX_PROC_GET_FACILITY_BY_ID] @PRACTICE_CODE BIGINT,   
                                       @LOC_ID        BIGINT  
AS  
     BEGIN  
         SELECT ftal.*,   
                ftft.DISPLAY_NAME AS FACILITY_TYPE_NAME  
         FROM dbo.FOX_TBL_ACTIVE_LOCATIONS ftal  
              LEFT OUTER JOIN dbo.FOX_TBL_FACILITY_TYPE ftft ON ftal.FACILITY_TYPE_ID = ftft.FACILITY_TYPE_ID  
                                                                AND ftft.PRACTICE_CODE = @PRACTICE_CODE  
                                                                AND ISNULL(ftft.DELETED, 0) = 0  
         WHERE ftal.LOC_ID = @LOC_ID  
               AND ISNULL(FTAL.DELETED, 0) = 0  
               AND ftal.PRACTICE_CODE = @PRACTICE_CODE;  
     END;  
  
----------------------------------------------------------------------------------------------------  
