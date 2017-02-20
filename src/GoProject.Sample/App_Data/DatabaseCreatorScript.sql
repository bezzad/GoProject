USE [master]
GO
/****** Object:  Database [GoProject]    Script Date: 2/20/2017 3:52:08 PM ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'GoProject')
BEGIN
CREATE DATABASE [GoProject]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GoProject', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\GoProject.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'GoProject_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\GoProject_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END

GO
ALTER DATABASE [GoProject] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GoProject].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GoProject] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GoProject] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GoProject] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GoProject] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GoProject] SET ARITHABORT OFF 
GO
ALTER DATABASE [GoProject] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [GoProject] SET AUTO_SHRINK ON 
GO
ALTER DATABASE [GoProject] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GoProject] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GoProject] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GoProject] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GoProject] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GoProject] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GoProject] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GoProject] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GoProject] SET AUTO_UPDATE_STATISTICS_ASYNC ON 
GO
ALTER DATABASE [GoProject] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GoProject] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GoProject] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GoProject] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GoProject] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GoProject] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GoProject] SET RECOVERY FULL 
GO
ALTER DATABASE [GoProject] SET  MULTI_USER 
GO
ALTER DATABASE [GoProject] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GoProject] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GoProject] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GoProject] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [GoProject] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'GoProject', N'ON'
GO
ALTER DATABASE [GoProject] SET QUERY_STORE = OFF
GO
USE [GoProject]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [GoProject]
GO
/****** Object:  UserDefinedTableType [dbo].[Link]    Script Date: 2/20/2017 3:52:08 PM ******/
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'Link' AND ss.name = N'dbo')
CREATE TYPE [dbo].[Link] AS TABLE(
	[DiagramId] [varchar](100) NOT NULL,
	[From] [int] NOT NULL,
	[To] [int] NOT NULL,
	[FromPort] [nvarchar](50) NULL,
	[ToPort] [nvarchar](50) NULL,
	[Visible] [bit] NULL,
	[Text] [nvarchar](150) NULL,
	[Category] [varchar](50) NULL,
	[IsDefault] [bit] NULL,
	[PointsJson] [varchar](max) NULL,
	PRIMARY KEY CLUSTERED 
(
	[DiagramId] ASC,
	[From] ASC,
	[To] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[Node]    Script Date: 2/20/2017 3:52:08 PM ******/
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'Node' AND ss.name = N'dbo')
CREATE TYPE [dbo].[Node] AS TABLE(
	[DiagramId] [varchar](200) NOT NULL,
	[Key] [varchar](100) NOT NULL,
	[Category] [varchar](50) NOT NULL,
	[Loc] [varchar](50) NULL,
	[Text] [nvarchar](150) NULL,
	[EventType] [int] NULL,
	[EventDimension] [int] NULL,
	[GatewayType] [int] NULL,
	[TaskType] [int] NULL,
	[Group] [varchar](100) NULL,
	[IsGroup] [bit] NULL,
	[Color] [varchar](50) NULL,
	[Size] [varchar](50) NULL,
	[IsSubProcess] [bit] NULL,
	[Name] [nvarchar](150) NULL,
	PRIMARY KEY CLUSTERED 
(
	[DiagramId] ASC,
	[Key] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  Table [dbo].[Diagrams]    Script Date: 2/20/2017 3:52:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Diagrams]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Diagrams](
	[Id] [varchar](100) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Class] [varchar](50) NOT NULL,
	[IsReadOnly] [bit] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[Position] [varchar](100) NULL,
	[CreatorUserId] [int] NULL,
 CONSTRAINT [PK_Diagram] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Links]    Script Date: 2/20/2017 3:52:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Links]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Links](
	[DiagramId] [varchar](100) NOT NULL,
	[From] [int] NOT NULL,
	[To] [int] NOT NULL,
	[FromPort] [nvarchar](50) NULL,
	[ToPort] [nvarchar](50) NULL,
	[Visible] [bit] NULL,
	[Text] [nvarchar](150) NULL,
	[Category] [varchar](50) NULL,
	[IsDefault] [bit] NULL,
	[PointsJson] [varchar](max) NULL,
 CONSTRAINT [PK_Links_1] PRIMARY KEY CLUSTERED 
(
	[DiagramId] ASC,
	[From] ASC,
	[To] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Nodes]    Script Date: 2/20/2017 3:52:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Nodes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Nodes](
	[DiagramId] [varchar](200) NOT NULL,
	[Key] [varchar](100) NOT NULL,
	[Category] [varchar](50) NOT NULL,
	[Loc] [varchar](50) NULL,
	[Text] [nvarchar](150) NULL,
	[EventType] [int] NULL,
	[EventDimension] [int] NULL,
	[GatewayType] [int] NULL,
	[TaskType] [int] NULL,
	[Group] [varchar](100) NULL,
	[IsGroup] [bit] NULL,
	[Color] [varchar](50) NULL,
	[Size] [varchar](50) NULL,
	[IsSubProcess] [bit] NULL,
	[Name] [nvarchar](150) NULL,
 CONSTRAINT [PK_Nodes] PRIMARY KEY CLUSTERED 
(
	[DiagramId] ASC,
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Diagram_Class]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Diagrams] ADD  CONSTRAINT [DF_Diagram_Class]  DEFAULT ('go.GraphLinksModel') FOR [Class]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Diagram_IsReadOnly]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Diagrams] ADD  CONSTRAINT [DF_Diagram_IsReadOnly]  DEFAULT ((0)) FOR [IsReadOnly]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Diagram_ModifyDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Diagrams] ADD  CONSTRAINT [DF_Diagram_ModifyDate]  DEFAULT (getdate()) FOR [ModifyDate]
END

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertDiagramData]    Script Date: 2/20/2017 3:52:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertDiagramData]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_InsertDiagramData] AS' 
END
GO
-- =============================================
-- Author:		Behzad
-- Create date: 1395/12/01
-- Description:	Fill Go.Js tables by input data
-- =============================================
ALTER PROCEDURE [dbo].[sp_InsertDiagramData]
    @DiagramId varchar(100) ,
    @DiagramName NVARCHAR(200) ,
	@DiagramClass varchar(50),
	@DiagramPosition varchar(100) = NULL,
	@IsReadonly BIT = 0 ,
    @CreatorUserId INT ,
    @Nodes Node READONLY ,
    @Links Link READONLY
AS
    BEGIN TRY
        BEGIN TRANSACTION;

		IF(@DiagramId IS NOT NULL)
		BEGIN
			-- Clear old data
			DELETE  FROM dbo.Links WHERE   DiagramId = @DiagramId;
			DELETE  FROM dbo.Nodes WHERE   DiagramId = @DiagramId;
			DELETE  FROM dbo.Diagrams WHERE Id = @DiagramId;
		END 
		ELSE
			SET @DiagramId = NEWID()
            

		/******************************* Make just diagram **************************/
		INSERT INTO dbo.Diagrams
		        ( Id ,
		          [Name] ,
		          Class ,
		          IsReadOnly ,
		          ModifyDate ,
		          Position ,
		          CreatorUserId
		        )
		VALUES  ( @DiagramId , -- Id - varchar(200)
		          @DiagramName , -- Name - nvarchar(200)
		          @DiagramClass , -- Class - varchar(50)
		          @IsReadonly , -- IsReadOnly - bit
		          GETDATE() , -- ModifyDate - datetime
		          @DiagramPosition , -- Position - varchar(100)
		          @CreatorUserId  -- CreatorUserId - int
		        );
		/*****************************************************************************/





		/****************** Insert nodes by existing diagram id ****************/
		INSERT INTO dbo.Nodes
		        ( DiagramId ,
		          [Key] ,
		          Category ,
		          Loc ,
		          [Text] ,
		          EventType ,
		          EventDimension ,
		          GatewayType ,
		          TaskType ,
		          [Group] ,
		          IsGroup ,
		          Color ,
		          Size ,
		          IsSubProcess ,
		          Name
		        )
		SELECT * FROM @Nodes;
		/***********************************************************************/

	

		/****************** Insert nodes by existing diagram id ****************/
        INSERT  INTO dbo.Links
                ( DiagramId ,
                  [From] ,
                  [To] ,
                  FromPort ,
                  ToPort ,
                  Visible ,
                  [Text] ,
                  Category ,
                  IsDefault ,
                  PointsJson
                )
        SELECT * FROM @Links;
		/***********************************************************************/

		SELECT * FROM dbo.Diagrams WHERE Id = @DiagramId

        COMMIT TRANSACTION;       
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH;

GO
USE [master]
GO
ALTER DATABASE [GoProject] SET  READ_WRITE 
GO
