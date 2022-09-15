IF (OBJECT_ID('Fox_Get_Patient_Info') IS NOT NULL ) DROP PROCEDURE Fox_Get_Patient_Info  
GO 
--EXEC [Fox_Get_Patient_Info] 'Ahmed','one','','','','','',1011163              
CREATE PROCEDURE [dbo].[Fox_Get_Patient_Info] @Last_Name                VARCHAR(50),           
                                      @First_Name               VARCHAR(50),           
                                      @Middle_Name              VARCHAR(50),           
                                      @SSN                      VARCHAR(9),           
                                      @Gender                   VARCHAR(15),           
                                      @Date_Of_Birth            DATE,           
                                      @Chart_Id                 VARCHAR(100),           
                                      @PRACTICE_CODE            BIGINT,           
                                      @PRACTICE_ORGANIZATION_ID BIGINT                 
AS          
     IF(@PRACTICE_ORGANIZATION_ID = 0)          
         BEGIN          
             SET @PRACTICE_ORGANIZATION_ID = NULL;          
         END;          
     IF(@Last_Name = '')          
         BEGIN          
             SET @Last_Name = NULL;          
         END;          
         ELSE          
         BEGIN          
             SET @Last_Name = '%'+@Last_Name+'%';          
         END;          
     IF(@First_Name = '')          
         BEGIN          
             SET @First_Name = NULL;          
         END;          
         ELSE          
         BEGIN          
             SET @First_Name = '%'+@First_Name+'%';          
         END;          
     IF(@Middle_Name = '')          
         BEGIN          
             SET @Middle_Name = NULL;          
         END;          
         ELSE          
         BEGIN          
             SET @Middle_Name = '%'+@Middle_Name+'%';          
         END;          
     IF(@SSN = '')          
         BEGIN          
             SET @SSN = NULL;          
         END;          
         ELSE          
         BEGIN          
             SET @SSN = '%'+@SSN+'%';          
         END;          
     IF(@Gender = '')          
         BEGIN          
             SET @Gender = NULL;          
         END;          
         ELSE          
         BEGIN          
             SET @Gender = @Gender;          
         END;          
     IF(@Date_Of_Birth = '')          
         BEGIN          
             SET @Date_Of_Birth = NULL;          
         END;          
         ELSE          
         BEGIN          
             SET @Date_Of_Birth = @Date_Of_Birth;          
         END;          
     IF(@Chart_Id = '')          
         BEGIN          
             SET @Chart_Id = NULL;          
         END;          
         ELSE          
         BEGIN          
             SET @Chart_Id = '%'+@Chart_Id+'%';          
         END;        
     
   --  
        
   IF (ISNULL(@Last_Name, '') <> '' AND ISNULL(@SSN, '') <> '') OR (ISNULL(@Last_Name, '') <> '' AND ISNULL(@Date_Of_Birth, '') <> '')  
         BEGIN          
             SET @PRACTICE_ORGANIZATION_ID = NULL;          
         END;    
   --    
  
     BEGIN          
         SELECT TOP (100) ROW_NUMBER() OVER(ORDER BY SSN) AS ROW,           
                          First_Name,           
                          Last_Name,           
                          CONVERT(VARCHAR(100), p.Patient_Account) AS Patient_Account,           
                          RTRIM(LTRIM(SSN)) AS SSN,          
                          CASE UPPER(Gender)          
                              WHEN 'M'          
                              THEN 'Male'          
                              WHEN 'MALE'          
                              THEN 'Male'          
                              WHEN 'F'          
                              THEN 'Female'          
                              WHEN 'FEMALE'          
                        THEN 'Female'          
                          END GENDER,           
                          CONVERT(VARCHAR, Date_Of_Birth, 101) AS Date_Of_Birth,           
                 Chart_Id AS MRN,           
                          isnull(loc.ZIP, '') AS ZIP,           
                          isnull(loc.City, '') AS City,           
                          isnull(loc.STATE, '') AS STATE,           
                          isnull(loc.Address, '') AS HomeAddress,          
          
                          --CASE WHEN LEN(loc.City) > 0  THEN loc.City   ELSE '' END as   City,            
          --CASE WHEN LEN(loc.STATE) > 0  THEN loc.[STATE]   ELSE '' END as   [STATE],            
                          MIDDLE_NAME,              
                          -- CASE  WHEN LEN(pa.Address) > 0  THEN pa.Address ELSE '' END AS HomeAddress,               
                          --                  END            
                          --+CASE              
 --                          WHEN LEN(pa.City) > 0              
                          --                          THEN pa.City+', '              
                          --                          ELSE ''              
                      --                      END+CASE              
                          --                       WHEN LEN(pa.STATE) > 0              
                          --                              THEN pa.STATE+', '              
                          --                              ELSE ''              
                          --                          END+CASE              
                          --                                  WHEN LEN(pa.ZIP) > 0              
                          --                                  THEN pa.ZIP              
                          --                                  ELSE ''              
                          --                              END+CASE              
                          --                                      WHEN LEN(pa.Country) > 0              
                          --                                      THEN ', '+pa.Country              
                          --                                      ELSE ''              
          
                          p.Cell_Phone,           
                          Business_Phone,           
                          p.Home_Phone,           
                          t.INSURANCE_NAME AS PrimaryInsurance,           
                          p.Email_Address,           
                          fp.Title,           
                          fp.Fax_Number,       
        fp.PRACTICE_ORGANIZATION_ID  
         FROM patient AS p WITH(NOLOCK)          
              LEFT JOIN FOX_TBL_PATIENT_ADDRESS AS pa ON pa.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT          
                                                         AND pa.ADDRESS_TYPE LIKE 'Private Home'          
                                                         AND ISNULL(pa.DELETED, 0) = 0          
          
              LEFT JOIN Fox_Tbl_Patient_POS AS pos ON pos.Patient_POS_ID =          
         (          
             SELECT TOP 1 Patient_POS_ID          
             FROM Fox_Tbl_Patient_POS          
             WHERE ISNULL(Patient_Account,'') = p.Patient_Account and loc_id<>0 and isnull(deleted,0)=0          
   order by Patient_Account,Created_Date          
         )          
          
              LEFT JOIN FOX_TBL_ACTIVE_LOCATIONS AS loc ON loc.Loc_ID = pos.Loc_ID              
                                                           -- AND pa.ADDRESS_TYPE LIKE 'Private Home'              
                                                           AND ISNULL(loc.DELETED, 0) = 0          
              LEFT JOIN FOX_TBL_PATIENT AS fp ON fp.PATIENT_ACCOUNT = p.PATIENT_ACCOUNT          
        AND ISNULL(fp.DELETED, 0) = 0    
              LEFT JOIN          
         (          
             SELECT DISTINCT           
                    PI.Patient_Account,           
                    fi.INSURANCE_NAME          
             FROM FOX_TBL_PATIENT_INSURANCE AS PI          
            JOIN FOX_TBL_INSURANCE AS fi ON fi.FOX_TBL_INSURANCE_ID = PI.FOX_TBL_INSURANCE_ID          
             WHERE PI.Pri_Sec_Oth_Type = 'P'          
                   AND ISNULL(PI.Deleted, 0) = 0       
         ) t ON t.Patient_Account = p.Patient_Account        
         WHERE p.practice_code = @PRACTICE_CODE          
               AND (@FIRST_NAME IS NULL          
                    OR FIRST_NAME LIKE '%'+@FIRST_NAME+'%')          
               AND (@SSN IS NULL          
                    OR SSN LIKE '%'+@SSN+'%')          
               AND (@GENDER IS NULL          
                 OR CASE UPPER(Gender)          
                           WHEN 'M'          
                           THEN 'MALE'          
                           WHEN 'MALE'          
                           THEN 'MALE'          
                           WHEN 'F'          
                           THEN 'FEMALE'         
                           WHEN 'FEMALE'          
                           THEN 'FEMALE'          
                       END = UPPER(@GENDER))          
               AND (@LAST_NAME IS NULL          
                    OR LAST_NAME LIKE '%'+@LAST_NAME+'%')          
               AND (@MIDDLE_NAME IS NULL          
    OR MIDDLE_NAME LIKE '%'+@MIDDLE_NAME+'%')          
               AND (@DATE_OF_BIRTH IS NULL          
                    OR CONVERT(DATE, DATE_OF_BIRTH) = @DATE_OF_BIRTH)          
               AND (@Chart_Id IS NULL          
                    OR Chart_Id LIKE '%'+@Chart_Id+'%')          
                AND (@PRACTICE_ORGANIZATION_ID IS NULL          
                     OR fp.PRACTICE_ORGANIZATION_ID = @PRACTICE_ORGANIZATION_ID          
                     OR ISNULL(fp.PRACTICE_ORGANIZATION_ID, 1) = 1)          
               AND ISNULL(p.Deleted, 0) = 0;          
     END; 
