using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using StackExchange.Redis;
using System.Security.Claims;

namespace BookStoreApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class BooksController : ControllerBase
    {
        private readonly IBookBL _bookService;

        public BooksController(IBookBL bookService)
        {
            _bookService = bookService;
        }

        [Authorize(Roles = "customer, admin")]
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetListOfAllBooks()
        {
            
            try
            {
                var claim = User.FindFirstValue(ClaimTypes.Role);
                Console.WriteLine(claim);
                var res = await _bookService.GetAllBooks();
                if (res == null)
                {
                    return Ok(new ResponseModel<BookEntity> { Status = 200, Message = "No Books Available!" });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetBookById")]
        [Authorize(Roles = "customer, admin")]
        public async Task<IActionResult> GetABookById(int BookId)
        {
            try
            {
                var res = await _bookService.GetBook(BookId);
                if (res == null)
                {
                    return Ok(new ResponseModel<BookEntity> { Status = 200, Message = "No Books Available!" });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddBook")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddNewBookToDB(BookModel NewBook)
        {
            try
            {
                var res = await _bookService.AddBook(NewBook);
                if (res == null)
                {
                    return BadRequest(new ResponseModel<BookEntity> { Status = 400, Message = "Unable to Add Book!" });
                }
                return Ok(new ResponseModel<BookEntity>
                {
                    Status = 201,
                    Message = "Book Added!",
                    Data = new BookEntity
                    {
                        Book_Id = res.Book_Id,
                        Title = res.Title,
                        Author = res.Author,
                        Genre = res.Genre,
                        Price = res.Price,
                        Image_Url = res.Image_Url

                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPatch("Update-Book")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBookById(int BookId, BookModel Model)
        {
            try
            {
                var res = await _bookService.UpdateBookDetails(BookId, Model);
                if (res == false)
                {
                    return BadRequest(new ResponseModel<BookEntity> { Status = 204, Message = "Book Not Updated!" });
                }
                return Ok(new ResponseModel<BookEntity> { Status = 201, Message = "Book Updated!" });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete-Book")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBookById(int BookId)
        {
            try
            {
                var res = await _bookService.DeleteBook(BookId);
                if (res == false)
                {
                    return BadRequest(new ResponseModel<BookEntity> { Status = 204, Message = "Book Not Deleted!" });
                }
                return Ok(new ResponseModel<BookEntity> { Status = 204, Message = "Book Deleted!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
