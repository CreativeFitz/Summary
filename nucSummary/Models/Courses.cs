using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models
{
    public class Courses
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        [Display(Name = "Course Code")]
        public string Code { get; set; }

        
        [Display(Name ="School")]
        public string Host { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public List<Assessments> Assessments { get; set; } = new List<Assessments>();

        public List<StudentCourses> Students { get; set; } = new List<StudentCourses>();

        public List<CourseTags> CourseTags { get; set; } = new List<CourseTags>();

    }
}
