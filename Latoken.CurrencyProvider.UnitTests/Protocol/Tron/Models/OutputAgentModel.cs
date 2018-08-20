using Latoken.CurrencyProvider.Common.Interfaces;

namespace Latoken.CurrencyProvider.UnitTests.Models
{
	internal class OutputAgentModel: IOutputAgent
	{
		public OutputAgentModel(string privateKey, string address)
		{
			this.address = address;
			this.privateKey = privateKey;
		}

		public string address { get; set; }
		public string privateKey { get; set; }
	}
}