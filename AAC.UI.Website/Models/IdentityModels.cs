using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
namespace AAC.UI.Website.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IUser<int> //IdentityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int RoleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<ApplicationUser,int> manager )
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync( this, DefaultAuthenticationTypes.ApplicationCookie );
            // Add custom user claims here
            //userIdentity.AddClaim( new Claim( "RoleID", RoleID.ToString() ) );
            //userIdentity.AddClaim( new Claim( "FirstName", this.FirstName ) );
            //userIdentity.AddClaim( new Claim( "LastName", this.LastName ) );
            return userIdentity;
        }
        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    //userIdentity.AddClaim( new Claim( "UserId", this.UserId.ToString() ) );
        //    //userIdentity.AddClaim( new Claim( "FullName", this.FullName ) );
        //    return userIdentity;
        //}
    }

    public class ApplicationDbContext : DbContext // IdentityDbContext<ApplicationUser>
    {
        //public ApplicationDbContext()
        //    : base("DefaultConnection", throwIfV1Schema: false)
        //{
        //}
        public ApplicationDbContext() : base( "DefaultConnection" ){ }
        public ApplicationDbContext( string connectionString ) : base( connectionString ) { }
        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Configurations.Add( new UserMapping() );
            base.OnModelCreating( modelBuilder );
        }
        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
    
    public class UserMapping : EntityTypeConfiguration<ApplicationUser>
    {
        public UserMapping()
        {
            this.ToTable( "[User]" );
            this.HasKey( x => x.Id ).Property( x => x.Id ).HasColumnName( "ID" );
            this.Property( x => x.UserName ).HasColumnName( "Username" );
        }
    }

    public class ApplicationUserStore : IUserStore<ApplicationUser, int>, IUserPasswordStore<ApplicationUser, int>
    {
        private ApplicationDbContext _dbContext;

        public ApplicationUserStore()
        {
            _dbContext = new ApplicationDbContext();
        }

        public Task UpdateAsync( ApplicationUser user )
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public Task CreateAsync( ApplicationUser user )
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync( ApplicationUser user )
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync( int userId )
        {
            ApplicationUser user = await _dbContext.ApplicationUsers
                .Where( x => x.Id == userId )
                .FirstOrDefaultAsync();
            return user;
        }
        //public async Task<ApplicationUser> FindAsync( string userName, string password )
        //{
        //    ApplicationUser user = await _dbContext.ApplicationUsers
        //        .Where( x => x.UserName == userName && x.Password==password )
        //        .FirstOrDefaultAsync();
        //    return user;
        //}
        public async Task<ApplicationUser> FindByNameAsync( string userName )
        {
            ApplicationUser user = await _dbContext.ApplicationUsers
                .Where( x => x.UserName == userName )
                .FirstOrDefaultAsync();
            return user;
        }

        public Task<string> GetPasswordHashAsync( ApplicationUser user )
        {
            if ( user == null )
            {
                throw new ArgumentNullException( "user is null" );
            }
            PasswordHasher hasher = new PasswordHasher();
            return Task.FromResult( hasher.HashPassword( user.Password ) );
        }

        public Task<bool> HasPasswordAsync( ApplicationUser user )
        {
            return Task.FromResult( user.Password != null );
        }

        public Task SetPasswordHashAsync( ApplicationUser user, string passwordHash )
        {
            throw new NotImplementedException();
        }
    }

}