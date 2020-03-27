using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApplication.Models
{
    public class CreateViewModel
    {
        [Display(Name = "Person Name")]
        [Required]
        [StringLength(50)]
        public string PersonName { get; set; }

        [Display(Name = "Date Added")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }

        [Required]
        [Display(Name = "Group")]
        public string SelectedGroupId { get; set; }
        public IEnumerable<SelectListItem> Group { get; set; }
    }
}
