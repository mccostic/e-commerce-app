using Microsoft.AspNetCore.Mvc;
using ecom.Models;
using ecom.Work.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly ProductManager _productManager;
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
