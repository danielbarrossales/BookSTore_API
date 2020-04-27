using BookStore_API.Contracts;
using BookStore_API.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
    public class AuthorRepository : BaseRepository, IAuthorRepository
    { 
        
        public AuthorRepository(BookStoreContext db) : base (db)
        {
            
        }

        public async Task<bool> Create(Author entity)
        {
            await _db.Authors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            _db.Authors.Remove(entity);
            return await Save();
        }

        public async Task<IList<Author>> FindAll()
        {
            return await _db.Authors.ToListAsync();
        }

        public async Task<Author> FindById(long id)
        {
            return await _db.Authors.FindAsync(id);
        }

        public async Task<bool> IsInDatabase(long id)
        {
            return await _db.Authors.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Author entity)
        {
            _db.Authors.Update(entity);
            return await Save();
        }
    }
}
