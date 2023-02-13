using System;
namespace BankAccountAPI.Services
{
	public class AccountNumberGeneratorService
	{

		public string generate()
		{
            String startWith = "32";
            Random generator = new Random();
            String r = generator.Next(0, 999999).ToString("D6");
            string accNumber  = startWith + r;

            return accNumber;
        }
    }
}

