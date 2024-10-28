using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

using Logs = AAC.Data.Entities.Logs;
namespace AAC.Core.Mappings
{
    public class LogsMapping : EntityTypeConfiguration<Logs>
    {
        public LogsMapping()
        {
            this.ToTable( "Logs" )
                .HasKey( x => x.ID );
        }
    }
}
