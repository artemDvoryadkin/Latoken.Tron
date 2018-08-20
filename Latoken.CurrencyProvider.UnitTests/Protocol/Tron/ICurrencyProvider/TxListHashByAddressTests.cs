using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latoken.CurrencyProvider.UnitTests.ICurrencyProvider
{
	[TestClass]
	public class TxListHashByAddressTests :BaseTest
	{
		[DataTestMethod]
		[DataRow("TRQGijAaik4DfXSQM7cBFxn376YPycTm2W")]
		public void TxListHashByAddress(string txid)
		{
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

			List<string> txListHashByAddress = currencyProvider.txListHashByAddress(txid);

			Console.WriteLine($"{txid}:{txListHashByAddress.Count}");
			Assert.AreEqual(100, txListHashByAddress.Count);
		}
	}
}