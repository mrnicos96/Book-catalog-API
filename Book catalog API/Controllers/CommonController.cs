using Book_catalog_API.ApplicationContext;
using Book_catalog_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Book_catalog_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly DBContext context;

        public CommonController(DBContext dB) => context = dB;

        // GET / api / {authorName}
        [HttpGet("{authorName}")]
        public async Task<int> GetNumberOfBooksAsync(string authorName) => await DBContext.GetNumberOfBooksAsync(context, authorName);

        // POST / api
        [HttpPost]
        public async Task<ActionResult<Item>> CreateBookAsync(Item item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            await DBContext.CreateItemAsync(context, item);
            return Ok(item);
        }

        // PUT / api
        [HttpPut]
        public async Task<ActionResult<Book>> UpdateBookAsync(Item item)
        {
            if (item == null)
            {
                return BadRequest();
            }


            if (!DBContext.CheakAnyBookInDB(context, item.BookName))
            {
                return NotFound();
            }

            if (!DBContext.CheakAnyAuthorInDB(context, item.AuthorName))
            {
                return NotFound();
            }
            await DBContext.UpdateBookAsync(context, item);
            return Ok(item);
        }
    }
}
