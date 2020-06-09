USE [flex_sc_internet_flex]
GO

SET ANSI_NULLS ON
GO
 
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (
SELECT * 
FROM   sys.columns 
WHERE  object_id = OBJECT_ID(N'[dbo].[Tasks]') AND lower(name) = 'taskdata')
BEGIN
	ALTER TABLE [dbo].[Tasks]
	ADD [TaskData] [nvarchar](max) NULL;
END;

IF NOT EXISTS (
SELECT * 
FROM   sys.columns 
WHERE  object_id = OBJECT_ID(N'[dbo].[Jobs]') AND lower(name) = 'joborigin')
BEGIN
	ALTER TABLE [dbo].[Jobs]
	ADD [JobOrigin] [nvarchar](max) NULL;
END;