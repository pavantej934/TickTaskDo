/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [TickTaskDoe]
GO

/****** Object:  Table [dbo].[ToDoes]    Script Date: 3/14/2018 6:34:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ToDoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Desc] [nvarchar](max) NULL,
	[Done] [bit] NOT NULL,
	[User_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.ToDoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ToDoes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ToDoes_dbo.AspNetUsers_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[ToDoes] CHECK CONSTRAINT [FK_dbo.ToDoes_dbo.AspNetUsers_User_Id]
GO


