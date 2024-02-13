CREATE TABLE [dbo].[PersonAddresses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[PersonId] INT NOT NULL,
	[AddressId] INT NOT NULL,
	CONSTRAINT [FK_PersonAddress_Person] FOREIGN KEY ([PersonId]) REFERENCES [People]([Id]),
	CONSTRAINT [FK_PersonAddress_Address] FOREIGN KEY ([AddressId]) REFERENCES [Addresses]([Id])
)
