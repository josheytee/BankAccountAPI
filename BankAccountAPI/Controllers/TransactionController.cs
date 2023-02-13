using System;
using BankAccountAPI.Services.Interfaces;
using BankAccountAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountAPI.Controllers;

[Route("api/v1/transaction")]
[ApiController]
public class TransactionController: ControllerBase
	{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
		{
        _transactionService = transactionService;
    }

    //- Deposit Money
    [HttpPost]
    [Route("deposit")]
    public IActionResult MakeDeposit([FromBody]MakeDepositModel model)
    {
        return Ok(_transactionService.Deposit(model.AccountNumber, model.Amount, model.Pin));
    }

    //- Transfer(from one account to another)
    [HttpPost]
    [Route("transfer")]
    public IActionResult MakeFundsTransfer([FromBody] MakeTransferModel model)
    {
        return Ok(_transactionService.Transfer(model.FromAccount, model.ToAccount, model.Amount, model.Pin));
    }
}

