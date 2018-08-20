using System;
using Latoken.CurrencyProvider.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latoken.CurrencyProvider.UnitTests.ICurrencyProvider
{
	[TestClass]
	public class FromNativeNumberTests : BaseTest
	{
		[DataTestMethod]
		[DataRow("1000000", 1)]
		[DataRow("100", 0.0001)]
		[DataRow("1", 0.000001)]
		[DataRow("123456654321", 123456.654321)]
		public void FromNativeNumberTest(string amountString, double resultDouble)
		{
			decimal resultDecimal = Convert.ToDecimal(resultDouble);
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

			decimal nativeNumber = currencyProvider.fromNativeNumber(amountString);

			Console.WriteLine($"{amountString}:{resultDecimal}:{nativeNumber}");
			Assert.AreEqual(resultDecimal, nativeNumber);
		}


		[DataTestMethod]
		[DataRow("-1", ParseAmountException.CodeEnum.MinAmount)]
		[DataRow("10000000000000001", ParseAmountException.CodeEnum.MaxAmount)]
		[DataRow("10-0000001", ParseAmountException.CodeEnum.Format)]
		public void ExceptionTest(string amountString, ParseAmountException.CodeEnum codeEnum)
		{
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = GetMainNetCurrencyProvider;

			ParseAmountException exception = Assert.ThrowsException<ParseAmountException>(() => currencyProvider.fromNativeNumber(amountString));

			Assert.AreEqual(codeEnum, exception.Code);
			Console.WriteLine($"{amountString}:{exception.Code}:{codeEnum}");
		}
	}
}