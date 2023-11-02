IF(OBJECT_ID('FOX_PROC_UPDATE_PROVIDER_NAME') IS NOT NULL) DROP PROCEDURE FOX_PROC_UPDATE_PROVIDER_NAME  
GO 
-- Author:  < Aftab Khan >
--Create date: < 10 / 19 / 2023 >
--Description: < Update the Clinician for not associated survey>                
-- =============================================     

CREATE PROCEDURE FOX_PROC_UPDATE_PROVIDER_NAME   
-- Add the parameters for the stored procedure here
    @SURVEY_ID BIGINT,
    @PRACTICE_CODE BIGINT,
    @PROVIDER VARCHAR(255),  
    @MODIFIED_BY VARCHAR(255),  
    @PATIENT_ACCOUNT BIGINT  
AS  
BEGIN  
    UPDATE FOX_TBL_PATIENT_SURVEY   
    SET   
        PROVIDER = @PROVIDER,
        MODIFIED_BY = @MODIFIED_BY,
        MODIFIED_DATE = GETDATE();   
    WHERE   
        PATIENT_ACCOUNT_NUMBER = @PATIENT_ACCOUNT   
        AND SURVEY_ID = @SURVEY_ID  
END