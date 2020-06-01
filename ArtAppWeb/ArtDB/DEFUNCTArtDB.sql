
/* Check if database already exists */

IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name= 'ArtAppDB')
BEGIN
	DROP DATABASE [ArtAppDB]
	print '' print '*** Dropping Database BicycleDB_AM'
END
GO

print '' print '*** Creating Database BicycleDB_AM'

GO

CREATE DATABASE [ArtAppDB]
GO

print '' print '*** Using Database BicycleDB_AM'
GO

USE [ArtAppDB]
GO

print '' print '*** Creating User Table'
GO
CREATE TABLE [dbo].[User](
	[UserID] 	    [int] IDENTITY(1000000,1) 	NOT NULL,
	[Name] 	        [nvarchar](100)				NOT NULL,
	[PhoneNumber] 	[nvarchar](11)  			NOT NULL,
	[Email]			[nvarchar](250) 			NOT NULL,
	[PasswordHash]	[nvarchar](100) 			NOT NULL
		DEFAULT '9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E',
	[Active]		[bit]	                    NOT NULL DEFAULT 1,
	CONSTRAINT [pk_UserID] PRIMARY KEY([UserID] ASC),
	CONSTRAINT [ak_Email] UNIQUE([Email] ASC)
)
GO

print '' print '*** Creating sp_insert_user'
GO
CREATE PROCEDURE [sp_insert_user]
(
	@FirstName		[nvarchar](100),
	@PhoneNumber	[nvarchar](11),
	@Email			[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[User]
		([FirstName], [PhoneNumber], [Email])
	VALUES
		(@FirstName, @PhoneNumber, @Email)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_insert_user_no_phone'
GO
CREATE PROCEDURE [sp_insert_user_no_phone]
(
	@FirstName		[nvarchar](100),
	@Email			[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[User]
		([FirstName], [Email])
	VALUES
		(@FirstName, @Email)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_select_user_by_email'
GO
CREATE PROCEDURE [sp_select_user_by_email]
(
	@Email			[nvarchar](250)
)
AS
BEGIN
	SELECT [UserID], [Name], [PhoneNumber]
	FROM [dbo].[User]
    WHERE [Email] = @Email
END 
GO

print '' print '*** Creating sp_select_roles_by_UserID'
GO
CREATE PROCEDURE [sp_select_roles_by_userID]
(
	@UserID	 [int]
)
AS
BEGIN
	SELECT [RoleID]
	FROM [dbo].[UserRole]
    WHERE [UserID] = @UserID
END 
GO

print '' print '*** Adding index for name in User Table'
GO
CREATE NONCLUSTERED INDEX [ix_UserName] ON [User]([Name] ASC)
GO

print '' print '*** Adding index for email in User Table'
GO
CREATE NONCLUSTERED INDEX [ix_UserEmail] ON [User]([Email] ASC)
GO


print '' print '*** Creating Role Table'
GO
CREATE TABLE [dbo].[Role](
	[RoleID]		[nvarchar](50) NOT NULL,
	[Description] 	[nvarchar](250)	   NULL,
	CONSTRAINT [pk_RoleID] PRIMARY KEY([RoleID] ASC)
)
GO

print '' print '*** Creating sp_insert_role'
GO
CREATE PROCEDURE [sp_insert_role]
(
    @RoleID             [nvarchar](50),
	@Description		[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[Role]
		([RoleID], [Description])
	VALUES
		(@RoleID, @Description)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating Skill Table'
GO
CREATE TABLE [dbo].[Skill](
	[SkillID]		[nvarchar](50) NOT NULL,
	[Description] 	[nvarchar](250)	   NULL,
	CONSTRAINT [pk_RoleID] PRIMARY KEY([RoleID] ASC)
)
GO

print '' print '*** Creating sp_insert_skill'
GO
CREATE PROCEDURE [sp_insert_skill]
(
    @RoleID             [nvarchar](50),
	@Description		[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[Skill]
		([SkillID], [Description])
	VALUES
		(@SkillID, @Description)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating UserRole Table'
GO
CREATE TABLE [dbo].[UserRole](
	[UserID] 	[int] 			NOT NULL,
	[RoleID]		[nvarchar](50) 	NOT NULL,
	CONSTRAINT [pk_UserID_RoleID] PRIMARY KEY([UserID] ASC, [RoleId] ASC),
	CONSTRAINT [fk_User_UserID] FOREIGN KEY([UserID])
		REFERENCES [User]([UserID]),
	CONSTRAINT [fk_Role_RoleID] FOREIGN KEY([RoleID])
		REFERENCES [Role]([RoleID]) ON UPDATE CASCADE
	
)
GO

print '' print '*** Creating sp_insert_user_role'
GO
CREATE PROCEDURE [sp_insert_user_role]
(
    @UserID         [int],
	@RoleID             [nvarchar](50)
)
AS
BEGIN
	INSERT INTO [dbo].[UserRole]
		([UserID], [RoleID])
	VALUES
		(@UserID, @RoleID)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating UserSkill Table'
GO
CREATE TABLE [dbo].[UserSkill](
	[UserID] 	[int] 			NOT NULL,
	[SkillID]		[nvarchar](50) 	NOT NULL,
	CONSTRAINT [pk_UserID_SkillID] PRIMARY KEY([UserID] ASC, [SkillId] ASC),
	CONSTRAINT [fk_User_UserID] FOREIGN KEY([UserID])
		REFERENCES [User]([UserID]),
	CONSTRAINT [fk_Skill_SkillID] FOREIGN KEY([SkillID])
		REFERENCES [Skill]([SkillID]) ON UPDATE CASCADE
	
)
GO

print '' print '*** Creating sp_insert_user_Skill'
GO
CREATE PROCEDURE [sp_insert_user_Skill]
(
    @UserID         [int],
	@SkillID             [nvarchar](50)
)
AS
BEGIN
	INSERT INTO [dbo].[UserSkill]
		([UserID], [SkillID])
	VALUES
		(@UserID, @SkillID)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating Sample User Records'
GO
INSERT INTO [dbo].[User]
	([Name], [PhoneNumber], [Email])
	VALUES
	('System Admin', '1231230000', 'admin@company.com'),
	('Joseph Joestar', '1231231234', 'jstar@mail.com'),
	('Caesar Zeppeli', '1112223344', 'bubbleguy@mail.com'),
	('Bob Bobson', '9877654321', 'bob@bob.com')
GO

print '' print '*** Creating Sample Deactivated User Records'
GO
INSERT INTO [dbo].[User]
	([Name], [PhoneNumber], [Email], [Active])
	VALUES
	('Ben Hanna', '1113334455', 'ass@butts.com', 0)

print '' print '*** Creating Sample Role Records'
GO
INSERT INTO [dbo].[Role]
	([RoleID])
	VALUES
	('Administrator'),
	('Manager'),
	('Order'),
	('Inspection'),
	('Maintenance'),
	('CheckOut'),
	('Prep')
GO

print '' print '*** Creating Sample Skill Records'
GO
INSERT INTO [dbo].[Skill]
	([SkillID])
	VALUES
	('Administration'),
	('Writing'),
	('Realistic Painting'),
	('Stylized Drawing'),
	('Video Editing'),
	('Singing'),
	('Coloring')
GO

print '' print '*** Creating Sample UserSkill Records'
GO
INSERT INTO [dbo].[UserSkill]
	([UserID], [SkillID])
	VALUES
	(1000000, 'Administration'),
	(1000001, 'Writing'),
	(1000001, 'Realistic Painting'),
	(1000002, 'Stylized Drawing'),
	(1000002, 'Video Editing'),
	(1000003, 'Singing'),
	(1000003, 'Coloring')
GO

print '' print '*** Creating sp_authenticate_user'
GO
CREATE PROCEDURE [sp_authenticate_user]
(
	@Email			[nvarchar](250),
	@PasswordHash	[nvarchar](100)
)
AS
BEGIN
	SELECT  COUNT([UserID])
	FROM 	[dbo].[User]
	WHERE	[Email] = @Email
		AND	[PasswordHash] = @PasswordHash
		AND	[Active] = 1
END 
GO

print '' print '*** Creating sp_update_email'
GO
CREATE PROCEDURE [sp_update_email]
(
	@OldEmail		[nvarchar](250),
	@NewEmail		[nvarchar](250),
	@PasswordHash	[nvarchar](100)
)
AS
BEGIN
	UPDATE 	[dbo].[User]
	SET		[Email] = @NewEmail
	WHERE	[Email] = @OldEmail
		AND	[PasswordHash] = @PasswordHash
		AND	[Active] = 1
	RETURN @@ROWCOUNT
END 
GO

print '' print '*** Creating sp_update_password'
GO
CREATE PROCEDURE [sp_update_password]
(
    @UserID         [int],
	@OldPasswordHash	[nvarchar](100),
	@NewPasswordHash	[nvarchar](100)
	
)
AS
BEGIN
	UPDATE 	[dbo].[User]
	SET		[PasswordHash] = @NewPasswordHash
	WHERE	[UserID]   = @UserID
    AND	    [PasswordHash] = @OldPasswordHash
    AND	    [Active] = 1
	RETURN @@ROWCOUNT
END 
GO



print '' print '*** Creating CompensationMethod Table'
GO
CREATE TABLE [dbo].[CompensationMethod](
	[CompensationMethodID][int] IDENTITY(1000000,1) 	NOT NULL,
    [Type]           [nvarchar](25)             NOT NULL,
	[ExpirationDate] [date] 					    NULL,
	[CardNumber] 	 [nvarchar](25)				NOT NULL,
    [Preferred]      [bit]                      NOT NULL DEFAULT 0,
	[UserID] 	     [int] 						NOT NULL,
	CONSTRAINT [pk_CompensationMethodID] PRIMARY KEY([CompensationMethodID] ASC),
	CONSTRAINT [fk_CompensationMethod_ClientID] FOREIGN KEY([ClientID])
		REFERENCES [User]([UserID]),
	CONSTRAINT [ak_CardNumber] UNIQUE([CardNumber] ASC)
)
GO

print '' print '*** Creating sp_insert_CompensationMethod'
GO
CREATE PROCEDURE [sp_insert_compensation_method]
(
	@ExpirationDate	[date],
    @Type           [nvarchar](25),
	@CardNumber		[nvarchar](25),
    @Preferred      [bit],
	@ClientID    	[int]
)
AS
BEGIN
	INSERT INTO [dbo].[CompensationMethod]
		([ExpirationDate], [Type], [CardNumber], [ClientID])
	VALUES
		(@ExpirationDate, @Type, @CardNumber, @ClientID)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating Sample CompensationMethod Records'
GO
INSERT INTO [dbo].[CompensationMethod]
	([ExpirationDate], [CardNumber], [ClientID])
	VALUES
	('2020-01-20', 'Sample type', '123412341434', 0, 1000000),
	('2020-04-25', 'Sample type', '121412541264', 0, 1000001),
	('2020-03-15', 'Sample type', '122412741234', 0, 1000002),
	('2020-07-10', 'Sample type', '124412841294', 0, 1000003)
GO




print '' print '*** Creating Client Table'
GO
CREATE TABLE [dbo].[Client](
	[ClientID] 	[int] IDENTITY(1000000,1) 	NOT NULL,
	[FirstName] 	[nvarchar](50)				NOT NULL, 
	[LastName] 		[nvarchar](50)  			NOT NULL, 
	[PhoneNumber] 	[nvarchar](11)  			NOT NULL,
	[Email]			[nvarchar](250) 			NOT NULL,
	[DriversLicence][nvarchar](25) 				NOT NULL,
	CONSTRAINT [pk_ClientID] PRIMARY KEY([ClientID] ASC),
	CONSTRAINT [ak_EmailClient] UNIQUE([Email] ASC),
	CONSTRAINT [ak_DriversLicence] UNIQUE([DriversLicence] ASC)
)
GO

print '' print '*** Creating Sample Client Records'
GO
INSERT INTO [dbo].[Client]
	([FirstName], [LastName], [PhoneNumber], [Email], [DriversLicence])
	VALUES
	('Carl', 'Marx', '9879879876', 'cmarx@communism.net', 'Aezhju'),
	('Jotaro', 'Kujo', '2342342345', 'jstar2@mail.com', 'vbgbgdgf'),
	('Gyro', 'Zeppeli', '1122233445', 'bubbleguy@mail.com', 'ksadfdfzdfz'),
	('Rob', 'Robson', '8877654321', 'rob@rob.com', 'ikjhmbgfvzb')
GO

print '' print '*** Creating sp_insert_client'
GO
CREATE PROCEDURE [sp_insert_client]
(
	@FirstName		[nvarchar](250),
	@LastName		[nvarchar](100),
	@PhoneNumber	[nvarchar](11),
	@Email			[nvarchar](250),
    @DriversLicence [nvarchar](25)
)
AS
BEGIN
	INSERT INTO [dbo].[User]
		([FirstName], [LastName], [PhoneNumber], [Email], [DriversLicence])
	VALUES
		(@FirstName, @LastName, @PhoneNumber, @Email, @DriversLicence)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Adding index for last name in Client Table'
GO
CREATE NONCLUSTERED INDEX [ix_ClientLastName] ON [Client]([LastName] ASC)
GO

print '' print '*** Adding index for email in Client Table'
GO
CREATE NONCLUSTERED INDEX [ix_ClientEmail] ON [Client]([Email] ASC)
GO

print '' print '*** Creating PaymentMethod Table'
GO
CREATE TABLE [dbo].[PaymentMethod](
	[PaymentMethodID][int] IDENTITY(1000000,1) 	NOT NULL,
    [Type]           [nvarchar](25)             NOT NULL,
	[ExpirationDate] [date] 					    NULL,
	[CardNumber] 	 [nvarchar](25)				NOT NULL,
	[ClientID] 	     [int] 						NOT NULL,
	CONSTRAINT [pk_PaymentMethodID] PRIMARY KEY([PaymentMethodID] ASC),
	CONSTRAINT [fk_PaymentMethod_ClientID] FOREIGN KEY([ClientID])
		REFERENCES [Client]([ClientID]),
	CONSTRAINT [ak_CardNumber] UNIQUE([CardNumber] ASC)
)
GO

print '' print '*** Creating sp_insert_PaymentMethod'
GO
CREATE PROCEDURE [sp_insert_payment_method]
(
	@ExpirationDate	[date],
    @Type           [nvarchar](25),
	@CardNumber		[nvarchar](25),
	@ClientID    	[int]
)
AS
BEGIN
	INSERT INTO [dbo].[PaymentMethod]
		([ExpirationDate], [Type], [CardNumber], [ClientID])
	VALUES
		(@ExpirationDate, @Type, @CardNumber, @ClientID)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating Sample PaymentMethod Records'
GO
INSERT INTO [dbo].[PaymentMethod]
	([ExpirationDate], [CardNumber], [ClientID])
	VALUES
	('2020-01-20', 'Sample type', '123412341434', 1000000),
	('2020-04-25', 'Sample type', '121412541264', 1000001),
	('2020-03-15', 'Sample type', '122412741234', 1000002),
	('2020-07-10', 'Sample type', '124412841294', 1000003)
GO



print '' print '*** Creating SalesTerms Table'
GO
CREATE TABLE [dbo].[SalesTerms](
	[SalesTermsID]	[int] IDENTITY(1000000,1) 	NOT NULL,
	[Price]	        [money] 					NOT NULL,
	[WorkHours]	    [float]						NOT NULL,
	[Description]	[nvarchar](4000)			NOT	NULL,
	CONSTRAINT [pk_SalesTermsID] PRIMARY KEY([SalesTermsID] ASC)
)
GO

print '' print '*** Creating sp_insert_sales_terms'
GO
CREATE PROCEDURE [sp_insert_sales_terms]
(
	@PricePerHour	[money],
	@LateCharge		[money],
	@Description   	[nvarchar](4000)
)
AS
BEGIN
	INSERT INTO [dbo].[OrderTerms]
		([PricePerHour], [LateCharge], [Description])
	VALUES
		(@PricePerHour, @LateCharge, @Description)
	RETURN SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating Sample OrderTerms Records'
GO
INSERT INTO [dbo].[OrderTerms]
	([PricePerHour], [LateCharge], [Description])
	VALUES
	(100.00, 100.00, 'This is a sample description 0'),
	(101.00, 101.00, 'This is a sample description 1'),
	(102.00, 102.00, 'This is a sample description 2'),
	(103.00, 103.00, 'This is a sample description 3')
GO







	
	

print '' print '*** Creating Order Table'
GO
CREATE TABLE [dbo].[Order](
	[OrderID]	[int] IDENTITY(1000000,1)	NOT NULL,
	[ClientID][int]						NOT NULL,
	[UserID][int]						NOT NULL,
	[PaymentMethodID] [int]							NULL,
	[OrderDate][date]						NOT NULL,
	[Discount]	[decimal]					NOT NULL,
	[Total]		[money]						NOT NULL,
	CONSTRAINT [pk_OrderID] PRIMARY KEY([OrderID] ASC),
	CONSTRAINT [fk_Order_ClientID] FOREIGN KEY([ClientID])
		REFERENCES [Client]([ClientID]),
	CONSTRAINT [fk_Order_UserID] FOREIGN KEY([UserID])
		REFERENCES [User]([UserID]),
	CONSTRAINT [fk_Order_PaymentMethodID] FOREIGN KEY([PaymentMethodID])
		REFERENCES [PaymentMethod]([PaymentMethodID])
)
GO

print '' print '*** Creating Sample Order Records'
GO
INSERT INTO [dbo].[Order]
	([ClientID], [UserID], [PaymentMethodID], [OrderDate], [Discount], [Total])
	VALUES
	(1000000, 1000000, 1000000, '2015-12-17', 0.10, 500),
    (1000001, 1000001, 1000001, '2015-12-18', 0.10, 400),
    (1000002, 1000002, 1000002, '2015-12-18', 0.10, 300),
    (1000003, 1000003, 1000003, '2015-12-19', 0.10, 600),
    (1000000, 1000000, 1000000, '2015-12-20', 0.10, 500),
    (1000001, 1000001, 1000001, '2015-12-20', 0.10, 500)
GO	

print '' print '*** Creating OrderLine Table'
GO
CREATE TABLE [dbo].[OrderLine](
	[OrderID] 		[int]                       NOT NULL,
	[BicycleID] 	[nvarchar](20) 				NOT NULL,
	[Amount]		[money]						NOT NULL,
	[Quanity]		[decimal]					NOT NULL,
	CONSTRAINT [pk_OrderLine_OrderID] PRIMARY KEY([OrderID] ASC),
	CONSTRAINT [fk_OrderLine_BicycleID] FOREIGN KEY([BicycleID])
		REFERENCES [Bicycle]([BicycleID]),
	CONSTRAINT [fk_OrderLine_OrderID] FOREIGN KEY([OrderID])
		REFERENCES [Order]([OrderID])
)
GO

print '' print '*** Creating Sample OrderLine Records'
GO
INSERT INTO [dbo].[OrderLine]
	([OrderID], [BicycleID], [Amount], [Quanity], [UserIDOut], [UserIDIn], [CheckOutTime], [CheckInTime], [CheckInOkay], [CheckInNotes])
	VALUES
	(1000000, '100-AX-2B', 555.56, 1, 1000000, 1000000, '20160618 10:04:09 AM', '20160618 12:04:09 PM', 1, 'notes notes notes notes notes'),
    (1000001, '100-AX-3B', 444.44, 1, 1000001, 1000001, '20160618 10:14:09 AM', '20160618 12:14:09 PM', 1, 'notes notes notes notes notes'),
    (1000002, '100-AX-4B', 333.33, 1, 1000002, 1000002, '20160618 10:24:09 AM', '20160618 12:24:09 PM', 1, 'notes notes notes notes notes'),
    (1000003, '100-AX-5B', 666.66, 1, 1000003, 1000003, '20160618 10:34:09 AM', '20160618 12:34:09 PM', 1, 'notes notes notes notes notes'),
    (1000004, '100-AX-6B', 555.56, 1, 1000000, 1000000, '20160618 10:44:09 AM', '20160618 12:44:09 PM', 1, 'notes notes notes notes notes'),
    (1000005, '100-AX-2B', 555.56, 1, 1000001, 1000001, '20160618 10:54:09 AM', '20160618 12:54:09 PM', 1, 'notes notes notes notes notes')
GO

print '' print '*** Adding index for CheckInTime in the OrderLine table'
GO
CREATE NONCLUSTERED INDEX [ix_OrderLine_CheckInTime] ON [OrderLine]([CheckInTime] ASC)
GO

print '' print '*** Adding index for CheckOutTime in the OrderLine table'
GO
CREATE NONCLUSTERED INDEX [ix_OrderLine_CheckOutTime] ON [OrderLine]([CheckOutTime] ASC)
GO

