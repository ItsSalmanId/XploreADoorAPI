CREATE TABLE [dbo].[Fox_TBL_TASK_WORK_INTERFACE_MAPPING](
	[TWM_ID] [bigint] NOT NULL,
	[Task_Id] [bigint] NULL,
	[Work_Id] [bigint] NULL,
	[Interface_Id] [bigint] NULL,
	[Created_By] [varchar](25) NULL,
	[Created_Date] [datetime] NULL,
	[Modified_By] [varchar](25) NULL,
	[Modified_Date] [datetime] NULL,
	[Deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[TWM_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]
GO


