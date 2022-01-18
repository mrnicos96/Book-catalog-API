﻿using Book_catalog_API.ApplicationContext;
using Book_catalog_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_catalog_API.Controllers
{
    [Route("authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly DBContext context;

        public AuthorController(DBContext dB) => context = dB;

        // GET / authors
        [HttpGet]
        public async Task<IEnumerable<Author>> GetAuthorAsync()
        {
            return await DBContext.GetAuthorsAsync(context);
        }

        // DELETE / authors 
        [HttpDelete]
        public async Task<ActionResult<Author>> DeleteAuthorAsync(Author author)
        {
            if (DBContext.CheakAnyAuthorInDB(context, author.Name))
            {
                return NotFound();
            }
            else
            {
                await DBContext.DeleteAuthorAsync(context, author);
                return Ok(author);
            }
        }
    }
}
