if exists (select * from sysobjects where id = OBJECT_ID('[DepartmentResult]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [DepartmentResult]

CREATE TABLE [DepartmentResult] (
[errcode] [int]  NULL,
[errmsg] [varchar]  (100) NULL,
[id] [varchar]  (100) NULL,
[ESB_DepartmentID] [varchar]  (50) NOT NULL,
[CreateDate] [datetime]  NULL,
[ESB_DepartmentName] [varchar]  (100) NULL)


if exists (select * from sysobjects where id = OBJECT_ID('[DepartmentTrees]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [DepartmentTrees]

CREATE TABLE [DepartmentTrees] (
[Id] [int]  IDENTITY (1, 1)  NOT NULL,
[DD_Id] [varchar]  (50) NOT NULL,
[DepartmentId] [varchar]  (50) NOT NULL,
[FullName] [nvarchar]  (100) NOT NULL,
[ParentDepartmentId] [varchar]  (50) NULL,
[CreateDate] [datetime]  NULL,
[level] [int]  NULL,
[DD_ParentId] [varchar]  (50) NULL)

ALTER TABLE [DepartmentTrees] WITH NOCHECK ADD  CONSTRAINT [PK_DepartmentTrees] PRIMARY KEY  NONCLUSTERED ( [Id] )


if exists (select * from sysobjects where id = OBJECT_ID('[DingTalkCallBackLog]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [DingTalkCallBackLog]

CREATE TABLE [DingTalkCallBackLog] (
[Id] [bigint]  IDENTITY (1, 1)  NOT NULL,
[EventType] [varchar]  (50) NULL,
[TimeStamp] [varchar]  (50) NULL,
[UserId] [varchar]  (50) NULL,
[CreateDate] [datetime]  NULL DEFAULT (getdate()))

ALTER TABLE [DingTalkCallBackLog] WITH NOCHECK ADD  CONSTRAINT [PK_DingTalkCallBackLog] PRIMARY KEY  NONCLUSTERED ( [Id] )


if exists (select * from sysobjects where id = OBJECT_ID('[DingTalkCallBackOperation]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [DingTalkCallBackOperation]

CREATE TABLE [DingTalkCallBackOperation] (
[Id] [bigint]  IDENTITY (1, 1)  NOT NULL,
[EventType] [varchar]  (50) NULL,
[TimeStamp] [varchar]  (50) NULL,
[UserId] [varchar]  (50) NULL,
[IsOperation] [int]  NULL,
[CreateDate] [datetime]  NULL DEFAULT (getdate()))

ALTER TABLE [DingTalkCallBackOperation] WITH NOCHECK ADD  CONSTRAINT [PK_DingTalkCallBackOperation] PRIMARY KEY  NONCLUSTERED ( [Id] )


if exists (select * from sysobjects where id = OBJECT_ID('[Tbiz_ErroUpdateEmployeeInfo]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [Tbiz_ErroUpdateEmployeeInfo]

CREATE TABLE [Tbiz_ErroUpdateEmployeeInfo] (
[Id] [int]  IDENTITY (1, 1)  NOT NULL,
[UserId] [varchar]  (50) NULL,
[OldMobile] [varchar]  (50) NULL,
[NewMobile] [varchar]  (50) NULL,
[ErroCode] [varchar]  (200) NULL,
[CreateDate] [datetime]  NULL)

