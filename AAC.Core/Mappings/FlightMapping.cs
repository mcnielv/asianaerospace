using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

using Flight = AAC.Data.Entities.Flight;
using User = AAC.Data.Entities.User;
using AircraftType = AAC.Data.Entities.AircraftType;

namespace AAC.Core.Mappings
{
    public class FlightMapping : EntityTypeConfiguration<Flight>
    {
        public FlightMapping()
        {
            this.ToTable( "Flight" )
                .HasKey( x => x.ID );

            #region AircraftType
            this.HasRequired( x => x.AircraftType )
                .WithMany( x => x.Flights )
                .HasForeignKey( x => x.AircraftTypeID );
            #endregion

            #region Pilot
            this.HasRequired( x => x.Pilot )
                .WithMany( x => x.PilotFlights )
                .HasForeignKey( x => x.PilotID );
            #endregion

            #region CoPilot
            this.HasRequired( x => x.CoPilot )
                .WithMany( x => x.CoPilotFlights )
                .HasForeignKey( x => x.CoPilotID );
            #endregion

            #region Prepared by
            this.HasOptional( x => x.PreparedBy )
                .WithMany( x => x.PreparedByFlights )
                .HasForeignKey( x => x.PreparedByID );
            #endregion

            #region Reviewed by
            this.HasOptional( x => x.ReviewedBy )
                .WithMany( x => x.ReviewedByFlights )
                .HasForeignKey( x => x.ReviewedByID );
            #endregion

            #region Checked by
            this.HasOptional( x => x.Checkedby )
                .WithMany( x => x.CheckedByFlights )
                .HasForeignKey( x => x.CheckedByID );
            #endregion

            #region Noted by
            this.HasOptional( x => x.NotedBy )
                .WithMany( x => x.NotedByFlights )
                .HasForeignKey( x => x.NotedByID );
            #endregion

            //this.HasRequired( x => x.FlightDetails );
        }
    }
}
