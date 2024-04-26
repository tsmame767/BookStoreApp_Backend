using BusinessLayer.Interfaces;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class BookBL:IBookBL
    {
        private readonly IBookRL _bookService;

        public BookBL(IBookRL bookService)
        {
            _bookService = bookService;
        }

        public Task<BookEntity> AddBook(BookModel NewBook)
        {
            return _bookService.AddBook(NewBook);
        }
        
        public Task<List<BookEntity>> GetAllBooks()
        {
            return _bookService.GetAllBooks();
        }

        public Task<bool> UpdateBookDetails(int Book_Id, BookModel Model)
        {
            return _bookService.UpdateBookDetails(Book_Id, Model);
        }

        public Task<List<BookEntity>> GetBook(int BookId)
        {
            return _bookService.GetBook(BookId);
        }
        public Task<bool> DeleteBook(int BookId)
        {
            return _bookService.DeleteBook(BookId);
        }
    }
}
