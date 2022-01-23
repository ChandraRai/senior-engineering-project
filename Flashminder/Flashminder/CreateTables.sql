USE FlashMinder
GO
BEGIN

DROP TABLE IF EXISTS Flashcard;
DROP TABLE IF EXISTS CardType;
DROP TABLE IF EXISTS USERS;

CREATE TABLE [USERS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Privilege] [smallint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
));
END

BEGIN

CREATE TABLE [dbo].[CardType](
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NULL,
);
INSERT INTO CardType([Name]) VALUES
	('DEFAULT'),
	('MATCH')
END

BEGIN
CREATE TABLE [dbo].[Flashcard](
	UserId INT,
	Id INT PRIMARY KEY IDENTITY,
	FrontText NVARCHAR (500),
	BackText NVARCHAR (500),
	FrontImage VARBINARY(MAX),
	BackImage VARBINARY(MAX),
	fk_CardType INT NOT NULL,
	FOREIGN KEY (fk_CardType) REFERENCES CardType(Id), 
	FOREIGN KEY (UserId) REFERENCES USERS(Id),
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
	ModifiedDate DATETIME
);
END

BEGIN
INSERT INTO USERS(username, password, email, privilege) values('admin', '49e38d144e3ec4fc3966f0e1ec5731e54756b556', 'admin', 1)
END