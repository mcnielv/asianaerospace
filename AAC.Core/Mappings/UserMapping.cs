using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using User = AAC.Data.Entities.User;

namespace AAC.Core.Mappings
{
    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            this.ToTable( "User" )
                .HasKey( x => x.ID );

            #region Map Role
            this.HasRequired( x => x.Role )
                .WithMany( x => x.Users )
                .HasForeignKey( x => x.RoleID );
            #endregion          
        }
    }
}
