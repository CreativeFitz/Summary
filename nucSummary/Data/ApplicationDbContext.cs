using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using nucSummary.Models;

namespace nucSummary.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Assessments> Assessments { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<CourseTags> CourseTags { get; set; }
        public DbSet<Results> Results { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<StudentCourses> StudentCourses { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Tags> Tags { get; set; }

    }
}