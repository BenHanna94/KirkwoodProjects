
/* Check if database already exists */

IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name= 'ArtAppDB')
BEGIN
	DROP DATABASE [ArtAppDB]
	print '' print '*** Dropping Database ArtAppDB'
END
GO

print '' print '*** Creating Database ArtAppDB'

GO

CREATE DATABASE [ArtAppDB]
GO

print '' print '*** Using Database ArtAppDB'
GO

USE [ArtAppDB]
GO

print '' print '*** Creating CompensatedStatus Table'
GO
CREATE TABLE [dbo].[CompensatedStatus](
	[CompensatedStatusID]		[nvarchar](50) NOT NULL,
	[Description] 	            [nvarchar](250)	   NULL,
	CONSTRAINT [pk_CompensatedStatusID] PRIMARY KEY([CompensatedStatusID] ASC)
)
GO

print '' print '*** Creating Sample CompensatedStatus Records'
GO
INSERT INTO [dbo].[CompensatedStatus]
	([CompensatedStatusID])
	VALUES
	('Not Paid'),
	('Partial'),
	('Full'),
	('Not Needed')
GO

print '' print '*** Creating sp_insert_compensated_status'
GO
CREATE PROCEDURE [sp_insert_compensated_status]
(
    @CompensatedStatusID     [nvarchar](50),
	@Description		     [nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[CompensatedStatus]
		([CompensatedStatusID], [Description])
	VALUES
		(@CompensatedStatusID, @Description)
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_select_all_compensated_status'
GO
CREATE PROCEDURE [sp_select_all_compensated_status]
AS
BEGIN
	SELECT [CompensatedStatusID]
	FROM [dbo].[CompensatedStatus]
    ORDER BY [CompensatedStatusID]
END 
GO

print '' print '*** Creating User Table'
GO
CREATE TABLE [dbo].[User](
	[UserID] 	    [int] IDENTITY(1000000,1) 	NOT NULL,
	[Name] 	        [nvarchar](100)				NOT NULL,
	[PhoneNumber] 	[nvarchar](11)  			    NULL,
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
	@Name		    [nvarchar](100),
	@PhoneNumber	[nvarchar](11),
	@Email			[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[User]
		([Name], [PhoneNumber], [Email])
	VALUES
		(@Name, @PhoneNumber, @Email)
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_insert_user_no_phone'
GO
CREATE PROCEDURE [sp_insert_user_no_phone]
(
	@Name		    [nvarchar](100),
	@Email			[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[User]
		([Name], [Email])
	VALUES
		(@Name, @Email)
	SELECT SCOPE_IDENTITY()
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

print '' print '*** Creating sp_select_user_by_id'
GO
CREATE PROCEDURE [sp_select_user_by_id]
(
	@UserID			[int]
)
AS
BEGIN
	SELECT [UserID], [Name], [PhoneNumber], [Email], [Active]
	FROM [dbo].[User]
    WHERE [UserID] = @UserID
END 
GO

print '' print '*** Creating sp_select_users_by_active'
GO
CREATE PROCEDURE [sp_select_users_by_active]
(
    @Active      [bit]
)
AS
BEGIN
   SELECT [UserID], [Name], [PhoneNumber], [Email], [Active]
   FROM [dbo].[User]
   WHERE [Active] = @Active
   ORDER BY [Name]
END
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

print '' print '*** Creating sp_update_user'
GO
CREATE PROCEDURE [sp_update_user]
(
    @UserID			 [int],
    
    @NewName	    [nvarchar](100),
	@NewPhoneNumber	[nvarchar](11),
	@NewEmail		[nvarchar](250),
    
	@OldName	    [nvarchar](100),
	@OldPhoneNumber	[nvarchar](11),
	@OldEmail		[nvarchar](250)
)
AS
BEGIN
	UPDATE [dbo].[User]
    SET [Name]          = @NewName, 
        [PhoneNumber]   = @NewPhoneNumber,  
        [Email]         = @NewEmail
    WHERE   [UserID]    = @UserID
    AND     [Name]     = @OldName
    AND     [PhoneNumber]   = @OldPhoneNumber  
    AND     [Email]         = @OldEmail
    
    RETURN @@ROWCOUNT
END 
GO

print '' print '*** Creating sp_deactivate_user'
GO
CREATE PROCEDURE [sp_deactivate_user]
(
    @UserID			 [int]
)
AS
BEGIN
	UPDATE [dbo].[User]
    SET [Active] = 0
    WHERE [UserID] = @UserID
    
    RETURN @@ROWCOUNT
END 
GO

print '' print '*** Creating sp_reactivate_user'
GO
CREATE PROCEDURE [sp_reactivate_user]
(
    @UserID			 [int]
)
AS
BEGIN
	UPDATE [dbo].[User]
    SET [Active] = 1
    WHERE [UserID] = @UserID
    
    RETURN @@ROWCOUNT
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

print '' print '*** Creating Sample User Records'
GO
INSERT INTO [dbo].[User]
	([Name], [PhoneNumber], [Email])
	VALUES
	('System Admin', '1231230000', 'admin@company.com'),
	('Joseph Joestar', '1231231234', 'jstar@mail.com'),
	('Caesar Zeppeli', '1112223344', 'bubbleguy@mail.com'),
	('Bob Bobson', '98770654321', 'bob@bob.com'),
	('Billy-Joe Armstrong', '7673431234', 'GreenDay@rock.com'),
	('Peter Parker', '9877654320', 'wallcrawler@notaspider.com'),
    ('Joanne Smith', '3191231111', 'joanne@company.com')
GO

print '' print '*** Creating Sample Deactivated User Records'
GO
INSERT INTO [dbo].[User]
	([Name], [PhoneNumber], [Email], [Active])
	VALUES
	('Stew Beef', '4344344343', 'meat@meat.com', 0),
	('Ben Hanna', '1113334455', 'ech@myech.com', 0)
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
    @UserID             [int],
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
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating Skill Table'
GO
CREATE TABLE [dbo].[Skill](
	[SkillID]		[nvarchar](50) NOT NULL,
	[Description] 	[nvarchar](250)	   NULL,
	CONSTRAINT [pk_SkillID] PRIMARY KEY([SkillID] ASC)
)
GO

print '' print '*** Creating sp_insert_skill'
GO
CREATE PROCEDURE [sp_insert_skill]
(
    @SkillID            [nvarchar](50),
	@Description		[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[Skill]
		([SkillID], [Description])
	VALUES
		(@SkillID, @Description)
	SELECT SCOPE_IDENTITY()
END 
GO


print '' print '*** Creating UserRole Table'
GO
CREATE TABLE [dbo].[UserRole](
	[UserID] 	[int] 			NOT NULL,
	[RoleID]		[nvarchar](50) 	NOT NULL,
	CONSTRAINT [pk_UserID_RoleID] PRIMARY KEY([UserID] ASC, [RoleId] ASC),
	CONSTRAINT [fk_Role_UserID] FOREIGN KEY([UserID])
		REFERENCES [User]([UserID]) ON UPDATE CASCADE,
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
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_delete_user_role'
GO
CREATE PROCEDURE [sp_delete_user_role]
(
    @UserID             [int],
	@RoleID             [nvarchar](50)
)
AS
BEGIN
	DELETE FROM [dbo].[UserRole]
	WHERE   [UserID] = @UserID
    AND     [RoleID] = @RoleID
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating UserSkill Table'
GO
CREATE TABLE [dbo].[UserSkill](
	[UserID] 	[int] 			NOT NULL,
	[SkillID]		[nvarchar](50) 	NOT NULL,
	CONSTRAINT [pk_UserID_SkillID] PRIMARY KEY([UserID] ASC, [SkillID] ASC),
	CONSTRAINT [fk_Skill_UserID] FOREIGN KEY([UserID])
		REFERENCES [User]([UserID]) ON UPDATE CASCADE,
	CONSTRAINT [fk_Skill_SkillID] FOREIGN KEY([SkillID])
		REFERENCES [Skill]([SkillID]) ON UPDATE CASCADE
	
)
GO

print '' print '*** Creating sp_insert_user_skill'
GO
CREATE PROCEDURE [sp_insert_user_skill]
(
    @UserID         [int],
	@SkillID        [nvarchar](50)
)
AS
BEGIN
	INSERT INTO [dbo].[UserSkill]
		([UserID], [SkillID])
	VALUES
		(@UserID, @SkillID)
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_delete_user_skill'
GO
CREATE PROCEDURE [sp_delete_user_skill]
(
    @UserID             [int],
	@SkillID             [nvarchar](50)
)
AS
BEGIN
	DELETE FROM [dbo].[UserSkill]
	WHERE   [UserID] = @UserID
    AND     [SkillID] = @SkillID
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating Sample Role Records'
GO
INSERT INTO [dbo].[Role]
	([RoleID])
	VALUES
	('Admin'),
	('Manager'),
	('Customer Service'),
	('Contributor')
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

print '' print '*** Creating Sample UserRole Records'
GO
INSERT INTO [dbo].[UserRole]
	([UserID], [RoleID])
	VALUES
	(1000000, 'Admin'),
    (1000001, 'Contributor'),
	(1000002, 'Contributor'),
    (1000002, 'Customer Service'),
	(1000003, 'Customer Service'),
	(1000003, 'Manager'),
    (1000004, 'Manager'),
    (1000005, 'Customer Service'),
    (1000006, 'Admin'),
    (1000006, 'Manager'),
    (1000006, 'Customer Service'),
    (1000006, 'Contributor')
GO

print '' print '*** Creating sp_select_roles_by_userID'
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

print '' print '*** Creating sp_select_users_by_roleID'
GO
CREATE PROCEDURE [sp_select_users_by_roleID]
(
	@RoleID	 [nvarchar](50)
)
AS
BEGIN
	SELECT [UserID]
	FROM [dbo].[UserRole]
    WHERE [RoleID] = @RoleID
END 
GO

print '' print '*** Creating sp_select_skills_by_userID'
GO
CREATE PROCEDURE [sp_select_skills_by_userID]
(
	@UserID	 [int]
)
AS
BEGIN
	SELECT [SkillID]
	FROM [dbo].[UserSkill]
    WHERE [UserID] = @UserID
END 
GO

print '' print '*** Creating sp_select_all_roles'
GO
CREATE PROCEDURE [sp_select_all_roles]
AS
BEGIN
	SELECT [RoleID]
	FROM [dbo].[Role]
    ORDER BY [RoleID]
END 
GO

print '' print '*** Creating sp_select_all_skills'
GO
CREATE PROCEDURE [sp_select_all_skills]
AS
BEGIN
	SELECT [SkillID]
	FROM [dbo].[Skill]
    ORDER BY [SkillID]
END 
GO



print '' print '*** Creating Client Table'
GO
CREATE TABLE [dbo].[Client](
	[ClientID] 	[int] IDENTITY(1000000,1) 	    NOT NULL,
	[Name] 	        [nvarchar](50)				NOT NULL,
	[Contact]		[nvarchar](250) 			NOT NULL,
	[Notes]         [nvarchar](250) 				NULL
	CONSTRAINT [pk_ClientID] PRIMARY KEY([ClientID] ASC)
)
GO

print '' print '*** Creating Sample Client Records'
GO
INSERT INTO [dbo].[Client]
	([Name], [Contact], [Notes])
	VALUES
    ('No Client', 'None', 'Used for when work is self determined; When you are your only client'),
	('Carl Marx', 'Twitter', 'Aezhju'),
	('Jotaro Kujo', 'Reddit', 'vbgbgdgf'),
	('bubbleguy@mail.com', 'email', 'ksadfdfzdfz'),
	('Rob Robson', 'in Person', '800-phn-numb')
GO

print '' print '*** Creating sp_insert_client'
GO
CREATE PROCEDURE [sp_insert_client]
(
	@Name		    [nvarchar](50),
	@Contact		[nvarchar](250),
    @Notes          [nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[Client]
		([Name], [Contact], [Notes])
	VALUES
		(@Name, @Contact, @Notes)
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_select_all_clients'
GO
CREATE PROCEDURE [sp_select_all_clients]

AS
BEGIN
	SELECT [ClientID], [Name], [Contact], [Notes]
	FROM [dbo].[Client]
END
GO

print '' print '*** Creating sp_select_client_by_id'
GO
CREATE PROCEDURE [sp_select_client_by_id]
(
	@ClientID			[int]
)
AS
BEGIN
	SELECT [ClientID], [Name], [Contact], [Notes]
	FROM [dbo].[Client]
    WHERE [ClientID] = @ClientID
END 
GO

print '' print '*** Creating sp_update_client'
GO
CREATE PROCEDURE [sp_update_client]
(
    @ClientID			 [int],
    
    @NewName	    [nvarchar](100),
	@NewContact    	[nvarchar](250),
	@NewNotes		[nvarchar](250),
    
	@OldName	    [nvarchar](100),
	@OldContact	   [nvarchar](250),
	@OldNotes		[nvarchar](250)
)
AS
BEGIN
	UPDATE [dbo].[Client]
    SET [Name]          = @NewName, 
        [Contact]       = @NewContact,  
        [Notes]         = @NewNotes
    WHERE   [ClientID]  = @ClientID
    AND     [Name]      = @OldName
    AND     [Contact]   = @OldContact  
    AND     [Notes]     = @OldNotes
    
    RETURN @@ROWCOUNT
END 
GO

print '' print '*** Creating Reference Table'
GO
CREATE TABLE [dbo].[Reference](
    [ReferenceID] 	[int] IDENTITY(1000000,1)	NOT	NULL,
	[ReferenceName] [nvarchar](50)			    NOT NULL,
	[ClientID] 	    [int]				            NULL,
	[Description]	[nvarchar](250) 			    NULL,
	[FileLocation]  [nvarchar](250) 			NOT NULL
	CONSTRAINT [pk_ReferenceID] PRIMARY KEY([ReferenceID] ASC),
    CONSTRAINT [ak_ReferenceName] UNIQUE([ReferenceName] ASC),
    CONSTRAINT [fk_Reference_ClientID] FOREIGN KEY([ClientID]) 
        REFERENCES [Client]([ClientID])
)
GO

print '' print '*** Creating Sample Reference Records'
GO
INSERT INTO [dbo].[Reference]
	([ReferenceName], [ClientID], [Description], [FileLocation])
	VALUES
	('Communism Panda', 1000001, 'For Carl marx', 'panda.jpg'),
	('Buff Fish', 1000002, 'Jotaro', 'fish.jpg'),
	('Soap Bubble', 1000003, 'bubbleguy@mail.com', 'bubble.jpeg'),
	('Robbie Rotten', 1000004, 'Literally just Robbie Rotten', 'robbie.jpg')
GO


print '' print '*** Creating sp_insert_reference'
GO
CREATE PROCEDURE [sp_insert_reference]
(
	@ReferenceName	[nvarchar](50),
    @ClientID       [int],
	@Description	[nvarchar](250),
	@FileLocation	[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[Reference]
		([ReferenceName], [ClientID], [Description], [FileLocation])
	VALUES
		(@ReferenceName, @ClientID, @Description, @FileLocation)
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_select_references_by_name'
GO
CREATE PROCEDURE [sp_select_references_by_name]
(
	@ReferenceName			nvarchar(50)
)
AS
BEGIN
	SELECT [ReferenceID], [ClientID], [Description], [FileLocation]
	FROM [dbo].[Reference]
    WHERE [ReferenceName] = @ReferenceName
END 
GO

print '' print '*** Creating sp_select_references_by_id'
GO
CREATE PROCEDURE [sp_select_references_by_id]
(
	@ReferenceID			int
)
AS
BEGIN
	SELECT [ReferenceName], [ClientID], [Description], [FileLocation]
	FROM [dbo].[Reference]
    WHERE [ReferenceID] = @ReferenceID
END 
GO

print '' print '*** Creating sp_select_references_by_clientid'
GO
CREATE PROCEDURE [sp_select_references_by_clientid]
(
	@ClientID			int
)
AS
BEGIN
	SELECT [ReferenceID], [ReferenceName], [Description], [FileLocation]
	FROM [dbo].[Reference]
    WHERE [ClientID] = @ClientID
END 
GO

print '' print '*** Creating sp_select_references_by_pieceid'
GO
CREATE PROCEDURE [sp_select_references_by_pieceid]
(
	@PieceID			int
)
AS
BEGIN
	SELECT [Reference].[ReferenceID], [ReferenceName], [ClientID], [Description], [FileLocation]
	FROM [dbo].[Reference] JOIN [dbo].[PieceReference] 
    ON [Reference].[ReferenceID] = [PieceReference].[ReferenceID]
    WHERE [PieceReference].[PieceID] = @PieceID
END
GO

print '' print '*** Creating sp_select_all_references'
GO
CREATE PROCEDURE [sp_select_all_references]
AS
BEGIN
	SELECT [ReferenceID], [ReferenceName], [ClientID], [Description], [FileLocation]
	FROM [dbo].[Reference]
    ORDER BY [ReferenceName]
END 
GO


print '' print '*** Creating sp_update_reference'
GO
CREATE PROCEDURE [sp_update_reference]
(
    @ReferenceID			 [int],
    
    @NewReferenceName   [nvarchar](50),
    @NewClientID	    [int],
	@NewDescription	    [nvarchar](100),
	@NewFileLocation	[nvarchar](250),
    
    @OldReferenceName   [nvarchar](50),
	@OldClientID	    [int],
	@OldDescription	    [nvarchar](100),
	@OldFileLocation	[nvarchar](250)
)
AS
BEGIN
	UPDATE [dbo].[Reference]
    SET [ReferenceName]  = @NewReferenceName,
        [ClientID]       = @NewClientID, 
        [Description]    = @NewDescription, 
        [FileLocation]   = @NewFileLocation
        
    WHERE   [ReferenceID]    = @ReferenceID
    AND     [ReferenceName]  = @OldReferenceName 
    AND     [ClientID]       = @OldClientID 
    AND     [Description]    = @OldDescription
    AND     [FileLocation]   = @OldFileLocation
    
    RETURN @@ROWCOUNT
END 
GO

print '' print '*** Creating sp_delete_reference'
GO
CREATE PROCEDURE [sp_delete_reference]
(
    @ReferenceID         [int]
)
AS
BEGIN
	DELETE FROM [dbo].[Reference]
	WHERE   [ReferenceID] = @ReferenceID
	SELECT SCOPE_IDENTITY()
END 
GO



print '' print '*** Creating sp_select_client_by_referenceid'
GO
CREATE PROCEDURE [sp_select_client_by_referenceid]
(
	@ReferenceID			[nvarchar](50)
)
AS
BEGIN
	SELECT [Client].[ClientID], [Name], [Contact], [Notes]
	FROM [dbo].[Client] JOIN [dbo].[Reference] ON [Client].ClientID = Reference.ClientID
    WHERE [ReferenceID] = @ReferenceID
END 
GO

print '' print '*** Creating Project Table'
GO
CREATE TABLE [dbo].[Project](
	[ProjectID] 	[int] IDENTITY(1000000,1) 	NOT NULL,
	[Name] 	        [nvarchar](100)				NOT NULL,
	[Type] 	        [nvarchar](50)  			NOT NULL,
	[Description]   [nvarchar](250) 			    NULL,
    [Complete]      [bit]                       NOT NULL DEFAULT 0,
	CONSTRAINT [pk_ProjectID] PRIMARY KEY([ProjectID] ASC),
    CONSTRAINT [ak_ProjectName] UNIQUE([Name] ASC)
)
GO

print '' print '*** Creating Sample Project Records'
GO
INSERT INTO [dbo].[Project]
	([Name], [Type], [Description], [Complete])
	VALUES
	('Commission for Carl Marx', 'Illustration', 
     'A Communist panda lounges on the beach, a copy of Das Kapital is lying beside him', 0),
	('Fantasy Comic', 'Multi-page Comic', 'A 3 page comic collaberation', 0),
    ('Mecha Illustration', 'Single illustration', 'To be used as an example of a completed project', 1)
GO

print '' print '*** Creating Piece Table'
GO
CREATE TABLE [dbo].[Piece](
	[PieceID] 	            [int] IDENTITY(1000000,1) 	NOT NULL,
	[ProjectID] 	        [int]                      	NOT NULL,
    [UserID]                [int]                           NULL,
	[Description]           [nvarchar](250) 			NOT NULL,
    [Stage]                 [nvarchar](50)              NOT NULL,
    [Complete]              [bit]                       NOT NULL DEFAULT 0,
    [Compensation]          [money]                     NOT NULL DEFAULT 0,
    [CompensatedStatusID]   [nvarchar](50)      NOT NULL DEFAULT 'Not Paid',
    
	CONSTRAINT [pk_PieceID] PRIMARY KEY([PieceID] ASC),
    CONSTRAINT [fk_Piece_ProjectID] FOREIGN KEY([ProjectID]) 
        REFERENCES [Project]([ProjectID]),
    CONSTRAINT [fk_Piece_UserID] FOREIGN KEY([UserID]) 
        REFERENCES [User]([UserID]),
    CONSTRAINT [fk_Piece_Compensated] FOREIGN KEY([CompensatedStatusID]) 
        REFERENCES [CompensatedStatus]([CompensatedStatusID]) ON UPDATE CASCADE
)
GO

print '' print '*** Creating Sample Piece Records'
GO
INSERT INTO [dbo].[Piece]
	([ProjectID], [UserID], [Description], [Stage], [Complete], [Compensation], [CompensatedStatusID])
	VALUES
	(1000000, 1000001, 'Sketch for Carl Marx commission', 'Complete', 1, 20.00, 'Full'),
    (1000000, 1000001, 'Lineart inks for Carl Marx commission', 'Incomplete', 0, 20.00, 'Partial'),
    (1000000, 1000001, 'Colors for Carl Marx commission', 'Not Started', 0, 20.00, 'Not Paid'),
    (1000001, 1000002, 'Script for Fantasy Comic', 'Complete', 1, 0.00, 'Not Needed'),    
    (1000001, 1000003, 'Fantasy Comic page 1', 'Complete', 1, 100.00, 'Full'),
    (1000001, 1000003, 'Fantasy Comic page 2', 'Sketch finished, Colors started', 0, 100.00, 'Partial'),
    (1000001, 1000003, 'Fantasy Comic page 3', 'Sketch finished', 0, 100.00, 'Not Paid')
GO

print '' print '*** Creating sp_insert_piece'
GO
CREATE PROCEDURE [sp_insert_piece]
(
    @ProjectID 	    [int],          
    @UserID         [int],          
	@Description    [nvarchar](250),
    @Stage          [nvarchar](50) 
)
AS
BEGIN
   INSERT INTO [dbo].[Piece]
	   ([ProjectID], [UserID], [Description], [Stage])
   VALUES 
        (@ProjectID, @UserID, @Description, @Stage)
   SELECT SCOPE_IDENTITY()
END
GO

print '' print '*** Creating sp_update_piece'
GO
CREATE PROCEDURE [sp_update_piece]
(
	@PieceID		   [int],
          
    @OldUserID         [int],          
	@OldDescription    [nvarchar](250),
    @OldStage          [nvarchar](50), 
    @OldCompensation   [money],
    @OldCompensatedStatusID   [nvarchar](50),
    
    @NewUserID         [int],          
	@NewDescription    [nvarchar](250),
    @NewStage          [nvarchar](50),
    @NewCompensation   [money],
    @NewCompensatedStatusID   [nvarchar](50)  
)
AS
BEGIN
	UPDATE [dbo].[Piece]
		SET [UserID]      = @NewUserID,     
            [Description] = @NewDescription,
            [Stage]       = @NewStage,
            [Compensation]= @NewCompensation, 
            [CompensatedStatusID] = @NewCompensatedStatusID 
            
	WHERE 	[PieceID]     = @PieceID
	  AND	[UserID]      = @OldUserID
	  AND	[Description] = @OldDescription	
	  AND	[Stage]       = @OldStage
      AND   [Compensation]= @OldCompensation 
      AND   [CompensatedStatusID] = @OldCompensatedStatusID
	 
	RETURN @@ROWCOUNT
END
GO


print '' print '*** Creating PieceReference Table'
GO
CREATE TABLE [dbo].[PieceReference](
	[ReferenceID] 	[int] 	             NOT NULL,
	[PieceID]		[int] 	             NOT NULL,
	CONSTRAINT [pk_ReferenceID_PieceID] PRIMARY KEY([ReferenceID] ASC, [PieceID] ASC),
	CONSTRAINT [fk_Reference_ReferenceID] FOREIGN KEY([ReferenceID])
		REFERENCES [Reference]([ReferenceID]) ON UPDATE CASCADE,
	CONSTRAINT [fk_Reference_PieceID] FOREIGN KEY([PieceID])
		REFERENCES [Piece]([PieceID]) ON UPDATE CASCADE
)
GO

print '' print '*** Creating Sample PieceReference Records'
GO
INSERT INTO [dbo].[PieceReference]
	([ReferenceID], [PieceID])
	VALUES
	(1000000, 1000000),
	(1000000, 1000001),
	(1000000, 1000002)
GO

print '' print '*** Creating sp_insert_piece_reference'
GO
CREATE PROCEDURE [sp_insert_piece_reference]
(
    @ReferenceID         [int],
	@PieceID             [int]
)
AS
BEGIN
	INSERT INTO [dbo].[PieceReference]
		([ReferenceID], [PieceID])
	VALUES
		(@ReferenceID, @PieceID)
	SELECT SCOPE_IDENTITY()
END 
GO

print '' print '*** Creating sp_delete_piece_reference'
GO
CREATE PROCEDURE [sp_delete_piece_reference]
(
    @ReferenceID         [int],
	@PieceID             [int]
)
AS
BEGIN
	DELETE FROM [dbo].[PieceReference]
	WHERE   [ReferenceID] = @ReferenceID
    AND     [PieceID] = @PieceID
	SELECT SCOPE_IDENTITY()
END 
GO




print '' print '*** Creating sp_select_projects_by_complete'
GO
CREATE PROCEDURE [sp_select_projects_by_complete]
(
    @Complete      [bit]
)
AS
BEGIN
   SELECT [ProjectID], [Name], [Type], [Description]  
   FROM [dbo].[Project]
   WHERE [Complete] = @Complete
   ORDER BY [Name]
END
GO

print '' print '*** Creating sp_select_project_by_projectid'
GO
CREATE PROCEDURE [sp_select_project_by_projectid]
(
    @ProjectID      [int]
)
AS
BEGIN
   SELECT [Name], [Type], [Description], [Complete]  
   FROM [dbo].[Project]
   WHERE [ProjectID] = @ProjectID
END
GO

print '' print '*** Creating sp_insert_project'
GO
CREATE PROCEDURE [sp_insert_project]
(
    @Name 	        [nvarchar](100),
	@Type 	        [nvarchar](50),
	@Description    [nvarchar](250)        
)
AS
BEGIN
   INSERT INTO [dbo].[Project] 
        ([Name], [Type], [Description])
   VALUES 
        (@Name, @Type, @Description)
   SELECT SCOPE_IDENTITY()
END
GO

print '' print '*** Creating sp_update_project'
GO
CREATE PROCEDURE [sp_update_project]
(
	@ProjectID			[int],

	@OldName 	     [nvarchar](100),
	@OldType 	     [nvarchar](50),
	@OldDescription  [nvarchar](250),
	
	@NewName 	     [nvarchar](100),
	@NewType 	     [nvarchar](50),
	@NewDescription  [nvarchar](250)
)
AS
BEGIN
	UPDATE [dbo].[Project]
		SET [Name] = @NewName,
			[Type] = @NewType,	
			[Description] = @NewDescription
            
	WHERE 	[ProjectID]   = @ProjectID  
	  AND	[Name]        = @OldName
	  AND	[Type]        = @OldType	
	  AND	[Description] = @OldDescription
	 
	RETURN @@ROWCOUNT
END
GO

print '' print '*** Creating sp_complete_project'
GO
CREATE PROCEDURE [sp_complete_project]
(
	@ProjectID		 [int]
)
AS
BEGIN
	UPDATE [dbo].[Project]
		SET [Complete] = 1
            
	WHERE 	[ProjectID] = @ProjectID
	 
	RETURN @@ROWCOUNT
END
GO

print '' print '*** Creating sp_decomplete_project'
GO
CREATE PROCEDURE [sp_decomplete_project]
(
	@ProjectID		 [int]
)
AS
BEGIN
	UPDATE [dbo].[Project]
		SET [Complete] = 0
            
	WHERE 	[ProjectID] = @ProjectID
	 
	RETURN @@ROWCOUNT
END
GO

print '' print '*** Creating sp_select_pieces_by_projectid'
GO
CREATE PROCEDURE [sp_select_pieces_by_projectid]
(
    @ProjectID      [int],
    @Complete       [bit]
)
AS
BEGIN
   SELECT [PieceID], [UserID], [Description], [Stage], [Complete], [Compensation], [CompensatedStatusID]  
   FROM [dbo].[Piece]
   WHERE [ProjectID] = @ProjectID
   AND   [Complete] = @Complete
END
GO

print '' print '*** Creating sp_select_piece_by_id'
GO
CREATE PROCEDURE [sp_select_piece_by_id]
(
    @PieceID      [int]
)
AS
BEGIN
   SELECT [ProjectID], [UserID], [Description], [Stage], [Complete], [Compensation], [CompensatedStatusID]  
   FROM [dbo].[Piece]
   WHERE [PieceID] = @PieceID
END
GO

print '' print '*** Creating sp_complete_piece'
GO
CREATE PROCEDURE [sp_complete_piece]
(
	@PieceID		 [int]
)
AS
BEGIN
	UPDATE [dbo].[Piece]
		SET [Complete] = 1
            
	WHERE 	[PieceID] = @PieceID
	 
	RETURN @@ROWCOUNT
END
GO

print '' print '*** Creating sp_decomplete_piece'
GO
CREATE PROCEDURE [sp_decomplete_piece]
(
	@PieceID		 [int]
)
AS
BEGIN
	UPDATE [dbo].[Piece]
		SET [Complete] = 0
            
	WHERE 	[PieceID] = @PieceID
	 
	RETURN @@ROWCOUNT
END
GO