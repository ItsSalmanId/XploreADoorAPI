
-- =============================================              
-- Author:  <Muhammad Salmamn>              
-- Create date: <09/09/2023>              
-- Description: <Description,>     
---FOX_PROC_GET_PATINET_CONTACT_DETAILS 554558, 1012714          
-- =============================================              
          
CREATE PROCEDURE FOX_PROC_GET_PATINET_CONTACT_DETAILS                     
 @SENT_TO_ID BIGINT ,     
  @PRACTICE_CODE BIGINT          
AS              
BEGIN              
                  
select First_Name,Last_Name, pct.Type_Name  From Fox_Tbl_Patient_Contacts pc  WITH(NOLOCK)      
left outer join Fox_Tbl_Patient_Contact_Types pct WITH(NOLOCK) on  pct.Contact_Type_Id = pc.Contact_Type_Id        
where         
Contact_ID = @SENT_TO_ID and PRACTICE_CODE = @PRACTICE_CODE        
and ISNULL(pc.DELETED, 0) = 0        
and ISNULL(pct.DELETED, 0) = 0        
           
END