CREATE TYPE [dbo].[UDT_Contact]
AS TABLE 
(
  [Id]        INT,
  [FirstName] VARCHAR (50) NULL,
  [LastName]  VARCHAR (50) NULL,
  [Email]     VARCHAR (50) NULL,
  [Company]   VARCHAR (50) NULL,
  [Title]     VARCHAR (50) NULL
)
--UDT => Userdefined table