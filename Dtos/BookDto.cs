using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Dtos
{
    public class BookDto
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public uint? Year { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public long AuthorId { get; set; }
    }

    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }
        public uint? Year { get; set; }
        [Required]
        public string ISBN { get; set; }
        [StringLength(500)]
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        [Required]
        public long AuthorId { get; set; }
    }

    public class UpdateBookDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public uint? Year { get; set; }
        [StringLength(500)]
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
    }
}
