IF
OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
CREATE TABLE [__EFMigrationsHistory]
(
    [
    MigrationId]
    nvarchar
(
    150
) NOT NULL,
    [ProductVersion] nvarchar
(
    32
) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY
(
[
    MigrationId]
)
    );
END;
GO

BEGIN
TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
    IF
SCHEMA_ID(N'flashy') IS NULL EXEC(N'CREATE SCHEMA [flashy];');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[Roles]
(
    [
    Id]
    uniqueidentifier
    NOT
    NULL, [
    Name]
    nvarchar
(
    256
) NOT NULL,
    [NormalizedName] varchar
(
    256
) NOT NULL,
    [ConcurrencyStamp] varchar
(
    256
) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY
(
[
    Id]
)
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[Users]
(
    [
    Id]
    uniqueidentifier
    NOT
    NULL, [
    FirstName]
    nvarchar
(
    64
) NOT NULL,
    [LastName] nvarchar
(
    64
) NOT NULL,
    [UserName] nvarchar
(
    256
) NOT NULL,
    [NormalizedUserName] nvarchar
(
    256
) NOT NULL,
    [Email] nvarchar
(
    320
) NOT NULL,
    [NormalizedEmail] nvarchar
(
    320
) NOT NULL,
    [EmailConfirmed] bit NOT NULL DEFAULT CAST
(
    1 AS
    bit
),
    [PasswordHash] nvarchar
(
    256
) NOT NULL,
    [SecurityStamp] nvarchar
(
    256
) NOT NULL,
    [ConcurrencyStamp] nvarchar
(
    256
) NOT NULL,
    [PhoneNumber] nvarchar
(
    24
) NULL,
    [PhoneNumberConfirmed] bit NOT NULL DEFAULT CAST
(
    0 AS
    bit
),
    [TwoFactorEnabled] bit NOT NULL DEFAULT CAST
(
    0 AS
    bit
),
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL DEFAULT CAST
(
    1 AS
    bit
),
    [AccessFailedCount] int NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Users] PRIMARY KEY
(
[
    Id]
)
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[RoleClaims]
(
    [
    Id]
    int
    NOT
    NULL
    IDENTITY, [
    RoleId]
    uniqueidentifier
    NOT
    NULL, [
    ClaimType]
    varchar
(
    128
) NOT NULL,
    [ClaimValue] varchar
(
    128
) NOT NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY
(
[
    Id]
),
    CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY
(
[
    RoleId]
) REFERENCES [flashy].[Roles]
(
[
    Id]
) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[Decks]
(
    [
    Id]
    uniqueidentifier
    NOT
    NULL, [
    Name]
    nvarchar
(
    64
) NOT NULL,
    [NormalizedName] nvarchar
(
    64
) NOT NULL,
    [Description] nvarchar
(
    128
) NULL,
    [CreatedById] uniqueidentifier NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Decks] PRIMARY KEY
(
[
    Id]
),
    CONSTRAINT [FK_Decks_CreatedBy] FOREIGN KEY
(
[
    CreatedById]
) REFERENCES [flashy].[Users]
(
[
    Id]
) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[FlashCards]
(
    [
    Id]
    uniqueidentifier
    NOT
    NULL, [
    Front]
    nvarchar
(
    64
) NOT NULL,
    [NormalizedFront] nvarchar
(
    64
) NOT NULL,
    [Back] nvarchar
(
    64
) NOT NULL,
    [NormalizedBack] nvarchar
(
    64
) NOT NULL,
    [Hint] nvarchar
(
    128
) NULL,
    [CreatedById] uniqueidentifier NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_FlashCards] PRIMARY KEY
(
[
    Id]
),
    CONSTRAINT [FK_FlashCards_CreatedBy] FOREIGN KEY
(
[
    CreatedById]
) REFERENCES [flashy].[Users]
(
[
    Id]
) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[UserClaims]
(
    [
    Id]
    int
    NOT
    NULL
    IDENTITY, [
    UserId]
    uniqueidentifier
    NOT
    NULL, [
    ClaimType]
    varchar
(
    128
) NOT NULL,
    [ClaimValue] varchar
(
    128
) NOT NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY
(
[
    Id]
),
    CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY
(
[
    UserId]
) REFERENCES [flashy].[Users]
(
[
    Id]
) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[UserLogins]
(
    [
    LoginProvider]
    varchar
(
    128
) NOT NULL,
    [ProviderKey] varchar
(
    128
) NOT NULL,
    [NormalizedProviderDisplayName] varchar
(
    128
) NOT NULL,
    [ProviderDisplayName] nvarchar
(
    128
) NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY
(
    [
    LoginProvider],
[
    ProviderKey]
),
    CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY
(
[
    UserId]
) REFERENCES [flashy].[Users]
(
[
    Id]
) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[UserRoles]
(
    [
    UserId]
    uniqueidentifier
    NOT
    NULL, [
    RoleId]
    uniqueidentifier
    NOT
    NULL,
    CONSTRAINT [
    PK_UserRoles]
    PRIMARY
    KEY (
    [
    UserId],
[
    RoleId]
),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY
(
[
    RoleId]
) REFERENCES [flashy].[Roles]
(
[
    Id]
) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY
(
[
    UserId]
) REFERENCES [flashy].[Users]
(
[
    Id]
)
  ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[UserTokens]
(
    [
    UserId]
    uniqueidentifier
    NOT
    NULL, [
    LoginProvider]
    nvarchar
(
    128
) NOT NULL,
    [Name] nvarchar
(
    128
) NOT NULL,
    [Value] nvarchar
(
    256
) NOT NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY
(
    [
    UserId], [
    LoginProvider],
[
    Name]
),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY
(
[
    UserId]
) REFERENCES [flashy].[Users]
(
[
    Id]
) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[UsersDecks]
(
    [
    UserId]
    uniqueidentifier
    NOT
    NULL, [
    DeckId]
    uniqueidentifier
    NOT
    NULL,
    CONSTRAINT [
    PK_UsersDecks]
    PRIMARY
    KEY (
    [
    UserId],
[
    DeckId]
),
    CONSTRAINT [FK_UsersDecks_Decks] FOREIGN KEY
(
[
    DeckId]
) REFERENCES [flashy].[Decks]
(
[
    Id]
),
    CONSTRAINT [FK_UsersDecks_Users] FOREIGN KEY
(
[
    UserId]
) REFERENCES [flashy].[Users]
(
[
    Id]
)
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE TABLE [flashy].[DecksFlashCards]
(
    [
    DeckId]
    uniqueidentifier
    NOT
    NULL, [
    FlashCardId]
    uniqueidentifier
    NOT
    NULL, [
    FlashCardsId]
    uniqueidentifier
    NOT
    NULL,
    CONSTRAINT [
    PK_DecksFlashCards]
    PRIMARY
    KEY (
    [
    DeckId],
[
    FlashCardId]
),
    CONSTRAINT [FK_DecksFlashCards_Decks] FOREIGN KEY
(
[
    DeckId]
) REFERENCES [flashy].[Decks]
(
[
    Id]
),
    CONSTRAINT [FK_DecksFlashCards_FlashCards] FOREIGN KEY
(
[
    FlashCardId]
) REFERENCES [flashy].[FlashCards]
(
[
    Id]
),
    CONSTRAINT [FK_DecksFlashCards_FlashCards_FlashCardsId] FOREIGN KEY
(
[
    FlashCardsId]
) REFERENCES [flashy].[FlashCards]
(
[
    Id]
) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_Decks_CreatedAt] ON [flashy].[Decks] ([CreatedAt]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_Decks_CreatedById] ON [flashy].[Decks] ([CreatedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_Decks_NormalizedName] ON [flashy].[Decks] ([NormalizedName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_DecksFlashCards_FlashCardId] ON [flashy].[DecksFlashCards] ([FlashCardId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_DecksFlashCards_FlashCardsId] ON [flashy].[DecksFlashCards] ([FlashCardsId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_FlashCards_CreatedAt] ON [flashy].[FlashCards] ([CreatedAt]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_FlashCards_CreatedBy] ON [flashy].[FlashCards] ([CreatedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_FlashCards_NormalizedBack] ON [flashy].[FlashCards] ([NormalizedBack]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_FlashCards_NormalizedFront] ON [flashy].[FlashCards] ([NormalizedFront]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_RoleClaims_RoleId] ON [flashy].[RoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_Roles_Name] ON [flashy].[Roles] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_Roles_NormalizedName] ON [flashy].[Roles] ([NormalizedName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_UserClaims_UserId] ON [flashy].[UserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_UserLogins_NormalizedProviderDisplayName] ON [flashy].[UserLogins] ([NormalizedProviderDisplayName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_UserLogins_ProviderDisplayName] ON [flashy].[UserLogins] ([ProviderDisplayName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_UserLogins_UserId] ON [flashy].[UserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_UserRoles_RoleId] ON [flashy].[UserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_Users_Email] ON [flashy].[Users] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_Users_NormalizedEmail] ON [flashy].[Users] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_Users_NormalizedUserName] ON [flashy].[Users] ([NormalizedUserName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE UNIQUE INDEX [IX_Users_UserName] ON [flashy].[Users] ([UserName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
CREATE INDEX [IX_UsersDecks_DeckId] ON [flashy].[UsersDecks] ([DeckId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240209194144_InitialMigration'
)
BEGIN
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240209194144_InitialMigration', N'8.0.1');
END;
GO

COMMIT;
GO

