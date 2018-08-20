using Latoken.CurrencyProvider.Common.Intefaces;
using Latoken.CurrencyProvider.Common.Interfaces;

namespace Latoken.CurrencyProvider.UnitTests.Models
{
	internal class TransactionOptionsModels : ITransactionOptions
	{
		public TransactionOptionsModels(IOutputAgent agent, ITransactionCredentials credentials, long value)
		{
			this.agent = agent;
			this.credentials = credentials;
			this.value = value;
		}

		public IOutputAgent agent { get; set; }
		public ITransactionCredentials credentials { get; set; }
		public long value { get; set; }
	}
}