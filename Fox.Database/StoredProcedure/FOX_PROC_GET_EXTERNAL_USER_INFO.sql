-- =============================================                        
-- AUTHOR:  <Muhammad Iqbal>                        
-- CREATE DATE: <CREATE DATE, 15/05/2023>                        
-- DESCRIPTION: <GET PATIENT EXTERNAL USER LIST>                                      
-- [dbo].[FOX_PROC_SAVE_EXTERNAL_USER_INFO] 1012714, '', '','','','','', 1012714       
CREATE PROCEDURE FOX_PROC_GET_EXTERNAL_USER_INFO               
@FOX_FRICTIONLESS_UNAVAILABLE_ID BIGINT,                              
@PRACTICE_CODE  BIGINT                    
AS     
BEGIN                    
Select FOX_FRICTIONLESS_UNAVAILABLE_ID From FOX_TBL_FRICTIONLESS_SERVICE_UNAVAILABLE_ZIPCODE WITH (NOLOCK) where FOX_FRICTIONLESS_UNAVAILABLE_ID =  @FOX_FRICTIONLESS_UNAVAILABLE_ID   and DELETED = 0          
END 