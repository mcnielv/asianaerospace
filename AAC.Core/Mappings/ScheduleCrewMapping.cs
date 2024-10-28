using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScheduleCrew = AAC.Data.Entities.ScheduleCrew;
namespace AAC.Core.Mappings
{
    public class ScheduleCrewMapping : EntityTypeConfiguration<ScheduleCrew>
    {
        public ScheduleCrewMapping()
        {
            this.ToTable( "ScheduleCrew" )
                .HasKey( x => x.ID );

            #region SChedule(Schedule)
            this.HasRequired( x => x.Schedule )
                .WithMany( x => x.ScheduleCrews )
                .HasForeignKey( x => x.ScheduleID );
            #endregion

            #region Crew(User)
            this.HasRequired( x => x.Crew )
                .WithMany( x => x.Crews )
                .HasForeignKey( x => x.CrewID );
            #endregion
        }
    }
}
