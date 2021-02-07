using Dapper;
using DapperParameters;
using DapperPOC.UserDefined.TableType;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperPOC
{
    public class DapperRepoSP
    {
        IDbConnection db;
        public DapperRepoSP(string conn)
        {
            db = new SqlConnection(conn);
        }
        // https://www.nuget.org/packages/Dapper.ParameterExtensions/0.2.0
        public int Insert(List<UT_Contact> contacts)
        {
            var parameters = new DynamicParameters();
            parameters.AddTable("@contacts", "UDT_Contact", contacts);
            return db.Query<int>("mySpInsertContacts", parameters, commandType: CommandType.StoredProcedure).First();
        }

        public int Update(List<UT_Contact> contacts)
        {
            var parameters = new DynamicParameters();
            parameters.AddTable("@contacts", "UDT_Contact", contacts);
            return db.Query<int>("UpdateContact", parameters, commandType: CommandType.StoredProcedure).First();
        }


        public List<UT_Contact> GetById (List<int> ids)
        {
            var command = "Select * from Contacts where Id in @ids";
            return db.Query<UT_Contact>(command, new { ids }).ToList();
        }

        public List<UT_Contact> GetAll()
        {
            var command = "Select * from Contacts";
            return db.Query<UT_Contact>(command).ToList();
        }
        public int GetCountForAll()
        {
            var command = "Select count(*) from contacts";
            return db.Query<int>(command).FirstOrDefault();
        }

    }
}