using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nucSummary.Models
{
    public class Tags
    {
        public int Id { get; set; }
        [Required]
        public string Tag { get; set; }
        
        public List<CourseTags> CourseTags { get; set; }
    }
}
