using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using BankAccountAPI.Data;
using BankAccountAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using AutoMapper;
using BankAccountAPI.Services.Interfaces;
using BankAccountAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankAccountAPI.Controllers
{
    [Route("api/v1/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IMapper _mapper;
        private IAccountService  _accountService;

       
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _mapper = mapper;
            _accountService = accountService;
        }

        //- Open an account (Savings or Current) using basic usual details for banks
        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateAccountModel accountModel)
        {
            if (!ModelState.IsValid) return BadRequest(accountModel);
            //map
            var account = _mapper.Map<Account>(accountModel);
            return Ok( _accountService.CreateAccount(account, accountModel.Pin, accountModel.ConfirmPin));
        }

        //- Check balance
        [HttpPost("get-balance")]
        public IActionResult GetBalance(GetBalanceModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model.AccountNumber);
            //map
            var account = _accountService.Authenticate(model.AccountNumber, model.Pin);
            return Ok(new { AccountNumber = model.AccountNumber, Balance = account.Balance});
        }
    }
}

