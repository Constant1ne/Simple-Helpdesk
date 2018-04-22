USE [Claims]
--GO

/****** Object:  Table [dbo].[History]    Script Date: 22.04.2018 16:21:44 ******/
SET ANSI_NULLS ON
--GO

SET QUOTED_IDENTIFIER ON
--GO

CREATE TABLE [dbo].[History](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Comment] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

--GO

ALTER TABLE [dbo].[History]  WITH CHECK ADD  CONSTRAINT [FK_History_History] FOREIGN KEY([ClaimId])
REFERENCES [dbo].[Claims] ([id])
ON DELETE CASCADE
--GO

ALTER TABLE [dbo].[History] CHECK CONSTRAINT [FK_History_History]
--GO


