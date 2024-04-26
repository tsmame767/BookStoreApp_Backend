using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IBookBL
    {
        Task<BookEntity> AddBook(BookModel NewBook);
        Task<List<BookEntity>> GetAllBooks();
        Task<bool> UpdateBookDetails(int Book_Id, BookModel Model);
        Task<List<BookEntity>> GetBook(int BookId);
        Task<bool> DeleteBook(int BookId);
    }
}
