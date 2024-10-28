using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Registration = AAC.Data.Entities.Registration;
namespace AAC.Core.Mappings
{
    public class RegistrationMapping : EntityTypeConfiguration<Registration>
    {
        public RegistrationMapping()
        {
            this.ToTable( "Registration" )
                .HasKey( x => x.ID );

            #region Map to AircraftType
            this.HasRequired( x => x.AircraftType )
                .WithMany( x => x.Registrations )
                .HasForeignKey( x => x.AircraftID );
            #endregion
        }
    }
}
