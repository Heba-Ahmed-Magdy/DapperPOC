using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace DapperPOC
{
    public class Contact
    {
        public int Id { get; set; }
        //I returned it back when I had started using of dapper.contrib 
        //public string FName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        [Write(false)]
        public List<Address> Addresses { get; } = new List<Address>();
    }
}