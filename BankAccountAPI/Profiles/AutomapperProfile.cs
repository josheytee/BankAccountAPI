using System;
using AutoMapper;
using BankAccountAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAccountAPI.Entities;

namespace BankAccountAPI.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CreateAccountModel, Account>();
        }
    }
}

