using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models
{
    public class CourseTags
    {
        public int Id { get; set; }

        public int TagId { get; set; }
        public int CourseId { get; set; }

        public Tags Tag { get; set; }

        public Courses Course { get; set; }

    }
}
