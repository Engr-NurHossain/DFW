-- For Error Logging with support for multiple levels
-- AppLogs => Debug level | DevLogs => Trace level | ErrLogs => Error level

CREATE TABLE AppLogs(
    Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
    Level varchar(10),
	Logger nvarchar(255),
	CallAction varchar(max) not null,
    Message nvarchar(max),
	Property1 nvarchar(max),
    Property2 nvarchar(max),
    AppDir nvarchar(max),
	Username varchar(50),
	CreatedOn datetime NOT NULL
);

CREATE TABLE DevLogs(
    Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
    Level varchar(10),
	Logger nvarchar(255),
	CallAction varchar(max) not null,
    Message nvarchar(max),
	Tags nvarchar(max),
	Params nvarchar(max),
	Username varchar(50),
	CreatedOn datetime NOT NULL
);

CREATE TABLE ErrLogs(
    Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
    Level varchar(10),
	Logger nvarchar(255),
	CallAction varchar(max) not null,
    Message nvarchar(max),
	ErrorMessage nvarchar(max),
    StackTrace nvarchar(max),
    Exception nvarchar(max),
	Username varchar(50),
	CreatedOn datetime NOT NULL
);
