using System;
using System.Collections.Generic;
using System.Text;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api;

namespace Latoken.CurrencyProvider.UnitTests
{
	public class BaseTest
	{
		public CurrencyProvider.Protocol.Tron.CurrencyProvider GetTestNetCurrencyProvider
		{
			get
			{
				ProtocolConfiguration configuration = new ProtocolConfiguration(TypeNet.Test, "39.105.72.181", 18888);
				CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider =
					new CurrencyProvider.Protocol.Tron.CurrencyProvider(configuration);

				return currencyProvider;
			}
		}

		public CurrencyProvider.Protocol.Tron.CurrencyProvider GetMainNetCurrencyProvider
		{
			get
			{
				ProtocolConfiguration configuration = new ProtocolConfiguration(TypeNet.Test, "13.125.249.129", 50051);
				CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider =
					new CurrencyProvider.Protocol.Tron.CurrencyProvider(configuration);

				return currencyProvider;
			}
		}

		public Wallet.WalletClient GetTestNetWalletClient
		{
			get
			{
				ProtocolConfiguration configuration = new ProtocolConfiguration(TypeNet.Test, "39.105.72.181", 18888);
				Wallet.WalletClient client = new Wallet.WalletClient(configuration.Channel);

				return client;
			}
		}

		public Wallet.WalletClient GetMainNetWalletClient
		{
			get
			{
				ProtocolConfiguration configuration = new ProtocolConfiguration(TypeNet.Main, "39.105.72.181", 18888);
				Wallet.WalletClient client = new Wallet.WalletClient(configuration.Channel);

				return client;
			}
		}

		public WalletExtension.WalletExtensionClient GetTestNetWalletExtensionClient
		{
			get
			{
				ProtocolConfiguration configuration = new ProtocolConfiguration(TypeNet.Test, "39.105.72.181", 18888);
				WalletExtension.WalletExtensionClient client = new WalletExtension.WalletExtensionClient(configuration.Channel);

				return client;
			}
		}

		public WalletExtension.WalletExtensionClient GetMainNetWalletExtensionClient
		{
			get
			{
				ProtocolConfiguration configuration = new ProtocolConfiguration(TypeNet.Main, "39.105.72.181", 18888);
				WalletExtension.WalletExtensionClient client = new WalletExtension.WalletExtensionClient(configuration.Channel);

				return client;
			}
		}

		public EmptyMessage GetEmptyMessage
		{
			get
			{
				EmptyMessage emptyMessage = new EmptyMessage();

				return emptyMessage;
			}
		}
	}
}