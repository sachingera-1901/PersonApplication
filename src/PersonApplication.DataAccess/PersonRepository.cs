using Microsoft.EntityFrameworkCore;
using PersonApplication.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace PersonApplication.DataAccess
{
    public class PersonRepository : IPersonRespository
    {
        private readonly PersonContext _context;

        public PersonRepository(PersonContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            return await _context.Group.ToListAsync();
        }

        public async Task AddPerson(Person person)
        {
            var group = await _context.Group.FirstAsync(x => x.Id == person.Group.Id);
            person.Group = group;
            _context.Add(person);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Person>> Search(string searchName, string searchGroup)
        {
            var persons = await _context.Person.Include(p => p.Group).ToListAsync();

            if (!string.IsNullOrEmpty(searchName))
            {
                persons = persons.Where(s => s.Name.Contains(searchName)).ToList();
            }

            if (!string.IsNullOrEmpty(searchGroup))
            {
                persons = persons.Where(s => s.Group.Name.Contains(searchGroup)).ToList();
            }

            return persons;
        }

        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            return await _context.Person.Include(p => p.Group).ToListAsync();
        }
    }
}
