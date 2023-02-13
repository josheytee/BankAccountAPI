using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankAccountAPI.Entities
{
    [Table("Accounts")]
	public class Account
	{
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public double Balance { get; set; } = 0;
        public string GeneratedNumber { get; set; } = string.Empty;
        public string Phone { get; set; }

        public string Email { get; set; }
        public string Type { get; set; } = AccountType.SAVINGS.ToString();

        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Account()
		{
            FullName = $"{FirstName} {LastName}";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
	}

    public enum AccountType
    {
        SAVINGS, CURRENT 
    }
}

