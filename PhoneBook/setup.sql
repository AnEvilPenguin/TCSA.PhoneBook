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
CREATE TABLE [Contacts] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(70) NOT NULL,
    [Email] nvarchar(254) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id])
    );

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250513172036_Initial', N'9.0.4');

ALTER TABLE [Contacts] ADD [CategoryId] int NULL;

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(70) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );

CREATE INDEX [IX_Contacts_CategoryId] ON [Contacts] ([CategoryId]);

CREATE UNIQUE INDEX [IX_Categories_Name] ON [Categories] ([Name]);

ALTER TABLE [Contacts] ADD CONSTRAINT [FK_Contacts_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250515165729_Category', N'9.0.4');

COMMIT;
GO
