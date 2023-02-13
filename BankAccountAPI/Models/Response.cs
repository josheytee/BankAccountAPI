using System;
namespace BankAccountAPI
{
	public class Response<T>
	{
		public string Id => $"{Guid.NewGuid().ToString()}";
		public string? Code { get; set; }
		public string? Message { get; set; }
		public T? Data { get; set; }
		
	}
}

