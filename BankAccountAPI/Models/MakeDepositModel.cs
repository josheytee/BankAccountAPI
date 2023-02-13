using System;
using System.ComponentModel.DataAnnotations;
using BankAccountAPI.Entities;

namespace BankAccountAPI.Models
{
	public class MakeDepositModel
	{
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Pin { get; set; }

    }
}



