IF (OBJECT_ID('FOX_PROC_GET_TreatmentLocation_By_Zip') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TreatmentLocation_By_Zip  
GO 

CREATE PROCEDURE [dbo].[FOX_PROC_GET_TreatmentLocation_By_Zip] @zip           VARCHAR(100), 
                                                              @PRACTICE_CODE BIGINT
AS
     BEGIN
         SET NOCOUNT ON;
         SELECT TOP 1 LOC_ID, 
                      CODE, 
                      NAME, 
                      ISNULL(Address, '') AS 'Address', 
                      Zip, 
                      City, 
                      STATE, 
                      Phone, 
                      REGION, 
                      ISNULL(Description, '') AS 'Description', 
                      Country, 
                      ISNULL(Work_Phone, '') AS 'Work_Phone', 
                      ISNULL(Cell_Phone, '') AS 'Cell_Phone', 
                      ISNULL(Email_Address, '') AS 'Email_Address', 
                      ISNULL(Fax, '') AS 'Fax', 
                      FACILITY_TYPE_ID, 
                      CREATED_DATE
         FROM FOX_TBL_ACTIVE_LOCATIONS
         WHERE(LEN(@zip) = 5
               AND REPLACE(zip, '-', '') LIKE '%'+@zip+'%')
              OR (LEN(@zip) = 9
                  AND REPLACE(zip, '-', '') LIKE '%'+@zip+'%')
              AND PRACTICE_CODE = @PRACTICE_CODE
         ORDER BY CREATED_DATE DESC;
     END;
