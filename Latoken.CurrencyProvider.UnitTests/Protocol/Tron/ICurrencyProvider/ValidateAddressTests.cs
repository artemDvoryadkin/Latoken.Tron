using System;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TronToken.CurrencyProvider.BussinesLogic.Model;

namespace Latoken.CurrencyProvider.UnitTests.ICurrencyProvider
{
	[TestClass]
	public class ValidateAddressTests :BaseTest
	{
		[DataTestMethod]
		[DataRow("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu",true)]
		[DataRow("TRb45eJbLbxCp2WQYQeNt9AoR3iiMKooLqs", false)]
		public void ValidateTest(string adressString, bool result)
		{
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

			bool resultValidate = currencyProvider.validateAddress(adressString);
			Console.WriteLine($"{adressString}:{resultValidate}");
			Assert.AreEqual(result, resultValidate);
		}
	}
}