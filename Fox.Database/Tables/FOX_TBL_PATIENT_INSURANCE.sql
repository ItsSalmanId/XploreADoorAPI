USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_PATIENT_INSURANCE]    Script Date: 7/21/2022 9:17:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_PATIENT_INSURANCE](
	[Patient_Insurance_Id] [bigint] NOT NULL,
	[Patient_Account] [bigint] NULL,
	[Parent_Patient_insurance_Id] [bigint] NULL,
	[Insurance_Id] [bigint] NOT NULL,
	[FOX_TBL_INSURANCE_ID] [bigint] NOT NULL,
	[Pri_Sec_Oth_Type] [varchar](2) NULL,
	[Co_Payment] [money] NULL,
	[Deductions] [money] NULL,
	[Policy_Number] [varchar](25) NOT NULL,
	[Group_Number] [varchar](25) NULL,
	[Effective_Date] [datetime] NULL,
	[Termination_Date] [datetime] NULL,
	[Subscriber] [bigint] NULL,
	[Relationship] [varchar](10) NULL,
	[Plan_Name] [varchar](20) NULL,
	[INACTIVE] [bit] NULL,
	[SUPRESS_BILLING_UNTIL] [datetime] NULL,
	[IS_COPAY_PER_VISIT] [bit] NULL,
	[DED_AMT_VERIFIED_ON] [datetime] NULL CONSTRAINT [DF_FOX_TBL_PATIENT_INSURANCE_DED_AMT_VERIFIED_ON]  DEFAULT (getdate()),
	[DED_POLICY_LIMIT_RESET_ON] [varchar](10) NULL,
	[YEARLY_DED_AMT] [money] NULL,
	[DED_MET] [money] NULL,
	[DED_MET_AS_OF] [datetime] NULL CONSTRAINT [DF_FOX_TBL_PATIENT_INSURANCE_DED_MET_AS_OF]  DEFAULT (getdate()),
	[DED_REMAINING] [money] NULL,
	[IS_PT_ST_THRESHOLD_REACHED] [bit] NULL,
	[IS_OT_THRESHOLD_REACHED] [bit] NULL,
	[PT_ST_TOT_AMT_USED] [money] NULL,
	[PT_ST_RT_AMT] [money] NULL,
	[PT_ST_OUTSIDE_AMT_USED]  AS ([PT_ST_TOT_AMT_USED]-[PT_ST_RT_AMT]),
	[OT_TOT_AMT_USED] [money] NULL,
	[OT_RT_AMT] [money] NULL,
	[OT_OUTSIDE_AMT_USED]  AS ([OT_TOT_AMT_USED]-[OT_RT_AMT]),
	[PT_ST_YTD_AMT] [money] NULL,
	[OT_YTD_AMT] [money] NULL,
	[BENEFIT_AMT_VERIFIED_ON] [datetime] NULL CONSTRAINT [DF_FOX_TBL_PATIENT_INSURANCE_BENEFIT_AMT_VERIFIED_ON]  DEFAULT (getdate()),
	[BENEFIT_POLICY_LIMIT_RESET_ON] [varchar](10) NULL,
	[MYB_LIMIT_DOLLARS] [money] NULL,
	[MYB_LIMIT_VISIT] [int] NULL,
	[MYB_USED_OUTSIDE_DOLLARS] [money] NULL,
	[MYB_USED_OUTSIDE_VISIT] [int] NULL,
	[MYB_USED_DOLLARS] [money] NULL,
	[MYB_USED_VISIT] [int] NULL,
	[MYB_REMAINING_DOLLARS] [money] NULL,
	[MYB_REMAINING_VISIT] [int] NULL,
	[MOP_AMT] [money] NULL,
	[MOP_USED_OUTSIDE_RT] [money] NULL,
	[MOP_USED] [money] NULL,
	[MOP_REMAINING] [money] NULL,
	[SPOKE_TO] [varchar](200) NULL,
	[CASE_ID] [bigint] NULL,
	[BENEFIT_COMMENTS] [varchar](max) NULL,
	[GENERAL_COMMENTS] [varchar](max) NULL,
	[FOX_INSURANCE_STATUS] [varchar](5) NULL,
	[VERIFIED_BY] [varchar](70) NULL,
	[IS_COPAY_PER] [bit] NULL,
	[IS_VERIFIED] [bit] NULL,
	[Created_By] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_INSURANCE_Created_By]  DEFAULT ('FOX TEAM'),
	[Created_Date] [datetime] NULL,
	[Modified_By] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_PATIENT_INSURANCE_Modified_By]  DEFAULT ('FOX TEAM'),
	[Modified_Date] [datetime] NULL,
	[Deleted] [bit] NULL CONSTRAINT [DF_FOX_TBL_PATIENT_INSURANCE_Deleted]  DEFAULT ((0)),
	[VERIFIED_DATE] [datetime] NULL,
	[CHK_ABN] [bit] NULL,
	[CHK_HOSPICE] [bit] NULL,
	[CHK_HOME_HEALTH_EPISODE] [bit] NULL,
	[ABN_LIMIT_ID] [bigint] NULL,
	[HOSPICE_LIMIT_ID] [bigint] NULL,
	[HOME_HEALTH_LIMIT_ID] [bigint] NULL,
	[FINANCIAL_CLASS_ID] [int] NULL,
	[Is_Authorization_Required] [bit] NULL CONSTRAINT [DF__FOX_TBL_P__Is_Au__35144AEB]  DEFAULT ((0)),
	[PR_PERIOD_ID] [int] NULL,
	[PERIODIC_PAYMENT] [money] NULL,
	[PR_DISCOUNT_ID] [int] NULL,
	[MTBC_Patient_Insurance_Id] [bigint] NULL,
	[Deceased_Date] [datetime] NULL,
	[IsWellness] [bit] NULL,
	[IsSkilled] [bit] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_943BB14091B649D0A9C7BE85D56B0AB2]  DEFAULT (newsequentialid()),
	[IS_PRIVATE_PAY] [bit] NULL,
	[FOX_GUID] [varchar](50) NULL,
	[FOX_ELIGIBILITY_GUID] [varchar](50) NULL,
	[ELIG_LOADED_ON] [datetime] NULL,
 CONSTRAINT [PK_FOX_TBL_PATIENT_INSURANCE] PRIMARY KEY CLUSTERED 
(
	[Patient_Insurance_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


