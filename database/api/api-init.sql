-- Check if the database exists before creating it
IF NOT EXISTS (SELECT name
FROM sys.databases
WHERE name = 'FlashyAPI')
BEGIN
  CREATE DATABASE FlashyAPI;
END
GO

USE FlashyAPI;
GO

-- Create FlashyAPI login if it doesn't exist
IF NOT EXISTS (SELECT name
FROM sys.sql_logins
WHERE name = 'FlashyAPI')
BEGIN
  CREATE LOGIN FlashyAPI WITH PASSWORD = 'Fl@shy@PI#062024!';
END
GO

-- Create FlashyAPI user if it doesn't exist
IF NOT EXISTS (SELECT name
FROM sys.database_principals
WHERE name = 'FlashyAPI')
BEGIN
  CREATE USER FlashyAPI FOR LOGIN FlashyAPI;
END
GO

-- Add FlashyAPI to db_owner role if not already a member
IF NOT EXISTS (SELECT *
FROM sys.database_role_members
WHERE role_principal_id = USER_ID('db_owner') AND member_principal_id = USER_ID('FlashyAPI'))
BEGIN
  ALTER ROLE db_owner ADD MEMBER FlashyAPI;
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

-- Add your tables and other schema elements here
