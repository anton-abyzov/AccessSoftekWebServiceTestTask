USE [AccessSoftekDb]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 07.06.2017 13:18:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItemID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Orders]    Script Date: 07.06.2017 13:18:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[TotalMoney] [int] NULL,
	[CustomerID] [int] NULL,
	[OrderCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[OrderItems] ON 

GO
INSERT [dbo].[OrderItems] ([OrderItemID], [OrderID]) VALUES (1, 2)
GO
INSERT [dbo].[OrderItems] ([OrderItemID], [OrderID]) VALUES (2, 2)
GO
INSERT [dbo].[OrderItems] ([OrderItemID], [OrderID]) VALUES (3, 2)
GO
INSERT [dbo].[OrderItems] ([OrderItemID], [OrderID]) VALUES (4, 3)
GO
SET IDENTITY_INSERT [dbo].[OrderItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

GO
INSERT [dbo].[Orders] ([OrderID], [TotalMoney], [CustomerID], [OrderCode]) VALUES (1, 100, 1, N'a')
GO
INSERT [dbo].[Orders] ([OrderID], [TotalMoney], [CustomerID], [OrderCode]) VALUES (3, 150, 1, N'c')
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
ALTER TABLE [dbo].[OrderItems]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Orders]
GO