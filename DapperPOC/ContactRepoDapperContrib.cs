using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace DapperPOC
{
    //here I use dapper.contrib which is an add-on for dapper to handle basic sql statments(it will generate them for you)
    //like insert update delete select
    public class ContactRepoDapperContrib : IContactRepo
    {
        IDbConnection db;
        public ContactRepoDapperContrib(string connection) => db = new SqlConnection(connection);
        public int Add(Contact contact)
        {
            //old versions from System.Data.SqlClient throws his exception "System.NotSupportedException: Enlisting in Ambient transactions is not supported." take care
            using var transaction = new TransactionScope();
            var id = (int)db.Insert(contact);
            try
            {
                if (contact.Addresses != null && contact.Addresses.Count != 0)
                {
                    foreach (var address in contact.Addresses)
                    {
                        address.ContactId = id;
                        //dapper.contrib Inserts an entity into table "Ts" and returns identity id or number of inserted
                        //that means it will generate this query "insert into Addresss values (....)"
                        //and the name of the table is Addresses not Addresss so I will need to add [Table("Addresses")] in Address.cs
                        db.Insert(address);
                    }
                }

                //The idea from opening transaction that if any error happens before I closed it, all actions will be recorded in log file
                //and if any exception happened it will be rolled back and vice versa
                //throw new Exception(); 

                transaction.Complete();
            }
            catch (Exception ex)
            {

            }
            return id;
        }
        public int BulkInsert(List<Contact> contacts)
        {
            //Once I pass a list to the Insert method, it will still doing only one insertion for one row 
            //but it will repeate it with the number of objects in the passing lst
            /*
             *so this way isn't a true bulk insert but in this case you will pass a list og objects and dapper 
             * will be responsible on repeatin the insert statment n times
             * ex: for list contains 2 objects it will be
             *     Insert into employees values(......)
             *     Insert into employees values(......)
            */
            var id = (int) db.Insert(contacts);
            return id;
        }

        #region[Using Delete for dapper.contrib]
        //public Contact Delete(int id)
        //{
        //    db.Delete(new Contact { Id = id });
        //    return GetByID(id);
        //}
        #endregion

        #region[Delete usng procs]
        public Contact Delete(int id)
        {
            return db.Query("DeleteContact", new { id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }
        #endregion
        public List<Contact> GetAll()
        {
            return db.GetAll<Contact>().ToList();
        }
        public List<dynamic> DynamicsGetAll()
        {
            return db.Query("SELECT * FROM contacts").ToList();
        }

        public List<Contact> GetAll(List<int> ids)
        {
            return db.Query<Contact>("SELECT * FROM contacts WHERE Id in @ids", new { ids }).ToList();
        }

        public Contact GetByID(int id)
        {
            return db.Get<Contact>(id);
        }
        public Contact NumericReplaceMentGetByID(int id)
        {
            return db.Query<Contact>("Select Id, FirstName FName, LastName, Email, Company, Title from contacts where Id= {=id}", new { id }).SingleOrDefault();
        }
        public Contact GetContactFullData(int id)
        {
            var command = " SELECT * FROM contacts where id= @id" +
                          " SELECT * from addresses where ContactId=@id";
            using (var resultSets = db.QueryMultiple(command, new { id }))// to return multiple result sets
            {
                var contact = resultSets.Read<Contact>().SingleOrDefault();
                var addresses = resultSets.Read<Address>().ToList();
                if (contact != null && addresses != null)
                {
                    contact.Addresses.AddRange(addresses);
                }
                return contact;
            }
        }

        public Contact Update(Contact contact)
        {
            db.Update(contact);
            return GetByID(contact.Id);
        }
    }
}
