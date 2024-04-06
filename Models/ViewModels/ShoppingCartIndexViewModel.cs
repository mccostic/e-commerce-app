using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Models.ViewModels
{
    public class ShoppingCartIndexViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        public decimal ShoppingCartTotal { get; set; }
        public string ReturnUrl { get; set; }
    }
}
