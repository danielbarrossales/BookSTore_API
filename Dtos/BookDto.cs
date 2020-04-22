using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Dtos
{
    public class BookDto
    {
        public long Id { get; set; }
        public uint? Year { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public AuthorDto Author { get; set; }
    }
}
