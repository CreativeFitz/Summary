using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models.ViewModels
{
    public class CourseReviewViewModel
    {
        
        public Courses Course { get; set; }

        public decimal OverallAverage { get; set; }

        public decimal DifficultyAverage { get; set; }

        public decimal RelevancyAverage { get; set; }

        
    }
}
