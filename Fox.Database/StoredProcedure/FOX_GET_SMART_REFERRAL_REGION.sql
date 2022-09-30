IF (OBJECT_ID('FOX_GET_SMART_REFERRAL_REGION') IS NOT NULL ) DROP PROCEDURE FOX_GET_SMART_REFERRAL_REGION  
GO 
CREATE PROCEDURE [dbo].[FOX_GET_SMART_REFERRAL_REGION]  
(@PRACTICE_CODE BIGINT,   
 @SEARCHVALUE   VARCHAR(MAX)  
)  
AS  
     BEGIN  
         IF(@SEARCHVALUE = '')  
             BEGIN  
                 SET @SEARCHVALUE = NULL;  
             END;  
         SELECT TOP (100) *  
         FROM FOX_TBL_REFERRAL_REGION  
         WHERE(REFERRAL_REGION_CODE LIKE '%'+@SEARCHVALUE+'%'  
               OR dbo.FOX_TBL_REFERRAL_REGION.REFERRAL_REGION_NAME LIKE '%'+@SEARCHVALUE+'%')  
              AND ISNULL(DELETED, 0) = 0  
              AND PRACTICE_CODE = @PRACTICE_CODE;  
     END;  
----------------------------------------------------------------------------------------------------  
--Task 131676:Dev Task: Populate phone number for SLC  
----------------------------------------------------------------------------------------------------  
