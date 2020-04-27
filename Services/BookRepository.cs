using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore_API.Contracts;
using BookStore_API.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore_API.Services
{
    public class BookRepository : BaseRepository, IBookRepository
    {
        public BookRepository(BookStoreContext db) : base(db)
        {
        }

        public async Task<bool> Create(Book entity)
        {
            await _db.Books.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            _db.Books.Remove(entity);
            return await Save();
        }

        public async Task<IList<Book>> FindAll()
        {
            return await _db.Books.ToListAsync();
        }

        public async Task<Book> FindById(long id)
        {
            return await _db.Books.FindAsync(id);
        }

        public async Task<bool> IsInDatabase(long id)
        {
            return await _db.Books.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Book entity)
        {
            _db.Books.Update(entity);
            return await Save();
        }
    }
}