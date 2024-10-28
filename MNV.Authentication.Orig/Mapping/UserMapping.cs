﻿using MNV.Authentication.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Authentication.Mapping
{
    class UserMapping : EntityTypeConfiguration<ApplicationUser>
    {
        public UserMapping()
        {
            this.ToTable("[User]");
            this.HasKey(x => x.Id).Property(x => x.Id).HasColumnName("UserID");
            this.Property(x => x.UserName).HasColumnName("UserName");
        }
    }
}
