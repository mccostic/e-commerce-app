using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Models.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public byte[] CategoryImage { get; set; }
        public int ProductCount { get; set; }
    }
}
