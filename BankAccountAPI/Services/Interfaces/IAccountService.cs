using System;
using BankAccountAPI.Entities;

namespace BankAccountAPI.Services.Interfaces
{
	public interface IAccountService
	{
		Account Authenticate(string AccountNumber, string Pin);
		Account CreateAccount(Account account, string pin, string ConfirmPin);
		Account GetAccountByAccountNumber(string AccountNumber, bool isEscapeException = false);
	}
}

