using MNV.Infrastructure.Authentication.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Authentication.Mapping
{
    class UserMapping : EntityTypeConfiguration<ApplicationUser>
    {
        public UserMapping()
        {
            this.ToTable("[User]");
            this.HasKey(x => x.Id).Property(x => x.Id).HasColumnName("ID");
            this.Property(x => x.UserName).HasColumnName("Username");
        }
    }
}
