using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace nucSummary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Account Creation Date")]
        public int DateCreated { get; set; }


        public List<Courses> Courses { get; set; } = new List<Courses>();

        public List<Reviews> Reviews { get; set; } = new List<Reviews>();


    }
}
