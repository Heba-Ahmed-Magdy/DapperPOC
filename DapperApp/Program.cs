using Dapper;
using DapperPOC;
using DapperPOC.UserDefined.TableType;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DapperApp
{
    class Program
    {
        /* The main methods for dapper are query,queryMultible,Execute*/
        static IConfigurationRoot config;
        static void Main(string[] args)
        {
            #region [A real bulk insert]
            Console.WriteLine("****************Real Bulk insert using SP and userdefined table type ****************");
            var dbSP = new DapperRepoSP("server=.;database=DapperPOC;Trusted_Connection=Yes;");

            var contactsSP = new List<UT_Contact>
            {
                new UT_Contact {FirstName = "Heba1", LastName = "Ahmed", Title = "AS", Company = "int", Email = "h@f.com" },
                new UT_Contact {FirstName = "Heba2", LastName = "Ahmed", Title = "AS", Company = "int", Email = "h@f.com" },
            };
           

            Console.WriteLine($"{dbSP.Insert(contactsSP)}");

            #endregion

            Console.WriteLine(JsonConvert.SerializeObject(dbSP.GetAll(),Formatting.Indented));

            Console.WriteLine("*************Get contacts for the given Ids comma seperated****************");
            var ids = new List<int>();
            Array.ForEach(Console.ReadLine().Split(','), (e) => { e = e.Trim(); ids.Add(int.Parse(e)); });
            contactsSP = dbSP.GetById(ids);

            if(contactsSP.Any())
            {
                foreach (var contact in contactsSP)
                {
                    if (contact.Id == 1) { contact.FirstName = "Ramy1"; }
                    if (contact.Id == 2) { contact.FirstName = "HAny1"; }
                }
                Console.WriteLine($"{dbSP.Update(contactsSP.Where(c=> c.Id==1 || c.Id == 2).ToList())}");
            }

            Console.WriteLine(JsonConvert.SerializeObject(dbSP.GetAll(), Formatting.Indented));

        }

        public void DummyMethod()
        {
            var contactRepo = CreateRepository();
            int id;
            var ids = new List<int>();

            #region [GetAll]
            var contacts = contactRepo.GetAll();
            Console.WriteLine($"No. of contacts = {contacts.Count}{contacts.First().FirstName}");
            Console.WriteLine(JsonConvert.SerializeObject(contacts, Formatting.Indented));
            #endregion

            #region [Create]
            var contact = new Contact { /*FName*/ FirstName = "Heba", LastName = "Ahmed", Title = "AS", Company = "int", Email = "h@f.com" };
            var address = new Address { StateId = 1, City = "Cairo" };
            contact.Addresses.Add(address);
            Console.WriteLine($"Id for inserted contact = {contactRepo.Add(contact)}");
            #endregion

            Console.WriteLine("Plz enter id for contact that you need to get, update then remove");
            id = int.Parse(Console.ReadLine());

            #region [FindBy]
            Console.WriteLine(JsonConvert.SerializeObject(contactRepo.GetContactFullData(id), Formatting.Indented));
            #endregion

            #region [Update]
            Console.WriteLine("--------UPDATE-------------");

            contact = contactRepo.GetByID(id);
            if (contact != null)
            {
                //contact.FName = contact.FName + "2";
                contact.FirstName = contact.FirstName + "2";
                Console.WriteLine(JsonConvert.SerializeObject(contactRepo.Update(contact), Formatting.Indented));
            }
            #endregion

            #region [Delete]
            Console.WriteLine(JsonConvert.SerializeObject(contactRepo.Delete(id)));
            #endregion

            Console.WriteLine("***********Plz enter id for contact that you need to get all of its data***********");
            id = int.Parse(Console.ReadLine());
            Console.WriteLine(JsonConvert.SerializeObject(contactRepo.GetContactFullData(id), Formatting.Indented));

            #region [In Operator]
            Console.WriteLine("*************Get contacts for the given Ids comma seperated****************");
            Array.ForEach(Console.ReadLine().Split(','), (e) => { e = e.Trim(); ids.Add(int.Parse(e)); });
            contacts = contactRepo.GetAll(ids);
            Console.WriteLine(JsonConvert.SerializeObject(contacts, Formatting.Indented));
            #endregion

            #region [dapper support mapping to dynamic objects]
            var dunamicContacts = contactRepo.DynamicsGetAll();
            Console.WriteLine($"No. of contacts = {dunamicContacts.Count}{dunamicContacts.First().FirstName}");//No intellesense as its dunamic object
            Console.WriteLine(JsonConvert.SerializeObject(dunamicContacts, Formatting.Indented));
            #endregion

            #region [Bulk insert]
            Console.WriteLine("****************Bulk insert********");
            contacts = new List<Contact>
            {
                new Contact {FirstName = "Heba1", LastName = "Ahmed", Title = "AS", Company = "int", Email = "h@f.com" },
                new Contact {FirstName = "Heba2", LastName = "Ahmed", Title = "AS", Company = "int", Email = "h@f.com" },
            };
            Console.WriteLine($"Id for inserted contacts = {contactRepo.BulkInsert(contacts)}");
            #endregion

            #region [Literal replacement]
            //It is instead of using parameters select * from contacts where Id = @id
            //we use "Literal replacement" which is better for execution plans and filtered index usage
            //its only can be used for boolean and numeric dataTypes
            Console.WriteLine("****Literal replacement**********Plz enter id for contact that you need to get, update then remove");
            id = int.Parse(Console.ReadLine());
            Console.WriteLine(JsonConvert.SerializeObject(contactRepo.GetContactFullData(id), Formatting.Indented));

            #endregion
        }


        public static void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",optional:true,reloadOnChange:true);
            config = builder.Build();
        }
        private static IContactRepo CreateRepository()
        {
            //return new ContactRepo(config.GetSection("ConnectionStrings")["DefaultConnection"]);

            //return new ContactRepo("server=.;database=DapperPOC;Trusted_Connection=Yes;"); 
            return new ContactRepoDapperContrib("server=.;database=DapperPOC;Trusted_Connection=Yes;"); 

        }
    }
}
