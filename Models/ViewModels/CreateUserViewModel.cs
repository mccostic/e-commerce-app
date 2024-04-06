using System;
using System.ComponentModel.DataAnnotations;

namespace ecom.Models.ViewModels
{
	public class CreateUserViewModel
	{
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

