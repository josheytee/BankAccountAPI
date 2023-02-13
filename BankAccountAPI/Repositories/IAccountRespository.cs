﻿using Dapper;
using BankAccountAPI.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace BankAccountAPI.Repositories
{
    public class AccountRepository: IDisposable
    {
        private readonly IConfiguration _configuration;

        NpgsqlConnection _connection { get; }

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<bool> CreateAccount(Account account)
        {
            var affected = await _connection.ExecuteAsync
                ("INSERT INTO Accounts(Number, Type, FirstName, LastName, Amount) " +
                "VALUES(@ProductName, @Description, @Amount)",
                new { account.Number, account.Type, account.FirstName, account.LastName, account.Amount });
            return affected != 0;
        }

        //public async Task<bool> DeleteDiscount(string productName)
        //{
        //    var affected = await _connection.ExecuteAsync
        //        ("DELETE FROM Coupon WHERE ProductName=@ProductName",
        //        new { ProductName = productName });
        //    return affected != 0;
        //}

        //public async Task<Coupon> GetDiscount(string productName)
        //{
        //    var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>
        //         ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
        //    if (coupon is null)
        //        return new Coupon
        //        {
        //            ProductName = "No Discount",
        //            Amount = 0,
        //            Description = "No Discount Desc."
        //        };

        //    return coupon;
        //}

        //public async Task<bool> UpdateDiscount(Coupon coupon)
        //{
        //    var affected = await _connection.ExecuteAsync
        //        ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id",
        //        new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });
        //    return affected != 0;
        //}

        void IDisposable.Dispose() => _connection.Dispose();
    }
}
