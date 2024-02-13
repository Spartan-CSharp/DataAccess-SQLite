CREATE TABLE [dbo].[People]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL, 
	[IsActive] BIT NULL,
	[EmployerId] INT NULL,
	CONSTRAINT [FK_Person_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [Employers]([Id])
)
