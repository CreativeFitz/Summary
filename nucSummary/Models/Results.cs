using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models
{
    public class Results
    {
        public int Id { get; set; }

        public int AssessmentId { get; set; }

        public int StudentId { get; set; }

        [Display(Name = "Raw Score")]
        public int ScoreRaw { get; set; }

        [Display(Name = "Status")]
        public string LessonStatus { get; set; }

        [Display(Name = "Time Taken")]
        public int SessionTime { get; set; }

        public Assessments Assessment { get; set; }
        public Students Student { get; set; }
        

        

    }
}
