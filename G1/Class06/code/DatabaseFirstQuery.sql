/* =============================================================================
   TodoAppDb - Database First demo
   Class 06 - Advanced Entity Framework

   Run the whole file in SSMS (F5) BEFORE scaffolding.
   Safe to run again: it drops the database and starts clean.

   Here the database is the source of truth. Every choice below - type, length,
   NULL or NOT NULL, key, index, foreign key - shows up in the C# that EF Core
   generates. The comments say where.
   ============================================================================= */
USE [master];
GO

-- Re-runnable: kick everyone off and drop the old database if it is there.
IF DB_ID('TodoAppDb') IS NOT NULL
BEGIN
    ALTER DATABASE [TodoAppDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [TodoAppDb];
END
GO

CREATE DATABASE [TodoAppDb];
GO

USE [TodoAppDb];
GO


/* -----------------------------------------------------------------------------
   Category
   -------------------------------------------------------------------------- */
CREATE TABLE [dbo].[Category]
(
    [Id]   INT           IDENTITY(1,1) NOT NULL,   -- IDENTITY  -> ValueGeneratedOnAdd()
    [Name] NVARCHAR(50)                NOT NULL,   -- NOT NULL  -> string  (never null)

    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- A unique index scaffolds as .HasIndex(e => e.Name).IsUnique()
CREATE UNIQUE INDEX [IX_Category_Name] ON [dbo].[Category] ([Name]);
GO


/* -----------------------------------------------------------------------------
   Status
   -------------------------------------------------------------------------- */
CREATE TABLE [dbo].[Status]
(
    [Id]          INT            IDENTITY(1,1) NOT NULL,
    [Name]        NVARCHAR(50)                 NOT NULL,
    [Description] NVARCHAR(250)                    NULL,   -- NULL -> string?

    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO


/* -----------------------------------------------------------------------------
   Todo

   The three date columns are the interesting part. What each becomes in C#:

     DueDate      DATE       nullable - a day, no time. Not every todo has one.
                             -> DateOnly?   NOT DateTime. EF Core 8 maps the
                                SQL "date" type to DateOnly.
     CreatedAt    DATETIME2  set by the database via DEFAULT, so the app never
                             has to remember it.
                             -> DateTime  + HasDefaultValueSql("(sysutcdatetime())")
     CompletedAt  DATETIME2  nullable - null until the todo is done.
                             -> DateTime?

   All UTC: SYSUTCDATETIME(), never GETDATE(). Convert to local time in the UI.
   -------------------------------------------------------------------------- */
CREATE TABLE [dbo].[Todo]
(
    [Id]          INT            IDENTITY(1,1) NOT NULL,
    [Description] NVARCHAR(1000)               NOT NULL,
    [DueDate]     DATE                             NULL,
    [CreatedAt]   DATETIME2(7)                 NOT NULL
        CONSTRAINT [DF_Todo_CreatedAt] DEFAULT (SYSUTCDATETIME()),
    [CompletedAt] DATETIME2(7)                     NULL,
    [CategoryId]  INT                          NOT NULL,
    [StatusId]    INT                          NOT NULL,

    CONSTRAINT [PK_Todo] PRIMARY KEY CLUSTERED ([Id] ASC),

    -- No cascade on purpose: deleting a Category must not silently delete
    -- every Todo in it. SQL Server refuses the delete instead, and EF Core
    -- scaffolds this as .OnDelete(DeleteBehavior.ClientSetNull).
    CONSTRAINT [FK_Todo_Category] FOREIGN KEY ([CategoryId])
        REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [FK_Todo_Status] FOREIGN KEY ([StatusId])
        REFERENCES [dbo].[Status] ([Id])
);
GO

-- Foreign key columns we filter on deserve an index.
CREATE INDEX [IX_Todo_CategoryId] ON [dbo].[Todo] ([CategoryId]);
CREATE INDEX [IX_Todo_StatusId]   ON [dbo].[Todo] ([StatusId]);
GO


/* -----------------------------------------------------------------------------
   Seed data - so the very first GET returns something.
   -------------------------------------------------------------------------- */
INSERT INTO [dbo].[Category] ([Name]) VALUES
    (N'Work'),
    (N'Home'),
    (N'Study');
GO

INSERT INTO [dbo].[Status] ([Name], [Description]) VALUES
    (N'New',         N'Created, nobody started it yet'),
    (N'In Progress', N'Someone is working on it right now'),
    (N'Done',        NULL);   -- Description is nullable, so we may leave it out
GO

-- Dates are relative to "today" so this demo never goes stale, and the
-- /api/todos/overdue endpoint always has something to return.
DECLARE @Today DATE = CAST(SYSUTCDATETIME() AS DATE);

INSERT INTO [dbo].[Todo] ([Description], [DueDate], [CompletedAt], [CategoryId], [StatusId]) VALUES
    (N'Send last week''s report', DATEADD(DAY, -4, @Today), NULL,                              1, 1),  -- overdue
    (N'Prepare the Class 06 demo', DATEADD(DAY,  2, @Today), NULL,                              1, 2),
    (N'Review submitted homework', DATEADD(DAY,  5, @Today), NULL,                              1, 1),
    (N'Buy groceries',             NULL,                     NULL,                              2, 1),  -- no deadline
    (N'Read the EF Core docs',     DATEADD(DAY, -1, @Today), DATEADD(HOUR, -3, SYSUTCDATETIME()), 3, 3); -- done, so not overdue
GO
-- CreatedAt was never mentioned above - the DEFAULT filled it in for us.


/* -----------------------------------------------------------------------------
   Sanity check - run this and you should see 4 todos with their names resolved.
   -------------------------------------------------------------------------- */
SELECT  t.[Id],
        t.[Description],
        c.[Name] AS [Category],
        s.[Name] AS [Status],
        t.[DueDate],
        t.[CreatedAt],
        t.[CompletedAt]
FROM        [dbo].[Todo]     t
INNER JOIN  [dbo].[Category] c ON c.[Id] = t.[CategoryId]
INNER JOIN  [dbo].[Status]   s ON s.[Id] = t.[StatusId]
ORDER BY    t.[Id];
GO
