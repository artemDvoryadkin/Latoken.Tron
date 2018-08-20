using System;
using System.Collections.Generic;
using System.Text;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;

namespace Latoken.CurrencyProvider.Common.Configuration
{
	public interface IGrpcHostConfig
	{
		int Port { get; }

		string Host { get; }

		int? MaxConcurrentStreams { get; }

		TimeSpan? TimeOutMilliSecunds { get; }

		TypeNet TypeNet { get; }
	}
}