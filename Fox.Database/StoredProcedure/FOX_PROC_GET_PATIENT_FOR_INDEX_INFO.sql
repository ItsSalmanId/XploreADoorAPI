-- Modified By : <AFTAB AHMED KHAN, 09/29/2022>      
-- DESCRIPTION: <OFFSET CLOUSE ISSUE FIXING>   
--exec [FOX_PROC_GET_PATIENT_FOR_INDEX_INFO] '','','g','','','',1011163,1,100000,'','',1        
CREATE PROCEDURE [dbo].[FOX_PROC_GET_PATIENT_FOR_INDEX_INFO] --'','','','','','',10111663,1,1,'','',1  
(                      
@First_Name VARCHAR(50)                      
,@Last_Name VARCHAR(50)                      
,@Middle_Name VARCHAR(50)                      
,@CHART_ID VARCHAR(100)                      
,@SSN VARCHAR(9)                      
,@Gender VARCHAR(15)                      
,@PRACTICE_CODE BIGINT                      
,@CURRENT_PAGE INT                      
,@RECORD_PER_PAGE INT                      
,@PRACTICE_ORGANIZATION_ID BIGINT                      
,@DOB VARCHAR(50)                      
,@Patient_Alias BIT                      
)                      
AS                      
DECLARE                        
--@First_Name VARCHAR(50)=''                      
--,@Last_Name VARCHAR(50)=''                      
--,@Middle_Name VARCHAR(50)='g'                      
--,@SSN VARCHAR(9)=''                      
--,@Gender VARCHAR(15)=''                      
--,@DOB VARCHAR(50)=''                      
--,@CHART_ID VARCHAR(100)=''                      
--,@Patient_Alias BIT=1                      
--,@PRACTICE_CODE BIGINT=1011163                      
--,@PRACTICE_ORGANIZATION_ID BIGINT=''                      
--,@CURRENT_PAGE INT=1                      
--,@RECORD_PER_PAGE INT=500                      
-- ,                    
 @StringQuery nvarchar(max)                      
DECLARE @firstnamelen int=(select len(@First_Name))                      
,@lastnamelen int=(select len(@Last_Name))                      
BEGIN                      
--SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED            
IF(@CURRENT_PAGE = 0)            
BEGIN            
SET @CURRENT_PAGE = 1            
END            
IF (@First_Name='')                      
BEGIN                      
SET @First_Name=NULL                      
END                      
ELSE                      
BEGIN                      
SET @First_Name=@First_Name                      
END                      
IF (@Last_Name='')                      
BEGIN                      
SET @Last_Name=NULL                      
END                      
ELSE                      
BEGIN                      
SET @Last_Name=@Last_Name                      
END                      
IF (@Middle_Name='')                      
BEGIN                      
SET @Middle_Name=NULL                      
END                      
ELSE                      
BEGIN                      
SET @Middle_Name=@Middle_Name+'%'                      
END                      
IF (@CHART_ID='')                      
BEGIN                      
SET @CHART_ID=NULL                      
END                      
ELSE                      
BEGIN                      
SET @CHART_ID=@CHART_ID+'%'                      
END                      
IF (@SSN='')                      
BEGIN                      
SET @SSN=NULL                      
END                      
ELSE                      
BEGIN                      
SET @SSN=@SSN+'%'                      
END                      
IF (@Gender='')                      
BEGIN                      
SET @Gender=NULL                      
END                      
ELSE                      
IF (@RECORD_PER_PAGE=0)                      
BEGIN                      
SELECT @RECORD_PER_PAGE=COUNT(*)                      
FROM PATIENT with (nolock)                   
END                      
ELSE                      
BEGIN                      
SET @RECORD_PER_PAGE=@RECORD_PER_PAGE                      
END                      
IF (@DOB='')                      
BEGIN                      
SET @DOB=NULL                      
END                      
ELSE                      
BEGIN                      
SET @DOB=@DOB+'%'                      
END                      
IF(@PRACTICE_ORGANIZATION_ID=0)                      
BEGIN                      
SET @PRACTICE_ORGANIZATION_ID=NULL;                      
END;                      
SET @CURRENT_PAGE=@CURRENT_PAGE-1                      
DECLARE @START_FROM INT=@CURRENT_PAGE*@RECORD_PER_PAGE                   
DECLARE @TOATL_PAGESUDM FLOAT                      
DECLARE @TOTAL_RECORDS INT           
DECLARE @IS_ACQUISITION BIT                            
IF OBJECT_ID('tempdb..#PatientInformation') IS NOT NULL DROP TABLE #PatientInformation;                      
set @StringQuery =                      
'SELECT                      
FC.NAME AS FINANCIAL_CLASS_NAME,                      
FC.FINANCIAL_CLASS_ID,            
convert(VARCHAR(50),p.Patient_Account) as  Patient_Account,                      
ISNULL(p.Chart_Id, '''') AS MRN,                      
p.First_Name,                      
p.Last_Name,                      
p.MIDDLE_NAME,                      
pa.FIRST_NAME AS FIRST_NAME_ALIAS,                      
pa.LAST_NAME AS LAST_NAME_ALIAS,                      
pa.MIDDLE_INITIALS AS MIDDLE_INITIALS_ALIAS,                      
CASE                      
WHEN @Patient_Alias=cast(1 as bit)                      
AND pa.PATIENT_ALIAS_ID IS NOT NULL                 
THEN cast(1 as bit)                      
ELSE cast(0 as bit)                      
END AS Is_Patient_Alias,           
FTA.IS_ACQUISITION AS IS_ACQUISITION,                       
CONVERT(VARCHAR, p.Date_Of_Birth, 101) AS Date_Of_Birth,                      
p.Gender,                      
RTRIM(LTRIM(p.SSN)) AS SSN,                      
p.CREATED_DATE,                      
FTP.PRACTICE_ORGANIZATION_ID,                      
ROW_NUMBER() OVER(ORDER BY p.CREATED_DATE DESC) AS ROW                      
into #PatientInformation                      
FROM Patient p with (nolock)           
LEFT JOIN  FOX_TBL_ACQUISITION_PATIENT AS FTA with (nolock) ON FTA.Patient_Account=p.Patient_Account                                    
AND ISNULL(FTA.DELETED, 0)=0                  
LEFT JOIN  FOX_TBL_PATIENT AS FTP with (nolock) ON FTP.Patient_Account=p.Patient_Account                      
AND ISNULL(FTP.DELETED, 0)=0                      
LEFT JOIN FOX_TBL_FINANCIAL_CLASS AS FC with (nolock) ON FTP.FINANCIAL_CLASS_ID=FC.FINANCIAL_CLASS_ID                      
AND p.Practice_Code=FC.PRACTICE_CODE                      
AND ISNULL(FC.DELETED, 0)=0                      
LEFT JOIN FOX_TBL_PATIENT_ALIAS AS pa with (nolock) ON pa.PATIENT_ACCOUNT=p.PATIENT_ACCOUNT                      
AND ISNULL(pa.DELETED, 0)=0                      
AND @Patient_Alias=cast(1 as bit)                      
WHERE                      
p.Practice_Code= @PRACTICE_CODE                      
AND(@PRACTICE_ORGANIZATION_ID IS NULL OR FTP.PRACTICE_ORGANIZATION_ID=@PRACTICE_ORGANIZATION_ID OR ISNULL(FTP.PRACTICE_ORGANIZATION_ID,1)=1)                      
AND(@Middle_Name IS NULL OR p.MIDDLE_NAME LIKE ''%''+@Middle_Name+''%'' OR pa.MIDDLE_INITIALS LIKE ''%''+@Middle_Name+''%'')                      
AND(@CHART_ID IS NULL OR ISNULL(p.Chart_Id, '''') LIKE ''%''+@CHART_ID+''%'')                      
AND(@SSN IS NULL OR ISNULL(p.SSN, '''') LIKE ''%''+@SSN+''%'')                      
AND(@GENDER IS NULL OR CASE UPPER(p.Gender) WHEN ''M'' THEN ''MALE'' WHEN ''MALE'' THEN ''MALE'' WHEN ''F'' THEN ''FEMALE'' WHEN ''FEMALE'' THEN ''FEMALE'' END=UPPER(@GENDER))                      
AND(@DOB IS NULL OR CONVERT(VARCHAR(50), ISNULL(p.Date_Of_Birth, 0), 101) LIKE ''%''+@DOB+''%'')                      
AND(ISNULL(p.Deleted, 0) != 1)                      
AND(Chart_Id > ''00000999'' OR Chart_Id='''' OR Chart_Id IS NULL)                      
AND(ftp.Patient_status IS NULL OR ftp.Patient_status NOT LIKE ''error'') AND (@FIRST_NAME IS NULL';                      
                      
  while(@firstnamelen > 0)                      
begin                      
set @StringQuery=@StringQuery + ' or p.First_Name like ''' + substring(@First_Name,1,@firstnamelen) + '%''';                      
set @firstnamelen=@firstnamelen -1;                      
end                      
set @StringQuery=@StringQuery + ') AND (@Last_Name IS NULL';                      
--for last name                      
while(@lastnamelen > 0)                      
begin                      
set @StringQuery=@StringQuery + ' or p.Last_Name like ''' + substring(@Last_Name,1,@lastnamelen) + '%''';                      
set @lastnamelen=@lastnamelen -1;                      
end                      
set @StringQuery=@StringQuery + ')  ORDER BY';                      
--for first name                      
declare @FirstCounter int                      
set @FirstCounter=-1;                      
set @firstnamelen=(select len(ISNULL(@First_Name,'')))                      
if(@firstnamelen > 0)                      
begin                      
set @StringQuery=@StringQuery + ' case ';                      
while(@FirstCounter < (select len(@First_Name)-1))                      
begin                      
set @StringQuery=@StringQuery + ' WHEN ' + 'p.First_Name like ''' + substring(@First_Name,1,@firstnamelen) + '%'' THEN ' + convert(varchar(5),@FirstCounter + 1);                      
set @firstnamelen=@firstnamelen -1;                      
set @FirstCounter=@FirstCounter + 1;                      
end                      
set @StringQuery=@StringQuery + ' ELSE ' + convert(varchar(5),(select len(ISNULL(@First_Name,'')))) + ' END';                      
end                      
--for last name                      
declare @SecondCounter int                      
set @SecondCounter=-1;                      
set @lastnamelen=(select len(ISNULL(@Last_Name,'')))                      
if(@lastnamelen>0)                      
begin                      
if(cast ((select len(ISNULL(@First_Name,''))) as int) > 0)                      
begin                      
 set @StringQuery=@StringQuery + ',';                      
end                      
 set @StringQuery=@StringQuery + ' case ';                      
while(@SecondCounter < (select len(@Last_Name)-1))                      
begin                      
set @StringQuery=@StringQuery + ' WHEN ' + 'p.Last_Name like ''' + substring(@Last_Name,1,@lastnamelen) + '%'' THEN ' + convert(varchar(5),@SecondCounter + 1);                      
set @lastnamelen=@lastnamelen -1;                      
set @SecondCounter=@SecondCounter + 1;                      
end                      
set @StringQuery=@StringQuery + ' ELSE ' + convert(varchar(5),(select len(@Last_Name))) + ' END';                      
end                      
if(cast((select len(ISNULL(@First_Name,''))) as int)>0)                      
begin                      
set @StringQuery=@StringQuery + ', p.First_Name';                      
end                      
if(cast((select len(ISNULL(@Last_Name,''))) as int)>0)                      
begin                      
set @StringQuery=@StringQuery + ' , p.Last_Name';                      
end                      
if((cast((select len(ISNULL(@First_Name,''))) as int)=0) and (cast ((select len(ISNULL(@Last_Name,''))) as int)=0))                      
begin                
set @StringQuery=@StringQuery + ' p.CREATED_DATE';                       
end                      
set @StringQuery=@StringQuery + ' set @TOATL_PAGESUDM=(select count(*) from #PatientInformation)                      
set @TOTAL_RECORDS =@TOATL_PAGESUDM                       
SET @TOATL_PAGESUDM=CEILING(@TOATL_PAGESUDM/@RECORD_PER_PAGE)                      
                      
SELECT                      
FC.NAME AS FINANCIAL_CLASS_NAME,                      
FC.FINANCIAL_CLASS_ID,                      
convert(VARCHAR(50),p.Patient_Account) as  Patient_Account,                      
ISNULL(p.Chart_Id, '''') AS MRN,                      
p.First_Name,                      
p.Last_Name,                      
p.MIDDLE_NAME,                      
pa.FIRST_NAME AS FIRST_NAME_ALIAS,                      
pa.LAST_NAME AS LAST_NAME_ALIAS,                      
pa.MIDDLE_INITIALS AS MIDDLE_INITIALS_ALIAS,                      
CASE                      
WHEN @Patient_Alias=cast(1 as bit)                      
AND pa.PATIENT_ALIAS_ID IS NOT NULL                      
THEN cast(1 as bit)                      
ELSE cast(0 as bit)                      
END AS Is_Patient_Alias,         
FTA.IS_ACQUISITION AS IS_ACQUISITION,                        
CONVERT(VARCHAR, p.Date_Of_Birth, 101) AS Date_Of_Birth,                     
p.Gender,                      
RTRIM(LTRIM(p.SSN)) AS SSN,                      
p.CREATED_DATE,                      
FTP.PRACTICE_ORGANIZATION_ID,                      
ROW_NUMBER() OVER(ORDER BY p.CREATED_DATE DESC) AS ROW                      
,@TOATL_PAGESUDM AS TOTAL_RECORD_PAGES                      
,@TOTAL_RECORDS TOTAL_RECORDS                      
FROM Patient p with (nolock)            
LEFT JOIN  FOX_TBL_ACQUISITION_PATIENT AS FTA with (nolock) ON FTA.Patient_Account=p.Patient_Account                                    
AND ISNULL(FTA.DELETED, 0)=0                      
LEFT JOIN  FOX_TBL_PATIENT AS FTP with (nolock) ON FTP.Patient_Account=p.Patient_Account                      
AND ISNULL(FTP.DELETED, 0)=0                      
LEFT JOIN FOX_TBL_FINANCIAL_CLASS AS FC with (nolock) ON FTP.FINANCIAL_CLASS_ID=FC.FINANCIAL_CLASS_ID                      
AND p.Practice_Code=FC.PRACTICE_CODE                      
AND ISNULL(FC.DELETED, 0)=0                      
LEFT JOIN FOX_TBL_PATIENT_ALIAS AS pa with (nolock) ON pa.PATIENT_ACCOUNT=p.PATIENT_ACCOUNT                      
AND ISNULL(pa.DELETED, 0)=0                      
AND @Patient_Alias=cast(1 as bit)                      
WHERE                      
p.Practice_Code= @PRACTICE_CODE                      
AND(@PRACTICE_ORGANIZATION_ID IS NULL OR FTP.PRACTICE_ORGANIZATION_ID=@PRACTICE_ORGANIZATION_ID OR ISNULL(FTP.PRACTICE_ORGANIZATION_ID,1)=1)                      
AND(@Middle_Name IS NULL OR p.MIDDLE_NAME LIKE ''%''+@Middle_Name+''%'' OR pa.MIDDLE_INITIALS LIKE ''%''+@Middle_Name+''%'')                      
AND(@CHART_ID IS NULL OR ISNULL(p.Chart_Id, '''') LIKE ''%''+@CHART_ID+''%'')                      
AND(@SSN IS NULL OR ISNULL(p.SSN, '''') LIKE ''%''+@SSN+''%'')                      
AND(@GENDER IS NULL OR CASE UPPER(p.Gender) WHEN ''M'' THEN ''MALE'' WHEN ''MALE'' THEN ''MALE'' WHEN ''F'' THEN ''FEMALE'' WHEN ''FEMALE'' THEN ''FEMALE'' END=UPPER(@GENDER))              
AND(@DOB IS NULL OR CONVERT(VARCHAR(50), ISNULL(p.Date_Of_Birth, 0), 101) LIKE ''%''+@DOB+''%'')                      
AND(ISNULL(p.Deleted, 0) != 1)                      
AND(Chart_Id > ''00000999'' OR Chart_Id='''' OR Chart_Id IS NULL)                      
AND(ftp.Patient_status IS NULL OR ftp.Patient_status NOT LIKE ''error'') AND (@FIRST_NAME IS NULL';                      
set @firstnamelen=(select len(ISNULL(@First_Name,'')))                      
while(@firstnamelen > 0)                      
begin                      
set @StringQuery=@StringQuery + ' or p.First_Name like ''' + substring(@First_Name,1,@firstnamelen) + '%''';                      
set @firstnamelen=@firstnamelen -1;                      
end                      
set @StringQuery=@StringQuery + ') AND (@Last_Name IS NULL';                      
--for last name                      
set @lastnamelen=(select len(ISNULL(@Last_Name,'')))                      
while(@lastnamelen > 0)                      
begin                      
set @StringQuery=@StringQuery + ' or p.Last_Name like ''' + substring(@Last_Name,1,@lastnamelen) + '%''';                      
set @lastnamelen=@lastnamelen -1;                      
end                      
set @StringQuery=@StringQuery + ')  ORDER BY';                      
--for first name                      
declare @FirstCounter1 int                      
set @FirstCounter1=-1;                      
set @firstnamelen=(select len(ISNULL(@First_Name,'')))                      
if(@firstnamelen > 0)                      
begin                      
set @StringQuery=@StringQuery + ' case ';                      
while(@FirstCounter1 < (select len(ISNULL(@First_Name,''))-1))                      
begin                      
set @StringQuery=@StringQuery + ' WHEN ' + 'p.First_Name like ''' + substring(@First_Name,1,@firstnamelen) + '%'' THEN ' + convert(varchar(5),@FirstCounter1 + 1);        
set @firstnamelen=@firstnamelen -1;                      
set @FirstCounter1=@FirstCounter1 + 1;                      
end                      
set @StringQuery=@StringQuery + ' ELSE ' + convert(varchar(5),(select len(ISNULL(@First_Name,'')))) + ' END';                      
end                      
--for last name                      
declare @SecondCounter1 int                      
set @SecondCounter1=-1;                      
set @lastnamelen=(select len(ISNULL(@Last_Name,'')))                      
if(@lastnamelen>0)                      
begin                      
if(cast ((select len(ISNULL(@First_Name,''))) as int) > 0)                      
begin                      
 set @StringQuery=@StringQuery + ',';                      
end                      
 set @StringQuery=@StringQuery + ' case ';                      
while(@SecondCounter1 < (select len(ISNULL(@Last_Name,''))-1))                      
begin                      
set @StringQuery=@StringQuery + ' WHEN ' + 'p.Last_Name like ''' + substring(@Last_Name,1,@lastnamelen) + '%'' THEN ' + convert(varchar(5),@SecondCounter1 + 1);                      
set @lastnamelen=@lastnamelen -1;                      
set @SecondCounter1=@SecondCounter1 + 1;                   
end                      
set @StringQuery=@StringQuery + ' ELSE ' + convert(varchar(5),(select len(ISNULL(@Last_Name,'')))) + ' END';                      
end                      
if(cast((select len(ISNULL(@First_Name,''))) as int)>0)                      
begin                      
set @StringQuery=@StringQuery + ', p.First_Name';                      
end                      
if(cast((select len(ISNULL(@Last_Name,''))) as int)>0)                      
begin                      
set @StringQuery=@StringQuery + ' , p.Last_Name';                      
end                      
if((cast((select len(ISNULL(@First_Name,''))) as int)=0) and (cast ((select len(ISNULL(@Last_Name,''))) as int)=0))                      
begin                      
set @StringQuery=@StringQuery + ' p.CREATED_DATE';                       
end                      
set @StringQuery=@StringQuery + ' OFFSET @START_FROM ROWS                      
FETCH NEXT @RECORD_PER_PAGE ROWS ONLY'                      
--print @StringQuery                      
EXECUTE SP_EXECUTESQL @StringQuery                      
,N'@First_Name VARCHAR(50),@Last_Name VARCHAR(50),@Middle_Name VARCHAR(50),@SSN VARCHAR(9),@Gender VARCHAR(15),@DOB VARCHAR(50),@CHART_ID VARCHAR(100),@Patient_Alias BIT,@IS_ACQUISITION BIT,@PRACTICE_CODE BIGINT,@PRACTICE_ORGANIZATION_ID BIGINT          
  
    
      
           
,@CURRENT_PAGE INT,@RECORD_PER_PAGE INT, @TOATL_PAGESUDM FLOAT, @TOTAL_RECORDS INT, @START_FROM INT'                      
,@First_Name                      
,@Last_Name                      
,@Middle_Name                      
,@SSN                      
,@Gender                      
,@DOB                      
,@CHART_ID                      
,@Patient_Alias            
,@IS_ACQUISITION                      
,@PRACTICE_CODE                      
,@PRACTICE_ORGANIZATION_ID                      
,@CURRENT_PAGE                      
,@RECORD_PER_PAGE                      
,@TOATL_PAGESUDM                      
,@TOTAL_RECORDS                      
,@START_FROM                      
END  