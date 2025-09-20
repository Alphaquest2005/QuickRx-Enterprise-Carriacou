INSERT INTO [dbo].[Item] ([Description], [ItemNotDiscountable], [ItemId], [ItemLookupCode], [Department], [Category], [Price], [Cost], [Quantity], [ExtendedDescription], [Inactive], [DateCreated], [ItemType], [SalesTax]) VALUES (N'Ticket', 0, 1, N'Ticket', N'CarPark', N'Ticket', CAST(2.5000 AS Decimal(19, 4)), CAST(2.5000 AS Decimal(19, 4)), 0, N'Ticket', 0, N'2012-01-01 00:00:00', N'Ticket',CAST(.15000 AS Decimal(19, 4)))
INSERT INTO [dbo].[Item] ([Description], [ItemNotDiscountable], [ItemId], [ItemLookupCode], [Department], [Category], [Price], [Cost], [Quantity], [ExtendedDescription], [Inactive], [DateCreated], [ItemType], [SalesTax]) VALUES (N'Golly Mix Drink', 0, 2, N'123', N'Groceries', N'Snacks', CAST(1.0000 AS Decimal(19, 4)), CAST(0.5000 AS Decimal(19, 4)), 100, N'Golly Mix Drink Nestle', 0, N'2012-01-01 00:00:00', N'Stock', CAST(.07000 AS Decimal(19, 4)))


SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT INTO [dbo].[Customers] ([CustomerId], [FirstName], [LastName], [CompanyName]) VALUES (1, N'Kellon', N'Rubin', N'Port')
SET IDENTITY_INSERT [dbo].[Customers] OFF

SET IDENTITY_INSERT [dbo].[Company] ON
INSERT INTO [dbo].[Company] ([Id], [CompanyName], [Address], [SoftwareName]) VALUES (1, N'GrenReal', N'St. George''s', N'QicTick')
SET IDENTITY_INSERT [dbo].[Company] OFF

SET IDENTITY_INSERT [dbo].[Cashiers] ON
INSERT INTO [dbo].[Cashiers] ([CashierId], [FirstName], [LastName]) VALUES (1, N'Joseph', N'Bartholomew')
SET IDENTITY_INSERT [dbo].[Cashiers] OFF

SET IDENTITY_INSERT [dbo].[Stores] ON
INSERT INTO [dbo].[Stores] ([StoreId], [StoreCode], [StoreAddress], [CompanyId]) VALUES (1, N'G2', N'Gate 2', 1)
SET IDENTITY_INSERT [dbo].[Stores] OFF

SET IDENTITY_INSERT [dbo].[Stations] ON
INSERT INTO [dbo].[Stations] ([StationId], [StationCode], [StoreId]) VALUES (1, N'S1', 1)
SET IDENTITY_INSERT [dbo].[Stations] OFF
