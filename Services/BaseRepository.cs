using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore_API.Contracts;
using BookStore_API.Database.Entities;

namespace BookStore_API.Services
{
    public abstract class BaseRepository 
    {
        protected readonly BookStoreContext _db;

        protected BaseRepository(BookStoreContext db)
        {
            _db = db;
        }
    }
}
