using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineExam.Models
{
    public class DB : DbContext
    {
        public DbSet<User> Users { get; set; }
      
        public DbSet<UserRole> Roles { get; set; }

        public DbSet<Programmes> Programme { get; set; }

        public DbSet<SubProgram> SubPrograms { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Chapter> Chapters { get; set; }

    }
}