using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperPOC
{
    public class ContactRepo : IContactRepo
    {
        IDbConnection db;
        public ContactRepo(string connection)
        {
            db = new SqlConnection(connection);
        }
        public int Add(Contact contact)
        {
            var command = "Insert into Contacts( FirstName, LastName, Email, Company, Title) Values( @FName, @LastName, @Email, @Company, @Title)" +
                "Select Cast(SCOPE_IDENTITY()as int)";
            var id = db.Query(command, contact).Single();
            return id;
        }

        public Contact Delete(int id)
        {
            var command = "DELETE FROM contacts where Id=@id";
            db.Execute(command, new { id });
            return GetByID(id);
        }

        public List<Contact> GetAll()
        {
            #region [there was no difference here bet. col names and c# object field names]
            //return db.Query<Contact>("Select * from contacts")
            //         .ToList();
            #endregion

            #region []
            // case the field name in c# object is different from the column name then we will need to use sql alias.
            return db.Query<Contact>("Select Id, FirstName FName, LastName, Email, Company, Title from contacts")
                     .ToList();
            #endregion

        }

        public List<Contact> GetAll(List<int> ids)
        {
            return db.Query<Contact>("SELECT * FROM contacts WHERE Id in @ids", new { ids }).ToList();
        }
        public List<dynamic> DynamicsGetAll()
        {
            return db.Query("SELECT * FROM contacts").ToList();
        }
        public Contact GetByID(int id)
        {
            return db.Query<Contact>("Select Id, FirstName FName, LastName, Email, Company, Title from contacts where Id=@id", new { id }).SingleOrDefault();
        }
        public Contact NumericReplaceMentGetByID(int id)
        {
            return db.Query<Contact>("Select Id, FirstName FName, LastName, Email, Company, Title from contacts where Id= {=id}", new { id }).SingleOrDefault();
        }

        public Contact GetContactFullData(int id)
        {
            throw new NotImplementedException();
        }

        public Contact Update(Contact contact)
        {
            var command = " UPDATE contacts" +
                          " SET FirstName=@FName," +
                          " LastName=@LastName," +
                          " Email=@Email," +
                          " Company=@Company," +
                          " Title=@Title" +
                          " Where Id=@Id";
            db.Execute(command, contact);
            return GetByID(contact.Id);
        }

        public int BulkInsert(List<Contact> contacts)
        {
            throw new NotImplementedException();
        }
    }
}
