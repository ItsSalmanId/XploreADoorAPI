IF (OBJECT_ID('FOX_PROC_UPDTAE_PATIENT_INFO') IS NOT NULL ) DROP PROCEDURE FOX_PROC_UPDTAE_PATIENT_INFO  
GO 
-- =============================================                
-- AUTHOR:  <DEVELOPER, ABDUR RAFAY>                
-- CREATE DATE: <CREATE DATE, 06/23/2020>                
-- DESCRIPTION: <UPDTAE PATIENT INFO>                
                
 --EXEC FOX_PROC_UPDTAE_PATIENT_INFO         
 --     101116354815222,        
 --  '10/22/2019 3:17:09 AM',    
 --    'Testing',          
 --    'Sp',    
 --    'M',    
 --    'male',          
 --    '120215478',          
 --    'RAFAY',        
 --    '10/22/2019 3:17:09 AM',        
 --     0                           
 CREATE PROCEDURE [dbo].[FOX_PROC_UPDTAE_PATIENT_INFO]    
 @Patient_Account BIGINT NULL    
,@Date_Of_Birth DATETIME NULL         
,@First_Name  VARCHAR(50) NULL        
,@Last_Name VARCHAR(100) NULL      
,@Middle_Name CHAR(1) NULL      
,@Gender  VARCHAR(15) NULL          
,@SSN CHAR(20) NULL        
,@MODIFIED_BY VARCHAR(70) NULL        
,@MODIFIED_DATE DATETIME NULL        
,@DELETED BIT NULL        
        
AS           
BEGIN                
              
UPDATE PATIENT          
SET        
 Date_Of_Birth = CAST(@Date_Of_Birth AS DATETIME)                                 
,First_Name = @First_Name        
,Last_Name = @Last_Name        
,Gender = @Gender        
,MI = @Middle_Name      
,SSN = @SSN        
,MODIFIED_BY = @MODIFIED_BY        
,MODIFIED_DATE =  CAST(@MODIFIED_DATE AS DATETIME)        
,DELETED = @DELETED          
WHERE Patient_Account = @Patient_Account      
END 

