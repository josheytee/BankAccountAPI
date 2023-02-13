using System;
using System.ComponentModel.DataAnnotations;
using BankAccountAPI.Entities;

namespace BankAccountAPI.Models
{
	public class CreateAccountModel
	{

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //cummulative
        [Required]
        [RegularExpression(@"^[0-9]{4}$")]
        public string Pin { get; set; }

        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }
	}
}



