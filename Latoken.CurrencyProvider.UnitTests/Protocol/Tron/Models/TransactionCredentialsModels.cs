using Latoken.CurrencyProvider.Common.Interfaces;

namespace Latoken.CurrencyProvider.UnitTests.Models
{
	internal class TransactionCredentialsModels : ITransactionCredentials
	{
		public TransactionCredentialsModels(string address)
		{
			this.address = address;
		}

		public string address { get; set; }
	}
}