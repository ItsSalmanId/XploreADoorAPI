IF (OBJECT_ID('FOX_PROC_GET_MTBCSOFT_WEB_CASE_STATUS_PREV_COMMENTS') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_MTBCSOFT_WEB_CASE_STATUS_PREV_COMMENTS  
GO 
CREATE PROCEDURE [dbo].[FOX_PROC_GET_MTBCSOFT_WEB_CASE_STATUS_PREV_COMMENTS]                  
(                      
  @Caseno Varchar(30)                 
)                      
AS                      
Select                       
case isnull(user_fname,'')+' '+isnull(user_lname,'')                      
when '' then upper(isnull(CS_User_Id,''))                      
else upper(isnull(CS_User_Id,'')+' - '+isnull(user_fname,'')+' '+isnull(user_lname,''))                       
end as user_name,                      
convert(varchar,CUCR.CS_Created_Date,22) as CS_Created_Date, UPPER(CUCR.CS_Created_By) AS CS_Created_By, CS_User_Response, CS_User_Client_Response_ID,CUCR.cs_authorize,                       
CS_Show_On_Web = CAST((case CS_Show_On_Web                       
when 'Y' then 1                      
else 0                      
end) as bit),                  
'Delete' [Delete] ,isnull(CUCR.new_message,0) new_message,              
--Added by Adeel Shahid as per CMF 1197                
--Added by Raheel Mamoon as per cmf # 1353              
auto_generated_resp,case convert(varchar,CUCR.CS_Expected_Closing_Date,101) when '01/01/1900' then '' else convert(varchar,CUCR.CS_Expected_Closing_Date,101) end CS_Expected_Closing_Date,          
CASE           
when Exists(select  top 1 user_fname + ' '  + user_lname as Name from users t           
where CUCR.CS_Case_No like '%'+@Caseno+'%' and CUCR.CS_User_Id= t.User_Id)          
Then  'MTBC'           
when  Exists(select  top 1 first_name + ' '  + last_name   as Name from Web_Users t           
 where CUCR.CS_Case_No like '%'+@Caseno+'%' and CUCR.CS_User_Id= t.user_name)          
 Then  'WEB'           
           
ELSE          
'Not Exist'          
 End User_Type,Department_Name,CS_Practice_Code          
           
--Added by Raheel Mamoon as per cmf # 1353                  
--End Added by Adeel Shahid as per CMF 1197                
From CS_User_Client_Response CUCR with (nolock)                           
left outer join users SU with (nolock) on su.user_id = CUCR.cs_user_id              
left outer join Department D with (nolock) on D.Department_Id= su.Department_Id           
left outer join CS_Case_Progress CCP with (nolock) on CCP.CS_Case_No = CUCR.CS_Case_No                           
Where CUCR.cs_case_no = @Caseno                     
and isnull(cucr.cs_deleted,0)=0                     
Order by CUCR.CS_Created_Date desc 
