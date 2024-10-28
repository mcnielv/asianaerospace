using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

using FlightDetail = AAC.Data.Entities.FlightDetail;

namespace AAC.Core.Mappings
{
    public class FlightDetailMapping : EntityTypeConfiguration<FlightDetail>
    {
        public FlightDetailMapping()
        {
            this.ToTable( "FlightDetail" )
                .HasKey( x => x.ID );

            #region Flight
            this.HasRequired( x => x.Flight )
                .WithMany( x => x.FlightDetails )
                .HasForeignKey( x => x.FlightID );
            #endregion

            #region From
            this.HasRequired( x => x.From )
                .WithMany( x => x.FlightDetailsFrom )
                .HasForeignKey( x => x.FromID );
            #endregion

            #region To
            this.HasRequired( x => x.To )
                .WithMany( x => x.FlightDetailsTo )
                .HasForeignKey( x => x.ToID );
            #endregion
        }
    }
}
