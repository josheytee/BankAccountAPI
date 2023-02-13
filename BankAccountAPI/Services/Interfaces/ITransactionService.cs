using System;
using BankAccountAPI.Entities;
namespace BankAccountAPI.Services.Interfaces
{
	public interface ITransactionService
	{
        Response<Transaction> Deposit(string AccountNumber, double Amount, string TransactionPin);

        Response<Transaction> Transfer(string FromAccount, string ToAccount, double Amount, string TransactionPin);

    }
}

