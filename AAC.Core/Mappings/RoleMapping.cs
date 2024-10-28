using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

using Role = AAC.Data.Entities.Role;

namespace AAC.Core.Mappings
{
    public class RoleMapping : EntityTypeConfiguration<Role>
    {
        public RoleMapping()
        {
            this.ToTable( "Role" )
                .HasKey( x => x.ID );
        }
    }
}
