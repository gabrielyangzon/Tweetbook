using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public interface IIdentityService
    {
        Task<bool> RegisterAsync(User user);

        Task<bool> LoginAsync(string email,string password);
    }
}
