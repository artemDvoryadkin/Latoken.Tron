using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Latoken.CurrencyProvider.Common.Helpers.Crypto.Sha;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

namespace Latoken.CurrencyProvider.UnitTests.Garbage
{
	[TestClass]
	public class TransactionSignTests : BaseTest
	{

		[TestMethod]
		public void Main4Test()
		{
			Wallet.WalletClient wallet = GetMainNetWalletClient;

			var keyTriple = new KeyTriple("B7E85CA5910922C49DCCB92EE48DFEC13A60674845CB7152C86B88F31DA6DC67");
			byte[] hashTransaction1 = StringHelper.HexStringToByteArray("a7e974c6e69fb7741bf5e08de8a2d8f6617826c59422b440de22e0612b03c393");
			BytesMessage bytesMessage1 = new BytesMessage();
			bytesMessage1.Value = ByteString.CopyFrom(hashTransaction1);
			Transaction transactionLoad1 = wallet.GetTransactionByIdAsync(bytesMessage1).GetAwaiter().GetResult();
			Transaction.Types.Contract contract = transactionLoad1.RawData.Contract[0];
			TransferContract transferContract = TransferContract.Parser.ParseFrom(contract.Parameter.Value.ToByteArray());
			byte[] publicKeyOwner = transferContract.OwnerAddress.ToByteArray();
			string encode = Base58.Encode(publicKeyOwner);
			byte[] transactionRaw = transactionLoad1.RawData.ToByteArray();
			byte[] hash = Sha256.Hash(transactionRaw);
			string signBase64 = transactionLoad1.Signature[0].ToBase64();
			byte[] byteArray = transactionLoad1.Signature[0].ToByteArray();
			bool isSignatureValidFromBytes = new ECSigner().IsSignatureValidFromBytes(hash, byteArray, publicKeyOwner);

		}

		[TestMethod]
		public void Main3Test()
		{
			Wallet.WalletClient wallet = GetMainNetWalletClient;

			var keyTriple = new KeyTriple("B7E85CA5910922C49DCCB92EE48DFEC13A60674845CB7152C86B88F31DA6DC67");
			byte[] hashTransaction1 = StringHelper.HexStringToByteArray("a7e974c6e69fb7741bf5e08de8a2d8f6617826c59422b440de22e0612b03c393");
			BytesMessage bytesMessage1 = new BytesMessage();
			bytesMessage1.Value = ByteString.CopyFrom(hashTransaction1);
			Transaction transactionLoad1 = wallet.GetTransactionByIdAsync(bytesMessage1).GetAwaiter().GetResult();
			TransferContract transferContract = TransferContract.Parser.ParseFrom(transactionLoad1.RawData.Contract[0].Parameter.Value.ToByteArray()); ;
		}

		[TestMethod]
		public void Main2Test()
		{
			WalletExtension.WalletExtensionClient walletExtension = GetMainNetWalletExtensionClient;


			var keyTriple = new KeyTriple("B7E85CA5910922C49DCCB92EE48DFEC13A60674845CB7152C86B88F31DA6DC67");
			AccountPaginated accountPaginated = new AccountPaginated();

			Account account = new Account();
			account.Address = ByteString.CopyFrom(keyTriple.GetAddressWallet(TypeNet.Main).SubArray(0, 21));
			accountPaginated.Account = account;
			accountPaginated.Offset = 0;
			accountPaginated.Limit = 10;
			TransactionList transactionList = walletExtension.GetTransactionsFromThisAsync(accountPaginated).GetAwaiter().GetResult();
		}

		[TestMethod]
		public void ValidateTransactionTest()
		{
			Wallet.WalletClient wallet = GetMainNetWalletClient;

			byte[] hashTransaction1 =
				StringHelper.HexStringToByteArray("a7e974c6e69fb7741bf5e08de8a2d8f6617826c59422b440de22e0612b03c393");
			BytesMessage bytesMessage1 = new BytesMessage();
			bytesMessage1.Value = ByteString.CopyFrom(hashTransaction1);
			Transaction signedTransaction = wallet.GetTransactionByIdAsync(bytesMessage1).GetAwaiter().GetResult();

			Assert.IsTrue(signedTransaction.Signature.Count == signedTransaction.RawData.Contract.Count);

			RepeatedField<Transaction.Types.Contract> listContract = signedTransaction.RawData.Contract;
			byte[] hash = Sha256.Hash(signedTransaction.RawData.ToByteArray());
			int count = signedTransaction.Signature.Count;

			if (count == 0)
			{
				Assert.Fail();
			}

			for (int i = 0; i < count; ++i)
			{
				try
				{
					Transaction.Types.Contract contract = listContract[i];
					TransferContract transferContract = TransferContract.Parser.ParseFrom(signedTransaction.RawData.Contract[i].Parameter.Value.ToByteArray());


					byte[] owner = transferContract.OwnerAddress.ToByteArray();
					signedTransaction.Signature[i].ToByteArray();

					string signatureBase64 = Base64.ToBase64String(signedTransaction.Signature[i].ToByteArray());
					byte[] address = signatureToAddress(hash, signatureBase64);
					if (!Arrays.AreEqual(owner, address))
					{
						Assert.Fail();
					}
				}
				catch (Exception e)
				{
					e.ToString();
					Assert.Fail();
				}
			}

			Assert.IsTrue(true);
		}

		public static byte[] signatureToAddress(byte[] messageHash, String signatureBase64) 
		{
			return computeAddress(signatureToKeyBytes(messageHash, signatureBase64));
		}

		public static byte[] computeAddress(byte[] pubBytes)
		{
			return sha3omit12(pubBytes.SubArray(1, pubBytes.Length - 1));
		}

		public static byte[] sha3omit12(byte[] input)
		{
			byte[] hash = Sha256.Hash(input);
			byte[] address = hash.SubArray(11, hash.Length - 11);
			address[0] = TypeNode.AddressPreFixByte;

			return address;
		}

		public static byte[] signatureToKeyBytes(byte[] messageHash, String signatureBase64)
		{

			byte[] signatureEncoded;
			try
			{
				signatureEncoded = Base64.Decode(signatureBase64);
			}
			catch (Exception e)
			{
				// This is what you getData back from Bouncy Castle if base64 doesn't
				// decode :(
				throw new Exception("Could not decode base64", e);
			}

// Parse the signature bytes into r/s and the selector value.
			if (signatureEncoded.Length < 65)
			{
				throw new Exception("Signature truncated, expected 65 " +
				                             "bytes and got " + signatureEncoded.Length);
			}

			byte re = (byte)(signatureEncoded[0] & 0xFF);

			ECDSASignature ecdsaSignature = ECDSASignature.fromComponents(
				signatureEncoded.SubArray(1, 32),
				signatureEncoded.SubArray(33, 32),
				re);

			return signatureToKeyBytes(
				messageHash,
				ecdsaSignature);
		}

		public static byte[] signatureToKeyBytes(byte[] messageHash,
			ECDSASignature sig)
		{
			check(messageHash.Length == 32, "messageHash argument has length " +
			                                messageHash.Length);
			int header = sig.v;
			// The header byte: 0x1B = first key with even y, 0x1C = first key
			// with odd y,
			//                  0x1D = second key with even y, 0x1E = second key
			// with odd y
			if (header < 27 || header > 34)
			{
				throw new Exception("Header byte out of range: " + header);
			}

			if (header >= 31)
			{
				header -= 4;
			}

			int recId = header - 27;
			byte[] key = recoverPubBytesFromSignature(recId, sig,
				messageHash);
			if (key == null)
			{
				throw new Exception("Could not recover public key from " +
				                             "signature");
			}

			return key;
		}

		public static byte[] recoverPubBytesFromSignature(int recId,
	 ECDSASignature sig,
	 byte[] messageHash)
		{
			check(recId >= 0, "recId must be positive");
			check(sig.r.SignValue >= 0, "r must be positive");
			check(sig.s.SignValue >= 0, "s must be positive");
			check(messageHash != null, "messageHash must not be null");
			// 1.0 For j from 0 to h   (h == recId here and the loop is outside
			// this function)
			//   1.1 Let x = r + jn
			BigInteger n = ECKey.Curve.N;  // Curve order.
			BigInteger i = BigInteger.ValueOf((long)recId / 2);
			BigInteger x = sig.r.Add(i.Multiply(n));
			//   1.2. Convert the integer x to an octet string X of length mlen
			// using the conversion routine
			//        specified in Section 2.3.7, where mlen = ⌈(log2 p)/8⌉ or
			// mlen = ⌈m/8⌉.
			//   1.3. Convert the octet string (16 set binary digits)||X to an
			// elliptic curve point R using the
			//        conversion routine specified in Section 2.3.4. If this
			// conversion routine outputs “invalid”, then
			//        do another iteration of Step 1.
			//
			// More concisely, what these points mean is to use X as a compressed
			// public key.
			Org.BouncyCastle.Math.EC.FpCurve curve = (Org.BouncyCastle.Math.EC.FpCurve)ECKey.Curve.Curve;
			BigInteger prime = curve.Q;  // Bouncy Castle is not consistent
											  // about the letter it uses for the prime.
			if (x.CompareTo(prime) >= 0)
			{
				// Cannot have point co-ordinates larger than this as everything
				// takes place modulo Q.
				return null;
			}
			// Compressed allKeys require you to know an extra bit of data about the
			// y-coord as there are two possibilities.
			// So it's encoded in the recId.
			ECPoint R = decompressKey(x, (recId & 1) == 1);
			//   1.4. If nR != point at infinity, then do another iteration of
			// Step 1 (callers responsibility).
			if (!R.Multiply(n).IsInfinity)
			{
				return null;
			}
			//   1.5. Compute e from M using Steps 2 and 3 of ECDSA signature
			// verification.
			BigInteger e = new BigInteger(1, messageHash);
			//   1.6. For k from 1 to 2 do the following.   (loop is outside this
			// function via iterating recId)
			//   1.6.1. Compute a candidate public key as:
			//               Q = mi(r) * (sR - eG)
			//
			// Where mi(x) is the modular multiplicative inverse. We transform
			// this into the following:
			//               Q = (mi(r) * s ** R) + (mi(r) * -e ** G)
			// Where -e is the modular additive inverse of e, that is z such that
			// z + e = 0 (mod n). In the above equation
			// ** is point multiplication and + is point addition (the EC group
			// operator).
			//
			// We can find the additive inverse by subtracting e from zero then
			// taking the mod. For example the additive
			// inverse of 3 modulo 11 is 8 because 3 + 8 mod 11 = 0, and -3 mod
			// 11 = 8.
			BigInteger eInv = BigInteger.Zero.Subtract(e).Mod(n);
			BigInteger rInv = sig.r.ModInverse(n);
			BigInteger srInv = rInv.Multiply(sig.s).Mod(n);
			BigInteger eInvrInv = rInv.Multiply(eInv).Mod(n);
			FpPoint q = (FpPoint)ECAlgorithms.SumOfTwoMultiplies(ECKey.Params.G, eInvrInv, R, srInv);
			
			return q.GetEncoded(/* compressed */ false);
		}

		private static ECPoint decompressKey(BigInteger xBN, bool yBit)
		{
			//X9IntegerConverter x9 = new X9IntegerConverter();
			byte[] compEnc = X9IntegerConverter.IntegerToBytes(xBN, 1 + X9IntegerConverter.GetByteLength(ECKey.Curve.Curve));
			compEnc[0] = (byte)(yBit ? 0x03 : 0x02);
			return ECKey.Curve.Curve.DecodePoint(compEnc);
		}

		private static void check(bool test, String message)
		{
			if (!test)
			{
				throw new Exception(message);
			}
		}

		private static ECDSASignature fromComponents(byte[] r, byte[] s)
		{
			return new ECDSASignature(new BigInteger(1, r), new BigInteger(1,
				s));
		}

		[TestMethod]
		public void MainTest()
		{
			Wallet.WalletClient wallet = GetMainNetWalletClient;

			string privateStrFrom = "D95611A9AF2A2A45359106222ED1AFED48853D9A44DEFF8DC7913F5CBA727366";
			//TJCnKsPa7y5okkXvQAidZBzqx3QyQ6sxMW

			KeyTriple keyTripleFrom = new KeyTriple(privateStrFrom);
			string encode = Base58.Encode(keyTripleFrom.GetAddressWallet(TypeNet.Main));
			string encode1 = Base58.Encode(keyTripleFrom.GetAddressWallet(TypeNet.Test));


			string privateStrTo = "443A94E9EA11A10FB9E40E239ACD005F6FC4FFC43F302484281F804A76EB9419"; 
			//TYJ6pQuVDYc7ZFDf2xDjmUNidDeEEmXgBP
			KeyTriple keyTripleTo = new KeyTriple(privateStrTo);

			byte[] decode = Base58.Decode("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu");
			byte[] subArray = decode.SubArray(0,21);
			subArray[0] = 160;

			Account account = new Account();
		//	account.a
		//	account.Address = ByteString.CopyFrom(keyTripleFrom.GetAddressWallet(TypeNet.Test).SubArray(0,21));
		//	account.Address = ByteString.CopyFrom(keyTripleFrom.GetAddressWallet(TypeNet.Main));
			account.Address = ByteString.CopyFrom(keyTripleFrom.GetAddressWallet(TypeNet.Main).SubArray(0,21));
		//	account.Address = ByteString.CopyFrom(subArray);
		
			TaskAwaiter<Account> taskAwaiter = wallet.GetAccountAsync(account).GetAwaiter();
			Account result = taskAwaiter.GetResult();
			AccountNetMessage accountNetMessage = wallet.GetAccountNetAsync(account).GetAwaiter().GetResult();
			BytesMessage bytesMessage1 = new BytesMessage();
			bytesMessage1.Value = ByteString.CopyFrom(BitConverter.GetBytes(43586L));
			Block block = wallet.GetBlockByIdAsync(bytesMessage1).GetAwaiter().GetResult();


			byte[] privateBytes = keyTripleFrom.PrivateKey;
			byte[] fromBytes = keyTripleFrom.GetAddressWallet(TypeNet.Main).SubArray(1, 20);
			byte[] toBytes = keyTripleTo.GetAddressWallet(TypeNet.Main).SubArray(1, 20);
			long amount = 24071978L; //100 TRX, api only receive trx in drop, and 1 trx = 1000000 drop

			Transaction transaction = CreateTransaction(wallet, fromBytes, toBytes, amount);
			byte[] transactionBytes = transaction.ToByteArray();


			//sign a transaction
			Transaction transaction1 = Sign(transaction, keyTripleFrom);
			//get byte transaction
			byte[] transaction2 = transaction1.ToByteArray();
			Console.WriteLine("transaction2 ::::: " + transaction2.ToHexString2());

			//sign a transaction in byte format and return a Transaction object
			var transaction3 = signTransaction2Object(transactionBytes, keyTripleFrom);
			Console.WriteLine("transaction3 ::::: " + transaction3.ToByteArray().ToHexString2());


			//sign a transaction in byte format and return a Transaction in byte format
			var transaction4 = signTransaction2Byte(transactionBytes, keyTripleFrom);
			Console.WriteLine("transaction4 ::::: " + transaction4.ToHexString2());
		//	Transaction transactionSigned =
		//		Wallet.WalletClient.signTransactionByApi(transaction, ecKey.getPrivKeyBytes());
		//	byte[]
		//		transaction5 = transactionSigned.toByteArray();
		//	Console.WriteLine("transaction5 ::::: " + ByteArray.toHexString(transaction5));
		//	if (!Arrays.equals(transaction4, transaction5))
		//		Console.WriteLine("transaction4 is not equals to transaction5 !!!!!");

		//	boolean result = broadcast(transaction4);

		//	Console.WriteLine(result);
			broadcast(wallet,transaction1);
		}


		private Transaction CreateTransaction(Wallet.WalletClient wallet, byte[] fromBytes, byte[] toBytes, long amount)
		{
			Transaction transactionBuilder = new Transaction();
			Block newestBlock = wallet.GetNowBlockAsync(GetEmptyMessage).GetAwaiter().GetResult();

			Transaction.Types.Contract contractBuilder = new Transaction.Types.Contract();
			TransferContract transferContractBuilder = new TransferContract();
				
			transferContractBuilder.Amount = amount;
			transferContractBuilder.ToAddress = ByteString.CopyFrom(toBytes);
			transferContractBuilder.OwnerAddress = ByteString.CopyFrom(fromBytes);
			try
			{
				Any any = Any.Pack(transferContractBuilder.Clone());
				contractBuilder.Parameter = any;
			}
			catch (Exception)
			{
				return null;
			}

			contractBuilder.Type = Transaction.Types.Contract.Types.ContractType.TransferContract;
			transactionBuilder.RawData = new Transaction.Types.raw();
			transactionBuilder.RawData.Contract.Add(contractBuilder);
			transactionBuilder.RawData.Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			transactionBuilder.RawData.Expiration = newestBlock.BlockHeader.RawData.Timestamp + 10 * 60 * 60 * 1000;

			Transaction transaction = transactionBuilder.Clone();
			var refTransaction = SetReference(transaction, newestBlock);

			return refTransaction;
		}

		public Transaction Sign(Transaction transaction, KeyTriple keyTriple)
		{
			Transaction transactionBuilderSigned = transaction.Clone();
			byte[] hash = Sha256.Hash(transaction.RawData.ToByteArray());
			RepeatedField<Transaction.Types.Contract> listContract = transaction.RawData.Contract;
			for (int i = 0; i < listContract.Count; i++)
			{
				// внимание тут подпись нужно  смотреть внимательное что переделалось
				byte[] signatureBytes = keyTriple.GetSignature(hash);
				ByteString bsSign = ByteString.CopyFrom(signatureBytes);
				transactionBuilderSigned.Signature
					.Add(bsSign); //Each contract may be signed with a different private key in the future.
			}

			transaction = transactionBuilderSigned.Clone();

			return transaction;
		}

		private Transaction SetReference(Transaction transaction, Block newestBlock)
		{
			long blockHeight = newestBlock.BlockHeader.RawData.Number;
			byte[] blockHash = getBlockHash(newestBlock);
			byte[] refBlockNum = BitConverter.GetBytes(blockHeight);

			Transaction.Types.raw rawData = transaction.RawData.Clone();
			rawData.RefBlockHash = ByteString.CopyFrom(blockHash.SubArray(8, 8));
			rawData.RefBlockBytes = ByteString.CopyFrom(refBlockNum.SubArray(6, 2));

			Transaction clone = transaction.Clone();
			clone.RawData = rawData;

			return clone;
		}

		private byte[] getBlockHash(Block block)
		{
			return Sha256.Hash(block.BlockHeader.RawData.ToByteArray());
		}

		private string getTransactionHash(Transaction transaction)
		{
			byte[] transactionHash = Sha256.Hash(transaction.RawData.ToByteArray());
			string txid = transactionHash.ToHexString2();

			return txid;
		}

		private byte[] signTransaction2Byte(byte[] transactionBytes, KeyTriple privateKey)
		{
			Transaction transaction1 = Transaction.Parser.ParseFrom(transactionBytes);
			byte[] rawdata = transaction1.RawData.ToByteArray();
			byte[] hash = Sha256.Hash(rawdata);

			byte[] sign = privateKey.GetSignature(hash);
			ByteString byteString = ByteString.CopyFrom(sign);
			Transaction transaction = transaction1.Clone();
			transaction.Signature.Add(byteString);
			return transaction.ToByteArray();
		}

		private Transaction signTransaction2Object(byte[] transaction, KeyTriple privateKey)
		{
			Transaction transaction1 = Transaction.Parser.ParseFrom(transaction);
			byte[] rawdata = transaction1.RawData.ToByteArray();
			byte[] hash = Sha256.Hash(rawdata);
			byte[] sign = privateKey.GetSignature(hash);

			transaction1.Signature.Add(ByteString.CopyFrom(sign));

			return transaction1;
		}

		private void broadcast(Wallet.WalletClient wallet, Transaction transactionBytes)
		{
			Return result = wallet.BroadcastTransactionAsync(transactionBytes).GetAwaiter().GetResult();
		}
	}

	public class ECDSASignature
	{
		public readonly BigInteger r, s;
		public byte v;

		public ECDSASignature(BigInteger r, BigInteger s)
		{
			this.r = r;
			this.s = s;
		}

		private static ECDSASignature fromComponents(byte[] r, byte[] s)
		{
			return new ECDSASignature(new BigInteger(1, r), new BigInteger(1,
				s));
		}

		/**
		 * @param r -
		 * @param s -
		 * @param v -
		 * @return -
		 */
		public static ECDSASignature fromComponents(byte[] r, byte[] s, byte
			v)
		{
			ECDSASignature signature = fromComponents(r, s);
			signature.v = v;
			return signature;
		}
	}

	public class TypeNode
	{
		static TypeNode()
		{
			AddressPreFixByte = 160;
		}

		public static byte AddressPreFixByte { get; set; }
	}

	public sealed class X9IntegerConverter
	{
		private X9IntegerConverter()
		{
		}

		public static int GetByteLength(
			ECFieldElement fe)
		{
			return (fe.FieldSize + 7) / 8;
		}

		public static int GetByteLength(
			ECCurve c)
		{
			return (c.FieldSize + 7) / 8;
		}

		public static byte[] IntegerToBytes(
			BigInteger s,
			int qLength)
		{
			byte[] bytes = s.ToByteArrayUnsigned();

			if (qLength < bytes.Length)
			{
				byte[] tmp = new byte[qLength];
				Array.Copy(bytes, bytes.Length - tmp.Length, tmp, 0, tmp.Length);
				return tmp;
			}
			else if (qLength > bytes.Length)
			{
				byte[] tmp = new byte[qLength];
				Array.Copy(bytes, 0, tmp, tmp.Length - bytes.Length, bytes.Length);
				return tmp;
			}

			return bytes;
		}
	}
}