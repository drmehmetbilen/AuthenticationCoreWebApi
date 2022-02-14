using AuthenticationCoreWebApi.Models;

namespace AuthenticationCoreWebApi.Services
{
    public interface IBookService
    {
        public Book Create(Book book);
        public Book Get(int id);
        public List<Book> GetAll();

        public Book Update(Book book);
        public bool Delete(int id);
    }
}
