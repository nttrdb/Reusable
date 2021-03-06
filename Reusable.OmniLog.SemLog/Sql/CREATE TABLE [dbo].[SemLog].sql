/****** Object:  Table [dbo].[SemLog]    Script Date: 2017-11-21 13:11:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SemLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime2](7) NOT NULL,
	[Environment] [nvarchar](50) NOT NULL,
	[Product] [nvarchar](50) NOT NULL,
	[Logger] [nvarchar](50) NOT NULL,
	[Transaction] [nvarchar](50) NULL,
	[Layer] [nvarchar](50) NOT NULL,
	[Level] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NULL,
	[Snapshot] [nvarchar](max) NULL,
	[Event] [nvarchar](50) NULL,
	[Elapsed] [bigint] NULL,
	[Result] [nvarchar](50) NULL,
	[Message] [nvarchar](max) NULL,
	[Exception] [nvarchar](max) NULL,
	[CallerMemberName] [nvarchar](200) NULL,
	[CallerLineNumber] [int] NULL,
	[CallerFilePath] [nvarchar](200) NULL,
 CONSTRAINT [PK_SemLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Environment]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_Environment] ON [dbo].[SemLog]
(
	[Environment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Event]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_Event] ON [dbo].[SemLog]
(
	[Event] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Layer]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_Layer] ON [dbo].[SemLog]
(
	[Layer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Level]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_Level] ON [dbo].[SemLog]
(
	[Level] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Logger]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_Logger] ON [dbo].[SemLog]
(
	[Logger] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Product]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_Product] ON [dbo].[SemLog]
(
	[Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Result]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_Result] ON [dbo].[SemLog]
(
	[Result] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_State]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_State] ON [dbo].[SemLog]
(
	[State] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_Transaction]    Script Date: 2017-11-21 13:11:48 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction] ON [dbo].[SemLog]
(
	[Transaction] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

