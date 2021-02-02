CREATE PROCEDURE [dbo].[mySpInsertContacts]
	@contacts [dbo].[ud_t_Contact] readonly
AS
	INSERT INTO Contacts( FirstName, LastName, Email, Company, Title)
	SELECT * FROM  @contacts;

	Select @@ROWCOUNT;
RETURN 0