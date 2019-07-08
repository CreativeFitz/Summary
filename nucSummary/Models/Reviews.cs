using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models
{
    public class Reviews
    {
        public int? Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int CourseId { get; set; }
        [Required]
        [Display(Name = "Difficulty Rating")]
        public int Difficulty { get; set; }
        [Required]
        [Display(Name = "Content Rating")]
        public int Content { get; set; }
        [Required]
        [Display(Name = "Design Rating")]
        public int Design { get; set; }
        [Required]
        [Display(Name = "Assessments Rating")]
        public int Assessments { get; set; }
        [Required]
        [Display(Name = "Exercises Rating")]
        public int Exercises { get; set; }
        [Required]
        [Display(Name = "Relevancy Rating")]
        public int Relevancy { get; set; }
        public string Overview { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Courses Course { get; set; }
    }
}
