using System;
using System.ComponentModel.DataAnnotations;
using BankAccountAPI.Entities;

namespace BankAccountAPI.Models
{
	public class GetBalanceModel
	{
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string Pin { get; set; } 
	}
}



