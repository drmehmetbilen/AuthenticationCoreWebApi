using AuthenticationCoreWebApi.Models;
using AuthenticationCoreWebApi.Repository;

namespace AuthenticationCoreWebApi.Services
{
    public class BookService : IBookService
    {
        public Book Create(Book book)
        {
            book.Id = BookRepository.Books.Count + 1;
            BookRepository.Books.Add(book);
            return book;
        }

        public bool Delete(int id)
        {

            var oldBook = Get(id);
            if (oldBook != null)
            {
                BookRepository.Books.Remove(oldBook);
                return true;
            }
            return false;

        }

        public Book Get(int id)
        {
            var book = BookRepository.Books.FirstOrDefault(s => s.Id == id);
            if (book == null)
            {
                return null;
            }
            return book;
        }

        public List<Book> GetAll()
        {
            var books = BookRepository.Books;
            return books;
        }

        public Book Update(Book book)
        {
            var oldBook = Get(book.Id);
            if (oldBook != null)
            {
                oldBook.Rating = book.Rating;
                oldBook.Title = book.Title;
                oldBook.Author = book.Author;
                return oldBook;

            }
            return null;
        }
    }
}
