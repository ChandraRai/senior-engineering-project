/****** Object:  Table [dbo].[USERS]    Script Date: 2022-01-19 1:08:37 AM ******/
CREATE TABLE [dbo].[USERS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Privilege] [smallint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
);