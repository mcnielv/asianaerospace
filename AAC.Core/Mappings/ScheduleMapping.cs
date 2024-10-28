using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Schedule = AAC.Data.Entities.Schedule;

namespace AAC.Core.Mappings
{
    public class ScheduleMapping : EntityTypeConfiguration<Schedule>
    {
        public ScheduleMapping()
        {
            this.ToTable( "Schedule" )
                .HasKey( x => x.ID );

            #region RouteStart(Destination)
            this.HasRequired( x => x.RouteStart )
                .WithMany( x => x.ScheduleStarts )
                .HasForeignKey( x => x.RouteStartID );
            #endregion

            #region RouteDestination(Destination)
            this.HasRequired( x => x.RouteDestination )
                .WithMany( x => x.ScheduleDestinations )
                .HasForeignKey( x => x.RouteDestinationID );
            #endregion

            #region RouteEnd(Destination)
            this.HasRequired( x => x.RouteEnd )
                .WithMany( x => x.ScheduleEnds )
                .HasForeignKey( x => x.RouteEndID );
            #endregion

            #region Pilot(User)
            this.HasRequired( x => x.Pilot )
                .WithMany( x => x.PilotSchedules )
                .HasForeignKey( x => x.PilotID );
            #endregion

            #region CoPilot(User)
            this.HasRequired( x => x.AssistantPilot )
                .WithMany( x => x.AssistantPilotSchedules )
                .HasForeignKey( x => x.AssistantPilotID );
            #endregion

            #region Regsitration
            this.HasRequired( x => x.Registration )
                .WithMany( x => x.Schedules )
                .HasForeignKey( x => x.RegistrationID );
            #endregion

            #region AircraftType
            this.HasRequired( x => x.AircraftType )
                .WithMany( x => x.Schedules )
                .HasForeignKey( x => x.AircraftID );
            #endregion
        }
    }
}
