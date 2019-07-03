using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models
{
    public class StudentCourses
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }

        public Courses Course { get; set; }
        public Students Student { get; set; }
    }
}
