using System;
using System.IO;
using Google.Protobuf;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Latoken.CurrencyProvider.Protocol.Tron;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;
using SHA3.Net;

namespace Latoken.CurrencyProvider.UnitTests.Garbage
{
	[TestClass]
	public class CryptoTests : BaseTest
	{

		[TestMethod]
		public void ATest()
		{
			ECSigner ecSigner = new ECSigner("F43EBCC94E6C257EDBE559183D1A8778B2D5A08040902C0F0A77A3343A1D0EA5");
			byte[] encoded = ecSigner._keyPair.ECPublicKey.Q.GetEncoded();
			byte[] computeHash = Sha3.Sha3256().ComputeHash(encoded,1,64);
			string string2 = computeHash.ToHexString2();


			String pubkey04 = "04e6a49d098ee94871252622b8a8b727e5cdf81b7138e5ac16591887f6e8c10e881e4b4250fa8c87f5b29ad020216a9ffd0acf5995a627d6c70e4dd274c54c20bd";
			string hexString04 = Sha3.Sha3256().ComputeHash(pubkey04.FromHexToByteArray2(),1,64).ToHexString2();
		}


		[TestMethod]
		public void SignUnsignTest()
		{
			ECSigner ecSigner = new ECSigner();
			var message = "test";
			string sign = ecSigner.Sign(message);
			bool isSignatureValid = ecSigner.IsSignatureValid(message, sign);
		}
		[TestMethod]
		public void SignUnsign2Test()
		{
			ECSigner ecSigner = new ECSigner("4C4CA01FBD362CABE6654337AC23B22496EE58D79CEDBEE26139D551BDCA5ADA");
			string message = "test";
			string sign = ecSigner.Sign(message);
			bool isSignatureValid = ecSigner.IsSignatureValid(message, sign);

			Assert.IsTrue(isSignatureValid);
		}


		[DataTestMethod]
		[DataRow("4C4CA01FBD362CABE6654337AC23B22496EE58D79CEDBEE26139D551BDCA5ADA", null, TypeNet.Main, "TT6sPQbC9FYPdy9sce8tg1gnfvr1A7A5j3")]
		[DataRow("F43EBCC94E6C257EDBE559183D1A8778B2D5A08040902C0F0A77A3343A1D0EA5", "04e6a49d098ee94871252622b8a8b727e5cdf81b7138e5ac16591887f6e8c10e881e4b4250fa8c87f5b29ad020216a9ffd0acf5995a627d6c70e4dd274c54c20bd", TypeNet.Test, "27jbj4qgTM1hvReST6hEa8Ep8RDo2AM8TJo")]
		public void TestAddressECKeyTest(string privateKeyHex, string publicKeyHes, TypeNet typeNet, string address)
		{
			KeyTriple keyTriple = new KeyTriple(privateKeyHex);
			byte[] addressWallet = keyTriple.GetAddressWallet(typeNet);
			if (!string.IsNullOrEmpty(publicKeyHes))
			{
				string publicKeyBase58 = keyTriple.PublicKey.ToHexString2();
				Assert.IsTrue(String.Equals(publicKeyHes, publicKeyBase58, StringComparison.CurrentCultureIgnoreCase));
			}
			string base58 = Base58.Encode(addressWallet);

			Assert.IsTrue(String.Equals(address, base58, StringComparison.CurrentCultureIgnoreCase));
		}

		[TestMethod]
		public void CalcTransactionIdTest()
		{
			Wallet.WalletClient wallet = GetMainNetWalletClient;

			//	BytesMessage bytesMessage = BytesMessage.Parser.ParseFrom(hexStringToByteArray);;
			//		Transaction byIdAsync = wallet.GetTransactionByIdAsync(bytesMessage).GetAwaiter().GetResult();

			string transactionHashId = "f5eea816a16b1907f8b5c6394e133f285001585ce07f657240701511436ba836";

			BytesMessage bytesMessage = new BytesMessage();
			bytesMessage.Value = ByteString.CopyFrom(transactionHashId.FromHexToByteArray2());

			Transaction transaction = wallet.GetTransactionByIdAsync(bytesMessage).GetAwaiter().GetResult();
			var hashTransaction = new TransactionHelper().GetTransactionHash(transaction);
			string hashTransactionToHex = hashTransaction.ToHexString2();

			Assert.AreEqual(transactionHashId, hashTransactionToHex);
		}

		[TestMethod]
		public void PublicPrivateKeyPairDemoTest()
		{
			string privateKey = "123456789";
			string publicKey = ECKeySign.GetPublicKeyFromPrivateKey(privateKey);
			Console.WriteLine(publicKey);

			privateKey = "68040878110175628235481263019639686";
			publicKey = ECKeySign.GetPublicKeyFromPrivateKey(privateKey);
			Console.WriteLine(publicKey);
		}

	

		[TestMethod]
		public void SignatureDemo()
		{
			var privateKey = "68040878110175628235481263019639686";

			var publicKey = ECKeySign.GetPublicKeyFromPrivateKey(privateKey);

			var message = "Hello World!";

			var signature = ECKeySign.GetSignature(privateKey, message);
			Console.WriteLine(signature);

			var isvalid = ECKeySign.VerifySignature(message, publicKey, signature);
			Console.WriteLine("Valid signature? " + isvalid);
		}

		[TestMethod]
		public void TransactionsDemo()
		{
			var privateKey = "68040878110175628235481263019639686";
			var publicKey = ECKeySign.GetPublicKeyFromPrivateKeyEx(privateKey);
			String transaction = publicKey + 10.ToString() +
			                     "QrSNX7KxzGnQqauPiXKxP58nhukU252RKAmSqg17L8h7BpU984g4mxHck6cLzhArADz2p1xo3BwAsbiaLhQaziyu";
			String signature = ECKeySign.GetSignature(privateKey, transaction);

			Console.WriteLine("Transaction signature: " + signature);

			bool isValidTransaction = ECKeySign.VerifySignature(transaction.ToString(), publicKey, signature);
			Console.WriteLine("Valid transaction message: " + isValidTransaction);
		}
	}
}