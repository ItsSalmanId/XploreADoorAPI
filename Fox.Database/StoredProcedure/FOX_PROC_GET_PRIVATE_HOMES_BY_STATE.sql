IF (OBJECT_ID('FOX_PROC_GET_PRIVATE_HOMES_BY_STATE') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PRIVATE_HOMES_BY_STATE  
GO
CREATE PROC [dbo].[FOX_PROC_GET_PRIVATE_HOMES_BY_STATE] @STATE           VARCHAR(100), 
                                                @PRACTICE_CODE   BIGINT, 
                                                @PATIENT_ACCOUNT BIGINT
AS
     BEGIN
         SELECT ftal.LOC_ID, 
                ISNULL(ftal.REGION, ftpa.POS_REGION) REGION, 
                ftal.CODE, 
                ftal.NAME, 
                ISNULL(ftal.Address, ftpa.ADDRESS) Address, 
                ISNULL(ftal.City, ftpa.CITY) City, 
                ISNULL(ftal.State, ftpa.[STATE]) State, 
                ISNULL(ftal.Zip, ftpa.ZIP) Zip, 
                ISNULL(ftal.Phone, ftpa.POS_Phone) Phone, 
                ISNULL(ftal.Fax, ftpa.POS_Fax) Fax, 
                ftal.POS_Code, 
                ftal.FOL, 
                ftal.FACILITY_TYPE_ID, 
                ftal.Capacity, 
                ftal.Census, 
                ftal.PT, 
                ftal.OT, 
                ftal.ST, 
                ftal.EP, 
                ftal.LEAD, 
                ftal.Parent, 
                ftal.Description, 
                ftal.Last_Update, 
                ftal.CREATED_BY, 
                ftal.CREATED_DATE, 
                ftal.MODIFIED_BY, 
                ftal.MODIFIED_DATE, 
                ftal.DELETED, 
                ISNULL(ftal.Country, ftpa.POS_County) Country, 
                ftal.PRACTICE_CODE, 
                ISNULL(ftal.Work_Phone, ftpa.POS_Work_Phone) Work_Phone, 
                ISNULL(ftal.Cell_Phone, ftpa.POS_Cell_Phone) Cell_Phone, 
                ISNULL(ftal.Email_Address, ftpa.POS_Email_Address) Email_Address, 
                ftal.IS_PRIVATE_HOME, 
                ftal.IS_ACTIVE,
                ftal.PT_PROVIDER_ID, 
                ftal.OT_PROVIDER_ID, 
                ftal.ST_PROVIDER_ID, 
                ftal.EP_PROVIDER_ID, 
                ftpa.PATIENT_POS_ID, 
                ftpp.Is_Void, 
                ftpp.Is_Default, 
                ftpp.Effective_From, 
                ftpp.Effective_To
         FROM dbo.FOX_TBL_ACTIVE_LOCATIONS ftal
              JOIN dbo.FOX_TBL_FACILITY_TYPE ftft ON FTAL.FACILITY_TYPE_ID = FTFT.FACILITY_TYPE_ID
                                                     AND LOWER(FTFT.NAME) = 'private home'
                                                     AND ftft.PRACTICE_CODE = @PRACTICE_CODE
              RIGHT JOIN dbo.Fox_Tbl_Patient_POS ftpp ON ftal.LOC_ID = ftpp.Loc_ID
                                                         AND ftpp.Patient_Account = @PATIENT_ACCOUNT
                                                         AND ISNULL(FTPP.Deleted, '') = 0
              LEFT JOIN dbo.FOX_TBL_PATIENT_ADDRESS ftpa ON FTPP.Patient_POS_ID = FTPA.PATIENT_POS_ID
         WHERE ISNULL(ftal.DELETED, '') = 0
               AND ftal.PRACTICE_CODE = @PRACTICE_CODE
               AND LOWER(SUBSTRING(ftal.CODE, 1, 2)) = LOWER(@STATE);
     END;

---------------------------------------------------------------------------------

