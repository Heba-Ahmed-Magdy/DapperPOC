CREATE PROCEDURE [dbo].[mySpInsertContacts]
	@contacts [dbo].[UDT_Contact] readonly
AS
	INSERT INTO Contacts( FirstName, LastName, Email, Company, Title)
	SELECT FirstName, LastName, Email, Company, Title FROM  @contacts;

	Select @@ROWCOUNT;
RETURN 0