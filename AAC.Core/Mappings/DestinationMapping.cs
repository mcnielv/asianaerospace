using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Destination = AAC.Data.Entities.Destination;

namespace AAC.Core.Mappings
{
    public class DestinationMapping : EntityTypeConfiguration<Destination>
    {
        public DestinationMapping()
        {
            this.ToTable( "Destination" )
                .HasKey(x=>x.ID);
        }
    }
}
