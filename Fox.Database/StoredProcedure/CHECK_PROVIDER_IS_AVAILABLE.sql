IF (OBJECT_ID('CHECK_PROVIDER_IS_AVAILABLE') IS NOT NULL ) DROP PROCEDURE CHECK_PROVIDER_IS_AVAILABLE  
GO 
Create PROCEDURE [dbo].[CHECK_PROVIDER_IS_AVAILABLE]    
(@PROVIDER       BIGINT,    
@APPOINMENT_DATE DATETIME,     
@PRACTICE_CODE   BIGINT)    
AS    
     BEGIN    
   IF(@provider = '')    
             BEGIN    
                 SET @provider = NULL;    
             END;    
             ELSE    
             BEGIN    
                 SET @provider = @provider;    
             END;    
     SELECT CONVERT(VARCHAR, PTO_START_DATE) FROM WS_TBL_FOX_PTO APPOINTMENT_DATE     
  WHERE     
          (CONVERT(DATE, @APPOINMENT_DATE) BETWEEN CONVERT(DATE, PTO_START_DATE) AND CONVERT(DATE,PTO_END_DATE))    
    AND (@provider IS NULL    
               OR ProviderID = @PROVIDER)    
    AND ISNULL(DELETED, 0) = 0     
    AND PRACTICE_CODE = @PRACTICE_CODE    
     END;