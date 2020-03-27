using Microsoft.EntityFrameworkCore;
using PersonApplication.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonApplication.DataAccess
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options)
            : base(options)
        {
        }

        public PersonContext()
           : base()
        {
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Group> Group { get; set; }
    }
}
