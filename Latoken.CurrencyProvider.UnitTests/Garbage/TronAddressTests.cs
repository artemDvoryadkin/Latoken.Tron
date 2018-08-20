using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TronToken.CurrencyProvider.BussinesLogic.Model;

namespace Latoken.CurrencyProvider.UnitTests.Garbage
{

	[TestClass]
	public class TronAddressTests
	{
		[TestMethod]
		public void ValidateCheckSumBytesAddressTest()
		{
			byte[] decodeBase58 = Base58.Decode("TVZYdy2wzsEzEUQXEyGeXVfmsZ2AmGaVs8");
			bool validateAddressWithCheckSumm = KeyTriple.ValidateAddressWithCheckSumm(decodeBase58);
			Assert.IsTrue(validateAddressWithCheckSumm);

			decodeBase58[3] = 0;
			validateAddressWithCheckSumm = KeyTriple.ValidateAddressWithCheckSumm(decodeBase58);
			Assert.IsFalse(validateAddressWithCheckSumm);
		}
	}
}