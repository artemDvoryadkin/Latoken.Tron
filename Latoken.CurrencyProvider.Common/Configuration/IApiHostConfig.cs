using Latoken.CurrencyProvider.Common.Helpers.Crypto;

namespace Latoken.CurrencyProvider.Common.Configuration
{
	public interface IApiHostConfig
	{
		string Url { get; }
		TypeNet TypeNet { get; }
	}
}