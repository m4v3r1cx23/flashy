-- Check if the database exists before creating it
IF NOT EXISTS (SELECT name
FROM sys.databases
WHERE name = 'FlashyIdentity')
BEGIN
  CREATE DATABASE FlashyIdentity;
END
GO

USE FlashyIdentity;
GO

-- Create FlashyIdentity login if it doesn't exist
IF NOT EXISTS (SELECT name
FROM sys.sql_logins
WHERE name = 'FlashyIdentity')
BEGIN
  CREATE LOGIN FlashyIdentity WITH PASSWORD = 'Fl@shyId3ntity#062024!';
END
GO

-- Create FlashyIdentity user if it doesn't exist
IF NOT EXISTS (SELECT name
FROM sys.database_principals
WHERE name = 'FlashyIdentity')
BEGIN
  CREATE USER FlashyIdentity FOR LOGIN FlashyIdentity;
END
GO

-- Add FlashyIdentity to db_owner role if not already a member
IF NOT EXISTS (SELECT *
FROM sys.database_role_members
WHERE role_principal_id = USER_ID('db_owner') AND member_principal_id = USER_ID('FlashyIdentity'))
BEGIN
  ALTER ROLE db_owner ADD MEMBER FlashyIdentity;
END
GO

-- Create FlashyAdmin login if it doesn't exist
IF NOT EXISTS (SELECT name
FROM sys.sql_logins
WHERE name = 'FlashyAdmin')
BEGIN
  CREATE LOGIN FlashyAdmin WITH PASSWORD = 'Fl@shyAdmin#062024!';
END
GO

-- Create FlashyAdmin user if it doesn't exist
IF NOT EXISTS (SELECT name
FROM sys.database_principals
WHERE name = 'FlashyAdmin')
BEGIN
  CREATE USER FlashyAdmin FOR LOGIN FlashyAdmin;
END
GO

-- Grant sysadmin role to FlashyAdmin if not already a member
IF NOT EXISTS (SELECT *
FROM sys.server_role_members
WHERE role_principal_id = SUSER_SID('sysadmin') AND member_principal_id = SUSER_SID('FlashyAdmin'))
BEGIN
  ALTER SERVER ROLE sysadmin ADD MEMBER FlashyAdmin;
END
GO
