using MNV.Infrastructure.Authentication.Mapping;
using MNV.Infrastructure.Authentication.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Authentication
{
    public class AdoptAClassroomIdentityDbContext : DbContext
    {
        private const string _connectionString = "data source=test.adoptaclassroomtest.com;initial catalog=kleotest;persist security info=True;user id=webtest;password=VctEYDwW;multipleactiveresultsets=True;";

        public AdoptAClassroomIdentityDbContext() : base("IdentityDb") { }
        public AdoptAClassroomIdentityDbContext(string connectionString)
            : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMapping());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
