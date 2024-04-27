using Dapper;
using ModelLayer.DTO;
using RepositoryLayer.Database;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class BookRL:IBookRL
    {
        private readonly DBContext _dBContext;

        public BookRL(DBContext dbContext)
        {
            _dBContext = dbContext;
        }

        public async Task<BookEntity> AddBook(BookModel NewBook)
        {
            var query = "insert into Book (Title, Author, Genre, Description, Price, Quantity, Image_Url) values (@Title, @Author, @Genre, @Description, @Price, @Quantity, @Image_Url)";
            using(var connect = _dBContext.CreateConnection())
            {
                var AddResult = await connect.ExecuteAsync(query,
                    new
                    {
                        Title = NewBook.Title,
                        Author = NewBook.Author,
                        Genre = NewBook.Genre,
                        Price = NewBook.Price,
                        Quantity = NewBook.Quantity,
                        Description = NewBook.Description,
                        Image_Url = NewBook.Image_Url
                    });
                if(AddResult == 0)
                {
                    throw new Exception("Book Not Inserted!");
                }

                var CurrentBookData = await connect.QueryFirstOrDefaultAsync<BookEntity>("select Top 1 * from book order by book_id desc");
                if (CurrentBookData==null)
                {
                    return null;
                }
                return CurrentBookData;
            }
        }

        public async Task<List<BookEntity>> GetAllBooks()
        {
            var query = "select * from book";

            using(var connect = _dBContext.CreateConnection())
            {
                var ListOfBooks = await connect.QueryAsync<BookEntity>(query);
                if (ListOfBooks == null)
                {
                    return null;
                }
                return ListOfBooks.ToList();
            }
        }

        public async Task<List<BookEntity>> GetBook(int BookId)
        {
            var query = "select * from book where book_id=@bookId";

            using (var connect = _dBContext.CreateConnection())
            {
                var ListOfBooks = await connect.QueryAsync<BookEntity>(query, new {bookId = BookId});
                if (ListOfBooks == null)
                {
                    return null;
                }
                return ListOfBooks.ToList();
            }
        }

        public async Task<bool> UpdateBookDetails(int Book_Id, BookModel Model)
        {
            var UpdateBook = 0;
            var query = "update book set Title=@Title, Author=@Author, Genre=@Genre, Description = @Description, Price=@Price, Quantity = @Quantity, Image_Url=@Image_Url where book_id=@BookId";

            using (var connect = _dBContext.CreateConnection())
            {
                UpdateBook = await connect.ExecuteAsync(query, new
                {
                    BookId = Book_Id,
                    Title = Model.Title,
                    Author = Model.Author,
                    Genre = Model.Genre,
                    Price = Model.Price,
                    Description = Model.Description,
                    Quantity = Model.Quantity,
                    Image_Url = Model.Image_Url
                });

                if(UpdateBook < 1)
                {
                    return false;
                }
            }
            return true;

        }

        public async Task<bool> DeleteBook(int BookId)
        {
            var DeleteBook = 0;
            var query = "delete from book where book_id=@bookId";

            using(var connect = _dBContext.CreateConnection())
            {
                DeleteBook = await connect.ExecuteAsync(query, new { bookId = BookId });
                if(DeleteBook < 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
