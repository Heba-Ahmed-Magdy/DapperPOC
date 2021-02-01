CREATE TABLE [dbo].[Addresses] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [ContactId]     INT            NULL,
    [AddressType]   VARCHAR (10)  NULL,
    [StreetAddress] VARCHAR (50)  NULL,
    [City]          VARCHAR (50)  NULL,
    [StateId]       INT           NULL,
    [PostalCode]    VARCHAR (20)  NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Addresses_Contacts] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Addresses_States] FOREIGN KEY ([StateId]) REFERENCES [dbo].[States] ([Id])
);

