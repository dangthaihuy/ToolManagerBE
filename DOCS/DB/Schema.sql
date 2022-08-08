USE [netcorebackend]
GO
IF NOT EXISTS
(
    SELECT * FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tbl_menu'
)
BEGIN
	CREATE TABLE [dbo].[tbl_menu](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Code] [varchar](20) NULL,
	[Name] [nvarchar](128) NULL,
	[Description] [nvarchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [date] NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedDate] [date] NULL,
	[Status] [smallint] NULL	
	)
END
GO
IF NOT EXISTS
(
    SELECT * FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tbl_form'
)
BEGIN
	CREATE TABLE [dbo].[tbl_form](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Code] [varchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ShortDescription] [nvarchar](220) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [date] NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedDate] [date] NULL,
	[Status] [smallint] NOT NULL
)
END
GO
IF NOT EXISTS
(
    SELECT * FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tbl_workFlow'
)
BEGIN
	CREATE TABLE [dbo].[tbl_workFlow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](220) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [date] NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedDate] [date] NULL,
	[Status] [smallint] NOT NULL
)
END
GO
IF NOT EXISTS
(
    SELECT * FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tbl_menu_workflow'
)
BEGIN
	CREATE TABLE [dbo].[tbl_menu_workflow](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[MenuId] [int] NOT NULL,
	[WorkFlowId] [int] NOT NULL,
)
END
GO
IF NOT EXISTS
(
    SELECT * FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tbl_workflow_form'
)
BEGIN
	CREATE TABLE [dbo].[tbl_workflow_form](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[WorkFlowId] [int] NOT NULL,
	[FormId] [int] NOT NULL,
)
END
GO
IF NOT EXISTS
(
    SELECT * FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tbl_form_filed'
)
BEGIN
	CREATE TABLE [dbo].[tbl_form_filed](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[FormId] [int] NOT NULL,
	[Template] [text] NULL,
)
END
GO
IF NOT EXISTS
(
    SELECT * FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'employee_fill_form'
)
BEGIN
	CREATE TABLE [dbo].[employee_fill_form](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FormId] [int] NOT NULL,
	[FormFieldId] [int] NOT NULL,
	[Value] [nvarchar](2000) NULL,
	[Type] [varchar] (20) NULL,
	[EmployeeId] [int] NOT NULL,
)
END
GO