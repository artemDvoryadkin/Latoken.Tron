using System;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TronToken.CurrencyProvider.BussinesLogic.Model;

namespace Latoken.CurrencyProvider.UnitTests.ICurrencyProvider
{
	[TestClass]
	public class BalanseByAddressTest : BaseTest
	{
		[DataTestMethod]
		[DataRow("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu", StringType.Base58, TypeNet.Test)]
		[DataRow("TRb45eJbLbxCp2WQYQeNt9AoR3iiMKooLq", StringType.Base58, TypeNet.Main)]
		[DataRow("TFAzcjLQzdanDmLwZNux56SwqfbCChjjnR", StringType.Base58, TypeNet.Main)]
		[DataRow("THG48yHsR6inxrCJk2hhxZPsFLq1ehP88V", StringType.Base58, TypeNet.Main)]
		public void BalanceGreateZero(string addressBase58, StringType stringType, TypeNet typeNet)
		{
			ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet);

			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = new CurrencyProvider.Protocol.Tron.CurrencyProvider(protocolConfiguration);

			long balance = currencyProvider.balanceByAddress(addressBase58);
			Console.WriteLine($"{addressBase58}:{balance}");
			Assert.IsTrue(balance > 0);
		}

		[DataTestMethod]
		[DataRow("TVZYdy2wzsEzEUQXEyGeXVfmsZ2AmGaVs8", StringType.Base58, TypeNet.Main)]
		
		public void BalanceZero(string adrressBase58, StringType stringType, TypeNet typeNet)
		{
			ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet);

			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = new CurrencyProvider.Protocol.Tron.CurrencyProvider(protocolConfiguration);

			long balance = currencyProvider.balanceByAddress(adrressBase58);
			Console.WriteLine($"{adrressBase58}:{balance}");
			Assert.IsTrue(balance == 0);
		}
	}
}