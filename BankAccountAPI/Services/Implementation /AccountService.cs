using System;
using System.Text;
using BankAccountAPI.Data;
using BankAccountAPI.Entities;
using BankAccountAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BankAccountAPI.Services.Implementation
{
	public class AccountService: IAccountService
	{
        private readonly DataContext _dbContext;

         public AccountService(DataContext dbContext)
         {
            _dbContext = dbContext;
         }

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }

            return true;
        }

        private static void CreatePinHash(string Pin, out byte[] pinHash, out byte[] pinSalt)
        {
            //checks Pin
            if (string.IsNullOrEmpty(Pin)) throw new ArgumentNullException("Pin");
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
            }
        }


        public Account Authenticate(string AccountNumber, string Pin)
        {
            if (string.IsNullOrEmpty(AccountNumber) || string.IsNullOrEmpty(Pin))
                throw new ApplicationException("Account number or Pin can not be empty");

            var account = _dbContext.Accounts.FirstOrDefault(x => x.GeneratedNumber == AccountNumber);
            //is account null
            if (account is null)
                throw new ApplicationException("Invalid Auth details");

            //so user exists,

            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
                throw new ApplicationException("Invalid Pin provided");

            //auth successful
            return account;
            
        }

        public Account CreateAccount(Account account, string Pin, string ConfirmPin)
        {
            
            //validate
            //if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin cannot be empty");
            //does a user with this email exist already?
            if (_dbContext.Accounts.Any(x => x.Email.Equals(account.Email) )) throw new ApplicationException("A user with this email exists");
            //is Pin eequal to confirmmpin
            if (!Pin.Equals(ConfirmPin)) throw new ApplicationException("Pins do not match.");

            account.FullName = $"{account.FirstName} {account.LastName}";
            account.GeneratedNumber = GetUniqueAccountNumber();

            //if validation passes
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);

            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();

            return account;
         
        }

        private string GetUniqueAccountNumber()
        {
            Account account;
            string accountNumber;
            do
            {
                accountNumber = GenerateAccountNumberLogic();
                account = GetAccountByAccountNumber(accountNumber, true);
                
            } while (account != null);

            return accountNumber;
        }

        private string GenerateAccountNumberLogic()
        {
            string value = "1234567890";
            return new string(Enumerable.Repeat(value, 10).Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public Account GetAccountByAccountNumber(string AccountNumber, bool isEscapeException = false)
        {
            var account = _dbContext.Accounts.Where(x => x.GeneratedNumber == AccountNumber).SingleOrDefault();

            if (!isEscapeException)
            {
                if (account is null) throw new ApplicationException("Account Not Found");

            }

            return account;            
        }

    }
}

