IF (OBJECT_ID('FOX_GET_SMART_ORDERING_SOURCE') IS NOT NULL ) DROP PROCEDURE FOX_GET_SMART_ORDERING_SOURCE  
GO               
--[dbo].FOX_GET_SMART_ORDERING_SOURCE 1011163,'TES'          
CREATE PROCEDURE [dbo].FOX_GET_SMART_ORDERING_SOURCE        
@PRACTICE_CODE BIGINT,               
@SEARCHVALUE   VARCHAR(200)                         
--DECLARE @PRACTICE_CODE BIGINT = 1011163,               
-- @SEARCHVALUE   VARCHAR(200) = 'TES'              
AS              
     BEGIN              
         IF(@SEARCHVALUE = '')              
             BEGIN              
                 SET @SEARCHVALUE = NULL;              
             END;              
             ELSE              
         IF(CHARINDEX(' | ', @SEARCHVALUE) > 0)              
             BEGIN              
                 SET @SEARCHVALUE = replace(@SEARCHVALUE, ' | ', ' ');              
             END;              
         SELECT TOP (100) SOURCE_ID,               
                          s.CODE,               
                          s.REFERRAL_REGION,              
                          CASE              
                              WHEN s.REFERRAL_REGION IS NULL              
                              THEN LTRIM(RTRIM(s.FIRST_NAME))+' '+LTRIM(RTRIM(s.LAST_NAME))              
                              ELSE(LTRIM(RTRIM(s.FIRST_NAME))+' '+LTRIM(RTRIM(s.LAST_NAME))+' | '+LTRIM(RTRIM(s.REFERRAL_REGION)))              
                          END AS NAME,               
                          s.FIRST_NAME,               
                          s.LAST_NAME,               
                          s.[ZIP],               
                          s.[ADDRESS],               
                          s.CITY,               
                          s.[STATE],               
                          s.PHONE,               
                          s.NPI,               
                          s.FAX,               
                          s.INACTIVE_REASON_ID,               
                          s.NOTES,        
        s.Email,               
                          isnull(s.ACO, '') AS REFERRAL_SOURCE_ACO,               
                          s.ACO_ID AS REFERRAL_SOURCE_ACO_ID,               
                          isnull(i.NAME, '') ACO_NAME,               
                          isnull(po.NAME, '') PRACTICE_ORGANIZATION_NAME,               
                          AU.ROLE_ID,               
                          ISNULL(AU.USER_TYPE, '') USER_TYPE,               
                          AU.ACO,               
                          id1.NAME AS ACO_NAME_TEXT,            
        id1.CODE AS ACO_CODE,            
        id1.DESCRIPTION AS ACO_DESCRIPTION,               
                          AU.HOSPITAL,               
                          id2.NAME AS HOSPITAL_NAME_TEXT,            
        id2.CODE AS HOSPITAL_CODE,            
        id2.DESCRIPTION AS HOSPITAL_DESCRIPTION,               
                          AU.SNF,               
                          id3.NAME AS SNF_NAME_TEXT,            
        id3.NAME AS SNF_CODE,            
        id3.NAME AS SNF_DESCRIPTION,               
                          AU.HHH,               
                          id4.NAME AS HHH_NAME_TEXT,            
        id4.NAME AS HHH_CODE,            
        id4.NAME AS HHH_DESCRIPTION            
         FROM FOX_TBL_ORDERING_REF_SOURCE s              
              LEFT JOIN FOX_TBL_IDENTIFIER i ON i.IDENTIFIER_ID = s.ACO_ID              
              LEFT JOIN FOX_TBL_PRACTICE_ORGANIZATION po ON po.PRACTICE_ORGANIZATION_ID = s.PRACTICE_ORGANIZATION_ID              
              LEFT JOIN FOX_TBL_APPLICATION_USER AU ON AU.USR_REFERRAL_SOURCE_ID = s.SOURCE_ID              
              LEFT JOIN FOX_TBL_IDENTIFIER id1 ON id1.IDENTIFIER_ID = AU.ACO              
              LEFT JOIN FOX_TBL_IDENTIFIER id2 ON id2.IDENTIFIER_ID = AU.HOSPITAL              
              LEFT JOIN FOX_TBL_IDENTIFIER id3 ON id3.IDENTIFIER_ID = AU.SNF              
              LEFT JOIN FOX_TBL_IDENTIFIER id4 ON id4.IDENTIFIER_ID = AU.HHH              
         WHERE(LTRIM(RTRIM(s.FIRST_NAME)) LIKE '%'+@SEARCHVALUE+'%'              
               OR LTRIM(RTRIM(s.LAST_NAME)) LIKE '%'+@SEARCHVALUE+'%'                           OR LTRIM(RTRIM(s.FIRST_NAME))+' '+LTRIM(RTRIM(s.LAST_NAME)) LIKE '%'+@SEARCHVALUE+'%'              
               OR LTRIM(RTRIM(s.FIRST_NAME))+' '+LTRIM(RTRIM(s.LAST_NAME))+' '+LTRIM(RTRIM(S.REFERRAL_REGION)) LIKE '%'+@SEARCHVALUE+'%'            
               OR LTRIM(RTRIM(s.NPI)) LIKE '%'+@SEARCHVALUE+'%')              
              AND ISNULL(s.deleted, 0) = 0              
              AND s.PRACTICE_CODE = @PRACTICE_CODE              
     AND s.INACTIVE_REASON_ID IS NULL;              
              
END 

