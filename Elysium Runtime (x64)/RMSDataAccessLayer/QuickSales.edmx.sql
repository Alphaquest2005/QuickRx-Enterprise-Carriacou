
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/16/2012 13:11:47
-- Generated from EDMX file: D:\Prism Projects\PrismApplication1\RMSDataAccessLayer\QuickSales.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [QuickSales];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TransactionTransactionEntry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntryBase] DROP CONSTRAINT [FK_TransactionTransactionEntry];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase] DROP CONSTRAINT [FK_CustomerTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerPass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pass] DROP CONSTRAINT [FK_CustomerPass];
GO
IF OBJECT_ID(N'[dbo].[FK_PassTicket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase_Ticket] DROP CONSTRAINT [FK_PassTicket];
GO
IF OBJECT_ID(N'[dbo].[FK_CashierTransactionBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase] DROP CONSTRAINT [FK_CashierTransactionBase];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchTransactionBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase] DROP CONSTRAINT [FK_BatchTransactionBase];
GO
IF OBJECT_ID(N'[dbo].[FK_StoreStation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stations] DROP CONSTRAINT [FK_StoreStation];
GO
IF OBJECT_ID(N'[dbo].[FK_StationTransactionBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase] DROP CONSTRAINT [FK_StationTransactionBase];
GO
IF OBJECT_ID(N'[dbo].[FK_TransactionBaseTenderEntryEx]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TenderEntryEx] DROP CONSTRAINT [FK_TransactionBaseTenderEntryEx];
GO
IF OBJECT_ID(N'[dbo].[FK_DoctorPrescription]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase_Prescription] DROP CONSTRAINT [FK_DoctorPrescription];
GO
IF OBJECT_ID(N'[dbo].[FK_TicketSetupTicketItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item_TicketItem] DROP CONSTRAINT [FK_TicketSetupTicketItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemTransactionEntryBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntryBase] DROP CONSTRAINT [FK_ItemTransactionEntryBase];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientPrescription]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase_Prescription] DROP CONSTRAINT [FK_PatientPrescription];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyStore]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stores] DROP CONSTRAINT [FK_CompanyStore];
GO
IF OBJECT_ID(N'[dbo].[FK_StationBatch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Batches] DROP CONSTRAINT [FK_StationBatch];
GO
IF OBJECT_ID(N'[dbo].[FK_CashierCashierLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CashierLogs] DROP CONSTRAINT [FK_CashierCashierLog];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchCashier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Batches] DROP CONSTRAINT [FK_BatchCashier];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchCashier1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Batches] DROP CONSTRAINT [FK_BatchCashier1];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchTransactionBase1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase] DROP CONSTRAINT [FK_BatchTransactionBase1];
GO
IF OBJECT_ID(N'[dbo].[FK_Ticket_inherits_TransactionBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase_Ticket] DROP CONSTRAINT [FK_Ticket_inherits_TransactionBase];
GO
IF OBJECT_ID(N'[dbo].[FK_Cashier_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Cashier] DROP CONSTRAINT [FK_Cashier_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Doctor_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Doctor] DROP CONSTRAINT [FK_Doctor_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Prescription_inherits_TransactionBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase_Prescription] DROP CONSTRAINT [FK_Prescription_inherits_TransactionBase];
GO
IF OBJECT_ID(N'[dbo].[FK_TicketItem_inherits_Item]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item_TicketItem] DROP CONSTRAINT [FK_TicketItem_inherits_Item];
GO
IF OBJECT_ID(N'[dbo].[FK_Patient_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Patient] DROP CONSTRAINT [FK_Patient_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_TicketEntry_inherits_TransactionEntryBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntryBase_TicketEntry] DROP CONSTRAINT [FK_TicketEntry_inherits_TransactionEntryBase];
GO
IF OBJECT_ID(N'[dbo].[FK_TransactionEntry_inherits_TransactionEntryBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntryBase_TransactionEntry] DROP CONSTRAINT [FK_TransactionEntry_inherits_TransactionEntryBase];
GO
IF OBJECT_ID(N'[dbo].[FK_Transaction_inherits_TransactionBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase_Transaction] DROP CONSTRAINT [FK_Transaction_inherits_TransactionBase];
GO
IF OBJECT_ID(N'[dbo].[FK_PrescriptionEntry_inherits_TransactionEntryBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntryBase_PrescriptionEntry] DROP CONSTRAINT [FK_PrescriptionEntry_inherits_TransactionEntryBase];
GO
IF OBJECT_ID(N'[dbo].[FK_Medicine_inherits_Item]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item_Medicine] DROP CONSTRAINT [FK_Medicine_inherits_Item];
GO
IF OBJECT_ID(N'[dbo].[FK_Customers_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Customers] DROP CONSTRAINT [FK_Customers_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_StockItem_inherits_Item]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item_StockItem] DROP CONSTRAINT [FK_StockItem_inherits_Item];
GO
IF OBJECT_ID(N'[dbo].[FK_QuickPrescription_inherits_TransactionBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionBase_QuickPrescription] DROP CONSTRAINT [FK_QuickPrescription_inherits_TransactionBase];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Item]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Item];
GO
IF OBJECT_ID(N'[dbo].[TransactionBase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionBase];
GO
IF OBJECT_ID(N'[dbo].[TransactionEntryBase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionEntryBase];
GO
IF OBJECT_ID(N'[dbo].[Company]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Company];
GO
IF OBJECT_ID(N'[dbo].[Persons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons];
GO
IF OBJECT_ID(N'[dbo].[Pass]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pass];
GO
IF OBJECT_ID(N'[dbo].[Batches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Batches];
GO
IF OBJECT_ID(N'[dbo].[Stations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stations];
GO
IF OBJECT_ID(N'[dbo].[Stores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stores];
GO
IF OBJECT_ID(N'[dbo].[TenderEntryEx]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TenderEntryEx];
GO
IF OBJECT_ID(N'[dbo].[TicketSetup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TicketSetup];
GO
IF OBJECT_ID(N'[dbo].[CashierLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CashierLogs];
GO
IF OBJECT_ID(N'[dbo].[TransactionBase_Ticket]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionBase_Ticket];
GO
IF OBJECT_ID(N'[dbo].[Persons_Cashier]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons_Cashier];
GO
IF OBJECT_ID(N'[dbo].[Persons_Doctor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons_Doctor];
GO
IF OBJECT_ID(N'[dbo].[TransactionBase_Prescription]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionBase_Prescription];
GO
IF OBJECT_ID(N'[dbo].[Item_TicketItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Item_TicketItem];
GO
IF OBJECT_ID(N'[dbo].[Persons_Patient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons_Patient];
GO
IF OBJECT_ID(N'[dbo].[TransactionEntryBase_TicketEntry]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionEntryBase_TicketEntry];
GO
IF OBJECT_ID(N'[dbo].[TransactionEntryBase_TransactionEntry]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionEntryBase_TransactionEntry];
GO
IF OBJECT_ID(N'[dbo].[TransactionBase_Transaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionBase_Transaction];
GO
IF OBJECT_ID(N'[dbo].[TransactionEntryBase_PrescriptionEntry]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionEntryBase_PrescriptionEntry];
GO
IF OBJECT_ID(N'[dbo].[Item_Medicine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Item_Medicine];
GO
IF OBJECT_ID(N'[dbo].[Persons_Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons_Customers];
GO
IF OBJECT_ID(N'[dbo].[Item_StockItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Item_StockItem];
GO
IF OBJECT_ID(N'[dbo].[TransactionBase_QuickPrescription]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionBase_QuickPrescription];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Item'
CREATE TABLE [dbo].[Item] (
    [Description] nvarchar(30)  NOT NULL,
    [ItemNotDiscountable] bit  NOT NULL,
    [ItemId] int IDENTITY(1,1) NOT NULL,
    [ItemLookupCode] nvarchar(25)  NOT NULL,
    [Department] nvarchar(30)  NOT NULL,
    [Category] nvarchar(30)  NOT NULL,
    [Price] decimal(19,4)  NOT NULL,
    [Cost] decimal(19,4)  NOT NULL,
    [Quantity] float  NOT NULL,
    [ExtendedDescription] nvarchar(max)  NOT NULL,
    [Inactive] bit  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [SalesTax] decimal(5,3)  NOT NULL
);
GO

-- Creating table 'TransactionBase'
CREATE TABLE [dbo].[TransactionBase] (
    [StationId] int  NOT NULL,
    [BatchId] int  NOT NULL,
    [CloseBatchId] int  NULL,
    [TransactionNumber] nvarchar(max)  NOT NULL,
    [Time] datetime  NOT NULL,
    [CustomerId] int  NULL,
    [CashierId] int  NOT NULL,
    [Comment] nvarchar(255)  NULL,
    [ReferenceNumber] nvarchar(50)  NULL,
    [Status] nvarchar(max)  NULL,
    [StoreCode] varchar(30)  NULL,
    [TransactionId] int IDENTITY(1,1) NOT NULL,
    [OpenClose] bit  NOT NULL
);
GO

-- Creating table 'TransactionEntryBase'
CREATE TABLE [dbo].[TransactionEntryBase] (
    [StoreID] int  NOT NULL,
    [TransactionEntryId] int IDENTITY(1,1) NOT NULL,
    [TransactionId] int  NOT NULL,
    [ItemId] int  NOT NULL,
    [Price] decimal(19,4)  NOT NULL,
    [Quantity] decimal(8,3)  NOT NULL,
    [Taxable] bit  NOT NULL,
    [Comment] nvarchar(255)  NULL,
    [TransactionTime] datetime  NULL,
    [SalesTaxPercent] decimal(19,4)  NOT NULL,
    [Discount] decimal(10,3)  NULL
);
GO

-- Creating table 'Company'
CREATE TABLE [dbo].[Company] (
    [CompanyId] int IDENTITY(1,1) NOT NULL,
    [CompanyName] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Address1] nvarchar(max)  NULL,
    [SoftwareName] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Motto] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Persons'
CREATE TABLE [dbo].[Persons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [CompanyName] nvarchar(max)  NULL,
    [Salutation] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [InActive] bit  NULL
);
GO

-- Creating table 'Pass'
CREATE TABLE [dbo].[Pass] (
    [PassId] int IDENTITY(1,1) NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [CustomerId] int  NOT NULL,
    [PassNumber] nvarchar(max)  NOT NULL,
    [FreePass] bit  NOT NULL
);
GO

-- Creating table 'Batches'
CREATE TABLE [dbo].[Batches] (
    [BatchId] int IDENTITY(1,1) NOT NULL,
    [OpeningCash] float  NOT NULL,
    [EndingCash] float  NULL,
    [OpeningTime] datetime  NOT NULL,
    [ClosingTime] datetime  NULL,
    [TotalTender] float  NULL,
    [TotalChange] float  NULL,
    [Status] nvarchar(max)  NOT NULL,
    [StationId] int  NOT NULL,
    [OpeningCashier] int  NOT NULL,
    [ClosingCashier] int  NULL,
    [Sales] float  NOT NULL,
    [OpenTransactions] int  NOT NULL,
    [CloseTransactions] int  NOT NULL
);
GO

-- Creating table 'Stations'
CREATE TABLE [dbo].[Stations] (
    [StationId] int IDENTITY(1,1) NOT NULL,
    [StationCode] nvarchar(max)  NOT NULL,
    [StoreId] int  NOT NULL,
    [ReceiptPrinterName] nvarchar(max)  NOT NULL,
    [MachineName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Stores'
CREATE TABLE [dbo].[Stores] (
    [StoreId] int IDENTITY(1,1) NOT NULL,
    [StoreCode] nvarchar(max)  NOT NULL,
    [StoreAddress] nvarchar(max)  NOT NULL,
    [CompanyId] int  NOT NULL,
    [TransactionSeed] int  NOT NULL,
    [SeedTransaction] int  NOT NULL
);
GO

-- Creating table 'TenderEntryEx'
CREATE TABLE [dbo].[TenderEntryEx] (
    [TenderEntryId] int IDENTITY(1,1) NOT NULL,
    [TransactionId] int  NOT NULL,
    [CashAmount] decimal(10,3)  NULL,
    [CustomerId] int  NULL,
    [CreditCardNumber] nvarchar(max)  NULL,
    [CreditCardAmount] decimal(10,3)  NULL,
    [CheckNumber] nvarchar(max)  NULL,
    [CheckAmount] decimal(10,3)  NULL,
    [AccountAmount] decimal(10,3)  NULL
);
GO

-- Creating table 'TicketSetup'
CREATE TABLE [dbo].[TicketSetup] (
    [FreeMinutes] int  NOT NULL,
    [ItemId] int  NOT NULL
);
GO

-- Creating table 'CashierLogs'
CREATE TABLE [dbo].[CashierLogs] (
    [CashierLogId] int IDENTITY(1,1) NOT NULL,
    [MachineName] nvarchar(max)  NOT NULL,
    [LoginTime] datetime  NOT NULL,
    [LogoutTime] datetime  NULL,
    [Status] nvarchar(max)  NOT NULL,
    [PersonId] int  NOT NULL
);
GO

-- Creating table 'TransactionBase_Ticket'
CREATE TABLE [dbo].[TransactionBase_Ticket] (
    [PassId] int  NULL,
    [TransactionId] int  NOT NULL
);
GO

-- Creating table 'Persons_Cashier'
CREATE TABLE [dbo].[Persons_Cashier] (
    [SPassword] nvarchar(max)  NULL,
    [LoginName] nvarchar(max)  NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Persons_Doctor'
CREATE TABLE [dbo].[Persons_Doctor] (
    [Code] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'TransactionBase_Prescription'
CREATE TABLE [dbo].[TransactionBase_Prescription] (
    [DoctorId] int  NULL,
    [Repeat] int  NOT NULL,
    [PatientId] int  NULL,
    [TransactionId] int  NOT NULL
);
GO

-- Creating table 'Item_TicketItem'
CREATE TABLE [dbo].[Item_TicketItem] (
    [Price1] decimal(5,3)  NOT NULL,
    [Price2] decimal(5,3)  NOT NULL,
    [ItemId] int  NOT NULL
);
GO

-- Creating table 'Persons_Patient'
CREATE TABLE [dbo].[Persons_Patient] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'TransactionEntryBase_TicketEntry'
CREATE TABLE [dbo].[TransactionEntryBase_TicketEntry] (
    [VehicleNumber] nvarchar(max)  NULL,
    [StartDateTime] datetime  NOT NULL,
    [EndDateTime] datetime  NULL,
    [TransactionEntryId] int  NOT NULL
);
GO

-- Creating table 'TransactionEntryBase_TransactionEntry'
CREATE TABLE [dbo].[TransactionEntryBase_TransactionEntry] (
    [TransactionEntryId] int  NOT NULL
);
GO

-- Creating table 'TransactionBase_Transaction'
CREATE TABLE [dbo].[TransactionBase_Transaction] (
    [TransactionId] int  NOT NULL
);
GO

-- Creating table 'TransactionEntryBase_PrescriptionEntry'
CREATE TABLE [dbo].[TransactionEntryBase_PrescriptionEntry] (
    [Dosage] nvarchar(max)  NULL,
    [ExpiryDate] nvarchar(max)  NULL,
    [TransactionEntryId] int  NOT NULL
);
GO

-- Creating table 'Item_Medicine'
CREATE TABLE [dbo].[Item_Medicine] (
    [SuggestedDosage] nvarchar(max)  NOT NULL,
    [ItemId] int  NOT NULL
);
GO

-- Creating table 'Persons_Customers'
CREATE TABLE [dbo].[Persons_Customers] (
    [CustomerType] nvarchar(max)  NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Item_StockItem'
CREATE TABLE [dbo].[Item_StockItem] (
    [ItemId] int  NOT NULL
);
GO

-- Creating table 'TransactionBase_QuickPrescription'
CREATE TABLE [dbo].[TransactionBase_QuickPrescription] (
    [TransactionId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ItemId] in table 'Item'
ALTER TABLE [dbo].[Item]
ADD CONSTRAINT [PK_Item]
    PRIMARY KEY CLUSTERED ([ItemId] ASC);
GO

-- Creating primary key on [TransactionId] in table 'TransactionBase'
ALTER TABLE [dbo].[TransactionBase]
ADD CONSTRAINT [PK_TransactionBase]
    PRIMARY KEY CLUSTERED ([TransactionId] ASC);
GO

-- Creating primary key on [TransactionEntryId] in table 'TransactionEntryBase'
ALTER TABLE [dbo].[TransactionEntryBase]
ADD CONSTRAINT [PK_TransactionEntryBase]
    PRIMARY KEY CLUSTERED ([TransactionEntryId] ASC);
GO

-- Creating primary key on [CompanyId] in table 'Company'
ALTER TABLE [dbo].[Company]
ADD CONSTRAINT [PK_Company]
    PRIMARY KEY CLUSTERED ([CompanyId] ASC);
GO

-- Creating primary key on [Id] in table 'Persons'
ALTER TABLE [dbo].[Persons]
ADD CONSTRAINT [PK_Persons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [PassId] in table 'Pass'
ALTER TABLE [dbo].[Pass]
ADD CONSTRAINT [PK_Pass]
    PRIMARY KEY CLUSTERED ([PassId] ASC);
GO

-- Creating primary key on [BatchId] in table 'Batches'
ALTER TABLE [dbo].[Batches]
ADD CONSTRAINT [PK_Batches]
    PRIMARY KEY CLUSTERED ([BatchId] ASC);
GO

-- Creating primary key on [StationId] in table 'Stations'
ALTER TABLE [dbo].[Stations]
ADD CONSTRAINT [PK_Stations]
    PRIMARY KEY CLUSTERED ([StationId] ASC);
GO

-- Creating primary key on [StoreId] in table 'Stores'
ALTER TABLE [dbo].[Stores]
ADD CONSTRAINT [PK_Stores]
    PRIMARY KEY CLUSTERED ([StoreId] ASC);
GO

-- Creating primary key on [TenderEntryId] in table 'TenderEntryEx'
ALTER TABLE [dbo].[TenderEntryEx]
ADD CONSTRAINT [PK_TenderEntryEx]
    PRIMARY KEY CLUSTERED ([TenderEntryId] ASC);
GO

-- Creating primary key on [ItemId] in table 'TicketSetup'
ALTER TABLE [dbo].[TicketSetup]
ADD CONSTRAINT [PK_TicketSetup]
    PRIMARY KEY CLUSTERED ([ItemId] ASC);
GO

-- Creating primary key on [CashierLogId] in table 'CashierLogs'
ALTER TABLE [dbo].[CashierLogs]
ADD CONSTRAINT [PK_CashierLogs]
    PRIMARY KEY CLUSTERED ([CashierLogId] ASC);
GO

-- Creating primary key on [TransactionId] in table 'TransactionBase_Ticket'
ALTER TABLE [dbo].[TransactionBase_Ticket]
ADD CONSTRAINT [PK_TransactionBase_Ticket]
    PRIMARY KEY CLUSTERED ([TransactionId] ASC);
GO

-- Creating primary key on [Id] in table 'Persons_Cashier'
ALTER TABLE [dbo].[Persons_Cashier]
ADD CONSTRAINT [PK_Persons_Cashier]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Persons_Doctor'
ALTER TABLE [dbo].[Persons_Doctor]
ADD CONSTRAINT [PK_Persons_Doctor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [TransactionId] in table 'TransactionBase_Prescription'
ALTER TABLE [dbo].[TransactionBase_Prescription]
ADD CONSTRAINT [PK_TransactionBase_Prescription]
    PRIMARY KEY CLUSTERED ([TransactionId] ASC);
GO

-- Creating primary key on [ItemId] in table 'Item_TicketItem'
ALTER TABLE [dbo].[Item_TicketItem]
ADD CONSTRAINT [PK_Item_TicketItem]
    PRIMARY KEY CLUSTERED ([ItemId] ASC);
GO

-- Creating primary key on [Id] in table 'Persons_Patient'
ALTER TABLE [dbo].[Persons_Patient]
ADD CONSTRAINT [PK_Persons_Patient]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [TransactionEntryId] in table 'TransactionEntryBase_TicketEntry'
ALTER TABLE [dbo].[TransactionEntryBase_TicketEntry]
ADD CONSTRAINT [PK_TransactionEntryBase_TicketEntry]
    PRIMARY KEY CLUSTERED ([TransactionEntryId] ASC);
GO

-- Creating primary key on [TransactionEntryId] in table 'TransactionEntryBase_TransactionEntry'
ALTER TABLE [dbo].[TransactionEntryBase_TransactionEntry]
ADD CONSTRAINT [PK_TransactionEntryBase_TransactionEntry]
    PRIMARY KEY CLUSTERED ([TransactionEntryId] ASC);
GO

-- Creating primary key on [TransactionId] in table 'TransactionBase_Transaction'
ALTER TABLE [dbo].[TransactionBase_Transaction]
ADD CONSTRAINT [PK_TransactionBase_Transaction]
    PRIMARY KEY CLUSTERED ([TransactionId] ASC);
GO

-- Creating primary key on [TransactionEntryId] in table 'TransactionEntryBase_PrescriptionEntry'
ALTER TABLE [dbo].[TransactionEntryBase_PrescriptionEntry]
ADD CONSTRAINT [PK_TransactionEntryBase_PrescriptionEntry]
    PRIMARY KEY CLUSTERED ([TransactionEntryId] ASC);
GO

-- Creating primary key on [ItemId] in table 'Item_Medicine'
ALTER TABLE [dbo].[Item_Medicine]
ADD CONSTRAINT [PK_Item_Medicine]
    PRIMARY KEY CLUSTERED ([ItemId] ASC);
GO

-- Creating primary key on [Id] in table 'Persons_Customers'
ALTER TABLE [dbo].[Persons_Customers]
ADD CONSTRAINT [PK_Persons_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ItemId] in table 'Item_StockItem'
ALTER TABLE [dbo].[Item_StockItem]
ADD CONSTRAINT [PK_Item_StockItem]
    PRIMARY KEY CLUSTERED ([ItemId] ASC);
GO

-- Creating primary key on [TransactionId] in table 'TransactionBase_QuickPrescription'
ALTER TABLE [dbo].[TransactionBase_QuickPrescription]
ADD CONSTRAINT [PK_TransactionBase_QuickPrescription]
    PRIMARY KEY CLUSTERED ([TransactionId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TransactionId] in table 'TransactionEntryBase'
ALTER TABLE [dbo].[TransactionEntryBase]
ADD CONSTRAINT [FK_TransactionTransactionEntry]
    FOREIGN KEY ([TransactionId])
    REFERENCES [dbo].[TransactionBase]
        ([TransactionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TransactionTransactionEntry'
CREATE INDEX [IX_FK_TransactionTransactionEntry]
ON [dbo].[TransactionEntryBase]
    ([TransactionId]);
GO

-- Creating foreign key on [CustomerId] in table 'TransactionBase'
ALTER TABLE [dbo].[TransactionBase]
ADD CONSTRAINT [FK_CustomerTransaction]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Persons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerTransaction'
CREATE INDEX [IX_FK_CustomerTransaction]
ON [dbo].[TransactionBase]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'Pass'
ALTER TABLE [dbo].[Pass]
ADD CONSTRAINT [FK_CustomerPass]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Persons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerPass'
CREATE INDEX [IX_FK_CustomerPass]
ON [dbo].[Pass]
    ([CustomerId]);
GO

-- Creating foreign key on [PassId] in table 'TransactionBase_Ticket'
ALTER TABLE [dbo].[TransactionBase_Ticket]
ADD CONSTRAINT [FK_PassTicket]
    FOREIGN KEY ([PassId])
    REFERENCES [dbo].[Pass]
        ([PassId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PassTicket'
CREATE INDEX [IX_FK_PassTicket]
ON [dbo].[TransactionBase_Ticket]
    ([PassId]);
GO

-- Creating foreign key on [CashierId] in table 'TransactionBase'
ALTER TABLE [dbo].[TransactionBase]
ADD CONSTRAINT [FK_CashierTransactionBase]
    FOREIGN KEY ([CashierId])
    REFERENCES [dbo].[Persons_Cashier]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CashierTransactionBase'
CREATE INDEX [IX_FK_CashierTransactionBase]
ON [dbo].[TransactionBase]
    ([CashierId]);
GO

-- Creating foreign key on [BatchId] in table 'TransactionBase'
ALTER TABLE [dbo].[TransactionBase]
ADD CONSTRAINT [FK_BatchTransactionBase]
    FOREIGN KEY ([BatchId])
    REFERENCES [dbo].[Batches]
        ([BatchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchTransactionBase'
CREATE INDEX [IX_FK_BatchTransactionBase]
ON [dbo].[TransactionBase]
    ([BatchId]);
GO

-- Creating foreign key on [StoreId] in table 'Stations'
ALTER TABLE [dbo].[Stations]
ADD CONSTRAINT [FK_StoreStation]
    FOREIGN KEY ([StoreId])
    REFERENCES [dbo].[Stores]
        ([StoreId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StoreStation'
CREATE INDEX [IX_FK_StoreStation]
ON [dbo].[Stations]
    ([StoreId]);
GO

-- Creating foreign key on [StationId] in table 'TransactionBase'
ALTER TABLE [dbo].[TransactionBase]
ADD CONSTRAINT [FK_StationTransactionBase]
    FOREIGN KEY ([StationId])
    REFERENCES [dbo].[Stations]
        ([StationId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StationTransactionBase'
CREATE INDEX [IX_FK_StationTransactionBase]
ON [dbo].[TransactionBase]
    ([StationId]);
GO

-- Creating foreign key on [TransactionId] in table 'TenderEntryEx'
ALTER TABLE [dbo].[TenderEntryEx]
ADD CONSTRAINT [FK_TransactionBaseTenderEntryEx]
    FOREIGN KEY ([TransactionId])
    REFERENCES [dbo].[TransactionBase]
        ([TransactionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TransactionBaseTenderEntryEx'
CREATE INDEX [IX_FK_TransactionBaseTenderEntryEx]
ON [dbo].[TenderEntryEx]
    ([TransactionId]);
GO

-- Creating foreign key on [DoctorId] in table 'TransactionBase_Prescription'
ALTER TABLE [dbo].[TransactionBase_Prescription]
ADD CONSTRAINT [FK_DoctorPrescription]
    FOREIGN KEY ([DoctorId])
    REFERENCES [dbo].[Persons_Doctor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DoctorPrescription'
CREATE INDEX [IX_FK_DoctorPrescription]
ON [dbo].[TransactionBase_Prescription]
    ([DoctorId]);
GO

-- Creating foreign key on [ItemId] in table 'Item_TicketItem'
ALTER TABLE [dbo].[Item_TicketItem]
ADD CONSTRAINT [FK_TicketSetupTicketItem]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[TicketSetup]
        ([ItemId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ItemId] in table 'TransactionEntryBase'
ALTER TABLE [dbo].[TransactionEntryBase]
ADD CONSTRAINT [FK_ItemTransactionEntryBase]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[Item]
        ([ItemId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemTransactionEntryBase'
CREATE INDEX [IX_FK_ItemTransactionEntryBase]
ON [dbo].[TransactionEntryBase]
    ([ItemId]);
GO

-- Creating foreign key on [PatientId] in table 'TransactionBase_Prescription'
ALTER TABLE [dbo].[TransactionBase_Prescription]
ADD CONSTRAINT [FK_PatientPrescription]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Persons_Patient]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientPrescription'
CREATE INDEX [IX_FK_PatientPrescription]
ON [dbo].[TransactionBase_Prescription]
    ([PatientId]);
GO

-- Creating foreign key on [CompanyId] in table 'Stores'
ALTER TABLE [dbo].[Stores]
ADD CONSTRAINT [FK_CompanyStore]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([CompanyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyStore'
CREATE INDEX [IX_FK_CompanyStore]
ON [dbo].[Stores]
    ([CompanyId]);
GO

-- Creating foreign key on [StationId] in table 'Batches'
ALTER TABLE [dbo].[Batches]
ADD CONSTRAINT [FK_StationBatch]
    FOREIGN KEY ([StationId])
    REFERENCES [dbo].[Stations]
        ([StationId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StationBatch'
CREATE INDEX [IX_FK_StationBatch]
ON [dbo].[Batches]
    ([StationId]);
GO

-- Creating foreign key on [PersonId] in table 'CashierLogs'
ALTER TABLE [dbo].[CashierLogs]
ADD CONSTRAINT [FK_CashierCashierLog]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Persons_Cashier]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CashierCashierLog'
CREATE INDEX [IX_FK_CashierCashierLog]
ON [dbo].[CashierLogs]
    ([PersonId]);
GO

-- Creating foreign key on [OpeningCashier] in table 'Batches'
ALTER TABLE [dbo].[Batches]
ADD CONSTRAINT [FK_BatchCashier]
    FOREIGN KEY ([OpeningCashier])
    REFERENCES [dbo].[Persons_Cashier]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchCashier'
CREATE INDEX [IX_FK_BatchCashier]
ON [dbo].[Batches]
    ([OpeningCashier]);
GO

-- Creating foreign key on [ClosingCashier] in table 'Batches'
ALTER TABLE [dbo].[Batches]
ADD CONSTRAINT [FK_BatchCashier1]
    FOREIGN KEY ([ClosingCashier])
    REFERENCES [dbo].[Persons_Cashier]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchCashier1'
CREATE INDEX [IX_FK_BatchCashier1]
ON [dbo].[Batches]
    ([ClosingCashier]);
GO

-- Creating foreign key on [CloseBatchId] in table 'TransactionBase'
ALTER TABLE [dbo].[TransactionBase]
ADD CONSTRAINT [FK_BatchTransactionBase1]
    FOREIGN KEY ([CloseBatchId])
    REFERENCES [dbo].[Batches]
        ([BatchId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchTransactionBase1'
CREATE INDEX [IX_FK_BatchTransactionBase1]
ON [dbo].[TransactionBase]
    ([CloseBatchId]);
GO

-- Creating foreign key on [TransactionId] in table 'TransactionBase_Ticket'
ALTER TABLE [dbo].[TransactionBase_Ticket]
ADD CONSTRAINT [FK_Ticket_inherits_TransactionBase]
    FOREIGN KEY ([TransactionId])
    REFERENCES [dbo].[TransactionBase]
        ([TransactionId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Persons_Cashier'
ALTER TABLE [dbo].[Persons_Cashier]
ADD CONSTRAINT [FK_Cashier_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Persons]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Persons_Doctor'
ALTER TABLE [dbo].[Persons_Doctor]
ADD CONSTRAINT [FK_Doctor_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Persons]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TransactionId] in table 'TransactionBase_Prescription'
ALTER TABLE [dbo].[TransactionBase_Prescription]
ADD CONSTRAINT [FK_Prescription_inherits_TransactionBase]
    FOREIGN KEY ([TransactionId])
    REFERENCES [dbo].[TransactionBase]
        ([TransactionId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ItemId] in table 'Item_TicketItem'
ALTER TABLE [dbo].[Item_TicketItem]
ADD CONSTRAINT [FK_TicketItem_inherits_Item]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[Item]
        ([ItemId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Persons_Patient'
ALTER TABLE [dbo].[Persons_Patient]
ADD CONSTRAINT [FK_Patient_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Persons]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TransactionEntryId] in table 'TransactionEntryBase_TicketEntry'
ALTER TABLE [dbo].[TransactionEntryBase_TicketEntry]
ADD CONSTRAINT [FK_TicketEntry_inherits_TransactionEntryBase]
    FOREIGN KEY ([TransactionEntryId])
    REFERENCES [dbo].[TransactionEntryBase]
        ([TransactionEntryId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TransactionEntryId] in table 'TransactionEntryBase_TransactionEntry'
ALTER TABLE [dbo].[TransactionEntryBase_TransactionEntry]
ADD CONSTRAINT [FK_TransactionEntry_inherits_TransactionEntryBase]
    FOREIGN KEY ([TransactionEntryId])
    REFERENCES [dbo].[TransactionEntryBase]
        ([TransactionEntryId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TransactionId] in table 'TransactionBase_Transaction'
ALTER TABLE [dbo].[TransactionBase_Transaction]
ADD CONSTRAINT [FK_Transaction_inherits_TransactionBase]
    FOREIGN KEY ([TransactionId])
    REFERENCES [dbo].[TransactionBase]
        ([TransactionId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TransactionEntryId] in table 'TransactionEntryBase_PrescriptionEntry'
ALTER TABLE [dbo].[TransactionEntryBase_PrescriptionEntry]
ADD CONSTRAINT [FK_PrescriptionEntry_inherits_TransactionEntryBase]
    FOREIGN KEY ([TransactionEntryId])
    REFERENCES [dbo].[TransactionEntryBase]
        ([TransactionEntryId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ItemId] in table 'Item_Medicine'
ALTER TABLE [dbo].[Item_Medicine]
ADD CONSTRAINT [FK_Medicine_inherits_Item]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[Item]
        ([ItemId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Persons_Customers'
ALTER TABLE [dbo].[Persons_Customers]
ADD CONSTRAINT [FK_Customers_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Persons]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ItemId] in table 'Item_StockItem'
ALTER TABLE [dbo].[Item_StockItem]
ADD CONSTRAINT [FK_StockItem_inherits_Item]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[Item]
        ([ItemId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TransactionId] in table 'TransactionBase_QuickPrescription'
ALTER TABLE [dbo].[TransactionBase_QuickPrescription]
ADD CONSTRAINT [FK_QuickPrescription_inherits_TransactionBase]
    FOREIGN KEY ([TransactionId])
    REFERENCES [dbo].[TransactionBase]
        ([TransactionId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------