USE [master]
GO
/****** Object:  Database [flex_sc_internet_flex]    Script Date: 01.06.2015 12:52:28 ******/
CREATE DATABASE [flex_sc_internet_flex]
GO
ALTER DATABASE [flex_sc_internet_flex] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [flex_sc_internet_flex].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [flex_sc_internet_flex] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET ARITHABORT OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [flex_sc_internet_flex] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [flex_sc_internet_flex] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [flex_sc_internet_flex] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET  DISABLE_BROKER 
GO
ALTER DATABASE [flex_sc_internet_flex] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [flex_sc_internet_flex] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET RECOVERY FULL 
GO
ALTER DATABASE [flex_sc_internet_flex] SET  MULTI_USER 
GO
ALTER DATABASE [flex_sc_internet_flex] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [flex_sc_internet_flex] SET DB_CHAINING OFF 
GO
ALTER DATABASE [flex_sc_internet_flex] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [flex_sc_internet_flex] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [flex_sc_internet_flex]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 01.06.2015 12:52:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Fields]    Script Date: 01.06.2015 12:52:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fields](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[File_Id] [int] NULL,
	[Session_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Fields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Files]    Script Date: 01.06.2015 12:52:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Files](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContentType] [nvarchar](max) NOT NULL,
	[ContentLength] [int] NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[Data] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_dbo.Files] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Forms]    Script Date: 01.06.2015 12:52:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Forms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Forms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 01.06.2015 12:52:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[JobOrigin] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Jobs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sessions]    Script Date: 01.06.2015 12:52:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Language] [nvarchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Form_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Sessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 01.06.2015 12:52:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[NumberOfTries] [int] NOT NULL,
	[LastTry] [datetime] NOT NULL,
	[TaskData] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Tasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[JobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_File_Id]    Script Date: 01.06.2015 12:52:28 ******/
CREATE NONCLUSTERED INDEX [IX_File_Id] ON [dbo].[Fields]
(
	[File_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Session_Id]    Script Date: 01.06.2015 12:52:28 ******/
CREATE NONCLUSTERED INDEX [IX_Session_Id] ON [dbo].[Fields]
(
	[Session_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Form_Id]    Script Date: 01.06.2015 12:52:28 ******/
CREATE NONCLUSTERED INDEX [IX_Form_Id] ON [dbo].[Sessions]
(
	[Form_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_JobId]    Script Date: 01.06.2015 12:52:28 ******/
CREATE NONCLUSTERED INDEX [IX_JobId] ON [dbo].[Tasks]
(
	[JobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Fields]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Fields_dbo.Files_File_Id] FOREIGN KEY([File_Id])
REFERENCES [dbo].[Files] ([Id])
GO
ALTER TABLE [dbo].[Fields] CHECK CONSTRAINT [FK_dbo.Fields_dbo.Files_File_Id]
GO
ALTER TABLE [dbo].[Fields]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Fields_dbo.Sessions_Session_Id] FOREIGN KEY([Session_Id])
REFERENCES [dbo].[Sessions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Fields] CHECK CONSTRAINT [FK_dbo.Fields_dbo.Sessions_Session_Id]
GO
ALTER TABLE [dbo].[Sessions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Sessions_dbo.Forms_Form_Id] FOREIGN KEY([Form_Id])
REFERENCES [dbo].[Forms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sessions] CHECK CONSTRAINT [FK_dbo.Sessions_dbo.Forms_Form_Id]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Tasks_dbo.Jobs_JobId] FOREIGN KEY([JobId])
REFERENCES [dbo].[Jobs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_dbo.Tasks_dbo.Jobs_JobId]
GO
USE [master]
GO
ALTER DATABASE [flex_sc_internet_flex] SET  READ_WRITE 
GO
