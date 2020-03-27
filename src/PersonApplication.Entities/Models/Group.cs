using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApplication.Entities.Models
{
    public class Group
    {
        public Group()
        {
            this.Persons = new HashSet<Person>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
