using System.Runtime.Intrinsics.X86;
using AutoMapper;
using BankAccountAPI.Controllers;
using BankAccountAPI.Models;
using BankAccountAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using NUnit.Framework;


namespace BankAccountAPI.UnitTests;

[TestClass]
public class TestTransactionController
{
    Mock<ITransactionService> _transactionService;
    public TestTransactionController()
    {
         _transactionService = new Mock<ITransactionService>();
    }

    [TestMethod]
    public void MakeDepositReturns_200()
    {
        //Arange
        var depositModel = new MakeDepositModel()
        {
            AccountNumber = "1000000000",
            Amount = 102.20d,
            Pin = "1111"
        };


        var transaction = new TransactionController(_transactionService.Object);

        //Act
        var result = transaction.MakeDeposit(depositModel);

        //Result

        NUnit.Framework.Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ApplicationException))]
    public void MakeDepositThrowsException()
    {
        //Arange
        var depositModel = new MakeDepositModel()
        {
            Amount = 102.20d,
            Pin = "1111",
            AccountNumber = "1000000000",
        };

        _transactionService.Setup(_ => _.Deposit(depositModel.AccountNumber, depositModel.Amount, depositModel.Pin)).
            Throws(new ApplicationException());

        var controller = new TransactionController(_transactionService.Object);

        _transactionService.Object.Deposit(depositModel.AccountNumber, depositModel.Amount, depositModel.Pin);
    }

    [TestMethod]
    public void MakeFundsTransferReturns_200()
    {
        //Arange
        var transferModel = new MakeTransferModel()
        {
            Amount = 102.20d,
            Pin = "1111",
            FromAccount = "1100000000",
            ToAccount = "1000000000"
        };


        var transaction = new TransactionController(_transactionService.Object);

        //Act
        var result = transaction.MakeFundsTransfer(transferModel);

        //Result

        NUnit.Framework.Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ApplicationException))]
    public void MakeFundsTransferThrowsException()
    {
        //Arange
        var transferModel = new MakeTransferModel()
        {
            Amount = 102.20d,
            Pin = "1111",
            FromAccount = "1000000000",
            ToAccount = "1000000000"
        };

        _transactionService.Setup(_ => _.Transfer(transferModel.FromAccount, transferModel.ToAccount, transferModel.Amount, transferModel.Pin)).
            Throws(new ApplicationException());

        var controller = new TransactionController(_transactionService.Object);

        _transactionService.Object.Transfer(transferModel.FromAccount, transferModel.ToAccount, transferModel.Amount, transferModel.Pin);
            
    }

}
