using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MNV.Infrastructure.Authentication.Model
{
    public class ApplicationUserStore : IUserStore<ApplicationUser, int>, IUserPasswordStore<ApplicationUser, int>
    {
        private AdoptAClassroomIdentityDbContext _dbContext;

        public ApplicationUserStore()
        {
            _dbContext = new AdoptAClassroomIdentityDbContext();
        }
        
        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            ApplicationUser user = await _dbContext.ApplicationUsers
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            ApplicationUser user = await _dbContext.ApplicationUsers
                .Where(x => x.UserName == userName)
                .FirstOrDefaultAsync();
            return user;
        }        

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user is null");
            }
            PasswordHasher hasher = new PasswordHasher();
            return Task.FromResult(hasher.HashPassword(user.Password));
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Password != null);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
