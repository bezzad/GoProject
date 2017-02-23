USE [CAS]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDiagramNodes]    Script Date: 05/12/1395 09:56:00 ق.ظ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetDiagramNodes]
	@DiagramId VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- All Expense Centers Data
	--SELECT  ec.ExpenseCenterId,
	--		ec.[Name] ,
	--		ec.ExpenseCenterTypeId ,
	--		ect.Title AS ExpenseCenterTypeName,
	--		ec.ExpenseCenterCode ,
	--		ec.LineCode,
	--		wtp.WorkStationToProductId,
	--		wtp.WorkStationId,
	--		w.Name,
	--		wtp.ProductId,
	--		p.ProductName,
	--		p.group1_code,
	--		CONVERT(BIT, CASE WHEN p.ProductName IS NULL THEN 0 ELSE 1 END) AS EndProduct,
	--		wtp.OrderWorksatation
	--FROM    [CAS].[dbo].[ExpenseCenter] ec
	--		INNER JOIN [CAS].[dbo].[ExpenseCenterType] ect ON ec.ExpenseCenterTypeId = ect.ExpenseCenterTypeId
	--		INNER JOIN dbo.WorkStationToProduct wtp ON wtp.ExpenseCenterId = ec.ExpenseCenterId
	--		INNER JOIN dbo.WorkStation w ON w.WorkStationId = wtp.WorkStationId
	--		Left JOIN dbo.Product p ON p.ProductId = wtp.ProductId
	--		ORDER BY ec.ExpenseCenterId, wtp.OrderWorksatation;

	DECLARE @Nodes TABLE (
			[Key] [VARCHAR](100) NOT NULL,
			[Category] [INT] NOT NULL,
			[Loc] [VARCHAR](50) NULL,
			[Text] [NVARCHAR](150) NULL,
			[EventType] [INT] NULL,
			[EventDimension] [INT] NULL,
			[GatewayType] [INT] NULL,
			[TaskType] [INT] NULL,
			[Group] [VARCHAR](100) NULL,
			[IsGroup] [BIT] NULL,
			[Color] [VARCHAR](50) NULL,
			[Size] [VARCHAR](50) NULL,
			[IsSubProcess] [BIT] NULL,
			[Name] [NVARCHAR](150) NULL,
			OrderWorksatation INT NOT NULL,
			IsInput BIT NULL
		)

	 DECLARE @Links TABLE (
			[From] [INT] NOT NULL,
			[To] [INT] NOT NULL
		)

	DECLARE @PoolKey VARCHAR(100) = 'Pool_'+ @DiagramId
	DECLARE @LaneKey VARCHAR(100) = 'Lane_'+ @DiagramId
		


		-- Pool Node:
		INSERT INTO @Nodes
				( [Key] ,
				Category ,
				Loc ,
				Text ,
				EventType ,
				EventDimension ,
				GatewayType ,
				TaskType ,
				[Group] ,
				IsGroup ,
				Name,
				OrderWorksatation
				)
		SELECT
				@PoolKey AS [Key],
				7 AS Category,
				NULL AS Loc,
				N'مرکز هزینه ' + [NAME] AS [Text],
				NULL AS  EventType,
				NULL AS  EventDimension,
				NULL AS GatewayType,
				NULL AS  TaskType,
				NULL  AS  [Group],
				1  AS  IsGroup,
				'PoolNode' AS [Name],
				0
			FROM  dbo.ExpenseCenter
			WHERE ExpenseCenterId = @DiagramId

		-- Lane Node:
		INSERT INTO @Nodes
				( [Key] ,
				Category ,
				Loc ,
				Text ,
				EventType ,
				EventDimension ,
				GatewayType ,
				TaskType ,
				[Group] ,
				IsGroup ,
				Name,
				OrderWorksatation
				)
		SELECT
			@LaneKey AS [Key],
			8 AS Category,
			NULL AS Loc,
			N'خط تولید' AS [Text],
			NULL AS  EventType,
			NULL AS  EventDimension,
			NULL AS GatewayType,
			NULL AS  TaskType,
			@PoolKey  AS  [Group],
			1  AS  IsGroup,
			'LaneNode' AS [Name],
			0


		-- Activity Nodes:
		INSERT INTO @Nodes
				( [Key] ,
				Category ,
				Loc ,
				Text ,
				EventType ,
				EventDimension ,
				GatewayType ,
				TaskType ,
				[Group] ,
				IsGroup ,
				Name,
				OrderWorksatation
				)
		SELECT 
			CONVERT(VARCHAR(10), wtp.OrderWorksatation) + CONVERT(VARCHAR(10), wtp.WorkStationId) AS [Key],
			1, --activity
			NULL AS Loc,
			w.[Name] AS [Text],
			NULL AS EventType,
			NULL AS EventDimension,
			NULL AS GatewayType,
			6 AS TaskType,
			@LaneKey as [Group],
			0 as IsGroup,
			'ActivityNode' AS [Name],
			wtp.OrderWorksatation
		FROM    [CAS].[dbo].[ExpenseCenter] ec        
				INNER JOIN dbo.WorkStationToProduct wtp ON wtp.ExpenseCenterId = ec.ExpenseCenterId
				INNER JOIN dbo.WorkStation w ON w.WorkStationId = wtp.WorkStationId							
				WHERE ec.ExpenseCenterId = @DiagramId
				GROUP BY wtp.OrderWorksatation, CONVERT(VARCHAR(10), wtp.OrderWorksatation) + CONVERT(VARCHAR(10), wtp.WorkStationId), w.[Name]
				ORDER BY wtp.OrderWorksatation


		-- Event Nodes
		INSERT INTO @Nodes
				( [Key] ,
				Category ,
				Loc ,
				Text ,
				EventType ,
				EventDimension ,
				GatewayType ,
				TaskType ,
				[Group] ,
				IsGroup ,
				Name,
				OrderWorksatation,
				IsInput
				)
		SELECT  
			wtp.WorkStationToProductId AS [Key],
			0 AS Category,
			NULL AS Loc,
			p.code_name AS [Text],
			CASE p.TypeCodeKala 
				WHEN 0 THEN 1 -- مواد اولیه
				WHEN 1 THEN 11 -- نیمه ساخته
                WHEN 2 THEN	13 -- نهایی
				ELSE 11 
			END AS EventType,
			CASE p.TypeCodeKala 
				WHEN 0 THEN 1 -- مواد اولیه
				WHEN 1 THEN 6 -- نیمه ساخته
                WHEN 2 THEN	8 -- نهایی
				ELSE 6 
			END AS EventDimension,
			NULL AS GatewayType,
			NULL AS TaskType,
			@LaneKey as [Group],
			0 as IsGroup,
			'EventNode' AS [Name],
			wtp.OrderWorksatation,
			CASE p.TypeCodeKala 
				WHEN 2 THEN 0 -- محصول نهایی
				ELSE 1
			END AS IsInput
			--wtp.ProductId
		FROM    [CAS].[dbo].[ExpenseCenter] ec        
				INNER JOIN dbo.WorkStationToProduct wtp ON wtp.ExpenseCenterId = ec.ExpenseCenterId
				INNER JOIN dbo.WorkStation w ON w.WorkStationId = wtp.WorkStationId
				Left JOIN [dbo].[ProductView]  p ON p.coding_main_id = wtp.ProductId
				WHERE ec.ExpenseCenterId = @DiagramId
				ORDER BY wtp.OrderWorksatation






		-- Add Nodes Links

		-- Input event nodes to task node and task node to output event nodes links
		INSERT INTO @Links
		        ( [From], [To] )
		SELECT 
			CASE WHEN eventNodes.IsInput = 1 then eventNodes.[Key] ELSE  taskNodes.[Key] END AS [From],
			CASE WHEN eventNodes.IsInput = 1 then taskNodes.[Key]  ELSE eventNodes.[Key] END AS [To]
		FROM @Nodes eventNodes 
				INNER JOIN @Nodes taskNodes ON tasknodes.OrderWorksatation = eventNodes.OrderWorksatation
		WHERE eventNodes.Category = 0 -- event nodes
				AND taskNodes.Category = 1 -- activity
		ORDER BY eventNodes.OrderWorksatation, tasknodes.OrderWorksatation



		-- Output event nodes to next task nodes links
		INSERT INTO @Links
		        ( [From], [To] )
		SELECT eventNodes.[Key] AS [From], taskNodes.[Key] AS [To]
		FROM @Nodes eventNodes 
				INNER JOIN @Nodes taskNodes ON tasknodes.OrderWorksatation = eventNodes.OrderWorksatation + 1
		WHERE eventNodes.Category = 0 -- event nodes
				AND taskNodes.Category = 1 -- activity
				AND eventNodes.IsInput = 0 -- just output event nodes
		ORDER BY eventNodes.OrderWorksatation, tasknodes.OrderWorksatation

		-- Output event nodes to next task nodes links
		INSERT INTO @Links
		        ( [From], [To] )
		SELECT preTasks.[Key] AS [From], nextTasks.[Key] AS [To]
		FROM @Nodes preTasks
			INNER JOIN @Nodes nextTasks ON nextTasks.OrderWorksatation = preTasks.OrderWorksatation + 1
		WHERE preTasks.Category = 1 -- task nodes
	     AND nextTasks.Category = 1 -- task nodes
		ORDER BY preTasks.OrderWorksatation, nextTasks.OrderWorksatation


		

		-- Get Result to Application
		SELECT * FROM @Nodes	
		SELECT * FROM @Links
END
