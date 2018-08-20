using System;

namespace Latoken.CurrencyProvider.Common.Exceptions
{
	public class CurrencyProviderException : ApplicationException
	{
		public CurrencyProviderException()
		{
		}

		public CurrencyProviderException(string message) : base(message)
		{
		}

		public CurrencyProviderException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}