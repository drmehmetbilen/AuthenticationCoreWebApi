using AuthenticationCoreWebApi.Models;

namespace AuthenticationCoreWebApi.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
