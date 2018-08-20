namespace Latoken.CurrencyProvider.Common.Interfaces.Models
{
	public class OutputAgent 
	{
		public OutputAgent(string privateKey, string address)
		{
			this.address = address;
			this.privateKey = privateKey;
		}

		public string address { get; set; }
		public string privateKey { get; set; }
	}
}