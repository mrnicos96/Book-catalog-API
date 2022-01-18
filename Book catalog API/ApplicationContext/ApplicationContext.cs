using Book_catalog_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_catalog_API.ApplicationContext
{
    public class DBContext : DbContext
    {
        private DbSet<Book> Books { get; set; } 
        private DbSet<Author> Authors { get; set; }
        private DbSet<BookАuthor> BooksАuthors { get; set; }

        public DBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=testdb;Username=postgres;Password=12345");
        }

        public static async Task<IEnumerable<Book>> GetBooksAsync(DBContext dB) => await dB.Books.ToListAsync();

        public static async Task<IEnumerable<Author>> GetAuthorsAsync(DBContext dB) => await dB.Authors.ToListAsync();

        public static async Task<int> GetNumberOfBooksAsync (DBContext dB, string authorName)
        {
            
            if (CheakAnyAuthorInDB (dB, authorName))
            {
                var author = await dB.Authors.FirstOrDefaultAsync(x => x.Name == authorName);
                var authorIdsList = await dB.BooksАuthors.Where(x => x.IdАuthor == author.Id).ToListAsync();
                return authorIdsList.Count;
            }
            else
            {
                return 0;
            }
        }

        public static async Task CreateItemAsync (DBContext dB, Item item)
        {
            Book book = new Book();
            book.Id = new Guid();
            book.Name = item.BookName;
            await dB.Books.AddRangeAsync(book);

            Author author = new Author();
            author.Id = new Guid();
            author.Name = item.AuthorName;
            await dB.Authors.AddRangeAsync(author);

            BookАuthor bookАuthor = new BookАuthor();
            bookАuthor.IdBook = book.Id;
            bookАuthor.IdАuthor = author.Id;
            await dB.BooksАuthors.AddRangeAsync(bookАuthor);

            await dB.SaveChangesAsync();
            if (!dB.Books.Any(x => x.Id == book.Id))
            {
                ;
            }
        }

        public static bool CheakAnyBookInDB(DBContext dB, string bookName) => dB.Books.Any(x => x.Name == bookName);

        public static bool CheakAnyAuthorInDB(DBContext dB, string authorName) => dB.Authors.Any(x => x.Name == authorName);

        public static async Task UpdateBookAsync (DBContext dB, Item item)
        {
            var existingBook = await dB.Books.FirstOrDefaultAsync(x => x.Name == item.BookName);
            existingBook.Name = item.BookName;
            dB.Books.Update(existingBook);
            await dB.SaveChangesAsync();

            if (!dB.Books.Any(x => x.Name == item.BookName))
            {
                ;
            }

            var existingAuthor = await dB.Authors.FirstOrDefaultAsync(x => x.Name == item.AuthorName);
            existingAuthor.Name = item.AuthorName;
            dB.Authors.Update(existingAuthor);

            await dB.SaveChangesAsync();
        }

        public static async Task  DeleteBookAsync (DBContext dB, Book book)
        {
            dB.Books.Remove(book);
            dB.SaveChanges();

            var idList = await dB.BooksАuthors.Where(x => x.IdBook == book.Id).ToListAsync();            
            if (idList == null)
                return;
            var author = new Author();
            foreach (var id in idList)
            {
                author = await dB.Authors.FirstOrDefaultAsync(x => x.Id == id.IdАuthor);
                dB.Authors.Remove(author);
                dB.SaveChanges();
            }

            dB.RemoveRange(idList);
            dB.SaveChanges();
        }

        public static async Task DeleteAuthorAsync(DBContext dB, Author author)
        {
            dB.Authors.Remove(author);
            dB.SaveChanges();

            var idList = await dB.BooksАuthors.Where(x => x.IdАuthor == author.Id).ToListAsync();
            if (idList == null)
                return;
            var book = new Book();
            foreach (var id in idList)
            {
                book = await dB.Books.FirstOrDefaultAsync(x => x.Id == id.IdBook);
                if (book != null)
                {
                    dB.Books.Remove(book);
                    dB.SaveChanges();
                }                
            }

            dB.RemoveRange(idList);
            dB.SaveChanges();
        }
    }
}
