using Book_catalog_API.ApplicationContext;
using Book_catalog_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Book_catalog_API.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DBContext context;

        public BooksController(DBContext dB) => context = dB;

        // GET / books
        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await DBContext.GetBooksAsync(context);
        }

        // PUT / books
        [HttpPut]
        public async Task<ActionResult<Author>> UpdateAsync(Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }


            if (DBContext.CheakAnyBookInDB(context, book.Name))
            {
                return BadRequest();
            }

            if (!DBContext.CheakAnyBookInDB(context, book))
            {
                return NotFound();
            }
            await DBContext.UpdateBookAsync(context, book);

            return Ok(book);
        }

        // DELETE / book 
        [HttpDelete]
        public async Task<ActionResult<Book>> DeleteBookAsync(Book book)
        {
            if (!DBContext.CheakAnyBookInDB(context, book.Name))
            {
                return NotFound();
            }
            else
            {
                await DBContext.DeleteBookAsync(context, book);
                return Ok(book);
            }
        }
    }
}
