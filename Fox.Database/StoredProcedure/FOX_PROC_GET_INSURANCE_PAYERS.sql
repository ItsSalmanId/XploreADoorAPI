IF (OBJECT_ID('FOX_PROC_GET_INSURANCE_PAYERS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_INSURANCE_PAYERS  
GO   
  CREATE Procedure [dbo].[FOX_PROC_GET_INSURANCE_PAYERS ] --'','medi'        
(        
 @PatientState varchar(50),    
 @Insurance_Description varchar(100)        
)        
AS   
--declare        
-- @PatientState varchar(50) = '',    
-- @Insurance_Description varchar(100) = '* Tricare For Life tertiary'        
BEGIN        
 IF(@Insurance_Description='')        
  BEGIN        
   SET @Insurance_Description = NULL        
  END        
 ELSE        
  BEGIN        
   SET @Insurance_Description ='%'+ @Insurance_Description + '%'        
  END        
    
if(@PatientState = '')    
begin    
SELECT top 100   
 ii.FOX_TBL_INSURANCE_ID,   
 ip.InsPayer_Id,   
 i.Insurance_Id,   
 ii.INSURANCE_NAME as InsPayer_Description,    
 case when isnull(ii.ADDRESS,'') <> ''  
  then ii.ADDRESS  
  when isnull(i.Insurance_Address,'') <> ''  
  then i.Insurance_Address  
  else ''  
  end  
  as Insurance_Address,  
  
 case when isnull(ii.PHONE,'') <> ''  
  then ii.PHONE  
  when isnull(i.Insurance_Phone_Number1,'') <> ''  
  then i.Insurance_Phone_Number1  
  else ''  
  end  
  as Insurance_Phone_Number1,  
  
 isnull(i.Insurance_Phone_Number2,'') as FAX,  
  
  case when isnull(ii.ZIP,'') <> ''  
  then ii.ZIP  
  when isnull(i.Insurance_Zip,'') <> ''  
  then i.Insurance_Zip  
  else ''  
  end  
  as Insurance_Zip,  
      
 isnull(i.Insurance_City,'') as Insurance_City,  
     
 case when isnull(ii.STATE,'') <> ''  
  then ii.STATE  
  when isnull(i.Insurance_State,'') <> ''  
  then i.Insurance_State  
  else ''  
  end  
  as Insurance_State  
  
 FROM  FOX_TBL_INSURANCE ii   
 left join INSURANCES i on i.Insurance_Id = convert(bigint,ii.INSURANCE_ID)    
 left join INSURANCE_PAYERS ip on ip.InsPayer_Id = i.InsPayer_Id  
 WHERE (@Insurance_Description IS NULL OR ii.INSURANCE_NAME LIKE '%' + @Insurance_Description + '%') and ip.InsPayer_Id is not null and i.INSURANCE_ID is not null     
end    
else    
 SELECT top 100   
ii.FOX_TBL_INSURANCE_ID,   
 ip.InsPayer_Id,   
 i.Insurance_Id,   
 ii.INSURANCE_NAME as InsPayer_Description,    
 case when isnull(ii.ADDRESS,'') <> ''  
  then ii.ADDRESS  
  when isnull(i.Insurance_Address,'') <> ''  
  then i.Insurance_Address  
  else ''  
  end  
  as Insurance_Address,  
  
 case when isnull(ii.PHONE,'') <> ''  
  then ii.PHONE  
  when isnull(i.Insurance_Phone_Number1,'') <> ''  
  then i.Insurance_Phone_Number1  
  else ''  
  end  
  as Insurance_Phone_Number1,  
  
 isnull(i.Insurance_Phone_Number2,'') as FAX,  
  
  case when isnull(ii.ZIP,'') <> ''  
  then ii.ZIP  
  when isnull(i.Insurance_Zip,'') <> ''  
  then i.Insurance_Zip  
  else ''  
  end  
  as Insurance_Zip,  
      
 isnull(i.Insurance_City,'') as Insurance_City,  
     
 case when isnull(ii.STATE,'') <> ''  
  then ii.STATE  
  when isnull(i.Insurance_State,'') <> ''  
  then i.Insurance_State  
  else ''  
  end  
  as Insurance_State  
  
 FROM  FOX_TBL_INSURANCE ii   
 left join INSURANCES i on i.Insurance_Id = convert(bigint,ii.INSURANCE_ID)    
 left join INSURANCE_PAYERS ip on ip.InsPayer_Id = i.InsPayer_Id  
 WHERE  ( @Insurance_Description IS NULL OR ii.INSURANCE_NAME LIKE '%' + @Insurance_Description + '%') and ii.STATE = @PatientState  
   and ip.InsPayer_Id is not null and i.INSURANCE_ID is not null           
end    
