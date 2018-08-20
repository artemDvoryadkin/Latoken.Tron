using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Latoken.CurrencyProvider.Common.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latoken.CurrencyProvider.UnitTests.ICurrencyProvider
{
	[TestClass]
	public class TxGetSendingResultTests : BaseTest
	{
		[DataTestMethod]
		[DataRow("41c712036e6be669c4586005c573af516cdd9223e308b4b6f3d1314c3f0f88e2")]
		public void TxGetSendingResultTest(string txid)
		{
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

			ITransactionSendResult txGetSendingResult = currencyProvider.txGetSendingResult(txid);


			//Console.WriteLine($"{adressString}:{resultValidate}");
			//Assert.AreEqual(result, resultValidate);
		}
	}
}