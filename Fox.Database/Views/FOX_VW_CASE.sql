USE [MIS_DB]
GO

/****** Object:  View [dbo].[FOX_VW_CASE]    Script Date: 7/25/2022 1:56:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[FOX_VW_CASE]              
AS        
     SELECT DISTINCT c.CASE_ID,               
            c.PRACTICE_CODE,               
            c.PATIENT_ACCOUNT,               
            c.CASE_TYPE_ID,        
   c.IsWellness,        
   c.IsSkilled,  
   c.DISCHARGE_DATE,              
            ct.NAME AS CASE_TYPE_NAME,               
            d.DISCIPLINE_ID,               
            d.NAME AS DISCIPLINE_NAME,               
            c.CASE_NO,               
            c.RT_CASE_NO,             
            c.CASE_STATUS_ID,        
   c.PATIENT_RESP_INS_ID,               
            cs.NAME AS CASE_STATUS_NAME,               
            cs.STATUS_COLOR AS CASE_STATUS_COLOR,               
            c.CASE_SUFFIX_ID,               
            c.GROUP_IDENTIFIER_ID,               
            c.TREATING_PROVIDER_ID,        
   cs2.NAME AS CASE_SUFFIX_NAME,        
            p.LAST_NAME+', '+p.FIRST_NAME+(CASE              
                                                    WHEN ISNULL(p.INDIVIDUAL_NPI, '') <> ''              
                                                    THEN ' | NPI: '+p.INDIVIDUAL_NPI              
                                                    ELSE ''              
                                                END) AS TreatingProviderName,                 
            --tp.NOTES AS TreatingProviderNotes,                 
            c.POS_ID,      
   CASE       
    WHEN loc.NAME LIKE '%Private%Home'          
    THEN loc.CODE +  ' ' +  loc.NAME        
    ELSE loc.NAME      
    END AS PosLocation,               
            reg.REFERRAL_REGION_ID AS POSRegionID,               
            ISNULL(reg.REFERRAL_REGION_CODE, '') AS POSRegionCode,               
            ISNULL(reg.REFERRAL_REGION_NAME, '') AS POSRegionName,               
            c.TREATING_REGION_ID,               
            tr.REFERRAL_REGION_NAME AS TREATING_REFERRAL_REGION,               
            c.IS_MANUAL_CHANGE_REGION,               
            c.ADMISSION_DATE,               
            c.START_CARE_DATE,               
            c.END_CARE_DATE,               
            c.VISIT_PER_WEEK,               
            c.NO_OF_WEEK,               
            c.TOTAL_VISITS,               
            c.WORK_ID,               
            c.ORDERING_REF_SOURCE_ID,               
            ors.LAST_NAME+', '+ors.FIRST_NAME+(CASE              
                                                   WHEN ors.REFERRAL_REGION <> NULL              
                                                   THEN ors.REFERRAL_REGION              
                                                   ELSE ''              
                                               END) AS OrderingRefName,               
            ors.NOTES AS OrderingRefNotes,               
            c.ACU_IDENTIFIER_ID,               
            acoi.NAME AS ACOIdentifier,    
   acoi.CODE AS ACOCode,    
   acoi.DESCRIPTION AS ACODescription,               
            c.HOSPITAL_IDENTIFIER_ID,               
            hi.NAME AS HospitalIdentifier,    
   hi.CODE AS HospitalCode,    
   hi.DESCRIPTION AS HospitalDescription,               
            c.SNF_IDENTIFIER_ID,               
            snfi.NAME AS SNFIdentifier,    
   snfi.CODE AS SNFCode,    
   snfi.DESCRIPTION AS SNFDescription,               
            c.HHH_IDENTIFIER_ID,               
   hhhi.NAME AS HHHIdentifier,    
   hhhi.CODE AS HHHCode,    
            hhhi.DESCRIPTION AS HHHDescription,               
            c.REF_REGION_ID,               
            ref.REFERRAL_REGION_NAME AS REFERRAL_REGION,               
            c.SOURCE_OF_REFERRAL_ID,               
            c.PRIMARY_PHY_ID,               
            pp.LAST_NAME+', '+pp.FIRST_NAME+' | '+pp.REFERRAL_REGION AS PriamryPhy,               
            c.CERTIFYING_REF_SOURCE_ID,      
           CASE              
                WHEN crs.REFERRAL_REGION IS NULL              
                THEN crs.FIRST_NAME+' '+crs.LAST_NAME              
            ELSE(crs.FIRST_NAME+' '+crs.LAST_NAME+' | '+crs.REFERRAL_REGION)              
            END AS CertifyRefSource,               
            crs.LAST_NAME+', '+crs.FIRST_NAME AS CertifyFullName,               
            crs.NOTES AS CertifySourceNotes,               
            crs.PHONE AS CertifyPhone,               
            crs.FAX AS CaertifyFax,  
            crs.ADDRESS AS CertifyAddress,  
            crs.ZIP AS CertifyZip,  
            crs.CITY AS CertifyCity,  
            crs.STATE AS CertifyState,  
   w.UNIQUE_ID,               
            c.CREATED_BY,               
   c.CREATED_DATE,               
            c.MODIFIED_BY,               
            c.MODIFIED_DATE,               
            c.DELETED,               
            c.HEAR_ABOUT_US_ID,      
   c.HOLD_DATE,      
   c.HOLD_TILL_DATE,      
   c.HOLD_DURATION,  
   c.HOLD_FOLLOW_UP_DATE,
   c.NON_ADMIT_REASON,
   hauo.FOX_TBL_HEAR_ABOUT_US_OPTION_ID,  
   subtask.RT_CODE AS HOLD_RT_CODE,  
            CASE              
   WHEN c.HEAR_ABOUT_US_ID IS NULL          
                THEN ''              
                ELSE '['+hauo.CODE+']'+' '+hauo.NAME              
            END AS HEAR_ABOUT_US_DISPLAY_TEXT              
     FROM dbo.FOX_TBL_CASE AS c WITH(NOLOCK)              
          LEFT OUTER JOIN dbo.FOX_TBL_CASE_TYPE AS ct ON ct.CASE_TYPE_ID = c.CASE_TYPE_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_CASE_STATUS AS cs ON cs.CASE_STATUS_ID = c.CASE_STATUS_ID              
    LEFT OUTER JOIN dbo.FOX_TBL_CASE_SUFFIX AS cs2 ON cs2.CASE_SUFFIX_ID = c.CASE_SUFFIX_ID             
          LEFT OUTER JOIN dbo.FOX_TBL_PROVIDER AS p ON p.FOX_PROVIDER_ID = c.TREATING_PROVIDER_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_ORDERING_REF_SOURCE AS pp ON pp.SOURCE_ID = c.PRIMARY_PHY_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_ORDERING_REF_SOURCE AS crs ON crs.SOURCE_ID = c.CERTIFYING_REF_SOURCE_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_REFERRAL_REGION AS tr ON tr.REFERRAL_REGION_ID = c.TREATING_REGION_ID AND tr.PRACTICE_CODE = c.PRACTICE_CODE                 
          LEFT OUTER JOIN dbo.FOX_TBL_ORDERING_REF_SOURCE AS ors ON ors.SOURCE_ID = c.ORDERING_REF_SOURCE_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_REFERRAL_REGION AS ref ON ref.REFERRAL_REGION_ID = c.REF_REGION_ID AND ref.PRACTICE_CODE = c.PRACTICE_CODE                 
          LEFT OUTER JOIN dbo.FOX_TBL_ACTIVE_LOCATIONS AS loc ON loc.LOC_ID = c.POS_ID  and c.PRACTICE_CODE =  loc.PRACTICE_CODE and    ISNULL(loc.deleted, 0)= 0           
          LEFT OUTER JOIN dbo.FOX_TBL_IDENTIFIER AS acoi ON acoi.IDENTIFIER_ID = c.ACU_IDENTIFIER_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_IDENTIFIER AS hi ON hi.IDENTIFIER_ID = c.HOSPITAL_IDENTIFIER_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_IDENTIFIER AS snfi ON snfi.IDENTIFIER_ID = c.SNF_IDENTIFIER_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_IDENTIFIER AS hhhi ON hhhi.IDENTIFIER_ID = c.HHH_IDENTIFIER_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_DISCIPLINE AS d ON d.DISCIPLINE_ID = c.DISCIPLINE_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_WORK_QUEUE AS w ON w.WORK_ID = c.WORK_ID              
          LEFT OUTER JOIN dbo.FOX_TBL_REFERRAL_REGION AS reg ON reg.REFERRAL_REGION_CODE = loc.REGION  AND reg.PRACTICE_CODE = loc.PRACTICE_CODE AND    ISNULL(reg.deleted, 0)= 0      
          LEFT OUTER JOIN dbo.FOX_TBL_HEAR_ABOUT_US_OPTIONS AS hauo ON hauo.FOX_TBL_HEAR_ABOUT_US_OPTION_ID = c.HEAR_ABOUT_US_ID  
    LEFT OUTER JOIN dbo.FOX_TBL_TASK_SUB_TYPE AS subtask ON c.HOLD_DURATION = subtask.NAME and isnull(subtask.DELETED,0) = 0 and  c.PRACTICE_CODE  = subtask.PRACTICE_CODE; 


GO

