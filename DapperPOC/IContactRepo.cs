using System.Collections.Generic;

namespace DapperPOC
{
    public interface IContactRepo
    {
        List<Contact> GetAll();
        List<Contact> GetAll(List<int> ids);
        List<dynamic> DynamicsGetAll();
        Contact GetByID(int id);
        Contact NumericReplaceMentGetByID(int id);
        int Add(Contact contact);
        int BulkInsert(List<Contact> contacts);
        Contact Update(Contact contact);
        Contact Delete(int id);
        Contact GetContactFullData(int id);
    }
}