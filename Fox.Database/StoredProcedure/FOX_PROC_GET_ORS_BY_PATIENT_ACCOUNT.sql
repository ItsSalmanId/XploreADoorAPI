IF (OBJECT_ID('FOX_PROC_GET_ORS_BY_PATIENT_ACCOUNT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ORS_BY_PATIENT_ACCOUNT  
GO    
-- =============================================              
-- Author:  <Author,Abdur Rafay>              
-- Create date: <Create Date,06/22/2021>              
-- DESCRIPTION: <GET ORS BY PATIENT ACCOUNT>                  
-- [FOX_PROC_GET_ORS_BY_PATIENT_ACCOUNT] 101271453412157, 1012714              
CREATE PROCEDURE [dbo].[FOX_PROC_GET_ORS_BY_PATIENT_ACCOUNT](        
@PATIENT_ACCOUNT BIGINT,         
@PRACTICE_CODE BIGINT)          
AS          
     BEGIN          
         SET NOCOUNT ON;          
   SELECT TOP 1  sour.SOURCE_ID AS SOURCE_ID , sour.NPI AS NPI       
   FROM FOX_TBL_CASE cas    
       LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE sour on cas.ORDERING_REF_SOURCE_ID = sour.SOURCE_ID     
           AND sour.PRACTICE_CODE = 1012714 AND ISNULL(sour.DELETED,0) = 0    
   WHERE PATIENT_ACCOUNT = @PATIENT_ACCOUNT     
    AND  ISNULL(cas.ORDERING_REF_SOURCE_ID, '') <> ''    
    AND cas.PRACTICE_CODE = @PRACTICE_CODE     
    AND ISNULL(cas.DELETED,0) = 0    
   ORDER BY cas.CREATED_DATE DESC         
     END; 