using System;
using System.ComponentModel.DataAnnotations;
using BankAccountAPI.Entities;

namespace BankAccountAPI.Models
{
	public class MakeTransferModel
	{
        //[Required]
        public string FromAccount { get; set; }

        //[Required]
        public string ToAccount { get; set; }

        //[Required]
        public double Amount { get; set; }

        //[Required]
        public string Pin { get; set; } 
	}
}



