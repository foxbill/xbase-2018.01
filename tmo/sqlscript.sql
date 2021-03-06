USE [tmw_bz]
GO
--删除原有的表以及数据
DECLARE @tableName AS Nvarchar(50) --查询表名条件（小心！，确保like条件是你要Drop的表.TableName尽量精确）  
  
SET @tableName='pt_'    
  
--------------------------------------  
  
--SELECT name FROM sys.tables   WHERE name LIKE '%'+@tableName+'%' --查询出要删除表的名称  
  
IF @tableName='' SET @tableName='pt_'--初始化TableName为tableName,防止@tableName为空  
  
DECLARE @tableNames AS Nvarchar(3000)  
  
DECLARE @sql AS Nvarchar(3000)  
  
SET @tableNames=  
  
(SELECT ','+name FROM sys.tables   WHERE name LIKE +@tableName+'%'  FOR XML PATH(''))  
  
SET @tableNames= Stuff(@tableNames,1,1,'')  
  
SET @sql='DROP TABLE '+@tableNames  
  
EXEC(@sql);
/****** Object:  Table [dbo].[pt_shoppingCart]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_shoppingCart](
	[ID] [nvarchar](50) NULL,
	[SessionId] [nvarchar](50) NULL,
	[ProductId] [nvarchar](50) NULL,
	[Total] [nvarchar](50) NULL,
	[sCount] [nvarchar](50) NULL,
	[DelFlag] [int] NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[pt_shoppingCart] ([ID], [SessionId], [ProductId], [Total], [sCount], [DelFlag], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'1', N'1', N'1', N'1', N'1', 0, N'admin', NULL, NULL, NULL)
/****** Object:  Table [dbo].[pt_productType]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_productType](
	[ID] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[pt_productType] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'1', NULL, N'车', NULL, NULL, NULL, NULL, NULL)
/****** Object:  Table [dbo].[pt_productSeries]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_productSeries](
	[ID] [nvarchar](50) NULL,
	[BrandId] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Depth] [nvarchar](50) NULL,
	[ParentId] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[pt_productSeries] ([ID], [BrandId], [Name], [Depth], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'1', N'1', N'雷凌', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productSeries] ([ID], [BrandId], [Name], [Depth], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'2', N'1', N'汉兰达', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productSeries] ([ID], [BrandId], [Name], [Depth], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'3', NULL, N'凯美瑞', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
/****** Object:  Table [dbo].[pt_productOperate]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_productOperate](
	[ID] [nvarchar](50) NOT NULL,
	[ProductId] [nvarchar](50) NULL,
	[OperateId] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_pt_productOperate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[pt_productOperate] ([ID], [ProductId], [OperateId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'1', N'1', N'5', N'我并没有做出什么评价来', N'otzvNjqgCta450ARcTsdyRFzgjGU', CAST(0x0000A4E30101A120 AS DateTime), N'admin', CAST(0x0000A4E30101A120 AS DateTime))
INSERT [dbo].[pt_productOperate] ([ID], [ProductId], [OperateId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'2', N'1', N'5', N'这车真心不错，就是价格有点偏贵', N'admin', CAST(0x0000A4E30101A120 AS DateTime), N'admin', CAST(0x0000A4E30101A120 AS DateTime))
INSERT [dbo].[pt_productOperate] ([ID], [ProductId], [OperateId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'3', N'1', N'2', NULL, N'admin', CAST(0x0000A4E30101A120 AS DateTime), N'admin', CAST(0x0000A4E30101A120 AS DateTime))
/****** Object:  Table [dbo].[pt_productModel]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_productModel](
	[ID] [nvarchar](50) NULL,
	[TypeId] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Depth] [nvarchar](50) NULL,
	[Sort] [int] NULL,
	[Icon] [nvarchar](50) NULL,
	[ParentId] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'10101', N'1', N'微型车', N'2', 1, NULL, N'101', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'10102', N'1', N'小型车', N'2', 1, NULL, N'101', N'', NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'10103', N'1', N'紧凑型车', N'2', 1, NULL, N'101', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'10104', N'1', N'中型车', N'2', 1, NULL, N'101', N'', NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'10105', N'1', N'中大型车', N'2', 1, NULL, N'101', N'', NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'10106', N'1', N'大型车', N'2', 1, NULL, N'101', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'102', N'1', N'SUV', N'1', 1, NULL, N'1', N'Sport Utility Vehicle，中文意思是运动型多用途汽车', NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'103', N'1', N'MPV', N'1', 1, NULL, N'1', N'多用途汽车(multi-Purpose Vehicles)', NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'104', N'1', N'跑车', N'1', 1, NULL, N'1', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'105', N'1', N'皮卡', N'1', 1, NULL, N'1', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'106', N'1', N'微面', N'1', 1, NULL, N'1', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'107', N'1', N'轻客', N'1', 1, NULL, N'1', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'108', N'1', N'CDV', N'1', 1, NULL, N'1', N'Car Derived Van，也就是说基于轿车平台的厢式车', NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productModel] ([ID], [TypeId], [Name], [Depth], [Sort], [Icon], [ParentId], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'101', N'1', N'轿车', N'1', 1, NULL, N'1', NULL, NULL, NULL, NULL, NULL)
/****** Object:  Table [dbo].[pt_productDetail]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_productDetail](
	[ID] [nvarchar](50) NULL,
	[ProductId] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Sort] [nvarchar](50) NULL,
	[GroupId] [nvarchar](50) NULL,
	[ParentId] [nvarchar](50) NULL,
	[Icon] [nvarchar](50) NULL,
	[Body] [nvarchar](50) NULL,
	[Url] [nvarchar](50) NULL,
	[DelFlag] [int] NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pt_productCustomField]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_productCustomField](
	[ID] [nvarchar](50) NULL,
	[ProductId] [nvarchar](50) NULL,
	[Field] [nvarchar](50) NULL,
	[Display] [nvarchar](50) NULL,
	[Value] [nvarchar](50) NULL,
	[DelFlag] [int] NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pt_productComment]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_productComment](
	[ID] [nvarchar](50) NULL,
	[ProductId] [nvarchar](50) NULL,
	[CommentText] [nvarchar](50) NULL,
	[DelFlag] [int] NULL,
	[CommentorId] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pt_productBrand]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_productBrand](
	[ID] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[pt_productBrand] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'1', NULL, N'丰田', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productBrand] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'2', NULL, N'本田', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productBrand] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'3', NULL, N'雷克萨斯', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productBrand] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'4', NULL, N'宝马', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productBrand] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'5', NULL, N'马自达', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productBrand] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'6', NULL, N'北京现代', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_productBrand] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'7', NULL, N'奥迪', NULL, NULL, NULL, NULL, NULL)
/****** Object:  Table [dbo].[pt_product]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_product](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Model] [nvarchar](50) NULL,
	[Series] [nvarchar](50) NULL,
	[Brand] [nvarchar](50) NULL,
	[Picture] [nvarchar](50) NULL,
	[CostPirce] [decimal](18, 0) NULL,
	[UnitPirce] [decimal](18, 0) NULL,
	[DelFlag] [int] NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'pt_product', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'型号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'pt_product', @level2type=N'COLUMN',@level2name=N'Model'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'pt_product', @level2type=N'COLUMN',@level2name=N'Series'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'pt_product', @level2type=N'COLUMN',@level2name=N'Brand'
GO
SET IDENTITY_INSERT [dbo].[pt_product] ON
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (1, NULL, N'IS250', N'1', N'101', N'1', N'1', N'images/allcar_01.jpg', CAST(375000 AS Decimal(18, 0)), CAST(375000 AS Decimal(18, 0)), 1, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (2, N'', N'IS250C', N'1', N'101', N'1', N'1', N'images/allcar_02.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (3, N'', N'IS250C', N'1', N'101', N'1', N'1', N'images/allcar_03.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (4, N'', N'ES250/350', N'1', N'101', N'2', N'1', N'images/allcar_04.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (5, N'', N'GS350', N'1', N'101', N'1', N'1', N'images/allcar_06.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (6, N'', N'RX270', N'1', N'102', N'3', N'1', N'images/allcar_08.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (7, N'', N'LX570', N'1', N'102', N'2', N'1', N'images/allcar_09.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (8, N'', N'CT200h', N'1', N'101', N'1', N'1', N'images/allcar_10.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (9, N'', N'EX300h', N'1', N'102', N'1', N'1', N'images/allcar_15.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
INSERT [dbo].[pt_product] ([ID], [Code], [Name], [Type], [Model], [Series], [Brand], [Picture], [CostPirce], [UnitPirce], [DelFlag], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (10, N'', N'LF-LC', N'1', N'103', N'2', N'1', N'images/allcar_17.jpg', CAST(645000 AS Decimal(18, 0)), CAST(645000 AS Decimal(18, 0)), 1, N'', N'default', CAST(0x0000A4DF00000000 AS DateTime), N'default', CAST(0x0000A4DF00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[pt_product] OFF
/****** Object:  Table [dbo].[pt_orderDetail]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_orderDetail](
	[ID] [nvarchar](50) NULL,
	[OrderId] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[SubTotal] [nvarchar](50) NULL,
	[StyleId] [nvarchar](50) NULL,
	[pCount] [nvarchar](50) NULL,
	[DelFlag] [int] NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pt_order]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_order](
	[ID] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[AddressId] [nvarchar](50) NULL,
	[Total] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[DelFlag] [int] NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pt_operate]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_operate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_pt_operate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[pt_operate] ON
INSERT [dbo].[pt_operate] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (1, NULL, N'点赞', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_operate] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (2, NULL, N'收藏', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_operate] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (3, NULL, N'查看', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_operate] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (4, NULL, N'分享', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_operate] ([ID], [Code], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (5, NULL, N'评价', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[pt_operate] OFF
/****** Object:  Table [dbo].[pt_navbar]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_navbar](
	[ID] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[DefaultSort] [int] NULL,
	[Depth] [nvarchar](50) NULL,
	[ParentId] [nvarchar](50) NULL,
	[Url] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[pt_navbar] ([ID], [Code], [Name], [DefaultSort], [Depth], [ParentId], [Url], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'1', N'00001', N'首页', 1, N'1', N'0', N'index.html', N'首页', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_navbar] ([ID], [Code], [Name], [DefaultSort], [Depth], [ParentId], [Url], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'2', N'00002', N'文章', 2, N'1', N'0', N'list.html', N'列表页', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_navbar] ([ID], [Code], [Name], [DefaultSort], [Depth], [ParentId], [Url], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'3', N'00003', N'广告', 3, N'1', N'0', N'advert.html', N'广告', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_navbar] ([ID], [Code], [Name], [DefaultSort], [Depth], [ParentId], [Url], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'4', N'00004', N'商城', 4, N'1', N'0', N'product.html', N'显示所有车型', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
/****** Object:  Table [dbo].[pt_companyType]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_companyType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_pt_companyType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[pt_companyType] ON
INSERT [dbo].[pt_companyType] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (1, N'国有独资公司', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyType] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (2, N'股份有限公司', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyType] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (3, N'个体工商户', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyType] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (4, N'个人独资企业', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyType] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (5, N'私营合伙企业', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[pt_companyType] OFF
/****** Object:  Table [dbo].[pt_companyTrade]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_companyTrade](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_pt_companyTrade] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[pt_companyTrade] ON
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (1, N'保险业', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (2, N'采矿', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (3, N'能源', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (4, N'餐饮', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (5, N'宾馆', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (6, N'电讯业', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (7, N'房地产', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (8, N'服务', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (9, N'服装业', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (10, N'公益组织', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (11, N'广告业', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (12, N'航空航天', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (13, N'化学', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (14, N'健康', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (15, N'保健', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (16, N'建筑业', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (17, N'教育', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (18, N'培训', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (19, N'计算机', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (20, N'金属冶炼', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (21, N'警察', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (22, N'餐饮', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (23, N'消防', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (24, N'军人', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (25, N'会计', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (26, N'服务', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (27, N'美容', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (28, N'媒体', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (29, N'出版', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (30, N'木材', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (31, N'造纸', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (32, N'零售', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (33, N'批发', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (34, N'农业', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (35, N'旅游业', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (36, N'司法', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (37, N'律师', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (38, N'司机', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (39, N'体育运动', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (40, N'学术研究', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (41, N'演艺', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (42, N'医疗服务', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (43, N'艺术', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (44, N'设计', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (45, N'银行', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (46, N'金融', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (47, N'因特网', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (48, N'音乐舞蹈', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (49, N'邮政快递', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (50, N'运输业', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (51, N'政府机关', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (52, N'机械制造', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companyTrade] ([ID], [Name], [Code], [Type], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (53, N'咨询', N'', N'', N'', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[pt_companyTrade] OFF
/****** Object:  Table [dbo].[pt_companySize]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_companySize](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_pt_companySize] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[pt_companySize] ON
INSERT [dbo].[pt_companySize] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (1, N'0-20', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companySize] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (2, N'20-50', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companySize] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (3, N'50-100', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companySize] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (4, N'100-500', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companySize] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (5, N'500-2000', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_companySize] ([ID], [Name], [Code], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (6, N'2000以上', NULL, NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[pt_companySize] OFF
/****** Object:  Table [dbo].[pt_companyArticle]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_companyArticle](
	[ID] [nvarchar](50) NULL,
	[CompanyID] [nvarchar](50) NULL,
	[ArticleID] [nvarchar](50) NULL,
	[Code] [nvarchar](60) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pt_company]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_company](
	[ID] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](100) NULL,
	[Leader] [nvarchar](50) NULL,
	[Founded] [datetime] NULL,
	[Size] [nvarchar](50) NULL,
	[Title] [nvarchar](50) NULL,
	[Logo] [nvarchar](50) NULL,
	[HomeUrl] [nvarchar](50) NULL,
	[Picture] [nvarchar](100) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Trade] [nvarchar](50) NULL,
	[DelFlag] [int] NULL,
	[Contactor] [nvarchar](50) NULL,
	[Summary] [nchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[pt_company] ([ID], [Name], [Code], [Address], [Leader], [Founded], [Size], [Title], [Logo], [HomeUrl], [Picture], [Phone], [Email], [Type], [Trade], [DelFlag], [Contactor], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'1', N'甜蜜窝科技有限公司', N'10010', N'广州', N'严雪', CAST(0x0000A4DE00000000 AS DateTime), N'1', N'日本本田', N'images/logo1.png', N'index.html', NULL, N'15913649874', N'gaegooo@126.com', N'1', N'1', 1, N'黄艳飞', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
/****** Object:  Table [dbo].[pt_articleType]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_articleType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](60) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[pt_articleType] ON
INSERT [dbo].[pt_articleType] ([ID], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (1, N'新闻信息', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_articleType] ([ID], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (2, N'首页横幅', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_articleType] ([ID], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (3, N'说明介绍', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_articleType] ([ID], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (4, N'推广宣传', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_articleType] ([ID], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (5, N'文案作品', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_articleType] ([ID], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (6, N'专业技术', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_articleType] ([ID], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (7, N'公司活动', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_articleType] ([ID], [Name], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (8, N'首页屏幕', NULL, N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[pt_articleType] OFF
/****** Object:  Table [dbo].[pt_articleOperate]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_articleOperate](
	[ID] [nvarchar](50) NULL,
	[ArticleId] [nvarchar](50) NULL,
	[OperateId] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pt_article]    Script Date: 07/28/2015 18:06:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pt_article](
	[ID] [nvarchar](50) NULL,
	[Title] [nvarchar](60) NULL,
	[Author] [nvarchar](20) NULL,
	[CoverPicture] [nvarchar](50) NULL,
	[Type] [int] NULL,
	[Flag] [int] NULL,
	[DelFlag] [int] NULL,
	[Url] [nvarchar](50) NULL,
	[body] [nvarchar](50) NULL,
	[Summary] [nvarchar](300) NULL,
	[Creater] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[Updater] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
GO
INSERT [dbo].[pt_article] ([ID], [Title], [Author], [CoverPicture], [Type], [Flag], [DelFlag], [Url], [body], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'001', N'棒球', N'jan', N'images/1.jpg', 2, 0, 1, N'detail.html', NULL, N'棒球运动是一种以棒打球为主要特点，集体性、对抗性很强的球类运动项目，在美国、日本尤为盛行。', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_article] ([ID], [Title], [Author], [CoverPicture], [Type], [Flag], [DelFlag], [Url], [body], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'002', N'冲浪', N'jan', N'images/2.jpg', 2, 0, 1, N'detail1.html', NULL, N'冲浪是以海浪为动力，利用自身的高超技巧和平衡能力，搏击海浪的一项运动。运动员站立在冲浪板上，或利用腹板、跪板、充气的橡皮垫、划艇、皮艇等驾驭海浪的一项水上运动。', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_article] ([ID], [Title], [Author], [CoverPicture], [Type], [Flag], [DelFlag], [Url], [body], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'003', N'自行车', N'jan', N'images/3.jpg', 2, 0, 1, N'detail.html', NULL, N'以自行车为工具比赛骑行速度的体育运动。1896年第一届奥林匹克运动会上被列为正式比赛项目。环法赛为最著名的世界自行车锦标赛。', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
INSERT [dbo].[pt_article] ([ID], [Title], [Author], [CoverPicture], [Type], [Flag], [DelFlag], [Url], [body], [Summary], [Creater], [CreateTime], [Updater], [UpdateTime]) VALUES (N'004', N'Hello, world!', N'jan', NULL, 8, 0, 1, NULL, NULL, N'Donec id elit non mi porta gravida at eget metus. Fusce dapibus, tellus ac cursus commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus. Etiam porta sem malesuada magna mollis euismod. Donec sed odio dui.', N'default', CAST(0x0000A4DE00000000 AS DateTime), N'default', CAST(0x0000A4DE00000000 AS DateTime))
/****** Object:  Default [DF_pt_article_Flag]    Script Date: 07/28/2015 18:06:22 ******/
ALTER TABLE [dbo].[pt_article] ADD  CONSTRAINT [DF_pt_article_Flag]  DEFAULT ((0)) FOR [Flag]
GO
/****** Object:  Default [DF_pt_article_DelFlag]    Script Date: 07/28/2015 18:06:22 ******/
ALTER TABLE [dbo].[pt_article] ADD  CONSTRAINT [DF_pt_article_DelFlag]  DEFAULT ((1)) FOR [DelFlag]
GO
/****** Object:  Default [DF_pt_productModel_TypeId]    Script Date: 07/28/2015 18:06:22 ******/
ALTER TABLE [dbo].[pt_productModel] ADD  CONSTRAINT [DF_pt_productModel_TypeId]  DEFAULT ((1)) FOR [TypeId]
GO
/****** Object:  Default [DF_pt_productType_Sort]    Script Date: 07/28/2015 18:06:22 ******/
ALTER TABLE [dbo].[pt_productModel] ADD  CONSTRAINT [DF_pt_productType_Sort]  DEFAULT ((1)) FOR [Sort]
GO
