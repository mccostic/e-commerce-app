using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ecom.Models;
using ecom.Models.ViewModels;
using ecom.Work.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace ecom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CategoryManager _categoryManager;
        private ProductManager _productManager;
        private ContactUsManager _contactUsManager;
        private readonly UserManager<ApplicationUser> _userManager; // UserManager service

        public HomeController(ILogger<HomeController> logger, 
            CategoryManager categoryManager,
            ProductManager productManager,
            ContactUsManager contactUsManager,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _categoryManager = categoryManager;
            _contactUsManager = contactUsManager;
            _productManager = productManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                Categories = await _categoryManager.GetCategoriesForHomePage(),
                Products =await _productManager.GetProductList()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserManagement()
        {
            var allUsers = _userManager.Users.ToList();
            var adminUsers = new List<ApplicationUser>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    adminUsers.Add(user);
                }
            }

            var nonAdminUsers = allUsers.Except(adminUsers).ToList(); // Exclude admin users

            return View(nonAdminUsers); // Pass only non-admin users to the view
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    // Optionally, add the user to a role, log the creation, send a confirmation email, etc.

                    return RedirectToAction(nameof(UserManagement));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(UserManagement));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("DeleteUser", user); // Return to the confirmation view if deletion failed
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ContactMessages()
        {
            var messages = await _contactUsManager.GetContactMessageList();
            return View(messages);
        }


        public async Task<IActionResult> Products(int? id)
        {
            var model = new ProductsViewModel();
            if(id != null)
            {
                Category categ = await _categoryManager.GetCategoryById((int)id);
                model.Category = categ;
                model.Products = categ.Products.ToList();
            }
            else
            {
                model.Category = null;
                model.Products = await _productManager.GetProductList();
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
