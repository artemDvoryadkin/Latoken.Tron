using System;
using Latoken.CurrencyProvider.Common.Exceptions;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Utilities.Encoders;
using TronToken.CurrencyProvider.BussinesLogic.Model;

namespace Latoken.CurrencyProvider.UnitTests.Garbage
{
	[TestClass]
	public class TronAddressTests2 :BaseTest
	{

		[DataTestMethod]
		[DataRow("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu", true, null)]
		[DataRow("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSwЖ", false, ParseAddressException.CodeEnum.Format)]
		[DataRow("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSw", false, ParseAddressException.CodeEnum.CheckSumm)]
		[DataRow("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSwd", false, ParseAddressException.CodeEnum.Lenght)]
		[DataRow("APwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSw", false, ParseAddressException.CodeEnum.FirstByte)]
		public void Base58AddressTest(string addressBase58, bool isValid, object exceptionCode)
		{
			try
			{
				TronAddress tronAddress = TronAddress.CreateTronAddress(addressBase58);
				Assert.IsTrue(tronAddress != null);
			}
			catch (ParseAddressException parseAddressException)
			{
				if (exceptionCode != null)
					Assert.AreEqual((ParseAddressException.CodeEnum) exceptionCode, parseAddressException.Code);
			}
		}

		[DataTestMethod]
		[DataRow("QZk1doS8ZZ9RZgRrVslaDpnxJly9UShLwg ==", true, null)]
		[DataRow("QZk1doS8ZZ9RZgRrVslaDpnxJly9UShLwg ==1", false, ParseAddressException.CodeEnum.Format)]
		public void Base64AddressTest(string addressBase64, bool isValid, object exceptionCode)
		{
			byte[] bytesAdressFromString = StringHelper.GetBytesAdressFromString("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu");
			var decode = bytesAdressFromString.ToHexString2();

			try
			{
				TronAddress tronAddress = TronAddress.CreateTronAddress(addressBase64);
				Assert.IsTrue(tronAddress != null);
			}
			catch (ParseAddressException parseAddressException)
			{
				if (exceptionCode != null)
					Assert.AreEqual((ParseAddressException.CodeEnum)exceptionCode, parseAddressException.Code);
			}
		}

		[DataTestMethod]
		[DataRow("4199357684BC659F5166046B56C95A0E99F1265CBD51284BC", true, null)]
		[DataRow("4199357684BC659F5166046B56C95A0E99F1265CBD51284BQ", false, ParseAddressException.CodeEnum.Format)]
		public void HexAddressTest(string addressHex, bool isValid, object exceptionCode)
		{
			try
			{
				TronAddress tronAddress = TronAddress.CreateTronAddress(addressHex);
				Assert.IsTrue(tronAddress != null);
			}
			catch (ParseAddressException parseAddressException)
			{
				if (exceptionCode != null)
					Assert.AreEqual((ParseAddressException.CodeEnum)exceptionCode, parseAddressException.Code);
			}
		}

		[DataTestMethod]
		[DataRow("TLWY31TNNkqENXNcSctb2mqH1qvRdwbjFL")]
		public void AddressToBytesToAddressTest(string addressHex)
		{
				TronAddress tronAddress = TronAddress.CreateTronAddress(addressHex);
				string tronAddressAddressBase58 = tronAddress.AddressBase58;
			Assert.AreEqual(addressHex, tronAddressAddressBase58);
		}
	}
}