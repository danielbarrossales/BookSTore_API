using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Database.Entities
{
    [Table("Books")]
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        public uint? Year { get; set; }

        [Required]
        [MaxLength(50)]
        public string ISBN { get; set; }

        [MaxLength(500)]
        public string Summary { get; set; }

        [MaxLength(150)]
        public string Image { get; set; }

        [Column(TypeName="money")]
        public decimal? Price { get; set; }
        
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
        [Required]
        public long AuthorId { get; set; }
    }
}
