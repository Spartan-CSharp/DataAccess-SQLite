CREATE TABLE [dbo].[EmployerAddresses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployerId] INT NOT NULL,
	[AddressId] INT NOT NULL,
	CONSTRAINT [FK_EmployerAddress_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [Employers]([Id]),
	CONSTRAINT [FK_EmployerAddress_Address] FOREIGN KEY ([AddressId]) REFERENCES [Addresses]([Id])
)
