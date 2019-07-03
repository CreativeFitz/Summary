using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models
{
    public class Students
    {
       
            public int Id { get; set; }

            [Required]
            [Display(Name = "Student Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Student Identification")]
            public string StudentCode { get; set; }

        public List<Results> Results { get; set; } = new List<Results>();

        public List<StudentCourses> Courses { get; set; } = new List<StudentCourses>();

        }
    }