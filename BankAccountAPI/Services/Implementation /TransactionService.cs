using System;
using BankAccountAPI.Data;
using BankAccountAPI.Entities;
using BankAccountAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Dapper.SqlMapper;
using Newtonsoft.Json;

namespace BankAccountAPI.Services.Implementation
{
	public class TransactionService: ITransactionService
	{
        private readonly IAccountService _accountService;
        private readonly DataContext _dbContext;

        public TransactionService(IAccountService accountService, DataContext dbContext)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }        

        public Response<Transaction> Deposit(string AccountNumber, double Amount, string TransactionPin)
        {
            if (Amount <= 0) throw new ApplicationException("Amount be have a value");
            
            Response<Transaction> response = new();
            Account destinationAccount; //individual

            destinationAccount =  _accountService.GetAccountByAccountNumber(AccountNumber);
            destinationAccount.Balance += Amount;


            Transaction transaction = new Transaction();

            var authenticateUser =  _accountService.Authenticate(AccountNumber, TransactionPin);

            transaction.CreatedAt = DateTime.Now;
            transaction.Type = TransactionType.DEPOSIT.ToString();
            transaction.Amount = Amount;
            transaction.SourceAccount = null;
            transaction.DestinationAccount = AccountNumber;
            transaction.Status = TransactionStatus.SUCCESSFUL.ToString();
            transaction.Particulars = $"NEW Deposit Transaction has been made to " +
                $"=> {JsonConvert.SerializeObject(transaction.DestinationAccount)} ON" +
                $" {transaction.CreatedAt} TRAN_TYPE =>  {transaction.Type} " +
                $"TRAN_STATUS => {transaction.Status}";


            //sso there was an update
            response.Code = "00";
            response.Message = "Transaction Successful!";
            response.Data = transaction;

          
            _dbContext.Accounts.Update(destinationAccount);
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();


            return response;

        }
      
      
       
        public Response<Transaction> Transfer(string FromAccount, string ToAccount, double Amount, string TransactionPin)
        {
            //3 accounts or 2 are involved
            if (FromAccount.Equals(ToAccount)) throw new ApplicationException("You cannot transfer money to yourself");

            //FromAccount is our current user/customer's account and we'll authenticate with it...
            var response = new Response<Transaction>();
            Account destinationAccount; //target account where money is being sent to...
            Transaction transaction = new Transaction();

            //let's authenticate first
            var authenticatedAccount = _accountService.Authenticate(FromAccount, TransactionPin);
            
            //user authenticated, then let's process funds transfer;
            
            //sourceAccount = _accountService.GetAccountByAccountNumber(FromAccount);
            destinationAccount = _accountService.GetAccountByAccountNumber(ToAccount);

            transaction.CreatedAt = DateTime.Now;
            transaction.Type = TransactionType.TRANSFER.ToString();
            transaction.SourceAccount = FromAccount;
            transaction.DestinationAccount = ToAccount;
            transaction.Amount = Amount;

            if (authenticatedAccount.Balance > Amount )
            {
                authenticatedAccount.Balance -= Amount; //remove the tranamount from the source customer's 
                destinationAccount.Balance += Amount; //add tranamount to our target customer's balance...
                transaction.Particulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transaction.SourceAccount)} " +
                    $"TO DESTINATION => {JsonConvert.SerializeObject(transaction.DestinationAccount)} ON {transaction.CreatedAt} " +
                    $"TRAN_TYPE =>  {transaction.Type} TRAN_STATUS => {transaction.Status}";


                //so there was an update in the context State
                transaction.Status = TransactionStatus.SUCCESSFUL.ToString();
                response.Code = "00";
                response.Message = "Transaction Successful!";
                response.Data = transaction;
            }
            else
            {
                transaction.Status = TransactionStatus.FAILED.ToString();
                response.Code = "99";
                transaction.Particulars = $"Transaction FROM SOURCE {JsonConvert.SerializeObject(transaction.SourceAccount)} TO DESTINATION => {JsonConvert.SerializeObject(transaction.DestinationAccount)} ON {transaction.CreatedAt} TRAN_TYPE =>  {transaction.Type} TRAN_STATUS => {transaction.Status} Failed due to insufient funds";
                response.Message = "Insuficient Funds";
                response.Data = null;

            }


            _dbContext.Accounts.Update(authenticatedAccount);
            _dbContext.Accounts.Update(destinationAccount);
            _dbContext.Transactions.Add(transaction);

            _dbContext.SaveChanges();


            return response;

        }
    }
}

