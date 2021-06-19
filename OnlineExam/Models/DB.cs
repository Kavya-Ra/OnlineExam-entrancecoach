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

        public System.Data.Entity.DbSet<OnlineExam.ViewModel.ClassViewModel> ClassViewModels { get; set; }

        public System.Data.Entity.DbSet<OnlineExam.ViewModel.ProgrammesViewModel> ProgrammesViewModels { get; set; }
    }
}