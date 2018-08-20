using System;
using System.Collections.Generic;
using Latoken.CurrencyProvider.Protocol.Tron.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latoken.CurrencyProvider.UnitTests.ICurrencyProvider
{
	[TestClass]
	public class TxListByAddressTests : BaseTest
	{
		[DataTestMethod]
		[DataRow("TRQGijAaik4DfXSQM7cBFxn376YPycTm2W")]
		public void TxListByAddressTest(string txid)
		{
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

			List<object> txListByAddress = currencyProvider.txListByAddress(txid);

			Console.WriteLine($"{txid}:{txListByAddress.Count}");
			Assert.AreEqual(100, txListByAddress.Count);
		}

	    [DataTestMethod]
	    [DataRow("TLLM21wteSPs4hKjbxgmH1L6poyMjeTbHm")]
	    public void txListTransactionModelByAddress(string txid)
	    {
	        CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

	        List<TransactionAtom> transactions = currencyProvider.txListTransactionModelByAddress(txid);

	        Console.WriteLine($"{transactions}:{transactions.Count}");
	        Assert.AreEqual(100, transactions.Count);
	    }


    }
}