namespace Latoken.CurrencyProvider.Common.Interfaces.Models
{
	public class TransactionCredentials
	{
		public TransactionCredentials(string address)
		{
			this.address = address;
		}

		public string address { get; set; }
	}
}