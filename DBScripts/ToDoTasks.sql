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

/****** Object:  Table [dbo].[ToDoTasks]    Script Date: 3/18/2018 12:27:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ToDoTasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Desc] [nvarchar](max) NULL,
	[Done] [bit] NOT NULL,
	[User_Id] [nvarchar](128) NULL,
	[ListId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ToDoTasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ToDoTasks] ADD  DEFAULT ((0)) FOR [ListId]
GO

ALTER TABLE [dbo].[ToDoTasks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ToDoes_dbo.AspNetUsers_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[ToDoTasks] CHECK CONSTRAINT [FK_dbo.ToDoes_dbo.AspNetUsers_User_Id]
GO

ALTER TABLE [dbo].[ToDoTasks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ToDoTasks_dbo.ToDoLists_ListId] FOREIGN KEY([ListId])
REFERENCES [dbo].[ToDoLists] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ToDoTasks] CHECK CONSTRAINT [FK_dbo.ToDoTasks_dbo.ToDoLists_ListId]
GO


