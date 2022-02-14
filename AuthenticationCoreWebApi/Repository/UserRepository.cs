using AuthenticationCoreWebApi.Models;

namespace AuthenticationCoreWebApi.Repository
{
    public class UserRepository
    {

        public static List<User> Users = new()
        {
            new() { UserName = "mbilen", EmailAdress = "mb@mb.com", GivenName = "mehmet", SurName = "bilen", Password = "1234", Role = "admin" },
            new() { UserName = "iuysal", EmailAdress = "iu@mb.com", GivenName = "ilhan", SurName = "uysal", Password = "4321", Role = "standart" }
        };
    }
}
