using AutoMapper;
using BankAccountAPI.Controllers;
using BankAccountAPI.Models;
using BankAccountAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;


namespace BankAccountAPI.UnitTests;

[TestClass]
public class TestAccountController
{
    AccountController _accountController;
    Mock<IAccountService> _accountService;
    Mock<IMapper> _mapper;

    public TestAccountController()
    {
        _accountService = new Mock<IAccountService>();
        _mapper = new Mock<IMapper>();

        _accountController = new AccountController(_accountService.Object, _mapper.Object);
    }

    [TestMethod]
    public void CreateAccountReturns_200()
    {
        //Arange
        var CreateModel = new CreateAccountModel()
        {
            FirstName="Fuminiyi",
            LastName="Kayode",
            Phone="0607433445",
            Email="aa@gg.com",
            Type="SAVINGS",
            Pin="1111",
            ConfirmPin="1111"
        };
      
        //Act
        var result = _accountController.Create(CreateModel);

        //Result

        NUnit.Framework.Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ApplicationException))]
    public void CreateAccountThrowsExceeption()
    {
        //Arange
        var CreateModel = new CreateAccountModel()
        {
            FirstName = "Fuminiyi",
            LastName = "Kayode",
            Phone = "0607433445",
            Email = "usetr@example.com",
            Type = "SAVINGS",
            Pin = "111121",
            ConfirmPin = "1111"
        };
        var acc = new Entities.Account()
        {
            FirstName = CreateModel.FirstName,
            LastName = CreateModel.LastName,
            Phone = CreateModel.Phone,
            Email = CreateModel.Email,
            Type = CreateModel.Type
        };
        //Act
        _accountService.Setup(_ => _.CreateAccount(acc, CreateModel.Pin, CreateModel.ConfirmPin)).Throws(new ApplicationException());
        _accountService.Object.CreateAccount(acc, CreateModel.Pin, CreateModel.ConfirmPin);

        //var result = _accountController.Create(CreateModel);

        //Result

        //NUnit.Framework.Assert.IsInstanceOf<OkObjectResult>(result);
    }


    [TestMethod]
    public void GetBalanceReturns_200()
    {
        //Arange
        var balanceModel = new GetBalanceModel() { Pin = "1111", AccountNumber = "0384155907" };
        _accountService.Setup(_ => _.Authenticate(balanceModel.AccountNumber, balanceModel.Pin))
            .Returns(new Entities.Account());


        //Act
        var result = _accountController.GetBalance(balanceModel);


        //Result
        NUnit.Framework.Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ApplicationException))]
    public void GetBalanceThrowsError()
    {
        //Arange
        var balanceModel = new GetBalanceModel() { Pin = "1111", AccountNumber = "10000" };
        _accountService.Setup(_ => _.Authenticate(balanceModel.AccountNumber, balanceModel.Pin))
            .Throws(new ApplicationException());

        //Act
        var result = _accountController.GetBalance(balanceModel);


        //Result

        NUnit.Framework.Assert.IsInstanceOf<OkObjectResult>(result);
    }


}
