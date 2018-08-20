using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TronToken.BussinesLogic;

namespace Latoken.CurrencyProvider.UnitTests.Garbage
{
	[TestClass]
	public class CurrencyProviderTests : BaseTest
	{
		CurrencyProvider.Protocol.Tron.CurrencyProvider _currencyProvider;
		string _addressValid = "TAahLbGTZk6YuCycii72datPQEtyC5x231";
		string _addressNotValid = "TAahLbGTZk6YuCycii72datPQEtyC5x2310000";

		public CurrencyProviderTests()
		{
			_currencyProvider = GetMainNetCurrencyProvider;
		}

		[TestMethod]
		public void balanceByAddress_GetBalanceTest()
		{
			long balanceByAddress = _currencyProvider.balanceByAddress("TWxezzaF6Lyvh4mNVQRj9okro8hqC3LfJt");

			Assert.IsTrue(balanceByAddress > 0);
		}

		[TestMethod]
		public void balanceByAddress_GetBalance1Test()
		{
			//todo: значение ассчета можно проверить логикой так как значение должено лежать в диапазоне значений.
			bool validateAddress = _currencyProvider.validateAddress(_addressValid);

			Assert.IsTrue(validateAddress);

			validateAddress = _currencyProvider.validateAddress(_addressNotValid);

			Assert.IsFalse(validateAddress);
		}

		[TestMethod]
		public void TxGetReceipt_GetTranByHashIdTest()
		{
			object txGetReceipt = _currencyProvider.txGetReceipt("ccb6bbbe36590f000a52152b64550d438ab90d1b057a31a6951a7b0ce48d4154");
		}

		[TestMethod]
		public void TxListHashByAddress_GetTranByHashIdTest()
		{
			List<string> txListHashByAddress = _currencyProvider.txListHashByAddress(_addressValid);
			Assert.IsTrue(txListHashByAddress.Count > 0);
		}

		[TestMethod]
		public void txListByAddress_GetTranByHashIdTest()
		{
			List<object> objects = _currencyProvider.txListByAddress(_addressValid);
			Assert.IsTrue(objects.Count > 0);
		}
	}
}