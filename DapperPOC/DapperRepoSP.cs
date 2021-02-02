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

        public int Insert(List<UT_Contact> contacts)
        {
            var parameters = new DynamicParameters();
            parameters.AddTable("@contacts", "ud_t_Contact", contacts);
            return db.Query<int>("mySpInsertContacts", parameters, commandType: CommandType.StoredProcedure).First();
        }

    }
}