using Microsoft.EntityFrameworkCore;
using Tweetbook.Domain;
using Tweetbook.Migrations;

namespace Tweetbook.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly DataContext _dataContext;
        public IdentityService(DataContext dataContext)
        {
            _dataContext = dataContext;


            if (!_dataContext.Users.Any()) return;
            
            
            _dataContext.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "admin",
                Password = "admin",

            });

            _dataContext.SaveChanges();
        }


        public async Task<bool> RegisterAsync(User user)
        {
            await _dataContext.AddAsync(user);

            var result = await _dataContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var userFound = await _dataContext.Users.SingleOrDefaultAsync(x =>
                     x.Email == email &&
                     x.Password == password);

            return userFound != null;
        }
    }
}
