using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankAccountAPI.Entities
{
    [Table("Transactions")]
	public class Transaction
	{
        [Key]
        public int Id { get; set; }
        public string Reference { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; } = TransactionStatus.FAILED.ToString();

        public bool IsSuccessful => Status.Equals(TransactionStatus.SUCCESSFUL.ToString());

        public string? SourceAccount { get; set; } = string.Empty;

        public string DestinationAccount { get; set; } 
        public string Particulars { get; set; }
        public string Type { get; set; } = TransactionType.DEPOSIT.ToString();
        public DateTime CreatedAt { get; set; }


        public Transaction()
		{
            Reference = $"{Guid.NewGuid().ToString().Replace("-","").Substring(1,27)}";
		}
	}

    public enum TransactionStatus
    {
        SUCCESSFUL,
        FAILED
    }

    public enum TransactionType
    {
        DEPOSIT,
        TRANSFER
    }
}

