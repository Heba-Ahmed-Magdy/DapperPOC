Create PROCEDURE [dbo].[UpdateContact]
	@contacts UDT_Contact Readonly
AS
	UPDATE Contacts 
	SET 
		Contacts.FirstName = c.FirstName,
		Contacts.LastName = c.LastName,
		Contacts.Title = c.Title,
		Contacts.Email = c.Email,
		Contacts.Company = c.Company
	    FROM @contacts c
		WHERE Contacts.Id = c.Id

	Select @@ROWCOUNT;
RETURN 0
