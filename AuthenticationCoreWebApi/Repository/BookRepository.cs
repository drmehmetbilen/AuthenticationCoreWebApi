using AuthenticationCoreWebApi.Models;

namespace AuthenticationCoreWebApi.Repository
{
    public class BookRepository
    {
        public static List<Book> Books = new()
        {
            new() { Id = 1, Title = "Kurt Kanunu", Author = "Kurt Kanunu", Rating=4 },
            new() { Id = 2, Title = "Savaşçı", Author = "Doğan Cüceloğlu", Rating=5 },
        };
    }
}
