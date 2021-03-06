USE [master]
GO
/****** Object:  Database [supido_example]    Script Date: 06/09/2015 15:53:03 ******/
CREATE DATABASE [supido_example]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'supido_example', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\supido_example.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'supido_example_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\supido_example_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [supido_example] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [supido_example].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [supido_example] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [supido_example] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [supido_example] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [supido_example] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [supido_example] SET ARITHABORT OFF 
GO
ALTER DATABASE [supido_example] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [supido_example] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [supido_example] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [supido_example] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [supido_example] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [supido_example] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [supido_example] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [supido_example] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [supido_example] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [supido_example] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [supido_example] SET  DISABLE_BROKER 
GO
ALTER DATABASE [supido_example] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [supido_example] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [supido_example] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [supido_example] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [supido_example] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [supido_example] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [supido_example] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [supido_example] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [supido_example] SET  MULTI_USER 
GO
ALTER DATABASE [supido_example] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [supido_example] SET DB_CHAINING OFF 
GO
ALTER DATABASE [supido_example] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [supido_example] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [supido_example]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 06/09/2015 15:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[client_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[client_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Department]    Script Date: 06/09/2015 15:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[department_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[department_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 06/09/2015 15:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[project_id] [int] IDENTITY(1,1) NOT NULL,
	[client_id] [int] NOT NULL,
	[name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Service]    Script Date: 06/09/2015 15:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[service_id] [int] IDENTITY(1,1) NOT NULL,
	[project_id] [int] NOT NULL,
	[name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Session]    Script Date: 06/09/2015 15:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Session](
	[session_token] [nvarchar](32) NOT NULL,
	[user_id] [int] NOT NULL,
	[creation_dttm] [datetime] NOT NULL,
	[update_dttm] [datetime] NOT NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[session_token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Task]    Script Date: 06/09/2015 15:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[task_id] [int] IDENTITY(1,1) NOT NULL,
	[service_id] [int] NOT NULL,
	[task_type_id] [int] NOT NULL,
	[assigned_user_id] [int] NOT NULL,
	[name] [nvarchar](150) NOT NULL,
	[priority] [int] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[task_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaskType]    Script Date: 06/09/2015 15:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskType](
	[task_type_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TaskType] PRIMARY KEY CLUSTERED 
(
	[task_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 06/09/2015 15:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[department_id] [int] NOT NULL,
	[name] [nvarchar](150) NOT NULL,
	[email] [nvarchar](150) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Client] ON 

INSERT [dbo].[Client] ([client_id], [name]) VALUES (1, N'Client 1')
INSERT [dbo].[Client] ([client_id], [name]) VALUES (2, N'Client 2')
INSERT [dbo].[Client] ([client_id], [name]) VALUES (3, N'Client 3')
INSERT [dbo].[Client] ([client_id], [name]) VALUES (4, N'Client 4')
INSERT [dbo].[Client] ([client_id], [name]) VALUES (5, N'Client 5')
SET IDENTITY_INSERT [dbo].[Client] OFF
SET IDENTITY_INSERT [dbo].[Department] ON 

INSERT [dbo].[Department] ([department_id], [name]) VALUES (1, N'Department 1')
INSERT [dbo].[Department] ([department_id], [name]) VALUES (2, N'Department 2')
INSERT [dbo].[Department] ([department_id], [name]) VALUES (3, N'Department 3')
INSERT [dbo].[Department] ([department_id], [name]) VALUES (4, N'Department 4')
INSERT [dbo].[Department] ([department_id], [name]) VALUES (5, N'Department 5')
SET IDENTITY_INSERT [dbo].[Department] OFF
SET IDENTITY_INSERT [dbo].[Project] ON 

INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (1, 1, N'Project 1')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (2, 1, N'Project 2')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (3, 2, N'Project 3')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (4, 2, N'Project 4')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (5, 3, N'Project 5')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (6, 3, N'Project 6')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (7, 4, N'Project 7')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (8, 4, N'Project 8')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (9, 5, N'Project 9')
INSERT [dbo].[Project] ([project_id], [client_id], [name]) VALUES (10, 5, N'Project 10')
SET IDENTITY_INSERT [dbo].[Project] OFF
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (1, 1, N'Service 1')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (2, 1, N'Service 2')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (3, 2, N'Service 3')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (4, 2, N'Service 4')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (5, 3, N'Service 5')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (6, 3, N'Service 6')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (7, 4, N'Service 7')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (8, 4, N'Service 8')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (9, 5, N'Service 9')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (10, 5, N'Service 10')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (11, 6, N'Service 11')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (12, 6, N'Service 12')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (13, 7, N'Service 13')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (14, 7, N'Service 14')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (15, 8, N'Service 15')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (16, 8, N'Service 16')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (17, 9, N'Service 17')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (18, 9, N'Service 18')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (19, 10, N'Service 19')
INSERT [dbo].[Service] ([service_id], [project_id], [name]) VALUES (20, 10, N'Service 20')
SET IDENTITY_INSERT [dbo].[Service] OFF
SET IDENTITY_INSERT [dbo].[Task] ON 

INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (1, 1, 1, 1, N'Task 1', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (2, 1, 2, 2, N'Task 2', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (3, 2, 3, 3, N'Task 3', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (5, 2, 4, 4, N'Task 4', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (6, 3, 5, 5, N'Task 5', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (7, 3, 6, 6, N'Task 6', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (8, 4, 7, 7, N'Task 7', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (9, 4, 1, 8, N'Task 8', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (10, 5, 2, 9, N'Task 9', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (11, 5, 3, 10, N'Task 10', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (12, 6, 4, 1, N'Task 11', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (13, 6, 5, 2, N'Task 12', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (14, 7, 6, 3, N'Task 13', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (15, 7, 7, 4, N'Task 14', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (16, 8, 1, 5, N'Task 15', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (17, 8, 2, 6, N'Task 16', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (18, 9, 3, 7, N'Task 17', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (19, 9, 4, 8, N'Task 18', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (20, 10, 5, 9, N'Task 19', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (21, 10, 6, 10, N'Task 20', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (22, 11, 7, 1, N'Task 21', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (23, 11, 1, 2, N'Task 22', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (24, 12, 2, 3, N'Task 23', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (25, 12, 3, 4, N'Task 24', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (27, 13, 4, 5, N'Task 25', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (28, 13, 5, 6, N'Task 26', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (29, 14, 6, 7, N'Task 27', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (30, 14, 7, 8, N'Task 28', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (31, 15, 1, 9, N'Task 29', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (32, 15, 2, 10, N'Task 30', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (33, 16, 3, 1, N'Task 31', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (34, 16, 4, 2, N'Task 32', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (35, 17, 5, 3, N'Task 33', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (36, 17, 6, 4, N'Task 34', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (37, 18, 7, 5, N'Task 35', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (38, 18, 1, 6, N'Task 36', 4)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (39, 19, 2, 7, N'Task 37', 1)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (40, 19, 3, 8, N'Task 38', 2)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (41, 20, 4, 9, N'Task 39', 3)
INSERT [dbo].[Task] ([task_id], [service_id], [task_type_id], [assigned_user_id], [name], [priority]) VALUES (42, 20, 5, 10, N'Task 40', 4)
SET IDENTITY_INSERT [dbo].[Task] OFF
SET IDENTITY_INSERT [dbo].[TaskType] ON 

INSERT [dbo].[TaskType] ([task_type_id], [name]) VALUES (1, N'Management')
INSERT [dbo].[TaskType] ([task_type_id], [name]) VALUES (2, N'Consulting')
INSERT [dbo].[TaskType] ([task_type_id], [name]) VALUES (3, N'Analysis')
INSERT [dbo].[TaskType] ([task_type_id], [name]) VALUES (4, N'Development')
INSERT [dbo].[TaskType] ([task_type_id], [name]) VALUES (5, N'Testing')
INSERT [dbo].[TaskType] ([task_type_id], [name]) VALUES (6, N'Deployment')
INSERT [dbo].[TaskType] ([task_type_id], [name]) VALUES (7, N'UX')
SET IDENTITY_INSERT [dbo].[TaskType] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (1, 1, N'User 1', N'user1@company.com', N'user1')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (2, 1, N'User2', N'user2@company.com', N'user2')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (3, 2, N'User3', N'user3@company.com', N'user3')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (4, 2, N'User4', N'user4@company.com', N'user4')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (5, 3, N'User5', N'user5@company.com', N'user5')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (6, 3, N'User6', N'user6@company.com', N'user6')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (7, 4, N'User7', N'user7@company.com', N'user7')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (8, 4, N'User8', N'user8@company.com', N'user8')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (9, 5, N'User9', N'user9@company.com', N'user9')
INSERT [dbo].[User] ([user_id], [department_id], [name], [email], [password]) VALUES (10, 5, N'User10', N'user10@company.com', N'user10')
SET IDENTITY_INSERT [dbo].[User] OFF
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Client] FOREIGN KEY([client_id])
REFERENCES [dbo].[Client] ([client_id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Client]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Service_Project] FOREIGN KEY([project_id])
REFERENCES [dbo].[Project] ([project_id])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Service_Project]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_User]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Service] FOREIGN KEY([service_id])
REFERENCES [dbo].[Service] ([service_id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Service]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskType] FOREIGN KEY([task_type_id])
REFERENCES [dbo].[TaskType] ([task_type_id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_TaskType]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_User] FOREIGN KEY([assigned_user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Department] FOREIGN KEY([department_id])
REFERENCES [dbo].[Department] ([department_id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Department]
GO
USE [master]
GO
ALTER DATABASE [supido_example] SET  READ_WRITE 
GO
