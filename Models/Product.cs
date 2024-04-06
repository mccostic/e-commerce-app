using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public string LongDescription { get; set; }
        [Range(1, 500)]
        public decimal Price { get; set; }

        [Required]
        public byte[] PreviewIage { get; set; }

        public int inStock { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
