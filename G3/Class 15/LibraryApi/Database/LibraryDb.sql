IF DB_ID('LibraryDbNet8') IS NULL
BEGIN
    CREATE DATABASE LibraryDbNet8;
END
GO

USE LibraryDbNet8;
GO

IF OBJECT_ID('dbo.Book', 'U') IS NOT NULL DROP TABLE dbo.Book;
IF OBJECT_ID('dbo.Author', 'U') IS NOT NULL DROP TABLE dbo.Author;
GO

CREATE TABLE dbo.Author
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL,
    UpdatedDate DATETIME2 NOT NULL
);
GO

CREATE INDEX IX_Author_LastName ON dbo.Author(LastName);
GO

CREATE TABLE dbo.Book
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Isbn NVARCHAR(20) NOT NULL,
    Year INT NOT NULL,
    PageCount INT NOT NULL,
    Genre NVARCHAR(30) NOT NULL,
    AuthorId INT NOT NULL,
    CreatedDate DATETIME2 NOT NULL,
    UpdatedDate DATETIME2 NOT NULL,
    CONSTRAINT FK_Book_Author
        FOREIGN KEY (AuthorId) REFERENCES dbo.Author(Id)
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_Book_AuthorId ON dbo.Book(AuthorId);
GO

SET IDENTITY_INSERT dbo.Author ON;

INSERT INTO dbo.Author
    (Id, FirstName, LastName, Country, CreatedDate, UpdatedDate)
VALUES
    (1, 'George', 'Orwell', 'United Kingdom', '2026-01-01', '2026-01-01'),
    (2, 'Isaac', 'Asimov', 'United States', '2026-01-01', '2026-01-01'),
    (3, 'Ursula', 'Le Guin', 'United States', '2026-01-01', '2026-01-01'),
    (4, 'Yuval', 'Harari', 'Israel', '2026-01-01', '2026-01-01');

SET IDENTITY_INSERT dbo.Author OFF;
GO

SET IDENTITY_INSERT dbo.Book ON;

INSERT INTO dbo.Book
    (Id, Title, Isbn, Year, PageCount, Genre, AuthorId, CreatedDate, UpdatedDate)
VALUES
    (1, '1984', '9780451524935', 1949, 328, 'Fiction', 1, '2026-01-01', '2026-01-01'),
    (2, 'Animal Farm', '9780452284241', 1945, 112, 'Fiction', 1, '2026-01-01', '2026-01-01'),
    (3, 'Homage to Catalonia', '9780156421171', 1938, 232, 'History', 1, '2026-01-01', '2026-01-01'),
    (4, 'Foundation', '9780553293357', 1951, 255, 'Science', 2, '2026-01-01', '2026-01-01'),
    (5, 'I, Robot', '9780553382563', 1950, 253, 'Science', 2, '2026-01-01', '2026-01-01'),
    (6, 'A Wizard of Earthsea', '9780553383041', 1968, 183, 'Fantasy', 3, '2026-01-01', '2026-01-01'),
    (7, 'The Left Hand of Darkness', '9780441478125', 1969, 304, 'Fantasy', 3, '2026-01-01', '2026-01-01'),
    (8, 'Sapiens', '9780062316097', 2011, 443, 'History', 4, '2026-01-01', '2026-01-01'),
    (9, 'Homo Deus', '9780062464316', 2015, 450, 'History', 4, '2026-01-01', '2026-01-01');

SET IDENTITY_INSERT dbo.Book OFF;
GO
