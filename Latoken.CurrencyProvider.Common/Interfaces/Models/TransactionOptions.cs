namespace Latoken.CurrencyProvider.Common.Interfaces.Models
{
	public class TransactionOptions 
	{
		public TransactionOptions(OutputAgent agent, TransactionCredentials credentials, long value)
		{
			this.agent = agent;
			this.credentials = credentials;
			this.value = value;
		}

		public OutputAgent agent { get; set; }
		public TransactionCredentials credentials { get; set; }
		public long value { get; set; }
	}
}