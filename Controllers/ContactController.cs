
using System;
using ecom.Data;
using ecom.Models;
using ecom.Models.ViewModels;
using ecom.Work.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecom.Controllers
{
	public class ContactController : Controller
    {
        //TODO: Make project use unit of work and repository pattern
        //private UnitOfWork _unitOfWork;
        private ContactUsManager _contactUsManager;


        public ContactController(ContactUsManager contactUsManager)
        {
            _contactUsManager = contactUsManager;
         
        }
        public IActionResult Index()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var message = new ContactMessage
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message
                };

                await _contactUsManager.CreateContactUsMessage(message);
             
                return RedirectToAction("Confirmation");
            }

            return View(model);
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}

