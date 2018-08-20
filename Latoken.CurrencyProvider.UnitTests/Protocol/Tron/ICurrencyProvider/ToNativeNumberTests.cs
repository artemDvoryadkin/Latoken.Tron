using System;
using Latoken.CurrencyProvider.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latoken.CurrencyProvider.UnitTests.ICurrencyProvider
{
	[TestClass]
	public class ToNativeNumberTests : BaseTest
	{
		[DataTestMethod]
		[DataRow("0.000 001", (long)1)]
		[DataRow("1,3", 1300000)]
		[DataRow("123456,654321", 123456654321)]
		[DataRow("0.123456", 123456)]
		public void ToNativeNumberTest(string amountString, long resultLong)
		{
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

			long nativeNumber = currencyProvider.toNativeNumber(amountString);

			Console.WriteLine($"{amountString}:{resultLong}:{nativeNumber}");
			Assert.AreEqual(resultLong, nativeNumber);
		}


		[DataTestMethod]
		[DataRow("0.000 0001", ParseAmountException.CodeEnum.MinAmount)]
		[DataRow("1000000001", ParseAmountException.CodeEnum.MaxAmount)]
		[DataRow("10-0000001", ParseAmountException.CodeEnum.Format)]
		public void ExceptionTest(string amountString, ParseAmountException.CodeEnum codeEnum)
		{
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

			ParseAmountException exception = Assert.ThrowsException<ParseAmountException>(() => currencyProvider.toNativeNumber(amountString));

			Assert.AreEqual(codeEnum, exception.Code);
			Console.WriteLine($"{amountString}:{exception.Code}:{codeEnum}");
		}
	}
}