IF (OBJECT_ID('FOX_GET_SMART_DIAGNOSIS_WEBEHR') IS NOT NULL) DROP PROCEDURE FOX_GET_SMART_DIAGNOSIS_WEBEHR
GO
CREATE PROCEDURE [dbo].[FOX_GET_SMART_DIAGNOSIS_WEBEHR] -- '', 'gastroenteropathy', '', 1011163, '', '', '',''  
	(
	@CODE VARCHAR(50)
	,@DESCRIPTION NVARCHAR(1000)
	,@OPTIONSEARCH VARCHAR(50)
	,-- B for begin and C for contain                                       
	@PRACTICE_CODE VARCHAR(25)
	,@PROVIDER_CODE VARCHAR(25)
	,@CODE_TYPE VARCHAR(10)
	,-- ICD9 for ICD9 CODE, SNO for SNOMEDCT CODE and ICD10 for ICD10 CODE                                      
	@CATEGORY VARCHAR(2)
	,-- A FOR ALL AND M FOR MY LIST         
	@DOS VARCHAR(50)
	)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	--declare @CODE  VARCHAR(50)                                       
	--declare @DESCRIPTION VARCHAR(1000)                              
	--declare @OPTIONSEARCH VARCHAR(50) -- B for begin and C for contain                                       
	--declare @PRACTICE_CODE VARCHAR(25)                                    
	--declare @PROVIDER_CODE VARCHAR(25)                                               
	--declare @CODE_TYPE VARCHAR(10)  -- ICD9 for ICD9 CODE, SNO for SNOMEDCT CODE and ICD10 for ICD10 CODE                                      
	--declare @CATEGORY VARCHAR(2)              
	--declare @DOS VARCHAR(50)                        
	-- declare @date varchar(20)=CONVERT(varchar, getdate(),101) 
	-- select @date
	--SET @CODE = ''                        
	--SET @DESCRIPTION = 'K0'                                      
	SET @OPTIONSEARCH = 'C'
	--SET @PRACTICE_CODE = '1011163'                                      
	SET @PROVIDER_CODE = ''
	SET @CODE_TYPE = 'ICD10'
	SET @CATEGORY = 'a'
	SET @DOS = CONVERT(VARCHAR, getdate(), 101)

	------------------------------------------String Based QUERY  
	IF @DOS IS NULL
		SET @DOS = GETDATE();

	DECLARE @STR VARCHAR(max)
	DECLARE @STRORDERBY VARCHAR(100)
	DECLARE @totalrow INT
		,@RowCount INT
		,@DESWord VARCHAR(max)
		,@detaildescription VARCHAR(max)
		,@detaildescriptionREV VARCHAR(max)

	SET @STRORDERBY = CASE 
			WHEN @CODE <> ''
				THEN 'ORDER BY 1'
			ELSE ' ORDER BY 2'
			END
	SET @RowCount = 1
	SET @detaildescription = ''
	SET @detaildescriptionREV = ''

	IF object_id('tempdb..#TEMP') IS NOT NULL
		DROP TABLE #TEMP

	SELECT ROW_NUMBER() OVER (
			ORDER BY items
			) AS rownumber
		,items AS words
	INTO #TEMP
	FROM Web_SplitFunction(@DESCRIPTION, ' ')

	SELECT @totalrow = count(words)
	FROM #TEMP

	IF @OPTIONSEARCH = 'C'
	BEGIN
		WHILE @RowCount <= @totalrow
		BEGIN
			SELECT @DESWord = words
			FROM #TEMP
			WHERE rownumber = @RowCount

			IF @CODE_TYPE = 'ICD9'
			BEGIN
				SET @detaildescription = @detaildescription + ' AND D.DIAG_DESCRIPTION LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @DESWord + '%'' ELSE ''' + @DESWord + '%'' END '
			END
			ELSE
			BEGIN
				IF @CATEGORY = 'A'
				BEGIN --D10           
					SET @detaildescription = @detaildescription + ' AND IC.DIAG_DESCRIPTION LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @DESWord + '%'' ELSE ''' + @DESWord + '%'' END '
					SET @detaildescriptionREV = @detaildescriptionREV + ' AND CODES10.DIAG_DESCRIPTION LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @DESWord + '%'' ELSE ''' + @DESWord + '%'' END '
				END
				ELSE
				BEGIN
					SET @detaildescription = @detaildescription + ' AND IC.DIAG_DESCRIPTION LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @DESWord + '%'' ELSE ''' + @DESWord + '%'' END '
				END
			END

			SET @RowCount = @RowCount + 1
		END
	END
	ELSE IF @DESCRIPTION != ''
	BEGIN
		IF @CODE_TYPE = 'ICD9'
		BEGIN
			SET @detaildescription = @detaildescription + ' AND D.DIAG_DESCRIPTION LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @DESCRIPTION + '%'' ELSE ''' + @DESCRIPTION + '%'' END '
		END
		ELSE
		BEGIN
			IF @CATEGORY = 'A'
			BEGIN
				SET @detaildescription = @detaildescription + ' AND IC.DIAG_DESCRIPTION LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @DESCRIPTION + '%'' ELSE ''' + @DESCRIPTION + '%'' END '
				SET @detaildescriptionREV = @detaildescriptionREV + ' AND CODES10.DIAG_DESCRIPTION LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @DESCRIPTION + '%'' ELSE ''' + @DESCRIPTION + '%'' END '
			END
			ELSE
			BEGIN
				SET @detaildescription = @detaildescription + ' AND IC.DIAG_DESCRIPTION LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @DESCRIPTION + '%'' ELSE ''' + @DESCRIPTION + '%'' END '
			END
		END
	END

	IF (@CATEGORY = 'M')
	BEGIN
		-- FOR ICD9                                      
		IF (@CODE_TYPE = 'ICD9')
		BEGIN
			SET @STR = 'SELECT Distinct TOP 100 D.DIAG_CODE AS CODE,                             
          CASE WHEN ( CONVERT(DATE, ''' + @DOS + ''') < CONVERT(DATE, D.DIAG_EFFECTIVE_DATE) OR CONVERT(DATE, D.DIAG_EXPIRY_DATE) < CONVERT(DATE, ''' + @DOS + 
				''') ) THEN   
    REPLACE(DIAG_DESCRIPTION,''(RED)'','''')+'' ''+''(RED)'' ELSE DIAG_DESCRIPTION END AS CODE_DESCRIPTION,  
    CASE WHEN D.ICD_VERSION = ''I9'' THEN ''ICD9'' WHEN D.ICD_VERSION = ''I10'' THEN ''ICD10'' ELSE ''BOTH'' END AS CODE_TYPE  ,
CONVERT(varchar, D.DIAG_EFFECTIVE_DATE,101) as DIAG_EFFECTIVE_DATE,
CONVERT(varchar, D.DIAG_EXPIRY_DATE,101) as DIAG_EXPIRY_DATE
          FROM PROVIDER_DIAGNOSIS PD                                       
          JOIN DIAGNOSIS D ON PD.DIAG_CODE = D.DIAG_CODE                                      
          --left outer join icd10_to_icd9  D10  on D10.ICD_9=REPLACE(D.Diag_Code,''.'','''')     
          --LEFT OUTER JOIN  ICD10_CODES CODES10  ON  D10.ICD_10=CODES10.ICD10                                
         WHERE D.DIAG_CODE LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @CODE + '%'' ELSE ''' + @CODE + '%'' END AND                                            
          PD.PRACTICE_CODE =''' + 
				@PRACTICE_CODE + '''      
  AND PD.PROVIDER_CODE IN(''' + @PROVIDER_CODE + '''                                     
          AND ISNULL(CODE_TYPE,''ICD9'') = ''' + @CODE_TYPE + '''                                           
          AND ISNULL(PD.DELETED,0)<>1 and isnull(D.is_active,0) = 1 and isnull(D.deleted,0) <> 1 AND  D.ICD_VERSION IN (''i9'',''b'') --AND ISNULL(D.DELETED,0)<>1 /* and CODES10.Status_code = 1  */     
          ' + @detaildescription + @STRORDERBY
		END

		-- FOR ICD10                                      
		IF (@CODE_TYPE = 'ICD10')
		BEGIN
			SET @STR = 'SELECT Distinct TOP 100 IC.DIAG_CODE AS CODE,   
 CASE WHEN ( CONVERT(DATE, ''' + @DOS + ''') < CONVERT(DATE, IC.DIAG_EFFECTIVE_DATE) OR CONVERT(DATE, IC.DIAG_EXPIRY_DATE) < CONVERT(DATE, ''' + @DOS + ''') ) THEN   
 REPLACE(IC.DIAG_DESCRIPTION,''(RED)'','''')+'' ''+''(RED)'' ELSE IC.DIAG_DESCRIPTION END AS CODE_DESCRIPTION,  
 CASE WHEN IC.ICD_VERSION = ''I9'' THEN ''ICD9'' WHEN IC.ICD_VERSION = ''I10'' THEN ''ICD10'' ELSE ''BOTH'' END AS CODE_TYPE ,
CONVERT(varchar, IC.DIAG_EFFECTIVE_DATE,101) as DIAG_EFFECTIVE_DATE,
CONVERT(varchar, IC.DIAG_EXPIRY_DATE,101) as DIAG_EXPIRY_DATE                                                                                                         
    FROM PROVIDER_DIAGNOSIS PD                                       
    JOIN DIAGNOSIS IC ON IC.DIAG_CODE = PD.DIAG_CODE ' + ' WHERE IC.DIAG_CODE LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @CODE + '%'' ELSE ''' + @CODE + 
				'%'' END AND                           
    PD.PRACTICE_CODE = ''' + @PRACTICE_CODE + '''   
    AND IC.Icd_version = ''i10''        
 AND IC.ICD_VERSION IN (''i10'',''b'')                                        
    AND PD.PROVIDER_CODE = ''' + @PROVIDER_CODE + '''                                       
    AND ISNULL(CODE_TYPE,''ICD9'') = ''' + @CODE_TYPE + '''                
    AND ISNULL(PD.DELETED,0) <> 1 and ISNULL(IC.IS_ACTIVE,1) = 1                            
    ' + @detaildescription + @STRORDERBY
		END
	END
			-- FOR ALL                                      
	ELSE
	BEGIN -- FOR ICD9                                      
		IF (@CODE_TYPE = 'ICD9')
		BEGIN
			SET @STR = 'select distinct a.* from ( SELECT TOP 100 D.DIAG_CODE AS CODE,   
  CASE WHEN ( CONVERT(DATE, ''' + @DOS + ''') < CONVERT(DATE, D.DIAG_EFFECTIVE_DATE) OR CONVERT(DATE, D.DIAG_EXPIRY_DATE) < CONVERT(DATE, ''' + @DOS + ''') ) THEN   
  REPLACE(DIAG_DESCRIPTION,''(RED)'','''')+'' ''+''(RED)'' ELSE DIAG_DESCRIPTION END AS CODE_DESCRIPTION,  
   CASE WHEN D.ICD_VERSION = ''I9'' THEN ''ICD9'' WHEN D.ICD_VERSION = ''I10'' THEN ''ICD10'' ELSE ''BOTH'' END AS CODE_TYPE  ,
CONVERT(varchar, D.DIAG_EFFECTIVE_DATE,101) as DIAG_EFFECTIVE_DATE,
CONVERT(varchar, D.DIAG_EXPIRY_DATE,101) as DIAG_EXPIRY_DATE           
        
         FROM DIAGNOSIS D                                      
         --left outer join icd10_to_icd9  D10  on D10.ICD_9=REPLACE(D.Diag_Code,''.'','''')                                
         --LEFT OUTER JOIN  ICD10_CODES CODES10  ON  D10.ICD_10=CODES10.ICD10                              
     WHERE D.DIAG_CODE LIKE CASE WHEN ''' + @OPTIONSEARCH + 
				''' = ''C'' THEN ''%' + @CODE + '%'' ELSE ''' + @CODE + '%'' END AND                                     
         ISNULL(D.DELETED,0)<>1 and isnull(D.deleted,0) <> 1 AND D.ICD_VERSION IN (''i9'',''b'')  --and isnull(CODES10.Status_code,''1'') = 1                      
         ' + @detaildescription + 
				'UNION ALL  SELECT TOP 100 D.DIAG_CODE AS CODE, CASE WHEN CONVERT(DATE,DIAG_EXPIRY_DATE) < CONVERT(DATE,GETDATE())   
   THEN REPLACE(DIAG_DESCRIPTION,''(RED)'','''')+'' ''+''(RED)'' ELSE DIAG_DESCRIPTION END AS CODE_DESCRIPTION,  
    CASE WHEN D.ICD_VERSION = ''I9'' THEN ''ICD9'' WHEN D.ICD_VERSION = ''I10'' THEN ''ICD10'' ELSE ''BOTH'' END AS CODE_TYPE ,
CONVERT(varchar, D.DIAG_EFFECTIVE_DATE,101) as DIAG_EFFECTIVE_DATE,
CONVERT(varchar, D.DIAG_EXPIRY_DATE,101) as DIAG_EXPIRY_DATE                             
         FROM DIAGNOSIS D            
        -- left outer join icd9_to_icd10  D10  on D10.ICD_9=REPLACE(D.Diag_Code,''.'','''')                                
         --LEFT OUTER JOIN  ICD10_CODES CODES10  ON  D10.ICD_10=CODES10.ICD10               
         WHERE D.DIAG_CODE LIKE CASE WHEN ''' + @OPTIONSEARCH + ''' = ''C'' THEN ''%' + @CODE + '%'' ELSE ''' + @CODE + 
				'%'' END                
         AND ISNULL(D.DELETED,0)<>1 and isnull(D.deleted,0) <> 1 AND D.ICD_VERSION IN (''i9'',''b'')  --and isnull(CODES10.Status_code,''1'') = 1                                    
         ' + @detaildescription + ')  a ' + @STRORDERBY + ' '
		END

		-- FOR ICD10               
		--select @detaildescriptionREV                                     
		IF (@CODE_TYPE = 'ICD10')
		BEGIN
			SET @STR = '  select distinct a.* from (                
   SELECT TOP 100 IC.DIAG_CODE AS CODE,   
   CASE WHEN ( CONVERT(DATE, ''' + @DOS + ''') < CONVERT(DATE, IC.DIAG_EFFECTIVE_DATE) OR CONVERT(DATE, IC.DIAG_EXPIRY_DATE) < CONVERT(DATE, ''' + @DOS + ''') ) THEN   
   REPLACE(IC.DIAG_DESCRIPTION,''(RED)'','''')+'' ''+''(RED)'' ELSE IC.DIAG_DESCRIPTION END AS CODE_DESCRIPTION,  
   CASE WHEN IC.ICD_VERSION = ''I9'' THEN ''ICD9'' WHEN IC.ICD_VERSION = ''I10'' THEN ''ICD10'' ELSE ''BOTH'' END AS CODE_TYPE,
CONVERT(varchar, IC.DIAG_EFFECTIVE_DATE,101) as DIAG_EFFECTIVE_DATE,
CONVERT(varchar, IC.DIAG_EXPIRY_DATE,101) as DIAG_EXPIRY_DATE                                               
         FROM DIAGNOSIS IC '
				--+'WHERE IC.DIAG_CODE LIKE( CASE WHEN '''+@OPTIONSEARCH+''' = ''C'' THEN ''%' + @CODE + '%'' ELSE '''+@CODE + '%'' END 
				+ 'where  (IC.DIAG_DESCRIPTION  LIKE ''%' + @DESCRIPTION + '%'' OR IC.DIAG_CODE  LIKE ''%' + @DESCRIPTION + '%'') 
		 		 AND                           
         ISNULL(IC.DELETED,0)<>1 and isnull(IC.Is_Active,''1'') = 1 AND IC.ICD_VERSION IN (''i10'',''b'')                                    
          )  a  ' + @STRORDERBY + ' '
		END
	END

	PRINT @STR

	EXEC (@STR)
END
	------------------------------------------FAKE QUERY  
	--SELECT DISTINCT A.* FROM (                
	-- SELECT TOP 100   
	--  IC.DIAG_CODE AS CODE,   
	--  CASE WHEN ( CONVERT(DATE, '10/16/2016') < CONVERT(DATE, IC.DIAG_EFFECTIVE_DATE) OR CONVERT(DATE, IC.DIAG_EXPIRY_DATE) < CONVERT(DATE, '10/16/2016') ) THEN    
	--  REPLACE(IC.DIAG_DESCRIPTION,'(RED)','')+' '+'(RED)' ELSE IC.DIAG_DESCRIPTION END AS CODE_DESCRIPTION,  
	--  CASE WHEN IC.ICD_VERSION = 'I9' THEN 'ICD9' WHEN IC.ICD_VERSION = 'I10' THEN 'ICD10' ELSE 'BOTH' END AS CODE_TYPE                                    
	-- FROM DIAGNOSIS IC WHERE IC.DIAG_CODE LIKE CASE WHEN 'C' = 'C' THEN '%F32.8%' ELSE 'F32.8%' END AND                           
	--ISNULL(IC.DELETED,0)<>1 AND ISNULL(IC.IS_ACTIVE,'1') = 1 AND IC.ICD_VERSION IN ('I10','B')                     
	--)  A  ORDER BY 1   
	--END  
