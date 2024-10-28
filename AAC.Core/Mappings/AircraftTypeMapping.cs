using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

using AircraftType = AAC.Data.Entities.AircraftType;
namespace AAC.Core.Mappings
{
    public class AircraftTypeMapping : EntityTypeConfiguration<AircraftType>
    {
        public AircraftTypeMapping()
        {
            this.ToTable( "AircraftType" )
                .HasKey( x => x.ID );
        }
    }
}
