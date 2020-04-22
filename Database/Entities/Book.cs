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
        public uint? Year { get; set; }

        [Required]
        [MaxLength(50)]
        public string ISBN { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        [MaxLength(150)]
        public string Image { get; set; }

        [Required]
        [Column(TypeName="money")]
        public decimal? Price { get; set; }
        
        [Required]
        public Author Author { get; set; }
    }
}
