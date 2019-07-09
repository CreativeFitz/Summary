using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models.ViewModels
{
    public class CourseReviewViewModel
    {
        public Courses Course { get; set; }
        public List<Reviews> Reviews { get; set; }
    }
}
