using PersonApplication.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonApplication.DataAccess
{
    public interface IPersonRespository
    {
        Task<IEnumerable<Group>> GetAllGroups();
        Task AddPerson(Person person);
        Task<IEnumerable<Person>> Search(string searchName, string searchGroup);
        Task<IEnumerable<Person>> GetAllPersons();
    }
}
