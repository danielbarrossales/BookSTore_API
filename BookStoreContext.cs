using Microsoft.EntityFrameworkCore;

namespace BookStore_API
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext (DbContextOptions<BookStoreContext> options) : base (options) { }
    }
}