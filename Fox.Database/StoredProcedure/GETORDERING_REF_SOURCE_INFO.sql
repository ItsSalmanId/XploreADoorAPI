IF (OBJECT_ID('GetOrdering_Ref_Source_info') IS NOT NULL ) DROP PROCEDURE GetOrdering_Ref_Source_info  
GO 
--GetOrdering_Ref_Source_info 552103          
CREATE Procedure GetOrdering_Ref_Source_info           
@CASE_ID bigint                
as                
begin                
--select o.SOURCE_ID,o.LAST_NAME+','+FIRST_NAME as OrderingRefName from  FOX_TBL_CASE w              
select top 1 o.SOURCE_ID,o.LAST_NAME,FIRST_NAME from  FOX_TBL_CASE w            
join FOX_TBL_ORDERING_REF_SOURCE o on w.ORDERING_REF_SOURCE_ID = o.SOURCE_ID                
where w.CASE_ID = @CASE_ID               
and ISNULL(w.DELETED,0)= 0               
and ISNULL(o.DELETED,0)= 0               
and w.PRACTICE_CODE = o.PRACTICE_CODE          
Order By w.MODIFIED_DATE Desc                
end   
  
  
