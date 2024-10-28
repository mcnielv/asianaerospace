using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

using AAC.Core.Mappings;
using AAC.Framework.ICore;

namespace AAC.Core
{
    public class AAContext : DbContext, IDBContext
    {
        #region Constructor
        public AAContext() : base( "name=Aac.Connection" ) { }
        #endregion

        #region Method(s)
        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Configurations.Add( new UserMapping() );
            modelBuilder.Configurations.Add( new RoleMapping() );
            modelBuilder.Configurations.Add( new DestinationMapping() );
            modelBuilder.Configurations.Add( new AircraftTypeMapping() );
            modelBuilder.Configurations.Add( new FlightMapping() );
            modelBuilder.Configurations.Add( new FlightDetailMapping() );
            modelBuilder.Configurations.Add( new RegistrationMapping() );
            modelBuilder.Configurations.Add( new ScheduleMapping() );
            modelBuilder.Configurations.Add( new ScheduleCrewMapping() );
            modelBuilder.Configurations.Add( new LogsMapping() );
            base.OnModelCreating( modelBuilder );
        }
        #endregion

        #region DbContext

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this.Set<T>();
        }

        public T Add<T>( T item ) where T : class
        {
            this.Set<T>().Add( item );
            return item;
        }

        public T Remove<T>( T item ) where T : class
        {
            this.Set<T>().Remove( item );
            return item;
        }
        
        public T Update<T>( T item ) where T : class
        {
            var entry = this.Entry( item );

            if ( entry != null )
            {
                entry.CurrentValues.SetValues( item );
            }
            else
            {
                this.Attach( item );
            }

            return item;
        }

        public T Attach<T>( T item ) where T : class
        {
            this.Set<T>().Attach( item );
            return item;
        }

        public T Detach<T>( T item ) where T : class
        {
            this.Entry( item ).State = EntityState.Detached;
            return item;
        }
        
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public void ExecuteTSQL( string tsql )
        {
            base.Database.ExecuteSqlCommand( tsql );
            base.SaveChanges();
        }

        public T GetColumn<T>( string tsql )
        {
            return base.Database.SqlQuery<T>( tsql ).FirstOrDefault<T>();
        }
        
        public List<T> GetList<T>( string tsql ) where T : class
        {
            return base.Database.SqlQuery<T>( tsql ).ToList();
        }
        #endregion
    }
}
