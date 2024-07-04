IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'flashy') IS NULL EXEC(N'CREATE SCHEMA [flashy];');
GO

CREATE TABLE [flashy].[Roles] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NOT NULL,
    [NormalizedName] varchar(256) NOT NULL,
    [ConcurrencyStamp] varchar(256) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [flashy].[Users] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(64) NOT NULL,
    [LastName] nvarchar(64) NOT NULL,
    [UserName] nvarchar(256) NOT NULL,
    [NormalizedUserName] nvarchar(256) NOT NULL,
    [Email] nvarchar(320) NOT NULL,
    [NormalizedEmail] nvarchar(320) NOT NULL,
    [EmailConfirmed] bit NOT NULL DEFAULT CAST(1 AS bit),
    [PasswordHash] nvarchar(256) NOT NULL,
    [SecurityStamp] nvarchar(256) NOT NULL,
    [ConcurrencyStamp] nvarchar(256) NOT NULL,
    [PhoneNumber] nvarchar(24) NULL,
    [PhoneNumberConfirmed] bit NOT NULL DEFAULT CAST(0 AS bit),
    [TwoFactorEnabled] bit NOT NULL DEFAULT CAST(0 AS bit),
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [AccessFailedCount] int NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [flashy].[RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] varchar(128) NOT NULL,
    [ClaimValue] varchar(128) NOT NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [flashy].[Roles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [flashy].[UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] varchar(128) NOT NULL,
    [ClaimValue] varchar(128) NOT NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [flashy].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [flashy].[UserLogins] (
    [LoginProvider] varchar(128) NOT NULL,
    [ProviderKey] varchar(128) NOT NULL,
    [NormalizedProviderDisplayName] varchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(128) NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [flashy].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [flashy].[UserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [flashy].[Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [flashy].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [flashy].[UserTokens] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [flashy].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_RoleClaims_RoleId] ON [flashy].[RoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [IX_Roles_Name] ON [flashy].[Roles] ([Name]);
GO

CREATE UNIQUE INDEX [IX_Roles_NormalizedName] ON [flashy].[Roles] ([NormalizedName]);
GO

CREATE INDEX [IX_UserClaims_UserId] ON [flashy].[UserClaims] ([UserId]);
GO

CREATE INDEX [IX_UserLogins_NormalizedProviderDisplayName] ON [flashy].[UserLogins] ([NormalizedProviderDisplayName]);
GO

CREATE INDEX [IX_UserLogins_ProviderDisplayName] ON [flashy].[UserLogins] ([ProviderDisplayName]);
GO

CREATE INDEX [IX_UserLogins_UserId] ON [flashy].[UserLogins] ([UserId]);
GO

CREATE INDEX [IX_UserRoles_RoleId] ON [flashy].[UserRoles] ([RoleId]);
GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [flashy].[Users] ([Email]);
GO

CREATE UNIQUE INDEX [IX_Users_NormalizedEmail] ON [flashy].[Users] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [IX_Users_NormalizedUserName] ON [flashy].[Users] ([NormalizedUserName]);
GO

CREATE UNIQUE INDEX [IX_Users_UserName] ON [flashy].[Users] ([UserName]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240703224454_Initial-Migration', N'8.0.6');
GO

COMMIT;
GO

