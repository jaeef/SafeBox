-- ============================================
-- SafeBox Database Setup Script
-- Run this script in SQL Server Management Studio (SSMS) 
-- to create the SafeBox database and all required tables.
-- ============================================

-- Step 1: Create the database
CREATE DATABASE [SafeBox];
GO
USE [SafeBox];
GO

-- Step 2: Create Roles table
CREATE TABLE Roles (
    Role_ID   INT IDENTITY(1,1) PRIMARY KEY,
    Role_Name NVARCHAR(50) NOT NULL UNIQUE
);

-- Step 3: Create Users table
CREATE TABLE Users (
    User_ID       INT IDENTITY(1,1) PRIMARY KEY,
    Username      NVARCHAR(100)  NOT NULL UNIQUE,
    Email         NVARCHAR(255)  NOT NULL UNIQUE,
    Password_Hash VARBINARY(MAX) NOT NULL,
    Role_ID       INT            NOT NULL DEFAULT 2,
    Status        NVARCHAR(20)   NOT NULL DEFAULT 'Active',
    Created_At    DATETIME       NOT NULL DEFAULT GETDATE(),
    Last_Login    DATETIME       NULL,
    Recovery_Hint NVARCHAR(500)  NULL,
    FOREIGN KEY (Role_ID) REFERENCES Roles(Role_ID)
);

-- Step 4: Create Admins table
CREATE TABLE Admins (
    Admin_ID       INT IDENTITY(1,1) PRIMARY KEY,
    Admin_Username NVARCHAR(100) NOT NULL UNIQUE,
    Email          NVARCHAR(255) NOT NULL,
    Password_Hash  NVARCHAR(MAX) NOT NULL,
    Created_At     DATETIME      NOT NULL DEFAULT GETDATE()
);

-- Step 5: Create Vaults table
CREATE TABLE Vaults (
    Vault_ID    INT IDENTITY(1,1) PRIMARY KEY,
    Vault_Name  NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500) NULL,
    User_ID     INT           NOT NULL,
    Created_At  DATETIME      NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID)
);

-- Step 6: Create Files table
CREATE TABLE Files (
    File_ID       INT IDENTITY(1,1) PRIMARY KEY,
    File_Name     NVARCHAR(500)  NOT NULL,
    Original_Name NVARCHAR(500)  NOT NULL,
    Extension     NVARCHAR(50)   NULL,
    File_Type     NVARCHAR(100)  NULL,
    File_Size     BIGINT         NOT NULL DEFAULT 0,
    File_Data     VARBINARY(MAX) NULL,
    Vault_ID      INT            NOT NULL,
    Uploaded_At   DATETIME       NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (Vault_ID) REFERENCES Vaults(Vault_ID)
);

-- Step 7: Create Activity Logs table
CREATE TABLE ActivityLogs (
    Log_ID      INT IDENTITY(1,1) PRIMARY KEY,
    User_ID     INT            NOT NULL,
    Action_Type NVARCHAR(100)  NOT NULL,
    Description NVARCHAR(500)  NULL,
    Timestamp   DATETIME       NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID)
);

-- Step 8: Create Audit Logs table (Admin actions)
CREATE TABLE AuditLogs (
    Audit_ID    INT IDENTITY(1,1) PRIMARY KEY,
    Admin_ID    INT            NOT NULL,
    Action      NVARCHAR(200)  NOT NULL,
    Description NVARCHAR(500)  NULL,
    Timestamp   DATETIME       NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (Admin_ID) REFERENCES Admins(Admin_ID)
);

-- Step 9: Create Shared Access table
CREATE TABLE SharedAccess (
    Access_ID    INT IDENTITY(1,1) PRIMARY KEY,
    File_ID      INT           NOT NULL,
    Shared_By    INT           NOT NULL,
    Shared_With  INT           NOT NULL,
    Permission   NVARCHAR(50)  NOT NULL DEFAULT 'Read',
    Shared_At    DATETIME      NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (File_ID) REFERENCES Files(File_ID),
    FOREIGN KEY (Shared_By) REFERENCES Users(User_ID),
    FOREIGN KEY (Shared_With) REFERENCES Users(User_ID)
);

-- Step 10: Seed default roles
INSERT INTO Roles (Role_Name) VALUES ('Admin'), ('User');

-- ============================================
-- Done! Your SafeBox database is ready.
-- Now update the connection string in:
--   SafeBox/Infrastructure/Data/DatabaseHelper.cs
-- Replace YOUR_SERVER with your actual SQL Server instance name.
-- ============================================
